using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class USER_ROLEDAL : GEMDataAccess<USER_ROLE>, IUSER_ROLEOperations
	{
		public USER_ROLEDAL() : base() { }
        public USER_ROLEDAL(string dataSourceId) : base(dataSourceId) { }

        #region IRoleOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_GetAvailableEntitiesByUser", OperationType = GEMOperationType.Select)]
        public List<USER_ROLE> GetAvailableEntitiesByUser(int userPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<USER_ROLE>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserPk", userPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_USER_ROLE_GetAssignedEntitiesByUser]", OperationType = GEMOperationType.Select)]
        public List<USER_ROLE> GetAssignedEntitiesByUser(int userPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<USER_ROLE>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserPk", userPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_USER_ROLE_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_GetVisibleRoles", OperationType = GEMOperationType.Select)]
        public List<USER_ROLE> GetVisibleRoles()
        {
            DateTime methodStart = DateTime.Now;
            List<USER_ROLE> entities = new List<USER_ROLE>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_GetRolesByUserID", OperationType = GEMOperationType.Select)]
        public List<USER_ROLE> GetRolesByUserID(Int32 userID)
        {
            DateTime methodStart = DateTime.Now;
            List<USER_ROLE> entities = new List<USER_ROLE>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserID", userID, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_GetActiveRolesByUserID", OperationType = GEMOperationType.Select)]
        public List<USER_ROLE> GetActiveRolesByUserID(Int32 userID)
        {
            DateTime methodStart = DateTime.Now;
            List<USER_ROLE> entities = new List<USER_ROLE>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserID", userID, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_GetRoleByName", OperationType = GEMOperationType.Select)]
        public USER_ROLE GetRoleByName(String name)
        {
            DateTime methodStart = DateTime.Now;
            USER_ROLE entity = null;

            try
            {
                // Input parameters validation
                if (name == null) return entity;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("Name", name, DbType.String, ParameterDirection.Input));

                entity = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entity;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_GetActiveRolesByUsername", OperationType = GEMOperationType.Select)]
        public List<USER_ROLE> GetActiveRolesByUsername(String username)
        {
            DateTime methodStart = DateTime.Now;
            List<USER_ROLE> entities = new List<USER_ROLE>();

            try
            {
                // Input parameters validation
                if (username == null) return entities;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("Username", username, DbType.String, ParameterDirection.Input));

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

		#region ICRUDOperations<Role> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_GetEntity", OperationType = GEMOperationType.Select)]
        public override USER_ROLE GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<USER_ROLE> GetEntities()
		{
			return base.GetEntities();
		}

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<USER_ROLE> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<USER_ROLE> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_Save", OperationType = GEMOperationType.Save)]
        public override USER_ROLE Save(USER_ROLE entity)
		{
			return base.Save(entity);
		}

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

        public override List<USER_ROLE> SaveCollection(List<USER_ROLE> entities)
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
