using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ready.Model;
using System.Transactions;

namespace EVMessage.MarketingAuthorisation
{
    public partial class MADataExporter
    {
        public enum MADataExporterModeType
        {
            Validate,
            ValidateAndExportToDB
        }

        public class SaveOptions
        {
            private bool _ignoreValidationExceptions;
            private bool _ignoreErrorsInValidationProcess;
            private bool _rollbackAllAtSaveException;
            private bool _populateMAEntityMNTable;
            private bool _updateExistingEntities;

            public bool IgnoreValidationExceptions
            {
                get { return _ignoreValidationExceptions; }
                set { _ignoreValidationExceptions = value; }
            }
            
            public bool IgnoreErrorsInValidationProcess
            {
                get { return _ignoreErrorsInValidationProcess; }
                set { _ignoreErrorsInValidationProcess = value; }
            }
            
            public bool RollbackAllAtSaveException
            {
                get { return _rollbackAllAtSaveException; }
                set { _rollbackAllAtSaveException = value; }
            }
            
            public bool PopulateMAEntityMNTable
            {
                get { return _populateMAEntityMNTable; }
                set { _populateMAEntityMNTable = value; }
            }

            public bool UpdateExistingEntities
            {
                get { return _updateExistingEntities; }
                set { _updateExistingEntities = value; }
            }
        }

        private class MAReadyStruct
        {
            private List<AuthorisedProductStruct> _authorisedProductStructList;
            private List<DocumentStruct> _documentStructList;

            public List<AuthorisedProductStruct> AuthorisedProductStructList
            {
                get
                {
                    if (_authorisedProductStructList == null)
                    {
                        _authorisedProductStructList = new List<AuthorisedProductStruct>();
                    }

                    return _authorisedProductStructList;
                }
                set { _authorisedProductStructList = value; }
            }

            public List<DocumentStruct> DocumentStructList
            {
                get
                {
                    if (_documentStructList == null)
                    {
                        _documentStructList = new List<DocumentStruct>();
                    }

                    return _documentStructList;
                }
                set { _documentStructList = value; }
            }
        }

        private class AuthorisedProductStruct
        {
            private AuthorisedProduct _authorisedProduct;
            private ProductStruct _productStruct;
            private List<Meddra_pk> _meddraList;
            private QPPVPersonStruct _qppvPersonStruct;
            private List<DocumentStruct> _documentStructList;

            private Organization_PK _licenceHolder;
            private Organization_PK _masterFileLocation;
            private Organization_in_role_ _licenceHolderOrgInRole;
            private Organization_in_role_ _masterFileLocationOrgInRole;

            private string _localNumber;
            private XevprmOperationType _operationType;

            public AuthorisedProduct AuthorisedProduct
            {
                get { return _authorisedProduct; }
                set { _authorisedProduct = value; }
            }

            public ProductStruct ProductStruct
            {
                get
                {
                    if (_productStruct == null)
                    {
                        return new ProductStruct();
                    }

                    return _productStruct;
                }
                set { _productStruct = value; }
            }

            public List<Meddra_pk> MeddraList
            {
                get
                {
                    if (_meddraList == null)
                    {
                        _meddraList = new List<Meddra_pk>();
                    }

                    return _meddraList;
                }
                set { _meddraList = value; }
            }

            public QPPVPersonStruct QppvPersonStruct
            {
                get
                {
                    if (_qppvPersonStruct == null)
                    {
                        return new QPPVPersonStruct();
                    }
                    return _qppvPersonStruct;
                }
                set { _qppvPersonStruct = value; }
            }

            public List<DocumentStruct> DocumentStructList
            {
                get
                {
                    if (_documentStructList == null)
                    {
                        _documentStructList = new List<DocumentStruct>();
                    }

                    return _documentStructList;
                }
                set { _documentStructList = value; }
            }

            public Organization_PK LicenceHolder
            {
                get { return _licenceHolder; }
                set { _licenceHolder = value; }
            }

            public Organization_PK MasterFileLocation
            {
                get { return _masterFileLocation; }
                set { _masterFileLocation = value; }
            }

            public Organization_in_role_ LicenceHolderOrgInRole
            {
                get { return _licenceHolderOrgInRole; }
                set { _licenceHolderOrgInRole = value; }
            }

            public Organization_in_role_ MasterFileLocationOrgInRole
            {
                get { return _masterFileLocationOrgInRole; }
                set { _masterFileLocationOrgInRole = value; }
            }

            public string LocalNumber
            {
                get { return _localNumber; }
                set { _localNumber = value; }
            }

            public XevprmOperationType OperationType
            {
                get { return _operationType; }
                set { _operationType = value; }
            }
        }

        private class ProductStruct
        {
            private Product_PK _product;
            private List<PharmaceuticalProductStruct> _pharmaceuticalProductStructList;
            private List<Product_atc_mn_PK> _productAtcMNList;

            public Product_PK Product
            {
                get { return _product; }
                set { _product = value; }
            }

            public List<PharmaceuticalProductStruct> PharmaceuticalProductStructList
            {
                get
                {
                    if (_pharmaceuticalProductStructList == null)
                    {
                        _pharmaceuticalProductStructList = new List<PharmaceuticalProductStruct>();
                    }

                    return _pharmaceuticalProductStructList;
                }
                set { _pharmaceuticalProductStructList = value; }
            }

            public List<Product_atc_mn_PK> ProductAtcMNList
            {
                get
                {
                    if (_productAtcMNList == null)
                    {
                        _productAtcMNList = new List<Product_atc_mn_PK>();
                    }

                    return _productAtcMNList;
                }
                set { _productAtcMNList = value; }
            }
        }

        private class PharmaceuticalProductStruct
        {
            private Pharmaceutical_product_PK _pharmaceuticalProduct;
            private List<Activeingredient_PK> _activeIngredientList;
            private List<Excipient_PK> _excipientList;
            private List<Adjuvant_PK> _adjuvantList;
            private List<Pp_ar_mn_PK> _adminRoutePPMNList;
            private List<Pp_md_mn_PK> _medicalDevicePPMNList;

            public Pharmaceutical_product_PK PharmaceuticalProduct
            {
                get { return _pharmaceuticalProduct; }
                set { _pharmaceuticalProduct = value; }
            }

            public List<Activeingredient_PK> ActiveIngredientList
            {
                get
                {
                    if (_activeIngredientList == null)
                    {
                        _activeIngredientList = new List<Activeingredient_PK>();
                    }

                    return _activeIngredientList;
                }
                set { _activeIngredientList = value; }
            }

            public List<Excipient_PK> ExcipientList
            {
                get
                {
                    if (_excipientList == null)
                    {
                        _excipientList = new List<Excipient_PK>();
                    }

                    return _excipientList;
                }
                set { _excipientList = value; }
            }

            public List<Adjuvant_PK> AdjuvantList
            {
                get
                {
                    if (_adjuvantList == null)
                    {
                        _adjuvantList = new List<Adjuvant_PK>();
                    }

                    return _adjuvantList;
                }
                set { _adjuvantList = value; }
            }

            public List<Pp_ar_mn_PK> AdminRoutePPMNList
            {
                get
                {
                    if (_adminRoutePPMNList == null)
                    {
                        _adminRoutePPMNList = new List<Pp_ar_mn_PK>();
                    }

                    return _adminRoutePPMNList;
                }
                set { _adminRoutePPMNList = value; }
            }

            public List<Pp_md_mn_PK> MedicalDevicePPMNList
            {
                get
                {
                    if (_medicalDevicePPMNList == null)
                    {
                        _medicalDevicePPMNList = new List<Pp_md_mn_PK>();
                    }

                    return _medicalDevicePPMNList;
                }
                set { _medicalDevicePPMNList = value; }
            }
        }

        private class DocumentStruct
        {
            private Document_PK _document;
            private Attachment_PK _attachment;
            private string _attachmentLocalNumber;
            private XevprmOperationType _attachmentOperationType;
            private List<Document_language_mn_PK> _documentLanguageCodeMNList;

            public Document_PK Document
            {
                get { return _document; }
                set { _document = value; }
            }

            public Attachment_PK Attachment
            {
                get { return _attachment; }
                set { _attachment = value; }
            }

            public List<Document_language_mn_PK> DocumentLanguageCodeMNList
            {
                get
                {
                    if (_documentLanguageCodeMNList == null)
                    {
                        _documentLanguageCodeMNList = new List<Document_language_mn_PK>();
                    }

                    return _documentLanguageCodeMNList;
                }
                set { _documentLanguageCodeMNList = value; }
            }

            public string AttachmentLocalNumber
            {
                get { return _attachmentLocalNumber; }
                set { _attachmentLocalNumber = value; }
            }

            public XevprmOperationType AttachmentOperationType
            {
                get { return _attachmentOperationType; }
                set { _attachmentOperationType = value; }
            }
        }

        private class QPPVPersonStruct
        {
            private Person_PK _qppvPerson;
            private Person_in_role_PK _qppvPersonInRole;
            private Qppv_code_PK _qppvCode;

            public Person_PK QppvPerson
            {
                get { return _qppvPerson; }
                set { _qppvPerson = value; }
            }

            public Qppv_code_PK QppvCode
            {
                get { return _qppvCode; }
                set { _qppvCode = value; }
            }

            public Person_in_role_PK QppvPersonInRole
            {
                get { return _qppvPersonInRole; }
                set { _qppvPersonInRole = value; }
            }
        }

        private void SaveMAReadyStructToDB()
        {
            bool saveAllOrNothing = false;
            string exceptionMessage = "Error occured at saving marketing authorisation to database. See inner exception for more details.";

            if (_maSaveOptions.RollbackAllAtSaveException)
            {
                saveAllOrNothing = true;
            }

            TransactionScope ts = null;
            try
            {
                if (saveAllOrNothing)
                {
                    ts = new TransactionScope();
                }

                foreach (AuthorisedProductStruct authorisedProductStruct in _maReadyStruct.AuthorisedProductStructList)
                {
                    //Save product
                    try
                    {
                        if (authorisedProductStruct.ProductStruct.Product != null)
                        {
                            Product_PK dbProduct = _product_PKOperations.Save(authorisedProductStruct.ProductStruct.Product);
                            authorisedProductStruct.ProductStruct.Product.product_PK = dbProduct.product_PK;

                            if (_maSaveOptions.PopulateMAEntityMNTable)
                            {
                                _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null,_maPK,dbProduct.product_PK,(int)MAEntityType.Product));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _saveExceptions.Add(ex);

                        if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                    }

                    //Save pharmaceutical products
                    if (authorisedProductStruct.ProductStruct.PharmaceuticalProductStructList.Count > 0)
                    {
                        foreach (PharmaceuticalProductStruct pharmaceuticalProductStruct in authorisedProductStruct.ProductStruct.PharmaceuticalProductStructList)
                        {
                            int? dbPharmaceuticalProductPK = null;

                            //Save pharmaceutical product
                            try
                            {
                                if (pharmaceuticalProductStruct.PharmaceuticalProduct != null)
                                {
                                    Pharmaceutical_product_PK dbPharmaceuticalProduct = _pharmaceutical_product_PKOperations.Save(pharmaceuticalProductStruct.PharmaceuticalProduct);
                                    pharmaceuticalProductStruct.PharmaceuticalProduct.pharmaceutical_product_PK = dbPharmaceuticalProduct.pharmaceutical_product_PK;
                                    dbPharmaceuticalProductPK = dbPharmaceuticalProduct.pharmaceutical_product_PK;

                                    if (_maSaveOptions.PopulateMAEntityMNTable)
                                    {
                                        _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null, _maPK, dbPharmaceuticalProduct.pharmaceutical_product_PK, (int)MAEntityType.PharmaceuticalProduct));
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }

                            //Assign pharmaceutical product to product
                            try
                            {
                                if (authorisedProductStruct.ProductStruct.Product != null && authorisedProductStruct.ProductStruct.Product.product_PK != null && dbPharmaceuticalProductPK != null)
                                {
                                    Product_mn_PK dbProductPPMN = new Product_mn_PK();
                                    dbProductPPMN.pp_FK = dbPharmaceuticalProductPK;
                                    dbProductPPMN.product_FK = authorisedProductStruct.ProductStruct.Product.product_PK;
                                    dbProductPPMN = _product_mn_PKOperations.Save(dbProductPPMN);
                                }
                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }

                            //Save active ingredients
                            try
                            {
                                for (int activeIngIndex = 0; activeIngIndex < pharmaceuticalProductStruct.ActiveIngredientList.Count; activeIngIndex++)
                                {
                                    pharmaceuticalProductStruct.ActiveIngredientList[activeIngIndex].pp_FK = dbPharmaceuticalProductPK;

                                    Activeingredient_PK dbActiveIngredient = _activeingredient_PKOperations.Save(pharmaceuticalProductStruct.ActiveIngredientList[activeIngIndex]);
                                    pharmaceuticalProductStruct.ActiveIngredientList[activeIngIndex].activeingredient_PK = dbActiveIngredient.activeingredient_PK;

                                    if (_maSaveOptions.PopulateMAEntityMNTable)
                                    {
                                        _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null, _maPK, dbActiveIngredient.activeingredient_PK, (int)MAEntityType.ActiveIngredient));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }

                            //Save excipients
                            try
                            {
                                for (int excipientIndex = 0; excipientIndex < pharmaceuticalProductStruct.ExcipientList.Count; excipientIndex++)
                                {
                                    pharmaceuticalProductStruct.ExcipientList[excipientIndex].pp_FK = dbPharmaceuticalProductPK;
                                    
                                    Excipient_PK dbExcipient = _excipient_PKOperations.Save(pharmaceuticalProductStruct.ExcipientList[excipientIndex]);
                                    pharmaceuticalProductStruct.ExcipientList[excipientIndex].excipient_PK = dbExcipient.excipient_PK;

                                    if (_maSaveOptions.PopulateMAEntityMNTable)
                                    {
                                        _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null, _maPK, dbExcipient.excipient_PK, (int)MAEntityType.Excipient));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }

                            //Save adjuvants
                            try
                            {
                                for (int adjuvantIndex = 0; adjuvantIndex < pharmaceuticalProductStruct.AdjuvantList.Count; adjuvantIndex++)
                                {
                                    pharmaceuticalProductStruct.AdjuvantList[adjuvantIndex].pp_FK = dbPharmaceuticalProductPK;

                                    Adjuvant_PK dbAdjuvant = _adjuvant_PKOperations.Save(pharmaceuticalProductStruct.AdjuvantList[adjuvantIndex]);
                                    pharmaceuticalProductStruct.AdjuvantList[adjuvantIndex].adjuvant_PK = dbAdjuvant.adjuvant_PK;

                                    if (_maSaveOptions.PopulateMAEntityMNTable)
                                    {
                                        _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null, _maPK, dbAdjuvant.adjuvant_PK, (int)MAEntityType.Adjuvant));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }

                            //Save administration routes
                            try
                            {
                                for (int adminRoutePPMNIndex = 0; adminRoutePPMNIndex < pharmaceuticalProductStruct.AdminRoutePPMNList.Count; adminRoutePPMNIndex++)
                                {
                                    if (dbPharmaceuticalProductPK != null)
                                    {
                                        pharmaceuticalProductStruct.AdminRoutePPMNList[adminRoutePPMNIndex].pharmaceutical_product_FK = dbPharmaceuticalProductPK;
                                        pharmaceuticalProductStruct.AdminRoutePPMNList[adminRoutePPMNIndex] = _pp_ar_mn_PKOperations.Save(pharmaceuticalProductStruct.AdminRoutePPMNList[adminRoutePPMNIndex]);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }

                            //Save medical devices
                            try
                            {
                                for (int medicalDevicePPMNIndex = 0; medicalDevicePPMNIndex < pharmaceuticalProductStruct.MedicalDevicePPMNList.Count; medicalDevicePPMNIndex++)
                                {
                                    if (dbPharmaceuticalProductPK != null)
                                    {
                                        pharmaceuticalProductStruct.MedicalDevicePPMNList[medicalDevicePPMNIndex].pharmaceutical_product_FK = dbPharmaceuticalProductPK;
                                        pharmaceuticalProductStruct.MedicalDevicePPMNList[medicalDevicePPMNIndex] = _pp_md_mn_PKOperations.Save(pharmaceuticalProductStruct.MedicalDevicePPMNList[medicalDevicePPMNIndex]);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }
                        }
                    }

                    //Save product atcs
                    if (authorisedProductStruct.ProductStruct.ProductAtcMNList.Count > 0)
                    {
                        for (int productAtcMNIndex = 0; productAtcMNIndex < authorisedProductStruct.ProductStruct.ProductAtcMNList.Count; productAtcMNIndex++)
                        {
                            try
                            {
                                if (authorisedProductStruct.ProductStruct.Product != null && authorisedProductStruct.ProductStruct.Product.product_PK != null)
                                {
                                    authorisedProductStruct.ProductStruct.ProductAtcMNList[productAtcMNIndex].product_FK = authorisedProductStruct.ProductStruct.Product.product_PK;
                                    authorisedProductStruct.ProductStruct.ProductAtcMNList[productAtcMNIndex] = _product_atc_mn_PKOperations.Save(authorisedProductStruct.ProductStruct.ProductAtcMNList[productAtcMNIndex]);
                                }
                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }
                        }
                    }


                    //Save person
                    try
                    {
                        if (authorisedProductStruct.QppvPersonStruct.QppvPerson != null)
                        {
                            Person_PK dbPerson = _person_PKOperations.Save(authorisedProductStruct.QppvPersonStruct.QppvPerson);
                            authorisedProductStruct.QppvPersonStruct.QppvPerson.person_PK = dbPerson.person_PK;

                            if (_maSaveOptions.PopulateMAEntityMNTable)
                            {
                                _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null, _maPK, dbPerson.person_PK, (int)MAEntityType.Person));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _saveExceptions.Add(ex);

                        if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                    }

                    //Add QPPV role to person
                    try
                    {
                        if (authorisedProductStruct.QppvPersonStruct.QppvPersonInRole != null)
                        {
                            if (authorisedProductStruct.QppvPersonStruct.QppvPerson != null && authorisedProductStruct.QppvPersonStruct.QppvPerson.person_PK != null)
                            {
                                authorisedProductStruct.QppvPersonStruct.QppvPersonInRole.person_FK = authorisedProductStruct.QppvPersonStruct.QppvPerson.person_PK;

                                Person_in_role_PK dbPersonInRole = _person_in_role_PKOperations.Save(authorisedProductStruct.QppvPersonStruct.QppvPersonInRole);
                                authorisedProductStruct.QppvPersonStruct.QppvPersonInRole.person_in_role_PK = dbPersonInRole.person_in_role_PK;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _saveExceptions.Add(ex);

                        if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                    }

                    //Save QPPV code
                    try
                    {
                        if (authorisedProductStruct.QppvPersonStruct.QppvCode != null)
                        {
                            if (authorisedProductStruct.QppvPersonStruct.QppvPerson != null && authorisedProductStruct.QppvPersonStruct.QppvPerson.person_PK != null)
                            {
                                authorisedProductStruct.QppvPersonStruct.QppvCode.person_FK = authorisedProductStruct.QppvPersonStruct.QppvPerson.person_PK;
                            }

                            Qppv_code_PK dbQppvCode = _qppv_code_PKOperations.Save(authorisedProductStruct.QppvPersonStruct.QppvCode);
                            authorisedProductStruct.QppvPersonStruct.QppvCode.qppv_code_PK = dbQppvCode.qppv_code_PK;

                            authorisedProductStruct.AuthorisedProduct.qppv_code_FK = dbQppvCode.qppv_code_PK;

                            if (_maSaveOptions.PopulateMAEntityMNTable)
                            {
                                _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null, _maPK, dbQppvCode.qppv_code_PK, (int)MAEntityType.QppvCode));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _saveExceptions.Add(ex);

                        if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                    }

                    //Save licence holder
                    try
                    {
                        if (authorisedProductStruct.LicenceHolder != null)
                        {
                            Organization_PK dbLicenceHolder = _organization_PKOperations.Save(authorisedProductStruct.LicenceHolder);
                            authorisedProductStruct.LicenceHolder.organization_PK = dbLicenceHolder.organization_PK;

                            if (_maSaveOptions.PopulateMAEntityMNTable)
                            {
                                _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null, _maPK, dbLicenceHolder.organization_PK, (int)MAEntityType.Organization));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _saveExceptions.Add(ex);

                        if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                    }

                    //Add licence holder role to organization
                    try
                    {
                        if (authorisedProductStruct.LicenceHolderOrgInRole != null)
                        {
                            if (authorisedProductStruct.LicenceHolder != null && authorisedProductStruct.LicenceHolder.organization_PK != null)
                            {
                                authorisedProductStruct.LicenceHolderOrgInRole.organization_FK = authorisedProductStruct.LicenceHolder.organization_PK;

                                Organization_in_role_ dbOrgInRole = _organization_in_role_Operations.Save(authorisedProductStruct.LicenceHolderOrgInRole);
                                authorisedProductStruct.LicenceHolderOrgInRole.organization_in_role_ID = dbOrgInRole.organization_in_role_ID;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _saveExceptions.Add(ex);

                        if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                    }

                    //Save master file location
                    try
                    {
                        if (authorisedProductStruct.MasterFileLocation != null)
                        {
                            Organization_PK dbMasterFileLocation = _organization_PKOperations.Save(authorisedProductStruct.MasterFileLocation);
                            authorisedProductStruct.MasterFileLocation.organization_PK = dbMasterFileLocation.organization_PK;

                            if (_maSaveOptions.PopulateMAEntityMNTable)
                            {
                                _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null, _maPK, dbMasterFileLocation.organization_PK, (int)MAEntityType.Organization));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _saveExceptions.Add(ex);

                        if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                    }

                    //Add master file location role to organization
                    try
                    {
                        if (authorisedProductStruct.MasterFileLocationOrgInRole != null)
                        {
                            if (authorisedProductStruct.MasterFileLocation != null && authorisedProductStruct.MasterFileLocation.organization_PK != null)
                            {
                                authorisedProductStruct.MasterFileLocationOrgInRole.organization_FK = authorisedProductStruct.MasterFileLocation.organization_PK;

                                Organization_in_role_ dbOrgInRole = _organization_in_role_Operations.Save(authorisedProductStruct.MasterFileLocationOrgInRole);
                                authorisedProductStruct.MasterFileLocationOrgInRole.organization_in_role_ID = dbOrgInRole.organization_in_role_ID;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _saveExceptions.Add(ex);

                        if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                    }

                    //Save authorised product
                    try
                    {
                        if (authorisedProductStruct.AuthorisedProduct != null)
                        {
                            if (authorisedProductStruct.LicenceHolder != null && authorisedProductStruct.LicenceHolder.organization_PK != null)
                            {
                                authorisedProductStruct.AuthorisedProduct.organizationmahcode_FK = authorisedProductStruct.LicenceHolder.organization_PK;
                            }

                            if (authorisedProductStruct.MasterFileLocation != null && authorisedProductStruct.MasterFileLocation.organization_PK != null)
                            {
                                authorisedProductStruct.AuthorisedProduct.mflcode_FK = authorisedProductStruct.MasterFileLocation.organization_PK;
                            }

                            if (authorisedProductStruct.ProductStruct.Product != null && authorisedProductStruct.ProductStruct.Product.product_PK != null)
                            {
                                authorisedProductStruct.AuthorisedProduct.product_FK = authorisedProductStruct.ProductStruct.Product.product_PK;
                            }

                            AuthorisedProduct dbAuthorisedProduct = _authorisedProductOperations.Save(authorisedProductStruct.AuthorisedProduct);
                            authorisedProductStruct.AuthorisedProduct.ap_PK = dbAuthorisedProduct.ap_PK;

                            if (_maSaveOptions.PopulateMAEntityMNTable)
                            {
                                _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null, _maPK, dbAuthorisedProduct.ap_PK, (int)MAEntityType.AuthorisedProduct));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _saveExceptions.Add(ex);

                        if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                    }

                    //Save meddra

                    if (authorisedProductStruct.MeddraList.Count > 0)
                    {
                        for (int meddraIndex = 0; meddraIndex < authorisedProductStruct.MeddraList.Count; meddraIndex++)
                        {
                            try
                            {
                                Meddra_pk dbMeddra = _meddra_pkOperations.Save(authorisedProductStruct.MeddraList[meddraIndex]);
                                authorisedProductStruct.MeddraList[meddraIndex].meddra_pk = dbMeddra.meddra_pk;

                                if (authorisedProductStruct.AuthorisedProduct != null && authorisedProductStruct.AuthorisedProduct.ap_PK != null)
                                {
                                    Meddra_ap_mn_PK dbMeddraApMN = new Meddra_ap_mn_PK();
                                    dbMeddraApMN.ap_FK = authorisedProductStruct.AuthorisedProduct.ap_PK;
                                    dbMeddraApMN.meddra_FK = authorisedProductStruct.MeddraList[meddraIndex].meddra_pk;

                                    dbMeddraApMN = _meddra_ap_mn_PKOperations.Save(dbMeddraApMN);
                                }

                                if (_maSaveOptions.PopulateMAEntityMNTable)
                                {
                                    _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null, _maPK, dbMeddra.meddra_pk, (int)MAEntityType.Meddra));
                                }
                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }
                        }
                    }


                    if (authorisedProductStruct.DocumentStructList.Count > 0)
                    {
                        for (int documentIndex = 0; documentIndex < authorisedProductStruct.DocumentStructList.Count; documentIndex++)
                        {
                            int? dbDocumentPK = null;
                            try
                            {
                                if (authorisedProductStruct.DocumentStructList[documentIndex].Document != null)
                                {
                                    Document_PK dbDocument = _document_PKOperations.Save(authorisedProductStruct.DocumentStructList[documentIndex].Document);
                                    authorisedProductStruct.DocumentStructList[documentIndex].Document.document_PK = dbDocument.document_PK;
                                    dbDocumentPK = dbDocument.document_PK;

                                    if (_maSaveOptions.PopulateMAEntityMNTable)
                                    {
                                        _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null, _maPK, dbDocument.document_PK, (int)MAEntityType.Document));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }

                            //Assign document to authorised product
                            try
                            {
                                if (authorisedProductStruct.AuthorisedProduct != null && authorisedProductStruct.AuthorisedProduct.ap_PK != null && dbDocumentPK != null)
                                {
                                    Ap_document_mn_PK dbApDocumentMN = new Ap_document_mn_PK();
                                    dbApDocumentMN.document_FK = dbDocumentPK;
                                    dbApDocumentMN.ap_FK = authorisedProductStruct.AuthorisedProduct.ap_PK;

                                    dbApDocumentMN = _ap_document_mn_PKOperations.Save(dbApDocumentMN);
                                }
                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }

                            //Save document language codes
                            try
                            {
                                if (dbDocumentPK != null)
                                {
                                    for (int docLangCodeMNIndex = 0; docLangCodeMNIndex < authorisedProductStruct.DocumentStructList[documentIndex].DocumentLanguageCodeMNList.Count; docLangCodeMNIndex++)
                                    {
                                        authorisedProductStruct.DocumentStructList[documentIndex].DocumentLanguageCodeMNList[docLangCodeMNIndex].document_FK = dbDocumentPK;
                                        Document_language_mn_PK dbDocLangCodeMN = _document_language_mn_PKOperations.Save(authorisedProductStruct.DocumentStructList[documentIndex].DocumentLanguageCodeMNList[docLangCodeMNIndex]);

                                        authorisedProductStruct.DocumentStructList[documentIndex].DocumentLanguageCodeMNList[docLangCodeMNIndex].document_language_mn_PK = dbDocLangCodeMN.document_language_mn_PK;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }

                            //Save document attachment
                            try
                            {
                                if (authorisedProductStruct.DocumentStructList[documentIndex].Attachment != null)
                                {
                                    if (dbDocumentPK != null)
                                    {
                                        authorisedProductStruct.DocumentStructList[documentIndex].Attachment.document_FK = dbDocumentPK;
                                    }

                                    Attachment_PK dbAttachment = _attachment_PKOperations.Save(authorisedProductStruct.DocumentStructList[documentIndex].Attachment);
                                    authorisedProductStruct.DocumentStructList[documentIndex].Attachment.attachment_PK = dbAttachment.attachment_PK;

                                    if (_maSaveOptions.PopulateMAEntityMNTable)
                                    {
                                        _ma_ma_entity_mn_PKOperations.Save(new Ma_ma_entity_mn_PK(null, _maPK, dbAttachment.attachment_PK, (int)MAEntityType.Attachment));
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                _saveExceptions.Add(ex);

                                if (saveAllOrNothing) throw new Exception(exceptionMessage, ex);
                            }
                        }
                    }
                }

                if (ts != null)
                {
                    ts.Complete();
                    ts.Dispose();
                }
            }
            catch (Exception ex)
            {
                if (ts != null)
                {
                    ts.Dispose();
                }
                _exceptions.Add(ex);
            }
        }

        private void ResetMADataExporter()
        {
            if (_validationExceptions == null) _validationExceptions = new List<ValidationException>();
            if (_exceptions == null) _exceptions = new List<Exception>();
            if (_saveExceptions == null) _saveExceptions = new List<Exception>();

            _validationExceptions.Clear();
            _exceptions.Clear();
            _saveExceptions.Clear();

            _maReadyStruct = new MAReadyStruct();
        }
    }
}
