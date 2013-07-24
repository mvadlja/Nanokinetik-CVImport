using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;

namespace EVMessage.AS2
{
    public class AS2MIMEUtilities
    {
        public AS2MIMEUtilities()
        {
        }
 
        /// <summary>
        /// return a unique MIME style boundary
        /// this needs to be unique enought not to occur within the data
        /// and so is a Guid without - or { } characters.
        /// </summary>
        /// <returns></returns>
        protected static string MIMEBoundary()
        {
            return "----=_Part_" + Guid.NewGuid().ToString("N") + "";
        }
 
        /// <summary>
        /// Creates the a Mime header out of the components listed.
        /// </summary>
        /// <param name="sContentType">Content type</param>
        /// <param name="sEncoding">Encoding method</param>
        /// <param name="sDisposition">Disposition options</param>
        /// <returns>A string containing the three headers.</returns>
        public static string MIMEHeader(string sContentType, string sEncoding, string sDisposition)
        {
            string sOut = "";
 
            sOut = "Content-Type: " + sContentType + Environment.NewLine;
           
            if (sDisposition != "" )
                sOut += "Content-Disposition: " + sDisposition + Environment.NewLine;

            if (sEncoding != "")
                sOut += "Content-Transfer-Encoding: " + sEncoding + Environment.NewLine;

            sOut = sOut + Environment.NewLine;
 
            return sOut;
        }
 
        /// <summary>
        /// Return a single array of bytes out of all the supplied byte arrays.
        /// </summary>
        /// <param name="arBytes">Byte arrays to add</param>
        /// <returns>The single byte array.</returns>
        public static byte[] ConcatBytes(params byte[][] arBytes)
        {
            long lLength = 0;
            long lPosition = 0;
 
            //Get total size required.
            foreach(byte[] ar in arBytes)
                lLength += ar.Length;
 
            //Create new byte array
            byte[] toReturn = new byte[lLength];
                
            //Fill the new byte array
            foreach(byte[] ar in arBytes)
            {
                ar.CopyTo(toReturn,lPosition);
                lPosition += ar.Length;
            }
 
            return toReturn;
        }
 
        /// <summary>
        /// Create a Message out of byte arrays (this makes more sense than the above method)
        /// </summary>
        /// <param name="sContentType">Content type ie multipart/report</param>
        /// <param name="sEncoding">The encoding provided...</param>
        /// <param name="sDisposition">The disposition of the message...</param>
        /// <param name="abMessageParts">The byte arrays that make up the components</param>
        /// <returns>The message as a byte array.</returns>
        public static byte[] CreateMessage(string sContentType, string sEncoding, string sDisposition, params byte[][] abMessageParts)
        {
            int iHeaderLength=0;
            return CreateMessage(sContentType, sEncoding, sDisposition, out iHeaderLength, abMessageParts);
        }
        /// <summary>
        /// Create a Message out of byte arrays (this makes more sense than the above method)
        /// </summary>
        /// <param name="sContentType">Content type ie multipart/report</param>
        /// <param name="sEncoding">The encoding provided...</param>
        /// <param name="sDisposition">The disposition of the message...</param>
        /// <param name="iHeaderLength">The length of the headers.</param>
        /// <param name="abMessageParts">The message parts.</param>
        /// <returns>The message as a byte array.</returns>
        public static byte[] CreateMessage(string sContentType, string sEncoding, string sDisposition, out int iHeaderLength, params byte[][] abMessageParts)
        {
            long lLength = 0;
            long lPosition = 0;
 
            // Only one part... Add headers only...
            if (abMessageParts.Length==1)
            {
                byte[] bHeader = ASCIIEncoding.ASCII.GetBytes(MIMEHeader(sContentType, sEncoding, sDisposition));
                iHeaderLength = bHeader.Length;
                return ConcatBytes(bHeader, abMessageParts[0]);
            }
            else
            {
                // get boundary and "static" subparts.
                string sBoundary = MIMEBoundary();
                byte[] bPackageHeader = ASCIIEncoding.ASCII.GetBytes(MIMEHeader(sContentType + "; boundary=\"" + sBoundary + "\"", sEncoding, sDisposition));
                byte[] bBoundary = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + "------" + sBoundary + Environment.NewLine);
                byte[] bFinalFooter = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + "------" + sBoundary + "--" + Environment.NewLine);
 
                //Calculate the total size required.
                iHeaderLength = bPackageHeader.Length;
 
                foreach(byte[] ar in abMessageParts)
                    lLength += ar.Length;
                lLength += iHeaderLength + bBoundary.Length*abMessageParts.Length +
                    bFinalFooter.Length;
 
                //Create new byte array to that size.
                byte[] toReturn = new byte[lLength];
                
                //Copy the headers in.
                bPackageHeader.CopyTo(toReturn, lPosition);
                lPosition += bPackageHeader.Length;
 
                //Fill the new byte array in by coping the message parts.
                foreach(byte[] ar in abMessageParts)
                {
                    bBoundary.CopyTo(toReturn, lPosition);
                    lPosition += bBoundary.Length;
 
                    ar.CopyTo(toReturn,lPosition);
                    lPosition += ar.Length;
                }
 
                //Finally add the footer boundary.
                bFinalFooter.CopyTo(toReturn, lPosition);
 
                return toReturn;
            }
        }

        public static byte[] Sign(byte[] arMessage, string signingCertThumbPrint, out string sContentType)
        {
            byte[] bInPKCS7 = new byte[0];

            // get a MIME boundary
            string sBoundary = MIMEBoundary();

            // Get the Headers for the entire message.
            sContentType = "multipart/signed; protocol=\"application/pkcs7-signature\"; micalg=sha1; boundary=\"" + sBoundary + "\"";

            // Define the boundary byte array.
            byte[] bBoundary = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + "--" + sBoundary + Environment.NewLine);

            // Encode the header for the signature portion.
            byte[] bSignatureHeader = ASCIIEncoding.ASCII.GetBytes(MIMEHeader("application/pkcs7-signature; name=\"smime.p7s\"", "binary", "attachment; filename=\"smime.p7s\""));

            // Get the signature.
            byte[] bSignature = AS2Encryption.Encode(arMessage, signingCertThumbPrint);

            // Calculate the final footer elements.
            byte[] bFinalFooter = ASCIIEncoding.ASCII.GetBytes("--" + sBoundary + "--" + Environment.NewLine);

            // Concatenate all the above together to form the message.
            bInPKCS7 = ConcatBytes(bBoundary, arMessage, bBoundary, bSignatureHeader, bSignature, bFinalFooter);

            return bInPKCS7;
        }

        /// <summary>
        /// Signs a message and returns a MIME encoded array of bytes containing the signature.
        /// </summary>
        /// <param name="arMessage"></param>
        /// <param name="bPackageHeader"></param>
        /// <returns></returns>
        public static byte[] Sign(byte[] arMessage, string signerCert, string signerPassword, out string sContentType)
        {
            byte[] bInPKCS7 = new byte[0];
 
            // get a MIME boundary
            string sBoundary = MIMEBoundary();
 
            // Get the Headers for the entire message.
            sContentType = "multipart/signed; protocol=\"application/pkcs7-signature\"; micalg=sha1; boundary=\"" + sBoundary + "\"";
            
            // Define the boundary byte array.
            byte[] bBoundary = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + "--" + sBoundary + Environment.NewLine);
                
            // Encode the header for the signature portion.
            byte[] bSignatureHeader = ASCIIEncoding.ASCII.GetBytes(MIMEHeader("application/pkcs7-signature; name=\"smime.p7s\"", "binary", "attachment; filename=\"smime.p7s\""));
    
            // Get the signature.
            byte[] bSignature = AS2Encryption.Encode(arMessage, signerCert, signerPassword);
 
            // Calculate the final footer elements.
            byte[] bFinalFooter = ASCIIEncoding.ASCII.GetBytes("--" + sBoundary + "--" + Environment.NewLine);
 
            // Concatenate all the above together to form the message.
            bInPKCS7 = ConcatBytes(bBoundary, arMessage, bBoundary, bSignatureHeader, bSignature, bFinalFooter);
 
            return bInPKCS7;
        }

        public static string ComputeMIC(byte[] data)
        {
            using (SHA1Managed sha1 = new SHA1Managed())
            {
                byte[] hash = sha1.ComputeHash(data);
                return Convert.ToBase64String(hash);
            }
        }

        public static byte[] ExtractPayload(byte[] data)
        {
            Encoding enc = Encoding.GetEncoding("iso-8859-1");

            string message = enc.GetString(data);

            Match boundaryMatch = Regex.Match(message, "------=([^\\s]+)", RegexOptions.IgnoreCase);
            string boundary = "------=" + boundaryMatch.Groups[1].Value;

            int firstPart = message.IndexOf(boundary) + boundary.Length;
            int lastPart = message.IndexOf(boundary + "--") + (boundary + "--").Length;

            message = message.Substring(firstPart, lastPart - firstPart).TrimStart();
            string payload = message.Substring(0, message.IndexOf(boundary) - 2);

            return enc.GetBytes(payload);
        }

        private static byte[] MDNInner(string messageID, string originalRecipient, string finalRecipient, string MIC)
        {
            byte[] mdnInner = new byte[0];

            string sBoundary = MIMEBoundary();

            byte[] sContentType = ASCIIEncoding.ASCII.GetBytes("Content-Type: multipart/report;\r\n\tboundary=\"" + sBoundary + "\";\r\n\treport-type=disposition-notification\r\n");

            byte[] bBoundary = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + "--" + sBoundary + Environment.NewLine);

            StringBuilder sbContent = new StringBuilder();
            sbContent.Append("Content-Type: text/plain; charset=us-ascii");
            sbContent.Append("\r\n");
            sbContent.Append("\r\n");
            sbContent.Append("This is MDN info for message ACK number: " + messageID);
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

        public static byte[] MDN(byte[] receivedMsgData, string messageID, string originalRecipient, string finalRecipient, string signingCertThumbPrint, out string sBoundary)
        {
            byte[] mdn = new byte[0];

            sBoundary = MIMEBoundary();

            byte[] payload = ExtractPayload(receivedMsgData);
            string MIC = ComputeMIC(payload);

            byte[] bBoundary = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + "--" + sBoundary + Environment.NewLine);
            byte[] bReportMessage = MDNInner(messageID, originalRecipient, finalRecipient, MIC);
            byte[] bSignatureHeader = ASCIIEncoding.ASCII.GetBytes(MIMEHeader("application/pkcs7-signature", "binary", ""));
            byte[] bSignature = AS2Encryption.Encode(bReportMessage, signingCertThumbPrint);
            byte[] bFinalFooter = ASCIIEncoding.ASCII.GetBytes("--" + sBoundary + "--\r\n\r\n");

            mdn = ConcatBytes(bBoundary, bReportMessage, bBoundary, bSignatureHeader, bSignature, bFinalFooter);

            return mdn;
        }
    }
}
