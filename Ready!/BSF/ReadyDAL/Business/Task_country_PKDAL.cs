// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	30.11.2011. 14:02:30
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TASK_COUNTRY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Task_country_PKDAL : GEMDataAccess<Task_country_PK>, ITask_country_PKOperations
	{
		public Task_country_PKDAL() : base() { }
		public Task_country_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ITask_country_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_COUNTRY_MN_GetCountriesByTask", OperationType = GEMOperationType.Select)]
        public List<Task_country_PK> GetCountriesByTask(Int32? task_PK)
        {
            DateTime methodStart = DateTime.Now;
            List<Task_country_PK> entities = new List<Task_country_PK>();

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_COUNTRY_MN_DeleteByTaskPK", OperationType = GEMOperationType.Select)]
        public void DeleteByTaskPK(Int32? task_PK)
        {
            DateTime methodStart = DateTime.Now;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("task_PK", task_PK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Task_country_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_COUNTRY_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Task_country_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_COUNTRY_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Task_country_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_COUNTRY_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Task_country_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_COUNTRY_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Task_country_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_COUNTRY_MN_Save", OperationType = GEMOperationType.Save)]
		public override Task_country_PK Save(Task_country_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_COUNTRY_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Task_country_PK> SaveCollection(List<Task_country_PK> entities)
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
