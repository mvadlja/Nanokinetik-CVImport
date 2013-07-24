// ======================================================================================================================
// Author:		TomoZ560\Tomo
// Create date:	5.12.2011. 23:21:03
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PROJECT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PROJECT")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Project_PK
    {
        private Int32? _project_PK;
        private Int32? _user_FK;
        private String _name;
        private String _comment;
        private DateTime? _start_date;
        private DateTime? _expected_finished_date;
        private DateTime? _actual_finished_date;
        private String _description;
        private Int32? _internal_status_type_FK;
        private bool _automatic_alerts_on;
        private bool _prevent_start_date_alert;
        private bool _prevent_exp_finish_date_alert;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? project_PK
        {
            get { return _project_PK; }
            set { _project_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? user_FK
        {
            get { return _user_FK; }
            set { _user_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String comment
        {
            get { return _comment; }
            set { _comment = value; }
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

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? internal_status_type_FK
        {
            get { return _internal_status_type_FK; }
            set { _internal_status_type_FK = value; }
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

        #endregion

        public Project_PK() { }
        public Project_PK(Int32? project_PK, Int32? user_FK, String name, String comment, DateTime? start_date, DateTime? expected_finished_date, DateTime? actual_finished_date, String description, Int32? internal_status_type_FK, bool automatic_alerts_on, bool prevent_start_date_alert, bool prevent_exp_finish_date_alert)
        {
            this.project_PK = project_PK;
            this.user_FK = user_FK;
            this.name = name;
            this.comment = comment;
            this.start_date = start_date;
            this.expected_finished_date = expected_finished_date;
            this.actual_finished_date = actual_finished_date;
            this.description = description;
            this.internal_status_type_FK = internal_status_type_FK;
            this.automatic_alerts_on = automatic_alerts_on;
            this.prevent_start_date_alert = prevent_start_date_alert;
            this.prevent_exp_finish_date_alert = prevent_exp_finish_date_alert;
        }
    }

    public interface IProject_PKOperations : ICRUDOperations<Project_PK>
    {
        DataSet GetPPSearcher(String name,String internalStatus, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetTabMenuItemsCount(Int32 project_PK, int? personFk);

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormSearchDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        bool AbleToDeleteEntity(int projectPk);

        List<Project_PK> GetAvailableEntitiesByActivity(int activityPk);
        List<Project_PK> GetAssignedEntitiesByActivity(int activityPk);
    }
}
