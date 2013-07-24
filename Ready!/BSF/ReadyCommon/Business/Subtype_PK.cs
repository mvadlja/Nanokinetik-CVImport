// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:10:41
// Description:	GEM2 Generated class for table SSI.dbo.SUBTYPE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SUBTYPE")]
    //[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Subtype_PK
	{
		private Int32? _subtype_PK;
		private String _substance_class_subtype;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? subtype_PK
		{
			get { return _subtype_PK; }
			set { _subtype_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String substance_class_subtype
		{
			get { return _substance_class_subtype; }
			set { _substance_class_subtype = value; }
		}

		#endregion

		public Subtype_PK() { }
		public Subtype_PK(Int32? subtype_PK, String substance_class_subtype)
		{
			this.subtype_PK = subtype_PK;
			this.substance_class_subtype = substance_class_subtype;
		}
	}

	public interface ISubtype_PKOperations : ICRUDOperations<Subtype_PK>
	{
        List<Subtype_PK> GetSubtypeBySCLFPK(Int32? SCLFPK);
	}
}
