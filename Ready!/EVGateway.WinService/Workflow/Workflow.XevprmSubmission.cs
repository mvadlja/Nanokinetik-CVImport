using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using CommonComponents;
using EVGateway.WinService.NanokinetikEDMS;
using EVMessage.AS2;
using Ionic.Zip;
using Ready.Model;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;

namespace EVGateway.WinService.Workflow
{
    public partial class Workflow
    {
        public enum EDMSDocumentVersion
        {
            SelectedVersion,
            Current
        }

        public int XevprmMessageSubmissionDelay;

        public bool SubmitXevprmMessage(int xevprmMessagePk)
        {
            bool isXevprmMessageSubmitted = false;
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

                if (xevprmMessage.XevprmStatus != XevprmStatus.ReadyToSubmit)
                {
                    string description = string.Format("Xevprm submission failed. Xevprm doesn't have status 'ReadyToSubmit'.");
                    Log.Xevprm.LogEvent(description, xevprmMessagePk, xevprmMessage.XevprmStatus);

                    return false;
                }

                if (xevprmMessage.gateway_submission_date.HasValue && xevprmMessage.gateway_submission_date.Value.AddSeconds(XevprmMessageSubmissionDelay) > DateTime.Now)
                {
                    string description = string.Format("Xevprm submission delayed. Xevprm message status changed to ReadyToSubmit in the last {0} seconds. Xevprm message will be submitted in the next iteration.", XevprmMessageSubmissionDelay);
                    Log.Xevprm.LogEvent(description, xevprmMessagePk, xevprmMessage.XevprmStatus);

                    return false;
                }

                xevprmMessage.XevprmStatus = XevprmStatus.SubmittingMessage;
                _xevprmMessageOperation.Save(xevprmMessage);

                XRootNamespace xRootNamespace = XRootNamespace.Parse(xevprmMessage.xml);

                string as2SenderId = xRootNamespace.evprm.ichicsrmessageheader.messagesenderidentifier;
                string xevprmMessageNumber = xRootNamespace.evprm.ichicsrmessageheader.messagenumb;

                string xevprmMessageFileName = GetXevprmMessageFileName(as2SenderId, xevprmMessageNumber);
                xevprmMessage.generated_file_name = xevprmMessageFileName;

                var xevprmMessageXmlData = GetXevprmMessageXmlData(xevprmMessage.xml);

                var xevprmMessageZipData = GetXevprmMessageZipData(xevprmMessageFileName, xevprmMessageXmlData, xRootNamespace.evprm, xevprmMessage.submitted_FK);

                if (!SenderThumbprint.Keys.Contains(as2SenderId))
                {
                    var description = string.Format("Xevprm submission failed. SenderID '{0}' is not valid! Check service config file.", as2SenderId);
                    Log.Xevprm.LogEvent(description, xevprmMessagePk, xevprmMessage.XevprmStatus);

                    xevprmMessage.XevprmStatus = XevprmStatus.SubmissionFailed;
                    _xevprmMessageOperation.Save(xevprmMessage);

                    return false;
                }
                if (!GatewayThumbprint.Keys.Contains(AS2GatewayID))
                {
                    var description = string.Format("Xevprm submission failed. GatewayID '{0}' is not valid! Check service config file.", AS2GatewayID);
                    Log.Xevprm.LogEvent(description, xevprmMessagePk, xevprmMessage.XevprmStatus);

                    xevprmMessage.XevprmStatus = XevprmStatus.SubmissionFailed;
                    _xevprmMessageOperation.Save(xevprmMessage);

                    return false;
                }

                var as2SendSettings = new AS2SendSettings()
                {
                    Uri = new Uri(AS2ExchangePointURI),
                    MsgData = xevprmMessageZipData,
                    Filename = xevprmMessageFileName + ".zip",
                    MessageID = xevprmMessageNumber,
                    From = as2SenderId,
                    To = AS2GatewayID,
                    TimeoutMs = AS2Timeout,
                    SigningCertThumbPrint = SenderThumbprint[as2SenderId],
                    RecipientCertThumbPrint = GatewayThumbprint[AS2GatewayID],
                    MDNReceiptURL = this.MDNReceiptURL
                };

                var submissionResult = AS2Send.SendFile(as2SendSettings);

                switch (submissionResult.Result)
                {
                    case AS2SendResult.ResultType.NotSent:
                        {
                            xevprmMessage.XevprmStatus = XevprmStatus.SubmissionFailed;

                            var description = string.Format("Xevprm submission failed. Not sent. Error type: {0}.", submissionResult.Error);
                            Log.Xevprm.LogError(submissionResult.Exception, description, xevprmMessagePk, xevprmMessage.XevprmStatus);
                        }
                        break;
                    case AS2SendResult.ResultType.NoResponse:
                        {
                            xevprmMessage.XevprmStatus = XevprmStatus.SubmissionFailed;

                            var description = string.Format("Xevprm submission failed. No response from EMA. Error type: {0}.", submissionResult.Error);
                            Log.Xevprm.LogError(submissionResult.Exception, description, xevprmMessagePk, xevprmMessage.XevprmStatus);
                        }
                        break;
                    case AS2SendResult.ResultType.ResponseReceived:

                        if (submissionResult.ResponseCode == HttpStatusCode.OK)
                        {
                            isXevprmMessageSubmitted = true;

                            xevprmMessage.XevprmStatus = XevprmStatus.MDNPending;

                            Log.Xevprm.LogEvent("Xevprm message submitted successfully.", xevprmMessagePk, xevprmMessage.XevprmStatus);
                        }
                        else
                        {
                            xevprmMessage.XevprmStatus = XevprmStatus.SubmissionFailed;

                            var description = string.Format("Xevprm submission failed. Response code = {0}. Error type: {1}.", submissionResult.ResponseCode, submissionResult.Error);
                            Log.Xevprm.LogError(submissionResult.Exception, description, xevprmMessagePk, xevprmMessage.XevprmStatus);
                        }
                        break;
                }

                _xevprmMessageOperation.Save(xevprmMessage);

                var sentMessage = new Sent_message_PK
                {
                    msg_data = xevprmMessageZipData,
                    sent_time = DateTime.Now,
                    msg_type = (int)SentMessageType.EVPRM,
                    xevmpd_FK = xevprmMessagePk
                };

                _sentMessageOperations.Save(sentMessage);

            }
            catch (Exception ex)
            {
                var description = isXevprmMessageSubmitted ? "Post xevprm submission processing error." : "Xevprm submission failed. ";
                Log.Xevprm.LogError(ex, description, xevprmMessagePk, XevprmStatus.ReadyToSubmit);

                if (xevprmMessage != null)
                {
                    xevprmMessage.XevprmStatus = XevprmStatus.SubmissionFailed;
                    _xevprmMessageOperation.Save(xevprmMessage);
                }

                return false;
            }

            return isXevprmMessageSubmitted;
        }

        private static string GetXevprmMessageFileName(string xevprmMessageSenderId, string xevprmMessageNumber)
        {
            string fileNameFirstPart = string.Format("{0}_{1:yyyyMMdd}_{2:HHmmss}", xevprmMessageSenderId, DateTime.UtcNow, DateTime.UtcNow);
            string fileNameSecondPart = string.Format("{0}_{1}", Guid.NewGuid().ToString("N").Substring(0, 8), xevprmMessageNumber);

            return string.Format("{0}_{1}", fileNameFirstPart, fileNameSecondPart);
        }

        private static byte[] GetXevprmMessageXmlData(string xml)
        {
            var xevprmXmlBuilder = new StringBuilder();
            xevprmXmlBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-16\"?>");
            xevprmXmlBuilder.Append(xml);

            return Encoding.Unicode.GetBytes(xevprmXmlBuilder.ToString());
        }

        private static byte[] GetXevprmMessageZipData(string xevprmMessageFileName, byte[] xevprmMessageXmlData, evprm evprm, int? submitted_FK)
        {
            byte[] xevprmMessageZipData;
            using (var ms = new MemoryStream())
            {
                using (var zip = new ZipFile())
                {
                    zip.AddEntry(xevprmMessageFileName + ".xml", xevprmMessageXmlData);

                    if (evprm != null && evprm.attachments != null && evprm.attachments.attachment.Count > 0)
                    {
                        var attachmentOperations = new Attachment_PKDAL();
                        var documentOperations = new Document_PKDAL();
                        var typeOperations = new Type_PKDAL();
                        var userOperations = new USERDAL();

                        foreach (var item in evprm.attachments.attachment)
                        {
                            int attachmentPk = 0;
                            if (!int.TryParse(item.localnumber, out attachmentPk))
                            {
                                throw new Exception(string.Format("Attachment's localnumber='{0}' is not valid int!", item.localnumber));
                            };

                            var attachment = attachmentOperations.GetEntity(attachmentPk);
                            Document_PK document = null;
                            if (attachment != null && !string.IsNullOrWhiteSpace(attachment.EDMSDocumentId))
                            {
                                document = documentOperations.GetEntity(attachment.document_FK);
                            }

                            if (attachment != null && attachment.disk_file != null)
                            {
                                zip.AddEntry(attachment.attachmentname, attachment.disk_file);
                            }
                            else if (attachment != null && attachment.disk_file == null && document != null && document.EDMSDocument.HasValue && document.EDMSDocument.Value) // EDMS document type
                            {
                                var documentFormat = typeOperations.GetEntity(document.attachment_type_FK);
                                var _formatType = formatType.ORIGINAL;
                                if (documentFormat != null && documentFormat.name.Trim().ToLower() == "pdf") _formatType = formatType.PDF;

                                var username = string.Empty;
                                var user = userOperations.GetEntity(submitted_FK);
                                if (user == null || string.IsNullOrWhiteSpace(user.Username))
                                {
                                    throw new Exception("Username must be specified for EDMS attachment type!");
                                }
                                username = user.Username;

                                var documentVersion = document.EDMSVersionNumber;
                                if (document.EDMSBindingRule.ToLower() == EDMSDocumentVersion.Current.ToString().ToLower()) documentVersion = EDMSDocumentVersion.Current.ToString().ToUpper();
               
                                EDMSDocument edmsDocument = null;
                                try
                                {
                                    using (var edmsWsClient = new EDMS_WSClient())
                                    {
                                        edmsDocument = edmsWsClient.getDocument(document.EDMSDocumentId, username, documentVersion, _formatType);
                                    }
                                }
                                catch(Exception ex)
                                {
                                    throw new Exception(string.Format("Error downloading EDMS file. Method call parameters: edmsWsClient.getDocument({0}, {1}, {2}, {3})", document.EDMSDocumentId, username, documentVersion, _formatType));
                                }

                                if (edmsDocument == null || edmsDocument.content == null || edmsDocument.content.Length == 0)
                                {
                                    throw new Exception("EDMS attachment doesn't contain any data!");
                                }

                                var attachmentName = attachment.attachmentname;

                                if (attachmentName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                                {
                                    throw new Exception("EDMS attachment name contains special file name characters!");
                                }
                                
                                zip.AddEntry(attachmentName, edmsDocument.content);
                            }
                            else if (attachment == null)
                            {
                                throw new Exception(string.Format("Attachment with Pk='{0}' can't be found in database!", attachmentPk));
                            }
                            else
                            {
                                throw new Exception(string.Format("Attachment with Pk='{0}' doesn't contain any data!", attachmentPk));
                            }
                        }
                    }

                    zip.Save(ms);
                }
                ms.Seek(0, SeekOrigin.Begin);
                xevprmMessageZipData = ms.ToArray();
            }

            return xevprmMessageZipData;
        }
    }
}
