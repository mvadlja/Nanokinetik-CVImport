// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	24.2.2012. 13:14:01
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.DOCUMENT_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "DOCUMENT_SAVED_SEARCH")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Document_saved_search_PK
	{
        private Int32? _document_saved_search_PK;
        private Int32? _product_FK;
        private Int32? _ap_FK;
        private Int32? _project_FK;
        private Int32? _activity_FK;
        private Int32? _task_FK;
        private String _name;
        private Int32? _type_FK;
        private Int32? _version_number;
        private Int32? _version_label;
        private String _document_number;
        private Int32? _person_FK;
        private Int32? _regulatory_status;
        private DateTime? _change_date_from;
        private DateTime? _change_date_to;
        private DateTime? _effective_start_date_from;
        private DateTime? _effective_start_date_to;
        private DateTime? _effective_end_date_from;
        private DateTime? _effective_end_date_to;
        private String _displayName;
        private Int32? _user_FK1;
        private String _gridLayout;
        private Boolean? _isPublic;
        private Int32? _pp_FK;
        private DateTime? _version_date_from;
        private DateTime? _version_date_to;
        private String _ev_code;
        private String _content;
        private String _language_code;
        private String _comments;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? document_saved_search_PK
        {
            get { return _document_saved_search_PK; }
            set { _document_saved_search_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? product_FK
        {
            get { return _product_FK; }
            set { _product_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? ap_FK
        {
            get { return _ap_FK; }
            set { _ap_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? project_FK
        {
            get { return _project_FK; }
            set { _project_FK = value; }
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

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? type_FK
        {
            get { return _type_FK; }
            set { _type_FK = value; }
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

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String document_number
        {
            get { return _document_number; }
            set { _document_number = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? person_FK
        {
            get { return _person_FK; }
            set { _person_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? regulatory_status
        {
            get { return _regulatory_status; }
            set { _regulatory_status = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? change_date_from
        {
            get { return _change_date_from; }
            set { _change_date_from = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? change_date_to
        {
            get { return _change_date_to; }
            set { _change_date_to = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? effective_start_date_from
        {
            get { return _effective_start_date_from; }
            set { _effective_start_date_from = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? effective_start_date_to
        {
            get { return _effective_start_date_to; }
            set { _effective_start_date_to = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? effective_end_date_from
        {
            get { return _effective_end_date_from; }
            set { _effective_end_date_from = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? effective_end_date_to
        {
            get { return _effective_end_date_to; }
            set { _effective_end_date_to = value; }
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

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? pp_FK
        {
            get { return _pp_FK; }
            set { _pp_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? version_date_from
        {
            get { return _version_date_from; }
            set { _version_date_from = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? version_date_to
        {
            get { return _version_date_to; }
            set { _version_date_to = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ev_code
        {
            get { return _ev_code; }
            set { _ev_code = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String content
        {
            get { return _content; }
            set { _content = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String language_code
        {
            get { return _language_code; }
            set { _language_code = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        #endregion

        public Document_saved_search_PK() { }
        public Document_saved_search_PK(Int32? document_saved_search_PK, Int32? product_FK, Int32? ap_FK, Int32? project_FK, Int32? activity_FK, Int32? task_FK, String name, Int32? type_FK, Int32? version_number, Int32? version_label, String document_number, Int32? person_FK, Int32? regulatory_status, DateTime? change_date_from, DateTime? change_date_to, DateTime? effective_start_date_from, DateTime? effective_start_date_to, DateTime? effective_end_date_from, DateTime? effective_end_date_to, String displayName, Int32? user_FK1, String gridLayout, Boolean? isPublic, Int32? pp_FK, DateTime? version_date_from, DateTime? version_date_to, String ev_code, String content, String language_code, String comments)
        {
            this.document_saved_search_PK = document_saved_search_PK;
            this.product_FK = product_FK;
            this.ap_FK = ap_FK;
            this.project_FK = project_FK;
            this.activity_FK = activity_FK;
            this.task_FK = task_FK;
            this.name = name;
            this.type_FK = type_FK;
            this.version_number = version_number;
            this.version_label = version_label;
            this.document_number = document_number;
            this.person_FK = person_FK;
            this.regulatory_status = regulatory_status;
            this.change_date_from = change_date_from;
            this.change_date_to = change_date_to;
            this.effective_start_date_from = effective_start_date_from;
            this.effective_start_date_to = effective_start_date_to;
            this.effective_end_date_from = effective_end_date_from;
            this.effective_end_date_to = effective_end_date_to;
            this.displayName = displayName;
            this.user_FK1 = user_FK1;
            this.gridLayout = gridLayout;
            this.isPublic = isPublic;
            this.pp_FK = pp_FK;
            this.version_date_from = version_date_from;
            this.version_date_to = version_date_to;
            this.ev_code = ev_code;
            this.content = content;
            this.language_code = language_code;
            this.comments = comments;
        }
    }

	public interface IDocument_saved_search_PKOperations : ICRUDOperations<Document_saved_search_PK>
	{
        List<Document_saved_search_PK> GetEntitiesByUserOrPublic(Int32? user_fk);
	}
}
