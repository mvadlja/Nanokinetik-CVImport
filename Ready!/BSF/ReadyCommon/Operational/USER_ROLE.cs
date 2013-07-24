using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.ComponentModel;
using System.Runtime.Serialization;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
    [GEMAuditing(DataSourceId = "Default", Database = "Kmis", Table = "USER_ROLE", Active = true)]
    public class USER_ROLE
	{
		private Int32? _user_role_PK;
		private String _name;
		private String _display_name;
		private String _description;
		private Boolean? _active;
		private DateTime? _row_version;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? User_role_PK
		{
            get { return _user_role_PK; }
			set
			{
                _user_role_PK = value;
			}
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String Name
		{
			get { return _name; }
			set
			{
				_name = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String Display_name
		{
            get { return _display_name; }
			set
			{
                _display_name = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String Description
		{
			get { return _description; }
			set
			{
				_description = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
		public Boolean? Active
		{
			get { return _active; }
			set
			{
				_active = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", IsRowVersion = true, ParameterType = DbType.DateTime)]
		public DateTime? Row_version
		{
            get { return _row_version; }
			set
			{
                _row_version = value;
			}
		}

		#endregion

		public USER_ROLE() { }
        public USER_ROLE(Int32? user_role_PK, String name, String display_name, String description, Boolean? active, DateTime? row_version)
		{
            this.User_role_PK = user_role_PK;
			this.Name = name;
            this.Display_name = display_name;
			this.Description = description;
            this.Active = active;
            this.Row_version = row_version;
		}
	}

    public interface IUSER_ROLEOperations : ICRUDOperations<USER_ROLE>
	{
        List<USER_ROLE> GetRolesByUserID(Int32 userID);
        List<USER_ROLE> GetActiveRolesByUserID(Int32 userID);
        USER_ROLE GetRoleByName(String name);
        List<USER_ROLE> GetActiveRolesByUsername(String username);
        List<USER_ROLE> GetVisibleRoles();

        DataSet GetListFormDataSet(Dictionary<string, string> filters, int pageNumber, int pageSize, List<GEMOrderBy> orderByConditions, out int totalRecordsCount);
        List<USER_ROLE> GetAvailableEntitiesByUser(int userPk);
        List<USER_ROLE> GetAssignedEntitiesByUser(int userPk);
    }
}
