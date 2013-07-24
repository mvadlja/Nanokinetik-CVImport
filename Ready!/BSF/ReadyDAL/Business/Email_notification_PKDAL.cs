// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/29/2012 11:16:13 AM
// Description:	GEM2 Generated class for table ready_dev.dbo.EMAIL_NOTIFICATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Email_notification_PKDAL : GEMDataAccess<Email_notification_PK>, IEmail_notification_PKOperations
	{
		public Email_notification_PKDAL() : base() { }
		public Email_notification_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IEmail_notification_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMAIL_NOTIFICATION_GetEntitiesByNotificationType", OperationType = GEMOperationType.Select)]
        public List<Email_notification_PK> GetEntitiesByNotificationType(NotificationType notificationType)
        {
            DateTime methodStart = DateTime.Now;
            List<Email_notification_PK> result = new List<Email_notification_PK>();

            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (notificationType != NotificationType.NULL) parameters.Add(new GEMDbParameter("notification_type_FK", (int)notificationType, DbType.Int32, ParameterDirection.Input));

                result = base.ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return result;
        }

		#endregion

		#region ICRUDOperations<Email_notification_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMAIL_NOTIFICATION_GetEntity", OperationType = GEMOperationType.Select)]
		public override Email_notification_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMAIL_NOTIFICATION_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Email_notification_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMAIL_NOTIFICATION_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Email_notification_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMAIL_NOTIFICATION_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Email_notification_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMAIL_NOTIFICATION_Save", OperationType = GEMOperationType.Save)]
		public override Email_notification_PK Save(Email_notification_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMAIL_NOTIFICATION_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Email_notification_PK> SaveCollection(List<Email_notification_PK> entities)
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
