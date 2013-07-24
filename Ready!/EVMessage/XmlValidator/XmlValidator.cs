using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using System.Xml;
using System.IO;

namespace EVMessage.XmlValidator
{
    public class XmlValidator
    {
        List<XmlValidatorException> _exceptions;

        public bool IsValid
        {
            get { return Errors.Count == 0; }
        }

        public List<XmlValidatorException> Errors
        {
            get { return _exceptions.Where(item => item.XmlValidatorExceptionType == XmlValidatorExceptionType.Error).ToList(); }
        }

        public List<XmlValidatorException> Warnings
        {
            get { return _exceptions.Where(item => item.XmlValidatorExceptionType == XmlValidatorExceptionType.Warning).ToList(); }
        }

        public List<XmlValidatorException> Exceptions
        {
            get { return _exceptions; }
        }

        public XmlValidator()
        {
            _exceptions = new List<XmlValidatorException>();
        }

        public bool Validate(string xml, string xsd = null, Encoding encoding = null)
        {
            _exceptions.Clear();

            if (string.IsNullOrWhiteSpace(xml))
            {
                _exceptions.Add(new XmlValidatorException(0, 0, "Xml is empty.", XmlValidatorExceptionType.Error));

                return false;
            }

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            using (MemoryStream xmlMS = new MemoryStream(encoding.GetBytes(xml)))
            {
                if (xsd != null)
                {
                    using (MemoryStream xsdMS = new MemoryStream(encoding.GetBytes(xsd)))
                    {
                        return Validate(xmlMS, xsdMS);
                    }
                }
                else
                {
                    return Validate(xmlMS, null);
                }
            }
        }

        public bool Validate(byte[] xml, byte[] xsd = null)
        {
            _exceptions.Clear();

            if (xml == null || xml.Length == 0)
            {
                _exceptions.Add(new XmlValidatorException(0, 0, "Xml is empty.", XmlValidatorExceptionType.Error));
                return false;
            }

            using (MemoryStream xmlMS = new MemoryStream(xml))
            {
                if (xsd != null)
                {
                    using (MemoryStream xsdMS = new MemoryStream(xsd))
                    {
                        return Validate(xmlMS, xsdMS);
                    }
                }
                else
                {
                    return Validate(xmlMS, null);
                }
            }
        }

        public bool Validate(MemoryStream xmlMS, MemoryStream xsdMS = null)
        {
            _exceptions.Clear();

            if (xmlMS == null || xmlMS.Length == 0)
            {
                _exceptions.Add(new XmlValidatorException(0, 0, "Xml is empty.", XmlValidatorExceptionType.Error));
                return false;
            }

            XmlSchema maSchema = null;
            XmlReaderSettings settings = null;
            
            if (xsdMS != null)
            {
                xsdMS.Seek(0, SeekOrigin.Begin);

                maSchema = XmlSchema.Read(xsdMS, ValidationCallBack);

                settings = new XmlReaderSettings();
                settings.Schemas.Add(maSchema);
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
            }
            else
            {
                settings = new XmlReaderSettings();
                settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);
                settings.ValidationType = ValidationType.None;
            }

            xmlMS.Seek(0, SeekOrigin.Begin);

            using (XmlReader xmlReader = XmlReader.Create(xmlMS, settings))
            {
                try
                {
                    while (xmlReader.Read())
                    {

                    }
                }
                catch (XmlException ex)
                {
                    var exception = new XmlValidatorException(ex, XmlValidatorExceptionType.Error);
                    _exceptions.Add(exception);

                    return false;
                }
            }

            if (Errors.Count > 0)
            {
                return false;
            }

            return true;
        }

        private void ValidationCallBack(object sender, ValidationEventArgs args)
        {
            if (args.Severity == System.Xml.Schema.XmlSeverityType.Warning)
            {
                if (args.Message.StartsWith("Could not find schema information"))
                {
                    if (!Errors.Any(error => error.Message.StartsWith("Could not find schema information")))
                    {
                        var exception = new XmlValidatorException(args.Exception, XmlValidatorExceptionType.Error);
                        exception.Message = "Could not find schema information. Xml namespace is missing or it's invalid.";
                        exception.LinePosition += (sender as System.Xml.XmlReader).Name.Length;
                        _exceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XmlValidatorException(args.Exception, XmlValidatorExceptionType.Warning);
                    _exceptions.Add(exception);
                }
            }
            else if (args.Severity == System.Xml.Schema.XmlSeverityType.Error)
            {
                var exception = new XmlValidatorException(args.Exception, XmlValidatorExceptionType.Error);
                _exceptions.Add(exception);
            }
        }

        public List<string> GetValidationErrors()
        {
            List<string> errors = new List<string>(Errors.Count);

            foreach (XmlValidatorException ex in Errors)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Error!");
                sb.Append(" (Line: " + ex.LineNumber);
                sb.Append(" Position: " + ex.LinePosition + ")");
                sb.Append(" " + ex.Message);

                errors.Add(sb.ToString());
            }

            return errors;
        }

        public List<string> GetValidationWarnings()
        {
            List<string> warnings = new List<string>(Warnings.Count);

            foreach (XmlValidatorException ex in Warnings)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("Warning!");
                sb.Append(" (Line: " + ex.LineNumber);
                sb.Append(" Position: " + ex.LinePosition + ")");
                sb.Append(" " + ex.Message);

                warnings.Add(sb.ToString());
            }

            return warnings;
        }

        public List<string> GetValidationExceptions()
        {
            List<string> exceptions = new List<string>(Exceptions.Count);

            foreach (XmlValidatorException ex in Exceptions)
            {
                StringBuilder sb = new StringBuilder();
                
                if (ex.XmlValidatorExceptionType == XmlValidatorExceptionType.Error)
                {
                    sb.Append("Error!");
                }
                else
                {
                    sb.Append("Warning!");
                }
                
                sb.Append(" (Line: " + ex.LineNumber);
                sb.Append(" Position: " + ex.LinePosition + ")");
                sb.Append(" " + ex.Message);

                exceptions.Add(sb.ToString());
            }

            return exceptions;
        }
    }
}
