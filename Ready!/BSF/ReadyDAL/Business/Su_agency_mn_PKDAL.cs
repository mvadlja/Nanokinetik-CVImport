// ======================================================================================================================
// Author:		Koki-PC\Koki
// Create date:	12/23/2011 12:16:39 PM
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SU_AGENCY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Su_agency_mn_PKDAL : GEMDataAccess<Su_agency_mn_PK>, ISu_agency_mn_PKOperations
	{
		public Su_agency_mn_PKDAL() : base() { }
		public Su_agency_mn_PKDAL(string dataSourceId) : base(dataSourceId) { }
         
		#region ISu_agency_mn_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SU_AGENCY_MN_DeleteBySubmissionUnitPK", OperationType = GEMOperationType.Select)]
        public void DeleteBySubmissionUnitPK(Int32? submissionUnitPk)
        {
            var methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("submissionUnitPk", submissionUnitPk, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SU_AGENCY_MN_GetAgencyBySU", OperationType = GEMOperationType.Select)]
        public DataSet GetAgencyBySU(Int32? su_FK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet entities = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (su_FK != null) parameters.Add(new GEMDbParameter("su_FK", su_FK, DbType.Int32, ParameterDirection.Input));

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

		#endregion

		#region ICRUDOperations<Su_agency_mn_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SU_AGENCY_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Su_agency_mn_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SU_AGENCY_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Su_agency_mn_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SU_AGENCY_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Su_agency_mn_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SU_AGENCY_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Su_agency_mn_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SU_AGENCY_MN_Save", OperationType = GEMOperationType.Save)]
		public override Su_agency_mn_PK Save(Su_agency_mn_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SU_AGENCY_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Su_agency_mn_PK> SaveCollection(List<Su_agency_mn_PK> entities)
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
