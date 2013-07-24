// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:40:35
// Description:	GEM2 Generated class for table SSI.dbo.RS_CHEMICAL_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "RS_CHEMICAL_MN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Rs_chemical_mn_PK
	{
		private Int32? _rs_chemical_mn_PK;
		private Int32? _rs_FK;
		private Int32? _chemical_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? rs_chemical_mn_PK
		{
			get { return _rs_chemical_mn_PK; }
			set { _rs_chemical_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? rs_FK
		{
			get { return _rs_FK; }
			set { _rs_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? chemical_FK
		{
			get { return _chemical_FK; }
			set { _chemical_FK = value; }
		}

		#endregion

		public Rs_chemical_mn_PK() { }
		public Rs_chemical_mn_PK(Int32? rs_chemical_mn_PK, Int32? rs_FK, Int32? chemical_FK)
		{
			this.rs_chemical_mn_PK = rs_chemical_mn_PK;
			this.rs_FK = rs_FK;
			this.chemical_FK = chemical_FK;
		}
	}

	public interface IRs_chemical_mn_PKOperations : ICRUDOperations<Rs_chemical_mn_PK>
	{

	}
}
