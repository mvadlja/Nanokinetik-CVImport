// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	5.9.2012. 13:03:02
// Description:	GEM2 Generated class for table ready_dev.dbo.MARKETING_AUTHORISATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "ready_dev", Table = "MARKETING_AUTHORISATION")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Marketing_authorisation_PK
	{

        public enum MAStatus{
            NULL,
            MAFileLoaded, // XML je uèitan s dika, i zapisan u bazu
            MAFileReceived,// - file je otvoren, u bazu je zapisan header filea. Servis dohvaæa sve u ovom statusu i obraðuje. U sluèaju uspješne obrade 
            MAReceivedProcessingFileFailed, // - neuspjelo procesiranje primljenog ma
            MAReceived, // - ma uspješno primljen, validiran i kreiran report
            MAReceivedErrors, // - ma uspješno primljen, nije validan i kreiran report

            MAXevprmValidationSuccessful,// - uspješno insertanje podataka u bazu, uspješna DataTranslation validacija i validacija EMA poslovnih pravila, xml poruke kreiran i poruka je spremna za slanje. 
            MAXevprmValidationFailed,// - uspješno insertanje podataka u bazu, uspješna DataTranslation validacija, nesupješna validacija EMA poslovnih pravila
            MAProcessingDataFailed,// - generalna greška prilikom procesiranja podataka, ma ostaje u ovom stanju
            MAValidationSuccessful,// - uspješno insertanje podataka u bazu, uspješna DataTranslation validacija i validacija EMA poslovnih pravila, xml poruke kreiran i poruka je spremna za slanje. Report je kreiran. 
            MAValidationFailed,// - neuspješna DataTranslation validacija ili nesupješna validacija EMA poslovnih pravila, report je kreiran
            MAValidationReportSendingFailed, //slanje reporta je neuspjelo

            MAXevprmSent,// - xevprm poruka poslana i kreiran je report.
            MAXevprmProcessingSendingFailed,// - xevprm poruka nije uspješno poslana nakon n pokušaja ili report nije kreiran
  
            ACKReceived,//ACK is received
            ACKReceivedProcessedSuccessfully, //ACK report is sent sucessfully
            ACKReceivedProcessingFailed //ACK failed -not received or report is not sent
        }

        private Int32? _marketing_authorisation_PK;
        private String _ready_id;
        private Int32? _ma_status_FK;
        private String _message_folder;
        private DateTime? _creation_time;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? marketing_authorisation_PK
        {
            get { return _marketing_authorisation_PK; }
            set { _marketing_authorisation_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ready_id
        {
            get { return _ready_id; }
            set { _ready_id = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? ma_status_FK
        {
            get { return _ma_status_FK; }
            set { _ma_status_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String message_folder
        {
            get { return _message_folder; }
            set { _message_folder = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? creation_time
        {
            get { return _creation_time; }
            set { _creation_time = value; }
        }

        #endregion

        public Marketing_authorisation_PK() { }
        public Marketing_authorisation_PK(Int32? marketing_authorisation_PK, String ready_id, Int32? ma_status_FK, String message_folder, DateTime? creation_time)
        {
            this.marketing_authorisation_PK = marketing_authorisation_PK;
            this.ready_id = ready_id;
            this.ma_status_FK = ma_status_FK;
            this.message_folder = message_folder;
            this.creation_time = creation_time;
        }
    }


	public interface IMarketing_authorisation_PKOperations : ICRUDOperations<Marketing_authorisation_PK>
	{
        List<Marketing_authorisation_PK> GetEntitiesByStatus(Marketing_authorisation_PK.MAStatus status);
        Marketing_authorisation_PK GetEntityByReadyId(String readyId);
	}
}
