// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:51:51
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_AR_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PP_AR_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Pp_ar_mn_PK
	{
		private Int32? _pp_ar_mn_PK;
		private Int32? _admin_route_FK;
		private Int32? _pharmaceutical_product_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? pp_ar_mn_PK
		{
			get { return _pp_ar_mn_PK; }
			set { _pp_ar_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? admin_route_FK
		{
			get { return _admin_route_FK; }
			set { _admin_route_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? pharmaceutical_product_FK
		{
			get { return _pharmaceutical_product_FK; }
			set { _pharmaceutical_product_FK = value; }
		}

		#endregion

		public Pp_ar_mn_PK() { }
		public Pp_ar_mn_PK(Int32? pp_ar_mn_PK, Int32? admin_route_FK, Int32? pharmaceutical_product_FK)
		{
			this.pp_ar_mn_PK = pp_ar_mn_PK;
			this.admin_route_FK = admin_route_FK;
			this.pharmaceutical_product_FK = pharmaceutical_product_FK;
		}
	}

	public interface IPp_ar_mn_PKOperations : ICRUDOperations<Pp_ar_mn_PK>
	{
        List<Pp_ar_mn_PK> GetAdminRoutesByPPPK(Int32? pharmaceutical_product_FK);

        void DeleteByPharmaceuticalProduct(int pharmaceuticalProductPk);
    }
}
