// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	1.11.2011. 11:37:51
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SUBSTANCES
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "SUBSTANCES")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Substance_PK
	{
		private Int32? _substance_PK;
		private String _ev_code;
		private String _substance_name;
		private String _synonym1;
		private String _synonym1_language;
		private String _synonym2;
		private String _synonym2_language;
		private String _synonym3;
		private String _synonym3_language;
		private String _synonym4;
		private String _synonym4_language;
		private String _synonym5;
		private String _synonym5_language;
		private String _synonym6;
		private String _synonym6_language;
		private String _synonym7;
		private String _synonym7_language;
		private String _synonym8;
		private String _synonym8_language;
		private String _synonym9;
		private String _synonym9_language;
		private String _synonym10;
		private String _synonym10_language;
		private String _synonym11;
		private String _synonym11_language;
		private String _synonym12;
		private String _synonym12_language;
		private String _synonym13;
		private String _synonym13_language;
		private String _synonym14;
		private String _synonym14_language;
		private String _synonym15;
		private String _synonym15_language;
		private String _synonym16;
		private String _synonym16_language;
		private String _synonym17;
		private String _synonym17_language;
		private String _synonym18;
		private String _synonym18_language;
		private String _synonym19;
		private String _synonym19_language;
		private String _synonym20;
		private String _synonym20_language;
		private String _synonym21;
		private String _synonym21_language;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? substance_PK
		{
			get { return _substance_PK; }
			set { _substance_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String ev_code
		{
			get { return _ev_code; }
			set { _ev_code = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String substance_name
		{
			get { return _substance_name; }
			set { _substance_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym1
		{
			get { return _synonym1; }
			set { _synonym1 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym1_language
		{
			get { return _synonym1_language; }
			set { _synonym1_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym2
		{
			get { return _synonym2; }
			set { _synonym2 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym2_language
		{
			get { return _synonym2_language; }
			set { _synonym2_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym3
		{
			get { return _synonym3; }
			set { _synonym3 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym3_language
		{
			get { return _synonym3_language; }
			set { _synonym3_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym4
		{
			get { return _synonym4; }
			set { _synonym4 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym4_language
		{
			get { return _synonym4_language; }
			set { _synonym4_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym5
		{
			get { return _synonym5; }
			set { _synonym5 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym5_language
		{
			get { return _synonym5_language; }
			set { _synonym5_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym6
		{
			get { return _synonym6; }
			set { _synonym6 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym6_language
		{
			get { return _synonym6_language; }
			set { _synonym6_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym7
		{
			get { return _synonym7; }
			set { _synonym7 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym7_language
		{
			get { return _synonym7_language; }
			set { _synonym7_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym8
		{
			get { return _synonym8; }
			set { _synonym8 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym8_language
		{
			get { return _synonym8_language; }
			set { _synonym8_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym9
		{
			get { return _synonym9; }
			set { _synonym9 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym9_language
		{
			get { return _synonym9_language; }
			set { _synonym9_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym10
		{
			get { return _synonym10; }
			set { _synonym10 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym10_language
		{
			get { return _synonym10_language; }
			set { _synonym10_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym11
		{
			get { return _synonym11; }
			set { _synonym11 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym11_language
		{
			get { return _synonym11_language; }
			set { _synonym11_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym12
		{
			get { return _synonym12; }
			set { _synonym12 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym12_language
		{
			get { return _synonym12_language; }
			set { _synonym12_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym13
		{
			get { return _synonym13; }
			set { _synonym13 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym13_language
		{
			get { return _synonym13_language; }
			set { _synonym13_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym14
		{
			get { return _synonym14; }
			set { _synonym14 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym14_language
		{
			get { return _synonym14_language; }
			set { _synonym14_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym15
		{
			get { return _synonym15; }
			set { _synonym15 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym15_language
		{
			get { return _synonym15_language; }
			set { _synonym15_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym16
		{
			get { return _synonym16; }
			set { _synonym16 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym16_language
		{
			get { return _synonym16_language; }
			set { _synonym16_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym17
		{
			get { return _synonym17; }
			set { _synonym17 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym17_language
		{
			get { return _synonym17_language; }
			set { _synonym17_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym18
		{
			get { return _synonym18; }
			set { _synonym18 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym18_language
		{
			get { return _synonym18_language; }
			set { _synonym18_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym19
		{
			get { return _synonym19; }
			set { _synonym19 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym19_language
		{
			get { return _synonym19_language; }
			set { _synonym19_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym20
		{
			get { return _synonym20; }
			set { _synonym20 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym20_language
		{
			get { return _synonym20_language; }
			set { _synonym20_language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym21
		{
			get { return _synonym21; }
			set { _synonym21 = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String synonym21_language
		{
			get { return _synonym21_language; }
			set { _synonym21_language = value; }
		}

		#endregion

		public Substance_PK() { }
		public Substance_PK(Int32? substance_PK, String ev_code, String substance_name, String synonym1, String synonym1_language, String synonym2, String synonym2_language, String synonym3, String synonym3_language, String synonym4, String synonym4_language, String synonym5, String synonym5_language, String synonym6, String synonym6_language, String synonym7, String synonym7_language, String synonym8, String synonym8_language, String synonym9, String synonym9_language, String synonym10, String synonym10_language, String synonym11, String synonym11_language, String synonym12, String synonym12_language, String synonym13, String synonym13_language, String synonym14, String synonym14_language, String synonym15, String synonym15_language, String synonym16, String synonym16_language, String synonym17, String synonym17_language, String synonym18, String synonym18_language, String synonym19, String synonym19_language, String synonym20, String synonym20_language, String synonym21, String synonym21_language)
		{
			this.substance_PK = substance_PK;
			this.ev_code = ev_code;
			this.substance_name = substance_name;
			this.synonym1 = synonym1;
			this.synonym1_language = synonym1_language;
			this.synonym2 = synonym2;
			this.synonym2_language = synonym2_language;
			this.synonym3 = synonym3;
			this.synonym3_language = synonym3_language;
			this.synonym4 = synonym4;
			this.synonym4_language = synonym4_language;
			this.synonym5 = synonym5;
			this.synonym5_language = synonym5_language;
			this.synonym6 = synonym6;
			this.synonym6_language = synonym6_language;
			this.synonym7 = synonym7;
			this.synonym7_language = synonym7_language;
			this.synonym8 = synonym8;
			this.synonym8_language = synonym8_language;
			this.synonym9 = synonym9;
			this.synonym9_language = synonym9_language;
			this.synonym10 = synonym10;
			this.synonym10_language = synonym10_language;
			this.synonym11 = synonym11;
			this.synonym11_language = synonym11_language;
			this.synonym12 = synonym12;
			this.synonym12_language = synonym12_language;
			this.synonym13 = synonym13;
			this.synonym13_language = synonym13_language;
			this.synonym14 = synonym14;
			this.synonym14_language = synonym14_language;
			this.synonym15 = synonym15;
			this.synonym15_language = synonym15_language;
			this.synonym16 = synonym16;
			this.synonym16_language = synonym16_language;
			this.synonym17 = synonym17;
			this.synonym17_language = synonym17_language;
			this.synonym18 = synonym18;
			this.synonym18_language = synonym18_language;
			this.synonym19 = synonym19;
			this.synonym19_language = synonym19_language;
			this.synonym20 = synonym20;
			this.synonym20_language = synonym20_language;
			this.synonym21 = synonym21;
			this.synonym21_language = synonym21_language;
		}
	}

	public interface ISubstance_PKOperations : ICRUDOperations<Substance_PK>
	{
        DataSet GetSubstancesByNameSearcher(String name, String evcode, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);

	    bool EntityWithEvCodeExists(string evCode);
	}
}
