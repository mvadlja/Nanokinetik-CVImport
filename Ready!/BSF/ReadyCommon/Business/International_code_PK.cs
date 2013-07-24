// ======================================================================================================================
// Author:		Mateo-PC\Mateo
// Create date:	4.1.2012. 12:02:23
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.INTERNATIONAL_CODE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "INTERNATIONAL_CODE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class International_code_PK
	{
		private Int32? _international_code_PK;
		private String _sourcecode;
		private Int32? _resolutionmode_sources;
		private String _referencetext;
		private Int32? _resolutionmode_substance;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? international_code_PK
		{
			get { return _international_code_PK; }
			set { _international_code_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String sourcecode
		{
			get { return _sourcecode; }
			set { _sourcecode = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? resolutionmode_sources
		{
			get { return _resolutionmode_sources; }
			set { _resolutionmode_sources = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String referencetext
		{
			get { return _referencetext; }
			set { _referencetext = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? resolutionmode_substance
		{
			get { return _resolutionmode_substance; }
			set { _resolutionmode_substance = value; }
		}

		#endregion

		public International_code_PK() { }
		public International_code_PK(Int32? international_code_PK, String sourcecode, Int32? resolutionmode_sources, String referencetext, Int32? resolutionmode_substance)
		{
			this.international_code_PK = international_code_PK;
			this.sourcecode = sourcecode;
			this.resolutionmode_sources = resolutionmode_sources;
			this.referencetext = referencetext;
			this.resolutionmode_substance = resolutionmode_substance;
		}
	}

	public interface IInternational_code_PKOperations : ICRUDOperations<International_code_PK>
	{

        List<International_code_PK> GetIntCodesByAs(int? as_PK);
    }
}
