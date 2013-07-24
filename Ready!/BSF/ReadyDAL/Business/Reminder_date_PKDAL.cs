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
    public class Reminder_date_PKDAL : GEMDataAccess<Reminder_date_PK>, IReminder_date_PKOperations
    {
        public Reminder_date_PKDAL() : base() { }
        public Reminder_date_PKDAL(string dataSourceId) : base(dataSourceId) { }

        #region IReminder_date_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_DATES_GetEntitiesByReminder", OperationType = GEMOperationType.Select)]
        public List<Reminder_date_PK> GetEntitiesByReminder(Int32? reminder_FK)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Reminder_date_PK>();

            try
            {
                var outputValues = new Dictionary<string, object>();
                var parameters = new List<GEMDbParameter>();

                if (reminder_FK != null) parameters.Add(new GEMDbParameter("reminder_FK", reminder_FK, DbType.Int32, ParameterDirection.Input));

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

        #endregion

        #region ICRUDOperations<Reminder_date_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_DATES_GetEntity", OperationType = GEMOperationType.Select)]
        public override Reminder_date_PK GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_DATES_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Reminder_date_PK> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_DATES_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Reminder_date_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_DATES_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Reminder_date_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_DATES_Save", OperationType = GEMOperationType.Save)]
        public override Reminder_date_PK Save(Reminder_date_PK entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_DATES_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<Reminder_date_PK> SaveCollection(List<Reminder_date_PK> entities)
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
