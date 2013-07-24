// ======================================================================================================================
// Author:		Mateo-HP\Mateo
// Create date:	8.12.2011. 10:11:06
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.PROJECT_COUNTRY_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "PROJECT_COUNTRY_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Project_country_PK
	{
		private Int32? _project_country_PK;
		private Int32? _project_FK;
		private Int32? _country_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? project_country_PK
		{
			get { return _project_country_PK; }
			set { _project_country_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? project_FK
		{
			get { return _project_FK; }
			set { _project_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? country_FK
		{
			get { return _country_FK; }
			set { _country_FK = value; }
		}

		#endregion

		public Project_country_PK() { }
		public Project_country_PK(Int32? project_country_PK, Int32? project_FK, Int32? country_FK)
		{
			this.project_country_PK = project_country_PK;
			this.project_FK = project_FK;
			this.country_FK = country_FK;
		}
	}

	public interface IProject_country_PKOperations : ICRUDOperations<Project_country_PK>
	{
        DataSet GetCountriesByProject(int projectID);

        void deleteByProject(int projectID);
    }
}
