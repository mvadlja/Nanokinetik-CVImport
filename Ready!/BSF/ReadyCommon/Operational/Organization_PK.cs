// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	21.1.2012. 9:09:39
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ORGANIZATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ORGANIZATION")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Organization_PK
    {
        private Int32? _organization_PK;
        private Int32? _type_org_EMEA;
        private Int32? _type_org_FK;
        private String _name_org;
        private String _localnumber;
        private String _ev_code;
        private String _organizationsenderid_EMEA;
        private String _address;
        private String _city;
        private String _state;
        private String _postcode;
        private Int32? _countrycode_FK;
        private String _tel_number;
        private String _tel_extension;
        private String _tel_countrycode;
        private String _fax_number;
        private String _fax_extenstion;
        private String _fax_countrycode;
        private String _email;
        private String _comment;
        private String _mfl_evcode;
        private String _mflcompany;
        private String _mfldepartment;
        private String _mflbuilding;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? organization_PK
        {
            get { return _organization_PK; }
            set { _organization_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? type_org_EMEA
        {
            get { return _type_org_EMEA; }
            set { _type_org_EMEA = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? type_org_FK
        {
            get { return _type_org_FK; }
            set { _type_org_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String name_org
        {
            get { return _name_org; }
            set { _name_org = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String localnumber
        {
            get { return _localnumber; }
            set { _localnumber = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String ev_code
        {
            get { return _ev_code; }
            set { _ev_code = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String organizationsenderid_EMEA
        {
            get { return _organizationsenderid_EMEA; }
            set { _organizationsenderid_EMEA = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String address
        {
            get { return _address; }
            set { _address = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String city
        {
            get { return _city; }
            set { _city = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String state
        {
            get { return _state; }
            set { _state = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String postcode
        {
            get { return _postcode; }
            set { _postcode = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? countrycode_FK
        {
            get { return _countrycode_FK; }
            set { _countrycode_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String tel_number
        {
            get { return _tel_number; }
            set { _tel_number = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String tel_extension
        {
            get { return _tel_extension; }
            set { _tel_extension = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String tel_countrycode
        {
            get { return _tel_countrycode; }
            set { _tel_countrycode = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String fax_number
        {
            get { return _fax_number; }
            set { _fax_number = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String fax_extenstion
        {
            get { return _fax_extenstion; }
            set { _fax_extenstion = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String fax_countrycode
        {
            get { return _fax_countrycode; }
            set { _fax_countrycode = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String email
        {
            get { return _email; }
            set { _email = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String mfl_evcode
        {
            get { return _mfl_evcode; }
            set { _mfl_evcode = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String mflcompany
        {
            get { return _mflcompany; }
            set { _mflcompany = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String mfldepartment
        {
            get { return _mfldepartment; }
            set { _mfldepartment = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String mflbuilding
        {
            get { return _mflbuilding; }
            set { _mflbuilding = value; }
        }

        #endregion

        public Organization_PK() { }
        public Organization_PK(Int32? organization_PK, Int32? type_org_EMEA, Int32? type_org_FK, String name_org, String localnumber, String ev_code, String organizationsenderid_EMEA, String address, String city, String state, String postcode, Int32? countrycode_FK, String tel_number, String tel_extension, String tel_countrycode, String fax_number, String fax_extenstion, String fax_countrycode, String email, String comment, String mfl_evcode, String mflcompany, String mfldepartment, String mflbuilding)
        {
            this.organization_PK = organization_PK;
            this.type_org_EMEA = type_org_EMEA;
            this.type_org_FK = type_org_FK;
            this.name_org = name_org;
            this.localnumber = localnumber;
            this.ev_code = ev_code;
            this.organizationsenderid_EMEA = organizationsenderid_EMEA;
            this.address = address;
            this.city = city;
            this.state = state;
            this.postcode = postcode;
            this.countrycode_FK = countrycode_FK;
            this.tel_number = tel_number;
            this.tel_extension = tel_extension;
            this.tel_countrycode = tel_countrycode;
            this.fax_number = fax_number;
            this.fax_extenstion = fax_extenstion;
            this.fax_countrycode = fax_countrycode;
            this.email = email;
            this.comment = comment;
            this.mfl_evcode = mfl_evcode;
            this.mflcompany = mflcompany;
            this.mfldepartment = mfldepartment;
            this.mflbuilding = mflbuilding;
        }
    }

    public interface IOrganization_PKOperations : ICRUDOperations<Organization_PK>
    {
        List<Organization_PK> GetOrganizationsByRole(String role_name);
        List<Organization_PK> GetAvailableApplicantsForActivity(Int32? activity_pk);
        List<Organization_PK> GetAssignedApplicantsForActivity(Int32? activity_pk);
        DataSet GetDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        List<Organization_PK> GetAssignedDistributorsByAp(Int32? authorisedProductFk);
        List<Organization_PK> GetAvailableDistributorsByAp(Int32? authorisedProductFk);
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);

        List<Organization_PK> GetAssignedAgenciesForSubmissionUnit(Int32? submissionUnitPk);
        List<Organization_PK> GetAvailableAgenciesForSubmissionUnit(Int32? submissionUnitPk);
    }
}
