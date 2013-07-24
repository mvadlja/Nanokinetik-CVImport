// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	17.10.2011. 16:25:20
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORGANIZATION_ROLE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Role_org_PKDAL : GEMDataAccess<Role_org_PK>, IRole_org_PKOperations
	{
		public Role_org_PKDAL() : base() { }
		public Role_org_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IRole_org_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_ROLE_GetAssignedEntitiesByOrganization", OperationType = GEMOperationType.Select)]
        public List<Role_org_PK> GetAssignedEntitiesByOrganization(int organizationPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Role_org_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("OrganizationPk", organizationPk, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_ROLE_GetAvailableEntitiesByOrganization", OperationType = GEMOperationType.Select)]
        public List<Role_org_PK> GetAvailableEntitiesByOrganization(int organizationPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Role_org_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("OrganizationPk", organizationPk, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters);

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

		#region ICRUDOperations<Role_org_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_ROLE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Role_org_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_ROLE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Role_org_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_ROLE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Role_org_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_ROLE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Role_org_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_ROLE_Save", OperationType = GEMOperationType.Save)]
		public override Role_org_PK Save(Role_org_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_ROLE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Role_org_PK> SaveCollection(List<Role_org_PK> entities)
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
