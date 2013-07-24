using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Configuration;
using System.IO;
using System.Text;
using System.Xml;
using Ready.Model;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using EVMessage;
using System.Threading;
using System.Text.RegularExpressions;
using System.Configuration;

namespace EVGateway.WebApp
{
    public enum ReceivedMessageType
    {
        ACK, MDN
    }

    public enum DestinationDB
    {
        NULL,
        Production,
        Test,
        ProductionAndTest
    }

    public enum AS2HandlerMode
    {
        NULL,
        Production,
        Test,
        ProductionAndTest
    }

    public class AS2Listener : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string sTo = context.Request.Headers["AS2-To"];
            string sFrom = context.Request.Headers["AS2-From"];

            AS2HandlerMode as2handlerMode = getAs2HandlerMode();

            List<string> prodAS2IDList = getAs2IdsFromConfig("ProductionAS2IDS");
            List<string> testAS2IDList = getAs2IdsFromConfig("TestAS2IDS");

            DateTime dateTimeNow = DateTime.Now;

            if (context.Request.HttpMethod == "POST" || context.Request.HttpMethod == "PUT" ||
               (context.Request.HttpMethod == "GET" && context.Request.QueryString.Count > 0))
            {

                if (sFrom == null || sTo == null)
                {
                    // Invalid AS2 Request.
                    // Section 6.2 The AS2-To and AS2-From header fields MUST be present in all AS2 messages

                    BadRequest(context.Response, "Invalid or unauthorized AS2 request received.");

                    DestinationDB destinationDb = getDestinationDB(as2handlerMode, sTo, prodAS2IDList, testAS2IDList);

                    LogError(destinationDb, "Invalid or unauthorized AS2 request received.", context, dateTimeNow);
                }
                else
                {
                    string sDateTimeNow = dateTimeNow.ToString("yyyy-MM-dd_hh-mm-ss");
                    string filename = sDateTimeNow + "_" + System.IO.Path.GetRandomFileName().Split('.')[0];

                    try
                    {
                        using (Stream output = File.OpenWrite(context.Server.MapPath("~/Data/") + filename + ".dat"))
                        using (Stream input = context.Request.InputStream)
                        {
                            input.CopyTo(output);
                            output.Close();
                            input.Seek(0, SeekOrigin.Begin);

                            ProcessReceivedData(context, dateTimeNow, filename, as2handlerMode, prodAS2IDList, testAS2IDList);
                        }
                    }
                    catch (Exception ex)
                    {
                        string description = "Error at processing received data!";

                        DestinationDB destinationDb = getDestinationDB(as2handlerMode, sTo, prodAS2IDList, testAS2IDList);

                        LogError(destinationDb, description, context, dateTimeNow, filename);
                    }
                }
            }
            else
            {
                GetMessage(context.Response);

                DestinationDB destinationDb = getDestinationDB(as2handlerMode, null, prodAS2IDList, testAS2IDList);

                LogError(destinationDb, "GET request received.", context, dateTimeNow);
            }
        }

        private static HttpStatusCode HandleWebResponse(HttpWebRequest http)
        {
            HttpStatusCode statusCode;
            using (HttpWebResponse response = (HttpWebResponse)http.GetResponse())
            {
                statusCode = response.StatusCode;
                response.Close();
            }

            return statusCode;
        }

        public void ProcessReceivedData(HttpContext context, DateTime dateTimeNow, string filename, AS2HandlerMode as2handlerMode, List<string> prodAS2IDList, List<string> testAS2IDList)
        {
            string sDateTimeNow = dateTimeNow.ToString("yyyy-MM-dd_hh-mm-ss");
            string filePath = context.Server.MapPath("~/Data/");

            byte[] data = context.Request.BinaryRead(context.Request.TotalBytes);

            bool isEncrypted = context.Request.ContentType.Contains("application/pkcs7-mime");

            string senderID = context.Request.Headers["AS2-To"];

            if (isEncrypted) // encrypted and signed inside
            {
                byte[] decryptedData = new byte[0];
                bool isSuccessfullyDecrypted = false;

                try
                {
                    EnvelopedCms envelopedCms = new EnvelopedCms();
                    envelopedCms.Decode(data);
                    envelopedCms.Decrypt();

                    decryptedData = envelopedCms.Encode();

                    isSuccessfullyDecrypted = true;
                }
                catch (Exception ex)
                {
                    string description = "Error at decrypting received content!";

                    DestinationDB destinationDb = getDestinationDB(as2handlerMode, senderID, prodAS2IDList, testAS2IDList);

                    LogError(destinationDb, description, context, dateTimeNow, filename);
                }

                if (isSuccessfullyDecrypted)
                {
                    try
                    {
                        File.WriteAllBytes(filePath + "DECRYPTED__" + filename + ".txt", decryptedData);
                    }
                    catch (Exception ex)
                    {
                        string description = "Error at saving ACK to file system!";

                        DestinationDB destinationDb = getDestinationDB(as2handlerMode, senderID, prodAS2IDList, testAS2IDList);

                        LogError(destinationDb, ex, description, context, dateTimeNow, filename);
                    }

                    try
                    {
                        StringBuilder sbHeader = new StringBuilder();

                        foreach (string key in context.Request.Headers.AllKeys)
                        {
                            sbHeader.AppendLine(key + ": " + context.Request.Headers[key]);
                        }

                        int? receivedMessageFK = null;

                        DestinationDB destinationDb = getDestinationDB(as2handlerMode, senderID, prodAS2IDList, testAS2IDList);

                        if (destinationDb == DestinationDB.Production || destinationDb == DestinationDB.ProductionAndTest)
                        {
                            IRecieved_message_PKOperations _recieved_message_PKOperations = new Recieved_message_PKDAL();
                            Recieved_message_PK receivedMessage = new Recieved_message_PK();
                            receivedMessage.msg_data = decryptedData;
                            receivedMessage.received_time = dateTimeNow;
                            receivedMessage.msg_type = (int)ReceivedMessageType.ACK;
                            receivedMessage.processed = false;
                            receivedMessage.as_header = sbHeader.ToString();

                            receivedMessage = _recieved_message_PKOperations.Save(receivedMessage);

                            receivedMessageFK = receivedMessage.recieved_message_PK;
                        }

                        if (destinationDb == DestinationDB.Test || destinationDb == DestinationDB.ProductionAndTest)
                        {
                            IRecieved_message_PKTestOperations _recieved_message_PKTestOperations = new Recieved_message_PKTestDAL();
                            Recieved_message_PKTest receivedMessage = new Recieved_message_PKTest();
                            receivedMessage.msg_data = decryptedData;
                            receivedMessage.received_time = dateTimeNow;
                            receivedMessage.msg_type = (int)ReceivedMessageType.ACK;
                            receivedMessage.processed = false;
                            receivedMessage.as_header = sbHeader.ToString();

                            receivedMessage = _recieved_message_PKTestOperations.Save(receivedMessage);

                            receivedMessageFK = receivedMessage.recieved_message_PK;
                        }

                        string sData = ASCIIEncoding.ASCII.GetString(decryptedData).Replace("\0", "");

                        Match origMsgNumMatch = Regex.Match(sData, "<originalmessagenumb>(\\S+)</originalmessagenumb>", RegexOptions.IgnoreCase);

                        string messageNumber = origMsgNumMatch.Success ? origMsgNumMatch.Groups[1].Value : null;

                        LogEvent(destinationDb, "ACK received", context, dateTimeNow, filename, messageNumber, receivedMessageFK, ReceivedMessageType.ACK);
                    }
                    catch (Exception ex)
                    {
                        string description = "Error at saving ACK to database!";
                        LogErrorToFile(ex, description, context, dateTimeNow, filename);
                    }
                }
            }
            else //MDN
            {

                try
                {
                    File.WriteAllBytes(filePath + "MDN__" + filename + ".txt", data);
                }
                catch (Exception ex)
                {
                    string description = "Error at saving MDN to file system!";

                    DestinationDB destinationDb = getDestinationDB(as2handlerMode, senderID, prodAS2IDList, testAS2IDList);

                    LogError(destinationDb, ex, description, context, dateTimeNow, filename);
                }

                try
                {
                    StringBuilder sbHeader = new StringBuilder();

                    foreach (string key in context.Request.Headers.AllKeys)
                    {
                        sbHeader.AppendLine(key + ": " + context.Request.Headers[key]);
                    }

                    int? receivedMessageFK = null;

                    DestinationDB destinationDb = getDestinationDB(as2handlerMode, senderID, prodAS2IDList, testAS2IDList);

                    if (destinationDb == DestinationDB.Production || destinationDb == DestinationDB.ProductionAndTest)
                    {
                        IRecieved_message_PKOperations _recieved_message_PKOperations = new Recieved_message_PKDAL();
                        Recieved_message_PK receivedMessage = new Recieved_message_PK();

                        receivedMessage.msg_data = data;
                        receivedMessage.received_time = dateTimeNow;
                        receivedMessage.msg_type = (int)ReceivedMessageType.MDN;
                        receivedMessage.processed = false;
                        receivedMessage.as_header = sbHeader.ToString();

                        receivedMessage = _recieved_message_PKOperations.Save(receivedMessage);

                        receivedMessageFK = receivedMessage.recieved_message_PK;
                    }

                    if (destinationDb == DestinationDB.Test || destinationDb == DestinationDB.ProductionAndTest)
                    {
                        IRecieved_message_PKTestOperations _recieved_message_PKTestOperations = new Recieved_message_PKTestDAL();
                        Recieved_message_PKTest receivedMessage = new Recieved_message_PKTest();

                        receivedMessage.msg_data = data;
                        receivedMessage.received_time = dateTimeNow;
                        receivedMessage.msg_type = (int)ReceivedMessageType.MDN;
                        receivedMessage.processed = false;
                        receivedMessage.as_header = sbHeader.ToString();

                        receivedMessage = _recieved_message_PKTestOperations.Save(receivedMessage);

                        receivedMessageFK = receivedMessage.recieved_message_PK;
                    }

                    string sData = ASCIIEncoding.ASCII.GetString(data);
                    Match origMsgNumMatch = Regex.Match(sData, "Original-Message-ID:\\s+(\\S+)\\s+", RegexOptions.IgnoreCase);

                    string messageNumber = origMsgNumMatch.Success ? origMsgNumMatch.Groups[1].Value : null;

                    LogEvent(destinationDb, "MDN received", context, dateTimeNow, filename, messageNumber, receivedMessageFK, ReceivedMessageType.MDN);
                }
                catch (Exception ex)
                {
                    string description = "Error at saving MDN to database!";
                    LogErrorToFile(ex, description, context, dateTimeNow, filename);
                }
            }
        }

        public static void BadRequest(HttpResponse response, string message)
        {
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.StatusDescription = "Bad context.Request";

            response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 3.2 Final//EN"">"
            + @"<HTML><HEAD><TITLE>400 Bad context.Request</TITLE></HEAD>"
            + @"<BODY><H1>400 Bad context.Request</H1><HR>There was a error processing this context.Request.  The reason given by the server was:"
            + @"<P><font size=-1>" + message + @"</Font><HR></BODY></HTML>");
        }

        public static void GetMessage(HttpResponse response)
        {
            response.StatusCode = 200;
            response.StatusDescription = "Okay";

            response.Write(@"<!DOCTYPE HTML PUBLIC ""-//W3C//DTD HTML 3.2 Final//EN"">"
            + @"<HTML><HEAD><TITLE>Generic AS2 Receiver</TITLE></HEAD>"
            + @"<BODY><H1>200 Okay</H1><HR>This is to inform you that the AS2 interface is working and is "
            + @"accessable from your location.  This is the standard response to all who would send a GET "
            + @"request to this page instead of the POST context.Request defined by the AS2 Draft Specifications.<HR></BODY></HTML>");
        }

        private void FillAS2LogEntryWithContextData(ref As2_handler_log_PK logEntry, ref HttpContext context)
        {
            logEntry.connection = context.Request.Headers.AllKeys.Contains("Connection") ? context.Request.Headers["Connection"] : null;
            logEntry.date = context.Request.Headers.AllKeys.Contains("Date") ? context.Request.Headers["Date"] : null;
            logEntry.content_length = context.Request.Headers.AllKeys.Contains("Content-Length") ? context.Request.Headers["Content-Length"] : null;
            logEntry.content_type = context.Request.Headers.AllKeys.Contains("Content-Type") ? context.Request.Headers["Content-Type"] : null;
            logEntry.from = context.Request.Headers.AllKeys.Contains("From") ? context.Request.Headers["From"] : null;
            logEntry.host = context.Request.Headers.AllKeys.Contains("Host") ? context.Request.Headers["Host"] : null;
            logEntry.user_agent = context.Request.Headers.AllKeys.Contains("User-Agent") ? context.Request.Headers["User-Agent"] : null;
            logEntry.message_ID = context.Request.Headers.AllKeys.Contains("Message-ID") ? context.Request.Headers["Message-ID"] : null;
            logEntry.mime_version = context.Request.Headers.AllKeys.Contains("MIME-Version") ? context.Request.Headers["MIME-Version"] : null;
            logEntry.content_transfer_encoding = context.Request.Headers.AllKeys.Contains("Content-Transfer-Encoding") ? context.Request.Headers["Content-Transfer-Encoding"] : null;
            logEntry.content_disposition = context.Request.Headers.AllKeys.Contains("Content-Disposition") ? context.Request.Headers["Content-Disposition"] : null;
            logEntry.disposition_notification_to = context.Request.Headers.AllKeys.Contains("Disposition-Notification-To") ? context.Request.Headers["Disposition-Notification-To"] : null;
            logEntry.disposition_notification_options = context.Request.Headers.AllKeys.Contains("Disposition-Notification-Options") ? context.Request.Headers["Disposition-Notification-Options"] : null;
            logEntry.receipt_delivery_option = context.Request.Headers.AllKeys.Contains("Receipt-Delivery-Option") ? context.Request.Headers["Receipt-Delivery-Option"] : null;
            logEntry.ediint_features = context.Request.Headers.AllKeys.Contains("EDIINT-Features") ? context.Request.Headers["EDIINT-Features"] : null;
            logEntry.as2_version = context.Request.Headers.AllKeys.Contains("AS2-Version") ? context.Request.Headers["AS2-Version"] : null;
            logEntry.as2_to = context.Request.Headers.AllKeys.Contains("AS2-To") ? context.Request.Headers["AS2-To"] : null;
            logEntry.as2_from = context.Request.Headers.AllKeys.Contains("AS2-From") ? context.Request.Headers["AS2-From"] : null;
        }

        private void FillAS2LogEntryWithContextData(ref As2_handler_log_PKTest logEntry, ref HttpContext context)
        {
            logEntry.connection = context.Request.Headers.AllKeys.Contains("Connection") ? context.Request.Headers["Connection"] : null;
            logEntry.date = context.Request.Headers.AllKeys.Contains("Date") ? context.Request.Headers["Date"] : null;
            logEntry.content_length = context.Request.Headers.AllKeys.Contains("Content-Length") ? context.Request.Headers["Content-Length"] : null;
            logEntry.content_type = context.Request.Headers.AllKeys.Contains("Content-Type") ? context.Request.Headers["Content-Type"] : null;
            logEntry.from = context.Request.Headers.AllKeys.Contains("From") ? context.Request.Headers["From"] : null;
            logEntry.host = context.Request.Headers.AllKeys.Contains("Host") ? context.Request.Headers["Host"] : null;
            logEntry.user_agent = context.Request.Headers.AllKeys.Contains("User-Agent") ? context.Request.Headers["User-Agent"] : null;
            logEntry.message_ID = context.Request.Headers.AllKeys.Contains("Message-ID") ? context.Request.Headers["Message-ID"] : null;
            logEntry.mime_version = context.Request.Headers.AllKeys.Contains("MIME-Version") ? context.Request.Headers["MIME-Version"] : null;
            logEntry.content_transfer_encoding = context.Request.Headers.AllKeys.Contains("Content-Transfer-Encoding") ? context.Request.Headers["Content-Transfer-Encoding"] : null;
            logEntry.content_disposition = context.Request.Headers.AllKeys.Contains("Content-Disposition") ? context.Request.Headers["Content-Disposition"] : null;
            logEntry.disposition_notification_to = context.Request.Headers.AllKeys.Contains("Disposition-Notification-To") ? context.Request.Headers["Disposition-Notification-To"] : null;
            logEntry.disposition_notification_options = context.Request.Headers.AllKeys.Contains("Disposition-Notification-Options") ? context.Request.Headers["Disposition-Notification-Options"] : null;
            logEntry.receipt_delivery_option = context.Request.Headers.AllKeys.Contains("Receipt-Delivery-Option") ? context.Request.Headers["Receipt-Delivery-Option"] : null;
            logEntry.ediint_features = context.Request.Headers.AllKeys.Contains("EDIINT-Features") ? context.Request.Headers["EDIINT-Features"] : null;
            logEntry.as2_version = context.Request.Headers.AllKeys.Contains("AS2-Version") ? context.Request.Headers["AS2-Version"] : null;
            logEntry.as2_to = context.Request.Headers.AllKeys.Contains("AS2-To") ? context.Request.Headers["AS2-To"] : null;
            logEntry.as2_from = context.Request.Headers.AllKeys.Contains("AS2-From") ? context.Request.Headers["AS2-From"] : null;
        }

        private AS2HandlerMode getAs2HandlerMode()
        {
            bool isProdDefined = false;
            bool isTestDefined = false;

            string prodConnectionString = string.Empty;
            string testConnectionString = string.Empty;

            foreach (ConnectionStringSettings connectionString in ConfigurationManager.ConnectionStrings)
            {
                if (!string.IsNullOrWhiteSpace(connectionString.Name))
                {
                    if (connectionString.Name.Trim() == "Ready_poss_wc")
                    {
                        isProdDefined = true;
                        if (!string.IsNullOrWhiteSpace(connectionString.ConnectionString))
                            prodConnectionString = connectionString.ConnectionString.Trim();
                    }
                    else if (connectionString.Name.Trim() == "Ready_poss_wcTest")
                    {
                        isTestDefined = true;
                        if (!string.IsNullOrWhiteSpace(connectionString.ConnectionString))
                            testConnectionString = connectionString.ConnectionString.Trim();
                    }
                }
            }

            if (isProdDefined && isTestDefined)
            {
                if (prodConnectionString == testConnectionString)
                {
                    return AS2HandlerMode.Production;
                }
                else
                {
                    return AS2HandlerMode.ProductionAndTest;
                }
            }

            if (isProdDefined && !isTestDefined)
            {
                return AS2HandlerMode.Production;
            }

            if (!isProdDefined && isTestDefined)
            {
                return AS2HandlerMode.Test;
            }

            return AS2HandlerMode.NULL;
        }

        private List<string> getAs2IdsFromConfig(string key)
        {
            string ids = ConfigurationManager.AppSettings[key];

            if (!string.IsNullOrWhiteSpace(ids))
            {
                List<string> idsList = ids.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                idsList.ForEach(delegate(string item) { if (item != null) item.Trim(); });

                return idsList;
            }

            return new List<string>();
        }

        private DestinationDB getDestinationDB(AS2HandlerMode as2HandlerMode, string senderID, List<string> prodIds, List<string> testIds)
        {
            if (as2HandlerMode == AS2HandlerMode.NULL) return DestinationDB.NULL;

            if (as2HandlerMode == AS2HandlerMode.Production) return DestinationDB.Production;

            if (as2HandlerMode == AS2HandlerMode.Test) return DestinationDB.Test;

            if (as2HandlerMode == AS2HandlerMode.ProductionAndTest)
            {
                if (!string.IsNullOrWhiteSpace(senderID))
                {
                    if (prodIds.Contains(senderID))
                    {
                        return DestinationDB.Production;
                    }
                    else if (testIds.Contains(senderID))
                    {
                        return DestinationDB.Test;
                    }
                    else
                    {
                        return DestinationDB.ProductionAndTest;
                    }
                }
                else
                {
                    return DestinationDB.ProductionAndTest;
                }
            }

            return DestinationDB.NULL;
        }

        private string ConstructErrorDescription(Exception ex, string description = null)
        {
            StringBuilder errorBuilder = new StringBuilder();

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

        void LogError(DestinationDB destinationDb, Exception ex, string description, HttpContext context, DateTime receivedTime, string filename = null)
        {
            description = ConstructErrorDescription(ex, description);

            LogError(destinationDb, description, context, receivedTime, filename);
        }

        void LogError(DestinationDB destinationDb, string description, HttpContext context, DateTime receivedTime, string filename = null)
        {
            try
            {
                if (destinationDb == DestinationDB.Production || destinationDb == DestinationDB.ProductionAndTest)
                {
                    IAs2_handler_log_PKOperations _as2_handler_log_PKOperations = new As2_handler_log_PKDAL();

                    As2_handler_log_PK logEntry = new As2_handler_log_PK()
                    {
                        event_type = "ERROR",
                        description = description,
                        received_time = receivedTime,
                        filename = filename,
                        message_number = null,
                        received_message_FK = null
                    };

                    FillAS2LogEntryWithContextData(ref logEntry, ref context);
                    _as2_handler_log_PKOperations.Save(logEntry);
                }

                if (destinationDb == DestinationDB.Test || destinationDb == DestinationDB.ProductionAndTest)
                {
                    IAs2_handler_log_PKTestOperations _as2_handler_log_PKTestOperations = new As2_handler_log_PKTestDAL();

                    As2_handler_log_PKTest logEntryTest = new As2_handler_log_PKTest()
                    {
                        event_type = "ERROR",
                        description = description,
                        received_time = receivedTime,
                        filename = filename,
                        message_number = null,
                        received_message_FK = null
                    };

                    FillAS2LogEntryWithContextData(ref logEntryTest, ref context);
                    _as2_handler_log_PKTestOperations.Save(logEntryTest);
                }

                if (destinationDb == DestinationDB.NULL)
                {
                    LogErrorToFile(new Exception("Destination database is NULL!"), string.Empty, context, receivedTime, filename);
                }
            }
            catch (Exception ex)
            {
                string error = "Error at saving error to database!";
                LogErrorToFile(ex, description + " | " + error, context, receivedTime, filename);
            }
        }

        void LogEvent(DestinationDB destinationDb, string description, HttpContext context, DateTime receivedTime, string filename, string messageNumber, int? receivedMessageFK, ReceivedMessageType receivedMsgType)
        {
            try
            {
                if (destinationDb == DestinationDB.Production || destinationDb == DestinationDB.ProductionAndTest)
                {
                    IAs2_handler_log_PKOperations _as2_handler_log_PKOperations = new As2_handler_log_PKDAL();
                    
                    As2_handler_log_PK logEntry = new As2_handler_log_PK()
                    {
                        event_type = receivedMsgType == ReceivedMessageType.ACK ? "ACK received" : "MDN received",
                        description = description,
                        received_time = receivedTime,
                        filename = filename,
                        message_number = messageNumber,
                        received_message_FK = receivedMessageFK
                    };

                    FillAS2LogEntryWithContextData(ref logEntry, ref context);
                    _as2_handler_log_PKOperations.Save(logEntry);
                }

                if (destinationDb == DestinationDB.Test || destinationDb == DestinationDB.ProductionAndTest)
                {
                    IAs2_handler_log_PKTestOperations _as2_handler_log_PKTestOperations = new As2_handler_log_PKTestDAL();

                    As2_handler_log_PKTest logEntryTest = new As2_handler_log_PKTest()
                    {
                        event_type = receivedMsgType == ReceivedMessageType.ACK ? "ACK received" : "MDN received",
                        description = description,
                        received_time = receivedTime,
                        filename = filename,
                        message_number = messageNumber,
                        received_message_FK = receivedMessageFK
                    };

                    FillAS2LogEntryWithContextData(ref logEntryTest, ref context);
                    _as2_handler_log_PKTestOperations.Save(logEntryTest);
                }

                if (destinationDb == DestinationDB.NULL)
                {
                    LogErrorToFile(new Exception("Destination database is NULL!"), string.Empty, context, receivedTime, filename, messageNumber, receivedMessageFK);
                }
            }
            catch (Exception ex)
            {
                string error = "Error at saving event to database!";
                LogErrorToFile(ex, description + " | " + error, context, receivedTime, filename, messageNumber, receivedMessageFK, receivedMsgType);
            }
        }

        void LogErrorToFile(Exception ex, string description, HttpContext context, DateTime receivedTime, string filename = null, string messageNumber = null, int? receivedMessageFK = null, ReceivedMessageType? receivedMsgType = null)
        {
            description = ConstructErrorDescription(ex, description);

            LogErrorToFile(description, context, receivedTime, filename, messageNumber, receivedMessageFK, receivedMsgType);
        }

        void LogErrorToFile(string description, HttpContext context, DateTime receivedTime, string filename = null, string messageNumber = null, int? receivedMessageFK = null, ReceivedMessageType? receivedMsgType = null)
        {
            StringBuilder errorBuilder = new StringBuilder();

            errorBuilder.AppendLine("-------------------------------------------------------");
            errorBuilder.AppendLine(DateTime.Now.ToString() + Environment.NewLine);


            foreach (string key in context.Request.Headers.AllKeys)
            {
                errorBuilder.AppendLine(key + ": " + context.Request.Headers[key]);
            }

            errorBuilder.AppendLine();

            if (receivedMsgType != null)
            {
                errorBuilder.AppendLine("Received message type: " + (receivedMsgType == ReceivedMessageType.ACK ? "ACK" : "MDN"));
            }
            if (messageNumber != null)
            {
                errorBuilder.AppendLine("Message number: " + messageNumber);
            }
            if (filename != null)
            {
                errorBuilder.AppendLine("File name: " + filename);
            }
            if (receivedMessageFK != null)
            {
                errorBuilder.AppendLine("Received message FK: " + receivedMessageFK);
            }

            errorBuilder.AppendLine("ERROR description:" + Environment.NewLine);
            errorBuilder.AppendLine(description);

            try
            {
                string path = context.Server.MapPath("~/Data/log.txt");
                using (StreamWriter streamWriter = File.AppendText(path))
                {
                    streamWriter.WriteLine(errorBuilder.ToString());
                    streamWriter.Flush();
                }
            }
            catch
            {
                // Could not save to database nor to a auxiliary log file
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}