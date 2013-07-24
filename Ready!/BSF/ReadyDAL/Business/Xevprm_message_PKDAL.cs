// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	11.11.2011. 10:14:03
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ACTIVITY
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class Xevprm_message_PKDAL : GEMDataAccess<Xevprm_message_PK>, IXevprm_message_PKOperations
    {
        public Xevprm_message_PKDAL() : base() { }
        public Xevprm_message_PKDAL(string dataSourceId) : base(dataSourceId) { }

        #region xEVPMD_message_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_XEVPRM_MESSAGE_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_MESSAGE_GetLatestMessageNumberByXevprmEntity", OperationType = GEMOperationType.Select)]
        public string GetLatestMessageNumberByXevprmEntity(int xevprmEntityPk, int xevprmEntityType)
        {
            DateTime methodStart = DateTime.Now;
            string result = null;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("XevprmEntityPk", xevprmEntityPk, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("XevprmEntityType", xevprmEntityType, DbType.Int32, ParameterDirection.Input));

                result = (string)base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return result;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_MESSAGE_GetLatestEntityByXevprmEntity", OperationType = GEMOperationType.Select)]
        public Xevprm_message_PK GetLatestEntityByXevprmEntity(int xevprmEntityPk, int xevprmEntityType)
        {
            DateTime methodStart = DateTime.Now;
            Xevprm_message_PK result = null;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("XevprmEntityPk", xevprmEntityPk, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("XevprmEntityType", xevprmEntityType, DbType.Int32, ParameterDirection.Input));

                result = base.ExecuteProcedureReturnEntity(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return result;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_MESSAGE_GetEntitiesPksReadyForSubmission", OperationType = GEMOperationType.Select)]
        public List<int> GetEntitiesPksReadyForSubmission()
        {
            DateTime methodStart = DateTime.Now;
            var result = new List<int>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                DataSet messagePks = base.ExecuteProcedureReturnDataSet(parameters);

                if (messagePks != null &&
                    messagePks.Tables.Count > 0 &&
                    messagePks.Tables[0].Columns.Contains("xevprm_message_PK") &&
                    messagePks.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in messagePks.Tables[0].Rows)
                    {
                        if (row["xevprm_message_PK"] != DBNull.Value)
                        {
                            result.Add((int)row["xevprm_message_PK"]);
                        }
                    }
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return result;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_MESSAGE_GetEntitiesPksReadyForMDNSubmission", OperationType = GEMOperationType.Select)]
        public List<int> GetEntitiesPksReadyForMDNSubmission()
        {
            DateTime methodStart = DateTime.Now;
            var result = new List<int>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                DataSet messagePks = base.ExecuteProcedureReturnDataSet(parameters);

                if (messagePks != null &&
                    messagePks.Tables.Count > 0 &&
                    messagePks.Tables[0].Columns.Contains("xevprm_message_PK") &&
                    messagePks.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in messagePks.Tables[0].Rows)
                    {
                        if (row["xevprm_message_PK"] != DBNull.Value)
                        {
                            result.Add((int)row["xevprm_message_PK"]);
                        }
                    }
                }

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return result;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_xEVPRMStatistic", OperationType = GEMOperationType.Select)]
        public DataSet GetXevprmStatistic()
        {
            DateTime methodStart = DateTime.Now;
            DataSet result = null;

            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();
                result = base.ExecuteProcedureReturnDataSet(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return result;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_MESSAGE_GetEntityByMessageNumber", OperationType = GEMOperationType.Select)]
        public Xevprm_message_PK GetEntityByMessageNumber(string message_number)
        {
            DateTime methodStart = DateTime.Now;
            Xevprm_message_PK entity = null;

            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();
                parameters.Add(new GEMDbParameter("message_number", message_number, DbType.String, ParameterDirection.Input));

                entity = base.ExecuteProcedureReturnEntity(parameters);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            // Logging operation if applicable
            base.LogOperation(methodStart);

            return entity;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_MESSAGE_GetEntityByMA", OperationType = GEMOperationType.Select)]
        public Xevprm_message_PK GetEntityByMA(int? ma_fk)
        {
            DateTime methodStart = DateTime.Now;
            Xevprm_message_PK entity = null;
            if (ma_fk == null) return null;
            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();
                parameters.Add(new GEMDbParameter("ma_fk", ma_fk, DbType.String, ParameterDirection.Input));

                entity = base.ExecuteProcedureReturnEntity(parameters);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            // Logging operation if applicable
            base.LogOperation(methodStart);

            return entity;
        }

        #endregion

        #region ICRUDOperations<Activity_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_MESSAGE_GetEntity", OperationType = GEMOperationType.Select)]
        public override Xevprm_message_PK GetEntity<PKType>(PKType entityId)
        {
            return base.GetEntity<PKType>(entityId);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_MESSAGE_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Xevprm_message_PK> GetEntities()
        {
            return base.GetEntities();
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_MESSAGE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Xevprm_message_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_MESSAGE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Xevprm_message_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
        {
            return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_MESSAGE_Save", OperationType = GEMOperationType.Save)]
        public override Xevprm_message_PK Save(Xevprm_message_PK entity)
        {
            return base.Save(entity);
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_MESSAGE_Delete", OperationType = GEMOperationType.Delete)]
        public override void Delete<PKType>(PKType entityId)
        {
            base.Delete<PKType>(entityId);
        }

        public override List<Xevprm_message_PK> SaveCollection(List<Xevprm_message_PK> entities)
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
