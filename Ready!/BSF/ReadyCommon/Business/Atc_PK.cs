// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	21.10.2011. 9:40:38
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.ATC
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ATC")]
    [GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    public class Atc_PK
    {
        private Int32? _atc_PK;
        private Int32? _operationtype;
        private Int32? _type_term;
        private String _atccode;
        private String _newownerid;
        private String _atccode_desc;
        private Int32? _versiondateformat;
        private String _versiondate;
        private String _comments;
        private String _pom_code;
        private String _pom_subcode;
        private String _pom_ddd;
        private String _pom_u;
        private String _pom_ar;
        private String _pom_note;
        private String _name;
        private String _name_archive;
        private String _search_by;
        private Boolean? _is_group;
        private String _evpmd_code;
        private String _value;
        private Boolean? _is_maunal_entry;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? atc_PK
        {
            get { return _atc_PK; }
            set { _atc_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? operationtype
        {
            get { return _operationtype; }
            set { _operationtype = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? type_term
        {
            get { return _type_term; }
            set { _type_term = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String atccode
        {
            get { return _atccode; }
            set { _atccode = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String newownerid
        {
            get { return _newownerid; }
            set { _newownerid = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String atccode_desc
        {
            get { return _atccode_desc; }
            set { _atccode_desc = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? versiondateformat
        {
            get { return _versiondateformat; }
            set { _versiondateformat = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String versiondate
        {
            get { return _versiondate; }
            set { _versiondate = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String comments
        {
            get { return _comments; }
            set { _comments = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String pom_code
        {
            get { return _pom_code; }
            set { _pom_code = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String pom_subcode
        {
            get { return _pom_subcode; }
            set { _pom_subcode = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String pom_ddd
        {
            get { return _pom_ddd; }
            set { _pom_ddd = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String pom_u
        {
            get { return _pom_u; }
            set { _pom_u = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String pom_ar
        {
            get { return _pom_ar; }
            set { _pom_ar = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String pom_note
        {
            get { return _pom_note; }
            set { _pom_note = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String name
        {
            get { return _name; }
            set { _name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String name_archive
        {
            get { return _name_archive; }
            set { _name_archive = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String search_by
        {
            get { return _search_by; }
            set { _search_by = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? is_group
        {
            get { return _is_group; }
            set { _is_group = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String evpmd_code
        {
            get { return _evpmd_code; }
            set { _evpmd_code = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String value
        {
            get { return _value; }
            set { _value = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? is_maunal_entry
        {
            get { return _is_maunal_entry; }
            set { _is_maunal_entry = value; }
        }

        #endregion

        public Atc_PK() { }
        public Atc_PK(Int32? atc_PK, Int32? operationtype, Int32? type_term, String atccode, String newownerid, String atccode_desc, Int32? versiondateformat, String versiondate, String comments, String pom_code, String pom_subcode, String pom_ddd, String pom_u, String pom_ar, String pom_note, String name, String name_archive, String search_by, Boolean? is_group, String evpmd_code, String value, Boolean? is_maunal_entry)
        {
            this.atc_PK = atc_PK;
            this.operationtype = operationtype;
            this.type_term = type_term;
            this.atccode = atccode;
            this.newownerid = newownerid;
            this.atccode_desc = atccode_desc;
            this.versiondateformat = versiondateformat;
            this.versiondate = versiondate;
            this.comments = comments;
            this.pom_code = pom_code;
            this.pom_subcode = pom_subcode;
            this.pom_ddd = pom_ddd;
            this.pom_u = pom_u;
            this.pom_ar = pom_ar;
            this.pom_note = pom_note;
            this.name = name;
            this.name_archive = name_archive;
            this.search_by = search_by;
            this.is_group = is_group;
            this.evpmd_code = evpmd_code;
            this.value = value;
            this.is_maunal_entry = is_maunal_entry;
        }
    }

    public interface IAtc_PKOperations : ICRUDOperations<Atc_PK>
    {
        DataSet GetATCSearcher(String code, String name, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        List<Atc_PK> GetEntitiesInsertedByHand();
        List<Atc_PK> GetEntitiesByProduct(int productPk);
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
    }
}
