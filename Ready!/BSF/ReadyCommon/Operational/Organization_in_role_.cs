// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	17.10.2011. 16:25:20
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORGANIZATION_IN_ROLE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ORGANIZATION_IN_ROLE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Organization_in_role_
	{
		private Int32? _organization_in_role_ID;
		private Int32? _organization_FK;
		private Int32? _role_org_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? organization_in_role_ID
		{
			get { return _organization_in_role_ID; }
			set { _organization_in_role_ID = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? organization_FK
		{
			get { return _organization_FK; }
			set { _organization_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? role_org_FK
		{
			get { return _role_org_FK; }
			set { _role_org_FK = value; }
		}

		#endregion

		public Organization_in_role_() { }
		public Organization_in_role_(Int32? organization_in_role_ID, Int32? organization_FK, Int32? role_org_FK)
		{
			this.organization_in_role_ID = organization_in_role_ID;
			this.organization_FK = organization_FK;
			this.role_org_FK = role_org_FK;
		}
	}

	public interface IOrganization_in_role_Operations : ICRUDOperations<Organization_in_role_>
	{
        List<Organization_in_role_> GetOrganisationInRolesByOrganisationPK(Int32? organization_FK);
        DataSet GetOrganizationsByRole(String role_name);
        DataSet GetOrganizationsByRoleSearcher(String role_name, String name, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	    void DeleteByOrganization(int organizationPk);
	}
}
