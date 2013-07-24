// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	4.11.2011. 13:24:50
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SSI_CONTROLED_VOCABULARY
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
    [Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SSI_CONTROLED_VOCABULARY")]
    //[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
    public class Ssi__cont_voc_PK
    {
        private Int32? _ssi__cont_voc_PK;
        private String _list_name;
        private Double? _term_id;
        private String _term_name_english;
        private String _latin_name_latin;
        private String _synonim1;
        private String _synonim2;
        private String _description;
        private String _field8;
        private String _field9;
        private String _field10;
        private String _field11;
        private String _field12;
        private String _field13;
        private String _field14;
        private Int32? _custom_sort;
        private String _evcode;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? ssi__cont_voc_PK
        {
            get { return _ssi__cont_voc_PK; }
            set { _ssi__cont_voc_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String list_name
        {
            get { return _list_name; }
            set { _list_name = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Double)]
        public Double? term_id
        {
            get { return _term_id; }
            set { _term_id = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String term_name_english
        {
            get { return _term_name_english; }
            set { _term_name_english = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String latin_name_latin
        {
            get { return _latin_name_latin; }
            set { _latin_name_latin = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String synonim1
        {
            get { return _synonim1; }
            set { _synonim1 = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String synonim2
        {
            get { return _synonim2; }
            set { _synonim2 = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Description
        {
            get { return _description; }
            set { _description = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Field8
        {
            get { return _field8; }
            set { _field8 = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Field9
        {
            get { return _field9; }
            set { _field9 = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Field10
        {
            get { return _field10; }
            set { _field10 = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Field11
        {
            get { return _field11; }
            set { _field11 = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Field12
        {
            get { return _field12; }
            set { _field12 = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Field13
        {
            get { return _field13; }
            set { _field13 = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Field14
        {
            get { return _field14; }
            set { _field14 = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? custom_sort
        {
            get { return _custom_sort; }
            set { _custom_sort = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String Evcode
        {
            get { return _evcode; }
            set { _evcode = value; }
        }

        #endregion

        public Ssi__cont_voc_PK() { }
        public Ssi__cont_voc_PK(Int32? ssi__cont_voc_PK, String list_name, Double? term_id, String term_name_english, String latin_name_latin, String synonim1, String synonim2, String description, String field8, String field9, String field10, String field11, String field12, String field13, String field14, String evcode)
        {
            this.ssi__cont_voc_PK = ssi__cont_voc_PK;
            this.list_name = list_name;
            this.term_id = term_id;
            this.term_name_english = term_name_english;
            this.latin_name_latin = latin_name_latin;
            this.synonim1 = synonim1;
            this.synonim2 = synonim2;
            this.Description = description;
            this.Field8 = field8;
            this.Field9 = field9;
            this.Field10 = field10;
            this.Field11 = field11;
            this.Field12 = field12;
            this.Field13 = field13;
            this.Field14 = field14;
            this.Evcode = evcode;
        }
    }

    public interface ISsi__cont_voc_PKOperations : ICRUDOperations<Ssi__cont_voc_PK>
    {
        List<Ssi__cont_voc_PK> GetConcentrationTypes();
        List<Ssi__cont_voc_PK> GetPrefixes();
        List<Ssi__cont_voc_PK> GetPrefixesSubstanceClass();
        List<Ssi__cont_voc_PK> GetEntitiesByListName(String list_name);
        List<Ssi__cont_voc_PK> GetDomainByONPK(Int32? ONPK);
        List<Ssi__cont_voc_PK> GetJurByONPK(Int32? ONPK);
    }
}
