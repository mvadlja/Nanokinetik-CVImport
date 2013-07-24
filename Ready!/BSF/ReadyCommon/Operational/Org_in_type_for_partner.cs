// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	17.10.2011. 16:25:20
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORG_IN_TYPE_FOR_PARTNER
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ORG_IN_TYPE_FOR_PARTNER")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Org_in_type_for_partner
	{
		private Int32? _org_in_type_for_partner_ID;
		private Int32? _organization_FK;
		private Int32? _org_type_for_partner_FK;
        private Int32? _product_FK;
	    private String _partnerName;
	    private String _partnerTypeName;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? org_in_type_for_partner_ID
		{
			get { return _org_in_type_for_partner_ID; }
			set { _org_in_type_for_partner_ID = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? organization_FK
		{
			get { return _organization_FK; }
			set { _organization_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? org_type_for_partner_FK
		{
			get { return _org_type_for_partner_FK; }
			set { _org_type_for_partner_FK = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? product_FK
        {
            get { return _product_FK; }
            set { _product_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String PartnerName
        {
            get { return _partnerName; }
            set { _partnerName = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String PartnerTypeName
        {
            get { return _partnerTypeName; }
            set { _partnerTypeName = value; }
        }

		#endregion

		public Org_in_type_for_partner() { }
        public Org_in_type_for_partner(Int32? org_in_type_for_partner_ID, Int32? organization_FK, Int32? org_type_for_partner_FK, Int32? product_FK, String partnerName, String partnerTypeName)
		{
			this.org_in_type_for_partner_ID = org_in_type_for_partner_ID;
			this.organization_FK = organization_FK;
			this.org_type_for_partner_FK = org_type_for_partner_FK;
            this.product_FK = product_FK;
            this.PartnerName = partnerName;
            this.PartnerTypeName = partnerTypeName;
		}
	}

	public interface IOrg_in_type_for_partnerOperations : ICRUDOperations<Org_in_type_for_partner>
	{
        List<Org_in_type_for_partner> GetOrganisationPartnerTypesByOrganisationPK(Int32? organization_FK, Int32? product_FK);
        DataSet GetOrganizationsByPartnerRoleSearcher(String role_name, String name_org, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        List<Org_in_type_for_partner> GetEntitiesByProduct(int productPk);
        void DeleteByProduct(int productPk);
	}
}
