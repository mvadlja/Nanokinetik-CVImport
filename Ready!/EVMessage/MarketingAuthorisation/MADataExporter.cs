using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ready.Model;
using EVMessage.MarketingAuthorisation.Schema;
using System.Globalization;

namespace EVMessage.MarketingAuthorisation
{
    public partial class MADataExporter
    {
        #region Declarations

        private string _EMASenderID = "SANDOZEVMPDTEST";

        private MADataExporterModeType _maDataExporterMode;

        private SaveOptions _maSaveOptions;

        private MAReadyStruct _maReadyStruct;

        private List<ValidationException> _validationExceptions;
        private List<Exception> _exceptions;
        private List<Exception> _saveExceptions;

        private int? _maPK;
        private int? _responisbleUserPK;

        private ControlledVocabulary _cv;

        #region Ready model DAL

        private IAuthorisedProductOperations _authorisedProductOperations;
        private IProduct_PKOperations _product_PKOperations;

        private IOrganization_PKOperations _organization_PKOperations;
        private IOrganization_in_role_Operations _organization_in_role_Operations;
        private IRole_org_PKOperations _role_org_PKOperations;
        private ICountry_PKOperations _country_PKOperations;
        private IType_PKOperations _type_PKOperations;

        private IDocument_PKOperations _document_PKOperations;
        private IAp_document_mn_PKOperations _ap_document_mn_PKOperations;
        private IDocument_language_mn_PKOperations _document_language_mn_PKOperations;
        private ILanguagecode_PKOperations _languagecode_PKOperations;
        private IAttachment_PKOperations _attachment_PKOperations;
        private IMa_attachment_PKOperations _ma_attachment_PKOperations;

        private IAtc_PKOperations _atc_PKOperations;
        private IProduct_atc_mn_PKOperations _product_atc_mn_PKOperations;

        private IMeddra_pkOperations _meddra_pkOperations;
        private IMeddra_ap_mn_PKOperations _meddra_ap_mn_PKOperations;

        private IPharmaceutical_product_PKOperations _pharmaceutical_product_PKOperations;
        private IPharmaceutical_form_PKOperations _pharmaceutical_form_PKOperations;
        private IProduct_mn_PKOperations _product_mn_PKOperations;

        private IActiveingredient_PKOperations _activeingredient_PKOperations;
        private IExcipient_PKOperations _excipient_PKOperations;
        private IAdjuvant_PKOperations _adjuvant_PKOperations;
        private ISubstance_PKOperations _substance_PKOperations;
        private ISsi__cont_voc_PKOperations _ssi__cont_voc_PKOperations;

        private IAdminroute_PKOperations _adminroute_PKOperations;
        private IPp_ar_mn_PKOperations _pp_ar_mn_PKOperations;

        private IMedicaldevice_PKOperations _medicaldevice_PKOperations;
        private IPp_md_mn_PKOperations _pp_md_mn_PKOperations;

        private IPerson_PKOperations _person_PKOperations;
        private IPerson_role_PKOperations _person_role_PKOperations;
        private IPerson_in_role_PKOperations _person_in_role_PKOperations;
        private IQppv_code_PKOperations _qppv_code_PKOperations;

        private IMa_ma_entity_mn_PKOperations _ma_ma_entity_mn_PKOperations;
        private IXevprm_message_PKOperations _xevprmMessageOperations;

        #endregion

        #endregion

        #region Properties

        public string EMASenderID
        {
            get { return _EMASenderID; }
            set { _EMASenderID = value; }
        }

        public MADataExporterModeType MADataExporterMode
        {
            get { return _maDataExporterMode; }
            set { _maDataExporterMode = value; }
        }

        public SaveOptions MASaveOptions
        {
            get { return _maSaveOptions; }
            set { _maSaveOptions = value; }
        }

        public List<ValidationException> ValidationExceptions
        {
            get { return _validationExceptions; }
            set { _validationExceptions = value; }
        }

        public List<Exception> Exceptions
        {
            get { return _exceptions; }
            set { _exceptions = value; }
        }

        public List<Exception> SaveExceptions
        {
            get { return _saveExceptions; }
            set { _saveExceptions = value; }
        }

        public int? ResponisbleUserPK
        {
            get { return _responisbleUserPK; }
            set { _responisbleUserPK = value; }
        }

        public List<AuthorisedProduct> InsertedAuthorisedProducts
        {
            get
            {
                var dbAuthorisedProductList = new List<AuthorisedProduct>();

                if (_maReadyStruct.AuthorisedProductStructList != null && _maReadyStruct.AuthorisedProductStructList.Count > 0)
                {
                    _maReadyStruct.AuthorisedProductStructList.ForEach(delegate(AuthorisedProductStruct apStruct)
                    {
                        if (apStruct.AuthorisedProduct != null && apStruct.AuthorisedProduct.ap_PK != null && apStruct.OperationType == XevprmOperationType.Insert)
                        {
                            dbAuthorisedProductList.Add(apStruct.AuthorisedProduct);
                        }
                    });
                }

                return dbAuthorisedProductList;
            }
        }

        public List<Tuple<AuthorisedProduct, XevprmOperationType>> ExportedAuthorisedProducts
        {
            get
            {
                var dbAuthorisedProductDictionary = new List<Tuple<AuthorisedProduct, XevprmOperationType>>();

                if (_maReadyStruct.AuthorisedProductStructList != null && _maReadyStruct.AuthorisedProductStructList.Count > 0)
                {
                    _maReadyStruct.AuthorisedProductStructList.ForEach(delegate(AuthorisedProductStruct apStruct)
                    {
                        if (apStruct.AuthorisedProduct != null && apStruct.AuthorisedProduct.ap_PK != null)
                        {
                            dbAuthorisedProductDictionary.Add(new Tuple<AuthorisedProduct, XevprmOperationType>(apStruct.AuthorisedProduct, apStruct.OperationType));
                        }
                    });
                }

                return dbAuthorisedProductDictionary;
            }
        }

        public List<AuthorisedProduct> GetExportedAuthorisedProducts(XevprmOperationType operationType)
        {
            var dbAuthorisedProductList = new List<AuthorisedProduct>();

            if (_maReadyStruct.AuthorisedProductStructList != null && _maReadyStruct.AuthorisedProductStructList.Count > 0)
            {
                _maReadyStruct.AuthorisedProductStructList.ForEach(delegate(AuthorisedProductStruct apStruct)
                {
                    if (apStruct.AuthorisedProduct != null && apStruct.AuthorisedProduct.ap_PK != null && apStruct.OperationType == operationType)
                    {
                        dbAuthorisedProductList.Add(apStruct.AuthorisedProduct);
                    }
                });
            }

            return dbAuthorisedProductList;
        }

        #endregion

        #region Constructors

        public MADataExporter()
        {
            _validationExceptions = new List<ValidationException>();
            _exceptions = new List<Exception>();
            _saveExceptions = new List<Exception>();

            _maReadyStruct = new MAReadyStruct();

            _maDataExporterMode = MADataExporterModeType.ValidateAndExportToDB;

            _maSaveOptions = new SaveOptions
            {
                IgnoreValidationExceptions = false,
                IgnoreErrorsInValidationProcess = false,
                RollbackAllAtSaveException = true,
                PopulateMAEntityMNTable = true,
                UpdateExistingEntities = true
            };

            _maPK = null;
            _responisbleUserPK = null;

            InitDAL();

            _cv = ControlledVocabulary.Instance;
        }

        #endregion

        #region Methods

        private void InitDAL()
        {
            _authorisedProductOperations = new AuthorisedProductDAL();
            _product_PKOperations = new Product_PKDAL();

            _organization_PKOperations = new Organization_PKDAL();
            _organization_in_role_Operations = new Organization_in_role_DAL();
            _role_org_PKOperations = new Role_org_PKDAL();
            _country_PKOperations = new Country_PKDAL();
            _type_PKOperations = new Type_PKDAL();

            _document_PKOperations = new Document_PKDAL();
            _ap_document_mn_PKOperations = new Ap_document_mn_PKDAL();
            _document_language_mn_PKOperations = new Document_language_mn_PKDAL();
            _languagecode_PKOperations = new Languagecode_PKDAL();
            _attachment_PKOperations = new Attachment_PKDAL();
            _ma_attachment_PKOperations = new Ma_attachment_PKDAL();

            _atc_PKOperations = new Atc_PKDAL();
            _product_atc_mn_PKOperations = new Product_atc_mn_PKDAL();

            _meddra_pkOperations = new Meddra_pkDAL();
            _meddra_ap_mn_PKOperations = new Meddra_ap_mn_PKDAL();

            _pharmaceutical_product_PKOperations = new Pharmaceutical_product_PKDAL();
            _pharmaceutical_form_PKOperations = new Pharmaceutical_form_PKDAL();
            _product_mn_PKOperations = new Product_mn_PKDAL();

            _activeingredient_PKOperations = new Activeingredient_PKDAL();
            _excipient_PKOperations = new Excipient_PKDAL();
            _adjuvant_PKOperations = new Adjuvant_PKDAL();
            _substance_PKOperations = new Substance_PKDAL();
            _ssi__cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();

            _adminroute_PKOperations = new Adminroute_PKDAL();
            _pp_ar_mn_PKOperations = new Pp_ar_mn_PKDAL();

            _medicaldevice_PKOperations = new Medicaldevice_PKDAL();
            _pp_md_mn_PKOperations = new Pp_md_mn_PKDAL();

            _person_PKOperations = new Person_PKDAL();
            _person_role_PKOperations = new Person_role_PKDAL();
            _person_in_role_PKOperations = new Person_in_role_PKDAL();
            _qppv_code_PKOperations = new Qppv_code_PKDAL();

            _ma_ma_entity_mn_PKOperations = new Ma_ma_entity_mn_PKDAL();
            _xevprmMessageOperations = new Xevprm_message_PKDAL();
        }

        public bool ExportMAToDB(string maXML, int? maPK = null)
        {
            ResetMADataExporter();

            if (string.IsNullOrWhiteSpace(maXML))
            {
                _exceptions.Add(new Exception("MA XML is null or empty."));
                return false;
            }

            _maPK = maPK;
            if (maPK == null)
            {
                _maSaveOptions.PopulateMAEntityMNTable = false;
            }

            marketingauthorisation ma = null;
            try
            {
                XRootNamespace xRootNamespace = XRootNamespace.Parse(maXML);
                ma = xRootNamespace.marketingauthorisation;
            }
            catch (Exception ex)
            {
                _exceptions.Add(ex);
                return false;
            }

            return ExportMAToDB(ma);
        }

        public bool ExportMAToDB(marketingauthorisation ma, int? maPK = null)
        {
            ResetMADataExporter();

            if (ma == null)
            {
                _exceptions.Add(new Exception("MA is null or empty."));
                return false;
            }

            _maPK = maPK;
            if (maPK == null)
            {
                _maSaveOptions.PopulateMAEntityMNTable = false;
            }

            try
            {

                #region Attachments

                if (ma.attachments != null && ma.attachments.attachment != null && ma.attachments.attachment.Count > 0)
                {
                    List<attachmentType> maAttachmentList = ma.attachments.attachment.ToList();

                    for (int maAttachIndex = 0; maAttachIndex < maAttachmentList.Count; maAttachIndex++)
                    {
                        #region Limitations

                        //if (maAttachIndex > 0)
                        //{
                        //    string message = "Attachments section contains more than one attachment. Authorised product may have only one attachment referenced by ppiattachment.";
                        //    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                        //    exception.AddDescription("attachments", string.Format("attachment[{0}]", maAttachIndex), null);
                        //    _validationExceptions.Add(exception);

                        //    continue;
                        //}

                        #endregion


                        string maAttachLocation = string.Format("attachments.attachment[{0}]", maAttachIndex);

                        attachmentType maAttachment = maAttachmentList[maAttachIndex];

                        var dbAttachment = new Attachment_PK();
                        var dbDocument = new Document_PK();

                        var documentStruct = new DocumentStruct();
                        documentStruct.Attachment = dbAttachment;
                        documentStruct.Document = dbDocument;

                        dbDocument.person_FK = _responisbleUserPK;
                        dbDocument.EDMSDocument = false; 

                        if (!string.IsNullOrWhiteSpace(maAttachment.filename))
                        {
                            dbAttachment.attachmentname = maAttachment.filename.Trim();

                            Ma_attachment_PK maAttachmentInDb = _ma_attachment_PKOperations.GetEntityByFileName(maAttachment.filename.Trim());
                            if (maAttachmentInDb != null)
                            {
                                dbAttachment.disk_file = maAttachmentInDb.file_data;
                            }
                            else
                            {
                                string message = "File can't be found.";
                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                exception.AddDescription(maAttachLocation, "filename", maAttachment.filename.Trim());
                                _validationExceptions.Add(exception);
                            }
                        }
                        else
                        {
                            string message = "Invalid value. Value must be specified.";
                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                            exception.AddDescription(maAttachLocation, "filename", string.Empty);
                            _validationExceptions.Add(exception);
                        }

                        dbDocument.name = maAttachment.attachmentname;

                        //Attachment file type
                        Type_PK attachmentFileTypeInDb = _cv.AttachmentFileTypeList.Find(attachmentFileType => attachmentFileType.type.Trim() == maAttachment.filetype.ToString());
                        if (attachmentFileTypeInDb != null)
                        {
                            dbDocument.attachment_type_FK = attachmentFileTypeInDb.type_PK;
                        }
                        else if (maAttachment.filetype.NotIn(1, 2, 3, 4, 5))
                        {
                            string message = "Value is not in Controlled Vocabullary.";
                            var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                            exception.AddDescription(maAttachLocation, "filetype", maAttachment.filetype.ToString());
                            _validationExceptions.Add(exception);
                        }
                        else
                        {
                            _exceptions.Add(new Exception(string.Format("Type for attachment file type = '{0}' can't be found in database.", maAttachment.filetype)));
                        }

                        //Attachment version number
                        if (maAttachment.attachmentversion != null && maAttachment.attachmentversion.Trim() == "1")
                        {
                            Type_PK attachmentVersionNumberInDb = _cv.AttachmentVersionNumberList.Find(attachmentVersionNumber => attachmentVersionNumber.name.Trim() == maAttachment.attachmentversion.Trim());
                            if (attachmentVersionNumberInDb != null)
                            {
                                dbDocument.version_number = attachmentVersionNumberInDb.type_PK;
                            }
                            else
                            {
                                _exceptions.Add(new Exception("Type for attachment version = '1' can't be found in database."));
                            }
                        }
                        else
                        {
                            string attachmentversion = maAttachment.attachmentversion ?? string.Empty;

                            string message = "Invalid value. Value of attachmentversion should be '1'.";
                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                            exception.AddDescription(maAttachLocation, "attachmentversion", attachmentversion.Trim());
                            _validationExceptions.Add(exception);
                        }

                        //Attachment type
                        if (maAttachment.attachmenttype1 == 1)
                        {
                            Type_PK attachmentTypeInDb = _cv.AttachmentTypeList.Find(attachmentType => attachmentType.ev_code.Trim() == maAttachment.attachmenttype1.ToString());
                            if (attachmentTypeInDb != null)
                            {
                                dbDocument.type_FK = attachmentTypeInDb.type_PK;

                                if (attachmentTypeInDb.ev_code.Trim() == "1") //PPI
                                {
                                    Type_PK dbRegulatoryStatus = _cv.TypeList.Find(type => type.group.Trim() == "PPIRS" && type.name != null && type.name.Trim().ToLower() == "valid");
                                    if (dbRegulatoryStatus != null)
                                    {
                                        dbDocument.regulatory_status = dbRegulatoryStatus.type_PK;
                                    }

                                    Type_PK dbVersionLabel = _cv.TypeList.Find(type => type.group.Trim() == "VL" && type.name != null && type.name.Trim().ToLower() == "n/a");
                                    if (dbVersionLabel != null)
                                    {
                                        dbDocument.version_label = dbVersionLabel.type_PK;
                                    }
                                }
                            }
                            else
                            {
                                _exceptions.Add(new Exception("Type for attachment type = '1' can't be found in database."));
                            }
                        }
                        else if (maAttachment.attachmenttype1 == 2)
                        {
                            string message = "Only PPI attachment is supported. Value of attachmenttype should be '1'.";
                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                            exception.AddDescription(maAttachLocation, "attachmenttype", maAttachment.attachmenttype1.ToString());
                            _validationExceptions.Add(exception);
                        }
                        else if (maAttachment.attachmenttype1 != 1 && maAttachment.attachmenttype1 != 2)
                        {
                            string message = "Value is not in Controlled Vocabulary.";
                            var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                            exception.AddDescription(maAttachLocation, "attachmenttype1", maAttachment.attachmenttype1.ToString());
                            _validationExceptions.Add(exception);
                        }

                        //Attachment version date
                        if (maAttachment.attachmentversiondate != null)
                        {
                            DateTime attachmentVersionDate;

                            if (DateTime.TryParseExact(maAttachment.attachmentversiondate.Trim(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out attachmentVersionDate))
                            {
                                dbDocument.version_date = attachmentVersionDate;
                                dbDocument.version_date_format = "102";
                            }
                            else
                            {
                                string message = "Value is not valid or it is not in valid format.";
                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValueFormat, SeverityType.Error);
                                exception.AddDescription(maAttachLocation, "attachmentversiondate", maAttachment.attachmentversiondate.Trim());
                                _validationExceptions.Add(exception);
                            }
                        }

                        //Attachment language code
                        if (maAttachment.languagecode != null)
                        {
                            Languagecode_PK languageCodeInDb = _cv.LanguageCodeList.Find(languageCode => languageCode.code.Trim() == maAttachment.languagecode.Trim());
                            if (languageCodeInDb != null)
                            {
                                var dbDocumentLanguageCodeMN = new Document_language_mn_PK();
                                dbDocumentLanguageCodeMN.language_FK = languageCodeInDb.languagecode_PK;
                                dbDocumentLanguageCodeMN.document_FK = null;

                                documentStruct.DocumentLanguageCodeMNList.Add(dbDocumentLanguageCodeMN);
                            }
                            else
                            {
                                string message = "Value is not in Controlled Vocabulary.";
                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                exception.AddDescription(maAttachLocation, "languagecode", maAttachment.languagecode.Trim());
                                _validationExceptions.Add(exception);
                            }
                        }

                        //Local number
                        if (!string.IsNullOrWhiteSpace(maAttachment.localnumber))
                        {
                            documentStruct.AttachmentLocalNumber = maAttachment.localnumber.Trim();
                        }

                        //Operation type
                        documentStruct.AttachmentOperationType = (XevprmOperationType)maAttachment.operationtype;

                        #region Limitations

                        var operationtype = (XevprmOperationType)maAttachment.operationtype;

                        //Operation type
                        if (operationtype.NotIn(XevprmOperationType.Insert))
                        {
                            string message = "Only insert operation is supported.";
                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                            exception.AddDescription(maAttachLocation, "operationtype", maAttachment.operationtype.ToString());
                            _validationExceptions.Add(exception);
                        }

                        #endregion

                        _maReadyStruct.DocumentStructList.Add(documentStruct);
                    }
                }

                #endregion

                #region Authorised products

                if (ma.authorisedproducts != null && ma.authorisedproducts.authorisedproduct != null && ma.authorisedproducts.authorisedproduct.Count > 0)
                {
                    for (int maAuthProdIndex = 0; maAuthProdIndex < ma.authorisedproducts.authorisedproduct.Count; maAuthProdIndex++)
                    {
                        #region Limitations

                        if (maAuthProdIndex > 0)
                        {
                            string message = "Authorisedproducts section contains more than one authorisedproduct. Only one authorisedproduct per marketing authorisation is supported.";
                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                            exception.AddDescription("authorisedproducts", string.Format("authorisedproduct[{0}]", maAuthProdIndex), null);
                            _validationExceptions.Add(exception);

                            continue;
                        }

                        #endregion

                        string maAuthProdLocation = string.Format("authorisedproducts.authorisedproduct[{0}]", maAuthProdIndex);

                        authorisedproductType maAuthorisedProduct = ma.authorisedproducts.authorisedproduct[maAuthProdIndex];

                        var dbAuthorisedProduct = new AuthorisedProduct();
                        var dbProduct = new Product_PK();

                        var operationtype = (XevprmOperationType)maAuthorisedProduct.operationtype;

                        #region Operation type limitations

                        if (operationtype.NotIn(XevprmOperationType.Insert, XevprmOperationType.Update, XevprmOperationType.Variation, XevprmOperationType.Nullify, XevprmOperationType.Withdraw))
                        {
                            string message = "Only insert, update, variation, nullify and withdraw operation types are supported.";
                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                            exception.AddDescription(maAuthProdLocation, "operationtype", maAuthorisedProduct.operationtype.ToString());
                            _validationExceptions.Add(exception);

                            continue;
                        }

                        if (operationtype.NotIn(XevprmOperationType.Insert))
                        {
                            if (string.IsNullOrWhiteSpace(maAuthorisedProduct.ev_code))
                            {
                                string message = "If operationtype is not '1' (Insert) ev_code must be present.";
                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                exception.AddDescription(maAuthProdLocation, "ev_code", maAuthorisedProduct.ev_code);
                                _validationExceptions.Add(exception);

                                continue;
                            }
                        }
                        else if (!string.IsNullOrWhiteSpace(maAuthorisedProduct.ev_code))
                        {
                            string message = "Entity already has ev_code therefore it can't be inserted.";
                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                            exception.AddDescription(maAuthProdLocation, "operationtype", maAuthorisedProduct.operationtype.ToString());
                            _validationExceptions.Add(exception);

                            continue;
                        }

                        bool entityExistsInReady = false;

                        if (_maSaveOptions.UpdateExistingEntities && operationtype.NotIn(XevprmOperationType.Insert))
                        {
                            if (!string.IsNullOrWhiteSpace(maAuthorisedProduct.ev_code))
                            {
                                dbAuthorisedProduct = _authorisedProductOperations.GetEntityByEVCode(maAuthorisedProduct.ev_code);

                                if (dbAuthorisedProduct != null)
                                {
                                    entityExistsInReady = true;

                                    bool xevprmMessageInProgress = false;

                                    var latestXevprmMessage = _xevprmMessageOperations.GetLatestEntityByXevprmEntity(dbAuthorisedProduct.ap_PK.Value, (int)XevprmEntityType.AuthorisedProduct);

                                    if (latestXevprmMessage != null)
                                    {
                                        if (!(latestXevprmMessage.XevprmStatus.In(XevprmStatus.MDNReceivedError, XevprmStatus.ACKDelivered) ||
                                            (latestXevprmMessage.XevprmStatus.In(XevprmStatus.ACKReceived) && latestXevprmMessage.ack_type.In(2, 3))))
                                        {
                                            xevprmMessageInProgress = true;
                                        }
                                    }

                                    if (xevprmMessageInProgress)
                                    {
                                        string message = string.Format("Xevprm message with message number '{0}' is still in progress therefore requested operation can not be performed.", latestXevprmMessage.message_number);
                                        var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                        exception.AddDescription(maAuthProdLocation, "operationtype", maAuthorisedProduct.operationtype.ToString());
                                        _validationExceptions.Add(exception);

                                        continue;
                                    }
                                }
                                else
                                {
                                    dbAuthorisedProduct = new AuthorisedProduct();
                                }
                            }

                            if (dbAuthorisedProduct.product_FK.HasValue)
                            {
                                dbProduct = _product_PKOperations.GetEntity(dbAuthorisedProduct.product_FK) ?? new Product_PK();
                            }
                        }

                        bool overwriteExisting = _maSaveOptions.UpdateExistingEntities && entityExistsInReady;

                        #endregion

                        var authorisedProductStruct = new AuthorisedProductStruct();

                        authorisedProductStruct.AuthorisedProduct = dbAuthorisedProduct;

                        var productStruct = new ProductStruct();
                        productStruct.Product = dbProduct;

                        #region Authorised product details

                        dbAuthorisedProduct.responsible_user_person_FK = _responisbleUserPK;
                        dbProduct.responsible_user_person_FK = _responisbleUserPK;

                        dbAuthorisedProduct.article_57_reporting = true;

                        //Comments
                        dbAuthorisedProduct.evprm_comments = maAuthorisedProduct.comments.TrimIfNotNull();

                        //Enquiry email
                        dbAuthorisedProduct.phv_email = maAuthorisedProduct.enquiryemail.TrimIfNotNull();

                        //Enquiry phone
                        dbAuthorisedProduct.phv_phone = maAuthorisedProduct.enquiryphone.TrimIfNotNull();

                        //EV Code
                        dbAuthorisedProduct.ev_code = maAuthorisedProduct.ev_code.TrimIfNotNull();

                        //Local number
                        dbAuthorisedProduct.localnumber = maAuthorisedProduct.localnumber.TrimIfNotNull();
                        authorisedProductStruct.LocalNumber = maAuthorisedProduct.localnumber.TrimIfNotNull();

                        //Operation type
                        authorisedProductStruct.OperationType = (XevprmOperationType)maAuthorisedProduct.operationtype;

                        //Info date
                        if (overwriteExisting)
                        {
                            dbAuthorisedProduct.infodate = null;
                        }
                        if (maAuthorisedProduct.infodate != null)
                        {
                            DateTime infoDate;

                            if (DateTime.TryParseExact(maAuthorisedProduct.infodate.Trim(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out infoDate))
                            {
                                dbAuthorisedProduct.infodate = infoDate;
                            }
                            else
                            {
                                string message = "Value is not valid or it is not in valid format.";
                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValueFormat, SeverityType.Error);
                                exception.AddDescription(maAuthProdLocation, "infodate", maAuthorisedProduct.infodate.Trim());
                                _validationExceptions.Add(exception);
                            }
                        }

                        //MAH
                        if (overwriteExisting)
                        {
                            dbAuthorisedProduct.organizationmahcode_FK = null;
                        }
                        if (maAuthorisedProduct.mahcode != null)
                        {
                            if (maAuthorisedProduct.mahcode.resolutionmode != 2)
                            {
                                string message = "Value of resolutionmode must be '2'. Insert for Marketing Authorisation Holder is not supported.";
                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                exception.AddDescription(maAuthProdLocation, "mahcode arg: resolutionmode", maAuthorisedProduct.mahcode.resolutionmode.ToString());
                                _validationExceptions.Add(exception);
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(maAuthorisedProduct.mahcode.TypedValue))
                                {
                                    List<Organization_PK> licenceHolderList = _organization_PKOperations.GetOrganizationsByRole("Licence holder").Where(organization => !string.IsNullOrWhiteSpace(organization.ev_code)).ToList();
                                    licenceHolderList = licenceHolderList ?? new List<Organization_PK>();

                                    Organization_PK organizationInDb = licenceHolderList.Find(organization => organization.ev_code.Trim() == maAuthorisedProduct.mahcode.TypedValue.Trim());
                                    if (organizationInDb != null)
                                    {
                                        dbAuthorisedProduct.organizationmahcode_FK = organizationInDb.organization_PK;
                                        organizationInDb.organizationsenderid_EMEA = _EMASenderID;
                                        authorisedProductStruct.LicenceHolder = organizationInDb;
                                    }
                                    else
                                    {
                                        var dbLicenceHolder = new Organization_PK();
                                        dbLicenceHolder.ev_code = maAuthorisedProduct.mahcode.TypedValue.Trim();

                                        int? licenceHolderNum = licenceHolderList.Max(delegate(Organization_PK org)
                                        {
                                            if (org.name_org.StartsWith("Licence Holder"))
                                            {
                                                int number = 0;
                                                if (int.TryParse(org.name_org.Replace("Licence Holder", "").Trim(), out number)) return number;
                                                else return null;
                                            }
                                            return null;
                                        });

                                        if (licenceHolderNum == null) licenceHolderNum = 0;
                                        licenceHolderNum++;

                                        dbLicenceHolder.name_org = string.Format("Licence Holder {0}", licenceHolderNum);

                                        dbLicenceHolder.organizationsenderid_EMEA = _EMASenderID;

                                        authorisedProductStruct.LicenceHolder = dbLicenceHolder;

                                        Role_org_PK organizationRoleInDb = _role_org_PKOperations.GetEntities().Find(role => role.role_name != null && role.role_name.Trim().ToLower() == "licence holder");
                                        if (organizationRoleInDb != null)
                                        {
                                            var dbOrgInRole = new Organization_in_role_();
                                            dbOrgInRole.role_org_FK = organizationRoleInDb.role_org_PK;

                                            authorisedProductStruct.LicenceHolderOrgInRole = dbOrgInRole;
                                        }
                                        else
                                        {
                                            _exceptions.Add(new Exception("Licence Holder role could not be found in database."));
                                        }
                                    }
                                }
                                else
                                {
                                    string message = "Invalid value. Value must be specified.";
                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                    exception.AddDescription(maAuthProdLocation, "mahcode", maAuthorisedProduct.mahcode.TypedValue.TrimIfNotNull());
                                    _validationExceptions.Add(exception);
                                }
                            }
                        }

                        //Master file location
                        if (overwriteExisting)
                        {
                            dbAuthorisedProduct.mflcode_FK = null;
                        }
                        if (maAuthorisedProduct.mflcode != null)
                        {
                            if (maAuthorisedProduct.mflcode.resolutionmode != 2)
                            {
                                string message = "Value of resolutionmode must be '2'. Insert for Master File Location is not supported.";
                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                exception.AddDescription(maAuthProdLocation, "mflcode arg: resolutionmode", maAuthorisedProduct.mflcode.resolutionmode.ToString());
                                _validationExceptions.Add(exception);
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(maAuthorisedProduct.mflcode.TypedValue))
                                {
                                    List<Organization_PK> masterFileLocationList = _organization_PKOperations.GetOrganizationsByRole("Master File Location").Where(organization => !string.IsNullOrWhiteSpace(organization.mfl_evcode)).ToList();
                                    masterFileLocationList = masterFileLocationList ?? new List<Organization_PK>();

                                    Organization_PK organizationInDb = masterFileLocationList.Find(organization => organization.mfl_evcode.Trim() == maAuthorisedProduct.mflcode.TypedValue.Trim());
                                    if (organizationInDb != null)
                                    {
                                        dbAuthorisedProduct.mflcode_FK = organizationInDb.organization_PK;
                                    }
                                    else
                                    {
                                        var dbMasterFileLocation = new Organization_PK();
                                        dbMasterFileLocation.mfl_evcode = maAuthorisedProduct.mflcode.TypedValue.Trim();

                                        int? masterFileLocationNum = masterFileLocationList.Max(delegate(Organization_PK org)
                                        {
                                            if (org.name_org.StartsWith("Master File Location"))
                                            {
                                                int number = 0;
                                                if (int.TryParse(org.name_org.Replace("Master File Location", "").Trim(), out number)) return number;
                                                else return null;
                                            }
                                            return null;
                                        });

                                        if (masterFileLocationNum == null) masterFileLocationNum = 0;
                                        masterFileLocationNum++;

                                        dbMasterFileLocation.name_org = string.Format("Master File Location {0}", masterFileLocationNum);

                                        authorisedProductStruct.MasterFileLocation = dbMasterFileLocation;

                                        Role_org_PK organizationRoleInDb = _role_org_PKOperations.GetEntities().Find(role => role.role_name != null && role.role_name.Trim().ToLower() == "master file location");
                                        if (organizationRoleInDb != null)
                                        {
                                            var dbOrgInRole = new Organization_in_role_();
                                            dbOrgInRole.role_org_FK = organizationRoleInDb.role_org_PK;

                                            authorisedProductStruct.MasterFileLocationOrgInRole = dbOrgInRole;
                                        }
                                        else
                                        {
                                            _exceptions.Add(new Exception("Master File Location role could not be found in database."));
                                        }
                                    }
                                }
                                else
                                {
                                    string message = "Invalid value. Value must be specified.";
                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                    exception.AddDescription(maAuthProdLocation, "mflcode", maAuthorisedProduct.mflcode.TypedValue.TrimIfNotNull());
                                    _validationExceptions.Add(exception);
                                }
                            }
                        }

                        //QPPV
                        if (overwriteExisting)
                        {
                            dbAuthorisedProduct.qppv_code_FK = null;
                        }
                        if (maAuthorisedProduct.qppvcode != null && !string.IsNullOrWhiteSpace(maAuthorisedProduct.qppvcode.TypedValue))
                        {
                            List<Qppv_code_PK> qppvCodeList = _qppv_code_PKOperations.GetEntities().Where(qppvCode => !string.IsNullOrWhiteSpace(qppvCode.qppv_code)).ToList();
                            qppvCodeList = qppvCodeList ?? new List<Qppv_code_PK>();

                            Qppv_code_PK qppvCodeInDb = qppvCodeList.Find(qppvCode => qppvCode.qppv_code.Trim() == maAuthorisedProduct.qppvcode.TypedValue.Trim());

                            if (qppvCodeInDb != null)
                            {
                                dbAuthorisedProduct.qppv_code_FK = qppvCodeInDb.qppv_code_PK;
                            }
                            else
                            {
                                Person_role_PK qppvRoleInDb = _person_role_PKOperations.GetEntities().Find(role => role.person_name != null && role.person_name.Trim().ToLower() == "qppv");
                                if (qppvRoleInDb == null)
                                {
                                    _exceptions.Add(new Exception("Qppv role could not be found in database."));
                                }
                                else
                                {
                                    var qppvPersonStruct = new QPPVPersonStruct();

                                    Person_PK dbQppvPerson = null;
                                    Person_in_role_PK dbPersonInRole = null;

                                    var dbQppvCode = new Qppv_code_PK();

                                    Person_PK personInDb = _person_PKOperations.GetAllEntities().Find(person => person.name != null && person.name.Trim() == "QPPV" && person.familyname != null && person.familyname.Trim() == "Person");

                                    if (personInDb != null)
                                    {
                                        dbQppvCode.person_FK = personInDb.person_PK;
                                    }
                                    else
                                    {
                                        dbQppvPerson = new Person_PK();
                                        dbQppvPerson.name = "QPPV";
                                        dbQppvPerson.familyname = "Person";

                                        dbPersonInRole = new Person_in_role_PK();
                                        dbPersonInRole.person_role_FK = qppvRoleInDb.person_role_PK;
                                    }

                                    dbQppvCode.qppv_code = maAuthorisedProduct.qppvcode.TypedValue.Trim();

                                    qppvPersonStruct.QppvPerson = dbQppvPerson;
                                    qppvPersonStruct.QppvCode = dbQppvCode;
                                    qppvPersonStruct.QppvPersonInRole = dbPersonInRole;

                                    authorisedProductStruct.QppvPersonStruct = qppvPersonStruct;
                                }
                            }
                        }
                        else if (maAuthorisedProduct.qppvcode != null && string.IsNullOrWhiteSpace(maAuthorisedProduct.qppvcode.TypedValue))
                        {
                            string message = "Invalid value. Value must be specified.";
                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                            exception.AddDescription(maAuthProdLocation, "qppvcode", maAuthorisedProduct.qppvcode.TypedValue.TrimIfNotNull());
                            _validationExceptions.Add(exception);
                        }

                        #region Limitations

                        //New owner id
                        if (maAuthorisedProduct.newownerid != null)
                        {
                            string message = "Field newownerid is reserved for EMA therefore it should not be used.";
                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                            exception.AddDescription(maAuthProdLocation, "newownerid", maAuthorisedProduct.newownerid.Trim());
                            _validationExceptions.Add(exception);

                            dbProduct.newownerid = maAuthorisedProduct.newownerid.Trim();
                        }

                        //Sender local code
                        if (maAuthorisedProduct.senderlocalcode != null)
                        {
                            string message = "Field senderlocalcode is not supported therefore it should not be used.";
                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                            exception.AddDescription(maAuthProdLocation, "senderlocalcode", maAuthorisedProduct.senderlocalcode.Trim());
                            _validationExceptions.Add(exception);

                            dbProduct.senderlocalcode = maAuthorisedProduct.senderlocalcode.Trim();
                        }

                        //Previous EV Codes
                        if (maAuthorisedProduct.previousevcodes != null)
                        {
                            string message = "Development products are not supported therefore this section should not be used.";
                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                            exception.AddDescription(maAuthProdLocation, "previousevcodes", null);
                            _validationExceptions.Add(exception);
                        }

                        #endregion

                        #endregion

                        #region Authorisation

                        if (overwriteExisting)
                        {
                            dbAuthorisedProduct.authorisationcountrycode_FK = null;
                            dbAuthorisedProduct.authorisationdate = null;
                            dbAuthorisedProduct.authorisationwithdrawndate = null;
                            dbAuthorisedProduct.authorisationnumber = null;
                            dbAuthorisedProduct.authorisationstatus_FK = null;

                            dbProduct.authorisation_procedure = null;
                            dbProduct.orphan_drug = null;
                            dbProduct.intensive_monitoring = null;
                            dbProduct.product_number = null;
                        }

                        if (maAuthorisedProduct.authorisation != null)
                        {
                            authorisedproductType.authorisationLocalType maAuthorisation = maAuthorisedProduct.authorisation;

                            string authorisationMALocation = maAuthProdLocation + ".authorisation";
                            //Authorisation country code
                            if (maAuthorisation.authorisationcountrycode != null)
                            {
                                Country_PK countryInDb = _cv.CountryList.Find(country => country.abbreviation.Trim().ToLower() == maAuthorisation.authorisationcountrycode.Trim().ToLower());
                                if (countryInDb != null)
                                {
                                    dbAuthorisedProduct.authorisationcountrycode_FK = countryInDb.country_PK;
                                }
                                else
                                {
                                    string message = "Value is not in Controlled Vocabulary.";
                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                    exception.AddDescription(authorisationMALocation, "authorisationcountrycode", maAuthorisation.authorisationcountrycode.Trim());
                                    _validationExceptions.Add(exception);
                                }
                            }

                            //Authorisation date
                            if (maAuthorisation.authorisationdate != null)
                            {
                                DateTime authorisationDate = DateTime.Now;

                                string format = "yyyyMMdd";

                                if (maAuthorisation.authorisationdateformat == "102")
                                {
                                    format = "yyyyMMdd";
                                }
                                else if (maAuthorisation.authorisationdateformat == "610")
                                {
                                    format = "yyyyMM";
                                }

                                if (DateTime.TryParseExact(maAuthorisation.authorisationdate.Trim(), format, CultureInfo.InvariantCulture, DateTimeStyles.None, out authorisationDate))
                                {
                                    dbAuthorisedProduct.authorisationdate = authorisationDate;
                                }
                                else
                                {
                                    string message = "Value is not valid or it is not in valid format.";
                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValueFormat, SeverityType.Error);
                                    exception.AddDescription(authorisationMALocation, "authorisationdate", maAuthorisation.authorisationdate.Trim());
                                    _validationExceptions.Add(exception);
                                }
                            }

                            //Withdrawn date
                            if (maAuthorisation.withdrawndate != null)
                            {
                                DateTime withdrawnDate = DateTime.Now;

                                if (DateTime.TryParseExact(maAuthorisation.withdrawndate.Trim(), "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out withdrawnDate))
                                {
                                    dbAuthorisedProduct.authorisationwithdrawndate = withdrawnDate;
                                }
                                else
                                {
                                    string message = "Value is not valid or it is not in valid format.";
                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValueFormat, SeverityType.Error);
                                    exception.AddDescription(authorisationMALocation, "withdrawndate", maAuthorisation.withdrawndate.Trim());
                                    _validationExceptions.Add(exception);
                                }
                            }

                            //Authorisation number
                            if (!string.IsNullOrWhiteSpace(maAuthorisation.authorisationnumber))
                            {
                                dbAuthorisedProduct.authorisationnumber = maAuthorisation.authorisationnumber.Trim();
                            }

                            //Authorisation procedure
                            Type_PK authorisationProcedureInDb = _cv.AuthorisationProcedureList.Find(type => type.ev_code.Trim() == maAuthorisation.authorisationprocedure.ToString());

                            if (authorisationProcedureInDb != null)
                            {
                                dbProduct.authorisation_procedure = authorisationProcedureInDb.type_PK;
                            }
                            else
                            {
                                string message = "Value is not in Controlled Vocabulary.";
                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                exception.AddDescription(authorisationMALocation, "authorisationprocedure", maAuthorisation.authorisationprocedure.ToString());
                                _validationExceptions.Add(exception);
                            }

                            //Authorisation status
                            if (maAuthorisation.authorisationstatus != null)
                            {
                                Type_PK authorisationStatusInDb = _cv.AuthorisationStatusList.Find(type => type.ev_code.Trim() == maAuthorisation.authorisationstatus.ToString());

                                if (authorisationStatusInDb != null)
                                {
                                    dbAuthorisedProduct.authorisationstatus_FK = authorisationStatusInDb.type_PK;
                                }
                                else
                                {
                                    string message = "Value is not in Controlled Vocabulary.";
                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                    exception.AddDescription(authorisationMALocation, "authorisationstatus", maAuthorisation.authorisationstatus.ToString());
                                    _validationExceptions.Add(exception);
                                }
                            }

                            //mrp number
                            dbProduct.product_number = maAuthorisation.mrpnumber.TrimIfNotNull();

                            //eu number
                            if (!string.IsNullOrWhiteSpace(maAuthorisation.eunumber))
                            {
                                if (!string.IsNullOrWhiteSpace(maAuthorisation.authorisationnumber) && maAuthorisation.eunumber.Trim() == maAuthorisation.authorisationnumber.Trim())
                                {
                                    dbAuthorisedProduct.authorisationnumber = maAuthorisation.eunumber.Trim();
                                }
                                else if (maAuthorisation.authorisationprocedure == 1)
                                {
                                    string message = "If value of authorisation procedure corresponds to 'Centralised' eunumber must match authorisationnumber.";
                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                    exception.AddDescription(authorisationMALocation, "eunumber", maAuthorisation.eunumber.Trim());
                                    _validationExceptions.Add(exception);
                                }
                                else
                                {
                                    string message = "If value of authorisation procedure doesn't correspond to 'Centralised' eunumber must be empty.";
                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                    exception.AddDescription(authorisationMALocation, "eunumber", maAuthorisation.eunumber.Trim());
                                    _validationExceptions.Add(exception);
                                }
                            }

                            //Orphan drug
                            if (maAuthorisation.orphandrug != null)
                            {
                                if (maAuthorisation.orphandrug == 1)
                                {
                                    dbProduct.orphan_drug = true;
                                }
                                else if (maAuthorisation.orphandrug == 2)
                                {
                                    dbProduct.orphan_drug = false;
                                }
                                else
                                {
                                    string message = "Value is not valid. Valid values: '1', '2'.";
                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                    exception.AddDescription(authorisationMALocation, "orphandrug", maAuthorisation.orphandrug.ToString());
                                    _validationExceptions.Add(exception);
                                }
                            }

                            //Intensive monitoring
                            if (maAuthorisation.intensivemonitoring != null)
                            {
                                dbProduct.intensive_monitoring = maAuthorisation.intensivemonitoring;
                            }
                        }

                        #endregion

                        #region Presentation

                        if (overwriteExisting)
                        {
                            dbAuthorisedProduct.packagedesc = null;
                            dbAuthorisedProduct.product_name = null;
                            dbAuthorisedProduct.productshortname = null;
                            dbAuthorisedProduct.productcompanyname = null;
                            dbAuthorisedProduct.productgenericname = null;
                            dbAuthorisedProduct.productform = null;
                            dbAuthorisedProduct.productstrenght = null;

                            dbProduct.name = null;
                        }

                        if (maAuthorisedProduct.presentationname != null)
                        {
                            //Package description
                            if (!string.IsNullOrWhiteSpace(maAuthorisedProduct.presentationname.packagedesc))
                            {
                                dbAuthorisedProduct.packagedesc = maAuthorisedProduct.presentationname.packagedesc.Trim();
                            }

                            //Product name
                            if (!string.IsNullOrWhiteSpace(maAuthorisedProduct.presentationname.productname))
                            {
                                dbAuthorisedProduct.product_name = maAuthorisedProduct.presentationname.productname.Trim();
                                dbProduct.name = maAuthorisedProduct.presentationname.productname.Trim();
                            }

                            //Product short name
                            if (!string.IsNullOrWhiteSpace(maAuthorisedProduct.presentationname.productshortname))
                            {
                                dbAuthorisedProduct.productshortname = maAuthorisedProduct.presentationname.productshortname.Trim();
                            }

                            //Product company name
                            if (!string.IsNullOrWhiteSpace(maAuthorisedProduct.presentationname.productcompanyname))
                            {
                                dbAuthorisedProduct.productcompanyname = maAuthorisedProduct.presentationname.productcompanyname.Trim();
                            }

                            //Product generic name
                            if (!string.IsNullOrWhiteSpace(maAuthorisedProduct.presentationname.productgenericname))
                            {
                                dbAuthorisedProduct.productgenericname = maAuthorisedProduct.presentationname.productgenericname.Trim();
                            }

                            //Product form
                            if (!string.IsNullOrWhiteSpace(maAuthorisedProduct.presentationname.productform))
                            {
                                dbAuthorisedProduct.productform = maAuthorisedProduct.presentationname.productform.Trim();
                            }

                            //Product strength
                            if (!string.IsNullOrWhiteSpace(maAuthorisedProduct.presentationname.productstrength))
                            {
                                dbAuthorisedProduct.productstrenght = maAuthorisedProduct.presentationname.productstrength.Trim();
                            }

                        }

                        #endregion

                        #region Product ATCs

                        if (overwriteExisting)
                        {
                            _product_atc_mn_PKOperations.DeleteByProductPK(dbProduct.product_PK);
                        }

                        if (maAuthorisedProduct.productatcs != null && maAuthorisedProduct.productatcs.productatc != null && maAuthorisedProduct.productatcs.productatc.Count > 0)
                        {
                            List<authorisedproductType.productatcsLocalType.productatcLocalType> maAtcList = maAuthorisedProduct.productatcs.productatc.ToList();

                            for (int maProductAtcIndex = 0; maProductAtcIndex < maAtcList.Count; maProductAtcIndex++)
                            {
                                string maProductAtcLocation = string.Format("{0}.productatcs.productatc[{1}]", maAuthProdLocation, maProductAtcIndex);

                                authorisedproductType.productatcsLocalType.productatcLocalType maAtc = maAtcList[maProductAtcIndex];

                                var dbProductAtcMn = new Product_atc_mn_PK();

                                if (maAtc.atccode != null)
                                {
                                    if (maAtc.atccode.resolutionmode != 2)
                                    {
                                        string message = "Value of resolutionmode must be '2'. Insert for Anatomical Therapeutic Chemical is not supported.";
                                        var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                        exception.AddDescription(maProductAtcLocation, "atccode arg: resolutionmode", maAtc.atccode.resolutionmode.ToString());
                                        _validationExceptions.Add(exception);
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(maAtc.atccode.TypedValue))
                                        {
                                            Atc_PK atcInDb = _cv.AtcList.Find(atc => atc.atccode.Trim() == maAtc.atccode.TypedValue.Trim());
                                            if (atcInDb != null)
                                            {
                                                dbProductAtcMn.atc_FK = atcInDb.atc_PK;
                                                dbProductAtcMn.product_FK = null;

                                                productStruct.ProductAtcMNList.Add(dbProductAtcMn);
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maProductAtcLocation, "atccode", maAtc.atccode.TypedValue.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }
                                        else
                                        {
                                            string atccode = maAtc.atccode.TypedValue ?? string.Empty;

                                            string message = "Invalid value. Value must be specified.";
                                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                            exception.AddDescription(maProductAtcLocation, "atccode", atccode.Trim());
                                            _validationExceptions.Add(exception);
                                        }
                                    }
                                }
                            }
                        }

                        #endregion

                        #region Product indications

                        if (overwriteExisting)
                        {
                            _meddra_ap_mn_PKOperations.DeleteMeddraByAP(dbAuthorisedProduct.ap_PK);
                        }

                        if (maAuthorisedProduct.productindications != null && maAuthorisedProduct.productindications.productindication != null && maAuthorisedProduct.productindications.productindication.Count > 0)
                        {
                            List<authorisedproductType.productindicationsLocalType.productindicationLocalType> maIndicationList = ma.authorisedproducts.authorisedproduct[maAuthProdIndex].productindications.productindication.ToList();

                            for (int maIndicationIndex = 0; maIndicationIndex < maIndicationList.Count; maIndicationIndex++)
                            {
                                string maIndicationLocation = string.Format("{0}.productindications.productindication[{1}]", maAuthProdLocation, maIndicationIndex);

                                authorisedproductType.productindicationsLocalType.productindicationLocalType maIndication = maIndicationList[maIndicationIndex];

                                var dbMeddra = new Meddra_pk();

                                dbMeddra.code = maIndication.meddracode.ToString();

                                Type_PK meddraVersionInDb = _cv.MeddraVersionList.Find(delegate(Type_PK meddraVersion)
                                {
                                    decimal version = 0;
                                    if (decimal.TryParse(meddraVersion.name.Trim(), NumberStyles.Number, CultureInfo.InvariantCulture.NumberFormat, out version))
                                    {
                                        if (version == maIndication.meddraversion) return true;
                                    }
                                    return false;
                                });

                                if (meddraVersionInDb != null)
                                {
                                    dbMeddra.version_type_FK = meddraVersionInDb.type_PK;
                                }
                                else
                                {
                                    string message = "Value is not in Controlled Vocabulary.";
                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                    exception.AddDescription(maIndicationLocation, "meddraversion", maIndication.meddraversion.ToString());
                                    _validationExceptions.Add(exception);
                                }

                                if (!string.IsNullOrWhiteSpace(maIndication.meddralevel))
                                {
                                    Type_PK meddraLevelInDb = _cv.MeddraLevelList.Find(meddraLevel => meddraLevel.name.Trim().ToLower() == maIndication.meddralevel.Trim() || meddraLevel.name.Trim().ToUpper() == maIndication.meddralevel.Trim());
                                    if (meddraLevelInDb != null)
                                    {
                                        dbMeddra.level_type_FK = meddraLevelInDb.type_PK;
                                    }
                                    else
                                    {
                                        string message = "Value is not in Controlled Vocabulary.";
                                        var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                        exception.AddDescription(maIndicationLocation, "meddralevel", maIndication.meddralevel.Trim());
                                        _validationExceptions.Add(exception);
                                    }
                                }
                                else
                                {
                                    string meddralevel = maIndication.meddralevel ?? string.Empty;

                                    string message = "Invalid value. Value must be specified.";
                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                    exception.AddDescription(maIndicationLocation, "meddralevel", meddralevel.Trim());
                                    _validationExceptions.Add(exception);
                                }

                                authorisedProductStruct.MeddraList.Add(dbMeddra);
                            }
                        }

                        #endregion

                        #region PPI attachments

                        if (overwriteExisting)
                        {
                            _ap_document_mn_PKOperations.DeleteByAuthorisedProduct(dbAuthorisedProduct.ap_PK);
                        }

                        if (maAuthorisedProduct.ppiattachments != null && maAuthorisedProduct.ppiattachments.ppiattachment != null && maAuthorisedProduct.ppiattachments.ppiattachment.Count > 0)
                        {
                            List<authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType> maPPIAttachmentList = maAuthorisedProduct.ppiattachments.ppiattachment.ToList();

                            for (int maPPIAttachIndex = 0; maPPIAttachIndex < maPPIAttachmentList.Count; maPPIAttachIndex++)
                            {
                                #region Limitations

                                //if (maPPIAttachIndex > 0)
                                //{
                                //    string message = "Ppiattachments section contains more than one ppiattachment. Only one ppiattachment per authorised product is supported.";
                                //    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                //    exception.AddDescription(maAuthProdLocation + ".ppiattachments", string.Format("ppiattachment[{0}]", maPPIAttachIndex), null);
                                //    _validationExceptions.Add(exception);

                                //    continue;
                                //}

                                #endregion

                                string maPPIAttachLocation = string.Format("{0}.ppiattachments.ppiattachment[{1}]", maAuthProdLocation, maPPIAttachIndex);

                                authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType maPPIAttachment = maPPIAttachmentList[maPPIAttachIndex];

                                if (maPPIAttachment.attachmentcode != null)
                                {
                                    if (!string.IsNullOrWhiteSpace(maPPIAttachment.attachmentcode.TypedValue))
                                    {
                                        if (maPPIAttachment.attachmentcode.resolutionmode == 1)
                                        {
                                            DocumentStruct refDocumentStruct = _maReadyStruct.DocumentStructList.Find(docStruct => !string.IsNullOrWhiteSpace(docStruct.AttachmentLocalNumber) && docStruct.AttachmentLocalNumber.Trim() == maPPIAttachment.attachmentcode.TypedValue.Trim());
                                            if (refDocumentStruct != null)
                                            {
                                                authorisedProductStruct.DocumentStructList.Add(refDocumentStruct);
                                            }
                                            else if (ma.attachments != null && ma.attachments.attachment != null && ma.attachments.attachment.Count(attach => !string.IsNullOrWhiteSpace(attach.localnumber) && attach.localnumber.Trim() == maPPIAttachment.attachmentcode.TypedValue.Trim()) > 0)
                                            {
                                                string message = string.Format("Attachment with localnumber = '{0}' can't be referenced because attachment section contains more than one attachment and only first attachment is taken into consideration.", maPPIAttachment.attachmentcode.TypedValue.Trim());
                                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                exception.AddDescription(maPPIAttachLocation, "attachmentcode", maPPIAttachment.attachmentcode.TypedValue.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                string message = string.Format("Attachment with localnumber = '{0}' can't be found in attachments section.", maPPIAttachment.attachmentcode.TypedValue.Trim());
                                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                exception.AddDescription(maPPIAttachLocation, "attachmentcode", maPPIAttachment.attachmentcode.TypedValue.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }
                                        else if (maPPIAttachment.attachmentcode.resolutionmode == 2)
                                        {
                                            if (ma.attachments != null && ma.attachments.attachment != null)
                                            {
                                                if (ma.attachments.attachment.Count == 1)
                                                {
                                                    string message = string.Format("According to resolutionmode = '2' ppiattachment contains EV Code. Attachment from attachments section won't be considered.");
                                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                    exception.AddDescription(maPPIAttachLocation, "attachmentcode", maPPIAttachment.attachmentcode.TypedValue.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                                else if (ma.attachments.attachment.Count > 1)
                                                {
                                                    string message = string.Format("According to resolutionmode = '2' ppiattachment contains EV Code. Attachments from attachments section won't be considered.");
                                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                    exception.AddDescription(maPPIAttachLocation, "attachmentcode", maPPIAttachment.attachmentcode.TypedValue.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }

                                            Attachment_PK attachmentInDb = _attachment_PKOperations.GetEntityByEVCode(maPPIAttachment.attachmentcode.TypedValue.Trim());
                                            if (attachmentInDb != null)
                                            {
                                                var documentStruct = new DocumentStruct();

                                                attachmentInDb.attachment_PK = null;
                                                documentStruct.Attachment = attachmentInDb;

                                                Document_PK documentInDb = _document_PKOperations.GetEntity(attachmentInDb.document_FK);
                                                if (documentInDb != null)
                                                {
                                                    List<Document_language_mn_PK> documentLanguageCodeMNListInDb = _document_language_mn_PKOperations.GetLanguagesByDocument(documentInDb.document_PK);

                                                    if (documentLanguageCodeMNListInDb != null && documentLanguageCodeMNListInDb.Count > 0)
                                                    {
                                                        documentLanguageCodeMNListInDb.ForEach(delegate(Document_language_mn_PK documentLanguageCodeMNInDb)
                                                        {
                                                            documentLanguageCodeMNInDb.document_language_mn_PK = null;
                                                            documentLanguageCodeMNInDb.document_FK = null;
                                                            documentStruct.DocumentLanguageCodeMNList.Add(documentLanguageCodeMNInDb);
                                                        });
                                                    }

                                                    documentInDb.document_PK = null;
                                                    documentStruct.Document = documentInDb;

                                                    authorisedProductStruct.DocumentStructList.Add(documentStruct);
                                                }
                                                else
                                                {
                                                    string message = string.Format("Document with ID = '{0}' can't be loaded from database.", attachmentInDb.document_FK);
                                                    _exceptions.Add(new Exception(message));
                                                }

                                            }
                                            else
                                            {
                                                var dbAttachment = new Attachment_PK();
                                                dbAttachment.ev_code = maPPIAttachment.attachmentcode.TypedValue.Trim();
                                                dbAttachment.attachmentname = "PPI Attachment";

                                                var dbDocument = new Document_PK();
                                                dbDocument.name = "PPI Document";

                                                Type_PK ppiType = _cv.TypeList.Find(type => !string.IsNullOrWhiteSpace(type.name) && type.name.Trim().ToLower() == "ppi");
                                                if (ppiType != null)
                                                {
                                                    dbDocument.type_FK = ppiType.type_PK;
                                                }

                                                dbDocument.person_FK = _responisbleUserPK;

                                                var documentStruct = new DocumentStruct();
                                                documentStruct.Attachment = dbAttachment;
                                                documentStruct.Document = dbDocument;

                                                authorisedProductStruct.DocumentStructList.Add(documentStruct);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        string attachmentcode = maPPIAttachment.attachmentcode.TypedValue ?? string.Empty;

                                        string message = "Invalid value. Value must be specified.";
                                        var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                        exception.AddDescription(maPPIAttachLocation, "attachmentcode", attachmentcode.Trim());
                                        _validationExceptions.Add(exception);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Pharmaceutical products

                        if (overwriteExisting)
                        {
                            _product_mn_PKOperations.DeleteByProductPK(dbProduct.product_PK);
                        }

                        if (maAuthorisedProduct.pharmaceuticalproducts != null && maAuthorisedProduct.pharmaceuticalproducts.pharmaceuticalproduct != null && maAuthorisedProduct.pharmaceuticalproducts.pharmaceuticalproduct.Count > 0)
                        {
                            List<pharmaceuticalproductType> maPharmaceuticalProductList = maAuthorisedProduct.pharmaceuticalproducts.pharmaceuticalproduct.ToList();

                            for (int maPPIndex = 0; maPPIndex < maPharmaceuticalProductList.Count; maPPIndex++)
                            {
                                string maPPLocation = string.Format("{0}.pharmaceuticalproducts.pharmaceuticalproduct[{1}]", maAuthProdLocation, maPPIndex);

                                pharmaceuticalproductType maPharmaceuticalProduct = maPharmaceuticalProductList[maPPIndex];

                                var dbPharmaceuticalProduct = new Pharmaceutical_product_PK();

                                var pharmaceuticalProductStruct = new PharmaceuticalProductStruct();

                                pharmaceuticalProductStruct.PharmaceuticalProduct = dbPharmaceuticalProduct;

                                dbPharmaceuticalProduct.responsible_user_FK = _responisbleUserPK;

                                #region Pharmaceutical Form

                                if (maPharmaceuticalProduct.pharmformcode != null)
                                {
                                    if (maPharmaceuticalProduct.pharmformcode.resolutionmode != 2)
                                    {
                                        string message = "Value of resolutionmode must be '2'. Insert for Pharmaceutical Form is not supported.";
                                        var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                        exception.AddDescription(maPPLocation, "pharmformcode arg: resolutionmode", maPharmaceuticalProduct.pharmformcode.resolutionmode.ToString());
                                        _validationExceptions.Add(exception);
                                    }
                                    else
                                    {
                                        if (!string.IsNullOrWhiteSpace(maPharmaceuticalProduct.pharmformcode.TypedValue))
                                        {
                                            Pharmaceutical_form_PK pharmaceuticalFormInDb = _cv.PharmaceuticalFormList.Find(pharmaceticalForm => pharmaceticalForm.ev_code.Trim() == maPharmaceuticalProduct.pharmformcode.TypedValue.Trim());
                                            if (pharmaceuticalFormInDb != null)
                                            {
                                                dbPharmaceuticalProduct.Pharmform_FK = pharmaceuticalFormInDb.pharmaceutical_form_PK;
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maPPLocation, "pharmformcode", maPharmaceuticalProduct.pharmformcode.TypedValue.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }
                                        else
                                        {
                                            string pharmformcode = maPharmaceuticalProduct.pharmformcode.TypedValue ?? string.Empty;

                                            string message = "Invalid value. Value must be specified.";
                                            var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                            exception.AddDescription(maPPLocation, "pharmformcode", pharmformcode.Trim());
                                            _validationExceptions.Add(exception);
                                        }
                                    }
                                }

                                #endregion

                                #region Active Ingredients

                                if (maPharmaceuticalProduct.activeingredients != null && maPharmaceuticalProduct.activeingredients.activeingredient != null && maPharmaceuticalProduct.activeingredients.activeingredient.Count > 0)
                                {
                                    List<pharmaceuticalproductType.activeingredientsLocalType.activeingredientLocalType> maActiveIngredientList = maPharmaceuticalProduct.activeingredients.activeingredient.ToList();

                                    for (int maActiveIngIndex = 0; maActiveIngIndex < maActiveIngredientList.Count; maActiveIngIndex++)
                                    {
                                        string maActiveIngLocation = string.Format("{0}.activeingredients.activeingredient[{1}]", maPPLocation, maActiveIngIndex);

                                        pharmaceuticalproductType.activeingredientsLocalType.activeingredientLocalType maActiveIngredient = maActiveIngredientList[maActiveIngIndex];

                                        Activeingredient_PK dbActiveIngredient = new Activeingredient_PK();

                                        if (maActiveIngredient.concentrationtypecode != null)
                                        {
                                            Ssi__cont_voc_PK concTypeInDb = _cv.ConcentrationTypeList.Find(concType => concType.Evcode.Trim() == maActiveIngredient.concentrationtypecode.ToString());
                                            if (concTypeInDb != null)
                                            {
                                                dbActiveIngredient.concentrationtypecode = concTypeInDb.ssi__cont_voc_PK;
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maActiveIngLocation, "concentrationtypecode", maActiveIngredient.concentrationtypecode.ToString());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maActiveIngredient.substancecode != null)
                                        {
                                            if (maActiveIngredient.substancecode.resolutionmode != 2)
                                            {
                                                string message = "Value of resolutionmode must be '2'. Insert for Substance is not supported.";
                                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                exception.AddDescription(maActiveIngLocation, "substancecode arg: resolutionmode", maActiveIngredient.substancecode.resolutionmode.ToString());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrWhiteSpace(maActiveIngredient.substancecode.TypedValue))
                                                {
                                                    Substance_PK substanceInDb = _cv.SubstanceList.Find(substance => substance.ev_code.Trim() == maActiveIngredient.substancecode.TypedValue.Trim());
                                                    if (substanceInDb != null)
                                                    {
                                                        dbActiveIngredient.substancecode_FK = substanceInDb.substance_PK;
                                                    }
                                                    else
                                                    {
                                                        string message = "Value is not in Controlled Vocabulary.";
                                                        var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                        exception.AddDescription(maActiveIngLocation, "substancecode", maActiveIngredient.substancecode.TypedValue.Trim());
                                                        _validationExceptions.Add(exception);
                                                    }
                                                }
                                                else
                                                {
                                                    string substancecode = maActiveIngredient.substancecode.TypedValue ?? string.Empty;

                                                    string message = "Invalid value. Value must be specified.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                    exception.AddDescription(maActiveIngLocation, "substancecode", substancecode.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }


                                        dbActiveIngredient.lowamountnumervalue = maActiveIngredient.lowamountnumervalue;
                                        dbActiveIngredient.lowamountdenomvalue = maActiveIngredient.lowamountdenomvalue;

                                        if (maActiveIngredient.highamountnumervalue.HasValue)
                                        {
                                            dbActiveIngredient.highamountnumervalue = maActiveIngredient.highamountnumervalue;
                                        }

                                        if (maActiveIngredient.highamountdenomvalue.HasValue)
                                        {
                                            dbActiveIngredient.highamountdenomvalue = maActiveIngredient.highamountdenomvalue;
                                        }

                                        if (maActiveIngredient.lowamountnumerprefix != null)
                                        {
                                            Ssi__cont_voc_PK prefixInDb = _cv.PrefixList.Find(prefix => prefix.term_name_english.Trim() == maActiveIngredient.lowamountnumerprefix.Trim());
                                            if (prefixInDb != null)
                                            {
                                                dbActiveIngredient.lowamountnumerprefix = prefixInDb.ssi__cont_voc_PK.ToString();
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maActiveIngLocation, "lowamountnumerprefix", maActiveIngredient.lowamountnumerprefix.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maActiveIngredient.lowamountdenomprefix != null)
                                        {
                                            Ssi__cont_voc_PK prefixInDb = _cv.PrefixList.Find(prefix => prefix.term_name_english.Trim() == maActiveIngredient.lowamountdenomprefix.Trim());
                                            if (prefixInDb != null)
                                            {
                                                dbActiveIngredient.lowamountdenomprefix = prefixInDb.ssi__cont_voc_PK.ToString();
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maActiveIngLocation, "lowamountdenomprefix", maActiveIngredient.lowamountdenomprefix.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maActiveIngredient.highamountnumerprefix != null)
                                        {
                                            Ssi__cont_voc_PK prefixInDb = _cv.PrefixList.Find(prefix => prefix.term_name_english.Trim() == maActiveIngredient.highamountnumerprefix.Trim());
                                            if (prefixInDb != null)
                                            {
                                                dbActiveIngredient.highamountnumerprefix = prefixInDb.ssi__cont_voc_PK.ToString();
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maActiveIngLocation, "highamountnumerprefix", maActiveIngredient.highamountnumerprefix.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maActiveIngredient.highamountdenomprefix != null)
                                        {
                                            Ssi__cont_voc_PK prefixInDb = _cv.PrefixList.Find(prefix => prefix.term_name_english.Trim() == maActiveIngredient.highamountdenomprefix.Trim());
                                            if (prefixInDb != null)
                                            {
                                                dbActiveIngredient.highamountdenomprefix = prefixInDb.ssi__cont_voc_PK.ToString();
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maActiveIngLocation, "highamountdenomprefix", maActiveIngredient.highamountdenomprefix.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maActiveIngredient.lowamountnumerunit != null)
                                        {
                                            Ssi__cont_voc_PK unitInDb = _cv.NumeratorUnitList.Find(numeratorUnit => numeratorUnit.term_name_english.Trim() == maActiveIngredient.lowamountnumerunit.Trim());
                                            if (unitInDb != null)
                                            {
                                                dbActiveIngredient.lowamountnumerunit = unitInDb.ssi__cont_voc_PK.ToString();
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maActiveIngLocation, "lowamountnumerunit", maActiveIngredient.lowamountnumerunit.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maActiveIngredient.lowamountdenomunit != null)
                                        {
                                            Ssi__cont_voc_PK unitInDb = _cv.DenominatorMeasureUnitList.Find(denominatorUnit => denominatorUnit.term_name_english.Trim() == maActiveIngredient.lowamountdenomunit.Trim());
                                            if (unitInDb != null)
                                            {
                                                dbActiveIngredient.lowamountdenomunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                dbActiveIngredient.ExpressedBy_FK = _cv.ExpressedByUnitsOfMeasure != null ? _cv.ExpressedByUnitsOfMeasure.ssi__cont_voc_PK : null;
                                            }
                                            else
                                            {
                                                unitInDb = _cv.DenominatorPresentationUnitList.Find(denominatorUnit => denominatorUnit.term_name_english.Trim() == maActiveIngredient.lowamountdenomunit.Trim());
                                                if (unitInDb != null)
                                                {
                                                    dbActiveIngredient.lowamountdenomunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                    dbActiveIngredient.ExpressedBy_FK = _cv.ExpressedByUnitsOfPresentation != null ? _cv.ExpressedByUnitsOfPresentation.ssi__cont_voc_PK : null;
                                                }
                                                else
                                                {
                                                    string message = "Value is not in Controlled Vocabulary.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                    exception.AddDescription(maActiveIngLocation, "lowamountdenomunit", maActiveIngredient.lowamountdenomunit.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        if (maActiveIngredient.highamountnumerunit != null)
                                        {
                                            Ssi__cont_voc_PK unitInDb = _cv.NumeratorUnitList.Find(numeratorUnit => numeratorUnit.term_name_english.Trim() == maActiveIngredient.highamountnumerunit.Trim());
                                            if (unitInDb != null)
                                            {
                                                dbActiveIngredient.highamountnumerunit = unitInDb.ssi__cont_voc_PK.ToString();
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maActiveIngLocation, "highamountnumerunit", maActiveIngredient.highamountnumerunit.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maActiveIngredient.highamountdenomunit != null)
                                        {
                                            Ssi__cont_voc_PK unitInDb = _cv.DenominatorMeasureUnitList.Find(denominatorUnit => denominatorUnit.term_name_english.Trim() == maActiveIngredient.highamountdenomunit.Trim());
                                            if (unitInDb != null)
                                            {
                                                dbActiveIngredient.highamountdenomunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                dbActiveIngredient.ExpressedBy_FK = _cv.ExpressedByUnitsOfMeasure != null ? _cv.ExpressedByUnitsOfMeasure.ssi__cont_voc_PK : null;
                                            }
                                            else
                                            {
                                                unitInDb = _cv.DenominatorPresentationUnitList.Find(denominatorUnit => denominatorUnit.term_name_english.Trim() == maActiveIngredient.highamountdenomunit.Trim());
                                                if (unitInDb != null)
                                                {
                                                    dbActiveIngredient.highamountdenomunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                    dbActiveIngredient.ExpressedBy_FK = _cv.ExpressedByUnitsOfPresentation != null ? _cv.ExpressedByUnitsOfPresentation.ssi__cont_voc_PK : null;
                                                }
                                                else
                                                {
                                                    string message = "Value is not in Controlled Vocabulary.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                    exception.AddDescription(maActiveIngLocation, "highamountdenomunit", maActiveIngredient.highamountdenomunit.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        dbActiveIngredient.concise = ConstructConcise(dbActiveIngredient);

                                        pharmaceuticalProductStruct.ActiveIngredientList.Add(dbActiveIngredient);
                                    }
                                }
                                #endregion

                                #region Excipients

                                if (maPharmaceuticalProduct.excipients != null && maPharmaceuticalProduct.excipients.excipient != null && maPharmaceuticalProduct.excipients.excipient.Count > 0)
                                {
                                    List<pharmaceuticalproductType.excipientsLocalType.excipientLocalType> maExcipientList = maPharmaceuticalProduct.excipients.excipient.ToList();

                                    //THIS IS NOT EMA BUSINESS RULE! This is according to Ready web application.
                                    bool allowOnlySubstanceCode = true;

                                    for (int maExcipientIndex = 0; maExcipientIndex < maExcipientList.Count; maExcipientIndex++)
                                    {
                                        string maExcipientLocation = string.Format("{0}.excipients.excipient[{1}]", maPPLocation, maExcipientIndex);

                                        pharmaceuticalproductType.excipientsLocalType.excipientLocalType maExcipient = maExcipientList[maExcipientIndex];

                                        Excipient_PK dbExcipient = new Excipient_PK();

                                        if (maExcipient.substancecode != null)
                                        {
                                            if (maExcipient.substancecode.resolutionmode != 2)
                                            {
                                                string message = "Value of resolutionmode must be '2'. Insert for Substance is not supported.";
                                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "substancecode arg: resolutionmode", maExcipient.substancecode.resolutionmode.ToString());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrWhiteSpace(maExcipient.substancecode.TypedValue))
                                                {
                                                    Substance_PK substanceInDb = _cv.SubstanceList.Find(substance => substance.ev_code.Trim() == maExcipient.substancecode.TypedValue.Trim());
                                                    if (substanceInDb != null)
                                                    {
                                                        dbExcipient.substancecode_FK = substanceInDb.substance_PK;
                                                    }
                                                    else
                                                    {
                                                        string message = "Value is not in Controlled Vocabulary.";
                                                        var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                        exception.AddDescription(maExcipientLocation, "substancecode", maExcipient.substancecode.TypedValue.Trim());
                                                        _validationExceptions.Add(exception);
                                                    }
                                                }
                                                else
                                                {
                                                    string substancecode = maExcipient.substancecode.TypedValue ?? string.Empty;

                                                    string message = "Invalid value. Value must be specified.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                    exception.AddDescription(maExcipientLocation, "substancecode", substancecode.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        if (maExcipient.concentrationtypecode != null)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the concentrationtypecode is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "concentrationtypecode", maExcipient.concentrationtypecode.ToString());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                Ssi__cont_voc_PK concTypeInDb = _cv.ConcentrationTypeList.Find(concType => concType.Evcode.Trim() == maExcipient.concentrationtypecode.ToString());
                                                if (concTypeInDb != null)
                                                {
                                                    dbExcipient.concentrationtypecode = concTypeInDb.ssi__cont_voc_PK;
                                                }
                                                else
                                                {
                                                    string message = "Value is not in Controlled Vocabulary.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                    exception.AddDescription(maExcipientLocation, "concentrationtypecode", maExcipient.concentrationtypecode.ToString());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        if (maExcipient.lowamountnumervalue.HasValue)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the lowamountnumervalue is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "lowamountnumervalue", maExcipient.lowamountnumervalue.ToString());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                dbExcipient.lowamountnumervalue = maExcipient.lowamountnumervalue;
                                            }
                                        }

                                        if (maExcipient.lowamountdenomvalue.HasValue)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the lowamountdenomvalue is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "lowamountdenomvalue", maExcipient.lowamountdenomvalue.ToString());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                dbExcipient.lowamountdenomvalue = maExcipient.lowamountdenomvalue;
                                            }
                                        }

                                        if (maExcipient.highamountnumervalue.HasValue)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the highamountnumervalue is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "highamountnumervalue", maExcipient.highamountnumervalue.ToString());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                dbExcipient.highamountnumervalue = maExcipient.highamountnumervalue;
                                            }
                                        }

                                        if (maExcipient.highamountdenomvalue.HasValue)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the highamountdenomvalue is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "highamountdenomvalue", maExcipient.highamountdenomvalue.ToString());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                dbExcipient.highamountdenomvalue = maExcipient.highamountdenomvalue;
                                            }
                                        }

                                        if (maExcipient.lowamountnumerprefix != null)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the lowamountnumerprefix is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "lowamountnumerprefix", maExcipient.lowamountnumerprefix.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                Ssi__cont_voc_PK prefixInDb = _cv.PrefixList.Find(prefix => prefix.term_name_english.Trim() == maExcipient.lowamountnumerprefix.Trim());
                                                if (prefixInDb != null)
                                                {
                                                    dbExcipient.lowamountnumerprefix = prefixInDb.ssi__cont_voc_PK.ToString();
                                                }
                                                else
                                                {
                                                    string message = "Value is not in Controlled Vocabulary.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                    exception.AddDescription(maExcipientLocation, "lowamountnumerprefix", maExcipient.lowamountnumerprefix.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        if (maExcipient.lowamountdenomprefix != null)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the lowamountdenomprefix is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "lowamountdenomprefix", maExcipient.lowamountdenomprefix.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                Ssi__cont_voc_PK prefixInDb = _cv.PrefixList.Find(prefix => prefix.term_name_english.Trim() == maExcipient.lowamountdenomprefix.Trim());
                                                if (prefixInDb != null)
                                                {
                                                    dbExcipient.lowamountdenomprefix = prefixInDb.ssi__cont_voc_PK.ToString();
                                                }
                                                else
                                                {
                                                    string message = "Value is not in Controlled Vocabulary.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                    exception.AddDescription(maExcipientLocation, "lowamountdenomprefix", maExcipient.lowamountdenomprefix.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        if (maExcipient.highamountnumerprefix != null)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the lowamountdenomprefix is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "lowamountdenomprefix", maExcipient.lowamountdenomprefix.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                Ssi__cont_voc_PK prefixInDb = _cv.PrefixList.Find(prefix => prefix.term_name_english.Trim() == maExcipient.highamountnumerprefix.Trim());
                                                if (prefixInDb != null)
                                                {
                                                    dbExcipient.highamountnumerprefix = prefixInDb.ssi__cont_voc_PK.ToString();
                                                }
                                                else
                                                {
                                                    string message = "Value is not in Controlled Vocabulary.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                    exception.AddDescription(maExcipientLocation, "highamountnumerprefix", maExcipient.highamountnumerprefix.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        if (maExcipient.highamountdenomprefix != null)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the highamountdenomprefix is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "highamountdenomprefix", maExcipient.highamountdenomprefix.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                Ssi__cont_voc_PK prefixInDb = _cv.PrefixList.Find(prefix => prefix.term_name_english.Trim() == maExcipient.highamountdenomprefix.Trim());
                                                if (prefixInDb != null)
                                                {
                                                    dbExcipient.highamountdenomprefix = prefixInDb.ssi__cont_voc_PK.ToString();
                                                }
                                                else
                                                {
                                                    string message = "Value is not in Controlled Vocabulary.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                    exception.AddDescription(maExcipientLocation, "highamountdenomprefix", maExcipient.highamountdenomprefix.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        if (maExcipient.lowamountnumerunit != null)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the lowamountnumerunit is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "lowamountnumerunit", maExcipient.lowamountnumerunit.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                Ssi__cont_voc_PK unitInDb = _cv.NumeratorUnitList.Find(numeratorUnit => numeratorUnit.term_name_english.Trim() == maExcipient.lowamountnumerunit.Trim());
                                                if (unitInDb != null)
                                                {
                                                    dbExcipient.lowamountnumerunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                }
                                                else
                                                {
                                                    string message = "Value is not in Controlled Vocabulary.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                    exception.AddDescription(maExcipientLocation, "lowamountnumerunit", maExcipient.lowamountnumerunit.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        if (maExcipient.lowamountdenomunit != null)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the lowamountdenomunit is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "lowamountdenomunit", maExcipient.lowamountdenomunit.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                Ssi__cont_voc_PK unitInDb = _cv.DenominatorMeasureUnitList.Find(denominatorUnit => denominatorUnit.term_name_english.Trim() == maExcipient.lowamountdenomunit.Trim());
                                                if (unitInDb != null)
                                                {
                                                    dbExcipient.lowamountdenomunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                    dbExcipient.ExpressedBy_FK = _cv.ExpressedByUnitsOfMeasure != null ? _cv.ExpressedByUnitsOfMeasure.ssi__cont_voc_PK : null;
                                                }
                                                else
                                                {
                                                    unitInDb = _cv.DenominatorPresentationUnitList.Find(denominatorUnit => denominatorUnit.term_name_english.Trim() == maExcipient.lowamountdenomunit.Trim());
                                                    if (unitInDb != null)
                                                    {
                                                        dbExcipient.lowamountdenomunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                        dbExcipient.ExpressedBy_FK = _cv.ExpressedByUnitsOfPresentation != null ? _cv.ExpressedByUnitsOfPresentation.ssi__cont_voc_PK : null;
                                                    }
                                                    else
                                                    {
                                                        string message = "Value is not in Controlled Vocabulary.";
                                                        var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                        exception.AddDescription(maExcipientLocation, "lowamountdenomunit", maExcipient.lowamountdenomunit.Trim());
                                                        _validationExceptions.Add(exception);
                                                    }
                                                }
                                            }
                                        }

                                        if (maExcipient.highamountnumerunit != null)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the highamountnumerunit is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "highamountnumerunit", maExcipient.highamountnumerunit.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                Ssi__cont_voc_PK unitInDb = _cv.NumeratorUnitList.Find(numeratorUnit => numeratorUnit.term_name_english.Trim() == maExcipient.highamountnumerunit.Trim());
                                                if (unitInDb != null)
                                                {
                                                    dbExcipient.higamountnumerunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                }
                                                else
                                                {
                                                    string message = "Value is not in Controlled Vocabulary.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                    exception.AddDescription(maExcipientLocation, "highamountnumerunit", maExcipient.highamountnumerunit.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        if (maExcipient.highamountdenomunit != null)
                                        {
                                            if (allowOnlySubstanceCode)
                                            {
                                                string message = "Excipient can contain only substancecode, the highamountdenomunit is not allowed.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maExcipientLocation, "highamountdenomunit", maExcipient.highamountdenomunit.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                Ssi__cont_voc_PK unitInDb = _cv.DenominatorMeasureUnitList.Find(denominatorUnit => denominatorUnit.term_name_english.Trim() == maExcipient.highamountdenomunit.Trim());
                                                if (unitInDb != null)
                                                {
                                                    dbExcipient.highamountdenomunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                    dbExcipient.ExpressedBy_FK = _cv.ExpressedByUnitsOfMeasure != null ? _cv.ExpressedByUnitsOfMeasure.ssi__cont_voc_PK : null;
                                                }
                                                else
                                                {
                                                    unitInDb = _cv.DenominatorPresentationUnitList.Find(denominatorUnit => denominatorUnit.term_name_english.Trim() == maExcipient.highamountdenomunit.Trim());
                                                    if (unitInDb != null)
                                                    {
                                                        dbExcipient.highamountdenomunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                        dbExcipient.ExpressedBy_FK = _cv.ExpressedByUnitsOfPresentation != null ? _cv.ExpressedByUnitsOfPresentation.ssi__cont_voc_PK : null;
                                                    }
                                                    else
                                                    {
                                                        string message = "Value is not in Controlled Vocabulary.";
                                                        var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                        exception.AddDescription(maExcipientLocation, "highamountdenomunit", maExcipient.highamountdenomunit.Trim());
                                                        _validationExceptions.Add(exception);
                                                    }
                                                }
                                            }
                                        }

                                        dbExcipient.concise = ConstructConcise(dbExcipient);

                                        pharmaceuticalProductStruct.ExcipientList.Add(dbExcipient);
                                    }
                                }
                                #endregion

                                #region Adjuvants

                                if (maPharmaceuticalProduct.adjuvants != null && maPharmaceuticalProduct.adjuvants.adjuvant != null && maPharmaceuticalProduct.adjuvants.adjuvant.Count > 0)
                                {
                                    List<pharmaceuticalproductType.adjuvantsLocalType.adjuvantLocalType> maAdjuvantList = maPharmaceuticalProduct.adjuvants.adjuvant.ToList();

                                    for (int maAdjuvantIndex = 0; maAdjuvantIndex < maAdjuvantList.Count; maAdjuvantIndex++)
                                    {
                                        string maAdjuvantLocation = string.Format("{0}.adjuvants.adjuvant[{1}]", maPPLocation, maAdjuvantIndex);

                                        pharmaceuticalproductType.adjuvantsLocalType.adjuvantLocalType maAdjuvant = maAdjuvantList[maAdjuvantIndex];

                                        Adjuvant_PK dbAdjuvant = new Adjuvant_PK();

                                        if (maAdjuvant.concentrationtypecode != null)
                                        {
                                            Ssi__cont_voc_PK concTypeInDb = _cv.ConcentrationTypeList.Find(concType => concType.Evcode.Trim() == maAdjuvant.concentrationtypecode.ToString());
                                            if (concTypeInDb != null)
                                            {
                                                dbAdjuvant.concentrationtypecode = concTypeInDb.ssi__cont_voc_PK;
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maAdjuvantLocation, "concentrationtypecode", maAdjuvant.concentrationtypecode.ToString());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maAdjuvant.substancecode != null)
                                        {
                                            if (maAdjuvant.substancecode.resolutionmode != 2)
                                            {
                                                string message = "Value of resolutionmode must be '2'. Insert for Substance is not supported.";
                                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                exception.AddDescription(maAdjuvantLocation, "substancecode arg: resolutionmode", maAdjuvant.substancecode.resolutionmode.ToString());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrWhiteSpace(maAdjuvant.substancecode.TypedValue))
                                                {
                                                    Substance_PK substanceInDb = _cv.SubstanceList.Find(substance => substance.ev_code.Trim() == maAdjuvant.substancecode.TypedValue.Trim());
                                                    if (substanceInDb != null)
                                                    {
                                                        dbAdjuvant.substancecode_FK = substanceInDb.substance_PK;
                                                    }
                                                    else
                                                    {
                                                        string message = "Value is not in Controlled Vocabulary.";
                                                        var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                        exception.AddDescription(maAdjuvantLocation, "substancecode", maAdjuvant.substancecode.TypedValue.Trim());
                                                        _validationExceptions.Add(exception);
                                                    }
                                                }
                                                else
                                                {
                                                    string substancecode = maAdjuvant.substancecode.TypedValue ?? string.Empty;

                                                    string message = "Invalid value. Value must be specified.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                    exception.AddDescription(maAdjuvantLocation, "substancecode", substancecode.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        dbAdjuvant.lowamountnumervalue = maAdjuvant.lowamountnumervalue;
                                        dbAdjuvant.lowamountdenomvalue = maAdjuvant.lowamountdenomvalue;
                                        dbAdjuvant.highamountnumervalue = maAdjuvant.highamountnumervalue;
                                        dbAdjuvant.highamountdenomvalue = maAdjuvant.highamountdenomvalue;

                                        if (maAdjuvant.lowamountnumerprefix != null)
                                        {
                                            Ssi__cont_voc_PK prefixInDb = _cv.PrefixList.Find(prefix => prefix.term_name_english.Trim() == maAdjuvant.lowamountnumerprefix.Trim());
                                            if (prefixInDb != null)
                                            {
                                                dbAdjuvant.lowamountnumerprefix = prefixInDb.ssi__cont_voc_PK.ToString();
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maAdjuvantLocation, "lowamountnumerprefix", maAdjuvant.lowamountnumerprefix.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maAdjuvant.lowamountdenomprefix != null)
                                        {
                                            Ssi__cont_voc_PK prefixInDb = _cv.PrefixList.Find(prefix => prefix.term_name_english.Trim() == maAdjuvant.lowamountdenomprefix.Trim());
                                            if (prefixInDb != null)
                                            {
                                                dbAdjuvant.lowamountdenomprefix = prefixInDb.ssi__cont_voc_PK.ToString();
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maAdjuvantLocation, "lowamountdenomprefix", maAdjuvant.lowamountdenomprefix.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maAdjuvant.highamountnumerprefix != null)
                                        {
                                            Ssi__cont_voc_PK prefixInDb = _cv.PrefixList.Find(prefix => prefix.term_name_english.Trim() == maAdjuvant.highamountnumerprefix.Trim());
                                            if (prefixInDb != null)
                                            {
                                                dbAdjuvant.highamountnumerprefix = prefixInDb.ssi__cont_voc_PK.ToString();
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maAdjuvantLocation, "highamountnumerprefix", maAdjuvant.highamountnumerprefix.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maAdjuvant.highamountdenomprefix != null)
                                        {
                                            Ssi__cont_voc_PK prefixInDb = _cv.PrefixList.Find(prefix => prefix.term_name_english.Trim() == maAdjuvant.highamountdenomprefix.Trim());
                                            if (prefixInDb != null)
                                            {
                                                dbAdjuvant.highamountdenomprefix = prefixInDb.ssi__cont_voc_PK.ToString();
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maAdjuvantLocation, "highamountdenomprefix", maAdjuvant.highamountdenomprefix.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maAdjuvant.lowamountnumerunit != null)
                                        {
                                            Ssi__cont_voc_PK unitInDb = _cv.NumeratorUnitList.Find(numeratorUnit => numeratorUnit.term_name_english.Trim() == maAdjuvant.lowamountnumerunit.Trim());
                                            if (unitInDb != null)
                                            {
                                                dbAdjuvant.lowamountnumerunit = unitInDb.ssi__cont_voc_PK.ToString();
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maAdjuvantLocation, "lowamountnumerunit", maAdjuvant.lowamountnumerunit.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maAdjuvant.lowamountdenomunit != null)
                                        {
                                            Ssi__cont_voc_PK unitInDb = _cv.DenominatorMeasureUnitList.Find(denominatorUnit => denominatorUnit.term_name_english.Trim() == maAdjuvant.lowamountdenomunit.Trim());
                                            if (unitInDb != null)
                                            {
                                                dbAdjuvant.lowamountdenomunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                dbAdjuvant.ExpressedBy_FK = _cv.ExpressedByUnitsOfMeasure != null ? _cv.ExpressedByUnitsOfMeasure.ssi__cont_voc_PK : null;
                                            }
                                            else
                                            {
                                                unitInDb = _cv.DenominatorPresentationUnitList.Find(denominatorUnit => denominatorUnit.term_name_english.Trim() == maAdjuvant.lowamountdenomunit.Trim());
                                                if (unitInDb != null)
                                                {
                                                    dbAdjuvant.lowamountdenomunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                    dbAdjuvant.ExpressedBy_FK = _cv.ExpressedByUnitsOfPresentation != null ? _cv.ExpressedByUnitsOfPresentation.ssi__cont_voc_PK : null;
                                                }
                                                else
                                                {
                                                    string message = "Value is not in Controlled Vocabulary.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                    exception.AddDescription(maAdjuvantLocation, "lowamountdenomunit", maAdjuvant.lowamountdenomunit.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        if (maAdjuvant.highamountnumerunit != null)
                                        {
                                            Ssi__cont_voc_PK unitInDb = _cv.NumeratorUnitList.Find(numeratorUnit => numeratorUnit.term_name_english.Trim() == maAdjuvant.highamountnumerunit.Trim());
                                            if (unitInDb != null)
                                            {
                                                dbAdjuvant.higamountnumerunit = unitInDb.ssi__cont_voc_PK.ToString();
                                            }
                                            else
                                            {
                                                string message = "Value is not in Controlled Vocabulary.";
                                                var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                exception.AddDescription(maAdjuvantLocation, "highamountnumerunit", maAdjuvant.highamountnumerunit.Trim());
                                                _validationExceptions.Add(exception);
                                            }
                                        }

                                        if (maAdjuvant.highamountdenomunit != null)
                                        {
                                            Ssi__cont_voc_PK unitInDb = _cv.DenominatorMeasureUnitList.Find(denominatorUnit => denominatorUnit.term_name_english.Trim() == maAdjuvant.highamountdenomunit.Trim());
                                            if (unitInDb != null)
                                            {
                                                dbAdjuvant.highamountdenomunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                dbAdjuvant.ExpressedBy_FK = _cv.ExpressedByUnitsOfMeasure != null ? _cv.ExpressedByUnitsOfMeasure.ssi__cont_voc_PK : null;
                                            }
                                            else
                                            {
                                                unitInDb = _cv.DenominatorPresentationUnitList.Find(denominatorUnit => denominatorUnit.term_name_english.Trim() == maAdjuvant.highamountdenomunit.Trim());
                                                if (unitInDb != null)
                                                {
                                                    dbAdjuvant.highamountdenomunit = unitInDb.ssi__cont_voc_PK.ToString();
                                                    dbAdjuvant.ExpressedBy_FK = _cv.ExpressedByUnitsOfPresentation != null ? _cv.ExpressedByUnitsOfPresentation.ssi__cont_voc_PK : null;
                                                }
                                                else
                                                {
                                                    string message = "Value is not in Controlled Vocabulary.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                    exception.AddDescription(maAdjuvantLocation, "highamountdenomunit", maAdjuvant.highamountdenomunit.Trim());
                                                    _validationExceptions.Add(exception);
                                                }
                                            }
                                        }

                                        dbAdjuvant.concise = ConstructConcise(dbAdjuvant);

                                        pharmaceuticalProductStruct.AdjuvantList.Add(dbAdjuvant);
                                    }
                                }
                                #endregion

                                #region Administration routes

                                if (maPharmaceuticalProduct.adminroutes != null && maPharmaceuticalProduct.adminroutes.adminroute != null && maPharmaceuticalProduct.adminroutes.adminroute.Count > 0)
                                {
                                    List<pharmaceuticalproductType.adminroutesLocalType.adminrouteLocalType> maAdminRouteList = maPharmaceuticalProduct.adminroutes.adminroute.ToList();

                                    for (int maAdminRouteIndex = 0; maAdminRouteIndex < maAdminRouteList.Count; maAdminRouteIndex++)
                                    {
                                        string maAdminRouteLocation = string.Format("{0}.adminroutes.adminroute[{1}]", maPPLocation, maAdminRouteIndex);

                                        pharmaceuticalproductType.adminroutesLocalType.adminrouteLocalType maAdminRoute = maAdminRouteList[maAdminRouteIndex];

                                        Pp_ar_mn_PK dbAdminRoutePPMN = new Pp_ar_mn_PK();

                                        if (maAdminRoute.adminroutecode != null)
                                        {
                                            if (maAdminRoute.adminroutecode.resolutionmode != 2)
                                            {
                                                string message = "Value of resolutionmode must be '2'. Insert for Administration Route is not supported.";
                                                var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                exception.AddDescription(maAdminRouteLocation, "adminroutecode arg: resolutionmode", maAdminRoute.adminroutecode.resolutionmode.ToString());
                                                _validationExceptions.Add(exception);
                                            }
                                            else
                                            {
                                                if (!string.IsNullOrWhiteSpace(maAdminRoute.adminroutecode.TypedValue))
                                                {
                                                    Adminroute_PK adminRouteInDb = _cv.AdminRouteList.Find(adminRoute => adminRoute.ev_code.Trim() == maAdminRoute.adminroutecode.TypedValue.Trim());
                                                    if (adminRouteInDb != null)
                                                    {
                                                        dbAdminRoutePPMN.admin_route_FK = adminRouteInDb.adminroute_PK;
                                                    }
                                                    else
                                                    {
                                                        string message = "Value is not in Controlled Vocabulary.";
                                                        var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                                        exception.AddDescription(maAdminRouteLocation, "adminroutecode", maAdminRoute.adminroutecode.TypedValue.Trim());
                                                        _validationExceptions.Add(exception);
                                                    }
                                                }
                                                else
                                                {
                                                    string adminroutecode = maAdminRoute.adminroutecode.TypedValue ?? string.Empty;

                                                    string message = "Invalid value. Value must be specified.";
                                                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Error);
                                                    exception.AddDescription(maAdminRouteLocation, "adminroutecode", adminroutecode.Trim());
                                                    _validationExceptions.Add(exception);
                                                }

                                            }
                                        }

                                        pharmaceuticalProductStruct.AdminRoutePPMNList.Add(dbAdminRoutePPMN);
                                    }
                                }
                                #endregion

                                #region Medical devices

                                if (maPharmaceuticalProduct.medicaldevices != null && maPharmaceuticalProduct.medicaldevices.medicaldevice != null && maPharmaceuticalProduct.medicaldevices.medicaldevice.Count > 0)
                                {
                                    List<pharmaceuticalproductType.medicaldevicesLocalType.medicaldeviceLocalType> maMedicalDeviceList = maPharmaceuticalProduct.medicaldevices.medicaldevice.ToList();

                                    for (int maMedicalDeviceIndex = 0; maMedicalDeviceIndex < maMedicalDeviceList.Count; maMedicalDeviceIndex++)
                                    {
                                        string maMedicalDeviceLocation = string.Format("{0}.medicaldevices.medicaldevice[{1}]", maPPLocation, maMedicalDeviceIndex);

                                        pharmaceuticalproductType.medicaldevicesLocalType.medicaldeviceLocalType maMedicalDevice = maMedicalDeviceList[maMedicalDeviceIndex];

                                        Pp_md_mn_PK dbMedicalDevicePPMN = new Pp_md_mn_PK();

                                        Medicaldevice_PK medicalDeviceInDb = _cv.MedicalDeviceList.Find(medicalDevice => medicalDevice.ev_code.Trim() == maMedicalDevice.medicaldevicecode.ToString());
                                        if (medicalDeviceInDb != null)
                                        {
                                            dbMedicalDevicePPMN.pp_medical_device_FK = medicalDeviceInDb.medicaldevice_PK;
                                        }
                                        else
                                        {
                                            string message = "Value is not in Controlled Vocabulary.";
                                            var exception = new ValidationException(message, ValidationExceptionType.ValueNotInCV, SeverityType.Error);
                                            exception.AddDescription(maMedicalDeviceLocation, "medicaldevicecode", maMedicalDevice.medicaldevicecode.ToString());
                                            _validationExceptions.Add(exception);
                                        }

                                        pharmaceuticalProductStruct.MedicalDevicePPMNList.Add(dbMedicalDevicePPMN);
                                    }
                                }
                                #endregion

                                dbPharmaceuticalProduct.name = ConstructPharmaceuticalProductName(pharmaceuticalProductStruct);

                                productStruct.PharmaceuticalProductStructList.Add(pharmaceuticalProductStruct);
                            }
                        }
                        #endregion

                        authorisedProductStruct.ProductStruct = productStruct;
                        _maReadyStruct.AuthorisedProductStructList.Add(authorisedProductStruct);
                    }
                }
                else
                {
                    string message = "Authorised product section is empty. Number of entities inserted: 0.";
                    var exception = new ValidationException(message, ValidationExceptionType.InvalidValue, SeverityType.Warning);
                    exception.AddDescription("marketingauthorisation", "authorisedproducts", string.Empty);
                    _validationExceptions.Add(exception);
                }
                #endregion

                if (_maDataExporterMode == MADataExporterModeType.ValidateAndExportToDB)
                {
                    if ((_maSaveOptions.IgnoreValidationExceptions || _validationExceptions.Count == 0) &&
                        (_maSaveOptions.IgnoreErrorsInValidationProcess || _exceptions.Count == 0))
                    {
                        SaveMAReadyStructToDB();
                    }
                }
            }
            catch (Exception ex)
            {
                _exceptions.Add(ex);
                return false;
            }

            if (_validationExceptions.Count > 0 || _exceptions.Count > 0 || _saveExceptions.Count > 0)
            {
                return false;
            }
            return true;
        }

        private string ConstructConcise(Activeingredient_PK activeIngredient)
        {
            string concise = "N/A";

            if (activeIngredient == null) return concise;

            string substanceName = string.Empty;
            string numMinValue = string.Empty;
            string numMinPrefix = string.Empty;
            string numMaxValue = string.Empty;
            string numMaxPrefix = string.Empty;
            string numUnit = string.Empty;
            string denMinValue = string.Empty;
            string denMinPrefix = string.Empty;
            string denMaxValue = string.Empty;
            string denMaxPrefix = string.Empty;
            string denUnit = string.Empty;

            try
            {
                if (activeIngredient.substancecode_FK != null)
                {
                    //Substance_PK substance = _substance_PKOperations.GetEntity(activeIngredient.substancecode_FK);
                    Substance_PK substance = _cv.SubstanceList.Find(item => item.substance_PK == activeIngredient.substancecode_FK);

                    if (substance != null && !string.IsNullOrWhiteSpace(substance.substance_name))
                    {
                        substanceName = substance.substance_name.Trim();
                    }
                }

                bool isRange = false;
                bool hasConcentrationType = false;
                bool isExpressedByUnitsOfMeasure = false;

                if (activeIngredient.concentrationtypecode != null)
                {
                    //Ssi__cont_voc_PK concentrationType = _ssi__cont_voc_PKOperations.GetEntity(activeIngredient.concentrationtypecode);
                    Ssi__cont_voc_PK concentrationType = _cv.ConcentrationTypeList.Find(item => item.ssi__cont_voc_PK == activeIngredient.concentrationtypecode);

                    if (concentrationType != null)
                    {
                        int evcode;
                        if (int.TryParse(concentrationType.Evcode, out evcode))
                        {
                            hasConcentrationType = true;

                            if (concentrationType.Evcode == "2")
                            {
                                isRange = true;
                            }
                        }
                    }
                }

                if (activeIngredient.ExpressedBy_FK != null)
                {
                    //Ssi__cont_voc_PK expressedBy = _ssi__cont_voc_PKOperations.GetEntity(activeIngredient.ExpressedBy_FK);
                    Ssi__cont_voc_PK expressedBy = _cv.ExpressedByList.Find(item => item.ssi__cont_voc_PK == activeIngredient.ExpressedBy_FK);

                    if (expressedBy != null && expressedBy.term_name_english != null && expressedBy.term_name_english.Trim().ToLower() == "units of measure")
                    {
                        isExpressedByUnitsOfMeasure = true;
                    }
                }

                if (hasConcentrationType)
                {
                    if (isRange)
                    {
                        string valueFormat = "#0.#####";

                        numMinValue = activeIngredient.lowamountnumervalue.HasValue ? activeIngredient.lowamountnumervalue.Value.ToString(valueFormat) : string.Empty;
                        numMaxValue = activeIngredient.highamountnumervalue.HasValue ? activeIngredient.highamountnumervalue.Value.ToString(valueFormat) : string.Empty;

                        if (isExpressedByUnitsOfMeasure)
                        {
                            denMinValue = activeIngredient.lowamountdenomvalue.HasValue ? activeIngredient.lowamountdenomvalue.Value.ToString(valueFormat) : string.Empty;
                            denMaxValue = denMinValue;
                        }
                        else
                        {
                            denMinValue = activeIngredient.lowamountdenomvalue.HasValue ? ((int)activeIngredient.lowamountdenomvalue.Value).ToString() : string.Empty;
                            denMaxValue = denMinValue;
                        }


                        if (IsValidInt(activeIngredient.lowamountnumerprefix))
                        {
                            //Ssi__cont_voc_PK prefix = _ssi__cont_voc_PKOperations.GetEntity(activeIngredient.lowamountnumerprefix);
                            Ssi__cont_voc_PK prefix = _cv.PrefixList.Find(item => item.ssi__cont_voc_PK == int.Parse(activeIngredient.lowamountnumerprefix));

                            if (prefix != null && prefix.Field8 != null)
                            {
                                numMinPrefix = prefix.Field8.Trim();
                                if (prefix.Description != null && prefix.Description.Trim().ToLower() == "single") numMinPrefix = string.Empty;
                            }
                        }

                        if (IsValidInt(activeIngredient.lowamountdenomprefix))
                        {
                            //Ssi__cont_voc_PK prefix = _ssi__cont_voc_PKOperations.GetEntity(activeIngredient.lowamountdenomprefix);
                            Ssi__cont_voc_PK prefix = _cv.PrefixList.Find(item => item.ssi__cont_voc_PK == int.Parse(activeIngredient.lowamountdenomprefix));

                            if (prefix != null && prefix.Field8 != null)
                            {
                                denMinPrefix = prefix.Field8.Trim();
                                if (prefix.Description != null && prefix.Description.Trim().ToLower() == "single") denMinPrefix = string.Empty;
                            }
                        }

                        if (IsValidInt(activeIngredient.highamountnumerprefix))
                        {
                            //Ssi__cont_voc_PK prefix = _ssi__cont_voc_PKOperations.GetEntity(activeIngredient.highamountnumerprefix);
                            Ssi__cont_voc_PK prefix = _cv.PrefixList.Find(item => item.ssi__cont_voc_PK == int.Parse(activeIngredient.highamountnumerprefix));

                            if (prefix != null && prefix.Field8 != null)
                            {
                                numMaxPrefix = prefix.Field8.Trim();
                                if (prefix.Description != null && prefix.Description.Trim().ToLower() == "single") numMaxPrefix = string.Empty;
                            }
                        }

                        if (IsValidInt(activeIngredient.highamountdenomprefix))
                        {
                            //Ssi__cont_voc_PK prefix = _ssi__cont_voc_PKOperations.GetEntity(activeIngredient.highamountdenomprefix);
                            Ssi__cont_voc_PK prefix = _cv.PrefixList.Find(item => item.ssi__cont_voc_PK == int.Parse(activeIngredient.highamountdenomprefix));

                            if (prefix != null && prefix.Field8 != null)
                            {
                                denMaxPrefix = prefix.Field8.Trim();
                                if (prefix.Description != null && prefix.Description.Trim().ToLower() == "single") denMaxPrefix = string.Empty;
                            }
                        }

                        denMaxPrefix = denMinPrefix;

                        if (IsValidInt(activeIngredient.lowamountnumerunit))
                        {
                            //Ssi__cont_voc_PK unit = _ssi__cont_voc_PKOperations.GetEntity(activeIngredient.lowamountnumerunit);
                            Ssi__cont_voc_PK unit = _cv.UnitList.Find(item => item.ssi__cont_voc_PK == int.Parse(activeIngredient.lowamountnumerunit));

                            if (unit != null && unit.Field8 != null)
                            {
                                numUnit = unit.Field8.Trim();
                            }
                        }

                        if (IsValidInt(activeIngredient.lowamountdenomunit))
                        {
                            //Ssi__cont_voc_PK unit = _ssi__cont_voc_PKOperations.GetEntity(activeIngredient.lowamountdenomunit);
                            Ssi__cont_voc_PK unit = _cv.UnitList.Find(item => item.ssi__cont_voc_PK == int.Parse(activeIngredient.lowamountdenomunit));

                            if (isExpressedByUnitsOfMeasure)
                            {
                                if (unit != null && unit.Field8 != null)
                                {
                                    denUnit = unit.Field8.Trim();
                                }
                            }
                            else
                            {
                                if (unit != null && unit.Description != null)
                                {
                                    denUnit = unit.Description.Trim();

                                    if (denUnit.ToLower() == "each") denUnit = string.Empty;
                                }
                            }
                        }

                        string conciseNumPart = string.Empty;

                        if (numMinValue == numMaxValue)
                        {
                            if (numMinPrefix == numMaxPrefix)
                            {
                                conciseNumPart = string.Format("{0} {1} {2}{3}", substanceName, numMinValue, numMinPrefix, numUnit);
                            }
                            else
                            {
                                conciseNumPart = string.Format("{0} {1} {2} - {3} {4}{5}", substanceName, numMinValue, numMinPrefix, numMaxValue, numMaxPrefix, numUnit);
                            }
                        }
                        else
                        {
                            if (numMinPrefix == numMaxPrefix)
                            {
                                conciseNumPart = string.Format("{0} {1} - {2} {3}{4}", substanceName, numMinValue, numMaxValue, numMaxPrefix, numUnit);
                            }
                            else
                            {
                                conciseNumPart = string.Format("{0} {1} {2} - {3} {4}{5}", substanceName, numMinValue, numMinPrefix, numMinValue, numMaxPrefix, numUnit);
                            }
                        }

                        string conciseDenPart = string.Empty;

                        if (denMinValue == "1")
                        {
                            conciseDenPart = conciseDenPart = string.Format("{0}{1}", denMinPrefix, denUnit);
                        }
                        else
                        {
                            conciseDenPart = conciseDenPart = string.Format("{0} {1}{2}", denMinValue, denMinPrefix, denUnit);
                        }

                        concise = string.Format("{0}/{1}", conciseNumPart, conciseDenPart);
                    }
                    else
                    {
                        string valueFormat = "#0.#####";

                        numMinValue = activeIngredient.lowamountnumervalue.HasValue ? activeIngredient.lowamountnumervalue.Value.ToString(valueFormat) : string.Empty;

                        if (isExpressedByUnitsOfMeasure)
                        {
                            denMinValue = activeIngredient.lowamountdenomvalue.HasValue ? activeIngredient.lowamountdenomvalue.Value.ToString(valueFormat) : string.Empty;
                        }
                        else
                        {
                            denMinValue = activeIngredient.lowamountdenomvalue.HasValue ? ((int)activeIngredient.lowamountdenomvalue.Value).ToString() : string.Empty;
                        }

                        if (IsValidInt(activeIngredient.lowamountnumerprefix))
                        {
                            //Ssi__cont_voc_PK prefix = _ssi__cont_voc_PKOperations.GetEntity(activeIngredient.lowamountnumerprefix);
                            Ssi__cont_voc_PK prefix = _cv.PrefixList.Find(item => item.ssi__cont_voc_PK == int.Parse(activeIngredient.lowamountnumerprefix));

                            if (prefix != null && prefix.Field8 != null)
                            {
                                numMinPrefix = prefix.Field8.Trim();
                                if (prefix.Description != null && prefix.Description.Trim().ToLower() == "single") numMinPrefix = string.Empty;
                            }
                        }

                        if (IsValidInt(activeIngredient.lowamountdenomprefix))
                        {
                            //Ssi__cont_voc_PK prefix = _ssi__cont_voc_PKOperations.GetEntity(activeIngredient.lowamountdenomprefix);
                            Ssi__cont_voc_PK prefix = _cv.PrefixList.Find(item => item.ssi__cont_voc_PK == int.Parse(activeIngredient.lowamountdenomprefix));

                            if (prefix != null && prefix.Field8 != null)
                            {
                                denMinPrefix = prefix.Field8.Trim();
                                if (prefix.Description != null && prefix.Description.Trim().ToLower() == "single") denMinPrefix = string.Empty;
                            }
                        }

                        if (IsValidInt(activeIngredient.lowamountnumerunit))
                        {
                            //Ssi__cont_voc_PK unit = _ssi__cont_voc_PKOperations.GetEntity(activeIngredient.lowamountnumerunit);
                            Ssi__cont_voc_PK unit = _cv.UnitList.Find(item => item.ssi__cont_voc_PK == int.Parse(activeIngredient.lowamountnumerunit));

                            if (unit != null && unit.Field8 != null)
                            {
                                numUnit = unit.Field8.Trim();
                            }
                        }

                        if (IsValidInt(activeIngredient.lowamountdenomunit))
                        {
                            //Ssi__cont_voc_PK unit = _ssi__cont_voc_PKOperations.GetEntity(activeIngredient.lowamountdenomunit);
                            Ssi__cont_voc_PK unit = _cv.UnitList.Find(item => item.ssi__cont_voc_PK == int.Parse(activeIngredient.lowamountdenomunit));

                            if (isExpressedByUnitsOfMeasure)
                            {
                                if (unit != null && unit.Field8 != null)
                                {
                                    denUnit = unit.Field8.Trim();
                                }
                            }
                            else
                            {
                                if (unit != null && unit.Description != null)
                                {
                                    denUnit = unit.Description.Trim();

                                    if (denUnit.ToLower() == "each") denUnit = string.Empty;
                                }
                            }
                        }

                        if (denMinValue == "1")
                        {
                            concise = string.Format("{0} {1} {2}{3}/{4}{5}", substanceName, numMinValue, numMinPrefix, numUnit, denMinPrefix, denUnit);
                        }
                        else
                        {
                            concise = string.Format("{0} {1} {2}{3}/{4} {5}{6}", substanceName, numMinValue, numMinPrefix, numUnit, denMinValue, denMinPrefix, denUnit);
                        }
                    }
                }
                else
                {
                    concise = substanceName;
                }
            }
            catch (Exception ex)
            {
                _exceptions.Add(ex);
            }

            if (string.IsNullOrWhiteSpace(concise))
            {
                concise = "N/A";
            }

            return concise;
        }

        private string ConstructConcise(Excipient_PK excipient)
        {
            string concise = "N/A";

            if (excipient == null) return concise;

            string substanceName = string.Empty;

            try
            {
                if (excipient.substancecode_FK != null)
                {
                    //Substance_PK substance = _substance_PKOperations.GetEntity(excipient.substancecode_FK);
                    Substance_PK substance = _cv.SubstanceList.Find(item => item.substance_PK == excipient.substancecode_FK);

                    substanceName = substance.substance_name;
                }
            }
            catch (Exception ex)
            {
                _exceptions.Add(ex);
            }

            concise = substanceName;

            if (string.IsNullOrWhiteSpace(concise))
            {
                concise = "N/A";
            }

            return concise;
        }

        private string ConstructConcise(Adjuvant_PK adjuvant)
        {
            string concise = "N/A";

            if (adjuvant == null) return concise;

            string substanceName = string.Empty;
            string numMinValue = string.Empty;
            string numMinPrefix = string.Empty;
            string numMaxValue = string.Empty;
            string numMaxPrefix = string.Empty;
            string numUnit = string.Empty;
            string denMinValue = string.Empty;
            string denMinPrefix = string.Empty;
            string denMaxValue = string.Empty;
            string denMaxPrefix = string.Empty;
            string denUnit = string.Empty;

            try
            {
                if (adjuvant.substancecode_FK != null)
                {
                    //Substance_PK substance = _substance_PKOperations.GetEntity(adjuvant.substancecode_FK);
                    Substance_PK substance = _cv.SubstanceList.Find(item => item.substance_PK == adjuvant.substancecode_FK);

                    if (substance != null && !string.IsNullOrWhiteSpace(substance.substance_name))
                    {
                        substanceName = substance.substance_name.Trim();
                    }
                }

                bool isRange = false;
                bool hasConcentrationType = false;
                bool isExpressedByUnitsOfMeasure = false;

                if (adjuvant.concentrationtypecode != null)
                {
                    //Ssi__cont_voc_PK concentrationType = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.concentrationtypecode);
                    Ssi__cont_voc_PK concentrationType = _cv.ConcentrationTypeList.Find(item => item.ssi__cont_voc_PK == adjuvant.concentrationtypecode);

                    if (concentrationType != null)
                    {
                        int evcode;
                        if (int.TryParse(concentrationType.Evcode, out evcode))
                        {
                            hasConcentrationType = true;

                            if (concentrationType.Evcode == "2")
                            {
                                isRange = true;
                            }
                        }
                    }
                }

                if (adjuvant.ExpressedBy_FK != null)
                {
                    //Ssi__cont_voc_PK expressedBy = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.ExpressedBy_FK);
                    Ssi__cont_voc_PK expressedBy = _cv.ExpressedByList.Find(item => item.ssi__cont_voc_PK == adjuvant.ExpressedBy_FK);

                    if (expressedBy != null && expressedBy.term_name_english != null && expressedBy.term_name_english.Trim().ToLower() == "units of measure")
                    {
                        isExpressedByUnitsOfMeasure = true;
                    }
                }

                if (hasConcentrationType)
                {
                    if (isRange)
                    {
                        string valueFormat = "#0.#####";

                        numMinValue = adjuvant.lowamountnumervalue.HasValue ? adjuvant.lowamountnumervalue.Value.ToString(valueFormat) : string.Empty;
                        numMaxValue = adjuvant.highamountnumervalue.HasValue ? adjuvant.highamountnumervalue.Value.ToString(valueFormat) : string.Empty;

                        if (isExpressedByUnitsOfMeasure)
                        {
                            denMinValue = adjuvant.lowamountdenomvalue.HasValue ? adjuvant.lowamountdenomvalue.Value.ToString(valueFormat) : string.Empty;
                            denMaxValue = denMinValue;
                        }
                        else
                        {
                            denMinValue = adjuvant.lowamountdenomvalue.HasValue ? ((int)adjuvant.lowamountdenomvalue.Value).ToString() : string.Empty;
                            denMaxValue = denMinValue;
                        }


                        if (IsValidInt(adjuvant.lowamountnumerprefix))
                        {
                            //Ssi__cont_voc_PK prefix = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.lowamountnumerprefix);
                            Ssi__cont_voc_PK prefix = _cv.PrefixList.Find(item => item.ssi__cont_voc_PK == int.Parse(adjuvant.lowamountnumerprefix));

                            if (prefix != null && prefix.Field8 != null)
                            {
                                numMinPrefix = prefix.Field8.Trim();
                                if (prefix.Description != null && prefix.Description.Trim().ToLower() == "single") numMinPrefix = string.Empty;
                            }
                        }

                        if (IsValidInt(adjuvant.lowamountdenomprefix))
                        {
                            //Ssi__cont_voc_PK prefix = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.lowamountdenomprefix);
                            Ssi__cont_voc_PK prefix = _cv.PrefixList.Find(item => item.ssi__cont_voc_PK == int.Parse(adjuvant.lowamountdenomprefix));

                            if (prefix != null && prefix.Field8 != null)
                            {
                                denMinPrefix = prefix.Field8.Trim();
                                if (prefix.Description != null && prefix.Description.Trim().ToLower() == "single") denMinPrefix = string.Empty;
                            }
                        }

                        if (IsValidInt(adjuvant.highamountnumerprefix))
                        {
                            //Ssi__cont_voc_PK prefix = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.highamountnumerprefix);
                            Ssi__cont_voc_PK prefix = _cv.PrefixList.Find(item => item.ssi__cont_voc_PK == int.Parse(adjuvant.highamountnumerprefix));

                            if (prefix != null && prefix.Field8 != null)
                            {
                                numMaxPrefix = prefix.Field8.Trim();
                                if (prefix.Description != null && prefix.Description.Trim().ToLower() == "single") numMaxPrefix = string.Empty;
                            }
                        }

                        if (IsValidInt(adjuvant.highamountdenomprefix))
                        {
                            //Ssi__cont_voc_PK prefix = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.highamountdenomprefix);
                            Ssi__cont_voc_PK prefix = _cv.PrefixList.Find(item => item.ssi__cont_voc_PK == int.Parse(adjuvant.highamountdenomprefix));

                            if (prefix != null && prefix.Field8 != null)
                            {
                                denMaxPrefix = prefix.Field8.Trim();
                                if (prefix.Description != null && prefix.Description.Trim().ToLower() == "single") denMaxPrefix = string.Empty;
                            }
                        }

                        denMaxPrefix = denMinPrefix;

                        if (IsValidInt(adjuvant.lowamountnumerunit))
                        {
                            //Ssi__cont_voc_PK unit = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.lowamountnumerunit);
                            Ssi__cont_voc_PK unit = _cv.UnitList.Find(item => item.ssi__cont_voc_PK == int.Parse(adjuvant.lowamountnumerunit));

                            if (unit != null && unit.Field8 != null)
                            {
                                numUnit = unit.Field8.Trim();
                            }
                        }

                        if (IsValidInt(adjuvant.lowamountdenomunit))
                        {
                            //Ssi__cont_voc_PK unit = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.lowamountdenomunit);
                            Ssi__cont_voc_PK unit = _cv.UnitList.Find(item => item.ssi__cont_voc_PK == int.Parse(adjuvant.lowamountdenomunit));

                            if (isExpressedByUnitsOfMeasure)
                            {
                                if (unit != null && unit.Field8 != null)
                                {
                                    denUnit = unit.Field8.Trim();
                                }
                            }
                            else
                            {
                                if (unit != null && unit.Description != null)
                                {
                                    denUnit = unit.Description.Trim();

                                    if (denUnit.ToLower() == "each") denUnit = string.Empty;
                                }
                            }
                        }

                        string conciseNumPart = string.Empty;

                        if (numMinValue == numMaxValue)
                        {
                            if (numMinPrefix == numMaxPrefix)
                            {
                                conciseNumPart = string.Format("{0} {1} {2}{3}", substanceName, numMinValue, numMinPrefix, numUnit);
                            }
                            else
                            {
                                conciseNumPart = string.Format("{0} {1} {2} - {3} {4}{5}", substanceName, numMinValue, numMinPrefix, numMaxValue, numMaxPrefix, numUnit);
                            }
                        }
                        else
                        {
                            if (numMinPrefix == numMaxPrefix)
                            {
                                conciseNumPart = string.Format("{0} {1} - {2} {3}{4}", substanceName, numMinValue, numMaxValue, numMaxPrefix, numUnit);
                            }
                            else
                            {
                                conciseNumPart = string.Format("{0} {1} {2} - {3} {4}{5}", substanceName, numMinValue, numMinPrefix, numMinValue, numMaxPrefix, numUnit);
                            }
                        }

                        string conciseDenPart = string.Empty;

                        if (denMinValue == "1")
                        {
                            conciseDenPart = conciseDenPart = string.Format("{0}{1}", denMinPrefix, denUnit);
                        }
                        else
                        {
                            conciseDenPart = conciseDenPart = string.Format("{0} {1}{2}", denMinValue, denMinPrefix, denUnit);
                        }

                        concise = string.Format("{0}/{1}", conciseNumPart, conciseDenPart);
                    }
                    else
                    {
                        string valueFormat = "#0.#####";

                        numMinValue = adjuvant.lowamountnumervalue.HasValue ? adjuvant.lowamountnumervalue.Value.ToString(valueFormat) : string.Empty;

                        if (isExpressedByUnitsOfMeasure)
                        {
                            denMinValue = adjuvant.lowamountdenomvalue.HasValue ? adjuvant.lowamountdenomvalue.Value.ToString(valueFormat) : string.Empty;
                        }
                        else
                        {
                            denMinValue = adjuvant.lowamountdenomvalue.HasValue ? ((int)adjuvant.lowamountdenomvalue.Value).ToString() : string.Empty;
                        }

                        if (IsValidInt(adjuvant.lowamountnumerprefix))
                        {
                            //Ssi__cont_voc_PK prefix = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.lowamountnumerprefix);
                            Ssi__cont_voc_PK prefix = _cv.PrefixList.Find(item => item.ssi__cont_voc_PK == int.Parse(adjuvant.lowamountnumerprefix));

                            if (prefix != null && prefix.Field8 != null)
                            {
                                numMinPrefix = prefix.Field8.Trim();
                                if (prefix.Description != null && prefix.Description.Trim().ToLower() == "single") numMinPrefix = string.Empty;
                            }
                        }

                        if (IsValidInt(adjuvant.lowamountdenomprefix))
                        {
                            //Ssi__cont_voc_PK prefix = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.lowamountdenomprefix);
                            Ssi__cont_voc_PK prefix = _cv.PrefixList.Find(item => item.ssi__cont_voc_PK == int.Parse(adjuvant.lowamountdenomprefix));

                            if (prefix != null && prefix.Field8 != null)
                            {
                                denMinPrefix = prefix.Field8.Trim();
                                if (prefix.Description != null && prefix.Description.Trim().ToLower() == "single") denMinPrefix = string.Empty;
                            }
                        }

                        if (IsValidInt(adjuvant.lowamountnumerunit))
                        {
                            //Ssi__cont_voc_PK unit = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.lowamountnumerunit);
                            Ssi__cont_voc_PK unit = _cv.UnitList.Find(item => item.ssi__cont_voc_PK == int.Parse(adjuvant.lowamountnumerunit));

                            if (unit != null && unit.Field8 != null)
                            {
                                numUnit = unit.Field8.Trim();
                            }
                        }

                        if (IsValidInt(adjuvant.lowamountdenomunit))
                        {
                            //Ssi__cont_voc_PK unit = _ssi__cont_voc_PKOperations.GetEntity(adjuvant.lowamountdenomunit);
                            Ssi__cont_voc_PK unit = _cv.UnitList.Find(item => item.ssi__cont_voc_PK == int.Parse(adjuvant.lowamountdenomunit));

                            if (isExpressedByUnitsOfMeasure)
                            {
                                if (unit != null && unit.Field8 != null)
                                {
                                    denUnit = unit.Field8.Trim();
                                }
                            }
                            else
                            {
                                if (unit != null && unit.Description != null)
                                {
                                    denUnit = unit.Description.Trim();

                                    if (denUnit.ToLower() == "each") denUnit = string.Empty;
                                }
                            }
                        }

                        if (denMinValue == "1")
                        {
                            concise = string.Format("{0} {1} {2}{3}/{4}{5}", substanceName, numMinValue, numMinPrefix, numUnit, denMinPrefix, denUnit);
                        }
                        else
                        {
                            concise = string.Format("{0} {1} {2}{3}/{4} {5}{6}", substanceName, numMinValue, numMinPrefix, numUnit, denMinValue, denMinPrefix, denUnit);
                        }
                    }
                }
                else
                {
                    concise = substanceName;
                }
            }
            catch (Exception ex)
            {
                _exceptions.Add(ex);
            }

            if (string.IsNullOrWhiteSpace(concise))
            {
                concise = "N/A";
            }

            return concise;
        }

        private string ConstructPharmaceuticalProductName(PharmaceuticalProductStruct pharmaceuticalProductStruct)
        {
            string ppName = string.Empty;

            try
            {
                int num = 0;
                foreach (Activeingredient_PK activeIngredient in pharmaceuticalProductStruct.ActiveIngredientList)
                {
                    if (string.IsNullOrWhiteSpace(activeIngredient.concise))
                    {
                        activeIngredient.concise = ConstructConcise(activeIngredient);
                    }

                    ppName += (num++) == 0 ? activeIngredient.concise.Trim() : "+" + activeIngredient.concise.Trim();
                }

                if (string.IsNullOrWhiteSpace(ppName))
                {
                    ppName = "N/A";
                }

                if (pharmaceuticalProductStruct.PharmaceuticalProduct != null && pharmaceuticalProductStruct.PharmaceuticalProduct.Pharmform_FK != null)
                {
                    //Pharmaceutical_form_PK pharmaceuticalForm = _pharmaceutical_form_PKOperations.GetEntity(pharmaceuticalProductStruct.PharmaceuticalProduct.Pharmform_FK);
                    Pharmaceutical_form_PK pharmaceuticalForm = _cv.PharmaceuticalFormList.Find(item => item.pharmaceutical_form_PK == pharmaceuticalProductStruct.PharmaceuticalProduct.Pharmform_FK);

                    if (pharmaceuticalForm != null && !string.IsNullOrWhiteSpace(pharmaceuticalForm.name))
                    {
                        ppName += " (" + pharmaceuticalForm.name + ")";
                    }
                }
            }
            catch (Exception ex)
            {
                _exceptions.Add(ex);
            }

            if (string.IsNullOrWhiteSpace(ppName))
            {
                ppName = "N/A";
            }

            return ppName;
        }

        private bool IsValidInt(string s)
        {
            bool isValid = false;
            int temp = 0;

            if (!String.IsNullOrEmpty(s))
            {
                if (int.TryParse(s, out temp)) isValid = true;
            }

            return isValid;
        }

        #endregion
    }
}
