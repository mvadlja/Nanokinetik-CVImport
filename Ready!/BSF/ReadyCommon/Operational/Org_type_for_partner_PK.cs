// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	17.10.2011. 16:25:20
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORG_TYPE_FOR_PARTNER
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ORG_TYPE_FOR_PARTNER")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Org_type_for_partner_PK
	{
		private Int32? _org_type_for_partner_PK;
		private String _org_type_name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? org_type_for_partner_PK
		{
			get { return _org_type_for_partner_PK; }
			set { _org_type_for_partner_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String org_type_name
		{
			get { return _org_type_name; }
			set { _org_type_name = value; }
		}

		#endregion

		public Org_type_for_partner_PK() { }
		public Org_type_for_partner_PK(Int32? org_type_for_partner_PK, String org_type_name)
		{
			this.org_type_for_partner_PK = org_type_for_partner_PK;
			this.org_type_name = org_type_name;
		}
	}

	public interface IOrg_type_for_partner_PKOperations : ICRUDOperations<Org_type_for_partner_PK>
	{

	}
}
