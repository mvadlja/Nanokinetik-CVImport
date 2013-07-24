// ======================================================================================================================
// Author:		POSSIMUSIT-MATE\Mateo
// Create date:	4.5.2012. 11:03:57
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SERVICE_LOG
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "SERVICE_LOG")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Service_log_PK
	{
		private Int32? _service_log_PK;
		private DateTime? _log_time;
		private String _description;
		private Int32? _user_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? service_log_PK
		{
			get { return _service_log_PK; }
			set { _service_log_PK = value; }
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

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_FK
		{
			get { return _user_FK; }
			set { _user_FK = value; }
		}

		#endregion

		public Service_log_PK() { }
		public Service_log_PK(Int32? service_log_PK, DateTime? log_time, String description, Int32? user_FK)
		{
			this.service_log_PK = service_log_PK;
			this.log_time = log_time;
			this.description = description;
			this.user_FK = user_FK;
		}
	}

	public interface IService_log_PKOperations : ICRUDOperations<Service_log_PK>
	{
        DateTime? TimeOfLastChange();
        DataSet GetReportDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	}
}
