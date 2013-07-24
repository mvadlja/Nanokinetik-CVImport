using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using System.Configuration;
using CommonTypes;

namespace AspNetUI.Support
{
    public partial class TopMenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void GenerateMenuItems(List<XmlLocation> locations)
        {
            XmlLocation tempLocation;
            TableCell tc;
            TableRow tr = new TableRow();

            //tblTopMenu.Rows.Clear();

            // Top menu location
            XmlLocation topMenuLocation = AspNetUIFramework.LocationManager.Instance.GetLocationByName("Level1-TopMenu", AspNetUIFramework.CacheManager.Instance.AppLocations);

            if (topMenuLocation != null)
            {
                // Getting children of top menu location (menu items)
                List<XmlLocation> topMenuChildrenSections = AspNetUIFramework.LocationManager.Instance.GetChildLocations(topMenuLocation.LogicalUniqueName, AspNetUIFramework.CacheManager.Instance.AppLocations);

                foreach (XmlLocation loc in topMenuChildrenSections)
                {
                    // Bind only active locations that have parent Level1-TopMenu
                    if (loc.Active == true)
                    {
                        // Right on this location
                        RightTypes right = AspNetUIFramework.LocationManager.Instance.AuthorizeLocation(loc, AspNetUIFramework.CacheManager.Instance.AppLocations);
                        if (right == RightTypes.Restricted) continue;

                        // Always bind url location of first child - top level locations do not point to aspx
                        tempLocation = AspNetUIFramework.LocationManager.Instance.FindFirstAuthorizedActiveChildFileLocationInHierarchy(new List<XmlLocation>() { loc }, locations);

                        if (tempLocation != null)
                        {
                            tc = new TableCell();
                            tc.ID = "topMenuCell_" + loc.LogicalUniqueName;
                            tc.Text = loc.DisplayName;
                            tc.ToolTip = loc.Description;

                            tc.CssClass = "topMenuItem";

                            if (tempLocation.LocationTarget == LocationTarget._self)
                            {
                                tc.Attributes["onclick"] = "javascript:document.location='" + tempLocation.LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + "'";
                            }
                            else if (tempLocation.LocationTarget == LocationTarget._blank)
                            {
                                tc.Attributes["onclick"] = "javascript:window.open('" + tempLocation.LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + "', '" + tempLocation.DisplayName + "');";
                            }

                            tr.Cells.Add(tc);
                        }
                    }
                }
            }

            tblTopMenu.Rows.Add(tr);
        }

        public void GenerateNewTopMenu(List<XmlLocation> locations, XmlLocation topLevelParent, XmlLocation currentLocation)
        {
            Dictionary<string, List<XmlLocation>> subMenuItems2 = new Dictionary<string, List<XmlLocation>>();
            List<string> subMenuItems = new List<string>();
            XmlLocation tempLocation;
            
            string topMenuString = "<div id=menuh>";

            //tblTopMenu.Rows.Clear();

            // Top menu location
            XmlLocation topMenuLocation = AspNetUIFramework.LocationManager.Instance.GetLocationByName("Level1-TopMenu", AspNetUIFramework.CacheManager.Instance.AppLocations);

            if (topMenuLocation != null)
            {
                // Getting children of top menu location (menu items)
                List<XmlLocation> topMenuChildrenSections = AspNetUIFramework.LocationManager.Instance.GetChildLocations(topMenuLocation.LogicalUniqueName, AspNetUIFramework.CacheManager.Instance.AppLocations);

                foreach (XmlLocation loc in topMenuChildrenSections)
                {
                    // Bind only active locations that have parent Level1-TopMenu
                    if (loc.Active == true)
                    {
                        // Right on this location
                        RightTypes right = AspNetUIFramework.LocationManager.Instance.AuthorizeLocation(loc, AspNetUIFramework.CacheManager.Instance.AppLocations);
                        if (right == RightTypes.Restricted) continue;

                        // Always bind url location of first child - top level locations do not point to aspx
                        tempLocation = AspNetUIFramework.LocationManager.Instance.FindFirstAuthorizedActiveChildFileLocationInHierarchy(new List<XmlLocation>() { loc }, locations);


                        if (tempLocation != null)
                        {
                            List<XmlLocation> subMenuParentSections2 = AspNetUIFramework.LocationManager.Instance.GetChildLocations(loc.LogicalUniqueName, AspNetUIFramework.CacheManager.Instance.AppLocations);
                            List<XmlLocation> subMenuParentSections = new List<XmlLocation>();
                            foreach (XmlLocation loc2 in subMenuParentSections2)
                                if ((bool)loc2.GenerateInTopMenu) subMenuParentSections.Add(loc2);


                            // Search for top level children and if any, add to 1st level submenu
                            if (subMenuParentSections.Count > 1)
                            {
                                string link;
                                if (loc.DisplayName == "READY!")
                                    link = ConfigurationManager.AppSettings["AppVirtualPath"] + "/Views/AuthorisedProductView/List.aspx";
                                else
                                    link = ConfigurationManager.AppSettings["AppVirtualPath"] + "/" + tempLocation.LocationUrl.Substring(2);
                                if(loc == topLevelParent)
                                    topMenuString += "<ul><li class=topMenuItemSelected><a href=" + link + " class=top_parent_selected>" + loc.DisplayName + "</a>";
                                else
                                    topMenuString += "<ul><li><a href=" + link + " class=top_parent>" + loc.DisplayName + "</a>";

                                List<XmlLocation> subMenuItemsTmp4 = AspNetUIFramework.LocationManager.Instance.GetChildLocations(loc.LogicalUniqueName, AspNetUIFramework.CacheManager.Instance.AppLocations);
                                List<XmlLocation> subMenuItemsTmp = new List<XmlLocation>();
                                foreach (XmlLocation loc2 in subMenuItemsTmp4)
                                    if ((bool)loc2.GenerateInTopMenu) subMenuItemsTmp.Add(loc2);

                                topMenuString += "<ul>";
                                foreach (var itemTmp in subMenuItemsTmp)
                                {
                                    List<XmlLocation> subMenuItemsTmp2 = AspNetUIFramework.LocationManager.Instance.GetChildLocations(itemTmp.LogicalUniqueName, AspNetUIFramework.CacheManager.Instance.AppLocations);
                                    int count = subMenuItemsTmp2.Count;
                                    foreach (XmlLocation locTmp in subMenuItemsTmp2)
                                    {
                                        if (!(bool)locTmp.GenerateInTopMenu)
                                            --count;
                                    }

                                    bool relPath = true;
                                    // Search for 2nd level children and if any, add item to 2nd level submenu
                                    if (count > 1)
                                    {
                                        if(itemTmp.LogicalUniqueName == currentLocation.LogicalUniqueName)
                                            topMenuString += "<li class=topMenuItemSelected><a href=# class=top_parent_selected>" + itemTmp.DisplayName + "</a>";
                                        else
                                            topMenuString += "<li><a href=# class=parent>" + itemTmp.DisplayName + "</a>";

                                        List<XmlLocation> subMenuItemsTmp3 = AspNetUIFramework.LocationManager.Instance.GetChildLocations(itemTmp.LogicalUniqueName, AspNetUIFramework.CacheManager.Instance.AppLocations);

                                        topMenuString += "<ul>";
                                        foreach (var itemTmp2 in subMenuItemsTmp3)
                                        {
                                            relPath = itemTmp2.LocationUrl.Contains("~");
                                            if (relPath)
                                            {
                                                topMenuString += "<li><a href=" + ConfigurationManager.AppSettings["AppVirtualPath"] + "/" + itemTmp2.LocationUrl.Substring(2) + ">" + itemTmp2.DisplayName + "</a></li>";
                                            }
                                            else
                                            {
                                                topMenuString += "<li><a href=" + itemTmp2.LocationUrl + ">" + itemTmp2.DisplayName + "</a></li>";
                                            }
                                        }
                                        topMenuString += "</ul></li>";
                                    }
                                    else if (count == 1)
                                    {
                                        // Only one child, add item to 1st level submenu with its data (LocationUrl, DisplayName)
                                        XmlLocation itemTmp3 = AspNetUIFramework.LocationManager.Instance.GetChildLocations(itemTmp.LogicalUniqueName, AspNetUIFramework.CacheManager.Instance.AppLocations)[0];
                                        relPath = itemTmp3.LocationUrl.Contains("~");
                                        if (relPath)
                                        {
                                            topMenuString += "<li><a href=" + ConfigurationManager.AppSettings["AppVirtualPath"] + "/" + itemTmp3.LocationUrl.Substring(2) + ">" + itemTmp3.DisplayName + "</a></li>";
                                        }
                                        else
                                        {
                                            topMenuString += "<li><a href=" + itemTmp3.LocationUrl + ">" + itemTmp3.DisplayName + "</a></li>";
                                        }
                                    }
                                    else
                                    {
                                        // No 2nd level children, add item to 1st level submenu
                                        string cssclass = "";
                                        string cssclass2 = "";
                                        if (itemTmp.LogicalUniqueName == currentLocation.LogicalUniqueName && itemTmp.ParentLocationID==currentLocation.ParentLocationID)
                                        {
                                            cssclass = " class=topMenuItemSelected";
                                            cssclass2 = " class=top_parent_selected";
                                        }
                                        relPath = itemTmp.LocationUrl.Contains("~");
                                        if (relPath)
                                        {
                                            topMenuString += "<li" + cssclass + "><a " + cssclass2+ " href=" + ConfigurationManager.AppSettings["AppVirtualPath"] + "/" + itemTmp.LocationUrl.Substring(2) + ">" + itemTmp.DisplayName + "</a></li>";
                                        }
                                        else {
                                            topMenuString += "<li" + cssclass + "><a " + cssclass2 + " href=" + itemTmp.LocationUrl + ">" + itemTmp.DisplayName + "</a></li>";
                                        
                                        }
                                    }

                                }

                                topMenuString += "</ul></li></ul>";
                          
                            }
                            else if (subMenuParentSections.Count == 1)
                            {
                                // No top level children, add item to top menu
                                topMenuString += "<ul><li><a href=" + ConfigurationManager.AppSettings["AppVirtualPath"] + "/" + subMenuParentSections[0].LocationUrl.Substring(2) + " class=top_parent>" + loc.DisplayName + "</a></li></ul>";
                            }

                        }
                    }
                }
            }

            // End of main DIV tag
            topMenuString += "</div>"; 

            // Add created DIV tag to the table row
            TableCell tc = new TableCell();
            TableRow tr = new TableRow();
            tc.Text = topMenuString;
            tr.Cells.Add(tc);
            tblTopMenu.Rows.Clear();
            tblTopMenu.Rows.Add(tr);

            TopMenuUpdatePanel.Update();
        }

        public void SelectVisibleLink(XmlLocation location)
        {
            if (tblTopMenu.Rows.Count > 0)
            {
                foreach (TableRow row in tblTopMenu.Rows)
                {
                    foreach (Control c in row.Controls)
                    {
                        if (c is TableCell)
                        {
                            (c as TableCell).Text = "Fuck you!";
                        }
                        //Console.WriteLine(c.ToString());
                        //if (c is WebControl)
                        //{
                        //    (c as WebControl).Attributes.CssStyle.Add("color", "red !important");
                        //}
                    }
                }
            }
        }

        public void SelectItem(XmlLocation location)
        {
            // Selecting table cell of location, or it's parent location
            TableCell selectedItem = (TableCell)tblTopMenu.FindControl("topMenuCell_" + location.LogicalUniqueName);

            if (selectedItem == null)
            {
                // Check parent
                selectedItem = (TableCell)tblTopMenu.FindControl("topMenuCell_" + location.ParentLocationID);
            }

            if (selectedItem != null)
            {
                selectedItem.CssClass = "topMenuItemSelected";
            }
        }

        public void UnSelectItem(XmlLocation location)
        {
            // Selecting table cell of location, or it's parent location
            TableCell selectedItem = (TableCell)tblTopMenu.FindControl("topMenuCell_" + location.LogicalUniqueName);

            if (selectedItem != null)
            {
                selectedItem.CssClass = "topMenuItem";
            }
        }
    }
}