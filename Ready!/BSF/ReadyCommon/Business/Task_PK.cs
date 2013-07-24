// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	30.11.2011. 13:59:30
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TASK
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "TASK")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Task_PK
	{
		private Int32? _task_PK;
		private Int32? _activity_FK;
		private Int32? _user_FK;
		private Int32? _task_name_FK;
		private String _description;
		private String _comment;
		private Int32? _type_internal_status_FK;
		private DateTime? _start_date;
		private DateTime? _expected_finished_date;
		private DateTime? _actual_finished_date;
		private Int32? _pOM_internal_status;
        private bool _automatic_alerts_on;
        private bool _prevent_start_date_alert;
        private bool _prevent_exp_finish_date_alert;
        private String _task_ID;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? task_PK
		{
			get { return _task_PK; }
			set { _task_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? activity_FK
		{
			get { return _activity_FK; }
			set { _activity_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_FK
		{
			get { return _user_FK; }
			set { _user_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? task_name_FK
		{
			get { return _task_name_FK; }
			set { _task_name_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String description
		{
			get { return _description; }
			set { _description = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String comment
		{
			get { return _comment; }
			set { _comment = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? type_internal_status_FK
		{
			get { return _type_internal_status_FK; }
			set { _type_internal_status_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? start_date
		{
			get { return _start_date; }
			set { _start_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? expected_finished_date
		{
			get { return _expected_finished_date; }
			set { _expected_finished_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? actual_finished_date
		{
			get { return _actual_finished_date; }
			set { _actual_finished_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? POM_internal_status
		{
			get { return _pOM_internal_status; }
			set { _pOM_internal_status = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public bool automatic_alerts_on
        {
            get { return _automatic_alerts_on; }
            set { _automatic_alerts_on = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public bool prevent_start_date_alert
        {
            get { return _prevent_start_date_alert; }
            set { _prevent_start_date_alert = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public bool prevent_exp_finish_date_alert
        {
            get { return _prevent_exp_finish_date_alert; }
            set { _prevent_exp_finish_date_alert = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String task_ID
        {
            get { return _task_ID; }
            set { _task_ID = value; }
        }

		#endregion

		public Task_PK() { }
        public Task_PK(Int32? task_PK, Int32? activity_FK, Int32? user_FK, Int32? task_name_FK, String description, String comment, Int32? type_internal_status_FK, DateTime? start_date, DateTime? expected_finished_date, DateTime? actual_finished_date, Int32? pOM_internal_status, bool automatic_alerts_on, bool prevent_start_date_alert, bool prevent_exp_finish_date_alert, String task_ID)
		{
			this.task_PK = task_PK;
			this.activity_FK = activity_FK;
			this.user_FK = user_FK;
			this.task_name_FK = task_name_FK;
			this.description = description;
			this.comment = comment;
			this.type_internal_status_FK = type_internal_status_FK;
			this.start_date = start_date;
			this.expected_finished_date = expected_finished_date;
			this.actual_finished_date = actual_finished_date;
			this.POM_internal_status = pOM_internal_status;
            this.automatic_alerts_on = automatic_alerts_on;
            this.prevent_start_date_alert = prevent_start_date_alert;
            this.prevent_exp_finish_date_alert = prevent_exp_finish_date_alert;
            this._task_ID = task_ID;
		}
	}

	public interface ITask_PKOperations : ICRUDOperations<Task_PK>
	{
        List<Task_PK> GetTasksByActivity(int? activity_PK);
        DataSet GetTaskSearcherDataSet(String name, String activity, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetTabMenuItemsCount(Int32 task_PK, int? personFk);

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormSearchDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);

        bool AbleToDeleteEntity(int taskPk);
	}
}
