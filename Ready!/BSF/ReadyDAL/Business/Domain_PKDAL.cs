// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:42:52
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.DOMAIN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Domain_PKDAL : GEMDataAccess<Domain_PK>, IDomain_PKOperations
	{
		public Domain_PKDAL() : base() { }
		public Domain_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IDomain_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOMAIN_GetAssignedEntitiesByProduct", OperationType = GEMOperationType.Select)]
        public List<Domain_PK> GetAssignedEntitiesByProduct(int productPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Domain_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ProductPk", productPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOMAIN_GetAvailableEntitiesByProduct", OperationType = GEMOperationType.Select)]
        public List<Domain_PK> GetAvailableEntitiesByProduct(int productPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Domain_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ProductPk", productPk, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Domain_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOMAIN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Domain_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOMAIN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Domain_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOMAIN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Domain_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOMAIN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Domain_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOMAIN_Save", OperationType = GEMOperationType.Save)]
		public override Domain_PK Save(Domain_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_DOMAIN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Domain_PK> SaveCollection(List<Domain_PK> entities)
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
