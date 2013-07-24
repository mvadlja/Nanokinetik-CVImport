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
using Ionic.Zip;
using Ready.Model;

namespace EMAListener
{
    public enum ReceivedMessageType
    {
        xEVPRM, MDN
    }

    public class EMAListener : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string sTo = context.Request.Headers["AS2-To"];
            string sFrom = context.Request.Headers["AS2-From"];

            DateTime dateTimeNow = DateTime.Now;

            if (context.Request.HttpMethod == "POST" || context.Request.HttpMethod == "PUT" ||
               (context.Request.HttpMethod == "GET" && context.Request.QueryString.Count > 0))
            {

                if (sFrom == null || sTo == null)
                {
                    // Invalid AS2 Request.
                    // Section 6.2 The AS2-To and AS2-From header fields MUST be present in all AS2 messages

                    BadRequest(context.Response, "Invalid or unauthorized AS2 request received.");
                    LogError("Invalid or unauthorized AS2 request received.", context, dateTimeNow);
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
                            try
                            {
                                ProcessReceivedData(context, dateTimeNow, filename);
                                context.Response.StatusCode = 200;
                                context.Response.StatusDescription = "Okay";

                                context.Response.Write(@"Request is processed successfully.");
                            }
                            catch (Exception e)
                            {
                                LogError(e, "Failed to process received data.", context, DateTime.Now, filename);
                                context.Response.StatusCode = 500;
                                context.Response.StatusDescription = "Internal server error!";

                                context.Response.Write(@"Request processing failed.");
                            }
                           
                        }
                    }
                    catch (Exception ex)
                    {
                        string description = "Error at processing received data!";
                        LogError(ex, description, context, dateTimeNow, filename);
                    }
                }
            }
            else
            {
                GetMessage(context.Response);
                LogError("GET request received.", context, dateTimeNow);
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

        public void ProcessReceivedData(HttpContext context, DateTime dateTimeNow, string filename)
        {
            string sDateTimeNow = dateTimeNow.ToString("yyyy-MM-dd_hh-mm-ss");
            string filePath = context.Server.MapPath("~/Data/");

            byte[] data = context.Request.BinaryRead(context.Request.TotalBytes);
            

            bool isEncrypted = context.Request.ContentType.Contains("application/pkcs7-mime");

            if (isEncrypted) // encrypted and signed inside
            {
                byte[] decryptedData = new byte[0];
                bool isSuccessfullyDecrypted = false;

                try
                {
                    EnvelopedCms envelopedCms = new EnvelopedCms();
                    // NB. the message will have been encrypted with your public key.
                    // The corresponding private key must be installed in the Personal Certificates folder of the user
                    // this process is running as.
                    envelopedCms.Decode(data);
                    envelopedCms.Decrypt();
                    decryptedData = envelopedCms.Encode();
                    isSuccessfullyDecrypted = true;
                }
                catch (Exception ex)
                {
                    string description = "Error at decrypting received content!";
                    LogError(ex, description, context, dateTimeNow, filename);
                }

                String decryptedFile = filePath + "DECRYPTED__" + filename + ".zip";
                String unzippedPath = decryptedFile.Replace(".zip", "");
                String messageFilePath = Path.Combine(unzippedPath, Path.GetFileName(decryptedFile).Replace(".zip", ".xml"));
                if (isSuccessfullyDecrypted)
                {
                    try
                    {
                        //Extracts first message part -> zipped xEVPRM content for xEVPRM message
                        byte[] payload = ExtractPayload(decryptedData,1);
                        File.WriteAllBytes(decryptedFile, payload);
                        
                        ZipFile file = new ZipFile(decryptedFile);
                        file.ExtractAll(unzippedPath);
                        
                    }
                    catch (Exception ex)
                    {
                        string description = "Error at saving decrypted data, and unzipping it!";
                        LogError(ex, description, context, dateTimeNow, filename);
                    }

                    try
                    {

                        string senderID = context.Request.Headers["AS2-To"];

                        StringBuilder sbHeader = new StringBuilder();
                        foreach (string key in context.Request.Headers.AllKeys)
                        {
                            sbHeader.AppendLine(key + ": " + context.Request.Headers[key]);
                        }

                        IEma_received_file_PKOperations _ema_received_file_PKOperations = new Ema_received_file_PKDAL();
                        Ema_received_file_PK receivedFile = new Ema_received_file_PK();

                        //find message file
                        foreach (FileInfo fi in new DirectoryInfo(unzippedPath).GetFiles())
                        {
                            if (fi.FullName.EndsWith(".xml")) messageFilePath = fi.FullName;
                        }

                        receivedFile.file_data = decryptedData;
                        receivedFile.received_time = dateTimeNow;
                        receivedFile.file_type = "xEVPRM";
                        receivedFile.status = 0;
                        receivedFile.as2_from = context.Request.Headers["AS2-From"];
                        receivedFile.as2_to = context.Request.Headers["AS2-To"];
                        receivedFile.xevprm_path = messageFilePath;
                        receivedFile.as2_header = sbHeader.ToString();
                        _ema_received_file_PKOperations.Save(receivedFile);


                        string sData = ASCIIEncoding.ASCII.GetString(decryptedData).Replace("\0", "");

                        Match origMsgNumMatch = Regex.Match(sData, "<originalmessagenumb>(\\S+)</originalmessagenumb>", RegexOptions.IgnoreCase);

                        string messageNumber = origMsgNumMatch.Success ? origMsgNumMatch.Groups[1].Value : null;

                        LogEvent("xEVPRM received", context, dateTimeNow, filename, messageNumber, null, ReceivedMessageType.xEVPRM);

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
                    LogError(ex, description, context, dateTimeNow, filename);
                }

                try
                {
                 
                    string senderID = context.Request.Headers["AS2-To"];
                    StringBuilder sbHeader = new StringBuilder();

                    foreach (string key in context.Request.Headers.AllKeys)
                    {
                        sbHeader.AppendLine(key + ": " + context.Request.Headers[key]);
                    }

                    IEma_received_file_PKOperations _ema_received_file_PKOperations = new Ema_received_file_PKDAL();
                    Ema_received_file_PK receivedMessage = new Ema_received_file_PK();
        
                    receivedMessage.file_data = data;
                    receivedMessage.received_time = dateTimeNow;
                    receivedMessage.file_type = "MDN";
                    receivedMessage.status = 0;
                    receivedMessage.as2_from = context.Request.Headers["AS2-From"];
                    receivedMessage.as2_to = context.Request.Headers["AS2-To"];
                    receivedMessage.xevprm_path = "";
                    receivedMessage.as2_header = sbHeader.ToString();
                    _ema_received_file_PKOperations.Save(receivedMessage);


                    string sData = ASCIIEncoding.ASCII.GetString(data);
                    Match origMsgNumMatch = Regex.Match(sData, "Original-Message-ID:\\s+(\\S+)\\s+", RegexOptions.IgnoreCase);

                    string messageNumber = origMsgNumMatch.Success ? origMsgNumMatch.Groups[1].Value : null;

                    LogEvent("MDN received", context, dateTimeNow, filename, messageNumber, null, ReceivedMessageType.MDN);
                }
                catch (Exception ex)
                {
                    string description = "Error at saving MDN to database!";
                    LogErrorToFile(ex, description, context, dateTimeNow, filename);
                }
            }
        }

        public byte[] ExtractPayload(byte[] data, int partNr)
        {
            Encoding enc = Encoding.GetEncoding("iso-8859-1");

            string message = enc.GetString(data);

            Match boundaryMatch = Regex.Match(message, "------=([^\\s]+)", RegexOptions.IgnoreCase);
            string boundary = "------=" + boundaryMatch.Groups[1].Value;
            String[] parts = message.Split(new String[] { boundary }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length <= partNr) return new byte[0];
            string payload = parts[partNr].Trim();
            String binEnc = "binary";
            payload = payload.Substring(payload.IndexOf(binEnc) + binEnc.Length).Trim();


            return enc.GetBytes(payload);
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


        #region Logs

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
        void LogError(Exception ex, string description, HttpContext context, DateTime receivedTime, string filename = null)
        {
            description = ConstructErrorDescription(ex, description);

            LogErrorToFile(description, context, receivedTime, filename);
        }

        void LogError(string description, HttpContext context, DateTime receivedTime, string filename = null)
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
            LogErrorToFile(null, description, context, receivedTime, filename);
         
        }

        void LogEvent(string description, HttpContext context, DateTime receivedTime, string filename, string messageNumber, int? receivedMessageFK, ReceivedMessageType receivedMsgType)
        {
            IAs2_handler_log_PKOperations _as2_handler_log_PKOperations = new As2_handler_log_PKDAL();

            As2_handler_log_PK logEntry = new As2_handler_log_PK()
            {
                event_type = receivedMsgType == ReceivedMessageType.xEVPRM ? "xEVPRM received" : "MDN received",
                description = description,
                received_time = receivedTime,
                filename = filename,
                message_number = messageNumber,
                received_message_FK = receivedMessageFK
            };

            FillAS2LogEntryWithContextData(ref logEntry, ref context);
            LogErrorToFile(null, description , context, receivedTime, filename, messageNumber, receivedMessageFK, receivedMsgType);
   
        
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
                errorBuilder.AppendLine("Received message type: " + (receivedMsgType == ReceivedMessageType.xEVPRM ? "xEVPRM" : "MDN"));
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

            if (!File.Exists(context.Server.MapPath("~/Data/log.txt")))
            {
                File.Create(context.Server.MapPath("~/Data/log.txt"));
            }
            string path = context.Server.MapPath("~/Data/log.txt");
            using (StreamWriter streamWriter = File.AppendText(path))
            {
                streamWriter.WriteLine(errorBuilder.ToString());
                streamWriter.Flush();
            }
        }
        #endregion

        #region Helper methods
        private Dictionary<String, String> LoadSenders()
        {
           
            Dictionary<String, String> senders = new Dictionary<string, string>();
             if (!ConfigurationManager.AppSettings.AllKeys.Contains("Senders")) return senders;
            String[] sendersParts = ConfigurationManager.AppSettings["Senders"].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (String s in sendersParts)
            {
                String[] sender = s.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                if (sender.Length != 2) continue;
                if (senders.ContainsKey(sender[0].Trim())) senders.Add(sender[0].Trim(),sender[1].Trim());
            }
            return senders;
        }
        #region
        #endregion
        #endregion
        public bool IsReusable
        {
            get { return false; }
        }
    }
}