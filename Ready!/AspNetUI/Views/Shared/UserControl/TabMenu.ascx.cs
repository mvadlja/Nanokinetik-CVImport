using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using AspNetUI.Support;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class TabMenu : System.Web.UI.UserControl
    {
        public ControlCollection TabControls
        {
            get { return pnlTabs.Controls; }
        }

        public Location_PK RefererLocation
        {
            get { return ViewState["refererLocation"] != null ? (Location_PK)ViewState["refererLocation"] : null; }
            set { ViewState["refererLocation"] = value; }
        }

        public Permission OneTimePermissionTokenSessionTabMenu
        {
            get
            {
                if (Session["OneTimePermissionTokenSessionTabMenu"] != null)
                {
                    var oneTimePermissionTokenSessionTabMenu = (Permission)Session["OneTimePermissionTokenSessionTabMenu"];
                    return oneTimePermissionTokenSessionTabMenu;
                }
                return Permission.None;
            }
            set { Session["OneTimePermissionTokenSessionTabMenu"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            var masterPage = (Template.Default)Page.Master;
            if (masterPage != null)
            {
                if (masterPage.OneTimePermissionToken != Permission.None) OneTimePermissionTokenSessionTabMenu = masterPage.OneTimePermissionToken;

                if (!IsPostBack)
                {
                    RefererLocation = Support.LocationManager.GetRefererLocation();
                }
            }
        }

        public void GenerateMenuItemsByRights(List<Location_PK> locations, Location_PK currentLocation)
        {
            if (currentLocation == null || String.IsNullOrEmpty(currentLocation.parent_unique_name)) return;

            TabControls.Clear();
            var parentLocation = Support.LocationManager.Instance.GetLocationByName(currentLocation.parent_unique_name, locations);
            if (parentLocation == null) return;

            if (Support.LocationManager.Instance.IsLocationFolder(parentLocation.unique_name, locations))
            {
                var comrades = locations.FindAll(loc => loc.parent_unique_name == parentLocation.unique_name && loc.generate_in_top_menu != null && (bool)loc.generate_in_top_menu).OrderBy(loc => loc.menu_order).ToList();

                pnlTabs.Controls.Add(new LiteralControl("<ul>"));

                foreach (var comrade in comrades)
                {
                    if (comrade.active != true) continue;

                    var masterPage = (Template.Default)Page.Master;
                    if (masterPage == null) continue;

                    var hl = new HyperLink();

                    if (masterPage.OneTimePermissionToken != Permission.None) OneTimePermissionTokenSessionTabMenu = masterPage.OneTimePermissionToken;

                    if ((comrade.unique_name == currentLocation.unique_name && OneTimePermissionTokenSessionTabMenu == Permission.View))
                    {
                        pnlTabs.Controls.Add(new LiteralControl("<li>"));
                        hl = new HyperLink
                        {
                            ID = string.Format("mnuTabItem_{0}", comrade.unique_name),
                            Text = comrade.display_name,
                            NavigateUrl = comrade.location_url.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"])
                        };

                        pnlTabs.Controls.Add(hl);
                        continue;
                    }

                    if (SecurityHelper.IsPermitted(Permission.View, comrade))
                    {
                        pnlTabs.Controls.Add(new LiteralControl("<li>"));
                        hl = new HyperLink
                        {
                            ID = string.Format("mnuTabItem_{0}", comrade.unique_name),
                            Text = comrade.display_name,
                            NavigateUrl = comrade.location_url.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"])
                        };

                        pnlTabs.Controls.Add(hl);
                        continue;
                    }

                    if (currentLocation.unique_name != comrade.unique_name ||
                        (RefererLocation == null || !SecurityHelper.IsPermitted(Permission.View, RefererLocation)))
                    {
                        continue;
                    }

                    pnlTabs.Controls.Add(new LiteralControl("<li>"));
                    hl = new HyperLink
                    {
                        ID = string.Format("mnuTabItem_{0}", comrade.unique_name),
                        Text = comrade.display_name,
                        NavigateUrl = comrade.location_url.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"])
                    };

                    pnlTabs.Controls.Add(hl);
                }
                pnlTabs.Controls.Add(new LiteralControl("</ul>"));
            }
        }

        public void GenerateTabMenuItems(List<Location_PK> locations, Location_PK currentLocation, List<string> locationNamesToGenerate = null)
        {
            var parentLocation = Support.LocationManager.Instance.GetLocationByName(currentLocation.parent_unique_name, Support.CacheManager.Instance.AppLocations);

            var tabMenuItems = Support.LocationManager.Instance.GetChildLocations(parentLocation.unique_name, Support.CacheManager.Instance.AppLocations).OrderBy(loc => loc.menu_order);

            pnlTabs.Controls.Clear();

            if (!tabMenuItems.Any()) return;

            DataSet dsTabMenuItemCount = GetTabMenuItemsCount(currentLocation);
            DataTable dtTabMenuItemCount = dsTabMenuItemCount != null && dsTabMenuItemCount.Tables.Count > 0 ? dsTabMenuItemCount.Tables[0] : null;

            pnlTabs.Controls.Add(new LiteralControl("<ul>"));

            foreach (var tabMenuItem in tabMenuItems)
            {
                if (locationNamesToGenerate == null)
                {
                    if ((tabMenuItem.active != null && !(bool)tabMenuItem.active) ||
                        (tabMenuItem.generate_in_tab_menu != null && !(bool)tabMenuItem.generate_in_tab_menu))
                    {
                        if (RefererLocation == null)
                        {
                            if (tabMenuItem.show_location == null || !tabMenuItem.show_location.Value || tabMenuItem.unique_name != currentLocation.unique_name) continue;
                        }
                        else continue;
                    }
                }
                else
                {
                    if (!locationNamesToGenerate.Contains(tabMenuItem.unique_name)) continue;
                }

                var masterPage = (Template.Default)Page.Master;
                if (masterPage != null && masterPage.OneTimePermissionToken != Permission.None) OneTimePermissionTokenSessionTabMenu = masterPage.OneTimePermissionToken;

                HyperLink hlTabMenuItem;
                string tabMenuNumber;
                if (OneTimePermissionTokenSessionTabMenu == Permission.ViewComradeTab || // case for explicitely permitting comrades tab view
                    (tabMenuItem.unique_name == currentLocation.unique_name && OneTimePermissionTokenSessionTabMenu == Permission.View) || // case for explicitely permistting (only) view after referer page is not "view" permitted. i.e. saving entity on page that does not have view permissions
                    (tabMenuItem.unique_name == currentLocation.unique_name && (RefererLocation != null && SecurityHelper.IsPermitted(Permission.View, RefererLocation)))) // case for view permission delegation from referer page
                {
                    OneTimePermissionTokenSessionTabMenu = Permission.View;
                    //Add number of elements to tab name
                    tabMenuNumber = "";
                    if (dtTabMenuItemCount != null && dtTabMenuItemCount.Rows.Count > 0 &&
                        dtTabMenuItemCount.Columns.Contains(tabMenuItem.unique_name) &&
                        dtTabMenuItemCount.Rows[0][tabMenuItem.unique_name] != null &&
                        ValidationHelper.IsValidInt(dtTabMenuItemCount.Rows[0][tabMenuItem.unique_name].ToString()))
                    {
                        tabMenuNumber = ": " + dtTabMenuItemCount.Rows[0][tabMenuItem.unique_name];
                    }

                    pnlTabs.Controls.Add(new LiteralControl("<li>"));
                    hlTabMenuItem = new HyperLink
                    {
                        ID = string.Format("mnuTabItem_{0}", tabMenuItem.unique_name),
                        Text = tabMenuItem.display_name + tabMenuNumber
                    };

                    if (tabMenuItem.location_target == LocationTarget._self.ToString()) GenerateTabMenuLink(tabMenuItem, hlTabMenuItem);

                    pnlTabs.Controls.Add(hlTabMenuItem);
                    pnlTabs.Controls.Add(new LiteralControl("</li>"));
                    continue;
                }

                if (!SecurityHelper.IsPermitted(Permission.View, tabMenuItem))
                {
                    continue;
                }

                tabMenuNumber = "";
                if (dtTabMenuItemCount != null && dtTabMenuItemCount.Rows.Count > 0 &&
                    dtTabMenuItemCount.Columns.Contains(tabMenuItem.unique_name) &&
                    dtTabMenuItemCount.Rows[0][tabMenuItem.unique_name] != null &&
                    ValidationHelper.IsValidInt(dtTabMenuItemCount.Rows[0][tabMenuItem.unique_name].ToString()))
                {
                    tabMenuNumber = ": " + dtTabMenuItemCount.Rows[0][tabMenuItem.unique_name];
                }

                pnlTabs.Controls.Add(new LiteralControl("<li>"));
                hlTabMenuItem = new HyperLink
                            {
                                ID = string.Format("mnuTabItem_{0}", tabMenuItem.unique_name),
                                Text = tabMenuItem.display_name + tabMenuNumber
                            };

                if (tabMenuItem.location_target == LocationTarget._self.ToString()) GenerateTabMenuLink(tabMenuItem, hlTabMenuItem);

                pnlTabs.Controls.Add(hlTabMenuItem);
                pnlTabs.Controls.Add(new LiteralControl("</li>"));
            }
            pnlTabs.Controls.Add(new LiteralControl("</ul>"));
        }

        private void GenerateTabMenuLink(Location_PK tabMenuItem, HyperLink hlTabMenuItem)
        {
            if (tabMenuItem.parent_unique_name == "Prod")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idProd", Request.QueryString["idProd"],
                    new[] { "ProdPreview", "ProdDocList", "ProdDocPreview", "ProdAuthProdList", "ProdPharmProdList", "ProdActList", "ProdSubUnitList", "ProdXevprmList", "ProdAuditTrailList", "ProdAlertList" });
            }
            else if (tabMenuItem.parent_unique_name == "AuthProd")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idAuthProd", Request.QueryString["idAuthProd"],
                    new[] { "AuthProdPreview", "AuthProdDocList", "AuthProdDocPreview", "AuthProdXevprmList", "AuthProdAuditTrailList", "AuthProdAlertList" });
            }
            else if (tabMenuItem.parent_unique_name == "PharmProd")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idPharmProd", Request.QueryString["idPharmProd"],
                    new[] { "PharmProdPreview", "PharmProdDocList", "PharmProdDocPreview", "AuthProdXevprmList", "PharmProdAuditTrailList" });
            }
            else if (tabMenuItem.parent_unique_name == "ActMy")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idAct", Request.QueryString["idAct"],
                    new[] { "ActMyPreview", "ActMyDocList", "ActMyDocPreview", "ActMyTaskList", "ActMyTimeUnitList", "ActMySubUnitList", "ActMyAuditTrailList", "ActMyAlertList" });
            }
            else if (tabMenuItem.parent_unique_name == "Act")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idAct", Request.QueryString["idAct"],
                    new[] { "ActPreview", "ActDocList", "ActDocPreview", "ActTaskList", "ActTimeUnitList", "ActSubUnitList", "ActAuditTrailList", "ActAlertList" });
            }
            else if (tabMenuItem.parent_unique_name == "TimeUnitMy")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idTimeUnit", Request.QueryString["idTimeUnit"],
                    new[] { "TimeUnitMyPreview", "TimeUnitMyActPreview", "TimeUnitMyActFormEdit", "TimeUnitMyActFormSaveAs", "TimeUnitMyProdList" });
            }
            else if (tabMenuItem.parent_unique_name == "TimeUnit")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idTimeUnit", Request.QueryString["idTimeUnit"],
                    new[] { "TimeUnitPreview", "TimeUnitActPreview", "TimeUnitActFormEdit", "TimeUnitActFormSaveAs", "TimeUnitProdList" });
            }
            else if (tabMenuItem.parent_unique_name == "Proj")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idProj", Request.QueryString["idProj"],
                    new[] { "ProjPreview", "ProjDocList", "ProjDocPreview", "ProjActList", "ProjAlertList" });
            }
            else if (tabMenuItem.parent_unique_name == "Task")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idTask", Request.QueryString["idTask"],
                    new[] { "TaskPreview", "TaskDocList", "TaskDocPreview", "TaskSubUnitList", "TaskAlertList" });
            }
            else if (tabMenuItem.parent_unique_name == "Person")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idPerson", Request.QueryString["idPerson"],
                    new[] { "PersonFormNew", "PersonFormEdit", "UserSecurityFormEdit" });
            }
            else if (tabMenuItem.parent_unique_name == "SubUnit")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idSubUnit", Request.QueryString["idSubUnit"],
                    new[] { "SubUnitPreview", "SubUnitActPreview", "SubUnitActFormEdit", "SubUnitActFormSaveAs", "SubUnitTaskPreview", "SubUnitTaskFormEdit", "SubUnitTaskFormSaveAs" });
            }
            else if (tabMenuItem.parent_unique_name == "Doc")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idDoc", Request.QueryString["idDoc"],
                    new[] { "DocPreview", "DocAuditTrailList", "DocAlertList" });
            }
            else if (tabMenuItem.parent_unique_name == "Account")
            {
                GenerateTabMenuLink(hlTabMenuItem, tabMenuItem, "idAlert", Request.QueryString["idAlert"],
                    new[] { "ReminderFormEdit", "AlertAuditTrailList" });
            }
        }

        private void GenerateTabMenuLink(HyperLink hlTabMenuItem, Location_PK tabMenuItem, string idEntityQueryKey, string idEntityQueryValue, params string[] tabsUniqueName)
        {
            var queryStart = tabMenuItem.location_url.EndsWith(".aspx") ? "?" : "&";
            if (tabsUniqueName.Contains(tabMenuItem.unique_name))
            {
                if (!string.IsNullOrWhiteSpace(idEntityQueryValue))
                {
                    hlTabMenuItem.NavigateUrl = string.Format("{0}{1}{2}={3}", tabMenuItem.location_url.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]), queryStart, idEntityQueryKey, idEntityQueryValue);
                }
                else
                {
                    hlTabMenuItem.NavigateUrl = string.Format("{0}", tabMenuItem.location_url.Replace("~", ConfigurationManager.AppSettings["AppVirtualPath"]));
                }
            }
        }

        private DataSet GetTabMenuItemsCount(Location_PK currentLocation)
        {
            IPerson_PKOperations personOperations = new Person_PKDAL();
            var person = personOperations.GetPersonByUserID(SessionManager.Instance.CurrentUser.UserID);
            var personFk = person != null ? person.person_PK : null;
            DataSet ds = null;
            switch (currentLocation.parent_unique_name)
            {
                case "Prod":
                    var idProd = Request.QueryString["idProd"];
                    var productOperations = new Product_PKDAL();
                    if (ValidationHelper.IsValidInt(idProd)) ds = productOperations.GetTabMenuItemsCount(Int32.Parse(idProd), personFk);
                    break;
                case "AuthProd":
                    var idAuthProd = Request.QueryString["idAuthProd"];
                    var authorisedProductOperations = new AuthorisedProductDAL();
                    if (ValidationHelper.IsValidInt(idAuthProd)) ds = authorisedProductOperations.GetTabMenuItemsCount(Int32.Parse(idAuthProd), personFk);
                    break;
                case "PharmProd":
                    var idPharmProd = Request.QueryString["idPharmProd"];
                    var pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
                    if (ValidationHelper.IsValidInt(idPharmProd)) ds = pharmaceuticalProductOperations.GetTabMenuItemsCount(Int32.Parse(idPharmProd));
                    break;
                case "TimeUnitMy":
                case "TimeUnit":
                    var idTimeUnit = Request.QueryString["idTimeUnit"];
                    var timeUnitOperations = new Time_unit_PKDAL();
                    if (ValidationHelper.IsValidInt(idTimeUnit)) ds = timeUnitOperations.GetTabMenuItemsCount(Int32.Parse(idTimeUnit));
                    break;
                case "ActMy":
                case "Act":
                    var idAct = Request.QueryString["idAct"];
                    var activityOperations = new Activity_PKDAL();
                    if (ValidationHelper.IsValidInt(idAct)) ds = activityOperations.GetTabMenuItemsCount(Int32.Parse(idAct), personFk);
                    break;
                case "Proj":
                    var idProj = Request.QueryString["idProj"];
                    var projectOperations = new Project_PKDAL();
                    if (ValidationHelper.IsValidInt(idProj)) ds = projectOperations.GetTabMenuItemsCount(Int32.Parse(idProj), personFk);
                    break;
                case "Task":
                    var idTask = Request.QueryString["idTask"];
                    var taskOperations = new Task_PKDAL();
                    if (ValidationHelper.IsValidInt(idTask)) ds = taskOperations.GetTabMenuItemsCount(Int32.Parse(idTask), personFk);
                    break;
                case "SubUnit":
                    var idSubUnit = Request.QueryString["idSubUnit"];
                    var submissionUnitOperations = new Subbmission_unit_PKDAL();
                    if (ValidationHelper.IsValidInt(idSubUnit)) ds = submissionUnitOperations.GetTabMenuItemsCount(Int32.Parse(idSubUnit));
                    break;
                case "Doc":
                    var idDoc = Request.QueryString["idDoc"];
                    var documentOperations = new Document_PKDAL();
                    if (ValidationHelper.IsValidInt(idDoc)) ds = documentOperations.GetTabMenuItemsCount(Int32.Parse(idDoc), personFk);
                    break;
                case "Account":
                    var idAlert = Request.QueryString["idAlert"];
                    var reminderOperations = new Reminder_PKDAL();
                    if (ValidationHelper.IsValidInt(idAlert)) ds = reminderOperations.GetTabMenuItemsCount(Int32.Parse(idAlert));
                    break;
            }
            return ds;
        }

        public void SelectItem(Location_PK location, List<Location_PK> locations)
        {
            if (pnlTabs == null || location == null) return;

            var selectedItem = (HyperLink)pnlTabs.FindControl("mnuTabItem_" + location.unique_name);
            if (selectedItem != null) selectedItem.CssClass = "mnuTabItemSelected";
        }
    }
}