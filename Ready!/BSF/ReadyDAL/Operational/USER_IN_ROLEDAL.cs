using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class USER_IN_ROLEDAL : GEMDataAccess<USER_IN_ROLE>, IUSER_IN_ROLEOperations
	{
		public USER_IN_ROLEDAL() : base() { }
        public USER_IN_ROLEDAL(string dataSourceId) : base(dataSourceId) { }

		#region IUserInRoleOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_IN_ROLE_DeleteByUser", OperationType = GEMOperationType.Select)]
        public void DeleteByUser(int userPk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserPk", userPk, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_IN_ROLE_DeleteByUserRole", OperationType = GEMOperationType.Select)]
        public void DeleteByUserRole(int userRolePk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserRolePk", userRolePk, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_IN_ROLE_GetUserInRoleByRoleIDAndUserID", OperationType = GEMOperationType.Select)]
        public USER_IN_ROLE GetUserInRoleByRoleIDAndUserID(Int32 userID, Int32 roleID)
        {
            DateTime methodStart = DateTime.Now;
            USER_IN_ROLE entity = null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserID", userID, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("RoleID", roleID, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_IN_ROLE_GetUsersInRolesByUserID", OperationType = GEMOperationType.Select)]
        public List<USER_IN_ROLE> GetUsersInRolesByUserID(Int32 userID)
        {
            DateTime methodStart = DateTime.Now;
            List<USER_IN_ROLE> entities = new List<USER_IN_ROLE>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_IN_ROLE_GetUsersInRolesByRoleID", OperationType = GEMOperationType.Select)]
        public List<USER_IN_ROLE> GetUsersInRolesByRoleID(Int32 roleID)
        {
            DateTime methodStart = DateTime.Now;
            List<USER_IN_ROLE> entities = new List<USER_IN_ROLE>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("RoleID", roleID, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<UserInRole> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_IN_ROLE_GetEntity", OperationType = GEMOperationType.Select)]
        public override USER_IN_ROLE GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_IN_ROLE_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<USER_IN_ROLE> GetEntities()
		{
			return base.GetEntities();
		}

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_IN_ROLE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<USER_IN_ROLE> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_IN_ROLE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<USER_IN_ROLE> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_IN_ROLE_Save", OperationType = GEMOperationType.Save)]
        public override USER_IN_ROLE Save(USER_IN_ROLE entity)
		{
			return base.Save(entity);
		}

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_IN_ROLE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

        public override List<USER_IN_ROLE> SaveCollection(List<USER_IN_ROLE> entities)
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
