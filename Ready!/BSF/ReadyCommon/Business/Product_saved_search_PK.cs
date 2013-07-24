// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	24.2.2012. 13:18:08
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PRODUCT_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PRODUCT_SAVED_SEARCH")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Product_saved_search_PK
    {
        private Int32? _product_saved_search_PK;
        private String _name;
        private Int32? _pharmaaceutical_product_FK;
        private Int32? _indication_FK;
        private String _product_number;
        private Int32? _type_product_FK;
        private Int32? _client_organization_FK;
        private Int32? _domain_FK;
        private Int32? _procedure_type;
        private String _product_ID;
        private Int32? _country_FK;
        private Int32? _manufacturer_FK;
        private String _psur;
        private String _displayName;
        private Int32? _user_FK;
        private String _gridLayout;
        private Boolean? _isPublic;
        private DateTime? _nextdlp_from;
        private DateTime? _nextdlp_to;
        private Int32? _responsible_user_FK;
        private Boolean? _article57_reporting;
        private String _drug_atcs;
        private String _client_name;
        private String _activeSubstances;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? product_saved_search_PK
        {
            get { return _product_saved_search_PK; }
            set { _product_saved_search_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? pharmaaceutical_product_FK
        {
            get { return _pharmaaceutical_product_FK; }
            set { _pharmaaceutical_product_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? indication_FK
        {
            get { return _indication_FK; }
            set { _indication_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String product_number
        {
            get { return _product_number; }
            set { _product_number = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? type_product_FK
        {
            get { return _type_product_FK; }
            set { _type_product_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? client_organization_FK
        {
            get { return _client_organization_FK; }
            set { _client_organization_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? domain_FK
        {
            get { return _domain_FK; }
            set { _domain_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? procedure_type
        {
            get { return _procedure_type; }
            set { _procedure_type = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String product_ID
        {
            get { return _product_ID; }
            set { _product_ID = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? country_FK
        {
            get { return _country_FK; }
            set { _country_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? manufacturer_FK
        {
            get { return _manufacturer_FK; }
            set { _manufacturer_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String psur
        {
            get { return _psur; }
            set { _psur = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String displayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? user_FK
        {
            get { return _user_FK; }
            set { _user_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String gridLayout
        {
            get { return _gridLayout; }
            set { _gridLayout = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? isPublic
        {
            get { return _isPublic; }
            set { _isPublic = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? nextdlp_from
        {
            get { return _nextdlp_from; }
            set { _nextdlp_from = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? nextdlp_to
        {
            get { return _nextdlp_to; }
            set { _nextdlp_to = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? responsible_user_FK
        {
            get { return _responsible_user_FK; }
            set { _responsible_user_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? article57_reporting
        {
            get { return _article57_reporting; }
            set { _article57_reporting = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String client_name
        {
            get { return _client_name; }
            set { _client_name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String drug_atcs
        {
            get { return _drug_atcs; }
            set { _drug_atcs = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String activeSubstances
        {
            get { return _activeSubstances; }
            set { _activeSubstances = value; }
        }
        #endregion

        public Product_saved_search_PK() { }
        public Product_saved_search_PK(Int32? product_saved_search_PK, String name, Int32? pharmaaceutical_product_FK, Int32? indication_FK, String product_number, Int32? type_product_FK, Int32? client_organization_FK, Int32? domain_FK, Int32? procedure_type, String product_ID, Int32? country_FK, Int32? manufacturer_FK, String psur, String displayName, Int32? user_FK, String gridLayout, Boolean? isPublic, DateTime? nextdlp_from, DateTime? nextdlp_to, Int32? responsible_user_FK, Boolean? article57_reporting, String drug_atcs, String client_name, String activeSubstances)
        {
            this.product_saved_search_PK = product_saved_search_PK;
            this.name = name;
            this.pharmaaceutical_product_FK = pharmaaceutical_product_FK;
            this.indication_FK = indication_FK;
            this.product_number = product_number;
            this.type_product_FK = type_product_FK;
            this.client_organization_FK = client_organization_FK;
            this.domain_FK = domain_FK;
            this.procedure_type = procedure_type;
            this.product_ID = product_ID;
            this.country_FK = country_FK;
            this.manufacturer_FK = manufacturer_FK;
            this.psur = psur;
            this.displayName = displayName;
            this.user_FK = user_FK;
            this.gridLayout = gridLayout;
            this.isPublic = isPublic;
            this.nextdlp_from = nextdlp_from;
            this.nextdlp_to = nextdlp_to;
            this._responsible_user_FK = responsible_user_FK;
            this._drug_atcs = drug_atcs;
            this._client_name = client_name;
            this._activeSubstances = activeSubstances;
        }
    }

    public interface IProduct_saved_search_PKOperations : ICRUDOperations<Product_saved_search_PK>
    {
        List<Product_saved_search_PK> GetEntitiesByUserOrPublic(Int32? user_fk);
    }
}
