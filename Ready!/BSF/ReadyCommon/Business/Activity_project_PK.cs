// ======================================================================================================================
// Author:		Mateo-HP\Mateo
// Create date:	6.12.2011. 13:47:41
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY_PROJECT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ACTIVITY_PROJECT_MN", Active=false)]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Activity_project_PK
	{
		private Int32? _activity_project_PK;
		private Int32? _activity_FK;
		private Int32? _project_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? activity_project_PK
		{
			get { return _activity_project_PK; }
			set { _activity_project_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? activity_FK
		{
			get { return _activity_FK; }
			set { _activity_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? project_FK
		{
			get { return _project_FK; }
			set { _project_FK = value; }
		}

		#endregion

		public Activity_project_PK() { }
		public Activity_project_PK(Int32? activity_project_PK, Int32? activity_FK, Int32? project_FK)
		{
			this.activity_project_PK = activity_project_PK;
			this.activity_FK = activity_FK;
			this.project_FK = project_FK;
		}
	}

	public interface IActivity_project_PKOperations : ICRUDOperations<Activity_project_PK>
	{
        void StartSessionActivity(int? activity_PK);
        void EndSessionActivity(int? activity_PK);
        void DeleteByActivity(int activityPk);
	}
}
