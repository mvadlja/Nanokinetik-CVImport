using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class AuditingMasterDAL : GEMDataAccess<AuditingMaster>, IAuditingMasterOperations
	{
		public AuditingMasterDAL() : base() { }
		public AuditingMasterDAL(string dataSourceId) : base(dataSourceId) { }

		#region IAuditingMasterOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_GetAuditMasterIDBySessionToken", OperationType = GEMOperationType.Select)]
        public Int32? GetAuditMasterIDBySessionToken(String session_token)
        {
            DateTime methodStart = DateTime.Now;
            Int32? result = null;
            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (!string.IsNullOrEmpty(session_token)) parameters.Add(new GEMDbParameter("session_token", session_token, DbType.String, ParameterDirection.Input));
                result = (Int32?)Convert.ToInt32(base.ExecuteProcedureReturnScalar(parameters));
                

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return result;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_GetRecordVersions", OperationType = GEMOperationType.Select)]
        public DataSet GetRecordVersions(int PKValue, string table_name)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("PKValue", PKValue, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("table_name", table_name, DbType.String, ParameterDirection.Input));

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


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_AuditingMaster_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AUDIT_GetRecordVersionsAP", OperationType = GEMOperationType.Select)]
        public DataSet GetRecordVersionsAP(int PKValue)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ap_PK", PKValue, DbType.Int32, ParameterDirection.Input));

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


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_SearchEntries", OperationType = GEMOperationType.Select)]
        public List<AuditingMaster> SearchAuditingMaster(String dBName, String operation, String serverName, string dateFrom, string dateTill, String tableName, String username, int pageNumber, int pageSize, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            List<AuditingMaster> entities = new List<AuditingMaster>();
            totalRecordsCount = 0;

            try
            {
                // Input parameters validation
                if (dBName == null || operation == null || serverName == null || tableName == null || username == null) return entities;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                if (dBName != null && dBName.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("DBName", base.PrepareParameterValue(dBName), DbType.String, ParameterDirection.Input));
                if (operation != null && operation.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("Operations", base.PrepareParameterValue(operation), DbType.String, ParameterDirection.Input));
                if (serverName != null && serverName.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("ServerName", base.PrepareParameterValue(serverName), DbType.String, ParameterDirection.Input));
                if (dateFrom != null && dateFrom.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("DateFrom", (DateTime?)Convert.ToDateTime(base.PrepareParameterValue(dateFrom)), DbType.DateTime, ParameterDirection.Input));
                if (dateTill != null && dateTill.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("DateTill", (DateTime?)Convert.ToDateTime(base.PrepareParameterValue(dateTill)), DbType.DateTime, ParameterDirection.Input));
                if (tableName != null && tableName.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("TableName", base.PrepareParameterValue(tableName), DbType.String, ParameterDirection.Input));
                if (username != null && username.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("Username", base.PrepareParameterValue(username), DbType.String, ParameterDirection.Input));
                
                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));

                entities = base.ExecuteProcedureReturnEntities(parameters, out outputValues);
                totalRecordsCount = (int)outputValues["totalRecordsCount"];

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return entities;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_GetDistinctTableNames", OperationType = GEMOperationType.Select)]
        public DataSet GetDistinctTableName(String dBName)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                // Input parameters validation
                if (dBName == null) return ds;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("DBName", dBName, DbType.String, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_GetDistinctServerNames", OperationType = GEMOperationType.Select)]
        public DataSet GetDistinctServerName()
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
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

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_GetDistinctUsernames", OperationType = GEMOperationType.Select)]
        public DataSet GetDistinctUserNames()
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
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

            return ds;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_GetDistinctDBNames", OperationType = GEMOperationType.Select)]
        public DataSet GetDistinctDBNames()
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
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

            return ds;
        }

		#endregion

		#region ICRUDOperations<AuditingMaster> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_GetEntity", OperationType = GEMOperationType.Select)]
		public override AuditingMaster GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<AuditingMaster> GetEntities()
		{
            throw new NotImplementedException();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<AuditingMaster> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
            throw new NotImplementedException();
        }

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<AuditingMaster> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
            throw new NotImplementedException();
        }

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_Save", OperationType = GEMOperationType.Save)]
		public override AuditingMaster Save(AuditingMaster entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AuditingMaster_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<AuditingMaster> SaveCollection(List<AuditingMaster> entities)
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
