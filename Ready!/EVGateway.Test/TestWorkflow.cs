using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Ionic.Zip;
using eudravigilance.ema.europa.eu.schema.emaxevmpd;
using xEVMPD;
using Ready.Model;
using EVMessage.AS2;
using System.Transactions;
using System.Net;
using System.Threading;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Globalization;
using System.Diagnostics;
using EVMessage.Xevprm;

namespace EVGateway.Test
{
    struct Task
    {
        public int AuthorisedProductID { get; set; }

        public int RepeatNumber { get; set; }

        public int AttachmentID { get; set; }

        public Task(int authorisedProductID, int attachmentID, int repeatNumber)
            : this()
        {
            AuthorisedProductID = authorisedProductID;
            AttachmentID = attachmentID;
            RepeatNumber = repeatNumber;
        }
    }

    class TestWorkflow
    {
        static IXevprm_message_PKOperations _xevprm_message_PKOperation = new Xevprm_message_PKDAL();
        static ISent_message_PKOperations _sent_message_PKOperations = new Sent_message_PKDAL();
        static IAuthorisedProductOperations _authorisedProductOperations = new AuthorisedProductDAL();
        static IAttachment_PKOperations _attachment_PKOperations = new Attachment_PKDAL();
        static IXevprm_log_PKOperations _logOperations = new Xevprm_log_PKDAL();

        private static readonly string _gatewayThumbprint = ConfigurationManager.AppSettings["gatewayThumbprint"];
        private static readonly string _signingThumbprint = ConfigurationManager.AppSettings["signingThumbprint"];

        private static readonly string _AS2SenderID = ConfigurationManager.AppSettings["AS2SenderID"];
        private static readonly string _AS2GatewayID = ConfigurationManager.AppSettings["AS2GatewayID"];
        private static readonly string _AS2ExchangePointURI = ConfigurationManager.AppSettings["AS2ExchangePointURI"];
        private static readonly string _MDNReceiptURL = ConfigurationManager.AppSettings["MDNReceiptURL"];
        private static readonly string _EMAMDNReceiptURL = ConfigurationManager.AppSettings["EMAMDNReceiptURL"];

        static void Main(string[] args)
        {
            IRecieved_message_PKOperations recievedMessagePkOperations = new Recieved_message_PKDAL();
            var rcv = recievedMessagePkOperations.GetEntity(44);

            //Test();

            //ParseXevprmMessagesAndPopulateXevprmEntityDetailsTables();

            //TestEVGateway();

            //TestUpdateOperationWithMFL();
        }

        static void Test()
        {
            string filename = "D:\\Downloads\\ma.xml";
            byte[] data = File.ReadAllBytes(filename);
            
            using (MemoryStream ms = new MemoryStream(data))
            {
                EVMessage.XmlValidator.XmlValidator xmlValidator = new EVMessage.XmlValidator.XmlValidator();

                xmlValidator.Validate(data, ASCIIEncoding.ASCII.GetBytes(EVMessage.Properties.Resources.MarketingAuthorisationXSD));
                //xmlValidator.ValidateMarketingAuthorisationXML(ms);

                xmlValidator.GetValidationExceptions().ForEach(delegate(string item) { Console.WriteLine(item); });
            }
            
            xEVPRMessage xmsg = new xEVPRMessage();
            XRootNamespace xRootNamespace = XRootNamespace.Parse(File.ReadAllText("D:\\Downloads\\ma1.xml"));
            xmsg.Message = xRootNamespace.evprm;
            
        }

        //static void ParseXevprmMessagesAndPopulateXevprmEntityDetailsTables()
        //{
        //    IXevprm_message_PKOperations _xevprm_message_PKOperations = new Xevprm_message_PKDAL();
        //    IXevprm_entity_details_mn_PKOperations _xevprm_entity_details_mn_PKOperations = new Xevprm_entity_details_mn_PKDAL();
        //    IXevprm_ap_details_PKOperations _xevprm_ap_details_PKOperations = new Xevprm_ap_details_PKDAL();
        //    IXevprm_attachment_details_PKOperations _xevprm_attachment_details_PKOperations = new Xevprm_attachment_details_PKDAL();
        //    IXevprm_entity_type_PKOperations _xevprmEntityTypePKOperations = new Xevprm_entity_type_PKDAL();
        //    IAuthorisedProductOperations _authorisedProductOperations = new AuthorisedProductDAL();
        //    IProduct_PKOperations _product_PKOperations = new Product_PKDAL();
        //    IType_PKOperations _type_PKOperations = new Type_PKDAL();
        //    IOrganization_PKOperations _organization_PKOperations = new Organization_PKDAL();
        //    IDocument_PKOperations _document_PKOperations = new Document_PKDAL();
        //    IAttachment_PKOperations _attachment_PKOperations = new Attachment_PKDAL();
        //    ILanguagecode_PKOperations _languagecode_PKOperations = new Languagecode_PKDAL();
        //    IDocument_language_mn_PKOperations _document_language_mn_PKOperations = new Document_language_mn_PKDAL();

        //    List<Xevprm_message_PK> messages = _xevprm_message_PKOperations.GetEntities();

        //    Xevprm_entity_type_PK apEntityType = _xevprmEntityTypePKOperations.GetEntities().Find(item => item.name != null && item.name.Trim().ToLower() == "authorised product");

        //    if (apEntityType == null)
        //    {
        //        Console.WriteLine("'Authorised product' xevprm entity type is missing from XEVPRM_ENTITY_TYPE table");
        //        Console.ReadLine();
        //        return;
        //    }

        //    Xevprm_entity_type_PK attachEntityType = _xevprmEntityTypePKOperations.GetEntities().Find(item => item.name != null && item.name.Trim().ToLower() == "attachment");

        //    if (attachEntityType == null)
        //    {
        //        Console.WriteLine("'Attachment' xevprm entity type is missing from XEVPRM_ENTITY_TYPE table");
        //        Console.ReadLine();
        //        return;
        //    }

        //    int numSuccessfullyParsed = 0;

        //    foreach (Xevprm_message_PK message in messages)
        //    {
        //        try
        //        {
        //            message.message_number = message.xevprm_message_PK.HasValue ? message.xevprm_message_PK.Value.ToString() : null;
        //            message.deleted = false;

        //            Xevprm_ap_details_PK xevprmAPDetails = new Xevprm_ap_details_PK();
        //            Xevprm_attachment_details_PK xevprmAttachDetails = new Xevprm_attachment_details_PK();

        //            xevprmAPDetails.ap_FK = message.ap_FK;

        //            AuthorisedProduct ap = _authorisedProductOperations.GetEntity(message.ap_FK);

        //            if (ap != null)
        //            {
        //                xevprmAPDetails.related_product_FK = ap.product_FK;

        //                if (ap.product_FK.HasValue)
        //                {
        //                    Product_PK product = _product_PKOperations.GetEntity(ap.product_FK);

        //                    if (product != null)
        //                    {
        //                        xevprmAPDetails.related_product_name = product.name;
        //                    }
        //                }

        //                Type_PK authStatus = _type_PKOperations.GetEntity(ap.authorisationstatus_FK);

        //                if (authStatus != null)
        //                {
        //                    xevprmAPDetails.authorisation_status = authStatus.name;
        //                }

        //                Organization_PK licenceHolder = _organization_PKOperations.GetEntity(ap.organizationmahcode_FK);

        //                if (licenceHolder != null)
        //                {
        //                    xevprmAPDetails.licence_holder = licenceHolder.name_org;
        //                }

        //                List<Document_PK> docList = _document_PKOperations.GetDocumentsByAP(ap.ap_PK.Value);

        //                foreach (Document_PK document in docList)
        //                {
        //                    Type_PK typeDoc = _type_PKOperations.GetEntity(document.type_FK);
        //                    if (typeDoc != null && !string.IsNullOrWhiteSpace(typeDoc.name) && typeDoc.name.Trim().ToLower() == "ppi")
        //                    {
        //                        List<Attachment_PK> attachList = _attachment_PKOperations.GetAttachmentsForDocument(document.document_PK.Value);

        //                        if (attachList != null && attachList.Count > 0)
        //                        {
        //                            xevprmAttachDetails.attachment_FK = attachList[0].attachment_PK;

        //                            Type_PK attachType = _type_PKOperations.GetEntity(document.type_FK);

        //                            if (attachType != null)
        //                            {
        //                                xevprmAttachDetails.attachment_type = attachType.name;
        //                            }

        //                            Type_PK fileType = _type_PKOperations.GetEntity(document.attachment_type_FK);

        //                            if (fileType != null)
        //                            {
        //                                xevprmAttachDetails.file_type = fileType.name;
        //                            }

        //                            Type_PK attachVersion = _type_PKOperations.GetEntity(document.version_number);

        //                            if (attachVersion != null)
        //                            {
        //                                xevprmAttachDetails.attachment_version = attachVersion.name;
        //                            }

        //                            List<Document_language_mn_PK> documentLanguageMNList = _document_language_mn_PKOperations.GetLanguagesByDocument(document.document_PK);

        //                            if (documentLanguageMNList != null && documentLanguageMNList.Count > 0)
        //                            {
        //                                Languagecode_PK languageCode = _languagecode_PKOperations.GetEntity(documentLanguageMNList[0].language_FK);
        //                                if (languageCode != null)
        //                                {
        //                                    xevprmAttachDetails.language_code = languageCode.code;
        //                                }
        //                            }

        //                            xevprmAttachDetails.attachment_name = document.name;
        //                            xevprmAttachDetails.attachment_version_date = document.version_date;
        //                            xevprmAttachDetails.file_name = attachList[0].attachmentname;
        //                        }

        //                        break;
        //                    }
        //                }
        //            }

        //            if (!string.IsNullOrWhiteSpace(message.xml))
        //            {
        //                bool isSuccessfullyParsed = true;
        //                xEVPRMessage xmsg = new xEVPRMessage();

        //                try
        //                {
        //                    XRootNamespace xRootNamespace = XRootNamespace.Parse(message.xml);
        //                    xmsg.Message = xRootNamespace.evprm;
        //                }
        //                catch
        //                {
        //                    isSuccessfullyParsed = false;
        //                    Console.WriteLine("Message {0} has xml but parsing error occured.", message.xevprm_message_PK);
        //                }

        //                if (isSuccessfullyParsed && xmsg.Message != null)
        //                {
        //                    if (xmsg.Message.ichicsrmessageheader != null)
        //                    {
        //                        message.sender_ID = xmsg.Message.ichicsrmessageheader.messagesenderidentifier;
        //                    }

        //                    if (xmsg.Message.authorisedproducts != null && xmsg.Message.authorisedproducts.authorisedproduct != null && xmsg.Message.authorisedproducts.authorisedproduct.Count > 0)
        //                    {
        //                        if (xmsg.Message.authorisedproducts.authorisedproduct[0].presentationname != null)
        //                        {
        //                            xevprmAPDetails.ap_name = xmsg.Message.authorisedproducts.authorisedproduct[0].presentationname.productname;
        //                            xevprmAPDetails.package_description = xmsg.Message.authorisedproducts.authorisedproduct[0].presentationname.packagedesc;
        //                        }

        //                        if (xmsg.Message.authorisedproducts.authorisedproduct[0].authorisation != null)
        //                        {
        //                            xevprmAPDetails.authorisation_country_code = xmsg.Message.authorisedproducts.authorisedproduct[0].authorisation.authorisationcountrycode;
        //                            xevprmAPDetails.authorisation_number = xmsg.Message.authorisedproducts.authorisedproduct[0].authorisation.authorisationnumber;
        //                        }

        //                        xevprmAPDetails.operation_type = xmsg.Message.authorisedproducts.authorisedproduct[0].operationtype;
        //                    }

        //                    if (xmsg.Message.attachments != null && xmsg.Message.attachments.attachment != null && xmsg.Message.attachments.attachment.Count > 0)
        //                    {
        //                        xevprmAttachDetails.attachment_name = xmsg.Message.attachments.attachment[0].attachmentname;
        //                        xevprmAttachDetails.attachment_version = xmsg.Message.attachments.attachment[0].attachmentversion;

        //                        DateTime attachmentVersionDate;
        //                        if (DateTime.TryParseExact(xmsg.Message.attachments.attachment[0].attachmentversiondate, "yyyymmdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out attachmentVersionDate))
        //                        {
        //                            xevprmAttachDetails.attachment_version_date = attachmentVersionDate;
        //                        }

        //                        xevprmAttachDetails.file_name = xmsg.Message.attachments.attachment[0].filename;
        //                        xevprmAttachDetails.language_code = xmsg.Message.attachments.attachment[0].languagecode;
        //                        xevprmAttachDetails.operation_type = xmsg.Message.attachments.attachment[0].operationtype;
        //                    }
        //                }
        //            }

        //            if (!string.IsNullOrWhiteSpace(message.ack))
        //            {
        //                bool isSuccessfullyParsed = true;
        //                ACKMessage ack = new ACKMessage();

        //                try
        //                {
        //                    ack.From(message.ack);
        //                }
        //                catch
        //                {
        //                    isSuccessfullyParsed = false;
        //                    Console.WriteLine("Message {0} has ack but parsing error occured.", message.xevprm_message_PK);
        //                }

        //                if (isSuccessfullyParsed)
        //                {
        //                    message.ack_type = (int?) ack.MessageStatus;

        //                    if (ack.Message != null &&
        //                        ack.Message.acknowledgment != null &&
        //                        ack.Message.acknowledgment.reportacknowledgment != null &&
        //                        ack.Message.acknowledgment.reportacknowledgment.Count() > 0)
        //                    {
        //                        foreach (var reportacknowledgment in ack.Message.acknowledgment.reportacknowledgment)
        //                        {
        //                            if (reportacknowledgment.operationtype == "1" && reportacknowledgment.operationresult == "2")
        //                            {
        //                                if (reportacknowledgment.reportname == reportacknowledgmentReportname.AUTHORISEDPRODUCT)
        //                                {
        //                                    xevprmAPDetails.ev_code = reportacknowledgment.ev_code;
        //                                }
        //                                else if (reportacknowledgment.reportname == reportacknowledgmentReportname.ATTACHMENT)
        //                                {
        //                                    xevprmAttachDetails.ev_code = reportacknowledgment.ev_code;
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            using (TransactionScope ts = new TransactionScope())
        //            {
        //                xevprmAPDetails = _xevprm_ap_details_PKOperations.Save(xevprmAPDetails);
        //                xevprmAttachDetails = _xevprm_attachment_details_PKOperations.Save(xevprmAttachDetails);

        //                _xevprm_message_PKOperation.Save(message);

        //                _xevprm_entity_details_mn_PKOperations.Save(new Xevprm_entity_details_mn_PK(null, message.xevprm_message_PK, xevprmAPDetails.xevprm_ap_details_PK, apEntityType.xevprm_entity_type_PK));
        //                _xevprm_entity_details_mn_PKOperations.Save(new Xevprm_entity_details_mn_PK(null, message.xevprm_message_PK, xevprmAttachDetails.xevprm_attachment_details_PK, attachEntityType.xevprm_entity_type_PK));

        //                ts.Complete();
        //            }

        //            numSuccessfullyParsed++;

        //            Console.WriteLine("Message {0} parsed successfully", message.xevprm_message_PK);
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine("Message {0} failed to parse. Error: {1} | StackTrace: {2}", message.xevprm_message_PK, ex.Message, ex.StackTrace);
        //            Console.ReadLine();
        //        }

        //    }
        //    Console.WriteLine("__________________________________________________________");
        //    Console.WriteLine("Result: {0}/{1} messages parsed successfully.", numSuccessfullyParsed, messages.Count);

        //    Console.ReadLine();
        //}

        //private static void TestEVGateway()
        //{
        //    LogEvent("__________________________Test: EVGateway.Test started__________________________");

        //    LogEvent("Loading configuration file started");
        //    List<Task> tasks = GetTasksFromConfig();
        //    LogEvent("Loading configuration file finished");

        //    LogEvent("Creating messages for sending started");
        //    List<Xevprm_message_PK> messagesToSend = CreateMessagesToSend(tasks);
        //    LogEvent("Creating messages for sending finished");

        //    LogEvent("Sending messages started");
        //    SendMessages(messagesToSend);
        //    LogEvent("Sending messages finished");

        //    LogEvent("__________________________Test: EVGateway.Test finished__________________________\r\n");
        //}

        //private static void TestUpdateOperationWithMFL()
        //{
        //    ConstructAndSendMasterFileLocation();

        //    //IRecieved_message_PKOperations _recieved_message_PKOperations = new Recieved_message_PKDAL();
        //    //Recieved_message_PK receivedMessage = _recieved_message_PKOperations.GetEntity(1);
        //    //SendAsyncMDNForAck(receivedMessage);

        //    //UpdateAndSendMasterFileLocation(270);
        //}

        //private static List<Xevprm_message_PK> CreateMessagesToSend(List<Task> tasks)
        //{
        //    var messagesToSend = new List<Xevprm_message_PK>();

        //    foreach (Task task in tasks)
        //    {
        //        LogEventDetails("Processing task (APID = " + task.AuthorisedProductID + "; AttachID = " + task.AttachmentID + "; Num = " + task.RepeatNumber + ") started");
        //        AuthorisedProduct ap;
        //        try
        //        {
        //            //using (TransactionScope ts = new TransactionScope())
        //            //{
        //            if (ConfigurationManager.AppSettings["CreateNewAPDocAttach"] != null &&
        //                ConfigurationManager.AppSettings["CreateNewAPDocAttach"].Trim().ToLower() == "true")
        //            {
        //                LogEventDetails("Creating new authorised product, document and attachment started");

        //                bool success = CreateNewAP(task, out ap);

        //                if (success)
        //                {
        //                    LogEventDetails("Creating new authorised product, document and attachment finished");
        //                }
        //                else
        //                {
        //                    LogEventDetails("Creating new authorised product, document and attachment failed");
        //                    throw new Exception("Creating new authorised product, document and attachment failed");
        //                }
        //            }
        //            else
        //            {
        //                ap = _authorisedProductOperations.GetEntity(task.AuthorisedProductID);
        //            }

        //            if (ap != null)
        //            {
        //                LogEventDetails("AP (APID=" + ap.ap_PK + ") loaded successfully");
        //            }
        //            else
        //            {
        //                LogEventDetails("AP failed to load");
        //                throw new Exception("AP failed to load");
        //            }

        //            for (int msgNum = 0; msgNum < task.RepeatNumber; msgNum++)
        //            {
        //                LogEventDetails("Creating new message started");
        //                var message = new Xevprm_message_PK
        //                                  {
        //                                      message_creation_date = DateTime.Now,
        //                                      status = XevprmStatus.Created,
        //                                      //OperationType = XevprmOperationType.Insert,
        //                                      //ap_FK = ap.ap_PK,
        //                                      user_FK = 0
        //                                  };
        //                message = _xevprm_message_PKOperation.Save(message);

        //                bool success = xEVPRMessage.ValidateAP((int)message.xevprm_message_PK, (int)ap.ap_PK, XevprmOperationType.Insert);

        //                if (!success)
        //                {
        //                    LogEventDetails("Creating new message failed");
        //                    throw new Exception("AP not valid!");
        //                }

        //                message.status = XevprmStatus.Ready;

        //                var msgConstructor = new xEVPRMessage();
        //                msgConstructor.ConstructFrom(message);
        //                message.xml = msgConstructor.ToXmlString();

        //                message.xml_hash = ComputeHash(message.xml);
        //                message.gateway_submission_date = DateTime.Now;
        //                message = _xevprm_message_PKOperation.Save(message);

        //                LogEventDetails("Creating new message finished (MessageID=" + message.xevprm_message_PK + ")");
        //                messagesToSend.Add(message);
        //            }
        //            //ts.Complete();
        //            //}
        //            LogEventDetails("Processing task (APID = " + task.AuthorisedProductID + "; AttachID = " + task.AttachmentID + "; Num = " + task.RepeatNumber + ") finished");
        //        }
        //        catch
        //        {
        //            LogEventDetails("Processing task (APID = " + task.AuthorisedProductID + "; AttachID = " + task.AttachmentID + "; Num = " + task.RepeatNumber + ") failed");
        //        }
        //    }

        //    return messagesToSend;
        //}

        //private static bool CreateNewAP(Task task, out AuthorisedProduct apNew)
        //{
        //    IAuthorisedProductOperations _authorisedProductOperations = new AuthorisedProductDAL();
        //    IDocument_PKOperations _documentPkOperations = new Document_PKDAL();
        //    IAp_document_mn_PKOperations _apDocumentMnPkOperations = new Ap_document_mn_PKDAL();
        //    IAttachment_PKOperations _attachmentPkOperations = new Attachment_PKDAL();
        //    IType_PKOperations _typePkOperations = new Type_PKDAL();
        //    IMeddra_ap_mn_PKOperations _meddra_ap_mn_PKOperations = new Meddra_ap_mn_PKDAL();
        //    IDocument_language_mn_PKOperations _document_language_mn_PKOperations = new Document_language_mn_PKDAL();

        //    apNew = new AuthorisedProduct();

        //    try
        //    {
        //        AuthorisedProduct apOld = _authorisedProductOperations.GetEntity(task.AuthorisedProductID);

        //        if (apOld == null) return false;

        //        List<Document_PK> apOldDocuments = _documentPkOperations.GetDocumentsByAP(apOld.ap_PK.Value);

        //        Document_PK docOld = apOldDocuments.Find(delegate(Document_PK doc)
        //        {
        //            Type_PK type = _typePkOperations.GetEntity(doc.type_FK);

        //            if (type != null && type.name.Trim().ToLower() == "ppi")
        //                return true;

        //            return false;
        //        });

        //        if (docOld == null) return false;

        //        Attachment_PK attachOld = _attachmentPkOperations.GetEntity(task.AttachmentID);

        //        if (attachOld == null) return false;

        //        foreach (PropertyInfo propertyInfo in typeof(AuthorisedProduct).GetProperties())
        //        {
        //            object oldValue = apOld.GetType().GetProperty(propertyInfo.Name).GetValue(apOld, null);
        //            apNew.GetType().GetProperty(propertyInfo.Name).SetValue(apNew, oldValue, null);
        //        }

        //        apNew.ap_PK = null;
        //        apNew.ev_code = null;
        //        apNew = _authorisedProductOperations.Save(apNew);

        //        List<Meddra_ap_mn_PK> medraAPMNListAll = _meddra_ap_mn_PKOperations.GetEntities();

        //        List<Meddra_ap_mn_PK> medraAPMNListNew = new List<Meddra_ap_mn_PK>();

        //        foreach (Meddra_ap_mn_PK medra in medraAPMNListAll)
        //        {
        //            if (medra.ap_FK == apOld.ap_PK)
        //            {
        //                medraAPMNListNew.Add(new Meddra_ap_mn_PK(null, apNew.ap_PK, medra.meddra_FK));
        //            }
        //        }

        //        _meddra_ap_mn_PKOperations.SaveCollection(medraAPMNListNew);

        //        var docNew = new Document_PK();

        //        foreach (PropertyInfo propertyInfo in typeof(Document_PK).GetProperties())
        //        {
        //            object oldValue = docOld.GetType().GetProperty(propertyInfo.Name).GetValue(docOld, null);
        //            docNew.GetType().GetProperty(propertyInfo.Name).SetValue(docNew, oldValue, null);
        //        }
        //        docNew.document_PK = null;
        //        docNew = _documentPkOperations.Save(docNew);

        //        _apDocumentMnPkOperations.Save(new Ap_document_mn_PK(null, docNew.document_PK, apNew.ap_PK));

        //        List<Document_language_mn_PK> docLanguagesOld = _document_language_mn_PKOperations.GetLanguagesByDocument(docOld.document_PK);

        //        docLanguagesOld.ForEach(delegate(Document_language_mn_PK docLang)
        //        {
        //            docLang.document_FK = docNew.document_PK;
        //            docLang.document_language_mn_PK = null;
        //        });

        //        _document_language_mn_PKOperations.SaveCollection(docLanguagesOld);

        //        var attachNew = new Attachment_PK();

        //        foreach (PropertyInfo propertyInfo in typeof(Attachment_PK).GetProperties())
        //        {
        //            object oldValue = attachOld.GetType().GetProperty(propertyInfo.Name).GetValue(attachOld, null);
        //            attachNew.GetType().GetProperty(propertyInfo.Name).SetValue(attachNew, oldValue, null);
        //        }
        //        attachNew.attachment_PK = null;
        //        attachNew.document_FK = docNew.document_PK;
        //        attachNew.ev_code = null;
        //        attachNew = _attachmentPkOperations.Save(attachNew);
        //    }
        //    catch
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //static List<Task> GetTasksFromConfig()
        //{
        //    List<Task> tasks = new List<Task>();

        //    string allConfigTasks = ConfigurationManager.AppSettings["Tasks"];

        //    if (string.IsNullOrWhiteSpace(allConfigTasks)) return tasks;

        //    string[] configTasksArray = allConfigTasks.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

        //    if (!configTasksArray.Any()) return tasks;

        //    LogEventDetails("Loading " + configTasksArray.Count() + " tasks from config file");
        //    foreach (string configTask in configTasksArray)
        //    {
        //        Task task = new Task();

        //        int authorisedProductID = 0;
        //        int attachmentID = 0;
        //        int repeatNumber = 0;

        //        string[] configTaskParams = configTask.Trim().Split('|');

        //        if (configTaskParams.Count() < 3) continue;

        //        if (int.TryParse(configTaskParams[0], out authorisedProductID))
        //        {
        //            task.AuthorisedProductID = authorisedProductID;
        //        }

        //        if (int.TryParse(configTaskParams[1], out attachmentID))
        //        {
        //            task.AttachmentID = attachmentID;
        //        }

        //        if (int.TryParse(configTaskParams[2], out repeatNumber))
        //        {
        //            task.RepeatNumber = repeatNumber;
        //        }

        //        LogEventDetails("Task loaded: Authorised Product ID = " + authorisedProductID + "; Attachment ID = " + attachmentID + "; Number of messages = " + repeatNumber);
        //        tasks.Add(task);
        //    }

        //    return tasks;
        //}

        //public static void SendMessages(List<Xevprm_message_PK> messagesToSend)
        //{
        //    foreach (Xevprm_message_PK message in messagesToSend)
        //    {
        //        ProcessMessage(message);
        //    }
        //}

        //public static void ProcessMessage(Xevprm_message_PK message)
        //{
        //    try
        //    {
        //        //XEVMPD_log_PK log = new XEVMPD_log_PK();
        //        //log.Event = "| SERVICE | Processing message to send!";
        //        //_logOperations.Save(log);

        //        xEVPRMessage xmsg = new xEVPRMessage();
        //        XRootNamespace xRootNamespace = XRootNamespace.Parse(message.xml);
        //        xmsg.Message = xRootNamespace.evprm;

        //        string generatedFilename = _AS2SenderID + "__" + DateTime.Now.ToString("yyyy_MM_dd__HH_mm_ss") + "__" + xmsg.Message.ichicsrmessageheader.messagenumb;

        //        StringBuilder xmsgSB = new StringBuilder();
        //        xmsgSB.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-16\"?>");

        //        //for ack03
        //        //message.XML = message.XML.Replace(" xmlns=\"http://eudrav", " ");

        //        xmsgSB.Append(message.xml);
        //        byte[] xmsgData = UnicodeEncoding.Unicode.GetBytes(xmsgSB.ToString());

        //        byte[] xmsgZipData = new byte[0];
        //        using (var ms = new MemoryStream())
        //        {
        //            using (ZipFile zip = new ZipFile())
        //            {
        //                zip.AddEntry(generatedFilename + ".xml", xmsgData);
        //                try
        //                {
        //                    if (xmsg.Message.attachments != null && xmsg.Message.attachments.attachment.Count > 0)
        //                    {
        //                        _attachment_PKOperations = new Attachment_PKDAL();
        //                        foreach (var item in xmsg.Message.attachments.attachment)
        //                        {
        //                            int attachmentPK = 0;
        //                            if (int.TryParse(item.localnumber, out attachmentPK))
        //                            {
        //                                Attachment_PK attachment = _attachment_PKOperations.GetEntity(attachmentPK);
        //                                if (attachment != null && attachment.disk_file != null)
        //                                {
        //                                    zip.AddEntry(attachment.attachmentname, attachment.disk_file);
        //                                }
        //                                else
        //                                {
        //                                    _logOperations.AddNewEntry(message.xevprm_message_PK, "Error: Failed to add attachment to zip, attachment doesn't contain any data.");
        //                                }
        //                            }

        //                        }
        //                    }
        //                }
        //                catch (Exception e)
        //                {
        //                    _logOperations.AddNewEntry(message.xevprm_message_PK, "Error: Failed to add attachment to zip. (" + e.StackTrace + ")");
        //                    throw e;
        //                }
        //                zip.Save(ms);
        //            }
        //            ms.Seek(0, SeekOrigin.Begin);
        //            xmsgZipData = ms.ToArray();
        //        }

        //        AS2SendSettings as2SendSettings = new AS2SendSettings()
        //        {
        //            Uri = new Uri(_AS2ExchangePointURI),
        //            MsgData = xmsgZipData,
        //            Filename = generatedFilename + ".zip",//"bpet1.zip",
        //            MessageID = xmsg.Message.ichicsrmessageheader.messagenumb,
        //            From = _AS2SenderID,
        //            To = _AS2GatewayID,
        //            TimeoutMs = 3600000,
        //            SigningCertThumbPrint = _signingThumbprint,
        //            RecipientCertThumbPrint = _gatewayThumbprint,
        //            MDNReceiptURL = _MDNReceiptURL
        //        };

        //        AS2Send.SendFile(as2SendSettings);

        //        message.message_status_FK = (int)XevprmStatus.InProgress_Transferred;
        //        message.generated_file_name = generatedFilename;
        //        _xevprm_message_PKOperation.Save(message);

        //        _logOperations.AddNewEntry(message.xevprm_message_PK, "Sending successful");

        //        Sent_message_PK sentMessage = new Sent_message_PK();
        //        sentMessage.msg_data = xmsgZipData;
        //        sentMessage.sent_time = DateTime.Now;
        //        sentMessage.msg_type = (int)SentMessageType.EVPRM;
        //        sentMessage.xevmpd_FK = message.xevprm_message_PK;
        //        _sent_message_PKOperations.Save(sentMessage);
        //    }
        //    catch (Exception e)
        //    {
        //        _logOperations.AddNewEntry(message.xevprm_message_PK, "Sending failed: " + e.StackTrace);
        //        message.message_status_FK = (int)XevprmStatus.InProgress;
        //        _xevprm_message_PKOperation.Save(message);
        //    }

        //}

        //public static string ComputeHash(string data)
        //{
        //    data = PrepareXMLForHash(data);
        //    string result = string.Empty;
        //    using (SHA1Managed sha1 = new SHA1Managed())
        //    {
        //        byte[] hash = sha1.ComputeHash(UnicodeEncoding.Unicode.GetBytes(data));
        //        result = Convert.ToBase64String(hash);
        //    }
        //    return result;
        //}

        //private static string PrepareXMLForHash(string data)
        //{
        //    List<string> elementsToRemove = new List<string>() { "messagedate", "localnumber" };

        //    return RemoveXMLElements(data, elementsToRemove); ;
        //}

        //public static string RemoveXMLElements(string data, List<string> tags)
        //{
        //    foreach (var tag in tags)
        //    {
        //        string openTag = "<" + tag + ">";
        //        string closeTag = "</" + tag + ">";
        //        int openTagIndex = -1;
        //        int closeTagIndex = -1;

        //        int numMatches = Regex.Matches(data, openTag).Count;
        //        for (int counter = 0; counter < numMatches; counter++)
        //        {
        //            openTagIndex = -1;
        //            closeTagIndex = -1;

        //            openTagIndex = data.LastIndexOf(openTag);
        //            closeTagIndex = data.IndexOf(closeTag, openTagIndex);

        //            if (openTagIndex > -1 && closeTagIndex > -1)
        //            {
        //                //include empty spaces before element
        //                int startIndex = openTagIndex;
        //                int emptySpacesLength = Regex.Match(data.Substring(0, startIndex), "[^\\r\\n>]*$").Length;
        //                startIndex -= emptySpacesLength;

        //                //include empty spaces after element
        //                int endIndex = closeTagIndex + closeTag.Length;
        //                emptySpacesLength = Regex.Match(data.Substring(endIndex), "^[^\\r\\n<]*[\\r\\n]*").Length;
        //                endIndex += emptySpacesLength;

        //                //remove element
        //                data = data.Remove(startIndex, endIndex - startIndex);
        //            }
        //        }

        //        //remove all selfclosing tags, empty elements
        //        data = Regex.Replace(data, "[^\\r\\n>]*<" + tag + "\\s*/>[^\\r\\n<]*[\\r\\n]*", "");
        //    }

        //    return data;
        //}

        //private static void LogEvent(String text)
        //{
        //    StreamWriter fileWriter = null;

        //    string logFile = ConfigurationManager.AppSettings["LogFile"];

        //    if (string.IsNullOrWhiteSpace(logFile))
        //        return;

        //    if (File.Exists(logFile))
        //        fileWriter = File.AppendText(logFile);
        //    else
        //        fileWriter = File.CreateText(logFile);

        //    fileWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ": " + text);

        //    fileWriter.Flush();
        //    fileWriter.Close();
        //}

        //private static void LogEventDetails(String text)
        //{
        //    StreamWriter fileWriter = null;

        //    string logFile = ConfigurationManager.AppSettings["LogFile"];

        //    if (string.IsNullOrWhiteSpace(logFile))
        //        return;

        //    if (File.Exists(logFile))
        //        fileWriter = File.AppendText(logFile);
        //    else
        //        fileWriter = File.CreateText(logFile);

        //    fileWriter.WriteLine("\t->" + text);

        //    fileWriter.Flush();
        //    fileWriter.Close();
        //}

        //static void ConstructAndSendMasterFileLocation()
        //{
        //    IOrganization_PKOperations _organization_PKOperations = new Organization_PKDAL();

        //    Organization_PK masterFileLocation = _organization_PKOperations.GetEntity(271);
        //    //Organization_PK masterFileLocation = new Organization_PK()
        //    //{
        //    //    address = "Ilica 100",
        //    //    city = "Zagreb",
        //    //    postcode = "10000",
        //    //    countrycode_FK = 39
        //    //};

        //    //masterFileLocation = _organization_PKOperations.Save(masterFileLocation);

        //    List<xEVMPD.XevprmXmlError> constructionErrors = new List<XevprmXmlError>();

        //    xEVMPD.xEVPRMessage xevprmConstructor = new xEVPRMessage();

        //    xevprmConstructor.ConstructMessageHeader(Guid.NewGuid().ToString());

        //    bool success = xevprmConstructor.ConstructMasterFileLocation(masterFileLocation, XevprmOperationType.Insert, out constructionErrors);

        //    if (success)
        //    {

        //        StringBuilder xevprmSB = new StringBuilder();
        //        xevprmSB.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-16\"?>");
        //        xevprmSB.Append(xevprmConstructor.ToXmlString());

        //        byte[] xmsgData = UnicodeEncoding.Unicode.GetBytes(xevprmSB.ToString());

        //        string generatedFilename = _AS2SenderID + "__" + DateTime.Now.ToString("yyyy_MM_dd__HH_mm_ss") + "__" + xevprmConstructor.Message.ichicsrmessageheader.messagenumb;

        //        File.WriteAllBytes(generatedFilename, xmsgData);

        //        byte[] xmsgZipData = new byte[0];
        //        using (var ms = new MemoryStream())
        //        {
        //            using (ZipFile zip = new ZipFile())
        //            {
        //                zip.AddEntry(generatedFilename + ".xml", xmsgData);
        //                zip.Save(ms);
        //            }
        //            ms.Seek(0, SeekOrigin.Begin);
        //            xmsgZipData = ms.ToArray();
        //        }

        //        AS2SendSettings as2SendSettings = new AS2SendSettings()
        //        {
        //            Uri = new Uri(_AS2ExchangePointURI),
        //            MsgData = xmsgZipData,
        //            Filename = generatedFilename + ".zip",
        //            MessageID = xevprmConstructor.Message.ichicsrmessageheader.messagenumb,
        //            From = _AS2SenderID,
        //            To = _AS2GatewayID,
        //            TimeoutMs = 10000,
        //            SigningCertThumbPrint = _signingThumbprint,
        //            RecipientCertThumbPrint = _gatewayThumbprint,
        //            MDNReceiptURL = _MDNReceiptURL
        //        };

        //        AS2Send.SendFile(as2SendSettings);
        //    }
        //}

        //static void UpdateAndSendMasterFileLocation(int id)
        //{
        //    IOrganization_PKOperations _organization_PKOperations = new Organization_PKDAL();
        //    Organization_PK masterFileLocation = _organization_PKOperations.GetEntity(id);

        //    masterFileLocation.address += "1";

        //    masterFileLocation = _organization_PKOperations.Save(masterFileLocation);

        //    List<xEVMPD.XevprmXmlError> constructionErrors = new List<XevprmXmlError>();

        //    xEVMPD.xEVPRMessage xevprmConstructor = new xEVPRMessage();

        //    xevprmConstructor.ConstructMessageHeader(Guid.NewGuid().ToString());

        //    bool success = xevprmConstructor.ConstructMasterFileLocation(masterFileLocation, XevprmOperationType.Update, out constructionErrors);

        //    if (success)
        //    {

        //        StringBuilder xevprmSB = new StringBuilder();
        //        xevprmSB.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-16\"?>");
        //        xevprmSB.Append(xevprmConstructor.ToXmlString());

        //        byte[] xmsgData = UnicodeEncoding.Unicode.GetBytes(xevprmSB.ToString());

        //        string generatedFilename = _AS2SenderID + "__" + DateTime.Now.ToString("yyyy_MM_dd__HH_mm_ss") + "__" + xevprmConstructor.Message.ichicsrmessageheader.messagenumb;

        //        File.WriteAllBytes(generatedFilename, xmsgData);

        //        byte[] xmsgZipData = new byte[0];
        //        using (var ms = new MemoryStream())
        //        {
        //            using (ZipFile zip = new ZipFile())
        //            {
        //                zip.AddEntry(generatedFilename + ".xml", xmsgData);
        //                zip.Save(ms);
        //            }
        //            ms.Seek(0, SeekOrigin.Begin);
        //            xmsgZipData = ms.ToArray();
        //        }

        //        AS2SendSettings as2SendSettings = new AS2SendSettings()
        //        {
        //            Uri = new Uri(_AS2ExchangePointURI),
        //            MsgData = xmsgZipData,
        //            Filename = generatedFilename + ".zip",
        //            MessageID = xevprmConstructor.Message.ichicsrmessageheader.messagenumb,
        //            From = _AS2SenderID,
        //            To = _AS2GatewayID,
        //            TimeoutMs = 10000,
        //            SigningCertThumbPrint = _signingThumbprint,
        //            RecipientCertThumbPrint = _gatewayThumbprint,
        //            MDNReceiptURL = _MDNReceiptURL
        //        };

        //        AS2Send.SendFile(as2SendSettings);
        //    }
        //}

        //static void SendAsyncMDNForAck(Recieved_message_PK receivedMessage)
        //{
        //    StringBuilder sb = new StringBuilder();

        //    try
        //    {
        //        string receiptDeliveryOption = string.Empty;
        //        string messageID = string.Empty;

        //        Match receiptDeliveryOptionMatch = Regex.Match(receivedMessage.as_header, "Receipt-Delivery-Option:\\s+([^\\r\\n]+)\\r\\n", RegexOptions.IgnoreCase);
        //        if (receiptDeliveryOptionMatch.Success)
        //        {
        //            receiptDeliveryOption = receiptDeliveryOptionMatch.Groups[1].Value;
        //        }
        //        else if (!String.IsNullOrWhiteSpace(_EMAMDNReceiptURL))
        //        {
        //            receiptDeliveryOption = _EMAMDNReceiptURL;
        //        }
        //        else
        //        {
        //            receiptDeliveryOption = "http://195.144.18.238:8080/exchange/EVMPDHVAL"; // test envoironment
        //        }

        //        Match messageIDMatch = Regex.Match(receivedMessage.as_header, "Message-Id:\\s+([^\\r\\n]+)\\r\\n", RegexOptions.IgnoreCase);
        //        if (messageIDMatch.Success)
        //        {
        //            messageID = messageIDMatch.Groups[1].Value.Trim();
        //        }

        //        byte[] payload = ExtractPayload(receivedMessage.msg_data, null);
        //        string MIC = ComputeMIC(payload);

        //        string dateTimeNow = DateTime.Now.ToString("dd-MM-yyyy-hh-mm");

        //        byte[] sentData = new byte[100000];
        //        HttpStatusCode code = SendAsyncMDN2Sign(new Uri(receiptDeliveryOption), "", _AS2SenderID, _AS2GatewayID, messageID, _AS2SenderID, _AS2SenderID, MIC, dateTimeNow, out sentData);


        //        sb.AppendLine();
        //        sb.AppendLine("Receipt-Delivery-Option: " + receiptDeliveryOption);
        //        sb.AppendLine("URI: " + receiptDeliveryOption);
        //        sb.AppendLine("MessageID: " + messageID);
        //        sb.AppendLine("From: " + _AS2SenderID);
        //        sb.AppendLine("To: " + _AS2GatewayID);

        //    }
        //    catch (Exception e)
        //    {
        //        _logOperations.AddNewEntry(receivedMessage.xevmpd_FK, "Sending MDN failed: " + e.StackTrace + " MDN parameters: " + sb.ToString());
        //    }

        //}

        //public static string ComputeMIC(byte[] data)
        //{
        //    using (SHA1Managed sha1 = new SHA1Managed())
        //    {
        //        byte[] hash = sha1.ComputeHash(data);
        //        return Convert.ToBase64String(hash);
        //    }
        //}

        //public static byte[] ExtractPayload(byte[] data, string filePathToSave)
        //{
        //    Encoding enc = Encoding.GetEncoding("iso-8859-1");

        //    string message = enc.GetString(data);

        //    Match boundaryMatch = Regex.Match(message, "------=([^\\s]+)", RegexOptions.IgnoreCase);
        //    string boundary = "------=" + boundaryMatch.Groups[1].Value;

        //    int firstPart = message.IndexOf(boundary) + boundary.Length;
        //    int lastPart = message.IndexOf(boundary + "--") + (boundary + "--").Length;

        //    message = message.Substring(firstPart, lastPart - firstPart).TrimStart();
        //    string payload = message.Substring(0, message.IndexOf(boundary) - 2);

        //    if (!String.IsNullOrWhiteSpace(filePathToSave)) File.WriteAllBytes(filePathToSave, enc.GetBytes(payload));

        //    return enc.GetBytes(payload);
        //}

        //protected static string MIMEBoundary()
        //{
        //    return "----=_Part_" + Guid.NewGuid().ToString("N") + "";
        //}

        //public static byte[] MDNInner(string messageID, string originalRecipient, string finalRecipient, string MIC)
        //{
        //    byte[] mdnInner = new byte[0];

        //    // get a MIME boundary
        //    string sBoundary = MIMEBoundary();

        //    byte[] sContentType = ASCIIEncoding.ASCII.GetBytes("Content-Type: multipart/report;\r\n\tboundary=\"" + sBoundary + "\";\r\n\treport-type=disposition-notification\r\n");

        //    byte[] bBoundary = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + "--" + sBoundary + Environment.NewLine);

        //    StringBuilder sbContent = new StringBuilder();
        //    sbContent.Append("Content-Type: text/plain; charset=us-ascii");
        //    sbContent.Append("\r\n");
        //    sbContent.Append("\r\n");
        //    sbContent.Append("This is MDN info for message ACK number: " + messageID);
        //    sbContent.Append("\r\n");

        //    byte[] innerContent1 = ASCIIEncoding.ASCII.GetBytes(sbContent.ToString());

        //    sbContent.Clear();
        //    sbContent.Append("Content-Type: message/disposition-notification");
        //    sbContent.Append("\r\n");
        //    sbContent.Append("\r\n");
        //    sbContent.Append("Original-Recipient: rfc822; " + originalRecipient);
        //    sbContent.Append("\r\n");
        //    sbContent.Append("Final-Recipient: rfc822; " + finalRecipient);
        //    sbContent.Append("\r\n");
        //    sbContent.Append("Original-Message-ID: " + messageID);
        //    sbContent.Append("\r\n");
        //    sbContent.Append("Disposition: automatic-action/MDN-sent-automatically; processed");
        //    sbContent.Append("\r\n");
        //    sbContent.Append("Received-Content-MIC: " + MIC + ", sha1");
        //    sbContent.Append("\r\n");
        //    sbContent.Append("\r\n");
        //    sbContent.Append("\r\n");

        //    byte[] innerContent2 = ASCIIEncoding.ASCII.GetBytes(sbContent.ToString());

        //    byte[] bFinalFooter = ASCIIEncoding.ASCII.GetBytes("--" + sBoundary + "--\r\n");

        //    // Concatenate all the above together to form the message.
        //    mdnInner = ConcatBytes(sContentType, bBoundary, innerContent1, bBoundary, innerContent2, bFinalFooter);

        //    return mdnInner;
        //}

        //public static byte[] EncodeMDNInner(byte[] arMessage)
        //{
        //    X509Store store = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
        //    store.Open(OpenFlags.ReadOnly);
        //    //X509Certificate2 cert = store.Certificates.Find(X509FindType.FindByThumbprint, (object)"2BFC2DE0B850CCC919AF128ADF2DB526A5850198", true)[0];

        //    X509Certificate2 cert = store.Certificates.Find(X509FindType.FindByThumbprint, (object)"3638DC94E680889D2C793D6BF0213AFE25FDBF64", true)[0];
        //    ContentInfo contentInfo = new ContentInfo(arMessage);

        //    SignedCms signedCms = new SignedCms(contentInfo, true); // <- true detaches the signature
        //    CmsSigner cmsSigner = new CmsSigner(cert);

        //    signedCms.ComputeSignature(cmsSigner);
        //    byte[] signature = signedCms.Encode();

        //    return signature;
        //}

        //public static byte[] MDN(string messageID, string originalRecipient, string finalRecipient, out string sBoundary, string MIC)
        //{
        //    byte[] mdn = new byte[0];

        //    sBoundary = MIMEBoundary();

        //    byte[] bBoundary = ASCIIEncoding.ASCII.GetBytes(Environment.NewLine + "--" + sBoundary + Environment.NewLine);
        //    byte[] bReportMessage = MDNInner(messageID, originalRecipient, finalRecipient, MIC);
        //    byte[] bSignatureHeader = ASCIIEncoding.ASCII.GetBytes(MIMEHeader("application/pkcs7-signature", "binary", ""));
        //    byte[] bSignature = EncodeMDNInner(bReportMessage);
        //    byte[] bFinalFooter = ASCIIEncoding.ASCII.GetBytes("--" + sBoundary + "--\r\n\r\n");

        //    mdn = ConcatBytes(bBoundary, bReportMessage, bBoundary, bSignatureHeader, bSignature, bFinalFooter);

        //    return mdn;
        //}

        //public static string MIMEHeader(string sContentType, string sEncoding, string sDisposition)
        //{
        //    string sOut = "";

        //    sOut = "Content-Type: " + sContentType + Environment.NewLine;

        //    if (sDisposition != "")
        //        sOut += "Content-Disposition: " + sDisposition + Environment.NewLine;

        //    if (sEncoding != "")
        //        sOut += "Content-Transfer-Encoding: " + sEncoding + Environment.NewLine;

        //    sOut = sOut + Environment.NewLine;

        //    return sOut;
        //}

        //public static HttpStatusCode SendAsyncMDN2Sign(Uri uri, string signedFileData, string from, string to, string messageID, string originalRecipient, string finalRecipient, string MIC, string dateTimeNow, out byte[] sentData)
        //{
        //    StringBuilder sbContent = new StringBuilder();

        //    HttpWebRequest http = (HttpWebRequest)WebRequest.Create(uri);

        //    //Define the standard request objects
        //    http.Method = "POST";
        //    http.AllowAutoRedirect = true;
        //    http.KeepAlive = false;
        //    http.PreAuthenticate = false; //Means there will be two requests sent if Authentication required.
        //    http.SendChunked = false;

        //    http.UserAgent = "MY SENDING AGENT";

        //    //These Headers are common to all transactions
        //    http.Headers.Add("Mime-Version", "1.0");
        //    http.Headers.Add("AS2-Version", "1.2");
        //    http.Headers.Add("AS2-From", from);
        //    http.Headers.Add("AS2-To", to);
        //    http.Headers.Add("Message-Id", messageID);

        //    string sBoundary = string.Empty;
        //    byte[] fileData = new byte[0];
        //    try
        //    {
        //        fileData = MDN(messageID, originalRecipient, finalRecipient, out sBoundary, MIC);
        //    }
        //    catch (Exception e)
        //    {
        //        //Log("Error creating ASYNC_MDN_UTF8:" + e.StackTrace);
        //    }
        //    http.ContentLength = fileData.Length;
        //    http.ContentType = "multipart/signed; protocol=\"application/pkcs7-signature\"; micalg=\"sha1\"; boundary=\"" + sBoundary + "\"";

        //    //try
        //    //{
        //    //    File.WriteAllBytes(@"c:\testMDN\SENT_ASYNC_MDN_UTF8__Bytes_" + dateTimeNow + ".txt", fileData);
        //    //}
        //    //catch (Exception e)
        //    //{
        //    //    //Log("Error processing save ASYNC_MDN_UTF8:" + e.StackTrace);
        //    //}

        //    Stream oRequestStream = http.GetRequestStream();
        //    Thread.Sleep(700);
        //    oRequestStream.Write(fileData, 0, fileData.Length);
        //    //Thread.Sleep(700);
        //    oRequestStream.Flush();
        //    //Thread.Sleep(700);
        //    oRequestStream.Close();

        //    HttpStatusCode statusCode = HandleWebResponse(http);

        //    sentData = fileData;

        //    return statusCode;
        //}

        //private static HttpStatusCode HandleWebResponse(HttpWebRequest http)
        //{
        //    HttpStatusCode statusCode;
        //    using (HttpWebResponse response = (HttpWebResponse)http.GetResponse())
        //    {
        //        statusCode = response.StatusCode;
        //        response.Close();
        //    }

        //    return statusCode;
        //}

        //public static byte[] ConcatBytes(params byte[][] arBytes)
        //{
        //    long lLength = 0;
        //    long lPosition = 0;

        //    //Get total size required.
        //    foreach (byte[] ar in arBytes)
        //        lLength += ar.Length;

        //    //Create new byte array
        //    byte[] toReturn = new byte[lLength];

        //    //Fill the new byte array
        //    foreach (byte[] ar in arBytes)
        //    {
        //        ar.CopyTo(toReturn, lPosition);
        //        lPosition += ar.Length;
        //    }

        //    return toReturn;
        //}

    }
}
