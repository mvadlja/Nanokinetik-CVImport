// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.APPROVED_SUBSTANCE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "APPROVED_SUBSTANCE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Approved_substance_PK
	{
		private Int32? _approved_substance_PK;
		private Int32? _operationtype;
		private Int32? _virtual;
		private String _localnumber;
		private String _ev_code;
		private String _sourcecode;
		private Int32? _resolutionmode;
		private String _substancename;
		private String _casnumber;
		private String _molecularformula;
		private Int32? _class;
		private String _cbd;
		private Int32? _substancetranslations_FK;
		private Int32? _substancealiases_FK;
		private Int32? _internationalcodes_FK;
		private Int32? _previous_ev_codes_FK;
		private Int32? _substancessis_FK;
		private Int32? _substance_attachment_FK;
		private String _comments;
    
		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? approved_substance_PK
		{
			get { return _approved_substance_PK; }
			set { _approved_substance_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? operationtype
		{
			get { return _operationtype; }
			set { _operationtype = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? Virtual
		{
			get { return _virtual; }
			set { _virtual = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String localnumber
		{
			get { return _localnumber; }
			set { _localnumber = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ev_code
		{
			get { return _ev_code; }
			set { _ev_code = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String sourcecode
		{
			get { return _sourcecode; }
			set { _sourcecode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? resolutionmode
		{
			get { return _resolutionmode; }
			set { _resolutionmode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String substancename
		{
			get { return _substancename; }
			set { _substancename = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String casnumber
		{
			get { return _casnumber; }
			set { _casnumber = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String molecularformula
		{
			get { return _molecularformula; }
			set { _molecularformula = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? Class
		{
			get { return _class; }
			set { _class = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String cbd
		{
			get { return _cbd; }
			set { _cbd = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substancetranslations_FK
		{
			get { return _substancetranslations_FK; }
			set { _substancetranslations_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substancealiases_FK
		{
			get { return _substancealiases_FK; }
			set { _substancealiases_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? internationalcodes_FK
		{
			get { return _internationalcodes_FK; }
			set { _internationalcodes_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? previous_ev_codes_FK
		{
			get { return _previous_ev_codes_FK; }
			set { _previous_ev_codes_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substancessis_FK
		{
			get { return _substancessis_FK; }
			set { _substancessis_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substance_attachment_FK
		{
			get { return _substance_attachment_FK; }
			set { _substance_attachment_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String comments
		{
			get { return _comments; }
			set { _comments = value; }
		}

		#endregion

		public Approved_substance_PK() { }
		public Approved_substance_PK(Int32? approved_substance_PK, Int32? operationtype, Int32? Virtual, String localnumber, String ev_code, String sourcecode, Int32? resolutionmode, String substancename, String casnumber, String molecularformula, Int32? Class, String cbd, Int32? substancetranslations_FK, Int32? substancealiases_FK, Int32? internationalcodes_FK, Int32? previous_ev_codes_FK, Int32? substancessis_FK, Int32? substance_attachment_FK, String comments)
		{
			this.approved_substance_PK = approved_substance_PK;
			this.operationtype = operationtype;
			this.Virtual = Virtual;
			this.localnumber = localnumber;
			this.ev_code = ev_code;
			this.sourcecode = sourcecode;
			this.resolutionmode = resolutionmode;
			this.substancename = substancename;
			this.casnumber = casnumber;
			this.molecularformula = molecularformula;
			this.Class = Class;
			this.cbd = cbd;
			this.substancetranslations_FK = substancetranslations_FK;
			this.substancealiases_FK = substancealiases_FK;
			this.internationalcodes_FK = internationalcodes_FK;
			this.previous_ev_codes_FK = previous_ev_codes_FK;
			this.substancessis_FK = substancessis_FK;
			this.substance_attachment_FK = substance_attachment_FK;
			this.comments = comments;
		}
	}

	public interface IApproved_substance_PKOperations : ICRUDOperations<Approved_substance_PK>
    {
        DataSet GetApprovedSubstance(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
    }
}
