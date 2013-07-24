using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Linq;
using AspNetUI.Views.Shared.Template;
using EVMessage.Xevprm;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.OrganizationView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idOrg;

        private IOrganization_PKOperations _organizationOperations;
        private IRole_org_PKOperations _organizationRoleOperations;
        private IOrganization_in_role_Operations _organizationOrganizationRoleMnOperations;
        private ICountry_PKOperations _countryOperations;
        private IType_PKOperations _typeOperations;
        private IUSEROperations _userOperations;
        private IPerson_PKOperations _personOperations;
        private ILast_change_PKOperations _lastChangeOperations;

        #endregion

        #region Properties

        private bool IsMasterFileLocation
        {
            get { return (bool)ViewState["Organization_IsMasterFileLocation"]; }
            set { ViewState["Organization_IsMasterFileLocation"] = value; }
        }

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

            _idOrg = ValidationHelper.IsValidInt(Request.QueryString["idOrg"]) ? int.Parse(Request.QueryString["idOrg"]) : (int?)null;

            _organizationOperations = new Organization_PKDAL();
            _organizationRoleOperations = new Role_org_PKDAL();
            _organizationOrganizationRoleMnOperations = new Organization_in_role_DAL();
            _countryOperations = new Country_PKDAL();
            _typeOperations = new Type_PKDAL();
            _userOperations = new USERDAL();
            _personOperations = new Person_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            lbAuOrganizationRole.OnItemsAssigned += LbAuOrganizationRole_OnItemsAssigned;
            lbAuOrganizationRole.OnItemsUnAssigned += LbAuOrganizationRole_OnItemsUnAssigned;
        }

        private void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
            SetFormControlsDefaultsBeforeBind(null);
        }

        #endregion

        #region Fill

        private void ClearForm(object arg)
        {
            // Left pane
            txtOrganizationName.Text = String.Empty;
            txtEvcode.Text = String.Empty;
            txtMflEvCode.Text = String.Empty;
            txtOrganizationSenderId.Text = String.Empty;
            txtAddress.Text = String.Empty;
            txtCity.Text = String.Empty;
            txtState.Text = String.Empty;
            txtPostcode.Text = String.Empty;
            ddlCountry.SelectedValue = String.Empty;
            txtTelephoneNumber.Text = String.Empty;
            txtTelephoneExtension.Text = String.Empty;
            txtTelephoneCountryCode.Text = String.Empty;
            txtFaxNumber.Text = String.Empty;
            txtFaxExtension.Text = String.Empty;
            txtFaxCountryCode.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txtComment.Text = String.Empty;
            ddlType.SelectedValue = String.Empty;
            lbAuOrganizationRole.Clear();

            // Right pane
            txtCompany.Text = String.Empty;
            txtDepartment.Text = String.Empty;
            txtBuilding.Text = String.Empty;
            txtLastChange.Text = String.Empty;
        }

        private void FillFormControls(object arg)
        {
            FillDdlCountry();
            FillDdlType();
            if (FormType == FormType.New) FillLbAuRoles();
        }

        private void SetFormControlsDefaultsBeforeBind(object arg)
        {
            IsMasterFileLocation = false;
        }

        private void SetFormControlsDefaults(object arg)
        {
            EnableMflUserControls(IsMasterFileLocation);

            var listItem = ddlType.DdlInput.Items.FindByText("Licence holder");
            ddlType.SelectedValue = listItem != null ? listItem.Value : null;

            txtLastChange.Enabled = false;

            StylizeArticle57RelevantControls(true);
        }

        private void FillDdlCountry()
        {
            var countryList = _countryOperations.GetEntitiesCustomSort();
            ddlCountry.FillAdvanced(countryList, x => string.Format("{0} - {1}", x.abbreviation, x.name), x => x.country_PK);
        }

        private void FillDdlType()
        {
            var organizationTypeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.OrganizationType);
            ddlType.Fill(organizationTypeList, x => x.name, x => x.type_PK);
            ddlType.SortItemsByText();
        }

        private void FillLbAuRoles()
        {
            var organizationRolelist = _organizationRoleOperations.GetEntities();
            lbAuOrganizationRole.Fill(lbAuOrganizationRole.LbInputFrom, organizationRolelist, x => x.role_name, x => x.role_org_PK);
            lbAuOrganizationRole.LbInputFrom.SortItemsByText();
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idOrg.HasValue) return;

            var organization = _organizationOperations.GetEntity(_idOrg.Value);
            if (organization == null || !organization.organization_PK.HasValue) return;

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------

            // Organization name
            txtOrganizationName.Text = organization.name_org;

            // EV Code
            txtEvcode.Text = organization.ev_code;

            // Organization sender ID
            txtOrganizationSenderId.Text = organization.organizationsenderid_EMEA;

            // Address
            txtAddress.Text = organization.address;

            // City
            txtCity.Text = organization.city;

            // State
            txtState.Text = organization.state;

            // Postcode
            txtPostcode.Text = organization.postcode;

            // Country
            ddlCountry.SelectedId = organization.countrycode_FK;

            // Telephone number
            txtTelephoneNumber.Text = organization.tel_number;

            // Telephone extension
            txtTelephoneExtension.Text = organization.tel_extension;

            // Telephone country code
            txtTelephoneCountryCode.Text = organization.tel_countrycode;

            // Fax number
            txtFaxNumber.Text = organization.fax_number;

            // Fax extension
            txtFaxExtension.Text = organization.fax_extenstion;

            // Fax country code
            txtFaxCountryCode.Text = organization.fax_countrycode;

            // Email
            txtEmail.Text = organization.email;

            // Comment
            txtComment.Text = organization.comment;

            // Authorised product ID
            ddlType.SelectedId = organization.type_org_EMEA;

            // Organization roles
            BindLbAuRole(organization.organization_PK.Value);


            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------

            // Master file location
            BindMasterFileLocation(organization);

            // Last change
            txtLastChange.Text = LastChange.GetFormattedString(organization.organization_PK, "ORGANIZATION", _lastChangeOperations, _personOperations);

            if (Request.QueryString["XevprmValidation"] != null)
            {
                ValidateFormForXevprm(organization);
            }
        }

        private void BindMasterFileLocation(Organization_PK organization)
        {
            if (!organization.organization_PK.HasValue) return;

            var assignedOrganizationRoleList = _organizationRoleOperations.GetAssignedEntitiesByOrganization(organization.organization_PK.Value);

            if (assignedOrganizationRoleList == null || assignedOrganizationRoleList.Count == 0) return;

            if (assignedOrganizationRoleList.Find(x => x.role_name == Constant.OrganizationRoleName.MasterFileLocation) != null)
            {
                IsMasterFileLocation = true;
                EnableMflUserControls(true);

                //Master file location EV Code
                txtMflEvCode.Text = organization.mfl_evcode;

                //Company
                txtCompany.Text = organization.mflcompany;

                //Department
                txtDepartment.Text = organization.mfldepartment;

                //Building
                txtBuilding.Text = organization.mflbuilding;
            }
        }

        private void BindLbAuRole(int organizationPk)
        {
            var assignedOrganizationRoleList = _organizationRoleOperations.GetAssignedEntitiesByOrganization(organizationPk);
            lbAuOrganizationRole.LbInputTo.Fill(assignedOrganizationRoleList, x => x.role_name, x => x.role_org_PK);
            lbAuOrganizationRole.LbInputTo.SortItemsByText();

            var availableOrganizationRoleList = _organizationRoleOperations.GetAvailableEntitiesByOrganization(organizationPk);
            lbAuOrganizationRole.LbInputFrom.Fill(availableOrganizationRoleList, x => x.role_name, x => x.role_org_PK);
            lbAuOrganizationRole.LbInputFrom.SortItemsByText();
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(txtOrganizationName.Text))
            {
                errorMessage += "Organisation name can't be empty.<br />";
                txtOrganizationName.ValidationError = "Organisation name can't be empty.";
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
            txtOrganizationName.ValidationError = String.Empty;
            txtEvcode.ValidationError = String.Empty;
            txtMflEvCode.ValidationError = String.Empty;
            txtOrganizationSenderId.ValidationError = String.Empty;

            // Right pane
        }

        private void ValidateFormForXevprm(Organization_PK organization)
        {
            if (Request.QueryString["XevprmValidation"].NotIn("1", "2", "3", "4", "6")) return;

            IAuthorisedProductOperations authorisedProductOperations = new AuthorisedProductDAL();
            var authorisedProduct = ValidationHelper.IsValidInt(Request.QueryString["idAuthProdXevprm"]) ? authorisedProductOperations.GetEntity(int.Parse(Request.QueryString["idAuthProdXevprm"])) : null;

            if (authorisedProduct == null) return;

            XevprmOperationType operationType;
            Enum.TryParse(Request.QueryString["XevprmValidation"], true, out operationType);

            var validationResult = XevprmXml.ValidateAuthorisedProduct(authorisedProduct, operationType);

            if (validationResult.XevprmValidationExceptions.Count == 0) return;

            var errorControlMaping = new Dictionary<string, Control>()
                {
                {XevprmValidationRules.AP.mflcode.DataType.RuleId, txtMflEvCode},
                {XevprmValidationRules.AP.mflcode.BR3.RuleId, txtMflEvCode},
                {XevprmValidationRules.AP.mahcode.DataType.RuleId, txtEvcode},
                {XevprmValidationRules.AP.mahcode.BR2.RuleId, txtEvcode},
                {XevprmValidationRules.H.messagesenderidentifier.BR1.RuleId, txtOrganizationSenderId},
                {XevprmValidationRules.H.messagesenderidentifier.DataType.RuleId, txtOrganizationSenderId},
            };

            foreach (var error in validationResult.XevprmValidationExceptions)
            {
                if (error.XevprmValidationRuleId == null || !errorControlMaping.Keys.Contains(error.XevprmValidationRuleId)) continue;
                if (error.ReadyRootEntityType != typeof(Organization_PK) || error.ReadyRootEntityPk != _idOrg) continue;

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
            
            var organization = new Organization_PK();

            if (FormType == FormType.Edit && _idOrg.HasValue)
            {
                organization = _organizationOperations.GetEntity(_idOrg.Value);
            }

            if (organization == null) return null;

            organization.name_org = txtOrganizationName.Text;
            organization.ev_code = txtEvcode.Text;
            organization.organizationsenderid_EMEA = txtOrganizationSenderId.Text;
            organization.address = txtAddress.Text;
            organization.city = txtCity.Text;
            organization.state = txtState.Text;
            organization.postcode = txtPostcode.Text;
            organization.countrycode_FK = ddlCountry.SelectedId;
            organization.tel_number = txtTelephoneNumber.Text;
            organization.tel_extension = txtTelephoneExtension.Text;
            organization.tel_countrycode = txtTelephoneCountryCode.Text;
            organization.fax_number = txtFaxNumber.Text;
            organization.fax_extenstion = txtFaxExtension.Text;
            organization.fax_countrycode = txtFaxCountryCode.Text;
            organization.email = txtEmail.Text;
            organization.comment = txtComment.Text;
            organization.type_org_EMEA = ddlType.SelectedId;

            //Save master file location
            SaveMasterFileLocation(ref organization);

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                organization = _organizationOperations.Save(organization);

                SaveOrganizationRoles(organization.organization_PK, auditTrailSessionToken);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, organization.organization_PK, "ORGANIZATION", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, organization.organization_PK, "ORGANIZATION", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return organization;
        }

        private void SaveMasterFileLocation(ref Organization_PK organization)
        {
            organization.mfl_evcode = IsMasterFileLocation ? txtMflEvCode.Text : null;
            organization.mflcompany = IsMasterFileLocation ? txtCompany.Text : null;
            organization.mfldepartment = IsMasterFileLocation ? txtDepartment.Text : null;
            organization.mflbuilding = IsMasterFileLocation ? txtBuilding.Text : null;
        }

        private void SaveOrganizationRoles(int? organizationPk, string auditTrailSessionToken)
        {
            if (!organizationPk.HasValue) return;

            var complexAuditNewValue = "";
            var complexAuditOldValue = "";

            var organizationRoleList = _organizationRoleOperations.GetAssignedEntitiesByOrganization(organizationPk.Value);

            foreach (var organizationRole in organizationRoleList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += organizationRole.role_name;
            }

            _organizationOrganizationRoleMnOperations.DeleteByOrganization(organizationPk.Value);

            var organizationOrganizationRoleMnList = new List<Organization_in_role_>(lbAuOrganizationRole.LbInputTo.Items.Count);
            foreach (ListItem listItem in lbAuOrganizationRole.LbInputTo.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                organizationOrganizationRoleMnList.Add(new Organization_in_role_(null, organizationPk, int.Parse(listItem.Value)));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (organizationOrganizationRoleMnList.Count > 0)
            {
                _organizationOrganizationRoleMnOperations.SaveCollection(organizationOrganizationRoleMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, organizationPk.ToString(), "ORGANIZATION_IN_ROLE");
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
                    if (From == "Org") Response.Redirect(string.Format("~/Views/OrganizationView/List.aspx?EntityContext={0}", EntityContext.Organisation));
                    Response.Redirect(string.Format("~/Views/OrganizationView/List.aspx?EntityContext={0}", EntityContext.Organisation));
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedOrganisation = SaveForm(null);

                        Response.Redirect(string.Format("~/Views/OrganizationView/List.aspx?EntityContext={0}", EntityContext.Organisation));
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

        #region Organization role assignement

        private void LbAuOrganizationRole_OnItemsAssigned(object sender, FormEventArgs<List<ListItem>> e)
        {
            if (e.Data.Find(x => x.Text == Constant.OrganizationRoleName.MasterFileLocation) != null)
            {
                IsMasterFileLocation = true;
                EnableMflUserControls(true);
            }
        }

        private void LbAuOrganizationRole_OnItemsUnAssigned(object sender, FormEventArgs<List<ListItem>> e)
        {
            if (e.Data.Find(x => x.Text == Constant.OrganizationRoleName.MasterFileLocation) != null)
            {
                IsMasterFileLocation = false;
                EnableMflUserControls(false);
            }
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
            var location = Support.LocationManager.Instance.GetLocationByName("Org", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            location = Support.LocationManager.Instance.GetLocationByName("Org", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                var topLevelParent = MasterPage.FindTopLevelParent(location);

                MasterPage.CurrentLocation = location;
                MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            }
        }

        private void StylizeArticle57RelevantControls(bool? isArticle57Relevant)
        {
            txtEvcode.LblName.AddCssClass(Article57Reporting.GetCssClass(true, false, txtEvcode.Text, isArticle57Relevant));
            txtMflEvCode.LblName.AddCssClass(Article57Reporting.GetCssClass(true, false, txtMflEvCode.Text, isArticle57Relevant));
            txtOrganizationSenderId.LblName.AddCssClass(Article57Reporting.GetCssClass(false, false, txtOrganizationSenderId.Text, isArticle57Relevant));
            ddlType.LblName.AddCssClass(Article57Reporting.GetCssClass(false, false, ddlType.HasValue(), isArticle57Relevant));
        }

        private void EnableMflUserControls(bool enable)
        {
            txtMflEvCode.Enabled = enable;
            txtCompany.Enabled = enable;
            txtDepartment.Enabled = enable;
            txtBuilding.Enabled = enable;
        }


        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            Location_PK parentLocation = null;
            var isPermittedInsertOrganisation = false;
            if (EntityContext == EntityContext.Organisation)
            {
                parentLocation = Support.LocationManager.Instance.GetLocationByName("Org", Support.CacheManager.Instance.AppLocations);
                if (FormType == FormType.New) isPermittedInsertOrganisation = SecurityHelper.IsPermitted(Permission.InsertOrganisation, parentLocation);
                else if (FormType == FormType.Edit)
                {
                    if(EntityContext == EntityContext.Organisation)
                    {
                        isPermittedInsertOrganisation = SecurityHelper.IsPermitted( Permission.EditOrganisation, parentLocation);
                        var refererLocation = Support.LocationManager.GetRefererLocation();
                        if (!isPermittedInsertOrganisation && refererLocation != null) isPermittedInsertOrganisation = SecurityHelper.IsPermitted(Permission.EditOrganisation, refererLocation);
                    }
                }
            }

            if (isPermittedInsertOrganisation)
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
            
            txtLastChange.Enabled = false;

            EnableMflUserControls(IsMasterFileLocation);
            
            return true;
        }

        #endregion
    }
}