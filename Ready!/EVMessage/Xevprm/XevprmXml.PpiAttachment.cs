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
        private static bool ArePpiAttachmentsValid(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            return ValidatePpiAttachments(dbAuthorisedProduct, operationType).XevprmValidationExceptions.Count == 0;
        }

        private static ValidationResult ValidatePpiAttachments(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType)
        {
            authorisedproductType.ppiattachmentsLocalType evprmPpiAttachments;
            var evprmAttachments = new evprm.attachmentsLocalType();
            return ConstructPpiAttachments(dbAuthorisedProduct, operationType, out evprmPpiAttachments, ref evprmAttachments);
        }

        private static bool IsPpiAttachmentValid(Attachment_PK dbAttachment, XevprmOperationType operationType)
        {
            return ValidatePpiAttachment(dbAttachment, operationType).XevprmValidationExceptions.Count == 0;
        }

        private static ValidationResult ValidatePpiAttachment(Attachment_PK dbAttachment, XevprmOperationType operationType)
        {
            authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType evprmPpiAttachment;
            attachmentType evprmAttachment;
            return ConstructPpiAttachment(dbAttachment, operationType, out evprmPpiAttachment, out evprmAttachment);
        }

        private static ValidationResult ConstructPpiAttachments(AuthorisedProduct dbAuthorisedProduct, XevprmOperationType operationType,
            out authorisedproductType.ppiattachmentsLocalType evprmPpiAttachments, ref evprm.attachmentsLocalType evprmAttachments, string evprmApLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmPpiAttachments = null;

            if (dbAuthorisedProduct == null)
            {
                const string message = "ConstructPpiAttachments: Authorised product is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            validationExceptionTree.Value.ReadyEntity = dbAuthorisedProduct;

            IDocument_PKOperations documentOperations = new Document_PKDAL();
            IType_PKOperations typeOperations = new Type_PKDAL();
            IAttachment_PKOperations attachmentOperations = new Attachment_PKDAL();

            var authorisedProductHasPpiDocument = false;
            var documentHasPpiAttachment = false;

            var dbDocumentList = dbAuthorisedProduct.ap_PK.HasValue ? documentOperations.GetDocumentsByAP(dbAuthorisedProduct.ap_PK.Value) : new List<Document_PK>();

            if (dbDocumentList != null && dbDocumentList.Count > 0)
            {
                var evprmPpiAttachmentList = new List<authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType>();

                int attachmentIndex = evprmAttachments != null ? evprmAttachments.attachment.Count : 0;
                int ppiAttachmentIndex = 0;

                foreach (var dbDocument in dbDocumentList)
                {
                    var dbDocumentType = dbDocument.type_FK.HasValue ? typeOperations.GetEntity(dbDocument.type_FK) : null;

                    if (dbDocumentType == null) continue;

                    if (dbDocumentType.name == null || dbDocumentType.name.Trim().ToLower() != "ppi") continue;

                    //if (authorisedProductHasPpiDocument)
                    //{
                    //    var exception = new XevprmValidationException(new XevprmValidationRules.AP.PPI.CustomBR1(), operationType);
                    //    exception.AddReadyDescription(NavigateUrl.AuthorisedProductDocuments(dbAuthorisedProduct.ap_PK, operationType), typeof(Ap_document_mn_PK), null, "ap_document_mn_PK", null);
                    //    exception.AddEvprmDescription(evprmApLocation, "ppiattachments", null);
                    //    validationExceptions.Add(exception);
                    //}
                    //else
                    //{
                    //    authorisedProductHasPpiDocument = true;
                    //}
                    authorisedProductHasPpiDocument = true;

                    var dbAttachmentList = dbDocument.document_PK.HasValue ? attachmentOperations.GetAttachmentsForDocument(dbDocument.document_PK.Value) : new List<Attachment_PK>();
                    if (dbAttachmentList.Count == 1)
                    {
                        documentHasPpiAttachment = true;

                        var dbAttachment = dbAttachmentList[0];

                        string evprmAttachmentLocation = string.Format("attachments.attachment[{0}]", attachmentIndex);
                        string evprmPpiAttachmentLocation = string.Format("{0}.ppiattachments.ppiattachment[{1}]", evprmApLocation, ppiAttachmentIndex);

                        authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType evprmPpiAttachment;
                        attachmentType evprmAttachment;

                        var operationResult = ConstructPpiAttachment(dbAttachment, operationType, out evprmPpiAttachment, out evprmAttachment, evprmPpiAttachmentLocation, evprmAttachmentLocation);
                        UpdateExceptions(operationResult, ref validationExceptions, ref exceptions);

                        if (evprmAttachment != null)
                        {
                            if (evprmAttachments == null)
                            {
                                evprmAttachments = new evprm.attachmentsLocalType();
                            }

                            if (evprmAttachments.attachment == null)
                            {
                                evprmAttachments.attachment = new attachmentType[] { };
                            }

                            if (evprmAttachments.attachment.Count > 0 && evprmAttachments.attachment.ToList().Any(item => item.localnumber == evprmAttachment.localnumber))
                            {
                                exceptions.Add(new Exception(string.Format("Attachment with localnumber = '{0}' already exists in attachments section.", evprmAttachment.localnumber)));
                            }
                            else
                            {
                                evprmAttachments.attachment.Add(evprmAttachment);
                            }

                            attachmentIndex++;
                        }

                        if (evprmPpiAttachment != null)
                        {
                            evprmPpiAttachmentList.Add(evprmPpiAttachment);
                            ppiAttachmentIndex++;
                        }
                    }
                    else if (dbAttachmentList.Count == 0)
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.ATT.BRCustom1(), operationType);
                        exception.AddReadyDescription(NavigateUrl.AuthorisedProductDocument(dbDocument.document_PK, dbAuthorisedProduct.ap_PK, operationType), typeof(Attachment_PK), null, "document_FK", null);
                        validationExceptions.Add(exception);
                    }
                    else
                    {
                        exceptions.Add(new Exception(string.Format("Authorised product (PPI) document with id = '{0}' has more than one associated attachment.", dbDocument.document_PK)));
                    }
                }

                if (evprmPpiAttachmentList.Count > 0)
                {
                    evprmPpiAttachments = new authorisedproductType.ppiattachmentsLocalType();
                    evprmPpiAttachments.ppiattachment = evprmPpiAttachmentList.ToArray();
                }

                if (authorisedProductHasPpiDocument && documentHasPpiAttachment)
                {
                    if (evprmPpiAttachments == null ||
                        evprmPpiAttachments.ppiattachment == null ||
                        evprmPpiAttachments.ppiattachment.Count == 0)
                    {
                        exceptions.Add(new Exception(string.Format("{0}.ppiattachments section contains 0 ppiattachments although Authorised product has ppi attachment.", evprmApLocation)));
                    }
                    else if (evprmPpiAttachments.ppiattachment[0].attachmentcode != null &&
                        evprmPpiAttachments.ppiattachment[0].attachmentcode.resolutionmode == 1 &&
                        (evprmAttachments == null ||
                            evprmAttachments.attachment == null ||
                            evprmAttachments.attachment.Count == 0))
                    {
                        exceptions.Add(new Exception("attachments section contains 0 attachments although Authorised product has ppi attachment witch references attachment."));
                    }
                }
            }

            if (!authorisedProductHasPpiDocument && operationType.In(XevprmOperationType.Insert, XevprmOperationType.Update, XevprmOperationType.Variation))
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.AP.PPI.BR3(), operationType);
                exception.AddReadyDescription(NavigateUrl.AuthorisedProductDocuments(dbAuthorisedProduct.ap_PK, operationType), typeof(Ap_document_mn_PK), null, "ap_document_mn_PK", null);
                exception.AddEvprmDescription(evprmApLocation, "ppiattachments", null);
                validationExceptions.Add(exception);
            }

            validationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationExceptions);

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }

        private static ValidationResult ConstructPpiAttachment(Attachment_PK dbAttachment, XevprmOperationType operationType,
            out authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType evprmPpiAttachment, out attachmentType evprmAttachment, string evprmPpiAttachmentLocation = "", string evprmAttachmentLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmPpiAttachment = null;
            evprmAttachment = null;

            if (dbAttachment == null)
            {
                const string message = "ConstructPpiAttachment: Attachment is null!";
                return new ValidationResult(new ArgumentException(message), message);
            }

            IDocument_PKOperations documentOperations = new Document_PKDAL();
            var dbDocument = dbAttachment.document_FK.HasValue ? documentOperations.GetEntity(dbAttachment.document_FK) : null;

            if (dbDocument == null)
            {
                string message = string.Format("ConstructAttachment: Document with ID = '{0}' doesn't exist for attachment with ID = '{1}'!", dbAttachment.document_FK, dbAttachment.attachment_PK);
                return new ValidationResult(new ArgumentException(message), message);
            }

            IAp_document_mn_PKOperations authorisedProductDocumentMnOperations = new Ap_document_mn_PKDAL();
            IAuthorisedProductOperations authorisedProductOperations = new AuthorisedProductDAL();

            var authorisedProductDocumentMnList = authorisedProductDocumentMnOperations.GetAuthorizedProductsByDocumentFK(dbDocument.document_PK);

            AuthorisedProduct dbAuthorisedProduct = null;
            if (authorisedProductDocumentMnList.Count == 1)
            {
                dbAuthorisedProduct = authorisedProductOperations.GetEntity(authorisedProductDocumentMnList[0].ap_FK);
            }

            string readyDocNavigateurl = dbAuthorisedProduct != null ? NavigateUrl.AuthorisedProductDocument(dbDocument.document_PK, dbAuthorisedProduct.ap_PK, operationType) : NavigateUrl.Document(dbDocument.document_PK, operationType);

            validationExceptionTree.Value.ReadyEntity = dbDocument;

            if (!string.IsNullOrWhiteSpace(dbAttachment.ev_code))
            {
                if (dbAttachment.ev_code.Trim().Length <= 60)
                {
                    evprmPpiAttachment = new authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType();
                    evprmPpiAttachment.attachmentcode = new authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType.attachmentcodeLocalType();
                    evprmPpiAttachment.attachmentcode.resolutionmode = 2;
                    evprmPpiAttachment.attachmentcode.TypedValue = dbAttachment.ev_code.Trim();

                    evprmPpiAttachment.validitydeclaration = 1;
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.AP.PPI.attachmentcode.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyDocNavigateurl, () => dbAttachment.attachment_PK, () => dbAttachment.ev_code);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                #region Attachment

                {
                    var operationResult = ConstructAttachment(dbAttachment, XevprmOperationType.Insert, out evprmAttachment, evprmAttachmentLocation);
                    UpdateExceptions(operationResult, ref validationExceptions, ref exceptions);
                }

                #endregion

                if (evprmAttachment != null)
                {
                    evprmPpiAttachment = new authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType();
                    evprmPpiAttachment.attachmentcode = new authorisedproductType.ppiattachmentsLocalType.ppiattachmentLocalType.attachmentcodeLocalType();
                    evprmPpiAttachment.attachmentcode.resolutionmode = 1;
                    evprmPpiAttachment.attachmentcode.TypedValue = evprmAttachment.localnumber;
                    evprmPpiAttachment.validitydeclaration = 1;
                }
            }

            validationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationExceptions);

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }
    }
}
