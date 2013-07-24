// ======================================================================================================================
// Author:		Home-Laptop\Admin
// Create date:	27.2.2011. 22:41:02
// Description:	GEM2 Generated class for table Kmis.dbo.Users
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class USERDAL : GEMDataAccess<USER>, IUSEROperations
    {
        public USERDAL() : base() { }
        public USERDAL(string dataSourceId) : base(dataSourceId) { }

        #region IUserOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GetUserPermissions", OperationType = GEMOperationType.Select)]
        public DataSet GetUserPermissions(string username)
        {
            var methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("Username", username, DbType.String, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GetUsersDataSet", OperationType = GEMOperationType.Select)]
        public DataSet GetUsersDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_AuthenticateUser", OperationType = GEMOperationType.Select)]
        public USER AuthenticateUser(String userName, String password)
        {
            DateTime methodStart = DateTime.Now;
            USER entity = null;

            try
            {
                // Input parameters validation
                if (userName == null || password == null) return entity;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserName", userName, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("Password", password, DbType.String, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GetUserByUsername", OperationType = GEMOperationType.Select)]
        public USER GetUserByUsername(String userName)
        {
            DateTime methodStart = DateTime.Now;
            USER entity = null;

            try
            {
                // Input parameters validation
                if (userName == null) return entity;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserName", userName, DbType.String, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_SearchUsers", OperationType = GEMOperationType.Select)]
        public List<USER> SearchUsers(String userName, Int32? countryID, Int32? roleID, int pageNumber, int pageSize, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            List<USER> entities = new List<USER>();
            totalRecordsCount = 0;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (userName != null && userName.ToString() != String.Empty) parameters.Add(new GEMDbParameter("UserName", userName, DbType.String, ParameterDirection.Input));
                if (countryID != null) parameters.Add(new GEMDbParameter("CountryID", countryID, DbType.Int32, ParameterDirection.Input));
                if (roleID != null) parameters.Add(new GEMDbParameter("RoleID", roleID, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);
                totalRecordsCount = (int)outputValues["totalRecordsCount"];

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GetUserByEmail", OperationType = GEMOperationType.Select)]
        public USER GetUserByEmail(String email)
        {
            DateTime methodStart = DateTime.Now;
            USER entity = null;

            try
            {
                // Input parameters validation
                if (email == null) return entity;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("Email", email, DbType.String, ParameterDirection.Input));

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


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GetUserByPersonID", OperationType = GEMOperationType.Select)]
        public USER GetUserByPersonID(int? person_PK)
        {
            DateTime methodStart = DateTime.Now;
            USER entities = new USER();

            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (person_PK != null) parameters.Add(new GEMDbParameter("person_PK", person_PK, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_USER_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

        #region ICRUDOperations<User> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GetEntity", OperationType = GEMOperationType.Select)]
        public override USER GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<USER> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<USER> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<USER> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_Save", OperationType = GEMOperationType.Save)]
        public override USER Save(USER entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<USER> SaveCollection(List<USER> entities)
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
