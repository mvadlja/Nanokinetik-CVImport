// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 15:09:13
// Description:	GEM2 Generated class for table SSI.dbo.OFFICIAL_NAME_DOMAIN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "OFFICIAL_NAME_DOMAIN")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class On_domain_PK
	{
		private Int32? _on_domain_PK;
		private String _name_domain;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? on_domain_PK
		{
			get { return _on_domain_PK; }
			set { _on_domain_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name_domain
		{
			get { return _name_domain; }
			set { _name_domain = value; }
		}

		#endregion

		public On_domain_PK() { }
		public On_domain_PK(Int32? on_domain_PK, String name_domain)
		{
			this.on_domain_PK = on_domain_PK;
			this.name_domain = name_domain;
		}
	}

	public interface IOn_domain_PKOperations : ICRUDOperations<On_domain_PK>
	{

	}
}
