// ======================================================================================================================
// Author:		KRISTIJAN-HPDV7\Kristijan
// Create date:	18.2.2013. 13:27:41
// Description:	GEM2 Generated class for table ReadyDevRBAC.dbo.USER_ACTION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "USER_ACTION")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class User_action_PK
	{
		private Int32? _user_action_PK;
		private String _unique_name;
		private String _display_name;
		private String _description;
		private Boolean? _active;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? user_action_PK
		{
			get { return _user_action_PK; }
			set { _user_action_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
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

		public User_action_PK() { }
		public User_action_PK(Int32? user_action_PK, String unique_name, String display_name, String description, Boolean? active)
		{
			this.user_action_PK = user_action_PK;
			this.unique_name = unique_name;
			this.display_name = display_name;
			this.description = description;
			this.active = active;
		}
	}

	public interface IUser_action_PKOperations : ICRUDOperations<User_action_PK>
	{
        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        List<User_action_PK> GetEntitiesByLocation(int locationPk);
	}
}
