using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.IO;
using EVMessage.MarketingAuthorisation;
using EVMessage.XmlValidator;
using Ready.Model;
using System.Transactions;
using System.Text.RegularExpressions;
using System.Threading;

namespace SandozTaskScheduler
{
    public class Workflow
    {

        private static IMarketing_authorisation_PKOperations _marketingAuthorisationOperations;
        private static IMa_message_header_PKOperations _maMessageHeaderOperations;
        private static IMa_service_log_PKOperations _maServiceLogOperations;
        private static IMa_file_PKOperations _maFileOperations;
        private static IXevprm_message_PKOperations _xevprmMessageOperations;
        private static IMa_attachment_PKOperations _maAttachmentOperations;
        private static IMa_ma_entity_mn_PKOperations _maMaEntityMnOperations;
        private static string _serviceLogFile;
        private static readonly string MARootPath;
        private static readonly string InboundFolder;
        private static readonly string OutboundFolder;
        private static readonly string SMPCInboundFolder;

        private static DateTime lastArchivingTime;

        public static string ServiceLogFile
        {
            get { return _serviceLogFile; }
            set { _serviceLogFile = value; }
        }

        /// <summary>
        /// Static construcor. Initializes data access layer and Log file location.
        /// </summary>
        static Workflow()
        {
            //initialize Data acess Layer
            try
            {
                _marketingAuthorisationOperations = new Marketing_authorisation_PKDAL();
                _maMessageHeaderOperations = new Ma_message_header_PKDAL();
                _maServiceLogOperations = new Ma_service_log_PKDAL();
                _maFileOperations = new Ma_file_PKDAL();
                _xevprmMessageOperations = new Xevprm_message_PKDAL();
                _maAttachmentOperations = new Ma_attachment_PKDAL();
                _maMaEntityMnOperations = new Ma_ma_entity_mn_PKDAL();
            }
            catch (Exception e)
            {
                LogError(e, "Failed to initialize data acess layer.", Ma_service_log_PK.EventType.DBError);
            }

            string serviceLogFilePath = ConfigurationManager.AppSettings["LogFilePath"];
            string serviceLogFileName = ConfigurationManager.AppSettings["LogFileName"];

            if (Directory.Exists(serviceLogFilePath))
            {
                _serviceLogFile = serviceLogFilePath;
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(serviceLogFilePath);
                    _serviceLogFile = serviceLogFilePath;
                }
                catch (Exception ex)
                {
                    _serviceLogFile = "C:\\";
                }
            }

            if (!string.IsNullOrWhiteSpace(serviceLogFileName))
            {
                _serviceLogFile += serviceLogFileName;
            }
            else
            {
                _serviceLogFile += "MAServiceLog.txt";
            }

            //initializePaths
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["MARootPath"]))
            {
                MARootPath = ConfigurationManager.AppSettings["MARootPath"];
            }
            else
            {
                MARootPath = null;
                LogError(null, "MARootFolder cannot be found in App.config.", Ma_service_log_PK.EventType.Error);
            }

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["InboundFolder"]))
            {
                InboundFolder = ConfigurationManager.AppSettings["InboundFolder"];
            }
            else
            {
                InboundFolder = null;
                LogError(null, "Inbound folder cannot be found in App.config.", Ma_service_log_PK.EventType.Error);
            }

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["OutboundFolder"]))
            {
                OutboundFolder = ConfigurationManager.AppSettings["OutboundFolder"];
            }
            else
            {
                OutboundFolder = null;
                LogError(null, "Outbound folder cannot be found in App.config.", Ma_service_log_PK.EventType.Error);
            }

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["SMPCInboundFolder"]))
            {
                SMPCInboundFolder = ConfigurationManager.AppSettings["SMPCInboundFolder"];
            }
            else
            {
                SMPCInboundFolder = null;
                LogError(null, "SMPC folder cannot be found in App.config.", Ma_service_log_PK.EventType.Error);
            }

            lastArchivingTime = new DateTime(0);

        }

        /// <summary>
        /// Workflow starter method.
        /// Method starts workflow tasks in predefined order.
        /// </summary>
        public static void Start()
        {

            //  PrepareMessagese(); return;
            if (!IsDBReady())
            {
                LogEvent("DB check failed, processing iteration will be skipped.", Ma_service_log_PK.EventType.DBError);
                return;
            }

            if (MARootPath == null || InboundFolder == null || OutboundFolder == null || SMPCInboundFolder == null)
            {
                LogError(null, "Required folders are not well configured, processing iteration will be skipped.", Ma_service_log_PK.EventType.Error);
                return;
            }

            LogEvent("MA processing iteration started.", Ma_service_log_PK.EventType.Message);

            try
            {
                LogEvent("Processing new SMPCs started.", Ma_service_log_PK.EventType.Message);
                ProcessNewSPCs();
                LogEvent("Processing new SMPCs finished.", Ma_service_log_PK.EventType.Message);
            }
            catch (Exception ex)
            {
                LogError(ex, "Unexpected exception occurred while processing new SMPCs");
            }

            try
            {
                LogEvent("Processing received MA XML files started.", Ma_service_log_PK.EventType.Message);
                ProcessReceivedXMLMessages();
                LogEvent("Processing received MA XML files finished.", Ma_service_log_PK.EventType.Message);
            }
            catch (Exception ex)
            {
                LogError(ex, "Unexpected exception occurred while processing received MA XML files.");
            }

            try
            {
                LogEvent("Processing received MA files started.", Ma_service_log_PK.EventType.Message);
                ProcessReceivedFileMAs();
                LogEvent("Processing received MA files finished,", Ma_service_log_PK.EventType.Message);
            }
            catch (Exception ex)
            {
                LogError(ex, "Unexpected exception occurred while processing received MA files.");
            }

            try
            {
                LogEvent("Processing successfully received MAs started.", Ma_service_log_PK.EventType.Message);
                ProcessReceivedMAs();
                LogEvent("Processing successfully received MAs finished.", Ma_service_log_PK.EventType.Message);
            }
            catch (Exception ex)
            {
                LogError(ex, "Unexpected exception occurred while processing received MAs.");
            }

            try
            {
                LogEvent("Processing successfully validated MAs started.", Ma_service_log_PK.EventType.Message);
                ProcessSuccessfullyValidatedMAs();
                LogEvent("Processing successfully validated MAs finished.", Ma_service_log_PK.EventType.Message);
            }
            catch (Exception ex)
            {
                LogError(ex, "Unexpected exception occurred while processing successfully validated MAs.");
            }

            try
            {
                LogEvent("Processing successfully sent MAs started.", Ma_service_log_PK.EventType.Message);
                ProcessXEVPRMSentMAs();
                LogEvent("Processing successfully sent MAs finished.", Ma_service_log_PK.EventType.Message);
            }
            catch (Exception ex)
            {
                LogError(ex, "Unexpected exception occurred while processing xEVPRM sent MAs.");
            }

            LogEvent("MA processing iteration finished.", Ma_service_log_PK.EventType.Message);
        }

        #region INCOMING XML FILES PROCESSING

        /// <summary>
        /// Method searches for new received XML messages, and process them.
        /// To start processing, method must ensure that required folders 
        /// (location to move received MAs, outbound folder) exists. 
        /// If folders does not exist, and method fails to create them, process is aborted.
        /// </summary>
        private static void ProcessReceivedXMLMessages()
        {

            DirectoryInfo inboundFolder = CreateDirectoryInfo(Path.Combine(MARootPath, InboundFolder));
            if (inboundFolder == null || !inboundFolder.Exists)
            {
                LogError(null, "Inbound folder does not exists!", Ma_service_log_PK.EventType.Error);
                return;
            }

            FileInfo[] newFiles = null;
            try
            {
                newFiles = inboundFolder.GetFiles("*.xml", SearchOption.TopDirectoryOnly);
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to retrive files from inbound folder!", Ma_service_log_PK.EventType.DiskError);
                return;
            }

            foreach (FileInfo newFile in newFiles)
            {
                try
                {
                    ProcessReceivedMAXML(newFile);
                }
                catch (Exception ex)
                {
                    LogError(ex, "Unexpeced exception occured while processing " + newFile.FullName + ".", Ma_service_log_PK.EventType.Error);
                }
            }
        }

        /// <summary>
        /// Method process new file received in inbound folder.
        /// Method creates new MARKETING_AUTHORISATION record for received file
        /// and stores it in DB. If this process is successful, MA gets MAFileReceivedStatus
        /// otherwise gets MAReceivedProcessingFileFailed status.
        /// After that, received files is being moved to new location, if this process fails
        /// event with type MAReceivedFileTransferFailed is logged.
        /// </summary>
        /// <param name="messageFile"></param>
        /// <param name="outboundFolder"></param>
        private static void ProcessReceivedMAXML(FileInfo messageFile)
        {
            if (!messageFile.Exists)
            {
                LogError(null, "File: " + messageFile.FullName + " does not exists");
                return;
            }

            Ma_file_PK existingFile = null;
            try
            {
                existingFile = _maFileOperations.GetEntityByFileNameAndType(messageFile.Name, Ma_file_PK.FileType.MAFile);
            }
            catch (Exception e)
            {
                LogError(e, "Failed to retrieve file from DB.", Ma_service_log_PK.EventType.DBError);
                return;
            }

            if (existingFile != null)
            {
                LogError(null, "File: " + messageFile.FullName + " already exists in DB and cannot be processed!", Ma_service_log_PK.EventType.Error);
                return;
            }

            if (!IsFileReadyForProcessing(messageFile.FullName))
            {
                LogEvent("Message file is in use by another process. File name: " + messageFile.FullName, null, Ma_service_log_PK.EventType.FileInUse);
                return;
            }


            String readyId = GenerateReadyId();

            if (readyId == null)
            {
                LogError(null, "Cannot generate unique ready ID for the file: " + messageFile.FullName);
                return;
            }

            Marketing_authorisation_PK receivedMA = new Marketing_authorisation_PK()
            {
                ready_id = readyId,
                creation_time = DateTime.Now,
                ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAFileLoaded
            };

            Ma_file_PK maFile = null;
            try
            {
                using (TransactionScope scope = new TransactionScope())
                {

                    maFile = new Ma_file_PK()
                    {
                        file_name = messageFile.Name,
                        file_path = messageFile.DirectoryName,
                        file_type_FK = (int)Ma_file_PK.FileType.MAFile,
                        file_data = File.ReadAllBytes(messageFile.FullName),
                        ready_id_FK = receivedMA.ready_id
                    };

                    _marketingAuthorisationOperations.Save(receivedMA);
                    _maFileOperations.Save(maFile);
                    scope.Complete();
                }
            }
            catch (Exception dbException)
            {
                receivedMA.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedProcessingFileFailed;
                SaveMA(receivedMA);
                LogError(dbException, "Saving received MA file to database failed.", null, Ma_service_log_PK.EventType.MaFileSaveToDbFailed);
                return;
            }

            LogEvent("MA file sucessfully saved to db.", receivedMA.ready_id, Ma_service_log_PK.EventType.MaFileSavedToDbSuccessfully);

            Ma_message_header_PK msgHeader = null;

            try
            {
                msgHeader = ExtractMessageHeader(receivedMA, maFile);
            }
            catch (Exception e)
            {
                LogError(e, "Extracting message header failed", receivedMA.ready_id, Ma_service_log_PK.EventType.Error);
                receivedMA.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedProcessingFileFailed;
                SaveMA(receivedMA);
                return;
            }

            try
            {
                _maMessageHeaderOperations.Save(msgHeader);
            }
            catch (Exception ex)
            {
                LogError(ex, "Error saving message header to database!", receivedMA.ready_id, Ma_service_log_PK.EventType.DBError);
                receivedMA.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedProcessingFileFailed;
                SaveMA(receivedMA);
                return;
            }

            String messageFolder = ResolveMessageFolder(msgHeader);
            String inboundMessageFolder = String.IsNullOrWhiteSpace(messageFolder) ? (Path.Combine(MARootPath, InboundFolder, "Archive", "Junk")) : Path.Combine(MARootPath, InboundFolder, "Archive", messageFolder);
            DirectoryInfo moveToFolder = CreateDirectoryInfo(inboundMessageFolder);
            if (moveToFolder != null && !moveToFolder.Exists)
            {
                try
                {
                    Directory.CreateDirectory(moveToFolder.FullName);
                    moveToFolder = new DirectoryInfo(moveToFolder.FullName);
                }
                catch (Exception e)
                {
                    LogError(e, "Failed to create folder: " + moveToFolder.FullName, maFile.ready_id_FK + " for storing received messages!", Ma_service_log_PK.EventType.FailedToCreateDirectory);
                    moveToFolder = null;
                }
            }
            else if (moveToFolder == null)
            {
                LogError(null, "Failed to create DirectoryInfo for" + inboundMessageFolder + " for storing received message!", maFile.ready_id_FK, Ma_service_log_PK.EventType.FailedToCreateDirectory);
            }

            //Move file to processed XML location.
            if (moveToFolder != null && moveToFolder.Exists)
            {
                FileInfo newFile = CreateFileInfo(Path.Combine(moveToFolder.FullName, messageFile.Name));
                if (newFile != null && newFile.Exists)
                {
                    LogEvent("Target file: \"" + newFile.FullName + "\"already exists!", receivedMA.ready_id, Ma_service_log_PK.EventType.MAReceivedFileTransferFailed);
                }
                else
                {
                    try
                    {
                        File.Move(messageFile.FullName, newFile.FullName);
                        try
                        {
                            maFile.file_path = moveToFolder.FullName;
                            _maFileOperations.Save(maFile);
                        }
                        catch (Exception dbException)
                        {
                            LogError(dbException, "Exception occurred while moving file: Cannot store new file location to DB.", receivedMA.ready_id, Ma_service_log_PK.EventType.MAReceivedFileTransferFailed);
                        }
                        LogEvent("File: " + messageFile.Name + " successfully moved to: " + newFile.FullName, receivedMA.ready_id, Ma_service_log_PK.EventType.MAReceivedFileTransferedSuccessfully);
                    }
                    catch (Exception fileTransferException)
                    {
                        LogError(fileTransferException, "Exception occurred while moving file.", receivedMA.ready_id, Ma_service_log_PK.EventType.MAReceivedFileTransferFailed);
                    }
                }
            }
            else
            {
                LogError(null, "Target directory: \"" + moveToFolder + "\" does not exists!", receivedMA.ready_id, Ma_service_log_PK.EventType.MAReceivedFileTransferFailed);
            }


            receivedMA.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAFileReceived;
            receivedMA.message_folder = messageFolder;
            SaveMA(receivedMA);

        }

        #endregion

        #region SUCCESSFULY RECEIVED MA FILES PROCESSING
        /// <summary>
        /// Method process new MA with status  MAFileReceived (files that are successfully received and stored to DB).
        /// Files are retrieved from DB and forwarded to ProcessReceivedMAFileMA method.
        /// </summary>
        private static void ProcessReceivedFileMAs()
        {
            List<Marketing_authorisation_PK> notProcessedMAs = null;

            try
            {
                notProcessedMAs = _marketingAuthorisationOperations.GetEntitiesByStatus(Marketing_authorisation_PK.MAStatus.MAFileReceived);
            }
            catch (Exception dbException)
            {
                LogError(dbException, "Cannot read MAs from DB", null, Ma_service_log_PK.EventType.DBError);
                return;
            }

            foreach (Marketing_authorisation_PK ma in notProcessedMAs)
            {
                try
                {
                    ProcessReceivedFileMA(ma);
                }
                catch (Exception ex)
                {
                    LogError(ex, "Unexpected exception occured while processing received file MA.", ma.ready_id, Ma_service_log_PK.EventType.Error);
                }
            }
        }

        /// <summary>
        /// Received files is validated according to defined XSD, and appropriate report is send after validation success/fail.
        /// Successfully validated MA gets MAReceived status, while XSD validation failed MAs gets MAReceivedErrors status.
        /// If processing of an message fails, it will receive MAReceivedProcessingFileFailed status.
        /// </summary>
        /// <param name="ma"></param>
        private static void ProcessReceivedFileMA(Marketing_authorisation_PK ma)
        {
            EVMessage.XmlValidator.XmlValidator xmlValidator = new EVMessage.XmlValidator.XmlValidator();
            Ma_file_PK maFile;
            try
            {
                maFile = _maFileOperations.GetEntityByReadyIdAndType(ma.ready_id, Ma_file_PK.FileType.MAFile);
            }
            catch (Exception dbException)
            {
                LogError(dbException, "Cannot read MA file from DB-", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedProcessingFileFailed;
                SaveMA(ma);
                return;
            }

            if (maFile == null)
            {
                LogEvent("Cannot find MA File in DB", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedProcessingFileFailed;
                SaveMA(ma);
                return;
            }

            bool isValid = xmlValidator.Validate(maFile.file_data, Encoding.UTF8.GetBytes(EVMessage.Properties.Resources.MarketingAuthorisationXSD));
            if (isValid)
            {
                //check if messageDate is correct
                Ma_message_header_PK msgHeader = null;
                try
                {
                    msgHeader = _maMessageHeaderOperations.GetEntityByReadyId(ma.ready_id);
                }
                catch (Exception ex)
                {
                    LogError(ex, "Failed to get MA header from DB", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedProcessingFileFailed;
                    SaveMA(ma);
                    return;
                }

                if (!msgHeader.messagedate.HasValue)
                {
                    xmlValidator.Exceptions.Add(GetDateException(Encoding.UTF8.GetString(maFile.file_data)));
                    isValid = false;
                }

                
                List<XmlValidatorException> messageNumberExceptions = ValidateMessageNumber(msgHeader.registrationnumber);
                if (messageNumberExceptions.Count > 0)
                {
                    isValid = false;
         
                    xmlValidator.Exceptions.AddRange(messageNumberExceptions);
                }
                
            }

            if (isValid)
            {
                LogEvent("Received MA XML is valid.", ma.ready_id, Ma_service_log_PK.EventType.XMLValidationSuccesfull);
                UpdateMATo_MAReceived(ma);
            }
            else
            {
                LogEvent("Received MA XML is not valid.", ma.ready_id, Ma_service_log_PK.EventType.XMLValidationFailed);
                UpdateMATo_MAReceivedErrors(ma, maFile.file_data, xmlValidator);
            }
        }

        /// <summary>
        /// Method generates and sends MAReceived report and updates MA to MAReceived status.
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="maFileData"></param>
        private static void UpdateMATo_MAReceived(Marketing_authorisation_PK ma)
        {
            Ma_message_header_PK msgHeader = null;
            try
            {
                msgHeader = _maMessageHeaderOperations.GetEntityByReadyId(ma.ready_id);
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to get MA header from DB", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedProcessingFileFailed;
                SaveMA(ma);
                return;
            }

            EVMessage.StatusReport.StatusReportMessage statusReport = new EVMessage.StatusReport.StatusReportMessage();

            try
            {
                statusReport.CreateStatusReportFor_MAReceived(msgHeader, ma.ready_id);
            }
            catch (Exception e)
            {
                LogError(e, "Failed to create MAReceived report.", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedCreateReportFailed);
                return;
            }

            if (!statusReport.IsReportValid)
            {
                LogError(null, "Failed to create valid MAReceived report.", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedCreateReportFailed);
                return;
            }

            LogEvent("MAReceived report created successfully", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedCreateReportSucessful);
            if (SaveReportToDatabase(ma, statusReport, Ma_file_PK.FileType.MAStatusReport_Received))
            {
                LogEvent("MAReceived report successfully saved to DB.", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedSaveReportToDBSuccessful);
                if (SendReport(ma, statusReport))
                {
                    LogEvent("MAReceived report file is successfully saved to outbound folder.", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedSaveReportToOutboundFolderSuccessful);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceived;
                    SaveMA(ma);
                }
                else
                {
                    LogEvent("Error at saving MAReceived report to file.", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedSaveReportToOutboundFolderFailed);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedProcessingFileFailed;
                    SaveMA(ma);
                }
            }
            else
            {
                LogEvent("Error at saving MAReceived report to database", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedSaveReportToDBFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedProcessingFileFailed;
                SaveMA(ma);
            }
        }

        /// <summary>
        /// Method process XSD invalid message.
        /// MAReceivedErrors report is generated and MA is updated to MAReceivedErrors status.
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="fileData"></param>
        /// <param name="xmlValidator"></param>
        private static void UpdateMATo_MAReceivedErrors(Marketing_authorisation_PK ma, byte[] fileData, EVMessage.XmlValidator.XmlValidator xmlValidator)
        {
            //XML is not valid, send error status and abort message processing

            List<string> validationErrors = xmlValidator.GetValidationErrors();
            List<string> validationWarnings = xmlValidator.GetValidationWarnings();
            Ma_message_header_PK msgHeader = null;
            try
            {
                msgHeader = _maMessageHeaderOperations.GetEntityByReadyId(ma.ready_id);
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to get MA header from DB.", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedProcessingFileFailed;
                SaveMA(ma);
                return;
            }

            var statusReport = new EVMessage.StatusReport.StatusReportMessage();
            try
            {
                statusReport.CreateStatusReportFor_MAXmlValidationFailure(msgHeader, xmlValidator, Encoding.UTF8.GetString(fileData), ma);
            }
            catch (Exception e)
            {
                LogError(e, "Failed to create MAReceivedErrors report.", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedCreateReportFailed);
                return;
            }
            if (!statusReport.IsReportValid)
            {
                LogError(null, "Failed to create valid MAReceivedErrors report.", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedCreateReportFailed);
                return;
            }

            if (SaveReportToDatabase(ma, statusReport, Ma_file_PK.FileType.MAStatusReport_ReceivedErrors))
            {
                LogEvent("MAReceivedErrors report saved to DB.", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedSaveReportToDBSuccessful);
                if (SendReport(ma, statusReport))
                {
                    LogEvent("MAReceivedErrors report file is sucessfully saved to outbound folder.", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedSaveReportToOutboundFolderSuccessful);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedErrors;
                    _marketingAuthorisationOperations.Save(ma);
                }
                else
                {
                    LogEvent("Error at saving MAReceivedErrors report to file.", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedSaveReportToOutboundFolderFailed);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedProcessingFileFailed;
                    _marketingAuthorisationOperations.Save(ma);
                }
            }
            else
            {
                LogEvent("Error saving MAReceivedErrors report to database", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedSaveReportToDBFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAReceivedProcessingFileFailed;
                _marketingAuthorisationOperations.Save(ma);
            }
        }


        #endregion

        #region SUCCESSFULLY RECEIVED MAs PROCESSING
        /// <summary>
        /// Method process new MA with status = Marketing_authorisation_PK.MAStatus.MAReceived
        /// (files that are succesfully recieved and stored to DB).
        /// MAs are retrieved from DB and forwrded to ProcessReceivedMA method.
        /// </summary>
        private static void ProcessReceivedMAs()
        {
            List<Marketing_authorisation_PK> notProcessedMAs = _marketingAuthorisationOperations.GetEntitiesByStatus(Marketing_authorisation_PK.MAStatus.MAReceived);
            foreach (Marketing_authorisation_PK ma in notProcessedMAs)
            {
                try
                {
                    ProcessReceivedMA(ma);
                }
                catch (Exception ex)
                {
                    LogError(ex, "Unexpected exception occured while processing received MA.", ma.ready_id, Ma_service_log_PK.EventType.Error);
                }
            }
        }

        /// <summary>
        /// Firstly, MA is translated to Ready!, then xEVRPM message is generated and validated in the third step.
        /// If process is sucessfully completed, and xEVPRM is valid MAValidationSuccessful report is generated.
        /// If process is sucessfully completed, and xEVPRM is not valid MAValidationFailed report is generated.
        /// If translation or xEVPRM creation fails, MAValidationFailed report is generated.
        /// If report is sucessfully sent, MA gets MAValidationSuccessful or MAValidationFailed status.
        /// If validation process fails at any point MA gets MAProcessingDataFailed status.
        /// </summary>
        /// <param name="ma"></param>
        private static void ProcessReceivedMA(Marketing_authorisation_PK ma)
        {
            var maDataExporter = new MADataExporter();
            maDataExporter.MASaveOptions.IgnoreErrorsInValidationProcess = false;
            maDataExporter.MASaveOptions.IgnoreValidationExceptions = false;
            maDataExporter.MASaveOptions.RollbackAllAtSaveException = true;
            maDataExporter.MASaveOptions.UpdateExistingEntities = true;

            //MA translation to Ready! DB
            List<EVMessage.MarketingAuthorisation.ValidationException> translationErrors;
            bool isSuccessfullyTranslated = TranslateMaToDatabase(ma, maDataExporter, out translationErrors);
            if (!isSuccessfullyTranslated && maDataExporter.ValidationExceptions.Count > 0 && maDataExporter.SaveExceptions.Count == 0 && maDataExporter.Exceptions.Count == 0)
            {
                LogEvent("MA translation to DB failed.", ma.ready_id, Ma_service_log_PK.EventType.MADataTranslationFailed);

                UpdateMATo_MAValidationFailed(ma, translationErrors);
                return;
            }
            else if (maDataExporter.SaveExceptions.Count > 0 || maDataExporter.Exceptions.Count > 0)
            {
                var exceptionBuilder = new StringBuilder();

                foreach (var saveException in maDataExporter.SaveExceptions)
                {
                    exceptionBuilder.Append(string.Format("Save exception: {0} | StackTrace: {1}", saveException.Message, saveException.StackTrace));
                }

                foreach (var exception in maDataExporter.Exceptions)
                {
                    exceptionBuilder.Append(string.Format("Exception: {0} | StackTrace: {1}", exception.Message, exception.StackTrace));
                }

                LogEvent("MA translation process failed. " + exceptionBuilder, ma.ready_id, Ma_service_log_PK.EventType.Error);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAProcessingDataFailed;
                SaveMA(ma);
                return;
            }

            LogEvent("MA translation to DB successful.", ma.ready_id, Ma_service_log_PK.EventType.MADataTranslationSuccessful);

            //MA xEVPRM creation
            Exception XevprmCreationErrors;
            bool isXevprmSuccessfullyCreated = CreateMAXEVPRM(ma, maDataExporter, out XevprmCreationErrors);
            if (!isXevprmSuccessfullyCreated)
            {
                LogError(XevprmCreationErrors, "MA xEVRPM creation process failed.", ma.ready_id, Ma_service_log_PK.EventType.Error);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAProcessingDataFailed;
                SaveMA(ma);
                return;
            }

            LogEvent("Creating xEVPRM successful.", ma.ready_id, Ma_service_log_PK.EventType.MAXevprmCreationSuccessfull);

            //xEVPRM validation
            List<EVMessage.MarketingAuthorisation.ValidationException> xEVPRMValidationErrors;
            bool isXEVPRMSuccessfullyValidated = ValidateMAXEVPRM(ma, out xEVPRMValidationErrors);
            if (!isXEVPRMSuccessfullyValidated && xEVPRMValidationErrors != null && xEVPRMValidationErrors.Count > 0)
            {
                LogEvent("xEVPRM valdiaton failed.", ma.ready_id, Ma_service_log_PK.EventType.MAXevprmValidationFailed);

                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmValidationFailed;
                SaveMA(ma);

                if (!UpdateMAxEVPRMStatus(ma, XevprmStatus.ValidationFailed))
                {
                    LogEvent("Failed to update MAXEVPRM, new status should be Validation failed", ma.ready_id, Ma_service_log_PK.EventType.FailedToUpdateXEVPRMStatus);
                }

                UpdateMATo_MAValidationFailed(ma, xEVPRMValidationErrors);
                return;
            }
            else if (!isXEVPRMSuccessfullyValidated)
            {
                LogEvent("MA xEVPRM valdiaton process failed.", ma.ready_id, Ma_service_log_PK.EventType.Error);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAProcessingDataFailed;
                SaveMA(ma);

                if (!UpdateMAxEVPRMStatus(ma, XevprmStatus.ValidationFailed))
                {
                    LogEvent("Failed to update MAXEVPRM, new status should be Validation failed", ma.ready_id, Ma_service_log_PK.EventType.FailedToUpdateXEVPRMStatus);
                }

                return;
            }

            ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmValidationSuccessful;
            SaveMA(ma);

            LogEvent("xEVPRM validated successfully.", ma.ready_id, Ma_service_log_PK.EventType.MAXevprmValidationSuccessful);


            UpdateMATo_MAValidationSuccessful(ma);
        }

        /// <summary>
        /// Method generates and sends MAValidationSuccessful report, and updates MA to MAValidationSuccessful on sucess.
        /// If report is usccessfully sent, methods update related xEVPRM to status: XevprmStatus.InProgress, which
        /// signals that message should be sent.
        /// </summary>
        /// <param name="ma"></param>
        private static void UpdateMATo_MAValidationSuccessful(Marketing_authorisation_PK ma)
        {

            //Generate report
            var statusReport = new EVMessage.StatusReport.StatusReportMessage();
            Ma_message_header_PK maHeader = null;
            try
            {
                maHeader = _maMessageHeaderOperations.GetEntityByReadyId(ma.ready_id);
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to get MA header from DB", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationReportSendingFailed;
                SaveMA(ma);
            }
            if (maHeader == null)
            {

                LogError(null, "Failed to get MA header from DB (Retrieved header is null)", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationReportSendingFailed;
                SaveMA(ma);
                return;
            }

            try
            {
                statusReport.CreateStatusReportFor_MAValidationSuccessfull(maHeader, ma.ready_id);
            }
            catch (Exception e)
            {
                LogError(e, "Failed to create MAValidationSuccessfull report.", ma.ready_id, Ma_service_log_PK.EventType.MAValidationCreateReportFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationReportSendingFailed;
                SaveMA(ma);
                return;
            }
            if (!statusReport.IsReportValid)
            {
                LogError(null, "Failed to create valid MAValidationSuccessfull report.", ma.ready_id, Ma_service_log_PK.EventType.MAValidationCreateReportFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationReportSendingFailed;
                SaveMA(ma);
                return;
            }

            if (SaveReportToDatabase(ma, statusReport, Ma_file_PK.FileType.MAStatusReport_ValidationSuccessful))
            {
                LogEvent("MAValidationSuccessfull report file is successfully saved to DB.", ma.ready_id, Ma_service_log_PK.EventType.MAValidationSaveReportToDBSuccessful);
                if (SendReport(ma, statusReport))
                {
                    LogEvent("MAValidationSuccessfull report file is sucessfully saved to outbound folder.", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedSaveReportToOutboundFolderSuccessful);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationSuccessful;
                    SaveMA(ma);
                    if (!UpdateMAxEVPRMStatus(ma, XevprmStatus.ReadyToSubmit))
                    {
                        LogEvent("Failed to update MAXEVPRM, new status should be Ready to submit", ma.ready_id, Ma_service_log_PK.EventType.FailedToUpdateXEVPRMStatus);
                    }
                    try
                    {
                        Xevprm_message_PK message = _xevprmMessageOperations.GetEntityByMA(ma.marketing_authorisation_PK);
                        message.gateway_submission_date = DateTime.Now;
                        _xevprmMessageOperations.Save(message);
                    }
                    catch (Exception e)
                    {
                        LogError(e, "Failed to set gateway submission date.", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                    }
                    return;
                }
                else
                {
                    LogEvent("Error saving MAValidationSuccessfull report to file.", ma.ready_id, Ma_service_log_PK.EventType.MAValidationSaveReportToOutboundFolderFailed);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationReportSendingFailed;
                    SaveMA(ma);
                    return;
                }
            }
            else
            {
                LogEvent("Error saving MAValidationSuccessfull report to database", ma.ready_id, Ma_service_log_PK.EventType.MAReceivedSaveReportToDBFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationReportSendingFailed;
                SaveMA(ma);
            }
        }

        private static void UpdateMATo_MAValidationFailed(Marketing_authorisation_PK ma, List<EVMessage.MarketingAuthorisation.ValidationException> errors)
        {


            var statusReport = new EVMessage.StatusReport.StatusReportMessage();
            Ma_message_header_PK maHeader = null;
            try
            {
                maHeader = _maMessageHeaderOperations.GetEntityByReadyId(ma.ready_id);
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to get MA header from DB", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationReportSendingFailed;
                SaveMA(ma);
                return;
            }
            if (maHeader == null)
            {

                LogError(null, "Failed to get MA header from DB (Retrieved header is null)", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationReportSendingFailed;
                SaveMA(ma);
                return;
            }

            try
            {
                statusReport.CreateStatusReportFor_MAValidationFailed(maHeader, errors, ma.ready_id);
            }
            catch (Exception e)
            {
                LogError(e, "Failed to create MAValidationFailed report.", ma.ready_id, Ma_service_log_PK.EventType.MAValidationCreateReportFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationReportSendingFailed;
                SaveMA(ma);
                return;
            }
            if (!statusReport.IsReportValid)
            {
                LogError(null, "Failed to create valid MAValidationFailed report.", ma.ready_id, Ma_service_log_PK.EventType.MAValidationCreateReportFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationReportSendingFailed;
                SaveMA(ma);
                return;
            }

            if (SaveReportToDatabase(ma, statusReport, Ma_file_PK.FileType.MAStatusReport_ValidationFailed))
            {
                LogEvent("MAValidationFailed report file is successfully saved to DB.", ma.ready_id, Ma_service_log_PK.EventType.MAValidationSaveReportToDBSuccessful);
                LogEvent("Report saved to DB.", ma.ready_id, Ma_service_log_PK.EventType.MAValidationSaveReportToDBSuccessful);
                if (SendReport(ma, statusReport))
                {
                    LogEvent("MAValidationFailed report file is sucessfully saved to outbound folder.", ma.ready_id, Ma_service_log_PK.EventType.MAValidationSaveReportToOutboundFolderSuccessful);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationFailed;
                    _marketingAuthorisationOperations.Save(ma);
                }
                else
                {
                    LogEvent("Error saving MAValidationFailed report to file.", ma.ready_id, Ma_service_log_PK.EventType.MAValidationSaveReportToOutboundFolderFailed);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationReportSendingFailed;
                    _marketingAuthorisationOperations.Save(ma);
                }
            }
            else
            {
                LogEvent("Error saving MAValidationFailed report to database", ma.ready_id, Ma_service_log_PK.EventType.MAValidationSaveReportToDBFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAValidationReportSendingFailed;
                _marketingAuthorisationOperations.Save(ma);
            }

        }

        /// <summary>
        /// Method exports MA to Ready! DB.
        /// If process is successful, method will return true. Otherwise, methods returns false.
        /// If returned value is false, and validationExceptions list is empty, translation process 
        /// is failed (program error), otherwise MA contains validation errors.
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="validationExceptions"></param>
        /// <returns></returns>
        private static bool TranslateMaToDatabase(Marketing_authorisation_PK ma, EVMessage.MarketingAuthorisation.MADataExporter maDataExporter, out List<EVMessage.MarketingAuthorisation.ValidationException> validationExceptions)
        {
            validationExceptions = new List<ValidationException>();
            Ma_file_PK messageFile = null;
            try
            {
                messageFile = _maFileOperations.GetEntityByReadyIdAndType(ma.ready_id, Ma_file_PK.FileType.MAFile);
            }
            catch (Exception e)
            {
                LogError(e, "Failed to retrieve message data file from DB", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                return false;
            }

            if (messageFile == null)
            {
                return false;
            }

            String messageXML = Encoding.UTF8.GetString(messageFile.file_data);
            messageXML = messageXML.Substring(messageXML.IndexOf("<marketingauthorisation"));
            EVMessage.MarketingAuthorisation.Schema.XRoot xroot = EVMessage.MarketingAuthorisation.Schema.XRoot.Parse(messageXML);
     
            maDataExporter.EMASenderID = ConfigurationManager.AppSettings["EMASenderID"];
            bool isSuccessful = maDataExporter.ExportMAToDB(xroot.marketingauthorisation, ma.marketing_authorisation_PK);
            if (isSuccessful)
            {
                return true;
            }
            else
            {
                if (maDataExporter.ValidationExceptions.Count > 0)
                {
                    foreach (EVMessage.MarketingAuthorisation.ValidationException item in maDataExporter.ValidationExceptions)
                    {
                        string message = string.Format("(Type: {0}) {1}.{2} '{3}' : {4}", item.ValidationExceptionType.ToString(), item.Location, item.PropertyName, item.PropertyValue, item.Message);
                        validationExceptions.Add(new ValidationException(message, item.ValidationExceptionType, item.Severity));
                    }
                }
                return false;
            }
        }

        private static bool CreateMAXEVPRM(Marketing_authorisation_PK ma, EVMessage.MarketingAuthorisation.MADataExporter maDataExporter, out Exception ex)
        {

            ex = null;
            var createdXevprmMessageList = new List<Xevprm_message_PK>(maDataExporter.ExportedAuthorisedProducts.Count);

            foreach (var exportedAuthorisedProduct in maDataExporter.ExportedAuthorisedProducts)
            {
                string messageNumber = EVMessage.Xevprm.Xevprm.GenerateMessageNumber(ma.ready_id, 7);
                var createResult = EVMessage.Xevprm.Xevprm.CreateNewMessage(exportedAuthorisedProduct.Item1, XevprmEntityType.AuthorisedProduct, exportedAuthorisedProduct.Item2, 3, messageNumber);
                if (createResult.IsSuccess && createResult.Result != null)
                {
                    createdXevprmMessageList.Add(createResult.Result);
                }
                else
                {
                    ex = createResult.Exception;
                }
            }
            if (createdXevprmMessageList.Count == 1)
            {

                var relation = new Ma_ma_entity_mn_PK()
                {
                    ma_entity_FK = createdXevprmMessageList[0].xevprm_message_PK,
                    ma_entity_type_FK = (int)MAEntityType.XevprmMessage,
                    ma_FK = ma.marketing_authorisation_PK,
                };
                _maMaEntityMnOperations.Save(relation);
                return true;
            }
            else
            {
                return false;
            }

        }

        private static bool ValidateMAXEVPRM(Marketing_authorisation_PK ma, out List<EVMessage.MarketingAuthorisation.ValidationException> validationExceptions)
        {
            Xevprm_message_PK xevprmMessage = _xevprmMessageOperations.GetEntityByMA(ma.marketing_authorisation_PK);

            var xevprmXml = new EVMessage.Xevprm.XevprmXml();
            var constructResult = xevprmXml.ConstructXevprm(xevprmMessage);

            if (constructResult.IsSuccess)
            {
                xevprmMessage.xml = xevprmXml.Xml;
                try
                {
                    xevprmMessage.xml_hash = EVMessage.Xevprm.XevprmHelper.ComputeHash(xevprmMessage.xml);
                }
                catch (Exception e)
                {
                    LogError(e, "Failed to compute xml hash, XEVPRM_MESSAGE_PK: " + xevprmMessage.xevprm_message_PK, ma.ready_id, Ma_service_log_PK.EventType.Error);

                }
                xevprmMessage = _xevprmMessageOperations.Save(xevprmMessage);

                EVMessage.Xevprm.Xevprm.UpdateXevprmEntityDetailsTables(xevprmMessage);
                validationExceptions = new List<ValidationException>();
                return true;
            }
            else
            {
                if (xevprmXml.HasEvprmValidationExceptions)
                {
                    validationExceptions = new List<ValidationException>();

                    foreach (EVMessage.Xevprm.XevprmValidationException item in xevprmXml.EvprmValidationExceptions)
                    {
                        string evprmMessage = string.Format("{0}.{1} '{2}' : {3}", item.EvprmLocation, item.EvprmPropertyName, item.EvprmPropertyValue, item.EvprmMessage);
                        EVMessage.MarketingAuthorisation.SeverityType exType = SeverityType.NULL;
                        if (item.Severity == EVMessage.Xevprm.SeverityType.Error) exType = SeverityType.Error;
                        if (item.Severity == EVMessage.Xevprm.SeverityType.Warning) exType = SeverityType.Warning;

                        validationExceptions.Add(new ValidationException(evprmMessage, ValidationExceptionType.InvalidValue, exType));
                    }
                }
                else
                {
                    foreach (Exception e in xevprmXml.Exceptions)
                    {
                        LogError(e, "xEVPRM validation exception!", ma.ready_id, Ma_service_log_PK.EventType.Error);
                    }
                    foreach (EVMessage.Xevprm.XevprmValidationException e in xevprmXml.ReadyValidationExceptions)
                    {
                        LogError(null, "Ready! validation exception!: " + e.ReadyMessage, ma.ready_id, Ma_service_log_PK.EventType.Error);
                    }
                    validationExceptions = new List<ValidationException>();
                }
                return false;
            }
        }

        #endregion

        #region PROCESS SUCCESSFULLY VALIDATED MAs
        /// <summary>
        /// Method process new MA with status = Marketing_authorisation_PK.MAStatus.MAValidationSuccessful
        /// xEVPRM messages for MA with this status should be sent, and this method have to check if 
        /// xEVPRM is already sent.
        /// xEVPRM is sent if it is in status XevprmStatus.InProgress_Transferred which means that xEVPRM is sent, and ACK 
        /// is not already received. 
        /// Statuses: XevprmStatus.AuthorizedWIthErrors, XevprmStatus.AuthorizationFailed and message.status == XevprmStatus.Authorized 
        /// means that ACK is already recieved, whici implies xEVPRM is sent before, and MA should be processed.
        /// If xEVPRM is sent, an MAxEVRPMSent report is genrated and
        /// MA gets status MAXEVPRMSent.
        /// If report generation/sending process fails MA gets status MAXevprmProcessingSendingFailed.
        /// </summary>
        private static void ProcessSuccessfullyValidatedMAs()
        {
            List<Marketing_authorisation_PK> validatedMAs = null;

            try
            {
                validatedMAs = _marketingAuthorisationOperations.GetEntitiesByStatus(Marketing_authorisation_PK.MAStatus.MAValidationSuccessful);
            }
            catch (Exception e)
            {
                LogError(e, "Failed to retrieve validated MAs from DB", Ma_service_log_PK.EventType.DBError);
                return;
            }

            foreach (Marketing_authorisation_PK ma in validatedMAs)
            {
                Xevprm_message_PK message = null;
                try
                {
                    message = _xevprmMessageOperations.GetEntityByMA(ma.marketing_authorisation_PK);
                }
                catch (Exception e)
                {
                    LogEvent("Error occurred while retrieving xEVPRM from DB", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                    continue;
                }
                if (message == null)
                {
                    LogEvent("xEVPRM for MA is missing.", ma.ready_id, Ma_service_log_PK.EventType.xEVPRMMissing);
                    continue;
                }

                if (message.XevprmStatus.In(XevprmStatus.MDNReceivedSuccessful, //Message is transfered
                    XevprmStatus.ACKReceived, //ACK is already received, this implies message is transfered
                    XevprmStatus.ACKDeliveryFailed, //ACK is already received, this implies message is transfered
                    XevprmStatus.ACKDelivered) //ACK is already received, this implies message is transfered
                    )
                {
                    try
                    {
                        UpdateMATo_MAXevprmSent(ma, message);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, "Unexpected exception occured while updating MA to xEVPRM sent.", ma.ready_id, Ma_service_log_PK.EventType.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Method retrieves MA XEVPRM and xEVPRM to database, and to MA outbound folder. If this succeeds
        /// method generates and sends MAXevprmSent report. If process is successfull, MA will
        /// be updated to MASentToEMA status.
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="message"></param>
        private static void UpdateMATo_MAXevprmSent(Marketing_authorisation_PK ma, Xevprm_message_PK message)
        {
            //Generate report
            var statusReport = new EVMessage.StatusReport.StatusReportMessage();
            Ma_message_header_PK maHeader = null;
            try
            {
                maHeader = _maMessageHeaderOperations.GetEntityByReadyId(ma.ready_id);
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to get MA header from DB", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmProcessingSendingFailed;
                SaveMA(ma);
                return;
            }
            if (maHeader == null)
            {
                LogError(null, "Failed to get MA header from DB (Retrieved header is null)", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmProcessingSendingFailed;
                SaveMA(ma);
                return;
            }

            if (message.xml == null)
            {
                LogEvent("Message does not contain xEVPRM xml", ma.ready_id, Ma_service_log_PK.EventType.xEVPRMMissing);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmProcessingSendingFailed;
                SaveMA(ma);
                return;
            }

            String messageFileName = maHeader.registrationnumber + "_" + maHeader.registrationid + "_" + ma.ready_id + "_EMA_xEVPRM.xml";
            String filePath = Path.Combine(MARootPath, OutboundFolder, "Attachments", ma.message_folder);

            DirectoryInfo xEvprmFolder = CreateDirectoryInfo(filePath);
            if (xEvprmFolder != null && !xEvprmFolder.Exists)
            {
                try
                {
                    Directory.CreateDirectory(xEvprmFolder.FullName);
                    xEvprmFolder = new DirectoryInfo(xEvprmFolder.FullName);
                }
                catch (Exception ex)
                {
                    LogError(ex, "Failed to create folder: " + xEvprmFolder.FullName, ma.ready_id, Ma_service_log_PK.EventType.DiskError);
                }
            }

            if (xEvprmFolder == null || !xEvprmFolder.Exists)
            {
                LogError(null, "Folder for storing xEVPRM does not exists!", ma.ready_id, Ma_service_log_PK.EventType.DiskError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmProcessingSendingFailed;
                SaveMA(ma);
                return;
            }

            try
            {
                var xEVPRMFile = new Ma_file_PK()
                {
                    file_data = Encoding.UTF8.GetBytes(message.xml),
                    file_type_FK = (int)Ma_file_PK.FileType.MAxEVPRM,
                    file_name = messageFileName,
                    file_path = filePath,
                    ready_id_FK = ma.ready_id
                };

                _maFileOperations.Save(xEVPRMFile);
                LogEvent("XEVPRM saved to DB.", ma.ready_id, Ma_service_log_PK.EventType.MAXEvprmSaveToDBSuccessful);
            }
            catch (Exception e)
            {
                LogError(e, "xEVPRM save to DB failed.", ma.ready_id, Ma_service_log_PK.EventType.MAXevprmSaveToDBFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmProcessingSendingFailed;
                SaveMA(ma);
                return;
            }

            String path = Path.Combine(filePath, messageFileName);
            try
            {
                File.WriteAllText(path, message.xml);
                LogEvent("xEVPRM saved to oubound folder.", ma.ready_id, Ma_service_log_PK.EventType.MAXEvprmSaveToDiskSuccessful);
            }
            catch (Exception e)
            {
                LogError(e, "Error occurred while saving xEVPRM xml to disk, at location: " + path, ma.ready_id, Ma_service_log_PK.EventType.MAXevprmSaveToDiskFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmProcessingSendingFailed;
                SaveMA(ma);
                return;
            }

            try
            {
                statusReport.CreateStatusReportFor_MASentToEMA(maHeader, ma.ready_id, Path.Combine("Attachments", ma.message_folder, messageFileName));
            }
            catch (Exception e)
            {
                LogError(e, "Failed to create MASentToEMA report.", ma.ready_id, Ma_service_log_PK.EventType.MAXevprmSentCreateReportFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmProcessingSendingFailed;
                SaveMA(ma);
                return;
            }
            if (!statusReport.IsReportValid)
            {
                LogError(null, "Failed to create valid MASentToEMA report.", ma.ready_id, Ma_service_log_PK.EventType.MAXevprmSentCreateReportFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmProcessingSendingFailed;
                SaveMA(ma);
                return;
            }

            if (SaveReportToDatabase(ma, statusReport, Ma_file_PK.FileType.MAStatusReport_SentToEMA))
            {
                LogEvent("Saving SentToEMA report to DB successful.", ma.ready_id, Ma_service_log_PK.EventType.MAXevprmSentSaveReportToDBSuccessful);
                if (SendReport(ma, statusReport))
                {
                    LogEvent("SentToEMA report file is sucessfully saved to outbound folder.", ma.ready_id, Ma_service_log_PK.EventType.MAXevprmSentSaveReportToOutboundFolderSuccessful);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmSent;
                    SaveMA(ma);
                }
                else
                {
                    LogEvent("Error saving SentToEMA report to file.", ma.ready_id, Ma_service_log_PK.EventType.MAXevprmSentSaveReportToOutboundFolderFailed);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmProcessingSendingFailed;
                    SaveMA(ma);
                    return;
                }
            }
            else
            {
                LogEvent("Error saving SentToEMA report to database", ma.ready_id, Ma_service_log_PK.EventType.MAXevprmSentSaveReportToDBFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.MAXevprmProcessingSendingFailed;
                SaveMA(ma);
            }
        }
        #endregion

        #region PROCESS xEVPRM SENT MAs
        private static void ProcessXEVPRMSentMAs()
        {
            List<Marketing_authorisation_PK> sentMAs = null;
            try
            {
                sentMAs = _marketingAuthorisationOperations.GetEntitiesByStatus(Marketing_authorisation_PK.MAStatus.MAXevprmSent);
            }
            catch (Exception e)
            {
                LogError(e, "Retrieving sent MAs from database failed.", Ma_service_log_PK.EventType.DBError);
                return;
            }
            foreach (Marketing_authorisation_PK ma in sentMAs)
            {

                Xevprm_message_PK message = null;
                try
                {
                    message = _xevprmMessageOperations.GetEntityByMA(ma.marketing_authorisation_PK);
                }
                catch (Exception e)
                {
                    LogEvent("Error occurred while retrieving xEVPRM from DB", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                    continue;
                }
                if (message == null)
                {
                    LogEvent("xEVPRM for MA is missing.", ma.ready_id, Ma_service_log_PK.EventType.xEVPRMMissing);
                    continue;
                }
                if (message.XevprmStatus.In(XevprmStatus.ACKDelivered))
                {
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceived;
                    if (!SaveMA(ma))
                    {
                        LogEvent("Failed to update MA", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                        continue;
                    }
                    try
                    {
                        UpdateMATo_ACKReceivedProcessedSuccessfully(ma, message);
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, "Unexpected exception occured while updating MA to ACKReceivedProcessingSuccessfully.", ma.ready_id, Ma_service_log_PK.EventType.Error);
                    }
                }
            }
        }
        private static void UpdateMATo_ACKReceivedProcessedSuccessfully(Marketing_authorisation_PK ma, Xevprm_message_PK message)
        {

            if (message.ack == null)
            {
                LogEvent("Message does not contain ACK xml", ma.ready_id, Ma_service_log_PK.EventType.ACKMissing);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceivedProcessingFailed;
                SaveMA(ma);
                return;
            }

            var emaACK = new xEVMPD.ACKMessage();
            try
            {
                emaACK.From(message.ack);
            }
            catch (Exception e)
            {
                LogError(e, "Message does not contain valid ACK xml.", ma.ready_id, Ma_service_log_PK.EventType.ACKMissing);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceivedProcessingFailed;
                SaveMA(ma);
                return;
            }

            Ma_message_header_PK maHeader = null;
            try
            {
                maHeader = _maMessageHeaderOperations.GetEntityByReadyId(ma.ready_id);
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to get MA header from DB", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceivedProcessingFailed;
                SaveMA(ma);
            }

            if (maHeader == null)
            {

                LogError(null, "Failed to get MA header from DB (Retrieved header is null)", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceivedProcessingFailed;
                SaveMA(ma);
                return;
            }

            String messageFileName = maHeader.registrationnumber + "_" + maHeader.registrationid + "_" + ma.ready_id + "_EMA_ACK.xml";
            String filePath = Path.Combine(MARootPath, OutboundFolder, "Attachments", ma.message_folder);
            DirectoryInfo ackFolder = CreateDirectoryInfo(filePath);
            if (ackFolder != null && !ackFolder.Exists)
            {
                try
                {
                    Directory.CreateDirectory(ackFolder.FullName);
                    ackFolder = new DirectoryInfo(ackFolder.FullName);
                }
                catch (Exception ex)
                {
                    LogError(ex, "Failed to create folder: " + ackFolder.FullName, ma.ready_id, Ma_service_log_PK.EventType.DiskError);
                }
            }

            if (ackFolder == null || !ackFolder.Exists)
            {
                LogError(null, "Folder for storing ACK does not exists!", ma.ready_id, Ma_service_log_PK.EventType.DiskError);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceivedProcessingFailed;
                SaveMA(ma);
                return;
            }

            try
            {

                var ackFile = new Ma_file_PK()
                {
                    file_data = Encoding.UTF8.GetBytes(message.ack),
                    file_type_FK = (int)Ma_file_PK.FileType.MAAck,
                    file_name = messageFileName,
                    file_path = filePath,
                    ready_id_FK = ma.ready_id
                };

                _maFileOperations.Save(ackFile);
                LogEvent("ACK saved to DB", ma.ready_id, Ma_service_log_PK.EventType.MAACKSaveToDBSuccessful);
            }
            catch (Exception e)
            {
                LogError(e, "Saving ACK to DB failed.", ma.ready_id, Ma_service_log_PK.EventType.MAACKSaveToDBFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceivedProcessingFailed;
                SaveMA(ma);
                return;
            }

            String path = Path.Combine(filePath, messageFileName);
            try
            {
                File.WriteAllText(path, message.ack.Replace("UTF-8", "UTF-16").Replace("utf-8", "UTF-16"));
                LogEvent("ACK sucessfully created, at location: " + path, ma.ready_id, Ma_service_log_PK.EventType.MAACKSaveToDiskSuccessful);
            }
            catch (Exception e)
            {
                LogError(e, "Error occurred while saving ACK xml to disk, at location: " + path, ma.ready_id, Ma_service_log_PK.EventType.MAACKSaveToDiskFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceivedProcessingFailed;
                SaveMA(ma);
                return;
            }


            //Generate report
            var statusReport = new EVMessage.StatusReport.StatusReportMessage();
            try
            {
                statusReport.CreateStatusReportFor_ACKReceivedFromEMA(maHeader, emaACK, ma.ready_id, Path.Combine("Attachments", ma.message_folder, messageFileName));
            }
            catch (Exception e)
            {
                LogError(e, "Failed to create ACKReceivedFromEMA report.", ma.ready_id, Ma_service_log_PK.EventType.MAACKCreateReportFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceivedProcessingFailed;
                SaveMA(ma);
                return;
            }
            if (!statusReport.IsReportValid)
            {
                LogError(null, "Failed to create valid ACKReceivedFromEMA report.", ma.ready_id, Ma_service_log_PK.EventType.MAACKCreateReportFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceivedProcessingFailed;
                SaveMA(ma);
                return;
            }

            if (SaveReportToDatabase(ma, statusReport, Ma_file_PK.FileType.MAStatusReport_ACKReceivedFromEMA))
            {
                LogEvent("Saving ACKReceivedFromEMA report to DB successful.", ma.ready_id, Ma_service_log_PK.EventType.MAACKSaveReportToDBSuccessful);
                if (SendReport(ma, statusReport))
                {
                    LogEvent("ACKReceivedFromEMA report file is sucessfully saved to outbound folder.", ma.ready_id, Ma_service_log_PK.EventType.MAACKSaveReportToOutboundFolderSuccessful);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceivedProcessedSuccessfully;
                    SaveMA(ma);

                }
                else
                {
                    LogEvent("Error saving ACKReceivedFromEMA report to file.", ma.ready_id, Ma_service_log_PK.EventType.MAACKSaveReportToOutboundFolderFailed);
                    ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceivedProcessingFailed;
                    SaveMA(ma);
                    return;
                }
            }
            else
            {
                LogEvent("Error saving ACKReceivedFromEMA report to database", ma.ready_id, Ma_service_log_PK.EventType.MAACKSaveReportToDBFailed);
                ma.ma_status_FK = (int)Marketing_authorisation_PK.MAStatus.ACKReceivedProcessingFailed;
                SaveMA(ma);
            }
        }
        #endregion

        #region REPORT SENDING
        /// <summary>
        /// Metohod trys to save report to DB.
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="report"></param>
        /// <param name="fileType"></param>
        /// <returns>True if cuseeded, false otherweise</returns>
        private static bool SaveReportToDatabase(Marketing_authorisation_PK ma, EVMessage.StatusReport.StatusReportMessage report, Ma_file_PK.FileType fileType)
        {
            var reportFile = new Ma_file_PK()
            {
                file_data = Encoding.UTF8.GetBytes(report.StatusReportXML),
                file_name = report.StatusReport.report.reportacknowledgment.reportname,
                file_type_FK = (int)fileType,
                ready_id_FK = ma.ready_id,
                file_path = Path.Combine(MARootPath, OutboundFolder)
            };

            try
            {
                _maFileOperations.Save(reportFile);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Method sends report, ie. saves report to MA outbound folder.
        /// </summary>
        /// <param name="ma"></param>
        /// <param name="report"></param>
        /// <returns>True if suceeded, false otherweise.</returns>
        private static bool SendReport(Marketing_authorisation_PK ma, EVMessage.StatusReport.StatusReportMessage report)
        {

            DirectoryInfo outboundFolder = CreateDirectoryInfo(Path.Combine(MARootPath, OutboundFolder));
            if (outboundFolder != null && outboundFolder.Exists)
            {
                try
                {
                    File.WriteAllText(Path.Combine(outboundFolder.FullName, report.StatusReport.report.reportacknowledgment.reportname), report.StatusReportXML, Encoding.UTF8);
                    return true;
                }
                catch (Exception ex)
                {

                    LogError(ex, "Exception occurred while sending report to folder: " + outboundFolder.FullName, ma.ready_id);
                    return false;
                }
            }
            else
            {
                LogError(null, "Target path (" + outboundFolder.FullName + ") for sending report:" + report.StatusReport.report.reportacknowledgment.reportname + " does not exists!", ma.ready_id);
                return false;
            }

        }

        #endregion

        #region ARCHIVE

        private static List<int> arcivationHours;
        public static List<int> ArcivationHours
        {
            get { return arcivationHours; }
            set { arcivationHours = value; }
        }

        public static void StartArchivation()
        {
            bool performArchivation = false;

            //If archivationHours is null, archivation mode is interval, and shoud be processes
            if (arcivationHours == null)
            {
                performArchivation = true;
            }
            else if (arcivationHours.Contains(DateTime.Now.Hour) && lastArchivingTime.Hour != DateTime.Now.Hour)
            {
                //if archivation mode is hourly, shoud be processed if current hour is in archivationHours, and this hour
                //is not yet processed
                performArchivation = true;
            }

            if (performArchivation)
            {
                try
                {
                    LogEvent("Outbound archivation started.", Ma_service_log_PK.EventType.Message);
                    ArchiveOutboundFolder();
                    LogEvent("Outbound archivation finished.", Ma_service_log_PK.EventType.Message);
                }
                catch (Exception ex)
                {
                    LogError(ex, "Unexpected error occured while archiving outbound folder.", Ma_service_log_PK.EventType.Error);
                }
            }


        }

        private static void ArchiveOutboundFolder()
        {

            DirectoryInfo archiveDirectory = CreateDirectoryInfo(Path.Combine(MARootPath, OutboundFolder, "Archive"));
            if (archiveDirectory == null)
            {
                LogError(null, "Failed to create DirectoryInfo for outbound archive folder, path: " + archiveDirectory.FullName + " does not exists!", Ma_service_log_PK.EventType.DiskError);
                return;
            }

            if (!archiveDirectory.Exists)
            {
                try
                {
                    Directory.CreateDirectory(archiveDirectory.FullName);
                    archiveDirectory = new DirectoryInfo(archiveDirectory.FullName);
                }
                catch (Exception ex)
                {
                    LogError(ex, "Failed to create outbond archive directory: " + archiveDirectory.FullName, Ma_service_log_PK.EventType.DiskError);
                    return;
                }
            }

            foreach (FileInfo fi in archiveDirectory.GetFiles("*.xml"))
            {
                if (IsFileReadyForProcessing(fi.FullName))
                {
                    Regex readyId = new Regex("<readymessageid>(.*)</readymessageid>");
                    Match m = null;
                    try
                    {
                        m = readyId.Match(File.ReadAllText(fi.FullName, Encoding.UTF8));
                    }
                    catch (Exception ex)
                    {
                        LogError(ex, "Failed to match readyId for file: " + fi.FullName, Ma_service_log_PK.EventType.Error);
                        continue;
                    }
                    String targetPath = null;
                    if (m != null && m.Success && m.Groups.Count >= 2)
                    {
                        Marketing_authorisation_PK ma = null;
                        try
                        {
                            ma = _marketingAuthorisationOperations.GetEntityByReadyId(m.Groups[1].Value);
                        }
                        catch (Exception ex)
                        {
                            LogError(ex, "Failed to retrieve MA from database", Ma_service_log_PK.EventType.DBError);
                            continue;
                        }
                        if (ma == null)
                        {
                            LogError(null, "Cannot archive file:" + fi.FullName + ". Failed to get MA for readyId: " + m.Groups[1].Value + ".", Ma_service_log_PK.EventType.Error);
                            continue;
                        }

                        if (ma.message_folder != null)
                        {
                            targetPath = Path.Combine(MARootPath, OutboundFolder, "Archive", ma.message_folder);
                        }
                        else
                        {
                            targetPath = Path.Combine(MARootPath, OutboundFolder, "Archive", "Junk");
                        }
                    }
                    else
                    {
                        LogError(null, "Cannot archive file:" + fi.FullName + ". Failed to parse readyId", Ma_service_log_PK.EventType.Error);
                        continue;
                    }
                    if (targetPath != null)
                    {
                        DirectoryInfo di = CreateDirectoryInfo(targetPath);
                        if (di != null && !di.Exists)
                        {
                            try
                            {
                                Directory.CreateDirectory(targetPath);
                                di = new DirectoryInfo(targetPath);
                            }
                            catch (Exception ex)
                            {
                                LogError(ex, "Cannot archive file: " + fi.FullName + ". Failed to create directory: " + di.FullName, Ma_service_log_PK.EventType.Error);
                                di = null;
                            }
                        }
                        if (di != null)
                        {
                            try
                            {
                                File.Move(fi.FullName, Path.Combine(di.FullName, fi.Name));
                                List<Ma_file_PK> files = _maFileOperations.GetEntitiesByFileName(fi.Name);
                                if (files.Count == 0 || files.Count > 1)
                                {
                                    LogError(null, "Failed to retrieve file: " + fi.FullName + " from DB." + (files.Count == 0 ? " File does not exits." : " Retrieved " + files.Count + " report files with same name."));
                                }
                                else
                                {
                                    files[0].file_path = Path.GetDirectoryName(fi.FullName);
                                    _maFileOperations.Save(files[0]);
                                }
                                LogEvent("File: " + fi.Name + " successfully archived to: " + fi.FullName, files[0].ready_id_FK, Ma_service_log_PK.EventType.Message);
                            }
                            catch (Exception ex)
                            {
                                LogError(ex, "Cannot archive file: " + fi.FullName + ". Exception occured while moving file", Ma_service_log_PK.EventType.Error);
                            }
                        }
                        else
                        {
                            LogError(null, "Cannot archive file: " + fi.FullName + ". Directory: " + di.FullName + " does not exists.", Ma_service_log_PK.EventType.Error);
                        }
                    }
                }
            }
            lastArchivingTime = DateTime.Now;
        }

        #endregion

        #region SPC

        /// <summary>
        /// Method searches for new SPCs in predefined folder.
        /// An new SPC is stored to DB if new file appears in folder, or is newer than corresponding 
        /// file in DB.
        /// </summary>
        private static void ProcessNewSPCs()
        {

            Dictionary<String, DateTime> existingSPCs = null;
            try
            {
                existingSPCs = _maAttachmentOperations.GetNameAndDateForEntities();
            }
            catch (Exception dbException)
            {
                LogError(dbException, "Cannot load existing SMPCs from database", null);
                return;
            }

            DirectoryInfo SPCDirectory = CreateDirectoryInfo(Path.Combine(MARootPath, SMPCInboundFolder));
            if (SPCDirectory != null && SPCDirectory.Exists)
            {
                FileInfo[] files = SPCDirectory.GetFiles();
                Dictionary<FileInfo, int?> failedFiles = new Dictionary<FileInfo, int?>();

                foreach (FileInfo fi in files)
                {
                    if (!existingSPCs.ContainsKey(fi.Name))
                    {
                        if (IsFileReadyForProcessing(fi.FullName))
                        {
                            if (!SaveSPCFile(fi, null)) failedFiles.Add(fi, null);
                        }
                        continue;
                    }
                    else
                    {

                        DateTime existingFileLastWrite = existingSPCs[fi.Name];

                        if (existingFileLastWrite.AddMilliseconds(1000) < fi.LastWriteTime)
                        {
                            if (IsFileReadyForProcessing(fi.FullName))
                            {
                                int? attachmentPK = null;
                                try
                                {
                                    attachmentPK = _maAttachmentOperations.GetAttachmentPK(fi.Name);
                                }
                                catch (Exception e)
                                {
                                    LogError(e, "Failed to retrieve existing attachment PK, for SMPC file: " + fi.Name, Ma_service_log_PK.EventType.DBError);
                                    continue;
                                }
                                if (!SaveSPCFile(fi, attachmentPK)) failedFiles.Add(fi, attachmentPK);
                            }
                        }
                    }

                }

                int failedCount = failedFiles.Count;
                if (failedCount > 0)
                {
                    foreach (KeyValuePair<FileInfo, int?> fi in failedFiles)
                    {
                        if (SaveSPCFile(fi.Key, fi.Value)) failedCount--;
                    }
                }

                if (failedCount > 0)
                {
                    LogError(null, failedCount + " new SMPC files failed to save DB", Ma_service_log_PK.EventType.DBError);
                }

            }
            else
            {
                LogEvent("SMPCs inbound folder does not exists!", null, Ma_service_log_PK.EventType.DiskError);
            }
        }


        /// <summary>
        /// Method saves SPC to database.
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="attachemntPK"></param>
        private static bool SaveSPCFile(FileInfo fi, int? attachemntPK)
        {
            if (fi == null)
            {
                LogError(null, "SMPC FileInfo is null.", Ma_service_log_PK.EventType.Warning);
                return false;
            }

            Ma_attachment_PK newSPC;
            byte[] fileData = null;
            try
            {
                fileData = File.ReadAllBytes(fi.FullName);
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to read SMPC file from disk!", null);
                return false;
            }
            newSPC = new Ma_attachment_PK()
            {
                deleted = false,
                file_data = fileData,
                file_name = fi.Name,
                file_path = fi.DirectoryName,
                last_change = fi.LastWriteTime,
                ma_attachment_PK = attachemntPK,
            };
            try
            {
                _maAttachmentOperations.Save(newSPC);
                LogEvent("New SMPC file: " + fi.Name + " saved to DB successfully", Ma_service_log_PK.EventType.Message);
                return true;
            }
            catch (Exception ex)
            {
                LogError(ex, "Error while saving SMPC attachment to DB!", null);
                return false;
            }
        }
        #endregion

        #region HelperMethods


        private static byte[] RemoveBoom(byte[] bytes)
        {

            byte[] witouthBoom = null;
            if (bytes.Length > 2 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            {
                witouthBoom = new byte[bytes.Length - 3];
                Array.Copy(bytes, 3, witouthBoom, 0, witouthBoom.Length);
            }
            else
            {
                witouthBoom = new byte[bytes.Length];
                Array.Copy(bytes, 0, witouthBoom, 0, witouthBoom.Length);
            }

            return witouthBoom;
        }

        /// <summary>
        /// Method trys to generate unique ReadyID.
        /// If metohd fails to generate ID after 10 trys, ID will not be generated.
        /// </summary>
        /// <returns>ReadyID on sucess, otherweise null</returns>
        private static String GenerateReadyId()
        {
            int attemptsRemained = 10;
            try
            {
                while (attemptsRemained > 0)
                {
                    String readyId = Guid.NewGuid().ToString("N").Substring(0, 8);
                    Marketing_authorisation_PK existingFile = _marketingAuthorisationOperations.GetEntityByReadyId(readyId);
                    if (existingFile == null) return readyId;
                    attemptsRemained--;
                }
            }
            catch (Exception e)
            {
                LogError(e, "Failed to generate ready ID.", Ma_service_log_PK.EventType.ReadyIdGenerationFailed);
            }
            return null;
        }

        /// <summary>
        /// Method trys to save MA to database.
        /// If saving fails, DBError event will be logged.
        /// </summary>
        /// <param name="ma"></param>
        /// <returns>True on sucess, otherweise false.</returns>
        private static bool SaveMA(Marketing_authorisation_PK ma)
        {
            try
            {
                _marketingAuthorisationOperations.Save(ma);
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to save MA to database.", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                return false;
            }

            return true;
        }

        private static bool UpdateMAxEVPRMStatus(Marketing_authorisation_PK ma, XevprmStatus status)
        {
            try
            {
                Xevprm_message_PK message = _xevprmMessageOperations.GetEntityByMA(ma.marketing_authorisation_PK);
                if (message != null)
                {
                    message.XevprmStatus = status;
                    _xevprmMessageOperations.Save(message);
                    return true;
                }
                else
                {
                    LogEvent("Failed retrieve MA xEVPRM message", ma.ready_id, Ma_service_log_PK.EventType.Error);
                    return false;
                }
            }
            catch (Exception e)
            {
                LogEvent("Failed to update MAxEVPRM status.", ma.ready_id, Ma_service_log_PK.EventType.DBError);
                return false;
            }
        }

        /// <summary>
        /// Method checks if file is ready for processing.
        /// File is ready if it is possible to obtain exclusive acess for it, ie.
        /// it is not used by another process.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True if file is ready, otherewise false.</returns>
        private static bool IsFileReadyForProcessing(String path)
        {
            FileInfo file = CreateFileInfo(path);
            if (file == null || !file.Exists) return false;
            bool isReadyForProcessing = true;
            FileStream fileStream = null;
            try
            {
                fileStream = File.Open(file.FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (Exception ex)
            {
                isReadyForProcessing = false;
            }
            finally
            {
                if (fileStream != null) fileStream.Close();
            }

            return isReadyForProcessing;
        }

        private static Ma_message_header_PK ExtractMessageHeader(Marketing_authorisation_PK ma, Ma_file_PK maFile)
        {
            var xmlValidator = new EVMessage.XmlValidator.XmlValidator();
            bool isValid = false;
            try
            {
                isValid = xmlValidator.Validate(maFile.file_data, ASCIIEncoding.UTF8.GetBytes(EVMessage.Properties.Resources.MarketingAuthorisationXSD));
            }
            catch (Exception e)
            {
                isValid = false;
            }

            String messageXML = Encoding.UTF8.GetString(maFile.file_data);
            messageXML = messageXML.Substring(messageXML.IndexOf("<marketingauthorisation"));

            Ma_message_header_PK msgHeader = null;
            if (isValid)
            {
                EVMessage.MarketingAuthorisation.Schema.XRoot message = EVMessage.MarketingAuthorisation.Schema.XRoot.Parse(messageXML);
                EVMessage.MarketingAuthorisation.Schema.messageheaderType messageheader = message.marketingauthorisation.messageheader;
                DateTime? msgDate = null;
                try
                {
                    msgDate = DateTime.ParseExact(messageheader.messagedate, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    msgDate = null;
                }

                msgHeader = new Ma_message_header_PK()
                {
                    messageformatversion = messageheader.messageformatversion.Trim(),
                    messageformatrelease = messageheader.messageformatrelease.Trim(),
                    registrationnumber = messageheader.registrationnumber.Trim(),
                    registrationid = (long)messageheader.registrationid,
                    readymessageid = messageheader.readymessageid != null ? messageheader.readymessageid.Trim() : "",
                    messagedateformat = messageheader.messagedateformat.Trim(),
                    messagedate = msgDate,
                };
            }
            else
            {
                Regex reg = new Regex(@"<(?<tagName>registrationnumber|registrationid|readymessageid|messagedate|messagedateformat|messageformatversion|messageformatrelease)>(?<tagValue>.*?)</\1>");
                MatchCollection matches = null;
                try
                {
                    matches = reg.Matches(messageXML);
                }
                catch (Exception e)
                {
                    matches = null;
                }
                msgHeader = new Ma_message_header_PK();
                if (matches != null)
                {
                    foreach (Match m in matches)
                    {
                        String tagValue = m.Groups["tagValue"].Value;
                        switch (m.Groups["tagName"].Value)
                        {
                            case "registrationnumber":
                                msgHeader.registrationnumber = (!String.IsNullOrWhiteSpace(tagValue) && tagValue.Trim().Length <= 30) ? tagValue.Trim() : "";
                                break;
                            case "registrationid":
                                Int64 tmp;
                                if (String.IsNullOrWhiteSpace(tagValue) || tagValue.Length > 10 || !Int64.TryParse(tagValue, out tmp)) msgHeader.registrationid = null;
                                else msgHeader.registrationid = tmp;
                                break;
                            case "readymessageid":
                                msgHeader.readymessageid = (!String.IsNullOrWhiteSpace(tagValue) && tagValue.Trim().Length <= 60) ? tagValue.Trim() : "";
                                break;
                            case "messagedateformat":
                                msgHeader.messagedateformat = (!String.IsNullOrWhiteSpace(tagValue) && tagValue.Trim().Length <= 3) ? tagValue.Trim() : "";
                                break;
                            case "messageformatversion":
                                msgHeader.messageformatversion = (!String.IsNullOrWhiteSpace(tagValue) && tagValue.Trim().Length <= 3) ? tagValue.Trim() : "";
                                break;
                            case "messageformatrelease":
                                msgHeader.messageformatrelease = (!String.IsNullOrWhiteSpace(tagValue) && tagValue.Trim().Length <= 3) ? tagValue.Trim() : "";
                                break;
                            case "messagedate":
                                try
                                {
                                    msgHeader.messagedate = DateTime.ParseExact(tagValue, "yyyyMMddHHmmss", System.Globalization.CultureInfo.InvariantCulture);
                                }
                                catch (Exception e)
                                {
                                    msgHeader.messagedate = null;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    msgHeader.registrationnumber = String.Empty;
                    msgHeader.registrationid = null;
                    msgHeader.readymessageid = String.Empty;
                    msgHeader.messagedateformat = String.Empty;
                    msgHeader.messageformatversion = String.Empty;
                    msgHeader.messageformatrelease = String.Empty;
                    msgHeader.messagedate = null;
                }
            }
            msgHeader.message_file_name = maFile.file_name;
            msgHeader.ready_id_FK = ma.ready_id;
            return msgHeader;
        }

        /// <summary>
        /// Method resolves message folder name. Folder name is constructed from messagedate,
        /// messageregistrationnumber and messageregistrationid. If any of the data is missing
        /// message folder will be set to junk folder.
        /// </summary>
        /// <param name="fi"></param>
        /// <param name="msgHeader"></param>
        /// <returns></returns>
        private static String ResolveMessageFolder(Ma_message_header_PK msgHeader)
        {

            String msgReceivedDate = (msgHeader.messagedate.HasValue) ? msgHeader.messagedate.Value.ToString("yyyyMMdd") : String.Empty;
            if (String.IsNullOrEmpty(msgReceivedDate)) return null;
            if (!String.IsNullOrEmpty(msgHeader.registrationnumber) && (msgHeader.registrationid != null) && !String.IsNullOrEmpty(msgHeader.ready_id_FK))
            {

                try
                {
                    return Path.Combine(msgReceivedDate, msgHeader.registrationnumber + "_" + msgHeader.registrationid + "_" + msgHeader.ready_id_FK);
                }
                catch (Exception e)
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Method trys to create DirectoryInfo for specified path. 
        /// If method fails, event will be logged to error log, and null returned.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static DirectoryInfo CreateDirectoryInfo(String path)
        {
            try
            {
                return new DirectoryInfo(path);
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to create DirectoryInfo for the path: " + path, Ma_service_log_PK.EventType.DiskError);
                return null;
            }
        }

        private static FileInfo CreateFileInfo(String path)
        {
            try
            {
                return new FileInfo(path);
            }
            catch (Exception ex)
            {
                LogError(ex, "Failed to create FileInfo for the path: " + path, Ma_service_log_PK.EventType.DiskError);
                return null;
            }
        }

        /// <summary>
        /// Method performs simple DB query to ensure DB is working.
        /// </summary>
        /// <returns>True on success, false otherwise.</returns>
        private static bool IsDBReady()
        {
            try
            {
                Ma_service_log_PK testPK = _maServiceLogOperations.GetEntity(1);
                return true;
            }
            catch (Exception e)
            {
                LogError(e, "Database check failed.", Ma_service_log_PK.EventType.DBError);
                return false;
            }
        }


        private static XmlValidatorException GetDateException(String messageXML)
        {
            Regex reg = new Regex(@"<(?<tagName>messagedate)>(?<tagValue>.*?)</\1>");
            Match m = null;
            try
            {
                m = reg.Match(messageXML);
            }
            catch (Exception e)
            {
                m = null;
            }
            if (m == null) return new XmlValidatorException(0, 0, "Message date cannot be found!", XmlValidatorExceptionType.Error);

            String dateString = m.Groups["tagValue"].Value;
            StringBuilder message = new StringBuilder();
            //int tmp;
            //if (!int.TryParse(dateString.Substring(0, 4), out tmp))
            //{
            //    message.Append(" '" + dateString.Substring(0, 4) + "' does not represent valid year.");
            //}
            //else
            //{
            //    if (tmp == 0) message.Append(" '0000' is not valid year, minimum value for year is '0001'!");
            //    if (tmp > 9999) message.Append(" '" + dateString.Substring(0, 4) + "' is not valid year, maximum value for year is '9999'!");
            //}

            //if (!int.TryParse(dateString.Substring(4, 2), out tmp))
            //{
            //    message.Append(" '" + dateString.Substring(4, 2) + "' does not represent valid month.");
            //}
            //else
            //{
            //    if (tmp == 0) message.Append(" '00' is not valid month, minimum value for month is '01'!");
            //    if (tmp > 12) message.Append(" '" + dateString.Substring(4, 2) + "' is not valid month, maximum value for month is '12'!");
            //}

            //if (!int.TryParse(dateString.Substring(6, 2), out tmp))
            //{
            //    message.Append(" '" + dateString.Substring(4, 2) + "' does not represent valid day.");
            //}
            //else
            //{
            //    if (tmp == 0) message.Append(" '00' is not valid day, minimum value for day is '01'!");
            //    if (tmp > 31) message.Append(" '" + dateString.Substring(4, 2) + "' is not valid day, maximum value for day is '31'!");
            //}

            try
            {
                DateTime.ParseExact(dateString.Substring(0, 8), "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                message.Append(" '" + dateString.Substring(0, 8) + "' is not valid date string according to 'CCYYMMDD' format!");
            }

            try
            {
                DateTime.ParseExact(dateString.Substring(8), "HHmmss", System.Globalization.CultureInfo.InvariantCulture);
            }
            catch (Exception e)
            {
                message.Append(" '" + dateString.Substring(8) + "' is not valid time string according to 'HHmmss' format!");
            }

            return new XmlValidatorException(LineFromPos(messageXML, m.Index), 0, message.ToString(), XmlValidatorExceptionType.Error);


        }

        private static List<XmlValidatorException> ValidateMessageNumber(string msgNumber)
        {
            List<XmlValidatorException> exceptions = new List<XmlValidatorException>();
            foreach (char c in System.IO.Path.GetInvalidPathChars())
            {
                if (msgNumber.IndexOf(c) >= 0) exceptions.Add(new XmlValidatorException(0, 0, "Message number contains illegal character: '"+c+"'", XmlValidatorExceptionType.Error));
            }

            return exceptions;
        }

        private static int LineFromPos(string s, int pos)
        {
            int res = 1;
            for (int i = 0; i <= pos - 1; i++)
                if (s[i] == '\n') res++;
            return res;
        }

        #endregion

        #region Log events

        private static string ConstructErrorDescription(Exception ex, string description = null)
        {
            StringBuilder errorBuilder = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(description))
            {
                errorBuilder.Append(description);
            }

            if (ex != null)
            {
                if (errorBuilder.Length > 0)
                {
                    errorBuilder.Append(" |");

                }

                errorBuilder.Append(" Exception: " + ex.Message);

                if (ex.InnerException != null)
                {
                    errorBuilder.Append(" | Inner Exception: " + ex.InnerException.Message);
                }

                errorBuilder.Append(" | StackTrace: " + ex.StackTrace);
            }

            return errorBuilder.ToString();
        }

        private static void LogError(Exception ex, string description, String ready_id, Ma_service_log_PK.EventType errorType)
        {
            description = ConstructErrorDescription(ex, description);

            LogEvent(description, ready_id, errorType);
        }

        private static void LogError(Exception ex, string description, Ma_service_log_PK.EventType errorType)
        {
            LogError(ex, description, null, errorType);
        }

        private static void LogError(Exception ex, string description, String ready_id)
        {
            LogError(ex, description, ready_id, Ma_service_log_PK.EventType.Error);
        }

        private static void LogError(Exception ex, string description)
        {
            LogError(ex, description, null, Ma_service_log_PK.EventType.Error);
        }

        private static void LogEvent(string description, Ma_service_log_PK.EventType evtType)
        {
            LogEvent(description, null, evtType);
        }

        private static void LogEvent(string description, String ready_id, Ma_service_log_PK.EventType evtType)
        {
            var logEntry = new Ma_service_log_PK()
            {
                description = description,
                log_time = DateTime.Now,
                ready_id_FK = ready_id,
                event_type_FK = (int)evtType
            };

            try
            {
                _maServiceLogOperations.Save(logEntry);
            }
            catch (Exception ex)
            {
                string error = "Error at saving event to database!";
                LogErrorToFile(ex, description + " | " + error, ready_id);
            }
        }

        private static void LogErrorToFile(Exception ex, string description, String ready_id)
        {
            description = ConstructErrorDescription(ex, description);

            LogEventToFile(description, ready_id, Ma_service_log_PK.EventType.Error);
        }

        private static void LogEventToFile(string description, String ready_id, Ma_service_log_PK.EventType evtType)
        {
            StringBuilder errorBuilder = new StringBuilder();

            errorBuilder.AppendLine("-------------------------------------------------------");
            errorBuilder.AppendLine("Event type: " + evtType.ToString() + Environment.NewLine);
            errorBuilder.AppendLine(DateTime.Now.ToString() + Environment.NewLine);
            if (ready_id != null)
            {
                errorBuilder.AppendLine("Related file primary key: " + ready_id + " (Table: MA_INBOUND_FILE)" + Environment.NewLine);
            }
            errorBuilder.AppendLine(description);

            using (StreamWriter streamWriter = new System.IO.StreamWriter(ServiceLogFile, true))
            {
                streamWriter.WriteLine(errorBuilder.ToString());
                streamWriter.Flush();
            }
        }

        #endregion

        #region JUNK CODE (testing helpers)
        static Random rnd = new Random();
        private static String generateMessageVariation(String messageXML)
        {
            List<String> spcs = new List<string>();
            spcs.Add("Atorvastatin Billev SPC_6.4.2012.pdf");
            spcs.Add("AtorvastatinBillevSPC.pdf");
            spcs.Add("Gliclazida_Biva_SPC_PT.DOC");
            spcs.Add("SmPC Candesartan Billev tablet - SE.pdf");
            spcs.Add("SPC_2011101586_22-03-2012.pdf");
            spcs.Add("Word example PPI doc.docx");

            StringBuilder replacementHeader = new StringBuilder();
            replacementHeader.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            replacementHeader.AppendLine("<marketingauthorisation xmlns=\"EVMessage.MarketingAuthorisation.Schema\">");
            replacementHeader.AppendLine("<messageheader>");
            replacementHeader.AppendLine("<messageformatversion>1.0</messageformatversion>");
            replacementHeader.AppendLine("<messageformatrelease>1</messageformatrelease>");
            replacementHeader.AppendLine("<registrationnumber>" + Guid.NewGuid().ToString("N").Substring(15) + "</registrationnumber>");
            replacementHeader.AppendLine("<registrationid>" + Guid.NewGuid().ToString("N").Substring(15) + "</registrationid>");
            replacementHeader.AppendLine("<readymessageid>" + Guid.NewGuid().ToString("N").Substring(15) + "</readymessageid>");
            replacementHeader.AppendLine("<messagedateformat>204</messagedateformat>");
            replacementHeader.AppendLine("<messagedate>" + rnd.Next(2011, 2012) + rnd.Next(5, 6).ToString("#00") + rnd.Next(7, 13).ToString("#00") + "140512</messagedate>");
            replacementHeader.AppendLine("</messageheader>");
            messageXML = Regex.Replace(messageXML, @"(<evprm xmlns=(.|\n)*</ichicsrmessageheader>)", replacementHeader.ToString(), RegexOptions.Multiline);
            messageXML = Regex.Replace(messageXML, "</evprm>", "</marketingauthorisation>");
            Regex reg = new Regex("<filename>(.*)</filename>");
            Match m = reg.Match(messageXML);
            if (m.Success)
            {
                messageXML = messageXML.Replace(m.Groups[1].Value, spcs[rnd.Next(0, spcs.Count)]);
            }
            return messageXML;
        }
        private static void PrepareMessagese()
        {

            List<Xevprm_message_PK> messages = _xevprmMessageOperations.GetEntities();
            DirectoryInfo di = new DirectoryInfo(@"D:\Sandoz\newMessages\");
            int messageNr = 0;

            foreach (Xevprm_message_PK message in messages)
            {
                String messageXML = message.xml;

                for (int i = 0; i < 10; i++)
                {
                    File.WriteAllText(Path.Combine(di.FullName, "20120914msg" + messageNr + ".xml"), generateMessageVariation(messageXML));
                    messageNr++;
                }
                for (int i = 0; i < 2; i++)
                {
                    String destoryedMessage = generateMessageVariation(messageXML);
                    destoryedMessage = destoryedMessage.Remove(rnd.Next(0, destoryedMessage.Length - 6), rnd.Next(1, 4));
                    if (rnd.Next(0, 100) < 25) destoryedMessage = destoryedMessage.Remove(rnd.Next(0, destoryedMessage.Length - 15), rnd.Next(2, 7));
                    File.WriteAllText(Path.Combine(di.FullName, "20120914msg" + messageNr + "BAD.xml"), destoryedMessage);
                    messageNr++;
                }
                messageNr++;
            }
        }
        #endregion

    }
}
