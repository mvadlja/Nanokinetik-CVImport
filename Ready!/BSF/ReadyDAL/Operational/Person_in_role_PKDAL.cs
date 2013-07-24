// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	17.10.2011. 16:09:48
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PERSON_IN_ROLE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Person_in_role_PKDAL : GEMDataAccess<Person_in_role_PK>, IPerson_in_role_PKOperations
	{
		public Person_in_role_PKDAL() : base() { }
		public Person_in_role_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IPerson_in_role_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_IN_ROLE_GetPersonRolePKsByPersonPK", OperationType = GEMOperationType.Select)]
        public List<Person_in_role_PK> GetPersonRolePKsByPersonPK(Int32? person_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Person_in_role_PK> entities = new List<Person_in_role_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (person_FK != null) parameters.Add(new GEMDbParameter("person_FK", person_FK, DbType.Int32, ParameterDirection.Input));

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


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_IN_ROLE_GetPersonsByRoleSearcher", OperationType = GEMOperationType.Select)]
        public DataSet GetPersonsByRoleSearcher(String person_role, String name, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

                if (person_role != null && person_role.ToString() != String.Empty) parameters.Add(new GEMDbParameter("person_role", person_role, DbType.String, ParameterDirection.Input));
                if (name != null && name.ToString() != String.Empty) parameters.Add(new GEMDbParameter("name", name, DbType.String, ParameterDirection.Input));
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_IN_ROLE_DeleteByPerson", OperationType = GEMOperationType.Select)]
        public void DeleteByPerson(int? personPk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("personPk", personPk, DbType.Int32, ParameterDirection.Input));

                base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
        }

		#endregion

		#region ICRUDOperations<Person_in_role_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_IN_ROLE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Person_in_role_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_IN_ROLE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Person_in_role_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_IN_ROLE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Person_in_role_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_IN_ROLE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Person_in_role_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_IN_ROLE_Save", OperationType = GEMOperationType.Save)]
		public override Person_in_role_PK Save(Person_in_role_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_IN_ROLE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Person_in_role_PK> SaveCollection(List<Person_in_role_PK> entities)
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
