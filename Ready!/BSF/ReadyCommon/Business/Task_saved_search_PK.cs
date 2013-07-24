// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	24.2.2012. 13:42:31
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TASK_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "TASK_SAVED_SEARCH")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Task_saved_search_PK
	{
		private Int32? _task_saved_search_PK;
		private Int32? _activity_FK;
		private String _name;
		private Int32? _user_FK;
		private Int32? _type_internal_status_FK;
		private Int32? _country_FK;
		private DateTime? _start_date_from;
		private DateTime? _start_date_to;
		private DateTime? _expected_finished_date_from;
		private DateTime? _expected_finished_date_to;
		private DateTime? _actual_finished_date_from;
		private DateTime? _actual_finished_date_to;
		private String _displayName;
		private Int32? _user_FK1;
		private String _gridLayout;
		private Boolean? _isPublic;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? task_saved_search_PK
		{
			get { return _task_saved_search_PK; }
			set { _task_saved_search_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? activity_FK
		{
			get { return _activity_FK; }
			set { _activity_FK = value; }
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

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? type_internal_status_FK
		{
			get { return _type_internal_status_FK; }
			set { _type_internal_status_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? country_FK
		{
			get { return _country_FK; }
			set { _country_FK = value; }
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

		#endregion

		public Task_saved_search_PK() { }
        public Task_saved_search_PK(Int32? task_saved_search_PK, Int32? activity_FK, String name, Int32? user_FK, Int32? type_internal_status_FK, Int32? country_FK, DateTime? start_date_from, DateTime? start_date_to, DateTime? expected_finished_date_from, DateTime? expected_finished_date_to, DateTime? actual_finished_date_from, DateTime? actual_finished_date_to, String displayName, Int32? user_FK1, String gridLayout, Boolean? isPublic)
		{
			this.task_saved_search_PK = task_saved_search_PK;
			this.activity_FK = activity_FK;
			this.name = name;
			this.user_FK = user_FK;
			this.type_internal_status_FK = type_internal_status_FK;
			this.country_FK = country_FK;
			this.start_date_from = start_date_from;
			this.start_date_to = start_date_to;
			this.expected_finished_date_from = expected_finished_date_from;
			this.expected_finished_date_to = expected_finished_date_to;
			this.actual_finished_date_from = actual_finished_date_from;
			this.actual_finished_date_to = actual_finished_date_to;
            this.displayName = displayName;
			this.user_FK1 = user_FK1;
			this.gridLayout = gridLayout;
			this.isPublic = isPublic;
		}
	}

	public interface ITask_saved_search_PKOperations : ICRUDOperations<Task_saved_search_PK>
	{
        List<Task_saved_search_PK> GetEntitiesByUserOrPublic(Int32? user_fk);
	}
}
