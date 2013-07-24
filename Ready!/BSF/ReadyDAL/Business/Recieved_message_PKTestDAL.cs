// ======================================================================================================================
// Author:		KIKI-PC\Alan
// Create date:	11.4.2012. 9:45:03
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.RECIEVED_MESSAGE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	public class Recieved_message_PKTestDAL : GEMDataAccess<Recieved_message_PKTest>, IRecieved_message_PKTestOperations
	{
		public Recieved_message_PKTestDAL() : base() { }
        public Recieved_message_PKTestDAL(string dataSourceId) : base(dataSourceId) { }

		#region IRecieved_message_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_GetNotProcessedMessageIDs", OperationType = GEMOperationType.Select)]
        public List<int> GetNotProcessedMessageIDs()
        {
            DateTime methodStart = DateTime.Now;
            List<int> result = new List<int>();

            try
            {
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                DataSet messageIDs = base.ExecuteProcedureReturnDataSet(parameters);

                if (messageIDs != null &&
                    messageIDs.Tables.Count > 0 &&
                    messageIDs.Tables[0].Columns.Contains("recieved_message_PK") &&
                    messageIDs.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in messageIDs.Tables[0].Rows)
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
        public List<Recieved_message_PKTest> GetNotProcessedMessages()
        {
            DateTime methodStart = DateTime.Now;
            List<Recieved_message_PKTest> entities = new List<Recieved_message_PKTest>();
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


		#endregion

		#region ICRUDOperations<Recieved_message_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_GetEntity", OperationType = GEMOperationType.Select)]
        public override Recieved_message_PKTest GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<Recieved_message_PKTest> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<Recieved_message_PKTest> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<Recieved_message_PKTest> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_Save", OperationType = GEMOperationType.Save)]
        public override Recieved_message_PKTest Save(Recieved_message_PKTest entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_RECIEVED_MESSAGE_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

        public override List<Recieved_message_PKTest> SaveCollection(List<Recieved_message_PKTest> entities)
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
