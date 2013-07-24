// ======================================================================================================================
// Author:		POSSBOOK-DV7\Hrvoje
// Create date:	10.1.2013. 10:37:15
// Description:	GEM2 Generated class for table ReadyDev.dbo.REMINDER_STATUS
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Reminder_status_PKDAL : GEMDataAccess<Reminder_status_PK>, IReminder_status_PKOperations
	{
		public Reminder_status_PKDAL() : base() { }
		public Reminder_status_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IReminder_status_PKOperations Members



		#endregion

		#region ICRUDOperations<Reminder_status_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_STATUS_GetEntity", OperationType = GEMOperationType.Select)]
		public override Reminder_status_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_STATUS_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Reminder_status_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_STATUS_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Reminder_status_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_STATUS_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Reminder_status_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_STATUS_Save", OperationType = GEMOperationType.Save)]
		public override Reminder_status_PK Save(Reminder_status_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_REMINDER_STATUS_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Reminder_status_PK> SaveCollection(List<Reminder_status_PK> entities)
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
