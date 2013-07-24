// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	19.12.2011. 16:10:10
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SUBMISSION_UNIT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "SUBMISSION_UNIT")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Subbmission_unit_PK
	{
		private Int32? _subbmission_unit_PK;
		private Int32? _task_FK;
		private String _submission_ID;
		private Int32? _agency_role_FK;
		private String _comment;
		private Int32? _s_format_FK;
		private Int32? _description_type_FK;
		private DateTime? _dispatch_date;
		private DateTime? _receipt_date;
		private String _sequence;
		private String _dtd_schema_FK;
		private Int32? _organization_agency_FK;
		private Int32? _document_FK;
		private Int32? _ness_FK;
		private Int32? _ectd_FK;
        private Int32? _person_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? subbmission_unit_PK
		{
			get { return _subbmission_unit_PK; }
			set { _subbmission_unit_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? task_FK
		{
			get { return _task_FK; }
			set { _task_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String submission_ID
		{
			get { return _submission_ID; }
			set { _submission_ID = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? agency_role_FK
		{
			get { return _agency_role_FK; }
			set { _agency_role_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String comment
		{
			get { return _comment; }
			set { _comment = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? s_format_FK
		{
			get { return _s_format_FK; }
			set { _s_format_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? description_type_FK
		{
			get { return _description_type_FK; }
			set { _description_type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? dispatch_date
		{
			get { return _dispatch_date; }
			set { _dispatch_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? receipt_date
		{
			get { return _receipt_date; }
			set { _receipt_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String sequence
		{
			get { return _sequence; }
			set { _sequence = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String dtd_schema_FK
		{
			get { return _dtd_schema_FK; }
			set { _dtd_schema_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? organization_agency_FK
		{
			get { return _organization_agency_FK; }
			set { _organization_agency_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? document_FK
		{
			get { return _document_FK; }
			set { _document_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ness_FK
		{
			get { return _ness_FK; }
			set { _ness_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ectd_FK
		{
			get { return _ectd_FK; }
			set { _ectd_FK = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? person_FK
        {
            get { return _person_FK; }
            set { _person_FK = value; }
        }

		#endregion

		public Subbmission_unit_PK() { }
		public Subbmission_unit_PK(Int32? subbmission_unit_PK, Int32? task_FK, String submission_ID, Int32? agency_role_FK, String comment, Int32? s_format_FK, Int32? description_type_FK, DateTime? dispatch_date, DateTime? receipt_date, String sequence, String dtd_schema_FK, Int32? organization_agency_FK, Int32? document_FK, Int32? ness_FK, Int32? ectd_FK, Int32? person_FK)
		{
			this.subbmission_unit_PK = subbmission_unit_PK;
			this.task_FK = task_FK;
			this.submission_ID = submission_ID;
			this.agency_role_FK = agency_role_FK;
			this.comment = comment;
			this.s_format_FK = s_format_FK;
			this.description_type_FK = description_type_FK;
			this.dispatch_date = dispatch_date;
			this.receipt_date = receipt_date;
			this.sequence = sequence;
			this.dtd_schema_FK = dtd_schema_FK;
			this.organization_agency_FK = organization_agency_FK;
			this.document_FK = document_FK;
			this.ness_FK = ness_FK;
			this.ectd_FK = ectd_FK;
            this.person_FK = person_FK;
		}
	}

	public interface ISubbmission_unit_PKOperations : ICRUDOperations<Subbmission_unit_PK>
	{
        DataSet GetTabMenuItemsCount(Int32 su_PK);

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormSearchDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);

        bool AbleToDeleteEntity(int submissionUnitPk);
    }
}
