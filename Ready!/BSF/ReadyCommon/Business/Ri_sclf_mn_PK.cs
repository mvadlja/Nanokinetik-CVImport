// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:39:55
// Description:	GEM2 Generated class for table SSI.dbo.RI_SCLF_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "RI_SCLF_MN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Ri_sclf_mn_PK
	{
		private Int32? _ri_sclf_mn_PK;
		private Int32? _ref_info_FK;
		private Int32? _sclf_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? ri_sclf_mn_PK
		{
			get { return _ri_sclf_mn_PK; }
			set { _ri_sclf_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ref_info_FK
		{
			get { return _ref_info_FK; }
			set { _ref_info_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? sclf_FK
		{
			get { return _sclf_FK; }
			set { _sclf_FK = value; }
		}

		#endregion

		public Ri_sclf_mn_PK() { }
		public Ri_sclf_mn_PK(Int32? ri_sclf_mn_PK, Int32? ref_info_FK, Int32? sclf_FK)
		{
			this.ri_sclf_mn_PK = ri_sclf_mn_PK;
			this.ref_info_FK = ref_info_FK;
			this.sclf_FK = sclf_FK;
		}
	}

	public interface IRi_sclf_mn_PKOperations : ICRUDOperations<Ri_sclf_mn_PK>
	{

	}
}
