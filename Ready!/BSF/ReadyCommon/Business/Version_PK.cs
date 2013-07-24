// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:11:15
// Description:	GEM2 Generated class for table SSI.dbo.VERSION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "VERSION")]
    //[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Version_PK
	{
		private Int32? _version_PK;
		private Int32? _version_number;
		private String _effectve_date;
		private String _change_made;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? version_PK
		{
			get { return _version_PK; }
			set { _version_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? version_number
		{
			get { return _version_number; }
			set { _version_number = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String effectve_date
		{
			get { return _effectve_date; }
			set { _effectve_date = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String change_made
		{
			get { return _change_made; }
			set { _change_made = value; }
		}

		#endregion

		public Version_PK() { }
		public Version_PK(Int32? version_PK, Int32? version_number, String effectve_date, String change_made)
		{
			this.version_PK = version_PK;
			this.version_number = version_number;
			this.effectve_date = effectve_date;
			this.change_made = change_made;
		}
	}

	public interface IVersion_PKOperations : ICRUDOperations<Version_PK>
	{
        List<Version_PK> GetVERBySubstancePK(Int32? SubstancePK);

	}
}
