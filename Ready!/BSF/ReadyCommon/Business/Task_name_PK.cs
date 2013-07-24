// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	30.11.2011. 14:01:19
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TASK_NAME
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "TASK_NAME")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Task_name_PK
	{
		private Int32? _task_name_PK;
		private String _task_name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? task_name_PK
		{
			get { return _task_name_PK; }
			set { _task_name_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String task_name
		{
			get { return _task_name; }
			set { _task_name = value; }
		}

		#endregion

		public Task_name_PK() { }
		public Task_name_PK(Int32? task_name_PK, String task_name)
		{
			this.task_name_PK = task_name_PK;
			this.task_name = task_name;
		}
	}

	public interface ITask_name_PKOperations : ICRUDOperations<Task_name_PK>
	{
        DataSet GetEntityByName(String name);

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	}
}
