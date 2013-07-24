// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	3.11.2011. 12:24:55
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_ADJUVANT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PP_ADJUVANT")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Adjuvant_PK
	{
        private Int32? _adjuvant_PK;
        private Int32? _substancecode_FK;
        private Int32? _resolutionmode;
        private Int32? _concentrationtypecode;
        private Decimal? _lowamountnumervalue;
        private String _lowamountnumerprefix;
        private String _lowamountnumerunit;
        private Decimal? _lowamountdenomvalue;
        private String _lowamountdenomprefix;
        private String _lowamountdenomunit;
        private Decimal? _highamountnumervalue;
        private String _highamountnumerprefix;
        private String _higamountnumerunit;
        private Decimal? _highamountdenomvalue;
        private String _highamountdenomprefix;
        private String _highamountdenomunit;
        private Int32? _pp_FK;
        private Int32? _userID;
        private Int32? _expressedBy_FK;
        private String _concise;

        #region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? adjuvant_PK
        {
            get { return _adjuvant_PK; }
            set { _adjuvant_PK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? substancecode_FK
        {
            get { return _substancecode_FK; }
            set { _substancecode_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? resolutionmode
        {
            get { return _resolutionmode; }
            set { _resolutionmode = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? concentrationtypecode
        {
            get { return _concentrationtypecode; }
            set { _concentrationtypecode = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Decimal)]
        public Decimal? lowamountnumervalue
        {
            get { return _lowamountnumervalue; }
            set { _lowamountnumervalue = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String lowamountnumerprefix
        {
            get { return _lowamountnumerprefix; }
            set { _lowamountnumerprefix = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String lowamountnumerunit
        {
            get { return _lowamountnumerunit; }
            set { _lowamountnumerunit = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Decimal)]
        public Decimal? lowamountdenomvalue
        {
            get { return _lowamountdenomvalue; }
            set { _lowamountdenomvalue = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String lowamountdenomprefix
        {
            get { return _lowamountdenomprefix; }
            set { _lowamountdenomprefix = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String lowamountdenomunit
        {
            get { return _lowamountdenomunit; }
            set { _lowamountdenomunit = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Decimal)]
        public Decimal? highamountnumervalue
        {
            get { return _highamountnumervalue; }
            set { _highamountnumervalue = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String highamountnumerprefix
        {
            get { return _highamountnumerprefix; }
            set { _highamountnumerprefix = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String higamountnumerunit
        {
            get { return _higamountnumerunit; }
            set { _higamountnumerunit = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Decimal)]
        public Decimal? highamountdenomvalue
        {
            get { return _highamountdenomvalue; }
            set { _highamountdenomvalue = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String highamountdenomprefix
        {
            get { return _highamountdenomprefix; }
            set { _highamountdenomprefix = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String highamountdenomunit
        {
            get { return _highamountdenomunit; }
            set { _highamountdenomunit = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? pp_FK
        {
            get { return _pp_FK; }
            set { _pp_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? userID
        {
            get { return _userID; }
            set { _userID = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? ExpressedBy_FK
        {
            get { return _expressedBy_FK; }
            set { _expressedBy_FK = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String concise
        {
            get { return _concise; }
            set { _concise = value; }
        }

        #endregion

        public Adjuvant_PK() { }
        public Adjuvant_PK(Int32? adjuvant_PK, Int32? substancecode_FK, Int32? resolutionmode, Int32? concentrationtypecode, Decimal? lowamountnumervalue, String lowamountnumerprefix, String lowamountnumerunit, Decimal? lowamountdenomvalue, String lowamountdenomprefix, String lowamountdenomunit, Decimal? highamountnumervalue, String highamountnumerprefix, String higamountnumerunit, Decimal? highamountdenomvalue, String highamountdenomprefix, String highamountdenomunit, Int32? pp_FK, Int32? userID, Int32? expressedBy_FK, String concise)
        {
            this.adjuvant_PK = adjuvant_PK;
            this.substancecode_FK = substancecode_FK;
            this.resolutionmode = resolutionmode;
            this.concentrationtypecode = concentrationtypecode;
            this.lowamountnumervalue = lowamountnumervalue;
            this.lowamountnumerprefix = lowamountnumerprefix;
            this.lowamountnumerunit = lowamountnumerunit;
            this.lowamountdenomvalue = lowamountdenomvalue;
            this.lowamountdenomprefix = lowamountdenomprefix;
            this.lowamountdenomunit = lowamountdenomunit;
            this.highamountnumervalue = highamountnumervalue;
            this.highamountnumerprefix = highamountnumerprefix;
            this.higamountnumerunit = higamountnumerunit;
            this.highamountdenomvalue = highamountdenomvalue;
            this.highamountdenomprefix = highamountdenomprefix;
            this.highamountdenomunit = highamountdenomunit;
            this.pp_FK = pp_FK;
            this.userID = userID;
            this.ExpressedBy_FK = expressedBy_FK;
            this.concise = concise;
        }
    }

	public interface IAdjuvant_PKOperations : ICRUDOperations<Adjuvant_PK>
	{
		List<Adjuvant_PK> GetAdjuvantsByPPPK(Int32? pharmaceutical_product_FK);
        void DeleteNULLByUserID(Int32? userID);
	}
}
