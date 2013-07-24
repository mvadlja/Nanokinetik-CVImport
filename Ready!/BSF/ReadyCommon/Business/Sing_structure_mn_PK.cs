// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:44:19
// Description:	GEM2 Generated class for table SSI.dbo.SING_STRUCTURE_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SING_STRUCTURE_MN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Sing_structure_mn_PK
	{
		private Int32? _sing_structure_mn_PK;
		private Int32? _sing_FK;
		private Int32? _structure_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? sing_structure_mn_PK
		{
			get { return _sing_structure_mn_PK; }
			set { _sing_structure_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? sing_FK
		{
			get { return _sing_FK; }
			set { _sing_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? structure_FK
		{
			get { return _structure_FK; }
			set { _structure_FK = value; }
		}

		#endregion

		public Sing_structure_mn_PK() { }
		public Sing_structure_mn_PK(Int32? sing_structure_mn_PK, Int32? sing_FK, Int32? structure_FK)
		{
			this.sing_structure_mn_PK = sing_structure_mn_PK;
			this.sing_FK = sing_FK;
			this.structure_FK = structure_FK;
		}
	}

	public interface ISing_structure_mn_PKOperations : ICRUDOperations<Sing_structure_mn_PK>
	{

	}
}
