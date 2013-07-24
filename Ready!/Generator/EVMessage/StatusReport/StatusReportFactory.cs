using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EVMessage;
using EVMessage.StatusReport.Schema;

namespace EVMessage.StatusReport
{
    public class StatusReportFactory
    {
        private statusreport _statusReport;

        public statusreport StatusReport
        {
            get { return _statusReport; }
            set { _statusReport = value; }
        }

        public string StatusReportXML
        {
            get { return "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n" +_statusReport.ToString(); }
        }

        public void CreateStatusReportFor_MAXmlValidationFailure(MarketingAuthorisation.Schema.messageheaderType messageheader, XmlValidator.XmlValidator xmlValidator)
        {
            _statusReport = new statusreport();

            ConstructReportHeader();

            _statusReport.report = new report();

            var msgAck = new messageacknowledgment();
            _statusReport.report.messageacknowledgment = msgAck;

            var reportAck = new reportacknowledgment();
            _statusReport.report.reportacknowledgment = reportAck;

            if (messageheader != null)
            {
                if (messageheader.messagedate != null && messageheader.messagedate.Length == 14)
                {
                    msgAck.messagedate = messageheader.messagedate;
                }
                else
                {
                    msgAck.messagedate = "______________";
                }

                msgAck.messagedateformat = "204";

                if (messageheader.readymessageid != null && messageheader.readymessageid.Length <= 60)
                {
                    msgAck.readymessageid = messageheader.readymessageid;
                }

                if (messageheader.registrationid != null && messageheader.registrationid.Length <= 20)
                {
                    msgAck.registrationid = messageheader.registrationid;
                }
                else
                {
                    msgAck.registrationid = string.Empty;
                }

                if (messageheader.registrationnumber != null && messageheader.registrationnumber.Length <= 20)
                {
                    msgAck.registrationnumber = messageheader.registrationnumber;
                }
                else
                {
                    msgAck.registrationnumber = string.Empty;
                }

                if (!string.IsNullOrWhiteSpace(messageheader.registrationnumber) && !string.IsNullOrWhiteSpace(messageheader.registrationid))
                {
                    reportAck.reportname = messageheader.registrationnumber + "_" + messageheader.registrationid + "_Report_1";
                }
                else
                {
                    reportAck.reportname = System.Guid.NewGuid().ToString() + "_Report_1";
                }
            }
            else
            {
                msgAck.messagedate = "______________";
                msgAck.messagedateformat = "204";
                msgAck.registrationid = string.Empty;
                msgAck.registrationnumber = string.Empty;

                reportAck.reportname = System.Guid.NewGuid().ToString() + "_Report_1";
            }

           
            reportAck.reportstatuscode = "02";
            reportAck.reporttype = "02";

            reportAck.reportstatusmessage = "Parsing XML failed. Check reportcomments section for more details.";

            if (xmlValidator != null && !xmlValidator.IsValid)
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

        private void ConstructReportHeader()
        {
            _statusReport.reportheader = new reportheader();
            
            _statusReport.reportheader.reportformatversion = "1.0";
            _statusReport.reportheader.reportformatrelease = "1.0";
            _statusReport.reportheader.reportdateformat = "204";
            _statusReport.reportheader.reportdate = DateTime.Now.ToString("yyyyMMddHHmmss");
        }
    }
}
