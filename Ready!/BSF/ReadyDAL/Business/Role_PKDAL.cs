// ======================================================================================================================
// Author:		POSSBOOK-DV7\Hrvoje
// Create date:	23.1.2013. 13:22:29
// Description:	GEM2 Generated class for table ReadyRBAC.dbo.ROLE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Role_PKDAL : GEMDataAccess<Role_PK>, IRole_PKOperations
	{
		public Role_PKDAL() : base() { }
		public Role_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IRole_PKOperations Members



		#endregion

		#region ICRUDOperations<Role_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ROLE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Role_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ROLE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Role_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ROLE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Role_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ROLE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Role_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ROLE_Save", OperationType = GEMOperationType.Save)]
		public override Role_PK Save(Role_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ROLE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Role_PK> SaveCollection(List<Role_PK> entities)
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
