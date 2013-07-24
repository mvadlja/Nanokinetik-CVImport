using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model.Business
{

    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class XlsImport
    {
        private int? _xlsImportData_PK;

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? XlsImportData_PK
        {
            get { return _xlsImportData_PK; }
            set { _xlsImportData_PK = value; }
        }
    }
}

