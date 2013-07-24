// ======================================================================================================================
// Author:		KRISTIJAN-HPDV7\Kristijan
// Create date:	18.2.2013. 13:29:06
// Description:	GEM2 Generated class for table ReadyDevRBAC.dbo.USER_ROLE_ACTION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "USER_ROLE_ACTION")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class User_role_action_PK
	{
		private Int32? _user_role_action_PK;
		private Int32? _user_role_FK;
		private Int32? _location_FK;
		private Int32? _user_action_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? user_role_action_PK
		{
			get { return _user_role_action_PK; }
			set { _user_role_action_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_role_FK
		{
			get { return _user_role_FK; }
			set { _user_role_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? location_FK
		{
			get { return _location_FK; }
			set { _location_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_action_FK
		{
			get { return _user_action_FK; }
			set { _user_action_FK = value; }
		}

		#endregion

		public User_role_action_PK() { }
		public User_role_action_PK(Int32? user_role_action_PK, Int32? user_role_FK, Int32? location_FK, Int32? user_action_FK)
		{
			this.user_role_action_PK = user_role_action_PK;
			this.user_role_FK = user_role_FK;
			this.location_FK = location_FK;
			this.user_action_FK = user_action_FK;
		}
	}

	public interface IUser_role_action_PKOperations : ICRUDOperations<User_role_action_PK>
	{
        List<User_role_action_PK> GetEntitiesByUserRole(int userRolePk);
	}
}
