// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	19.10.2011. 15:23:17
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AP_ORGANIZATION_DIST_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Ap_organizatation_dist_mn_PKDAL : GEMDataAccess<Ap_organizatation_dist_mn_PK>, IAp_organizatation_dist_mn_PKOperations
	{
		public Ap_organizatation_dist_mn_PKDAL() : base() { }
		public Ap_organizatation_dist_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IAp_organizatation_dist_mn_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_ORGANIZATION_DIST_MN_GetDistibutorByAP", OperationType = GEMOperationType.Select)]
        public DataSet GetDistibutorByAP(Int32? ap_FK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet entities = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (ap_FK != null) parameters.Add(new GEMDbParameter("ap_FK", ap_FK, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnDataSet(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_ORGANIZATION_DIST_MN_GetAssignedDistributorsByAp", OperationType = GEMOperationType.Select)]
        public List<Ap_organizatation_dist_mn_PK> GetAssignedDistributorsByAp(Int32? authorisedProductFk)
        {
            DateTime methodStart = DateTime.Now;
            List<Ap_organizatation_dist_mn_PK> entities = new List<Ap_organizatation_dist_mn_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (authorisedProductFk != null) parameters.Add(new GEMDbParameter("authorisedProductFk", authorisedProductFk, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Ap_organizatation_dist_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_ORGANIZATION_DIST_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Ap_organizatation_dist_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_ORGANIZATION_DIST_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Ap_organizatation_dist_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_ORGANIZATION_DIST_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Ap_organizatation_dist_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_ORGANIZATION_DIST_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Ap_organizatation_dist_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_ORGANIZATION_DIST_MN_Save", OperationType = GEMOperationType.Save)]
		public override Ap_organizatation_dist_mn_PK Save(Ap_organizatation_dist_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AP_ORGANIZATION_DIST_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Ap_organizatation_dist_mn_PK> SaveCollection(List<Ap_organizatation_dist_mn_PK> entities)
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
