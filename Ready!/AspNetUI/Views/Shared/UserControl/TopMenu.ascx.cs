using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using System.Configuration;
using Ready.Model;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class TopMenu : System.Web.UI.UserControl
    {
        Template.Default _masterPage = null;

        public Permission OneTimePermissionToken
        {
            get { return ViewState["OneTimePermissionToken"] != null ? (Permission)ViewState["OneTimePermissionToken"] : Permission.None; }
            set { ViewState["OneTimePermissionToken"] = value; }
        }

        public Location_PK RefererLocation
        {
            get { return ViewState["refererLocation"] != null ? (Location_PK)ViewState["refererLocation"] : null; }
            set { ViewState["refererLocation"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _masterPage = (Template.Default)Page.Master;

            if (!IsPostBack)
            {
                if (_masterPage != null)
                {
                    OneTimePermissionToken = _masterPage.OneTimePermissionToken;
                    RefererLocation = LocationManager.GetRefererLocation();
                }
            }
        }

        public void GenerateNewTopMenu(List<Location_PK> locations, Location_PK topLevelParent, Location_PK currentLocation)
        {
            var topMenuString = "<div id=menuh>";

            // Top menu location
            var topMenuLocations = LocationManager.Instance.GetLocationsByNameContains("TopMenu", CacheManager.Instance.AppLocations);

            foreach (var topMenuLocation in topMenuLocations)
            {
                if (topMenuLocation == null) continue;

                // Getting children of top menu location (menu items)
                List<Location_PK> topMenuChildrenSections = LocationManager.Instance.GetChildLocations(topMenuLocation.unique_name, CacheManager.Instance.AppLocations).OrderBy(loc => loc.menu_order).ToList();

                foreach (Location_PK location in topMenuChildrenSections)
                {
                    // Bind only active locations that have parent Level1-TopMenu
                    if (location.active == true)
                    {
                        if (!SecurityHelper.IsPermitted(Permission.View, location)) continue;

                        Location_PK tempLocation = LocationManager.Instance.FindFirstAuthorizedActiveChildFileLocationInHierarchy(new List<Location_PK> { location }, locations);
                       
                        if (tempLocation != null)
                        {
                            List<Location_PK> subMenuParentSections2 = LocationManager.Instance.GetChildLocations(location.unique_name, CacheManager.Instance.AppLocations).ToList();
                            var subMenuParentSections = subMenuParentSections2.Where(loc2 => loc2.generate_in_top_menu != null && (bool)loc2.generate_in_top_menu).OrderBy(loc => loc.menu_order).ToList();

                            // Search for top level children and if any, add to 1st level submenu
                            if (subMenuParentSections.Count > 1)
                            {
                                var link = string.Format("{0}/{1}", ConfigurationManager.AppSettings["AppVirtualPath"], tempLocation.location_url.Substring(2));

                                if (!SecurityHelper.IsPermitted(Permission.View, tempLocation) && (RefererLocation == null || !SecurityHelper.IsPermitted(Permission.View, RefererLocation))) continue;
                                

                                if (location == topLevelParent)
                                {
                                    topMenuString += string.Format("<ul><li class=topMenuItemSelected><a href={0} class=top_parent_selected>{1}</a>", link, location.display_name);
                                }
                                else
                                {
                                    topMenuString += string.Format("<ul><li><a href={0} class=top_parent>{1}</a>", link, location.display_name);
                                }

                                var subMenuItemsTmp = LocationManager.Instance.GetChildLocations(location.unique_name, CacheManager.Instance.AppLocations)
                                                                                      .Where(loc => loc.generate_in_top_menu != null && (bool)loc.generate_in_top_menu).OrderBy(loc => loc.menu_order).ToList();

                                topMenuString += "<ul>";
                                foreach (var itemTmp in subMenuItemsTmp)
                                {
                                    if (!SecurityHelper.IsPermitted(Permission.View, itemTmp)) // && (refererLocation == null || !SecurityHelper.IsPermitted(Permission.View, refererLocation)))
                                    {
                                        continue;
                                    }

                                    List<Location_PK> subMenuItemsTmp2 = LocationManager.Instance.GetChildLocations(itemTmp.unique_name, CacheManager.Instance.AppLocations);
                                    int count = subMenuItemsTmp2.Count;
                                    foreach (Location_PK locTmp in subMenuItemsTmp2)
                                    {
                                        if (locTmp.generate_in_top_menu != null && !(bool)locTmp.generate_in_top_menu) --count;
                                    }

                                    bool relPath;
                                    if (count > 1)
                                    {
                                        if (itemTmp.unique_name == currentLocation.unique_name)
                                        {
                                            topMenuString += string.Format("<li class=topMenuItemSelected><a href=# class=top_parent_selected>{0}</a>", itemTmp.display_name);
                                        }
                                        else
                                        {
                                            topMenuString += string.Format("<li><a href=# class=parent>{0}</a>", itemTmp.display_name);
                                        }

                                        List<Location_PK> subMenuItemsTmp3 = LocationManager.Instance.GetChildLocations(itemTmp.unique_name, CacheManager.Instance.AppLocations).OrderBy(loc => loc.menu_order).ToList();

                                        topMenuString += "<ul>";
                                        foreach (var itemTmp2 in subMenuItemsTmp3)
                                        {
                                            relPath = itemTmp2.location_url.Contains("~");
                                            if (relPath)
                                            {
                                                topMenuString += string.Format("<li><a href={0}/{1}>{2}</a></li>", ConfigurationManager.AppSettings["AppVirtualPath"], itemTmp2.location_url.Substring(2), itemTmp2.display_name);
                                            }
                                            else
                                            {
                                                topMenuString += string.Format("<li><a href={0}>{1}</a></li>", itemTmp2.location_url, itemTmp2.display_name);
                                            }
                                        }
                                        topMenuString += "</ul></li>";
                                    }
                                    else if (count == 1)
                                    {
                                        // Only one child, add item to 1st level submenu with its data (LocationUrl, display_name)
                                        Location_PK itemTmp3 = LocationManager.Instance.GetChildLocations(itemTmp.unique_name, CacheManager.Instance.AppLocations)[0];
                                        if (!SecurityHelper.IsPermitted(Permission.View, itemTmp3) && (RefererLocation == null || !SecurityHelper.IsPermitted(Permission.View, RefererLocation))) continue;

                                        relPath = itemTmp3.location_url.Contains("~");
                                        if (relPath)
                                        {
                                            topMenuString += string.Format("<li><a href={0}/{1}>{2}</a></li>", ConfigurationManager.AppSettings["AppVirtualPath"], itemTmp3.location_url.Substring(2), itemTmp3.display_name);
                                        }
                                        else
                                        {
                                            topMenuString += string.Format("<li><a href={0}>{1}</a></li>", itemTmp3.location_url, itemTmp3.display_name);
                                        }
                                    }
                                    else
                                    {
                                        if (!SecurityHelper.IsPermitted(Permission.View, itemTmp) && (RefererLocation == null || !SecurityHelper.IsPermitted(Permission.View, RefererLocation))) continue;

                                        // No 2nd level children, add item to 1st level submenu
                                        var cssclass = string.Empty;
                                        var cssclass2 = string.Empty;
                                        if (itemTmp.unique_name == currentLocation.unique_name && itemTmp.parent_unique_name == currentLocation.parent_unique_name)
                                        {
                                            cssclass = " class=topMenuItemSelected";
                                            cssclass2 = " class=top_parent_selected";
                                        }
                                        relPath = itemTmp.location_url.Contains("~");
                                        if (relPath)
                                        {
                                            topMenuString += string.Format("<li{0}><a {1} href={2}/{3}>{4}</a></li>", cssclass, cssclass2, ConfigurationManager.AppSettings["AppVirtualPath"], itemTmp.location_url.Substring(2), itemTmp.display_name);
                                        }
                                        else
                                        {
                                            topMenuString += string.Format("<li{0}><a {1} href={2}>{3}</a></li>", cssclass, cssclass2, itemTmp.location_url, itemTmp.display_name);
                                        }
                                    }

                                }

                                topMenuString += "</ul></li></ul>";

                            }
                            else if (subMenuParentSections.Count == 1)
                            {
                                // No top level children, add item to top menu
                                topMenuString += "<ul><li><a href=" + ConfigurationManager.AppSettings["AppVirtualPath"] + "/" + subMenuParentSections[0].location_url.Substring(2) + " class=top_parent>" + location.display_name + "</a></li></ul>";
                            }
                        }
                    }
                }
            }

            // End of main DIV tag
            topMenuString += "</div>";

            // Add created DIV tag to the table row
            var tc = new TableCell();
            var tr = new TableRow();
            tc.Text = topMenuString;
            tr.Cells.Add(tc);
            tblTopMenu.Rows.Clear();
            tblTopMenu.Rows.Add(tr);
        }

        public void SelectItem(Location_PK location)
        {
            var selectedItem = (TableCell)tblTopMenu.FindControl("topMenuCell_" + location.unique_name) ?? (TableCell)tblTopMenu.FindControl("topMenuCell_" + location.parent_unique_name);

            if (selectedItem != null) selectedItem.CssClass = "topMenuItemSelected";
        }

        public void UnSelectItem(Location_PK location)
        {
            var selectedItem = (TableCell)tblTopMenu.FindControl("topMenuCell_" + location.unique_name);

            if (selectedItem != null) selectedItem.CssClass = "topMenuItem";
        }
    }
}