// ======================================================================================================================
// Author:		KIKI-PC\Alan
// Create date:	27.6.2012. 12:03:00
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.REMINDER_EMAIL_RECIPIENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Reminder_email_recipient_PKDAL : GEMDataAccess<Reminder_email_recipient_PK>, IReminder_email_recipient_PKOperations
	{
		public Reminder_email_recipient_PKDAL() : base() { }
		public Reminder_email_recipient_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IReminder_email_recipient_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_EMAIL_RECIPIENT_GetEntitiesByReminder", OperationType = GEMOperationType.Select)]
        public List<Reminder_email_recipient_PK> GetEntitiesByReminder(Int32? reminder_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Reminder_email_recipient_PK> entities = new List<Reminder_email_recipient_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

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

		#region ICRUDOperations<Reminder_email_recipient_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_EMAIL_RECIPIENT_GetEntity", OperationType = GEMOperationType.Select)]
		public override Reminder_email_recipient_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_EMAIL_RECIPIENT_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Reminder_email_recipient_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_EMAIL_RECIPIENT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Reminder_email_recipient_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_EMAIL_RECIPIENT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Reminder_email_recipient_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_EMAIL_RECIPIENT_Save", OperationType = GEMOperationType.Save)]
		public override Reminder_email_recipient_PK Save(Reminder_email_recipient_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_EMAIL_RECIPIENT_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Reminder_email_recipient_PK> SaveCollection(List<Reminder_email_recipient_PK> entities)
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
