using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;
using Ready.Model;

namespace EVMessage.Xevprm
{
    public partial class XevprmXml
    {
        public static bool IsAttachmentValid(Attachment_PK dbAttachment, XevprmOperationType operationType)
        {
            return ValidateAttachment(dbAttachment, operationType).XevprmValidationExceptions.Count == 0;
        }

        public static ValidationResult ValidateAttachment(Attachment_PK dbAttachment, XevprmOperationType operationType)
        {
            attachmentType evprmAttachment;
            return ConstructAttachment(dbAttachment, operationType, out evprmAttachment);
        }

        private static ValidationResult ConstructAttachment(Attachment_PK dbAttachment, XevprmOperationType operationType, out attachmentType evprmAttachment, string evprmAttachmentLocation = "")
        {
            var validationExceptionTree = new Tree<XevprmValidationTreeNode>();
            var validationExceptions = new List<XevprmValidationException>();
            var exceptions = new List<Exception>();
            evprmAttachment = null;

            if (dbAttachment == null)
            {
                const string message = "ConstructAttachment: Attachment is null!";
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

            validationExceptionTree.Value.ReadyEntity = dbDocument;

            evprmAttachment = new attachmentType();

            IType_PKOperations typeOperations = new Type_PKDAL();
            IDocument_language_mn_PKOperations documentLanguageMnOperations = new Document_language_mn_PKDAL();
            ILanguagecode_PKOperations languagecodeOperations = new Languagecode_PKDAL();

            string readyDocNavigateurl = dbAuthorisedProduct != null ? NavigateUrl.AuthorisedProductDocument(dbDocument.document_PK, dbAuthorisedProduct.ap_PK, operationType) : NavigateUrl.Document(dbDocument.document_PK, operationType);

            evprmAttachment.attachmenttype1 = 1; //PPI

            if (operationType.In(XevprmOperationType.Insert))
            {
                evprmAttachment.operationtype = (int)operationType;
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.ATT.operationtype.BR1(), operationType);
                exception.AddReadyDescription(readyDocNavigateurl, () => dbAttachment.attachment_PK, () => dbAttachment.attachment_PK);
                validationExceptions.Add(exception);
            }

            //Local number
            if (dbAttachment.attachment_PK.HasValue)
            {
                if (dbAttachment.attachment_PK.Value.ToString().Length <= 60)
                {
                    evprmAttachment.localnumber = dbAttachment.attachment_PK.Value.ToString();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.ATT.localnumber.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyDocNavigateurl, () => dbAttachment.attachment_PK, () => dbAttachment.attachment_PK);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.ATT.localnumber.Cardinality(), operationType);
                exception.AddReadyDescription(readyDocNavigateurl, () => dbAttachment.attachment_PK, () => dbAttachment.attachment_PK);
                validationExceptions.Add(exception);
            }

            //Version date
            if (dbDocument.version_date != null)
            {
                if (dbDocument.version_date.Value < DateTime.UtcNow.AddHours(12))
                {
                    evprmAttachment.attachmentversiondate = dbDocument.version_date.Value.ToString("yyyyMMdd");
                    evprmAttachment.versiondateformat = "102";
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.ATT.attachmentversiondate.BR3(), operationType);
                    exception.AddReadyDescription(readyDocNavigateurl, () => dbDocument.document_PK, () => dbDocument.version_date);
                    exception.AddEvprmDescription(evprmAttachmentLocation, "attachmentversiondate", dbDocument.version_date.Value.ToString("yyyyMMdd"));
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.ATT.attachmentversiondate.Cardinality(), operationType);
                exception.AddReadyDescription(readyDocNavigateurl, () => dbDocument.document_PK, () => dbDocument.version_date);
                exception.AddEvprmDescription(evprmAttachmentLocation, "attachmentversiondate", null);
                validationExceptions.Add(exception);
            }

            //File name
            if (!string.IsNullOrWhiteSpace(dbAttachment.attachmentname) && Path.HasExtension(dbAttachment.attachmentname))
            {
                if (dbAttachment.attachmentname.Trim().Length <= 200)
                {
                    evprmAttachment.filename = dbAttachment.attachmentname.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.ATT.filename.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyDocNavigateurl, () => dbAttachment.attachment_PK, () => dbAttachment.attachmentname);
                    validationExceptions.Add(exception);
                }

                //File type
                string extension = Path.GetExtension(dbAttachment.attachmentname.Trim());
                extension = extension != null ? extension.Replace(".", "") : string.Empty;

                var dbAttachmentFileType = dbDocument.attachment_type_FK.HasValue ? typeOperations.GetEntity(dbDocument.attachment_type_FK) : null;
                if (dbAttachmentFileType != null)
                {
                    if (dbAttachmentFileType.name != null && dbAttachmentFileType.name.Trim().ToLower() != extension.ToLower())
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.ATT.filetype.DataType(XevprmValidationRules.DataTypeEror.InvalidValue), operationType);
                        exception.AddReadyDescription(readyDocNavigateurl, () => dbDocument.document_PK, () => dbDocument.attachment_type_FK);
                        exception.AddEvprmDescription(evprmAttachmentLocation, "filetype", dbAttachmentFileType.ev_code);
                        validationExceptions.Add(exception);
                    }
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.ATT.filename.Cardinality(), operationType);
                exception.AddReadyDescription(readyDocNavigateurl, () => dbAttachment.attachment_PK, () => dbAttachment.attachmentname);
                exception.AddEvprmDescription(evprmAttachmentLocation, "filename", dbAttachment.attachmentname);
                validationExceptions.Add(exception);
            }

            //Attachment name
            if (!string.IsNullOrWhiteSpace(dbDocument.name))
            {
                if (dbDocument.name.Trim().Length <= 2000)
                {
                    evprmAttachment.attachmentname = dbDocument.name.Trim();
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.ATT.attachmentname.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                    exception.AddReadyDescription(readyDocNavigateurl, () => dbDocument.document_PK, () => dbDocument.name);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.ATT.attachmentname.Cardinality(), operationType);
                exception.AddReadyDescription(readyDocNavigateurl, () => dbDocument.document_PK, () => dbDocument.name);
                exception.AddEvprmDescription(evprmAttachmentLocation, "attachmentname", dbDocument.name);
                validationExceptions.Add(exception);
            }

            //Attachment version
            var dbAttachmentVersionType = typeOperations.GetEntity(dbDocument.version_number);
            if (dbAttachmentVersionType != null)
            {
                if (!string.IsNullOrWhiteSpace(dbAttachmentVersionType.name))
                {
                    if (dbAttachmentVersionType.name.Trim().Length <= 5)
                    {
                        evprmAttachment.attachmentversion = dbAttachmentVersionType.name.Trim();
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.ATT.attachmentversion.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                        exception.AddReadyDescription(readyDocNavigateurl, () => dbAttachmentVersionType.type_PK, () => dbAttachmentVersionType.name);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.ATT.attachmentversion.DataType(XevprmValidationRules.DataTypeEror.ValueIsMissing), operationType);
                    exception.AddReadyDescription(readyDocNavigateurl, () => dbAttachmentVersionType.type_PK, () => dbAttachmentVersionType.name);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.ATT.attachmentversion.Cardinality(), operationType);
                exception.AddReadyDescription(readyDocNavigateurl, () => dbDocument.document_PK, () => dbDocument.version_number);
                exception.AddEvprmDescription(evprmAttachmentLocation, "attachmentversion", null);
                validationExceptions.Add(exception);
            }

            //File type
            var dbAttachmentType = typeOperations.GetEntity(dbDocument.attachment_type_FK);
            if (dbAttachmentType != null)
            {
                if (!string.IsNullOrWhiteSpace(dbAttachmentType.type))
                {
                    if (dbAttachmentType.type.Trim().In("1", "2", "3", "4", "5"))
                    {
                        evprmAttachment.filetype = int.Parse(dbAttachmentType.type);
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.ATT.filetype.DataType(XevprmValidationRules.DataTypeEror.InvalidValueFormat), operationType);
                        exception.AddReadyDescription(readyDocNavigateurl, () => dbAttachmentType.type_PK, () => dbAttachmentType.type);
                        exception.AddEvprmDescription(evprmAttachmentLocation, "filetype", null);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    var exception = new XevprmValidationException(new XevprmValidationRules.ATT.filetype.DataType(XevprmValidationRules.DataTypeEror.ValueIsMissing), operationType);
                    exception.AddReadyDescription(readyDocNavigateurl, () => dbAttachmentType.type_PK, () => dbAttachmentType.type);
                    exception.AddEvprmDescription(evprmAttachmentLocation, "filetype", null);
                    validationExceptions.Add(exception);
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.ATT.filetype.Cardinality(), operationType);
                exception.AddReadyDescription(readyDocNavigateurl, () => dbDocument.document_PK, () => dbDocument.attachment_type_FK);
                exception.AddEvprmDescription(evprmAttachmentLocation, "filetype", null);
                validationExceptions.Add(exception);
            }

            //Language code
            var dbDocLangCodeMnList = documentLanguageMnOperations.GetLanguagesByDocument(dbDocument.document_PK);
            if (dbDocLangCodeMnList != null && dbDocLangCodeMnList.Count > 0)
            {
                var dbLanguageCode = languagecodeOperations.GetEntity(dbDocLangCodeMnList[0].language_FK);
                if (dbLanguageCode != null)
                {
                    if (!string.IsNullOrWhiteSpace(dbLanguageCode.code))
                    {
                        if (dbLanguageCode.code.Trim().Length <= 2)
                        {
                            evprmAttachment.languagecode = dbLanguageCode.code.Trim();
                        }
                        else
                        {
                            var exception = new XevprmValidationException(new XevprmValidationRules.ATT.languagecode.DataType(XevprmValidationRules.DataTypeEror.ValueLenghtToBig), operationType);
                            exception.AddReadyDescription(readyDocNavigateurl, () => dbLanguageCode.languagecode_PK, () => dbLanguageCode.code);
                            validationExceptions.Add(exception);
                        }
                    }
                    else
                    {
                        var exception = new XevprmValidationException(new XevprmValidationRules.ATT.languagecode.DataType(XevprmValidationRules.DataTypeEror.ValueIsMissing), operationType);
                        exception.AddReadyDescription(readyDocNavigateurl, () => dbLanguageCode.languagecode_PK, () => dbLanguageCode.code);
                        validationExceptions.Add(exception);
                    }
                }
                else
                {
                    exceptions.Add(new Exception(string.Format("Document and Language code MN relationship with ID = '{0}' is incorrect.", dbDocLangCodeMnList[0].document_language_mn_PK)));
                }
            }
            else
            {
                var exception = new XevprmValidationException(new XevprmValidationRules.ATT.languagecode.Cardinality(), operationType);
                exception.AddReadyDescription(readyDocNavigateurl, typeof(Document_language_mn_PK), null, "document_language_mn_PK", null);
                exception.AddEvprmDescription(evprmAttachmentLocation, "languagecode", null);
                validationExceptions.Add(exception);
            }

            validationExceptionTree.Value.XevprmValidationExceptions.AddRange(validationExceptions);

            return GetValidationResult(ref validationExceptions, ref exceptions, ref validationExceptionTree);
        }
    }
}
