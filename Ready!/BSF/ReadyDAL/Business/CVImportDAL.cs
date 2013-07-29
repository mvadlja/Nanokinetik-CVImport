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

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_CVImport_ImportData", OperationType = GEMOperationType.Select)]
        public DataSet ImportData(string data, string cvName)
        {
            DateTime methodStart = DateTime.Now;
            DataSet ds = null;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("Data", data, DbType.Xml, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("CVName", data, DbType.String, ParameterDirection.Input));

                ds = base.ExecuteProcedureReturnDataSet(parameters);

                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return ds;
        }
    }
}
