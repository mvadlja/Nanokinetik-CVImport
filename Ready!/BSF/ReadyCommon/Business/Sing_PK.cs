// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:08:22
// Description:	GEM2 Generated class for table SSI.dbo.SING
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SING")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Sing_PK
	{
		private Int32? _sing_PK;
		private Int32? _chemical_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? sing_PK
		{
			get { return _sing_PK; }
			set { _sing_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? chemical_FK
		{
			get { return _chemical_FK; }
			set { _chemical_FK = value; }
		}

		#endregion

		public Sing_PK() { }
		public Sing_PK(Int32? sing_PK, Int32? chemical_FK)
		{
			this.sing_PK = sing_PK;
			this.chemical_FK = chemical_FK;
		}
	}

	public interface ISing_PKOperations : ICRUDOperations<Sing_PK>
	{

	}
}
