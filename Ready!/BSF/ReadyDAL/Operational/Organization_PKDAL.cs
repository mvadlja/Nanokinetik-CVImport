// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	21.1.2012. 9:09:39
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORGANIZATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Organization_PKDAL : GEMDataAccess<Organization_PK>, IOrganization_PKOperations
	{
		public Organization_PKDAL() : base() { }
		public Organization_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IOrganization_PKOperations Members


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_GetDataSet", OperationType = GEMOperationType.Select)]
        public DataSet GetDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;
            string orderBy = null;

            try
            {
                // Generating order by clause
                orderBy = base.CreateOrderByClause(orderByConditions);

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);
                totalRecordsCount = (int)outputValues["totalRecordsCount"];

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_GetOrganizationsByRole", OperationType = GEMOperationType.Select)]
        public List<Organization_PK> GetOrganizationsByRole(String role_name)
        {
            DateTime methodStart = DateTime.Now;
            List<Organization_PK> entities = new List<Organization_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (role_name != null) parameters.Add(new GEMDbParameter("role_name", role_name, DbType.String, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_GetAvailableApplicantsForActivity", OperationType = GEMOperationType.Select)]
        public List<Organization_PK> GetAvailableApplicantsForActivity(Int32? activityPk)
        {
            DateTime methodStart = DateTime.Now;
            List<Organization_PK> entities = new List<Organization_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ActivityPk", activityPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_GetAssignedApplicantsForActivity", OperationType = GEMOperationType.Select)]
        public List<Organization_PK> GetAssignedApplicantsForActivity(Int32? activityPk)
        {
            DateTime methodStart = DateTime.Now;
            List<Organization_PK> entities = new List<Organization_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ActivityPk", activityPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_GetAssignedDistributorsByAp", OperationType = GEMOperationType.Select)]
        public List<Organization_PK> GetAssignedDistributorsByAp(Int32? authorisedProductFk)
        {
            DateTime methodStart = DateTime.Now;
            List<Organization_PK> entities = new List<Organization_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("authorisedProductFk", authorisedProductFk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_GetAvailableDistributorsByAp", OperationType = GEMOperationType.Select)]
        public List<Organization_PK> GetAvailableDistributorsByAp(Int32? authorisedProductFk)
        {
            DateTime methodStart = DateTime.Now;
            List<Organization_PK> entities = new List<Organization_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("authorisedProductFk", authorisedProductFk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_GetAvailableAgenciesForSubmissionUnit", OperationType = GEMOperationType.Select)]
        public List<Organization_PK> GetAvailableAgenciesForSubmissionUnit(Int32? submissionUnitPk)
        {
            DateTime methodStart = DateTime.Now;
            List<Organization_PK> entities = new List<Organization_PK>();

            try
            {
                var outputValues = new Dictionary<string, object>();
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("submissionUnitPk", submissionUnitPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_GetAssignedAgenciesForSubmissionUnit", OperationType = GEMOperationType.Select)]
        public List<Organization_PK> GetAssignedAgenciesForSubmissionUnit(Int32? submissionUnitPk)
        {
            var methodStart = DateTime.Now;
            var entities = new List<Organization_PK>();

            try
            {
                var outputValues = new Dictionary<string, object>();
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("submissionUnitPk", submissionUnitPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_ORGANIZATION_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
        public DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            var methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;

            try
            {
                // Generating order by clause
                var orderBy = base.CreateOrderByClause(orderByConditions);
                var parameters = (from pair in filters where !String.IsNullOrWhiteSpace(pair.Value) select new GEMDbParameter(pair.Key, pair.Value, DbType.String, ParameterDirection.Input)).ToList();

                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                Dictionary<string, object> outputValues;

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                if (outputValues.ContainsKey("totalRecordsCount"))
                {
                    totalRecordsCount = (int)outputValues["totalRecordsCount"];
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

		#endregion

		#region ICRUDOperations<Organization_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_GetEntity", OperationType = GEMOperationType.Select)]
		public override Organization_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Organization_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Organization_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Organization_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_Save", OperationType = GEMOperationType.Save)]
		public override Organization_PK Save(Organization_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ORGANIZATION_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Organization_PK> SaveCollection(List<Organization_PK> entities)
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
