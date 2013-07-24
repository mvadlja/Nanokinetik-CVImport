using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.IO;
using Ionic.Zip;

namespace EVMessage.AS2
{
    class EnvelopedSignedCms
    {
        const String signerName = "MessageSigner1";
        const String recipientName = "Recipient1";

        static void Main1(string[] args)
        {
            byte[] origMsg;
            Encoding unicode = Encoding.Unicode;

            byte[] msgBytes = File.ReadAllBytes(@"C:\zips\12_03_19_10_22_48.zip");

            //using (FileStream fs = new FileStream(@"C:\zips\12_03_16_12_33_55.zip", FileMode.Open))
            //{
            //    using (BinaryReader br = new BinaryReader(fs))
            //    {
            //        msgBytes = br.ReadBytes((int)fs.Length);  //unicode.GetBytes(msgString); // br.ReadBytes((int)fs.Length); // File.ReadAllBytes(@"C:\zips\xevprm2.xml"); // unicode.GetBytes(msg);

            //        br.Close();
            //        fs.Close();
            //    }
            //}
            

            Console.WriteLine("System.Security.Cryptography.Pkcs Sample: Encrypted, signed, decrypted, and verified message");
            //  Original message.
            const String msg = "Here are the sales figures for the upcoming quarterly report to Wall Street.";

            Console.WriteLine("\nOriginal message (len {0}): {1}  ", msg.Length, unicode.GetString(msgBytes));

            //  Convert message to array of bytes for signing.


            Console.WriteLine("\n\n------------------------------");
            Console.WriteLine("     SETUP OF CREDENTIALS     ");
            Console.WriteLine("------------------------------\n");

            //  The signer's private key, obtained by association with
            //  their signing certificate, is necessary to sign the 
            //  message.
            X509Certificate2 signerCert = new X509Certificate2(@"C:\hrvoje_temp\cert\Billev SI w Private Key.pfx", "P0ss1T"); // GetSignerCert();

            //  The recipient's certificate is necessary to encrypt
            //  the message for that recipient.
            X509Certificate2 recipientCert = new X509Certificate2(@"C:\hrvoje_temp\cert\Billev SI w Private Key.pfx", "P0ss1T"); // GetRecipientCert();

            Console.WriteLine("\n\n----------------------");
            Console.WriteLine("     SENDER SIDE      ");
            Console.WriteLine("----------------------\n");

            byte[] encodedSignedCms = AS2CryptoUtilities.SignMsg(msgBytes, signerCert);
            File.WriteAllBytes(@"C:\zips\SIGNEDBytesZip.zip", encodedSignedCms);

            //  Encrypt the encoded SignedCms message.
            byte[] encodedEnvelopedCms = AS2CryptoUtilities.EncryptMsg(encodedSignedCms, recipientCert, EncryptionAlgorithm.DES3);
            File.WriteAllBytes(@"C:\zips\ENCBytesZip.dat", encodedEnvelopedCms);

            Console.Write("\nMessage after encryption (len {0}):  ", encodedEnvelopedCms.Length);
            foreach (byte b in encodedEnvelopedCms)
            {
                Console.Write("{0:x}", b);
            }
            Console.WriteLine();


            Console.WriteLine("\n\n------------------------");
            Console.WriteLine("     RECIPIENT SIDE     ");
            Console.WriteLine("------------------------\n");


            encodedEnvelopedCms = File.ReadAllBytes(@"C:\zips\ENCBytesZip.dat");
            encodedSignedCms = AS2CryptoUtilities.DecryptMsg(encodedEnvelopedCms, EncryptionAlgorithm.DES3);

            Console.WriteLine("\nDecrypted Authenticated Message: {0}", unicode.GetString(encodedEnvelopedCms));

            //  Get the original message back after verification so
            //  it can be displayed.
            if (AS2CryptoUtilities.VerifyMsg(encodedSignedCms, out origMsg))
            {
                Console.WriteLine("\nMessage verified");
            }
            else
            {
                Console.WriteLine("\nMessage failed to verify");
            }

            //  Convert Unicode bytes to the original message string.
            Console.WriteLine("\nDecrypted Authenticated Message: {0}", unicode.GetString(origMsg));

            bool f = true;
            int poz = 0;
            int sc1Length = msgBytes.Length;
            int sc2Length = origMsg.Length;

            for (int i = 0; i < msgBytes.Length; i++)
                if (msgBytes[i] != origMsg[i])
                {
                    f = false;
                    poz = i;
                    break;
                }


            if (f)
            {

            }

            File.WriteAllBytes(@"C:\zips\msgBytesZip.zip", msgBytes);
            File.WriteAllBytes(@"C:\zips\origMsgZip.zip", origMsg);

            //using (ZipFile zip = new ZipFile())
            //{
            //    zip.AddEntry(@"C:\zips\testZip", origMsg);
            //    zip.Save(@"C:\zips\testZip3.zip");
            //}
        }


        //  Open the My (or Personal) certificate store. Search for
        //  credentials with which to sign the message. The certificate
        //  must have a the subject name "MessageSigner1".
        static public X509Certificate2 GetSignerCert()
        {
            //  Open the My certificate store.
            X509Store storeMy = new X509Store(StoreName.My,
                StoreLocation.CurrentUser);
            storeMy.Open(OpenFlags.ReadOnly);

            //  Display certificates to help troubleshoot the
            //  example's setup.
            Console.WriteLine("Found certs with the following subject " +
                "names in the {0} store:", storeMy.Name);
            foreach (X509Certificate2 cert in storeMy.Certificates)
            {
                Console.WriteLine("\t{0}", cert.SubjectName.Name);
            }

            //  Find the signer's certificate.
            X509Certificate2Collection certColl =
                storeMy.Certificates.Find(X509FindType.FindBySubjectName,
                signerName, false);
            Console.WriteLine(
                "Found {0} certificates in the {1} store with name {2}",
                certColl.Count, storeMy.Name, signerName);

            //  Check to see if the certificate suggested by the example
            //  requirements is not present.
            if (certColl.Count == 0)
            {
                Console.WriteLine(
                    "A suggested certificate to use for this example " +
                    "is not in the certificate store. Select " +
                    "an alternate certificate to use for " +
                    "signing the message.");
            }

            storeMy.Close();

            return certColl[0];
        }

        //  Open the AddressBook (called Other in Internet Explorer) 
        //  certificate store and search for a recipient
        //  certificate with which to encrypt the message. The certificate
        //  must have the subject name "Recipient1".
        static public X509Certificate2 GetRecipientCert()
        {
            //  Open the AddressBook local user X509 certificate store.
            X509Store storeAddressBook = new X509Store(StoreName.
                AddressBook, StoreLocation.CurrentUser);
            storeAddressBook.Open(OpenFlags.ReadOnly);

            //  Display certificates to help troubleshoot the 
            //  example's setup.
            Console.WriteLine(
                "Found certs with the following subject names in the " +
                "{0} store:",
                storeAddressBook.Name);
            foreach (X509Certificate2 cert in storeAddressBook.Certificates)
            {
                Console.WriteLine("\t{0}", cert.SubjectName.Name);
            }

            //  Get recipient certificate.
            //  For purposes of this sample, do not validate the
            //  certificate. Note that in a production environment,
            //  validating the certificate will probably be necessary.
            X509Certificate2Collection certColl = storeAddressBook.
                Certificates.Find(X509FindType.FindBySubjectName,
                recipientName, false);
            Console.WriteLine(
                "Found {0} certificates in the {1} store with name {2}",
                certColl.Count, storeAddressBook.Name, recipientName);

            //  Check to see if the certificate suggested by the example
            //  requirements is not present.
            if (certColl.Count == 0)
            {
                Console.WriteLine(
                    "A suggested certificate to use for this example " +
                    "is not in the certificate store. Select " +
                    "an alternate certificate to use for " +
                    "signing the message.");
            }

            storeAddressBook.Close();

            return certColl[0];
        }

        //  Sign the message by the using the private key of the signer.
        //  Note that signer's public key certificate is input here 
        //  because it is used to locate the corresponding private key.
        static public byte[] SignMsg(Byte[] msg, X509Certificate2 signerCert)
        {
            //  Place message in a ContentInfo object.
            //  This is required to build a SignedCms object.
            ContentInfo contentInfo = new ContentInfo(msg);

            //  Instantiate SignedCms object with the ContentInfo above.
            //  Has default SubjectIdentifierType IssuerAndSerialNumber.
            //  Has default Detached property value false, so message is
            //  included in the encoded SignedCms.
            SignedCms signedCms = new SignedCms(contentInfo);

            //  Formulate a CmsSigner object, which has all the needed
            //  characteristics of the signer.
            CmsSigner cmsSigner = new CmsSigner(signerCert);

            //  Sign the PKCS #7 message.
            Console.Write("Computing signature with signer subject " + "name {0} ... ", signerCert.SubjectName.Name);
            signedCms.ComputeSignature(cmsSigner);
            Console.WriteLine("Done.");

            //  Encode the PKCS #7 message.
            return signedCms.Encode();
        }

        //  Verify the encoded SignedCms message and return a Boolean
        //  value that specifies whether the verification was successful.
        //  Also return the original message that was signed, which is
        //  available as part of the SignedCms message after it
        //  is decoded.
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
                Console.Write("Checking signature on message ... ");
                signedCms.CheckSignature(true);
                Console.WriteLine("Done.");
            }
            catch (System.Security.Cryptography.CryptographicException e)
            {
                Console.WriteLine("VerifyMsg caught exception:  {0}",
                    e.Message);
                Console.WriteLine("The message may have been modified " +
                    "in transit or storage. Authenticity of the " +
                    "message is not guaranteed.");
                origMsg = null;
                return false;
            }

            origMsg = signedCms.ContentInfo.Content;

            return true;
        }

        //  Encrypt the message with the public key of
        //  the recipient. This is done by enveloping the message by
        //  using a EnvelopedCms object.
        static public byte[] EncryptMsg(Byte[] msg, X509Certificate2 recipientCert)
        {
            //  Place message in a ContentInfo object.
            //  This is required to build an EnvelopedCms object.
            ContentInfo contentInfo = new ContentInfo(msg);

            //  Instantiate EnvelopedCms object with the ContentInfo
            //  above.
            //  Has default SubjectIdentifierType IssuerAndSerialNumber.
            //  Has default ContentEncryptionAlgorithm property value
            //  RSA_DES_EDE3_CBC.
            EnvelopedCms envelopedCms = new EnvelopedCms(contentInfo);

            //  Formulate a CmsRecipient object that
            //  represents information about the recipient
            //  to encrypt the message for.
            CmsRecipient recip1 = new CmsRecipient(
                SubjectIdentifierType.IssuerAndSerialNumber,
                recipientCert);

            Console.Write("Encrypting data for a single recipient of subject name {0} ... ", recip1.Certificate.SubjectName.Name);

            //  Encrypt the message for the recipient.
            envelopedCms.Encrypt(recip1);
            Console.WriteLine("Done.");

            //  The encoded EnvelopedCms message contains the encrypted
            //  message and the information about each recipient that
            //  the message was enveloped for.
            return envelopedCms.Encode();
        }

        //  Decrypt the encoded EnvelopedCms message.
        static public Byte[] DecryptMsg(byte[] encodedEnvelopedCms)
        {
            //  Prepare object in which to decode and decrypt.
            EnvelopedCms envelopedCms = new EnvelopedCms();

            //  Decode the message.
            envelopedCms.Decode(encodedEnvelopedCms);

            //  Display the number of recipients the message is
            //  enveloped for; it should be 1 for this example.
            DisplayEnvelopedCms(envelopedCms, false);

            //  Decrypt the message for the single recipient.
            //  Note that the following call to the Decrypt method
            //  accomplishes the same result:
            //  envelopedCms.Decrypt();
            Console.Write("Decrypting Data ... ");
            envelopedCms.Decrypt(envelopedCms.RecipientInfos[0]);
            Console.WriteLine("Done.");

            return envelopedCms.Encode();
        }

        //  Display the ContentInfo property of a SignedCms object.
        private void DisplaySignedCmsContent(String desc, SignedCms signedCms)
        {
            Console.WriteLine(desc + " (length {0}):  ",
                signedCms.ContentInfo.Content.Length);
            foreach (byte b in signedCms.ContentInfo.Content)
            {
                Console.Write(b.ToString() + " ");
            }
            Console.WriteLine();
        }

        //  Display the ContentInfo property of an EnvelopedCms object.
        static private void DisplayEnvelopedCmsContent(String desc, EnvelopedCms envelopedCms)
        {
            Console.WriteLine(desc + " (length {0}):  ",
                envelopedCms.ContentInfo.Content.Length);
            foreach (byte b in envelopedCms.ContentInfo.Content)
            {
                Console.Write(b.ToString() + " ");
            }
            Console.WriteLine();
        }

        //  Display some properties of an EnvelopedCms object.
        static private void DisplayEnvelopedCms(EnvelopedCms e, Boolean displayContent)
        {
            Console.WriteLine("\nEnveloped PKCS #7 Message Information:");
            Console.WriteLine(
                "\tThe number of recipients for the Enveloped PKCS #7 " +
                "is:  {0}",
                e.RecipientInfos.Count);
            for (int i = 0; i < e.RecipientInfos.Count; i++)
            {
                Console.WriteLine(
                    "\tRecipient #{0} has type {1}.",
                    i + 1,
                    e.RecipientInfos[i].RecipientIdentifier.Type);
            }
            if (displayContent)
            {
                DisplayEnvelopedCmsContent("Enveloped PKCS #7 Content", e);
            }
            Console.WriteLine();
        }
    }
}
