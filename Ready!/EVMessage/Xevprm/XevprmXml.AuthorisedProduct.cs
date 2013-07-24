using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;
using Ready.Model;

namespace EVMessage.Xevprm
{
    public partial class XevprmXml
    {
        public static bool IsAuthorisedProductValid(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            return ValidateAuthorisedProduct(dbAuthorisedProduct, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidateAuthorisedProduct(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            authorisedproductType evprmAuthorisedProduct;
            var evprmAttachments = new evprm.attachmentsLocalType();
            return ConstructAuthorisedProduct(dbAuthorisedProduct, operationType, out evprmAuthorisedProduct, ref evprmAttachments);
        }

        private static ValidationResult ConstructAuthorisedProduct(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType, out authorisedproductType evprmAuthorisedProduct, ref evprm.attachmentsLocalType evprmAttachments, string evprmAuthProdLocation = "")
        {
            var authProdValidationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var prodValidationExceptionTree = new Tree<XevprmValidationTreeNode>();

            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmAuthorisedProduct = null;

            if (dbAuthorisedProduct == null)
            {
                const string message = "ConstructAuthorisedProduct: Authorised product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            authProdValidationExceptionTree.Value.ReadyEntity = dbAuthorisedProduct;

            evprmAuthorisedProduct = new authorisedproductType();

            #region Authorised product details

            IQppv_code_PKOperations qppvCodeOperations = new Qppv_code_PKDAL();
            IOrganization_PKOperations organizationOperations = new Organization_PKDAL();
            IProduct_PKOperations productOperations = new Product_PKDAL();

            string readyAuthProdNavigateurl = NavigateUrl.AuthorisedProduct(dbAuthorisedProduct.ap_PK, operationType);

            var dbProduct = productOperations.GetEntity(dbAuthorisedProduct.product_FK);
            if (dbProduct == null)
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.BRCustom1(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.product_FK);
                validationExceptions.Add(exception);
                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);

                return GetValidationResult(ref validationExceptions, ref exceptions, ref authProdValidationExceptionTree);
            }

            prodValidationExceptionTree.Value.ReadyEntity = dbProduct;

            evprmAuthorisedProduct.operationtype = (int)operationType;

            //Local number
            if (operationType.In(XevprmOperationType.Insert))
            {
                if (dbAuthorisedProduct.ap_PK.HasValue)
                {
                    if (dbAuthorisedProduct.ap_PK.Value.ToString().Length <= 60)
                    {
                        evprmAuthorisedProduct.localnumber = dbAuthorisedProduct.ap_PK.Value.ToString();
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.localnumber.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                        exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.ap_PK);
                        validationExceptions.Add(exception);
                        authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.localnumber.BR1(), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.ap_PK);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }

            //EV Code
            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.ev_code))
            {
                if (operationType.NotIn(XevprmOperationType.Insert))
                {
                    if (dbAuthorisedProduct.ev_code.Trim().Length <= 60)
                    {
                        evprmAuthorisedProduct.ev_code = dbAuthorisedProduct.ev_code.Trim();
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.ev_code.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                        exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.ev_code);
                        validationExceptions.Add(exception);
                        authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.ev_code.BR2(), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.ev_code);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }
            else if (operationType.NotIn(XevprmOperationType.Insert))
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.ev_code.BR1(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.ev_code);
                validationExceptions.Add(exception);
                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
            }

            //Evprm comments
            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.evprm_comments))
            {
                if (dbAuthorisedProduct.evprm_comments.Trim().Length <= 500)
                {
                    evprmAuthorisedProduct.comments = dbAuthorisedProduct.evprm_comments.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.comments.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.evprm_comments);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }
            else if (operationType.NotIn(XevprmOperationType.Insert, XevprmOperationType.Update, XevprmOperationType.Variation))
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.comments.BR1(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.evprm_comments);
                exception.AddEvprmDescription(evprmAuthProdLocation, "comments", dbAuthorisedProduct.evprm_comments);
                validationExceptions.Add(exception);
                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
            }

            //Enquiry email
            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.phv_email))
            {
                if (IsValidEmail(dbAuthorisedProduct.phv_email.Trim()))
                {
                    if (dbAuthorisedProduct.phv_email.Trim().Length <= 100)
                    {
                        evprmAuthorisedProduct.enquiryemail = dbAuthorisedProduct.phv_email.Trim();
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.enquiryemail.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                        exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.phv_email);
                        validationExceptions.Add(exception);
                        authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.enquiryemail.BR2(), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.phv_email);
                    exception.AddEvprmDescription(evprmAuthProdLocation, "enquiryemail", dbAuthorisedProduct.phv_email.Trim());
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }
            else if (operationType.In(XevprmOperationType.Insert, XevprmOperationType.Update, XevprmOperationType.Variation))
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.enquiryemail.BR1(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.phv_email);
                exception.AddEvprmDescription(evprmAuthProdLocation, "enquiryemail", dbAuthorisedProduct.phv_email);
                validationExceptions.Add(exception);
                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
            }

            //Enquiry phone
            if (!string.IsNullOrWhiteSpace(dbAuthorisedProduct.phv_phone))
            {
                if (dbAuthorisedProduct.phv_phone.Trim().Length <= 100)
                {
                    evprmAuthorisedProduct.enquiryphone = dbAuthorisedProduct.phv_phone.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.enquiryphone.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.phv_phone);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }
            else if (operationType.In(XevprmOperationType.Insert, XevprmOperationType.Update, XevprmOperationType.Variation))
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.enquiryphone.BR1(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.phv_phone);
                exception.AddEvprmDescription(evprmAuthProdLocation, "enquiryphone", dbAuthorisedProduct.phv_phone);
                validationExceptions.Add(exception);
                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
            }

            //Info date
            if (dbAuthorisedProduct.infodate.HasValue)
            {
                if (dbAuthorisedProduct.infodate.Value < DateTime.UtcNow.AddHours(12))
                {
                    evprmAuthorisedProduct.infodate = dbAuthorisedProduct.infodate.Value.ToString("yyyyMMdd");
                    evprmAuthorisedProduct.infodateformat = "102";
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.infodate.BR3(), operationType);
                    exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.infodate);
                    exception.AddEvprmDescription(evprmAuthProdLocation, "infodate", dbAuthorisedProduct.infodate.Value.ToString("yyyyMMdd"));
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }

            //QPPV
            var dbQppvCode = dbAuthorisedProduct.qppv_code_FK.HasValue ? qppvCodeOperations.GetEntity(dbAuthorisedProduct.qppv_code_FK) : null;
            if (dbQppvCode != null)
            {
                string readyPersonNavigateUrl = NavigateUrl.Person(dbQppvCode.person_FK, dbAuthorisedProduct.ap_PK, operationType);

                if (!string.IsNullOrWhiteSpace(dbQppvCode.qppv_code))
                {
                    if (dbQppvCode.qppv_code.Trim().Length <= 10)
                    {
                        int qppvCode = 0;
                        if (int.TryParse(dbQppvCode.qppv_code.Trim(), out qppvCode) && qppvCode > 0)
                        {
                            evprmAuthorisedProduct.qppvcode = new authorisedproductType.qppvcodeLocalType();
                            evprmAuthorisedProduct.qppvcode.TypedValue = qppvCode.ToString();
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.AP.qppvcode.DataType(XevprmValidationRules.DataTypeEror.InvalidValueFormat), operationType);
                            exception.AddReadyDescription(readyPersonNavigateUrl, () => dbQppvCode.qppv_code_PK, () => dbQppvCode.qppv_code);
                            exception.AddEvprmDescription(evprmAuthProdLocation, "qppvcode", dbQppvCode.qppv_code);
                            validationExceptions.Add(exception);
                            authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.qppvcode.DataType(XevprmValidationRules.DataTypeEror.InvalidValueFormat), operationType);
                        exception.AddReadyDescription(readyPersonNavigateUrl, () => dbQppvCode.qppv_code_PK, () => dbQppvCode.qppv_code);
                        exception.AddEvprmDescription(evprmAuthProdLocation, "qppvcode", dbQppvCode.qppv_code);
                        validationExceptions.Add(exception);
                        authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.qppvcode.CustomBR1(), operationType);
                    exception.AddReadyDescription(readyPersonNavigateUrl, () => dbQppvCode.qppv_code_PK, () => dbQppvCode.qppv_code);
                    exception.AddEvprmDescription(evprmAuthProdLocation, "qppvcode", dbQppvCode.qppv_code);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }
            else if (operationType.In(XevprmOperationType.Insert, XevprmOperationType.Update, XevprmOperationType.Variation))
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.qppvcode.BR1(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.qppv_code_FK);
                exception.AddEvprmDescription(evprmAuthProdLocation, "qppvcode", null);
                validationExceptions.Add(exception);
                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
            }

            //Master file location
            //Note: Insert for master file location is not supported

            var dbMasterFileLocation = dbAuthorisedProduct.mflcode_FK.HasValue ? organizationOperations.GetEntity(dbAuthorisedProduct.mflcode_FK) : null;
            if (dbMasterFileLocation != null)
            {
                string readyOrgNavigateUrl = NavigateUrl.Organization(dbMasterFileLocation.organization_PK, dbAuthorisedProduct.ap_PK, operationType);

                if (!string.IsNullOrWhiteSpace(dbMasterFileLocation.mfl_evcode))
                {
                    if (dbMasterFileLocation.mfl_evcode.Trim().Length <= 60)
                    {
                        evprmAuthorisedProduct.mflcode = new authorisedproductType.mflcodeLocalType();
                        evprmAuthorisedProduct.mflcode.resolutionmode = 2;
                        evprmAuthorisedProduct.mflcode.TypedValue = dbMasterFileLocation.mfl_evcode.Trim();
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.mflcode.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                        exception.AddReadyDescription(readyOrgNavigateUrl, () => dbMasterFileLocation.organization_PK, () => dbMasterFileLocation.mfl_evcode);
                        validationExceptions.Add(exception);
                        authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.mflcode.BR3(), operationType);
                    exception.AddReadyDescription(readyOrgNavigateUrl, () => dbMasterFileLocation.organization_PK, () => dbMasterFileLocation.mfl_evcode);
                    exception.AddEvprmDescription(evprmAuthProdLocation, "mflcode", dbMasterFileLocation.mfl_evcode);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }

            //Marketing authorisation holder
            //Note: Insert for marketing authorisation holder is not supported
            Organization_PK dbLicenceHolder = dbAuthorisedProduct.organizationmahcode_FK.HasValue ? organizationOperations.GetEntity(dbAuthorisedProduct.organizationmahcode_FK) : null;
            if (dbLicenceHolder != null)
            {
                string readyOrgNavigateUrl = NavigateUrl.Organization(dbLicenceHolder.organization_PK, dbAuthorisedProduct.ap_PK, operationType);
                if (!string.IsNullOrWhiteSpace(dbLicenceHolder.ev_code))
                {
                    if (dbLicenceHolder.ev_code.Trim().Length <= 60)
                    {
                        evprmAuthorisedProduct.mahcode = new authorisedproductType.mahcodeLocalType();
                        evprmAuthorisedProduct.mahcode.resolutionmode = 2;
                        evprmAuthorisedProduct.mahcode.TypedValue = dbLicenceHolder.ev_code.Trim();
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.AP.mahcode.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                        exception.AddReadyDescription(readyOrgNavigateUrl, () => dbLicenceHolder.organization_PK, () => dbLicenceHolder.ev_code);
                        validationExceptions.Add(exception);
                        authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.mahcode.BR2(), operationType);
                    exception.AddReadyDescription(readyOrgNavigateUrl, () => dbLicenceHolder.organization_PK, () => dbLicenceHolder.ev_code);
                    exception.AddEvprmDescription(evprmAuthProdLocation, "mahcode", dbLicenceHolder.mfl_evcode);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }

                //SenderID
                if (string.IsNullOrWhiteSpace(dbLicenceHolder.organizationsenderid_EMEA))
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.H.messagesenderidentifier.BR1(), operationType);
                    exception.AddReadyDescription(readyOrgNavigateUrl, () => dbLicenceHolder.organization_PK, () => dbLicenceHolder.organizationsenderid_EMEA);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
                else if (dbLicenceHolder.organizationsenderid_EMEA.Trim().Length < 3)
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.H.messagesenderidentifier.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToSmall), operationType);
                    exception.AddReadyDescription(readyOrgNavigateUrl, () => dbLicenceHolder.organization_PK, () => dbLicenceHolder.organizationsenderid_EMEA);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
                else if (dbLicenceHolder.organizationsenderid_EMEA.Trim().Length > 60)
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.H.messagesenderidentifier.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyOrgNavigateUrl, () => dbLicenceHolder.organization_PK, () => dbLicenceHolder.organizationsenderid_EMEA);
                    validationExceptions.Add(exception);
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.mahcode.Cardinality(), operationType);
                exception.AddReadyDescription(readyAuthProdNavigateurl, () => dbAuthorisedProduct.ap_PK, () => dbAuthorisedProduct.organizationmahcode_FK);
                exception.AddEvprmDescription(evprmAuthProdLocation, "mahcode", null);
                validationExceptions.Add(exception);
                authProdValidationExceptionTree.Value.XevprmValidationExceptions.Add(exception);
            }

            #endregion

            #region Authorisation

            {
                authorisedproductType.authorisationLocalType evprmAuthorisation;
                var validationResult = ConstructAuthorisation(dbAuthorisedProduct, operationType, out evprmAuthorisation, evprmAuthProdLocation);
                UpdateExceptions(validationResult, ref validationExceptions, ref exceptions);

                authProdValidationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationResult.XevprmValidationTree.Value.XevprmValidationExceptions);

                if (validationResult.XevprmValidationTree.Children.Count > 0)
                {
                    var prodObject = validationResult.XevprmValidationTree.Children[0].Value.ReadyEntity;

                    if (prodObject is Product_PK && (prodObject as Product_PK).product_PK == dbProduct.product_PK)
                    {
                        prodValidationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationResult.XevprmValidationTree.Children[0].Value.XevprmValidationExceptions);
                    }
                }

                //Add authorisation to authorised product
                if (evprmAuthorisation != null)
                {
                    evprmAuthorisedProduct.authorisation = evprmAuthorisation;
                }
            }

            #endregion

            #region Presentation name

            {
                authorisedproductType.presentationnameLocalType evprmPresentationName;
                var validationResult = ConstructPresentationName(dbAuthorisedProduct, operationType, out evprmPresentationName, evprmAuthProdLocation);
                UpdateExceptions(validationResult, ref validationExceptions, ref exceptions);

                authProdValidationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationResult.XevprmValidationExceptions);

                if (evprmPresentationName != null)
                {
                    evprmAuthorisedProduct.presentationname = evprmPresentationName;
                }
            }

            #endregion

            #region Product ATCs

            {
                authorisedproductType.productatcsLocalType evprmProductAtcs;

                var validationResult = ConstructProductAtcs(dbAuthorisedProduct, operationType, out evprmProductAtcs, evprmAuthProdLocation);
                UpdateExceptions(validationResult, ref validationExceptions, ref exceptions);

                prodValidationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationResult.XevprmValidationExceptions);

                if (evprmProductAtcs != null)
                {
                    evprmAuthorisedProduct.productatcs = evprmProductAtcs;
                }
            }

            #endregion

            #region Indications

            {
                authorisedproductType.productindicationsLocalType evprmProductIndications;
                var validationResult = ConstructProductIndications(dbAuthorisedProduct, operationType, out evprmProductIndications, evprmAuthProdLocation);
                UpdateExceptions(validationResult, ref validationExceptions, ref exceptions);

                authProdValidationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationResult.XevprmValidationExceptions);

                if (evprmProductIndications != null)
                {
                    evprmAuthorisedProduct.productindications = evprmProductIndications;
                }
            }

            #endregion

            #region Pharmaceutical products

            {
                authorisedproductType.pharmaceuticalproductsLocalType evprmPharmaceuticalProducts;
                var validationResult = ConstructPharmaceuticalProducts(dbAuthorisedProduct, operationType, out evprmPharmaceuticalProducts, evprmAuthProdLocation);
                UpdateExceptions(validationResult, ref validationExceptions, ref exceptions);

                if (validationResult.XevprmValidationTree.Value.ReadyEntity is AuthorisedProduct)
                {
                    authProdValidationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationResult.XevprmValidationTree.Value.XevprmValidationExceptions);
                }
                else if (validationResult.XevprmValidationTree.Value.ReadyEntity is Product_PK)
                {
                    prodValidationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationResult.XevprmValidationTree.Value.XevprmValidationExceptions);

                    if (validationResult.XevprmValidationTree.Children.Count > 0)
                    {
                        for (int i = validationResult.XevprmValidationTree.Children.Count - 1; i >= 0; i--)
                        {
                            var treeNode = validationResult.XevprmValidationTree.Children[i];
                            if (treeNode.Value.ReadyEntity is Pharmaceutical_product_PK && treeNode.Value.XevprmValidationExceptions.Count > 0)
                            {
                                prodValidationExceptionTree.Children.Add(treeNode);
                            }
                        }
                    }
                }

                if (evprmPharmaceuticalProducts != null)
                {
                    evprmAuthorisedProduct.pharmaceuticalproducts = evprmPharmaceuticalProducts;
                }
            }

            #endregion

            #region Printed product information attachments

            {
                authorisedproductType.ppiattachmentsLocalType evprmPpiAttachments;
                var validationResult = ConstructPpiAttachments(dbAuthorisedProduct, operationType, out evprmPpiAttachments, ref evprmAttachments, evprmAuthProdLocation);
                UpdateExceptions(validationResult, ref validationExceptions, ref exceptions);

                authProdValidationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationResult.XevprmValidationExceptions);

                if (evprmPpiAttachments != null)
                {
                    evprmAuthorisedProduct.ppiattachments = evprmPpiAttachments;
                }
            }

            #endregion

            if (prodValidationExceptionTree.Children.Count > 0 || prodValidationExceptionTree.Value.XevprmValidationExceptions.Count > 0)
            {
                authProdValidationExceptionTree.Children.Add(prodValidationExceptionTree);
            }

            return GetValidationResult(ref validationExceptions, ref exceptions, ref authProdValidationExceptionTree);
        }


    }
}
