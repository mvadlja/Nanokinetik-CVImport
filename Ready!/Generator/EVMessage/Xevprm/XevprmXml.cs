using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using CommonComponents;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;
using Ready.Model;
using System.Data;
using System.Globalization;
using System.IO;

namespace EVMessage.Xevprm
{
    public partial class XevprmXml
    {
        #region Declarations

        private static class NavigateUrl
        {
            private const string AuthorisedProductLink = "~/Views/AuthorisedProductView/Form.aspx?EntityContext=AuthorisedProduct&Action=Edit&idAuthProd={0}&XevprmValidation={1}";
            private const string OrganizationLink = "~/Views/OrganizationView/Form.aspx?EntityContext=Organisation&Action=Edit&idOrg={0}&idAuthProdXevprm={1}&XevprmValidation={2}";
            private const string PersonLink = "~/Views/PersonView/Form.aspx?EntityContext=Person&Action=Edit&idPerson={0}&idAuthProdXevprm={1}&XevprmValidation={2}";
            private const string DocumentLink = "~/Views/DocumentView/Form.aspx?EntityContext=Document&Action=Edit&idDoc={0}&XevprmValidation={1}";
            private const string AuthorisedProductDocumentLink = "~/Views/DocumentView/Form.aspx?EntityContext=AuthorisedProduct&Action=Edit&idDoc={0}&idAuthProd={1}&XevprmValidation={2}";
            private const string AuthorisedProductDocumentsLink = "~/Views/DocumentView/List.aspx?EntityContext=AuthorisedProduct&idAuthProd={0}&XevprmValidation={1}";
            private const string PharmaceuticalProductLink = "~/Views/PharmaceuticalProductView/Form.aspx?EntityContext=PharmaceuticalProduct&Action=Edit&idPharmProd={0}&XevprmValidation={1}";
            private const string ProductLink = "~/Views/ProductView/Form.aspx?EntityContext=Product&Action=Edit&idProd={0}&idAuthProdXevprm={1}&XevprmValidation={2}";

            public static string AuthorisedProduct(int? id, XevprmOperationType operationType)
            {
                return string.Format(AuthorisedProductLink, id, (int)operationType);
            }

            public static string Organization(int? id, int? idAuthProd, XevprmOperationType operationType)
            {
                return string.Format(OrganizationLink, id, idAuthProd, (int)operationType);
            }

            public static string Person(int? id, int? idAuthProd, XevprmOperationType operationType)
            {
                return string.Format(PersonLink, id, idAuthProd, (int)operationType);
            }

            public static string Document(int? id, XevprmOperationType operationType)
            {
                return string.Format(DocumentLink, id, (int)operationType);
            }

            public static string AuthorisedProductDocument(int? id, int? idAuthProd, XevprmOperationType operationType)
            {
                return string.Format(AuthorisedProductDocumentLink, id, idAuthProd, (int)operationType);
            }

            public static string AuthorisedProductDocuments(int? id, XevprmOperationType operationType)
            {
                return string.Format(AuthorisedProductDocumentsLink, id, (int)operationType);
            }

            public static string PharmaceuticalProduct(int? id, XevprmOperationType operationType)
            {
                return string.Format(PharmaceuticalProductLink, id, (int)operationType);
            }

            public static string Product(int? id, int? idAuthProd, XevprmOperationType operationType)
            {
                return string.Format(ProductLink, id, idAuthProd, (int)operationType);
            }
        }

        private evprm _evprm;

        private List<XevprmValidationException> _validationExceptions;

        private List<Exception> _exceptions;

        #region Ready model DAL

        private readonly IXevprm_message_PKOperations _xevprmMessageOperations;
        private readonly IXevprm_ap_details_PKOperations _xevprmApDetailsOperations;
        private readonly IAuthorisedProductOperations _authorisedProductOperations;

        #endregion

        #endregion

        #region Properties

        public evprm Evprm
        {
            get { return _evprm; }
            set { _evprm = value; }
        }

        public List<XevprmValidationException> ValidationExceptions
        {
            get { return _validationExceptions; }
            set { _validationExceptions = value; }
        }

        public List<Exception> Exceptions
        {
            get { return _exceptions; }
            set { _exceptions = value; }
        }

        public string Xml
        {
            get
            {
                string evprmXML = _evprm.ToString();
                evprmXML = evprmXML.Replace("<evprm xmlns=\"http://eudravigilance.ema.europa.eu/schema/emaxevmpd\">",
                        "<evprm xmlns=\"http://eudravigilance.ema.europa.eu/schema/emaxevmpd\" xmlns:ssi=\"http://eudravigilance.ema.europa.eu/schema/emaxevmpd_ssi\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://eudravigilance.ema.europa.eu/schema/emaxevmpd http://eudravigilance.ema.europa.eu/schema/emaxevmpd.xsd\">");
                return evprmXML;
            }
        }

        public bool IsValid
        {
            get { return _exceptions.Count == 0 && _validationExceptions.Count == 0; }
        }

        public bool HasExceptions
        {
            get { return _exceptions.Count > 0; }
        }

        public bool HasValidationExceptions
        {
            get { return _validationExceptions.Count > 0; }
        }

        public bool HasEvprmValidationExceptions
        {
            get { return _validationExceptions.Any(ex => ex.ExceptionDestination == ValidationExceptionDestination.Evprm || ex.ExceptionDestination == ValidationExceptionDestination.ReadyAndEvprm); }
        }

        public bool HasReadyValidationExceptions
        {
            get { return _validationExceptions.Any(ex => ex.ExceptionDestination == ValidationExceptionDestination.Ready || ex.ExceptionDestination == ValidationExceptionDestination.ReadyAndEvprm); }
        }

        public List<XevprmValidationException> EvprmValidationExceptions
        {
            get { return _validationExceptions.Where(ex => ex.ExceptionDestination == ValidationExceptionDestination.Evprm || ex.ExceptionDestination == ValidationExceptionDestination.ReadyAndEvprm).ToList(); }
        }

        public List<XevprmValidationException> ReadyValidationExceptions
        {
            get { return _validationExceptions.Where(ex => ex.ExceptionDestination == ValidationExceptionDestination.Ready || ex.ExceptionDestination == ValidationExceptionDestination.ReadyAndEvprm).ToList(); }
        }

        #endregion

        #region Constructors

        public XevprmXml()
        {
            _evprm = new evprm();
            _validationExceptions = new List<XevprmValidationException>();
            _exceptions = new List<Exception>();

            #region Ready model DAL

            _xevprmMessageOperations = new Xevprm_message_PKDAL();
            _xevprmApDetailsOperations = new Xevprm_ap_details_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();

            #endregion
        }
        #endregion

        #region Methods

        private void ResetXevprmXml()
        {
            _exceptions.Clear();
            _validationExceptions.Clear();
            _evprm = new evprm();
        }

        private void ClearExceptions()
        {
            _exceptions.Clear();
            _validationExceptions.Clear();
        }

        public OperationResult<object> ConstructXevprm(int? xevprmMessagePk)
        {
            ResetXevprmXml();

            if (xevprmMessagePk == null)
            {
                return new OperationResult<object>(false, "Xevprm message Pk is null.");
            }

            var xevprmMessage = _xevprmMessageOperations.GetEntity(xevprmMessagePk);

            if (xevprmMessage == null)
            {
                return new OperationResult<object>(false, string.Format("Xevprm message with Pk = '{0}' can't be loaded from database.", xevprmMessagePk));
            }

            return ConstructXevprm(xevprmMessage);
        }

        public OperationResult<object> ConstructXevprm(Xevprm_message_PK xevprmMessage)
        {
            if (xevprmMessage == null)
            {
                return new OperationResult<object>(false, "Xevprm message is null.");
            }

            if (xevprmMessage.xevprm_message_PK == null || _xevprmMessageOperations.GetEntity(xevprmMessage.xevprm_message_PK) == null)
            {
                return new OperationResult<object>(false, string.Format("Xevprm message with Pk = '{0}' can't be found in database.", xevprmMessage.xevprm_message_PK));
            }

            try
            {
                //Construct ichicsrmessageheader
                ConstructMessageHeader(xevprmMessage.message_number);

                //Add assigned authorised products to evprm
                List<Xevprm_ap_details_PK> xevprmApDetailsList = _xevprmApDetailsOperations.GetEntitiesByXevprm(xevprmMessage.xevprm_message_PK);

                if (xevprmApDetailsList != null && xevprmApDetailsList.Count > 0)
                {
                    int apDetailsIndex = 0;

                    foreach (Xevprm_ap_details_PK xevprmApDetails in xevprmApDetailsList)
                    {
                        apDetailsIndex++;

                        if (apDetailsIndex > 1)
                        {
                            _exceptions.Add(new Exception("More than one authorised product is assigned to xevprm message. Only first authorised product is added to evprm."));
                        }

                        var authorisedProduct = xevprmApDetails.ap_FK.HasValue ? _authorisedProductOperations.GetEntity(xevprmApDetails.ap_FK) : null;
                        if (authorisedProduct != null)
                        {
                            var operationResult = AddAuthorisedProduct(authorisedProduct, xevprmApDetails.OperationType);
                            UpdateExceptions(operationResult);
                        }
                        else
                        {
                            _exceptions.Add(new Exception("Authorised product is null."));
                        }
                    }

                    if (_evprm.authorisedproducts != null && _evprm.authorisedproducts.authorisedproduct != null && _evprm.authorisedproducts.authorisedproduct.Count == xevprmApDetailsList.Count)
                    {
                        if (_exceptions.Count == 0 && _validationExceptions.Count == 0)
                        {
                            return new OperationResult<object>(true, "Evprm succesfully created.");
                        }
                        else if (_validationExceptions.Count > 0 && _exceptions.Count > 0)
                        {
                            return new OperationResult<object>(false, "Evprm business rules validation failed and some unexpected errors occured.");
                        }
                        else if (_validationExceptions.Count > 0)
                        {
                            return new OperationResult<object>(false, "Evprm business rules validation failed.");
                        }
                        else if (_exceptions.Count > 0)
                        {
                            return new OperationResult<object>(false, "Some unexpected errors occured.");
                        }
                    }
                    else
                    {
                        return new OperationResult<object>(false, "Some authorised products are not added to evprm.");
                    }
                }
                else
                {
                    return new OperationResult<object>(false, "Xevprm message doesn't contain any entities.");
                }
            }
            catch (Exception ex)
            {
                _exceptions.Add(ex);
                return new OperationResult<object>(ex, "Constructing evprm failed.");
            }

            return new OperationResult<object>(false);
        }

        #region Message Header

        public void ConstructMessageHeader(string messageNumber, string senderID = null)
        {
            var messageHeader = new ichicsrmessageheaderType
            {
                messageformatversion = "2",
                messageformatrelease = "0",
                messagetype = "XEVPRM",
                messagedate = DateTime.Now.AddDays(-1).ToString("yyyyMMddHHmmss"),
                messagedateformat = "204",
                messagereceiveridentifier = System.Configuration.ConfigurationManager.AppSettings["MessageReceiverID"]
            };

            if (!string.IsNullOrWhiteSpace(senderID))
            {
                messageHeader.messagesenderidentifier = senderID;
            }
            else
            {
                messageHeader.messagesenderidentifier = System.Configuration.ConfigurationManager.AppSettings["MessageSenderID"];
            }

            messageHeader.messagenumb = messageNumber;

            _evprm.ichicsrmessageheader = messageHeader;
        }

        #endregion

        #region Authorised product

        private ValidationResult AddAuthorisedProduct(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();

            if (dbAuthorisedProduct == null)
            {
                const string message = "AddAuthorisedProduct: Authorised product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            if (operationType.NotIn(XevprmOperationType.Insert, XevprmOperationType.Update, XevprmOperationType.Variation, XevprmOperationType.Nullify, XevprmOperationType.Withdraw))
            {
                _exceptions.Add(new Exception(string.Format("Operation type = '{0}' is not supported for authorised product with ID ='{1}'.", operationType, dbAuthorisedProduct.ap_PK)));
            }

            int authProdIndex = 0;

            if (_evprm.authorisedproducts != null && _evprm.authorisedproducts.authorisedproduct != null && _evprm.authorisedproducts.authorisedproduct.Count > 0)
            {
                authProdIndex = _evprm.authorisedproducts.authorisedproduct.Count;
            }

            string evprmAuthProdLocation = string.Format("authorisedproducts.authorisedproduct[{0}]", authProdIndex);

            authorisedproductType evprmAuthorisedProduct;
            var evprmAttachments =  _evprm != null && _evprm.attachments != null ? _evprm.attachments : new evprm.attachmentsLocalType();

            var validationResult = ConstructAuthorisedProduct(dbAuthorisedProduct, operationType, out evprmAuthorisedProduct, ref evprmAttachments, evprmAuthProdLocation);
            UpdateExceptions(validationResult, ref validationExceptions, ref exceptions);

            if (evprmAuthorisedProduct != null)
            {
                if (_evprm.authorisedproducts == null)
                {
                    _evprm.authorisedproducts = new evprm.authorisedproductsLocalType();
                }

                if (_evprm.authorisedproducts.authorisedproduct == null)
                {
                    _evprm.authorisedproducts.authorisedproduct = new List<authorisedproductType>();
                }

                _evprm.authorisedproducts.authorisedproduct.Add(evprmAuthorisedProduct);

                //Set Sender ID
                IOrganization_PKOperations organizationOperations = new Organization_PKDAL();
                var dbLicenceHolder = dbAuthorisedProduct.organizationmahcode_FK.HasValue ? organizationOperations.GetEntity(dbAuthorisedProduct.organizationmahcode_FK) : null;

                if (dbLicenceHolder != null && !string.IsNullOrWhiteSpace(dbLicenceHolder.organizationsenderid_EMEA) && 
                    dbLicenceHolder.organizationsenderid_EMEA.Trim().Length >= 3 && dbLicenceHolder.organizationsenderid_EMEA.Trim().Length <= 60)
                {
                    if (_evprm != null && _evprm.ichicsrmessageheader != null)
                    {
                        _evprm.ichicsrmessageheader.messagesenderidentifier = dbLicenceHolder.organizationsenderid_EMEA.Trim();
                    }
                }
            }

            if (evprmAttachments != null && evprmAttachments.attachment.Count > 0)
            {
                _evprm.attachments = evprmAttachments;
            }

            return GetValidationResult(ref validationExceptions, ref exceptions);
        }

        private static bool IsValidEmail(string emailAddress)
        {
            if (string.IsNullOrWhiteSpace(emailAddress))
                return false;

            try
            {
                var m = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }

            //const string reEmail = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            //var r = new System.Text.RegularExpressions.Regex(reEmail);
            //return r.IsMatch(emailAddress);
        }


        private static bool IsValidInt(string s)
        {
            bool isValid = false;

            if (!String.IsNullOrEmpty(s))
            {
                int temp = 0;
                if (int.TryParse(s, out temp)) isValid = true;
            }

            return isValid;
        }

        private void UpdateExceptions(ValidationResult validationResult)
        {
            if (!validationResult.IsSuccess)
            {
                if (validationResult.XevprmValidationExceptions.Any())
                {
                    _validationExceptions.AddRange(validationResult.XevprmValidationExceptions);
                }

                if (validationResult.Exceptions.Any())
                {
                    _exceptions.AddRange(validationResult.Exceptions);
                }
            }
        }

        private static void UpdateExceptions(ValidationResult validationResult, ref List<XevprmValidationException> validationExceptions, ref List<Exception> exceptions)
        {
            if (!validationResult.IsSuccess)
            {
                if (validationResult.XevprmValidationExceptions.Any())
                {
                    validationExceptions.AddRange(validationResult.XevprmValidationExceptions);
                }

                if (validationResult.Exceptions.Any())
                {
                    exceptions.AddRange(validationResult.Exceptions);
                }
            }
        }

        private static ValidationResult GetValidationResult(ref List<XevprmValidationException> validationExceptions, ref List<Exception> exceptions)
        {
            if (exceptions.Any())
            {
                return new ValidationResult(false, "Error at validation!", validationExceptions, exceptions);
            }

            if (validationExceptions.Any())
            {
                return new ValidationResult(false, "Validation failed!", validationExceptions, exceptions);
            }

            return new ValidationResult(true, "Validation successful!");
        }

        private static ValidationResult GetValidationResult(ref List<XevprmValidationException> validationExceptions, ref List<Exception> exceptions, ref Tree<XevprmValidationTreeNode> xevprmValidationTree)
        {
            if (exceptions.Any())
            {
                return new ValidationResult(false, "Error at validation!", validationExceptions, exceptions, xevprmValidationTree);
            }

            if (validationExceptions.Any())
            {
                return new ValidationResult(false, "Validation failed!", validationExceptions, exceptions, xevprmValidationTree);
            }

            return new ValidationResult(true, "Validation successful!");
        }

        #endregion

        public static OperationResult<string> ConstructXevprmXml(int? xevprmMessagePk)
        {
            var xevprmXml = new XevprmXml();

            var operationResult = xevprmXml.ConstructXevprm(xevprmMessagePk);

            if (xevprmXml.Exceptions.Count > 0)
                return new OperationResult<string>(xevprmXml.Exceptions[0], operationResult.Description, xevprmXml.Xml);

            return new OperationResult<string>(operationResult.IsSuccess, operationResult.Description, xevprmXml.Xml);
        }

        public static OperationResult<string> ConstructXevprmXml(Xevprm_message_PK xevprmMessage)
        {
            var xevprmXml = new XevprmXml();

            var operationResult = xevprmXml.ConstructXevprm(xevprmMessage);

            if (xevprmXml.Exceptions.Count > 0)
                return new OperationResult<string>(xevprmXml.Exceptions[0], operationResult.Description, xevprmXml.Xml);

            return new OperationResult<string>(operationResult.IsSuccess, operationResult.Description, xevprmXml.Xml);
        }

        public static IEnumerable<ValidationResult> ValidateXevprm(int xevprmMessagePk, XevprmEntityType xevprmEntityType = XevprmEntityType.NULL, int? xevprmEntityPk = null)
        {
            IXevprm_entity_details_mn_PKOperations xevprmEntityDetailsMnOperations = new Xevprm_entity_details_mn_PKDAL();
            var xevprmEntityDetailMnList = xevprmEntityDetailsMnOperations.GetEntitiesByXevprm(xevprmMessagePk);

            if (xevprmEntityType != XevprmEntityType.NULL && xevprmEntityPk.HasValue)
            {
                xevprmEntityDetailMnList = xevprmEntityDetailMnList.Where(x => x.xevprm_entity_type_FK == (int)xevprmEntityType).ToList();
            }

            foreach (var xevprmEntityDetailMn in xevprmEntityDetailMnList)
            {
                switch (xevprmEntityDetailMn.XevprmEntityType)
                {
                    case XevprmEntityType.AuthorisedProduct:
                        {
                            IAuthorisedProductOperations authorisedProductOperations = new AuthorisedProductDAL();
                            var authorisedProduct = xevprmEntityDetailMn.xevprm_entity_FK.HasValue ? authorisedProductOperations.GetEntity(xevprmEntityDetailMn.xevprm_entity_FK) : null;
                            if (authorisedProduct != null)
                            {
                                yield return ValidateAuthorisedProduct(authorisedProduct, xevprmEntityDetailMn.XevprmOperationType);
                            }
                            else if (!xevprmEntityDetailMn.xevprm_entity_FK.HasValue)
                            {
                                IXevprm_ap_details_PKOperations xevprmApDetailsPkOperations = new Xevprm_ap_details_PKDAL();
                                var xevprmApDetail = xevprmApDetailsPkOperations.GetEntity(xevprmEntityDetailMn.xevprm_entity_details_FK);

                                if (xevprmApDetail != null)
                                {
                                    authorisedProduct = xevprmApDetail.ap_FK.HasValue ? authorisedProductOperations.GetEntity(xevprmApDetail.ap_FK) : null;
                                    if (authorisedProduct != null)
                                    {
                                        yield return ValidateAuthorisedProduct(authorisedProduct, xevprmEntityDetailMn.XevprmOperationType);
                                    }
                                }
                            }
                        }
                        break;

                    case XevprmEntityType.Attachment:
                        continue;
                        {
                            IAttachment_PKOperations attachmentOperations = new Attachment_PKDAL();

                            var attachment = xevprmEntityDetailMn.xevprm_entity_FK.HasValue ? attachmentOperations.GetEntity(xevprmEntityDetailMn.xevprm_entity_FK) : null;

                            if (attachment != null)
                            {
                                yield return ValidateAttachment(attachment, xevprmEntityDetailMn.XevprmOperationType);
                            }
                            else if (!xevprmEntityDetailMn.xevprm_entity_FK.HasValue)
                            {
                                IXevprm_attachment_details_PKOperations xevprmAttachmentDetailsOperations = new Xevprm_attachment_details_PKDAL();
                                var xevprmAttachmentDetail = xevprmAttachmentDetailsOperations.GetEntity(xevprmEntityDetailMn.xevprm_entity_details_FK);

                                if (xevprmAttachmentDetail != null)
                                {
                                    attachment = xevprmAttachmentDetail.attachment_FK.HasValue ? attachmentOperations.GetEntity(xevprmAttachmentDetail.attachment_FK) : null;
                                    if (attachment != null)
                                    {
                                        yield return ValidateAttachment(attachment, xevprmEntityDetailMn.XevprmOperationType);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }

        #endregion
    }
}
