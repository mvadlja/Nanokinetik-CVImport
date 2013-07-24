// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:37:33
// Description:	GEM2 Generated class for table SSI.dbo.ON_ONJ_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class On_onj_mn_PKDAL : GEMDataAccess<On_onj_mn_PK>, IOn_onj_mn_PKOperations
	{
		public On_onj_mn_PKDAL() : base() { }
		public On_onj_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IOn_onj_mn_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_ONJ_MN_GetEntitiesByONPK", OperationType = GEMOperationType.Select)]
        public List<On_onj_mn_PK> GetEntitiesByONPK(Int32? ONPK)
        {
            DateTime methodStart = DateTime.Now;
            List<On_onj_mn_PK> entities = new List<On_onj_mn_PK>();

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

		#region ICRUDOperations<On_onj_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_ONJ_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override On_onj_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_ONJ_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<On_onj_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_ONJ_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<On_onj_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_ONJ_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<On_onj_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_ONJ_MN_Save", OperationType = GEMOperationType.Save)]
		public override On_onj_mn_PK Save(On_onj_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ON_ONJ_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<On_onj_mn_PK> SaveCollection(List<On_onj_mn_PK> entities)
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
