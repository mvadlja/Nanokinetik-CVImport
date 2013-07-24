using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web.UI;
using AspNetUI.Views.Shared.Interface;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using EVMessage.Xevprm;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.AuthorisedProductView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idAuthProd;
        private int? _idProd;

        private bool? IsResponsibleUser
        {
            get { return ViewState["IsResponsibleUser"] != null ? (bool?)ViewState["IsResponsibleUser"] : null; }
            set { ViewState["IsResponsibleUser"] = value; }
        }

        private IAuthorisedProductOperations _authorizedProductOperations;
        private IProduct_PKOperations _productOperations;
        private IPerson_PKOperations _personOperations;
        private IType_PKOperations _typeOperations;
        private IOrganization_PKOperations _organizationOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private ICountry_PKOperations _countryOperations;
        private IReminder_PKOperations _reminderOperations;
        private IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations;
        private IUSEROperations _userOperations;
        private IAp_organizatation_dist_mn_PKOperations _authorisedProductDistributorsMn;
        private IMeddra_pkOperations _meddraOperations;
        private IMeddra_ap_mn_PKOperations _meddraAuthorisedProductMnOperations;
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
            StylizeArticle57RelevantControls(rbYnArticle57Reporting.SelectedValue);
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

            _idAuthProd = ValidationHelper.IsValidInt(Request.QueryString["idAuthProd"]) ? int.Parse(Request.QueryString["idAuthProd"]) : (int?)null;
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;

            _authorizedProductOperations = new AuthorisedProductDAL();
            _productOperations = new Product_PKDAL();
            _personOperations = new Person_PKDAL();
            _typeOperations = new Type_PKDAL();
            _organizationOperations = new Organization_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _countryOperations = new Country_PKDAL();
            _reminderOperations = new Reminder_PKDAL();
            _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            _userOperations = new USERDAL();
            _authorisedProductDistributorsMn = new Ap_organizatation_dist_mn_PKDAL();
            _meddraOperations = new Meddra_pkDAL();
            _meddraAuthorisedProductMnOperations = new Meddra_ap_mn_PKDAL();
            _reminderDateOperations = new Reminder_date_PKDAL();
        }

        public override void LoadActionQuery()
        {
            base.LoadActionQuery();

            if (_idProd.HasValue) EntityContext = EntityContext.Product;
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            txtSrRelatedProduct.Searcher.OnListItemSelected += RelatedProductSearcher_OnListItemSelected;
            txtSrQppv.Searcher.OnListItemSelected += QppvSearcher_OnListItemSelected;
            txtSrLocalQppv.Searcher.OnListItemSelected += LocalQppvSearcher_OnListItemSelected;

            dtAuthorisationDate.LnkSetReminder.Click += LnkSetAuthorisationDateReminder_Click;
            dtAuthorisationExpiryDate.LnkSetReminder.Click += LnkSetAuthorisationExpiryDateReminder_Click;
            dtLaunchDate.LnkSetReminder.Click += LnkSetLaunchDateReminder_Click;
            dtWithdrawnDate.LnkSetReminder.Click += LnkSetWithdrawnDateReminder_Click;
            dtSunsetClause.LnkSetReminder.Click += LnkSetSunsetClauseReminder_Click;

            Reminder.OnConfirmInputButtonProcess_Click += Reminder_OnConfirmInputButtonProcess_Click;

            lbExtIndications.OnAddClick += lbExtIndications_OnAddClick;
            lbExtIndications.OnEditClick += lbExtIndications_OnEditClick;
            lbExtIndications.OnRemoveClick += lbExtIndications_OnRemoveClick;
            IndicationsPopup.OnOkButtonClick += IndicationsPopup_OnOkButtonClick;

            rbYnArticle57Reporting.RbYn.SelectedIndexChanged += rbYnArticle57ReportingRbYnu_SelectedIndexChanged;

            ddlAuthorisationStatus.DdlInput.SelectedIndexChanged += ddlAuthorisationStatusDdlInput_SelectedIndexChanged;
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
            txtSrRelatedProduct.Text = String.Empty;
            rbYnArticle57Reporting.SelectedValue = false;
            rbYnSubstanceTranslations.SelectedValue = false;
            txtEvcode.Text = String.Empty;
            txtXEvprmStatus.Text = String.Empty;
            txtFullPresentationName.Text = String.Empty;
            txtProductShortName.Text = String.Empty;
            txtProductGenericName.Text = String.Empty;
            txtProductCompanyName.Text = String.Empty;
            txtProductStrengthName.Text = String.Empty;
            txtProductFormName.Text = String.Empty;
            txtPackageDescription.Text = String.Empty;
            txtCommentEvprm.Text = String.Empty;
            ddlResponsibleUser.SelectedValue = String.Empty;
            txtDescription.Text = String.Empty;
            txtAuthorisedProductId.Text = String.Empty;
            lbAuDistributors.Clear();
            txtShelfLife.Text = String.Empty;
            rbYnMarketed.SelectedValue = false;
            rbYnReimbursmentStatus.SelectedValue = null;
            rbYnReservationConfirmed.SelectedValue = null;
            ddlLegalStatus.SelectedValue = String.Empty;
            
            // Right pane
            ddlLicenceHolder.SelectedValue = String.Empty;
            ddlLicenceHolderGroup.SelectedValue = String.Empty;
            ddlLocalRepresentative.SelectedValue = String.Empty;
            txtSrQppv.Text = String.Empty;
            txtSrLocalQppv.Text = String.Empty;
            ddlMasterFileLocation.SelectedValue = String.Empty;
            txtPhVEmail.Text = String.Empty;
            txtPhVPhone.Text = String.Empty;
            ddlAuthorisationCountry.SelectedValue = String.Empty;
            ddlAuthorisationStatus.SelectedValue = String.Empty;
            txtAuthorisationNumber.Text = String.Empty;
            lbExtIndications.Clear();
            txtComment.Text = String.Empty;
            dtAuthorisationDate.Text = String.Empty;
            dtAuthorisationExpiryDate.Text = String.Empty;
            dtLaunchDate.Text = String.Empty;
            dtWithdrawnDate.Text = String.Empty;
            dtInfoDate.Text = String.Empty;
            dtSunsetClause.Text = String.Empty;
            txtReservedTo.Text = String.Empty;
            txtLocalCodes.Text = String.Empty;
            txtPackSize.Text = String.Empty;
        }

        private void FillFormControls(object arg)
        {
            FillDdlResponsibleUsers(null);
            FillDdlLegalStatus(null);
            FillDdlLicenceHolders(null);
            FillDdlLicenceHolderGroups(null);
            FillDdlLocalRepresentatives(null);
            FillDdlMasterFileLocations(null);
            FillDdlAuthorisationCountries(null);
            FillDdlAuthorisationStatus(null);
            if (FormType == FormType.New) FillDdlDistributors(null);
        }

        private void SetFormControlsDefaults(object arg)
        {
            lblPrvParentEntity.Visible = FormType != FormType.New;

            if (FormType == FormType.New || FormType == FormType.SaveAs)
            {
                var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                ddlResponsibleUser.SelectedValue = user != null ? user.Person_FK : null;
            }

            if (FormType == FormType.New)
            {
                rbYnArticle57Reporting.SelectedValue = true;
                rbYnSubstanceTranslations.SelectedValue = false;

                if (EntityContext == EntityContext.Product) BindParentEntityProduct(_idProd);

                HideReminders();
            }
            else if (FormType == FormType.SaveAs)
            {
                HideReminders();
            }

            var rootLocation = AspNetUI.Support.LocationManager.Instance.GetLocationByName("Root", AspNetUI.Support.CacheManager.Instance.AppLocations);
            if (rootLocation == null || !SecurityHelper.IsPermitted(Permission.AuthProdAndProdAdditionalAttributes, rootLocation))
            {
                rbYnReimbursmentStatus.Visible = false;
                rbYnReservationConfirmed.Visible = false;
                ddlLicenceHolderGroup.Visible = false;
                txtReservedTo.Visible = false;
                txtLocalCodes.Visible = false;
                txtPackSize.Visible = false;
            }

            if (rootLocation == null || !SecurityHelper.IsPermitted(Permission.XevprmShowFormAttributes, rootLocation))
            {
                rbYnArticle57Reporting.Visible = false;
                rbYnSubstanceTranslations.Visible = false;
                txtXEvprmStatus.Visible = false;
                txtProductGenericName.Visible = false;
                txtProductCompanyName.Visible = false;
                txtProductStrengthName.Visible = false;
                txtProductFormName.Visible = false;
                txtCommentEvprm.Visible = false;
                dtInfoDate.Visible = false;
            }

            BindDynamicControls(null);
        }

        /// <summary>
        /// Binds responsible users drop down list
        /// </summary>
        /// <param name="args"></param>
        private void FillDdlResponsibleUsers(object args)
        {
            var responsibleUsers = _personOperations.GetPersonsByRole(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUsers, "FullName", "person_PK");
            ddlResponsibleUser.SortItemsByText();
        }

        /// <summary>
        /// Binds legal status down list
        /// </summary>
        /// <param name="args"></param>
        private void FillDdlLegalStatus(object args)
        {
            var legalStatus = _typeOperations.GetTypesForDDL("LS");
            ddlLegalStatus.Fill(legalStatus, "name", "type_PK");
            ddlLegalStatus.SortItemsByText();
        }

        private void FillDdlDistributors(int? authorisedProductPk)
        {
            var availableDistributors = _organizationOperations.GetAvailableDistributorsByAp(authorisedProductPk);
            lbAuDistributors.Fill(availableDistributors, lbAuDistributors.LbInputFrom, "name_org", "organization_PK");
            lbAuDistributors.LbInputFrom.SortItemsByText();

            if (FormType == FormType.Edit || FormType == FormType.SaveAs)
            {
                var assignedDistributors = _organizationOperations.GetAssignedDistributorsByAp(authorisedProductPk);
                lbAuDistributors.Fill(assignedDistributors, lbAuDistributors.LbInputTo, "name_org", "organization_PK");
                lbAuDistributors.LbInputTo.SortItemsByText();
            }
        }

        /// <summary>
        /// Binds licence holders drop down list
        /// </summary>
        /// <param name="args"></param>
        private void FillDdlLicenceHolders(object args)
        {
            var licenceHolders = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.LicenceHolder);
            ddlLicenceHolder.Fill(licenceHolders, "name_org", "organization_PK");
            ddlLicenceHolder.SortItemsByText();
        }

        private void FillDdlLicenceHolderGroups(object args)
        {
            var licenceHolderGroups = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.LicenceHolderGroup);
            ddlLicenceHolderGroup.Fill(licenceHolderGroups, "name_org", "organization_PK");
            ddlLicenceHolderGroup.SortItemsByText();
        }

        /// <summary>
        /// Binds local representatives drop down list
        /// </summary>
        /// <param name="args"></param>
        private void FillDdlLocalRepresentatives(object args)
        {
            var localRepresentatives = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.LocalRepresentative);
            ddlLocalRepresentative.Fill(localRepresentatives, "name_org", "organization_PK");
            ddlLocalRepresentative.SortItemsByText();
        }

        /// <summary>
        /// Binds master file locations drop down list
        /// </summary>
        /// <param name="args"></param>
        private void FillDdlMasterFileLocations(object args)
        {
            var masterFileLocations = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.MasterFileLocation);
            ddlMasterFileLocation.Fill(masterFileLocations, "name_org", "organization_PK");
            ddlMasterFileLocation.SortItemsByText();
        }

        /// <summary>
        /// Binds authorisation countries drop down list
        /// </summary>
        /// <param name="args"></param>
        private void FillDdlAuthorisationCountries(object args)
        {
            var authorisationCountries = _countryOperations.GetEntitiesCustomSort();
            ddlAuthorisationCountry.Fill(authorisationCountries, Constant.Countries.DisplayNameFormat, "country_PK");
        }

        /// <summary>
        /// Binds authorisation countries drop down list
        /// </summary>
        /// <param name="args"></param>
        private void FillDdlAuthorisationStatus(object args)
        {
            var authorisationStatus = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.AuthorisationStatus);
            ddlAuthorisationStatus.Fill(authorisationStatus, "name", "type_PK");
            ddlAuthorisationStatus.SortItemsByText();
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idAuthProd.HasValue) return;

            var authorisedProduct = _authorizedProductOperations.GetEntity(_idAuthProd);
            if (authorisedProduct == null) return;

            // Entity
            lblPrvParentEntity.Text = authorisedProduct.product_name;

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------
            // Related product
            BindParentEntityProduct(authorisedProduct.product_FK);

            // Article 57 reporting
            rbYnArticle57Reporting.SelectedValue = authorisedProduct.article_57_reporting.HasValue && authorisedProduct.article_57_reporting.Value;

            // Substance translations
            rbYnSubstanceTranslations.SelectedValue = authorisedProduct.substance_translations.HasValue && authorisedProduct.substance_translations.Value;

            if (FormType == FormType.Edit)
            {
                // EVCODE
                txtEvcode.Text = authorisedProduct.ev_code;

                // xEVPRM status
                txtXEvprmStatus.Text = authorisedProduct.XEVPRM_status;
            }

            // Full presentation name
            txtFullPresentationName.Text = authorisedProduct.product_name;

            // Product short name
            txtProductShortName.Text = authorisedProduct.productshortname;

            // Product generic name
            txtProductGenericName.Text = authorisedProduct.productgenericname;

            // Product company name
            txtProductCompanyName.Text = authorisedProduct.productcompanyname;

            // Product strength name
            txtProductStrengthName.Text = authorisedProduct.productstrenght;

            // Product form name
            txtProductFormName.Text = authorisedProduct.productform;

            // Package description
            txtPackageDescription.Text = authorisedProduct.packagedesc;

            // Comment (EVPRM)
            txtCommentEvprm.Text = authorisedProduct.evprm_comments;

            // Responsible user
            BindResponsibleUser(authorisedProduct.responsible_user_person_FK);

            // Description
            txtDescription.Text = authorisedProduct.description;

            // Authorised product ID
            txtAuthorisedProductId.Text = authorisedProduct.product_ID;

            // Distributor Assignments
            FillDdlDistributors(authorisedProduct.ap_PK);

            // Shelf life
            txtShelfLife.Text = authorisedProduct.shelflife;

            // Marketed
            rbYnMarketed.SelectedValue = authorisedProduct.marketed.HasValue && authorisedProduct.marketed.Value;

            // Reimbursment status
            rbYnReimbursmentStatus.SelectedValue = authorisedProduct.reimbursment_status.HasValue && authorisedProduct.reimbursment_status.Value;

            // Reservation confirmed
            rbYnReservationConfirmed.SelectedValue = authorisedProduct.reservation_confirmed.HasValue && authorisedProduct.reservation_confirmed.Value;

            // Legal status
            var legalStatusInt = ValidationHelper.IsValidInt(authorisedProduct.legalstatus) ? (int?)Convert.ToInt32(authorisedProduct.legalstatus) : null;
            BindLegalStatus(legalStatusInt);

            

            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------
            // Licence holder
            BindLicenceHolder(authorisedProduct.organizationmahcode_FK);

            // Licence holder group
            BindLicenceHolderGroup(authorisedProduct.license_holder_group_FK);

            // Local representative 
            BindLocalRepresentative(authorisedProduct.local_representative_FK);

            // QPPV
            BindQppv(authorisedProduct.qppv_code_FK);

            // Local QPPV
            BindLocalQppv(authorisedProduct.local_qppv_code_FK);

            // Master File Location
            BindMasterFileLocation(authorisedProduct.mflcode_FK);

            // PhV EMail
            txtPhVEmail.Text = authorisedProduct.phv_email;

            // PhV Phone
            txtPhVPhone.Text = authorisedProduct.phv_phone;

            // Indications
            BindIndications(authorisedProduct.ap_PK);

            // Authorisation country
            BindAuthorisationCountry(authorisedProduct.authorisationcountrycode_FK);

            // Authorisation status
            BindAuthorisationStatus(authorisedProduct.authorisationstatus_FK);

            // Authorisation number
            txtAuthorisationNumber.Text = authorisedProduct.authorisationnumber;

            // Comment
            txtComment.Text = authorisedProduct.comment;

            // Authorisation date
            dtAuthorisationDate.Text = authorisedProduct.authorisationdate.HasValue ? ((DateTime)authorisedProduct.authorisationdate).ToString(Constant.DateTimeFormat) : String.Empty;

            // Authorisation expiry date
            dtAuthorisationExpiryDate.Text = authorisedProduct.authorisationexpdate.HasValue ? ((DateTime)authorisedProduct.authorisationexpdate).ToString(Constant.DateTimeFormat) : String.Empty;

            // Launch date
            dtLaunchDate.Text = authorisedProduct.launchdate.HasValue ? ((DateTime)authorisedProduct.launchdate).ToString(Constant.DateTimeFormat) : String.Empty;

            // Withdrawn date
            dtWithdrawnDate.Text = authorisedProduct.authorisationwithdrawndate.HasValue ? ((DateTime)authorisedProduct.authorisationwithdrawndate).ToString(Constant.DateTimeFormat) : String.Empty;

            // Info date
            dtInfoDate.Text = authorisedProduct.infodate.HasValue ? ((DateTime)authorisedProduct.infodate).ToString(Constant.DateTimeFormat) : String.Empty;

            // Sunset clause
            dtSunsetClause.Text = authorisedProduct.sunsetclause.HasValue ? ((DateTime)authorisedProduct.sunsetclause).ToString(Constant.DateTimeFormat) : String.Empty;

            // Reserved to
            txtReservedTo.Text = authorisedProduct.reserved_to;

            // Local codes
            txtLocalCodes.Text = authorisedProduct.local_codes;

            // Pack size
            txtPackSize.Text = authorisedProduct.pack_size;

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) IsResponsibleUser = authorisedProduct.responsible_user_person_FK == user.Person_FK;

            if (Request.QueryString["XevprmValidation"] != null)
            {
                ValidateFormForXevprm(authorisedProduct);
            }
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idAuthProd.HasValue) return;

            RefreshReminderStatus();
        }

        private void BindAuthorisationStatus(int? authorisationStatusFk)
        {
            var selectedAuthorisationStatus = _typeOperations.GetEntity(authorisationStatusFk);
            ddlAuthorisationStatus.SelectedValue = selectedAuthorisationStatus != null ? selectedAuthorisationStatus.type_PK : null;
        }

        private void BindAuthorisationCountry(int? authorisationCountryFk)
        {
            var selectedAuthorisationCountry = _countryOperations.GetEntity(authorisationCountryFk);
            ddlAuthorisationCountry.SelectedValue = selectedAuthorisationCountry != null ? selectedAuthorisationCountry.country_PK : null;
        }

        private void BindMasterFileLocation(int? masterFileLocationFk)
        {
            var selectedMasterFileLocation = _organizationOperations.GetEntity(masterFileLocationFk);
            ddlMasterFileLocation.SelectedValue = selectedMasterFileLocation != null ? selectedMasterFileLocation.organization_PK : null;
        }

        private void BindQppv(int? qppvCodeFk)
        {
            txtSrQppv.Text = PersonHelper.GetQppvNameFormatted(qppvCodeFk, string.Empty);
            txtSrQppv.SelectedEntityId = qppvCodeFk;
        }

        private void BindLocalQppv(int? localQppvCodeFk)
        {
            txtSrLocalQppv.Text = PersonHelper.GetQppvNameFormatted(localQppvCodeFk, string.Empty);
            txtSrLocalQppv.SelectedEntityId = localQppvCodeFk;
        }
        private void BindLocalRepresentative(int? localRepresentativeFk)
        {
            var selectedLocalRepresentative = _organizationOperations.GetEntity(localRepresentativeFk);
            ddlLocalRepresentative.SelectedValue = selectedLocalRepresentative != null ? selectedLocalRepresentative.organization_PK : null;
        }

        private void BindLicenceHolder(int? licenceHolderFk)
        {
            var selectedLicenceHolder = _organizationOperations.GetEntity(licenceHolderFk);
            ddlLicenceHolder.SelectedValue = selectedLicenceHolder != null ? selectedLicenceHolder.organization_PK : null;
        }

        private void BindLicenceHolderGroup(int? licenceHolderGroupFk)
        {
            var selectedLicenceHolderGroup = _organizationOperations.GetEntity(licenceHolderGroupFk);
            ddlLicenceHolderGroup.SelectedValue = selectedLicenceHolderGroup != null ? selectedLicenceHolderGroup.organization_PK : null;
        }

        private void BindLegalStatus(int? legalStatusFk)
        {
            var selectedLegalStatus = _typeOperations.GetEntity(legalStatusFk);
            ddlLegalStatus.SelectedValue = selectedLegalStatus != null ? selectedLegalStatus.type_PK : null;
        }

        private void BindResponsibleUser(int? responsibleUserFk)
        {
            var selectedResponsibleUser = _personOperations.GetEntity(responsibleUserFk);
            ddlResponsibleUser.SelectedValue = selectedResponsibleUser != null ? selectedResponsibleUser.person_PK : null;
        }

        private void BindParentEntityProduct(int? relatedProductFk)
        {
            var relatedProduct = _productOperations.GetEntity(relatedProductFk);
            if (relatedProduct == null)
            {
                lblPrvParentEntity.Text = Constant.ControlDefault.LbPrvText;
                return;
            }

            txtSrRelatedProduct.Text = relatedProduct.GetNameFormatted();
            txtSrRelatedProduct.SelectedEntityId = relatedProduct.product_PK;

            if (EntityContext == EntityContext.Product)
            {
                lblPrvParentEntity.Label = "Product:";
                lblPrvParentEntity.Visible = true;
                lblPrvParentEntity.Text = relatedProduct.name;
            }
        }

        private void BindIndications(int? authorisedProductPk)
        {
            var indicationsList = _meddraOperations.GetMeddraByAp(authorisedProductPk);
            lbExtIndications.Fill(indicationsList, "MeddraFullName", "meddra_pk", true);
            lbExtIndications.LbInput.SortItemsByText();
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(txtSrRelatedProduct.Text))
            {
                errorMessage += "Related product can't be empty.<br />";
                txtSrRelatedProduct.ValidationError = "Related product can't be empty.";
            }

            if (string.IsNullOrWhiteSpace(txtFullPresentationName.Text))
            {
                errorMessage += "Full presentation name can't be empty.<br />";
                txtFullPresentationName.ValidationError = "Full presentation name can't be empty.";
            }

            if (string.IsNullOrWhiteSpace(ddlAuthorisationCountry.SelectedValue.ToString()))
            {
                errorMessage += "Authorisation country can't be empty.<br />";
                ddlAuthorisationCountry.ValidationError = "Authorisation country can't be empty.";
            }

            if (!string.IsNullOrWhiteSpace(dtAuthorisationDate.Text) &&
                !ValidationHelper.IsValidDateTime(dtAuthorisationDate.Text, CultureInfoHr))
            {
                errorMessage += "Authorisation date is not a valid date.<br />";
                dtAuthorisationDate.ValidationError = "Authorisation date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtAuthorisationExpiryDate.Text) &&
                !ValidationHelper.IsValidDateTime(dtAuthorisationExpiryDate.Text, CultureInfoHr))
            {
                errorMessage += "Authorisation expiry date is not a valid date.<br />";
                dtAuthorisationExpiryDate.ValidationError = "Authorisation expiry date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtLaunchDate.Text) &&
                !ValidationHelper.IsValidDateTime(dtLaunchDate.Text, CultureInfoHr))
            {
                errorMessage += "Launch date is not a valid date.<br />";
                dtLaunchDate.ValidationError = "Launch date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtWithdrawnDate.Text) &&
                !ValidationHelper.IsValidDateTime(dtWithdrawnDate.Text, CultureInfoHr))
            {
                errorMessage += "Withdrawn date is not a valid date.<br />";
                dtWithdrawnDate.ValidationError = "Withdrawn date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtInfoDate.Text) &&
                !ValidationHelper.IsValidDateTime(dtInfoDate.Text, CultureInfoHr))
            {
                errorMessage += "Info date is not a valid date.<br />";
                dtInfoDate.ValidationError = "Info date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtSunsetClause.Text) &&
                !ValidationHelper.IsValidDateTime(dtSunsetClause.Text, CultureInfoHr))
            {
                errorMessage += "Sunset clause is not a valid date.<br />";
                dtSunsetClause.ValidationError = "Sunset clause is not a valid date.";
            }

            if (txtCommentEvprm.Text.Length > 500)
            {
                errorMessage += "Comment(EVPRM) can't be larger than 500 characters (" + txtCommentEvprm.Text.Length + ").<br />";
                txtCommentEvprm.ValidationError = "Comment(EVPRM) can't be larger than 500 characters (" + txtCommentEvprm.Text.Length + ").";
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
            txtSrRelatedProduct.ValidationError = String.Empty;
            rbYnArticle57Reporting.ValidationError = String.Empty;
            rbYnSubstanceTranslations.ValidationError = String.Empty;
            txtEvcode.ValidationError = String.Empty;
            txtXEvprmStatus.ValidationError = String.Empty;
            txtFullPresentationName.ValidationError = String.Empty;
            txtProductShortName.ValidationError = String.Empty;
            txtProductGenericName.ValidationError = String.Empty;
            txtProductCompanyName.ValidationError = String.Empty;
            txtProductFormName.ValidationError = String.Empty;
            txtPackageDescription.ValidationError = String.Empty;
            txtCommentEvprm.ValidationError = String.Empty;
            ddlResponsibleUser.ValidationError = String.Empty;
            txtDescription.ValidationError = String.Empty;
            txtAuthorisedProductId.ValidationError = String.Empty;
            lbAuDistributors.ValidationError = String.Empty;
            txtShelfLife.ValidationError = String.Empty;
            rbYnMarketed.ValidationError = String.Empty;
            ddlLegalStatus.ValidationError = String.Empty;

            // Right pane
            ddlLicenceHolder.ValidationError = String.Empty;
            ddlLocalRepresentative.ValidationError = String.Empty;
            txtSrQppv.ValidationError = String.Empty;
            txtSrLocalQppv.ValidationError = String.Empty;
            ddlMasterFileLocation.ValidationError = String.Empty;
            txtPhVEmail.ValidationError = String.Empty;
            txtPhVPhone.ValidationError = String.Empty;
            ddlAuthorisationCountry.ValidationError = String.Empty;
            ddlAuthorisationStatus.ValidationError = String.Empty;
            txtAuthorisationNumber.ValidationError = String.Empty;
            txtComment.ValidationError = String.Empty;
            dtAuthorisationDate.ValidationError = String.Empty;
            dtAuthorisationExpiryDate.ValidationError = String.Empty;
            dtLaunchDate.ValidationError = String.Empty;
            dtWithdrawnDate.ValidationError = String.Empty;
            dtInfoDate.ValidationError = String.Empty;
            dtSunsetClause.ValidationError = String.Empty;
        }

        private void ValidateFormForXevprm(AuthorisedProduct entity)
        {
            if (Request.QueryString["XevprmValidation"].NotIn("1", "2", "3", "4", "6")) return;

            if (entity == null) return;

            XevprmOperationType operationType;
            Enum.TryParse(Request.QueryString["XevprmValidation"], true, out operationType);

            var validationResult = XevprmXml.ValidateAuthorisedProduct(entity, operationType);

            if (validationResult.XevprmValidationExceptions.Count == 0) return;

            var errorControlMaping = new Dictionary<string, Control>()
                {
                {XevprmValidationRules.AP.BRCustom1.RuleId, txtSrRelatedProduct},
                {XevprmValidationRules.AP.ev_code.DataType.RuleId, txtEvcode},
                {XevprmValidationRules.AP.ev_code.BR1.RuleId, txtEvcode},
                {XevprmValidationRules.AP.ev_code.BR2.RuleId, txtEvcode},
                {XevprmValidationRules.AP.comments.DataType.RuleId, txtCommentEvprm},
                {XevprmValidationRules.AP.comments.BR1.RuleId, txtCommentEvprm},
                {XevprmValidationRules.AP.enquiryemail.DataType.RuleId, txtPhVEmail},
                {XevprmValidationRules.AP.enquiryemail.BR1.RuleId, txtPhVEmail},
                {XevprmValidationRules.AP.enquiryemail.BR2.RuleId, txtPhVEmail},
                {XevprmValidationRules.AP.enquiryphone.DataType.RuleId, txtPhVPhone},
                {XevprmValidationRules.AP.enquiryphone.BR1.RuleId, txtPhVPhone},
                {XevprmValidationRules.AP.infodate.BR3.RuleId, dtInfoDate},
                {XevprmValidationRules.AP.qppvcode.BR1.RuleId, txtSrQppv},
                {XevprmValidationRules.AP.mahcode.Cardinality.RuleId, ddlLicenceHolder},
                {XevprmValidationRules.AP.authorisation.authorisationcountrycode.DataType.RuleId, ddlAuthorisationCountry},
                {XevprmValidationRules.AP.authorisation.authorisationcountrycode.Cardinality.RuleId, ddlAuthorisationCountry},
                {XevprmValidationRules.AP.authorisation.authorisationnumber.DataType.RuleId, txtAuthorisationNumber},
                {XevprmValidationRules.AP.authorisation.authorisationnumber.BR1.RuleId, txtAuthorisationNumber},
                {XevprmValidationRules.AP.authorisation.eunumber.DataType.RuleId, txtAuthorisationNumber},
                {XevprmValidationRules.AP.authorisation.eunumber.BR1.RuleId, txtAuthorisationNumber},
                {XevprmValidationRules.AP.authorisation.authorisationstatus.DataType.RuleId, ddlAuthorisationStatus},
                {XevprmValidationRules.AP.authorisation.authorisationstatus.BR5.RuleId, ddlAuthorisationStatus},
                {XevprmValidationRules.AP.authorisation.authorisationstatus.BR3.RuleId, ddlAuthorisationStatus},
                {XevprmValidationRules.AP.authorisation.authorisationstatus.BR1.RuleId, ddlAuthorisationStatus},
                {XevprmValidationRules.AP.authorisation.withdrawndate.BR6.RuleId, dtWithdrawnDate},
                {XevprmValidationRules.AP.authorisation.withdrawndate.BR5.RuleId, dtWithdrawnDate},
                {XevprmValidationRules.AP.authorisation.withdrawndate.BR3.RuleId, dtWithdrawnDate},
                {XevprmValidationRules.AP.authorisation.withdrawndate.BR1.RuleId, dtWithdrawnDate},
                {XevprmValidationRules.AP.authorisation.authorisationdate.BR5.RuleId, dtAuthorisationDate},
                {XevprmValidationRules.AP.authorisation.authorisationdate.BR1.RuleId, dtAuthorisationDate},
                {XevprmValidationRules.AP.presentationname.productname.DataType.RuleId, txtFullPresentationName},
                {XevprmValidationRules.AP.presentationname.productname.Cardinality.RuleId, txtFullPresentationName},
                {XevprmValidationRules.AP.presentationname.productshortname.DataType.RuleId, txtProductShortName},
                {XevprmValidationRules.AP.presentationname.productgenericname.DataType.RuleId, txtProductGenericName},
                {XevprmValidationRules.AP.presentationname.productgenericname.BR2.RuleId, txtProductGenericName},
                {XevprmValidationRules.AP.presentationname.productcompanyname.DataType.RuleId, txtProductCompanyName},
                {XevprmValidationRules.AP.presentationname.productcompanyname.BR2.RuleId, txtProductCompanyName},
                {XevprmValidationRules.AP.presentationname.productstrenght.DataType.RuleId, txtProductStrengthName},
                {XevprmValidationRules.AP.presentationname.productform.DataType.RuleId, txtProductFormName},
                {XevprmValidationRules.AP.presentationname.packagedesc.DataType.RuleId, txtPackageDescription},
                {XevprmValidationRules.AP.IND.BR2.RuleId, lbExtIndications},
                {XevprmValidationRules.AP.IND.BR1.RuleId, lbExtIndications},
                {XevprmValidationRules.AP.IND.meddracode.DataType.RuleId, lbExtIndications},
                {XevprmValidationRules.AP.IND.meddracode.Cardinality.RuleId, lbExtIndications},
                {XevprmValidationRules.AP.IND.meddralevel.DataType.RuleId, lbExtIndications},
                {XevprmValidationRules.AP.IND.meddralevel.Cardinality.RuleId, lbExtIndications},
                {XevprmValidationRules.AP.IND.meddraversion.DataType.RuleId, lbExtIndications},
                {XevprmValidationRules.AP.IND.meddraversion.Cardinality.RuleId, lbExtIndications}

            };

            foreach (var error in validationResult.XevprmValidationExceptions)
            {
                if (error.XevprmValidationRuleId == null || !errorControlMaping.Keys.Contains(error.XevprmValidationRuleId)) continue;
                var control = errorControlMaping[error.XevprmValidationRuleId] as Shared.Interface.IXevprmValidationError;
                if (control == null) continue;
                control.ValidationError = error.ReadyMessage;
            }
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            AuthorisedProduct authorisedProduct;
            switch (FormType)
            {
                case FormType.SaveAs:
                case FormType.New:
                    authorisedProduct = new AuthorisedProduct();
                    break;
                case FormType.Edit:
                    if (_idAuthProd == null) return null;
                    authorisedProduct = _authorizedProductOperations.GetEntity(_idAuthProd);
                    break;
                default:
                    return null;
            }

            if (authorisedProduct == null) return null;

            authorisedProduct.product_FK = txtSrRelatedProduct.SelectedEntityId;
            authorisedProduct.article_57_reporting = rbYnArticle57Reporting.SelectedValue;
            authorisedProduct.substance_translations = rbYnSubstanceTranslations.SelectedValue;
            authorisedProduct.ev_code = txtEvcode.Text;
            authorisedProduct.product_name = txtFullPresentationName.Text;
            authorisedProduct.productshortname = txtProductShortName.Text;
            authorisedProduct.productgenericname = txtProductGenericName.Text;
            authorisedProduct.productcompanyname = txtProductCompanyName.Text;
            authorisedProduct.productstrenght = txtProductStrengthName.Text;
            authorisedProduct.productform = txtProductFormName.Text;
            authorisedProduct.packagedesc = txtPackageDescription.Text;
            authorisedProduct.evprm_comments = txtCommentEvprm.Text;
            authorisedProduct.responsible_user_person_FK = ddlResponsibleUser.SelectedId;
            authorisedProduct.description = txtDescription.Text;
            authorisedProduct.product_ID = txtAuthorisedProductId.Text;
            authorisedProduct.shelflife = txtShelfLife.Text;
            authorisedProduct.marketed = rbYnMarketed.SelectedValue;
            authorisedProduct.reimbursment_status = rbYnReimbursmentStatus.SelectedValue;
            authorisedProduct.reservation_confirmed = rbYnReservationConfirmed.SelectedValue;
            authorisedProduct.legalstatus = !string.IsNullOrWhiteSpace(ddlLegalStatus.SelectedValue.ToString()) ? ddlLegalStatus.SelectedValue.ToString() : null;
            authorisedProduct.organizationmahcode_FK = ddlLicenceHolder.SelectedId;
            authorisedProduct.license_holder_group_FK = ddlLicenceHolderGroup.SelectedId;
            authorisedProduct.local_representative_FK = ddlLocalRepresentative.SelectedId;
            authorisedProduct.qppv_code_FK = txtSrQppv.SelectedEntityId;
            authorisedProduct.local_qppv_code_FK = txtSrLocalQppv.SelectedEntityId;            
            authorisedProduct.mflcode_FK = !string.IsNullOrWhiteSpace(ddlMasterFileLocation.SelectedValue.ToString()) ? (int?)Convert.ToInt32(ddlMasterFileLocation.SelectedValue) : null;
            authorisedProduct.phv_email = txtPhVEmail.Text;
            authorisedProduct.phv_phone = txtPhVPhone.Text;
            authorisedProduct.authorisationcountrycode_FK = ddlAuthorisationCountry.SelectedId;
            authorisedProduct.authorisationstatus_FK = ddlAuthorisationStatus.SelectedId;
            authorisedProduct.authorisationnumber = txtAuthorisationNumber.Text;
            authorisedProduct.comment = txtComment.Text;
            authorisedProduct.authorisationdate = ValidationHelper.IsValidDateTime(dtAuthorisationDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtAuthorisationDate.Text) : null;
            authorisedProduct.authorisationexpdate = ValidationHelper.IsValidDateTime(dtAuthorisationExpiryDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtAuthorisationExpiryDate.Text) : null;
            authorisedProduct.launchdate = ValidationHelper.IsValidDateTime(dtLaunchDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtLaunchDate.Text) : null;
            authorisedProduct.authorisationwithdrawndate = ValidationHelper.IsValidDateTime(dtWithdrawnDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtWithdrawnDate.Text) : null;
            authorisedProduct.infodate = ValidationHelper.IsValidDateTime(dtInfoDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtInfoDate.Text) : null;
            authorisedProduct.sunsetclause = ValidationHelper.IsValidDateTime(dtSunsetClause.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtSunsetClause.Text) : null;
            authorisedProduct.reserved_to = txtReservedTo.Text;
            authorisedProduct.local_codes = txtLocalCodes.Text;
            authorisedProduct.pack_size = txtPackSize.Text;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                authorisedProduct = _authorizedProductOperations.Save(authorisedProduct);

                SaveDistributors(authorisedProduct.ap_PK, auditTrailSessionToken);
                SaveIndications(authorisedProduct, auditTrailSessionToken);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, authorisedProduct.ap_PK, "AUTHORISED_PRODUCT", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, authorisedProduct.ap_PK, "AUTHORISED_PRODUCT", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return authorisedProduct;
        }

        private void SaveDistributors(int? authorisedProductFk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = "";
            var complexAuditOldValue = "";

            var oldDistributors = _authorisedProductDistributorsMn.GetAssignedDistributorsByAp(authorisedProductFk);

            // Calculate old distributors complex audit value
            foreach (var oldDistributor in oldDistributors)
            {
                if (complexAuditOldValue != "") complexAuditOldValue += "|||";
                var distributor = _organizationOperations.GetEntity(oldDistributor.organization_FK);
                if (distributor != null && !string.IsNullOrWhiteSpace(distributor.name_org)) complexAuditOldValue += distributor.name_org;
            }

            // Delete old distributors
            if (oldDistributors.Count > 0)
            {
                var oldDistributorsPks = new List<int>();
                oldDistributorsPks.AddRange(oldDistributors.Select(apDMn => apDMn.ap_organizatation_dist_mn_PK != null ? (int)apDMn.ap_organizatation_dist_mn_PK : 0));
                _authorisedProductDistributorsMn.DeleteCollection(oldDistributorsPks);
            }

            // Add new items to a new distributors list to save.
            // Calculate new distributors complex audit value
            var newDistributorsLb = lbAuDistributors.LbInputTo.Items;
            var newDistributors = new List<Ap_organizatation_dist_mn_PK>();
            foreach (ListItem newDistributor in newDistributorsLb)
            {
                newDistributors.Add(new Ap_organizatation_dist_mn_PK(null, Int32.Parse(newDistributor.Value), authorisedProductFk));
                if (complexAuditNewValue != "") complexAuditNewValue += "|||";
                complexAuditNewValue += newDistributor.Text;
            }

            // Save new distributors
            if (newDistributors.Count > 0)
            {
                _authorisedProductDistributorsMn.SaveCollection(newDistributors);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, authorisedProductFk.ToString(), "AP_ORGANIZATION_DIST_MN");
        }

        private void SaveIndications(AuthorisedProduct authorisedProduct, string auditTrailSessionToken)
        {
            var authorisedProductFk = authorisedProduct.ap_PK;
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var oldIndicationList = _meddraOperations.GetMeddraByAp(authorisedProductFk);
            oldIndicationList.Sort((i1, i2) => Indications.GetFormattedText(i1).CompareTo(Indications.GetFormattedText(i2)));

            // Calculate old indications complex audit value
            foreach (var indication in oldIndicationList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += indication.MeddraFullName;
            }

            _meddraAuthorisedProductMnOperations.DeleteMeddraByAP(authorisedProductFk);
            var indicationIdsToDeleteList = lbExtIndications.GetDbItemsIdsToDelete(oldIndicationList, "meddra_pk");
            _meddraOperations.DeleteCollection(indicationIdsToDeleteList);

            // Calculate new indications complex audit value
            var newIndicationList = lbExtIndications.GetDataEntities<Meddra_pk>();
            newIndicationList.Sort((i1, i2) => Indications.GetFormattedText(i1).CompareTo(Indications.GetFormattedText(i2)));

            foreach (Meddra_pk indication in newIndicationList)
            {
                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += indication.MeddraFullName;
            }

            foreach (var newIndication in newIndicationList.Where(newIndication => newIndication.meddra_pk < 0))
            {
                newIndication.meddra_pk = null;
            }

            if (newIndicationList.Count > 0)
            {
                newIndicationList = _meddraOperations.SaveCollection(newIndicationList);

                // Connect saved indications to a current authorised product
                var newIndicationsApMnList = newIndicationList.Select(newIndication => new Meddra_ap_mn_PK(null, authorisedProductFk, newIndication.meddra_pk)).ToList();

                if (newIndicationsApMnList.Count > 0)
                {
                    _meddraAuthorisedProductMnOperations.SaveCollection(newIndicationsApMnList);
                }
            }

            if (complexAuditNewValue != complexAuditOldValue)
            {
                authorisedProduct.Indications = complexAuditNewValue;
                _authorizedProductOperations.Save(authorisedProduct);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, authorisedProductFk.ToString(), "MEDDRA_AP_MN");
        }

        #endregion

        #region Delete

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
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
                    if (EntityContext == EntityContext.AuthorisedProduct)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/AuthorisedProductView/Preview.aspx?EntityContext={0}&idAuthProd={1}", EntityContext, _idAuthProd));
                    }
                    else if (EntityContext == EntityContext.Product)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "ProdSearch") Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if (From == "ProdPreview" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                            else if (From == "Prod" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                        }
                    }
                    else if (EntityContext == EntityContext.Default)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "AuthProd") Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext.AuthorisedProduct));
                        }
                    }
                    Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext.AuthorisedProduct));
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedAuthorisedProduct = SaveForm(null);
                        if (savedAuthorisedProduct is AuthorisedProduct)
                        {
                            var authorisedProduct = savedAuthorisedProduct as AuthorisedProduct;
                            if (authorisedProduct.ap_PK.HasValue)
                            {
                                MasterPage.OneTimePermissionToken = Permission.View;
                                Response.Redirect(string.Format("~/Views/AuthorisedProductView/Preview.aspx?EntityContext={0}&idAuthProd={1}", EntityContext.AuthorisedProduct, authorisedProduct.ap_PK));
                            }
                        }
                        Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext.AuthorisedProduct));
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

        #region Reminders

        private void LnkSetAuthorisationDateReminder_Click(object sender, EventArgs e)
        {
            SetReminder(StringOperations.GetRelatedName(dtAuthorisationDate.Label), dtAuthorisationDate.Text);
        }

        public void LnkSetAuthorisationExpiryDateReminder_Click(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(dtAuthorisationExpiryDate.Label)), dtAuthorisationExpiryDate.Text);
        }

        public void LnkSetLaunchDateReminder_Click(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(dtLaunchDate.Label)), dtLaunchDate.Text);
        }

        public void LnkSetWithdrawnDateReminder_Click(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(dtWithdrawnDate.Label)), dtWithdrawnDate.Text);
        }

        public void LnkSetSunsetClauseReminder_Click(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(dtSunsetClause.Label)), dtSunsetClause.Text);
        }

        public void Reminder_OnConfirmInputButtonProcess_Click(object sender, EventArgs e)
        {
            Reminder_PK reminder = Reminder.ReminderVs;

            reminder.user_FK = SessionManager.Instance.CurrentUser.UserID;

            reminder.navigate_url = string.Format("~/Views/AuthorisedProductView/Preview.aspx?EntityContext={0}&idAuthProd={1}", EntityContext.AuthorisedProduct, _idAuthProd);
            reminder.TableName = ReminderTableName.AUTHORISED_PRODUCT;
            reminder.entity_FK = _idAuthProd;

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

        #region Related product searcher

        /// <summary>
        /// Handles related product list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RelatedProductSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var product = _productOperations.GetEntity(e.Data);

            if (product == null || product.product_PK == null) return;

            txtSrRelatedProduct.Text = product.GetNameFormatted(string.Empty);
            txtSrRelatedProduct.SelectedEntityId = product.product_PK;
        }

        #endregion

        #region Qppv searcher

        /// <summary>
        /// Handles qppv list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void QppvSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var qppvCodeId = e.Data;
            var qppvText = PersonHelper.GetQppvNameFormatted(qppvCodeId, string.Empty);

            if (!string.IsNullOrWhiteSpace(qppvText))
            {
                txtSrQppv.Text = qppvText.Trim();
            }

            txtSrQppv.SelectedEntityId = qppvCodeId;
        }

        #endregion

        #region Local Qppv searcher

        /// <summary>
        /// Handles local qppv list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LocalQppvSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var localQppvCodeId = e.Data;
            var localQppvText = PersonHelper.GetQppvNameFormatted(localQppvCodeId, string.Empty);

            if (!string.IsNullOrWhiteSpace(localQppvText))
            {
                txtSrLocalQppv.Text = localQppvText.Trim();
            }
            txtSrLocalQppv.SelectedEntityId = localQppvCodeId;
        }

        #endregion

        #region Indications

        /// <summary>
        /// Handles Add button on indications list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbExtIndications_OnAddClick(object sender, EventArgs e)
        {
            IndicationsPopup.ShowModalForm(null);
        }

        /// <summary>
        /// Handles Edit button on indications list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbExtIndications_OnEditClick(object sender, EventArgs e)
        {
            var selectedEntity = lbExtIndications.GetFirstSelectedEntityFromData<Meddra_pk>();

            if (selectedEntity is Meddra_pk)
            {
                IndicationsPopup.ShowModalForm(selectedEntity as Meddra_pk);
            }
        }

        /// <summary>
        /// Handles Remove button on indications list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbExtIndications_OnRemoveClick(object sender, EventArgs e)
        {
            lbExtIndications.RemoveSelected<Meddra_pk>();
        }

        void IndicationsPopup_OnOkButtonClick(object sender, FormEventArgs<Meddra_pk> e)
        {
            var indication = e.Data;

            if (!indication.meddra_pk.HasValue)
            {
                indication.meddra_pk = lbExtIndications.GetNextIdForNewEntity<Meddra_pk>();

                var listItem = new ListItem(Indications.GetFormattedText(indication), indication.meddra_pk.ToString());

                if (lbExtIndications.LbInput.Items.FindByText(listItem.Text) != null) return;

                lbExtIndications.LbInput.Items.Add(listItem);
                lbExtIndications.AddEntityToData(listItem.Value, indication);
            }
            else
            {
                var listItem = lbExtIndications.LbInput.Items.FindByValue(indication.meddra_pk.ToString());
                listItem.Text = Indications.GetFormattedText(indication);
                lbExtIndications.UpdateEntityFromData(listItem.Value, indication);
            }

            lbExtIndications.LbInput.SortItemsByText();
        }

        #endregion

        #region Article 57 reporting

        void rbYnArticle57ReportingRbYnu_SelectedIndexChanged(object sender, EventArgs e)
        {
            StylizeArticle57RelevantControls(rbYnArticle57Reporting.SelectedValue);
        }

        protected void ddlAuthorisationStatusDdlInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            StylizeWithdrawnDate(rbYnArticle57Reporting.SelectedValue);
        }

        #endregion

        #endregion

        #region Support methods

        private void HideReminders()
        {
            dtAuthorisationDate.ShowReminder = false;
            dtAuthorisationExpiryDate.ShowReminder = false;
            dtLaunchDate.ShowReminder = false;
            dtWithdrawnDate.ShowReminder = false;
            dtInfoDate.ShowReminder = false;
            dtSunsetClause.ShowReminder = false;
        }

        public void RefreshReminderStatus()
        {
            var tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.AUTHORISED_PRODUCT);
            AlerterHelper.RefreshReminderStatus(_reminderOperations, MasterPage, new List<IReminderCustomControl> { dtAuthorisationDate, dtWithdrawnDate, dtLaunchDate, dtAuthorisationExpiryDate, dtSunsetClause }, tableName, _idAuthProd);
        }

        public void SetReminder(String attributeName, String attributeValue)
        {
            var reminder = new Reminder_PK
            {
                reminder_type = "Authorised product",
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
            Location_PK location;

            if (FormType == FormType.New)
            {
                if (EntityContext == EntityContext.Product)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("ProdAuthProdList", Support.CacheManager.Instance.AppLocations);
                    MasterPage.TabMenu.TabControls.Clear();
                    tabMenu.Visible = true;
                    tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                    tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
                else
                {
                    location = Support.LocationManager.Instance.ParseLocationFromUrl(Request.ExtractCurrentQuery(new List<string> { "EntityContext", "Action" }), Support.CacheManager.Instance.AppLocations);
                    MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                    MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                }
            }
            else
            {
                location = Support.LocationManager.Instance.GetLocationByName("AuthProdPreview", Support.CacheManager.Instance.AppLocations);
                MasterPage.TabMenu.TabControls.Clear();
                tabMenu.Visible = true;
                tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location;

            if (EntityContext == EntityContext.Product)
            {
                location = Support.LocationManager.Instance.GetLocationByName("ProdAuthProdList", Support.CacheManager.Instance.AppLocations);
            }
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

        private void StylizeArticle57RelevantControls(bool? isArticle57Relevant)
        {
            Article57Reporting.RemoveAllArticle57CssClasses(pnlForm);

            // Left pane
            txtEvcode.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, txtEvcode.Text, isArticle57Relevant));
            txtFullPresentationName.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, txtFullPresentationName.Text, isArticle57Relevant));
            txtProductShortName.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, txtProductShortName.Text, isArticle57Relevant));
            txtProductGenericName.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, txtProductGenericName.Text, isArticle57Relevant));
            txtProductCompanyName.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, txtProductCompanyName.Text, isArticle57Relevant));
            txtProductStrengthName.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, txtProductStrengthName.Text, isArticle57Relevant));
            txtProductFormName.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, txtProductFormName.Text, isArticle57Relevant));
            txtPackageDescription.LblName.AddCssClass(Article57Reporting.GetCssClass(false, false, txtPackageDescription.Text, isArticle57Relevant));
            txtCommentEvprm.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, txtCommentEvprm.Text, isArticle57Relevant));

            // Right pane
            ddlLicenceHolder.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, ddlLicenceHolder.HasValue(), isArticle57Relevant));
            txtSrQppv.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, txtSrQppv.Text, isArticle57Relevant));
            // Local qppv was not sylized because there was no request for it
            ddlMasterFileLocation.LblName.AddCssClass(Article57Reporting.GetCssClass(false, false, ddlMasterFileLocation.HasValue(), isArticle57Relevant));
            txtPhVEmail.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, txtPhVEmail.Text, isArticle57Relevant));
            txtPhVPhone.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, txtPhVPhone.Text, isArticle57Relevant));
            lbExtIndications.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lbExtIndications.HasValue(), isArticle57Relevant));
            ddlAuthorisationCountry.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, ddlAuthorisationCountry.HasValue(), isArticle57Relevant));
            ddlAuthorisationStatus.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, ddlAuthorisationStatus.HasValue(), isArticle57Relevant));
            txtAuthorisationNumber.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, txtAuthorisationNumber.Text, isArticle57Relevant));
            dtAuthorisationDate.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, dtAuthorisationDate.Text, isArticle57Relevant));
            StylizeWithdrawnDate(isArticle57Relevant);
            dtInfoDate.LblName.AddCssClass(Article57Reporting.GetCssClass(false, false, dtInfoDate.Text, isArticle57Relevant));
        }

        private void StylizeWithdrawnDate(bool? isArticle57Relevant)
        {
            dtWithdrawnDate.LblName.RemoveCssClassContains("article57");

            if (isArticle57Relevant != null && (bool)isArticle57Relevant && (ddlAuthorisationStatus.Text.ToLower().Contains("not valid") || ddlAuthorisationStatus.Text.ToLower().Contains("suspended")))
            {
                dtWithdrawnDate.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, dtWithdrawnDate.Text, true));
            }
            else
            {
                dtWithdrawnDate.LblName.AddCssClass(Article57Reporting.GetCssClass(true, false, dtWithdrawnDate.Text, isArticle57Relevant));
            }
        }
        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedInsertAuthorisedProduct = false;

            if (EntityContext == EntityContext.Default) isPermittedInsertAuthorisedProduct = SecurityHelper.IsPermitted(Permission.InsertAuthorisedProduct);
            if (isPermittedInsertAuthorisedProduct)
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

            Location_PK rootLocation = null;
            if (MasterPage.RefererLocation != null)
            {
                isPermittedInsertAuthorisedProduct = SecurityHelper.IsPermittedAny(new List<Permission> { Permission.InsertAuthorisedProduct, Permission.SaveAsAuthorisedProduct, Permission.EditAuthorisedProduct }, MasterPage.RefererLocation);

                if (isPermittedInsertAuthorisedProduct)
                {
                    SecurityHelper.SetControlsForReadWrite(
                                   MasterPage.ContextMenu,
                                   new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") },
                                   new List<Panel> { PnlForm },
                                   new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } }
                               );
                }
            }

            rootLocation = Support.LocationManager.Instance.GetLocationByName("Root", Support.CacheManager.Instance.AppLocations);
            if (rootLocation != null)
            {
                if (!SecurityHelper.IsPermitted(Permission.MakeApArticle57Relevant, rootLocation))
                {
                    if (MasterPage.RefererLocation == null || !SecurityHelper.IsPermitted(Permission.MakeApArticle57Relevant, MasterPage.RefererLocation))
                    {
                        rbYnArticle57Reporting.Enabled = false;
                    }
                    else rbYnArticle57Reporting.Enabled = true;
                }
                else rbYnArticle57Reporting.Enabled = true;
            }
            else rbYnArticle57Reporting.Enabled = true;

            SecurityPageSpecificMy(IsResponsibleUser);

            txtEvcode.Enabled = false;
            txtXEvprmStatus.Enabled = false;

            return true;
        }

        #endregion
    }
}