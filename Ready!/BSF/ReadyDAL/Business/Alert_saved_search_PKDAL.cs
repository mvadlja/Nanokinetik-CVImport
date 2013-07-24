// ======================================================================================================================
// Author:		POSSBOOK-DV7\Hrvoje
// Create date:	7.3.2013. 15:44:37
// Description:	GEM2 Generated class for table ReadyDev.dbo.ALERT_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class Alert_saved_search_PKDAL : GEMDataAccess<Alert_saved_search_PK>, IAlert_saved_search_PKOperations
    {
        public Alert_saved_search_PKDAL() : base() { }
        public Alert_saved_search_PKDAL(string dataSourceId) : base(dataSourceId) { }

        #region IAlert_saved_search_PKOperations Members



        #endregion

        #region ICRUDOperations<Alert_saved_search_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ALERT_SAVED_SEARCH_GetEntity", OperationType = GEMOperationType.Select)]
        public override Alert_saved_search_PK GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ALERT_SAVED_SEARCH_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Alert_saved_search_PK> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ALERT_SAVED_SEARCH_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Alert_saved_search_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ALERT_SAVED_SEARCH_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Alert_saved_search_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ALERT_SAVED_SEARCH_Save", OperationType = GEMOperationType.Save)]
        public override Alert_saved_search_PK Save(Alert_saved_search_PK entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_ALERT_SAVED_SEARCH_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<Alert_saved_search_PK> SaveCollection(List<Alert_saved_search_PK> entities)
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
