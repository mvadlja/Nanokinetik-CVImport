// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:42:58
// Description:	GEM2 Generated class for table SSI.dbo.RS_SCLF_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "RS_SCLF_MN")]
    //[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Rs_sclf_mn_PK
	{
		private Int32? _rs_sclf_mn_PK;
		private Int32? _sclf_FK;
		private Int32? _rs_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? rs_sclf_mn_PK
		{
			get { return _rs_sclf_mn_PK; }
			set { _rs_sclf_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? sclf_FK
		{
			get { return _sclf_FK; }
			set { _sclf_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? rs_FK
		{
			get { return _rs_FK; }
			set { _rs_FK = value; }
		}

		#endregion

		public Rs_sclf_mn_PK() { }
		public Rs_sclf_mn_PK(Int32? rs_sclf_mn_PK, Int32? sclf_FK, Int32? rs_FK)
		{
			this.rs_sclf_mn_PK = rs_sclf_mn_PK;
			this.sclf_FK = sclf_FK;
			this.rs_FK = rs_FK;
		}
	}

	public interface IRs_sclf_mn_PKOperations : ICRUDOperations<Rs_sclf_mn_PK>
	{

	}
}
