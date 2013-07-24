// ======================================================================================================================
// Author:		POSSBOOK-DV7\Hrvoje
// Create date:	23.1.2013. 13:20:17
// Description:	GEM2 Generated class for table ReadyRBAC.dbo.LOCATION
// ======================================================================================================================

using System;
using System.Collections.Generic;
using System.Data;
using GEM2Common;

namespace Ready.Model
{
	[Serializable()]
    //[GEMAuditing(DataSourceId = "Default", Database = "Ready_poss_wc", Table = "LOCATION")]
	[GEMOperationsLogging(DataSourceId = "Default", Active = true)]
    [GEMDataSourceBinding(DataSourceId = "Default", ConnectionStringName = "Ready_poss_wc")]
	public class Location_PK
	{
        private Int32? _location_PK;
		private String _unique_name;
		private String _display_name;
		private String _description;
        private Int32? _navigation_level;
		private Boolean? _generate_in_top_menu;
		private Boolean? _generate_in_tab_menu;
		private Boolean? _active;
        private String _parent_unique_name;
        private String _location_target;
        private String _full_unique_path;
        private String _location_url;
        private Boolean? _old_location;
        private Int32? _menu_order;
        private String _permission;
        private Boolean? _show_location;

		#region Properties

        [GEMPropertyBinding(DataSourceId = "Default", IsPrimaryKey = true, ParameterType = DbType.Int32)]
        public Int32? location_PK
        {
            get { return _location_PK; }
            set { _location_PK = value; }
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

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? navigation_level
        {
            get { return _navigation_level; }
            set { _navigation_level = value; }
        }

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
		public Boolean? generate_in_top_menu
		{
			get { return _generate_in_top_menu; }
			set { _generate_in_top_menu = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
		public Boolean? generate_in_tab_menu
		{
			get { return _generate_in_tab_menu; }
			set { _generate_in_tab_menu = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
		public Boolean? active
		{
			get { return _active; }
			set { _active = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String parent_unique_name
		{
			get { return _parent_unique_name; }
			set { _parent_unique_name = value; }
		}

		[GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
		public String location_target
		{
			get { return _location_target; }
			set { _location_target = value; }
		}

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String full_unique_path
        {
            get { return _full_unique_path; }
            set { _full_unique_path = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String location_url
        {
            get { return _location_url; }
            set { _location_url = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? old_location
        {
            get { return _old_location; }
            set { _old_location = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Int32)]
        public Int32? menu_order
        {
            get { return _menu_order; }
            set { _menu_order = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.String)]
        public String permission
        {
            get { return _permission; }
            set { _permission = value; }
        }

        [GEMPropertyBinding(DataSourceId = "Default", ParameterType = DbType.Boolean)]
        public Boolean? show_location
        {
            get { return _show_location; }
            set { _show_location = value; }
        }

		#endregion

		public Location_PK() { }
        public Location_PK(Int32? location_PK, String unique_name, String display_name, String description, Int32? navigation_level, Boolean? generate_in_top_menu, Boolean? generate_in_tab_menu, Boolean? active, String parent_unique_name, String location_target, String full_unique_path, String location_url, Boolean old_location, Int32? menu_order, String permission, Boolean show_location)
		{
            this.location_PK = location_PK;
			this.unique_name = unique_name;
			this.display_name = display_name;
			this.description = description;
            this.navigation_level = navigation_level;
            this.generate_in_top_menu = generate_in_top_menu;
			this.generate_in_tab_menu = generate_in_tab_menu;
			this.active = active;
			this.parent_unique_name = parent_unique_name;
			this.location_target = location_target;
            this.full_unique_path = full_unique_path;
            this.location_url = location_url;
            this.old_location = old_location;
            this.menu_order = menu_order;
            this.permission = permission;
            this.show_location = show_location;
		}
	}

	public interface ILocation_PKOperations : ICRUDOperations<Location_PK>
	{
        Location_PK GetEntityByUniqueName(string uniqueName);
        List<Location_PK> GetUserPermissions(string username);
	}
}
