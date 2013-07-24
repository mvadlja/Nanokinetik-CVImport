// ======================================================================================================================
// Author:		Hrvoje-PC\Hrvoje
// Create date:	4.11.2011. 14:09:27
// Description:	GEM2 Generated class for table SSI.dbo.SUBSTANCE_CLASSIFICATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SUBSTANCE_CLASSIFICATION")]
    //[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Subst_clf_PK
	{
		private Int32? _subst_clf_PK;
		private String _domain;
		private String _substance_classification;
		private String _sclf_type;
		private String _sclf_code;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? subst_clf_PK
		{
			get { return _subst_clf_PK; }
			set { _subst_clf_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String domain
		{
			get { return _domain; }
			set { _domain = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String substance_classification
		{
			get { return _substance_classification; }
			set { _substance_classification = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String sclf_type
		{
			get { return _sclf_type; }
			set { _sclf_type = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String sclf_code
		{
			get { return _sclf_code; }
			set { _sclf_code = value; }
		}

		#endregion

		public Subst_clf_PK() { }
		public Subst_clf_PK(Int32? subst_clf_PK, String domain, String substance_classification, String sclf_type, String sclf_code)
		{
			this.subst_clf_PK = subst_clf_PK;
			this.domain = domain;
			this.substance_classification = substance_classification;
			this.sclf_type = sclf_type;
			this.sclf_code = sclf_code;
		}
	}

	public interface ISubst_clf_PKOperations : ICRUDOperations<Subst_clf_PK>
	{
        List<Subst_clf_PK> GetSCLFByRIPK(Int32? RIPK);
	}
}
