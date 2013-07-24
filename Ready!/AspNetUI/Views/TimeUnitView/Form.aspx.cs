using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI.Views.TimeUnitView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idTimeUnit;
        private int? _idAct;
        private bool? _isResponsibleUser;

        private ITime_unit_PKOperations _timeUnitOperations;
        private ITime_unit_name_PKOperations _timeUnitNameOperations;
        private IActivity_PKOperations _activityOperations;
        private IPerson_PKOperations _personOperations;
        private IUSEROperations _userOperations;
        private ILast_change_PKOperations _lastChangeOperations;

        #endregion

        #region Properties

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadFormVariables();
            BindEventHandlers();
            GenerateContextMenuItems();
            AssociatePanels(pnlForm, pnlFooter);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsPostBack) return;

            InitForm(null);

            if (FormType == FormType.Edit || FormType == FormType.SaveAs)
            {
                BindForm(null);
            }

            SetFormControlsDefaults(null);
            SecurityPageSpecific();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            GenerateTabMenuItems();
            GenerateTopMenuItems();
            MasterPage.UpTopMenu.Update();
        }

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();
            LoadActionQuery();

            _idTimeUnit = ValidationHelper.IsValidInt(Request.QueryString["idTimeUnit"]) ? int.Parse(Request.QueryString["idTimeUnit"]) : (int?)null;
            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;

            _timeUnitOperations = new Time_unit_PKDAL();
            _timeUnitNameOperations = new Time_unit_name_PKDAL();
            _activityOperations = new Activity_PKDAL();
            _personOperations = new Person_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _userOperations = new USERDAL();
        }

        public override void LoadActionQuery()
        {
            base.LoadActionQuery();
            if (_idAct.HasValue) EntityContext = EntityContext.Activity;
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            txtSrActivity.Searcher.OnListItemSelected += TxtSrActivity_OnListItemSelected;
        }

        private void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
        }

        #endregion

        #region Fill

        private void ClearForm(object arg)
        {
            // Entity name
            lblPrvTimeUnit.Text = Constant.DefaultEmptyValue;

            // Left pane
            txtSrActivity.Clear();
            ddlName.SelectedValue = string.Empty;
            ddlResponsibleUser.SelectedValue = String.Empty;
            txtDescription.Text = String.Empty;
            dtActualDate.Text = String.Empty;
            tTime.Clear();
            txtComment.Text = String.Empty;
        }

        private void FillFormControls(object arg)
        {
            FillDdlResponsibleUser();
            FillDdlName(null);
        }

        private void SetFormControlsDefaults(object arg)
        {
            if (FormType == FormType.New || FormType == FormType.SaveAs)
            {
                var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                ddlResponsibleUser.SelectedValue = user != null ? user.Person_FK : null;

                dtActualDate.Text = DateTime.Now.ToString(Constant.DateTimeFormat);
            }

            lblPrvTimeUnit.Visible = FormType != FormType.New;

            if (FormType == FormType.New)
            {
                if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
                {
                    var activity = _idAct.HasValue ? _activityOperations.GetEntity(_idAct) : null;

                    if (activity != null)
                    {
                        lblPrvTimeUnit.Visible = true;
                        lblPrvTimeUnit.Label = "Activity:";
                        lblPrvTimeUnit.Text = activity.name;

                        txtSrActivity.Text = activity.name;
                        txtSrActivity.SelectedEntityId = activity.activity_PK;
                    }
                    else
                    {
                        lblPrvTimeUnit.Text = Constant.ControlDefault.LbPrvText;
                    }
                }
            }
        }

        private void FillDdlResponsibleUser()
        {
            var responsibleUserList = _personOperations.GetPersonsByRole(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, x => x.FullName, x => x.person_PK);
            ddlResponsibleUser.SortItemsByText();
        }

        private void FillDdlName(object args)
        {
            var timeUnitNamelist = _timeUnitNameOperations.GetEntities();
            ddlName.Fill(timeUnitNamelist, x => x.time_unit_name, x => x.time_unit_name_PK);
            ddlName.SortItemsByText();
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idTimeUnit.HasValue) return;

            var timeUnit = _timeUnitOperations.GetEntity(_idTimeUnit.Value);
            if (timeUnit == null) return;

            // Entity
            var timeUnitName = timeUnit.time_unit_name_FK.HasValue ? _timeUnitNameOperations.GetEntity(timeUnit.time_unit_name_FK) : null;
            lblPrvTimeUnit.Text = timeUnitName != null ? timeUnitName.time_unit_name : Constant.ControlDefault.LbPrvText;

            // Activity
            BindActivity(timeUnit.activity_FK);

            // Name
            ddlName.SelectedId = timeUnit.time_unit_name_FK;

            // Responsible user
            ddlResponsibleUser.SelectedId = timeUnit.user_FK;

            // Description
            txtDescription.Text = timeUnit.description;

            // Actual date
            dtActualDate.Text = timeUnit.actual_date.HasValue ? timeUnit.actual_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            //Time
            tTime.TextHours = Convert.ToString(timeUnit.time_hours);
            tTime.TextMinutes = Convert.ToString(timeUnit.time_minutes);

            // Comment
            txtComment.Text = timeUnit.comment;

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = timeUnit.user_FK == user.Person_FK;
        }

        private void BindActivity(int? activityPk)
        {
            var activity = activityPk.HasValue ? _activityOperations.GetEntity(activityPk) : null;

            if (activity != null)
            {
                txtSrActivity.Text = activity.name;
                txtSrActivity.SelectedEntityId = activity.activity_PK;
            }
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (!ddlName.SelectedId.HasValue)
            {
                errorMessage += "Name can't be empty.<br />";
                ddlName.ValidationError = "Name can't be empty.";
            }

            if (!string.IsNullOrWhiteSpace(dtActualDate.Text) &&
                !ValidationHelper.IsValidDateTime(dtActualDate.Text, CultureInfoHr))
            {
                errorMessage += "Actual date is not a valid date.<br />";
                dtActualDate.ValidationError = "Actual date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(tTime.TextHours) &&
                !ValidationHelper.IsValidInt(tTime.TextHours))
            {
                errorMessage += "Time hours is not valid number.<br />";
                tTime.ValidationError = "Time hours is not valid number.";
            }
            else if (!string.IsNullOrWhiteSpace(tTime.TextMinutes) &&
                !ValidationHelper.IsValidInt(tTime.TextMinutes))
            {
                errorMessage += "Time minutes is not valid number.<br />";
                tTime.ValidationError = "Time minutes is not valid number.";
            }

            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                MasterPage.ModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }

            return true;
        }

        private void ClearValidationErrors()
        {
            // Left pane
            ddlName.ValidationError = String.Empty;
            dtActualDate.ValidationError = String.Empty;
            tTime.ValidationError = String.Empty;

            // Right pane

        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            var timeUnit = new Time_unit_PK();

            if (FormType == FormType.Edit)
            {
                timeUnit = _timeUnitOperations.GetEntity(_idTimeUnit);
            }

            if (timeUnit == null) return null;

            timeUnit.activity_FK = txtSrActivity.SelectedEntityId;
            timeUnit.time_unit_name_FK = ddlName.SelectedId;
            timeUnit.user_FK = ddlResponsibleUser.SelectedId;
            timeUnit.description = txtDescription.Text;
            timeUnit.actual_date = ValidationHelper.IsValidDateTime(dtActualDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtActualDate.Text) : null;
            timeUnit.time_hours = ValidationHelper.IsValidInt(tTime.TextHours) ? (int?)int.Parse(tTime.TextHours) : null;
            timeUnit.time_minutes = ValidationHelper.IsValidInt(tTime.TextMinutes) ? (int?)int.Parse(tTime.TextMinutes) : null;
            timeUnit.comment = txtComment.Text;

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) timeUnit.inserted_by = user.Person_FK;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                timeUnit = _timeUnitOperations.Save(timeUnit);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, timeUnit.time_unit_PK, "TIME_UNIT", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, timeUnit.time_unit_PK, "TIME_UNIT", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return timeUnit;
        }

        #endregion

        #region Delete

        private void DeleteEntity(object arg)
        {

        }

        #endregion

        #endregion

        #region Event handlers

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.Cancel:
                    if (EntityContext == EntityContext.TimeUnit || EntityContext == EntityContext.TimeUnitMy)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/TimeUnitView/Preview.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext, _idTimeUnit));
                    }
                    else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "Act" || From == "ActMy") Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "ActSearch") Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if ((From == "ActPreview" || From == "ActMyPreview") && _idAct.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                            else if ((From == "ActTimeUnitList" || From == "ActMyTimeUnitList") && _idAct.HasValue) Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                            else if (From == "TimeUnitActPreview" && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnit, _idTimeUnit));
                            else if (From == "TimeUnitMyActPreview" && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnitMy, _idTimeUnit));
                            else if (_idAct.HasValue) Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                        }
                    }
                    else if (EntityContext == EntityContext.Default)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "TimeUnit") Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext.TimeUnit));
                            else if (From == "TimeUnitMy") Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext.TimeUnitMy));
                        }
                    }
                    Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext.TimeUnit));
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedTimeUnit = SaveForm(null);
                        if (savedTimeUnit is Time_unit_PK)
                        {
                            var timeUnit = savedTimeUnit as Time_unit_PK;
                            if (timeUnit.time_unit_PK.HasValue)
                            {
                                MasterPage.OneTimePermissionToken = Permission.View;
                                if (EntityContext == EntityContext.TimeUnit) Response.Redirect(string.Format("~/Views/TimeUnitView/Preview.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnit, timeUnit.time_unit_PK));
                                else if (EntityContext == EntityContext.TimeUnitMy) Response.Redirect(string.Format("~/Views/TimeUnitView/Preview.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnitMy, timeUnit.time_unit_PK));
                                else Response.Redirect(string.Format("~/Views/TimeUnitView/Preview.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnit, timeUnit.time_unit_PK));
                            }
                        }
                        Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext.TimeUnit));
                    }
                    break;
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            OnContextMenuItemClick(null, new ContextMenuEventArgs(ContextMenuEventTypes.Save));
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            OnContextMenuItemClick(null, new ContextMenuEventArgs(ContextMenuEventTypes.Cancel));
        }

        #endregion

        #region Activity searcher

        void TxtSrActivity_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var activity = _activityOperations.GetEntity(e.Data);

            if (activity == null || activity.activity_PK == null) return;

            txtSrActivity.Text = activity.name;
            txtSrActivity.SelectedEntityId = activity.activity_PK;
        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContextMenuItems()
        {
            var contextMenu = new[]
            {
                new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), 
                new ContextMenuItem(ContextMenuEventTypes.Save, "Save"), 
            };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location;

            if (FormType == FormType.New)
            {
                if (EntityContext == EntityContext.Activity) location = Support.LocationManager.Instance.GetLocationByName("ActTimeUnitList", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.ActivityMy) location = Support.LocationManager.Instance.GetLocationByName("ActMyTimeUnitList", Support.CacheManager.Instance.AppLocations);
                else
                {
                    location = Support.LocationManager.Instance.ParseLocationFromUrl(Request.ExtractCurrentQuery(new List<string> { "EntityContext", "Action" }), Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                    if (location != null)
                    {
                        MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                        MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    }
                    return;
                }

                MasterPage.TabMenu.TabControls.Clear();
                tabMenu.Visible = true;
                if (location != null)
                {
                    tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                    tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
            else
            {
                location = Support.LocationManager.Instance.GetLocationByName("TimeUnitPreview", Support.CacheManager.Instance.AppLocations);
                MasterPage.TabMenu.TabControls.Clear();
                tabMenu.Visible = true;
                if (location != null)
                {
                    tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                    tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location;

            if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) location = Support.LocationManager.Instance.GetLocationByName("TimeUnitFormNew", Support.CacheManager.Instance.AppLocations);
            else
            {
                location = Support.LocationManager.Instance.ParseLocationFromUrl(Request.ExtractCurrentQuery(new List<string> { "EntityContext", "Action" }), Support.CacheManager.Instance.AppLocations);
            }

            if (location != null)
            {
                var topLevelParent = MasterPage.FindTopLevelParent(location);

                MasterPage.CurrentLocation = location;
                MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            }
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedInsertTimeUnit = false;

            if (EntityContext == EntityContext.Default) isPermittedInsertTimeUnit = SecurityHelper.IsPermitted(Permission.InsertTimeUnit);
            if (isPermittedInsertTimeUnit)
            {
                SecurityHelper.SetControlsForReadWrite(
                               MasterPage.ContextMenu,
                               new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") },
                               new List<Panel> { PnlForm },
                               new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } }
                           );
            }
            else
            {
                SecurityHelper.SetControlsForRead(
                                  MasterPage.ContextMenu,
                                  new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") },
                                  new List<Panel> { PnlForm },
                                  new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } }
                              );
            }

            if (MasterPage.RefererLocation != null)
            {
                isPermittedInsertTimeUnit = SecurityHelper.IsPermittedAny(new List<Permission> { Permission.InsertTimeUnit, Permission.SaveAsTimeUnit, Permission.EditTimeUnit }, MasterPage.RefererLocation);
                if (isPermittedInsertTimeUnit)
                {
                    SecurityHelper.SetControlsForReadWrite(
                                   MasterPage.ContextMenu,
                                   new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") },
                                   new List<Panel> { PnlForm },
                                   new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } }
                               );
                }
            }

            SecurityPageSpecificMy(_isResponsibleUser);

            return true;
        }

        #endregion
    }
}