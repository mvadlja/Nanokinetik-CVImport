// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	6.12.2011. 15:30:44
// Description:	GEM2 Generated class for table SSI.dbo.VERSION_SUBSTANCE_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "VERSION_SUBSTANCE_MN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Version_substance_mn_PK
	{
		private Int32? _version_substance_mn_PK;
		private Int32? _version_FK;
		private Int32? _substance_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? version_substance_mn_PK
		{
			get { return _version_substance_mn_PK; }
			set { _version_substance_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? version_FK
		{
			get { return _version_FK; }
			set { _version_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substance_FK
		{
			get { return _substance_FK; }
			set { _substance_FK = value; }
		}

		#endregion

		public Version_substance_mn_PK() { }
		public Version_substance_mn_PK(Int32? version_substance_mn_PK, Int32? version_FK, Int32? substance_FK)
		{
			this.version_substance_mn_PK = version_substance_mn_PK;
			this.version_FK = version_FK;
			this.substance_FK = substance_FK;
		}
	}

	public interface IVersion_substance_mn_PKOperations : ICRUDOperations<Version_substance_mn_PK>
	{

	}
}
