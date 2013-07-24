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

namespace AspNetUI.Views.TaskView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idTask;
        private int? _idAct;
        private int? _idSubUnit;
        private int? _idTimeUnit;
        private bool? _isResponsibleUser;

        private ITask_PKOperations _taskOperations;
        private ITask_name_PKOperations _taskNameOperations;
        private IActivity_PKOperations _activityOperations;
        private ITask_country_PKOperations _taskCountryOperations;
        private IPerson_PKOperations _personOperations;
        private IType_PKOperations _typeOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private ICountry_PKOperations _countryOperations;
        private IReminder_PKOperations _reminderOperations;
        private IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations;
        private IUSEROperations _userOperations;
        private IReminder_date_PKOperations _reminderDateOperations;
        private ISubbmission_unit_PKOperations _submissionUnitOperations;

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

            _idTask = ValidationHelper.IsValidInt(Request.QueryString["idTask"]) ? int.Parse(Request.QueryString["idTask"]) : (int?)null;
            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;
            _idSubUnit = ValidationHelper.IsValidInt(Request.QueryString["idSubUnit"]) ? int.Parse(Request.QueryString["idSubUnit"]) : (int?)null;
            _idTimeUnit = ValidationHelper.IsValidInt(Request.QueryString["idTimeUnit"]) ? int.Parse(Request.QueryString["idTimeUnit"]) : (int?)null;

            _taskOperations = new Task_PKDAL();
            _taskNameOperations = new Task_name_PKDAL();
            _activityOperations = new Activity_PKDAL();
            _taskCountryOperations = new Task_country_PKDAL();
            _personOperations = new Person_PKDAL();
            _typeOperations = new Type_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _countryOperations = new Country_PKDAL();
            _reminderOperations = new Reminder_PKDAL();
            _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            _userOperations = new USERDAL();
            _reminderDateOperations = new Reminder_date_PKDAL();
            _submissionUnitOperations = new Subbmission_unit_PKDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            txtSrActivity.Searcher.OnListItemSelected += TxtSrActivity_OnListItemSelected;
            txtSrActivity.OnRemove += TxtSrActivity_OnRemove;

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
            lblPrvParentEntity.Text = Constant.DefaultEmptyValue;

            txtSrActivity.Clear();
            ddlTaskName.SelectedValue = String.Empty;
            txtDescription.Text = String.Empty;
            ddlResponsibleUser.SelectedValue = String.Empty;
            ddlInternalStatus.Text = String.Empty;
            lbAuCountries.Clear();
            txtComment.Text = String.Empty;
            dtStartDate.Text = String.Empty;
            dtExpectedFinishedDate.Text = String.Empty;
            dtActualFinishedDate.Text = String.Empty;
        }

        private void FillFormControls(object arg)
        {
            FillDdlResponsibleUser();
            FillDdlName();
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

            lblPrvParentEntity.Visible = FormType != FormType.New;

            if (FormType == FormType.New)
            {
                if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) BindParentEntityActivity();
                else if (EntityContext == EntityContext.SubmissionUnit) BindParentEntitySubmissionUnit();

                rbYnAutomaticAlerts.SelectedValue = true;
            } 
            
            BindDynamicControls(null);
        }

        private void BindParentEntityActivity(int? idAct = null)
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Activity:";

            var activity = idAct.HasValue ? _activityOperations.GetEntity(idAct) : _idAct.HasValue ? _activityOperations.GetEntity(_idAct) : null;
            if (activity == null)
            {
                lblPrvParentEntity.Text = Constant.ControlDefault.LbPrvText;
                return;
            }

            lblPrvParentEntity.Text = !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.ControlDefault.LbPrvText;

            BindActivity(_idAct);
            AssignCountriesForActivity(activity);
        }

        private void BindParentEntitySubmissionUnit(int? idSubUnit = null)
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Submission unit:";

            var submissionUnit = idSubUnit.HasValue ? _submissionUnitOperations.GetEntity(idSubUnit) : _idSubUnit.HasValue ? _submissionUnitOperations.GetEntity(_idSubUnit) : null;
            if (submissionUnit == null)
            {
                lblPrvParentEntity.Text = Constant.ControlDefault.LbPrvText;
                return;
            }

            var submissionUnitDescription = _typeOperations.GetEntity(submissionUnit.description_type_FK);
            lblPrvParentEntity.Text = submissionUnitDescription != null && !string.IsNullOrWhiteSpace(submissionUnitDescription.name) ? submissionUnitDescription.name : Constant.ControlDefault.LbPrvText;
        }

        private void FillDdlResponsibleUser()
        {
            var responsibleUserList = _personOperations.GetPersonsByRole(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, x => x.FullName, x => x.person_PK);
            ddlResponsibleUser.SortItemsByText();
        }

        private void FillDdlName()
        {
            var taskNameList = _taskNameOperations.GetEntities();
            ddlTaskName.Fill(taskNameList, x => x.task_name, x => x.task_name_PK);
            ddlTaskName.SortItemsByText();
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
            if (!_idTask.HasValue) return;

            var task = _taskOperations.GetEntity(_idTask.Value);
            if (task == null || !task.task_PK.HasValue) return;

            // Entity
            var taskName = task.task_name_FK.HasValue ? _taskNameOperations.GetEntity(task.task_name_FK) : null;
            lblPrvParentEntity.Text = taskName != null ? taskName.task_name : Constant.ControlDefault.LbPrvText;

            // Activity
            BindActivity(task.activity_FK);

            // Name
            ddlTaskName.SelectedId = task.task_name_FK;

            // Task Id
            txtTaskId.Text = task.task_ID;

            // Description
            txtDescription.Text = task.description;

            // Responsible user
            ddlResponsibleUser.SelectedId = task.user_FK;

            // Internal status
            ddlInternalStatus.SelectedId = task.type_internal_status_FK;

            // Countries
            BindCountries(task.task_PK.Value);

            // Comment
            txtComment.Text = task.comment;

            // Start date
            dtStartDate.Text = task.start_date.HasValue ? task.start_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            // Expected finished date
            dtExpectedFinishedDate.Text = task.expected_finished_date.HasValue ? task.expected_finished_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            // Actual finished date
            dtActualFinishedDate.Text = task.actual_finished_date.HasValue ? task.actual_finished_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            //Time
            rbYnAutomaticAlerts.SelectedValue = task.automatic_alerts_on;

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = task.user_FK == user.Person_FK;
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idTask.HasValue) return;

            RefreshReminderStatus();
        }

        private void BindActivity(int? activityPk)
        {
            var activity = _activityOperations.GetEntity(activityPk);
            if (activity == null) return;

            txtSrActivity.Text = activity.name;
            txtSrActivity.SelectedEntityId = activity.activity_PK;
        }

        private void BindCountries(int taskPk)
        {
            var countryAvailableList = _countryOperations.GetAvailableEntitiesByTask(taskPk);
            countryAvailableList.SortByField(x => x.custom_sort_ID);
            lbAuCountries.LbInputFrom.Fill(countryAvailableList, x => string.Format("{0} - {1}", x.abbreviation, x.name), x => x.country_PK);

            var countryAssignedList = _countryOperations.GetAssignedEntitiesByTask(taskPk);
            countryAssignedList.SortByField(x => x.custom_sort_ID);
            lbAuCountries.LbInputTo.Fill(countryAssignedList, x => string.Format("{0} - {1}", x.abbreviation, x.name), x => x.country_PK);
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (!txtSrActivity.SelectedEntityId.HasValue)
            {
                errorMessage += "Activity can't be empty.<br />";
                txtSrActivity.ValidationError = "Activity can't be empty.";
            }

            if (!ddlTaskName.SelectedId.HasValue)
            {
                errorMessage += "Task name can't be empty.<br />";
                ddlTaskName.ValidationError = "Task name can't be empty.";
            }

            if (!ddlInternalStatus.SelectedId.HasValue)
            {
                errorMessage += "Internal status can't be empty.<br />";
                ddlInternalStatus.ValidationError = "Internal status can't be empty.";
            }

            if (lbAuCountries.LbInputTo.Items.Count == 0)
            {
                errorMessage += "At least one country must be selected.<br />";
                lbAuCountries.ValidationError = "At least one country must be selected.";
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
            txtSrActivity.ValidationError = String.Empty;
            ddlTaskName.ValidationError = String.Empty;
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

            var task = new Task_PK();

            if (FormType == FormType.Edit)
            {
                task = _taskOperations.GetEntity(_idTask);
            }

            if (task == null) return null;

            task.activity_FK = txtSrActivity.SelectedEntityId;
            task.task_name_FK = ddlTaskName.SelectedId;
            task.description = txtDescription.Text;
            task.user_FK = ddlResponsibleUser.SelectedId;
            task.type_internal_status_FK = ddlInternalStatus.SelectedId;
            task.comment = txtComment.Text;
            task.start_date = ValidationHelper.IsValidDateTime(dtStartDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtStartDate.Text) : null;
            task.expected_finished_date = ValidationHelper.IsValidDateTime(dtExpectedFinishedDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtExpectedFinishedDate.Text) : null;
            task.actual_finished_date = ValidationHelper.IsValidDateTime(dtActualFinishedDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtActualFinishedDate.Text) : null;
            task.automatic_alerts_on = rbYnAutomaticAlerts.SelectedValue ?? false;
            task.task_ID = txtTaskId.Text;

            // Prevent alert for start date if specified date is today
            if (task.start_date.HasValue && task.start_date.Value.Date == DateTime.Now.Date)
            {
                task.prevent_start_date_alert = true;
            }

            // Prevent alert for expected finished date if specified date is today
            if (task.expected_finished_date.HasValue && task.expected_finished_date.Value.Date == DateTime.Now.Date)
            {
                task.prevent_exp_finish_date_alert = true;
            }

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                task = _taskOperations.Save(task);

                if (!task.task_PK.HasValue) return null;

                SaveCountries(task.task_PK.Value, auditTrailSessionToken);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, task.task_PK, "TASK", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, task.task_PK, "TASK", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return task;
        }

        private void SaveCountries(int taskPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var countryList = _countryOperations.GetAssignedEntitiesByTask(taskPk);
            countryList.SortByField(x => x.custom_sort_ID);

            foreach (var country in countryList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += ", ";
                complexAuditOldValue += country.abbreviation;
            }

            _taskCountryOperations.DeleteByTaskPK(taskPk);

            var taskCountryMnList = new List<Task_country_PK>(lbAuCountries.LbInputTo.Items.Count);

            foreach (ListItem listItem in lbAuCountries.LbInputTo.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                taskCountryMnList.Add(new Task_country_PK(null, taskPk, int.Parse(listItem.Value)));

                var country = _countryOperations.GetEntity(listItem.Value);
                if (country != null && !string.IsNullOrWhiteSpace(country.abbreviation))
                {
                    if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += ", ";
                    complexAuditNewValue += country.abbreviation;
                }
            }

            if (taskCountryMnList.Count > 0)
            {
                _taskCountryOperations.SaveCollection(taskCountryMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, taskPk.ToString(), "TASK_COUNTRY_MN");
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
                    if (EntityContext == EntityContext.Task)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idTask.HasValue) Response.Redirect(string.Format("~/Views/TaskView/Preview.aspx?EntityContext={0}&idTask={1}", EntityContext, _idTask));
                    }
                    else if (EntityContext == EntityContext.SubmissionUnit)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs || FormType == FormType.New) && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "SubUnitTaskPreview" && _idSubUnit.HasValue) Response.Redirect(string.Format("~/Views/TaskView/Preview.aspx?EntityContext={0}&idSubUnit={1}", EntityContext, _idSubUnit));
                        }
                    }
                    else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "Act" || From == "ActMy") Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "ActSearch") Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if ((From == "ActPreview" || From == "ActMyPreview") && _idAct.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                            else if ((From == "ActTaskList" || From == "ActMyTaskList") && _idAct.HasValue) Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                            else if (From == "TimeUnitActPreview" && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnit, _idTimeUnit));
                            else if (From == "TimeUnitMyActPreview" && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnitMy, _idTimeUnit));
                        }
                    }
                    else if (EntityContext == EntityContext.Default)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "Task") Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext.Task));
                        }
                    }
                    Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext.Task));
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
            var saveTask = SaveForm(null);
            if (saveTask is Task_PK)
            {
                var task = saveTask as Task_PK;
                if (task.task_PK.HasValue)
                {
                    MasterPage.OneTimePermissionToken = Permission.View;
                    Response.Redirect(string.Format("~/Views/TaskView/Preview.aspx?EntityContext={0}&idTask={1}", EntityContext.Task, task.task_PK));
                }
            }
            Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext.Task));
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

            reminder.navigate_url = string.Format("~/Views/TaskView/Preview.aspx?EntityContext={0}idTask={1}", EntityContext.Task, _idTask);
            reminder.TableName = ReminderTableName.TASK;
            reminder.entity_FK = _idTask;

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

        #region Activity searcher

        void TxtSrActivity_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var activity = _activityOperations.GetEntity(e.Data);

            if (activity == null || activity.activity_PK == null) return;

            txtSrActivity.Text = activity.name;
            txtSrActivity.SelectedEntityId = activity.activity_PK;

            if (FormType == FormType.New)
            {
                AssignCountriesForActivity(activity);
            }
        }

        private void TxtSrActivity_OnRemove(object sender, FormEventArgs<object> e)
        {
            txtSrActivity.Clear();
            if (FormType == FormType.New)
            {
                lbAuCountries.Clear();
                FillLbAuCountries();
            }
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

        private void AssignCountriesForActivity(Activity_PK activity)
        {
            if (activity == null || !activity.activity_PK.HasValue) return;

            lbAuCountries.Clear();

            var countryAvailableList = _countryOperations.GetAvailableEntitiesByActivity(activity.activity_PK.Value);
            countryAvailableList.SortByField(x => x.custom_sort_ID);
            lbAuCountries.LbInputFrom.Fill(countryAvailableList, x => string.Format("{0} - {1}", x.abbreviation, x.name), x => x.country_PK);

            var countryAssignedList = _countryOperations.GetAssignedEntitiesByActivity(activity.activity_PK.Value);
            countryAssignedList.SortByField(x => x.custom_sort_ID);
            lbAuCountries.LbInputTo.Fill(countryAssignedList, x => string.Format("{0} - {1}", x.abbreviation, x.name), x => x.country_PK);
        }

        private void HideReminders()
        {
            dtStartDate.ShowReminder = false;
            dtExpectedFinishedDate.ShowReminder = false;
        }

        public void RefreshReminderStatus()
        {
            var tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.TASK);
            AlerterHelper.RefreshReminderStatus(_reminderOperations, MasterPage, new List<IReminderCustomControl> { dtStartDate, dtExpectedFinishedDate }, tableName, _idTask);
       }

        public void SetReminder(String attributeName, String attributeValue)
        {
            var reminder = new Reminder_PK
            {
                reminder_type = "Task",
                reminder_name = lblPrvParentEntity.Text,
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
            Location_PK location = null;

            if (FormType == FormType.New)
            {
                if (EntityContext == EntityContext.Activity) location = Support.LocationManager.Instance.GetLocationByName("ActTaskList", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.ActivityMy) location = Support.LocationManager.Instance.GetLocationByName("ActMyTaskList", Support.CacheManager.Instance.AppLocations);
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
                if (EntityContext == EntityContext.Task) location = Support.LocationManager.Instance.GetLocationByName("TaskPreview", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.SubmissionUnit) location = Support.LocationManager.Instance.GetLocationByName("SubUnitTaskPreview", Support.CacheManager.Instance.AppLocations);

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

            if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) location = Support.LocationManager.Instance.GetLocationByName("ActFormNew", Support.CacheManager.Instance.AppLocations);
            else
            {
                location = Support.LocationManager.Instance.ParseLocationFromUrl(Request.ExtractCurrentQuery(new List<string> { "EntityContext", "Action" }), Support.CacheManager.Instance.AppLocations);
            }

            var topLevelParent = MasterPage.FindTopLevelParent(location);

            if (location != null)
            {
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

            var isPermittedInsertTask = false;

            if (EntityContext == EntityContext.Default) isPermittedInsertTask = SecurityHelper.IsPermitted(Permission.InsertTask);
            if (isPermittedInsertTask)
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
                isPermittedInsertTask = SecurityHelper.IsPermittedAny(new List<Permission> { Permission.InsertTask, Permission.SaveAsTask, Permission.EditTask }, MasterPage.RefererLocation);
                if (isPermittedInsertTask)
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