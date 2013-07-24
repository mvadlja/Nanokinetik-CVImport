using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using Ready.Model;
using System.Timers;
using System.Text.RegularExpressions;
using System.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Net;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace EMAService
{
    public class AS2Gateway {

        private String gatewayId;
        private String messageReceiverId;
        private String certificcateThumbprint;
        private String gatewayUrl;

        public String GatewayUrl
        {
            get { return gatewayUrl; }
            set { gatewayUrl = value; }
        }

        public String CertificcateThumbprint
        {
            get { return certificcateThumbprint; }
            set { certificcateThumbprint = value; }
        }

        public String GatewayId
        {
            get { return gatewayId; }
            set { gatewayId = value; }
        }

        public String MessageReceiverId
        {
            get { return messageReceiverId; }
            set { messageReceiverId = value; }
        }

        public AS2Gateway(String  gatewayId, String messageReceiverId, string url, string thumbprint) 
        {
            this.gatewayId = gatewayId;
            this.messageReceiverId = messageReceiverId;
            this.gatewayUrl = url;
            this.certificcateThumbprint = thumbprint;
        }

        public static AS2Gateway FromCompactString(String compactString) {

            String[] parts = compactString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 4) return null;
            return new AS2Gateway(parts[0].Trim(), parts[1].Trim(), parts[2].Trim(), parts[3].Trim());
        }
    }

    public class Workflow
    {
        #region Declarations

        private string _AS2ExchangePointURI;
        private string _EMAMDNReceiptURL;

        private string _MDNReceiptURL;
        private int _AS2Timeout;

        private Dictionary<string, AS2Gateway> senders;
        private Dictionary<string, AS2Gateway> receivers;

        private static string _serviceLogFile;

        private int invalidMDNPercentage;
        private int ACK01Percentage;
        private int ACK02Percentage;
        private int ACK03Percentage;

        private IEma_received_file_PKOperations _ema_received_file_PKOperations;
        private IEma_sent_file_PKOperations _ema_sent_file_PKOperations;
        #endregion

        #region Properties


      

        public string AS2ExchangePointURI
        {
            get { return _AS2ExchangePointURI; }
            set { _AS2ExchangePointURI = value; }
        }

        public string EMAMDNReceiptURL
        {
            get { return _EMAMDNReceiptURL; }
            set { _EMAMDNReceiptURL = value; }
        }

        public string MDNReceiptURL
        {
            get { return _MDNReceiptURL; }
            set { _MDNReceiptURL = value; }
        }

        public int AS2Timeout
        {
            get { return _AS2Timeout; }
            set { _AS2Timeout = value; }
        }

        public static string ServiceLogFile
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_serviceLogFile))
                    return "C:\\ReadyEVGatewayServiceLog.txt";
                return _serviceLogFile;
            }
            set { _serviceLogFile = value; }
        }


        #endregion

        static Workflow() {
            string serviceLogFilePath = ConfigurationManager.AppSettings["LogFilePath"];
            string serviceLogFileName = ConfigurationManager.AppSettings["LogFileName"];

            if (Directory.Exists(serviceLogFilePath))
            {
                _serviceLogFile = serviceLogFilePath;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(serviceLogFilePath);
                    _serviceLogFile = serviceLogFilePath;
                }
                catch (Exception ex)
                {
                    _serviceLogFile = "C:\\";
                }
            }

            if (!string.IsNullOrWhiteSpace(serviceLogFileName))
            {
                _serviceLogFile += serviceLogFileName;
            }
            else
            {
                _serviceLogFile += "EMAServiceLog.txt";
            }
        }

        public Workflow()
        {

            senders = ParseGateways(ConfigurationManager.AppSettings["SenderGateways"]);
            receivers = ParseGateways(ConfigurationManager.AppSettings["ReceiverGateways"]);
            string as2Timeout = ConfigurationManager.AppSettings["AS2TimeoutSec"];

            if (int.TryParse(as2Timeout, out _AS2Timeout))
            {
                if (AS2Timeout > 0)
                {
                    AS2Timeout *= 1000;
                }
                else
                {
                    AS2Timeout = 3600000;
                }
            }
            else
            {
                AS2Timeout = 3600000;
            }


            if (!Int32.TryParse(ConfigurationManager.AppSettings["ACK01Percentage"], out this.ACK01Percentage)) this.ACK01Percentage = 75;
            if (this.ACK01Percentage > 100) this.ACK01Percentage = 75;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["ACK02Percentage"], out this.ACK02Percentage)) this.ACK02Percentage = 100-this.ACK02Percentage-5;
            if (this.ACK02Percentage > 100) this.ACK02Percentage = 100 - this.ACK01Percentage;
            if (this.ACK02Percentage < 0) this.ACK02Percentage = 0;
            this.ACK03Percentage = 100 - this.ACK01Percentage - this.ACK02Percentage;
            if (this.ACK03Percentage < 0) this.ACK03Percentage = 0;

            if (!Int32.TryParse(ConfigurationManager.AppSettings["InvalidMDNPercentage"], out this.invalidMDNPercentage)) this.invalidMDNPercentage = 3;
            

            _ema_received_file_PKOperations = new Ema_received_file_PKDAL();
            _ema_sent_file_PKOperations = new Ema_sent_file_PKDAL();

        }

        public void Start() {
            SendMDNs();
            ProcessReceivedMDNs();
            SendACKs();
        }

        #region MDNs

        public void ProcessReceivedMDNs()
        {

            List<Ema_received_file_PK> files = _ema_received_file_PKOperations.GetEntitiesByTypeAndStatus("MDN", 0);
            foreach (Ema_received_file_PK file in files)
            {
                
                try
                {
                string mdn = ASCIIEncoding.ASCII.GetString(file.file_data);

                Match boundaryMatch = Regex.Match(mdn, "boundary=\"([^\"]+)\"", RegexOptions.IgnoreCase);

                if (boundaryMatch.Success)
                {
                    string[] separators = { "--" + boundaryMatch.Groups[1].Value };
                    string[] mdnHeaders = mdn.Split(separators, StringSplitOptions.None);

                    if (mdnHeaders.Count() > 2)
                    {
                        string optionsHeader = mdnHeaders[1];
                        string recipientHeader = mdnHeaders[2];

                        Match origMsgNumMatch = Regex.Match(recipientHeader, "Original-Message-ID:\\s+(\\S+)\\s+", RegexOptions.IgnoreCase);

                        if (origMsgNumMatch.Success)
                        {
                            file.mdn_orig_msg_number =origMsgNumMatch.Groups[1].Value;
                            file.status = 1;
                        }
                        else
                        {
                            file.mdn_orig_msg_number = "cannnot parse";
                            file.status = 2;
                        }
                    }
              
                }
                
                _ema_received_file_PKOperations.Save(file);
                }
                catch (Exception e) 
                {
                    LogError(e, "Failed to process MDN, EMA_RECEIVED_FILE, PK=" + file.ema_received_file_PK);
                    file.status = 2;
                    try {
                        _ema_received_file_PKOperations.Save(file);
                    } catch (Exception e2) {
                        LogError(e2, "Failed to update EMA_RECEIVED_FILE status, PK=" + file.ema_received_file_PK);
                    }
                }
                
            }
        }

        public void SendMDNs()
        {

            List<Ema_received_file_PK> files = _ema_received_file_PKOperations.GetEntitiesByTypeAndStatus("xEVPRM", 0);
            foreach (Ema_received_file_PK file in files)
            {
                
               
                try
                {
                    String mdnString = Encoding.UTF8.GetString(file.file_data);
                    int randNr = rand.Next(1, 101);
                    SendMDN(file, file.as2_from, file.as2_to, randNr<100-invalidMDNPercentage?true:false);
                }
                catch (Exception e) 
                {
                    file.status = 3;
                    _ema_received_file_PKOperations.Save(file);
                    LogError(e, "Failed to send MDN for EMA_RECEIVED_FILE PK=" + file.ema_received_file_PK);
                }
                
            }
        }

        void SendMDN(Ema_received_file_PK receivedMessage, string as2SenderID, string as2GatewayID, bool receivedSuccessfully=true)
        {
            StringBuilder sb = new StringBuilder();

            
            try
            {
                string receiptDeliveryOption = string.Empty;
                string messageID = string.Empty;

                Match receiptDeliveryOptionMatch = Regex.Match(receivedMessage.as2_header, "Receipt-Delivery-Option:\\s+([^\\r\\n]+)\\r\\n", RegexOptions.IgnoreCase);
                if (receiptDeliveryOptionMatch.Success)
                {
                    receiptDeliveryOption = receiptDeliveryOptionMatch.Groups[1].Value;
                }
                else
                {
                    if (senders.ContainsKey(as2SenderID))
                    {
                        receiptDeliveryOption = senders[as2SenderID].GatewayUrl;
                    }
                    else
                    {
                        LogError(null, "Cannot resolve receipt URL for EMA_RECEIVED_MESSAGE PK=" + receivedMessage.ema_received_file_PK);
                        return;
                    }
                }

                Match messageIDMatch = Regex.Match(receivedMessage.as2_header, "Message-Id:\\s+([^\\r\\n]+)\\r\\n", RegexOptions.IgnoreCase);
                if (messageIDMatch.Success)
                {
                    messageID = messageIDMatch.Groups[1].Value.Trim();
                }

                byte[] payload = ExtractPayload(receivedMessage.file_data, null);
                string MIC = ComputeMIC(payload);

                string dateTimeNow = DateTime.Now.ToString("dd-MM-yyyy-hh-mm");

                byte[] sentData = new byte[100000];

                HttpStatusCode code = SendAsyncMDN2Sign(new Uri(receiptDeliveryOption), "", as2GatewayID, as2SenderID, messageID, as2GatewayID, as2GatewayID, MIC, dateTimeNow, out sentData, receivedSuccessfully);

                receivedMessage.status = 1;
                _ema_received_file_PKOperations.Save(receivedMessage);
                Ema_sent_file_PK sentMDN = new Ema_sent_file_PK()
                {
                    as_to = as2SenderID,
                    as2_from = as2GatewayID,
                    file_data = sentData,
                    sent_time = DateTime.Now,
                    file_type = "MDN",
                    status = 0
                };
                _ema_sent_file_PKOperations.Save(sentMDN);

                LogEvent("Sending MDN succeeded. HttpStatusCode: " + code.ToString() + " MDN parameters: " + sb.ToString() + " Received message PK: " + receivedMessage.ema_received_file_PK);
            }
            catch (Exception ex)
            {
                string description = ConstructErrorDescription(ex, "Error at sending MDN!");

                throw new Exception(description);
            }

        }

        public byte[] ExtractPayload(byte[] data, string filePathToSave)
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

        protected string MIMEBoundary()
        {
            return "----=_Part_" + Guid.NewGuid().ToString("N") + "";
        }

        public byte[] MDNInner(string messageID, string originalRecipient, string finalRecipient, string MIC, bool receivedSuccessfully=true)
        {
            byte[] mdnInner = new byte[0];

            // get a MIME boundary
            string sBoundary = MIMEBoundary();

            byte[] sContentType = ASCIIEncoding.ASCII.GetBytes("Content-Type: multipart/report;\r\n\tboundary=\"" + sBoundary + "\";\r\n\treport-type=disposition-notification\r\n");

            byte[] bBoundary = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + "--" + sBoundary + Environment.NewLine);

            StringBuilder sbContent = new StringBuilder();
            sbContent.Append("Content-Type: text/plain; charset=us-ascii");
            sbContent.Append("\r\n");
            sbContent.Append("\r\n");
            String infoMessage = receivedSuccessfully ? "This is MDN info for message ACK number: " + messageID + ". Unless stated otherwise, the message to which this MDN applies was successfully processed." :
                               "This is MDN info for message ACK number: " + messageID + ". An error occured while processing message.";  
            sbContent.Append(infoMessage);
            sbContent.Append("\r\n");

            byte[] innerContent1 = ASCIIEncoding.ASCII.GetBytes(sbContent.ToString());

            sbContent.Clear();
            sbContent.Append("Content-Type: message/disposition-notification");
            sbContent.Append("\r\n");
            sbContent.Append("\r\n");
            sbContent.Append("Original-Recipient: rfc822; " + originalRecipient);
            sbContent.Append("\r\n");
            sbContent.Append("Final-Recipient: rfc822; " + finalRecipient);
            sbContent.Append("\r\n");
            sbContent.Append("Original-Message-ID: " + messageID);
            sbContent.Append("\r\n");
            sbContent.Append("Disposition: automatic-action/MDN-sent-automatically; processed");
            sbContent.Append("\r\n");
            sbContent.Append("Received-Content-MIC: " + MIC + ", sha1");
            sbContent.Append("\r\n");
            sbContent.Append("\r\n");
            sbContent.Append("\r\n");

            byte[] innerContent2 = ASCIIEncoding.ASCII.GetBytes(sbContent.ToString());

            byte[] bFinalFooter = ASCIIEncoding.ASCII.GetBytes("--" + sBoundary + "--\r\n");

            // Concatenate all the above together to form the message.
            mdnInner = ConcatBytes(sContentType, bBoundary, innerContent1, bBoundary, innerContent2, bFinalFooter);

            return mdnInner;
        }

        public byte[] EncodeMDNInner(byte[] arMessage, string originalrecipient)
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);

            //X509Certificate2 cert = store.Certificates.Find(X509FindType.FindByThumbprint, (object)"2BFC2DE0B850CCC919AF128ADF2DB526A5850198", true)[0];

            if (!receivers.ContainsKey(originalrecipient))
            {
                throw new Exception("MDN: SenderID \"" + originalrecipient + "\" is not valid! Check service config file.");
            }

            X509Certificate2 cert = store.Certificates.Find(X509FindType.FindByThumbprint, (object)receivers[originalrecipient].CertificcateThumbprint, true)[0];

            ContentInfo contentInfo = new ContentInfo(arMessage);

            SignedCms signedCms = new SignedCms(contentInfo, true); // <- true detaches the signature
            CmsSigner cmsSigner = new CmsSigner(cert);

            signedCms.ComputeSignature(cmsSigner);
            byte[] signature = signedCms.Encode();

            return signature;
        }

        public byte[] MDN(string messageID, string originalRecipient, string finalRecipient, out string sBoundary, string MIC, bool receivedSuccessfully=true)
        {
            byte[] mdn = new byte[0];

            sBoundary = MIMEBoundary();

            byte[] bBoundary = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + "--" + sBoundary + Environment.NewLine);
            byte[] bReportMessage = MDNInner(messageID, originalRecipient, finalRecipient, MIC, receivedSuccessfully);
            byte[] bSignatureHeader = ASCIIEncoding.ASCII.GetBytes(MIMEHeader("application/pkcs7-signature", "binary", ""));
            byte[] bSignature = EncodeMDNInner(bReportMessage, originalRecipient);
            byte[] bFinalFooter = ASCIIEncoding.ASCII.GetBytes("--" + sBoundary + "--\r\n\r\n");

            mdn = ConcatBytes(bBoundary, bReportMessage, bBoundary, bSignatureHeader, bSignature, bFinalFooter);

            return mdn;
        }

        public string MIMEHeader(string sContentType, string sEncoding, string sDisposition)
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

        public HttpStatusCode SendAsyncMDN2Sign(Uri uri, string signedFileData, string from, string to, string messageID, string originalRecipient, string finalRecipient, string MIC, string dateTimeNow, out byte[] sentData, bool receivedSuccessfully=true)
        {
            StringBuilder sbContent = new StringBuilder();

            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(uri);

            //Define the standard request objects
            http.Method = "POST";
            http.AllowAutoRedirect = true;
            http.KeepAlive = false;
            http.PreAuthenticate = false; //Means there will be two requests sent if Authentication required.
            http.SendChunked = false;

            http.UserAgent = "MY SENDING AGENT";

            //These Headers are common to all transactions
            http.Headers.Add("Mime-Version", "1.0");
            http.Headers.Add("AS2-Version", "1.2");
            http.Headers.Add("AS2-From", from);
            http.Headers.Add("AS2-To", to);
            http.Headers.Add("Message-Id", messageID);

            string sBoundary = string.Empty;
            byte[] fileData = new byte[0];

            fileData = MDN(messageID, originalRecipient, finalRecipient, out sBoundary, MIC, receivedSuccessfully);

            http.ContentLength = fileData.Length;
            http.ContentType = "multipart/signed; protocol=\"application/pkcs7-signature\"; micalg=\"sha1\"; boundary=\"" + sBoundary + "\"";

            Stream oRequestStream = http.GetRequestStream();
            Thread.Sleep(700);
            oRequestStream.Write(fileData, 0, fileData.Length);
            //Thread.Sleep(700);
            oRequestStream.Flush();
            //Thread.Sleep(700);
            oRequestStream.Close();

            HttpStatusCode statusCode = HandleWebResponse(http);

            sentData = fileData;

            return statusCode;
        }

        public byte[] ConcatBytes(params byte[][] arBytes)
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

        private HttpStatusCode HandleWebResponse(HttpWebRequest http)
        {
            HttpStatusCode statusCode;
            using (HttpWebResponse response = (HttpWebResponse)http.GetResponse())
            {
                statusCode = response.StatusCode;
                response.Close();
            }

            return statusCode;
        }


        public string ComputeMIC(byte[] data)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(data);
                return Convert.ToBase64String(hash);
            }
        }
        #endregion

        #region ACKS
       

        public void SendACKs()
        {
            List<Ema_received_file_PK> files = _ema_received_file_PKOperations.GetEntitiesByTypeAndStatus("xEVPRM", 1);
            foreach (Ema_received_file_PK file in files)
            {
                
                try
                {
                    SendACK(file);
                }
                catch (Exception e)
                {
                    file.status = 4;
                    _ema_received_file_PKOperations.Save(file);
                    LogError(e, "Failed to send ACK for EMA_RECEIVED_FILE PK=" + file.ema_received_file_PK);
                }
                
            }
        }

        Random rand = new Random(DateTime.Now.Millisecond);
        private void SendACK(Ema_received_file_PK file)
        {


            AS2Gateway sender = (file.as2_to == null || receivers.ContainsKey(file.as2_to)) ? receivers[file.as2_to] : null;
            AS2Gateway receiver = (file.as2_from == null || senders.ContainsKey(file.as2_from)) ? senders[file.as2_from] : null;

            if (sender == null)
            {
                LogError(null, "Original message sender: " + file.as2_from ?? "" + " is not defined!");
            }
            if (sender == null)
            {
                LogError(null, "Original message receiver: " + file.as2_to ?? "" + " is not defined!");
            }

            if (sender == null || receiver == null) return;

            EVMessage.Acknowledgement.AckMessage ack = new EVMessage.Acknowledgement.AckMessage();
            eudravigilance.ema.europa.eu.schema.emaxevmpd.evprm msg;
            try
            {
                msg = eudravigilance.ema.europa.eu.schema.emaxevmpd.evprm.Parse(File.ReadAllText(file.xevprm_path, Encoding.Unicode));
            }
            catch (Exception e)
            {
                LogError(e, "Failed to parse original xEVPRM file");
                msg = null;
            }


            int randNum = rand.Next(1, 101);
            if (randNum < this.ACK01Percentage)
            {
                ack.GenerateNoError(msg, sender.MessageReceiverId, receiver.MessageReceiverId);
            }
            else if (randNum < this.ACK01Percentage+this.ACK02Percentage)
            {
                ack.GenerateFieldError(msg, sender.MessageReceiverId, receiver.MessageReceiverId);
            }
            else
            {
                ack.GenerateErrors(msg, sender.MessageReceiverId, receiver.MessageReceiverId);
            }


            String ackXML = ack.MessageXML;
            String receiptUrl = "";
            Match receiptDeliveryOptionMatch = Regex.Match(file.as2_header, "Receipt-Delivery-Option:\\s+([^\\r\\n]+)\\r\\n", RegexOptions.IgnoreCase);
            if (receiptDeliveryOptionMatch.Success)
            {
                receiptUrl = receiptDeliveryOptionMatch.Groups[1].Value;
            }
            else
            {
                LogError(null, "Cannot extract MDN recipient URL for EMAReceivuedMessagePK=" + file.ema_received_file_PK);
                return;
            }


            EVMessage.AS2.AS2SendSettings as2SendSettings = new EVMessage.AS2.AS2SendSettings()
            {
                Uri = new Uri(receiver.GatewayUrl),
                MsgData = Encoding.UTF8.GetBytes(ackXML),
                Filename = Guid.NewGuid().ToString() + ".xml",
                MessageID = ack.Message.ichicsrmessageheader.messagenumb.TypedValue.ToString(),
                From = sender.GatewayId,
                To = receiver.GatewayId,
                TimeoutMs = AS2Timeout,
                SigningCertThumbPrint = sender.CertificcateThumbprint,
                RecipientCertThumbPrint = receiver.CertificcateThumbprint,

                MDNReceiptURL = sender.GatewayUrl
            };

            EVMessage.AS2.AS2Send.SendFile(as2SendSettings);
            LogEvent("ACK sent for EMA_RECEIVED_FILE PK: " + file.ema_received_file_PK);
            file.status = 2;
            _ema_received_file_PKOperations.Save(file);
            Ema_sent_file_PK sentFile = new Ema_sent_file_PK()
            {
                as2_from = sender.GatewayId,
                as_to = receiver.GatewayId,
                file_data = Encoding.Unicode.GetBytes(ackXML),
                sent_time = DateTime.Now,
                file_type = "ACK",
                status = 0
            };
            _ema_sent_file_PKOperations.Save(sentFile);
        }
        #endregion

        #region Helper Methods
        private Dictionary<String, AS2Gateway> ParseGateways(String compactgateways)
        {
            String[] gatewayStrings = compactgateways.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            Dictionary<String, AS2Gateway> gateways = new Dictionary<string, AS2Gateway>();
            foreach (String gateway in gatewayStrings) {
                AS2Gateway g = AS2Gateway.FromCompactString(gateway);
                if (g != null && !gateways.ContainsKey(g.GatewayId)) gateways.Add(g.GatewayId,g); 
            }
            return gateways;
        }
        #endregion

        #region Log events

        private static string ConstructErrorDescription(Exception ex, string description = null)
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

        public static void LogError(Exception ex, string description)
        {
            description = ConstructErrorDescription(ex, description);

            LogEventToFile(description);
        }
        public static void LogEvent(string description)
        {

            LogErrorToFile(null, description);
        }

        public static void LogErrorToFile(Exception ex, string description)
        {
            description = ConstructErrorDescription(ex, description);

            LogEventToFile(description);
        }

        public static void LogEventToFile(string description)
        {
            StringBuilder errorBuilder = new StringBuilder();

            errorBuilder.AppendLine("-------------------------------------------------------");
            errorBuilder.AppendLine(DateTime.Now.ToString() + Environment.NewLine);

            errorBuilder.AppendLine(description);

            using (StreamWriter streamWriter = new System.IO.StreamWriter(ServiceLogFile, true))
            {
                streamWriter.WriteLine(errorBuilder.ToString());
                streamWriter.Flush();
            }
        }

        #endregion
    }
}
