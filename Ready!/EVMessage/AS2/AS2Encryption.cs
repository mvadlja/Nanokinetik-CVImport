using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Pkcs;
using System.IO;
using System.Xml;
using System.Web.UI;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace EVMessage.AS2
{
    public static class EncryptionAlgorithm
    {
        public static string DES3 = "3DES";
        public static string RC2 = "RC2";
    }

    public class AS2Encryption
    {
        public static byte[] Encode(byte[] arMessage, string signingCertThumbPrint)
        {
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadOnly);
            X509Certificate2 cert = store.Certificates.Find(X509FindType.FindByThumbprint, (object)signingCertThumbPrint, true)[0];
            store.Close();

            ContentInfo contentInfo = new ContentInfo(arMessage);

            SignedCms signedCms = new SignedCms(contentInfo, true); // <- true detaches the signature
            CmsSigner cmsSigner = new CmsSigner(cert);

            signedCms.ComputeSignature(cmsSigner);
            byte[] signature = signedCms.Encode();

            return signature;
        }

        public static byte[] Encode(byte[] arMessage, string signerCert, string signerPassword)
        {
            X509Certificate2 cert = new X509Certificate2(signerCert, signerPassword);
            ContentInfo contentInfo = new ContentInfo(arMessage);
 
            SignedCms signedCms = new SignedCms(contentInfo, true); // <- true detaches the signature
            CmsSigner cmsSigner = new CmsSigner(cert);
 
            signedCms.ComputeSignature(cmsSigner);
            byte[] signature = signedCms.Encode();
	 
            return signature;
        }
	 
        internal static byte[] Encrypt(byte[] message, string certThumbPrint, string encryptionAlgorithm)
        {
            if (!string.Equals(encryptionAlgorithm, EncryptionAlgorithm.DES3) && !string.Equals(encryptionAlgorithm, EncryptionAlgorithm.RC2))
	                throw new ArgumentException("encryptionAlgorithm argument must be 3DES or RC2 - value specified was:" + encryptionAlgorithm);
 
            X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2 cert = store.Certificates.Find(X509FindType.FindByThumbprint, (object)certThumbPrint, true)[0];
            store.Close();

            ContentInfo contentInfo = new ContentInfo(message);
 
            EnvelopedCms envelopedCms = new EnvelopedCms(contentInfo,
                new AlgorithmIdentifier(new System.Security.Cryptography.Oid(encryptionAlgorithm))); 
 
            CmsRecipient recipient = new CmsRecipient(SubjectIdentifierType.IssuerAndSerialNumber, cert);
	 
            envelopedCms.Encrypt(recipient);
 
            byte[] encoded = envelopedCms.Encode();
	 
            return encoded;
        }
	 
        internal static byte[] Decrypt(byte[] encodedEncryptedMessage, out string encryptionAlgorithmName)
        {
            EnvelopedCms envelopedCms = new EnvelopedCms();
            // NB. the message will have been encrypted with your public key.
            // The corresponding private key must be installed in the Personal Certificates folder of the user
            // this process is running as.
            envelopedCms.Decode(encodedEncryptedMessage);
	 
            envelopedCms.Decrypt();
            encryptionAlgorithmName = envelopedCms.ContentEncryptionAlgorithm.Oid.FriendlyName;
 
            return envelopedCms.Encode();
        }

        internal static byte[] Decrypt(byte[] encodedEncryptedMessage)
        {
            EnvelopedCms envelopedCms = new EnvelopedCms();
            envelopedCms.Decode(encodedEncryptedMessage);

            //X509Store store = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            //store.Open(OpenFlags.ReadOnly);

            X509Certificate2 certificate = new X509Certificate2(@"C:\hrvoje_temp\cert\Billev SI w Private Key.pfx", "P0ss1T");


            envelopedCms.Decrypt(new X509Certificate2Collection() {certificate} );
            return envelopedCms.Encode();
        }

        
    }
}
