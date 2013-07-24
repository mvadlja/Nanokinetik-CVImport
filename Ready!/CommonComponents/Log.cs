using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Ready.Model;

namespace CommonComponents
{
    public static class Log
    {
        #region Properties

        public static XevprmLog Xevprm
        {
            get { return XevprmLog.GetInstance(); }
        }

        public static AS2ServiceLog AS2Service
        {
            get { return AS2ServiceLog.GetInstance(); }
        }

        public static AS2ServiceEmailNotificationsLog AS2ServiceEmailNotifications
        {
            get { return AS2ServiceEmailNotificationsLog.GetInstance(); }
        }

        #endregion

        #region Nested classes

        public interface ILog
        {
            string LogFile { get; set; }

            void LogError(Exception ex, string description);

            void LogEvent(string eventDescription);
        }

        public abstract class LogAbstract : ILog
        {
            public string LogFile { get; set; }

            public virtual string ConstructErrorDescription(Exception ex, string description = null)
            {
                var errorBuilder = new StringBuilder();

                if (!string.IsNullOrWhiteSpace(description))
                {
                    errorBuilder.Append(description);
                }

                if (ex != null)
                {
                    //if (errorBuilder.Length > 0)
                    //{
                    //    errorBuilder.Append(" |");
                    //}

                    errorBuilder.Append(" Exception: " + ex.Message);

                    var innerException = ex.InnerException;

                    while (innerException != null)
                    {
                        errorBuilder.Append(" | Inner Exception: " + ex.InnerException.Message);

                        innerException = innerException.InnerException;
                    }

                    errorBuilder.Append(" | StackTrace: " + ex.StackTrace);
                }

                return errorBuilder.ToString();
            }

            public virtual void LogError(Exception ex, string description)
            {
                description = ConstructErrorDescription(ex, description);

                LogEvent(description);
            }

            public abstract void LogEvent(string eventDescription);

            protected virtual void LogErrorToFile(Exception ex, string description)
            {
                description = ConstructErrorDescription(ex, description);

                LogEventToFile(description);
            }

            protected virtual void LogEventToFile(string eventDescription)
            {
                var errorBuilder = new StringBuilder();

                errorBuilder.AppendLine("-------------------------------------------------------");
                errorBuilder.AppendLine(DateTime.Now.ToString() + Environment.NewLine);

                errorBuilder.AppendLine(eventDescription);

                try
                {
                    using (var streamWriter = new StreamWriter(LogFile, true))
                    {
                        streamWriter.WriteLine(errorBuilder.ToString());
                        streamWriter.Flush();
                    }
                }
                catch { }
            }
        }

        public class AS2ServiceLog : LogAbstract
        {
            private static AS2ServiceLog _as2ServiceInstance;

            private AS2ServiceLog()
            {
                string logsFilePath = ConfigurationManager.AppSettings["LogFilePath"];
                string serviceLogFileName = ConfigurationManager.AppSettings["ServiceLogFileName"];

                if (string.IsNullOrWhiteSpace(logsFilePath))
                {
                    throw new Exception("Configuration file is missing key 'LogFilePath' or value (AppSettings section).");
                }

                if (string.IsNullOrWhiteSpace(serviceLogFileName))
                {
                    throw new Exception("Configuration file is missing key 'ServiceLogFileName' or value (AppSettings section).");
                }

                LogFile = logsFilePath + serviceLogFileName;

                if (!Directory.Exists(logsFilePath))
                {
                    Directory.CreateDirectory(logsFilePath);
                }
            }

            private AS2ServiceLog(string logFile)
            {
                LogFile = logFile;
            }

            internal static AS2ServiceLog GetInstance()
            {
                return _as2ServiceInstance ?? (_as2ServiceInstance = new AS2ServiceLog());
            }

            internal static AS2ServiceLog GetInstance(string logFile)
            {
                return _as2ServiceInstance ?? (_as2ServiceInstance = new AS2ServiceLog(logFile));
            }

            public override void LogEvent(string eventDescription)
            {
                var logEntry = new Service_log_PK()
                {
                    description = eventDescription,
                    log_time = DateTime.Now,
                };

                try
                {
                    IService_log_PKOperations serviceLogOperations = new Service_log_PKDAL();
                    serviceLogOperations.Save(logEntry);
                }
                catch (Exception ex)
                {
                    const string error = "Error at saving event to database!";
                    LogErrorToFile(ex, eventDescription + " | " + error);
                }
            }
        }

        public class AS2ServiceEmailNotificationsLog : LogAbstract
        {
            private static AS2ServiceEmailNotificationsLog _as2ServiceEmailNotificationsInstance;

            private AS2ServiceEmailNotificationsLog()
            {
                string logsFilePath = ConfigurationManager.AppSettings["LogFilePath"];
                string emailNotificationLogFileName = ConfigurationManager.AppSettings["EmailNotificationLogFileName"];

                if (string.IsNullOrWhiteSpace(logsFilePath))
                {
                    throw new Exception("Configuration file is missing key 'LogFilePath' or value (AppSettings section).");
                }

                if (string.IsNullOrWhiteSpace(emailNotificationLogFileName))
                {
                    throw new Exception("Configuration file is missing key 'EmailNotificationLogFileName' or value (AppSettings section).");
                }

                LogFile = logsFilePath + emailNotificationLogFileName;

                if (!Directory.Exists(logsFilePath))
                {
                    Directory.CreateDirectory(logsFilePath);
                }
            }

            private AS2ServiceEmailNotificationsLog(string logFile)
            {
                LogFile = logFile;
            }

            internal static AS2ServiceEmailNotificationsLog GetInstance()
            {
                return _as2ServiceEmailNotificationsInstance ?? (_as2ServiceEmailNotificationsInstance = new AS2ServiceEmailNotificationsLog());
            }

            internal static AS2ServiceEmailNotificationsLog GetInstance(string logFile)
            {
                return _as2ServiceEmailNotificationsInstance ?? (_as2ServiceEmailNotificationsInstance = new AS2ServiceEmailNotificationsLog(logFile));
            }

            public override void LogEvent(string eventDescription)
            {
                LogEventToFile(eventDescription);
            }
        }

        public class XevprmLog : LogAbstract
        {
            private static XevprmLog _xevprmInstance;

            private XevprmLog() { }

            internal static XevprmLog GetInstance()
            {
                return _xevprmInstance ?? (_xevprmInstance = new XevprmLog());
            }

            public void LogError(Exception ex, string description, int? xevprmMessagePk = null, XevprmStatus xevprmStatus = XevprmStatus.NULL, string username = null)
            {
                description = ConstructErrorDescription(ex, description);

                LogEvent(description, xevprmMessagePk, xevprmStatus, username);
            }

            public override void LogEvent(string eventDescription)
            {
                LogEvent(eventDescription, null, XevprmStatus.NULL, Thread.CurrentPrincipal.Identity.Name);
            }

            public void LogEvent(string eventDescription, int? xevprmMessagePk, XevprmStatus xevprmStatus = XevprmStatus.NULL)
            {
                LogEvent(eventDescription, xevprmMessagePk, xevprmStatus, Thread.CurrentPrincipal.Identity.Name);
            }

            public void LogEvent(string eventDescription, int? xevprmMessagePk, XevprmStatus xevprmStatus, string username)
            {
                var logEntry = new Xevprm_log_PK()
                {
                    description = eventDescription,
                    log_time = DateTime.Now,
                    xevprm_message_FK = xevprmMessagePk,
                    username = username,
                    XevprmStatus = xevprmStatus
                };

                try
                {
                    IXevprm_log_PKOperations xevprmLogOperations = new Xevprm_log_PKDAL();
                    xevprmLogOperations.Save(logEntry);
                }
                catch (Exception ex)
                {

                }
            }
        }

        #endregion
    }
}
