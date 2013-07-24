using System;
using System.Collections.Generic;
using System.Data;
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

namespace AspNetUI.Views.ProductView
{
    public partial class Preview : PreviewPage
    {
        #region Declarations

        private int? _idProd;
        private bool? _isResponsibleUser;

        private IProduct_PKOperations _productOperations;
        private IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        private IAuthorisedProductOperations _authorisedProductOperations;
        private IPerson_PKOperations _personOperations;
        private IType_PKOperations _typeOperations;
        private IOrganization_PKOperations _organizationOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private ICountry_PKOperations _countryOperations;
        private IDomain_PKOperations _domainOperations;
        private IAtc_PKOperations _atcOperations;
        private IOrg_in_type_for_manufacturerOperations _manufacturerOperations;
        private IOrg_in_type_for_partnerOperations _partnerOperations;
        private IReminder_PKOperations _reminderOperations;
        private IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations;
        private IReminder_date_PKOperations _reminderDateOperations;
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
            GenerateContextMenuItems();
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

            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;

            _productOperations = new Product_PKDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();
            _personOperations = new Person_PKDAL();
            _typeOperations = new Type_PKDAL();
            _organizationOperations = new Organization_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _countryOperations = new Country_PKDAL();
            _domainOperations = new Domain_PKDAL();
            _atcOperations = new Atc_PKDAL();
            _manufacturerOperations = new Org_in_type_for_manufacturer_DAL();
            _partnerOperations = new Org_in_type_for_partner_DAL();
            _reminderOperations = new Reminder_PKDAL();
            _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            _reminderDateOperations = new Reminder_date_PKDAL();
            _userOperations = new USERDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            lblPrvNextDlp.LnkSetReminder.Click += LblPrvNextDlpSetReminder_OnClick;
            Reminder.OnConfirmInputButtonProcess_Click += Reminder_OnConfirmInputButtonProcess_Click;

            btnDelete.Click += btnDelete_OnClick;
            mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
        }

        void InitForm(object arg)
        {

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
            btnDelete.Visible = _idProd.HasValue && _productOperations.AbleToDeleteEntity(_idProd.Value);
            lblPrvPharmaceuticalProducts.PnlLinks.AddCssClass("word-wrap-break-word");

            var rootLocation = AspNetUI.Support.LocationManager.Instance.GetLocationByName("Root", AspNetUI.Support.CacheManager.Instance.AppLocations);
            if (rootLocation == null || !SecurityHelper.IsPermitted(Permission.AuthProdAndProdAdditionalAttributes, rootLocation))
            {
                lblPrvClientGroup.Visible = false;
                lblPrvRegion.Visible = false;
                lblPrvBatchSize.Visible = false;
                lblPrvPackSize.Visible = false;
                lblPrvStorageConditions.Visible = false;
                lblPrvPackagingMaterials.Visible = false;
            }

            BindDynamicControls(null);
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            if (!_idProd.HasValue) return;

            var product = _productOperations.GetEntity(_idProd);

            if (product == null || !product.product_PK.HasValue) return;

            lblPrvProduct.Text = product.name;

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------
            // Product name
            lblPrvProductName.Text = product.name;

            // Product description
            lblPrvDescription.Text = !string.IsNullOrEmpty(product.description) ? product.description : Constant.ControlDefault.LbPrvText;

            // Responsible user
            BindResponsibleUser(product.responsible_user_person_FK);

            // Pharmaceutical products
            BindPharmaceuticalProducts(product.product_PK.Value);

            // Product number
            lblPrvProductNumber.Text = !string.IsNullOrWhiteSpace(product.product_number) ? product.product_number : Constant.ControlDefault.LbPrvText;

            // Authorisation procedure
            BindAuthorisationProcedure(product.authorisation_procedure);

            // Drug atcs
            BindDrugAtcs(product.product_PK.Value);

            // Orphan drug
            lblPrvOrphanDrug.Text = product.orphan_drug.HasValue ? product.orphan_drug == true ? "Yes" : "No" : Constant.ControlDefault.LbPrvText;

            // Intensive monitoring
            lblPrvIntesiveMonitoring.Text = product.intensive_monitoring.HasValue ? product.intensive_monitoring == 1 ? "Yes" : "No" : Constant.ControlDefault.LbPrvText;

            // Client
            BindClient(product.client_organization_FK);

            // Client
            BindClientGroup(product.client_group_FK);

            // Domains
            BindDomains(product.product_PK.Value);

            // Type
            BindType(product.type_product_FK);

            // Product ID
            lblPrvProductId.Text = !string.IsNullOrWhiteSpace(product.product_ID) ? product.product_ID : Constant.ControlDefault.LbPrvText;

            // Countries
            BindCountries(product.product_PK.Value);

            // Region
            BindRegion(product.region_FK);

            // Manufacturers
            BindManufacturers(product.product_PK.Value);

            // Partners
            BindPartners(product.product_PK.Value);

            // Comment
            lblPrvComment.Text = !string.IsNullOrWhiteSpace(product.comments) ? product.comments : Constant.ControlDefault.LbPrvText;

            // PSUR Cycle
            lblPrvPsurCycle.Text = !string.IsNullOrWhiteSpace(product.psur) ? product.psur : Constant.ControlDefault.LbPrvText;

            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------
            // Next DLP
            lblPrvNextDlp.Text = product.next_dlp.HasValue ? product.next_dlp.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Last change
            lblPrvLastChange.Text = LastChange.GetFormattedString(product.product_PK, "PRODUCT", _lastChangeOperations, _personOperations);

            // Batch size
            lblPrvBatchSize.Text = !string.IsNullOrWhiteSpace(product.batch_size) ? product.batch_size : Constant.ControlDefault.LbPrvText;

            // Pack size
            lblPrvPackSize.Text = !string.IsNullOrWhiteSpace(product.pack_size) ? product.pack_size : Constant.ControlDefault.LbPrvText;

            // Storage conditions
            BindStorageConditions(product.storage_conditions_FK);

            // Packaging materials
            BindPackagingMaterials(product.product_PK.Value);
            //---------------------------------------------------------------FOOTER PANE --------------------------------------------------------------

            // Coloring
            var numberOfArticle57AuthProd = _authorisedProductOperations.IsArticle57(product.product_PK.Value);
            StylizeArticle57RelevantControls(numberOfArticle57AuthProd > 0);

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = product.responsible_user_person_FK == user.Person_FK;
        }

        private void BindPharmaceuticalProducts(int productPk)
        {
            lblPrvPharmaceuticalProducts.Text = Constant.ControlDefault.LbPrvText;

            DataSet pharProdFullNameDs = _pharmaceuticalProductOperations.GetEntitiesFullNameByProduct(productPk);

            if (pharProdFullNameDs == null || pharProdFullNameDs.Tables.Count == 0 || pharProdFullNameDs.Tables[0].Rows.Count == 0) return;

            if (!pharProdFullNameDs.Tables[0].Columns.Contains("pharmaceutical_product_PK") || !pharProdFullNameDs.Tables[0].Columns.Contains("FullName")) return;

            lblPrvPharmaceuticalProducts.ShowLinks = true;
            var pharProdFullNameList = new List<string>();

            foreach (DataRow row in pharProdFullNameDs.Tables[0].Rows)
            {
                lblPrvPharmaceuticalProducts.PnlLinks.Controls.Add(new HyperLink
                {
                    ID = string.Format("PharmaceuticalProduct_{0}", row["pharmaceutical_product_PK"]),
                    NavigateUrl = string.Format("~/Views/PharmaceuticalProductView/Preview.aspx?EntityContext={0}&idPharmProd={1}", EntityContext.PharmaceuticalProduct, row["pharmaceutical_product_PK"]),
                    Text = StringOperations.ReplaceNullOrWhiteSpace(Convert.ToString(row["FullName"]), Constant.UnknownValue)
                });
                lblPrvPharmaceuticalProducts.PnlLinks.Controls.Add(new LiteralControl("<br/>"));
                pharProdFullNameList.Add(Convert.ToString(row["FullName"]));
            }
            lblPrvPharmaceuticalProducts.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", pharProdFullNameList), Constant.ControlDefault.LbPrvText);
        }

        private void BindDrugAtcs(int productPk)
        {
            var atcList = _atcOperations.GetEntitiesByProduct(productPk);
            var atcNameList = atcList.Select(atc => atc.GetNameFormatted(Constant.ControlDefault.LbPrvText)).ToList();
            atcNameList.ForEach(item => StringOperations.ReplaceNullOrWhiteSpace(item, Constant.UnknownValue));

            lblPrvDrugAtcs.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", atcNameList), Constant.ControlDefault.LbPrvText);
        }

        private void BindDomains(int productPk)
        {
            var domainList = _domainOperations.GetAssignedEntitiesByProduct(productPk);
            var domainNameList = domainList.Select(domain => domain.name).ToList();
            domainNameList.ForEach(item => StringOperations.ReplaceNullOrWhiteSpace(item, Constant.UnknownValue));

            lblPrvDomains.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", domainNameList), Constant.ControlDefault.LbPrvText);
        }

        private void BindCountries(int productPk)
        {
            var countryList = _countryOperations.GetAssignedEntitiesByProduct(productPk);
            var countryAbbrevationList = countryList.Select(country => country.abbreviation).ToList();
            countryAbbrevationList.ForEach(item => StringOperations.ReplaceNullOrWhiteSpace(item, Constant.UnknownValue));

            lblPrvCountries.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", countryAbbrevationList), Constant.ControlDefault.LbPrvText);
        }

        private void BindManufacturers(int productPk)
        {
            var manufacturerList = _manufacturerOperations.GetEntitiesByProduct(productPk);
            var distinctManufacturerPkList = manufacturerList.Select(manufacturer => manufacturer.organization_FK).Distinct().ToList();

            var manufacturerNameList = new List<string>();
            distinctManufacturerPkList.ForEach(fk => manufacturerNameList.Add(GetManufacturerGroupNameFormated(manufacturerList.Where(x => x.organization_FK == fk).ToList())));

            lblPrvManufacturer.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(",<br>", manufacturerNameList), Constant.ControlDefault.LbPrvText);
        }

        private string GetManufacturerGroupNameFormated(List<Org_in_type_for_manufacturer> manufacturers)
        {
            var name = string.Empty;

            if (manufacturers.Count == 0) return Constant.UnknownValue;

            name = manufacturers[0].ManufacturerName;

            var manufacturerTypeNameList = new List<string>();

            foreach (var manufacturer in manufacturers)
            {
                if (!manufacturer.org_type_for_manu_FK.HasValue) continue;

                if (manufacturer.ManufacturerTypeName == "Active substance")
                {
                    manufacturerTypeNameList.Add(!string.IsNullOrWhiteSpace(manufacturer.SubstanceName) ? "Active Substance=&lt;" + manufacturer.SubstanceName + "&gt;" : "Active Substance");
                }
                else
                {
                    manufacturerTypeNameList.Add(!string.IsNullOrWhiteSpace(manufacturer.ManufacturerTypeName) ? manufacturer.ManufacturerTypeName : null);
                }
            }

            if (manufacturerTypeNameList.Any())
            {
                name += " (" + String.Join(", ", manufacturerTypeNameList) + ")";
            }

            return StringOperations.ReplaceNullOrWhiteSpace(name, Constant.UnknownValue);
        }

        private void BindPartners(int productPk)
        {
            var partnerList = _partnerOperations.GetEntitiesByProduct(productPk);
            var partnerNameList = partnerList.Select(partner => partner.GetNameFormatted(Constant.ControlDefault.LbPrvText)).ToList();
            partnerNameList.ForEach(item => StringOperations.ReplaceNullOrWhiteSpace(item, Constant.UnknownValue));

            lblPrvPartner.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(",<br>", partnerNameList), Constant.ControlDefault.LbPrvText);
        }

        private void BindPackagingMaterials(int productPk)
        {
            var packagingMaterialList = _typeOperations.GetAssignedPackagingMaterialsForProduct(productPk);
            var packagingMaterialNameList = packagingMaterialList.Select(packagingMaterial => packagingMaterial.name).ToList();
            packagingMaterialNameList.ForEach(item => StringOperations.ReplaceNullOrWhiteSpace(item, Constant.UnknownValue));

            lblPrvPackagingMaterials.Text = StringOperations.ReplaceNullOrWhiteSpace(String.Join(", ", packagingMaterialNameList), Constant.ControlDefault.LbPrvText);
        }

        private void BindFooterButtons()
        {
            if (SecurityHelper.IsPermitted(Permission.InsertDocument))
            {
                btnAddDocument.PostBackUrl = string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idProd={1}&From=ProdPreview", EntityContext.Product, _idProd);
            }
            else
            {
                btnAddDocument.Disable();
            }

            if (SecurityHelper.IsPermitted(Permission.InsertAuthorisedProduct))
            {
                btnAddAuthorisedProduct.PostBackUrl = string.Format("~/Views/AuthorisedProductView/Form.aspx?EntityContext={0}&Action=New&idProd={1}&From=ProdPreview", EntityContext.Product, _idProd);
            }
            else
            {
                btnAddAuthorisedProduct.Disable();
            }

            if (SecurityHelper.IsPermitted(Permission.InsertActivity))
            {
                btnAddActivity.PostBackUrl = string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=New&idProd={1}&From=ProdPreview", EntityContext.Product, _idProd);
            }
            else
            {
                btnAddActivity.Disable();
            }

            if (SecurityHelper.IsPermitted(Permission.InsertPharmaceuticalProduct))
            {
                btnAddPharmaceuticalProduct.PostBackUrl = string.Format("~/Views/PharmaceuticalProductView/Form.aspx?EntityContext={0}&Action=New&idProd={1}&From=ProdPreview", EntityContext.Product, _idProd);
            }
            else
            {
                btnAddPharmaceuticalProduct.Disable();
            }

            if (SecurityHelper.IsPermitted(Permission.InsertSubmissionUnit))
            {
                btnAddSubmissionUnit.PostBackUrl = string.Format("~/Views/SubmissionUnitView/Form.aspx?EntityContext={0}&Action=New&idProd={1}&From=ProdPreview", EntityContext.Product, _idProd);
            }
            else
            {
                btnAddSubmissionUnit.Disable();
            }
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idProd.HasValue) return;

            BindPharmaceuticalProducts(_idProd.Value);
           
            BindFooterButtons();

            RefreshReminderStatus();
        }

        private void BindResponsibleUser(int? responsibleUserFk)
        {
            var responsibleUser = responsibleUserFk != null ? _personOperations.GetEntity(responsibleUserFk) : null;
            lblPrvResponsibleUser.Text = responsibleUser != null && !string.IsNullOrWhiteSpace(responsibleUser.FullName) ? responsibleUser.FullName : Constant.ControlDefault.LbPrvText;
        }

        private void BindAuthorisationProcedure(int? authorisationProcedureFk)
        {
            var authProcType = authorisationProcedureFk.HasValue ? _typeOperations.GetEntity(authorisationProcedureFk) : null;
            var authProcName = authProcType != null ? authProcType.name : null;
            lblPrvAuthorisationProcedure.Text = !string.IsNullOrWhiteSpace(authProcName) ? authProcName : Constant.ControlDefault.LbPrvText;
        }

        private void BindClient(int? clientFk)
        {
            var client = clientFk.HasValue ? _organizationOperations.GetEntity(clientFk) : null;
            var clientName = client != null ? client.name_org : null;
            lblPrvClient.Text = !string.IsNullOrWhiteSpace(clientName) ? clientName : Constant.ControlDefault.LbPrvText;
        }

        private void BindClientGroup(int? clientGroupFk)
        {
            var clientGroup = clientGroupFk.HasValue ? _organizationOperations.GetEntity(clientGroupFk) : null;
            var clientGroupName = clientGroup != null ? clientGroup.name_org : null;
            lblPrvClientGroup.Text = !string.IsNullOrWhiteSpace(clientGroupName) ? clientGroupName : Constant.ControlDefault.LbPrvText;
        }

        private void BindType(int? typeFk)
        {
            var type = typeFk.HasValue ? _typeOperations.GetEntity(typeFk) : null;
            var typeName = type != null ? type.name : null;
            lblPrvType.Text = !string.IsNullOrWhiteSpace(typeName) ? typeName : Constant.ControlDefault.LbPrvText;
        }

        private void BindRegion(int? regionFk)
        {
            var region = regionFk.HasValue ? _typeOperations.GetEntity(regionFk) : null;
            var regionName = region != null ? region.name : null;
            lblPrvRegion.Text = !string.IsNullOrWhiteSpace(regionName) ? regionName : Constant.ControlDefault.LbPrvText;
        }

        private void BindStorageConditions(int? storageConditionsFk)
        {
            var storageConditions = storageConditionsFk.HasValue ? _typeOperations.GetEntity(storageConditionsFk) : null;
            var storageConditionsName = storageConditions != null ? storageConditions.name : null;
            lblPrvStorageConditions.Text = !string.IsNullOrWhiteSpace(storageConditionsName) ? storageConditionsName : Constant.ControlDefault.LbPrvText;
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

        private void DeleteEntity(int? entityPk)
        {
            if (entityPk.HasValue)
            {
                _productOperations.Delete(entityPk);
                Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
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

                        if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}{1}", EntityContext, query));
                        else if (EntityContext == EntityContext.TimeUnit || EntityContext == EntityContext.TimeUnitMy) Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext));
                        Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
                    }
                    break;

                case ContextMenuEventTypes.Edit:
                    {
                        if (EntityContext == EntityContext.Product && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Form.aspx?EntityContext={0}&Action=Edit&idProd={1}&From=ProdPreview", EntityContext, _idProd));
                        else if (EntityContext == EntityContext.TimeUnit && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Form.aspx?EntityContext={0}&Action=Edit&idProd={1}&From=TimeUnitPreview", EntityContext, _idProd));
                        else if (EntityContext == EntityContext.TimeUnitMy && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Form.aspx?EntityContext={0}&Action=Edit&idProd={1}&From=TimeUnitMyPreview", EntityContext, _idProd));
                        Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
                    }
                    break;

                case ContextMenuEventTypes.SaveAs:
                    {
                        if (EntityContext == EntityContext.Product && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Form.aspx?EntityContext={0}&Action=SaveAs&idProd={1}&From=ProdPreview", EntityContext, _idProd));
                        else if (EntityContext == EntityContext.TimeUnit && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Form.aspx?EntityContext={0}&Action=SaveAs&idProd={1}&From=TimeUnitPreview", EntityContext, _idProd));
                        else if (EntityContext == EntityContext.TimeUnitMy && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Form.aspx?EntityContext={0}&Action=SaveAs&idProd={1}&From=TimeUnitMyPreview", EntityContext, _idProd));
                        Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
                    }
                    break;

                case ContextMenuEventTypes.PreviousItem:
                    {
                        if (_idProd.HasValue)
                        {
                            Int32? prevProductPk = _productOperations.GetPrevAlphabeticalEntity(_idProd);

                            if (prevProductPk.HasValue)
                            {
                                Response.Redirect(string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, prevProductPk));
                            }
                        }

                        Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
                    }
                    break;

                case ContextMenuEventTypes.NextItem:
                    {
                        if (_idProd.HasValue)
                        {
                            Int32? nextProductPk = _productOperations.GetNextAlphabeticalEntity(_idProd);

                            if (nextProductPk.HasValue)
                            {
                                Response.Redirect(string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, nextProductPk));
                            }
                        }

                        Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
                    }
                    break;
            }
        }

        #endregion

        #region Reminders

        void Reminder_OnConfirmInputButtonProcess_Click(object sender, EventArgs e)
        {
            var reminder = Reminder.ReminderVs;

            reminder.user_FK = SessionManager.Instance.CurrentUser.UserID;

            reminder.navigate_url = string.Format("~/Views/ProductView/Preview.aspx?EntityContext=Product&idProd={0}", _idProd);
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

        private void LblPrvNextDlpSetReminder_OnClick(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(lblPrvNextDlp.Label)), lblPrvNextDlp.Text);
        }

        #endregion

        #region Delete

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_idProd);
        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContextMenuItems()
        {
            var contextMenu = new[]
            {
                new ContextMenuItem(ContextMenuEventTypes.Back, "Back"), 
                new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit"), 
                new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As"),
                new ContextMenuItem(ContextMenuEventTypes.PreviousItem, "Previous"),
                new ContextMenuItem(ContextMenuEventTypes.NextItem, "Next")
            };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);

            if (SecurityHelper.IsPermitted(Permission.View))
            {
                int? nextId = null;
                int? prevId = null;

                if (_idProd.HasValue)
                {
                    nextId = _productOperations.GetNextAlphabeticalEntity(_idProd);
                    prevId = _productOperations.GetPrevAlphabeticalEntity(_idProd);
                }

                if (prevId == null) MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.PreviousItem, "Previous") });
                if (nextId == null) MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.NextItem, "Next") });
            }
            else MasterPage.ContextMenu.SetContextMenuItemsDisabled( 
                new[] { 
                        new ContextMenuItem(ContextMenuEventTypes.NextItem, "Next"), 
                        new ContextMenuItem(ContextMenuEventTypes.PreviousItem, "Previous") 
                      });
        }

        private void GenerateTabMenuItems()
        {
            tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, MasterPage.CurrentLocation);
            tabMenu.SelectItem(MasterPage.CurrentLocation, Support.CacheManager.Instance.AppLocations);
        }

        private void StylizeArticle57RelevantControls(bool? isArticle57Relevant)
        {
            lblPrvPharmaceuticalProducts.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvPharmaceuticalProducts.Text, isArticle57Relevant));
            lblPrvProductNumber.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvProductNumber.Text, isArticle57Relevant));
            lblPrvAuthorisationProcedure.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvAuthorisationProcedure.Text, isArticle57Relevant));
            lblPrvDrugAtcs.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvDrugAtcs.Text, isArticle57Relevant));
            lblPrvOrphanDrug.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvOrphanDrug.Text, isArticle57Relevant));
            lblPrvIntesiveMonitoring.LblName.AddCssClass(Article57Reporting.GetCssClass(false, false, lblPrvIntesiveMonitoring.Text, isArticle57Relevant));
        }

        public void RefreshReminderStatus()
        {
            string tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.PRODUCT);
            AlerterHelper.RefreshReminderStatus(_reminderOperations, MasterPage, new List<IReminderCustomControl> { lblPrvNextDlp }, tableName, _idProd);
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

            if (!base.SecurityPageSpecific())
            {
                if (SecurityHelper.IsPermitted(Permission.SaveAsProduct)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });

                if (SecurityHelper.IsPermitted(Permission.EditProduct)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });

                if (SecurityHelper.IsPermitted(Permission.DeleteProduct)) StyleHelper.EnableLinkButtonsWithCssClass(PnlFooter, "Delete");
                else StyleHelper.DisableLinkButtonsWithCssClass(PnlFooter, "Delete");

                SecurityPageSpecificMy(_isResponsibleUser);
            }

            return true;
        }

        #endregion
    }
}