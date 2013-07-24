// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	30.11.2011. 14:02:30
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TASK_COUNTRY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "TASK_COUNTRY_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Task_country_PK
	{
		private Int32? _task_country_PK;
		private Int32? _task_FK;
		private Int32? _country_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? task_country_PK
		{
			get { return _task_country_PK; }
			set { _task_country_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? task_FK
		{
			get { return _task_FK; }
			set { _task_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? country_FK
		{
			get { return _country_FK; }
			set { _country_FK = value; }
		}

		#endregion

		public Task_country_PK() { }
		public Task_country_PK(Int32? task_country_PK, Int32? task_FK, Int32? country_FK)
		{
			this.task_country_PK = task_country_PK;
			this.task_FK = task_FK;
			this.country_FK = country_FK;
		}
	}

	public interface ITask_country_PKOperations : ICRUDOperations<Task_country_PK>
	{
        List<Task_country_PK> GetCountriesByTask(Int32? task_PK);
        void DeleteByTaskPK(Int32? task_PK);
	}
}
