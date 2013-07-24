// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	21.2.2012. 14:11:59
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.AP_SAVED_SEARCH
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "AP_SAVED_SEARCH")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Ap_saved_search_PK
    {
        private Int32? _ap_saved_search_PK;
        private Int32? _product_FK;
        private Int32? _authorisationcountrycode_FK;
        private String _productshortname;
        private Int32? _responsible_user_person_FK;
        private Int32? _client_org_FK;
        private String _packagedesc;
        private Int32? _authorisationstatus_FK;
        private String _legalstatus;
        private Boolean? _marketed;
        private Int32? _organizationmahcode_FK;
        private DateTime? _authorisationdateFrom;
        private DateTime? _authorisationdateTo;
        private DateTime? _authorisationexpdateFrom;
        private DateTime? _authorisationexpdateTo;
        private DateTime? _sunsetclauseFrom;
        private DateTime? _sunsetclauseTo;
        private String _authorisationnumber;
        private String _displayName;
        private Int32? _user_fk;
        private String _gridlayout;
        private Boolean _ispublic;
        private Boolean? _article57_reporting;
        private String _MEDDRA_FK;
        private Boolean? _substance_translations;
        private Int32? _qppv_person_FK;
        private string _ev_code;
        private Int32? _local_representative_FK;
        private String _indications;
        private Int32? _local_qppv_person_FK;
        private Int32? _mflcode_FK;
        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? ap_saved_search_PK
        {
            get { return _ap_saved_search_PK; }
            set { _ap_saved_search_PK = value; }
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

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String productshortname
        {
            get { return _productshortname; }
            set { _productshortname = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? responsible_user_person_FK
        {
            get { return _responsible_user_person_FK; }
            set { _responsible_user_person_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? client_org_FK
        {
            get { return _client_org_FK; }
            set { _client_org_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String packagedesc
        {
            get { return _packagedesc; }
            set { _packagedesc = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? authorisationstatus_FK
        {
            get { return _authorisationstatus_FK; }
            set { _authorisationstatus_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String legalstatus
        {
            get { return _legalstatus; }
            set { _legalstatus = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? marketed
        {
            get { return _marketed; }
            set { _marketed = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? organizationmahcode_FK
        {
            get { return _organizationmahcode_FK; }
            set { _organizationmahcode_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? authorisationdateFrom
        {
            get { return _authorisationdateFrom; }
            set { _authorisationdateFrom = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? authorisationdateTo
        {
            get { return _authorisationdateTo; }
            set { _authorisationdateTo = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? authorisationexpdateFrom
        {
            get { return _authorisationexpdateFrom; }
            set { _authorisationexpdateFrom = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? authorisationexpdateTo
        {
            get { return _authorisationexpdateTo; }
            set { _authorisationexpdateTo = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? sunsetclauseFrom
        {
            get { return _sunsetclauseFrom; }
            set { _sunsetclauseFrom = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
        public DateTime? sunsetclauseTo
        {
            get { return _sunsetclauseTo; }
            set { _sunsetclauseTo = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String authorisationnumber
        {
            get { return _authorisationnumber; }
            set { _authorisationnumber = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String displayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? user_fk
        {
            get { return _user_fk; }
            set { _user_fk = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String gridLayout
        {
            get { return _gridlayout; }
            set { _gridlayout = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean ispublic
        {
            get { return _ispublic; }
            set { _ispublic = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? article57_reporting
        {
            get { return _article57_reporting; }
            set { _article57_reporting = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? substance_translations
        {
            get { return _substance_translations; }
            set { _substance_translations = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String MEDDRA_FK
        {
            get { return _MEDDRA_FK; }
            set { _MEDDRA_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ev_code
        {
            get { return _ev_code; }
            set { _ev_code = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? qppv_person_FK
        {
            get { return _qppv_person_FK; }
            set { _qppv_person_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? local_representative_FK
        {
            get { return _local_representative_FK; }
            set { _local_representative_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public string indications
        {
            get { return _indications; }
            set { _indications = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? local_qppv_person_FK
        {
            get { return _local_qppv_person_FK; }
            set { _local_qppv_person_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? mflcode_FK
        {
            get { return _mflcode_FK; }
            set { _mflcode_FK = value; }
        }

        #endregion

        public Ap_saved_search_PK() { }
        public Ap_saved_search_PK(Int32? ap_saved_search_PK, Int32? product_FK, Int32? authorisationcountrycode_FK, String productshortname, Int32? responsible_user_person_FK, Int32? client_org_FK, String packagedesc, Int32? authorisationstatus_FK, String legalstatus, Boolean? marketed, Int32? organizationmahcode_FK, DateTime? authorisationdateFrom, DateTime? authorisationdateTo, DateTime? authorisationexpdateFrom, DateTime? authorisationexpdateTo, DateTime? sunsetclauseFrom, DateTime? sunsetclauseTo, String authorisationnumber, String displayName, Int32? user_fk, String gridlayout, Boolean ispublic, Boolean? article57_reporting, String MEDDRA, Boolean? substance_translations, String ev_code, Int32? qppv_person_FK, String indications, Int32? local_qppv_person_FK, Int32? mflcode_FK)
        {
            this.ap_saved_search_PK = ap_saved_search_PK;
            this.product_FK = product_FK;
            this.authorisationcountrycode_FK = authorisationcountrycode_FK;
            this.productshortname = productshortname;
            this.responsible_user_person_FK = responsible_user_person_FK;
            this.client_org_FK = client_org_FK;
            this.packagedesc = packagedesc;
            this.authorisationstatus_FK = authorisationstatus_FK;
            this.legalstatus = legalstatus;
            this.marketed = marketed;
            this.organizationmahcode_FK = organizationmahcode_FK;
            this.authorisationdateFrom = authorisationdateFrom;
            this.authorisationdateTo = authorisationdateTo;
            this.authorisationexpdateFrom = authorisationexpdateFrom;
            this.authorisationexpdateTo = authorisationexpdateTo;
            this.sunsetclauseFrom = sunsetclauseFrom;
            this.sunsetclauseTo = sunsetclauseTo;
            this.authorisationnumber = authorisationnumber;
            this.displayName = displayName;
            this.user_fk = user_fk;
            this.gridLayout = gridlayout;
            this.ispublic = ispublic;
            this.article57_reporting = article57_reporting;
            this.MEDDRA_FK = MEDDRA;
            this.substance_translations = substance_translations;
            this.ev_code = ev_code;
            this.qppv_person_FK = qppv_person_FK;
            this.indications = indications;
            this.local_qppv_person_FK = local_qppv_person_FK;
            this.mflcode_FK = mflcode_FK;
        }
    }

    public interface IAp_saved_search_PKOperations : ICRUDOperations<Ap_saved_search_PK>
    {
        List<Ap_saved_search_PK> GetEntitiesByUser(Int32? user_fk);
        List<Ap_saved_search_PK> GetEntitiesByUserOrPublic(Int32? user_fk);
    }
}
