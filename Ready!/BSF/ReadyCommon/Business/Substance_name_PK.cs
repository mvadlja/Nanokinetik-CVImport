// ======================================================================================================================
// Author:		ANTEC\Kiki
// Create date:	4.12.2011. 21:19:46
// Description:	GEM2 Generated class for table SSI.dbo.SUBSTANCE_NAME
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SUBSTANCE_NAME")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Substance_name_PK
	{
		private Int32? _substance_name_PK;
		private String _subst_name;
		private Int32? _subst_name_type_FK;
		private Int32? _language_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? substance_name_PK
		{
			get { return _substance_name_PK; }
			set { _substance_name_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String subst_name
		{
			get { return _subst_name; }
			set { _subst_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? subst_name_type_FK
		{
			get { return _subst_name_type_FK; }
			set { _subst_name_type_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? language_FK
		{
			get { return _language_FK; }
			set { _language_FK = value; }
		}

		#endregion

		public Substance_name_PK() { }
		public Substance_name_PK(Int32? substance_name_PK, String subst_name, Int32? subst_name_type_FK, Int32? language_FK)
		{
			this.substance_name_PK = substance_name_PK;
			this.subst_name = subst_name;
			this.subst_name_type_FK = subst_name_type_FK;
			this.language_FK = language_FK;
		}
	}

	public interface ISubstance_name_PKOperations : ICRUDOperations<Substance_name_PK>
	{
        List<Substance_name_PK> GetSNBySubstancePK(Int32? SubstancePK);
	}
}
