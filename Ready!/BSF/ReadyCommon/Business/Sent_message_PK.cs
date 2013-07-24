// ======================================================================================================================
// Author:		KIKI-PC\Alan
// Create date:	27.4.2012. 14:38:34
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SENT_MESSAGE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "SENT_MESSAGE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Sent_message_PK
	{
		private Int32? _sent_message_PK;
		private Byte[] _msg_data;
		private DateTime? _sent_time;
		private Int32? _msg_type;
		private Int32? _xevmpd_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? sent_message_PK
		{
			get { return _sent_message_PK; }
			set { _sent_message_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Binary)]
		public Byte[] msg_data
		{
			get { return _msg_data; }
			set { _msg_data = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? sent_time
		{
			get { return _sent_time; }
			set { _sent_time = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? msg_type
		{
			get { return _msg_type; }
			set { _msg_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? xevmpd_FK
		{
			get { return _xevmpd_FK; }
			set { _xevmpd_FK = value; }
		}

		#endregion

		public Sent_message_PK() { }
		public Sent_message_PK(Int32? sent_message_PK, Byte[] msg_data, DateTime? sent_time, Int32? msg_type, Int32? xevmpd_FK)
		{
			this.sent_message_PK = sent_message_PK;
			this.msg_data = msg_data;
			this.sent_time = sent_time;
			this.msg_type = msg_type;
			this.xevmpd_FK = xevmpd_FK;
		}
	}

	public interface ISent_message_PKOperations : ICRUDOperations<Sent_message_PK>
	{
        DataSet GetEntitiesDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        List<Sent_message_PK> GetMessagesByTimeSpan(DateTime periodStart, DateTime periodEnd);
        DataSet GetDataForTimeStatsByTimeSpan(DateTime periodStart, DateTime periodEnd);

        DataSet GetMDNDataForTimeStatsByTimeSpan(DateTime periodStart, DateTime periodEnd);
        DataSet GetACKDataForTimeStatsByTimeSpan(DateTime periodStart, DateTime periodEnd);
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
    }

    public enum SentMessageType
    {
        EVPRM, MDN
    }
}
