using System;
using System.Collections.Generic;
using System.Web.Security;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.UserSecurityView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idPerson;

        private IUSER_ROLEOperations _userRoleOperations;
        private IType_PKOperations _typeOperations;
        private IAd_domain_PKOperations _adDomainOperations;
        private IUSER_IN_ROLEOperations _userUserRoleMnOperations;
        private IUSEROperations _userOperations;
        private IPerson_PKOperations _personOperations;
        private ILast_change_PKOperations _lastChangeOperations;

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

            txtPassword.TxtInput.Attributes["value"] = txtPassword.Text;

            if (IsPostBack) return;

            InitForm(null);

            if (_idPerson == null)
            {
                mpInfo.ShowModalPopup("Error!", "<center>Please create a person before providing security rights!</center><br />");
                return;
            }

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

            _userRoleOperations = new USER_ROLEDAL();
            _typeOperations = new Type_PKDAL();
            _adDomainOperations = new Ad_domain_PKDAL();
            _userUserRoleMnOperations = new USER_IN_ROLEDAL();
            _userOperations = new USERDAL();
            _personOperations = new Person_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();

            _idPerson = ValidationHelper.IsValidInt(Request.QueryString["idPerson"]) ? int.Parse(Request.QueryString["idPerson"]) : (int?)null;

            LoadActionQuery();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
                mpInfo.OnOkButtonClick += mpInfo_OnOkButtonClick;
                mpInfo.OnCloseButtonClick += mpInfo_OnCloseButtonClick;
            }
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
            // Left pane
            rbYnActiveDirectoryUser.SelectedValue = null;
            ddlActiveDirectoryDomain.SelectedValue = null;
            txtUsername.Text = String.Empty;
            txtPassword.Text = String.Empty;
            ddlStatus.SelectedValue = null;
            lbAuUserRole.Clear();
            txtLastChange.Text = String.Empty;

            // Right pane
        }

        private void FillFormControls(object arg)
        {
            FillDdlStatus();
            FillDdlActiveDirectory();

            if (FormType == FormType.New)
            {
                FillLbAuUserRoles();
            }
        }

        private void FillDdlStatus()
        {
            var personStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.PersonStatus);
            personStatusList.SortByField(x => x.name);
            ddlStatus.Fill(personStatusList, x => x.name, x => x.type_PK);
        }

        private void FillDdlActiveDirectory()
        {
            var adDomainList = _adDomainOperations.GetEntities();
            adDomainList.SortByField(x => x.domain_alias);
            ddlActiveDirectoryDomain.Fill(adDomainList, x => x.domain_alias, x => x.ad_domain_PK);
        }

        private void FillLbAuUserRoles()
        {
            var userRoleList = _userRoleOperations.GetEntities();
            lbAuUserRole.LbInputFrom.Fill(userRoleList, x => x.Name, x => x.User_role_PK);
            lbAuUserRole.LbInputFrom.SortItemsByText();
        }

        private void SetFormControlsDefaults(object arg)
        {
            if (FormType == FormType.New)
            {
                rbYnActiveDirectoryUser.SelectedValue = false;

                ListItem activeStatus = ddlStatus.DdlInput.Items.FindByText("Active");
                ddlStatus.SelectedValue = activeStatus != null ? activeStatus.Value : null;    
            }
            
            txtPassword.Enabled = false;
            txtLastChange.Enabled = false;

            if (_idPerson.HasValue)
            {
                var person = _personOperations.GetEntity(_idPerson.Value);
                if (person != null)
                {
                    lblPrvPerson.Text = !string.IsNullOrWhiteSpace(person.FullName) ? person.FullName : Constant.ControlDefault.LbPrvText;
                }
                else lblPrvPerson.Text = Constant.ControlDefault.LbPrvText;
            }
            else lblPrvPerson.Text = Constant.ControlDefault.LbPrvText;
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idPerson.HasValue) return;

            var user = _userOperations.GetUserByPersonID(_idPerson.Value);
            if (user == null || !user.User_PK.HasValue)
            {
                FillLbAuUserRoles();
                return;
            }

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------

            // Active Directory user
            rbYnActiveDirectoryUser.SelectedValue = user.IsAdUser;

            // Active Directory domain
            ddlActiveDirectoryDomain.SelectedId = user.AdDomain;
            ddlActiveDirectoryDomain.Visible = user.IsAdUser == true;

            // Username
            txtUsername.Text = user.Username;

            // Password
            txtPassword.Text = user.Password;
            txtPassword.Visible = user.IsAdUser != true;

            // Status
            string status = user.Active != null ? user.Active.Value ? "Active" : "Inactive" : null;
            var listItem = ddlStatus.DdlInput.Items.FindByText(status);
            ddlStatus.SelectedValue = listItem != null ? listItem.Value : null;

            // User roles
            BindUserRoles(user.User_PK.Value);

            // Last change
            txtLastChange.Text = LastChange.GetFormattedString(user.User_PK, "USER", _lastChangeOperations, _personOperations);

            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------

        }

        private void BindUserRoles(int userPk)
        {
            var userRoleAvailableList = _userRoleOperations.GetAvailableEntitiesByUser(userPk);
            lbAuUserRole.LbInputFrom.Fill(userRoleAvailableList, x => x.Display_name, x => x.User_role_PK);
            lbAuUserRole.LbInputFrom.SortItemsByText();

            var userRoleAssignedList = _userRoleOperations.GetAssignedEntitiesByUser(userPk);
            lbAuUserRole.LbInputTo.Fill(userRoleAssignedList, x => x.Display_name, x => x.User_role_PK);
            lbAuUserRole.LbInputTo.SortItemsByText();
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (!string.IsNullOrWhiteSpace(txtUsername.Text) && !string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                var user = _userOperations.GetUserByUsername(txtUsername.Text.Trim());
                if (user != null && user.Person_FK != _idPerson)
                {
                    errorMessage += "Username already exists.<br />";
                    txtUsername.ValidationError = "Username already exists.";
                }
            }

            if (!ddlStatus.SelectedId.HasValue)
            {
                errorMessage += "Status can't be empty.<br />";
                ddlStatus.ValidationError = "Status can't be empty.";
            }

            if (lbAuUserRole.LbInputTo.Items.Count == 0)
            {
                errorMessage += "At least one role must be assigned.<br />";
                lbAuUserRole.ValidationError = "At least one role must be assigned.";
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
            txtUsername.ValidationError = String.Empty;
            ddlStatus.ValidationError = String.Empty;
            lbAuUserRole.ValidationError = String.Empty;

            // Right pane
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            var user = new USER();

            if (FormType == FormType.Edit && _idPerson.HasValue)
            {
                user = _userOperations.GetUserByPersonID(_idPerson.Value);
            }

            if (user == null || FormType == FormType.New || rbYnActiveDirectoryUser.SelectedValue == true)
            {
                if (user == null)
                {
                    user = new USER();
                    user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtPassword.Text, "SHA1");
                }

                user.Username = txtUsername.Text;
                user.Active = ddlStatus.Text == "Active";
                user.Person_FK = _idPerson;
                user.IsAdUser = rbYnActiveDirectoryUser.SelectedValue == true;
                user.AdDomain = ddlActiveDirectoryDomain.SelectedId;
            }
            else
            {
                user.Username = txtUsername.Text;
                user.IsAdUser = rbYnActiveDirectoryUser.SelectedValue == true;
                user.AdDomain = user.IsAdUser == true ? ddlActiveDirectoryDomain.SelectedId : null;
                user.Active = ddlStatus.Text == "Active";
            }

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                user = _userOperations.Save(user);

                if (!user.User_PK.HasValue) return null;

                SaveUserRoles(user.User_PK.Value, auditTrailSessionToken);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, user.User_PK, "USER", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, user.User_PK, "USER", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return user;
        }

        private void SaveUserRoles(int userPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var userRoleList = _userRoleOperations.GetAssignedEntitiesByUser(userPk);
            userRoleList.SortByField(x => x.Display_name);

            foreach (var userRole in userRoleList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += userRole.Display_name;
            }

            _userUserRoleMnOperations.DeleteByUser(userPk);

            var userUserRoleMnList = new List<USER_IN_ROLE>(lbAuUserRole.LbInputTo.Items.Count);

            foreach (ListItem listItem in lbAuUserRole.LbInputTo.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;

                userUserRoleMnList.Add(new USER_IN_ROLE(null, userPk, int.Parse(listItem.Value), DateTime.Now));
            }

            if (userUserRoleMnList.Count > 0)
            {
                _userUserRoleMnOperations.SaveCollection(userUserRoleMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, userPk.ToString(), "USER_IN_ROLE");
        }

        #endregion

        #region Delete

        private void DeleteEntity(object arg)
        {

        }

        #endregion

        #endregion

        #region Event handlers

        private void mpInfo_OnOkButtonClick(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/Views/PersonView/Form.aspx?EntityContext={0}&Action=New", EntityContext.Person));
        }

        private void mpInfo_OnCloseButtonClick(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/Views/PersonView/Form.aspx?EntityContext={0}&Action=New", EntityContext.Person));
        }

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
                        var savedSecurity = SaveForm(null);

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
            Location_PK location = null;
            List<string> locationNamesToGenerate = null;
            if (FormType == FormType.New)
            {
                location = Support.LocationManager.Instance.GetLocationByName("UserSecurityFormEdit", Support.CacheManager.Instance.AppLocations);
                locationNamesToGenerate = new List<string> { "PersonFormNew", "UserSecurityFormEdit" };
            }
            else if (FormType == FormType.Edit)
            {
                MasterPage.OneTimePermissionToken = Permission.ViewComradeTab;
                location = Support.LocationManager.Instance.GetLocationByName("UserSecurityFormEdit", Support.CacheManager.Instance.AppLocations);
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

        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedEditPersonSecurity = false;
            if (EntityContext == EntityContext.Person)
            {
                if (FormType == FormType.Edit) isPermittedEditPersonSecurity = SecurityHelper.IsPermitted(Permission.EditPersonSecurity);
            }

            if (isPermittedEditPersonSecurity)
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