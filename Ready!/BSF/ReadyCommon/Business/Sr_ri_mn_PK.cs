// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:44:44
// Description:	GEM2 Generated class for table SSI.dbo.SR_RI_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SR_RI_MN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Sr_ri_mn_PK
	{
		private Int32? _sr_ri_mn_PK;
		private Int32? _ri_FK;
		private Int32? _sr_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? sr_ri_mn_PK
		{
			get { return _sr_ri_mn_PK; }
			set { _sr_ri_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ri_FK
		{
			get { return _ri_FK; }
			set { _ri_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? sr_FK
		{
			get { return _sr_FK; }
			set { _sr_FK = value; }
		}

		#endregion

		public Sr_ri_mn_PK() { }
		public Sr_ri_mn_PK(Int32? sr_ri_mn_PK, Int32? ri_FK, Int32? sr_FK)
		{
			this.sr_ri_mn_PK = sr_ri_mn_PK;
			this.ri_FK = ri_FK;
			this.sr_FK = sr_FK;
		}
	}

	public interface ISr_ri_mn_PKOperations : ICRUDOperations<Sr_ri_mn_PK>
	{

	}
}
