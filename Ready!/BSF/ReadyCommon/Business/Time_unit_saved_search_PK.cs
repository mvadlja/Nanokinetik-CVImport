// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	24.2.2012. 13:42:31
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TIME_UNIT_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "TIME_UNIT_SAVED_SEARCH")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Time_unit_saved_search_PK
	{
		private Int32? _time_unit_saved_search_PK;
		private Int32? _activity_FK;
		private Int32? _time_unit_FK;
		private Int32? _user_FK;
		private DateTime? _actual_date_from;
		private DateTime? _actual_date_to;
		private String _displayName;
		private Int32? _user_FK1;
		private String _gridLayout;
		private Boolean? _isPublic;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? time_unit_saved_search_PK
		{
			get { return _time_unit_saved_search_PK; }
			set { _time_unit_saved_search_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? activity_FK
		{
			get { return _activity_FK; }
			set { _activity_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? time_unit_FK
		{
			get { return _time_unit_FK; }
			set { _time_unit_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_FK
		{
			get { return _user_FK; }
			set { _user_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? actual_date_from
		{
			get { return _actual_date_from; }
			set { _actual_date_from = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? actual_date_to
		{
			get { return _actual_date_to; }
			set { _actual_date_to = value; }
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

		public Time_unit_saved_search_PK() { }
        public Time_unit_saved_search_PK(Int32? time_unit_saved_search_PK, Int32? activity_FK, Int32? time_unit_FK, Int32? user_FK, DateTime? actual_date_from, DateTime? actual_date_to, String displayName, Int32? user_FK1, String gridLayout, Boolean? isPublic)
		{
			this.time_unit_saved_search_PK = time_unit_saved_search_PK;
			this.activity_FK = activity_FK;
			this.time_unit_FK = time_unit_FK;
			this.user_FK = user_FK;
			this.actual_date_from = actual_date_from;
			this.actual_date_to = actual_date_to;
            this.displayName = displayName;
			this.user_FK1 = user_FK1;
			this.gridLayout = gridLayout;
			this.isPublic = isPublic;
		}
	}

	public interface ITime_unit_saved_search_PKOperations : ICRUDOperations<Time_unit_saved_search_PK>
	{
        List<Time_unit_saved_search_PK> GetEntitiesByUserOrPublic(Int32? user_fk);
	}
}
