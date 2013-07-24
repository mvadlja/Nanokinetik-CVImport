// ======================================================================================================================
// Author:		POSSBOOK-DV7\Hrvoje
// Create date:	10.1.2013. 10:34:03
// Description:	GEM2 Generated class for table ReadyDev.dbo.REMINDER_DATES
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "ReadyDev", Table = "REMINDER_DATES")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Reminder_date_PK
    {
        private Int32? _reminder_date_PK;
        private DateTime? _reminder_date;
        private Int32? _reminder_repeating_mode_FK;
        private Int32? _reminder_status_FK;
        private Int32? _reminder_FK;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? reminder_date_PK
        {
            get { return _reminder_date_PK; }
            set { _reminder_date_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? reminder_date
        {
            get { return _reminder_date; }
            set { _reminder_date = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? reminder_repeating_mode_FK
        {
            get { return _reminder_repeating_mode_FK; }
            set { _reminder_repeating_mode_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? reminder_status_FK
        {
            get { return _reminder_status_FK; }
            set { _reminder_status_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? reminder_FK
        {
            get { return _reminder_FK; }
            set { _reminder_FK = value; }
        }

        #endregion

        public Reminder_date_PK() { }
        public Reminder_date_PK(Int32? reminder_date_PK, DateTime? reminder_date, Int32? reminder_repeating_mode_FK, Int32? reminder_status_FK, Int32? reminder_FK)
        {
            this.reminder_date_PK = reminder_date_PK;
            this.reminder_date = reminder_date;
            this.reminder_repeating_mode_FK = reminder_repeating_mode_FK;
            this.reminder_status_FK = reminder_status_FK;
            this.reminder_FK = reminder_FK;
        }
    }

    public interface IReminder_date_PKOperations : ICRUDOperations<Reminder_date_PK>
    {
        List<Reminder_date_PK> GetEntitiesByReminder(Int32? reminder_FK);
    }
}
