// ======================================================================================================================
// Author:		POSSBOOK-DV7\Hrvoje
// Create date:	7.3.2013. 15:44:37
// Description:	GEM2 Generated class for table ReadyDev.dbo.ALERT_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "ReadyDev", Table = "ALERT_SAVED_SEARCH")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Alert_saved_search_PK
    {
        private Int32? _alert_saved_search_PK;
        private Int32? _product_FK;
        private Int32? _ap_FK;
        private Int32? _project_FK;
        private Int32? _activity_FK;
        private Int32? _task_FK;
        private Int32? _document_FK;
        private String _gridLayout;
        private Boolean? _isPublic;
        private String _name;
        private Int32? _reminder_repeating_mode_FK;
        private Boolean? _send_mail;
        private String _displayName;
        private Int32? _user_FK;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? alert_saved_search_PK
        {
            get { return _alert_saved_search_PK; }
            set { _alert_saved_search_PK = value; }
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

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? document_FK
        {
            get { return _document_FK; }
            set { _document_FK = value; }
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

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? reminder_repeating_mode_FK
        {
            get { return _reminder_repeating_mode_FK; }
            set { _reminder_repeating_mode_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? send_mail
        {
            get { return _send_mail; }
            set { _send_mail = value; }
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

        #endregion

        public Alert_saved_search_PK() { }
        public Alert_saved_search_PK(Int32? alert_saved_search_PK, Int32? product_FK, Int32? ap_FK, Int32? project_FK, Int32? activity_FK, Int32? task_FK, Int32? document_FK, String gridLayout, Boolean? isPublic, String name, Int32? reminder_repeating_mode_FK, Boolean? send_mail, String displayName, Int32? user_FK)
        {
            this.alert_saved_search_PK = alert_saved_search_PK;
            this.product_FK = product_FK;
            this.ap_FK = ap_FK;
            this.project_FK = project_FK;
            this.activity_FK = activity_FK;
            this.task_FK = task_FK;
            this.document_FK = document_FK;
            this.gridLayout = gridLayout;
            this.isPublic = isPublic;
            this.name = name;
            this.reminder_repeating_mode_FK = reminder_repeating_mode_FK;
            this.send_mail = send_mail;
            this.displayName = displayName;
            this.user_FK = user_FK;
        }
    }

    public interface IAlert_saved_search_PKOperations : ICRUDOperations<Alert_saved_search_PK>
    {

    }
}
