// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	17.10.2011. 16:25:20
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORGANIZATION_ROLE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ORGANIZATION_ROLE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Role_org_PK
	{
		private Int32? _role_org_PK;
		private String _role_number;
		private String _role_name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? role_org_PK
		{
			get { return _role_org_PK; }
			set { _role_org_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String role_number
		{
			get { return _role_number; }
			set { _role_number = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String role_name
		{
			get { return _role_name; }
			set { _role_name = value; }
		}

		#endregion

		public Role_org_PK() { }
		public Role_org_PK(Int32? role_org_PK, String role_number, String role_name)
		{
			this.role_org_PK = role_org_PK;
			this.role_number = role_number;
			this.role_name = role_name;
		}
	}

	public interface IRole_org_PKOperations : ICRUDOperations<Role_org_PK>
	{
        List<Role_org_PK> GetAssignedEntitiesByOrganization(int organizationPk);
        List<Role_org_PK> GetAvailableEntitiesByOrganization(int organizationPk);
	}
}
