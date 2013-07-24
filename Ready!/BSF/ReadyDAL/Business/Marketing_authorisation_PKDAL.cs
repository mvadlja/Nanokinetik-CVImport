// ======================================================================================================================
// Author:		Tomo-PC\Tomo
// Create date:	5.9.2012. 13:03:02
// Description:	GEM2 Generated class for table ready_dev.dbo.MARKETING_AUTHORISATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Marketing_authorisation_PKDAL : GEMDataAccess<Marketing_authorisation_PK>, IMarketing_authorisation_PKOperations
	{
        public Marketing_authorisation_PKDAL() : base() { }
        public Marketing_authorisation_PKDAL(string dataSourceId) : base(dataSourceId) { }

        #region IMarketing_authorisation_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MARKETING_AUTHORISATION_GetEntitiesByStatus", OperationType = GEMOperationType.Select)]
        public List<Marketing_authorisation_PK> GetEntitiesByStatus(Marketing_authorisation_PK.MAStatus status)
        {
            DateTime methodStart = DateTime.Now;
            List<Marketing_authorisation_PK> entities = new List<Marketing_authorisation_PK>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ma_status_FK", (int)status, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MARKETING_AUTHORISATION_GetEntityByReadyId", OperationType = GEMOperationType.Select)]
        public Marketing_authorisation_PK GetEntityByReadyId(String readyId)
        {
            DateTime methodStart = DateTime.Now;
            Marketing_authorisation_PK entity = null;
            if (readyId == null) return null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();
                parameters.Add(new GEMDbParameter("ready_id", readyId, DbType.String, ParameterDirection.Input));
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

        #region ICRUDOperations<Marketing_authorisation_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MARKETING_AUTHORISATION_GetEntity", OperationType = GEMOperationType.Select)]
        public override Marketing_authorisation_PK GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MARKETING_AUTHORISATION_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Marketing_authorisation_PK> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MARKETING_AUTHORISATION_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Marketing_authorisation_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MARKETING_AUTHORISATION_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Marketing_authorisation_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MARKETING_AUTHORISATION_Save", OperationType = GEMOperationType.Save)]
        public override Marketing_authorisation_PK Save(Marketing_authorisation_PK entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_MARKETING_AUTHORISATION_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<Marketing_authorisation_PK> SaveCollection(List<Marketing_authorisation_PK> entities)
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
