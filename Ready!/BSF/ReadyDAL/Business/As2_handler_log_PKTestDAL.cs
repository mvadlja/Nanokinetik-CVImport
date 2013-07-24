// ======================================================================================================================
// Author:		Kiki-PC\Kiki
// Create date:	8/7/2012 9:46:44 AM
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AS2_HANDLER_LOG
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    public class As2_handler_log_PKTestDAL : GEMDataAccess<As2_handler_log_PKTest>, IAs2_handler_log_PKTestOperations
	{
		public As2_handler_log_PKTestDAL() : base() { }
        public As2_handler_log_PKTestDAL(string dataSourceId) : base(dataSourceId) { }

		#region IAs2_handler_log_PKOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "[proc_AS2_HANDLER_LOG_GetReportDataSet]", OperationType = GEMOperationType.Select)]
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

		#endregion

		#region ICRUDOperations<As2_handler_log_PK> Members

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AS2_HANDLER_LOG_GetEntity", OperationType = GEMOperationType.Select)]
        public override As2_handler_log_PKTest GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AS2_HANDLER_LOG_GetEntities", OperationType = GEMOperationType.Select)]
        public override List<As2_handler_log_PKTest> GetEntities()
		{
			return base.GetEntities();
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AS2_HANDLER_LOG_GetEntitiesWP", OperationType = GEMOperationType.Select)]
        public override List<As2_handler_log_PKTest> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AS2_HANDLER_LOG_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
        public override List<As2_handler_log_PKTest> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
			return base.GetEntities(pageNumber, pageSize, orderByConditions, out totalRecordsCount);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AS2_HANDLER_LOG_Save", OperationType = GEMOperationType.Save)]
        public override As2_handler_log_PKTest Save(As2_handler_log_PKTest entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_AS2_HANDLER_LOG_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

        public override List<As2_handler_log_PKTest> SaveCollection(List<As2_handler_log_PKTest> entities)
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
