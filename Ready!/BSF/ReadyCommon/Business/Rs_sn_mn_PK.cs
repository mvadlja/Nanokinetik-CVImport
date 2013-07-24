// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:43:11
// Description:	GEM2 Generated class for table SSI.dbo.RS_SN_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "RS_SN_MN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Rs_sn_mn_PK
	{
		private Int32? _rs_sn_mn_PK;
		private Int32? _rs_FK;
		private Int32? _substance_name_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? rs_sn_mn_PK
		{
			get { return _rs_sn_mn_PK; }
			set { _rs_sn_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? rs_FK
		{
			get { return _rs_FK; }
			set { _rs_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substance_name_FK
		{
			get { return _substance_name_FK; }
			set { _substance_name_FK = value; }
		}

		#endregion

		public Rs_sn_mn_PK() { }
		public Rs_sn_mn_PK(Int32? rs_sn_mn_PK, Int32? rs_FK, Int32? substance_name_FK)
		{
			this.rs_sn_mn_PK = rs_sn_mn_PK;
			this.rs_FK = rs_FK;
			this.substance_name_FK = substance_name_FK;
		}
	}

	public interface IRs_sn_mn_PKOperations : ICRUDOperations<Rs_sn_mn_PK>
	{

	}
}
