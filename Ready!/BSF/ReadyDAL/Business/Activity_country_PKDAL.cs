// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	11.11.2011. 10:14:17
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY_COUNTRY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Activity_country_PKDAL : GEMDataAccess<Activity_country_PK>, IActivity_country_PKOperations
	{
		public Activity_country_PKDAL() : base() { }
		public Activity_country_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IActivity_country_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_COUNTRY_MN_DeleteByActivityPK", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_COUNTRY_MN_GetCountriesListByActivity", OperationType = GEMOperationType.Select)]
        public List<Activity_country_PK> GetCountriesListByActivity(Int32? activity_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Activity_country_PK> entities = new List<Activity_country_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (activity_PK != null) parameters.Add(new GEMDbParameter("activity_FK", activity_PK, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_COUNTRY_MN_GetCountriesByActivity", OperationType = GEMOperationType.Select)]
        public DataSet GetCountriesByActivity(Int32? activity_FK)
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

		#region ICRUDOperations<Activity_country_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_COUNTRY_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Activity_country_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_COUNTRY_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Activity_country_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_COUNTRY_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Activity_country_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_COUNTRY_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Activity_country_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_COUNTRY_MN_Save", OperationType = GEMOperationType.Save)]
		public override Activity_country_PK Save(Activity_country_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ACTIVITY_COUNTRY_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Activity_country_PK> SaveCollection(List<Activity_country_PK> entities)
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
