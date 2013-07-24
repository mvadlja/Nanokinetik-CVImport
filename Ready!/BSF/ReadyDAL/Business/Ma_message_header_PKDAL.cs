// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	5.9.2012. 14:33:35
// Description:	GEM2 Generated class for table ready_dev.dbo.MA_MESSAGE_HEADER
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class Ma_message_header_PKDAL : GEMDataAccess<Ma_message_header_PK>, IMa_message_header_PKOperations
    {
        public Ma_message_header_PKDAL() : base() { }
        public Ma_message_header_PKDAL(string dataSourceId) : base(dataSourceId) { }

        #region IMa_message_header_PKOperations Members


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MESSAGE_HEADER_GetEntityByReadyId", OperationType = GEMOperationType.Select)]
        public Ma_message_header_PK GetEntityByReadyId(String readyId)
        {
            DateTime methodStart = DateTime.Now;
            Ma_message_header_PK entity = null;
            if (readyId == null) return null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();
                parameters.Add(new GEMDbParameter("ready_id_FK", readyId, DbType.String, ParameterDirection.Input));
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


        #endregion

        #region ICRUDOperations<Ma_message_header_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MESSAGE_HEADER_GetEntity", OperationType = GEMOperationType.Select)]
        public override Ma_message_header_PK GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MESSAGE_HEADER_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Ma_message_header_PK> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MESSAGE_HEADER_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Ma_message_header_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MESSAGE_HEADER_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Ma_message_header_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MESSAGE_HEADER_Save", OperationType = GEMOperationType.Save)]
        public override Ma_message_header_PK Save(Ma_message_header_PK entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_MESSAGE_HEADER_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<Ma_message_header_PK> SaveCollection(List<Ma_message_header_PK> entities)
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
