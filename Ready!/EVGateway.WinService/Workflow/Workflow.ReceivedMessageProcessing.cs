using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using CommonComponents;
using Ready.Model;
using xEVMPD;

namespace EVGateway.WinService.Workflow
{
    public partial class Workflow
    {
        #region MDN

        public bool ProcessReceivedMDNMessage(int receivedMessagePk)
        {
            Recieved_message_PK receivedMessage = null;

            try
            {
                receivedMessage = _receivedMessageOperations.GetEntity(receivedMessagePk);

                if (receivedMessage == null)
                {
                    string description = string.Format("Received message with Pk = '{0}' doesn't exist.", receivedMessagePk);
                    Log.AS2Service.LogEvent(description);

                    return false;
                }

                receivedMessage.processed = true;
                receivedMessage.processed_time = DateTime.Now;

                string mdn = Encoding.ASCII.GetString(receivedMessage.msg_data);

                var boundaryMatch = Regex.Match(mdn, "boundary=\"([^\"]+)\"", RegexOptions.IgnoreCase);

                if (!boundaryMatch.Success)
                {
                    var description = string.Format("Error at parsing received MDN message with Pk='{0}'. Failed to match boundary.", receivedMessagePk);
                    Log.AS2Service.LogEvent(description);

                    receivedMessage.is_successfully_processed = false;
                    receivedMessage.processing_error = "Parsing error: Failed to match boundary.";

                    _receivedMessageOperations.Save(receivedMessage);

                    return false;
                }

                string[] separators = { "--" + boundaryMatch.Groups[1].Value };
                string[] mdnHeaders = mdn.Split(separators, StringSplitOptions.None);

                if (mdnHeaders.Count() < 3)
                {
                    var description = string.Format("Error at parsing received MDN message with Pk='{0}'. Failed to match recipient header.", receivedMessagePk);
                    Log.AS2Service.LogEvent(description);

                    receivedMessage.is_successfully_processed = false;
                    receivedMessage.processing_error = "Parsing error: Failed to match recipient header.";

                    _receivedMessageOperations.Save(receivedMessage);

                    return false;
                }

                string optionsHeader = mdnHeaders[1];
                string recipientHeader = mdnHeaders[2];

                Match origMsgNumMatch = Regex.Match(recipientHeader, "Original-Message-ID:\\s+(\\S+)\\s+", RegexOptions.IgnoreCase);

                if (!origMsgNumMatch.Success)
                {
                    var description = string.Format("Error at parsing received MDN message with Pk='{0}'. Failed to match Original-Message-ID.", receivedMessagePk);
                    Log.AS2Service.LogEvent(description);

                    receivedMessage.is_successfully_processed = false;
                    receivedMessage.processing_error = "Parsing error: Failed to match Original-Message-ID.";

                    _receivedMessageOperations.Save(receivedMessage);

                    return false;
                }

                var xevprmMessage = _xevprmMessageOperation.GetEntityByMessageNumber(origMsgNumMatch.Groups[1].Value);

                if (xevprmMessage == null || xevprmMessage.deleted == true)
                {
                    var description = string.Format("Error at processing received MDN message with Pk='{0}'. Xevprm message with messagenumber='{1}' doesn't exist.", receivedMessagePk, origMsgNumMatch.Groups[1].Value);
                    Log.AS2Service.LogEvent(description);

                    receivedMessage.is_successfully_processed = false;
                    receivedMessage.processing_error = string.Format("Xevprm message with messagenumber='{0}' doesn't exist.", origMsgNumMatch.Groups[1].Value);

                    _receivedMessageOperations.Save(receivedMessage);

                    return false;
                }

                if (xevprmMessage.XevprmStatus != XevprmStatus.MDNPending)
                {
                    var description = string.Format("Error at processing received MDN message with Pk='{0}'. Xevprm message with Pk='{1}' has status '{2}'. Expected status: 'MDNPending'. MDN discarded.",
                        receivedMessagePk, xevprmMessage.xevprm_message_PK, xevprmMessage.XevprmStatus);
                    Log.AS2Service.LogEvent(description);

                    receivedMessage.is_successfully_processed = false;
                    receivedMessage.processing_error = string.Format("Xevprm message with Pk='{0}' has status '{1}'. Expected status: 'MDNPending'. MDN discarded.",
                        xevprmMessage.xevprm_message_PK, xevprmMessage.XevprmStatus);

                    _receivedMessageOperations.Save(receivedMessage);

                    return false;
                }

                string mdnError = optionsHeader.Substring(optionsHeader.IndexOf("was successfully processed.") + "was successfully processed.".Length).Trim();

                receivedMessage.xevmpd_FK = xevprmMessage.xevprm_message_PK;
                xevprmMessage.received_message_FK = receivedMessage.recieved_message_PK;

                if (!string.IsNullOrWhiteSpace(mdnError))
                {
                    xevprmMessage.XevprmStatus = XevprmStatus.MDNReceivedError;
                    _xevprmMessageOperation.Save(xevprmMessage);

                    Log.Xevprm.LogEvent(string.Format("MDN with error received. Error: {0}", mdnError), xevprmMessage.xevprm_message_PK, xevprmMessage.XevprmStatus);

                    receivedMessage.processing_error = mdnError;
                    receivedMessage.status = (int)MDNStatus.Failed;
                }
                else
                {
                    xevprmMessage.XevprmStatus = XevprmStatus.MDNReceivedSuccessful;
                    _xevprmMessageOperation.Save(xevprmMessage);

                    Log.Xevprm.LogEvent("Successful MDN received.", xevprmMessage.xevprm_message_PK, xevprmMessage.XevprmStatus);

                    receivedMessage.status = (int?)MDNStatus.Success;
                }

                receivedMessage.is_successfully_processed = true;
                _receivedMessageOperations.Save(receivedMessage);

                return true;
            }
            catch (Exception ex)
            {
                if (receivedMessage != null)
                {
                    receivedMessage.is_successfully_processed = false;
                    receivedMessage.processing_error = Log.Xevprm.ConstructErrorDescription(ex);
                    _receivedMessageOperations.Save(receivedMessage);
                }

                var description = string.Format("Error at processing received MDN message with Pk='{0}'.", receivedMessagePk);
                Log.AS2Service.LogError(ex, description);

                return false;
            }
        }

        #endregion

        #region ACK

        private bool ProcessReceivedACKMessage(int receivedMessagePk)
        {
            Recieved_message_PK receivedMessage = null;

            try
            {
                receivedMessage = _receivedMessageOperations.GetEntity(receivedMessagePk);

                if (receivedMessage == null)
                {
                    string description = string.Format("Received message with Pk='{0}' doesn't exist.", receivedMessagePk);
                    Log.AS2Service.LogEvent(description);

                    return false;
                }

                receivedMessage.processed = true;
                receivedMessage.processed_time = DateTime.Now;

                string ackXml = ExtractACK(receivedMessage.msg_data, Encoding.UTF8);

                var origMsgNumMatch = Regex.Match(ackXml, "<originalmessagenumb>(\\S+)</originalmessagenumb>", RegexOptions.IgnoreCase);
                if (!origMsgNumMatch.Success)
                {
                    var description = string.Format("Error at parsing received ACK message with Pk='{0}'. Failed to match original message number.", receivedMessagePk);
                    Log.AS2Service.LogEvent(description);

                    receivedMessage.is_successfully_processed = false;
                    receivedMessage.processing_error = "Parsing error: Failed to match original message number.";

                    return false;
                }

                var xevprmMessage = _xevprmMessageOperation.GetEntityByMessageNumber(origMsgNumMatch.Groups[1].Value);

                if (xevprmMessage == null || xevprmMessage.deleted == true)
                {
                    var description = string.Format("Error at processing received ACK message with Pk='{0}'. Xevprm message with messagenumber='{1}' doesn't exist.", receivedMessagePk, origMsgNumMatch.Groups[1].Value);
                    Log.AS2Service.LogEvent(description);

                    receivedMessage.is_successfully_processed = false;
                    receivedMessage.processing_error = string.Format("Xevprm message with messagenumber='{0}' doesn't exist.", origMsgNumMatch.Groups[1].Value);

                    _receivedMessageOperations.Save(receivedMessage);

                    return false;
                }

                if (xevprmMessage.XevprmStatus != XevprmStatus.MDNReceivedSuccessful)
                {
                    var description = string.Format("Error at processing received ACK message with Pk='{0}'. Xevprm message with Pk='{1}' has status '{2}'. Expected status: 'MDNReceivedSuccessful'. ACK discarded.",
                        receivedMessagePk, xevprmMessage.xevprm_message_PK, xevprmMessage.XevprmStatus);
                    Log.AS2Service.LogEvent(description);

                    receivedMessage.is_successfully_processed = false;
                    receivedMessage.processing_error = string.Format("Xevprm message with Pk='{0}' has status '{1}'. Expected status: 'MDNReceivedSuccessful'. ACK discarded.",
                        xevprmMessage.xevprm_message_PK, xevprmMessage.XevprmStatus);

                    _receivedMessageOperations.Save(receivedMessage);

                    return false;
                }

                xevprmMessage.ack = ackXml;
                xevprmMessage.XevprmStatus = XevprmStatus.ACKReceived;
                xevprmMessage.gateway_ack_date = receivedMessage.received_time;

                var ack = new ACKMessage();
                ack.From(xevprmMessage.ack);

                xevprmMessage.ack_type = (int?)ack.MessageStatus;
                xevprmMessage.received_message_FK = receivedMessage.recieved_message_PK;

                using (var ts = new TransactionScope())
                {
                    if (ack.MessageStatus == ACKMessage_Status.NoErrors)
                    {
                        ProcessACKWithNoErrors(ack, xevprmMessage);
                    }

                    xevprmMessage = _xevprmMessageOperation.Save(xevprmMessage);

                    ts.Complete();
                }

                Log.Xevprm.LogEvent(string.Format("ACK 0{0} received.", xevprmMessage.ack_type), xevprmMessage.xevprm_message_PK, xevprmMessage.XevprmStatus);

                receivedMessage.status = (int?)ack.MessageStatus;
                receivedMessage.xevmpd_FK = xevprmMessage.xevprm_message_PK;
                receivedMessage.is_successfully_processed = true;

                _receivedMessageOperations.Save(receivedMessage);

                return true;
            }
            catch (Exception ex)
            {
                if (receivedMessage != null)
                {
                    receivedMessage.is_successfully_processed = false;
                    receivedMessage.processing_error = Log.Xevprm.ConstructErrorDescription(ex);
                    _receivedMessageOperations.Save(receivedMessage);
                }

                var description = string.Format("Error at processing received ACK message with Pk='{0}'.", receivedMessagePk);
                Log.AS2Service.LogError(ex, description);

                return false;
            }
        }

        private static string ExtractACK(byte[] data, Encoding encoding)
        {
            string ack = string.Empty;

            string messageStr = encoding.GetString(data).Replace("\0", "");

            int ackStart = messageStr.IndexOf("<?xml");
            int ackEnd = messageStr.IndexOf("</evprmack>") + ("</evprmack>").Length;

            if (ackStart != -1 && ackEnd != -1)
            {
                ack = messageStr.Substring(ackStart, ackEnd - ackStart);
                ack = ack.Replace("UTF-8", "UTF-16");
                ack = ack.Replace("utf-8", "UTF-16");
            }

            return ack;
        }

        private void ProcessACKWithNoErrors(ACKMessage ack, Xevprm_message_PK message)
        {
            foreach (var reportacknowledgment in ack.Message.acknowledgment.reportacknowledgment)
            {
                if (reportacknowledgment.operationtype == "1" && reportacknowledgment.operationresult == "2") // operationtype == 1 => Insert, operationresult == 2 => Successfully inserted!
                {
                    if (reportacknowledgment.reportname == reportacknowledgmentReportname.AUTHORISEDPRODUCT)
                    {
                        SetAuthorisedProductEvCode(message, reportacknowledgment);
                    }
                    else if (reportacknowledgment.reportname == reportacknowledgmentReportname.ATTACHMENT)
                    {
                        SetAttachmentEvCode(message, reportacknowledgment);
                    }
                }
                else if (reportacknowledgment.operationtype == "2" && reportacknowledgment.operationresult == "4")
                {
                    //Do nothing
                }
                else if (reportacknowledgment.operationtype == "3" && reportacknowledgment.operationresult == "4")
                {
                    //Do nothing
                }
                else if (reportacknowledgment.operationtype == "4" && reportacknowledgment.operationresult == "3")
                {
                    if (reportacknowledgment.reportname == reportacknowledgmentReportname.AUTHORISEDPRODUCT)
                    {
                        RemoveAuthorisedProductEvCode(message);
                    }
                }
                else if (reportacknowledgment.operationtype == "6" )//&& reportacknowledgment.operationresult == "3")
                {
                    if (reportacknowledgment.reportname == reportacknowledgmentReportname.AUTHORISEDPRODUCT)
                    {
                        RemoveAuthorisedProductEvCode(message);
                    }
                }
                
            }
        }

        private void RemoveAuthorisedProductEvCode(Xevprm_message_PK message)
        {
            IXevprm_ap_details_PKOperations xevprmApDetailsOperations = new Xevprm_ap_details_PKDAL();
            var xevprmApDetails = xevprmApDetailsOperations.GetEntityForXevprm(message.xevprm_message_PK);

            if (xevprmApDetails != null)
            {
                xevprmApDetails.ev_code = string.Empty;

                xevprmApDetailsOperations.Save(xevprmApDetails);

                var authorisedProduct = _authorisedProductOperations.GetEntity(xevprmApDetails.ap_FK);
                if (authorisedProduct != null)
                {
                    authorisedProduct.ev_code = string.Empty;
                    _authorisedProductOperations.Save(authorisedProduct);
                }
            }
        }

        private void SetAttachmentEvCode(Xevprm_message_PK message, reportacknowledgment reportacknowledgment)
        {
            var attachment = _attachmentOperations.GetEntity(reportacknowledgment.localnumber);

            if (attachment == null) return;

            attachment.ev_code = reportacknowledgment.ev_code;
            _attachmentOperations.Save(attachment);

            IXevprm_attachment_details_PKOperations xevprmAttachmentDetailsOperations = new Xevprm_attachment_details_PKDAL();
            var xevprmAttachmentDetails = xevprmAttachmentDetailsOperations.GetEntityForXevprm(message.xevprm_message_PK);

            if (xevprmAttachmentDetails != null)
            {
                xevprmAttachmentDetails.ev_code = reportacknowledgment.ev_code;

                xevprmAttachmentDetailsOperations.Save(xevprmAttachmentDetails);
            }
        }

        private void SetAuthorisedProductEvCode(Xevprm_message_PK message, reportacknowledgment reportacknowledgment)
        {
            var authorisedProduct = _authorisedProductOperations.GetEntity(reportacknowledgment.localnumber);
            
            if (authorisedProduct == null) return;

            authorisedProduct.ev_code = reportacknowledgment.ev_code;
            _authorisedProductOperations.Save(authorisedProduct);

            IXevprm_ap_details_PKOperations xevprmApDetailsOperations = new Xevprm_ap_details_PKDAL();
            var xevprmApDetails = xevprmApDetailsOperations.GetEntityForXevprm(message.xevprm_message_PK);

            if (xevprmApDetails != null)
            {
                xevprmApDetails.ev_code = reportacknowledgment.ev_code;

                xevprmApDetailsOperations.Save(xevprmApDetails);
            }
        }

        #endregion
    }
}
