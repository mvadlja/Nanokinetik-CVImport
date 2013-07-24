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

namespace AspNetUI.Views.ActivityView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idAct;
        private int? _idProd;
        private int? _idProj;
        private int? _idSubUnit;
        private int? _idTimeUnit;
        private bool? _isResponsibleUser;

        private IActivity_PKOperations _activityOperations;
        private IProduct_PKOperations _productOperations;
        private IProject_PKOperations _projectOperations;
        private IActivity_product_PKOperations _activityProductMnOperations;
        private IActivity_project_PKOperations _activityProjectMnOperations;
        private IActivity_organization_applicant_PKOperations _activityApplicantMnOperations;
        private IActivity_type_PKOperations _activityTypeMnOperations;
        private IActivity_country_PKOperations _activityCountryOperations;
        private IPerson_PKOperations _personOperations;
        private IType_PKOperations _typeOperations;
        private IOrganization_PKOperations _organizationOperations;
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

            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;
            _idProj = ValidationHelper.IsValidInt(Request.QueryString["idProj"]) ? int.Parse(Request.QueryString["idProj"]) : (int?)null;
            _idSubUnit = ValidationHelper.IsValidInt(Request.QueryString["idSubUnit"]) ? int.Parse(Request.QueryString["idSubUnit"]) : (int?)null;
            _idTimeUnit = ValidationHelper.IsValidInt(Request.QueryString["idTimeUnit"]) ? int.Parse(Request.QueryString["idTimeUnit"]) : (int?)null;

            _activityOperations = new Activity_PKDAL();
            _productOperations = new Product_PKDAL();
            _projectOperations = new Project_PKDAL();
            _activityApplicantMnOperations = new Activity_organization_applicant_PKDAL();
            _activityProductMnOperations = new Activity_product_PKDAL();
            _activityProjectMnOperations = new Activity_project_PKDAL();
            _activityTypeMnOperations = new Activity_type_PKDAL();
            _activityCountryOperations = new Activity_country_PKDAL();
            _personOperations = new Person_PKDAL();
            _typeOperations = new Type_PKDAL();
            _organizationOperations = new Organization_PKDAL();
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

            lbSrProducts.Searcher.OnOkButtonClick += LbSrProductsSearcher_OnOkButtonClick;
            lbSrProducts.OnRemoveClick += LbSrProducts_OnRemoveClick;
            lbSrProjects.Searcher.OnOkButtonClick += LbSrProjectsSearcher_OnOkButtonClick;

            dtStartDate.LnkSetReminder.Click += DtStartDateLnkSetReminder_OnClick;
            dtExpectedFinishedDate.LnkSetReminder.Click += DtExpectedFinishedLnkSetReminder_OnClick;
            dtSubmissionDate.LnkSetReminder.Click += DtSubmissionDateLnkSetReminder_OnClick;
            dtApprovalDate.LnkSetReminder.Click += DtApprovalDateLnkSetReminder_OnClick;

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

            // Left pane
            lbSrProducts.Clear();
            lbSrProjects.Clear();
            txtName.Text = String.Empty;
            txtActivityId.Text = String.Empty;
            txtDescription.Text = String.Empty;
            ddlResponsibleUser.Text = String.Empty;
            txtProcedureNumber.Text = String.Empty;
            ddlProcedureType.Text = String.Empty;
            lbAuTypes.Clear();
            ddlRegulatoryStatus.Text = String.Empty;
            ddlInternalStatus.Text = String.Empty;
            ddlActivityMode.Text = String.Empty;
            ddlApplicant.Text = String.Empty;
            lbAuCountries.Clear();
            txtLegalBasisOfApplication.Text = String.Empty;
            txtComment.Text = String.Empty;
            dtStartDate.Text = String.Empty;
            dtExpectedFinishedDate.Text = String.Empty;
            dtActualFinishedDate.Text = String.Empty;
            dtSubmissionDate.Text = String.Empty;
            dtApprovalDate.Text = String.Empty;
            rbYnBillable.SelectedValue = null;
            rbYnAutomaticAlerts.SelectedValue = null;

            // Right pane
        }

        private void FillFormControls(object arg)
        {
            FillDdlResponsibleUser();
            FillDdlProcedureType();
            FillDdlRegulatoryStatus();
            FillDdlInternalStatus();
            FillDdlActivityMode();
            FillDdlApplicant();

            if (FormType == FormType.New)
            {
                FillLbAuTypes();
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
                lblPrvParentEntity.Text = Constant.ControlDefault.LbPrvText;

                if (EntityContext == EntityContext.Product) BindParentEntityProduct(_idProd);
                else if (EntityContext == EntityContext.Project) BindParentEntityProject(_idProj);

                rbYnAutomaticAlerts.SelectedValue = true;
                rbYnBillable.SelectedValue = true;
            }

            BindDynamicControls(null);
        }

        private void BindParentEntityProject(int? idProj)
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Project:";

            var project = _projectOperations.GetEntity(idProj);
            if (project == null)
            {
                lblPrvParentEntity.Text = Constant.ControlDefault.LbPrvText;
                return;
            }

            lblPrvParentEntity.Text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.ControlDefault.LbPrvText;
            if (project.project_PK.HasValue) lbSrProjects.LbInput.Items.Add(new ListItem(lblPrvParentEntity.Text, Convert.ToString(project.project_PK)));
        }

        private void BindParentEntityProduct(int? idProd)
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Product:";

            var product = _productOperations.GetEntity(idProd);

            if (product == null)
            {
                lblPrvParentEntity.Text = Constant.ControlDefault.LbPrvText;
                return;
            }

            lblPrvParentEntity.Text = !string.IsNullOrWhiteSpace(product.name) ? product.name : Constant.ControlDefault.LbPrvText;
            lbSrProducts.LbInput.Items.Add(new ListItem(product.GetNameFormatted(Constant.UnknownValue), Convert.ToString(product.product_PK)));

            AssignCountriesForProduct(product);
            SortCountries(lbAuCountries.LbInputTo);
        }

        private void FillDdlResponsibleUser()
        {
            var responsibleUserList = _personOperations.GetPersonsByRole(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, x => x.FullName, x => x.person_PK);
            ddlResponsibleUser.SortItemsByText();
        }

        private void FillDdlProcedureType()
        {
            var procedureTypeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.AuthorisationProcedure);
            ddlProcedureType.Fill(procedureTypeList, x => x.name, x => x.type_PK);
            ddlProcedureType.SortItemsByText();
        }

        private void FillDdlRegulatoryStatus()
        {
            var regulatoryStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.RegulatoryStatus);
            ddlRegulatoryStatus.Fill(regulatoryStatusList, x => x.name, x => x.type_PK);
            ddlRegulatoryStatus.SortItemsByText();
        }

        private void FillDdlInternalStatus()
        {
            var internalStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.InternalStatus);
            ddlInternalStatus.Fill(internalStatusList, x => x.name, x => x.type_PK);
            ddlInternalStatus.SortItemsByText();
        }

        private void FillDdlActivityMode()
        {
            var activityModeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.ActivityMode);
            ddlActivityMode.Fill(activityModeList, x => x.name, x => x.type_PK);
            ddlActivityMode.SortItemsByText();
        }

        private void FillDdlApplicant()
        {
            var applicantList = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.Applicant);
            ddlApplicant.Fill(applicantList, x => x.name_org, x => x.organization_PK);
            ddlApplicant.SortItemsByText();
        }

        private void FillLbAuTypes()
        {
            var typeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.Types);
            lbAuTypes.LbInputFrom.Fill(typeList, x => x.name, x => x.type_PK);
            lbAuTypes.LbInputFrom.SortItemsByText();
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
            if (!_idAct.HasValue) return;

            var activity = _activityOperations.GetEntity(_idAct.Value);
            if (activity == null || !activity.activity_PK.HasValue) return;

            // Entity
            lblPrvParentEntity.Text = !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.ControlDefault.LbPrvText;

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------

            // Bind products
            BindProducts(activity.activity_PK.Value);

            // Bind projects
            BindProjects(activity.activity_PK.Value);

            // Name
            txtName.Text = activity.name;

            // Activity ID
            txtActivityId.Text = activity.activity_ID;

            // Description
            txtDescription.Text = activity.description;

            // Responsible user
            ddlResponsibleUser.SelectedId = activity.user_FK;

            // Procedure number
            txtProcedureNumber.Text = activity.procedure_number;

            // Procedure type
            ddlProcedureType.SelectedId = activity.procedure_type_FK;

            // Types
            BindTypes(activity.activity_PK.Value);

            // Regulatory status
            ddlRegulatoryStatus.SelectedId = activity.regulatory_status_FK;

            // Internal status
            ddlInternalStatus.SelectedId = activity.internal_status_FK;

            // Activity mode
            ddlActivityMode.SelectedId = activity.mode_FK;

            // Applicant
            BindApplicant(activity.activity_PK.Value);

            // Countries
            BindCountries(activity.activity_PK.Value);

            // Legal basis of application
            txtLegalBasisOfApplication.Text = activity.legal;

            // Comment
            txtComment.Text = activity.comment;

            // Start date
            dtStartDate.Text = activity.start_date.HasValue ? activity.start_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            // Expected finished date
            dtExpectedFinishedDate.Text = activity.expected_finished_date.HasValue ? activity.expected_finished_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            // Actual finished date
            dtActualFinishedDate.Text = activity.actual_finished_date.HasValue ? activity.actual_finished_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            // Submission date
            dtSubmissionDate.Text = activity.submission_date.HasValue ? activity.submission_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            // Approval date
            dtApprovalDate.Text = activity.approval_date.HasValue ? activity.approval_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            // Billable
            rbYnBillable.SelectedValue = activity.billable;

            // Automatic alerts
            rbYnAutomaticAlerts.SelectedValue = activity.automatic_alerts_on;

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = activity.user_FK == user.Person_FK;

            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idAct.HasValue) return;

            RefreshReminderStatus();
        }

        private void BindProducts(int activityPk)
        {
            var productList = _productOperations.GetEntitiesByActivity(activityPk);
            productList.ForEach(x => x.name = x.GetNameFormatted(Constant.UnknownValue));
            lbSrProducts.Fill(productList, x => x.name, x => x.product_PK);
            lbSrProducts.LbInput.SortItemsByText();
        }

        private void BindProjects(int activityPk)
        {
            var projectList = _projectOperations.GetAssignedEntitiesByActivity(activityPk);
            lbSrProjects.Fill(projectList, x => x.name, x => x.project_PK);
            lbSrProjects.LbInput.SortItemsByText();
        }

        private void BindTypes(int activityPk)
        {
            var typeAvailableList = _typeOperations.GetAvailableTypesForActivity(activityPk);
            typeAvailableList.SortByField(x => x.name);
            lbAuTypes.LbInputFrom.Fill(typeAvailableList, x => x.name, x => x.type_PK);

            var typeAssignedList = _typeOperations.GetAssignedTypesForActivity(activityPk);
            typeAssignedList.SortByField(x => x.name);
            lbAuTypes.LbInputTo.Fill(typeAssignedList, x => x.name, x => x.type_PK);
        }

        private void BindApplicant(int activityPk)
        {
            var applicantAssignedList = _organizationOperations.GetAssignedApplicantsForActivity(activityPk);

            if (applicantAssignedList.Count > 0)
            {
                ddlApplicant.SelectedId = applicantAssignedList[0].organization_PK;
            }
        }

        private void BindCountries(int activityPk)
        {
            var countryAvailableList = _countryOperations.GetAvailableEntitiesByActivity(activityPk);
            countryAvailableList.SortByField(x => x.custom_sort_ID);
            lbAuCountries.LbInputFrom.Fill(countryAvailableList, x => string.Format("{0} - {1}", x.abbreviation, x.name), x => x.country_PK);

            var countryAssignedList = _countryOperations.GetAssignedEntitiesByActivity(activityPk);
            countryAssignedList.SortByField(x => x.custom_sort_ID);
            lbAuCountries.LbInputTo.Fill(countryAssignedList, x => string.Format("{0} - {1}", x.abbreviation, x.name), x => x.country_PK);
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (lbSrProducts.LbInput.Items.Count == 0)
            {
                errorMessage += "Products can't be empty.<br />";
                lbSrProducts.ValidationError = "Products can't be empty.";
            }

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorMessage += "Name can't be empty.<br />";
                txtName.ValidationError = "Name can't be empty.";
            }

            if (!ddlProcedureType.SelectedId.HasValue)
            {
                errorMessage += "Procedure type can't be empty.<br />";
                ddlProcedureType.ValidationError = "Procedure type can't be empty.";
            }

            if (lbAuTypes.LbInputTo.Items.Count == 0)
            {
                errorMessage += "At least one type must be selected.<br />";
                lbAuTypes.ValidationError = "At least one type must be selected.";
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

            if (string.IsNullOrWhiteSpace(dtStartDate.Text))
            {
                errorMessage += "Start date can't be empty.<br />";
                dtStartDate.ValidationError = "Start date can't be empty.";
            }

            if (!string.IsNullOrWhiteSpace(dtStartDate.Text) && !ValidationHelper.IsValidDateTime(dtStartDate.Text, CultureInfoHr))
            {
                errorMessage += "Start date is not a valid date.<br />";
                dtStartDate.ValidationError = "Start date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtExpectedFinishedDate.Text) && !ValidationHelper.IsValidDateTime(dtExpectedFinishedDate.Text, CultureInfoHr))
            {
                errorMessage += "Expected finished date is not a valid date.<br />";
                dtExpectedFinishedDate.ValidationError = "Expected finished date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtActualFinishedDate.Text) && !ValidationHelper.IsValidDateTime(dtActualFinishedDate.Text, CultureInfoHr))
            {
                errorMessage += "Actual finished date is not a valid date.<br />";
                dtActualFinishedDate.ValidationError = "Actual finished date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtSubmissionDate.Text) && !ValidationHelper.IsValidDateTime(dtSubmissionDate.Text, CultureInfoHr))
            {
                errorMessage += "Submission date is not a valid date.<br />";
                dtSubmissionDate.ValidationError = "Submission date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtApprovalDate.Text) && !ValidationHelper.IsValidDateTime(dtApprovalDate.Text, CultureInfoHr))
            {
                errorMessage += "Approval date is not a valid date.<br />";
                dtApprovalDate.ValidationError = "Approval date is not a valid date.";
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
            lbSrProducts.ValidationError = String.Empty;
            txtName.ValidationError = String.Empty;
            ddlProcedureType.ValidationError = String.Empty;
            lbAuTypes.ValidationError = String.Empty;
            ddlInternalStatus.ValidationError = String.Empty;
            lbAuCountries.ValidationError = String.Empty;
            dtStartDate.ValidationError = String.Empty;
            dtExpectedFinishedDate.ValidationError = String.Empty;
            dtActualFinishedDate.ValidationError = String.Empty;
            dtSubmissionDate.ValidationError = String.Empty;
            dtApprovalDate.ValidationError = String.Empty;

            // Right pane
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            var activity = new Activity_PK();

            if (FormType == FormType.Edit)
            {
                activity = _activityOperations.GetEntity(_idAct);
            }

            if (activity == null) return null;

            activity.name = txtName.Text;
            activity.activity_ID = txtActivityId.Text;
            activity.description = txtDescription.Text;
            activity.user_FK = ddlResponsibleUser.SelectedId;
            activity.procedure_number = txtProcedureNumber.Text;
            activity.procedure_type_FK = ddlProcedureType.SelectedId;
            activity.regulatory_status_FK = ddlRegulatoryStatus.SelectedId;
            activity.internal_status_FK = ddlInternalStatus.SelectedId;
            activity.mode_FK = ddlActivityMode.SelectedId;
            activity.legal = txtLegalBasisOfApplication.Text;
            activity.comment = txtComment.Text;
            activity.start_date = ValidationHelper.IsValidDateTime(dtStartDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtStartDate.Text) : null;
            activity.expected_finished_date = ValidationHelper.IsValidDateTime(dtExpectedFinishedDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtExpectedFinishedDate.Text) : null;
            activity.actual_finished_date = ValidationHelper.IsValidDateTime(dtActualFinishedDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtActualFinishedDate.Text) : null;
            activity.submission_date = ValidationHelper.IsValidDateTime(dtSubmissionDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtSubmissionDate.Text) : null;
            activity.approval_date = ValidationHelper.IsValidDateTime(dtApprovalDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtApprovalDate.Text) : null;
            activity.billable = rbYnBillable.SelectedValue ?? false;
            activity.automatic_alerts_on = rbYnAutomaticAlerts.SelectedValue ?? false;

            // Prevent alert for start date if specified date is today
            if (activity.start_date.HasValue && activity.start_date.Value.Date == DateTime.Now.Date)
            {
                activity.prevent_start_date_alert = true;
            }

            // Prevent alert for expected finished date if specified date is today
            if (activity.expected_finished_date.HasValue && activity.expected_finished_date.Value.Date == DateTime.Now.Date)
            {
                activity.prevent_exp_finish_date_alert = true;
            }

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                activity = _activityOperations.Save(activity);

                if (!activity.activity_PK.HasValue) return null;

                SaveProducts(activity.activity_PK.Value, auditTrailSessionToken);
                SaveProjects(activity.activity_PK.Value, auditTrailSessionToken);
                SaveTypes(activity.activity_PK.Value, auditTrailSessionToken);
                SaveApplicant(activity.activity_PK.Value, auditTrailSessionToken);
                SaveCountries(activity.activity_PK.Value, auditTrailSessionToken);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, activity.activity_PK, "ACTIVITY", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, activity.activity_PK, "ACTIVITY", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return activity;
        }

        private void SaveProducts(int activityPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var productList = _productOperations.GetAssignedEntitiesByActivity(activityPk);
            productList.SortByField(x => x.name);

            foreach (var product in productList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += product.GetNameFormatted();
            }

            _activityProductMnOperations.DeleteByActivityPK(activityPk);

            var activityProductMnList = new List<Activity_product_PK>(lbSrProducts.LbInput.Items.Count);

            foreach (ListItem listItem in lbSrProducts.LbInput.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                activityProductMnList.Add(new Activity_product_PK(null, activityPk, int.Parse(listItem.Value)));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (activityProductMnList.Count > 0)
            {
                _activityProductMnOperations.SaveCollection(activityProductMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, activityPk.ToString(), "ACTIVITY_PRODUCT_MN");
        }

        private void SaveProjects(int activityPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var projectList = _projectOperations.GetAssignedEntitiesByActivity(activityPk);
            projectList.SortByField(x => x.name);

            foreach (var project in projectList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += project.name;
            }

            _activityProjectMnOperations.DeleteByActivity(activityPk);

            var activityProjectMnList = new List<Activity_project_PK>(lbSrProjects.LbInput.Items.Count);

            foreach (ListItem listItem in lbSrProjects.LbInput.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                activityProjectMnList.Add(new Activity_project_PK(null, activityPk, int.Parse(listItem.Value)));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (activityProjectMnList.Count > 0)
            {
                _activityProjectMnOperations.SaveCollection(activityProjectMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, activityPk.ToString(), "ACTIVITY_PROJECT_MN");
        }

        private void SaveTypes(int activityPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var typeList = _typeOperations.GetAssignedTypesForActivity(activityPk);
            typeList.SortByField(x => x.name);

            foreach (var type in typeList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += type.name;
            }

            _activityTypeMnOperations.DeleteByActivityPK(activityPk);

            var activityTypeMnList = new List<Activity_type_PK>(lbAuTypes.LbInputTo.Items.Count);

            foreach (ListItem listItem in lbAuTypes.LbInputTo.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                activityTypeMnList.Add(new Activity_type_PK(null, activityPk, int.Parse(listItem.Value)));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (activityTypeMnList.Count > 0)
            {
                _activityTypeMnOperations.SaveCollection(activityTypeMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, activityPk.ToString(), "ACTIVITY_TYPE_MN");
        }

        private void SaveApplicant(int activityPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var applicantList = _organizationOperations.GetAssignedApplicantsForActivity(activityPk);
            applicantList.SortByField(x => x.name_org);

            foreach (var applicant in applicantList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += applicant.name_org;
            }

            _activityApplicantMnOperations.DeleteByActivityPK(activityPk);

            var activityApplicantMnList = new List<Activity_organization_applicant_PK>();

            if (ddlApplicant.SelectedId.HasValue)
            {
                activityApplicantMnList.Add(new Activity_organization_applicant_PK(null, activityPk, ddlApplicant.SelectedId));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += ddlApplicant.Text;
            }

            if (activityApplicantMnList.Count > 0)
            {
                _activityApplicantMnOperations.SaveCollection(activityApplicantMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, activityPk.ToString(), "ACTIVITY_APPLICANT_MN");
        }

        private void SaveCountries(int activityPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var countryList = _countryOperations.GetAssignedEntitiesByActivity(activityPk);
            countryList.SortByField(x => x.custom_sort_ID);

            foreach (var country in countryList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += ", ";
                complexAuditOldValue += country.abbreviation;
            }

            _activityCountryOperations.DeleteByActivityPK(activityPk);

            var activityCountryMnList = new List<Activity_country_PK>(lbAuCountries.LbInputTo.Items.Count);

            foreach (ListItem listItem in lbAuCountries.LbInputTo.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                activityCountryMnList.Add(new Activity_country_PK(null, activityPk, int.Parse(listItem.Value)));

                var country = _countryOperations.GetEntity(listItem.Value);
                if (country != null && !string.IsNullOrWhiteSpace(country.abbreviation))
                {
                    if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += ", ";
                    complexAuditNewValue += country.abbreviation;
                }
            }

            if (activityCountryMnList.Count > 0)
            {
                _activityCountryOperations.SaveCollection(activityCountryMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, activityPk.ToString(), "ACTIVITY_COUNTRY_MN");
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
                    if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idAct.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                        else if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "AuthProdPreview" && _idAct.HasValue) Response.Redirect(string.Format("~/Views/AuthorisedProductView/Preview.aspx?EntityContext={0}&idAuthProd={1}", EntityContext, _idAct));
                            else if (From == "AuthProd") Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "AuthProdSearch") Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if (From == "ProdPreview" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                            else if (From == "Prod") Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                            else if (From == "ProdSearch") Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                        }
                    }
                    else if (EntityContext == EntityContext.Product)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "Prod") Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "ProdActList" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                            else if (From == "ProdPreview" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                            else if (From == "ProdSearch") Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                        }
                    }
                    else if (EntityContext == EntityContext.SubmissionUnit)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs || FormType == FormType.New) && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "SubUnitActPreview" && _idSubUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idSubUnit={1}", EntityContext, _idSubUnit));
                        }
                    }
                    else if (EntityContext == EntityContext.Project)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "ProjActList" && _idProj.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}&idProj={1}", EntityContext, _idProj));
                            else if (From == "ProjPreview" && _idProj.HasValue) Response.Redirect(string.Format("~/Views/ProjectView/Preview.aspx?EntityContext={0}&idProj={1}", EntityContext, _idProj));
                            else if (From == "Proj") Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "ProjSearch") Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                        }
                    }
                    else if (EntityContext == EntityContext.TimeUnit || EntityContext == EntityContext.TimeUnitMy || EntityContext == EntityContext.SubmissionUnit)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "SubUnitActPreview" && _idSubUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idSubUnit={1}", EntityContext.SubmissionUnit, _idSubUnit));
                            else if (From == "TimeUnitActPreview" && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnit, _idTimeUnit));
                            else if (From == "TimeUnitMyActPreview" && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnitMy, _idTimeUnit));
                        }
                    }
                    else if (EntityContext == EntityContext.Default)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "Act") Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext.Activity));
                            else if (From == "ActMy") Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext.ActivityMy));
                        }
                    }
                    Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext.Activity));
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
            if (result is Activity_PK)
            {
                var activity = result as Activity_PK;
                if (activity.activity_PK.HasValue)
                {
                    MasterPage.OneTimePermissionToken = Permission.View;
                    if (FormType == FormType.SaveAs) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext.Activity, activity.activity_PK));
                    else if (EntityContext == EntityContext.SubmissionUnit) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext, activity.activity_PK));
                    else if (EntityContext == EntityContext.Activity) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext, activity.activity_PK));
                    else if (EntityContext == EntityContext.ActivityMy) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext, activity.activity_PK));
                    else Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext.Activity, activity.activity_PK));
                }
            }
            Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
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

        public void DtSubmissionDateLnkSetReminder_OnClick(object sender, EventArgs eventArgs)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(dtSubmissionDate.Label)), dtSubmissionDate.Text);
        }

        public void DtApprovalDateLnkSetReminder_OnClick(object sender, EventArgs eventArgs)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(dtApprovalDate.Label)), dtApprovalDate.Text);
        }

        public void Reminder_OnConfirmInputButtonProcess_Click(object sender, EventArgs e)
        {
            var reminder = Reminder.ReminderVs;

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

        #region Products searcher

        private void LbSrProductsSearcher_OnOkButtonClick(object sender, FormEventArgs<List<int>> formEventArgs)
        {
            foreach (var selectedId in lbSrProducts.Searcher.SelectedItems)
            {
                if (lbSrProducts.LbInput.Items.FindByValue(selectedId.ToString()) != null) continue;

                var product = _productOperations.GetEntity(selectedId);

                if (product == null || !product.product_PK.HasValue) continue;

                lbSrProducts.LbInput.Items.Add(new ListItem(product.GetNameFormatted(Constant.UnknownValue), selectedId.ToString()));

                if (FormType == FormType.New)
                {
                    AssignCountriesForProduct(product);
                    SortCountries(lbAuCountries.LbInputTo);
                }
            }
        }

        void LbSrProducts_OnRemoveClick(object sender, EventArgs e)
        {
            lbSrProducts.LbInput.RemoveSelected();

            if (FormType == FormType.New)
            {
                lbAuCountries.Clear();
                FillLbAuCountries();

                foreach (ListItem listItem in lbSrProducts.LbInput.Items)
                {
                    if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                    var product = _productOperations.GetEntity(int.Parse(listItem.Value));

                    if (product == null) continue;

                    AssignCountriesForProduct(product);
                }

                SortCountries(lbAuCountries.LbInputTo);
            }
        }

        #endregion

        #region Project searcher

        private void LbSrProjectsSearcher_OnOkButtonClick(object sender, FormEventArgs<List<int>> formEventArgs)
        {
            foreach (var selectedId in lbSrProjects.Searcher.SelectedItems)
            {
                if (lbSrProjects.LbInput.Items.FindByValue(selectedId.ToString()) != null) continue;

                var project = _projectOperations.GetEntity(selectedId);

                if (project != null)
                {
                    var text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.UnknownValue;

                    lbSrProjects.LbInput.Items.Add(new ListItem(text, selectedId.ToString()));
                }
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

        private void AssignCountriesForProduct(Product_PK product)
        {
            if (product == null || !product.product_PK.HasValue) return;

            var countryList = _countryOperations.GetAssignedEntitiesByProduct(product.product_PK.Value);

            foreach (var country in countryList)
            {
                var listItem = lbAuCountries.LbInputFrom.Items.FindByValue(Convert.ToString(country.country_PK));

                if (listItem == null) continue;

                lbAuCountries.LbInputTo.Items.Add(listItem);
                lbAuCountries.LbInputFrom.Items.Remove(listItem);
            }
        }

        private void HideReminders()
        {
            dtStartDate.ShowReminder = false;
            dtExpectedFinishedDate.ShowReminder = false;
            dtSubmissionDate.ShowReminder = false;
            dtApprovalDate.ShowReminder = false;
        }

        public void RefreshReminderStatus()
        {
            var tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.ACTIVITY);
            AlerterHelper.RefreshReminderStatus(_reminderOperations, MasterPage, new List<IReminderCustomControl> { dtStartDate, dtExpectedFinishedDate, dtSubmissionDate, dtApprovalDate }, tableName, _idAct);
        }

        public void SetReminder(String attributeName, String attributeValue)
        {
            var reminder = new Reminder_PK
            {
                reminder_type = "Activity",
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
                if (EntityContext == EntityContext.Product) location = Support.LocationManager.Instance.GetLocationByName("ProdActList", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.Project) location = Support.LocationManager.Instance.GetLocationByName("ProjActList", Support.CacheManager.Instance.AppLocations);
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
                if (EntityContext == EntityContext.Activity) location = Support.LocationManager.Instance.GetLocationByName("ActPreview", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.ActivityMy) location = Support.LocationManager.Instance.GetLocationByName("ActMyPreview", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.SubmissionUnit) location = Support.LocationManager.Instance.GetLocationByName("SubUnitActPreview", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.TimeUnit) location = Support.LocationManager.Instance.GetLocationByName("TimeUnitActPreview", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.TimeUnitMy) location = Support.LocationManager.Instance.GetLocationByName("TimeUnitMyActPreview", Support.CacheManager.Instance.AppLocations);

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
            Location_PK location = null;

            if (EntityContext == EntityContext.Product) location = Support.LocationManager.Instance.GetLocationByName("ProdActList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Project) location = Support.LocationManager.Instance.GetLocationByName("ProjActList", Support.CacheManager.Instance.AppLocations);
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

            var isPermittedInsertActivity = false;

            if (EntityContext == EntityContext.Default) isPermittedInsertActivity = SecurityHelper.IsPermitted(Permission.InsertActivity);
            if (isPermittedInsertActivity)
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
                isPermittedInsertActivity = SecurityHelper.IsPermittedAny(new List<Permission> { Permission.InsertActivity, Permission.SaveAsActivity, Permission.EditActivity }, MasterPage.RefererLocation);
                if (isPermittedInsertActivity)
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