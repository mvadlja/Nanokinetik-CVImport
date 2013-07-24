using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using AspNetUIFramework;
using CommonTypes;

namespace AspNetUI.Support
{
    public partial class LeftMenu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void GenerateMenuItems(List<XmlLocation> locations)
        {
            XmlLocation tempLocation;
            TableCell tc;
            TableRow tr;

            tblLeftMenu.Rows.Clear();

            foreach (XmlLocation loc in locations)
            {
                // Bind only active Level2 locations that have parent Level1-LeftMenu
                if (loc.ParentLocationID == "Level1-LeftMenu" && loc.LogicalUniqueName.Contains("Level2") && loc.Active == true)
                {
                    // Right on this location
                    RightTypes right = AspNetUIFramework.LocationManager.Instance.AuthorizeLocation(loc, AspNetUIFramework.CacheManager.Instance.AppLocations);
                    if (right == RightTypes.Restricted) continue;

                    // Always bind url location of first child - top level locations do not point to aspx
                    tempLocation = AspNetUIFramework.LocationManager.Instance.FindFirstAuthorizedActiveChildFileLocationInHierarchy(new List<XmlLocation>() { loc }, locations);

                    if (tempLocation != null)
                    {
                        tr = new TableRow();
                        tc = new TableCell();
                        tc.ID = "leftMenuCell_" + loc.LogicalUniqueName;
                        tc.Text = loc.DisplayName;
                        tc.ToolTip = loc.Description;

                        tc.CssClass = "leftMenuItem";

                        if (tempLocation.LocationTarget == LocationTarget._self)
                        {
                            tc.Attributes["onclick"] = "javascript:document.location='" + tempLocation.LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + "'";
                        }
                        else if (tempLocation.LocationTarget == LocationTarget._blank)
                        {
                            tc.Attributes["onclick"] = "javascript:window.open('" + tempLocation.LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + "', '" + tempLocation.DisplayName + "');";
                        }

                        tr.Cells.Add(tc);
                        tblLeftMenu.Rows.Add(tr);
                    }
                }
            }
        }

        public void SelectItem(XmlLocation location)
        {
            // Selecting table cell of location, or it's parent location
            TableCell selectedItem = (TableCell)tblLeftMenu.FindControl("leftMenuCell_" + location.LogicalUniqueName);

            if (selectedItem == null)
            {
                // Check parent
                selectedItem = (TableCell)tblLeftMenu.FindControl("leftMenuCell_" + location.ParentLocationID);
            }

            if (selectedItem != null)
            {
                selectedItem.CssClass = "leftMenuItemSelected";
            }
        }
    }
}
