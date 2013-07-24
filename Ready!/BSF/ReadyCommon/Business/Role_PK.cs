// ======================================================================================================================
// Author:		POSSBOOK-DV7\Hrvoje
// Create date:	23.1.2013. 13:22:29
// Description:	GEM2 Generated class for table ReadyRBAC.dbo.ROLE
// ======================================================================================================================

using System;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "ROLE")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Role_PK
	{
		private String _unique_name;
		private String _display_name;
		private String _description;
		private Boolean? _active;

		#region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.String)]
		public String unique_name
		{
			get { return _unique_name; }
			set { _unique_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String display_name
		{
			get { return _display_name; }
			set { _display_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String description
		{
			get { return _description; }
			set { _description = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
		public Boolean? active
		{
			get { return _active; }
			set { _active = value; }
		}

		#endregion

		public Role_PK() { }
		public Role_PK(String unique_name, String display_name, String description, Boolean? active)
		{
			this.unique_name = unique_name;
			this.display_name = display_name;
			this.description = description;
			this.active = active;
		}
	}

	public interface IRole_PKOperations : ICRUDOperations<Role_PK>
	{

	}
}
