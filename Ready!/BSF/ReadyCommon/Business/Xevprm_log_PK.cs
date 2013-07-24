// ======================================================================================================================
// Author:		POSSIMUSIT-MATE\Mateo
// Create date:	28.3.2012. 10:37:24
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.XEVMPD_LOG
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "XEVPRM_LOG")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Xevprm_log_PK
	{
		private Int32? _xevprm_log_PK;
		private Int32? _xevprm_message_FK;
		private DateTime? _log_time;
		private String _description;
	        private String _username;
                private Int32? _xevprm_status_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? xevprm_log_PK
		{
			get { return _xevprm_log_PK; }
			set { _xevprm_log_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? xevprm_message_FK
		{
			get { return _xevprm_message_FK; }
			set { _xevprm_message_FK = value; }
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
        public String username
        {
            get { return _username; }
            set { _username = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? xevprm_status_FK
        {
            get { return _xevprm_status_FK; }
            set { _xevprm_status_FK = value; }
        }

        public XevprmStatus XevprmStatus
        {
            get { return xevprm_status_FK != null && Enum.IsDefined(typeof(XevprmStatus), xevprm_status_FK) ? (XevprmStatus)xevprm_status_FK : XevprmStatus.NULL; }
            set { xevprm_status_FK = (int)value == 0 ? (int?)null : (int)value; }
        }
		#endregion

		public Xevprm_log_PK() { }
        public Xevprm_log_PK(Int32? xEVMPD_log_PK, Int32? xevprm_message_FK, DateTime? log_time, String description, String username, int? xevprm_status_FK)
		{
			this.xevprm_log_PK = xEVMPD_log_PK;
			this.xevprm_message_FK = xevprm_message_FK;
			this.log_time = log_time;
			this.description = description;
		    this.username = username;
            this.xevprm_status_FK = xevprm_status_FK;
		}
	}

	public interface IXevprm_log_PKOperations : ICRUDOperations<Xevprm_log_PK>
	{
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        string GetMessageSubmissionError(Int32? xevprmMessagePk, string messageType);
	}
}
