// ======================================================================================================================
// Author:		ANTEC\Kiki
// Create date:	4.12.2011. 20:03:17
// Description:	GEM2 Generated class for table SSI.dbo.ON_DOMAIN_ON_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class On_domain_on_mn_PKDAL : GEMDataAccess<On_domain_on_mn_PK>, IOn_domain_on_mn_PKOperations
	{
		public On_domain_on_mn_PKDAL() : base() { }
		public On_domain_on_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IOn_domain_on_mn_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_DOMAIN_ON_MN_GetEntitiesByONPK", OperationType = GEMOperationType.Select)]
        public List<On_domain_on_mn_PK> GetEntitiesByONPK(Int32? ONPK)
        {
            DateTime methodStart = DateTime.Now;
            List<On_domain_on_mn_PK> entities = new List<On_domain_on_mn_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (ONPK != null) parameters.Add(new GEMDbParameter("ONPK", ONPK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<On_domain_on_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_DOMAIN_ON_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override On_domain_on_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_DOMAIN_ON_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<On_domain_on_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_DOMAIN_ON_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<On_domain_on_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_DOMAIN_ON_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<On_domain_on_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_DOMAIN_ON_MN_Save", OperationType = GEMOperationType.Save)]
		public override On_domain_on_mn_PK Save(On_domain_on_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_DOMAIN_ON_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<On_domain_on_mn_PK> SaveCollection(List<On_domain_on_mn_PK> entities)
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
