using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.Web.UI.HtmlControls;
using AspNetUIFramework;
using CommonTypes;
using Ready.Model;

namespace AspNetUI.Support
{
    public partial class TabMenu : System.Web.UI.UserControl
    {
        IProduct_PKOperations _productPKOperations = new Product_PKDAL();
        IAuthorisedProductOperations _authorizedProductOperations = new AuthorisedProductDAL();
        IActivity_PKOperations _activityOperations = new Activity_PKDAL();
        IProject_PKOperations _projectPKOperations = new Project_PKDAL();
        ISubbmission_unit_PKOperations _submissionUnitOperations = new Subbmission_unit_PKDAL();
        ITask_PKOperations _task_PKOperations = new Task_PKDAL();
        ITime_unit_PKOperations _time_unit_PKOperations = new Time_unit_PKDAL();
        IPharmaceutical_product_PKOperations _pharmaceutical_product_PKOperations = new Pharmaceutical_product_PKDAL();
        IDocument_PKOperations _document_PKOperations = new Document_PKDAL();

        public ControlCollection TabControls
        {
            get { return pnlTabs.Controls; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }


        // Navigation for forms on level 2 (forms that are tied to other forms)
        public void GenerateMenuItemsByRights(List<XmlLocation> locations, XmlLocation currentLocation)
        {
            // Creating tabs for children of input location sieblings (over its parent)
            if (!String.IsNullOrEmpty(currentLocation.ParentLocationID))
            {
                TabControls.Clear();
                // Parent location which children are candidates for tabs
                XmlLocation parentLocation = AspNetUIFramework.LocationManager.Instance.GetLocationByName(currentLocation.ParentLocationID, locations);
                if (parentLocation == null) return;
                // Parent location must be folder
                if (AspNetUIFramework.LocationManager.Instance.IsLocationFolder(parentLocation.LogicalUniqueName, locations))
                {
                    // Candidates for tabs
                    List<XmlLocation> comrades2 = locations.FindAll((XmlLocation loc) => loc.ParentLocationID == parentLocation.LogicalUniqueName);
                    List<XmlLocation> comrades = new List<XmlLocation>();
                    foreach (XmlLocation loc2 in comrades2)
                        if ((bool)loc2.GenerateInTopMenu) comrades.Add(loc2);

                    pnlTabs.Controls.Add(new LiteralControl("<ul>"));
                    // Generating tabs
                    for (int i = 0; i < comrades.Count; i++)
                    {
                        if (comrades[i].Active == true)
                        {
                            // Right on this location
                            RightTypes right = AspNetUIFramework.LocationManager.Instance.AuthorizeLocation(comrades[i], AspNetUIFramework.CacheManager.Instance.AppLocations);
                            if (right == RightTypes.Restricted) continue;

                            pnlTabs.Controls.Add(new LiteralControl("<li>"));
                            HyperLink hl = new HyperLink();
                            hl.ID = "mnuTabItem_" + comrades[i].LogicalUniqueName;
                            hl.Text = comrades[i].DisplayName;

                            hl.NavigateUrl = comrades[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]);

                            pnlTabs.Controls.Add(hl);
                        }
                    }
                    pnlTabs.Controls.Add(new LiteralControl("</ul>"));
                }
            }
        }

        public void GenerateLevel4TabItemsByRights(List<XmlLocation> locations, XmlLocation currentLocation, string Action, string id)
        {
            // Creating tabs for children of input location sieblings (over its parent)
            //if (!String.IsNullOrEmpty(currentLocation.ParentLocationID))
            //{

            XmlLocation parentLocation = AspNetUIFramework.LocationManager.Instance.GetLocationByName(currentLocation.ParentLocationID, AspNetUIFramework.CacheManager.Instance.AppLocations);
            // Parent location which children are candidates for tabs
            //List<XmlLocation> tabMenuChildren = AspNetUIFramework.LocationManager.Instance.GetChildLocations(currentLocation.LogicalUniqueName, AspNetUIFramework.CacheManager.Instance.AppLocations);
            List<XmlLocation> tabMenuChildren = AspNetUIFramework.LocationManager.Instance.GetChildLocations(parentLocation.LogicalUniqueName, AspNetUIFramework.CacheManager.Instance.AppLocations);
            pnlTabs.Controls.Clear();
            // Check if there are any children
            if (tabMenuChildren.Count > 0)
            {
                
                //Prepare to add number of elements to tab name
                DataSet ds = null;
                switch (currentLocation.ParentLocationID)
                {
                    case "Level3-Prod":
                        var idProd = Request.QueryString["idProd"] ?? Request.QueryString["productID"] ?? Request.QueryString["id"];
                        if (ValidationHelper.IsValidInt(idProd)) ds = _productPKOperations.GetTabMenuItemsCount(Int32.Parse(idProd), null);
                        break;
                    case "Level3-AuthProd":
                        var idAuthProd = Request.QueryString["idAuthProd"] ?? Request.QueryString["idAP"] ?? Request.QueryString["id"];
                        if (ValidationHelper.IsValidInt(idAuthProd)) ds = _authorizedProductOperations.GetTabMenuItemsCount(Int32.Parse(idAuthProd), null);
                        break;
                    case "Level3-AuthorisedProductsMGMT":
                        if (ValidationHelper.IsValidInt(Request.QueryString["id"]))
                            ds = _authorizedProductOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["id"]), null);
                        break;
                    case "Level3-ProductsMGMT":
                        if (ValidationHelper.IsValidInt(Request.QueryString["productID"]))
                            ds = _productPKOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["productID"]), null);
                        else if (ValidationHelper.IsValidInt(Request.QueryString["id"]))
                            ds = _productPKOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["id"]), null);
                        break;
                    case "Level3-SubUnit":
                    case "Level3-SubmissionUnit":
                        if (ValidationHelper.IsValidInt(Request.QueryString["suid"]))
                            ds = _submissionUnitOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["suid"]));
                        break;
                    case "Level3-Activities":
                    case "Level3-ActMy":
                    case "Level3-ActAllList":
                        if (ValidationHelper.IsValidInt(Request.QueryString["idAct"]))
                            ds = _activityOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["idAct"]), null);
                        break;
                    case "Level3-Proj":
                        if (ValidationHelper.IsValidInt(Request.QueryString["projid"]))
                            ds = _projectPKOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["projid"]), null);
                        break;
                    case "Level3-Task":    
                    case "Level3-Tasks":
                        if (ValidationHelper.IsValidInt(Request.QueryString["idTask"]))
                            ds = _task_PKOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["idTask"]), null);
                        break;
                    case "Level3-Time":
                        if (ValidationHelper.IsValidInt(Request.QueryString["idtu"]))
                            ds = _time_unit_PKOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["idtu"]));
                        break;
                    case "Level3-TimeAll":
                        if (ValidationHelper.IsValidInt(Request.QueryString["idtu"]))
                            ds = _time_unit_PKOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["idtu"]));
                        break;
                    case "Level3-TimeUnitMyList":
                        if (ValidationHelper.IsValidInt(Request.QueryString["idtu"]))
                            ds = _time_unit_PKOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["idtu"]));
                        break;
                    case "Level3-TimeUnitAllList":
                        if (ValidationHelper.IsValidInt(Request.QueryString["idtu"]))
                            ds = _time_unit_PKOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["idtu"]));
                        break;
                    case "Level3-PharmProd":
                    case "Level3-PharmaceuticalProducts":
                        if (ValidationHelper.IsValidInt(Request.QueryString["idpp"]))
                            ds = _pharmaceutical_product_PKOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["idpp"]));
                        break;
                    case "Level3-DocAllList":
                        if (ValidationHelper.IsValidInt(Request.QueryString["idDoc"]))
                            ds = _document_PKOperations.GetTabMenuItemsCount(Int32.Parse(Request.QueryString["idDoc"]), null);
                        break;
                    default:
                        break;
                }
                List<String> lists = new List<String>();

                pnlTabs.Controls.Add(new LiteralControl("<ul>"));
                // Generating tabs
                for (int i = 0; i < tabMenuChildren.Count; i++)
                {
                    if ((tabMenuChildren[i].Active != null && !(bool)tabMenuChildren[i].Active) ||
                       (tabMenuChildren[i].GenerateInTabMenu != null && !(bool)tabMenuChildren[i].GenerateInTabMenu)) continue;

                    // Right on this location
                    RightTypes right = AspNetUIFramework.LocationManager.Instance.AuthorizeLocation(tabMenuChildren[i], AspNetUIFramework.CacheManager.Instance.AppLocations);
                    if (right == RightTypes.Restricted) continue;
                        
                    //Add number of elements to tab name
                    DataTable dt = ds != null && ds.Tables.Count > 0 ? ds.Tables[0] : null;
                    string number = "";
                    if (dt != null && dt.Rows.Count > 0 && 
                        dt.Columns.Contains(tabMenuChildren[i].LogicalUniqueName) &&
                        dt.Rows[0][tabMenuChildren[i].LogicalUniqueName] != null &&
                        ValidationHelper.IsValidInt(dt.Rows[0][tabMenuChildren[i].LogicalUniqueName].ToString()))
                        number = ": " + dt.Rows[0][tabMenuChildren[i].LogicalUniqueName].ToString() + "";

                    pnlTabs.Controls.Add(new LiteralControl("<li>"));
                    HyperLink hl = new HyperLink();
                    hl.ID = "mnuTabItem_" + tabMenuChildren[i].LogicalUniqueName;

                    hl.Text = tabMenuChildren[i].DisplayName + number;

                    string context = !String.IsNullOrWhiteSpace(Request.QueryString["con"]) ? "&con=" + Request.QueryString["con"] : "";

                    if (tabMenuChildren[i].LocationTarget == LocationTarget._self)
                    {
                        var query = tabMenuChildren[i].LocationUrl.EndsWith(".aspx") ? "?" : "&";
                        if (tabMenuChildren[i].ParentLocationID == "Level3-Prod")
                        {
                            var idProd = Request.QueryString["idProd"] ?? Request.QueryString["productID"] ?? Request.QueryString["id"];
                            
                            if (!string.IsNullOrEmpty(idProd))
                            {
                                if (tabMenuChildren[i].LogicalUniqueName == "Level4-ProdAuthProdList" || tabMenuChildren[i].LogicalUniqueName == "Level4-ProdPharmProdList" || tabMenuChildren[i].LogicalUniqueName == "Level4-ProdActList")
                                {
                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "idProd=" + idProd;
                                }
                                else
                                {
                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "f=l&id=" + idProd;
                                }
                            }
                        }

                        if (tabMenuChildren[i].ParentLocationID == "Level3-PharmProd")
                        {
                            var idPharmProd = Request.QueryString["idPharmProd"] ?? Request.QueryString["idpp"] ?? Request.QueryString["id"];

                            if (!string.IsNullOrEmpty(idPharmProd))
                            {
                                hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "f=l&idpp=" + idPharmProd;
                            }
                        }
                        else if (!string.IsNullOrEmpty(Request.QueryString["productID"]))
                        {
                            if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                            {
                                if (tabMenuChildren[i].ParentLocationID == "Level3-AuthorisedProductsMGMT")
                                {
                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "?Action=" + Action + context + "&id=" + Request.QueryString["id"];
                                }
                                else
                                {
                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "?Action=" + Action + context + "&id=" + Request.QueryString["productID"];
                                }
                            }
                            else
                            {
                                hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "?Action=" + Action + context + "&id=" + Request.QueryString["productID"];
                            }
                        }
                        else if (!String.IsNullOrEmpty(Request.QueryString["projid"]))
                        {
                            if (!String.IsNullOrEmpty(Request.QueryString["idAct"]))
                            {
                                if (!String.IsNullOrEmpty(Request.QueryString["idTask"]))
                                {
                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&projid=" + Request.QueryString["projid"] + "&idAct=" + Request.QueryString["idAct"] + "&idTask=" + Request.QueryString["idTask"];
                                }
                                else if (!String.IsNullOrEmpty(Request.QueryString["suid"]))
                                {
                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&projid=" + Request.QueryString["projid"] + "&idAct=" + Request.QueryString["idAct"] + "&suid=" + Request.QueryString["suid"];
                                }
                                else
                                {
                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&projid=" + Request.QueryString["projid"] + "&idAct=" + Request.QueryString["idAct"];
                                }
                            }
                            else
                            {
                                hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&projid=" + id;
                            }
                        }
                        else if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                        {
                            if (!String.IsNullOrEmpty(Request.QueryString["idAct"]))
                            {
                                if (!String.IsNullOrEmpty(Request.QueryString["idTask"]))
                                {

                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&id=" + Request.QueryString["id"] + "&idAct=" + Request.QueryString["idAct"] + "&idTask=" + Request.QueryString["idTask"];
                                }
                                else if (!String.IsNullOrEmpty(Request.QueryString["suid"]))
                                {
                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&id=" + Request.QueryString["id"] + "&idAct=" + Request.QueryString["idAct"] + "&suid=" + Request.QueryString["suid"];
                                }
                                else
                                {
                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&id=" + Request.QueryString["id"] + "&idAct=" + Request.QueryString["idAct"];
                                }
                            }
                            else if (Request.QueryString["idpp"] != null && Request.QueryString["id"] != null)
                            {
                                hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&id=" + Request.QueryString["id"] + "&idpp=" + Request.QueryString["idpp"];
                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(Request.QueryString["idtu"]))
                                {
                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "?Action=" + Action + context + "&idtu=" + Request.QueryString["idtu"] + "&id=" + Request.QueryString["id"];
                                }
                                else
                                {
                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&id=" + Request.QueryString["id"];
                                }
                                    
                            }
                        }
                        else if (!String.IsNullOrEmpty(Request.QueryString["idAct"]))
                        {
                            if (!String.IsNullOrEmpty(Request.QueryString["idTask"]))
                            {
                                hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&idAct=" + Request.QueryString["idAct"] + "&idTask=" + Request.QueryString["idTask"];
                            }
                                /*
                                else if (!String.IsNullOrEmpty(Request.QueryString["id"]))
                                {
                                    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + "?Action=" + f + context + "&idAct=" + Request.QueryString["idAct"] + "&id=" + Request.QueryString["id"];
                                }*/
                            else if (!String.IsNullOrEmpty(Request.QueryString["suid"]))
                            {
                                hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&idAct=" + Request.QueryString["idAct"] + "&suid=" + Request.QueryString["suid"];
                            }
                            else
                            {
                                hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&idAct=" + Request.QueryString["idAct"];
                            }
                        }

                        else if (!String.IsNullOrEmpty(Request.QueryString["suid"]))
                        {
                            //if (!String.IsNullOrEmpty(Request.QueryString["idTask"]))
                            //{
                            //    if (tabMenuChildren[i].ParentLocationID == "Level3-SubmissionUnit")
                            //        hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + "?f=" + f + context + "&suid=" + Request.QueryString["suid"] + "&idTask=" + Request.QueryString["idTask"];
                            //    else
                            //    hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + "?f=" + f + context + "&idTask=" + Request.QueryString["idTask"];
                            //}
                            //else
                            //{
                            hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&suid=" + Request.QueryString["suid"];
                            //}
                        }
                        else if (!String.IsNullOrEmpty(Request.QueryString["idTask"]))
                        {
                            hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&idTask=" + Request.QueryString["idTask"];
                        }
                        else if (!string.IsNullOrEmpty(Request.QueryString["idtu"]))
                        {
                            hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&idtu=" + Request.QueryString["idtu"];
                        }
                        else if (!string.IsNullOrEmpty(Request.QueryString["idDoc"]))
                        {
                            hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&idDoc=" + Request.QueryString["idDoc"];
                        }
                        else if (!string.IsNullOrEmpty(id))
                        {
                            hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&id=" + id;
                        }
                        else if (Request.QueryString["idpp"] != null && Request.QueryString["id"] != null)
                        {
                            hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&id=" + Request.QueryString["id"] + "&idpp=" + Request.QueryString["idpp"];
                        }
                        else
                        {
                            hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + query + "Action=" + Action + context + "&id=0";
                        }
                    }

                    else if (tabMenuChildren[i].LocationTarget == LocationTarget._blank)
                    {
                        if (!String.IsNullOrEmpty(Request.QueryString["projid"]))
                        {
                            hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + "?Action=" + Action + "&projid=" + id;
                        }
                        else if (id != null && id != "")
                        {
                            hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + "?Action=" + Action + "&id=" + id;
                        }
                        else
                        {
                            hl.NavigateUrl = tabMenuChildren[i].LocationUrl.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]) + "?Action=" + Action;
                        }
                    }

                    string idLay = !string.IsNullOrWhiteSpace(Request["idLay"]) ? "&idLay=" + Request["idLay"] : string.Empty;
                    hl.NavigateUrl += idLay;

                    pnlTabs.Controls.Add(hl);
                    pnlTabs.Controls.Add(new LiteralControl("</li>"));
                }

                pnlTabs.Controls.Add(new LiteralControl("</ul>"));
            }
            //}
        }



        public void SelectItem(XmlLocation location, List<XmlLocation> locations)
        {
            // Selecting table cell of location, or it's parent location
            HyperLink selectedItem = (HyperLink)pnlTabs.FindControl("mnuTabItem_" + location.LogicalUniqueName);

            if (selectedItem != null)
            {
                selectedItem.CssClass = "mnuTabItemSelected";
            }
        }

        // Hides tabs by names in input collection
        public void ModifyVisibleTabList(List<string> tabNames, bool visible)
        {
            List<string> tabs = new List<string>();
            List<Table> allRenderedTabs = GetAllTabsInAllTablesAndRows();

            foreach (string tab in tabNames)
            {
                foreach (Table renderedTab in allRenderedTabs)
                {
                    if (renderedTab.ID == "mnuTab_" + tab)
                    {
                        renderedTab.Visible = visible;
                        break;
                    }
                }
            }
        }

        // Private helper
        private List<Table> GetAllTabsInAllTablesAndRows()
        {
            List<Table> allTabs = new List<Table>();

            foreach (Control c in pnlTabs.Controls)
            {
                if (c is Table)
                {
                    allTabs.Add(c as Table);
                }
            }

            return allTabs;
        }
    }
}