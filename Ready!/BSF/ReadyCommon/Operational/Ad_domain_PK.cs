// ======================================================================================================================
// Author:		space-monkey\dpetek
// Create date:	15.5.2012. 11:16:09
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AD_DOMAIN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "AD_DOMAIN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Ad_domain_PK
	{
		private Int32? _ad_domain_PK;
		private String _domain_alias;
		private String _domain_connection_string;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? ad_domain_PK
		{
			get { return _ad_domain_PK; }
			set { _ad_domain_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String domain_alias
		{
			get { return _domain_alias; }
			set { _domain_alias = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String domain_connection_string
		{
			get { return _domain_connection_string; }
			set { _domain_connection_string = value; }
		}

		#endregion

		public Ad_domain_PK() { }
		public Ad_domain_PK(Int32? ad_domain_PK, String domain_alias, String domain_connection_string)
		{
			this.ad_domain_PK = ad_domain_PK;
			this.domain_alias = domain_alias;
			this.domain_connection_string = domain_connection_string;
		}
	}

	public interface IAd_domain_PKOperations : ICRUDOperations<Ad_domain_PK>
	{

	}
}
