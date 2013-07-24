// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:53:22
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_ADMINISTRATION_ROUTE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PP_ADMINISTRATION_ROUTE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Adminroute_PK
	{
        private Int32? _adminroute_PK;
        private String _adminroutecode;
        private Int32? _resolutionmode;
        private String _ev_code;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? adminroute_PK
        {
            get { return _adminroute_PK; }
            set { _adminroute_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String adminroutecode
        {
            get { return _adminroutecode; }
            set { _adminroutecode = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? resolutionmode
        {
            get { return _resolutionmode; }
            set { _resolutionmode = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ev_code
        {
            get { return _ev_code; }
            set { _ev_code = value; }
        }

        #endregion

        public Adminroute_PK() { }
        public Adminroute_PK(Int32? adminroute_PK, String adminroutecode, Int32? resolutionmode, String ev_code)
        {
            this.adminroute_PK = adminroute_PK;
            this.adminroutecode = adminroutecode;
            this.resolutionmode = resolutionmode;
            this.ev_code = ev_code;
        }
    }

	public interface IAdminroute_PKOperations : ICRUDOperations<Adminroute_PK>
	{
        DataSet GetAdminRoutesByCodeSearcher(String code, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	    List<Adminroute_PK> GetEntitiesByPharmaceuticalProduct(int pharmaceuticalProductPk);
	}
}
