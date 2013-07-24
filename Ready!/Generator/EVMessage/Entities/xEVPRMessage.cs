using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using Ready.Model;
using System.Data;
using Ionic.Zip;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;

namespace xEVMPD
{
    /// <summary>
    /// xEVPRM - eXtended EudraVIgilance Product Report Message
    /// </summary>
    public partial class xEVPRMessage : EVMessageBase
    {
        public evprm Message
        {
            get { return (evprm)message; }
            set { message = value; }
        }

        protected override Type MyType
        {
            get { return typeof(evprm); }
        }
        public XevprmOperationType OperationType { get; set; }

        public xEVPRMessage()
        {
            // create xsd-based message and set up default values
            Message = new evprm();
            //OperationType = XevprmOperationType.Insert;
        }

        #region validation



        public void Validate(string xsdPath)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.Schemas.Add(null, xsdPath);
            settings.ValidationType = ValidationType.Schema;
            settings.ValidationEventHandler += new System.Xml.Schema.ValidationEventHandler(ValidationHandler);

            MemoryStream XMLDoc = SerializeToXMLMemoryStream();
            XMLDoc.Seek(0, SeekOrigin.Begin);
            using (XmlReader validatingReader = XmlReader.Create(XMLDoc, settings))
            {
                while (validatingReader.Read()) { }
            }
        }

        public MemoryStream SerializeToXMLMemoryStream()
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(typeof(evprm));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);

                xs.Serialize(xmlTextWriter, Message);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                return memoryStream;
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e);
                return null;
            }
        }

        int ErrorsCount = 0;

        // Validation Error Message
        string ErrorMessage = "";

        public void ValidationHandler(object sender, ValidationEventArgs args)
        {
            ErrorMessage = ErrorMessage + args.Message + "\r\n";
            ErrorsCount++;
        }
        #endregion

    }

    public enum xEVPRM_OrganizationType
    {
        MAH = 1,
        Sponsor
    }

    public enum xEVPRM_AttachmentType
    {
        PPI = 1,
        PSI
    }


}
