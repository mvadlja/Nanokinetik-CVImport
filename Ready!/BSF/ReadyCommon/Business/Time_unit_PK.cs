// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	29.11.2011. 10:19:04
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TIME_UNIT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "TIME_UNIT")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Time_unit_PK
    {
        private Int32? _time_unit_PK;
        private Int32? _task_FK;
        private Int32? _user_FK;
        private Int32? _time_unit_name_FK;
        private String _description;
        private String _comment;
        private DateTime? _actual_date;
        private Int32? _time_hours;
        private Int32? _time_minutes;
        private Int32? _activity_FK;
        private Int32? _inserted_by;
        private String _name;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? time_unit_PK
        {
            get { return _time_unit_PK; }
            set { _time_unit_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? task_FK
        {
            get { return _task_FK; }
            set { _task_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? user_FK
        {
            get { return _user_FK; }
            set { _user_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? time_unit_name_FK
        {
            get { return _time_unit_name_FK; }
            set { _time_unit_name_FK = value; }
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

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? actual_date
        {
            get { return _actual_date; }
            set { _actual_date = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? time_hours
        {
            get { return _time_hours; }
            set { _time_hours = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? time_minutes
        {
            get { return _time_minutes; }
            set { _time_minutes = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? activity_FK
        {
            get { return _activity_FK; }
            set { _activity_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? inserted_by
        {
            get { return _inserted_by; }
            set { _inserted_by = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        public Time_unit_PK() { }
        public Time_unit_PK(Int32? time_unit_PK, Int32? task_FK, Int32? user_FK, Int32? time_unit_name_FK, String description, String comment, DateTime? actual_date, Int32? time_hours, Int32? time_minutes, Int32? activity_FK, Int32? inserted_by, String name)
        {
            this.time_unit_PK = time_unit_PK;
            this.task_FK = task_FK;
            this.user_FK = user_FK;
            this.time_unit_name_FK = time_unit_name_FK;
            this.description = description;
            this.comment = comment;
            this.actual_date = actual_date;
            this.time_hours = time_hours;
            this.time_minutes = time_minutes;
            this.activity_FK = activity_FK;
            this.inserted_by = inserted_by;
            this.Name = name;
        }
    }

    public interface ITime_unit_PKOperations : ICRUDOperations<Time_unit_PK>
    {
        DataSet GetTabMenuItemsCount(Int32 time_unit_PK);
        DataSet GetConnectedClients(int? project_PK);

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormSearchDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormGroupDataSet(DateTime? actualDate, Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions);
    }
}
