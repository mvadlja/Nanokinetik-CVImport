// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	17.10.2011. 12:48:01
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.COUNTRY
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Country_PKDAL : GEMDataAccess<Country_PK>, ICountry_PKOperations
	{
		public Country_PKDAL() : base() { }
		public Country_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ICountry_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetCountriesByTask", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetCountriesByTask(Int32? task_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Country_PK> entities = new List<Country_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("task_PK", task_PK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetEntityByCountryName", OperationType = GEMOperationType.Select)]
        public Country_PK GetEntityByCountryName(String name)
        {
            DateTime methodStart = DateTime.Now;
            Country_PK entitiy = new Country_PK();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (name != null && name.ToString() != String.Empty) parameters.Add(new GEMDbParameter("name", name, DbType.String, ParameterDirection.Input));

                entitiy = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entitiy;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetCountryCodeByProduct", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetCountryCodeByProduct(Int32? product_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Country_PK> entities = new List<Country_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("product_PK", product_PK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetEntityByAbbrevation", OperationType = GEMOperationType.Select)]
        public Country_PK GetEntityByAbbrevation(String name)
        {
            DateTime methodStart = DateTime.Now;
            Country_PK entitiy = new Country_PK();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (name != null && name.ToString() != String.Empty) parameters.Add(new GEMDbParameter("name", name, DbType.String, ParameterDirection.Input));

                entitiy = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entitiy;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetAssignedEntitiesByProduct", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetAssignedEntitiesByProduct(int productPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Country_PK>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetAvailableEntitiesByProduct", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetAvailableEntitiesByProduct(int productPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Country_PK>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetAssignedEntitiesByTask", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetAssignedEntitiesByTask(int taskPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Country_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("TaskPk", taskPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetAvailableEntitiesByTask", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetAvailableEntitiesByTask(int taskPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Country_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("TaskPk", taskPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetAssignedEntitiesByActivity", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetAssignedEntitiesByActivity(int activityPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Country_PK>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetAvailableEntitiesByActivity", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetAvailableEntitiesByActivity(int activityPK)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Country_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ActivityPK", activityPK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetAssignedEntitiesByProject", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetAssignedEntitiesByProject(int projectPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Country_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ProjectPk", projectPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetAvailableEntitiesByProject", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetAvailableEntitiesByProject(int projectPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Country_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ProjectPk", projectPk, DbType.Int32, ParameterDirection.Input));

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


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetSelectedEntitiesForProjectPK_MN", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetSelectedEntitiesForProjectPK_MN(Int32 project_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Country_PK> entities = new List<Country_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("project_PK", project_PK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetAvailableEntitiesForProjectPK_MN", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetAvailableEntitiesForProjectPK_MN(Int32 project_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Country_PK> entities = new List<Country_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("project_PK", project_PK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetSelectedEntitiesForActivityPK_MN", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetSelectedEntitiesForActivityPK_MN(Int32 activity_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Country_PK> entities = new List<Country_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("activity_PK", activity_PK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetAvailableEntitiesForActivityPK_MN", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetAvailableEntitiesForActivityPK_MN(Int32 activity_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Country_PK> entities = new List<Country_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("activity_PK", activity_PK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetSelectedEntitiesForTask_MN", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetSelectedEntitiesForTaskPK_MN(Int32 task_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Country_PK> entities = new List<Country_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("task_PK", task_PK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetAvailableEntitiesForTask_MN", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetAvailableEntitiesForTaskPK_MN(Int32 task_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Country_PK> entities = new List<Country_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("task_PK", task_PK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetEntitiesCustomSort", OperationType = GEMOperationType.Select)]
        public List<Country_PK> GetEntitiesCustomSort()
        {
            DateTime methodStart = DateTime.Now;
            List<Country_PK> entities = new List<Country_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

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

		#endregion

		#region ICRUDOperations<Country_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetEntity", OperationType = GEMOperationType.Select)]
		public override Country_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Country_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Country_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Country_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_Save", OperationType = GEMOperationType.Save)]
		public override Country_PK Save(Country_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_COUNTRY_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Country_PK> SaveCollection(List<Country_PK> entities)
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
