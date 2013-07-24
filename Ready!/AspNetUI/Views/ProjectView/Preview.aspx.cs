using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Interface;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Views.ProjectView
{
    public partial class Preview : PreviewPage
    {
        #region Declarations

        private int? _idProj;
        private bool? _isResponsibleUser;

        private IProject_PKOperations _projectOperations;
        private IType_PKOperations _typeOperations;
        private ICountry_PKOperations _countryOperations;
        private IPerson_PKOperations _personOperations;
        private IReminder_PKOperations _reminderOperations;
        private IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations;
        private IReminder_date_PKOperations _reminderDateOperations;
        private ILast_change_PKOperations _lastChangeOperations;
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

            _idProj = ValidationHelper.IsValidInt(Request.QueryString["idProj"]) ? int.Parse(Request.QueryString["idProj"]) : (int?)null;

            _projectOperations = new Project_PKDAL();
            _typeOperations = new Type_PKDAL();
            _countryOperations = new Country_PKDAL();
            _personOperations = new Person_PKDAL();
            _reminderOperations = new Reminder_PKDAL();
            _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            _reminderDateOperations = new Reminder_date_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _userOperations = new USERDAL();
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
            if (!_idProj.HasValue) return;

            // Set Delete button visibility rules
            btnDelete.Visible = _projectOperations.AbleToDeleteEntity(_idProj.Value);

            BindDynamicControls(null);
        }


        #endregion

        #region Bind

        void BindForm(object arg)
        {
            if (!_idProj.HasValue) return;

            var project = _projectOperations.GetEntity(_idProj);
            if (project == null || !project.project_PK.HasValue) return;

            // Entity
            lblPrvProject.Text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.ControlDefault.LbPrvText;

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------

            // Project name
            lblPrvName.Text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.ControlDefault.LbPrvText;

            // Description
            lblPrvDescription.Text = !string.IsNullOrWhiteSpace(project.description) ? project.description : Constant.ControlDefault.LbPrvText;

            // Responsible user
            var responsibleUser = project.user_FK.HasValue ? _personOperations.GetEntity(project.user_FK) : null;
            lblPrvResponsibleUser.Text = responsibleUser != null ? responsibleUser.FullName : Constant.ControlDefault.LbPrvText;

            // Internal status
            var internalStatusType = project.internal_status_type_FK.HasValue ? _typeOperations.GetEntity(project.internal_status_type_FK) : null;
            lblPrvInternalStatus.Text = internalStatusType != null ? internalStatusType.name : Constant.ControlDefault.LbPrvText;

            // Country
            BindCountries(project.project_PK.Value);

            // Comment
            lblPrvComment.Text = !string.IsNullOrWhiteSpace(project.comment) ? project.comment : Constant.ControlDefault.LbPrvText;

            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------

            // Start date
            lblPrvStartDate.Text = project.start_date.HasValue ? project.start_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Expected finished date
            lblPrvExpectedFinishedDate.Text = project.expected_finished_date.HasValue ? project.expected_finished_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Actual finished date
            lblPrvActualFinishedDate.Text = project.actual_finished_date.HasValue ? project.actual_finished_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Automatic alerts
            lblPrvAutomaticAlerts.Text = project.automatic_alerts_on ? "Yes" : "No";

            // Last change
            lblPrvLastChange.Text = LastChange.GetFormattedString(project.project_PK, "PROJECT", _lastChangeOperations, _personOperations);

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = project.user_FK == user.Person_FK;
        }

        private void BindCountries(int taskPk)
        {
            var countryList = _countryOperations.GetAssignedEntitiesByProject(taskPk);
            countryList.SortByField(x => x.custom_sort_ID);
            var countryAbbrevationList = countryList.Select(country => country.abbreviation).ToList();
            countryAbbrevationList.ForEach(item => StringOperations.ReplaceNullOrWhiteSpace(item, Constant.UnknownValue));

            lblPrvCountry.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", countryAbbrevationList), Constant.ControlDefault.LbPrvText);
        }

        private void BindDynamicControls(object arg)
        {
            BindFooterButtons();

            RefreshReminderStatus();
        }

        private void BindFooterButtons()
        {
            if (SecurityHelper.IsPermitted(Permission.InsertDocument)) btnAddDocument.PostBackUrl = string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idProj={1}&From=ProjPreview", EntityContext, _idProj);
            else btnAddDocument.Disable();

            if (SecurityHelper.IsPermitted(Permission.InsertActivity)) btnAddActivity.PostBackUrl = string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=New&idProj={1}&From=ProjPreview", EntityContext, _idProj);
            else btnAddActivity.Disable();
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
                    _projectOperations.Delete(entityPk);
                    Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext.Project));
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

                        if (EntityContext == EntityContext.Project) Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}{1}", EntityContext, query));
                        Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext.Project));
                    }
                    break;

                case ContextMenuEventTypes.Edit:
                    {
                        if (EntityContext == EntityContext.Project && _idProj.HasValue) Response.Redirect(string.Format("~/Views/ProjectView/Form.aspx?EntityContext={0}&Action=Edit&idProj={1}&From=ProjPreview", EntityContext, _idProj));
                        Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext.Project));
                    }
                    break;

                case ContextMenuEventTypes.SaveAs:
                    {
                        if (EntityContext == EntityContext.Project && _idProj.HasValue) Response.Redirect(string.Format("~/Views/ProjectView/Form.aspx?EntityContext={0}&Action=SaveAs&idProj={1}&From=ProjPreview", EntityContext, _idProj));
                        Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext.Project));
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

            reminder.navigate_url = string.Format("~/Views/ProjectView/Preview.aspx?EntityContext={0}&idProj={1}", EntityContext.Project, _idProj);
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

        #region Delete

        private void btnDelete_OnClick(object sender, EventArgs eventArgs)
        {
            mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_idProj);
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
            tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, MasterPage.CurrentLocation);
            tabMenu.SelectItem(MasterPage.CurrentLocation, Support.CacheManager.Instance.AppLocations);
        }

        public void RefreshReminderStatus()
        {
            string tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.PROJECT);
            AlerterHelper.RefreshReminderStatus(_reminderOperations, MasterPage, new List<IReminderCustomControl> { lblPrvStartDate, lblPrvExpectedFinishedDate }, tableName, _idProj);
        }

        public void SetReminder(String attributeName, String attributeValue)
        {
            var reminder = new Reminder_PK
            {
                reminder_type = "Project",
                reminder_name = lblPrvName.Text,
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
                if (SecurityHelper.IsPermitted(Permission.SaveAsProject)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });

                if (SecurityHelper.IsPermitted(Permission.EditProject)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });

                if (SecurityHelper.IsPermitted(Permission.DeleteProject)) StyleHelper.EnableLinkButtonsWithCssClass(PnlFooter, "Delete");
                else StyleHelper.DisableLinkButtonsWithCssClass(PnlFooter, "Delete");

                SecurityPageSpecificMy(_isResponsibleUser);
            }

            return true;
        }

        #endregion
    }
}