// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	17.10.2011. 15:38:28
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PERSON_ROLE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Person_role_PKDAL : GEMDataAccess<Person_role_PK>, IPerson_role_PKOperations
	{
		public Person_role_PKDAL() : base() { }
		public Person_role_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IPerson_role_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_ROLE_GetAssignedEntitiesByPerson", OperationType = GEMOperationType.Select)]
        public List<Person_role_PK> GetAssignedEntitiesByPerson(int? personPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Person_role_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("personPk", personPk, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_ROLE_GetAvailableEntitiesByPerson", OperationType = GEMOperationType.Select)]
        public List<Person_role_PK> GetAvailableEntitiesByPerson(int? personPk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<Person_role_PK>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("personPk", personPk, DbType.Int32, ParameterDirection.Input));

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


		#endregion

		#region ICRUDOperations<Person_role_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_ROLE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Person_role_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_ROLE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Person_role_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_ROLE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Person_role_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_ROLE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Person_role_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_ROLE_Save", OperationType = GEMOperationType.Save)]
		public override Person_role_PK Save(Person_role_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_PERSON_ROLE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Person_role_PK> SaveCollection(List<Person_role_PK> entities)
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
