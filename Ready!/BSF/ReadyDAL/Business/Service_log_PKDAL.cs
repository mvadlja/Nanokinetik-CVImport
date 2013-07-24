// ======================================================================================================================
// Author:		POSSIMUSIT-MATE\Mateo
// Create date:	4.5.2012. 11:03:57
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SERVICE_LOG
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class Service_log_PKDAL : GEMDataAccess<Service_log_PK>, IService_log_PKOperations
    {
        public Service_log_PKDAL() : base() { }
        public Service_log_PKDAL(string dataSourceId) : base(dataSourceId) { }

        #region IService_log_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SERVICE_LOG_GetTimeofLastChange", OperationType = GEMOperationType.Select)]
        public DateTime? TimeOfLastChange()
        {
            DateTime methodStart = DateTime.Now;
            //DateTime? ds = null;

            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                var ds = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);

                return ds != null ? ds.log_time : null;
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return null;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_SERVICE_LOG_GetReportDataSet]", OperationType = GEMOperationType.Select)]
        public DataSet GetReportDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;
            string orderBy = null;

            try
            {
                // Generating order by clause
                orderBy = base.CreateOrderByClause(orderByConditions);

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);
                totalRecordsCount = (int)outputValues["totalRecordsCount"];

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_SERVICE_LOG_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
        public DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            var methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;

            try
            {
                // Generating order by clause
                var orderBy = base.CreateOrderByClause(orderByConditions);
                var parameters = (from pair in filters where !String.IsNullOrWhiteSpace(pair.Value) select new GEMDbParameter(pair.Key, pair.Value, DbType.String, ParameterDirection.Input)).ToList();

                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("OrderByQuery", orderBy, DbType.String, ParameterDirection.Input));

                Dictionary<string, object> outputValues;

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                if (outputValues.ContainsKey("totalRecordsCount"))
                {
                    totalRecordsCount = (int)outputValues["totalRecordsCount"];
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }
        #endregion

        #region ICRUDOperations<Service_log_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SERVICE_LOG_GetEntity", OperationType = GEMOperationType.Select)]
        public override Service_log_PK GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SERVICE_LOG_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Service_log_PK> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SERVICE_LOG_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Service_log_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SERVICE_LOG_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Service_log_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SERVICE_LOG_Save", OperationType = GEMOperationType.Save)]
        public override Service_log_PK Save(Service_log_PK entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SERVICE_LOG_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<Service_log_PK> SaveCollection(List<Service_log_PK> entities)
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
