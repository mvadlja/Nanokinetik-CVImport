using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVMessage.XmlValidator
{
    public enum XmlValidatorExceptionType
    {
        Error,
        Warning
    }

    public class XmlValidatorException
    {
        int _lineNumber;
        int _linePosition;
        string _message;
        XmlValidatorExceptionType _xmlValidatorExceptionType;

        public XmlValidatorExceptionType XmlValidatorExceptionType
        {
            get { return _xmlValidatorExceptionType; }
            set { _xmlValidatorExceptionType = value; }
        }

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public int LinePosition
        {
            get { return _linePosition; }
            set { _linePosition = value; }
        }

        public int LineNumber
        {
            get { return _lineNumber; }
            set { _lineNumber = value; }
        }

        public XmlValidatorException(int lineNumber, int linePosition, string message, XmlValidatorExceptionType xmlValidatorExceptionType)
        {
            LineNumber = lineNumber;
            LinePosition = linePosition;
            Message = message;
            XmlValidatorExceptionType = xmlValidatorExceptionType;
        }

        public XmlValidatorException(System.Xml.Schema.XmlSchemaException ex, XmlValidatorExceptionType xmlValidatorExceptionType)
        {
            LineNumber = ex.LineNumber;
            LinePosition = ex.LinePosition;
            Message = ex.Message;
            XmlValidatorExceptionType = xmlValidatorExceptionType;
        }

        public XmlValidatorException(System.Xml.XmlException ex, XmlValidatorExceptionType xmlValidatorExceptionType)
        {
            LineNumber = ex.LineNumber;
            LinePosition = ex.LinePosition;
            Message = ex.Message;
            XmlValidatorExceptionType = xmlValidatorExceptionType;
        }
    }
}
