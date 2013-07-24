// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:45:44
// Description:	GEM2 Generated class for table SSI.dbo.SUBTYPE_SCLF_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SUBTYPE_SCLF_MN")]
    //[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Subtype_sclf_mn_PK
	{
		private Int32? _subtype_sclf_mn_PK;
		private Int32? _subtype_FK;
		private Int32? _sclf_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? subtype_sclf_mn_PK
		{
			get { return _subtype_sclf_mn_PK; }
			set { _subtype_sclf_mn_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? subtype_FK
		{
			get { return _subtype_FK; }
			set { _subtype_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? sclf_FK
		{
			get { return _sclf_FK; }
			set { _sclf_FK = value; }
		}

		#endregion

		public Subtype_sclf_mn_PK() { }
		public Subtype_sclf_mn_PK(Int32? subtype_sclf_mn_PK, Int32? subtype_FK, Int32? sclf_FK)
		{
			this.subtype_sclf_mn_PK = subtype_sclf_mn_PK;
			this.subtype_FK = subtype_FK;
			this.sclf_FK = sclf_FK;
		}
	}

	public interface ISubtype_sclf_mn_PKOperations : ICRUDOperations<Subtype_sclf_mn_PK>
	{

	}
}
