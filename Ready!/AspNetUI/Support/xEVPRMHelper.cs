using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ready.Model;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Data;
using xEVMPD;
using AspNetUIFramework;
using EVMessage.Xevprm;

namespace AspNetUI.Support
{
    public class xEVPRMHelper
    {
        static IDocument_PKOperations _documentOperations = new Document_PKDAL();
        static IAuthorisedProductOperations _authorizedProductOperations = new AuthorisedProductDAL();
        static IProduct_PKOperations _productOperations = new Product_PKDAL();
        static IAttachment_PKOperations _attachmentOperations = new Attachment_PKDAL();
        static IAp_document_mn_PKOperations _apDocumentMNOperations = new Ap_document_mn_PKDAL();
        static IDocument_language_mn_PKOperations _docLanguageCode = new Document_language_mn_PKDAL();

        static List<Type_PK> allTypes;

        /// <summary>
        /// Copies PPI document from Product to Authorized product
        /// </summary>
        /// <param name="ap_PK"></param>
        public static void CopyPPItoAP(int? ap_PK)
        {
            if (!ap_PK.HasValue) return;
            if (allTypes == null) allTypes = CBLoader.LoadTypes();

            List<Document_PK> docsOnAP = _documentOperations.GetDocumentsByAP(ap_PK.Value);
            foreach (Document_PK doc in docsOnAP)
            {
                Type_PK docType = allTypes.Find(type => type.type_PK == doc.type_FK);
                if (docType != null && docType.name.ToLower().Contains("ppi"))
                    return; //if AP already has PPI no aditional action is required
            }

            AuthorisedProduct authorizedProduct = _authorizedProductOperations.GetEntity(ap_PK);
            if (authorizedProduct == null) return;

            Product_PK product = _productOperations.GetEntity(authorizedProduct.product_FK);
            if (product == null) return;

            List<Document_PK> docsOnP = _documentOperations.GetDocumentsByProduct(product.product_PK.Value);
            foreach (Document_PK doc in docsOnP)
            {
                Type_PK docType = allTypes.Find(type => type.type_PK == doc.type_FK);
                if (docType != null && docType.name.ToLower().Contains("ppi"))
                {
                    List<Attachment_PK> attachments = _attachmentOperations.GetAttachmentsForDocumentWithDiskFile(doc.document_PK.Value);
                    if (attachments.Count == 0) return;
                    Attachment_PK PPIAttachmentProduct = attachments.First();

                    Document_PK newPPIDocument = new Document_PK()
                    {
                        attachment_name = doc.attachment_name,
                        attachment_type_FK = doc.attachment_type_FK,
                        change_date = doc.change_date,
                        comment = doc.comment,
                        description = doc.description,
                        document_code = doc.document_code,
                        effective_end_date = doc.effective_end_date,
                        effective_start_date = doc.effective_start_date,
                        localnumber = doc.localnumber,
                        name = doc.name,
                        person_FK = doc.person_FK,
                        regulatory_status = doc.regulatory_status,
                        type_FK = doc.type_FK,
                        version_date = doc.version_date,
                        version_date_format = doc.version_date_format,
                        version_label = doc.version_label,
                        version_number = doc.version_number

                    };
                    newPPIDocument = _documentOperations.Save(newPPIDocument);

                    Attachment_PK newPPIAttachment = new Attachment_PK()
                    {
                        attachmentname = PPIAttachmentProduct.attachmentname,
                        disk_file = PPIAttachmentProduct.disk_file,
                        document_FK = newPPIDocument.document_PK,
                        filetype = PPIAttachmentProduct.filetype,
                        session_id = new Guid(),
                        pom_type = PPIAttachmentProduct.pom_type,
                        userID = PPIAttachmentProduct.userID
                    };
                    newPPIAttachment = _attachmentOperations.Save(newPPIAttachment);

                    Ap_document_mn_PK newDocAP = new Ap_document_mn_PK()
                    {
                        ap_FK = ap_PK,
                        document_FK = newPPIDocument.document_PK
                    };
                    _apDocumentMNOperations.Save(newDocAP);

                    List<Document_language_mn_PK> langCodesOndDoc = _docLanguageCode.GetLanguagesByDocument(doc.document_PK);
                    foreach (Document_language_mn_PK langCode in langCodesOndDoc)
                    {
                        langCode.document_FK = newPPIDocument.document_PK;
                        langCode.document_language_mn_PK = null;
                    }
                    _docLanguageCode.SaveCollection(langCodesOndDoc);
                    return;
                }
            }
        }

        public static bool HasNullifyMessage(DataTable apDT)
        {
            if (apDT != null)
            {
                foreach(DataRow item in apDT.Rows) 
                {
                    if (item["operation_type"] != null && (int)item["operation_type"] == 4 &&
                        item["message_status_FK"] != null && (int)item["message_status_FK"] != 12)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}