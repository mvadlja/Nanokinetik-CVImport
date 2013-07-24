using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Ready.Model;

namespace CommonComponents
{
    public static class LogOperations
    {
        public enum LogType
        {
            As2ServiceLog,
            As2ServiceEmailNotificationsLog,
            XevprmLog
        };

        public static string As2ServiceLogFile { get; set; }

        public static string As2ServiceEmailNotificationsLogFile { get; set; }

        private static string ConstructErrorDescription(Exception ex, string description = null)
        {
            var errorBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(description))
            {
                errorBuilder.Append(description);
            }

            if (ex != null)
            {
                if (errorBuilder.Length > 0)
                {
                    errorBuilder.Append(" |");
                }

                errorBuilder.Append(" Exception: " + ex.Message);

                if (ex.InnerException != null)
                {
                    errorBuilder.Append(" | Inner Exception: " + ex.InnerException.Message);
                }

                errorBuilder.Append(" | StackTrace: " + ex.StackTrace);
            }

            return errorBuilder.ToString();
        }

        public static void LogError(LogType logType, Exception ex, string description)
        {
            description = ConstructErrorDescription(ex, description);

            LogEvent(logType, description);
        }

        public static void LogEvent(LogType logType, string description)
        {
            if (logType == LogType.As2ServiceLog)
            {
                var logEntry = new Service_log_PK()
                {
                    description = description,
                    log_time = DateTime.Now,
                };

                try
                {
                    IService_log_PKOperations serviceLogOperations = new Service_log_PKDAL();
                    serviceLogOperations.Save(logEntry);
                }
                catch (Exception ex)
                {
                    string error = "Error at saving event to database!";
                    LogErrorToFile(logType, ex, description + " | " + error);
                }
            }
            else
            {
                LogEventToFile(logType, description);
            }
        }

        public static void LogErrorToFile(LogType logType, Exception ex, string description)
        {
            description = ConstructErrorDescription(ex, description);

            LogEventToFile(logType, description);
        }

        public static void LogEventToFile(LogType logType, string description)
        {
            var errorBuilder = new StringBuilder();

            errorBuilder.AppendLine("-------------------------------------------------------");
            errorBuilder.AppendLine(DateTime.Now.ToString() + Environment.NewLine);

            errorBuilder.AppendLine(description);

            try
            {
                if (logType == LogType.As2ServiceLog)
                {
                    using (var streamWriter = new StreamWriter(As2ServiceLogFile, true))
                    {
                        streamWriter.WriteLine(errorBuilder.ToString());
                        streamWriter.Flush();
                    }
                }
                else if (logType == LogType.As2ServiceEmailNotificationsLog)
                {
                    using (var streamWriter = new StreamWriter(As2ServiceEmailNotificationsLogFile, true))
                    {
                        streamWriter.WriteLine(errorBuilder.ToString());
                        streamWriter.Flush();
                    }
                }
            }
            catch { }
        }

    }
}
