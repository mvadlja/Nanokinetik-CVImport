// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:48:18
// Description:	GEM2 Generated class for table SSI.dbo.OFFICIAL_NAME_JURISDICTION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "OFFICIAL_NAME_JURISDICTION")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Jurisdiction_PK
	{
		private Int32? _jurisdiction_PK;
		private String _on_jurisd;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? jurisdiction_PK
		{
			get { return _jurisdiction_PK; }
			set { _jurisdiction_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String on_jurisd
		{
			get { return _on_jurisd; }
			set { _on_jurisd = value; }
		}

		#endregion

		public Jurisdiction_PK() { }
		public Jurisdiction_PK(Int32? jurisdiction_PK, String on_jurisd)
		{
			this.jurisdiction_PK = jurisdiction_PK;
			this.on_jurisd = on_jurisd;
		}
	}

	public interface IJurisdiction_PKOperations : ICRUDOperations<Jurisdiction_PK>
	{

	}
}
