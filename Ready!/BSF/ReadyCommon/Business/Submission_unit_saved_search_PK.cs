// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	24.2.2012. 13:19:06
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SUBMISSION_UNIT_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "SUBMISSION_UNIT_SAVED_SEARCH")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Submission_unit_saved_search_PK
	{
		private Int32? _submission_unit_saved_search_PK;
		private Int32? _product_FK;
		private Int32? _activity_FK;
		private Int32? _task_FK;
		private Int32? _description_type_FK;
		private Int32? _agency_FK;
		private Int32? _rms_FK;
		private String _submission_ID;
		private Int32? _s_format_FK;
		private String _sequence;
        private Int32? _dtd_schema_FK;
		private DateTime? _dispatch_date_from;
		private DateTime? _dispatch_date_to;
		private DateTime? _receipt_date_from;
		private DateTime? _receipt_to;
		private String _displayName;
		private Int32? _user_FK;
		private String _gridLayout;
		private Boolean? _isPublic;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? submission_unit_saved_search_PK
		{
			get { return _submission_unit_saved_search_PK; }
			set { _submission_unit_saved_search_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? activity_FK
		{
			get { return _activity_FK; }
			set { _activity_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? task_FK
		{
			get { return _task_FK; }
			set { _task_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? description_type_FK
		{
			get { return _description_type_FK; }
			set { _description_type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? agency_FK
		{
			get { return _agency_FK; }
			set { _agency_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? rms_FK
		{
			get { return _rms_FK; }
			set { _rms_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String submission_ID
		{
			get { return _submission_ID; }
			set { _submission_ID = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? s_format_FK
		{
			get { return _s_format_FK; }
			set { _s_format_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String sequence
		{
			get { return _sequence; }
			set { _sequence = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? dtd_schema_FK
		{
			get { return _dtd_schema_FK; }
			set { _dtd_schema_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? dispatch_date_from
		{
			get { return _dispatch_date_from; }
			set { _dispatch_date_from = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? dispatch_date_to
		{
			get { return _dispatch_date_to; }
			set { _dispatch_date_to = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? receipt_date_from
		{
			get { return _receipt_date_from; }
			set { _receipt_date_from = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? receipt_to
		{
			get { return _receipt_to; }
			set { _receipt_to = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String displayName
		{
			get { return _displayName; }
			set { _displayName = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_FK
		{
			get { return _user_FK; }
			set { _user_FK = value; }
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

		public Submission_unit_saved_search_PK() { }
        public Submission_unit_saved_search_PK(Int32? submission_unit_saved_search_PK, Int32? product_FK, Int32? activity_FK, Int32? task_FK, Int32? description_type_FK, Int32? agency_FK, Int32? rms_FK, String submission_ID, Int32? s_format_FK, String sequence, Int32? dtd_schema_FK, DateTime? dispatch_date_from, DateTime? dispatch_date_to, DateTime? receipt_date_from, DateTime? receipt_to, String displayName, Int32? user_FK, String gridLayout, Boolean? isPublic)
		{
			this.submission_unit_saved_search_PK = submission_unit_saved_search_PK;
			this.product_FK = product_FK;
			this.activity_FK = activity_FK;
			this.task_FK = task_FK;
			this.description_type_FK = description_type_FK;
			this.agency_FK = agency_FK;
			this.rms_FK = rms_FK;
			this.submission_ID = submission_ID;
			this.s_format_FK = s_format_FK;
			this.sequence = sequence;
			this.dtd_schema_FK = dtd_schema_FK;
			this.dispatch_date_from = dispatch_date_from;
			this.dispatch_date_to = dispatch_date_to;
			this.receipt_date_from = receipt_date_from;
			this.receipt_to = receipt_to;
            this.displayName = displayName;
			this.user_FK = user_FK;
			this.gridLayout = gridLayout;
			this.isPublic = isPublic;
		}
	}

	public interface ISubmission_unit_saved_search_PKOperations : ICRUDOperations<Submission_unit_saved_search_PK>
	{
        List<Submission_unit_saved_search_PK> GetEntitiesByUserOrPublic(Int32? user_fk);
	}
}
