using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Transactions;
using CommonComponents;
using Ready.Model;
using System.Configuration;
namespace EVMessage.Xevprm
{
    public static class Xevprm
    {
        public static OperationResult<Xevprm_message_PK> CreateNewMessage(object entity, XevprmEntityType entityType, XevprmOperationType operationType, int userPk, string messageNumber = null)
        {
            if (entity == null)
            {
                return new OperationResult<Xevprm_message_PK>(false, "Entity is null.");
            }

            if (entityType == XevprmEntityType.AuthorisedProduct)
            {
                if (entity is AuthorisedProduct)
                {
                    return CreateNewMessageForAuthorisedProduct(entity as AuthorisedProduct, operationType, userPk, messageNumber);
                }
                else if (entity is int)
                {
                    IAuthorisedProductOperations authorisedProductOperations = new AuthorisedProductDAL();
                    var authorisedProduct = authorisedProductOperations.GetEntity(entity);

                    if (authorisedProduct != null)
                    {
                        return CreateNewMessageForAuthorisedProduct(authorisedProduct, operationType, userPk, messageNumber);
                    }
                    else
                    {
                        return new OperationResult<Xevprm_message_PK>(false, "Authorised product with ID=" + entity + " doesn't exist.");
                    }
                }
                else
                {
                    return new OperationResult<Xevprm_message_PK>(false, "Entity type doesn't match declared type.");
                }
            }

            return new OperationResult<Xevprm_message_PK>(false, "Failed to create message.");
        }

        private static OperationResult<Xevprm_message_PK> CreateNewMessageForAuthorisedProduct(AuthorisedProduct authorisedProduct, XevprmOperationType operationType, int userPk, string messageNumber)
        {
            IXevprm_message_PKOperations xevprmMessageOperations = new Xevprm_message_PKDAL();
            IXevprm_entity_details_mn_PKOperations xevprmEntityDetailsMnOperations = new Xevprm_entity_details_mn_PKDAL();
            IXevprm_ap_details_PKOperations xevprmApDetailsOperations = new Xevprm_ap_details_PKDAL();
            IXevprm_attachment_details_PKOperations xevprmAttachmentDetailsOperations = new Xevprm_attachment_details_PKDAL();

            IType_PKOperations typeOperations = new Type_PKDAL();
            IDocument_PKOperations documentOperations = new Document_PKDAL();
            IAttachment_PKOperations attachmentOperations = new Attachment_PKDAL();

            var message = new Xevprm_message_PK();
            message.message_number = !string.IsNullOrWhiteSpace(messageNumber) ? messageNumber : GenerateMessageNumber(15);

            if (string.IsNullOrWhiteSpace(message.message_number))
            {
                return new OperationResult<Xevprm_message_PK>(false, "Failed to generate unique message number.");
            }

            message.user_FK = userPk;
            message.message_creation_date = DateTime.Now;
            message.XevprmStatus = XevprmStatus.Created;
            message.deleted = false;

            var messageAuthProdDetails = new Xevprm_ap_details_PK
            {
                OperationType = operationType,
                ap_FK = authorisedProduct.ap_PK
            };

            using (var transactionScope = new TransactionScope())
            {
                message = xevprmMessageOperations.Save(message);
                
                messageAuthProdDetails = xevprmApDetailsOperations.Save(messageAuthProdDetails);

                var xevprmAuthProdDetailsMn = new Xevprm_entity_details_mn_PK
                {
                    xevprm_message_FK = message.xevprm_message_PK,
                    xevprm_entity_details_FK = messageAuthProdDetails.xevprm_ap_details_PK,
                    xevprm_entity_type_FK = (int)XevprmEntityType.AuthorisedProduct,
                    xevprm_entity_FK = authorisedProduct.ap_PK,
                    XevprmOperationType = operationType
                };

                xevprmEntityDetailsMnOperations.Save(xevprmAuthProdDetailsMn);

                var documentList = authorisedProduct.ap_PK.HasValue ? documentOperations.GetDocumentsByAP(authorisedProduct.ap_PK.Value) : new List<Document_PK>();

                foreach (var document in documentList)
                {
                    var typeDoc = document.type_FK.HasValue ? typeOperations.GetEntity(document.type_FK) : null;

                    if (typeDoc == null || string.IsNullOrWhiteSpace(typeDoc.name) || typeDoc.name.Trim().ToLower() != "ppi") continue;

                    var attachmentList = document.document_PK.HasValue ? attachmentOperations.GetAttachmentsForDocument(document.document_PK.Value) : new List<Attachment_PK>();

                    if (attachmentList == null || attachmentList.Count <= 0 || !string.IsNullOrWhiteSpace(attachmentList[0].ev_code)) continue;

                    var messageAttachmentDetails = new Xevprm_attachment_details_PK
                    {
                        attachment_FK = attachmentList[0].attachment_PK,
                        operation_type = 1
                    };

                    messageAttachmentDetails = xevprmAttachmentDetailsOperations.Save(messageAttachmentDetails);

                    var xevprmAttachDetailsMn = new Xevprm_entity_details_mn_PK
                    {
                        xevprm_message_FK = message.xevprm_message_PK,
                        xevprm_entity_details_FK = messageAttachmentDetails.xevprm_attachment_details_PK,
                        xevprm_entity_type_FK = (int)XevprmEntityType.Attachment,
                        xevprm_entity_FK = attachmentList[0].attachment_PK,
                        XevprmOperationType = XevprmOperationType.Insert
                    };

                    xevprmEntityDetailsMnOperations.Save(xevprmAttachDetailsMn);
                }

                var updateResult = UpdateXevprmEntityDetailsTables(message);

                if (!updateResult.IsSuccess)
                {
                    return new OperationResult<Xevprm_message_PK>(updateResult.Exception, updateResult.Description);
                }

                transactionScope.Complete();
            }

            return new OperationResult<Xevprm_message_PK>(true, "Message created successfully.", message);
        }

        public static OperationResult<object> UpdateXevprmEntityDetailsTables(int xevprmMessagePk)
        {
            IXevprm_message_PKOperations xevprmMessageOperations = new Xevprm_message_PKDAL();
            var xevprmMessage = xevprmMessageOperations.GetEntity(xevprmMessagePk);

            return UpdateXevprmEntityDetailsTables(xevprmMessage);
        }

        public static OperationResult<object> UpdateXevprmEntityDetailsTables(Xevprm_message_PK xevprmMessage)
        {
            if (xevprmMessage == null)
            {
                return new OperationResult<object>(false, "Xevprm message is null.");
            }

            try
            {
                IXevprm_entity_details_mn_PKOperations xevprmEntityDetailsMnOperations = new Xevprm_entity_details_mn_PKDAL();
                IXevprm_ap_details_PKOperations xevprmApDetailsOperations = new Xevprm_ap_details_PKDAL();
                IXevprm_attachment_details_PKOperations xevprmAttachmentDetailsOperations = new Xevprm_attachment_details_PKDAL();
                IXevprm_message_PKOperations xevprmMessageOperations = new Xevprm_message_PKDAL();

                IAuthorisedProductOperations authorisedProductOperations = new AuthorisedProductDAL();
                IProduct_PKOperations productOperations = new Product_PKDAL();
                IType_PKOperations typeOperations = new Type_PKDAL();
                IOrganization_PKOperations organizationOperations = new Organization_PKDAL();
                IDocument_PKOperations documentOperations = new Document_PKDAL();
                IAttachment_PKOperations attachmentOperations = new Attachment_PKDAL();
                ILanguagecode_PKOperations languagecodeOperations = new Languagecode_PKDAL();
                IDocument_language_mn_PKOperations documentLanguageMnOperations = new Document_language_mn_PKDAL();
                ICountry_PKOperations countryOperations = new Country_PKDAL();

                var messageEntitesMn = xevprmEntityDetailsMnOperations.GetEntitiesByXevprm(xevprmMessage.xevprm_message_PK);

                var errorsSb = new StringBuilder();

                foreach (var messageEntityMn in messageEntitesMn)
                {
                    if (messageEntityMn.xevprm_entity_type_FK == (int)XevprmEntityType.AuthorisedProduct)
                    {
                        var xevprmAuthProdDetails = xevprmApDetailsOperations.GetEntity(messageEntityMn.xevprm_entity_details_FK);

                        if (xevprmAuthProdDetails == null)
                        {
                            errorsSb.AppendLine("Xevprm authorised product details with ID=" + messageEntityMn.xevprm_entity_details_FK + " doesn't exist.");
                            continue;
                        }

                        var authorisedProduct = authorisedProductOperations.GetEntity(xevprmAuthProdDetails.ap_FK);

                        if (authorisedProduct == null)
                        {
                            errorsSb.AppendLine("Authorised product with ID=" + xevprmAuthProdDetails.ap_FK + " doesn't exist.");
                            continue;
                        }

                        xevprmAuthProdDetails.related_product_FK = authorisedProduct.product_FK;
                        xevprmAuthProdDetails.ap_name = authorisedProduct.product_name;
                        xevprmAuthProdDetails.package_description = authorisedProduct.packagedesc;
                        xevprmAuthProdDetails.authorisation_number = authorisedProduct.authorisationnumber;
                        xevprmAuthProdDetails.ev_code = authorisedProduct.ev_code;

                        var country = countryOperations.GetEntity(authorisedProduct.authorisationcountrycode_FK);
                        if (country != null)
                        {
                            xevprmAuthProdDetails.authorisation_country_code = country.abbreviation;
                        }

                        var product = productOperations.GetEntity(authorisedProduct.product_FK);

                        if (product != null)
                        {
                            xevprmAuthProdDetails.related_product_name = product.name;
                        }

                        var authStatus = typeOperations.GetEntity(authorisedProduct.authorisationstatus_FK);

                        if (authStatus != null)
                        {
                            xevprmAuthProdDetails.authorisation_status = authStatus.name;
                        }

                        var licenceHolder = organizationOperations.GetEntity(authorisedProduct.organizationmahcode_FK);

                        if (licenceHolder != null)
                        {
                            xevprmAuthProdDetails.licence_holder = licenceHolder.name_org;
                            xevprmMessage.sender_ID = licenceHolder.organizationsenderid_EMEA;
                        }

                        xevprmApDetailsOperations.Save(xevprmAuthProdDetails);
                        xevprmMessageOperations.Save(xevprmMessage);
                    }
                    else if (messageEntityMn.xevprm_entity_type_FK == (int)XevprmEntityType.Attachment)
                    {
                        var xevprmAttachmentDetails = xevprmAttachmentDetailsOperations.GetEntity(messageEntityMn.xevprm_entity_details_FK);

                        if (xevprmAttachmentDetails == null)
                        {
                            errorsSb.AppendLine("Xevprm attachment details with ID=" + messageEntityMn.xevprm_entity_details_FK + " doesn't exist. ");
                            continue;
                        }

                        if (!xevprmAttachmentDetails.attachment_FK.HasValue)
                        {
                            xevprmAttachmentDetailsOperations.Delete(xevprmAttachmentDetails.xevprm_attachment_details_PK);
                            continue;
                        }
                        
                        var attachment = attachmentOperations.GetEntityWithoutDiskFile(xevprmAttachmentDetails.attachment_FK);

                        if (attachment == null)
                        {
                            errorsSb.AppendLine("Attachment with ID=" + xevprmAttachmentDetails.attachment_FK + " doesn't exist. ");
                            continue;
                        }
                        xevprmAttachmentDetails.file_name = attachment.attachmentname;

                        var document = documentOperations.GetEntity(attachment.document_FK);

                        if (document == null)
                        {
                            errorsSb.AppendLine("Document with ID=" + xevprmAttachmentDetails.attachment_FK + " doesn't exist. ");
                            continue;
                        }

                        var attachType = typeOperations.GetEntity(document.type_FK);

                        if (attachType != null)
                        {
                            xevprmAttachmentDetails.attachment_type = attachType.name;
                        }

                        var fileType = typeOperations.GetEntity(document.attachment_type_FK);

                        if (fileType != null)
                        {
                            xevprmAttachmentDetails.file_type = fileType.name;
                        }

                        var attachVersion = typeOperations.GetEntity(document.version_number);

                        if (attachVersion != null)
                        {
                            xevprmAttachmentDetails.attachment_version = attachVersion.name;
                        }

                        var documentLanguageMnList = documentLanguageMnOperations.GetLanguagesByDocument(document.document_PK);

                        if (documentLanguageMnList != null && documentLanguageMnList.Count > 0)
                        {
                            var languageCode = languagecodeOperations.GetEntity(documentLanguageMnList[0].language_FK);
                            if (languageCode != null)
                            {
                                xevprmAttachmentDetails.language_code = languageCode.code;
                            }
                        }

                        xevprmAttachmentDetails.attachment_name = document.name;
                        xevprmAttachmentDetails.attachment_version_date = document.version_date;

                        xevprmAttachmentDetailsOperations.Save(xevprmAttachmentDetails);
                    }
                }

                if (!string.IsNullOrWhiteSpace(errorsSb.ToString()))
                {
                    return new OperationResult<object>(false, "Xevprm entity details tables updated with current data but some errors encountered: " + errorsSb);
                }

                return new OperationResult<object>(true, "Xevprm entity details tables successfully updated with current data.");
            }
            catch (Exception ex)
            {
                return new OperationResult<object>(ex, "Unexpected error occured at updating xevprm entity details tables.");
            }
        }


        public static string GenerateMessageNumber(int messageNumberLength)
        {
            var xevprmMessageOperations = new Xevprm_message_PKDAL();

            int attemptsRemained = 10;

            while (attemptsRemained > 0)
            {
                if (messageNumberLength > Guid.NewGuid().ToString("N").Length) messageNumberLength = Guid.NewGuid().ToString("N").Length;
                if (messageNumberLength < 1) messageNumberLength = 1;

                string messageNumber = Guid.NewGuid().ToString("N").Substring(0, messageNumberLength);

                messageNumber = Regex.Replace(messageNumber, ".{8}", "$0_");
                messageNumber = messageNumber.TrimEnd(new[] { '_' });

                var existingXevprm = xevprmMessageOperations.GetEntityByMessageNumber(messageNumber);
                if (existingXevprm == null)
                {
                    return messageNumber;
                }
                attemptsRemained--;
            }

            return null;
        }

        public static string GenerateMessageNumber(string prefix, int messageNumberLength)
        {
            var xevprmMessageOperations = new Xevprm_message_PKDAL();

            int attemptsRemained = 10;

            while (attemptsRemained > 0)
            {
                if (messageNumberLength > Guid.NewGuid().ToString("N").Length) messageNumberLength = Guid.NewGuid().ToString("N").Length;
                if (messageNumberLength < 1) messageNumberLength = 1;

                string messageNumber = Guid.NewGuid().ToString("N").Substring(0, messageNumberLength);

                messageNumber = Regex.Replace(messageNumber, ".{8}", "$0_");
                messageNumber = prefix + '_' + messageNumber;

                messageNumber = messageNumber.Trim(new[] { '_' });

                var existingXevprm = xevprmMessageOperations.GetEntityByMessageNumber(messageNumber);
                if (existingXevprm == null)
                {
                    return messageNumber;
                }
                attemptsRemained--;
            }

            return null;
        }
    }
}
