// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	19.10.2011. 15:23:17
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AP_ORGANIZATION_DIST_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "AP_ORGANIZATION_DIST_MN", Active=true)]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Ap_organizatation_dist_mn_PK
	{
		private Int32? _ap_organizatation_dist_mn_PK;
		private Int32? _organization_FK;
		private Int32? _ap_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? ap_organizatation_dist_mn_PK
		{
			get { return _ap_organizatation_dist_mn_PK; }
			set { _ap_organizatation_dist_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? organization_FK
		{
			get { return _organization_FK; }
			set { _organization_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ap_FK
		{
			get { return _ap_FK; }
			set { _ap_FK = value; }
		}

		#endregion

		public Ap_organizatation_dist_mn_PK() { }
		public Ap_organizatation_dist_mn_PK(Int32? ap_organizatation_dist_mn_PK, Int32? organization_FK, Int32? ap_FK)
		{
			this.ap_organizatation_dist_mn_PK = ap_organizatation_dist_mn_PK;
			this.organization_FK = organization_FK;
			this.ap_FK = ap_FK;
		}
	}

	public interface IAp_organizatation_dist_mn_PKOperations : ICRUDOperations<Ap_organizatation_dist_mn_PK>
	{
        DataSet GetDistibutorByAP(Int32? ap_FK);
        List<Ap_organizatation_dist_mn_PK> GetAssignedDistributorsByAp(Int32? authorisedProductFk);
	}
}
