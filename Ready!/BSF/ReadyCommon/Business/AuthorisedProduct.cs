// ======================================================================================================================
// Author:		TomoZ560\Tomo
// Create date:	13.10.2011. 22:14:14
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AUTHORISED_PRODUCT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "AUTHORISED_PRODUCT", Active=true)]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class AuthorisedProduct
	{
		private Int32? _ap_PK;
		private Int32? _product_FK;
		private Int32? _authorisationcountrycode_FK;
		private Int32? _organizationmahcode_FK;
		private String _product_name;
		private String _productshortname;
		private String _authorisationnumber;
        private Int32? _authorisationstatus_FK;
		private DateTime? _authorisationdate;
		private DateTime? _authorisationexpdate;
		private String _comment;
		private DateTime? _authorisationwithdrawndate;
		private String _packagedesc;
		private Boolean? _marketed;
		private String _legalstatus;
		private String _withdrawndateformat;
		private Int32? _mflcode_FK;
		private Int32? _qppvcode_person_FK;
		private String _product_ID;
		private String _ev_code;
		private String _xEVPRM_status;
		private Int32? _responsible_user_person_FK;
		private DateTime? _launchdate;
		private String _description;
		private String _authorised_product_ID;
		private String _authorisationdateformat;
		private String _evprm_comments;
		private String _localnumber;
		private String _ap_ID;
		private String _shelflife;
        private String _productgenericname;
        private String _productcompanyname;
        private String _productstrenght;
        private String _productform;
        private DateTime? _infodate;
        private String _phv_email;
        private String _phv_phone;
        private Boolean? _article_57_reporting;
        private DateTime? _sunsetclause;
        private Boolean? _substance_translations;
        private Int32? _update_PK;
        private Int32? _qppv_code_FK;
	    private Int32? _local_representative_FK;
        private String _indications;
        private Int32? _local_qppv_code_FK;
        private Int32? _license_holder_group_FK;
        private Boolean? _reservation_confirmed;
        private String _reserved_to;
        private String _local_codes;
        private String _pack_size;
        private Boolean? _reimbursment_status;
       
		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? ap_PK
		{
			get { return _ap_PK; }
			set { _ap_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? product_FK
		{
			get { return _product_FK; }
			set { _product_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? authorisationcountrycode_FK
		{
			get { return _authorisationcountrycode_FK; }
			set { _authorisationcountrycode_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? organizationmahcode_FK
		{
			get { return _organizationmahcode_FK; }
			set { _organizationmahcode_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String product_name
		{
			get { return _product_name; }
			set { _product_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String productshortname
		{
			get { return _productshortname; }
			set { _productshortname = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String authorisationnumber
		{
			get { return _authorisationnumber; }
			set { _authorisationnumber = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? authorisationstatus_FK
		{
            get { return _authorisationstatus_FK; }
            set { _authorisationstatus_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? authorisationdate
		{
			get { return _authorisationdate; }
			set { _authorisationdate = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? authorisationexpdate
		{
			get { return _authorisationexpdate; }
			set { _authorisationexpdate = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String comment
		{
			get { return _comment; }
			set { _comment = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? authorisationwithdrawndate
		{
			get { return _authorisationwithdrawndate; }
			set { _authorisationwithdrawndate = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String packagedesc
		{
			get { return _packagedesc; }
			set { _packagedesc = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? marketed
		{
			get { return _marketed; }
			set { _marketed = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String legalstatus
		{
			get { return _legalstatus; }
			set { _legalstatus = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String withdrawndateformat
		{
			get { return _withdrawndateformat; }
			set { _withdrawndateformat = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? mflcode_FK
		{
			get { return _mflcode_FK; }
			set { _mflcode_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? qppvcode_person_FK
		{
			get { return _qppvcode_person_FK; }
			set { _qppvcode_person_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String product_ID
		{
			get { return _product_ID; }
			set { _product_ID = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ev_code
		{
			get { return _ev_code; }
			set { _ev_code = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String XEVPRM_status
		{
			get { return _xEVPRM_status; }
			set { _xEVPRM_status = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? responsible_user_person_FK
		{
			get { return _responsible_user_person_FK; }
			set { _responsible_user_person_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? launchdate
		{
			get { return _launchdate; }
			set { _launchdate = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String description
		{
			get { return _description; }
			set { _description = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String authorised_product_ID
		{
			get { return _authorised_product_ID; }
			set { _authorised_product_ID = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String authorisationdateformat
		{
			get { return _authorisationdateformat; }
			set { _authorisationdateformat = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String evprm_comments
		{
			get { return _evprm_comments; }
			set { _evprm_comments = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String localnumber
		{
			get { return _localnumber; }
			set { _localnumber = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ap_ID
		{
			get { return _ap_ID; }
			set { _ap_ID = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String shelflife
		{
			get { return _shelflife; }
			set { _shelflife = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String productgenericname
        {
            get { return _productgenericname; }
            set { _productgenericname = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String productcompanyname
        {
            get { return _productcompanyname; }
            set { _productcompanyname = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String productstrenght
        {
            get { return _productstrenght; }
            set { _productstrenght = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String productform
        {
            get { return _productform; }
            set { _productform = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? infodate
        {
            get { return _infodate; }
            set { _infodate = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String phv_email
        {
            get { return _phv_email; }
            set { _phv_email = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String phv_phone
        {
            get { return _phv_phone; }
            set { _phv_phone = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? article_57_reporting
        {
            get { return _article_57_reporting; }
            set { _article_57_reporting = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? sunsetclause
        {
            get { return _sunsetclause; }
            set { _sunsetclause = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? substance_translations
        {
            get { return _substance_translations; }
            set { _substance_translations = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? update_PK
        {
            get { return _update_PK; }
            set { _update_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? qppv_code_FK
        {
            get { return _qppv_code_FK; }
            set { _qppv_code_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? local_representative_FK
        {
            get { return _local_representative_FK; }
            set { _local_representative_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
	    public string Indications
	    {
	        get { return _indications; }
	        set { _indications = value; }
	    }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? local_qppv_code_FK
        {
            get { return _local_qppv_code_FK; }
            set { _local_qppv_code_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? license_holder_group_FK
        {
            get { return _license_holder_group_FK; }
            set { _license_holder_group_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? reservation_confirmed
        {
            get { return _reservation_confirmed; }
            set { _reservation_confirmed = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String reserved_to
        {
            get { return _reserved_to; }
            set { _reserved_to = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String local_codes
        {
            get { return _local_codes; }
            set { _local_codes = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String pack_size
        {
            get { return _pack_size; }
            set { _pack_size = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? reimbursment_status
        {
            get { return _reimbursment_status; }
            set { _reimbursment_status = value; }
        }
	    #endregion

		public AuthorisedProduct() { }
        public AuthorisedProduct(Int32? ap_PK, Int32? product_FK, Int32? authorisationcountrycode_FK, Int32? organizationmahcode_FK, String product_name, String productshortname, String authorisationnumber, Int32? _authorisationstatus_FK, DateTime? authorisationdate, DateTime? authorisationexpdate, String comment, DateTime? authorisationwithdrawndate, String packagedesc, Boolean? marketed, String legalstatus, String withdrawndateformat, Int32? mflcode_FK, Int32? qppvcode_person_FK, String product_ID, String ev_code, String xEVPRM_status, Int32? responsible_user_person_FK, DateTime? launchdate, String description, String authorised_product_ID, String authorisationdateformat, String evprm_comments, String localnumber, String ap_ID, String shelflife, String productgenericname, String productcompanyname, String productstrenght, String productform, DateTime? infodate, String phv_email, String phv_phone, Boolean? article_57_reporting, DateTime? sunsetclause, Boolean? substance_translations, Int32? qppv_code_FK, Int32? local_representative_FK, String indications, Int32? local_qppv_code_FK, Int32? license_holder_group_FK, Boolean? reservation_confirmed, String reserved_to, String local_codes, String pack_size, Boolean? reimbursment_status)
		{
			this.ap_PK = ap_PK;
			this.product_FK = product_FK;
			this.authorisationcountrycode_FK = authorisationcountrycode_FK;
			this.organizationmahcode_FK = organizationmahcode_FK;
			this.product_name = product_name;
			this.productshortname = productshortname;
			this.authorisationnumber = authorisationnumber;
            this.authorisationstatus_FK = _authorisationstatus_FK;
			this.authorisationdate = authorisationdate;
			this.authorisationexpdate = authorisationexpdate;
			this.comment = comment;
			this.authorisationwithdrawndate = authorisationwithdrawndate;
			this.packagedesc = packagedesc;
			this.marketed = marketed;
			this.legalstatus = legalstatus;
			this.withdrawndateformat = withdrawndateformat;
			this.mflcode_FK = mflcode_FK;
			this.qppvcode_person_FK = qppvcode_person_FK;
			this.product_ID = product_ID;
			this.ev_code = ev_code;
			this.XEVPRM_status = xEVPRM_status;
			this.responsible_user_person_FK = responsible_user_person_FK;
			this.launchdate = launchdate;
			this.description = description;
			this.authorised_product_ID = authorised_product_ID;
			this.authorisationdateformat = authorisationdateformat;
			this.evprm_comments = evprm_comments;
			this.localnumber = localnumber;
			this.ap_ID = ap_ID;
			this.shelflife = shelflife;
            this.productgenericname = productgenericname;
            this.productcompanyname = productcompanyname;
            this.productstrenght = productstrenght;
            this.productform = productform;
            this.infodate = infodate;
            this.phv_email = phv_email;
            this.phv_phone = phv_phone;
            this.article_57_reporting = article_57_reporting;
            this.sunsetclause = sunsetclause;
            this.substance_translations = substance_translations;
            this.qppv_code_FK = qppv_code_FK;
            this.local_representative_FK = local_representative_FK;
            this.Indications = indications;
            this.local_qppv_code_FK = local_qppv_code_FK;
            this.license_holder_group_FK = license_holder_group_FK;
            this.reservation_confirmed = reservation_confirmed;
            this.reserved_to = reserved_to;
            this.local_codes = local_codes;
            this.pack_size = pack_size;
            this.reimbursment_status = reimbursment_status;
		}
	}

    public interface IAuthorisedProductOperations : ICRUDOperations<AuthorisedProduct>
	{
        DataSet AProductsSearcher(string name, string description, int currentPage, int recordsPerPage, List<GEMOrderBy> gobList, out int tempCount);
        DataSet GetTabMenuItemsCount(Int32 ap_PK, int? personFk);

        Int32? IsArticle57(Int32? product_PK);
        Int32? GetNextAlphabeticalEntity(Int32? ap_PK);
        Int32? GetPrevAlphabeticalEntity(Int32? ap_PK);

        List<int> GetA57RelEntityIDsWithoutXevprmByProduct(int? product_FK);
        List<AuthorisedProduct> GetEntitiesByQppvCode(int qppvCodeFk);
        // TODO: get entities by local qppv code if necessary

        DataSet GetListFormDataSet(Dictionary<String, String> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetListFormSearchDataSet(Dictionary<String, String> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        bool AbleToDeleteEntity(int authorisedProductPk);
        AuthorisedProduct GetEntityByEVCode(String evcode);
    }
}
