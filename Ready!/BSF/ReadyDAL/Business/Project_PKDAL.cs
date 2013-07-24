// ======================================================================================================================
// Author:		TomoZ560\Tomo
// Create date:	5.12.2011. 23:21:03
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PROJECT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Project_PKDAL : GEMDataAccess<Project_PK>, IProject_PKOperations
	{
		public Project_PKDAL() : base() { }
		public Project_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IProject_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_GetAvailableEntitiesByActivity", OperationType = GEMOperationType.Select)]
        public List<Project_PK> GetAvailableEntitiesByActivity(int activityPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Project_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_PROJECT_GetAssignedEntitiesByActivity]", OperationType = GEMOperationType.Select)]
        public List<Project_PK> GetAssignedEntitiesByActivity(int activityPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Project_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_AbleToDeleteEntity", OperationType = GEMOperationType.Select)]
        public bool AbleToDeleteEntity(int projectPk)
        {
            DateTime methodStart = DateTime.Now;
            var ableToDelete = false;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ProjectPk", projectPk, DbType.Int32, ParameterDirection.Input));

                var result = (int?)base.ExecuteProcedureReturnScalar(parameters);

                ableToDelete = result == 1;

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ableToDelete;

        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_PROJECT_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_PROJECT_GetListFormSearchDataSet]", OperationType = GEMOperationType.Select)]
        public DataSet GetListFormSearchDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_GetTabMenuItemsCount", OperationType = GEMOperationType.Select)]
        public DataSet GetTabMenuItemsCount(Int32 project_PK, int? personFk)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("project_PK", project_PK, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("ResponsibleUserPk", personFk, DbType.Int32, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_Project_GetPPSearcher]", OperationType = GEMOperationType.Select)]
        public DataSet GetPPSearcher(String name, String internalStatus, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

                if (name != null && name.ToString() != String.Empty) parameters.Add(new GEMDbParameter("name", name, DbType.String, ParameterDirection.Input));
                if (internalStatus != null && internalStatus.ToString() != String.Empty) parameters.Add(new GEMDbParameter("internalStatus", internalStatus, DbType.String, ParameterDirection.Input));
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

		#endregion

		#region ICRUDOperations<Project_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_GetEntity", OperationType = GEMOperationType.Select)]
		public override Project_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Project_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Project_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Project_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_Save", OperationType = GEMOperationType.Save)]
		public override Project_PK Save(Project_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PROJECT_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Project_PK> SaveCollection(List<Project_PK> entities)
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
