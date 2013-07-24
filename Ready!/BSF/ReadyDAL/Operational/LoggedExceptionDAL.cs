using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Kmis.Model
{
	public class LoggedExceptionDAL : GEMDataAccess<LoggedException>, ILoggedExceptionOperations
	{
		public LoggedExceptionDAL() : base() { }
		public LoggedExceptionDAL(string dataSourceId) : base(dataSourceId) { }

		#region ILoggedExceptionOperations Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LoggedExceptions_SearchEntries", OperationType = GEMOperationType.Select)]
        public DataSet SearchEntries(String username, String exceptionType, String dateFrom, String dateTill, String serverName, String uniqueKey, int pageNumber, int pageSize, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;

            try
            {
                // Input parameters validation
                //if (username == null || exceptionType == null || serverName == null) return ds;

                Dictionary<string, object> outputValues = new Dictionary<string, object>();
                List<GEMDbParameter> parameters = new List<GEMDbParameter>();

                Guid temp = Guid.Empty;

                if (username != null && username.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("Username", base.PrepareParameterValue(username), DbType.String, ParameterDirection.Input));
                if (exceptionType != null && exceptionType.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("ExceptionType", exceptionType, DbType.String, ParameterDirection.Input));
                if (serverName != null && serverName.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("ServerName", base.PrepareParameterValue(serverName), DbType.String, ParameterDirection.Input));                
                if (dateFrom != null && dateFrom.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("DateFrom", (DateTime?)Convert.ToDateTime(base.PrepareParameterValue(dateFrom)), DbType.DateTime, ParameterDirection.Input));
                if (dateTill != null && dateTill.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("DateTill", (DateTime?)Convert.ToDateTime(base.PrepareParameterValue(dateTill)), DbType.DateTime, ParameterDirection.Input));
                if (serverName != null && serverName.ToString() != String.Empty)
                    parameters.Add(new GEMDbParameter("ServerName", serverName, DbType.String, ParameterDirection.Input));
                if (uniqueKey != null && uniqueKey.ToString() != String.Empty && Guid.TryParse(uniqueKey, out temp))
                    parameters.Add(new GEMDbParameter("UniqueKey", base.PrepareParameterValue(uniqueKey), DbType.Guid, ParameterDirection.Input));

                parameters.Add(new GEMDbParameter("PageNum", pageNumber, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("PageSize", pageSize, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LoggedExceptions_GetDistinctExceptionTypes", OperationType = GEMOperationType.Select)]
        public DataSet GetDistinctExceptionTypes()
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

		#region ICRUDOperations<LoggedException> Members

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LoggedExceptions_GetEntry", OperationType = GEMOperationType.Select)]
		public override LoggedException GetEntity<PKType>(PKType entityId)
		{
			return base.GetEntity<PKType>(entityId);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LoggedExceptions_GetEntities", OperationType = GEMOperationType.Select)]
		public override List<LoggedException> GetEntities()
		{
            throw new NotImplementedException();
        }

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LoggedExceptions_GetEntitiesWP", OperationType = GEMOperationType.Select)]
		public override List<LoggedException> GetEntities(int pageNumber, int pageSize, out int totalRecordsCount)
		{
            throw new NotImplementedException();
        }

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LoggedExceptions_GetEntitiesWPS", OperationType = GEMOperationType.Select)]
		public override List<LoggedException> GetEntities(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount)
		{
            throw new NotImplementedException();
        }

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LoggedExceptions_Save", OperationType = GEMOperationType.Save)]
		public override LoggedException Save(LoggedException entity)
		{
			return base.Save(entity);
		}

		[GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_LoggedExceptions_Delete", OperationType = GEMOperationType.Delete)]
		public override void Delete<PKType>(PKType entityId)
		{
			base.Delete<PKType>(entityId);
		}

		public override List<LoggedException> SaveCollection(List<LoggedException> entities)
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
