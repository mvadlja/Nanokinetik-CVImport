// ======================================================================================================================
// Author:		BUBI-PC\possimus
// Create date:	21.10.2011. 11:32:26
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.LANGUAGE_CODE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "LANGUAGE_CODE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Languagecode_PK
	{
		private Int32? _languagecode_PK;
		private String _name;
		private String _code;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? languagecode_PK
		{
			get { return _languagecode_PK; }
			set { _languagecode_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String code
		{
			get { return _code; }
			set { _code = value; }
		}

		#endregion

		public Languagecode_PK() { }
		public Languagecode_PK(Int32? languagecode_PK, String name, String code)
		{
			this.languagecode_PK = languagecode_PK;
			this.name = name;
			this.code = code;
		}
	}

	public interface ILanguagecode_PKOperations : ICRUDOperations<Languagecode_PK>
	{
        List<Languagecode_PK> GetLanguageCodeByDocument(Int32? document_PK);
	}
}
