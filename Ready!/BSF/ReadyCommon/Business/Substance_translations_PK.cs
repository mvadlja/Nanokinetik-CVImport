// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SUBSTANCE_TRANSLATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "SUBSTANCE_TRANSLATION")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Substance_translations_PK
	{
		private Int32? _substance_translations_PK;
		private String _languagecode;
		private String _term;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? substance_translations_PK
		{
			get { return _substance_translations_PK; }
			set { _substance_translations_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String languagecode
		{
			get { return _languagecode; }
			set { _languagecode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String term
		{
			get { return _term; }
			set { _term = value; }
		}

		#endregion

		public Substance_translations_PK() { }
		public Substance_translations_PK(Int32? substance_translations_PK, String languagecode, String term)
		{
			this.substance_translations_PK = substance_translations_PK;
			this.languagecode = languagecode;
			this.term = term;
		}
	}

	public interface ISubstance_translations_PKOperations : ICRUDOperations<Substance_translations_PK>
	{

        List<Substance_translations_PK> GetSubTranslationByAS(int? as_PK);
    }
}
