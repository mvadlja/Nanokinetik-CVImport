// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	19.10.2011. 12:00:31
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORG_IN_TYPE_FOR_MANUFACTURER
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ORG_IN_TYPE_FOR_MANUFACTURER")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Org_in_type_for_manufacturer
	{
		private Int32? _org_in_type_for_manufacturer_ID;
		private Int32? _organization_FK;
		private Int32? _org_type_for_manu_FK;
        private Int32? _product_FK;
        private Int32? _substance_FK;
	    private String _substanceName;
        private String _manufacturerName;
        private String _manufacturerTypeName;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? org_in_type_for_manufacturer_ID
		{
			get { return _org_in_type_for_manufacturer_ID; }
			set { _org_in_type_for_manufacturer_ID = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? organization_FK
		{
			get { return _organization_FK; }
			set { _organization_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? org_type_for_manu_FK
		{
			get { return _org_type_for_manu_FK; }
			set { _org_type_for_manu_FK = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? product_FK
        {
            get { return _product_FK; }
            set { _product_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? substance_FK
        {
            get { return _substance_FK; }
            set { _substance_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String SubstanceName
        {
            get { return _substanceName; }
            set { _substanceName = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ManufacturerName
        {
            get { return _manufacturerName; }
            set { _manufacturerName = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ManufacturerTypeName
        {
            get { return _manufacturerTypeName; }
            set { _manufacturerTypeName = value; }
        }

		#endregion

		public Org_in_type_for_manufacturer() { }
        public Org_in_type_for_manufacturer(Int32? org_in_type_for_manufacturer_ID, Int32? organization_FK, Int32? org_type_for_manu_FK, Int32? product_FK, Int32? substance_FK, String substanceName, String manufacturerName, String manufacturerTypeName)
		{
			this.org_in_type_for_manufacturer_ID = org_in_type_for_manufacturer_ID;
			this.organization_FK = organization_FK;
			this.org_type_for_manu_FK = org_type_for_manu_FK;
            this.product_FK = product_FK;
            this._substance_FK = substance_FK;
            this._substanceName = substanceName;
            this._manufacturerName = manufacturerName;
            this._manufacturerTypeName = manufacturerTypeName;
		}
	}

	public interface IOrg_in_type_for_manufacturerOperations : ICRUDOperations<Org_in_type_for_manufacturer>
	{
        List<Org_in_type_for_manufacturer> GetOrganisationManufacturerTypesByOrganisationPK(Int32? organization_FK, Int32? product_FK);
        List<Org_in_type_for_manufacturer> GetEntitiesByProduct(int productPk);
	    void DeleteByProduct(int productPk);
	}
}
