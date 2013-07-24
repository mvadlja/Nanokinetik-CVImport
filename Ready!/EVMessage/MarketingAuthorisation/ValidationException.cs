using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVMessage.MarketingAuthorisation
{
    public enum SeverityType
    {
        NULL,
        Error,
        Warning
    }

    public enum ValidationExceptionType
    {
        ValueNotInCV,
        InvalidValue,
        InvalidValueFormat,
        InvalidType
    }

    public class ValidationException
    {
        string _message;
        string _location;
        string _propertyName;
        string _propertyValue;

        SeverityType _severity;
        ValidationExceptionType _validationExceptionType;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; }
        }

        public string PropertyName
        {
            get { return _propertyName; }
            set { _propertyName = value; }
        }

        public string PropertyValue
        {
            get { return _propertyValue; }
            set { _propertyValue = value; }
        }

        public SeverityType Severity
        {
            get { return _severity; }
            set { _severity = value; }
        }

        public ValidationExceptionType ValidationExceptionType
        {
            get { return _validationExceptionType; }
            set { _validationExceptionType = value; }
        }

        public ValidationException() { }

        public ValidationException(string message, ValidationExceptionType validationExceptionType, SeverityType severity)
        {
            _message = message;
            _validationExceptionType = validationExceptionType;
            _severity = severity;
        }

        public void AddDescription(string location, string propertyName, string propertyValue)
        {
            _location = location;
            _propertyName = propertyName;
            _propertyValue = propertyValue;
        }

    }
}
