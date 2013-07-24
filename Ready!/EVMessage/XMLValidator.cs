using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Text;

namespace xEVMPD
{
    public class XMLValidator
    {
        // Validation Error Count
        static int ErrorsCount = 0;

        // Validation Error Message
        static string ErrorMessage = "";

        public static void ValidationHandler(object sender, ValidationEventArgs args)
        {
            ErrorMessage = ErrorMessage + args.Message + "\r\n";
            ErrorsCount++;
        }

        public static void Validate(MemoryStream XMLDoc, string xsdPath)
        {
            try
            {
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.Schemas.Add(null, xsdPath);
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(ValidationHandler);


                XMLDoc.Seek(0, SeekOrigin.Begin);
                //XmlDocument doc = new XmlDocument();
                //doc.Load(XMLDoc);
                
                
                //StringReader r = new StringReader(strXMLDoc);

                using (XmlReader validatingReader = XmlReader.Create(XMLDoc, settings))
                //using (XmlReader validatingReader = XmlReader.Create(r, settings))
                {
                    while (validatingReader.Read()) { /* just loop through document */ }
                }               

                // Raise exception, if XML validation fails
                if (ErrorsCount > 0) 
                    throw new Exception(ErrorMessage);                

                // XML Validation succeeded
                Console.WriteLine("XML validation succeeded.\r\n");
            }
            catch (Exception error)
            {
                // XML Validation failed
                Console.WriteLine("XML validation failed." + "\r\n" +
                "Error Message: " + error.Message);
            }
        }
    }
}