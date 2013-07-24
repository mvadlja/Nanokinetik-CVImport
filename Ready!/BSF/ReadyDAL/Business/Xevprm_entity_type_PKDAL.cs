// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/21/2012 2:15:17 PM
// Description:	GEM2 Generated class for table ready_billev_production.dbo.XEVPRM_ENTITY_TYPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Xevprm_entity_type_PKDAL : GEMDataAccess<Xevprm_entity_type_PK>, IXevprm_entity_type_PKOperations
	{
		public Xevprm_entity_type_PKDAL() : base() { }
		public Xevprm_entity_type_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IXevprm_entity_type_PKOperations Members



		#endregion

		#region ICRUDOperations<Xevprm_entity_type_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_TYPE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Xevprm_entity_type_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_TYPE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Xevprm_entity_type_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_TYPE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Xevprm_entity_type_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_TYPE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Xevprm_entity_type_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_TYPE_Save", OperationType = GEMOperationType.Save)]
		public override Xevprm_entity_type_PK Save(Xevprm_entity_type_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_ENTITY_TYPE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Xevprm_entity_type_PK> SaveCollection(List<Xevprm_entity_type_PK> entities)
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
