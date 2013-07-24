// ======================================================================================================================
// Author:		KRISTIJAN-HPDV7\Kristijan
// Create date:	18.2.2013. 15:59:08
// Description:	GEM2 Generated class for table ReadyDevRBAC.dbo.LOCATION_USER_ACTION_MN
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    [GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "LOCATION_USER_ACTION_MN")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Location_user_action_mn_PK
	{
		private Int32? _location_user_action_mn_PK;
		private Int32? _location_FK;
		private Int32? _user_action_FK;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? location_user_action_mn_PK
		{
			get { return _location_user_action_mn_PK; }
			set { _location_user_action_mn_PK = value; }
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

		public Location_user_action_mn_PK() { }
		public Location_user_action_mn_PK(Int32? location_user_action_mn_PK, Int32? location_FK, Int32? user_action_FK)
		{
			this.location_user_action_mn_PK = location_user_action_mn_PK;
			this.location_FK = location_FK;
			this.user_action_FK = user_action_FK;
		}
	}

	public interface ILocation_user_action_mn_PKOperations : ICRUDOperations<Location_user_action_mn_PK>
	{

	}
}
