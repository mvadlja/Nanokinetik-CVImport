// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	5.9.2012. 14:17:32
// Description:	GEM2 Generated class for table ready_dev.dbo.MA_FILE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class Ma_file_PKDAL : GEMDataAccess<Ma_file_PK>, IMa_file_PKOperations
    {public Ma_file_PKDAL() : base() { }
        
        public Ma_file_PKDAL(string dataSourceId) : base(dataSourceId) { }

        #region IMa_file_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_FILE_GetEntityByReadyIdAndType", OperationType = GEMOperationType.Select)]
        public Ma_file_PK GetEntityByReadyIdAndType(String readyId, Ma_file_PK.FileType type)
        {
            DateTime methodStart = DateTime.Now;
            Ma_file_PK entity = null;
            if (readyId == null) return null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("file_type_FK", (int)type, DbType.Int32, ParameterDirection.Input));
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_FILE_GetEntityByFileNameAndType", OperationType = GEMOperationType.Select)]
        public Ma_file_PK GetEntityByFileNameAndType(String fileName, Ma_file_PK.FileType type)
        {
            DateTime methodStart = DateTime.Now;
            Ma_file_PK entity = null;
            if (fileName == null) return null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("file_type_FK", (int)type, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("file_name", fileName, DbType.String, ParameterDirection.Input));
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_FILE_GetEntitiesByFileName", OperationType = GEMOperationType.Select)]
        public List<Ma_file_PK> GetEntitiesByFileName(String fileName)
        {
            DateTime methodStart = DateTime.Now;
            List<Ma_file_PK> entiities = null;
            if (fileName == null) return null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("file_name", fileName, DbType.String, ParameterDirection.Input));
                entiities = base.ExecuteProcedureReturnEntities(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entiities;
        }



        #endregion

        #region ICRUDOperations<Ma_file_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_FILE_GetEntity", OperationType = GEMOperationType.Select)]
        public override Ma_file_PK GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_FILE_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Ma_file_PK> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_FILE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Ma_file_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_FILE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Ma_file_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_FILE_Save", OperationType = GEMOperationType.Save)]
        public override Ma_file_PK Save(Ma_file_PK entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_FILE_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<Ma_file_PK> SaveCollection(List<Ma_file_PK> entities)
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
