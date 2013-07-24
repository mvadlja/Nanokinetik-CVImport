// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	17.10.2011. 10:57:22
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PERSON
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Person_PKDAL : GEMDataAccess<Person_PK>, IPerson_PKOperations
	{
		public Person_PKDAL() : base() { }
		public Person_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IPerson_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetEntitiesWithUser", OperationType = GEMOperationType.Select)]
        public List<Person_PK> GetEntitiesWithUser()
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Person_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetAvailableEntitiesByUserRole", OperationType = GEMOperationType.Select)]
        public List<Person_PK> GetAvailableEntitiesByUserRole(int userRolePk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Person_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserRolePk", userRolePk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_PERSON_GetAssignedEntitiesByUserRole]", OperationType = GEMOperationType.Select)]
        public List<Person_PK> GetAssignedEntitiesByUserRole(int userRolePk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Person_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("UserRolePk", userRolePk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetEntitiesByRoleName", OperationType = GEMOperationType.Select)]
        public List<Person_PK> GetEntitiesByRoleName(string roleName)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Person_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                if (!string.IsNullOrWhiteSpace(roleName)) parameters.Add(new GEMDbParameter("RoleName", roleName, DbType.String, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetAllEntities", OperationType = GEMOperationType.Select)]
        public List<Person_PK> GetAllEntities()
        {
            DateTime methodStart = DateTime.Now;
            List<Person_PK> entities = new List<Person_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetDataSet", OperationType = GEMOperationType.Select)]
        public DataSet GetPersonDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetPersonsByRole", OperationType = GEMOperationType.Select)]
        public List<Person_PK> GetPersonsByRole(string roleName)
        {
            DateTime methodStart = DateTime.Now;
            List<Person_PK> entities = new List<Person_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (roleName != null) parameters.Add(new GEMDbParameter("role_name", roleName, DbType.String, ParameterDirection.Input));

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


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetPersonByUserID", OperationType = GEMOperationType.Select)]
        public Person_PK GetPersonByUserID(int? user_PK)
        {
            DateTime methodStart = DateTime.Now;
            Person_PK entities = new Person_PK();

            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (user_PK != null) parameters.Add(new GEMDbParameter("user_PK", user_PK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetPersonsSearcher", OperationType = GEMOperationType.Select)]
        public DataSet GetPersonsSearcher(string name, string email, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

                if (!string.IsNullOrEmpty(name)) parameters.Add(new GEMDbParameter("name", name, DbType.String, ParameterDirection.Input));
                if (!string.IsNullOrEmpty(email)) parameters.Add(new GEMDbParameter("email", email, DbType.String, ParameterDirection.Input));
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

	    [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetDataSet", OperationType = GEMOperationType.Select)]
        public DataSet GetPersonDataSet()
        {
            DateTime methodStart = DateTime.Now;
            DataSet entities = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_PERSON_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

		#region ICRUDOperations<Person_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetEntity", OperationType = GEMOperationType.Select)]
		public override Person_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Person_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Person_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Person_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_Save", OperationType = GEMOperationType.Save)]
		public override Person_PK Save(Person_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Person_PK> SaveCollection(List<Person_PK> entities)
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
