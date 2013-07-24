using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Ready.Model;

namespace AspNetUI.Views
{
    public partial class LocationsImport : System.Web.UI.Page
    {
        private ILocation_PKOperations _locationOperations;
        private List<Location_PK> locations;
        private string display_name = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            _locationOperations = new Location_PKDAL(); 
        }

        protected void btnImportLocations_Click(object sender, EventArgs e)
        {
            // locations = _locationOperations.GetEntities();
            // foreach (var location in locations) _locationOperations.Delete(location.unique_name);
            btnImportLocations.Enabled = false;

            var fs = new FileStream(@"D:\Nanokinetik work\ReadyDevRBAC\AspNetUI\App_Data\Locations.xml", FileMode.Open);
            var xmlreader = XmlReader.Create(fs);
            var parentLogicalUniqueNames = new Dictionary<int, string>();
            var menuOrder = new Dictionary<int, int>();
            var parentLogicalUniqueName = string.Empty;
            int? currentNavigationLevel = 0;

            while (xmlreader.Read())
            {
                if (xmlreader.NodeType == XmlNodeType.Element)
                {
                    string logicalUniqueName = xmlreader.GetAttribute("LogicalUniqueName");
                    if (!string.IsNullOrWhiteSpace(logicalUniqueName))
                    {
                        var location = _locationOperations.GetEntityByUniqueName(logicalUniqueName.Substring(7, logicalUniqueName.Length - 7)) ?? new Location_PK();
                        location.navigation_level = Convert.ToInt32((logicalUniqueName).ToCharArray()[5].ToString());

                        if (currentNavigationLevel != location.navigation_level)
                        {
                            if(menuOrder.ContainsKey(currentNavigationLevel.Value))
                            {
                                if (currentNavigationLevel > location.navigation_level) menuOrder[currentNavigationLevel.Value] = 1;
                            }

                            currentNavigationLevel = location.navigation_level;
                        } 
                        if (!menuOrder.ContainsKey(currentNavigationLevel.Value)) menuOrder.Add(currentNavigationLevel.Value, 1);

                        if (!xmlreader.IsEmptyElement)
                        {
                            string tmp = string.Empty;
                            parentLogicalUniqueName = logicalUniqueName.Substring(7, logicalUniqueName.Length - 7);
                            if (parentLogicalUniqueNames.ContainsKey((int)location.navigation_level))
                            {
                                parentLogicalUniqueNames.Remove((int) location.navigation_level);
                                parentLogicalUniqueNames.Add((int) location.navigation_level, parentLogicalUniqueName);
                            }
                            else
                            {
                                parentLogicalUniqueNames.Add((int) location.navigation_level, parentLogicalUniqueName);
                            }

                            if ((int) location.navigation_level != 1)
                            {
                                if (parentLogicalUniqueNames.ContainsKey((int)location.navigation_level - 1))
                                {
                                    if (parentLogicalUniqueNames.TryGetValue((int)location.navigation_level - 1, out tmp))
                                    {
                                        location.parent_unique_name = tmp;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(parentLogicalUniqueName))
                            {
                                string tmp = string.Empty;
                                if (parentLogicalUniqueNames.ContainsKey((int) location.navigation_level - 1))
                                {
                                    if (parentLogicalUniqueNames.TryGetValue((int) location.navigation_level - 1, out tmp))
                                    {
                                        location.parent_unique_name = tmp;
                                    }
                                }
                            }
                        }

                        location.unique_name = logicalUniqueName.Substring(7, logicalUniqueName.Length - 7);
                        location.display_name = xmlreader.GetAttribute("DisplayName", string.Empty);

                        var generateInTabMenu = xmlreader.GetAttribute("GenerateInTabMenu", string.Empty);
                        location.generate_in_tab_menu = string.IsNullOrWhiteSpace(generateInTabMenu) || Convert.ToBoolean(generateInTabMenu);

                        var generateInTopMenu = xmlreader.GetAttribute("GenerateInTopMenu", string.Empty);
                        location.generate_in_top_menu = string.IsNullOrWhiteSpace(generateInTopMenu) || Convert.ToBoolean(generateInTopMenu);

                        var oldLocation = xmlreader.GetAttribute("OldLocation", string.Empty);
                        location.old_location = !string.IsNullOrWhiteSpace(oldLocation) && Convert.ToBoolean(oldLocation);

                        location.location_url = xmlreader.GetAttribute("LocationUrl", string.Empty); 
                        location.active = Convert.ToBoolean(xmlreader.GetAttribute("Active", string.Empty));
                        location.location_target = xmlreader.GetAttribute("LocationTarget", string.Empty);
                        location.description = xmlreader.GetAttribute("Description", string.Empty);
                        location.menu_order = !location.old_location.Value && location.navigation_level != 0 ? menuOrder[currentNavigationLevel.Value]++ : 1;

                        _locationOperations.Save(location);
                    }
                }
            }

            fs.Close();
            btnUpdateFullPath_Click(null, null);

            btnImportLocations.Enabled = true;
        }

        protected void btnUpdateFullPath_Click(object sender, EventArgs e)
        {
            btnUpdateFullPath.Enabled = false;
            locations = _locationOperations.GetEntities();
            foreach (var location in locations)
            {
                display_name = string.Empty;
                location.full_unique_path = GetDisplayName(location);
                location.full_unique_path += location.display_name;
                _locationOperations.Save(location);
            }
            btnUpdateFullPath.Enabled = true;
        }

        private string GetFullUniquePath(Location_PK location)
        {
            var parent = GetParent(location);
            if (parent != null)
            {
                GetFullUniquePath(parent);
            }

            return parent != null ? display_name += !string.IsNullOrWhiteSpace(parent.unique_name) ? parent.unique_name + "-" : string.Empty : string.Empty;
        }

        private string GetDisplayName(Location_PK location)
        {
            var parent = GetParent(location);
            if (parent != null)
            {
                GetDisplayName(parent);
            }

            return parent != null ? display_name += !string.IsNullOrWhiteSpace(parent.display_name) ? parent.display_name + "-" : string.Empty : string.Empty;
        }

        private Location_PK GetParent(Location_PK location)
        {
            return locations.Find(loc => loc.unique_name == location.parent_unique_name);
        }
    }
}