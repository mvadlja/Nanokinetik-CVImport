// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	11.9.2012. 10:00:59
// Description:	GEM2 Generated class for table ready_dev.dbo.MA_EVENT_TYPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Ma_event_type_PKDAL : GEMDataAccess<Ma_event_type_PK>, IMa_event_type_PKOperations
	{
		public Ma_event_type_PKDAL() : base() { }
		public Ma_event_type_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IMa_event_type_PKOperations Members



		#endregion

		#region ICRUDOperations<Ma_event_type_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_EVENT_TYPE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Ma_event_type_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_EVENT_TYPE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Ma_event_type_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_EVENT_TYPE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Ma_event_type_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_EVENT_TYPE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Ma_event_type_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_EVENT_TYPE_Save", OperationType = GEMOperationType.Save)]
		public override Ma_event_type_PK Save(Ma_event_type_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_EVENT_TYPE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Ma_event_type_PK> SaveCollection(List<Ma_event_type_PK> entities)
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
