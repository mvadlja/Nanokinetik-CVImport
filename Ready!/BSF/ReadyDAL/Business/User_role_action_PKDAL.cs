// ======================================================================================================================
// Author:		KRISTIJAN-HPDV7\Kristijan
// Create date:	18.2.2013. 13:29:06
// Description:	GEM2 Generated class for table ReadyDevRBAC.dbo.USER_ROLE_ACTION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class User_role_action_PKDAL : GEMDataAccess<User_role_action_PK>, IUser_role_action_PKOperations
	{
		public User_role_action_PKDAL() : base() { }
		public User_role_action_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IUser_role_action_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_ACTION_GetEntitiesByUserRole", OperationType = GEMOperationType.Select)]
        public List<User_role_action_PK> GetEntitiesByUserRole(int userRolePk)
        {
            DateTime methodStart = DateTime.Now;
            var entities = new List<User_role_action_PK>();

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

		#endregion

		#region ICRUDOperations<User_role_action_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_ACTION_GetEntity", OperationType = GEMOperationType.Select)]
		public override User_role_action_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_ACTION_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<User_role_action_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_ACTION_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<User_role_action_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_ACTION_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<User_role_action_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_ACTION_Save", OperationType = GEMOperationType.Save)]
		public override User_role_action_PK Save(User_role_action_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_USER_ROLE_ACTION_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<User_role_action_PK> SaveCollection(List<User_role_action_PK> entities)
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
