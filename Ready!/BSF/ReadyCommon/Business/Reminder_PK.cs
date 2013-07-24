// ======================================================================================================================
// Author:		KIKI-PC\Alan
// Create date:	20.6.2012. 14:33:48
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.REMINDER
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "REMINDER", Active = true)]
    //[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Reminder_PK
    {
        private Int32? _reminder_PK;
        private Int32? _user_FK;
        private Int32? _responsible_user_FK;
        private String _table_name;
        private Int32? _entity_FK;
        private String _related_attribute_name;
        private String _related_attribute_value;
        private String _reminder_name;
        private String _reminder_type;
        private String _navigate_url;
        private DateTime? _reminder_date;
        private Int64? _time_before_activation;
        private Boolean? _remind_me_on_email;
        private String _additional_emails;
        private String _description;
        private String _status;
        private Boolean? _is_automatic;
        private Int32? _related_entity_FK;
        private String _repeating_mode;
        private Int32? _reminder_date_PK;
        private Int32? _reminder_user_status_FK;
        private String _comment;
        private List<Reminder_date_PK> _reminderDates;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? reminder_PK
        {
            get { return _reminder_PK; }
            set { _reminder_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? user_FK
        {
            get { return _user_FK; }
            set { _user_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? responsible_user_FK
        {
            get { return _responsible_user_FK; }
            set { _responsible_user_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String table_name
        {
            get { return _table_name; }
            set { _table_name = value; }
        }

        public ReminderTableName TableName
        {
            get
            {
                ReminderTableName reminderTableName;
                if (!ReminderTableName.TryParse(_table_name, true, out reminderTableName))
                {
                    reminderTableName = ReminderTableName.NULL;
                }

                return reminderTableName;
            }

            set { _table_name = Enum.GetName(typeof(ReminderTableName), value); }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? entity_FK
        {
            get { return _entity_FK; }
            set { _entity_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String related_attribute_name
        {
            get { return _related_attribute_name; }
            set { _related_attribute_name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String related_attribute_value
        {
            get { return _related_attribute_value; }
            set { _related_attribute_value = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String reminder_name
        {
            get { return _reminder_name; }
            set { _reminder_name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String reminder_type
        {
            get { return _reminder_type; }
            set { _reminder_type = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String navigate_url
        {
            get { return _navigate_url; }
            set { _navigate_url = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? reminder_date
        {
            get { return _reminder_date; }
            set { _reminder_date = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int64)]
        public Int64? time_before_activation
        {
            get { return _time_before_activation; }
            set { _time_before_activation = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? remind_me_on_email
        {
            get { return _remind_me_on_email; }
            set { _remind_me_on_email = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String additional_emails
        {
            get { return _additional_emails; }
            set { _additional_emails = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String description
        {
            get { return _description; }
            set { _description = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String status
        {
            get { return _status; }
            set { _status = value; }
        }

        public ReminderStatus Status
        {
            get
            {
                ReminderStatus reminderStatus;
                if (!ReminderStatus.TryParse(_status, true, out reminderStatus))
                {
                    reminderStatus = ReminderStatus.NULL;
                }

                return reminderStatus;
            }

            set { _status = Enum.GetName(typeof(ReminderStatus), value); }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? is_automatic
        {
            get { return _is_automatic; }
            set { _is_automatic = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? related_entity_FK
        {
            get { return _related_entity_FK; }
            set { _related_entity_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String repeating_mode
        {
            get { return _repeating_mode; }
            set { _repeating_mode = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? reminder_date_PK
        {
            get { return _reminder_date_PK; }
            set { _reminder_date_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? reminder_user_status_FK
        {
            get { return _reminder_user_status_FK; }
            set { _reminder_user_status_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        public List<Reminder_date_PK> ReminderDates
        {
            get { return _reminderDates; }
            set { _reminderDates = value; }
        }

        #endregion

        public Reminder_PK()
        {
            ReminderDates = new List<Reminder_date_PK>();
        }
        public Reminder_PK(Int32? reminder_PK, Int32? user_FK, Int32? responsible_user_FK, String table_name, Int32? entity_FK, String related_attribute_name, String related_attribute_value, String reminder_name, String reminder_type, String navigate_url, DateTime? reminder_date, Int64? time_before_activation, Boolean? remind_me_on_email, String additional_emails, String description, String status, Boolean? is_automatic, Int32? related_entity_FK, String repeating_mode, Int32? reminder_date_PK, Int32? reminder_user_status_FK, String comment)
        {
            this.reminder_PK = reminder_PK;
            this.user_FK = user_FK;
            this.responsible_user_FK = responsible_user_FK;
            this.table_name = table_name;
            this.entity_FK = entity_FK;
            this.related_attribute_name = related_attribute_name;
            this.related_attribute_value = related_attribute_value;
            this.reminder_name = reminder_name;
            this.reminder_type = reminder_type;
            this.navigate_url = navigate_url;
            this.reminder_date = reminder_date;
            this.time_before_activation = time_before_activation;
            this.remind_me_on_email = remind_me_on_email;
            this.additional_emails = additional_emails;
            this.description = description;
            this.status = status;
            this.is_automatic = is_automatic;
            this.related_entity_FK = related_entity_FK;
            this.repeating_mode = repeating_mode;
            this.reminder_date_PK = reminder_date_PK;
            this.reminder_user_status_FK = reminder_user_status_FK;
            this.comment = comment;

            ReminderDates = new List<Reminder_date_PK>();
        }
    }

    public interface IReminder_PKOperations : ICRUDOperations<Reminder_PK>
    {
        bool DoesAutomaticReminderAlreadyExists(string table_name, int? entity_FK, string related_attribute_name, DateTime? reminder_date);
        DataSet GetActiveRemindersForUser(Int32 user_PK);
        void DismissReminder(Int32 reminder_PK);
        DataSet GetEntitiesByResponsibleUser(Int32? responsible_user_FK, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        List<Reminder_PK> GetEntitiesReadyForEmail();
        void SetReminderStatus(Int32 reminder_date_FK, Int32 reminder_status_FK);
        void DeleteOldDismissedAutomaticReminders(DateTime remider_date);
        bool DoesActiveReminderExists(Int32? user_PK, String table_name, Int32? entity_FK, String related_attribute_name);
        DataSet GetTabMenuItemsCount(Int32? reminderPk);

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormSearchDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
    }

    public enum ReminderTableName
    {
        NULL,
        AUTHORISED_PRODUCT,
        PHARMACEUTICAL_PRODUCT,
        ACTIVITY,
        PRODUCT,
        PROJECT,
        TASK,
        DOCUMENT
    }

    public enum ReminderStatus
    {
        NULL,
        Active,
        EmailSent,
        Dismissed,
    }

    public enum ReminderRepeatingMode
    {
        Null,
        None,
        Daily,
        Weekly,
        Monthly,
        Yearly
    }

    public enum ReminderUserStatus
    {
        NULL,
        Done,
        Canceled,
        Pending,
        OverDue
    }
}
