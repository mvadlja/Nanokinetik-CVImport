// ======================================================================================================================
// Author:		Acer\Kiki
// Create date:	20.2.2012. 14:51:38
// Description:	GEM2 Generated class for table Ready_poss_wc.dbo.USER_GRID_SETTINGS
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
	[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "USER_GRID_SETTINGS")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
	[GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class User_grid_settings_PK
	{
		private Int32? _user_grid_settings_PK;
		private Int32? _user_FK;
		private String _grid_layout;
		private Boolean? _isdefault;
		private DateTime? _timestamp;
		private Boolean? _ql_visible;
        private String _grid_ID;
        private String _display_name;

		#region Properties

		[GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
		public Int32? user_grid_settings_PK
		{
			get { return _user_grid_settings_PK; }
			set { _user_grid_settings_PK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
		public Int32? user_FK
		{
			get { return _user_FK; }
			set { _user_FK = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String grid_layout
		{
			get { return _grid_layout; }
			set { _grid_layout = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
		public Boolean? isdefault
		{
			get { return _isdefault; }
			set { _isdefault = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.DateTime)]
		public DateTime? timestamp
		{
			get { return _timestamp; }
			set { _timestamp = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
		public Boolean? ql_visible
		{
			get { return _ql_visible; }
			set { _ql_visible = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String grid_ID
        {
            get { return _grid_ID; }
            set { _grid_ID = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String display_name
        {
            get { return _display_name; }
            set { _display_name = value; }
        }
		#endregion

		public User_grid_settings_PK() { }
		public User_grid_settings_PK(Int32? user_grid_settings_PK, Int32? user_FK, String grid_layout, Boolean? isdefault, DateTime? timestamp, Boolean? ql_visible, String grid_ID, String display_name)
		{
			this.user_grid_settings_PK = user_grid_settings_PK;
			this.user_FK = user_FK;
			this.grid_layout = grid_layout;
			this.isdefault = isdefault;
			this.timestamp = timestamp;
			this.ql_visible = ql_visible;
            this.grid_layout = grid_layout;
            this.grid_ID = grid_ID;
            this.display_name = display_name;
		}
	}

	public interface IUser_grid_settings_PKOperations : ICRUDOperations<User_grid_settings_PK>
	{
        List<User_grid_settings_PK> GetLayoutsByUsernameAndGrid(string username, string grid_ID);
        User_grid_settings_PK GetDefaultLayoutByUsernameAndGrid(string username, string grid_ID);
        void SetDefaultAndKeepFirstNLayouts(string username, string grid_ID, Int32? default_ugs_PK, Int32 num_to_keep);
        void DeleteLayoutsByUsernameAndGrid(string username, string grid_ID);
    }
}
