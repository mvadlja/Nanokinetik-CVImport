using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class AuditingDetailDAL : GEMDataAccess<AuditingDetail>, IAuditingDetailOperations
	{
		public AuditingDetailDAL() : base() { }
		public AuditingDetailDAL(string dataSourceId) : base(dataSourceId) { }

		#region IAuditingDetailOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_AuditingDetails_GetColumnValues]", OperationType = GEMOperationType.Select)]
        public DataSet GetColumnValues(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingDetails_GetDistinctColumns", OperationType = GEMOperationType.Select)]
        public DataSet GetDistinctColumns(String dBName, String tableName)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                // Input parameters validation
                if (dBName == null || tableName == null) return ds;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("DBName", dBName, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("TableName", tableName, DbType.String, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingDetails_GetEntriesForMasterAudit", OperationType = GEMOperationType.Select)]
        public List<AuditingDetail> GetEntriesForMasterAudit(Int32 masterID)
        {
            DateTime methodStart = DateTime.Now;
            List<AuditingDetail> entities = new List<AuditingDetail>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("MasterID", masterID, DbType.Int32, ParameterDirection.Input));

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
        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUDIT_GetColumnValue", OperationType = GEMOperationType.Select)]
        public AuditingDetail GetColumnValue(Int32? detailId) {
            DateTime methodStart = DateTime.Now;
            List<AuditingDetail> entities = new List<AuditingDetail>();

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();
        
                parameters.Add(new GEMDbParameter("detail_id", detailId, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            if (entities.Count >= 1) return entities[0];
            return null;
        }

		#endregion

		#region ICRUDOperations<AuditingDetail> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingDetails_GetEntity", OperationType = GEMOperationType.Select)]
		public override AuditingDetail GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingDetails_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<AuditingDetail> GetEntities()
		{
            throw new NotImplementedException();
        }

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingDetails_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<AuditingDetail> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
            throw new NotImplementedException();
        }

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingDetails_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<AuditingDetail> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
            throw new NotImplementedException();
        }

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingDetails_Save", OperationType = GEMOperationType.Save)]
		public override AuditingDetail Save(AuditingDetail entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingDetails_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<AuditingDetail> SaveCollection(List<AuditingDetail> entities)
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
