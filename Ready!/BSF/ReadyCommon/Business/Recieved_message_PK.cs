// ======================================================================================================================
// Author:		KIKI-PC\Alan
// Create date:	11.4.2012. 9:45:03
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.RECIEVED_MESSAGE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "RECIEVED_MESSAGE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Recieved_message_PK
	{
        private Int32? _recieved_message_PK;
        private Byte[] _msg_data;
        private DateTime? _received_time;
        private DateTime? _processed_time;
        private Boolean? _processed;
        private Boolean? _is_successfully_processed;
        private Int32? _msg_type;
        private String _as_header;
        private String _processing_error;
        private Int32? _xevmpd_FK;
        private Int32? _status;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? recieved_message_PK
        {
            get { return _recieved_message_PK; }
            set { _recieved_message_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Binary)]
        public Byte[] msg_data
        {
            get { return _msg_data; }
            set { _msg_data = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? received_time
        {
            get { return _received_time; }
            set { _received_time = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? processed_time
        {
            get { return _processed_time; }
            set { _processed_time = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? processed
        {
            get { return _processed; }
            set { _processed = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? is_successfully_processed
        {
            get { return _is_successfully_processed; }
            set { _is_successfully_processed = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? msg_type
        {
            get { return _msg_type; }
            set { _msg_type = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String as_header
        {
            get { return _as_header; }
            set { _as_header = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String processing_error
        {
            get { return _processing_error; }
            set { _processing_error = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? xevmpd_FK
        {
            get { return _xevmpd_FK; }
            set { _xevmpd_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? status
        {
            get { return _status; }
            set { _status = value; }
        }

        #endregion

        public Recieved_message_PK() { }
        public Recieved_message_PK(Int32? recieved_message_PK, Byte[] msg_data, DateTime? received_time, DateTime? processed_time, Boolean? processed, Boolean? is_successfully_processed, Int32? msg_type, String as_header, String processing_error, Int32? xevmpd_FK, Int32? status)
        {
            this.recieved_message_PK = recieved_message_PK;
            this.msg_data = msg_data;
            this.received_time = received_time;
            this.processed_time = processed_time;
            this.processed = processed;
            this.is_successfully_processed = is_successfully_processed;
            this.msg_type = msg_type;
            this.as_header = as_header;
            this.processing_error = processing_error;
            this.xevmpd_FK = xevmpd_FK;
            this.status = status;
        }
    }

	public interface IRecieved_message_PKOperations : ICRUDOperations<Recieved_message_PK>
	{
        List<Recieved_message_PK> GetNotProcessedMessages();
        List<int> GetNotProcessedEntitiesPks(ReceivedMessageType receivedMessageType);
        DataSet GetEntitiesDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        List<Recieved_message_PK> GetMessagesByTimeSpan(DateTime periodStart, DateTime periodEnd);
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
    }

    public enum ReceivedMessageType
    {
        ACK,
        MDN
    }

    public enum MDNStatus
    {
        Failed, Success
    }

    public enum ACKStatus
    {
        Success = 1, Errors, Failed
    }
}
