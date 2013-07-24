using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Transactions;
using System.Web.UI;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Form;
using AspNetUI.Views.Shared.Interface;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using System;
using EVMessage.Xevprm;
using Ready.Model;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.ProductView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idProd;
        private bool? _isResponsibleUser;

        private IProduct_PKOperations _productOperations;
        private IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        private IProduct_mn_PKOperations _productPharmaceuticalProductMnOperations;
        private IAuthorisedProductOperations _authorisedProductOperations;
        private IPerson_PKOperations _personOperations;
        private IType_PKOperations _typeOperations;
        private IOrganization_PKOperations _organizationOperations;
        private ICountry_PKOperations _countryOperations;
        private IProduct_country_mn_PKOperations _productCountryMnOperations;
        private IDomain_PKOperations _domainOperations;
        private IProduct_domain_mn_PKOperations _productDomainMnOperations;
        private IAtc_PKOperations _atcOperations;
        private IProduct_atc_mn_PKOperations _productAtcMnOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private IOrg_in_type_for_manufacturerOperations _manufacturerOperations;
        private IOrg_in_type_for_partnerOperations _partnerOperations;
        private IUSEROperations _userOperations;
        private IReminder_PKOperations _reminderOperations;
        private IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations;
        private IReminder_date_PKOperations _reminderDateOperations;
        private IProduct_packaging_material_mn_PKOperations _productPackagingMaterialMnOperations;

        public virtual event EventHandler<EventArgs> OnTopMenuChange;

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

            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;

            _productOperations = new Product_PKDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _productPharmaceuticalProductMnOperations = new Product_mn_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();
            _personOperations = new Person_PKDAL();
            _typeOperations = new Type_PKDAL();
            _organizationOperations = new Organization_PKDAL();
            _countryOperations = new Country_PKDAL();
            _productCountryMnOperations = new Product_country_mn_PKDAL();
            _domainOperations = new Domain_PKDAL();
            _productDomainMnOperations = new Product_domain_mn_PKDAL();
            _atcOperations = new Atc_PKDAL();
            _productAtcMnOperations = new Product_atc_mn_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _manufacturerOperations = new Org_in_type_for_manufacturer_DAL();
            _partnerOperations = new Org_in_type_for_partner_DAL();
            _userOperations = new USERDAL();
            _reminderOperations = new Reminder_PKDAL();
            _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            _reminderDateOperations = new Reminder_date_PKDAL();
            _productPackagingMaterialMnOperations = new Product_packaging_material_mn_PKDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            lbSrPharmaceuticalProducts.Searcher.OnOkButtonClick += LbSrPharmaceuticalProducts_OnOkButtonClick;
            lbSrPharmaceuticalProducts.OnNewClick += LbSrPharmaceuticalProducts_OnNewClick;

            lbSrDrugAtcs.Searcher.OnOkButtonClick += LbSrDrugAtcs_OnOkButtonClick;

            lbExtManufacturers.OnAddClick += LbExtManufacturers_OnAddClick;
            lbExtManufacturers.OnEditClick += LbExtManufacturers_OnEditClick;
            lbExtManufacturers.OnRemoveClick += LbExtManufacturers_OnRemoveClick;
            ManufacturerPopup.OnOkButtonClick += ManufacturerPopup_OnOkButtonClick;

            lbExtPartners.OnAddClick += LbExtPartnersOnOnAddClick;
            lbExtPartners.OnEditClick += LbExtPartners_OnEditClick;
            lbExtPartners.OnRemoveClick += LbExtPartners_OnRemoveClick;
            PartnerPopup.OnOkButtonClick += PartnerPopup_OnOkButtonClick;

            lbAuCountries.OnAssignClick += lbAuCountries_OnAssignClick;
            lbAuCountries.OnUnassignClick += lbAuCountries_OnUnassignClick;

            dtNextDlp.LnkSetReminder.Click += DtNextDlpSetReminder_OnClick;
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
            lblPrvProduct.Text = Constant.ControlDefault.LbPrvText;

            txtProductName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            ddlResponsibleUser.SelectedValue = string.Empty;
            lbSrPharmaceuticalProducts.Clear();
            txtProductNumber.Text = string.Empty;
            ddlAuthorisationProcedure.SelectedValue = string.Empty;
            lbSrDrugAtcs.Text = string.Empty;
            rbYnOrphanDrug.RbYn.SelectedIndex = -1;
            rbYnIntensiveMonitoring.RbYn.SelectedIndex = -1;
            ddlClient.SelectedValue = string.Empty;
            ddlClientGroup.SelectedValue = string.Empty;
            lbAuDomains.Clear();
            ddlType.Text = string.Empty;
            txtProductId.Text = string.Empty;
            lbAuCountries.Clear();
            ddlRegion.SelectedValue = string.Empty;
            lbExtManufacturers.Clear();
            lbExtPartners.Clear();
            txtComment.Text = string.Empty;
            txtPsurCycle.Text = string.Empty;
            dtNextDlp.Text = string.Empty;
            txtBatchSize.Text = string.Empty;
            txtPackSize.Text = string.Empty;
            ddlStorageConditions.SelectedValue = string.Empty;
            lbAuPackagingMaterials.Clear();
        }

        private void FillFormControls(object arg)
        {
            FillDdlResponsibleUser();
            FillDdlAuthorisationProcedure();
            FillDdlClient();
            FillDdlClientGroup();
            FillDdlType();
            FillDdlRegion();
            FillDdlStorageConditions();

            if (FormType == FormType.New)
            {
                FillLbAuDomains();
                FillLbAuCountries();
                FillLbAuPackagingMaterials();
            }
        }

        private void SetFormControlsDefaults(object arg)
        {
            lblPrvProduct.Visible = FormType != FormType.New;

            if (FormType == FormType.New || FormType == FormType.SaveAs)
            {
                var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                ddlResponsibleUser.SelectedValue = user != null ? user.Person_FK : null;
            }

            if (FormType == FormType.New)
            {
                StylizeArticle57RelevantControls(true);

                HideReminders();
            }
            else if (FormType == FormType.SaveAs)
            {
                HideReminders();
            }

            var rootLocation = AspNetUI.Support.LocationManager.Instance.GetLocationByName("Root", AspNetUI.Support.CacheManager.Instance.AppLocations);
            if (rootLocation == null || !SecurityHelper.IsPermitted(Permission.AuthProdAndProdAdditionalAttributes, rootLocation))
            {
                ddlClientGroup.Visible = false;
                ddlRegion.Visible = false;
                txtBatchSize.Visible = false;
                txtPackSize.Visible = false;
                ddlStorageConditions.Visible = false;
                lbAuPackagingMaterials.Visible = false;
            }

            BindDynamicControls(null);
        }

        private void FillDdlResponsibleUser()
        {
            var responsibleUserList = _personOperations.GetEntitiesByRoleName(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, x => x.FullName, x => x.person_PK);
            ddlResponsibleUser.SortItemsByText();
        }

        private void FillDdlAuthorisationProcedure()
        {
            var authorisationProcedureList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.AuthorisationProcedure);
            ddlAuthorisationProcedure.Fill(authorisationProcedureList, x => x.name, x => x.type_PK);
            ddlAuthorisationProcedure.SortItemsByText();
        }

        private void FillDdlClient()
        {
            var clientList = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.Client);
            ddlClient.Fill(clientList, x => x.name_org, x => x.organization_PK);
            ddlClient.SortItemsByText();
        }

        private void FillDdlClientGroup()
        {
            var clientGroupList = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.ClientGroup);
            ddlClientGroup.Fill(clientGroupList, x => x.name_org, x => x.organization_PK);
            ddlClientGroup.SortItemsByText();
        }

        private void FillDdlType()
        {
            var typeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.Type);
            ddlType.Fill(typeList, x => x.name, x => x.type_PK);
            ddlType.SortItemsByText();
        }

        private void FillDdlRegion()
        {
            var typeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.Region);
            ddlRegion.Fill(typeList, x => x.name, x => x.type_PK);
            ddlRegion.SortItemsByText();
        }

        private void FillDdlStorageConditions()
        {
            var typeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.StorageConditions);
            ddlStorageConditions.Fill(typeList, x => x.name, x => x.type_PK);
            ddlStorageConditions.SortItemsByText();
        }

        private void FillLbAuCountries()
        {
            var countryList = _countryOperations.GetEntitiesCustomSort();
            countryList.SortByField(x => x.custom_sort_ID);
            lbAuCountries.Fill(countryList, lbAuCountries.LbInputFrom, Constant.Countries.DisplayNameFormat, "country_PK");
        }

        private void FillLbAuDomains()
        {
            var domainList = _domainOperations.GetEntities();
            lbAuDomains.LbInputFrom.Fill(domainList, x => x.name, x => x.domain_PK);
            lbAuDomains.LbInputFrom.SortItemsByText();
        }

        private void FillLbAuPackagingMaterials()
        {
            var packagingMaterialList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.PackagingMaterial);
            lbAuPackagingMaterials.LbInputFrom.Fill(packagingMaterialList, x => x.name, x => x.type_PK);
            lbAuPackagingMaterials.LbInputFrom.SortItemsByText();
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idProd.HasValue) return;

            var product = _productOperations.GetEntity(_idProd);

            if (product == null || !product.product_PK.HasValue) return;

            // Entity
            lblPrvProduct.Text = product.name;

            // Product name
            txtProductName.Text = product.name;

            // Product description
            txtDescription.Text = product.description;

            // Responsible user
            ddlResponsibleUser.SelectedValue = product.responsible_user_person_FK;

            // Pharmaceutical products
            BindPharmaceuticalProducts(product.product_PK.Value);

            // Product number
            txtProductNumber.Text = product.product_number;

            // Authorisation procedure
            ddlAuthorisationProcedure.SelectedValue = product.authorisation_procedure;

            // Drug atcs
            BindDrugAtcs(product.product_PK.Value);

            // Orphan drug
            if (product.orphan_drug.HasValue)
            {
                rbYnOrphanDrug.SelectedValue = (bool)product.orphan_drug;
            }

            // Intensive monitoring
            if (product.intensive_monitoring.HasValue)
            {
                rbYnIntensiveMonitoring.SelectedValue = product.intensive_monitoring == 1;
            }

            // Client
            ddlClient.SelectedValue = product.client_organization_FK;

            // Client group
            ddlClientGroup.SelectedValue = product.client_group_FK;

            // Domains
            BindDomains(product.product_PK.Value);

            // Type
            ddlType.SelectedValue = product.type_product_FK;

            // Product ID
            txtProductId.Text = product.product_ID;

            // Countries
            BindCountries(product.product_PK.Value);

            // Region
            ddlRegion.SelectedValue = product.region_FK;

            // Manufacturers
            BindManufacturers(product.product_PK.Value);

            // Partners
            BindPartners(product.product_PK.Value);

            // Comment
            txtComment.Text = product.comments;

            // PSUR Cycle
            txtPsurCycle.Text = product.psur;

            // Next DLP
            dtNextDlp.Text = product.next_dlp.HasValue ? product.next_dlp.Value.ToString(Constant.DateTimeFormat) : string.Empty;

            // Batch size
            txtBatchSize.Text = product.batch_size;

            // Pack size
            txtPackSize.Text = product.pack_size;

            // Storage conditions
            ddlStorageConditions.SelectedValue = product.storage_conditions_FK;

            // Packaging materials
            BindPackagingMaterials(product.product_PK.Value);

            // Coloring
            var numberOfArticle57AuthProd = _authorisedProductOperations.IsArticle57(_idProd);
            StylizeArticle57RelevantControls(numberOfArticle57AuthProd > 0);

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = product.responsible_user_person_FK == user.Person_FK;

            if (Request.QueryString["XevprmValidation"] != null)
            {
                ValidateFormForXevprm(product);
            }
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idProd.HasValue) return;

            RefreshReminderStatus();
        }

        private void BindPharmaceuticalProducts(int productPk)
        {
            var pharmaceuticalProductList = _pharmaceuticalProductOperations.GetEntitiesByProduct(productPk);
            lbSrPharmaceuticalProducts.Fill(pharmaceuticalProductList, x => x.name, x => x.pharmaceutical_product_PK);
            lbSrPharmaceuticalProducts.LbInput.SortItemsByText();
        }

        private void BindDrugAtcs(int productPk)
        {
            var atcList = _atcOperations.GetEntitiesByProduct(productPk);
            lbSrDrugAtcs.FillAdvanced(atcList, x => x.GetNameFormatted(), x => x.atc_PK);
            lbSrDrugAtcs.LbInput.SortItemsByText();
        }

        private void BindDomains(int productPk)
        {
            var domainAvailableList = _domainOperations.GetAvailableEntitiesByProduct(productPk);
            lbAuDomains.LbInputFrom.Fill(domainAvailableList, x=> x.name, x=> x.domain_PK);
            lbAuDomains.LbInputFrom.SortItemsByText();

            var domainAssignedList = _domainOperations.GetAssignedEntitiesByProduct(productPk);
            lbAuDomains.LbInputTo.Fill(domainAssignedList, x => x.name, x => x.domain_PK);
            lbAuDomains.LbInputTo.SortItemsByText();
        }

        private void BindCountries(int productPk)
        {
            var countryAvailableList = _countryOperations.GetAvailableEntitiesByProduct(productPk);
            lbAuCountries.Fill(countryAvailableList, lbAuCountries.LbInputFrom, Constant.Countries.DisplayNameFormat, "country_PK");
            countryAvailableList.SortByField(x=> x.custom_sort_ID);

            var countryAssignedList = _countryOperations.GetAssignedEntitiesByProduct(productPk);
            lbAuCountries.Fill(countryAssignedList, lbAuCountries.LbInputTo, Constant.Countries.DisplayNameFormat, "country_PK");
            countryAssignedList.SortByField(x => x.custom_sort_ID);
        }

        private void BindManufacturers(int productPk)
        {
            var manufacturerList = _manufacturerOperations.GetEntitiesByProduct(productPk);

            foreach (var manufacturer in manufacturerList)
            {
                manufacturer.ManufacturerName = manufacturer.GetNameFormatted(string.Empty);
            }

            lbExtManufacturers.Fill(manufacturerList, x=> x.ManufacturerName, x=> x.org_in_type_for_manufacturer_ID, true);
            lbExtManufacturers.LbInput.SortItemsByText();
        }

        private void BindPartners(int productPk)
        {
            var partnerList = _partnerOperations.GetEntitiesByProduct(productPk);

            foreach (var partner in partnerList)
            {
                partner.PartnerName = partner.GetNameFormatted(string.Empty);
            }

            lbExtPartners.Fill(partnerList, x=> x.PartnerName, x=> x.org_in_type_for_partner_ID, true);
            lbExtPartners.LbInput.SortItemsByText();
        }

        private void BindPackagingMaterials(int productPk)
        {
            var packagingMaterialAvailableList = _typeOperations.GetAvailablePackagingMaterialsForProduct(productPk);
            lbAuPackagingMaterials.LbInputFrom.Fill(packagingMaterialAvailableList, x=> x.name, x=> x.type_PK);
            lbAuPackagingMaterials.LbInputFrom.SortItemsByText();

            var packagingMaterialAssignedList = _typeOperations.GetAssignedPackagingMaterialsForProduct(productPk);
            lbAuPackagingMaterials.LbInputTo.Fill(packagingMaterialAssignedList, x => x.name, x => x.type_PK);
            lbAuPackagingMaterials.LbInputTo.SortItemsByText();
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                errorMessage += "Name can't be empty.<br />";
                txtProductName.ValidationError = "Name can't be empty.";
            }

            if (lbSrPharmaceuticalProducts.LbInput.Items.Count == 0)
            {
                errorMessage += "Pharmaceutical products can't be empty.<br />";
                lbSrPharmaceuticalProducts.ValidationError = "Pharmaceutical products can't be empty.";
            }

            if (!ddlClient.SelectedId.HasValue)
            {
                errorMessage += "Client can't be empty.<br />";
                ddlClient.ValidationError = "Client can't be empty.";
            }

            if (lbAuDomains.LbInputTo.Items.Count == 0)
            {
                errorMessage += "You must select at least one domain.<br />";
                lbAuDomains.ValidationError = "You must select at least one domain.";
            }

            if (lbAuCountries.LbInputTo.Items.Count == 0)
            {
                errorMessage += "You must select at least one country.<br />";
                lbAuCountries.ValidationError = "You must select at least one country.";
            }

            if (!string.IsNullOrWhiteSpace(dtNextDlp.Text) && !ValidationHelper.IsValidDateTime(dtNextDlp.Text, CultureInfoHr))
            {
                errorMessage += "Next DLP is not a valid date.<br />";
                dtNextDlp.ValidationError = "Next DLP is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                MasterPage.ModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }

            return true;
        }

        private void ClearValidationErrors()
        {
            txtProductName.ValidationError = string.Empty;
            lbSrPharmaceuticalProducts.ValidationError = string.Empty;
            ddlClient.ValidationError = string.Empty;
            lbAuDomains.ValidationError = string.Empty;
            lbAuCountries.ValidationError = string.Empty;
            dtNextDlp.ValidationError = string.Empty;
        }

        private void ValidateFormForXevprm(Product_PK entity)
        {
            if (Request.QueryString["XevprmValidation"].NotIn("1", "2", "3", "4", "6")) return;

            var authorisedProduct = ValidationHelper.IsValidInt(Request.QueryString["idAuthProdXevprm"]) ? _authorisedProductOperations.GetEntity(int.Parse(Request.QueryString["idAuthProdXevprm"])) : null;

            if (authorisedProduct == null) return;

            XevprmOperationType operationType;
            Enum.TryParse(Request.QueryString["XevprmValidation"], true, out operationType);

            var validationResult = XevprmXml.ValidateAuthorisedProduct(authorisedProduct, operationType);

            if (validationResult.XevprmValidationExceptions.Count == 0) return;

            var errorControlMaping = new Dictionary<string, Control>()
                {
                {XevprmValidationRules.AP.PP.Cardinality.RuleId, lbSrPharmaceuticalProducts},
                {XevprmValidationRules.AP.ATC.Cardinality.RuleId, lbSrDrugAtcs},
                {XevprmValidationRules.AP.ATC.BR1.RuleId, lbSrDrugAtcs},
                {XevprmValidationRules.AP.ATC.atccode.Cardinality.RuleId, lbSrDrugAtcs},
                {XevprmValidationRules.AP.ATC.atccode.DataType.RuleId, lbSrDrugAtcs},
                {XevprmValidationRules.AP.authorisation.mrpnumber.DataType.RuleId, txtProductNumber},
                {XevprmValidationRules.AP.authorisation.mrpnumber.BR1.RuleId, txtProductNumber},
                {XevprmValidationRules.AP.authorisation.authorisationprocedure.Cardinality.RuleId, ddlAuthorisationProcedure},
                {XevprmValidationRules.AP.authorisation.authorisationprocedure.DataType.RuleId, ddlAuthorisationProcedure},
                {XevprmValidationRules.AP.authorisation.intensivemonitoring.DataType.RuleId, rbYnIntensiveMonitoring},
                {XevprmValidationRules.AP.authorisation.orphandrug.BR1.RuleId, rbYnOrphanDrug}
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

            var product = new Product_PK();

            if (FormType == FormType.Edit && _idProd.HasValue)
            {
                product = _productOperations.GetEntity(_idProd);
            }

            if (product == null) return null;

            //Product name
            product.name = txtProductName.Text;

            //Product description
            product.description = txtDescription.Text;

            //Responsible user
            product.responsible_user_person_FK = ddlResponsibleUser.SelectedId;

            //Product number
            product.product_number = txtProductNumber.Text;

            //Authorisation procedure
            product.authorisation_procedure = ddlAuthorisationProcedure.SelectedId;

            //Orphan drug
            product.orphan_drug = rbYnOrphanDrug.SelectedItem != null ? rbYnOrphanDrug.SelectedValue : (bool?)null;

            //Intensive monitoring
            product.intensive_monitoring = rbYnIntensiveMonitoring.SelectedItem != null ? rbYnIntensiveMonitoring.SelectedValue != null && rbYnIntensiveMonitoring.SelectedValue.Value ? 1 : 2 : (int?)null;

            //Client
            product.client_organization_FK = ddlClient.SelectedId;

            //Client group
            product.client_group_FK = ddlClientGroup.SelectedId;

            //Type
            product.type_product_FK = ddlType.SelectedId;

            //Product ID
            product.product_ID = txtProductId.Text;

            //Region
            product.region_FK = ddlRegion.SelectedId;

            //Comment
            product.comments = txtComment.Text;

            //PSUR Cycle
            product.psur = txtPsurCycle.Text;

            //Next DLP
            product.next_dlp = ValidationHelper.IsValidDateTime(dtNextDlp.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtNextDlp.Text) : null;

            //Batch size
            product.batch_size = txtBatchSize.Text;

            //Pack size
            product.pack_size = txtPackSize.Text;

            //Storage conditions
            product.storage_conditions_FK = ddlStorageConditions.SelectedId;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                product = _productOperations.Save(product);

                if (!product.product_PK.HasValue) return null;

                SavePharmaceuticalProducts(product.product_PK.Value, auditTrailSessionToken);
                SaveDrugAtcs(product.product_PK.Value, auditTrailSessionToken);
                SaveDomains(product.product_PK.Value, auditTrailSessionToken);
                SaveCountries(product.product_PK.Value, auditTrailSessionToken);
                SaveManufacturers(product.product_PK.Value, auditTrailSessionToken);
                SavePartners(product.product_PK.Value, auditTrailSessionToken);
                SavePackagingMaterials(product.product_PK.Value, auditTrailSessionToken);

                _pharmaceuticalProductOperations.UpdateCalculatedColumnByProduct(product.product_PK.Value, Pharmaceutical_product_PK.CalculatedColumn.Products);
                _productOperations.UpdateCalculatedColumn(product.product_PK.Value, Product_PK.CalculatedColumn.All);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, product.product_PK, "PRODUCT", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, product.product_PK, "PRODUCT", _lastChangeOperations, _userOperations);

                ts.Complete();
            }
            return product;
        }

        private void SavePharmaceuticalProducts(int productPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var pharmaceuticalProductList = _pharmaceuticalProductOperations.GetEntitiesByProduct(productPk);

            foreach (var pharmaceuticalProduct in pharmaceuticalProductList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += pharmaceuticalProduct.name;
            }

            _productPharmaceuticalProductMnOperations.DeleteByProductPK(productPk);

            var productPharmaceuticalProductMnList = new List<Product_mn_PK>(lbSrPharmaceuticalProducts.LbInput.Items.Count);

            foreach (ListItem listItem in lbSrPharmaceuticalProducts.LbInput.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                productPharmaceuticalProductMnList.Add(new Product_mn_PK(null, productPk, int.Parse(listItem.Value)));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (productPharmaceuticalProductMnList.Count > 0)
            {
                _productPharmaceuticalProductMnOperations.SaveCollection(productPharmaceuticalProductMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, productPk.ToString(), "PRODUCT_PP_MN");
        }

        private void SaveDrugAtcs(int productPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var atcList = _atcOperations.GetEntitiesByProduct(productPk);
            foreach (var atc in atcList)
            {
                var atcCode = !string.IsNullOrWhiteSpace(atc.atccode) ? atc.atccode : Constant.UnknownValue;
                var atcName = !string.IsNullOrWhiteSpace(atc.name) ? atc.name : Constant.UnknownValue;
                var text = string.Format("{0} ({1})", atcCode, atcName);
                atc.atccode = text;
            }

            atcList.Sort((atc1, atc2) => atc1.atccode.CompareTo(atc2.atccode));

            foreach (var atc in atcList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += atc.atccode;
            }

            _productAtcMnOperations.DeleteByProductPK(productPk);

            var productAtcMnList = new List<Product_atc_mn_PK>(lbSrDrugAtcs.LbInput.Items.Count);

            foreach (ListItem listItem in lbSrDrugAtcs.LbInput.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                productAtcMnList.Add(new Product_atc_mn_PK(null, productPk, int.Parse(listItem.Value)));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (productAtcMnList.Count > 0)
            {
                _productAtcMnOperations.SaveCollection(productAtcMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, productPk.ToString(), "PRODUCT_ATC_MN");
        }

        private void SaveDomains(int productPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var domainList = _domainOperations.GetAssignedEntitiesByProduct(productPk);
            domainList.Sort((d1, d2) => d1.name.CompareTo(d2.name));

            foreach (var domain in domainList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += domain.name;
            }

            _productDomainMnOperations.DeleteByProductPK(productPk);

            var productDomainMnList = new List<Product_domain_mn_PK>(lbAuDomains.LbInputTo.Items.Count);

            foreach (ListItem listItem in lbAuDomains.LbInputTo.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                productDomainMnList.Add(new Product_domain_mn_PK(null, productPk, int.Parse(listItem.Value)));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (productDomainMnList.Count > 0)
            {
                _productDomainMnOperations.SaveCollection(productDomainMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, productPk.ToString(), "PRODUCT_DOMAIN_MN");
        }

        private void SaveCountries(int productPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var countryList = _countryOperations.GetAssignedEntitiesByProduct(productPk);
            countryList.Sort((c1, c2) => c1.custom_sort_ID.HasValue && c2.custom_sort_ID.HasValue ? c1.custom_sort_ID.Value.CompareTo(c2.custom_sort_ID.Value) : 0);

            foreach (var country in countryList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += ", ";
                complexAuditOldValue += country.abbreviation;
            }

            _productCountryMnOperations.DeleteByProductPK(productPk);

            var productCountryMnList = new List<Product_country_mn_PK>(lbAuCountries.LbInputTo.Items.Count);

            foreach (ListItem listItem in lbAuCountries.LbInputTo.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                productCountryMnList.Add(new Product_country_mn_PK(null, int.Parse(listItem.Value), productPk));

                var country = _countryOperations.GetEntity(listItem.Value);
                if (country != null && !string.IsNullOrWhiteSpace(country.abbreviation))
                {
                    if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += ", ";
                    complexAuditNewValue += country.abbreviation;
                }
            }

            if (productCountryMnList.Count > 0)
            {
                _productCountryMnOperations.SaveCollection(productCountryMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, productPk.ToString(), "PRODUCT_COUNTRY_MN");
        }

        private void SaveManufacturers(int productPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var manufacturerList = _manufacturerOperations.GetEntitiesByProduct(productPk);
            manufacturerList.Sort((m1, m2) => m1.GetNameFormatted().CompareTo(m2.GetNameFormatted()));

            foreach (var manufacturer in manufacturerList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += manufacturer.GetNameFormatted(string.Empty);
            }

            var manufacturerIdListToDelete = lbExtManufacturers.GetDbItemsIdsToDelete(manufacturerList, "org_in_type_for_manufacturer_ID");
            _manufacturerOperations.DeleteCollection(manufacturerIdListToDelete);

            foreach (ListItem listItem in lbExtManufacturers.LbInput.Items)
            {
                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            manufacturerList = lbExtManufacturers.GetDataEntities<Org_in_type_for_manufacturer>();

            foreach (var manufacturer in manufacturerList)
            {
                if (FormType == FormType.SaveAs || manufacturer.org_in_type_for_manufacturer_ID < 0)
                    manufacturer.org_in_type_for_manufacturer_ID = null;
                manufacturer.product_FK = productPk;
            }

            if (manufacturerList.Count > 0)
            {
                _manufacturerOperations.SaveCollection(manufacturerList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, productPk.ToString(), "PRODUCT_MANUFACTURER_MN");
        }

        private void SavePartners(int productPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var partnerList = _partnerOperations.GetEntitiesByProduct(productPk);
            partnerList.Sort((p1, p2) => p1.GetNameFormatted().CompareTo(p2.GetNameFormatted()));

            foreach (var partner in partnerList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += partner.GetNameFormatted(string.Empty);
            }

            var partnerIdListToDelete = lbExtManufacturers.GetDbItemsIdsToDelete(partnerList, "org_in_type_for_partner_ID");
            _partnerOperations.DeleteCollection(partnerIdListToDelete);

            foreach (ListItem listItem in lbExtPartners.LbInput.Items)
            {
                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            partnerList = lbExtPartners.GetDataEntities<Org_in_type_for_partner>();

            foreach (var partner in partnerList)
            {
                if (FormType == FormType.SaveAs || partner.org_in_type_for_partner_ID < 0)
                    partner.org_in_type_for_partner_ID = null;
                partner.product_FK = productPk;
            }

            if (partnerList.Count > 0)
            {
                _partnerOperations.SaveCollection(partnerList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, productPk.ToString(), "PRODUCT_PARTNER_MN");
        }

        private void SavePackagingMaterials(int productPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var packagingMaterialList = _typeOperations.GetAssignedPackagingMaterialsForProduct(productPk);
            packagingMaterialList.Sort((d1, d2) => d1.name.CompareTo(d2.name));

            foreach (var packagingMaterial in packagingMaterialList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += packagingMaterial.name;
            }

            _productPackagingMaterialMnOperations.DeleteByProduct(productPk);

            var productPackagingmaterialMnList = new List<Product_packaging_material_mn_PK>(lbAuPackagingMaterials.LbInputTo.Items.Count);

            foreach (ListItem listItem in lbAuPackagingMaterials.LbInputTo.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                productPackagingmaterialMnList.Add(new Product_packaging_material_mn_PK(null, productPk, int.Parse(listItem.Value)));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (productPackagingmaterialMnList.Count > 0)
            {
                _productPackagingMaterialMnOperations.SaveCollection(productPackagingmaterialMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, productPk.ToString(), "PRODUCT_PACKAGING_MATERIAL_MN");
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
                    if (PharmaceuticalProductForm.Visible)
                    {
                        PharmaceuticalProductForm.Visible = false;
                        lblPrvProduct.Visible = FormType != FormType.New;
                        pnlForm.Visible = true;
                        return;
                    }

                    if (FormType == FormType.New)
                    {
                        if (EntityContext == EntityContext.Default)
                        {
                            if (!string.IsNullOrWhiteSpace(From))
                            {
                                if (From == "Prod") Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
                            }
                        }
                    }
                    else
                    {
                        if (_idProd.HasValue)
                        {
                            Response.Redirect(string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, _idProd));
                        }
                    }
                    Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
                    break;

                case ContextMenuEventTypes.Save:
                    {
                        if (PharmaceuticalProductForm.Visible)
                        {
                            if (PharmaceuticalProductForm.ValidateForm(null))
                            {
                                var result = PharmaceuticalProductForm.SaveForm(null);
                                if (result is Pharmaceutical_product_PK)
                                {
                                    var pharmaceuticalProduct = result as Pharmaceutical_product_PK;
                                    var text = !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.UnknownValue;
                                    lbSrPharmaceuticalProducts.LbInput.Items.Add(new ListItem(text, pharmaceuticalProduct.pharmaceutical_product_PK.ToString()));
                                }

                                PharmaceuticalProductForm.Visible = false;
                                lblPrvProduct.Visible = FormType != FormType.New;
                                pnlForm.Visible = true;
                            }
                        }
                        else if (ValidateForm(null))
                        {
                            var savedProduct = SaveForm(null);

                            if (savedProduct is Product_PK)
                            {
                                var product = savedProduct as Product_PK;
                                if (product.product_PK.HasValue)
                                {
                                    MasterPage.OneTimePermissionToken = Permission.View;
                                    Response.Redirect(string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, product.product_PK));
                                }
                            }
                            Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
                        }
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

        #region Pharmaceutical product

        private void LbSrPharmaceuticalProducts_OnNewClick(object sender, EventArgs e)
        {
            lblPrvProduct.Visible = false;
            pnlForm.Visible = false;
            PharmaceuticalProductForm.Visible = true;
            PharmaceuticalProductForm.FormType = FormType.New;
            PharmaceuticalProductForm.EntityContext = EntityContext.Product;
            PharmaceuticalProductForm.InitForm(null);
            PharmaceuticalProductForm.SetFormControlsDefaults(null);
            PharmaceuticalProductForm.BindGrid();
        }

        public void LbSrPharmaceuticalProducts_OnOkButtonClick(object sender, FormEventArgs<List<int>> e)
        {
            foreach (var selectedId in lbSrPharmaceuticalProducts.Searcher.SelectedItems)
            {
                if (lbSrPharmaceuticalProducts.LbInput.Items.FindByValue(selectedId.ToString()) != null) continue;

                var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(selectedId);

                if (pharmaceuticalProduct != null)
                {
                    var text = !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.UnknownValue;

                    lbSrPharmaceuticalProducts.LbInput.Items.Add(new ListItem(text, selectedId.ToString()));
                }
            }
        }

        #endregion

        #region Drug Atcs

        private void LbSrDrugAtcs_OnOkButtonClick(object sender, FormEventArgs<List<int>> e)
        {
            foreach (var selectedId in lbSrDrugAtcs.Searcher.SelectedItems)
            {
                if (lbSrDrugAtcs.LbInput.Items.FindByValue(selectedId.ToString()) != null) continue;

                var drugAct = _atcOperations.GetEntity(selectedId);

                if (drugAct != null)
                {
                    var atcCode = !string.IsNullOrWhiteSpace(drugAct.atccode) ? drugAct.atccode : Constant.UnknownValue;
                    var atcName = !string.IsNullOrWhiteSpace(drugAct.name) ? drugAct.name : Constant.UnknownValue;
                    var text = string.Format("{0} ({1})", atcCode, atcName);

                    lbSrDrugAtcs.LbInput.Items.Add(new ListItem(text, selectedId.ToString()));
                }
            }
        }

        #endregion

        #region Manufacturers

        private void LbExtManufacturers_OnAddClick(object sender, EventArgs e)
        {
            ManufacturerPopup.ShowModalForm(null);
        }

        private void LbExtManufacturers_OnEditClick(object sender, EventArgs e)
        {
            var selectedEntity = lbExtManufacturers.GetFirstSelectedEntityFromData<Org_in_type_for_manufacturer>();

            if (selectedEntity is Org_in_type_for_manufacturer)
            {
                ManufacturerPopup.ShowModalForm(selectedEntity as Org_in_type_for_manufacturer);
            }
        }

        private void LbExtManufacturers_OnRemoveClick(object sender, EventArgs e)
        {
            lbExtManufacturers.RemoveSelected<Org_in_type_for_manufacturer>();
        }

        private void ManufacturerPopup_OnOkButtonClick(object sender, FormEventArgs<Org_in_type_for_manufacturer> e)
        {
            var manufacturer = e.Data;

            if (!manufacturer.org_in_type_for_manufacturer_ID.HasValue)
            {
                manufacturer.org_in_type_for_manufacturer_ID = lbExtManufacturers.GetNextIdForNewEntity<Org_in_type_for_manufacturer>();

                var listItem = new ListItem(manufacturer.GetNameFormatted(string.Empty), manufacturer.org_in_type_for_manufacturer_ID.ToString());

                var manufacturerList = lbExtManufacturers.GetDataEntities<Org_in_type_for_manufacturer>();

                if (manufacturerList.Exists(x =>
                    x.organization_FK == manufacturer.organization_FK &&
                    x.org_type_for_manu_FK == manufacturer.org_type_for_manu_FK &&
                    x.substance_FK == manufacturer.substance_FK)) return;

                lbExtManufacturers.LbInput.Items.Add(listItem);
                lbExtManufacturers.AddEntityToData(listItem.Value, manufacturer);
            }
            else
            {
                var listItem = lbExtManufacturers.LbInput.Items.FindByValue(manufacturer.org_in_type_for_manufacturer_ID.ToString());
                listItem.Text = manufacturer.GetNameFormatted(string.Empty);
                lbExtManufacturers.UpdateEntityFromData(listItem.Value, manufacturer);
            }

            lbExtManufacturers.LbInput.SortItemsByText();
        }

        #endregion

        #region Partners

        private void LbExtPartnersOnOnAddClick(object sender, EventArgs eventArgs)
        {
            PartnerPopup.ShowModalForm(null);
        }

        private void LbExtPartners_OnEditClick(object sender, EventArgs e)
        {
            var selectedEntity = lbExtPartners.GetFirstSelectedEntityFromData<Org_in_type_for_partner>();

            if (selectedEntity is Org_in_type_for_partner)
            {
                PartnerPopup.ShowModalForm(selectedEntity as Org_in_type_for_partner);
            }
        }

        private void LbExtPartners_OnRemoveClick(object sender, EventArgs e)
        {
            lbExtPartners.RemoveSelected<Org_in_type_for_partner>();
        }

        private void PartnerPopup_OnOkButtonClick(object sender, FormEventArgs<Org_in_type_for_partner> e)
        {
            var partner = e.Data;

            if (!partner.org_in_type_for_partner_ID.HasValue)
            {
                partner.org_in_type_for_partner_ID = lbExtPartners.GetNextIdForNewEntity<Org_in_type_for_partner>();

                var listItem = new ListItem(partner.GetNameFormatted(string.Empty), partner.org_in_type_for_partner_ID.ToString());

                var partnerList = lbExtPartners.GetDataEntities<Org_in_type_for_partner>();
                if (partnerList.Exists(x =>
                    x.organization_FK == partner.organization_FK &&
                    x.org_type_for_partner_FK == partner.org_type_for_partner_FK)) return;

                lbExtPartners.LbInput.Items.Add(listItem);
                lbExtPartners.AddEntityToData(listItem.Value, partner);
            }
            else
            {
                var listItem = lbExtPartners.LbInput.Items.FindByValue(partner.org_in_type_for_partner_ID.ToString());
                listItem.Text = partner.GetNameFormatted(string.Empty);
                lbExtPartners.UpdateEntityFromData(listItem.Value, partner);
            }

            lbExtPartners.LbInput.SortItemsByText();
        }

        #endregion

        #region Reminders

        private void Reminder_OnConfirmInputButtonProcess_Click(object sender, EventArgs e)
        {
            var reminder = Reminder.ReminderVs;

            reminder.user_FK = SessionManager.Instance.CurrentUser.UserID;

            reminder.navigate_url = string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}idProd={1}", EntityContext.Product, _idProd);
            reminder.TableName = ReminderTableName.PRODUCT;
            reminder.entity_FK = _idProd;

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

        private void DtNextDlpSetReminder_OnClick(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(dtNextDlp.Label)), dtNextDlp.Text);
        }

        #endregion

        #region Countries

        private void lbAuCountries_OnUnassignClick(object sender, EventArgs e)
        {
            lbAuCountries.LbInputTo.MoveSelectedItemsTo(lbAuCountries.LbInputFrom);
            SortCountries(lbAuCountries, lbAuCountries.LbInputFrom);
        }

        private void lbAuCountries_OnAssignClick(object sender, EventArgs e)
        {
            lbAuCountries.LbInputFrom.MoveSelectedItemsTo(lbAuCountries.LbInputTo);
            SortCountries(lbAuCountries, lbAuCountries.LbInputTo);
        }

        #endregion

        #endregion

        #region Support methods

        private void SortCountries(ListBoxAu containerListBox, System.Web.UI.WebControls.ListBox sourceListBox)
        {
            var allCountries = _countryOperations.GetEntitiesCustomSort();
            var sortedList = sourceListBox.Items.GetSortedListByField(allCountries, "country_PK", "custom_sort_ID");
            containerListBox.Fill(sortedList, sourceListBox, Constant.Countries.DisplayNameFormat, "country_PK");
        }

        private void GenerateContextMenuItems()
        {
            var contexMenu = new[]
                                 {
                                     new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"),
                                     new ContextMenuItem(ContextMenuEventTypes.Save, "Save"),
                                 };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contexMenu);
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location;

            if (PharmaceuticalProductForm.Visible)
            {
                location = Support.LocationManager.Instance.GetLocationByName("PharmProdFormNew", Support.CacheManager.Instance.AppLocations);
                tabMenu.Visible = false;
                if (location != null)
                {
                    MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                    MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
            else if (FormType == FormType.New)
            {
                location = Support.LocationManager.Instance.ParseLocationFromUrl(Request.ExtractCurrentQuery(new List<string> { "EntityContext", "Action" }), Support.CacheManager.Instance.AppLocations);
                tabMenu.Visible = false;
                if (location != null)
                {
                    MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                    MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
            else
            {
                location = Support.LocationManager.Instance.GetLocationByName("ProdPreview", Support.CacheManager.Instance.AppLocations);
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

            if (PharmaceuticalProductForm.Visible)
            {
                location = Support.LocationManager.Instance.GetLocationByName("PharmProdFormNew", Support.CacheManager.Instance.AppLocations);
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
            lbSrPharmaceuticalProducts.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lbSrPharmaceuticalProducts.HasValue(), isArticle57Relevant));
            txtProductNumber.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, txtProductNumber.Text, isArticle57Relevant));
            ddlAuthorisationProcedure.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, ddlAuthorisationProcedure.HasValue(), isArticle57Relevant));
            lbSrDrugAtcs.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lbSrDrugAtcs.HasValue(), isArticle57Relevant));
            rbYnOrphanDrug.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, Convert.ToString(rbYnOrphanDrug.SelectedValue), isArticle57Relevant));
            rbYnIntensiveMonitoring.LblName.AddCssClass(Article57Reporting.GetCssClass(false, false, Convert.ToString(rbYnIntensiveMonitoring.SelectedValue), isArticle57Relevant));
        }

        private void HideReminders()
        {
            dtNextDlp.ShowReminder = false;
        }

        public void RefreshReminderStatus()
        {
            var tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.PRODUCT);
            AlerterHelper.RefreshReminderStatus(_reminderOperations, MasterPage, new List<IReminderCustomControl> { dtNextDlp }, tableName, _idProd);
        }

        public void SetReminder(String attributeName, String attributeValue)
        {
            var reminder = new Reminder_PK
            {
                reminder_type = "Product",
                reminder_name = lblPrvProduct.Text,
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

            base.SecurityPageSpecific();

            var isPermittedInsertProduct = false;

            if (EntityContext == EntityContext.Default) isPermittedInsertProduct = SecurityHelper.IsPermitted(Permission.InsertProduct);
            if (isPermittedInsertProduct)
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
                isPermittedInsertProduct = SecurityHelper.IsPermittedAny(new List<Permission> { Permission.InsertProduct, Permission.SaveAsProduct, Permission.EditProduct }, MasterPage.RefererLocation);
                if (isPermittedInsertProduct)
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