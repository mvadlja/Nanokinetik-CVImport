// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	24.2.2012. 13:15:49
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ACTIVITY_SAVED_SEARCH")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Activity_saved_search_PK
	{
		private Int32? _activity_saved_search_PK;
		private Int32? _project_FK;
		private Int32? _product_FK;
		private String _name;
		private Int32? _user_FK;
		private String _procedure_number;
		private Int32? _procedure_type_FK;
		private Int32? _type_FK;
		private Int32? _regulatory_status_FK;
		private Int32? _internal_status_FK;
		private Int32? _activity_mode_FK;
		private Int32? _applicant_FK;
		private Int32? _country_FK;
		private String _legal;
		private DateTime? _start_date_from;
		private DateTime? _start_date_to;
		private DateTime? _expected_finished_date_from;
		private DateTime? _expected_finished_date_to;
		private DateTime? _actual_finished_date_from;
		private DateTime? _actual_finished_date_to;
		private DateTime? _approval_date_from;
		private DateTime? _approval_date_to;
		private DateTime? _submission_date_from;
		private DateTime? _submission_date_to;
		private String _displayName;
		private Int32? _user_FK1;
		private String _gridLayout;
		private Boolean? _isPublic;
        private Boolean? _billable;
        private String _activity_Id;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? activity_saved_search_PK
		{
			get { return _activity_saved_search_PK; }
			set { _activity_saved_search_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? project_FK
		{
			get { return _project_FK; }
			set { _project_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_FK
		{
			get { return _user_FK; }
			set { _user_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String procedure_number
		{
			get { return _procedure_number; }
			set { _procedure_number = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? procedure_type_FK
		{
			get { return _procedure_type_FK; }
			set { _procedure_type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? type_FK
		{
			get { return _type_FK; }
			set { _type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? regulatory_status_FK
		{
			get { return _regulatory_status_FK; }
			set { _regulatory_status_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? internal_status_FK
		{
			get { return _internal_status_FK; }
			set { _internal_status_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? activity_mode_FK
		{
			get { return _activity_mode_FK; }
			set { _activity_mode_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? applicant_FK
		{
			get { return _applicant_FK; }
			set { _applicant_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? country_FK
		{
			get { return _country_FK; }
			set { _country_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String legal
		{
			get { return _legal; }
			set { _legal = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String activity_Id
        {
            get { return _activity_Id; }
            set { _activity_Id = value; }
        }

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? start_date_from
		{
			get { return _start_date_from; }
			set { _start_date_from = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? start_date_to
		{
			get { return _start_date_to; }
			set { _start_date_to = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? expected_finished_date_from
		{
			get { return _expected_finished_date_from; }
			set { _expected_finished_date_from = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? expected_finished_date_to
		{
			get { return _expected_finished_date_to; }
			set { _expected_finished_date_to = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? actual_finished_date_from
		{
			get { return _actual_finished_date_from; }
			set { _actual_finished_date_from = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? actual_finished_date_to
		{
			get { return _actual_finished_date_to; }
			set { _actual_finished_date_to = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? approval_date_from
		{
			get { return _approval_date_from; }
			set { _approval_date_from = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? approval_date_to
		{
			get { return _approval_date_to; }
			set { _approval_date_to = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? submission_date_from
		{
			get { return _submission_date_from; }
			set { _submission_date_from = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? submission_date_to
		{
			get { return _submission_date_to; }
			set { _submission_date_to = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String displayName
		{
			get { return _displayName; }
			set { _displayName = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_FK1
		{
			get { return _user_FK1; }
			set { _user_FK1 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String gridLayout
		{
			get { return _gridLayout; }
			set { _gridLayout = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
		public Boolean? isPublic
		{
			get { return _isPublic; }
			set { _isPublic = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? billable
        {
            get { return _billable; }
            set { _billable = value; }
        }

		#endregion

		public Activity_saved_search_PK() { }
        public Activity_saved_search_PK(Int32? activity_saved_search_PK, Int32? project_FK, Int32? product_FK, String name, Int32? user_FK, String procedure_number, Int32? procedure_type_FK, Int32? type_FK, Int32? regulatory_status_FK, Int32? internal_status_FK, Int32? activity_mode_FK, Int32? applicant_FK, Int32? country_FK, String legal, DateTime? start_date_from, DateTime? start_date_to, DateTime? expected_finished_date_from, DateTime? expected_finished_date_to, DateTime? actual_finished_date_from, DateTime? actual_finished_date_to, DateTime? approval_date_from, DateTime? approval_date_to, DateTime? submission_date_from, DateTime? submission_date_to, String displayName, Int32? user_FK1, String gridLayout, Boolean? isPublic, Boolean? billable, String activity_Id)
		{
			this.activity_saved_search_PK = activity_saved_search_PK;
			this.project_FK = project_FK;
			this.product_FK = product_FK;
			this.name = name;
			this.user_FK = user_FK;
			this.procedure_number = procedure_number;
			this.procedure_type_FK = procedure_type_FK;
			this.type_FK = type_FK;
			this.regulatory_status_FK = regulatory_status_FK;
			this.internal_status_FK = internal_status_FK;
			this.activity_mode_FK = activity_mode_FK;
			this.applicant_FK = applicant_FK;
			this.country_FK = country_FK;
			this.legal = legal;
            this._activity_Id = activity_Id;
			this.start_date_from = start_date_from;
			this.start_date_to = start_date_to;
			this.expected_finished_date_from = expected_finished_date_from;
			this.expected_finished_date_to = expected_finished_date_to;
			this.actual_finished_date_from = actual_finished_date_from;
			this.actual_finished_date_to = actual_finished_date_to;
			this.approval_date_from = approval_date_from;
			this.approval_date_to = approval_date_to;
			this.submission_date_from = submission_date_from;
			this.submission_date_to = submission_date_to;
			this.displayName = displayName;
			this.user_FK1 = user_FK1;
			this.gridLayout = gridLayout;
			this.isPublic = isPublic;
            this.billable = billable;
		}
	}

	public interface IActivity_saved_search_PKOperations : ICRUDOperations<Activity_saved_search_PK>
	{
        List<Activity_saved_search_PK> GetEntitiesByUserOrPublic(Int32? user_fk);
	}
}
