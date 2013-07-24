// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	11.11.2011. 10:14:03
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ACTIVITY", Active=true)]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Activity_PK
	{
		private Int32? _activity_PK;
		private Int32? _user_FK;
		private Int32? _mode_FK;
		private Int32? _procedure_type_FK;
		private String _name;
		private String _description;
		private String _comment;
		private Int32? _regulatory_status_FK;
		private DateTime? _start_date;
		private DateTime? _expected_finished_date;
		private DateTime? _actual_finished_date;
		private DateTime? _approval_date;
		private DateTime? _submission_date;
		private String _procedure_number;
		private String _legal;
		private String _cost;
		private Int32? _internal_status_FK;
        private String _activity_ID;
        private bool _automatic_alerts_on;
        private bool _prevent_start_date_alert;
        private bool _prevent_exp_finish_date_alert;
        private Boolean? _billable;
      
		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? activity_PK
		{
			get { return _activity_PK; }
			set { _activity_PK = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String activity_ID
        {
            get { return _activity_ID; }
            set { _activity_ID = value; }
        }

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_FK
		{
			get { return _user_FK; }
			set { _user_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? mode_FK
		{
			get { return _mode_FK; }
			set { _mode_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? procedure_type_FK
		{
			get { return _procedure_type_FK; }
			set { _procedure_type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
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
		public Int32? regulatory_status_FK
		{
			get { return _regulatory_status_FK; }
			set { _regulatory_status_FK = value; }
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

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? approval_date
		{
			get { return _approval_date; }
			set { _approval_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? submission_date
		{
			get { return _submission_date; }
			set { _submission_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String procedure_number
		{
			get { return _procedure_number; }
			set { _procedure_number = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String legal
		{
			get { return _legal; }
			set { _legal = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String cost
		{
			get { return _cost; }
			set { _cost = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? internal_status_FK
		{
			get { return _internal_status_FK; }
			set { _internal_status_FK = value; }
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

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? billable
        {
            get { return _billable; }
            set { _billable = value; }
        }

		#endregion

		public Activity_PK() { }
        public Activity_PK(Int32? activity_PK, Int32? user_FK, Int32? mode_FK, Int32? procedure_type_FK, String name, String description, String comment, Int32? regulatory_status_FK, DateTime? start_date, DateTime? expected_finished_date, DateTime? actual_finished_date, DateTime? approval_date, DateTime? submission_date, String procedure_number, String legal, String cost, Int32? internal_status_FK, String activity_ID, bool automatic_alerts_on, bool prevent_start_date_alert, bool prevent_exp_finish_date_alert, Boolean? billable)
		{
			this.activity_PK = activity_PK;
            this.activity_ID = activity_ID;
			this.user_FK = user_FK;
			this.mode_FK = mode_FK;
			this.procedure_type_FK = procedure_type_FK;
			this.name = name;
			this.description = description;
			this.comment = comment;
			this.regulatory_status_FK = regulatory_status_FK;
			this.start_date = start_date;
			this.expected_finished_date = expected_finished_date;
			this.actual_finished_date = actual_finished_date;
			this.approval_date = approval_date;
			this.submission_date = submission_date;
			this.procedure_number = procedure_number;
			this.legal = legal;
			this.cost = cost;
			this.internal_status_FK = internal_status_FK;
            this.automatic_alerts_on = automatic_alerts_on;
            this.prevent_start_date_alert = prevent_start_date_alert;
            this.prevent_exp_finish_date_alert = prevent_exp_finish_date_alert;
            this.billable = billable;
        }
	}

	public interface IActivity_PKOperations : ICRUDOperations<Activity_PK>
	{
        List<Activity_PK> GetActivityFromProduct(Int32? product_PK);

        DataSet GetActivitySearcherDataSet(String name, String applicant, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        List<Activity_PK> GetActivityFromProject(Int32? project_FK);
        DataSet GetTabMenuItemsCount(Int32 activity_PK, int? personFk);

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormSearchDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        bool AbleToDeleteEntity(int activityPk);
	}
}
