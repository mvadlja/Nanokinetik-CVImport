// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.SUBSTANCE_ALIAS
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "SUBSTANCE_ALIAS")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Substance_alias_PK
	{
		private Int32? _substance_alias_PK;
		private String _sourcecode;
		private Int32? _resolutionmode;
		private String _aliasname;
		private Int32? _substance_aliastranslations_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? substance_alias_PK
		{
			get { return _substance_alias_PK; }
			set { _substance_alias_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String sourcecode
		{
			get { return _sourcecode; }
			set { _sourcecode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? resolutionmode
		{
			get { return _resolutionmode; }
			set { _resolutionmode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String aliasname
		{
			get { return _aliasname; }
			set { _aliasname = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substance_aliastranslations_FK
		{
			get { return _substance_aliastranslations_FK; }
			set { _substance_aliastranslations_FK = value; }
		}

		#endregion

		public Substance_alias_PK() { }
		public Substance_alias_PK(Int32? substance_alias_PK, String sourcecode, Int32? resolutionmode, String aliasname, Int32? substance_aliastranslations_FK)
		{
			this.substance_alias_PK = substance_alias_PK;
			this.sourcecode = sourcecode;
			this.resolutionmode = resolutionmode;
			this.aliasname = aliasname;
			this.substance_aliastranslations_FK = substance_aliastranslations_FK;
		}
	}

	public interface ISubstance_alias_PKOperations : ICRUDOperations<Substance_alias_PK>
	{

        List<Substance_alias_PK> GetSubAlsByAs(int? as_PK);
    }
}
