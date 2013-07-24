using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Threading;
using System.Web.Security;
using System.Drawing;
using System.Web.UI;
using AspNetUI.Support;
using AspNetUIFramework;
using CommonTypes;
using Ready.Model;
using Kmis.Model;
using AspNetUI.Views.Shared.UserControl;
using LocationManager = AspNetUI.Support.LocationManager;
using TabMenu = AspNetUI.Views.Shared.UserControl.TabMenu;
using TopMenu = AspNetUI.Views.Shared.UserControl.TopMenu;

namespace AspNetUI.Views.Shared.Template
{
    public partial class Default : MasterPage
    {
        List<Location_PK> _topMenuLocations = new List<Location_PK>();
        RightTypes _rightTypeOfCurrentUserForThisPage = RightTypes.Default;

        // Operation managers
        private IUSEROperations _userOperations;
        private IUSER_ROLEOperations _roleOperations;
        private ILoggedExceptionOperations _loggedExceptionOperations;
        private IReminder_PKOperations _reminderOperations;
        private IPerson_PKOperations _personOperations;

        public Default()
        {
            CurrentLocation = null;
        }

        #region Properties

        public List<Location_PK> TopMenuLocations
        {
            get { return _topMenuLocations; }
            set { _topMenuLocations = value; }
        }

        public TabMenu TabMenu
        {
            get { return tabMenu; }
        }

        public TopMenu TopMenu
        {
            get { return topMenu; }
        }

        public ModalPopup ModalPopup
        {
            get { return modalPopup; }
        }

        public UpdatePanel UpTopMenu
        {
            get { return upTopMenu; }
        }

        public UpdatePanel UpCommon
        {
            get { return upCommon; }
        }

        public UpdatePanel UpUserSettings
        {
            get { return upUserSettings; }
        }

        public UpdatePanel UpModalPopups
        {
            get { return upModalPopups; }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadEntityContext();

            if (!Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                FormsAuthentication.RedirectToLoginPage();
            }

            if (!Page.ClientScript.IsClientScriptBlockRegistered(this.GetType(), "AppVirtualPath"))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "AppVirtualPath", "window.AppVirtualPath=\"" + ConfigurationManager.AppSettings["AppVirtualPath"] + "\";", true);
            }
			
            // Check only on first entry
            if (!IsPostBack)
            {
                RefererLocation = LocationManager.RetrieveMasterLocation(EntityContext) ?? LocationManager.GetRefererLocation();

                if (EntityContext == EntityContext.Person || EntityContext == EntityContext.Organisation) RefererLocation = LocationManager.GetRefererLocation();

                if (OneTimePermissionToken != Permission.View)
                {
                    if (!SecurityHelper.IsPermitted(Permission.View))
                    {
                        if (RefererLocation == null)
                        {
                            SecurityHelper.RedirectToRestrictedAreaErrorPage();
                        }
                        else
                        {
                            if (!SecurityHelper.IsPermitted(Permission.View, RefererLocation))
                            {
                                SecurityHelper.RedirectToRestrictedAreaErrorPage();
                             }
                        }
                    }
                }
            }

            try
            {
                CurrentLocation = LocationManager.Instance.ParseLocationFromUrl(Request.ExtractCurrentQuery(new List<string> { "EntityContext", "Action" }), Support.CacheManager.Instance.AppLocations);

                // If current location is not active, redirect to default
                if (CurrentLocation == null || (CurrentLocation != null && CurrentLocation.active == false))
                {
                    OneTimePermissionToken = Permission.View;
                    SecurityHelper.RedirectToRestrictedAreaErrorPage();  // This will throw ThreadAbortException
                }

                var activity = LocationManager.Instance.GetLocationByName("Act", Support.CacheManager.Instance.AppLocations);
                if (SecurityHelper.IsPermitted(Permission.View, activity))
                {
                    lnkLayoutHeader.NavigateUrl = "~/Views/ActivityView/List.aspx?EntityContext=Activity";
                    lnkLayoutHeaderLeft.HRef = "~/Views/ActivityView/List.aspx?EntityContext=Activity";
                    lnkLayoutHeader.NavigateUrl = "~/Views/ActivityView/List.aspx?EntityContext=Activity";
                }

                _userOperations = new USERDAL();
                _roleOperations = new USER_ROLEDAL();
                _loggedExceptionOperations = new LoggedExceptionDAL();
                _reminderOperations = new Reminder_PKDAL();
                _personOperations = new Person_PKDAL();


                AppUser user = CurrentUser;

                Person_PK person = _personOperations.GetPersonByUserID(user.UserID);
                if (person != null)
                {
                    lblLoginName.Text = person.FullName;
                    lblLoginName.NavigateUrl = string.Format("~/Views/Account/Form.aspx?EntityContext={0}", EntityContext.UserAccount);
                    lbtLogOut.Visible = true;
                }
                else
                {
                    lblLoginName.Text = CurrentUser == null ? String.Empty : CurrentUser.Username;
                    lblLoginName.NavigateUrl = string.Format("~/Views/Account/Form.aspx?EntityContext={0}", EntityContext.UserAccount);
                    lbtLogOut.Visible = true;
                }

                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ApplicationVersion"]))
                {
                    lblAppVersion.Text = string.Format("v{0} ", ConfigurationManager.AppSettings["ApplicationVersion"]);
                }
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException)) HandleClassicException(ex);
            }
        }

        public Location_PK FindTopLevelParent(Location_PK currentLocation)
        {
            List<Location_PK> locations = Support.CacheManager.Instance.AppLocations;
            do
            {
                currentLocation = locations.Find(loc => loc.unique_name == currentLocation.parent_unique_name);
            }
            while (currentLocation != null && currentLocation.navigation_level != 2);
            return currentLocation;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {
                Location_PK topLevelParent = FindTopLevelParent(CurrentLocation);
                topMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, CurrentLocation);
                topMenu.SelectItem(CurrentLocation);

                tabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, CurrentLocation);
                tabMenu.SelectItem(CurrentLocation, Support.CacheManager.Instance.AppLocations);

                SetReminderTextForUser();
                var rootLocation = LocationManager.Instance.GetLocationByName("Root", Support.CacheManager.Instance.AppLocations);
                if (rootLocation != null)
                {
                    if (SecurityHelper.IsPermitted(Permission.QuickLinkShowBar, rootLocation))
                    {
                        string path = Request.Url.LocalPath;
                        if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AppVirtualPath"]))
                        {
                            path = path.Replace(ConfigurationManager.AppSettings["AppVirtualPath"], "");
                        }
                        path = "~" + path;

                        int? idSearch = ValidationHelper.IsValidInt(Request.QueryString["idSearch"]) ? int.Parse(Request.QueryString["idSearch"]) : (int?)null;
                        string idLay = Request.QueryString["idLay"];

                        var selectedQuickLink = new Tuple<string, int?, string>(path, idSearch, idLay);

                        var quickLinksControl = QuickLinks.GenerateQuickLinks(SessionManager.Instance.CurrentUser.UserID, selectedQuickLink);

                        divQuickLinksContainer.Controls.Add(quickLinksControl);
                    }
                    else
                    {
                        divQuickLinksContainer.Visible = false;
                        divQuickLinksButtonOpen.Visible = false;
                    }
                }
                else
                {
                    divQuickLinksContainer.Visible = false;
                    divQuickLinksButtonOpen.Visible = false;
                }

                if (Session["MeasuresTime"] != null) lblLoginName.Text = Convert.ToString(Session["MeasuresTime"]);
            }
            catch (Exception ex)
            {
                HandleClassicException(ex);
            }
        }

        public void SetReminderTextForUser()
        {
            if (ConfigurationManager.AppSettings["RemindersEnabled"] != null &&
                ConfigurationManager.AppSettings["RemindersEnabled"].ToLower().Trim() == "true")
            {
                lnkReminders.Visible = true;
                lblRemindersSeparator.Visible = true;
            }
            else
            {
                lnkReminders.Visible = false;
                lblRemindersSeparator.Visible = false;
            }

            DataSet dataSet = _reminderOperations.GetActiveRemindersForUser(CurrentUser.UserID.HasValue ? CurrentUser.UserID.Value : 0);

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows != null &&
                dataSet.Tables[0].Rows.Count > 0 &&
                dataSet.Tables[0].Columns.Contains("NumReminders") && dataSet.Tables[0].Columns.Contains("OverDue"))
            {
                DataRow row = dataSet.Tables[0].Rows[0];
                int numReminders = row["NumReminders"] != null ? (int)row["NumReminders"] : 0;
                bool overDue = row["OverDue"] != null && row["OverDue"].ToString().Trim().ToLower() == "true";

                if (numReminders > 0)
                {
                    lnkReminders.Text = string.Format("You have {0} alert/s!", numReminders);
                    lnkReminders.ForeColor = overDue ? Color.Red : Color.Black;
                    lnkReminders.NavigateUrl = string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}", EntityContext.Alerter);
                }
                else
                {
                    lnkReminders.Text = "You don't have any alerts";
                    lnkReminders.ForeColor = Color.Black;
                    lnkReminders.NavigateUrl = string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}", EntityContext.Alerter);
                }
            }
            else
            {
                lnkReminders.Text = "";
                lnkReminders.ForeColor = Color.Black;
                lnkReminders.NavigateUrl = string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}", EntityContext.Alerter);
            } 
            
            upUserSettings.Update();
        }

        // Post load (after page / uc controls load) handler
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if(!IsPostBack) OneTimePermissionToken = Permission.None;
        }

        // Logout
        protected void lbtLogOut_Click(object sender, EventArgs e)
        {
            SecurityHelper.RedirectToLoginPage();
        }

        // About
        protected void lbtAbout_Click(object sender, EventArgs e)
        {
            aboutModalPopup.ShowModalPopup("", "");
        }

        public EntityContext EntityContext
        {
            get { return Session["EntityContext"] != null ? (EntityContext)Session["EntityContext"] : EntityContext.Unknown; }
            set { Session["EntityContext"] = value; }
        }

        public Permission OneTimePermissionToken
        {
            get
            {
                if (Session["OneTimePermissionToken"] != null)
                {
                    var oneTimePermissionToken = (Permission)Session["OneTimePermissionToken"];
                   
                    return oneTimePermissionToken;
                }
                return Permission.None;
            }
            set { Session["OneTimePermissionToken"] = value; }
        }

        private void LoadEntityContext()
        {
            EntityContext parsedEntityContext;
            Enum.TryParse(Request.QueryString["EntityContext"], out parsedEntityContext);
            EntityContext = parsedEntityContext;
        }

        #region Error Handleing

        protected override void OnError(EventArgs e)
        {
            base.OnError(e);

            Exception ex = Server.GetLastError();
            HandleClassicException(ex);
        }

        #endregion

        #region IMasterPageOperationalSupport Members

        public Location_PK CurrentLocation { get; set; }
        public Location_PK RefererLocation
        {
            get { return Session["RefererLocation"] != null ? (Location_PK)Session["RefererLocation"] : null; }
            set { Session["RefererLocation"] = value; }
        }

        public USER ResponsibleUser
        {
            get { return _userOperations.GetUserByUsername(SessionManager.Instance.CurrentUser.Username); }
        }

        public AppUser CurrentUser
        {
            get
            {
                if (_userOperations == null)
                {
                    _userOperations = new USERDAL();
                    _roleOperations = new USER_ROLEDAL();
                }

                if (SessionManager.Instance.CurrentUser == null)
                {
                    var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);

                    if (user != null)
                    {
                        var roles = _roleOperations.GetActiveRolesByUsername(Thread.CurrentPrincipal.Identity.Name);
                        var rolesArr = new string[roles.Count];

                        for (int i = 0; i < roles.Count; i++)
                        {
                            rolesArr[i] = roles[i].Name;
                        }

                        // Putting user in session so that it can be easily retreived from other components
                        SessionManager.Instance.CurrentUser = new AppUser(user.User_PK, user.Username, false, rolesArr);
                    }
                    else
                    {
                        // active directory user
                        var myName = Thread.CurrentPrincipal.Identity.Name;
                        if (myName != null)
                        {
                            var rolesArr = new string[1];
                            rolesArr[0] = "Office";
                            SessionManager.Instance.CurrentUser = new AppUser(-1, myName, true, rolesArr);
                        }
                    }
                }

                return SessionManager.Instance.CurrentUser;
            }
        }

        public RightTypes RightTypeOfCurrentUserForThisPage
        {
            get { return _rightTypeOfCurrentUserForThisPage; }
            set { _rightTypeOfCurrentUserForThisPage = value; }
        }

        public IContextMenu ContextMenu
        {
            get { return contextMenu; }
        }

        public void HandleClassicException(Exception ex)
        {
            string faultCode = String.Empty;

            if (ex == null) return;

            Server.ClearError();

            // All exceptions
            try
            {
                faultCode = Guid.NewGuid().ToString();
                string mess = ex.Message;

                // Logging exception
                var loggedException = new LoggedException(null, CurrentUser.Username, ex.GetType().Name, ex.Message, ex.TargetSite.Name, ex.StackTrace, ex.Source, DateTime.Now, Environment.MachineName, new Guid(faultCode));
                if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                {
                    loggedException.ExceptionMessage += string.Format(" | InnerException: {0}", ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.InnerException.Message))
                    {
                        loggedException.ExceptionMessage += string.Format(" | InnerException: {0}", ex.InnerException.InnerException.Message);
                    }
                }

                _loggedExceptionOperations.Save(loggedException);
            }
            catch (Exception)
            {
                // Ignore this exception!
            }

            // If not in development environment, redirect to error.aspx
            if (ConfigurationManager.AppSettings["DeploymentLocation"] != "Development")
            {
                Response.Redirect(string.Format("~/Error.aspx?guid={0}", Server.UrlEncode(faultCode)), false);
            }
            else
            {
                Response.Redirect(string.Format("~/Error.aspx?guid={0}&msg={1}&src={2}&ts={3}&st={4}",
                                                Server.UrlEncode(faultCode),
                                                Server.UrlEncode(ex.Message + (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message) ? " InnerException: " + ex.InnerException.Message : "")),
                                                Server.UrlEncode(ex.Source),
                                                Server.UrlEncode(ex.TargetSite.Name),
                                                Server.UrlEncode(ex.StackTrace)), false);
            }
        }

        #endregion

    }
}

