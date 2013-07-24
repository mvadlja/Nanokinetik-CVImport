// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	11.11.2011. 10:14:41
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY_ORGANIZATION_APP_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Activity_organization_applicant_PKDAL : GEMDataAccess<Activity_organization_applicant_PK>, IActivity_organization_applicant_PKOperations
	{
		public Activity_organization_applicant_PKDAL() : base() { }
		public Activity_organization_applicant_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IActivity_organization_applicant_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_ORGANIZATION_APP_MN_DeleteByActivityPK", OperationType = GEMOperationType.Select)]
        public void DeleteByActivityPK(Int32? activity_PK)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("activity_PK", activity_PK, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_ORGANIZATION_APP_MN_GetApplicantsByActivity", OperationType = GEMOperationType.Select)]
        public DataSet GetApplicantsByActivity(Int32? activity_FK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet entities = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (activity_FK != null) parameters.Add(new GEMDbParameter("activity_FK", activity_FK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Activity_organization_applicant_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_ORGANIZATION_APP_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Activity_organization_applicant_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_ORGANIZATION_APP_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Activity_organization_applicant_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_ORGANIZATION_APP_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Activity_organization_applicant_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_ORGANIZATION_APP_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Activity_organization_applicant_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_ORGANIZATION_APP_MN_Save", OperationType = GEMOperationType.Save)]
		public override Activity_organization_applicant_PK Save(Activity_organization_applicant_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_ORGANIZATION_APP_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Activity_organization_applicant_PK> SaveCollection(List<Activity_organization_applicant_PK> entities)
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
