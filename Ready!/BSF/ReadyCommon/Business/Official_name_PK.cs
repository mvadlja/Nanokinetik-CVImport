// ======================================================================================================================
// Author:		ANTEC\Kiki
// Create date:	4.12.2011. 10:28:29
// Description:	GEM2 Generated class for table SSI.dbo.OFFICIAL_NAME
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "OFFICIAL_NAME")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Official_name_PK
	{
		private Int32? _official_name_PK;
		private Int32? _on_type_FK;
		private Int32? _on_status_FK;
		private String _on_status_changedate;
		private Int32? _on_jurisdiction_FK;
		private Int32? _on_domain_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? official_name_PK
		{
			get { return _official_name_PK; }
			set { _official_name_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? on_type_FK
		{
			get { return _on_type_FK; }
			set { _on_type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? on_status_FK
		{
			get { return _on_status_FK; }
			set { _on_status_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String on_status_changedate
		{
			get { return _on_status_changedate; }
			set { _on_status_changedate = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? on_jurisdiction_FK
		{
			get { return _on_jurisdiction_FK; }
			set { _on_jurisdiction_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? on_domain_FK
		{
			get { return _on_domain_FK; }
			set { _on_domain_FK = value; }
		}

		#endregion

		public Official_name_PK() { }
		public Official_name_PK(Int32? official_name_PK, Int32? on_type_FK, Int32? on_status_FK, String on_status_changedate, Int32? on_jurisdiction_FK, Int32? on_domain_FK)
		{
			this.official_name_PK = official_name_PK;
			this.on_type_FK = on_type_FK;
			this.on_status_FK = on_status_FK;
			this.on_status_changedate = on_status_changedate;
			this.on_jurisdiction_FK = on_jurisdiction_FK;
			this.on_domain_FK = on_domain_FK;
		}
	}

	public interface IOfficial_name_PKOperations : ICRUDOperations<Official_name_PK>
	{
        List<Official_name_PK> GetONBySNPK(Int32? SNPK);
	}
}
