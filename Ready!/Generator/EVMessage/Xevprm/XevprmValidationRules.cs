using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EVMessage.Xevprm
{
    public interface IXevprmValidationRule
    {
        string Id { get; }
        string BusinessRule { get; }
        string ReadyMessage { get; }
        string EvprmMessage { get; }
    }

    public static class XevprmValidationRules
    {
        public enum DataTypeEror
        {
            ValueIsMissing,
            InvalidValue,
            InvalidValueFormat,
            ValueLenghtToBig,
            ValueLenghtToSmall
        }

        public static class H
        {
            public static class messagesenderidentifier
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "H.messagesenderidentifier.DataType";
                    private const string _businessRule = "H.5.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueLenghtToBig:
                            case DataTypeEror.ValueLenghtToSmall:
                                _readyMessage = "Organisation sender ID can't be shorter than 3 or longer than 60 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "H.messagesenderidentifier.BR1";
                    private const string _businessRule = "H.5.BR.1";
                    private const string _readyMessage = "Organisation sender ID is missing.";
                    private const string _evprmMessage = "";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }
        }

        public static class AP
        {
            public enum RegistrationProcedure
            {
                Centralised,
                Decentralised,
                Mutual
            }

            public class BRCustom1 : IXevprmValidationRule
            {
                private const string _id = "AP.BRCustom1";
                private const string _businessRule = "";
                private const string _readyMessage = "Related product can't be empty.";
                private string _evprmMessage;

                public static string RuleId { get { return _id; } }

                public string Id { get { return _id; } }
                public string BusinessRule { get { return _businessRule; } }
                public string ReadyMessage { get { return _readyMessage; } }
                public string EvprmMessage { get { return _evprmMessage; } }
            }

            public static class localnumber
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "AP.localnumber.DataType";
                    private const string _businessRule = "AP.1.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "Authorised product local number can't be longer than 60 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "AP.localnumber.BR1";
                    private const string _businessRule = "AP.1.BR.1";
                    private const string _readyMessage = "If the operation type is 'insert' then authorised product local number must be present.";
                    private const string _evprmMessage = "If the operationtype is 'insert' then localnumber must be present.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class ev_code
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "AP.ev_code.DataType";
                    private const string _businessRule = "AP.2.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "Authorised product EV Code can't be longer than 60 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "AP.ev_code.BR1";
                    private const string _businessRule = "AP.2.BR.1";
                    private const string _readyMessage = "If the operation type is NOT 'insert' then the EV Code must be present.";
                    private const string _evprmMessage = "If the operationtype is NOT 'insert' then the ev_code must be present.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class BR2 : IXevprmValidationRule
                {
                    private const string _id = "AP.ev_code.BR2";
                    private const string _businessRule = "AP.2.BR.2";
                    private const string _readyMessage = "If the operation type is 'insert' then the EV Code must be empty.";
                    private const string _evprmMessage = "If the operationtype is 'insert' then the ev_code must be empty.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class mahcode
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "AP.mahcode.DataType";
                    private const string _businessRule = "AP.4.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "Licence holder EV Code can't be longer than 60 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "AP.mahcode.Cardinality";
                    private const string _businessRule = "AP.4.Cardinality";
                    private const string _readyMessage = "Licence holder must be specified.";
                    private const string _evprmMessage = "The mahcode must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class BR2 : IXevprmValidationRule
                {
                    private const string _id = "AP.mahcode.BR2";
                    private const string _businessRule = "AP.4.BR.2";
                    private const string _readyMessage = "Licence holder EV Code must be specified.";
                    private const string _evprmMessage = "Value must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class qppvcode
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "AP.qppvcode.DataType";
                    private const string _businessRule = "AP.5.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.InvalidValueFormat:
                                _readyMessage = "QPPV Code must be positive integer with max 10 digits.";
                                _evprmMessage = "qppvcode must be positive integer with max 10 digits.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "AP.qppvcode.BR1";
                    private const string _businessRule = "AP.5.BR.1";
                    private const string _readyMessage = "If the operation type is 'insert', 'update' or 'variation', then QPPV Code must be specified.";
                    private const string _evprmMessage = "If the operationtype is 'insert', 'update' or 'variation', then qppvcode must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class CustomBR1 : IXevprmValidationRule
                {
                    private const string _id = "AP.qppvcode.CustomBR1";
                    private const string _businessRule = "AP.5.BR.1";
                    private const string _readyMessage = "Person is missing QPPV Code.";
                    private const string _evprmMessage = "If the operationtype is 'insert', 'update' or 'variation', then qppvcode must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class mflcode
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "AP.mflcode.DataType";
                    private const string _businessRule = "AP.6.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "Master file location EV Code can't be longer than 60 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class BR3 : IXevprmValidationRule
                {
                    private const string _id = "AP.mflcode.BR3";
                    private const string _businessRule = "AP.6.BR.3";
                    private const string _readyMessage = "Master file location EV Code must be specified.";
                    private const string _evprmMessage = "Value must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class enquiryemail
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "AP.enquiryemail.DataType";
                    private const string _businessRule = "AP.7.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "Authorised product PhV Email can't be longer than 100 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "AP.enquiryemail.BR1";
                    private const string _businessRule = "AP.7.BR.1";
                    private const string _readyMessage = "If the operation type is 'insert', 'update' or 'variation', then PhV Email must be specified.";
                    private const string _evprmMessage = "If the operationtype is 'insert', 'update' or 'variation', then enquiryemail must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class BR2 : IXevprmValidationRule
                {
                    private const string _id = "AP.enquiryemail.BR2";
                    private const string _businessRule = "AP.7.BR.2";
                    private const string _readyMessage = "If the PhV Email is present then the value must match the format of a valid e-mail address. E.g. name@org.domain.";
                    private const string _evprmMessage = "If the enquiryemail is present then the value must match the format of a valid e-mail address. E.g. name@org.domain.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class enquiryphone
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "AP.enquiryphone.DataType";
                    private const string _businessRule = "AP.8.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "Authorised product PhV Phone can't be longer than 100 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "AP.enquiryphone.BR1";
                    private const string _businessRule = "AP.8.BR.1";
                    private const string _readyMessage = "If the operation type is 'insert', 'update' or 'variation', then PhV Phone must be specified.";
                    private const string _evprmMessage = "If the operationtype is 'insert', 'update' or 'variation', then enquiryphone must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class infodate
            {
                public class BR3 : IXevprmValidationRule
                {
                    private const string _id = "AP.infodate.BR3";
                    private const string _businessRule = "AP.11.BR.3";
                    private const string _readyMessage = "If info date is specified then the value must be before the current time/date (GMT) + 12 hours.";
                    private const string _evprmMessage = "If infodate is specified then the value must be before the current time/date (GMT) + 12 hours.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class authorisation
            {
                public class authorisationcountrycode
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.authorisationcountrycode.DataType";
                        private const string _businessRule = "AP.12.1.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueIsMissing:
                                    _readyMessage = "Authorisation country code is missing abbreviation value.";
                                    break;
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Authorisation country code can't be longer than 2 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }

                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.authorisationcountrycode.Cardinality";
                        private const string _businessRule = "AP.12.1.Cardinality";
                        private const string _readyMessage = "Authorisation country code must be specified.";
                        private const string _evprmMessage = "Value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class authorisationprocedure
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.authorisationprocedure.DataType";
                        private const string _businessRule = "AP.12.2.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.InvalidValue:
                                    _readyMessage = "Authorisation procedure value must be positive number with max two digits.";
                                    break;
                                case DataTypeEror.InvalidValueFormat:
                                    _readyMessage = "Authorisation procedure value is not in valid format.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }

                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.authorisationprocedure.Cardinality";
                        private const string _businessRule = "AP.12.2.Cardinality";
                        private const string _readyMessage = "Authorisation procedure must be specified.";
                        private const string _evprmMessage = "Value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class authorisationstatus
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.authorisationstatus.DataType";
                        private const string _businessRule = "AP.12.3.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.InvalidValue:
                                    _readyMessage = "Authorisation status value can have max two digits.";
                                    break;
                                case DataTypeEror.InvalidValueFormat:
                                    _readyMessage = "Authorisation status value is not in valid format.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }

                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.authorisationstatus.BR1";
                        private const string _businessRule = "AP.12.3.BR.1";
                        private const string _readyMessage = "If the operation type is not 'nullification' then authorisation status must be present.";
                        private const string _evprmMessage = "If the operation type is not 'nullification' then authorisationstatus must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.authorisationstatus.BR3";
                        private const string _businessRule = "AP.12.3.BR.3";
                        private const string _readyMessage = "If the authorisation status is NOT 'Valid' or 'Suspended' then operation type must be 'withdrawn'.";
                        private const string _evprmMessage = "If the authorisationstatus is NOT 'Valid' or 'Suspended' then operationtype must be 'withdrawn'.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR5 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.authorisationstatus.BR5";
                        private const string _businessRule = "AP.12.3.BR.5";
                        private const string _readyMessage = "If the authorisation status is 'Valid' or 'Suspended' then operation type must NOT be 'withdrawn'.";
                        private const string _evprmMessage = "If the authorisationstatus is 'Valid' or 'Suspended' then operationtype must NOT be 'withdrawn'.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class authorisationnumber
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.authorisationnumber.DataType";
                        private const string _businessRule = "AP.12.4.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Authorisation number can't be longer than 100 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }

                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.authorisationnumber.BR1";
                        private const string _businessRule = "AP.12.4.BR.1";
                        private const string _readyMessage = "If the operation type is 'insert', 'update', 'variation' or 'withdrawal', then authorisation number must be present.";
                        private const string _evprmMessage = "If the operationtype is 'insert', 'update', 'variation' or 'withdrawal', then authorisationnumber must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class authorisationdate
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.authorisationdate.BR1";
                        private const string _businessRule = "AP.12.5.BR.1";
                        private const string _readyMessage = "If the operation type is 'insert', 'update', 'variation' or 'withdrawal', then authorisation date must be present.";
                        private const string _evprmMessage = "If the operationtype is 'insert', 'update', 'variation' or 'withdrawal', then authorisationdate must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR5 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.authorisationdate.BR5";
                        private const string _businessRule = "AP.12.5.BR.5";
                        private const string _readyMessage = "The value of authorisation date must not be greater than the current date/time (GMT) + 12 hours.";
                        private const string _evprmMessage = "The value of authorisationdate must not be greater than the current date/time (GMT) + 12 hours.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class mrpnumber
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.mrpnumber.DataType";
                        private const string _businessRule = "AP.12.7.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Product number (mrpnumber) can't be longer than 50 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.mrpnumber.BR1";
                        private const string _businessRule = "AP.12.7.BR.1";
                        private readonly string _readyMessage;
                        private readonly string _evprmMessage;

                        public BR1(RegistrationProcedure authorisationRegistrationProcedure)
                        {
                            switch (authorisationRegistrationProcedure)
                            {
                                case RegistrationProcedure.Centralised:
                                    _readyMessage = "If the medicinal product follows the 'EU centralised procedure', the 'EMA procedure number' may be specified.";
                                    _evprmMessage = "If the medicinal product follows the 'EU centralised procedure', the mrpnumber as 'EMA procedure number' may be specified.";
                                    break;
                                case RegistrationProcedure.Decentralised:
                                    _readyMessage = "If the authorisation procedure follows the 'EU decentralised procedure', the 'Decentralised procedure (DCP) number' must be specified.";
                                    _evprmMessage = "If the authorisationprocedure is an 'EU decentralised procedure', the mrpnumber as 'Decentralised procedure (DCP) number' must be specified.";
                                    break;
                                case RegistrationProcedure.Mutual:
                                    _readyMessage = "If the authorisation procedure follows the 'EU mutual recognition procedure', the 'Mutual recognition procedure (MRP) number' must be specified.";
                                    _evprmMessage = "If the authorisationprocedure is an 'EU mutual recognition procedure', the mrpnumber as 'Mutual recognition procedure (MRP) number' must be specified.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class eunumber
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.eunumber.DataType";
                        private const string _businessRule = "AP.12.8.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Authorisation number (eunumber) can't be longer than 50 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }

                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.eunumber.BR1";
                        private const string _businessRule = "AP.12.8.BR.1";
                        private const string _readyMessage = "If the authorisation procedure is an 'EU centralised procedure', the 'EU Authorisation Number' must be specified.";
                        private const string _evprmMessage = "If the authorisationprocedure is an 'EU centralised procedure', the eunumber as 'EU Authorisation Number' must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class orphandrug
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.orphandrug.BR1";
                        private const string _businessRule = "AP.12.9.BR.1";
                        private const string _readyMessage = "If the operation type is 'insert', 'update' or 'variation', then orphan drug must be present.";
                        private const string _evprmMessage = "If the operationtype is 'insert', 'update' or 'variation', then orphandrug must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class intensivemonitoring
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.intensivemonitoring.DataType";
                        private const string _businessRule = "AP.12.10.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.InvalidValue:
                                    _readyMessage = "Intensive monitoring value, when it's specified, must be '1' (Subject to intensive monitoring) or '2' (NOT subject to intensive monitoring).";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }

                    }
                }

                public class withdrawndate
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.withdrawndate.BR1";
                        private const string _businessRule = "AP.12.12.BR.1";
                        private const string _readyMessage = "If the operation type is 'withdrawal', then withdrawn date must be present.";
                        private const string _evprmMessage = "If the operation type is 'withdrawal', then withdrawndate must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.withdrawndate.BR3";
                        private const string _businessRule = "AP.12.12.BR.3";
                        private const string _readyMessage = "The value of Withdrawn date must not be greater than the current date/time (GMT) + 12 hours.";
                        private const string _evprmMessage = "The value of withdrawndate must not be greater than the current date/time (GMT) + 12 hours.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR5 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.withdrawndate.BR5";
                        private const string _businessRule = "AP.12.12.BR.5";
                        private const string _readyMessage = "If authorisation status is NOT 'Valid', then withdrawn date must be present.";
                        private const string _evprmMessage = "If the value of authorisationstatus doesn't corresponds to authorisation status 'Valid', then withdrawn date must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR6 : IXevprmValidationRule
                    {
                        private const string _id = "AP.authorisation.withdrawndate.BR6";
                        private const string _businessRule = "AP.12.12.BR.6";
                        private const string _readyMessage = "If authorisation status is 'Valid', then withdrawn date must be empty.";
                        private const string _evprmMessage = "If the value of authorisationstatus corresponds to authorisation status 'Valid', then withdrawn date must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }
            }

            public static class presentationname
            {
                public class productname
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.presentationname.productname.DataType";
                        private const string _businessRule = "AP.13.1.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Full presentation name can't be longer than 2000 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }

                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "AP.presentationname.productname.Cardinality";
                        private const string _businessRule = "AP.13.1.Cardinality";
                        private const string _readyMessage = "Full presentation name must be specified.";
                        private const string _evprmMessage = "Value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class productshortname
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.presentationname.productshortname.DataType";
                        private const string _businessRule = "AP.13.2.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Product short name can't be longer than 500 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }

                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "AP.presentationname.productshortname.BR2";
                        private const string _businessRule = "AP.13.2.BR.2";
                        private const string _readyMessage = "Either product short name or product generic name must be specified.";
                        private const string _evprmMessage = "Either productshortname or productgenericname must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class productgenericname
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.presentationname.productgenericname.DataType";
                        private const string _businessRule = "AP.13.3.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Product generic name can't be longer than 1000 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }

                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "AP.presentationname.productgenericname.BR2";
                        private const string _businessRule = "AP.13.3.BR.2";
                        private const string _readyMessage = "Either product short name or product generic name must be specified.";
                        private const string _evprmMessage = "Either productshortname or productgenericname must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class productcompanyname
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.presentationname.productcompanyname.DataType";
                        private const string _businessRule = "AP.13.4.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Product company name can't be longer than 250 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }

                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "AP.presentationname.productcompanyname.BR2";
                        private const string _businessRule = "AP.13.4.BR.2";
                        private const string _readyMessage = "If product short name is absent product company name must be present.";
                        private const string _evprmMessage = "If productshortname is absent productcompanyname must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class productstrenght
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.presentationname.productstrenght.DataType";
                        private const string _businessRule = "AP.13.5.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Product strength can't be longer than 250 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class productform
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.presentationname.productform.DataType";
                        private const string _businessRule = "AP.13.6.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Product form can't be longer than 500 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class packagedesc
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.presentationname.packagedesc.DataType";
                        private const string _businessRule = "AP.13.7.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Package description can't be longer than 2000 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }
            }

            public static class PP
            {
                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "AP.PP.Cardinality";
                    private const string _businessRule = "AP.PP.Cardinality";
                    private const string _readyMessage = "Product doesn't contain any pharmaceutical products. At least one pharmaceutical product must be present.";
                    private const string _evprmMessage = "pharmaceuticalproducts section can't be empty. At least one pharmaceuticalproduct must be present.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class ATC
            {
                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "AP.ATCs.Cardinality";
                    private const string _businessRule = "PP.ATCs.Cardinality";
                    private const string _readyMessage = "Authorised product doesn't contain any ATC code. At least one ATC code must be present.";
                    private const string _evprmMessage = "productatcs section can't be empty. At least one productatc must be present.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }


                public class atccode
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.ATC.atccode.DataType";
                        private const string _businessRule = "AP.ATC.1.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueIsMissing:
                                    _readyMessage = "The ATC code is missing value.";
                                    break;
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "The ATC code can't be longer than 60 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "AP.ATC.atccode.Cardinality";
                        private const string _businessRule = "AP.ATC.1.Cardinality";
                        private const string _readyMessage = "The ATC code of the authorised/registered product must be specified.";
                        private const string _evprmMessage = "The atccode of the authorised/registered product must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "AP.ATCs.BR1";
                    private const string _businessRule = "AP.ATCs.BR.1";
                    private const string _readyMessage = "Each referenced ATC code must be unique for the current product.";
                    private const string _evprmMessage = "Each referenced atccode must be unique for the current authorised product.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class IND
            {
                public class meddracode
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.IND.meddracode.DataType";
                        private const string _businessRule = "AP.IND.3.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueIsMissing:
                                    _readyMessage = "The MedDRA code must be specified.";
                                    break;
                                case DataTypeEror.InvalidValueFormat:
                                    _readyMessage = "The MedDRA code is not a valid integer.";
                                    break;
                                case DataTypeEror.InvalidValue:
                                    _readyMessage = "The MedDRA code can have max 8 digits.";
                                    _evprmMessage = "The meddracode can have max 8 digits.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "AP.IND.meddracode.Cardinality";
                        private const string _businessRule = "AP.IND.3.Cardinality";
                        private const string _readyMessage = "The MedDRA code for the indication term must be specified.";
                        private const string _evprmMessage = "The meddracode for the indication term must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class meddralevel
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.IND.meddralevel.DataType";
                        private const string _businessRule = "AP.IND.2.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueIsMissing:
                                    _readyMessage = "The MedDRA level is missing value";
                                    break;
                                case DataTypeEror.InvalidValue:
                                    _readyMessage = "The MedDRA level is not valid.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "AP.IND.meddralevel.Cardinality";
                        private const string _businessRule = "AP.IND.2.Cardinality";
                        private const string _readyMessage = "The level of the MedDRA hierarchy applied to code the indication must be specified.";
                        private const string _evprmMessage = "The meddralevel must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class meddraversion
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.IND.meddraversion.DataType";
                        private const string _businessRule = "AP.IND.1.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueIsMissing:
                                    _readyMessage = "The MedDRA version is missing value";
                                    break;
                                case DataTypeEror.InvalidValue:
                                    _readyMessage = "The MedDRA version is not valid decimal.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "AP.IND.meddraversion.Cardinality";
                        private const string _businessRule = "AP.IND.1.Cardinality";
                        private const string _readyMessage = "The MedDRA version must be specified.";
                        private const string _evprmMessage = "The meddraversion must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "AP.INDs.BR1";
                    private const string _businessRule = "AP.INDs.BR.1";
                    private const string _readyMessage = "If the operation type is 'insert', 'update' or 'variation' then at least one indication must be present.";
                    private const string _evprmMessage = "If the operation type is 'insert', 'update' or 'variation' then indications section must be present.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class BR2 : IXevprmValidationRule
                {
                    private const string _id = "AP.INDs.BR2";
                    private const string _businessRule = "AP.INDs.BR.2";
                    private const string _readyMessage = "Each referenced MedDRA code must be unique for the current product.";
                    private const string _evprmMessage = "Each referenced meddracode must be unique for the current product.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class PPI
            {
                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "AP.PPI.BR1";
                    private const string _businessRule = "AP.PPIs.BR.1";
                    private const string _readyMessage = "Each referenced attachment must be unique for current product.";
                    private const string _evprmMessage = "Each referenced attachment EV Code must be unique within the PPIs section.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class BR2 : IXevprmValidationRule
                {
                    private const string _id = "AP.PPI.BR2";
                    private const string _businessRule = "AP.PPIs.BR.2";
                    private const string _readyMessage = "Each referenced attachment must be unique for current product.";
                    private const string _evprmMessage = "Each referenced local number must be unique within the PPIs section.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class BR3 : IXevprmValidationRule
                {
                    private const string _id = "AP.PPI.BR3";
                    private const string _businessRule = "AP.PPIs.BR.3";
                    private const string _readyMessage = "If the operation type is 'insert', 'update' or 'variation' then authorised product must have PPI attachment.";
                    private const string _evprmMessage = "If the operation type is 'insert', 'update' or 'variation' (AP..1 < 4) then this element must be present.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class CustomBR1 : IXevprmValidationRule
                {
                    private const string _id = "AP.PPI.CustomBR1";
                    private const string _businessRule = "";
                    private const string _readyMessage = "Authorised product has more than one PPI attachment. Authorised product must have only one PPI attachment.";
                    private const string _evprmMessage = "authorisedproduct contains ppiattachments section with more than one ppiattachment. authorisedproduct must contain ppiattachments section with only one ppiattachment.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class attachmentcode
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.PPI.attachmentcode.DataType";
                        private const string _businessRule = "AP.PPI.1.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "The attachment EV Code can't be longer than 60 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "AP.PPI.attachmentcode.BR1";
                        private const string _businessRule = "AP.PPI.1.BR.1";
                        private const string _readyMessage = "Authorised product PPI attachment can't reference non existing attachment.";
                        private const string _evprmMessage = "If the value of resolution mode is 'local number' (AP.PPI.1..1 = 1) then the referenced local number must match a value in one of the fields ATT.1 in the attachments section.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "AP.PPI.attachmentcode.BR2";
                        private const string _businessRule = "AP.PPI.1.BR.2";
                        private const string _readyMessage = "If Authorised product PPI attachment doesn't have EV Code then attachment type must be PPI.";
                        private const string _evprmMessage = "If the value of resolution mode is 'local number' (AP.PPI.1..1 = 1) then the referenced local ATT must be a 'PPI' (ATT.5 = 1).";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "AP.PPI.attachmentcode.BR3";
                        private const string _businessRule = "AP.PPI.1.BR.1";
                        private const string _readyMessage = "If Authorised product PPI attachment has EV Code then referenced attachment type must be PPI.";
                        private const string _evprmMessage = "If the value of resolution mode is 'global' (AP.PPI.1..1) AND the field AP.3 is empty then the referenced EV Code must match a current attachment owned by the sender’s (H.5) EV Group, the referenced attachment must be a 'PPI'.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public static class validitydeclaration
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "AP.PPI.validitydeclaration.DataType";
                        private const string _businessRule = "AP.PPI.2.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.InvalidValue:
                                    _readyMessage = "The validity declaration value must be '1' or '2'.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "AP.PPI.validitydeclaration.BR1";
                        private const string _businessRule = "AP.PPI.2.BR.1";
                        private const string _readyMessage = "If the operation type is 'update' or 'variation' and New Owner ID is empty then validity declaration must be present.";
                        private const string _evprmMessage = "If the value of resolution mode is 'global' (AP.PPI.1..1) AND the field AP.3 is empty AND the value of operation type is 'update' or 'variation' (AP..1 = 2 or 3) then validitydeclaration must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }
            }

            public static class comments
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "AP.comments.DataType";
                    private const string _businessRule = "AP.14.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "Comment(EVPRM) can't be longer than 500 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "AP.comments.BR1";
                    private const string _businessRule = "AP.14.BR.1";
                    private const string _readyMessage = "If the value of operation type is NOT 'insert', 'update' or 'variation' then Comment(EVPRM) must be present.";
                    private const string _evprmMessage = "If the value of operation type is NOT 'insert', 'update' or 'variation' then comments must be present.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }
        }

        public static class PP
        {
            public static class pharmformcode
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "PP.pharmformcode.DataType";
                    private const string _businessRule = "PP.1.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueIsMissing:
                                _readyMessage = "The pharmaceutical form is missing EV Code.";
                                break;
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "The pharmaceutical form EV Code can't be longer than 60 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "PP.pharmformcode.Cardinality";
                    private const string _businessRule = "PP.1.Cardinality";
                    private const string _readyMessage = "The pharmaceutical form of the medicinal product must be specified.";
                    private const string _evprmMessage = "The pharmformcode of the medicinal product must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class AR
            {
                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "PP.AR.Cardinality";
                    private const string _businessRule = "PP.ARs.Cardinality";
                    private const string _readyMessage = "Pharmaceutical product doesn't contain any administration routes. At least one administration route must be present.";
                    private const string _evprmMessage = "adminroutes section can't be empty. At least one adminroute must be present.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class adminroutecode
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.AR.adminroutecode.DataType";
                        private const string _businessRule = "PP.AR.1.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueIsMissing:
                                    _readyMessage = "The administration route is missing EV Code.";
                                    break;
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "The administration route EV Code can't be longer than 60 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "PP.AR.BR1";
                    private const string _businessRule = "PP.ARs.BR.1";
                    private const string _readyMessage = "Each referenced administration route must be unique for the current pharmaceutical product.";
                    private const string _evprmMessage = "Each referenced administration route adminroutecode must be unique for the current pharmaceuticalproduct.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class MD
            {
                public class medicaldevicecode
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.MD.medicaldevicecode.DataType";
                        private const string _businessRule = "PP.MD.1.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.InvalidValueFormat:
                                    _readyMessage = "Medical device code is not in valid format.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "PP.MD.BR1";
                    private const string _businessRule = "PP.MDs.BR.1";
                    private const string _readyMessage = "Each referenced medical device must be unique for the current pharmaceutical product.";
                    private const string _evprmMessage = "Each referenced medicaldevicecode must be unique for the current pharmaceuticalproduct.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class ACT
            {
                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "PP.ACT.Cardinality";
                    private const string _businessRule = "PP.ACTs.Cardinality";
                    private const string _readyMessage = "Pharmaceutical product doesn't contain any active ingredients. At least one active ingredient must be present.";
                    private const string _evprmMessage = "activeingredients section can't be empty. At least one activeingredient must be present.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "PP.ACT.BR1";
                    private const string _businessRule = "PP.ACTs.BR.1";
                    private const string _readyMessage = "Each referenced active ingredient substance must be unique for the current pharmaceutical product.";
                    private const string _evprmMessage = "Each referenced active ingredient substancecode must be unique for the current pharmaceuticalproduct.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class substancecode
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.substancecode.DataType";
                        private const string _businessRule = "PP.ACT.1.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueIsMissing:
                                    _readyMessage = "Substance code is missing EV Code.";
                                    break;
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Substance code EV Code can't be longer than 60 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.substancecode.Cardinality";
                        private const string _businessRule = "PP.ACT.1.Cardinality";
                        private const string _readyMessage = "Substance must be specified.";
                        private const string _evprmMessage = "substancecode must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class concentrationtypecode
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.concentrationtypecode.BR1";
                        private const string _businessRule = "PP.ACT.2.BR.1";
                        private const string _readyMessage = "Concentration type is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.concentrationtypecode.Cardinality";
                        private const string _businessRule = "PP.ACT.2.Cardinality";
                        private const string _readyMessage = "Concentration type must be specified.";
                        private const string _evprmMessage = "concentrationtypecode must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountnumervalue
                {
                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountnumervalue.Cardinality";
                        private const string _businessRule = "PP.ACT.3.BR.1";
                        private const string _readyMessage = "The low limit amount numerator value or, for non-range measurements, the amount numerator value must be specified.";
                        private const string _evprmMessage = "The lowamountnumervalue value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountnumerprefix
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountnumerprefix.DataType";
                        private const string _businessRule = "PP.ACT.4.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Low amount numerator prefix can't be longer than 12 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountnumerprefix.BR1";
                        private const string _businessRule = "PP.ACT.4.BR.1";
                        private const string _readyMessage = "Low amount numerator prefix is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountnumerprefix.Cardinality";
                        private const string _businessRule = "PP.ACT.4.Cardinality";
                        private const string _readyMessage = "The low limit amount numerator prefix or, for non-range measurements, the amount numerator prefix must be specified.";
                        private const string _evprmMessage = "The lowamountnumerprefix value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountnumerunit
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountnumerunit.DataType";
                        private const string _businessRule = "PP.ACT.5.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Low amount numerator unit can't be longer than 70 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountnumerunit.BR1";
                        private const string _businessRule = "PP.ACT.5.BR.1";
                        private const string _readyMessage = "Low amount numerator unit is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountnumerunit.Cardinality";
                        private const string _businessRule = "PP.ACT.5.Cardinality";
                        private const string _readyMessage = "The low limit amount numerator unit or, for non-range measurements, the amount numerator unit must be specified.";
                        private const string _evprmMessage = "The lowamountnumerunit value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountdenomvalue
                {
                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountdenomvalue.Cardinality";
                        private const string _businessRule = "PP.ACT.6.Cardinality";
                        private const string _readyMessage = "The low limit amount denominator value or, for non-range measurements, the amount denominator value must be specified.";
                        private const string _evprmMessage = "The lowamountdenomvalue value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountdenomprefix
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountdenomprefix.DataType";
                        private const string _businessRule = "PP.ACT.7.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Low amount denominator prefix can't be longer than 12 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountdenomprefix.BR1";
                        private const string _businessRule = "PP.ACT.7.BR.1";
                        private const string _readyMessage = "Low amount denominator prefix is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountdenomprefix.Cardinality";
                        private const string _businessRule = "PP.ACT.7.Cardinality";
                        private const string _readyMessage = "The low limit amount denominator prefix, or for non-range measurements, the amount denominator prefix must be specified.";
                        private const string _evprmMessage = "The lowamountdenomprefix value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountdenomunit
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountdenomunit.DataType";
                        private const string _businessRule = "PP.ACT.8.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Low amount denominator unit can't be longer than 70 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountdenomunit.BR1";
                        private const string _businessRule = "PP.ACT.8.BR.1";
                        private const string _readyMessage = "Low amount denominator unit is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.lowamountdenomunit.Cardinality";
                        private const string _businessRule = "PP.ACT.8.Cardinality";
                        private const string _readyMessage = "The low limit denominator unit or, for non-range measurements, the amount numerator unit must be specified.";
                        private const string _evprmMessage = "The lowamountdenomunit value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountnumervalue
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountnumervalue.BR1";
                        private const string _businessRule = "PP.ACT.9.BR.1";
                        private const string _readyMessage = "If the quantity operator equates to 'range' (Concentration type = 2) the high limit amount numerator value must be specified.";
                        private const string _evprmMessage = "If the quantity operator equates to 'range' (concentrationtypecode = 2) the highamountnumervalue must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountnumervalue.BR2";
                        private const string _businessRule = "PP.ACT.9.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount numerator value must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountnumervalue must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountnumerprefix
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountnumerprefix.DataType";
                        private const string _businessRule = "PP.ACT.10.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "High amount numerator prefix can't be longer than 12 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountnumerprefix.BR1";
                        private const string _businessRule = "PP.ACT.10.BR.1";
                        private const string _readyMessage = "If the quantity operator equates to 'range' (Concentration type = 2) the high limit amount numerator prefix must be specified.";
                        private const string _evprmMessage = "If the quantity operator equates to 'range' (concentrationtypecode = 2) the highamountnumerprefix must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountnumerprefix.BR2";
                        private const string _businessRule = "PP.ACT.10.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount numerator prefix must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountnumerprefix must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountnumerprefix.BR3";
                        private const string _businessRule = "PP.ACT.10.BR.3";
                        private const string _readyMessage = "High amount numerator prefix is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountnumerunit
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountnumerunit.DataType";
                        private const string _businessRule = "PP.ACT.11.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "High amount numerator unit can't be longer than 70 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountnumerunit.BR1";
                        private const string _businessRule = "PP.ACT.11.BR.1";
                        private const string _readyMessage = "If the quantity operator equates to 'range' (Concentration type = 2) the high limit amount numerator unit must be specified.";
                        private const string _evprmMessage = "If the quantity operator equates to 'range' (concentrationtypecode = 2) the highamountnumerunit must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountnumerunit.BR2";
                        private const string _businessRule = "PP.ACT.11.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount numerator unit must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountnumerunit must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountnumerunit.BR3";
                        private const string _businessRule = "PP.ACT.11.BR.3";
                        private const string _readyMessage = "If high amount numerator unit is present the value must match the value of low amount numerator unit.";
                        private const string _evprmMessage = "If highamountnumerunit is present the value must match the value of lowamountnumerunit.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BRCustom1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountnumerunit.BRCustom1";
                        private const string _businessRule = null;
                        private const string _readyMessage = "High amount numerator unit is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountdenomvalue
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomvalue.BR1";
                        private const string _businessRule = "PP.ACT.12.BR.1";
                        private const string _readyMessage = "If the quantity operator equates to 'range' (Concentration type = 2) the high limit amount denominator value must be specified.";
                        private const string _evprmMessage = "If the quantity operator equates to 'range' (concentrationtypecode = 2) the highamountdenomvalue must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomvalue.BR2";
                        private const string _businessRule = "PP.ACT.12.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount denominator value must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountdenomvalue must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomvalue.BR3";
                        private const string _businessRule = "PP.ACT.12.BR.3";
                        private const string _readyMessage = "If high amount denominator value is present the value must match the value of low amount denominator value.";
                        private const string _evprmMessage = "If highamountdenomvalue is present the value must match the value of lowamountdenomvalue.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountdenomprefix
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomprefix.DataType";
                        private const string _businessRule = "PP.ACT.13.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "High amount denominator prefix can't be longer than 12 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomprefix.BR1";
                        private const string _businessRule = "PP.ACT.13.BR.1";
                        private const string _readyMessage = "If the quantity operator equates to 'range' (Concentration type = 2) the high limit amount denominator prefix must be specified.";
                        private const string _evprmMessage = "If the quantity operator equates to 'range' (concentrationtypecode = 2) the highamountdenomprefix must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomprefix.BR2";
                        private const string _businessRule = "PP.ACT.13.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount denominator prefix must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountdenomprefix must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomprefix.BR3";
                        private const string _businessRule = "PP.ACT.13.BR.3";
                        private const string _readyMessage = "If high amount denominator prefix is present the value must match the value of low amount denominator prefix.";
                        private const string _evprmMessage = "If highamountdenomprefix is present the value must match the value of lowamountdenomprefix.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BRCustom1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomprefix.BRCustom1";
                        private const string _businessRule = null;
                        private const string _readyMessage = "High amount denominator prefix is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountdenomunit
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomunit.DataType";
                        private const string _businessRule = "PP.ACT.14.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "High amount denominator unit can't be longer than 70 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomunit.BR1";
                        private const string _businessRule = "PP.ACT.14.BR.1";
                        private const string _readyMessage = "If the quantity operator equates to 'range' (Concentration type = 2) the high limit amount denominator unit must be specified.";
                        private const string _evprmMessage = "If the quantity operator equates to 'range' (concentrationtypecode = 2) the highamountdenomunit must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomunit.BR2";
                        private const string _businessRule = "PP.ACT.14.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount denominator unit must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountdenomunit must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomunit.BR3";
                        private const string _businessRule = "PP.ACT.14.BR.3";
                        private const string _readyMessage = "If high amount denominator unit is present the value must match the value of low amount denominator unit.";
                        private const string _evprmMessage = "If highamountdenomunit is present the value must match the value of lowamountdenomunit.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BRCustom1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ACT.highamountdenomunit.BRCustom1";
                        private const string _businessRule = null;
                        private const string _readyMessage = "High amount denominator unit is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }
            }

            public static class EXC
            {
                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "PP.EXC.BR1";
                    private const string _businessRule = "PP.EXCs.BR.1";
                    private const string _readyMessage = "Each referenced excipient substance must be unique for the current pharmaceutical product.";
                    private const string _evprmMessage = "Each referenced excipient substancecode must be unique for the current pharmaceuticalproduct.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class substancecode
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.substancecode.DataType";
                        private const string _businessRule = "PP.EXC.1.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueIsMissing:
                                    _readyMessage = "Substance code is missing EV Code.";
                                    break;
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Substance code EV Code can't be longer than 60 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.substancecode.Cardinality";
                        private const string _businessRule = "PP.EXC.1.Cardinality";
                        private const string _readyMessage = "Substance must be specified.";
                        private const string _evprmMessage = "substancecode must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class concentrationtypecode
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.concentrationtypecode.BR1";
                        private const string _businessRule = "PP.EXC.2.BR.1";
                        private const string _readyMessage = "Concentration type is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.concentrationtypecode.BR2";
                        private const string _businessRule = "PP.EXC.2.BR.2";
                        private string _readyMessage;
                        private string _evprmMessage;

                        private const string _readyMessageFormat = "If concentration type is present the {0} must be present.";
                        private const string _evprmMessageFormat = "If concentrationtypecode is present the {0} must be present.";

                        public BR2(string field)
                        {
                            switch (field)
                            {
                                case "lowamountnumervalue":
                                    _readyMessage = string.Format(_readyMessageFormat, "low limit numerator value");
                                    _evprmMessage = string.Format(_evprmMessageFormat, field);
                                    break;
                                case "lowamountnumerprefix":
                                    _readyMessage = string.Format(_readyMessageFormat, "low limit numerator prefix");
                                    _evprmMessage = string.Format(_evprmMessageFormat, field);
                                    break;
                                case "lowamountnumerunit":
                                    _readyMessage = string.Format(_readyMessageFormat, "low limit numerator unit");
                                    _evprmMessage = string.Format(_evprmMessageFormat, field);
                                    break;
                                case "lowamountdenomvalue":
                                    _readyMessage = string.Format(_readyMessageFormat, "low limit denominator value");
                                    _evprmMessage = string.Format(_evprmMessageFormat, field);
                                    break;
                                case "lowamountdenomprefix":
                                    _readyMessage = string.Format(_readyMessageFormat, "low limit denominator prefix");
                                    _evprmMessage = string.Format(_evprmMessageFormat, field);
                                    break;
                                case "lowamountdenomunit":
                                    _readyMessage = string.Format(_readyMessageFormat, "low limit denominator unit");
                                    _evprmMessage = string.Format(_evprmMessageFormat, field);
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.concentrationtypecode.BR3";
                        private const string _businessRule = "PP.EXC.2.BR.3";
                        private string _readyMessage;
                        private string _evprmMessage;

                        private const string _readyMessageFormat = "If concentration type is present and has a value of 2 'range' the {0} must be present.";
                        private const string _evprmMessageFormat = "If concentrationtypecode is present and has a value of 2 'range' the {0} must be present.";

                        public BR3(string field)
                        {
                            switch (field)
                            {
                                case "highamountnumervalue":
                                    _readyMessage = string.Format(_readyMessageFormat, "high limit numerator value");
                                    _evprmMessage = string.Format(_evprmMessageFormat, field);
                                    break;
                                case "highamountnumerprefix":
                                    _readyMessage = string.Format(_readyMessageFormat, "high limit numerator prefix");
                                    _evprmMessage = string.Format(_evprmMessageFormat, field);
                                    break;
                                case "highamountnumerunit":
                                    _readyMessage = string.Format(_readyMessageFormat, "high limit numerator unit");
                                    _evprmMessage = string.Format(_evprmMessageFormat, field);
                                    break;
                                case "highamountdenomvalue":
                                    _readyMessage = string.Format(_readyMessageFormat, "high limit denominator value");
                                    _evprmMessage = string.Format(_evprmMessageFormat, field);
                                    break;
                                case "highamountdenomprefix":
                                    _readyMessage = string.Format(_readyMessageFormat, "high limit denominator prefix");
                                    _evprmMessage = string.Format(_evprmMessageFormat, field);
                                    break;
                                case "highamountdenomunit":
                                    _readyMessage = string.Format(_readyMessageFormat, "high limit denominator unit");
                                    _evprmMessage = string.Format(_evprmMessageFormat, field);
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountnumervalue
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountnumervalue.BR1";
                        private const string _businessRule = "PP.EXC.3.BR.1";
                        private const string _readyMessage = "If the concentration type is present then low amount numerator value must be present.";
                        private const string _evprmMessage = "If the concentration type is present then lowamountnumervalue must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountnumerprefix
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountnumerprefix.DataType";
                        private const string _businessRule = "PP.EXC.4.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Low amount numerator prefix can't be longer than 12 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountnumerprefix.BR1";
                        private const string _businessRule = "PP.EXC.4.BR.1";
                        private const string _readyMessage = "Low amount numerator prefix is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class CustomBR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountnumerprefix.CustomBR1";
                        private const string _businessRule = "PP.EXC.2.BR.2";
                        private const string _readyMessage = "If the concentration type is present then low amount numerator prefix must be present.";
                        private const string _evprmMessage = "If the concentration type is present then lowamountnumerprefix must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountnumerunit
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountnumerunit.DataType";
                        private const string _businessRule = "PP.EXC.5.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Low amount numerator unit can't be longer than 70 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountnumerunit.BR1";
                        private const string _businessRule = "PP.EXC.5.BR.1";
                        private const string _readyMessage = "Low amount numerator unit is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class CustomBR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountnumerunit.CustomBR1";
                        private const string _businessRule = "PP.EXC.2.BR.2";
                        private const string _readyMessage = "If the concentration type is present then low amount numerator unit must be present.";
                        private const string _evprmMessage = "If the concentration type is present then lowamountnumerunit must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountdenomvalue
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountdenomvalue.BR1";
                        private const string _businessRule = "PP.EXC.6.BR.1";
                        private const string _readyMessage = "If the concentration type is present then low amount denominator value must be present.";
                        private const string _evprmMessage = "If the concentration type is present then lowamountdenomvalue must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountdenomprefix
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountdenomprefix.DataType";
                        private const string _businessRule = "PP.EXC.7.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Low amount denominator prefix can't be longer than 12 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountdenomprefix.BR1";
                        private const string _businessRule = "PP.EXC.7.BR.1";
                        private const string _readyMessage = "Low amount denominator prefix is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountdenomprefix.BR2";
                        private const string _businessRule = "PP.EXC.7.BR.2";
                        private const string _readyMessage = "If the concentration type is present then low amount denominator prefix must be present.";
                        private const string _evprmMessage = "If the concentration type is present then lowamountdenomprefix must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountdenomunit
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountdenomunit.DataType";
                        private const string _businessRule = "PP.EXC.8.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Low amount denominator unit can't be longer than 70 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountdenomunit.BR1";
                        private const string _businessRule = "PP.EXC.8.BR.1";
                        private const string _readyMessage = "Low amount denominator unit is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.lowamountdenomunit.BR2";
                        private const string _businessRule = "PP.EXC.8.BR.2";
                        private const string _readyMessage = "If the concentration type is present then low amount denominator unit must be present.";
                        private const string _evprmMessage = "If the concentration type is present then lowamountdenomunit must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountnumervalue
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountnumervalue.BR1";
                        private const string _businessRule = "PP.EXC.9.BR.1";
                        private const string _readyMessage = "If concentration type is present and has a value of 2 'range' the high limit numerator value must be present.";
                        private const string _evprmMessage = "If concentrationtypecode is present and has a value of 2 'range' the highamountnumervalue must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountnumervalue.BR2";
                        private const string _businessRule = "PP.EXC.9.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount numerator value must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountnumervalue must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountnumerprefix
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountnumerprefix.DataType";
                        private const string _businessRule = "PP.EXC.10.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "High amount numerator prefix can't be longer than 12 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountnumerprefix.BR1";
                        private const string _businessRule = "PP.EXC.10.BR.1";
                        private const string _readyMessage = "If concentration type is present and has a value of 2 'range' the high limit numerator prefix must be present.";
                        private const string _evprmMessage = "If concentrationtypecode is present and has a value of 2 'range' the highamountnumerprefix must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountnumerprefix.BR2";
                        private const string _businessRule = "PP.EXC.10.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount numerator prefix must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountnumerprefix must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountnumerprefix.BR3";
                        private const string _businessRule = "PP.EXC.10.BR.3";
                        private const string _readyMessage = "High amount numerator prefix is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountnumerunit
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountnumerunit.DataType";
                        private const string _businessRule = "PP.EXC.11.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "High amount numerator unit can't be longer than 70 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountnumerunit.BR1";
                        private const string _businessRule = "PP.EXC.11.BR.1";
                        private const string _readyMessage = "If concentration type is present and has a value of 2 'range' the high limit numerator unit must be present.";
                        private const string _evprmMessage = "If concentrationtypecode is present and has a value of 2 'range' the highamountnumerunit must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountnumerunit.BR2";
                        private const string _businessRule = "PP.EXC.11.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount numerator unit must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountnumerunit must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountnumerunit.BR3";
                        private const string _businessRule = "PP.EXC.11.BR.3";
                        private const string _readyMessage = "If high amount numerator unit is present the value must match the value of low amount numerator unit.";
                        private const string _evprmMessage = "If highamountnumerunit is present the value must match the value of lowamountnumerunit.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BRCustom1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountnumerunit.BRCustom1";
                        private const string _businessRule = null;
                        private const string _readyMessage = "High amount numerator unit is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountdenomvalue
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomvalue.BR1";
                        private const string _businessRule = "PP.EXC.12.BR.1";
                        private const string _readyMessage = "If concentration type is present and has a value of 2 'range' the high limit denominator value must be present.";
                        private const string _evprmMessage = "If concentrationtypecode is present and has a value of 2 'range' the highamountdenomvalue must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomvalue.BR2";
                        private const string _businessRule = "PP.EXC.12.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount denominator value must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountdenomvalue must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomvalue.BR3";
                        private const string _businessRule = "PP.EXC.12.BR.3";
                        private const string _readyMessage = "If high amount denominator value is present the value must match the value of low amount denominator value.";
                        private const string _evprmMessage = "If highamountdenomvalue is present the value must match the value of lowamountdenomvalue.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountdenomprefix
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomprefix.DataType";
                        private const string _businessRule = "PP.EXC.13.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "High amount denominator prefix can't be longer than 12 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomprefix.BR1";
                        private const string _businessRule = "PP.EXC.13.BR.1";
                        private const string _readyMessage = "If concentration type is present and has a value of 2 'range' the high limit denominator prefix must be present.";
                        private const string _evprmMessage = "If concentrationtypecode is present and has a value of 2 'range' the highamountdenomprefix must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomprefix.BR2";
                        private const string _businessRule = "PP.EXC.13.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount denominator prefix must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountdenomprefix must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomprefix.BR3";
                        private const string _businessRule = "PP.EXC.13.BR.3";
                        private const string _readyMessage = "If high amount denominator prefix is present the value must match the value of low amount denominator prefix.";
                        private const string _evprmMessage = "If highamountdenomprefix is present the value must match the value of lowamountdenomprefix.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BRCustom1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomprefix.BRCustom1";
                        private const string _businessRule = null;
                        private const string _readyMessage = "High amount denominator prefix is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountdenomunit
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomunit.DataType";
                        private const string _businessRule = "PP.EXC.14.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "High amount denominator unit can't be longer than 70 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomunit.BR1";
                        private const string _businessRule = "PP.EXC.14.BR.1";
                        private const string _readyMessage = "If concentration type is present and has a value of 2 'range' the high limit denominator unit must be present.";
                        private const string _evprmMessage = "If concentrationtypecode is present and has a value of 2 'range' the highamountdenomunit must be present.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomunit.BR2";
                        private const string _businessRule = "PP.EXC.14.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount denominator unit must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountdenomunit must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomunit.BR3";
                        private const string _businessRule = "PP.EXC.14.BR.3";
                        private const string _readyMessage = "If high amount denominator unit is present the value must match the value of low amount denominator unit.";
                        private const string _evprmMessage = "If highamountdenomunit is present the value must match the value of lowamountdenomunit.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BRCustom1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.EXC.highamountdenomunit.BRCustom1";
                        private const string _businessRule = null;
                        private const string _readyMessage = "High amount denominator unit is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }
            }

            public static class ADJ
            {
                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "PP.ADJ.BR1";
                    private const string _businessRule = "PP.ADJs.BR.1";
                    private const string _readyMessage = "Each referenced adjuvant substance must be unique for the current pharmaceutical product.";
                    private const string _evprmMessage = "Each referenced adjuvant substancecode must be unique for the current pharmaceuticalproduct.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class substancecode
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.substancecode.DataType";
                        private const string _businessRule = "PP.ADJ.1.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueIsMissing:
                                    _readyMessage = "Substance code is missing EV Code.";
                                    break;
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Substance code EV Code can't be longer than 60 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.substancecode.Cardinality";
                        private const string _businessRule = "PP.ADJ.1.Cardinality";
                        private const string _readyMessage = "Substance must be specified.";
                        private const string _evprmMessage = "substancecode must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class concentrationtypecode
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.concentrationtypecode.BR1";
                        private const string _businessRule = "PP.ADJ.2.BR.1";
                        private const string _readyMessage = "Concentration type is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.concentrationtypecode.Cardinality";
                        private const string _businessRule = "PP.ADJ.2.Cardinality";
                        private const string _readyMessage = "Concentration type must be specified.";
                        private const string _evprmMessage = "concentrationtypecode must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountnumervalue
                {
                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountnumervalue.Cardinality";
                        private const string _businessRule = "PP.ADJ.3.Cardinality";
                        private const string _readyMessage = "The low limit amount numerator value or, for non-range measurements, the amount numerator value must be specified.";
                        private const string _evprmMessage = "The lowamountnumervalue value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountnumerprefix
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountnumerprefix.DataType";
                        private const string _businessRule = "PP.ADJ.4.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Low amount numerator prefix can't be longer than 12 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountnumerprefix.BR1";
                        private const string _businessRule = "PP.ADJ.4.BR.1";
                        private const string _readyMessage = "Low amount numerator prefix is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountnumerprefix.Cardinality";
                        private const string _businessRule = "PP.ADJ.4.Cardinality";
                        private const string _readyMessage = "The low limit amount numerator prefix or, for non-range measurements, the amount numerator prefix must be specified.";
                        private const string _evprmMessage = "The lowamountnumerprefix value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountnumerunit
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountnumerunit.DataType";
                        private const string _businessRule = "PP.ADJ.5.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Low amount numerator unit can't be longer than 70 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountnumerunit.BR1";
                        private const string _businessRule = "PP.ADJ.5.BR.1";
                        private const string _readyMessage = "Low amount numerator unit is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountnumerunit.Cardinality";
                        private const string _businessRule = "PP.ADJ.5.Cardinality";
                        private const string _readyMessage = "The low limit amount numerator unit or, for non-range measurements, the amount numerator unit must be specified.";
                        private const string _evprmMessage = "The lowamountnumerunit value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountdenomvalue
                {
                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountdenomvalue.Cardinality";
                        private const string _businessRule = "PP.ADJ.6.Cardinality";
                        private const string _readyMessage = "The low limit amount denominator value or, for non-range measurements, the amount denominator value must be specified.";
                        private const string _evprmMessage = "The lowamountdenomvalue value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountdenomprefix
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountdenomprefix.DataType";
                        private const string _businessRule = "PP.ADJ.7.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Low amount denominator prefix can't be longer than 12 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountdenomprefix.BR1";
                        private const string _businessRule = "PP.ADJ.7.BR.1";
                        private const string _readyMessage = "Low amount denominator prefix is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountdenomprefix.Cardinality";
                        private const string _businessRule = "PP.ADJ.7.Cardinality";
                        private const string _readyMessage = "The low limit amount denominator prefix, or for non-range measurements, the amount denominator prefix must be specified.";
                        private const string _evprmMessage = "The lowamountdenomprefix value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class lowamountdenomunit
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountdenomunit.DataType";
                        private const string _businessRule = "PP.ADJ.8.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "Low amount denominator unit can't be longer than 70 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountdenomunit.BR1";
                        private const string _businessRule = "PP.ADJ.8.BR.1";
                        private const string _readyMessage = "Low amount denominator unit is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class Cardinality : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.lowamountdenomunit.Cardinality";
                        private const string _businessRule = "PP.ADJ.8.Cardinality";
                        private const string _readyMessage = "The low limit denominator unit or, for non-range measurements, the amount numerator unit must be specified.";
                        private const string _evprmMessage = "The lowamountdenomunit value must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountnumervalue
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountnumervalue.BR1";
                        private const string _businessRule = "PP.ADJ.9.BR.1";
                        private const string _readyMessage = "If the quantity operator equates to 'range' (Concentration type = 2) the high limit amount numerator value must be specified.";
                        private const string _evprmMessage = "If the quantity operator equates to 'range' (concentrationtypecode = 2) the highamountnumervalue must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountnumervalue.BR2";
                        private const string _businessRule = "PP.ADJ.9.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount numerator value must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountnumervalue must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountnumerprefix
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountnumerprefix.DataType";
                        private const string _businessRule = "PP.ADJ.10.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "High amount numerator prefix can't be longer than 12 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountnumerprefix.BR1";
                        private const string _businessRule = "PP.ADJ.10.BR.1";
                        private const string _readyMessage = "If the quantity operator equates to 'range' (Concentration type = 2) the high limit amount numerator prefix must be specified.";
                        private const string _evprmMessage = "If the quantity operator equates to 'range' (concentrationtypecode = 2) the highamountnumerprefix must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountnumerprefix.BR2";
                        private const string _businessRule = "PP.ADJ.10.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount numerator prefix must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountnumerprefix must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountnumerprefix.BR3";
                        private const string _businessRule = "PP.ADJ.10.BR.3";
                        private const string _readyMessage = "High amount numerator prefix is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountnumerunit
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountnumerunit.DataType";
                        private const string _businessRule = "PP.ADJ.11.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "High amount numerator unit can't be longer than 70 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountnumerunit.BR1";
                        private const string _businessRule = "PP.ADJ.11.BR.1";
                        private const string _readyMessage = "If the quantity operator equates to 'range' (Concentration type = 2) the high limit amount numerator unit must be specified.";
                        private const string _evprmMessage = "If the quantity operator equates to 'range' (concentrationtypecode = 2) the highamountnumerunit must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountnumerunit.BR2";
                        private const string _businessRule = "PP.ADJ.11.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount numerator unit must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountnumerunit must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountnumerunit.BR3";
                        private const string _businessRule = "PP.ADJ.11.BR.3";
                        private const string _readyMessage = "If high amount numerator unit is present the value must match the value of low amount numerator unit.";
                        private const string _evprmMessage = "If highamountnumerunit is present the value must match the value of lowamountnumerunit.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BRCustom1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountnumerunit.BRCustom1";
                        private const string _businessRule = null;
                        private const string _readyMessage = "High amount numerator unit is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountdenomvalue
                {
                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomvalue.BR1";
                        private const string _businessRule = "PP.ADJ.12.BR.1";
                        private const string _readyMessage = "If the quantity operator equates to 'range' (Concentration type = 2) the high limit amount denominator value must be specified.";
                        private const string _evprmMessage = "If the quantity operator equates to 'range' (concentrationtypecode = 2) the highamountdenomvalue must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomvalue.BR2";
                        private const string _businessRule = "PP.ADJ.12.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount denominator value must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountdenomvalue must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomvalue.BR3";
                        private const string _businessRule = "PP.ADJ.12.BR.3";
                        private const string _readyMessage = "If high amount denominator value is present the value must match the value of low amount denominator value.";
                        private const string _evprmMessage = "If highamountdenomvalue is present the value must match the value of lowamountdenomvalue.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountdenomprefix
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomprefix.DataType";
                        private const string _businessRule = "PP.ADJ.13.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "High amount denominator prefix can't be longer than 12 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomprefix.BR1";
                        private const string _businessRule = "PP.ADJ.13.BR.1";
                        private const string _readyMessage = "If the quantity operator equates to 'range' (Concentration type = 2) the high limit amount denominator prefix must be specified.";
                        private const string _evprmMessage = "If the quantity operator equates to 'range' (concentrationtypecode = 2) the highamountdenomprefix must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomprefix.BR2";
                        private const string _businessRule = "PP.ADJ.13.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount denominator prefix must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountdenomprefix must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomprefix.BR3";
                        private const string _businessRule = "PP.ADJ.13.BR.3";
                        private const string _readyMessage = "If high amount denominator prefix is present the value must match the value of low amount denominator prefix.";
                        private const string _evprmMessage = "If highamountdenomprefix is present the value must match the value of lowamountdenomprefix.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BRCustom1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomprefix.BRCustom1";
                        private const string _businessRule = null;
                        private const string _readyMessage = "High amount denominator prefix is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }

                public class highamountdenomunit
                {
                    public class DataType : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomunit.DataType";
                        private const string _businessRule = "PP.ADJ.14.DataType";
                        private string _readyMessage;
                        private string _evprmMessage;

                        public DataType(DataTypeEror dataTypeEror)
                        {
                            switch (dataTypeEror)
                            {
                                case DataTypeEror.ValueLenghtToBig:
                                    _readyMessage = "High amount denominator unit can't be longer than 70 characters.";
                                    break;
                            }
                        }

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomunit.BR1";
                        private const string _businessRule = "PP.ADJ.14.BR.1";
                        private const string _readyMessage = "If the quantity operator equates to 'range' (Concentration type = 2) the high limit amount denominator unit must be specified.";
                        private const string _evprmMessage = "If the quantity operator equates to 'range' (concentrationtypecode = 2) the highamountdenomunit must be specified.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR2 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomunit.BR2";
                        private const string _businessRule = "PP.ADJ.14.BR.2";
                        private const string _readyMessage = "If the quantity operator does not equates to 'range' (Concentration type != 2) the high limit amount denominator unit must be empty.";
                        private const string _evprmMessage = "If the quantity operator does not equates to 'range' (concentrationtypecode != 2) the highamountdenomunit must be empty.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BR3 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomunit.BR3";
                        private const string _businessRule = "PP.ADJ.14.BR.3";
                        private const string _readyMessage = "If high amount denominator unit is present the value must match the value of low amount denominator unit.";
                        private const string _evprmMessage = "If highamountdenomunit is present the value must match the value of lowamountdenomunit.";

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }

                    public class BRCustom1 : IXevprmValidationRule
                    {
                        private const string _id = "PP.ADJ.highamountdenomunit.BRCustom1";
                        private const string _businessRule = null;
                        private const string _readyMessage = "High amount denominator unit is not in Controlled Vocabullary.";
                        private string _evprmMessage;

                        public static string RuleId { get { return _id; } }

                        public string Id { get { return _id; } }
                        public string BusinessRule { get { return _businessRule; } }
                        public string ReadyMessage { get { return _readyMessage; } }
                        public string EvprmMessage { get { return _evprmMessage; } }
                    }
                }
            }
        }

        public static class ATT
        {
            public class BRCustom1 : IXevprmValidationRule
            {
                private const string _id = "ATT.BRCustom1";
                private const string _businessRule = "";
                private const string _readyMessage = "Document is missing attachment.";
                private string _evprmMessage;

                public static string RuleId { get { return _id; } }

                public string Id { get { return _id; } }
                public string BusinessRule { get { return _businessRule; } }
                public string ReadyMessage { get { return _readyMessage; } }
                public string EvprmMessage { get { return _evprmMessage; } }
            }

            public static class operationtype
            {
                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "ATT.operationtype.BR1";
                    private const string _businessRule = "ATT..1.BR.1";
                    private const string _readyMessage = "The only operation type accepted is 'Insert'.";
                    private const string _evprmMessage = "The only value accepted is 1 for 'insert'.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class localnumber
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "ATT.localnumber.DataType";
                    private const string _businessRule = "ATT.1.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "Attachment local number can't be longer than 60 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "ATT.localnumber.Cardinality";
                    private const string _businessRule = "ATT.1.Cardinality";
                    private const string _readyMessage = "Attachment local number must be specified.";
                    private const string _evprmMessage = "The localnumber must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class BR1 : IXevprmValidationRule
                {
                    private const string _id = "ATT.localnumber.BR1";
                    private const string _businessRule = "ATT.1.BR.1";
                    private const string _readyMessage = "The local number must be a unique value within the attachments (M.ATTs) section.";
                    private const string _evprmMessage = "The localnumber must be a unique value within the attachments (M.ATTs) section.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class filename
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "ATT.filename.DataType";
                    private const string _businessRule = "ATT.2.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.InvalidValue:
                                _readyMessage = "File name extension doesn't match specified attachment type.";
                                _evprmMessage = "Filename extension doesn't match specified filetype.";
                                break;
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "The attachment file name can't be longer than 200 characters.";
                                _evprmMessage = "The attachment filename can't be longer than 200 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "ATT.filename.Cardinality";
                    private const string _businessRule = "ATT.2.Cardinality";
                    private const string _readyMessage = "The file name of the attachment with file extension must be specified.";
                    private const string _evprmMessage = "The filename of the attachment with file extension must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class filetype
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "ATT.filetype.DataType";
                    private const string _businessRule = "ATT.3.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueIsMissing:
                                _readyMessage = "Attachment type is missing value.";
                                break;
                            case DataTypeEror.InvalidValue:
                                _readyMessage = "Attachment type is not valid. Attachment file extension doesn't match attachment type.";
                                _evprmMessage = "attachmenttype is not valid. Attachment file extension doesn't match attachmenttype.";
                                break;
                            case DataTypeEror.InvalidValueFormat:
                                _readyMessage = "Attachment type is not valid.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "ATT.filetype.Cardinality";
                    private const string _businessRule = "ATT.3.Cardinality";
                    private const string _readyMessage = "The file type of the attachment must be specified.";
                    private const string _evprmMessage = "The filetype of the attachment must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class attachmentname
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "ATT.attachmentname.DataType";
                    private const string _businessRule = "ATT.4.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "The attachment name can't be longer than 2000 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "ATT.attachmentname.Cardinality";
                    private const string _businessRule = "ATT.4.Cardinality";
                    private const string _readyMessage = "The name of the attachment given by the sender should be specified.";
                    private const string _evprmMessage = "The attachmentname given by the sender should be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class languagecode
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "ATT.languagecode.DataType";
                    private const string _businessRule = "ATT.6.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueIsMissing:
                                _readyMessage = "Attachment language code is missing abbreviation value.";
                                break;
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "Attachment language code can't be longer than 2 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "ATT.languagecode.Cardinality";
                    private const string _businessRule = "ATT.6.Cardinality";
                    private const string _readyMessage = "Language code must be specified.";
                    private const string _evprmMessage = "The languagecode must be specified using the two letter language code in the published list.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public static class attachmentversion
            {
                public class DataType : IXevprmValidationRule
                {
                    private const string _id = "ATT.attachmentversion.DataType";
                    private const string _businessRule = "ATT.7.DataType";
                    private string _readyMessage;
                    private string _evprmMessage;

                    public DataType(DataTypeEror dataTypeEror)
                    {
                        switch (dataTypeEror)
                        {
                            case DataTypeEror.ValueIsMissing:
                                _readyMessage = "Attachment version is missing value.";
                                break;
                            case DataTypeEror.ValueLenghtToBig:
                                _readyMessage = "Attachment version can't be longer than 5 characters.";
                                break;
                        }
                    }

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }

                }

                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "ATT.attachmentversion.Cardinality";
                    private const string _businessRule = "ATT.7.Cardinality";
                    private const string _readyMessage = "The version of the PPI/PSI attachment must be specified.";
                    private const string _evprmMessage = "The attachmentversion of the PPI/PSI attachment must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

            public class attachmentversiondate
            {
                public class Cardinality : IXevprmValidationRule
                {
                    private const string _id = "ATT.attachmentversiondate.Cardinality";
                    private const string _businessRule = "ATT.8.Cardinality";
                    private const string _readyMessage = "The date of the last update of the PPI document must be specified.";
                    private const string _evprmMessage = "The attachmentversiondate must be specified.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }

                public class BR3 : IXevprmValidationRule
                {
                    private const string _id = "ATT.attachmentversiondate.BR3";
                    private const string _businessRule = "ATT.8.BR.3";
                    private const string _readyMessage = "The date of the last update of the PPI document must not occur later than the current date/time (GMT) + twelve hours.";
                    private const string _evprmMessage = "The attachmentversiondate must not occur later than the current date/time (GMT) + twelve hours.";

                    public static string RuleId { get { return _id; } }

                    public string Id { get { return _id; } }
                    public string BusinessRule { get { return _businessRule; } }
                    public string ReadyMessage { get { return _readyMessage; } }
                    public string EvprmMessage { get { return _evprmMessage; } }
                }
            }

        }
    }
}
