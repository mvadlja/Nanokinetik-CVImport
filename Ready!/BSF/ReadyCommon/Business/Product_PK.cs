// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	7.10.2011. 23:39:58
// Description:	GEM2 Generated class for table V2_Project.dbo.PRODUCT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PRODUCT", Active=true)]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Product_PK
	{
        public enum CalculatedColumn
        {
            All,
            Countries,
            ActiveSubstances,
            DrugAtcs
        }

		private Int32? _product_PK;
		private String _newownerid;
		private String _senderlocalcode;
		private Boolean? _orphan_drug;
		private Int32? _intensive_monitoring;
		private Int32? _authorisation_procedure;
		private String _comments;
		private Int32? _responsible_user_person_FK;
		private String _psur;
		private DateTime? _next_dlp;
		private String _name;
		private String _description;
		private Int32? _client_organization_FK;
		private Int32? _type_product_FK;
		private String _product_number;
		private String _product_ID;
        private String _mrp_dcp;
        private String _eu_number;
        private Int32? _client_group_FK;
        private Int32? _region_FK;
        private String _batch_size;
        private String _pack_size;
        private Int32? _storage_conditions_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? product_PK
		{
			get { return _product_PK; }
			set { _product_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String newownerid
		{
			get { return _newownerid; }
			set { _newownerid = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String senderlocalcode
		{
			get { return _senderlocalcode; }
			set { _senderlocalcode = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? orphan_drug
		{
			get { return _orphan_drug; }
			set { _orphan_drug = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? intensive_monitoring
		{
			get { return _intensive_monitoring; }
			set { _intensive_monitoring = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? authorisation_procedure
		{
			get { return _authorisation_procedure; }
			set { _authorisation_procedure = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String comments
		{
			get { return _comments; }
			set { _comments = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? responsible_user_person_FK
		{
			get { return _responsible_user_person_FK; }
			set { _responsible_user_person_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String psur
		{
			get { return _psur; }
			set { _psur = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? next_dlp
		{
			get { return _next_dlp; }
			set { _next_dlp = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String description
		{
			get { return _description; }
			set { _description = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? client_organization_FK
		{
			get { return _client_organization_FK; }
			set { _client_organization_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? type_product_FK
		{
			get { return _type_product_FK; }
			set { _type_product_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String product_number
		{
			get { return _product_number; }
            set { _product_number = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String product_ID
		{
			get { return _product_ID; }
			set { _product_ID = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String mrp_dcp
        {
            get { return _mrp_dcp; }
            set { _mrp_dcp = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String eu_number
        {
            get { return _eu_number; }
            set { _eu_number = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? client_group_FK
        {
            get { return _client_group_FK; }
            set { _client_group_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? region_FK
        {
            get { return _region_FK; }
            set { _region_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String batch_size
        {
            get { return _batch_size; }
            set { _batch_size = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String pack_size
        {
            get { return _pack_size; }
            set { _pack_size = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? storage_conditions_FK
        {
            get { return _storage_conditions_FK; }
            set { _storage_conditions_FK = value; }
        }
		#endregion

		public Product_PK() { }
        public Product_PK(Int32? product_PK, String newownerid, String senderlocalcode, Boolean? orphan_drug, Int32? intensive_monitoring, Int32? authorisation_procedure, String comments, Int32? responsible_user_person_FK, String psur, DateTime? next_dlp, String name, String description, Int32? client_organization_FK, Int32? type_product_FK, String product_number, String product_ID, string mrpDcp, string euNumber, Int32? client_group_FK, Int32? region_FK, String batch_size, String pack_size, Int32? storage_conditions_FK)
		{
			this.product_PK = product_PK;
			this.newownerid = newownerid;
			this.senderlocalcode = senderlocalcode;
			this.orphan_drug = orphan_drug;
			this.intensive_monitoring = intensive_monitoring;
			this.authorisation_procedure = authorisation_procedure;
			this.comments = comments;
			this.responsible_user_person_FK = responsible_user_person_FK;
			this.psur = psur;
			this.next_dlp = next_dlp;
			this.name = name;
			this.description = description;
			this.client_organization_FK = client_organization_FK;
			this.type_product_FK = type_product_FK;
			this.product_number = product_number;
			this.product_ID = product_ID;
            this.mrp_dcp = mrpDcp;
            this.eu_number = euNumber;
            this.client_group_FK = client_group_FK;
            this.region_FK = region_FK;
            this.batch_size = batch_size;
            this.pack_size = pack_size;
            this.storage_conditions_FK = storage_conditions_FK;
		}
	}

	public interface IProduct_PKOperations : ICRUDOperations<Product_PK>
	{
        DataSet ProductsSearcher(String name, String description, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);

        List<Product_PK> GetAvailableEntitiesByActivity(Int32? activityPk);
        List<Product_PK> GetAssignedEntitiesByActivity(Int32? activityPk);

        List<Product_PK> GetEntitiesByPharmaceuticalProduct(int pharmaceuticalProductPk);
        List<Product_PK> GetEntitiesByActivity(int activityPk);
        DataSet GetTabMenuItemsCount(Int32 product_PK, int? personFk);

        Int32? GetNextAlphabeticalEntity(Int32? product_PK);
        Int32? GetPrevAlphabeticalEntity(Int32? product_PK);
	    bool AbleToDeleteEntity(int productPk);

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormSearchDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        void UpdateCalculatedColumn(int productPk, Product_PK.CalculatedColumn calculatedColumn);
        void UpdateCalculatedColumnByPharmaceuticalProduct(int pharmaceuticalProductPk, Product_PK.CalculatedColumn calculatedColumn);

        List<Product_PK> GetProductsByApDocument(int? documentFk, int? apFk);
        List<Product_PK> GetProductsByPDocument(int? documentFk, int? productFk);
        List<Product_PK> GetProductsByADocument(int? documentFk, int? activityFk);

        List<Product_PK> GetAssignedProductsForSubmissionUnit(Int32? submissionUnit_PK);
        List<Product_PK> GetProductsByActivity(int? activityPk);
    }
}
