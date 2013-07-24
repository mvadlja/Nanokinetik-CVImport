// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	14.9.2012. 10:30:25
// Description:	GEM2 Generated class for table ready_dev.dbo.EMA_RECEIVED_FILE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class Ema_received_file_PKDAL : GEMDataAccess<Ema_received_file_PK>, IEma_received_file_PKOperations
    {
        public Ema_received_file_PKDAL() : base() { }
        public Ema_received_file_PKDAL(string dataSourceId) : base(dataSourceId) { }

        #region IEma_received_file_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_RECEIVED_FILE_GetEntitiesByTypeAndStatus", OperationType = GEMOperationType.Select)]
        public List<Ema_received_file_PK> GetEntitiesByTypeAndStatus(String type, int? status)
        {
            DateTime methodStart = DateTime.Now;
            List<Ema_received_file_PK> entities = new List<Ema_received_file_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("file_type", type, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("status", status, DbType.Int32, ParameterDirection.Input));

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

        #region ICRUDOperations<Ema_received_file_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_RECEIVED_FILE_GetEntity", OperationType = GEMOperationType.Select)]
        public override Ema_received_file_PK GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_RECEIVED_FILE_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Ema_received_file_PK> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_RECEIVED_FILE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Ema_received_file_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_RECEIVED_FILE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Ema_received_file_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_RECEIVED_FILE_Save", OperationType = GEMOperationType.Save)]
        public override Ema_received_file_PK Save(Ema_received_file_PK entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_EMA_RECEIVED_FILE_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<Ema_received_file_PK> SaveCollection(List<Ema_received_file_PK> entities)
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
