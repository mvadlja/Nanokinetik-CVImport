// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	6.12.2011. 15:38:07
// Description:	GEM2 Generated class for table SSI.dbo.SUBSTANCE_SUBSTANCE_NAME_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SUBSTANCE_SUBSTANCE_NAME_MN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Substance_substance_name_mn_PK
	{
		private Int32? _substance_substance_name_mn_PK;
		private Int32? _substance_FK;
		private Int32? _substance_name_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? substance_substance_name_mn_PK
		{
			get { return _substance_substance_name_mn_PK; }
			set { _substance_substance_name_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substance_FK
		{
			get { return _substance_FK; }
			set { _substance_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substance_name_FK
		{
			get { return _substance_name_FK; }
			set { _substance_name_FK = value; }
		}

		#endregion

		public Substance_substance_name_mn_PK() { }
		public Substance_substance_name_mn_PK(Int32? substance_substance_name_mn_PK, Int32? substance_FK, Int32? substance_name_FK)
		{
			this.substance_substance_name_mn_PK = substance_substance_name_mn_PK;
			this.substance_FK = substance_FK;
			this.substance_name_FK = substance_name_FK;
		}
	}

	public interface ISubstance_substance_name_mn_PKOperations : ICRUDOperations<Substance_substance_name_mn_PK>
	{

	}
}
