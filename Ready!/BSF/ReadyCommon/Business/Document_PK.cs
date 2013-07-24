// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	20.10.2011. 10:05:30
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.DOCUMENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "DOCUMENT", Active=true)]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Document_PK
	{
        public enum CalculatedColumn
        {
            All,
            LanguageCodes,
            Attachments,
            RelatedEntites
        }

		private Int32? _document_PK;
		private Int32? _person_FK;
		private Int32? _type_FK;
		private String _name;
		private String _description;
		private String _comment;
		private String _document_code;
		private Int32? _regulatory_status;
		private Int32? _version_number;
		private Int32? _version_label;
		private DateTime? _change_date;
		private DateTime? _effective_start_date;
		private DateTime? _effective_end_date;
		private DateTime? _version_date;
		private String _localnumber;
		private String _version_date_format;
		private String _attachment_name;
        private Int32? _attachment_type_FK;
        private Int32? _EVCODE;
        private String _EDMSDocumentId;
        private String _EDMSBindingRule;
        private String _EDMSVersionNumber;
        private DateTime? _EDMSModifyDate;
        private Boolean? _EDMSDocument;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? document_PK
		{
			get { return _document_PK; }
			set { _document_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? person_FK
		{
			get { return _person_FK; }
			set { _person_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? type_FK
		{
			get { return _type_FK; }
			set { _type_FK = value; }
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

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String document_code
		{
			get { return _document_code; }
			set { _document_code = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? regulatory_status
		{
			get { return _regulatory_status; }
			set { _regulatory_status = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? version_number
		{
			get { return _version_number; }
			set { _version_number = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? version_label
		{
			get { return _version_label; }
			set { _version_label = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? change_date
		{
			get { return _change_date; }
			set { _change_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? effective_start_date
		{
			get { return _effective_start_date; }
			set { _effective_start_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? effective_end_date
		{
			get { return _effective_end_date; }
			set { _effective_end_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? version_date
		{
			get { return _version_date; }
			set { _version_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String localnumber
		{
			get { return _localnumber; }
			set { _localnumber = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String version_date_format
		{
			get { return _version_date_format; }
			set { _version_date_format = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String attachment_name
		{
			get { return _attachment_name; }
			set { _attachment_name = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? attachment_type_FK
        {
            get { return _attachment_type_FK; }
            set { _attachment_type_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? EVCODE
        {
            get { return _EVCODE; }
            set { _EVCODE = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String EDMSBindingRule
        {
            get { return _EDMSBindingRule; }
            set { _EDMSBindingRule = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? EDMSModifyDate
        {
            get { return _EDMSModifyDate; }
            set { _EDMSModifyDate = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String EDMSDocumentId
        {
            get { return _EDMSDocumentId; }
            set { _EDMSDocumentId = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String EDMSVersionNumber
        {
            get { return _EDMSVersionNumber; }
            set { _EDMSVersionNumber = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? EDMSDocument
        {
            get { return _EDMSDocument; }
            set { _EDMSDocument = value; }
        }

		#endregion

		public Document_PK() { }
        public Document_PK(Int32? document_PK, Int32? person_FK, Int32? type_FK, String name, String description, String comment, String document_code, Int32? regulatory_status, Int32? version_number, Int32? version_label, DateTime? change_date, DateTime? effective_start_date, DateTime? effective_end_date, DateTime? version_date, String localnumber, String version_date_format, String attachment_name, Int32? attachment_type_FK, Int32? EVCODE, String EDMSBindingRule, DateTime? EDMSModifyDate, String EDMSDocumentId, String EDMSVersionNumber, Boolean? EDMSDocument)
		{
			this.document_PK = document_PK;
			this.person_FK = person_FK;
			this.type_FK = type_FK;
			this.name = name;
			this.description = description;
			this.comment = comment;
			this.document_code = document_code;
			this.regulatory_status = regulatory_status;
			this.version_number = version_number;
			this.version_label = version_label;
			this.change_date = change_date;
			this.effective_start_date = effective_start_date;
			this.effective_end_date = effective_end_date;
			this.version_date = version_date;
			this.localnumber = localnumber;
			this.version_date_format = version_date_format;
			this.attachment_name = attachment_name;
            this._attachment_type_FK = attachment_type_FK;
            this._EVCODE = EVCODE;
            this.EDMSDocumentId = EDMSDocumentId;
            this.EDMSBindingRule = EDMSBindingRule;
            this.EDMSModifyDate = EDMSModifyDate;
            this.EDMSVersionNumber = EDMSVersionNumber;
            this.EDMSDocument = EDMSDocument;
		}
	}

	public interface IDocument_PKOperations : ICRUDOperations<Document_PK>
	{
        List<Document_PK> GetDocumentsByAP(Int32 ap_FK, string documentTypeName = null);
        List<Document_PK> GetDocumentsByProduct(Int32 product_FK);
        DataSet GetDocumentRelatedEntities(Int32 doc_PK);
        DataSet GetEDMSRelatedEntities(string EDMSDocumentId);
        
        DataSet GetTabMenuItemsCount(Int32 document_PK, int? personFk);

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormSearchDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);

        DataSet GetDocumentSearcher(String name, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);

        DataSet GetListFormDataSetEDMS(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);

        void UpdateCalculatedColumn(int documentPk, Document_PK.CalculatedColumn calculatedColumn);
	}
}
