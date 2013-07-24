// ======================================================================================================================
// Author:		ACER\Kiki
// Create date:	6.11.2011. 1:00:07
// Description:	GEM2 Generated class for table SSI.dbo.SUBSTANCE_CODE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SUBSTANCE_CODE")]
    //[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Substance_code_PK
	{
		private Int32? _substance_code_PK;
		private String _code;
		private Int32? _code_system_FK;
		private Int32? _code_system_id_FK;
		private Int32? _code_system_status_FK;
		private String _code_system_changedate;
		private String _comment;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? substance_code_PK
		{
			get { return _substance_code_PK; }
			set { _substance_code_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String code
		{
			get { return _code; }
			set { _code = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? code_system_FK
		{
			get { return _code_system_FK; }
			set { _code_system_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? code_system_id_FK
		{
			get { return _code_system_id_FK; }
			set { _code_system_id_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? code_system_status_FK
		{
			get { return _code_system_status_FK; }
			set { _code_system_status_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String code_system_changedate
		{
			get { return _code_system_changedate; }
			set { _code_system_changedate = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String comment
		{
			get { return _comment; }
			set { _comment = value; }
		}

		#endregion

		public Substance_code_PK() { }
		public Substance_code_PK(Int32? substance_code_PK, String code, Int32? code_system_FK, Int32? code_system_id_FK, Int32? code_system_status_FK, String code_system_changedate, String comment)
		{
			this.substance_code_PK = substance_code_PK;
			this.code = code;
			this.code_system_FK = code_system_FK;
			this.code_system_id_FK = code_system_id_FK;
			this.code_system_status_FK = code_system_status_FK;
			this.code_system_changedate = code_system_changedate;
			this.comment = comment;
		}
	}

	public interface ISubstance_code_PKOperations : ICRUDOperations<Substance_code_PK>
	{
        List<Substance_code_PK> GetSCBySubstancePK(Int32? SubstancePK);
	}
}
