using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AspNetUI.Views.Shared.Interface;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.ProjectView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idProj;
        private bool? _isResponsibleUser;

        private IProject_PKOperations _projectOperations;
        private IProject_country_PKOperations _projectCountryOperations;
        private IPerson_PKOperations _personOperations;
        private IType_PKOperations _typeOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private ICountry_PKOperations _countryOperations;
        private IReminder_PKOperations _reminderOperations;
        private IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations;
        private IUSEROperations _userOperations;
        private IReminder_date_PKOperations _reminderDateOperations;

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

            if (IsPostBack)
            {
                BindDynamicControls(null);
                return;
            }

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

            _idProj = ValidationHelper.IsValidInt(Request.QueryString["idProj"]) ? int.Parse(Request.QueryString["idProj"]) : (int?)null;

            _projectOperations = new Project_PKDAL();
            _projectCountryOperations = new Project_country_PKDAL();
            _personOperations = new Person_PKDAL();
            _typeOperations = new Type_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _countryOperations = new Country_PKDAL();
            _reminderOperations = new Reminder_PKDAL();
            _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            _userOperations = new USERDAL();
            _reminderDateOperations = new Reminder_date_PKDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            dtStartDate.LnkSetReminder.Click += DtStartDateLnkSetReminder_OnClick;
            dtExpectedFinishedDate.LnkSetReminder.Click += DtExpectedFinishedLnkSetReminder_OnClick;

            lbAuCountries.OnAssignClick += lbAuCountries_OnAssignClick;
            lbAuCountries.OnUnassignClick += lbAuCountries_OnUnassignClick;

            mpConfirmInternalStatus.OnYesButtonClick += MpConfirmInternalStatus_OnYesButtonClick;
            mpConfirmInternalStatus.OnNoButtonClick += MpConfirmInternalStatus_OnNoButtonClick;
            Reminder.OnConfirmInputButtonProcess_Click += Reminder_OnConfirmInputButtonProcess_Click;
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
            lblPrvProject.Text = Constant.DefaultEmptyValue;

            // Left pane
            txtName.Text = String.Empty;
            txtDescription.Text = String.Empty;
            ddlResponsibleUser.SelectedValue = String.Empty;
            ddlInternalStatus.Text = String.Empty;
            lbAuCountries.Clear();
            txtComment.Text = String.Empty;
            dtStartDate.Text = String.Empty;
            dtExpectedFinishedDate.Text = String.Empty;
            dtActualFinishedDate.Text = String.Empty;

            // Right pane
        }

        private void FillFormControls(object arg)
        {
            FillDdlResponsibleUser();
            FillDdlInternalStatus();

            if (FormType == FormType.New)
            {
                FillLbAuCountries();
            }
        }

        private void SetFormControlsDefaults(object arg)
        {
            if (FormType == FormType.New || FormType == FormType.SaveAs)
            {
                var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                ddlResponsibleUser.SelectedValue = user != null ? user.Person_FK : null;

                dtStartDate.Text = DateTime.Now.ToString(Constant.DateTimeFormat);

                HideReminders();
            }

            lblPrvProject.Visible = FormType != FormType.New;

            if (FormType == FormType.New)
            {
                rbYnAutomaticAlerts.SelectedValue = true;
            }

            BindDynamicControls(null);
        }

        private void FillDdlResponsibleUser()
        {
            var responsibleUserList = _personOperations.GetPersonsByRole(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, x => x.FullName, x => x.person_PK);
            ddlResponsibleUser.SortItemsByText();
        }

        private void FillDdlInternalStatus()
        {
            var internalStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.InternalStatus);
            ddlInternalStatus.Fill(internalStatusList, x => x.name, x => x.type_PK);
            ddlInternalStatus.SortItemsByText();
        }

        private void FillLbAuCountries()
        {
            var countryList = _countryOperations.GetEntitiesCustomSort();
            countryList.SortByField(x => x.custom_sort_ID);
            lbAuCountries.LbInputFrom.Fill(countryList, x => string.Format("{0} - {1}", x.abbreviation, x.name), x => x.country_PK);
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idProj.HasValue) return;

            var project = _projectOperations.GetEntity(_idProj.Value);
            if (project == null || !project.project_PK.HasValue) return;

            // Entity
            lblPrvProject.Text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.ControlDefault.LbPrvText;

            // Name
            txtName.Text = project.name;

            // Description
            txtDescription.Text = project.description;

            // Responsible user
            ddlResponsibleUser.SelectedId = project.user_FK;

            // Internal status
            ddlInternalStatus.SelectedId = project.internal_status_type_FK;

            // Countries
            BindCountries(project.project_PK.Value);

            // Comment
            txtComment.Text = project.comment;

            // Start date
            dtStartDate.Text = project.start_date.HasValue ? project.start_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            // Expected finished date
            dtExpectedFinishedDate.Text = project.expected_finished_date.HasValue ? project.expected_finished_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            // Actual finished date
            dtActualFinishedDate.Text = project.actual_finished_date.HasValue ? project.actual_finished_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            //Time
            rbYnAutomaticAlerts.SelectedValue = project.automatic_alerts_on;

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = project.user_FK == user.Person_FK;
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idProj.HasValue) return;

            RefreshReminderStatus();
        }

        private void BindCountries(int projectPk)
        {
            var countryAvailableList = _countryOperations.GetAvailableEntitiesByProject(projectPk);
            countryAvailableList.SortByField(x => x.custom_sort_ID);
            lbAuCountries.LbInputFrom.Fill(countryAvailableList, x => string.Format("{0} - {1}", x.abbreviation, x.name), x => x.country_PK);

            var countryAssignedList = _countryOperations.GetAssignedEntitiesByProject(projectPk);
            countryAssignedList.SortByField(x => x.custom_sort_ID);
            lbAuCountries.LbInputTo.Fill(countryAssignedList, x => string.Format("{0} - {1}", x.abbreviation, x.name), x => x.country_PK);
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();


            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorMessage += "Name can't be empty.<br />";
                txtName.ValidationError = "Name can't be empty.";
            }

            if (!string.IsNullOrWhiteSpace(dtStartDate.Text) &&
                !ValidationHelper.IsValidDateTime(dtStartDate.Text, CultureInfoHr))
            {
                errorMessage += "Start date is not a valid date.<br />";
                dtStartDate.ValidationError = "Start date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtExpectedFinishedDate.Text) &&
                !ValidationHelper.IsValidDateTime(dtExpectedFinishedDate.Text, CultureInfoHr))
            {
                errorMessage += "Expected finished date is not a valid date.<br />";
                dtExpectedFinishedDate.ValidationError = "Expected finished date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtActualFinishedDate.Text) &&
                !ValidationHelper.IsValidDateTime(dtActualFinishedDate.Text, CultureInfoHr))
            {
                errorMessage += "Actual finished date is not a valid date.<br />";
                dtActualFinishedDate.ValidationError = "Actual finished date is not a valid date.";
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
            txtName.ValidationError = String.Empty;
            ddlInternalStatus.ValidationError = String.Empty;
            lbAuCountries.ValidationError = String.Empty;
            dtStartDate.ValidationError = String.Empty;
            dtExpectedFinishedDate.ValidationError = String.Empty;
            dtActualFinishedDate.ValidationError = String.Empty;

            // Right pane
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            var project = new Project_PK();

            if (FormType == FormType.Edit)
            {
                project = _projectOperations.GetEntity(_idProj);
            }

            if (project == null) return null;

            project.name = txtName.Text;
            project.description = txtDescription.Text;
            project.user_FK = ddlResponsibleUser.SelectedId;
            project.internal_status_type_FK = ddlInternalStatus.SelectedId;
            project.comment = txtComment.Text;
            project.start_date = ValidationHelper.IsValidDateTime(dtStartDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtStartDate.Text) : null;
            project.expected_finished_date = ValidationHelper.IsValidDateTime(dtExpectedFinishedDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtExpectedFinishedDate.Text) : null;
            project.actual_finished_date = ValidationHelper.IsValidDateTime(dtActualFinishedDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtActualFinishedDate.Text) : null;
            project.automatic_alerts_on = rbYnAutomaticAlerts.SelectedValue ?? false;

            // Prevent alert for start date if specified date is today
            if (project.start_date.HasValue && project.start_date.Value.Date == DateTime.Now.Date)
            {
                project.prevent_start_date_alert = true;
            }

            // Prevent alert for expected finished date if specified date is today
            if (project.expected_finished_date.HasValue && project.expected_finished_date.Value.Date == DateTime.Now.Date)
            {
                project.prevent_exp_finish_date_alert = true;
            }

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                project = _projectOperations.Save(project);

                if (!project.project_PK.HasValue) return null;

                SaveCountries(project.project_PK.Value, auditTrailSessionToken);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, project.project_PK, "PROJECT", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, project.project_PK, "PROJECT", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return project;
        }

        private void SaveCountries(int projectPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var countryList = _countryOperations.GetAssignedEntitiesByProject(projectPk);
            countryList.SortByField(x => x.custom_sort_ID);

            foreach (var country in countryList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += ", ";
                complexAuditOldValue += country.abbreviation;
            }

            _projectCountryOperations.deleteByProject(projectPk);

            var projectCountryMnList = new List<Project_country_PK>(lbAuCountries.LbInputTo.Items.Count);

            foreach (ListItem listItem in lbAuCountries.LbInputTo.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                projectCountryMnList.Add(new Project_country_PK(null, projectPk, int.Parse(listItem.Value)));

                var country = _countryOperations.GetEntity(listItem.Value);
                if (country != null && !string.IsNullOrWhiteSpace(country.abbreviation))
                {
                    if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += ", ";
                    complexAuditNewValue += country.abbreviation;
                }
            }

            if (projectCountryMnList.Count > 0)
            {
                _projectCountryOperations.SaveCollection(projectCountryMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, projectPk.ToString(), "PROJECT_COUNTRY_MN");
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
                    if (EntityContext == EntityContext.Project)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idProj.HasValue) Response.Redirect(string.Format("~/Views/ProjectView/Preview.aspx?EntityContext={0}&idProj={1}", EntityContext, _idProj));
                    }
                    else if (EntityContext == EntityContext.Default)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "Proj") Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext.Project));
                        }
                    }
                    Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext.Project));
                    break;

                case ContextMenuEventTypes.Save:
                    PromptBeforeSaveForm();
                    break;
            }
        }

        private void PromptBeforeSaveForm()
        {
            if (ValidateForm(null))
            {
                if (!string.IsNullOrWhiteSpace(dtActualFinishedDate.Text) && ddlInternalStatus.SelectedItem.Text.Trim().ToLower() != "finished")
                {
                    mpConfirmInternalStatus.ShowModalPopup("Actual finished date", "<center>Change internal status to \"Finished\"?</center><br />", ModalPopupMode.YesNoCancel);
                    return;
                }

                if (string.IsNullOrWhiteSpace(dtActualFinishedDate.Text) && ddlInternalStatus.SelectedItem.Text.Trim().ToLower() == "finished")
                {
                    var message = string.Format("<center>Set Actual Finished date to Current date ({0})?</center><br />", DateTime.Now.Date.ToString("dd.MM.yyyy"));
                    mpConfirmInternalStatus.ShowModalPopup("Actual finished date", message, ModalPopupMode.YesNoCancel);
                    return;
                }

                SaveFormAfterPrompt();
            }
        }

        private void SaveFormAfterPrompt()
        {
            var result = SaveForm(null);
            if (result is Project_PK)
            {
                var project = result as Project_PK;
                if (project.project_PK.HasValue)
                {
                    MasterPage.OneTimePermissionToken = Permission.View;
                    Response.Redirect(string.Format("~/Views/ProjectView/Preview.aspx?EntityContext={0}&idProj={1}", EntityContext.Project, project.project_PK));
                }
            }
            Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext.Project));
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

        #region Reminders

        private void DtStartDateLnkSetReminder_OnClick(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(dtStartDate.Label)), dtStartDate.Text);
        }

        public void DtExpectedFinishedLnkSetReminder_OnClick(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(dtExpectedFinishedDate.Label)), dtExpectedFinishedDate.Text);
        }

        public void Reminder_OnConfirmInputButtonProcess_Click(object sender, EventArgs e)
        {
            var reminder = Reminder.ReminderVs;

            reminder.user_FK = SessionManager.Instance.CurrentUser.UserID;

            reminder.navigate_url = string.Format("~/Views/ProjectView/Preview.aspx?EntityContext={0}idProj={1}", EntityContext.Project, _idProj);
            reminder.TableName = ReminderTableName.PROJECT;
            reminder.entity_FK = _idProj;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;

                reminder = _reminderOperations.Save(reminder);

                if (!reminder.reminder_PK.HasValue) return;

                var reminderEmailRecipientPkList = Reminder.ReminderEmailRecipients.Select(reminderEmailrecipient => new Reminder_email_recipient_PK(null, reminder.reminder_PK, reminderEmailrecipient)).ToList();

                AlerterHelper.SaveEmailRecipients(reminderEmailRecipientPkList, reminder, auditTrailSessionToken);
                AlerterHelper.SaveReminderDates(reminder, auditTrailSessionToken);

                ts.Complete();
            }
            RefreshReminderStatus();
        }

        #endregion

        #region Confirm internal status

        private void MpConfirmInternalStatus_OnYesButtonClick(object sender, EventArgs eventArgs)
        {
            if (!string.IsNullOrWhiteSpace(dtActualFinishedDate.Text))
            {
                if (ddlInternalStatus.SelectedItem.Text != "Finished")
                {
                    var listItem = ddlInternalStatus.DdlInput.Items.FindByText("Finished");
                    if (listItem != null)
                    {
                        ddlInternalStatus.SelectedValue = listItem.Value;
                    }
                }
            }
            else
            {
                if (ddlInternalStatus.SelectedItem.Text == "Finished")
                {
                    dtActualFinishedDate.Text = DateTime.Now.ToString(Constant.DateTimeFormat);
                }
            }

            SaveFormAfterPrompt();
        }

        private void MpConfirmInternalStatus_OnNoButtonClick(object sender, EventArgs eventArgs)
        {
            SaveFormAfterPrompt();
        }

        #endregion

        #region Countries

        private void lbAuCountries_OnUnassignClick(object sender, EventArgs e)
        {
            lbAuCountries.LbInputTo.MoveSelectedItemsTo(lbAuCountries.LbInputFrom);
            SortCountries(lbAuCountries.LbInputFrom);
        }

        private void lbAuCountries_OnAssignClick(object sender, EventArgs e)
        {
            lbAuCountries.LbInputFrom.MoveSelectedItemsTo(lbAuCountries.LbInputTo);
            SortCountries(lbAuCountries.LbInputTo);
        }

        #endregion

        #endregion

        #region Support methods

        private void SortCountries(System.Web.UI.WebControls.ListBox sourceListBox)
        {
            var allCountries = _countryOperations.GetEntitiesCustomSort();
            var sortedList = sourceListBox.Items.GetSortedListByField(allCountries, x => x.country_PK, x => x.custom_sort_ID);
            sourceListBox.Fill(sortedList, x => string.Format("{0} - {1}", x.abbreviation, x.name), x => x.country_PK);
        }

        private void HideReminders()
        {
            dtStartDate.ShowReminder = false;
            dtExpectedFinishedDate.ShowReminder = false;
        }

        public void RefreshReminderStatus()
        {
            var tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.PROJECT);
            AlerterHelper.RefreshReminderStatus(_reminderOperations, MasterPage, new List<IReminderCustomControl> { dtStartDate, dtExpectedFinishedDate }, tableName, _idProj);
        }

        public void SetReminder(String attributeName, String attributeValue)
        {
            var reminder = new Reminder_PK
            {
                reminder_type = "Project",
                reminder_name = lblPrvProject.Text,
                related_attribute_name = attributeName,
                related_attribute_value = attributeValue
            };

            Reminder.ReminderVs = reminder;
            Reminder.ShowModalPopup("Set alert");
            RefreshReminderStatus();
        }

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
                location = Support.LocationManager.Instance.GetLocationByName("ProjFormNew", Support.CacheManager.Instance.AppLocations);
                if (location != null)
                {
                    MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                    MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                }
            }
            else
            {
                location = Support.LocationManager.Instance.GetLocationByName("ProjPreview", Support.CacheManager.Instance.AppLocations);
                if (location != null)
                {
                    MasterPage.TabMenu.TabControls.Clear();
                    tabMenu.Visible = true;
                    tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                    tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location;

            if (FormType == FormType.New)
            {
                location = Support.LocationManager.Instance.GetLocationByName("ProjFormNew", Support.CacheManager.Instance.AppLocations);
            }
            else
            {
                location = Support.LocationManager.Instance.GetLocationByName("ProjPreview", Support.CacheManager.Instance.AppLocations);
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

            var isPermittedInsertProject = false;
            if (EntityContext == EntityContext.Default) isPermittedInsertProject = SecurityHelper.IsPermitted(Permission.InsertProject);

            if (isPermittedInsertProject)
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
                isPermittedInsertProject = SecurityHelper.IsPermittedAny(new List<Permission> { Permission.InsertProject, Permission.SaveAsProject, Permission.EditProject }, MasterPage.RefererLocation);
                if (isPermittedInsertProject)
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