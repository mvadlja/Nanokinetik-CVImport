using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using CommonComponents;
using EVMessage.AS2;
using Ready.Model;
using xEVMPD;

namespace EVGateway.WinService.Workflow
{
    public partial class Workflow
    {
        private bool SubmitMDNMessage(int xevprmMessagePk)
        {
            bool isMDNMessageSubmitted = false;
            Xevprm_message_PK xevprmMessage = null;
            try
            {
                xevprmMessage = _xevprmMessageOperation.GetEntity(xevprmMessagePk);

                if (xevprmMessage == null)
                {
                    string description = string.Format("Xevprm message with Pk = '{0}' can't be found in database!", xevprmMessagePk);
                    Log.AS2Service.LogEvent(description);

                    return false;
                }

                if (xevprmMessage.XevprmStatus != XevprmStatus.ACKReceived)
                {
                    string description = string.Format("MDN submission failed. Xevprm doesn't have status 'ACKReceived'.");
                    Log.Xevprm.LogEvent(description, xevprmMessagePk, xevprmMessage.XevprmStatus);

                    return false;
                }

                xevprmMessage.XevprmStatus = XevprmStatus.SubmittingMDN;
                _xevprmMessageOperation.Save(xevprmMessage);

                var receivedMessage = xevprmMessage.received_message_FK.HasValue ? _receivedMessageOperations.GetEntity(xevprmMessage.received_message_FK) : null;

                if (receivedMessage == null || receivedMessage.msg_type != (int)ReceivedMessageType.ACK || string.IsNullOrWhiteSpace(xevprmMessage.ack))
                {
                    string description = string.Format("MDN submission failed. Received ACK message is missing.");
                    Log.Xevprm.LogEvent(description, xevprmMessagePk, xevprmMessage.XevprmStatus);

                    xevprmMessage.XevprmStatus = XevprmStatus.ACKDeliveryFailed;
                    _xevprmMessageOperation.Save(xevprmMessage);

                    return false;
                }

                var ack = new ACKMessage();
                ack.From(xevprmMessage.ack);

                var as2SenderId = ack.Message.ichicsrmessageheader.messagereceiveridentifier;
                var as2GatewayId = AS2GatewayID;

                string receiptDeliveryOption = GetReceiptDeliveryOption(receivedMessage);

                if (string.IsNullOrWhiteSpace(receiptDeliveryOption))
                {
                    string description = string.Format("MDN submission failed. Receipt delivery option is missing.");
                    Log.Xevprm.LogEvent(description, xevprmMessagePk, xevprmMessage.XevprmStatus);

                    xevprmMessage.XevprmStatus = XevprmStatus.ACKDeliveryFailed;
                    _xevprmMessageOperation.Save(xevprmMessage);

                    return false;
                }

                string messageId = GetMessageId(receivedMessage);

                if (string.IsNullOrWhiteSpace(messageId))
                {
                    string description = string.Format("MDN submission failed. Message-Id is missing.");
                    Log.Xevprm.LogEvent(description, xevprmMessagePk, xevprmMessage.XevprmStatus);

                    xevprmMessage.XevprmStatus = XevprmStatus.ACKDeliveryFailed;
                    _xevprmMessageOperation.Save(xevprmMessage);

                    return false;
                }

                byte[] payload = ExtractPayload(receivedMessage.msg_data, null);
                string mic = ComputeMIC(payload);

                string sBoundary = null;
                byte[] mdnData = MDN(messageId, as2SenderId, as2GatewayId, out sBoundary, mic);

                var as2SendSettings = new AS2SendSettings()
                {
                    Uri = new Uri(receiptDeliveryOption),
                    MsgData = mdnData,
                    MessageID = messageId,
                    From = as2SenderId,
                    To = as2GatewayId,
                    TimeoutMs = AS2Timeout,
                    Boundary = sBoundary
                };

                var submissionResult = AS2Send.SendAsyncMDN(as2SendSettings);

                switch (submissionResult.Result)
                {
                    case AS2SendResult.ResultType.NotSent:
                        {
                            xevprmMessage.XevprmStatus = XevprmStatus.ACKDeliveryFailed;

                            var description = string.Format("MDN submission failed. Not sent. Error type: {0}.", submissionResult.Error);
                            Log.Xevprm.LogError(submissionResult.Exception, description, xevprmMessagePk, xevprmMessage.XevprmStatus);
                        }
                        break;
                    case AS2SendResult.ResultType.NoResponse:
                        {
                            xevprmMessage.XevprmStatus = XevprmStatus.ACKDeliveryFailed;

                            var description = string.Format("MDN submission failed. No response from EMA. Error type: {0}.", submissionResult.Error);
                            Log.Xevprm.LogError(submissionResult.Exception, description, xevprmMessagePk, xevprmMessage.XevprmStatus);
                        }
                        break;
                    case AS2SendResult.ResultType.ResponseReceived:

                        if (submissionResult.ResponseCode == HttpStatusCode.OK)
                        {
                            isMDNMessageSubmitted = true;

                            xevprmMessage.XevprmStatus = XevprmStatus.ACKDelivered;

                            Log.Xevprm.LogEvent("MDN message submited successfully.", xevprmMessagePk, xevprmMessage.XevprmStatus);
                        }
                        else
                        {
                            xevprmMessage.XevprmStatus = XevprmStatus.ACKDeliveryFailed;

                            var description = string.Format("MDN submission failed. Response code = {0}. Error type: {1}.", submissionResult.ResponseCode, submissionResult.Error);
                            Log.Xevprm.LogError(submissionResult.Exception, description, xevprmMessagePk, xevprmMessage.XevprmStatus);
                        }
                        break;
                }

                _xevprmMessageOperation.Save(xevprmMessage);

                var sentMessage = new Sent_message_PK
                {
                    msg_data = mdnData,
                    sent_time = DateTime.Now,
                    msg_type = (int)SentMessageType.MDN,
                    xevmpd_FK = xevprmMessagePk
                };

                _sentMessageOperations.Save(sentMessage);

            }
            catch (Exception ex)
            {
                var description = isMDNMessageSubmitted ? "Post MDN submission processing error." : "MDN submission failed.";
                Log.Xevprm.LogError(ex, description, xevprmMessagePk, XevprmStatus.ACKReceived);

                if (xevprmMessage != null)
                {
                    xevprmMessage.XevprmStatus = XevprmStatus.ACKDeliveryFailed;
                    _xevprmMessageOperation.Save(xevprmMessage);
                }

                return false;
            }

            return isMDNMessageSubmitted;
        }

        private static string GetMessageId(Recieved_message_PK receivedMessage)
        {
            var messageIdMatch = Regex.Match(receivedMessage.as_header, "Message-Id:\\s+([^\\r\\n]+)\\r\\n", RegexOptions.IgnoreCase);

            return messageIdMatch.Success ? messageIdMatch.Groups[1].Value.Trim() : null;
        }

        private string GetReceiptDeliveryOption(Recieved_message_PK receivedMessage)
        {
            var receiptDeliveryOptionMatch = Regex.Match(receivedMessage.as_header, "Receipt-Delivery-Option:\\s+([^\\r\\n]+)\\r\\n", RegexOptions.IgnoreCase);

            return receiptDeliveryOptionMatch.Success ? receiptDeliveryOptionMatch.Groups[1].Value : EMAMDNReceiptURL;
        }

        private string ComputeMIC(byte[] data)
        {
            using (var sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(data);
                return Convert.ToBase64String(hash);
            }
        }

        private byte[] ExtractPayload(byte[] data, string filePathToSave)
        {
            Encoding enc = Encoding.GetEncoding("iso-8859-1");

            string message = enc.GetString(data);

            Match boundaryMatch = Regex.Match(message, "------=([^\\s]+)", RegexOptions.IgnoreCase);
            string boundary = "------=" + boundaryMatch.Groups[1].Value;

            int firstPart = message.IndexOf(boundary) + boundary.Length;
            int lastPart = message.IndexOf(boundary + "--") + (boundary + "--").Length;

            message = message.Substring(firstPart, lastPart - firstPart).TrimStart();
            string payload = message.Substring(0, message.IndexOf(boundary) - 2);

            if (!String.IsNullOrWhiteSpace(filePathToSave)) File.WriteAllBytes(filePathToSave, enc.GetBytes(payload));

            return enc.GetBytes(payload);
        }

        private string MIMEBoundary()
        {
            return "----=_Part_" + Guid.NewGuid().ToString("N") + "";
        }

        private string MIMEHeader(string sContentType, string sEncoding, string sDisposition)
        {
            string sOut = "";

            sOut = "Content-Type: " + sContentType + Environment.NewLine;

            if (sDisposition != "")
                sOut += "Content-Disposition: " + sDisposition + Environment.NewLine;

            if (sEncoding != "")
                sOut += "Content-Transfer-Encoding: " + sEncoding + Environment.NewLine;

            sOut = sOut + Environment.NewLine;

            return sOut;
        }

        private byte[] ConcatBytes(params byte[][] arBytes)
        {
            long lLength = 0;
            long lPosition = 0;

            //Get total size required.
            foreach (byte[] ar in arBytes)
                lLength += ar.Length;

            //Create new byte array
            byte[] toReturn = new byte[lLength];

            //Fill the new byte array
            foreach (byte[] ar in arBytes)
            {
                ar.CopyTo(toReturn, lPosition);
                lPosition += ar.Length;
            }

            return toReturn;
        }

        private byte[] MDNInner(string messageId, string originalRecipient, string finalRecipient, string mic)
        {
            byte[] mdnInner = new byte[0];

            // get a MIME boundary
            string sBoundary = MIMEBoundary();

            byte[] sContentType = Encoding.ASCII.GetBytes("Content-Type: multipart/report;\r\n\tboundary=\"" + sBoundary + "\";\r\n\treport-type=disposition-notification\r\n");

            byte[] bBoundary = Encoding.ASCII.GetBytes(Environment.NewLine + "--" + sBoundary + Environment.NewLine);

            var sbContent = new StringBuilder();
            sbContent.Append("Content-Type: text/plain; charset=us-ascii");
            sbContent.Append("\r\n");
            sbContent.Append("\r\n");
            sbContent.Append("This is MDN info for message ACK number: " + messageId);
            sbContent.Append("\r\n");

            byte[] innerContent1 = Encoding.ASCII.GetBytes(sbContent.ToString());

            sbContent.Clear();
            sbContent.Append("Content-Type: message/disposition-notification");
            sbContent.Append("\r\n");
            sbContent.Append("\r\n");
            sbContent.Append("Original-Recipient: rfc822; " + originalRecipient);
            sbContent.Append("\r\n");
            sbContent.Append("Final-Recipient: rfc822; " + finalRecipient);
            sbContent.Append("\r\n");
            sbContent.Append("Original-Message-ID: " + messageId);
            sbContent.Append("\r\n");
            sbContent.Append("Disposition: automatic-action/MDN-sent-automatically; processed");
            sbContent.Append("\r\n");
            sbContent.Append("Received-Content-MIC: " + mic + ", sha1");
            sbContent.Append("\r\n");
            sbContent.Append("\r\n");
            sbContent.Append("\r\n");

            byte[] innerContent2 = Encoding.ASCII.GetBytes(sbContent.ToString());

            byte[] bFinalFooter = Encoding.ASCII.GetBytes("--" + sBoundary + "--\r\n");

            // Concatenate all the above together to form the message.
            mdnInner = ConcatBytes(sContentType, bBoundary, innerContent1, bBoundary, innerContent2, bFinalFooter);

            return mdnInner;
        }

        private byte[] EncodeMDNInner(byte[] arMessage, string originalrecipient)
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2 cert = new X509Certificate2();

            try
            {
                cert = store.Certificates.Find(X509FindType.FindByThumbprint, (object)SenderThumbprint[originalrecipient], true)[0];
            }
            catch (Exception ex)
            {
                var description = string.Format("Thumbprint for SenderID='{0}' is not valid! Check service config file.", originalrecipient);
                throw new Exception(description, ex);
            }

            ContentInfo contentInfo = new ContentInfo(arMessage);

            SignedCms signedCms = new SignedCms(contentInfo, true); // <- true detaches the signature
            CmsSigner cmsSigner = new CmsSigner(cert);

            signedCms.ComputeSignature(cmsSigner);
            byte[] signature = signedCms.Encode();

            return signature;
        }

        private byte[] MDN(string messageID, string originalRecipient, string finalRecipient, out string sBoundary, string MIC)
        {
            sBoundary = MIMEBoundary();

            byte[] bBoundary = Encoding.ASCII.GetBytes(Environment.NewLine + "--" + sBoundary + Environment.NewLine);
            byte[] bReportMessage = MDNInner(messageID, originalRecipient, finalRecipient, MIC);
            byte[] bSignatureHeader = Encoding.ASCII.GetBytes(MIMEHeader("application/pkcs7-signature", "binary", ""));
            byte[] bSignature = EncodeMDNInner(bReportMessage, originalRecipient);
            byte[] bFinalFooter = Encoding.ASCII.GetBytes("--" + sBoundary + "--\r\n\r\n");

            return ConcatBytes(bBoundary, bReportMessage, bBoundary, bSignatureHeader, bSignature, bFinalFooter);
        }
    }
}
