// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	5.9.2012. 8:21:28
// Description:	GEM2 Generated class for table ready_dev.dbo.MA_SERVICE_LOG
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "ready_dev", Table = "MA_SERVICE_LOG")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Ma_service_log_PK
    {

        public enum EventType
        {
            NULL,
            Error,
            Warning,
            MAFileReceived,// - file primljen 
            MaFileSavedToDbSuccessfully,// - spremljen u bazu
            MaFileSaveToDbFailed,// - neuspješno spremljen u bazu
            MAReceivedFileTransferedSuccessfully,// - file premješten iz inbound u inbound-datetimereceived
            MAReceivedFileTransferFailed,// - neuspješno premještanje file-a 
            XMLValidationSuccesfull,// - prošla validacija xml-a
            XMLValidationFailed,// - neuspješna validacija xml-a
            MAReceivedCreateReportSucessful,
            MAReceivedCreateReportFailed,
            MAReceivedSaveReportToDBSuccessful,//
            MAReceivedSaveReportToDBFailed,//
            MAReceivedSaveReportToOutboundFolderSuccessful,//
            MAReceivedSaveReportToOutboundFolderFailed,//
            MAXMLDataValidationFailed,//

            MADataTranslationSuccessful,// - Ne postoje greške prilikom translacije podataka u bazu
            MADataTranslationFailed,// - Prilikom spremanja u bazu dogodila se greška zbog db constrainta
            MAXevprmCreationFailed,//
            MAXevprmCreationSuccessfull,//
            MAXevprmValidationSuccessful,//
            MAXevprmValidationFailed,//
            MAValidationCreateReportSucessful,
            MAValidationCreateReportFailed,
            MAValidationSaveReportToDBSuccessful,//
            MAValidationSaveReportToDBFailed,//
            MAValidationSaveReportToOutboundFolderSuccessful,//
            MAValidationSaveReportToOutboundFolderFailed,//
            MAValidationSuccessful,// - 
            MAValidationFailed,// - 

            MAXevprmSendingFailed,// - xevprm poruka nije uspješno poslana nakon n pokušaja
            MAXevprmSent,// - xevprm poruka poslana
            MAXevprmSaveToDBFailed,
            MAXEvprmSaveToDBSuccessful,
            MAXevprmSaveToDiskFailed,
            MAXEvprmSaveToDiskSuccessful,
            MAXevprmSentCreateReportSucessful,
            MAXevprmSentCreateReportFailed,
            MAXevprmSentSaveReportToDBSuccessful,//
            MAXevprmSentSaveReportToDBFailed,//
            MAXevprmSentSaveReportToOutboundFolderSuccessful,//
            MAXevprmSentSaveReportToOutboundFolderFailed,//

            MAACKReceived,// - xevprm poruka nije uspješno poslana nakon n pokušaja
            MAACKSaveToDBFailed,
            MAACKSaveToDBSuccessful,
            MAACKSaveToDiskFailed,
            MAACKSaveToDiskSuccessful,
            MAACKCreateReportSucessful,
            MAACKCreateReportFailed,
            MAACKSaveReportToDBSuccessful,//
            MAACKSaveReportToDBFailed,//
            MAACKSaveReportToOutboundFolderSuccessful,//
            MAACKSaveReportToOutboundFolderFailed,//

            ReadyIdGenerationFailed,
            FailedToCreateDirectory,
            FailedToUpdateXEVPRMStatus,
            xEVPRMMissing,
            DBError,
            FileInUse,
            DiskError,
            ACKMissing,
            Message,
        }


        private Int32? _ma_service_log_PK;
        private DateTime? _log_time;
        private String _description;
        private String _ready_id_FK;
        private Int32? _event_type_FK;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? ma_service_log_PK
        {
            get { return _ma_service_log_PK; }
            set { _ma_service_log_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? log_time
        {
            get { return _log_time; }
            set { _log_time = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ready_id_FK
        {
            get { return _ready_id_FK; }
            set { _ready_id_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? event_type_FK
        {
            get { return _event_type_FK; }
            set { _event_type_FK = value; }
        }

        #endregion

        public Ma_service_log_PK() { }
        public Ma_service_log_PK(Int32? ma_service_log_PK, DateTime? log_time, String description, String ready_id_FK, Int32? event_type_FK)
        {
            this.ma_service_log_PK = ma_service_log_PK;
            this.log_time = log_time;
            this.description = description;
            this.ready_id_FK = ready_id_FK;
            this.event_type_FK = event_type_FK;
        }
    }

    public interface IMa_service_log_PKOperations : ICRUDOperations<Ma_service_log_PK>
    {
        DataSet GetReportDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
    }
}
