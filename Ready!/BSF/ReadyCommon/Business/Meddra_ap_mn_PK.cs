// ======================================================================================================================
// Author:		space-monkey\dpetek
// Create date:	14.3.2012. 11:33:26
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.MEDDRA_AP_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "MEDDRA_AP_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Meddra_ap_mn_PK
	{
		private Int32? _meddra_ap_mn_PK;
		private Int32? _ap_FK;
		private Int32? _meddra_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? meddra_ap_mn_PK
		{
			get { return _meddra_ap_mn_PK; }
			set { _meddra_ap_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ap_FK
		{
			get { return _ap_FK; }
			set { _ap_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? meddra_FK
		{
			get { return _meddra_FK; }
			set { _meddra_FK = value; }
		}

		#endregion

		public Meddra_ap_mn_PK() { }
		public Meddra_ap_mn_PK(Int32? meddra_ap_mn_PK, Int32? ap_FK, Int32? meddra_FK)
		{
			this.meddra_ap_mn_PK = meddra_ap_mn_PK;
			this.ap_FK = ap_FK;
			this.meddra_FK = meddra_FK;
		}
	}

	public interface IMeddra_ap_mn_PKOperations : ICRUDOperations<Meddra_ap_mn_PK>
	{
        DataSet GetMEDDRAByAP(Int32? ap_FK);
        void DeleteMeddraByAP(Int32? ap_FK);
        void DeleteMNByMEDDRA(Int32? meddra_FK);
	}
}
