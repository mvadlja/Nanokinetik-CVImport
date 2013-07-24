// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:44:30
// Description:	GEM2 Generated class for table SSI.dbo.SN_ON_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SN_ON_MN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Sn_on_mn_PK
	{
		private Int32? _sn_on_mn_PK;
		private Int32? _official_name_FK;
		private Int32? _substance_name_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? sn_on_mn_PK
		{
			get { return _sn_on_mn_PK; }
			set { _sn_on_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? official_name_FK
		{
			get { return _official_name_FK; }
			set { _official_name_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substance_name_FK
		{
			get { return _substance_name_FK; }
			set { _substance_name_FK = value; }
		}

		#endregion

		public Sn_on_mn_PK() { }
		public Sn_on_mn_PK(Int32? sn_on_mn_PK, Int32? official_name_FK, Int32? substance_name_FK)
		{
			this.sn_on_mn_PK = sn_on_mn_PK;
			this.official_name_FK = official_name_FK;
			this.substance_name_FK = substance_name_FK;
		}
	}

	public interface ISn_on_mn_PKOperations : ICRUDOperations<Sn_on_mn_PK>
	{
        
	}
}
