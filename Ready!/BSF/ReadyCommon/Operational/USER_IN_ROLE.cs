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
    [GEMAuditing(DataSourceId = "Default", Database = "Kmis", Table = "USER_IN_ROLE", Active = true)]
    public class USER_IN_ROLE
	{
		private Int32? _user_in_role_PK;
		private Int32? _user_FK;
		private Int32? _user_role_FK;
		private DateTime? _row_version;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? User_in_role_PK
		{
            get { return _user_in_role_PK; }
			set
			{
                _user_in_role_PK = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? User_FK
		{
            get { return _user_FK; }
			set
			{
                _user_FK = value;
			}
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? User_role_FK
		{
            get { return _user_role_FK; }
			set
			{
                _user_role_FK = value;
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

		public USER_IN_ROLE() { }
        public USER_IN_ROLE(Int32? user_in_role_PK, Int32? user_FK, Int32? user_role_FK, DateTime? row_version)
		{
			this.User_in_role_PK= user_in_role_PK;
			this.User_FK = user_FK;
            this.User_role_FK = user_role_FK;
            this.Row_version = row_version;
		}
	}

    public interface IUSER_IN_ROLEOperations : ICRUDOperations<USER_IN_ROLE>
	{
        USER_IN_ROLE GetUserInRoleByRoleIDAndUserID(Int32 userID, Int32 roleID);
        List<USER_IN_ROLE> GetUsersInRolesByUserID(Int32 userID);
        List<USER_IN_ROLE> GetUsersInRolesByRoleID(Int32 roleID);
        void DeleteByUserRole(int userRolePk);
        void DeleteByUser(int userPk);
	}
}
