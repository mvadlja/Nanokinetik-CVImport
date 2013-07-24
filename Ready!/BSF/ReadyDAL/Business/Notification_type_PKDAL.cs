// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/29/2012 11:17:56 AM
// Description:	GEM2 Generated class for table ready_dev.dbo.NOTIFICATION_TYPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Notification_type_PKDAL : GEMDataAccess<Notification_type_PK>, INotification_type_PKOperations
	{
		public Notification_type_PKDAL() : base() { }
		public Notification_type_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region INotification_type_PKOperations Members



		#endregion

		#region ICRUDOperations<Notification_type_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NOTIFICATION_TYPE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Notification_type_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NOTIFICATION_TYPE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Notification_type_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NOTIFICATION_TYPE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Notification_type_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NOTIFICATION_TYPE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Notification_type_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NOTIFICATION_TYPE_Save", OperationType = GEMOperationType.Save)]
		public override Notification_type_PK Save(Notification_type_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_NOTIFICATION_TYPE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Notification_type_PK> SaveCollection(List<Notification_type_PK> entities)
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
