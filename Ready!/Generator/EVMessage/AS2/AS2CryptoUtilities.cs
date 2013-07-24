using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace EVMessage.AS2
{
    public class AS2CryptoUtilities
    {
        static public byte[] SignMsg(Byte[] msg, X509Certificate2 signerCert)
        {
            ContentInfo contentInfo = new ContentInfo(msg);

            SignedCms signedCms = new SignedCms(contentInfo);
            CmsSigner cmsSigner = new CmsSigner(signerCert);

            //  Sign the PKCS #7 message.
            signedCms.ComputeSignature(cmsSigner);

            //  Encode the PKCS #7 message.
            return signedCms.Encode();
        }

        static public byte[] SignMsg(Byte[] msg, string signerCertPath, string signerPassword)
        {
            X509Certificate2 signerCert = new X509Certificate2(signerCertPath, signerPassword);
            ContentInfo contentInfo = new ContentInfo(msg);

            SignedCms signedCms = new SignedCms(contentInfo);
            CmsSigner cmsSigner = new CmsSigner(signerCert);


            //  Sign the PKCS #7 message.
            signedCms.ComputeSignature(cmsSigner);

            //  Encode the PKCS #7 message.
            return signedCms.Encode();
        }

        static public bool VerifySignature(byte[] encodedSignedCms)
        {
            //  Prepare an object in which to decode and verify.
            SignedCms signedCms = new SignedCms();

            signedCms.Decode(encodedSignedCms);

            //  Catch a verification exception if you want to
            //  advise the message recipient that 
            //  security actions might be appropriate.
            try
            {
                //  Verify signature. Do not validate signer
                //  certificate for the purposes of this example.
                //  Note that in a production environment, validating
                //  the signer certificate chain will probably
                //  be necessary.
                signedCms.CheckSignature(true);
            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                return false;
            }

            return true;
        }

        static public byte[] EncryptMsg(Byte[] msg, X509Certificate2 recipientCert, string encryptionAlgorithm)
        {
            if (!string.Equals(encryptionAlgorithm, EncryptionAlgorithm.DES3) && !string.Equals(encryptionAlgorithm, EncryptionAlgorithm.RC2))
                throw new ArgumentException("EncryptionAlgorithm argument must be 3DES or RC2 - value specified was:" + encryptionAlgorithm);

            ContentInfo contentInfo = new ContentInfo(msg);

            //  Instantiate EnvelopedCms object with the ContentInfo
            //  above.
            //  Has default SubjectIdentifierType IssuerAndSerialNumber.
            //  Has default ContentEncryptionAlgorithm property value
            //  RSA_DES_EDE3_CBC.
            //EnvelopedCms envelopedCms = new EnvelopedCms(contentInfo);
            EnvelopedCms envelopedCms = new EnvelopedCms(contentInfo,
              new AlgorithmIdentifier(new System.Security.Cryptography.Oid(encryptionAlgorithm))); // should be 3DES or RC2

            //  Formulate a CmsRecipient object that
            //  represents information about the recipient
            //  to encrypt the message for.
            CmsRecipient recip1 = new CmsRecipient(SubjectIdentifierType.IssuerAndSerialNumber, recipientCert);

            //  Encrypt the message for the recipient.
            envelopedCms.Encrypt(recip1);

            //  The encoded EnvelopedCms message contains the encrypted
            //  message and the information about each recipient that
            //  the message was enveloped for.
            return envelopedCms.Encode();
        }

        //  Decrypt the encoded EnvelopedCms message.
        static public Byte[] DecryptMsg(byte[] encodedEnvelopedCms, string encryptionAlgorithm)
        {
            if (!string.Equals(encryptionAlgorithm, EncryptionAlgorithm.DES3) && !string.Equals(encryptionAlgorithm, EncryptionAlgorithm.RC2))
                throw new ArgumentException("EncryptionAlgorithm argument must be 3DES or RC2 - value specified was:" + encryptionAlgorithm);

            //  Prepare object in which to decode and decrypt.
            //EnvelopedCms envelopedCms = new EnvelopedCms();
            ContentInfo contentInfo = new ContentInfo(encodedEnvelopedCms);
            EnvelopedCms envelopedCms = new EnvelopedCms(contentInfo,
               new AlgorithmIdentifier(new System.Security.Cryptography.Oid(encryptionAlgorithm))); // should be 3DES or RC2

            //  Decode the message.
            envelopedCms.Decode(encodedEnvelopedCms);

            //  Decrypt the message for the single recipient.
            //  Note that the following call to the Decrypt method
            //  accomplishes the same result:
            //  envelopedCms.Decrypt();
            envelopedCms.Decrypt(envelopedCms.RecipientInfos[0]);

            return envelopedCms.Encode();
        }

        static public bool VerifyMsg(byte[] encodedSignedCms, out byte[] origMsg)
        {
            //  Prepare a SignedCms object in which to decode
            //  and verify.
            SignedCms signedCms = new SignedCms();

            signedCms.Decode(encodedSignedCms);

            //  Catch a verification exception in the event you want to
            //  advise the message recipient that security actions
            //  might be appropriate.
            try
            {
                //  Verify signature. Do not validate signer
                //  certificate for the purposes of this example.
                //  Note that in a production environment, validating
                //  the signer certificate chain will probably be
                //  necessary.
                signedCms.CheckSignature(true);
            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                origMsg = null;
                return false;
            }

            origMsg = signedCms.ContentInfo.Content;

            return true;
        }
	 
    }
}
