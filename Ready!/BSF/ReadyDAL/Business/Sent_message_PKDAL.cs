// ======================================================================================================================
// Author:		KIKI-PC\Alan
// Create date:	27.4.2012. 14:38:34
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SENT_MESSAGE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Sent_message_PKDAL : GEMDataAccess<Sent_message_PK>, ISent_message_PKOperations
	{
		public Sent_message_PKDAL() : base() { }
		public Sent_message_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region ISent_message_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_SENT_MESSAGE_GetEntitiesDataSet]", OperationType = GEMOperationType.Select)]
        public DataSet GetEntitiesDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SENT_MESSAGE_GetMessagesByTimeSpan", OperationType = GEMOperationType.Select)]
        public List<Sent_message_PK> GetMessagesByTimeSpan(DateTime periodStart, DateTime periodEnd)
        {
            DateTime methodStart = DateTime.Now;
            List<Sent_message_PK> entities = new List<Sent_message_PK>();
            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();
                parameters.Add(new GEMDbParameter("startDate", periodStart, DbType.DateTime, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("endDate", periodEnd, DbType.DateTime, ParameterDirection.Input));
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_SENT_MESSAGE_GetDataForTimeStatsByTimeSpan]", OperationType = GEMOperationType.Select)]
        public DataSet GetDataForTimeStatsByTimeSpan(DateTime periodStart, DateTime periodEnd)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("startDate", periodStart, DbType.DateTime, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("endDate", periodEnd, DbType.DateTime, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_SENT_MESSAGE_GetMDNDataForTimeStatsByTimeSpan]", OperationType = GEMOperationType.Select)]
        public DataSet GetMDNDataForTimeStatsByTimeSpan(DateTime periodStart, DateTime periodEnd)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("startDate", periodStart, DbType.DateTime, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("endDate", periodEnd, DbType.DateTime, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_SENT_MESSAGE_GetACKDataForTimeStatsByTimeSpan]", OperationType = GEMOperationType.Select)]
        public DataSet GetACKDataForTimeStatsByTimeSpan(DateTime periodStart, DateTime periodEnd)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("startDate", periodStart, DbType.DateTime, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("endDate", periodEnd, DbType.DateTime, ParameterDirection.Input));

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


        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_SENT_MESSAGE_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

		#region ICRUDOperations<Sent_message_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SENT_MESSAGE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Sent_message_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SENT_MESSAGE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Sent_message_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SENT_MESSAGE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<Sent_message_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SENT_MESSAGE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<Sent_message_PK> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SENT_MESSAGE_Save", OperationType = GEMOperationType.Save)]
		public override Sent_message_PK Save(Sent_message_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_SENT_MESSAGE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Sent_message_PK> SaveCollection(List<Sent_message_PK> entities)
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
