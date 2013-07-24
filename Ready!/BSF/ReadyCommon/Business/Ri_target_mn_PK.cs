// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:40:22
// Description:	GEM2 Generated class for table SSI.dbo.RI_TARGET_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "RI_TARGET_MN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Ri_target_mn_PK
	{
		private Int32? _ri_target_mn_PK;
		private Int32? _ri_FK;
		private Int32? _target_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? ri_target_mn_PK
		{
			get { return _ri_target_mn_PK; }
			set { _ri_target_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ri_FK
		{
			get { return _ri_FK; }
			set { _ri_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? target_FK
		{
			get { return _target_FK; }
			set { _target_FK = value; }
		}

		#endregion

		public Ri_target_mn_PK() { }
		public Ri_target_mn_PK(Int32? ri_target_mn_PK, Int32? ri_FK, Int32? target_FK)
		{
			this.ri_target_mn_PK = ri_target_mn_PK;
			this.ri_FK = ri_FK;
			this.target_FK = target_FK;
		}
	}

	public interface IRi_target_mn_PKOperations : ICRUDOperations<Ri_target_mn_PK>
	{

	}
}
