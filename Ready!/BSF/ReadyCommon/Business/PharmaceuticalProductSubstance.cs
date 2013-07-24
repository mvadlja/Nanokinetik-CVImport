// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	3.11.2011. 13:08:15
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PP_ACTIVE_INGREDIENT
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMOperationsLogging(DataSourceId = "Default", Active = false)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class PharmaceuticalProductSubstance
	{
        public enum SubstanceType
        {
            ActiveIngredient,
            Excipient,
            Adjuvant
        }

        private Int32? _ppsubstance_PK;
		private Int32? _ppsubstance_FK;
		private Int32? _substancecode_FK;
		private Int32? _concentrationtypecode;
		private Decimal? _lowamountnumervalue;
		private String _lowamountnumerprefix;
		private String _lowamountnumerunit;
		private Decimal? _lowamountdenomvalue;
		private String _lowamountdenomprefix;
		private String _lowamountdenomunit;
		private Decimal? _highamountnumervalue;
		private String _highamountnumerprefix;
		private String _highamountnumerunit;
		private Decimal? _highamountdenomvalue;
		private String _highamountdenomprefix;
		private String _highamountdenomunit;
		private Int32? _pp_FK;
		private Int32? _expressedby_FK;
		private String _concise;
		private String _substancetype;
		private Int32? _user_FK;
		private String _sessionid;
		private DateTime? _modifieddate;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? ppsubstance_PK
		{
			get { return _ppsubstance_PK; }
			set { _ppsubstance_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ppsubstance_FK
		{
			get { return _ppsubstance_FK; }
			set { _ppsubstance_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substancecode_FK
		{
			get { return _substancecode_FK; }
			set { _substancecode_FK = value; }
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
		public String highamountnumerunit
		{
			get { return _highamountnumerunit; }
			set { _highamountnumerunit = value; }
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
		public Int32? expressedby_FK
		{
			get { return _expressedby_FK; }
			set { _expressedby_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String concise
		{
			get { return _concise; }
			set { _concise = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String substancetype
		{
			get { return _substancetype; }
			set { _substancetype = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_FK
		{
			get { return _user_FK; }
			set { _user_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String sessionid
		{
			get { return _sessionid; }
			set { _sessionid = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? modifieddate
		{
			get { return _modifieddate; }
			set { _modifieddate = value; }
		}

		#endregion

		public PharmaceuticalProductSubstance() { }
        public PharmaceuticalProductSubstance(Int32? ppsubstance_PK, Int32? ppsubstance_FK, Int32? substancecode_FK, Int32? concentrationtypecode, Decimal? lowamountnumervalue, String lowamountnumerprefix, String lowamountnumerunit, Decimal? lowamountdenomvalue, String lowamountdenomprefix, String lowamountdenomunit, Decimal? highamountnumervalue, String highamountnumerprefix, String highamountnumerunit, Decimal? highamountdenomvalue, String highamountdenomprefix, String highamountdenomunit, Int32? pp_FK, Int32? expressedby_FK, String concise, String substancetype, Int32? user_FK, String sessionid, DateTime? modifieddate)
		{
			this.ppsubstance_PK = ppsubstance_PK;
			this.ppsubstance_FK = ppsubstance_FK;
			this.substancecode_FK = substancecode_FK;
			this.concentrationtypecode = concentrationtypecode;
			this.lowamountnumervalue = lowamountnumervalue;
			this.lowamountnumerprefix = lowamountnumerprefix;
			this.lowamountnumerunit = lowamountnumerunit;
			this.lowamountdenomvalue = lowamountdenomvalue;
			this.lowamountdenomprefix = lowamountdenomprefix;
			this.lowamountdenomunit = lowamountdenomunit;
			this.highamountnumervalue = highamountnumervalue;
			this.highamountnumerprefix = highamountnumerprefix;
			this.highamountnumerunit = highamountnumerunit;
			this.highamountdenomvalue = highamountdenomvalue;
			this.highamountdenomprefix = highamountdenomprefix;
			this.highamountdenomunit = highamountdenomunit;
			this.pp_FK = pp_FK;
			this.expressedby_FK = expressedby_FK;
			this.concise = concise;
			this.substancetype = substancetype;
			this.user_FK = user_FK;
			this.sessionid = sessionid;
			this.modifieddate = modifieddate;
		}
	}

    public interface IPharmaceuticalProductSubstanceOperations : ICRUDOperations<PharmaceuticalProductSubstance>
    {
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        DataSet GetPreviewFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        void DeleteByTypeAndSessionId(string substanceType, string sessionId);
        void DeleteBySessionId(string sessionId);
        List<PharmaceuticalProductSubstance> GetEntitiesByTypeAndSessionId(string substanceType, string sessionId);
    }
}
