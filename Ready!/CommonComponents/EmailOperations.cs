using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Configuration;
using System.Text.RegularExpressions;
using Ready.Model;
using System.Configuration;

namespace CommonComponents
{
    public static class EmailOperations
    {
        public enum EventType
        {
            As2ServiceStart,
            As2ServiceStop
        }

        public enum ByWhom
        {
            System,
            User
        }

        #region Email notifications

        public static OperationResult<object> SendEmailNotifications(EventType eventType, ByWhom byWhom, string userFullName = "")
        {
            if (eventType == EventType.As2ServiceStart || eventType == EventType.As2ServiceStop)
            {
                try
                {
                    IEmail_notification_PKOperations emailNotificationOperations = new Email_notification_PKDAL();

                    var emailNotifications = emailNotificationOperations.GetEntitiesByNotificationType(NotificationType.AS2ServiceStartStop);

                    var emails = (from emailNotification in emailNotifications where IsValidEmail(emailNotification.email) select emailNotification.email).ToList();

                    if (emails.Count > 0)
                    {
                        string subject = ConstructAs2ServiceNotificationSubject();
                        string body = ConstructAs2ServiceNotificationBody(eventType, byWhom, emails, userFullName);

                        SendEmail(subject, body, true, emails, "smtp_1");
                    }
                }
                catch (Exception ex)
                {
                    return new OperationResult<object>(ex);
                }

                return new OperationResult<object>(true, "Email notifications successfully sent.");
            }

            return new OperationResult<object>(false, string.Format("There is no associated email template for this type of event ({0}). Sending email notifications failed.", eventType.ToString()));
        }

        public static void SendEmail(string subject, string body, bool isBodyHtml, IEnumerable<string> recipients, string smtpConfigName, MailPriority mailPriority = MailPriority.Normal)
        {
            var smtpSettings = (System.Net.Configuration.SmtpSection)ConfigurationManager.GetSection("mailSettings/" + smtpConfigName);

            var mail = new MailMessage
            {
                Subject = subject,
                Body = body,
                From = new MailAddress(smtpSettings.From),
                IsBodyHtml = isBodyHtml,
                Priority = mailPriority
            };

            foreach (string recipient in recipients)
            {
                mail.To.Add(new MailAddress(recipient));
            }

            var smtpServer = new SmtpClient()
             {
                 Port = smtpSettings.Network.Port,
                 Credentials = new System.Net.NetworkCredential(smtpSettings.Network.UserName, smtpSettings.Network.Password),
                 EnableSsl = smtpSettings.Network.EnableSsl,
                 Host = smtpSettings.Network.Host,
             };

            smtpServer.Send(mail);
        }

        private static string ConstructAs2ServiceNotificationSubject()
        {
            string subject = string.Format("READY! WARNING: {0:dd.MM.yyyy} AS2 Service", DateTime.Now);

            return subject;
        }

        private static string ConstructAs2ServiceNotificationBody(EventType eventType, ByWhom byWhom, IEnumerable<string> emails, string userFullName = "")
        {
            var bodySb = new StringBuilder();

            bodySb.Append("<html><body style='font-family: Arial; font-size: 12px;'>");
            bodySb.Append("Dear READY! Administrator,<br/><br/>");

            string status = eventType == EventType.As2ServiceStart
                                ? "on"
                                : eventType == EventType.As2ServiceStop ? "off" : "N/A";

            string whom = byWhom == ByWhom.System ? "SYSTEM" : byWhom == ByWhom.User ? userFullName + " through READY! interface" : "N/A";
            DateTime dt = DateTime.Now;

            string message = string.Format("Service is turned {0} by {1} on {2:dd.MM.yyyy} at {3:HH:mm:ss}", status, whom, dt, dt);

            bodySb.Append("<b style='color: red; font-family: Arial; font-size: 12px;  vertical-align: top'>" + message + "</b><br/><br/>");

            bodySb.Append("<table cellpadding='0' cellspacing='0' style='table-layout: fixed'>");
            bodySb.Append("<tr><td style='vertical-align: top; width: 160px; font-family: Arial; font-size: 12px;'>Notification email sent to:</td>");
            bodySb.Append("<td style='vertical-align: top; word-wrap: break-word;'>");

            foreach (var email in emails)
            {
                bodySb.Append("<div style='margin-bottom: 4px;'><b style='font-family: Arial; font-size: 12px;'>" + email + "</b></div>");
            }

            bodySb.Append("</td></tr></table><br/><br/>");
            bodySb.Append("READY! from Nanokinetik");
            bodySb.Append("</body></html>");

            return bodySb.ToString();
        }

        private static string ConvertPlainTextToHtml(string body)
        {
            var bodySb = new StringBuilder();

            bodySb.Append("<html><body>");

            if (!String.IsNullOrEmpty(body))
            {
                body = body.Replace("\t", "&nbsp;&nbsp;&nbsp;");
                body = body.Replace("\r\n", "<br/>");
                body = body.Replace("\n", "<br/>");

                bodySb.Append(body);
            }
            bodySb.Append("</body></html>");
            return bodySb.ToString();
        }

        private static bool IsValidEmail(string s)
        {
            const string reEmail = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (string.IsNullOrWhiteSpace(s))
                return false;
            var r = new Regex(reEmail);
            return r.IsMatch(s);
        }

        #endregion
    }
}
