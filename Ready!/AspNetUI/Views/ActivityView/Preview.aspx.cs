using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Interface;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Views.ActivityView
{
    public partial class Preview : PreviewPage
    {
        #region Declarations

        private int? _idAct;
        private int? _idSubUnit;
        private int? _idProj;
        private int? _idTimeUnit;
        private bool? _isResponsibleUser;

        private IActivity_PKOperations _activityOperations;
        private IProduct_PKOperations _productOperations;
        private IProject_PKOperations _projectOperations;
        private IType_PKOperations _typeOperations;
        private IOrganization_PKOperations _organizationOperations;
        private ICountry_PKOperations _countryOperations;
        private IPerson_PKOperations _personOperations;
        private IReminder_PKOperations _reminderOperations;
        private IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations;
        private IReminder_date_PKOperations _reminderDateOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private ISubbmission_unit_PKOperations _submissionUnitOperations;
        private ITask_PKOperations _taskOperations;
        private ITime_unit_PKOperations _timeUnitOperations;
        private ITime_unit_name_PKOperations _timeUnitNameOperations;
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

            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;
            _idSubUnit = ValidationHelper.IsValidInt(Request.QueryString["idSubUnit"]) ? int.Parse(Request.QueryString["idSubUnit"]) : (int?)null;
            _idProj = ValidationHelper.IsValidInt(Request.QueryString["idProj"]) ? int.Parse(Request.QueryString["idProj"]) : (int?)null;
            _idTimeUnit = ValidationHelper.IsValidInt(Request.QueryString["idTimeUnit"]) ? int.Parse(Request.QueryString["idTimeUnit"]) : (int?)null;

            _activityOperations = new Activity_PKDAL();
            _productOperations = new Product_PKDAL();
            _projectOperations = new Project_PKDAL();
            _typeOperations = new Type_PKDAL();
            _organizationOperations = new Organization_PKDAL();
            _countryOperations = new Country_PKDAL();
            _personOperations = new Person_PKDAL();
            _reminderOperations = new Reminder_PKDAL();
            _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            _reminderDateOperations = new Reminder_date_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _submissionUnitOperations = new Subbmission_unit_PKDAL();
            _taskOperations = new Task_PKDAL();
            _timeUnitOperations = new Time_unit_PKDAL();
            _timeUnitNameOperations = new Time_unit_name_PKDAL();
            _userOperations = new USERDAL();

            if (!_idAct.HasValue)
            {
                if (_idSubUnit.HasValue)
                {
                    var submissionUnit = _submissionUnitOperations.GetEntity(_idSubUnit);
                    if (submissionUnit != null)
                    {
                        var task = _taskOperations.GetEntity(submissionUnit.task_FK);
                        if (task != null) _idAct = task.activity_FK;
                    }
                }
                else if (_idTimeUnit.HasValue)
                {
                    var timeUnit = _timeUnitOperations.GetEntity(_idTimeUnit);
                    if (timeUnit != null)
                    {
                        _idAct = timeUnit.activity_FK;
                    }
                }
            }
        }

        private void BindEventHandlers()
        {
            lblPrvStartDate.LnkSetReminder.Click += LblPrvStartdateLnkSetReminder_OnClick;
            lblPrvExpectedFinishedDate.LnkSetReminder.Click += LblPrvExpectedFinishedDateLnkSetReminder_OnClick;
            lblPrvSubmissionDate.LnkSetReminder.Click += LblPrvSubmissionDateLnkSetReminder_OnClick;
            lblPrvApprovalDate.LnkSetReminder.Click += LblPrvApprovalDateLnkSetReminder_OnClick;

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
            if (!_idAct.HasValue) return;

            // Set Delete button visibility rules
            btnDelete.Visible = _activityOperations.AbleToDeleteEntity(_idAct.Value);

            BindDynamicControls(null);
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            if (!_idAct.HasValue) return;

            var activity = _activityOperations.GetEntity(_idAct);
            if (activity == null || !activity.activity_PK.HasValue) return;

            // Entity
            if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) lblPrvParentEntity.Text = !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.ControlDefault.LbPrvText;

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------

            // Name
            lblPrvName.Text = !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.ControlDefault.LbPrvText;

            // Activity ID
            lblPrvActivityId.Text = !string.IsNullOrWhiteSpace(activity.activity_ID) ? activity.activity_ID : Constant.ControlDefault.LbPrvText;

            // Description
            lblPrvDescription.Text = !string.IsNullOrWhiteSpace(activity.description) ? activity.description : Constant.ControlDefault.LbPrvText;

            // Responsible user
            var responsibleUser = activity.user_FK.HasValue ? _personOperations.GetEntity(activity.user_FK) : null;
            lblPrvResponsibleUser.Text = responsibleUser != null ? responsibleUser.FullName : Constant.ControlDefault.LbPrvText;

            // Procedure number
            lblPrvProcedureNumber.Text = !string.IsNullOrWhiteSpace(activity.procedure_number) ? activity.procedure_number : Constant.ControlDefault.LbPrvText;

            // Procedure type
            var procedureType = activity.procedure_type_FK.HasValue ? _typeOperations.GetEntity(activity.procedure_type_FK) : null;
            lblPrvProcedureType.Text = procedureType != null ? procedureType.name : Constant.ControlDefault.LbPrvText;

            // Types
            BindTypes(activity.activity_PK.Value);

            // Regulatory status
            var regulatoryStatusType = activity.regulatory_status_FK.HasValue ? _typeOperations.GetEntity(activity.regulatory_status_FK) : null;
            lblPrvRegulatoryStatus.Text = regulatoryStatusType != null ? regulatoryStatusType.name : Constant.ControlDefault.LbPrvText;

            // Internal status
            var internalStatusType = activity.internal_status_FK.HasValue ? _typeOperations.GetEntity(activity.internal_status_FK) : null;
            lblPrvInternalStatus.Text = internalStatusType != null ? internalStatusType.name : Constant.ControlDefault.LbPrvText;

            // Activity mode
            var activityModeType = activity.mode_FK.HasValue ? _typeOperations.GetEntity(activity.mode_FK) : null;
            lblPrvActivityMode.Text = activityModeType != null ? activityModeType.name : Constant.ControlDefault.LbPrvText;

            // Applicant
            BindApplicant(activity.activity_PK.Value);

            // Countries
            BindCountries(activity.activity_PK.Value);

            // Legal basis of application
            lblPrvLegalBasisOfApplication.Text = !string.IsNullOrWhiteSpace(activity.legal) ? activity.legal : Constant.ControlDefault.LbPrvText;

            // Comment
            lblPrvComment.Text = !string.IsNullOrWhiteSpace(activity.comment) ? activity.comment : Constant.ControlDefault.LbPrvText;

            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------

            // Start date
            lblPrvStartDate.Text = activity.start_date.HasValue ? activity.start_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Expected finished date
            lblPrvExpectedFinishedDate.Text = activity.expected_finished_date.HasValue ? activity.expected_finished_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Actual finished date
            lblPrvActualFinishedDate.Text = activity.actual_finished_date.HasValue ? activity.actual_finished_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Submission date
            lblPrvSubmissionDate.Text = activity.submission_date.HasValue ? activity.submission_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Approval date
            lblPrvApprovalDate.Text = activity.approval_date.HasValue ? activity.approval_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Billable
            lblPrvBillable.Text = activity.billable.HasValue && activity.billable.Value ? "Yes" : "No";

            // Automatic alerts
            lblPrvAutomaticAlerts.Text = activity.automatic_alerts_on ? "Yes" : "No";

            // Last change
            lblPrvLastChange.Text = LastChange.GetFormattedString(activity.activity_PK, "ACTIVITY", _lastChangeOperations, _personOperations);

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = activity.user_FK == user.Person_FK;
        }

        private void BindCountries(int activityPk)
        {
            var countryList = _countryOperations.GetAssignedEntitiesByActivity(activityPk);
            countryList.SortByField(x => x.custom_sort_ID);
            var countryAbbrevationList = countryList.Select(country => country.abbreviation).ToList();
            countryAbbrevationList.ForEach(item => StringOperations.ReplaceNullOrWhiteSpace(item, Constant.UnknownValue));

            lblPrvCountries.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", countryAbbrevationList), Constant.ControlDefault.LbPrvText);
        }

        private void BindTypes(int activityPk)
        {
            var typeList = _typeOperations.GetAssignedTypesForActivity(activityPk);
            var typeNameList = typeList.Select(x => x.name).ToList();
            typeNameList.Sort();
            typeNameList.ForEach(item => StringOperations.ReplaceNullOrWhiteSpace(item, Constant.UnknownValue));
            lblPrvTypes.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", typeNameList), Constant.ControlDefault.LbPrvText);
        }

        private void BindApplicant(int activityPk)
        {
            var applicantList = _organizationOperations.GetAssignedApplicantsForActivity(activityPk);
            var applicantNameList = applicantList.Select(x => x.name_org).ToList();
            applicantNameList.Sort();
            applicantNameList.ForEach(item => StringOperations.ReplaceNullOrWhiteSpace(item, Constant.UnknownValue));
            lblPrvApplicant.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", applicantNameList), Constant.ControlDefault.LbPrvText);
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idAct.HasValue) return;

            // Products
            BindProducts(_idAct.Value);

            // Projects
            BindProjects(_idAct.Value);

            BindFooterButtons();

            // Entity
            if (EntityContext == EntityContext.Project) BindParentEntityProject();
            else if (EntityContext == EntityContext.SubmissionUnit) BindParentEntitySubmissionUnit();
            else if (EntityContext == EntityContext.TimeUnit || EntityContext == EntityContext.TimeUnitMy) BindParentEntityTimeUnit();

            RefreshReminderStatus();
        }

        private void BindParentEntityProject(int? idProj = null)
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Project:";

            var project = idProj.HasValue ? _projectOperations.GetEntity(idProj) : _idProj.HasValue ? _projectOperations.GetEntity(_idProj) : null;
            if (project == null)
            {
                lblPrvParentEntity.Text = Constant.ControlDefault.LbPrvText;
                return;
            }

            lblPrvParentEntity.Text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.ControlDefault.LbPrvText;
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

        private void BindParentEntityTimeUnit(int? idTimeUnit = null)
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Time unit:";

            var timeUnit = idTimeUnit.HasValue ? _timeUnitOperations.GetEntity(idTimeUnit) : _idTimeUnit.HasValue ? _timeUnitOperations.GetEntity(_idTimeUnit) : null;
            if (timeUnit == null)
            {
                lblPrvParentEntity.Text = Constant.ControlDefault.LbPrvText;
                return;
            }

            var timeUnitName = timeUnit.time_unit_name_FK.HasValue ? _timeUnitNameOperations.GetEntity(timeUnit.time_unit_name_FK) : null;
            lblPrvParentEntity.Text = timeUnitName != null && !string.IsNullOrWhiteSpace(timeUnitName.time_unit_name) ? timeUnitName.time_unit_name : Constant.ControlDefault.LbPrvText;
        }

        private void BindFooterButtons()
        {
            var fromQueryPart = "&From=";
            var idQueryPart = string.Empty;
            if (EntityContext == EntityContext.Activity) fromQueryPart += "ActPreview";
            else if (EntityContext == EntityContext.ActivityMy) fromQueryPart += "ActMyPreview";
            else if (EntityContext == EntityContext.SubmissionUnit)
            {
                if(_idSubUnit.HasValue) idQueryPart = string.Format("&idSubUnit={0}", _idSubUnit);
                fromQueryPart += "SubUnitActPreview";
            }
            else if (EntityContext == EntityContext.TimeUnitMy)
            {
                if (_idTimeUnit.HasValue) idQueryPart = string.Format("&idTimeUnit={0}", _idTimeUnit);
                fromQueryPart += "TimeUnitMyActPreview";
            }
            else if (EntityContext == EntityContext.TimeUnit)
            {
                if (_idTimeUnit.HasValue) idQueryPart = string.Format("&idTimeUnit={0}", _idTimeUnit);
                fromQueryPart += "TimeUnitActPreview";
            }

            if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
            {
                if (SecurityHelper.IsPermitted(Permission.InsertDocument))
                {
                    btnAddDocument.PostBackUrl = string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idAct={1}{2}", EntityContext, _idAct, fromQueryPart);
                }
                else btnAddDocument.Disable();
            } 
            else if (EntityContext == EntityContext.SubmissionUnit || EntityContext == EntityContext.TimeUnit || EntityContext == EntityContext.TimeUnitMy)
            {
                if (SecurityHelper.IsPermitted(Permission.InsertDocument))
                {
                    btnAddDocument.PostBackUrl = string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idAct={1}{2}{3}", EntityContext.Activity, _idAct, fromQueryPart, idQueryPart);
                }
                else btnAddDocument.Disable();
            }

            if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
            {
                if (SecurityHelper.IsPermitted(Permission.InsertTask)) btnAddTask.PostBackUrl = string.Format("~/Views/TaskView/Form.aspx?EntityContext={0}&Action=New&idAct={1}{2}", EntityContext, _idAct, fromQueryPart);
                else btnAddTask.Disable();
            } 
            else if (EntityContext == EntityContext.SubmissionUnit ||EntityContext == EntityContext.TimeUnit || EntityContext == EntityContext.TimeUnitMy)
            {
                if (SecurityHelper.IsPermitted(Permission.InsertTask))
                {
                    btnAddTask.PostBackUrl = string.Format("~/Views/TaskView/Form.aspx?EntityContext={0}&Action=New&idAct={1}{2}{3}", EntityContext.Activity, _idAct, fromQueryPart, idQueryPart);
                }
                else btnAddTask.Disable();
            }

            if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
            {
                if (SecurityHelper.IsPermitted(Permission.InsertTimeUnit))
                {
                    btnAddTimeUnit.PostBackUrl = string.Format("~/Views/TimeUnitView/Form.aspx?EntityContext={0}&Action=New&idAct={1}{2}", EntityContext, _idAct, fromQueryPart);
                }
                else btnAddTimeUnit.Disable(); 
            } 
            else if (EntityContext == EntityContext.SubmissionUnit || EntityContext == EntityContext.TimeUnit || EntityContext == EntityContext.TimeUnitMy)
            {
                if (SecurityHelper.IsPermitted(Permission.InsertTimeUnit))
                {
                    btnAddTimeUnit.PostBackUrl = string.Format("~/Views/TimeUnitView/Form.aspx?EntityContext={0}&Action=New&idAct={1}{2}{3}", EntityContext.Activity, _idAct, fromQueryPart, idQueryPart);
                }
                else btnAddTimeUnit.Disable();
            }
        }

        private void BindProducts(int activityPk)
        {
            lblPrvProducts.Text = Constant.ControlDefault.LbPrvText;

            var productList = _productOperations.GetAssignedEntitiesByActivity(activityPk);

            if (productList == null || productList.Count == 0) return;

            lblPrvProducts.ShowLinks = true;
            lblPrvProducts.TextBold = true;
            lblPrvProducts.PnlLinks.Width = Unit.Pixel(800);

            foreach (var product in productList)
            {
                lblPrvProducts.PnlLinks.Controls.Add(new HyperLink
                {
                    ID = string.Format("Product_{0}", product.product_PK),
                    NavigateUrl = string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, product.product_PK),
                    Text = StringOperations.ReplaceNullOrWhiteSpace(product.name, Constant.UnknownValue)
                });
                lblPrvProducts.PnlLinks.Controls.Add(new LiteralControl("<br/>"));
            }
        }

        private void BindProjects(int activityPk)
        {
            lblPrvProducts.Text = Constant.ControlDefault.LbPrvText;

            var projectList = _projectOperations.GetAssignedEntitiesByActivity(activityPk);

            if (projectList == null || projectList.Count == 0) return;

            lblPrvProjects.ShowLinks = true;
            lblPrvProjects.TextBold = true;
            lblPrvProjects.PnlLinks.Width = Unit.Pixel(800);

            foreach (var project in projectList)
            {
                lblPrvProjects.PnlLinks.Controls.Add(new HyperLink
                {
                    ID = string.Format("Project_{0}", project.project_PK),
                    NavigateUrl = string.Format("~/Views/ProjectView/Preview.aspx?EntityContext={0}&idProj={1}", EntityContext.Project, project.project_PK),
                    Text = StringOperations.ReplaceNullOrWhiteSpace(project.name, Constant.UnknownValue)
                });
                lblPrvProjects.PnlLinks.Controls.Add(new LiteralControl("<br/>"));
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
                    _activityOperations.Delete(entityPk);
                    Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
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

                        if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}{1}", EntityContext, query));
                        else if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Project) Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.SubmissionUnit) Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.TimeUnit ||EntityContext == EntityContext.TimeUnitMy) Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext));
                        Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext.Activity));
                    }
                    break;

                case ContextMenuEventTypes.Edit:
                    {
                        if (EntityContext == EntityContext.Activity && _idAct.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=Edit&idAct={1}&From=ActPreview", EntityContext, _idAct));
                        else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=Edit&idAct={1}&From=ActMyPreview", EntityContext, _idAct));
                        else if (EntityContext == EntityContext.SubmissionUnit && _idAct.HasValue && _idSubUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=Edit&idAct={1}&idSubUnit={2}&From=SubUnitActPreview", EntityContext, _idAct, _idSubUnit));
                        else if (EntityContext == EntityContext.TimeUnit && _idAct.HasValue && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=Edit&idAct={1}&idTimeUnit={2}&From=TimeUnitActPreview", EntityContext, _idAct, _idTimeUnit));
                        else if (EntityContext == EntityContext.TimeUnitMy && _idAct.HasValue && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=Edit&idAct={1}&idTimeUnit={2}&From=TimeUnitMyActPreview", EntityContext, _idAct, _idTimeUnit));
                        Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext.Activity));
                    }
                    break;

                case ContextMenuEventTypes.SaveAs:
                    {
                        if (EntityContext == EntityContext.Activity && _idAct.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=SaveAs&idAct={1}&From=ActPreview", EntityContext, _idAct));
                        else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=SaveAs&idAct={1}&From=ActMyPreview", EntityContext, _idAct));
                        else if (EntityContext == EntityContext.SubmissionUnit && _idAct.HasValue && _idSubUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=SaveAs&idAct={1}&idSubUnit={2}&From=SubUnitActPreview", EntityContext, _idAct, _idSubUnit));
                        else if (EntityContext == EntityContext.TimeUnit && _idAct.HasValue && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=SaveAs&idAct={1}&idTimeUnit={2}&From=TimeUnitActPreview", EntityContext, _idAct, _idTimeUnit));
                        else if (EntityContext == EntityContext.TimeUnitMy && _idAct.HasValue && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=SaveAs&idAct={1}&idTimeUnit={2}&From=TimeUnitMyActPreview", EntityContext, _idAct, _idTimeUnit));
                        Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext.Activity));
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

        private void LblPrvSubmissionDateLnkSetReminder_OnClick(object sender, EventArgs eventArgs)
        {
            SetReminder(StringOperations.GetRelatedName(lblPrvSubmissionDate.Label), lblPrvSubmissionDate.Text);
        }

        private void LblPrvApprovalDateLnkSetReminder_OnClick(object sender, EventArgs eventArgs)
        {
            SetReminder(StringOperations.GetRelatedName(lblPrvApprovalDate.Label), lblPrvApprovalDate.Text);
        }

        public void Reminder_OnConfirmInputButtonProcess_Click(object sender, EventArgs e)
        {
            Reminder_PK reminder = Reminder.ReminderVs;

            reminder.user_FK = SessionManager.Instance.CurrentUser.UserID;

            reminder.navigate_url = string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext.Activity, _idAct);
            reminder.TableName = ReminderTableName.ACTIVITY;
            reminder.entity_FK = _idAct;

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
            DeleteEntity(_idAct);
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
            if (EntityContext == EntityContext.SubmissionUnit) location = Support.LocationManager.Instance.GetLocationByName("SubUnitActPreview", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.TimeUnit) location = Support.LocationManager.Instance.GetLocationByName("TimeUnitActPreview", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.TimeUnitMy) location = Support.LocationManager.Instance.GetLocationByName("TimeUnitMyActPreview", Support.CacheManager.Instance.AppLocations);
            
            MasterPage.TabMenu.TabControls.Clear();
            tabMenu.Visible = true;
            if (location != null)
            {
                tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                return;
            }
            
            tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, MasterPage.CurrentLocation);
            tabMenu.SelectItem(MasterPage.CurrentLocation, Support.CacheManager.Instance.AppLocations);
        }

        public void RefreshReminderStatus()
        {
            string tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.ACTIVITY);
            AlerterHelper.RefreshReminderStatus(_reminderOperations, MasterPage, new List<IReminderCustomControl> { lblPrvStartDate, lblPrvExpectedFinishedDate, lblPrvSubmissionDate, lblPrvApprovalDate }, tableName, _idAct);
        }

        public void SetReminder(String attributeName, String attributeValue)
        {
            var reminder = new Reminder_PK
            {
                reminder_type = "Activity",
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
                if (SecurityHelper.IsPermitted(Permission.SaveAsActivity)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] {new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As")});
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] {new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As")});

                if (SecurityHelper.IsPermitted(Permission.EditActivity)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] {new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit")});
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] {new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit")});

                if (SecurityHelper.IsPermitted(Permission.DeleteActivity)) StyleHelper.EnableLinkButtonsWithCssClass(PnlFooter, "Delete");
                else StyleHelper.DisableLinkButtonsWithCssClass(PnlFooter, "Delete");

                SecurityPageSpecificMy(_isResponsibleUser);
            }

            return true;
        }

        #endregion
    }
}