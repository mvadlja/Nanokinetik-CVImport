using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI;
using AspNetUI.Views.Shared.Template;
using EVMessage.Xevprm;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.PersonView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idPerson;

        private ICountry_PKOperations _countryOperations;
        private IUSEROperations _userOperations;
        private IPerson_PKOperations _personOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private IPerson_role_PKOperations _personRoleOperations;
        private IQppv_code_PKOperations _qppvCodeOperations;
        private IPerson_in_role_PKOperations _personInRoleMnOperations;
        private IAuthorisedProductOperations _authorisedProductOperations;

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

            _idPerson = ValidationHelper.IsValidInt(Request.QueryString["idPerson"]) ? int.Parse(Request.QueryString["idPerson"]) : (int?)null;

            _countryOperations = new Country_PKDAL();
            _userOperations = new USERDAL();
            _personOperations = new Person_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _personOperations = new Person_PKDAL();
            _qppvCodeOperations = new Qppv_code_PKDAL();
            _personInRoleMnOperations = new Person_in_role_PKDAL();
            _personRoleOperations = new Person_role_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            lbExtQppvCodes.OnAddClick += lbExtQppvCodes_OnAddClick;
            lbExtQppvCodes.OnEditClick += lbExtQppvCodes_OnEditClick;
            lbExtQppvCodes.OnRemoveClick += lbExtQppvCodes_OnRemoveClick;
            lbExtQppvCodes.LbInput.SelectedIndexChanged += LbExtQppvCodes_SelectedIndexChanged;
            QppvCodePopup.OnOkButtonClick += QppvCodePopup_OnOkButtonClick;
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
            ddlCountryCode.SelectedValue = String.Empty;
            txtName.Text = String.Empty;
            txtFamilyName.Text = String.Empty;
            lbExtQppvCodes.Clear();
            txtCompany.Text = String.Empty;
            txtDepartment.Text = String.Empty;
            txtBuilding.Text = String.Empty;
            txtStreet.Text = String.Empty;
            txtState.Text = String.Empty;
            txtPostcode.Text = String.Empty;
            txtCity.Text = String.Empty;
            txtPhoneCountryCode.Text = String.Empty;
            txtPhoneNumber.Text = String.Empty;
            txtPhoneExtension.Text = String.Empty;
            txtMobileCountryCode.Text = String.Empty;
            txtFaxCountryCode.Text = String.Empty;
            txtFaxNumber.Text = String.Empty;
            txtFaxExtension.Text = String.Empty;
            txtEmail.Text = String.Empty;
            txt24hTelNumber.Text = String.Empty;
            lbAuPersonRole.Clear();
        }

        private void FillFormControls(object arg)
        {
            FillDdlCountryCode();
            if (FormType == FormType.New) FillLbAuPersonRoles();
        }

        private void SetFormControlsDefaults(object arg)
        {
        }

        private void FillDdlCountryCode()
        {
            var countryCodeList = _countryOperations.GetEntitiesCustomSort();
            ddlCountryCode.FillAdvanced(countryCodeList, x => string.Format("{0} - {1}", x.abbreviation, x.name), x => x.country_PK);
        }

        private void FillLbAuPersonRoles()
        {
            var personRolelist = _personRoleOperations.GetEntities();
            lbAuPersonRole.Fill(lbAuPersonRole.LbInputFrom, personRolelist, p => p.person_name, p => p.person_role_PK);
            lbAuPersonRole.LbInputFrom.SortItemsByText();
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idPerson.HasValue) return;

            var person = _personOperations.GetEntity(_idPerson.Value);
            if (person == null || !person.person_PK.HasValue) return;

            // Entity
            lblPrvPerson.Text = person.FullName;

            // Country code
            ddlCountryCode.SelectedId = person.country_FK;

            // Name
            txtName.Text = person.name;

            // Family name
            txtFamilyName.Text = person.familyname;

            // Organization sender ID
            txtTitle.Text = person.title;

            // QPPV Codes
            BindQppvCodes(person.person_PK);

            // Company
            txtCompany.Text = person.company;

            // Department
            txtDepartment.Text = person.department;

            // Building
            txtBuilding.Text = person.building;

            // Street
            txtStreet.Text = person.street;

            // State
            txtState.Text = person.state;

            // Postcode
            txtPostcode.Text = person.postcode;

            // City
            txtCity.Text = person.city;

            // Phone country code
            txtPhoneCountryCode.Text = person.tel_countrycode;

            // Phone number
            txtPhoneNumber.Text = person.telnumber;

            // Phone extension
            txtPhoneExtension.Text = person.telextn;

            // Mobile country code
            txtMobileCountryCode.Text = person.cell_countrycode;

            // Mobile number
            txtMobileNumber.Text = person.cellnumber;

            // Fax country code
            txtFaxCountryCode.Text = person.fax_countrycode;

            // Fax number
            txtFaxNumber.Text = person.faxnumber;

            // Fax extension
            txtFaxExtension.Text = person.faxextn;

            // Email
            txtEmail.Text = person.email;

            // 24h tel. number
            txt24hTelNumber.Text = person.telnum24h;

            // Organization roles
            BindLbAuPersonRole(person.person_PK);

            // Last change
            txtLastChange.Text = LastChange.GetFormattedString(person.person_PK, "PERSON", _lastChangeOperations, _personOperations);

            StylizeArticle57RelevantControls(false);

            if (Request.QueryString["XevprmValidation"] != null)
            {
                ValidateFormForXevprm(person);
            }
        }

        private void BindQppvCodes(int? personPk)
        {
            if (!personPk.HasValue) return;

            var qppvCodesList = _qppvCodeOperations.GetQppvCodeByPerson(personPk);
            lbExtQppvCodes.Fill(qppvCodesList, "qppv_code", "qppv_code_PK", true);
            lbExtQppvCodes.LbInput.SortItemsByText();
        }

        private void BindLbAuPersonRole(int? personPk)
        {
            var assignedPersonRoleList = _personRoleOperations.GetAssignedEntitiesByPerson(personPk);
            lbAuPersonRole.LbInputTo.Fill(assignedPersonRoleList, x => x.person_name, x => x.person_role_PK);
            lbAuPersonRole.LbInputTo.SortItemsByText();

            var availablePersonRoleList = _personRoleOperations.GetAvailableEntitiesByPerson(personPk);
            lbAuPersonRole.LbInputFrom.Fill(availablePersonRoleList, x => x.person_name, x => x.person_role_PK);
            lbAuPersonRole.LbInputFrom.SortItemsByText();
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

            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                MasterPage.ModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }

            return true;
        }

        private void ClearValidationErrors()
        {
            txtName.ValidationError = String.Empty;
            lbExtQppvCodes.ValidationError = String.Empty;
        }

        private void ValidateFormForXevprm(Person_PK person)
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
                {XevprmValidationRules.AP.qppvcode.DataType.RuleId, lbExtQppvCodes},
                {XevprmValidationRules.AP.qppvcode.CustomBR1.RuleId, lbExtQppvCodes}
            };

            foreach (var error in validationResult.XevprmValidationExceptions)
            {
                if (error.XevprmValidationRuleId == null || !errorControlMaping.Keys.Contains(error.XevprmValidationRuleId)) continue;
                var control = errorControlMaping[error.XevprmValidationRuleId] as Shared.Interface.IXevprmValidationError;
                if (control == null) continue;
                control.ValidationError = error.ReadyMessage;

                if (control == lbExtQppvCodes)
                {
                    var qppvListItem = lbExtQppvCodes.LbInput.Items.FindByValue(Convert.ToString(error.ReadyEntityPk));
                    if (qppvListItem != null) qppvListItem.Selected = true;
                    control.ValidationError = string.Format("'{0}': {1}", StringOperations.ReplaceNullOrWhiteSpace(Convert.ToString(error.ReadyEntityPropertyValue),"N/A"), error.ReadyMessage);
                }
            }
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            var person = new Person_PK();

            if (FormType == FormType.Edit && _idPerson.HasValue)
            {
                person = _personOperations.GetEntity(_idPerson);
            }

            if (person == null) return null;

            person.country_FK = ddlCountryCode.SelectedId;
            person.name = txtName.Text;
            person.familyname = txtFamilyName.Text;
            person.title = txtTitle.Text;
            person.company = txtCompany.Text;
            person.department = txtDepartment.Text;
            person.building = txtBuilding.Text;
            person.street = txtStreet.Text;
            person.state = txtState.Text;
            person.postcode = txtPostcode.Text;
            person.city = txtCity.Text;
            person.tel_countrycode = txtPhoneCountryCode.Text;
            person.telnumber = txtPhoneNumber.Text;
            person.telextn = txtPhoneExtension.Text;
            person.cell_countrycode = txtMobileCountryCode.Text;
            person.cellnumber = txtMobileNumber.Text;
            person.fax_countrycode = txtFaxCountryCode.Text;
            person.faxnumber = txtFaxNumber.Text;
            person.faxextn = txtFaxExtension.Text;
            person.email = txtEmail.Text;
            person.telnum24h = txt24hTelNumber.Text;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                person = _personOperations.Save(person);

                SaveQppvCodes(person.person_PK, auditTrailSessionToken);
                SavePersonRoles(person.person_PK, auditTrailSessionToken);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, person.person_PK, "PERSON", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, person.person_PK, "PERSON", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return person;
        }

        private void SaveQppvCodes(int? personPk, string auditTrailSessionToken)
        {
            var qppvCodes = lbExtQppvCodes.GetDataEntities<Qppv_code_PK>();

            foreach (Qppv_code_PK qppv in qppvCodes)
            {
                if (qppv.qppv_code == Constant.UnknownValue) qppv.qppv_code = string.Empty;
                qppv.person_FK = personPk;
            }

            if (qppvCodes.Count == 0) qppvCodes.Add(new Qppv_code_PK(-1, personPk, null));
            var oldQppvCodeList = _qppvCodeOperations.GetQppvCodeByPerson(personPk);
            List<Qppv_code_PK> qppvCodeListToDelete = lbExtQppvCodes.GetDbItemsToDelete(oldQppvCodeList, "qppv_code_PK");

            foreach (Qppv_code_PK qppv in qppvCodes)
            {
                if (qppv.qppv_code_PK < 0) qppv.qppv_code_PK = null;
                _qppvCodeOperations.Save(qppv);
            }

            Qppv_code_PK emptyQppvCode = qppvCodes.FirstOrDefault(qppvCode => qppvCode.qppv_code == null && qppvCode.person_FK == personPk);
            if (emptyQppvCode != null)
            {
                foreach (var qppv in qppvCodeListToDelete)
                {
                    if (qppv.qppv_code_PK != null)
                    {
                        var authorisedProductListQppvs = _authorisedProductOperations.GetEntitiesByQppvCode(qppv.qppv_code_PK.Value);
                        foreach (var authorisedProduct in authorisedProductListQppvs)
                        {
                            authorisedProduct.qppv_code_FK = emptyQppvCode.qppv_code_PK;
                            _authorisedProductOperations.Save(authorisedProduct);
                        }
                    }
                }
            }

            foreach (var qppv in qppvCodeListToDelete)
            {
                if (qppv.qppv_code_PK != null) _qppvCodeOperations.Delete(qppv.qppv_code_PK);
            }
        }

        private void SavePersonRoles(int? personPk, string auditTrailSessionToken)
        {
            if (!personPk.HasValue) return;

            var complexAuditNewValue = "";
            var complexAuditOldValue = "";

            var personRoleList = _personRoleOperations.GetAssignedEntitiesByPerson(personPk);
            foreach (var personRole in personRoleList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += personRole.person_name;
            }

            _personInRoleMnOperations.DeleteByPerson(personPk);

            var personPersonRoleMnList = new List<Person_in_role_PK>(lbAuPersonRole.LbInputTo.Items.Count);
            foreach (ListItem listItem in lbAuPersonRole.LbInputTo.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                personPersonRoleMnList.Add(new Person_in_role_PK(null, personPk, int.Parse(listItem.Value)));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (personPersonRoleMnList.Count > 0)
            {
                _personInRoleMnOperations.SaveCollection(personPersonRoleMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, personPk.ToString(), "PERSON_IN_ROLE");
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
                    if (From == "Person") Response.Redirect(string.Format("~/Views/PersonView/List.aspx?EntityContext={0}", EntityContext.Person));
                    Response.Redirect(string.Format("~/Views/PersonView/List.aspx?EntityContext={0}", EntityContext.Person));
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedPerson = SaveForm(null);

                        Response.Redirect(string.Format("~/Views/PersonView/List.aspx?EntityContext={0}", EntityContext.Person));
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

        #region Qppv codes

        /// <summary>
        /// Handles Add button on qppv code list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbExtQppvCodes_OnAddClick(object sender, EventArgs e)
        {
            QppvCodePopup.ShowModalForm(null);
        }

        /// <summary>
        /// Handles Edit button on qppv code list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbExtQppvCodes_OnEditClick(object sender, EventArgs e)
        {
            var selectedEntity = lbExtQppvCodes.GetFirstSelectedEntityFromData<Qppv_code_PK>();

            if (selectedEntity is Qppv_code_PK)
            {
                QppvCodePopup.ShowModalForm(selectedEntity as Qppv_code_PK);
            }
        }

        /// <summary>
        /// Handles Remove button on qppv code list box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lbExtQppvCodes_OnRemoveClick(object sender, EventArgs e)
        {
            lbExtQppvCodes.RemoveSelected<Qppv_code_PK>();
        }

        void QppvCodePopup_OnOkButtonClick(object sender, FormEventArgs<Qppv_code_PK> e)
        {
            var qppvCode = e.Data;

            if (!qppvCode.qppv_code_PK.HasValue)
            {
                qppvCode.qppv_code_PK = lbExtQppvCodes.GetNextIdForNewEntity<Qppv_code_PK>();

                var listItem = new ListItem(qppvCode.qppv_code, qppvCode.qppv_code_PK.ToString());

                if (lbExtQppvCodes.LbInput.Items.FindByText(listItem.Text) != null) return;

                lbExtQppvCodes.LbInput.Items.Add(listItem);
                lbExtQppvCodes.AddEntityToData(listItem.Value, qppvCode);
            }
            else
            {
                var listItem = lbExtQppvCodes.LbInput.Items.FindByValue(qppvCode.qppv_code_PK.ToString());
                listItem.Text = qppvCode.qppv_code;
                lbExtQppvCodes.UpdateEntityFromData(listItem.Value, qppvCode);
            }

            lbExtQppvCodes.LbInput.SortItemsByText();
        }


        void LbExtQppvCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbExtQppvCodes.LbtnRemove.Enabled = lbExtQppvCodes.LbInput.SelectedItem.Text != Constant.UnknownValue;
        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContextMenuItems()
        {
            var contextMenu = new[]
            {
                new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), 
                new ContextMenuItem(ContextMenuEventTypes.Save, "Save")
            };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location = null;
            List<string> locationNamesToGenerate = null;
            if (FormType == FormType.New)
            {
                location = Support.LocationManager.Instance.GetLocationByName("PersonFormNew", Support.CacheManager.Instance.AppLocations);
                locationNamesToGenerate = new List<string> { "PersonFormNew", "UserSecurityFormEdit" };
            }
            else if (FormType == FormType.Edit)
            {
                location = Support.LocationManager.Instance.GetLocationByName("PersonFormEdit", Support.CacheManager.Instance.AppLocations);
                locationNamesToGenerate = new List<string> { "PersonFormEdit", "UserSecurityFormEdit" };
            }

            if (location != null)
            {
                tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location, locationNamesToGenerate);
                tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            var location = Support.LocationManager.Instance.GetLocationByName("Person", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                var topLevelParent = MasterPage.FindTopLevelParent(location);

                MasterPage.CurrentLocation = location;
                MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            }
        }

        private void StylizeArticle57RelevantControls(bool? isArticle57Relevant)
        {
            lbExtQppvCodes.LblName.AddCssClass(Article57Reporting.GetCssClass(true, false, lbExtQppvCodes.HasValue(), isArticle57Relevant));
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedInsertPerson = false;
            if (EntityContext == EntityContext.Person)
            {
                Location_PK parentLocation = Support.LocationManager.Instance.GetLocationByName("Person", Support.CacheManager.Instance.AppLocations);
                if (FormType == FormType.New) isPermittedInsertPerson = SecurityHelper.IsPermitted(Permission.InsertPerson, parentLocation);
                else if (FormType == FormType.Edit)
                {
                    if (EntityContext == EntityContext.Person)
                    {
                        isPermittedInsertPerson = SecurityHelper.IsPermitted(Permission.EditPerson, parentLocation);
                        var refererLocation = Support.LocationManager.GetRefererLocation();
                        if (!isPermittedInsertPerson && refererLocation != null) isPermittedInsertPerson = SecurityHelper.IsPermitted(Permission.EditPerson, refererLocation);
                    }
                }
            }

            if (isPermittedInsertPerson)
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

            return true;
        }

        #endregion
    }
}