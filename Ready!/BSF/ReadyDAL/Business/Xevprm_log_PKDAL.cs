// ======================================================================================================================
// Author:		POSSIMUSIT-MATE\Mateo
// Create date:	28.3.2012. 10:37:24
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.XEVMPD_LOG
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Xevprm_log_PKDAL : GEMDataAccess<Xevprm_log_PK>, IXevprm_log_PKOperations
	{
		public Xevprm_log_PKDAL() : base() { }
		public Xevprm_log_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IXevprm_log_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_LOG_GetMessageSubmissionError", OperationType = GEMOperationType.Save)]
        public string GetMessageSubmissionError(Int32? xevprmMessagePk, string messageType)
        {
            DateTime methodStart = DateTime.Now;
            string result = null;

            try
            {
                var outputValues = new Dictionary<string, object>();
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("XevprmMessagePk", xevprmMessagePk, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("MessageType", messageType, DbType.String, ParameterDirection.Input));

                result = Convert.ToString(base.ExecuteProcedureReturnScalar(parameters));

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }
            return result;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_XEVPRM_LOG_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

        #region ICRUDOperations<Xevprm_log_PK> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_LOG_GetEntity", OperationType = GEMOperationType.Select)]
		public override Xevprm_log_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_LOG_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Xevprm_log_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_LOG_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Xevprm_log_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_LOG_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Xevprm_log_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_LOG_Save", OperationType = GEMOperationType.Save)]
		public override Xevprm_log_PK Save(Xevprm_log_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XEVPRM_LOG_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Xevprm_log_PK> SaveCollection(List<Xevprm_log_PK> entities)
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
