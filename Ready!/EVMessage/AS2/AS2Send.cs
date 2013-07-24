using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Globalization;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;

namespace EVMessage.AS2
{

    public struct ProxySettings
    {
        public string Name;
        public string Username;
        public string Password;
        public string Domain;
    }

    public struct AS2SendSettings
    {
        public Uri Uri;

        public byte[] MsgData;

        public string From;
        public string To;

        public string Filename;
        public string RecipientCertThumbPrint;
        public string SigningCertThumbPrint;
        public string MessageID;
        public int TimeoutMs;

        public string ContentType;
        public string Boundary;

        public string MDNReceiptURL;
    }

    public struct AS2SendResult
    {
        public enum ResultType
        {
            NotSent,
            NoResponse,
            ResponseReceived
        }

        public enum ErrorType
        {
            NULL,
            ConfigurationError,
            SignError,
            EncryptError,
            SendWebRequestError,
            HandleWebResponseError
        }

        public ResultType Result;
        public HttpStatusCode ResponseCode;
        public Exception Exception;
        public ErrorType Error;
    }

    public class AS2Send
    {
        public static AS2SendResult SendFile(AS2SendSettings inAS2SendSettings)
        {
            byte[] content;
            HttpWebRequest http;
            string contentType;

            try
            {
                if (inAS2SendSettings.MsgData.Length == 0) throw new ArgumentException("Message data is empty!");

                content = inAS2SendSettings.MsgData;

                http = (HttpWebRequest)WebRequest.Create(inAS2SendSettings.Uri);

                http.Method = "POST";
                http.AllowAutoRedirect = true;
                http.KeepAlive = false;
                http.PreAuthenticate = false;
                http.SendChunked = false;
                http.UserAgent = "MY SENDING AGENT";

                //These Headers are common to all transactions
                http.Headers.Add("Mime-Version", "1.0");
                http.Headers.Add("AS2-Version", "1.2");
                http.Headers.Add("AS2-From", inAS2SendSettings.From);
                http.Headers.Add("AS2-To", inAS2SendSettings.To);
                //http.Headers.Add("Subject", inAS2SendSettings.filename);
                http.Headers.Add("Message-Id", inAS2SendSettings.MessageID);
                http.Timeout = inAS2SendSettings.TimeoutMs;

                // async MDN
                http.Headers.Add("Disposition-Notification-To", "support@nanokinetik.com");
                http.Headers.Add("Receipt-Delivery-Option", inAS2SendSettings.MDNReceiptURL);
                http.Headers.Add("Disposition-notification-options", "signed-receipt-protocol=optional,pkcs7-signature; signed-receipt-micalg=optional,sha1");

                contentType = "application/octet-stream";

                http.Headers.Add("Content-Transfer-Encoding", "binary");
                http.Headers.Add("Content-Disposition", "attachment; filename=\"" + inAS2SendSettings.Filename + "\"");
            }
            catch (Exception ex)
            {
                return new AS2SendResult() { Result = AS2SendResult.ResultType.NotSent, Exception = ex, Error = AS2SendResult.ErrorType.ConfigurationError };
            }

            try
            {
                bool sign = !string.IsNullOrEmpty(inAS2SendSettings.SigningCertThumbPrint);

                if (sign)
                {
                    // Wrap the file data with a mime header
                    inAS2SendSettings.Filename = "inline; filename=" + inAS2SendSettings.Filename;
                    content = AS2MIMEUtilities.CreateMessage(contentType, "binary", inAS2SendSettings.Filename, content);
                    content = AS2MIMEUtilities.Sign(content, inAS2SendSettings.SigningCertThumbPrint, out contentType);
                }
            }
            catch (Exception ex)
            {
                return new AS2SendResult() { Result = AS2SendResult.ResultType.NotSent, Exception = ex, Error = AS2SendResult.ErrorType.SignError };
            }

            try
            {
                bool encrypt = !string.IsNullOrEmpty(inAS2SendSettings.RecipientCertThumbPrint);
                if (encrypt)
                {
                    if (string.IsNullOrEmpty(inAS2SendSettings.RecipientCertThumbPrint))
                    {
                        throw new ArgumentNullException(inAS2SendSettings.RecipientCertThumbPrint, "Recipient certificate thumbprint must be specified");
                    }

                    byte[] signedContentTypeHeader = ASCIIEncoding.ASCII.GetBytes("Content-Type: " + contentType + Environment.NewLine);
                    byte[] contentWithContentTypeHeaderAdded = AS2MIMEUtilities.ConcatBytes(signedContentTypeHeader, content);

                    content = AS2Encryption.Encrypt(contentWithContentTypeHeaderAdded, inAS2SendSettings.RecipientCertThumbPrint, EncryptionAlgorithm.DES3);
                    //content = AS2Encryption.Encrypt(contentWithContentTypeHeaderAdded, inAS2SendSettings.signingCertThumbPrint, EncryptionAlgorithm.DES3);

                    http.ContentLength = content.Length;
                    http.ContentType = "application/pkcs7-mime; smime-type=enveloped-data; name=\"smime.p7m\"";
                }
            }
            catch (Exception ex)
            {
                return new AS2SendResult() { Result = AS2SendResult.ResultType.NotSent, Exception = ex, Error = AS2SendResult.ErrorType.EncryptError };
            }

            try
            {
                SendWebRequest(http, content);
            }
            catch (Exception ex)
            {
                return new AS2SendResult() { Result = AS2SendResult.ResultType.NotSent, Exception = ex, Error = AS2SendResult.ErrorType.SendWebRequestError };
            }

            return HandleWebResponse(http);
        }

        public static AS2SendResult SendAsyncMDN(AS2SendSettings inAS2SendSettings)
        {
            StringBuilder sbContent = new StringBuilder();

            HttpWebRequest http = (HttpWebRequest)WebRequest.Create(inAS2SendSettings.Uri);

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
            http.Headers.Add("AS2-From", inAS2SendSettings.From);
            http.Headers.Add("AS2-To", inAS2SendSettings.To);
            http.Headers.Add("Message-Id", inAS2SendSettings.MessageID);

            http.ContentLength = inAS2SendSettings.MsgData.Length;
            http.ContentType = "multipart/signed; protocol=\"application/pkcs7-signature\"; micalg=\"sha1\"; boundary=\"" + inAS2SendSettings.Boundary + "\"";

            try
            {
                SendWebRequest(http, inAS2SendSettings.MsgData);
            }
            catch (Exception ex)
            {
                return new AS2SendResult() { Result = AS2SendResult.ResultType.NotSent, Exception = ex, Error = AS2SendResult.ErrorType.SendWebRequestError };
            }

            return HandleWebResponse(http);
        }

        private static AS2SendResult HandleWebResponse(HttpWebRequest http)
        {
            HttpStatusCode statusCode;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)http.GetResponse())
                {
                    statusCode = response.StatusCode;
                    response.Close();
                }
                return new AS2SendResult() { Result = AS2SendResult.ResultType.ResponseReceived, ResponseCode = statusCode };
            }
            catch (WebException ex)
            {
                if (ex.Response != null && ex.Response is HttpWebResponse)
                {
                    return new AS2SendResult()
                               {
                                   Result = AS2SendResult.ResultType.ResponseReceived, 
                                   ResponseCode = ((HttpWebResponse)ex.Response).StatusCode, 
                                   Exception = ex,
                                   Error = AS2SendResult.ErrorType.HandleWebResponseError
                               };
                }

                return new AS2SendResult() { Result = AS2SendResult.ResultType.NoResponse, Exception = ex, Error = AS2SendResult.ErrorType.HandleWebResponseError};
            }
            catch (Exception ex)
            {
                return new AS2SendResult() { Result = AS2SendResult.ResultType.NoResponse, Exception = ex, Error = AS2SendResult.ErrorType.HandleWebResponseError };
            }
        }

        private static void SendWebRequest(HttpWebRequest http, byte[] fileData)
        {
            Stream oRequestStream = http.GetRequestStream();
            Thread.Sleep(700);
            oRequestStream.Write(fileData, 0, fileData.Length);
            oRequestStream.Flush();
            oRequestStream.Close();
        }
    }
}


