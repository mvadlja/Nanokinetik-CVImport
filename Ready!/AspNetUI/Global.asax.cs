using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using AspNetUI.Support;
using AspNetUI.Views;
using PromptInstantiator;
using System.Configuration;
using Ready.Model;

namespace AspNetUI
{
    public class Global : HttpApplication
    {
        #region Dependency containers

        public static IInstanceContainer OperationalSupportProviders { get; set; }
        public static IInstanceContainer BusinessProviders { get; set; }

        #endregion

        protected void Application_Start(object sender, EventArgs e)
        {
            OperationalSupportProviders = new InstanceContainer().LoadConfiguration("OperationalSupportProviders");
            BusinessProviders = new InstanceContainer().LoadConfiguration("BusinessProviders");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            IPrincipal user = HttpContext.Current.User;
            if (user.Identity.IsAuthenticated && user.Identity.AuthenticationType == "Forms")
            {
                GetUserPermissions(user);

                Location_PK redirectLocation = LocationManager.GetFirstAuthorisedLocation();
               
                if (redirectLocation != null && !string.IsNullOrWhiteSpace(redirectLocation.location_url))
                {
                    FormsAuthentication.SetAuthCookie(user.Identity.Name, false);
                    Response.Redirect(redirectLocation.location_url);
                }
            }
            else
            {
                Session["UserPermissions"] = null;
            }
        }

        private void GetUserPermissions(IPrincipal usr)
        {
            ILocation_PKOperations locationOperations = new Location_PKDAL();

            var userPermissionsList = locationOperations.GetUserPermissions(usr.Identity.Name);

            var userPermissions = new Dictionary<Location_PK, List<Permission>>();

            if (userPermissionsList.Count > 0)
            {
                foreach (Location_PK location in userPermissionsList)
                {
                    Permission permission;
                    if (!Enum.TryParse(location.permission, out permission)) continue;

                    var existingLocation = userPermissions.Keys.FirstOrDefault(item => location.unique_name == item.unique_name);
                    if (existingLocation != null)
                    {
                        List<Permission> permissions;
                        userPermissions.TryGetValue(existingLocation, out permissions);
                        if (permissions != null && !permissions.Contains(permission))
                        {
                            userPermissions.Remove(existingLocation);
                            permissions.Add(permission);
                            userPermissions.Add(existingLocation, permissions);
                        }
                    }
                    else { userPermissions.Add(location, new List<Permission> { permission }); }
                }
            }

            Session["UserPermissions"] = userPermissions;
        }

        // CALLED FOR EVERY RESOURCE, not only .aspx files!!!
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Path.EndsWith("Reserved.ReportViewerWebControl.axd") &&
                Request.QueryString["ResourceStreamID"] != null &&
                Request.QueryString["ResourceStreamID"].ToLower().Contains("blank.gif"))
            {
                Response.Redirect(ConfigurationManager.AppSettings["BlankImageRedirectLink"]);
            }

            // ONLY .ASPX REQUEST!!!
            if (HttpContext.Current.Request.Url.AbsolutePath.Contains(".aspx"))
            {
                //Redirect old application requests to new ones
                RedirectOldApplicationRequests();

                LocationManager.Instance.RetreiveApplicationLocationsFromDb();
            }
        }


        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {
            Session["userPermissions"] = null;
        }

        protected void Application_End(object sender, EventArgs e)
        {

        }

        void RedirectOldApplicationRequests()
        {
            string path = Request.Url.LocalPath;
            if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AppVirtualPath"]))
            {
                path = path.Replace(ConfigurationManager.AppSettings["AppVirtualPath"], "");
            }
            path = "~" + path;

            if (!path.StartsWith("~/Views/Business/")) return;

            var queryString = HttpContext.Current.Request.QueryString;
            string query = null;

            switch (path)
            {
                case "~/Views/Business/APPropertiesView.aspx":
                    query = string.Format("&idAuthProd={0}", queryString["id"]);
                    Response.Redirect(string.Format("~/Views/AuthorisedProductView/Preview.aspx?EntityContext=AuthorisedProduct{0}", query));
                    break;

                case "~/Views/Business/ProductPKPPropertiesView.aspx":
                    query = string.Format("&idProd={0}", queryString["id"]);
                    Response.Redirect(string.Format("~/Views/ProductView/Preview.aspx?EntityContext=Product{0}", query));
                    break;

                case "~/Views/Business/PharmaceuticalProductProperties.aspx":
                    query = string.Format("&idPharmProd={0}", queryString["idpp"]);
                    Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/Preview.aspx?EntityContext=PharmaceuticalProduct{0}", query));
                    break;

                case "~/Views/Business/TasksProperties.aspx":
                    query = string.Format("&idTask={0}", queryString["idTask"]);
                    Response.Redirect(string.Format("~/Views/TaskView/Preview.aspx?EntityContext=Task{0}", query));
                    break;

                case "~/Views/Business/ProjectProperties.aspx":
                    query = string.Format("&idProj={0}", queryString["projid"]);
                    Response.Redirect(string.Format("~/Views/ProjectView/Preview.aspx?EntityContext=Project{0}", query));
                    break;

                case "~/Views/Business/APropertiesView.aspx":
                    query = string.Format("&idAct={0}", queryString["idAct"]);
                    Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext=Activity{0}", query));
                    break;

                case "~/Views/Business/PDocumentsView.aspx":
                    query = string.Format("&idProd={0}&idDoc={1}", queryString["id"], queryString["idDoc"]);
                    Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext=Product{0}", query));
                    break;

                case "~/Views/Business/APDocumentsView.aspx":
                    query = string.Format("&idAuthProd={0}&idDoc={1}", queryString["id"], queryString["idDoc"]);
                    Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext=AuthorisedProduct{0}", query));
                    break;

                case "~/Views/Business/ADocumentsView.aspx":
                    query = string.Format("&idAct={0}&idDoc={1}", queryString["idAct"], queryString["idDoc"]);
                    Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext=Activity{0}", query));
                    break;

                case "~/Views/Business/TDocumentsView.aspx":
                    query = string.Format("&idTask={0}&idDoc={1}", queryString["idTask"], queryString["idDoc"]);
                    Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext=Task{0}", query));
                    break;

                case "~/Views/Business/PPDocumentsView.aspx":
                    query = string.Format("&idPharmProd={0}&idDoc={1}", queryString["idpp"], queryString["idDoc"]);
                    Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext=PharmaceuticalProduct{0}", query));
                    break;

                case "~/Views/Business/ProjectDocumentsView.aspx":
                    query = string.Format("&idProj={0}&idDoc={1}", queryString["projid"], queryString["idDoc"]);
                    Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext=Project{0}", query));
                    break;

                case "~/Views/Business/DocumentsView.aspx":
                    query = string.Format("&idDoc={0}", queryString["idDoc"]);
                    Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext=Document{0}", query));
                    break;

            }
        }
    }
}