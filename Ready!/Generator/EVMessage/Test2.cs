#region Using directives

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;

#endregion

namespace EnvelopAMessageForOneRecipient
{
    class EnvelopedCmsSingleRecipient
    {
        const String recipientName = "Billev farmacija vzhod d.o.o.";

        static void Main2(string[] args)
        {
            Console.WriteLine("System.Security.Cryptography.Pkcs " +
            "Sample: Single-recipient encrypted and decrypted message");

            //  Original message.
            const String msg = "Here is your personal identification number:";

            Console.WriteLine("\nOriginal message (len {0}): {1}  ",
                              msg.Length, msg);

            //  Convert message to an array of Unicode bytes for signing.
            UnicodeEncoding unicode = new UnicodeEncoding();
            byte[] msgBytes = unicode.GetBytes(msg);

            Console.WriteLine("\n\n------------------------------");
            Console.WriteLine("     SETUP OF CREDENTIALS     ");
            Console.WriteLine("------------------------------\n");

            //  The recipient's certificate is necessary to encrypt
            //  the message for that recipient.
            X509Certificate2 recipientCert = GetRecipientCert();//new X509Certificate2(@"C:\hrvoje_temp\cert\Billev SI exported.p7b");
            //X509Certificate2 recipientCert = new X509Certificate2(@"C:\hrvoje_temp\cert\Billev SI exported.p7b");
            Console.WriteLine("\n\n----------------------");
            Console.WriteLine("     SENDER SIDE      ");
            Console.WriteLine("----------------------\n");

            byte[] encodedEnvelopedCms = EncryptMsg(msgBytes,
                recipientCert);

            

            Console.Write("\nMessage after encryption (len {0}):  ",
                encodedEnvelopedCms.Length);
            foreach (byte b in encodedEnvelopedCms)
            {
                Console.Write("{0:x}", b);
            }
            Console.WriteLine();

            Console.WriteLine("\n\n------------------------");
            Console.WriteLine("     RECIPIENT SIDE     ");
            Console.WriteLine("------------------------\n");

            Byte[] decryptedMsg = DecryptMsg(encodedEnvelopedCms);

            //  Convert Unicode bytes to the original message string.
            Console.WriteLine("\nDecrypted Message: {0}",
                unicode.GetString(decryptedMsg));
        }

        //  Open the AddressBook (also known as Other in 
        //  Internet Explorer) certificate store and search for 
        //  a recipient certificate with which to encrypt the 
        //  message. The certificate must have a subject name 
        //  of "Recipient1".
        static public X509Certificate2 GetRecipientCert()
        {
            //  Open the AddressBook local user X509 certificate store.
            X509Store storeAddressBook = new X509Store(StoreName.Root, StoreLocation.CurrentUser);
            storeAddressBook.Open(OpenFlags.ReadOnly);

            //  Display certificates to help troubleshoot 
            //  the example's setup.
            Console.WriteLine(
                "Found certs with the following subject names in the " +
                "{0} store:", storeAddressBook.Name);
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

        //  Encrypt the message with the public key of
        //  the recipient. This is done by enveloping the message by
        //  using an EnvelopedCms object.
        static public byte[] EncryptMsg(
            Byte[] msg,
            X509Certificate2 recipientCert)
        {
            //  Place the message in a ContentInfo object.
            //  This is required to build an EnvelopedCms object.
            ContentInfo contentInfo = new ContentInfo(msg);

            //  Instantiate an EnvelopedCms object with the ContentInfo
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

            Console.Write(
                "Encrypting data for a single recipient of " +
                "subject name {0} ... ",
                recip1.Certificate.SubjectName.Name);
            //  Encrypt the message for the recipient.
            envelopedCms.Encrypt(recip1);
            Console.WriteLine("Done.");

            //  The encoded EnvelopedCms message contains the message
            //  ciphertext and the information about each recipient 
            //  that the message was enveloped for.
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
            Console.Write("Decrypting Data ... ");

            X509Certificate2 certificate = new X509Certificate2(@"C:\hrvoje_temp\cert\Billev SI w Private Key.pfx", "P0ss1T");


            envelopedCms.Decrypt(new X509Certificate2Collection() { certificate });
            //envelopedCms.Decrypt();
            //envelopedCms.Decrypt(envelopedCms.RecipientInfos[0]);
            Console.WriteLine("Done.");

            //  The decrypted message occupies the ContentInfo property
            //  after the Decrypt method is invoked.
            return envelopedCms.ContentInfo.Content;
        }

        //  Display the ContentInfo property of an EnvelopedCms object.
        static private void DisplayEnvelopedCmsContent(String desc,
            EnvelopedCms envelopedCms)
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
        static private void DisplayEnvelopedCms(EnvelopedCms e,
            Boolean displayContent)
        {
            Console.WriteLine("\nEnveloped CMS/PKCS #7 Message " +
                "Information:");
            Console.WriteLine(
                "\tThe number of recipients for the Enveloped CMS/PKCS " +
                "#7 is: {0}", e.RecipientInfos.Count);
            for (int i = 0; i < e.RecipientInfos.Count; i++)
            {
                Console.WriteLine(
                    "\tRecipient #{0} has type {1}.",
                    i + 1,
                    e.RecipientInfos[i].RecipientIdentifier.Type);
            }
            if (displayContent)
            {
                DisplayEnvelopedCmsContent("Enveloped CMS/PKCS " +
                    "#7 Content", e);
            }
            Console.WriteLine();
        }
    }
}