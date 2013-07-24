using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Interface;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Views.TaskView
{
    public partial class Preview : PreviewPage
    {
        #region Declarations

        private int? _idTask;
        private int? _idSubUnit;
        private int? _idAct;
        private bool? _isResponsibleUser;

        private ITask_PKOperations _taskOperations;
        private ITask_name_PKOperations _taskNameOperations;
        private IActivity_PKOperations _activityOperations;
        private IType_PKOperations _typeOperations;
        private ICountry_PKOperations _countryOperations;
        private IPerson_PKOperations _personOperations;
        private IReminder_PKOperations _reminderOperations;
        private IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations;
        private IReminder_date_PKOperations _reminderDateOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private ISubbmission_unit_PKOperations _submissionUnitOperations;
        private IUSEROperations _userOperations;

        #endregion

        #region Properties

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadFormVariables();
            BindEventHandlers();
            GenerateTabMenuItems();
            GenerateContexMenuItems();
            AssociatePanels(pnlProperties, pnlFooter);
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
            BindForm(null);
            SetFormControlsDefaults(null);
            SecurityPageSpecific();
        }

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();
            LoadActionQuery();

            _idTask = ValidationHelper.IsValidInt(Request.QueryString["idTask"]) ? int.Parse(Request.QueryString["idTask"]) : (int?)null;
            _idSubUnit = ValidationHelper.IsValidInt(Request.QueryString["idSubUnit"]) ? int.Parse(Request.QueryString["idSubUnit"]) : (int?)null;
            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;

            _taskOperations = new Task_PKDAL();
            _taskNameOperations = new Task_name_PKDAL();
            _activityOperations = new Activity_PKDAL();
            _typeOperations = new Type_PKDAL();
            _countryOperations = new Country_PKDAL();
            _personOperations = new Person_PKDAL();
            _reminderOperations = new Reminder_PKDAL();
            _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            _reminderDateOperations = new Reminder_date_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _submissionUnitOperations = new Subbmission_unit_PKDAL();
            _userOperations = new USERDAL();

            if (!_idTask.HasValue && _idSubUnit.HasValue)
            {
                var submissionUnit = _submissionUnitOperations.GetEntity(_idSubUnit);
                if (submissionUnit != null) _idTask = submissionUnit.task_FK;
            }
        }

        private void BindEventHandlers()
        {
            lblPrvStartDate.LnkSetReminder.Click += LblPrvStartdateLnkSetReminder_OnClick;
            lblPrvExpectedFinishedDate.LnkSetReminder.Click += LblPrvExpectedFinishedDateLnkSetReminder_OnClick;

            Reminder.OnConfirmInputButtonProcess_Click += Reminder_OnConfirmInputButtonProcess_Click;

            mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
            btnDelete.Click += btnDelete_OnClick;

            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }
        }

        void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
        }

        #endregion

        #region Fill

        void ClearForm(object arg)
        {

        }

        void FillFormControls(object arg)
        {

        }

        void SetFormControlsDefaults(object arg)
        {
            if (!_idTask.HasValue) return;
            // Set Delete button visibility rules
            btnDelete.Visible = _taskOperations.AbleToDeleteEntity(_idTask.Value);

            BindDynamicControls(null);
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            if (!_idTask.HasValue) return;

            var task = _taskOperations.GetEntity(_idTask);
            if (task == null || !task.task_PK.HasValue) return;

            var taskName = task.task_name_FK.HasValue ? _taskNameOperations.GetEntity(task.task_name_FK) : null;

            // Entity
            if (EntityContext == EntityContext.Task) lblPrvParentEntity.Text = taskName != null ? taskName.task_name : Constant.ControlDefault.LbPrvText;

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------

            // Task name
            lblPrvTaskName.Text = taskName != null ? taskName.task_name : Constant.ControlDefault.LbPrvText;

            // Task Id
            lblPrvTaskId.Text = !string.IsNullOrWhiteSpace(task.task_ID) ? task.task_ID : Constant.ControlDefault.LbPrvText;

            // Description
            lblPrvDescription.Text = !string.IsNullOrWhiteSpace(task.description) ? task.description : Constant.ControlDefault.LbPrvText;

            // Responsible user
            var responsibleUser = task.user_FK.HasValue ? _personOperations.GetEntity(task.user_FK) : null;
            lblPrvResponsibleUser.Text = responsibleUser != null ? responsibleUser.FullName : Constant.ControlDefault.LbPrvText;

            // Internal status
            var internalStatusType = task.type_internal_status_FK.HasValue ? _typeOperations.GetEntity(task.type_internal_status_FK) : null;
            lblPrvInternalStatus.Text = internalStatusType != null ? internalStatusType.name : Constant.ControlDefault.LbPrvText;

            // Country
            BindCountries(task.task_PK.Value);

            // Comment
            lblPrvComment.Text = !string.IsNullOrWhiteSpace(task.comment) ? task.comment : Constant.ControlDefault.LbPrvText;

            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------

            // Start date
            lblPrvStartDate.Text = task.start_date.HasValue ? task.start_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Expected finished date
            lblPrvExpectedFinishedDate.Text = task.expected_finished_date.HasValue ? task.expected_finished_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Actual finished date
            lblPrvActualFinishedDate.Text = task.actual_finished_date.HasValue ? task.actual_finished_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Automatic alerts
            lblPrvAutomaticAlerts.Text = task.automatic_alerts_on ? "Yes" : "No";

            // Last change
            lblPrvLastChange.Text = LastChange.GetFormattedString(task.task_PK, "TASK", _lastChangeOperations, _personOperations);

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = task.user_FK == user.Person_FK;
        }

        private void BindCountries(int taskPk)
        {
            var countryList = _countryOperations.GetAssignedEntitiesByTask(taskPk);
            var countryAbbrevationList = countryList.Select(country => country.abbreviation).ToList();
            countryAbbrevationList.ForEach(item => StringOperations.ReplaceNullOrWhiteSpace(item, Constant.UnknownValue));

            lblPrvCountry.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", countryAbbrevationList), Constant.ControlDefault.LbPrvText);
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idTask.HasValue) return;

            var task = _taskOperations.GetEntity(_idTask);
            int? activityPk = task != null ? task.activity_FK : null;

            // Activity
            BindActivity(activityPk);

            BindFooterButtons();

            if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) BindParentEntityActivity();
            else if (EntityContext == EntityContext.SubmissionUnit) BindParentEntitySubmissionUnit();

            RefreshReminderStatus();
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

        private void BindFooterButtons()
        {
            if (SecurityHelper.IsPermitted(Permission.InsertDocument))
            {
                btnAddDocument.PostBackUrl = string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idTask={1}&From=TaskPreview", EntityContext.Task, _idTask);
            }
            else
            {
                btnAddDocument.Disable();
            }

            if (SecurityHelper.IsPermitted(Permission.InsertSubmissionUnit))
            {
                btnAddSubmissionUnit.PostBackUrl = string.Format("~/Views/SubmissionUnitView/Form.aspx?EntityContext={0}&Action=New&idTask={1}&From=TaskPreview", EntityContext.Task, _idTask);
            }
            else
            {
                btnAddSubmissionUnit.Disable();
            }
        }

        private void BindActivity(int? activityPk)
        {
            var activity = activityPk.HasValue ? _activityOperations.GetEntity(activityPk.Value) : null;
            if (activity != null && activity.activity_PK.HasValue)
            {
                lblPrvActivity.ShowLinks = true;

                var hlActivity = new HyperLink
                {
                    ID = string.Format("Activity_{0}", activityPk),
                    NavigateUrl = string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext.Activity, activityPk),
                    Text = StringOperations.ReplaceNullOrWhiteSpace(activity.name, Constant.UnknownValue)
                };

                lblPrvActivity.PnlLinks.Controls.Add(hlActivity);
            }
            else
            {
                lblPrvActivity.Text = Constant.ControlDefault.LbPrvText;
            }
        }

        #endregion

        #region Validate

        void ValidateForm(object arg)
        {

        }

        #endregion

        #region Save

        void SaveForm(object arg)
        {

        }

        #endregion

        #region Delete

        void DeleteEntity(int? entityPk)
        {
            if (entityPk.HasValue)
            {
                try
                {
                    _taskOperations.Delete(entityPk);
                    Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext.Task));
                }
                catch { }

            }

            MasterPage.ModalPopup.ShowModalPopup("Error!", "Could not delete entity! Contact your system administrator.");
        }

        #endregion

        #endregion

        #region Event handlers

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.Back:
                    {
                        var query = Request.QueryString["idLay"] != null ? string.Format("&idLay={0}", Request.QueryString["idLay"]) : null;

                        if (EntityContext == EntityContext.Task) Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}{1}", EntityContext, query));
                        else if (EntityContext == EntityContext.SubmissionUnit) Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}", EntityContext));
                        Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext.Task));
                    }
                    break;

                case ContextMenuEventTypes.Edit:
                    {
                        if (EntityContext == EntityContext.Task && _idTask.HasValue) Response.Redirect(string.Format("~/Views/TaskView/Form.aspx?EntityContext={0}&Action=Edit&idTask={1}&From=TaskPreview", EntityContext, _idTask));
                        else if (EntityContext == EntityContext.SubmissionUnit && _idTask.HasValue && _idSubUnit.HasValue) Response.Redirect(string.Format("~/Views/TaskView/Form.aspx?EntityContext={0}&Action=Edit&idTask={1}&idSubUnit={2}&From=SubUnitTaskPreview", EntityContext, _idTask, _idSubUnit));
                        Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext.Task));
                    }
                    break;

                case ContextMenuEventTypes.SaveAs:
                    {
                        if (EntityContext == EntityContext.Task && _idTask.HasValue) Response.Redirect(string.Format("~/Views/TaskView/Form.aspx?EntityContext={0}&Action=SaveAs&idTask={1}&From=TaskPreview", EntityContext, _idTask));
                        else if (EntityContext == EntityContext.SubmissionUnit && _idTask.HasValue && _idSubUnit.HasValue) Response.Redirect(string.Format("~/Views/TaskView/Form.aspx?EntityContext={0}&Action=SaveAs&idTask={1}&idSubUnit={2}&From=SubUnitTaskPreview", EntityContext, _idTask, _idSubUnit));
                        Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext.Task));
                    }
                    break;
            }
        }

        #endregion

        #region Reminders

        private void LblPrvStartdateLnkSetReminder_OnClick(object sender, EventArgs e)
        {
            SetReminder(StringOperations.GetRelatedName(lblPrvStartDate.Label), lblPrvStartDate.Text);
        }

        public void LblPrvExpectedFinishedDateLnkSetReminder_OnClick(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(lblPrvExpectedFinishedDate.Label)), lblPrvExpectedFinishedDate.Text);
        }

        public void Reminder_OnConfirmInputButtonProcess_Click(object sender, EventArgs e)
        {
            Reminder_PK reminder = Reminder.ReminderVs;

            reminder.user_FK = SessionManager.Instance.CurrentUser.UserID;

            reminder.navigate_url = string.Format("~/Views/TaskView/Preview.aspx?EntityContext={0}&idTask={1}", EntityContext.Task, _idTask);
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

        #region Delete

        private void btnDelete_OnClick(object sender, EventArgs eventArgs)
        {
            mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_idTask);
        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContexMenuItems()
        {
            var contextMenu = new[]
            {
                new ContextMenuItem(ContextMenuEventTypes.Back, "Back"), 
                new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit"), 
                new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As")
            };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location = null;
            if (EntityContext == EntityContext.SubmissionUnit)
            {
                location = Support.LocationManager.Instance.GetLocationByName("SubUnitTaskPreview", Support.CacheManager.Instance.AppLocations);
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
                tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, MasterPage.CurrentLocation);
                tabMenu.SelectItem(MasterPage.CurrentLocation, Support.CacheManager.Instance.AppLocations);
            }
        }

        public void RefreshReminderStatus()
        {
            string tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.TASK);
            AlerterHelper.RefreshReminderStatus(_reminderOperations, MasterPage, new List<IReminderCustomControl> { lblPrvStartDate, lblPrvExpectedFinishedDate }, tableName, _idTask);
        }

        public void SetReminder(String attributeName, String attributeValue)
        {
            var reminder = new Reminder_PK
            {
                reminder_type = "Task",
                reminder_name = lblPrvTaskName.Text,
                related_attribute_name = attributeName,
                related_attribute_value = attributeValue
            };

            Reminder.ReminderVs = reminder;
            Reminder.ShowModalPopup("Set alert");
            RefreshReminderStatus();
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            if (!base.SecurityPageSpecific())
            {
                if (SecurityHelper.IsPermitted(Permission.SaveAsTask)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });

                if (SecurityHelper.IsPermitted(Permission.EditTask)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });

                if (SecurityHelper.IsPermitted(Permission.DeleteTask)) StyleHelper.EnableLinkButtonsWithCssClass(PnlFooter, "Delete");
                else StyleHelper.DisableLinkButtonsWithCssClass(PnlFooter, "Delete");

                SecurityPageSpecificMy(_isResponsibleUser);
            }

            return true;
        }

        #endregion
    }
}