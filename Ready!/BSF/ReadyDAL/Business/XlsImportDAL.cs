using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model.Business
{
    public class XlsImportDAL : GEMDataAccess<XlsImport>
    {
        public XlsImportDAL() : base() { }
        public XlsImportDAL(string dataSourceId) : base(dataSourceId) { }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XlsImport_ImportData", OperationType = GEMOperationType.Select)]
        public bool ImportData(string data, int? responsiblePerson_UserID)
        {
            DateTime methodStart = DateTime.Now;
            var importSuccessful = false;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("Data", data, DbType.Xml, ParameterDirection.Input));
                parameters.Add(new GEMDbParameter("ResponsiblePerson_UserID", responsiblePerson_UserID, DbType.Int32, ParameterDirection.Input));

                importSuccessful = (bool)base.ExecuteProcedureReturnScalar(parameters);


                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return importSuccessful;
        }

        [GEMOperationBindingAttribute(DataSourceId = "Default", ProcedureName = "proc_XlsImport_ValidateImportData", OperationType = GEMOperationType.Select)]
        public string ValidateData(string data)
        {
            DateTime methodStart = DateTime.Now;
            string validationMessage = String.Empty;

            try
            {
                var parameters = new List<GEMDbParameter>();

                parameters.Add(new GEMDbParameter("Data", data, DbType.Xml, ParameterDirection.Input));

                //validationMessage = (string)base.ExecuteProcedureReturnScalar(parameters);


                // Logging operation if applicable
                base.LogOperation(methodStart);
            }
            catch (Exception ex)
            {
                base.HandleException(ex);
            }

            return validationMessage;
        }
    }
}
