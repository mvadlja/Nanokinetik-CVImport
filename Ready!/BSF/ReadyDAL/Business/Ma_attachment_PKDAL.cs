// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	6.9.2012. 17:11:51
// Description:	GEM2 Generated class for table ready_dev.dbo.MA_ATTACHMENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class Ma_attachment_PKDAL : GEMDataAccess<Ma_attachment_PK>, IMa_attachment_PKOperations
    {
        public Ma_attachment_PKDAL() : base() { }
        public Ma_attachment_PKDAL(string dataSourceId) : base(dataSourceId) { }

        #region IMa_attachment_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_ATTACHMENT_GetNameAndDateForEntities", OperationType = GEMOperationType.Select)]
        public Dictionary<String, DateTime> GetNameAndDateForEntities()
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
            Dictionary<String, DateTime> returnValues = new Dictionary<string, DateTime>();
            if (ds != null && ds.Tables.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    String fileName = (String)row["file_name"];
                    DateTime? lastChange = (row["last_change"] != null && row["last_change"] is DateTime?) ? (DateTime?)row["last_change"] : null;
                    if (lastChange == null || !lastChange.HasValue) lastChange = new DateTime(0);
                    returnValues.Add(fileName, lastChange.Value);

                }
            }

            return returnValues;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_ATTACHMENT_GetEntityByFileName", OperationType = GEMOperationType.Select)]
        public Ma_attachment_PK GetEntityByFileName(String fileName)
        {
            DateTime methodStart = DateTime.Now;
            Ma_attachment_PK entity = null;
            if (fileName == null) return null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_ATTACHMENT_GetAttachmentPK", OperationType = GEMOperationType.Select)]
        public int? GetAttachmentPK(String fileName)
        {
            int? attachmentPK = null;
            try
            {
                
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();
                parameters.Add(new GEMDbParameter("file_name", fileName, DbType.String, ParameterDirection.Input));
                attachmentPK = (int?)base.ExecuteProcedureReturnScalar(parameters);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return attachmentPK;

        }

        #endregion

        #region ICRUDOperations<Ma_attachment_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_ATTACHMENT_GetEntity", OperationType = GEMOperationType.Select)]
        public override Ma_attachment_PK GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_ATTACHMENT_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Ma_attachment_PK> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_ATTACHMENT_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Ma_attachment_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_ATTACHMENT_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Ma_attachment_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_ATTACHMENT_Save", OperationType = GEMOperationType.Save)]
        public override Ma_attachment_PK Save(Ma_attachment_PK entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MA_ATTACHMENT_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<Ma_attachment_PK> SaveCollection(List<Ma_attachment_PK> entities)
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
