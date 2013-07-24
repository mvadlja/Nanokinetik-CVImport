// ======================================================================================================================
// Author:		KIKI-PC\Alan
// Create date:	11.4.2012. 9:45:03
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.RECIEVED_MESSAGE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Recieved_message_PKDAL : GEMDataAccess<Recieved_message_PK>, IRecieved_message_PKOperations
	{
		public Recieved_message_PKDAL() : base() { }
		public Recieved_message_PKDAL(string dataSourceId) : base(dataSourceId) { }

		#region IRecieved_message_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_GetNotProcessedEntitiesPks", OperationType = GEMOperationType.Select)]
        public List<int> GetNotProcessedEntitiesPks(ReceivedMessageType receivedMessageType)
        {
            DateTime methodStart = DateTime.Now;
            var result = new List<int>();

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("ReceivedMessageType", (int)receivedMessageType, DbType.Int32, ParameterDirection.Input));

                DataSet messagePks = base.ExecuteProcedureReturnDataSet(parameters);

                if (messagePks != null &&
                    messagePks.Tables.Count > 0 &&
                    messagePks.Tables[0].Columns.Contains("recieved_message_PK") &&
                    messagePks.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in messagePks.Tables[0].Rows)
                    {
                        if (row["recieved_message_PK"] != DBNull.Value)
                        {
                            result.Add((int)row["recieved_message_PK"]);
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_GetNotProcessedMessages", OperationType = GEMOperationType.Select)]
        public List<Recieved_message_PK> GetNotProcessedMessages()
        {
            DateTime methodStart = DateTime.Now;
            List<Recieved_message_PK> entities = new List<Recieved_message_PK>();
            try
            {
                // Generating order by clause

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();


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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_GetMessagesByTimeSpan", OperationType = GEMOperationType.Select)]
        public List<Recieved_message_PK> GetMessagesByTimeSpan(DateTime periodStart, DateTime periodEnd)
        {
            DateTime methodStart = DateTime.Now;
            List<Recieved_message_PK> entities = new List<Recieved_message_PK>();
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_RECIEVED_MESSAGE_GetEntitiesDataSet]", OperationType = GEMOperationType.Select)]
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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_RECEIVED_MESSAGE_GetListFormDataSet]", OperationType = GEMOperationType.Select)]
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

		#region ICRUDOperations<Recieved_message_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_GetEntity", OperationType = GEMOperationType.Select)]
		public override Recieved_message_PK GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Recieved_message_PK> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<Recieved_message_PK> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

	

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_Save", OperationType = GEMOperationType.Save)]
		public override Recieved_message_PK Save(Recieved_message_PK entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<Recieved_message_PK> SaveCollection(List<Recieved_message_PK> entities)
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
