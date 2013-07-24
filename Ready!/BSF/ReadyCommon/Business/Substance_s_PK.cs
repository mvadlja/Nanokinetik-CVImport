// ======================================================================================================================
// Author:		ANTEC\Kiki
// Create date:	8.1.2012. 22:57:15
// Description:	GEM2 Generated class for table SSI.dbo.SUBSTANCE
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	//[GEMAuditing(DataSourceId = "Default", Database = "SSI", Table = "SUBSTANCE")]
	//[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "SSI_Default")]
	public class Substance_s_PK
	{
		private Int32? _substance_s_PK;
		private Int32? _language;
		private Int32? _substance_id;
		private Int32? _substance_class;
		private Int32? _ref_info_FK;
		private Int32? _sing_FK;
		private Int32? _responsible_user_FK;
		private String _description;
		private String _comments;
		private String _name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? substance_s_PK
		{
			get { return _substance_s_PK; }
			set { _substance_s_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? language
		{
			get { return _language; }
			set { _language = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substance_id
		{
			get { return _substance_id; }
			set { _substance_id = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? substance_class
		{
			get { return _substance_class; }
			set { _substance_class = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? ref_info_FK
		{
			get { return _ref_info_FK; }
			set { _ref_info_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? sing_FK
		{
			get { return _sing_FK; }
			set { _sing_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? responsible_user_FK
		{
			get { return _responsible_user_FK; }
			set { _responsible_user_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String description
		{
			get { return _description; }
			set { _description = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String comments
		{
			get { return _comments; }
			set { _comments = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String name
		{
			get { return _name; }
			set { _name = value; }
		}

		#endregion

		public Substance_s_PK() { }
		public Substance_s_PK(Int32? substance_s_PK, Int32? language, Int32? substance_id, Int32? substance_class, Int32? ref_info_FK, Int32? sing_FK, Int32? responsible_user_FK, String description, String comments, String name)
		{
			this.substance_s_PK = substance_s_PK;
			this.language = language;
			this.substance_id = substance_id;
			this.substance_class = substance_class;
			this.ref_info_FK = ref_info_FK;
			this.sing_FK = sing_FK;
			this.responsible_user_FK = responsible_user_FK;
			this.description = description;
			this.comments = comments;
			this.name = name;
		}
	}

	public interface ISubstance_s_PKOperations : ICRUDOperations<Substance_s_PK>
	{
        DataSet GetSubstancesDataSet(int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
	}
}
