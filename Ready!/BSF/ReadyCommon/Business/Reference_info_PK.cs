// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 13:49:12
// Description:	GEM2 Generated class for table SSI.dbo.REFERENCE_INFORMATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "REFERENCE_INFORMATION")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Reference_info_PK
	{
		private Int32? _reference_info_PK;
		private String _comment;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? reference_info_PK
		{
			get { return _reference_info_PK; }
			set { _reference_info_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String comment
		{
			get { return _comment; }
			set { _comment = value; }
		}

		#endregion

		public Reference_info_PK() { }
		public Reference_info_PK(Int32? reference_info_PK, String comment)
		{
			this.reference_info_PK = reference_info_PK;
			this.comment = comment;
		}
	}

	public interface IReference_info_PKOperations : ICRUDOperations<Reference_info_PK>
	{

	}
}
