using System;

namespace AspNetUIFramework
{
    [Serializable()]
    public class XmlLocation
    {
        private object _id;
        private string _logicalUniqueName;
        private int? _rowId;
        private bool? _active;
        private string _displayName;
        private string _nameShort;
        private string _locationUrl;
        private LocationTarget _locationTarget;
        private string _description;
        private string _roles;
        private string _actions;
        private string _parentLocationID;
        private bool? _generateInTopMenu;
        private bool? _generateInTabMenu;
        private bool? _oldLocation;

        #region Properties

        public object Id
        {
            get { return _id; }
            set { _id = value; }
        }
        public string LogicalUniqueName
        {
            get { return _logicalUniqueName; }
            set { _logicalUniqueName = value; }
        }
        public int? RowId
        {
            get { return _rowId; }
            set { _rowId = value; }
        }
        public bool? Active
        {
            get { return _active; }
            set { _active = value; }
        }
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        public string NameShort
        {
            get { return _nameShort; }
            set { _nameShort = value; }
        }
        public string LocationUrl
        {
            get { return _locationUrl; }
            set { _locationUrl = value; }
        }
        public LocationTarget LocationTarget
        {
            get { return _locationTarget; }
            set { _locationTarget = value; }
        }
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        public string Roles
        {
            get { return _roles; }
            set { _roles = value; }
        }
        public string Actions
        {
            get { return _actions; }
            set { _actions = value; }
        }
        public string ParentLocationID
        {
            get { return _parentLocationID; }
            set { _parentLocationID = value; }
        }
        public bool? GenerateInTopMenu
        {
            get { return _generateInTopMenu; }
            set { _generateInTopMenu = value; }
        }

        public bool? GenerateInTabMenu
        {
            get { return _generateInTabMenu; }
            set { _generateInTabMenu = value; }
        }

        public bool? OldLocation
        {
            get { return _oldLocation; }
            set { _oldLocation = value; }
        }

        #endregion

        public XmlLocation() { }

        public XmlLocation(object id, string logicalUniqueName, int? rowId, bool? active, string DisplayName, string nameShort, string locationUrl, LocationTarget locationTarget, string description, string roles, string actions, string parentLocationID, bool generateInTopMenu, bool generateInTabMenu, bool oldLocation)
        {
            this.Id = id;
            this.LogicalUniqueName = logicalUniqueName;
            this.RowId = rowId;
            this.Active = active;
            this.DisplayName = DisplayName;
            this.NameShort = nameShort;
            this.LocationUrl = locationUrl;
            this.LocationTarget = locationTarget;
            this.Description = description;
            this.Roles = roles;
            this.Actions = actions;
            this.ParentLocationID = parentLocationID;
            this.GenerateInTopMenu = generateInTopMenu;
            this.GenerateInTabMenu = generateInTabMenu;
            this.OldLocation = oldLocation;
        }
    }
}
