using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model.Business
{
    public class CVImportDAL : GEMDataAccess<XlsImport>
    {
        public CVImportDAL() : base() { }
        public CVImportDAL(string dataSourceId) : base(dataSourceId) { }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_CVImport_ImportInitialData", OperationType = GEMOperationType.Select)]
        public bool ImportInitialData(string data, string cvName, int? responsibleUserID)
        {
            DateTime methodStart = DateTime.Now;
            bool isSuccessful = false;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("Data", data, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("CVName", cvName, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("ResponsibleUserID", responsibleUserID, DbType.Int32, ParameterDirection.Input));

                isSuccessful = (bool)base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return isSuccessful;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_CVImport_GetComparedData", OperationType = GEMOperationType.Select)]
        public DataSet GetComparedData(int? requestUserID, string actionName, int currentPage, int recordsPerPage, out int totalRecordsCount)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;
            totalRecordsCount = 0;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("RequestUserID", requestUserID, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("ActionName", actionName, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("CurrentPage", currentPage, DbType.Int32, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("RecordsPerPage", recordsPerPage, DbType.Int32, ParameterDirection.Input));

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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_CVImport_ApplyImportData", OperationType = GEMOperationType.Select)]
        public bool ApplyImportData(string data, string cvName, int? requestUserID)
        {
            DateTime methodStart = DateTime.Now;
            bool isSuccessful = false;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("Data", data, DbType.String, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("RequestUserID", requestUserID, DbType.Int32, ParameterDirection.Input));

                isSuccessful = (bool)base.ExecuteProcedureReturnScalar(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return isSuccessful;
        }
    }
}
