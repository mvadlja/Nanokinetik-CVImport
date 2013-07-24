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
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PP_ACTIVE_INGREDIENT")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Activeingredient_PK
	{
		private Int32? _activeingredient_PK;
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
		private String _highamountnumerunit;
		private Decimal? _highamountdenomvalue;
		private String _highamountdenomprefix;
		private String _highamountdenomunit;
		private Int32? _pp_FK;
		private Int32? _userID;
        private Int32? _strength_value;
        private String _strength_unit;
        private Int32? _expressedBy_FK;
        private String _concise;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? activeingredient_PK
		{
			get { return _activeingredient_PK; }
			set { _activeingredient_PK = value; }
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
		public Int32? userID
		{
			get { return _userID; }
			set { _userID = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? strength_value
        {
            get { return _strength_value; }
            set { _strength_value = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String strength_unit
        {
            get { return _strength_unit; }
            set { _strength_unit = value; }
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

		public Activeingredient_PK() { }
        public Activeingredient_PK(Int32? activeingredient_PK, Int32? substancecode_FK, Int32? resolutionmode, Int32? concentrationtypecode, Decimal? lowamountnumervalue, String lowamountnumerprefix, String lowamountnumerunit, Decimal? lowamountdenomvalue, String lowamountdenomprefix, String lowamountdenomunit, Decimal? highamountnumervalue, String highamountnumerprefix, String highamountnumerunit, Decimal? highamountdenomvalue, String highamountdenomprefix, String highamountdenomunit, Int32? pp_FK, Int32? userID, Int32? strength_value, String strength_unit, Int32? expressedBy_FK, String concise)
		{
			this.activeingredient_PK = activeingredient_PK;
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
			this.highamountnumerunit = highamountnumerunit;
			this.highamountdenomvalue = highamountdenomvalue;
			this.highamountdenomprefix = highamountdenomprefix;
			this.highamountdenomunit = highamountdenomunit;
			this.pp_FK = pp_FK;
			this.userID = userID;
            this.strength_value = strength_value;
            this.strength_unit = strength_unit;
            this.ExpressedBy_FK = expressedBy_FK;
            this.concise = concise;
		}
	}

	public interface IActiveingredient_PKOperations : ICRUDOperations<Activeingredient_PK>
	{
		List<Activeingredient_PK> GetIngredientsByPPPK(Int32? pharmaceutical_product_FK);
        void DeleteNULLByUserID(Int32? userID);
        DataSet GetPPSearcher(String name, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	}
}
