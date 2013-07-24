// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	24.2.2012. 13:42:31
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TASK_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Task_saved_search_PKDAL : GEMDataAccess<Task_saved_search_PK>, ITask_saved_search_PKOperations
	{
		public Task_saved_search_PKDAL() : base() { }
		public Task_saved_search_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ITask_saved_search_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_SAVED_SEARCH_GetEntitiesByUserOrPublic", OperationType = GEMOperationType.Select)]
        public List<Task_saved_search_PK> GetEntitiesByUserOrPublic(Int32? user_fk)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                List<Task_saved_search_PK> entities = new List<Task_saved_search_PK>();
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("@user_fk", user_fk, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);

                return entities;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
                return new List<Task_saved_search_PK>();
            }

        }

		#endregion

		#region ICRUDOperations<Task_saved_search_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_SAVED_SEARCH_GetEntity", OperationType = GEMOperationType.Select)]
		public override Task_saved_search_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_SAVED_SEARCH_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Task_saved_search_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_SAVED_SEARCH_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Task_saved_search_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_SAVED_SEARCH_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Task_saved_search_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_SAVED_SEARCH_Save", OperationType = GEMOperationType.Save)]
		public override Task_saved_search_PK Save(Task_saved_search_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_SAVED_SEARCH_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Task_saved_search_PK> SaveCollection(List<Task_saved_search_PK> entities)
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
