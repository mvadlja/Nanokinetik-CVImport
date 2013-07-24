// ======================================================================================================================
// Author:		KIKI-PC\Alan
// Create date:	20.6.2012. 14:33:48
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.REMINDER
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Reminder_PKDAL : GEMDataAccess<Reminder_PK>, IReminder_PKOperations
	{
		public Reminder_PKDAL() : base() { }
		public Reminder_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IReminder_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_GetTabMenuItemsCount", OperationType = GEMOperationType.Select)]
        public DataSet GetTabMenuItemsCount(Int32? reminderPk)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ReminderPk", reminderPk, DbType.Int32, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_REMINDER_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
        public DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            var methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;

            try
            {
                // Generating order by clause
                var orderBy = base.CreateOrderByClause(orderByConditions);
                var parameters = (from pair in filters where !String.IsNullOrWhiteSpace(pair.Value) select new GEMDbParameter(pair.Key, pair.Value, DbType.String, ParameterDirection.Input)).ToList();

                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                Dictionary<string, object> outputValues;

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                if (outputValues.ContainsKey("totalRecordsCount"))
                {
                    totalRecordsCount = (int)outputValues["totalRecordsCount"];
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_DoesAutomaticReminderAlreadyExists", OperationType = GEMOperationType.Complex)]
        public bool DoesAutomaticReminderAlreadyExists(string table_name, int? entity_FK, string related_attribute_name, DateTime? reminder_date)
        {
            DateTime methodStart = DateTime.Now;

            int? result = 0;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("table_name", table_name, DbType.String, ParameterDirection.Input));
                if (entity_FK != null) parameters.Add(new GEMDbParameter("entity_FK", entity_FK, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("related_attribute_name", related_attribute_name, DbType.String, ParameterDirection.Input));
                if (reminder_date.HasValue) parameters.Add(new GEMDbParameter("reminder_date", reminder_date, DbType.DateTime, ParameterDirection.Input));

                result = (int?)base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return result != null && result > 0 ? true : false;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_GetActiveRemindersForUser", OperationType = GEMOperationType.Complex)]
        public DataSet GetActiveRemindersForUser(Int32 user_PK)
        {
            DateTime methodStart = DateTime.Now;
            
            DataSet ds = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("user_PK", user_PK, DbType.Int32, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_GetActiveRemindersForUserEntity", OperationType = GEMOperationType.Complex)]
        public bool DoesActiveReminderExists(Int32? user_PK, String table_name, Int32? entity_FK, String related_attribute_name)
        {
            DateTime methodStart = DateTime.Now;

            bool result = false;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (user_PK.HasValue) parameters.Add(new GEMDbParameter("user_PK", user_PK, DbType.Int32, ParameterDirection.Input));
                if (table_name != null) parameters.Add(new GEMDbParameter("table_name", table_name, DbType.String, ParameterDirection.Input));
                if (entity_FK.HasValue) parameters.Add(new GEMDbParameter("entity_FK", entity_FK, DbType.Int32, ParameterDirection.Input));
                if (related_attribute_name != null) parameters.Add(new GEMDbParameter("related_attribute_name", related_attribute_name, DbType.String, ParameterDirection.Input));

                int? numReminders = (int?)base.ExecuteProcedureReturnScalar(parameters);

                result = numReminders > 0 ? true : false;

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return result;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_DismissReminder", OperationType = GEMOperationType.Complex)]
        public void DismissReminder(Int32 reminder_PK)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("reminder_PK", reminder_PK, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_GetEntitiesByResponsibleUser", OperationType = GEMOperationType.Select)]
        public DataSet GetEntitiesByResponsibleUser(Int32? responsible_user_FK, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = new DataSet();
            totalRecordsCount = 0;
            string orderBy = null;

            try
            {
                // Generating order by clause
                orderBy = base.CreateOrderByClause(orderByConditions);

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (responsible_user_FK != null) parameters.Add(new GEMDbParameter("responsible_user_FK", responsible_user_FK, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);
                totalRecordsCount = (int)outputValues["totalRecordsCount"];

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_GetEntitiesReadyForEmail", OperationType = GEMOperationType.Select)]
        public List<Reminder_PK> GetEntitiesReadyForEmail()
        {
            DateTime methodStart = DateTime.Now;
            List<Reminder_PK> entities = new List<Reminder_PK>();

            try
            {
                // Input parameters validation

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_SetReminderStatus", OperationType = GEMOperationType.Complex)]
        public void SetReminderStatus(Int32 reminder_date_FK, Int32 reminder_status_FK)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("reminder_date_FK", reminder_date_FK, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("reminder_status_FK", reminder_status_FK, DbType.String, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_DeleteOldDismissedAutomaticReminders", OperationType = GEMOperationType.Complex)]
        public void DeleteOldDismissedAutomaticReminders(DateTime remider_date)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("remider_date", remider_date, DbType.DateTime, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_REMINDER_GetListFormSearchDataSet]", OperationType = GEMOperationType.Select)]
        public DataSet GetListFormSearchDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            var methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;

            try
            {
                // Generating order by clause
                var orderBy = base.CreateOrderByClause(orderByConditions);
                var parameters = (from pair in filters where !String.IsNullOrWhiteSpace(pair.Value) select new GEMDbParameter(pair.Key, pair.Value, DbType.String, ParameterDirection.Input)).ToList();

                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                Dictionary<string, object> outputValues;

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                if (outputValues.ContainsKey("totalRecordsCount"))
                {
                    totalRecordsCount = (int)outputValues["totalRecordsCount"];
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }
	    #endregion

		#region ICRUDOperations<Reminder_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_GetEntity", OperationType = GEMOperationType.Select)]
		public override Reminder_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Reminder_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Reminder_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Reminder_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_Save", OperationType = GEMOperationType.Save)]
		public override Reminder_PK Save(Reminder_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Reminder_PK> SaveCollection(List<Reminder_PK> entities)
		{
			return base.SaveCollection(entities);
		}

		public override void DeleteCollection<PKType>(List<PKType> entityPKs)
		{
			base.DeleteCollection<PKType>(entityPKs);
		}

		#endregion
	}
}
