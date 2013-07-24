using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVMessage;
using EVMessage.StatusReport.Schema;
using System.Text.RegularExpressions;
using Ready.Model;
using System.Reflection;
namespace EVMessage.StatusReport
{
    public class StatusReportMessage
    {
        private static string MISSING_DATE = "______________";
        private statusreport _statusReport;

        public statusreport StatusReport
        {
            get { return _statusReport; }
            set { _statusReport = value; }
        }

        public bool IsReportValid
        {
            get
            {
                try
                {
                    XmlValidator.XmlValidator xmlReportValidator = new XmlValidator.XmlValidator();
                    return xmlReportValidator.Validate(this.StatusReportXML, Properties.Resources.StatusReportXSD);
                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }


        public string StatusReportXML
        {
            get { return "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" + _statusReport.ToString(); }
        }

        public void CreateStatusReportFor_MAXmlValidationFailure(Ma_message_header_PK messageheader, XmlValidator.XmlValidator xmlValidator, String messageXml, Marketing_authorisation_PK ma)
        {

            _statusReport = new statusreport();
            ConstructReportHeader();

            _statusReport.report = new report();

            var msgAck = new messageacknowledgment();
            _statusReport.report.messageacknowledgment = msgAck;

            var reportAck = new reportacknowledgment();
            _statusReport.report.reportacknowledgment = reportAck;


            FillMessageacknowledgement(messageheader, ma.ready_id);
            FillReportName("Report_MAReceivedErrors");

            reportAck.reportstatuscode = "02";
            reportAck.reporttype = "02";
            reportAck.reportstatusmessage = "Parsing XML failed. Check reportcomments section for more details.";

            if (xmlValidator != null && (!xmlValidator.IsValid || (xmlValidator.Exceptions != null && xmlValidator.Exceptions.Count >0)) )
            {
                reportAck.reportcomments = new reportacknowledgment.reportcommentsLocalType();

                var reportcommentList = new List<reportacknowledgment.reportcommentsLocalType.reportcommentLocalType>();

                xmlValidator.Exceptions.ForEach(
                    delegate(XmlValidator.XmlValidatorException ex)
                    {
                        var reportcomment = new reportacknowledgment.reportcommentsLocalType.reportcommentLocalType();

                        StringBuilder sb = new StringBuilder();

                        if (ex.XmlValidatorExceptionType == XmlValidator.XmlValidatorExceptionType.Error)
                        {
                            reportcomment.severity = 1;
                            sb.Append("Error!");
                        }
                        else
                        {
                            reportcomment.severity = 2;
                            sb.Append("Warning!");
                        }

                        sb.Append(" (Line: " + ex.LineNumber);
                        sb.Append(" Position: " + ex.LinePosition + ")");
                        sb.Append(" " + ex.Message);

                        reportcomment.commenttext = sb.ToString();
                        reportcommentList.Add(reportcomment);

                    });

          
                reportAck.reportcomments.reportcomment = reportcommentList.ToArray();
            }
            else
            {
                reportAck.reportstatusmessage = "Parsing XML failed.";
            }
        }

        public void CreateStatusReportFor_MAReceived(Ma_message_header_PK messageheader, String readyId)
        {
            _statusReport = new statusreport();
            ConstructReportHeader();

            _statusReport.report = new report();

            var msgAck = new messageacknowledgment();
            _statusReport.report.messageacknowledgment = msgAck;

            var reportAck = new reportacknowledgment();
            _statusReport.report.reportacknowledgment = reportAck;

            FillMessageacknowledgement(messageheader, readyId);
            FillReportName("Report_MAReceived");

            reportAck.reportstatuscode = "01";
            reportAck.reporttype = "01";

            reportAck.reportstatusmessage = "Marketing authorisation received successfully.";
        }

        public void CreateStatusReportFor_MAValidationSuccessfull(Ma_message_header_PK messageheader, String readyId)
        {
            _statusReport = new statusreport();
            ConstructReportHeader();

            _statusReport.report = new report();

            var msgAck = new messageacknowledgment();
            _statusReport.report.messageacknowledgment = msgAck;

            var reportAck = new reportacknowledgment();
            _statusReport.report.reportacknowledgment = reportAck;

            FillMessageacknowledgement(messageheader, readyId);
            FillReportName("Report_MAValidationSuccessful");

            reportAck.reportstatuscode = "03";
            reportAck.reporttype = "01";

            reportAck.reportstatusmessage = "Message is successfully validated.";
        }

        /// <summary>
        /// TODO : Add validation errors description.
        /// </summary>
        /// <param name="messageheader"></param>
        /// <param name="readyId"></param>
        public void CreateStatusReportFor_MAValidationFailed(Ma_message_header_PK messageheader, List<EVMessage.MarketingAuthorisation.ValidationException> exceptions, String readyId)
        {
            _statusReport = new statusreport();
            ConstructReportHeader();

            _statusReport.report = new report();

            var msgAck = new messageacknowledgment();
            _statusReport.report.messageacknowledgment = msgAck;

            var reportAck = new reportacknowledgment();
            _statusReport.report.reportacknowledgment = reportAck;

            FillMessageacknowledgement(messageheader, readyId);
            FillReportName("Report_MAValidationFailed");

            reportAck.reportstatusmessage = "MA validation failed. Check reportcomments section for more details.";

            if (exceptions != null && exceptions.Count > 0)
            {
                reportAck.reportcomments = new reportacknowledgment.reportcommentsLocalType();

                var reportcommentList = new List<reportacknowledgment.reportcommentsLocalType.reportcommentLocalType>();

                foreach (EVMessage.MarketingAuthorisation.ValidationException exception in exceptions)
                {
                    var reportcomment = new reportacknowledgment.reportcommentsLocalType.reportcommentLocalType();

                    StringBuilder sb = new StringBuilder();

                    if (exception.Severity == MarketingAuthorisation.SeverityType.Error)
                    {
                        reportcomment.severity = 1;
                        sb.Append("Error!");
                    }
                    else if (exception.Severity == MarketingAuthorisation.SeverityType.Warning)
                    {
                        reportcomment.severity = 2;
                        sb.Append("Warning!");
                    }
                    else
                    {
                        reportcomment.severity = 0;
                        sb.Append("Unknown!");
                    }
                    sb.Append(" " + exception.Message);

                    reportcomment.commenttext = sb.ToString();
                    reportcommentList.Add(reportcomment);
                }

                reportAck.reportcomments.reportcomment = reportcommentList.ToArray();
            }
            else
            {
                reportAck.reportstatusmessage = "MA validation failed.";
            }

            reportAck.reportstatuscode = "04";
            reportAck.reporttype = "02";

            reportAck.reportstatusmessage = "Validation failed. Check reportcomments section for more details.";
        }


        public void CreateStatusReportFor_MASentToEMA(Ma_message_header_PK messageheader, String readyId, String xEVPRMXMLRelativeName)
        {
            _statusReport = new statusreport();
            ConstructReportHeader();

            _statusReport.report = new report();

            var msgAck = new messageacknowledgment();
            _statusReport.report.messageacknowledgment = msgAck;

            var reportAck = new reportacknowledgment();
            _statusReport.report.reportacknowledgment = reportAck;


            FillMessageacknowledgement(messageheader, readyId);
            FillReportName("Report_MASentToEMA");

            msgAck.ev_attachment = xEVPRMXMLRelativeName;

            reportAck.reportstatuscode = "05";
            reportAck.reporttype = "01";
            reportAck.reportstatusmessage = "Marketing authorisation sent to EMA.";
        }


        public void CreateStatusReportFor_ACKReceivedFromEMA(Ma_message_header_PK messageheader, xEVMPD.ACKMessage emaACK, String readyId, String ackRelativePath)
        {
            _statusReport = new statusreport();
            ConstructReportHeader();

            _statusReport.report = new report();

            var msgAck = new messageacknowledgment();
            _statusReport.report.messageacknowledgment = msgAck;

            var reportAck = new reportacknowledgment();
            _statusReport.report.reportacknowledgment = reportAck;

            FillMessageacknowledgement(messageheader, readyId);
            FillReportName("ACKReceivedFromEMA");
           
            msgAck.ev_attachment = ackRelativePath;
            msgAck.ev_acktype = GetAckCodeString(emaACK.Message.acknowledgment.messageacknowledgment.transmissionacknowledgmentcode);
            if (msgAck.ev_acktype == null)
            {
                throw new Exception("Message acknowledgment does not have valid ACK code");
            }
            if (emaACK.Message.acknowledgment.reportacknowledgment != null)
            {
                msgAck.ev_entities = new messageacknowledgment.ev_entitiesLocalType();
                foreach (xEVMPD.reportacknowledgment ack in emaACK.Message.acknowledgment.reportacknowledgment)
                {
                    var ackReport = new messageacknowledgment.ev_entitiesLocalType.ev_entityLocalType();
                    String reportName = ack.reportname.ToString();
                    ackReport.entitytype = (!String.IsNullOrWhiteSpace(reportName) && (reportName == "ATTACHMENT" || reportName == "AUTHORISEDPRODUCT")) ? reportName : "";
                    ackReport.ev_code = (!String.IsNullOrWhiteSpace(ack.ev_code) && ack.ev_code.Length <= 60) ? ack.ev_code : "";
                    ackReport.localnumber = (!String.IsNullOrWhiteSpace(ack.localnumber) && ack.localnumber.Length <= 60) ? ack.localnumber : "";
                    int operationType;
                    ackReport.operationtype = (Int32.TryParse(ack.operationtype, out operationType)) ? operationType : 0;
                    int operationResult;
                    ackReport.operationresult = (Int32.TryParse(ack.operationresult, out operationResult)) ? operationResult : 0;
                    ackReport.operationresultdesc = (!String.IsNullOrEmpty(ack.operationresultdesc)) ? ack.operationresultdesc : "";
                    msgAck.ev_entities.ev_entity.Add(ackReport);

                }
            }

            reportAck.reportstatuscode = "06";
            reportAck.reporttype = "01";
            reportAck.reportstatusmessage = "ACK received from EMA.";
        }


        private void ConstructReportHeader()
        {
            _statusReport.reportheader = new reportheader();

            _statusReport.reportheader.reportformatversion = "1.0";
            _statusReport.reportheader.reportformatrelease = "1.0";
            _statusReport.reportheader.reportdateformat = "204";
            _statusReport.reportheader.reportdate = DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        private void FillMessageacknowledgement(Ma_message_header_PK messageheader, String readyId)
        {
            var msgAck = _statusReport.report.messageacknowledgment;
         
            if (messageheader.messagedate != null && messageheader.messagedate.HasValue)
            {
                msgAck.messagedate = messageheader.messagedate.Value.ToString("yyyyMMddHHmmss");
            }
            else
            {
                msgAck.messagedate = MISSING_DATE;
            }

            msgAck.messagedateformat = "204";

            if (readyId != null && readyId.Length <= 8)
            {
                msgAck.readymessageid = readyId;
            }
            else
            {
                msgAck.readymessageid = String.Empty;
            }

            long registrationId;

            if (messageheader.registrationid != null && messageheader.registrationid.HasValue && messageheader.registrationid.ToString().Length <= 10)
            {
                msgAck.registrationid = messageheader.registrationid.Value;
            }
            else
            {
                msgAck.registrationid = 0;
            }

            if (messageheader.registrationnumber != null && messageheader.registrationnumber.Length <= 30)
            {
                msgAck.registrationnumber = messageheader.registrationnumber;
            }
            else
            {
                msgAck.registrationnumber = string.Empty;
            }

            if (messageheader.message_file_name != null && messageheader.message_file_name.Length <= 1000)
            {
                msgAck.messagefilename = messageheader.message_file_name;
            }
            else
            {
                msgAck.messagefilename = string.Empty;
            }
        }

        private void FillReportName(String reportName)
        {
            var reportAck = _statusReport.report.reportacknowledgment;
            var msgAck = _statusReport.report.messageacknowledgment;

            if (!string.IsNullOrWhiteSpace(msgAck.registrationnumber) && (msgAck.registrationid !=0) && !string.IsNullOrWhiteSpace(msgAck.readymessageid) && (msgAck.messagedate!=MISSING_DATE) && IsValidPathPart(msgAck.registrationnumber))
            {
                reportAck.reportname = msgAck.registrationnumber + "_" + msgAck.registrationid + "_" + msgAck.readymessageid + "_"+reportName+".xml";
            }
            else
            {
                reportAck.reportname = "JUNK_"+System.Guid.NewGuid().ToString() + "_"+reportName+".xml";
            }
        }

        private bool IsValidPathPart(String path) {
            return path.IndexOfAny(System.IO.Path.GetInvalidFileNameChars()) < 0; 
        }


        public static string GetAckCodeString(Enum e)
        {
            // Get the Type of the enum
            Type t = e.GetType();
            // Get the FieldInfo for the member field with the enums name
            FieldInfo info = t.GetField(e.ToString("G"));
            // Check to see if the XmlEnumAttribute is defined on this field
            if (!info.IsDefined(typeof(System.Xml.Serialization.XmlEnumAttribute), false))
            {
                // If no XmlEnumAttribute then return null
                return null;
            }
            else
            {
                // Get the XmlEnumAttribute
                object[] o = info.GetCustomAttributes(typeof(System.Xml.Serialization.XmlEnumAttribute), false);
                System.Xml.Serialization.XmlEnumAttribute att = (System.Xml.Serialization.XmlEnumAttribute)o[0];
                return att.Name;
            }
        }

    }
}
