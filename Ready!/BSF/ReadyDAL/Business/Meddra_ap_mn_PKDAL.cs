// ======================================================================================================================
// Author:		space-monkey\dpetek
// Create date:	14.3.2012. 11:33:26
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.MEDDRA_AP_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class Meddra_ap_mn_PKDAL : GEMDataAccess<Meddra_ap_mn_PK>, IMeddra_ap_mn_PKOperations
	{
		public Meddra_ap_mn_PKDAL() : base() { }
		public Meddra_ap_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IMeddra_ap_mn_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_AP_MN_GetMEDDRAByAP", OperationType = GEMOperationType.Select)]
        public DataSet GetMEDDRAByAP(Int32? ap_FK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = new DataSet();
            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (ap_FK != null)
                {
                    parameters.Add(new GEMDbParameter("ap_FK", ap_FK, DbType.Int32, ParameterDirection.Input));
                }
                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_AP_MN_DeleteMEDDRAByAP", OperationType = GEMOperationType.Delete)]
        public void DeleteMeddraByAP(Int32? ap_FK)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ap_FK", ap_FK, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_AP_MN_DeleteMNByMEDDRA", OperationType = GEMOperationType.Delete)]
        public void DeleteMNByMEDDRA(Int32? meddra_FK)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("meddra_FK", meddra_FK, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

		#endregion

		#region ICRUDOperations<Meddra_ap_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_AP_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Meddra_ap_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_AP_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Meddra_ap_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_AP_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Meddra_ap_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_AP_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Meddra_ap_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_AP_MN_Save", OperationType = GEMOperationType.Save)]
		public override Meddra_ap_mn_PK Save(Meddra_ap_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MEDDRA_AP_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

        


		public override List<Meddra_ap_mn_PK> SaveCollection(List<Meddra_ap_mn_PK> entities)
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
