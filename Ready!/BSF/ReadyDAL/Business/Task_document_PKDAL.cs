// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	30.11.2011. 14:03:31
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.TASK_DOCUMENT_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Task_document_PKDAL : GEMDataAccess<Task_document_PK>, ITask_document_PKOperations
	{
		public Task_document_PKDAL() : base() { }
		public Task_document_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ITask_document_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_DOCUMENT_MN_AbleToDeleteEntity", OperationType = GEMOperationType.Select)]
        public bool AbleToDeleteEntity(int documentPk)
        {
            var methodStart = DateTime.Now;
            var ableToDelete = false;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("documentPk", documentPk, DbType.Int32, ParameterDirection.Input));

                var result = (int?)ExecuteProcedureReturnScalar(parameters);

                ableToDelete = result == 1;

                LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }

            return ableToDelete;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_DOCUMENT_MN_GetTasksByDocument", OperationType = GEMOperationType.Select)]
        public DataSet GetTasksByDocument(Int32? document_FK)
        {
            DateTime methodStart = DateTime.Now;
            DataSet entities = new DataSet();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (document_FK != null) parameters.Add(new GEMDbParameter("document_FK", document_FK, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_DOCUMENT_MN_GetTasksMNByDocument", OperationType = GEMOperationType.Select)]
        public List<Task_document_PK> GetTasksMNByDocument(Int32? document_FK)
        {
            DateTime methodStart = DateTime.Now;
            List<Task_document_PK> entities = new List<Task_document_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (document_FK != null) parameters.Add(new GEMDbParameter("document_FK", document_FK, DbType.Int32, ParameterDirection.Input));

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

		#region ICRUDOperations<Task_document_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_DOCUMENT_MN_GetEntity", OperationType = GEMOperationType.Select)]
		public override Task_document_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_DOCUMENT_MN_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Task_document_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_DOCUMENT_MN_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Task_document_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_DOCUMENT_MN_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Task_document_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_DOCUMENT_MN_Save", OperationType = GEMOperationType.Save)]
		public override Task_document_PK Save(Task_document_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_TASK_DOCUMENT_MN_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Task_document_PK> SaveCollection(List<Task_document_PK> entities)
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
