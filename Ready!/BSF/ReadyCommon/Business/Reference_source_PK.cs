// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	8.11.2011. 21:17:21
// Description:	GEM2 Generated class for table SSI.dbo.REFERENCE_SOURCE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "REFERENCE_SOURCE")]
    //[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Reference_source_PK
	{
		private Int32? _reference_source_PK;
		private Boolean? _public_domain;
		private Int32? _rs_type_FK;
		private Int32? _rs_class_FK;
		private String _rs_id;
		private String _rs_citation;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? reference_source_PK
		{
			get { return _reference_source_PK; }
			set { _reference_source_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
		public Boolean? public_domain
		{
			get { return _public_domain; }
			set { _public_domain = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? rs_type_FK
		{
			get { return _rs_type_FK; }
			set { _rs_type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? rs_class_FK
		{
			get { return _rs_class_FK; }
			set { _rs_class_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String rs_id
		{
			get { return _rs_id; }
			set { _rs_id = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String rs_citation
		{
			get { return _rs_citation; }
			set { _rs_citation = value; }
		}

		#endregion

		public Reference_source_PK() { }
		public Reference_source_PK(Int32? reference_source_PK, Boolean? public_domain, Int32? rs_type_FK, Int32? rs_class_FK, String rs_id, String rs_citation)
		{
			this.reference_source_PK = reference_source_PK;
			this.public_domain = public_domain;
			this.rs_type_FK = rs_type_FK;
			this.rs_class_FK = rs_class_FK;
			this.rs_id = rs_id;
			this.rs_citation = rs_citation;
		}
	}

	public interface IReference_source_PKOperations : ICRUDOperations<Reference_source_PK>
	{
        List<Reference_source_PK> GetRSBySNPK(Int32? SNPK);
        List<Reference_source_PK> GetRSBySCPK(Int32? SCPK);
        List<Reference_source_PK> GetRSByGEPK(Int32? GEPK);
        List<Reference_source_PK> GetRSBySCLFPK(Int32? SCLFPK);
        List<Reference_source_PK> GetRSByGenePK(Int32? GenePK);
        List<Reference_source_PK> GetRSByRELPK(Int32? RELPK);
        List<Reference_source_PK> GetRSByTRG(Int32? trg);
	}
}
