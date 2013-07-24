using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Views;
using AspNetUIFramework;
using CommonTypes;
using System.Configuration;
using System.Threading;
using System.Web.Security;
using Kmis.Model;
using AspNetUI.Support;
using System.Drawing;
using Ready.Model;

namespace AspNetUI
{

    public partial class MasterMain : System.Web.UI.MasterPage, IMasterPageOperationalSupport
    {
        XmlLocation _currentLocation = null;
        List<XmlLocation> _topMenuLocations = new List<XmlLocation>();
        List<XmlLocation> _leftMenuLocations = new List<XmlLocation>();
        RightTypes _rightTypeOfCurrentUserForThisPage = RightTypes.Default;

        // Operation managers
        IUSEROperations _userOperations;
        IUSER_ROLEOperations _roleOperations;
        ILoggedExceptionOperations _loggedExceptionOperations;
        IDowntimeOperations _downtimeOperations;
        ICountry_PKOperations _countryOperations;
        IReminder_PKOperations _reminderOperations;
        IPerson_PKOperations _personOperations;


        // Everything is based on user's country ID
        public int? CountryID
        {
            get { return Session["CountryID"] == null ? null : (int?)Session["CountryID"]; }
            set { Session["CountryID"] = value; }
        }

        // LogoURL
        public string LogoURL
        {
            get { return Session["LogoURL"] == null ? null : Session["LogoURL"].ToString(); }
            set { Session["LogoURL"] = value; }
        }

        // Currency is dependant on country
        public string Currency
        {
            get { return Session["Currency"] == null ? null : (string)Session["Currency"]; }
            set { Session["Curreny"] = value; }
        }

        // Currency is dependant on country
        public Control MenuSpacer
        {
            get { return menuSpacer; }
        }

        public bool InFormPostBack 
        {
            set { this.inFormPostBack.Value = value == true ? "true" : "false"; }
        }

        #region Properties

        public List<XmlLocation> TopMenuLocations
        {
            get { return _topMenuLocations; }
            set { _topMenuLocations = value; }
        }

        public List<XmlLocation> LeftMenuLocations
        {
            get { return _leftMenuLocations; }
            set { _leftMenuLocations = value; }
        }

        public TabMenu TabMenu
        {
            get { return tabMenu; }
        }

        public TopMenu TopMenu
        {
            get { return topMenu; }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            lnkLayoutHeaderLeft.HRef = "~/Views/ActivityView/List.aspx";
            lnkLayoutHeader.NavigateUrl = "~/Views/ActivityView/List.aspx";
            
            try
            {
                // Operation managers
                _userOperations = new USERDAL();
                _roleOperations = new USER_ROLEDAL();
                _loggedExceptionOperations = new LoggedExceptionDAL();
                _downtimeOperations = new DowntimeDAL();
                _countryOperations = new Country_PKDAL();
                _reminderOperations = new Reminder_PKDAL();
                _personOperations = new Person_PKDAL();

                Session["active"] = "page1|sort2|d6|a2|filterContains([internalStatus], 'Active')";
                Session["pending"] = "page1|sort2|d6|a2|filterContains([internalStatus], 'Pending')";
                Session["finished"] = "page1|sort2|d6|a2|filterContains([internalStatus], 'Finished')";



                // Setting async. error handleing
                ScriptManager.GetCurrent(this.Page).AsyncPostBackError += new EventHandler<AsyncPostBackErrorEventArgs>(ScriptManager_AsyncPostBackError);

                #region Resolves url location and handles it

                // Mapping application location to request path
                string path = Request.Url.LocalPath.ToLower();
                //if (Request.QueryString["f"]  == "l")
                if (Request.QueryString["f"] == "l" || Request.QueryString["f"] == "dn")
                    path += "?f=" + Request.QueryString["f"];
                CurrentLocation = AspNetUIFramework.LocationManager.Instance.ParseLocationFromUrl(path, AspNetUIFramework.CacheManager.Instance.AppLocations);

                if (CurrentLocation == null)
                    CurrentLocation = AspNetUIFramework.LocationManager.Instance.ParseLocationFromUrl(Request.Url.LocalPath.ToLower(), AspNetUIFramework.CacheManager.Instance.AppLocations);

                // If current location is not active, redirect to default
                if (CurrentLocation == null || CurrentLocation.Active == false)
                {
                    try
                    {
                        Response.Redirect("~/Default.aspx", false);
                    }
                    catch (ThreadAbortException ex)
                    {

                    }
                    //if ( redirectFailed ) Response.Redirect("~/Default.aspx");
                }

                #endregion

                #region Resolving user, his groups and rights on this url

                // Checking if user is authenticated and on thread
                if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
                {
                    // Fills session with current user info
                    AppUser user = CurrentUser;

                    // Right on this location
                    RightTypeOfCurrentUserForThisPage = AspNetUIFramework.LocationManager.Instance.AuthorizeLocation(CurrentLocation, AspNetUIFramework.CacheManager.Instance.AppLocations);

                    // If does not have rights, logout
                    if (RightTypeOfCurrentUserForThisPage == RightTypes.Restricted)
                    {
                        throw new Exception("User " + user.Username + " tried to access restricted resource on location " + CurrentLocation.LocationUrl + "!");
                    }
                    else
                    {
                        // Additional rights
                        if (!user.Roles.Contains("DELETE OPERATION") && RightTypeOfCurrentUserForThisPage > RightTypes.Update) RightTypeOfCurrentUserForThisPage = RightTypes.Update;

                        Person_PK person = _personOperations.GetPersonByUserID(user.UserID);
                        if (person != null)
                        {
                            lblLoginName.Text = person.FullName;
                            lblLoginName.NavigateUrl = string.Format("~/Views/Account/Form.aspx?EntityContext={0}", EntityContext.UserAccount);
                            lbtLogOut.Visible = true;
                        }
                        else
                        {   // Login info
                            lblLoginName.Text = CurrentUser == null ? String.Empty : CurrentUser.Username;
                            lblLoginName.NavigateUrl = string.Format("~/Views/Account/Form.aspx?EntityContext={0}", EntityContext.UserAccount);
                            lbtLogOut.Visible = true;
                        }
                    }
                }
                else
                {
                    lbtLogOut.Visible = false;
                }

                #endregion

                #region Request and postback handlers

                if (!IsPostBack)
                {
                    #region URL request

                    //

                    #endregion
                }
                else
                {
                    #region Postback handler

                    // PostbackManager.Instance.PostbackControl
                    // PostbackManager.Instance.PostbackArgument

                    #endregion
                }

                #endregion

                #region Quick links

                string localPath = Request.Url.LocalPath;
                if (!String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["AppVirtualPath"]))
                {
                    localPath = localPath.Replace(ConfigurationManager.AppSettings["AppVirtualPath"], "");
                }
                localPath = "~" + localPath;

                int? idSearch = ValidationHelper.IsValidInt(Request.QueryString["idSearch"]) ? int.Parse(Request.QueryString["idSearch"]) : (int?)null;
                string idLay = Request.QueryString["idLay"];

                var selectedQuickLink = new Tuple<string, int?, string>(localPath, idSearch, idLay);

                var quickLinksControl = QuickLinks.GenerateQuickLinks(SessionManager.Instance.CurrentUser.UserID, selectedQuickLink);
                divQuickLinksContainer.Controls.Add(quickLinksControl);

                #endregion
            }
            catch (Exception ex)
            {
                HandleClassicException(ex);
            }

            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ApplicationVersion"]))
            {
                Label1.Text = "v" + ConfigurationManager.AppSettings["ApplicationVersion"].ToString() + " ";
            }


        }

        public XmlLocation FindTopLevelParent(XmlLocation currentLocation)
        {
            List<XmlLocation> locations = AspNetUIFramework.CacheManager.Instance.AppLocations;

            do
            {
                currentLocation = locations.Find(loc => loc.LogicalUniqueName == currentLocation.ParentLocationID);
            }
            while (currentLocation != null && !currentLocation.LogicalUniqueName.Contains("Level2"));
            return currentLocation;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            try
            {

                #region Top menu generation and selection

                //topMenu.GenerateMenuItems(AspNetUIFramework.CacheManage.Instance.AppLocations);
                XmlLocation topLevelParent = FindTopLevelParent(CurrentLocation);
                topMenu.GenerateNewTopMenu(AspNetUIFramework.CacheManager.Instance.AppLocations, topLevelParent, CurrentLocation);
                topMenu.SelectItem(CurrentLocation);

                #endregion

                #region Left menu generation and selection

                leftMenu.GenerateMenuItems(AspNetUIFramework.CacheManager.Instance.AppLocations);
                leftMenu.SelectItem(CurrentLocation);
                #endregion

                #region Tab menu generation and selection

                if (Request.QueryString["n"] != null)
                {
                    switch (Request.QueryString["n"].ToString())
                    {
                        case "p":
                            CurrentLocation = AspNetUIFramework.LocationManager.Instance.GetLocationByName("Level3-NewProduct", AspNetUIFramework.CacheManager.Instance.AppLocations);
                            break;
                        case "ap":
                            CurrentLocation = AspNetUIFramework.LocationManager.Instance.GetLocationByName("Level3-NewAP", AspNetUIFramework.CacheManager.Instance.AppLocations);
                            break;
                        case "ssi":
                            CurrentLocation = AspNetUIFramework.LocationManager.Instance.GetLocationByName("Level3-NewSSI", AspNetUIFramework.CacheManager.Instance.AppLocations);
                            break;
                        //TODO: dodati nove lokacije-> pp, as, ssi, project, activity, task
                    }
                }

                if (Request.Url != null && Request.Url.ToString().Contains("ActivityPKView.aspx?f=l&idLay=my"))
                {
                    //This highlights My activities tab 
                    XmlLocation location = AspNetUIFramework.CacheManager.Instance.AppLocations.Find(item => item != null && item.LogicalUniqueName == "Level3-MyActivities");
                    tabMenu.SelectItem(location, AspNetUIFramework.CacheManager.Instance.AppLocations);
                    if (location != null)
                        CurrentLocation = location;
                }

                //tabMenu.GenerateMenuItemsByRights(AspNetUIFramework.CacheManage.Instance.AppLocations, CurrentLocation);
                tabMenu.GenerateMenuItemsByRights(AspNetUIFramework.CacheManager.Instance.AppLocations, CurrentLocation);
                tabMenu.SelectItem(CurrentLocation, AspNetUIFramework.CacheManager.Instance.AppLocations);



                #endregion

                #region Reminder

                SetReminderTextForUser();

                #endregion
            }
            catch (Exception ex)
            {
                HandleClassicException(ex);
            }
        }

        private void SetReminderTextForUser()
        {

      
            if (ConfigurationManager.AppSettings["RemindersEnabled"] != null &&
                ConfigurationManager.AppSettings["RemindersEnabled"].ToString().ToLower().Trim() == "true")
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

            if (dataSet != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows != null && dataSet.Tables[0].Rows.Count > 0 &&
                dataSet.Tables[0].Columns.Contains("NumReminders") && dataSet.Tables[0].Columns.Contains("OverDue"))
            {
                DataRow row = dataSet.Tables[0].Rows[0];
                int numReminders = row["NumReminders"] != null ? (int)row["NumReminders"] : 0;
                bool overDue = row["OverDue"] != null && row["OverDue"].ToString().Trim().ToLower() == "true" ? true : false;

                if (numReminders > 0)
                {
                    lnkReminders.Text = "You have " + numReminders.ToString() + " alert/s!";
                    lnkReminders.ForeColor = overDue ? Color.Red : Color.Black;
                    lnkReminders.NavigateUrl = "~/Views/Business/RemindersView.aspx";
                }
                else
                {
                    lnkReminders.Text = "You don't have any alerts";
                    lnkReminders.ForeColor = Color.Black;
                    lnkReminders.NavigateUrl = "~/Views/Business/RemindersView.aspx";
                }

            }
            else
            {
                lnkReminders.Text = "";
                lnkReminders.ForeColor = Color.Black;
                lnkReminders.NavigateUrl = "~/Views/Business/RemindersView.aspx";
            }
        }

        // Post load (after page / uc controls load) handler
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            try
            {
                // Security enforser => adapts form based on security info => RightType
                SecurityEnforcer.Instance.EnforceRights(this.Page, RightTypeOfCurrentUserForThisPage);

                // Entity tabs support
                // Showing all entity tabs if entity is selected
                if (Session["SelectedEntityID"] != null) TabMenu.ModifyVisibleTabList(new List<string>() { "Level3-Activities", "Level3-Calculations", "Level3-Contracts", "Level3-Deals", "Level3-Competition", "Level3-Notes", "Level3-CustomerService" }, true);
                // Else hiding all entity tabs except Contracts and Deals tab if null and current location is not Contracts and Deals tab
                else if (CurrentLocation.LogicalUniqueName != "Level3-Contracts" && CurrentLocation.LogicalUniqueName != "Level3-Deals" && CurrentLocation.LogicalUniqueName != "Level3-Competition") TabMenu.ModifyVisibleTabList(new List<string>() { "Level3-Activities", "Level3-Calculations", "Level3-Notes", "Level3-CustomerService" }, false);
                // Else hide all entity tabs except Contracts and Deals tab
                else TabMenu.ModifyVisibleTabList(new List<string>() { "Level3-Activities", "Level3-Calculations", "Level3-Notes", "Level3-CustomerService" }, false);
            }
            catch (Exception ex)
            {
                HandleClassicException(ex);
            }
        }

        public void RegisterPostbackTrigger(Control trigger)
        {
            smMain.RegisterPostBackControl(trigger);
        }

        // Logout
        protected void lbtLogOut_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            //FormsAuthentication.RedirectToLoginPage();
            Response.Redirect("~/Login.aspx", true);
            Response.End();
        }

        // About
        protected void lbtAbout_Click(object sender, EventArgs e)
        {
            AboutModalPopup.ShowModalPopup("", "");
        }

        // My account
        //protected void lbtMyAccount_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("~/Views/Account/PasswordPreferencesView.aspx");
        //}

        #region Error Handleing

        protected override void OnError(EventArgs e)
        {
            base.OnError(e);

            Exception ex = Server.GetLastError();
            HandleClassicException(ex);
        }

        // Async exception handleing
        protected void ScriptManager_AsyncPostBackError(object sender, AsyncPostBackErrorEventArgs e)
        {
            Exception ex = e.Exception;
            HandleAjaxException(ex);
        }

        #endregion

        #region IMasterPageOperationalSupport Members

        public XmlLocation CurrentLocation
        {
            get { return _currentLocation; }
            set { _currentLocation = value; }
        }

        public AppUser CurrentUser
        {
            get
            {
                if (_userOperations == null)
                {
                    _userOperations = new USERDAL();
                    _roleOperations = new USER_ROLEDAL();
                    _countryOperations = new Country_PKDAL();
                }

                if (SessionManager.Instance.CurrentUser == null)
                {
                    USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);

                    if (user != null)
                    {
                        List<USER_ROLE> roles = _roleOperations.GetActiveRolesByUsername(Thread.CurrentPrincipal.Identity.Name);
                        string[] rolesArr = new string[roles.Count];

                        for (int i = 0; i < roles.Count; i++)
                        {
                            rolesArr[i] = roles[i].Name;
                        }

                        // Putting user in session so that it can be easily retreived from other components
                        SessionManager.Instance.CurrentUser = new AppUser(user.User_PK, user.Username, false, rolesArr);


                        // Putting user's CountryID and to session
                        CountryID = user.Country_FK;

                        // Putting Country LOGO to session
                        Country_PK country = _countryOperations.GetEntity(CountryID);

                        //if (country != null)
                        //{
                        //    LogoURL = country.LogoURL;
                        //}
                    }
                    else
                    { // active directory user
                        string myName = Thread.CurrentPrincipal.Identity.Name;
                        if (myName != null)
                        {
                            string[] rolesArr = new string[1];
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

        public IViewStateController ViewStateController
        {
            get { return vscViewStateController; }
        }

        public IContextMenu ContextMenu
        {
            get { return contextMenu; }
        }

        public IModalPopup MessageModalPopup
        {
            get { return msgModalPopup; }
        }

        public IModalPopup AboutModalPopup
        {
            get { return aboutModalPopup; }
        }

        public IModalPopup ConfirmModalPopup
        {
            get { return cfmModalPopup; }
        }

        public void HandleClassicException(Exception ex)
        {
            string faultCode = String.Empty;

            if (ex == null) return;
            else
            {
                Server.ClearError();

                // All exceptions
                try
                {
                    faultCode = Guid.NewGuid().ToString();
                    string mess = ex.Message;

                    // Logging exception
                    LoggedException loggedException = new LoggedException(null, CurrentUser.Username, ex.GetType().Name, ex.Message, ex.TargetSite.Name, ex.StackTrace, ex.Source, DateTime.Now, Environment.MachineName, new Guid(faultCode));
                    if (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message))
                    {
                        loggedException.ExceptionMessage += " | InnerException: " + ex.InnerException.Message;
                        if (ex.InnerException.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.InnerException.Message))
                        {
                            loggedException.ExceptionMessage += " | InnerException: " + ex.InnerException.InnerException.Message;

                        }
                    }
                    
                    _loggedExceptionOperations.Save(loggedException);
                }
                catch (Exception loggingException)
                {
                    // Ignore this exception!
                }


                // If not in development environment, redirect to error.aspx
                if (ConfigurationManager.AppSettings["DeploymentLocation"] != "Development")
                {
                    Response.Redirect("~/Error.aspx?guid=" + Server.UrlEncode(faultCode), false);
                }
                else
                {
                    Response.Redirect("~/Error.aspx?guid=" + Server.UrlEncode(faultCode) + "&msg=" + Server.UrlEncode(ex.Message + (ex.InnerException != null && !string.IsNullOrWhiteSpace(ex.InnerException.Message) ? " InnerException: " + ex.InnerException.Message : "")) + "&src=" + Server.UrlEncode(ex.Source) + "&ts=" + Server.UrlEncode(ex.TargetSite.Name) + "&st=" + Server.UrlEncode(ex.StackTrace), false);
                }

            }
        }

        public void HandleAjaxException(Exception ex)
        {
            string faultCode = String.Empty;
            string errorMessage = String.Empty;

            Server.ClearError();

            // Handleing db REFERENCE errors
            if (ex.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint") || (ex.InnerException != null && ex.InnerException.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint")))
            {
                errorMessage = "The DELETE statement conflicted with the REFERENCE constraint.";
                errorMessage += "<br /><br />Error ID: " + faultCode;
            }
            // Optimistic concurrency exception
            else if (ex.Message.Contains("Save aborted due to optimistic concurrency.") || (ex.InnerException != null && ex.InnerException.Message.Contains("Save aborted due to optimistic concurrency.")))
            {
                errorMessage = "Save aborted due to optimistic concurrency.";
                errorMessage += "<br /><br />Error ID: " + faultCode;
            }
            // Unknown errors
            else
            {
                // ALL exceptions
                try
                {
                    faultCode = Guid.NewGuid().ToString();

                    // Logging exception
                    LoggedException loggedException = new LoggedException(null, CurrentUser.Username, ex.GetType().Name, ex.Message, ex.TargetSite.Name, ex.StackTrace, ex.Source, DateTime.Now, Environment.MachineName, new Guid(faultCode));
                    _loggedExceptionOperations.Save(loggedException);
                }
                catch (Exception loggingException)
                {
                    // Ignore this exception!
                }

                // If not in development environment, redirect to error.aspx
                if (ConfigurationManager.AppSettings["DeploymentLocation"] != "Development")
                {
                    Response.Redirect("~/Error.aspx?guid=" + Server.UrlEncode(faultCode));
                }
                else
                {
                    errorMessage = ex.GetType().Name + Environment.NewLine + ex.Message + Environment.NewLine + ex.Source + Environment.NewLine + ex.StackTrace + Environment.NewLine + ex.TargetSite.Name;
                    errorMessage += "<br /><br />ID greške: " + faultCode;
                }
            }

            // Setting AsyncPostBackErrorMessage which is then set as client (js) error message, and handled in js!
            ScriptManager.GetCurrent(this.Page).AsyncPostBackErrorMessage = errorMessage;
        }

        #endregion
    }
}