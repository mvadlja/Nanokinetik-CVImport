using System;
using System.Collections.Generic;
using System.Linq;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.UserRoleView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idUserRole;

        private IUSER_ROLEOperations _userRoleOperations;
        private IUser_role_action_PKOperations _userRoleActionOperations;
        private ILocation_PKOperations _locationOperations;
        private ILocation_user_action_mn_PKOperations _locationUserActionMnOperations;
        private IUser_action_PKOperations _userActionOperations;
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

            _userRoleOperations = new USER_ROLEDAL();
            _userRoleActionOperations = new User_role_action_PKDAL();
            _locationOperations = new Location_PKDAL();
            _locationUserActionMnOperations = new Location_user_action_mn_PKDAL();
            _userActionOperations = new User_action_PKDAL();
            _userUserRoleMnOperations = new USER_IN_ROLEDAL();
            _userOperations = new USERDAL();
            _personOperations = new Person_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();

            var idUserRoleString = Request.QueryString["idUserRole"];
            _idUserRole = ValidationHelper.IsValidInt(idUserRoleString) ? int.Parse(idUserRoleString) : (int?)null;

            LoadActionQuery();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
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
            txtDisplayName.Text = String.Empty;
            txtRole.Text = String.Empty;
            txtDescription.Text = String.Empty;
            rbYnActive.SelectedValue = null;
            txtLastChange.Text = String.Empty;
            cbInsertAll.Checked = false;
            cbEditAll.Checked = false;
            cbSaveAsAll.Checked = false;
            cbDeleteAll.Checked = false;
            cbViewAll.Checked = false;
            ddlLocation.SelectedValue = null;
            ddlAction.SelectedValue = null;
            lbExtSelectedPermissions.Clear();
            lbAuPerson.Clear();
        }

        private void FillFormControls(object arg)
        {
            FillDdlLocation();
            FillDdlUserAction();

            if (FormType == FormType.New)
            {
                FillLbAuPersonAssignements();
            }
        }

        private void FillDdlLocation()
        {
            var locationList = _locationOperations.GetEntities()
                .Where(x => x.show_location == true)
                .OrderBy(x => x.full_unique_path).ToList();
            ddlLocation.Fill(locationList, x => (!string.IsNullOrWhiteSpace(x.full_unique_path) ? x.full_unique_path : x.unique_name), x => x.location_PK);
        }

        private void FillDdlUserAction()
        {
            var userActionList = new List<User_action_PK>();

            if (ddlLocation.SelectedId.HasValue)
            {
                userActionList = _userActionOperations.GetEntitiesByLocation(ddlLocation.SelectedId.Value);
                userActionList.SortByField(x => x.display_name);
            }

            ddlAction.Fill(userActionList, x => x.display_name, x => x.user_action_PK);
        }

        private void FillLbAuPersonAssignements()
        {
            var personList = _personOperations.GetEntitiesWithUser();
            personList.SortByField(x => x.FullName);
            lbAuPerson.LbInputFrom.Fill(personList, x => x.FullName, x => x.person_PK);
        }

        private void SetFormControlsDefaults(object arg)
        {
            rbYnActive.SelectedValue = true;

            txtLastChange.Enabled = false;
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idUserRole.HasValue) return;

            var userRole = _userRoleOperations.GetEntity(_idUserRole.Value);
            if (userRole == null || !userRole.User_role_PK.HasValue) return;

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------

            // Display name
            txtDisplayName.Text = userRole.Display_name;

            // Action
            txtRole.Text = userRole.Name;

            // Description
            txtDescription.Text = userRole.Description;

            // Selected permissions
            BindSelectedPermissions(userRole.User_role_PK.Value);

            // Person assignments
            BindPersonAssignements(userRole.User_role_PK.Value);

            // Active
            rbYnActive.SelectedValue = userRole.Active;

            // Last change
            txtLastChange.Text = LastChange.GetFormattedString(userRole.User_role_PK, "USER_ROLE", _lastChangeOperations, _personOperations);

            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------

        }

        private void BindSelectedPermissions(int userRolePk)
        {
            var userRoleActionList = _userRoleActionOperations.GetEntitiesByUserRole(userRolePk);
            lbExtSelectedPermissions.FillAdvanced(userRoleActionList, GetPermissionName, x => x.user_role_action_PK, true);
            lbExtSelectedPermissions.LbInput.SortItemsByText();
        }

        private void BindPersonAssignements(int userRolePk)
        {
            var personAvailableList = _personOperations.GetAvailableEntitiesByUserRole(userRolePk);
            lbAuPerson.LbInputFrom.Fill(personAvailableList, x => x.FullName, x => x.person_PK);
            lbAuPerson.LbInputFrom.SortItemsByText();

            var personAssignedList = _personOperations.GetAssignedEntitiesByUserRole(userRolePk);
            lbAuPerson.LbInputTo.Fill(personAssignedList, x => x.FullName, x => x.person_PK);
            lbAuPerson.LbInputTo.SortItemsByText();
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(txtRole.Text))
            {
                errorMessage += "Role can't be empty.<br />";
                txtRole.ValidationError = "Role can't be empty.";
            }
            else if ((FormType == FormType.New || FormType == FormType.SaveAs) && _userRoleOperations.GetEntities().Exists(x => x.Name == txtRole.Text))
            {
                errorMessage += "Role unique name already exists! Please choose another unique name.<br />";
                txtRole.ValidationError = "Role unique name already exists! Please choose another unique name.";
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
            txtRole.ValidationError = String.Empty;

            // Right pane
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            var userRole = new USER_ROLE();

            if (FormType == FormType.Edit && _idUserRole.HasValue)
            {
                userRole = _userRoleOperations.GetEntity(_idUserRole.Value);
            }

            if (userRole == null) return null;

            userRole.Display_name = !string.IsNullOrWhiteSpace(txtDisplayName.Text) ? txtDisplayName.Text : txtRole.Text;
            userRole.Name = txtRole.Text;
            userRole.Description = txtDescription.Text;
            userRole.Active = rbYnActive.SelectedValue;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                userRole = _userRoleOperations.Save(userRole);

                if (!userRole.User_role_PK.HasValue) return null;

                SavePersonAssignments(userRole.User_role_PK.Value, auditTrailSessionToken);

                SaveSelectedPermissions(userRole.User_role_PK.Value, auditTrailSessionToken);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, userRole.User_role_PK, "USER_ROLE", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, userRole.User_role_PK, "USER_ROLE", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return userRole;
        }

        private void SaveSelectedPermissions(int userRolePk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var userRoleActionList = _userRoleActionOperations.GetEntitiesByUserRole(userRolePk);
            var userRoleActionNameList = userRoleActionList.Select(GetPermissionName).OrderBy(x => x).ToList();

            foreach (var userRoleActionName in userRoleActionNameList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += userRoleActionName;
            }

            var userRoleActionPkListToDelete = lbExtSelectedPermissions.GetDbItemsIdsToDelete(userRoleActionList, "user_role_action_PK");
            _userRoleActionOperations.DeleteCollection(userRoleActionPkListToDelete);

            foreach (ListItem listItem in lbExtSelectedPermissions.LbInput.Items)
            {

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            userRoleActionList = lbExtSelectedPermissions.GetDataEntities<User_role_action_PK>();

            foreach (var userRoleAction in userRoleActionList)
            {
                if (userRoleAction.user_role_action_PK < 0) userRoleAction.user_role_action_PK = null;
                userRoleAction.user_role_FK = userRolePk;
            }

            if (userRoleActionList.Count > 0)
            {
                _userRoleActionOperations.SaveCollection(userRoleActionList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, userRolePk.ToString(), "USER_ROLE_ACTION");
        }

        private void SavePersonAssignments(int userRolePk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var oldPersonList = _personOperations.GetAssignedEntitiesByUserRole(userRolePk);
            oldPersonList.SortByField(x => x.FullName);

            foreach (var person in oldPersonList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += person.FullName;
            }

            _userUserRoleMnOperations.DeleteByUserRole(userRolePk);

            var userUserRoleMnList = new List<USER_IN_ROLE>(lbAuPerson.LbInputTo.Items.Count);

            foreach (ListItem listItem in lbAuPerson.LbInputTo.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;

                var user = _userOperations.GetUserByPersonID(int.Parse(listItem.Value));

                if (user != null && user.User_PK.HasValue)
                {
                    userUserRoleMnList.Add(new USER_IN_ROLE(null, user.User_PK, userRolePk, DateTime.Now));
                }
            }

            if (userUserRoleMnList.Count > 0)
            {
                _userUserRoleMnOperations.SaveCollection(userUserRoleMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, userRolePk.ToString(), "USER_IN_ROLE");
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
                    if (From == "UserRole") Response.Redirect(string.Format("~/Views/UserRoleView/List.aspx?EntityContext={0}", EntityContext.UserRole));
                    Response.Redirect(string.Format("~/Views/UserRoleView/List.aspx?EntityContext={0}", EntityContext.UserRole));
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedUserRole = SaveForm(null);

                        Response.Redirect(string.Format("~/Views/UserRoleView/List.aspx?EntityContext={0}", EntityContext.UserRole));
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

        #region Group actions

        protected void cbInsertAll_OnCheckedChanged(object sender, EventArgs e)
        {
            if (cbInsertAll.Checked)
            {
                AddPermissionForAllLocations(Constant.UserAction.Insert);
            }
            else
            {
                RemovePermissionForAllLocations(Constant.UserAction.Insert);
            }
        }

        protected void cbEditAll_OnCheckedChanged(object sender, EventArgs e)
        {
            if (cbEditAll.Checked)
            {
                AddPermissionForAllLocations(Constant.UserAction.Edit);
            }
            else
            {
                RemovePermissionForAllLocations(Constant.UserAction.Edit);
            }
        }

        protected void cbSaveAsAll_OnCheckedChanged(object sender, EventArgs e)
        {
            if (cbSaveAsAll.Checked)
            {
                AddPermissionForAllLocations(Constant.UserAction.SaveAs);
            }
            else
            {
                RemovePermissionForAllLocations(Constant.UserAction.SaveAs);
            }
        }

        protected void cbDeleteAll_OnCheckedChanged(object sender, EventArgs e)
        {
            if (cbDeleteAll.Checked)
            {
                AddPermissionForAllLocations(Constant.UserAction.Delete);
            }
            else
            {
                RemovePermissionForAllLocations(Constant.UserAction.Delete);
            }
        }

        protected void cbViewAll_OnCheckedChanged(object sender, EventArgs e)
        {
            if (cbViewAll.Checked)
            {
                AddPermissionForAllLocations(Constant.UserAction.View);
            }
            else
            {
                RemovePermissionForAllLocations(Constant.UserAction.View);
            }
        }

        #endregion

        #region Permissions

        protected void ddlLocation_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            FillDdlUserAction();
        }

        protected void lnkAddPermission_OnClick(object sender, EventArgs e)
        {
            int? locationPk = ddlLocation.SelectedId;
            int? userActionPk = ddlAction.SelectedId;
            if (!locationPk.HasValue || !userActionPk.HasValue) return;

            string permissionName = string.Format("{0} ({1})", ddlLocation.Text, ddlAction.Text);

            if (lbExtSelectedPermissions.GetDataEntities<User_role_action_PK>().Exists(x => x.location_FK == locationPk && x.user_action_FK == userActionPk)) return;

            var userActionRole = new User_role_action_PK()
            {
                location_FK = locationPk,
                user_action_FK = userActionPk,
                user_role_action_PK = lbExtSelectedPermissions.GetNextIdForNewEntity<User_role_action_PK>()
            };

            string listItemvalue = Convert.ToString(userActionRole.user_role_action_PK);
            lbExtSelectedPermissions.LbInput.Items.Add(new ListItem(permissionName, listItemvalue));
            lbExtSelectedPermissions.AddEntityToData(listItemvalue, userActionRole);
        }

        protected void lnkRemovePermission_OnClick(object sender, EventArgs e)
        {
            lbExtSelectedPermissions.RemoveSelected<User_role_action_PK>();
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
            var location = Support.LocationManager.Instance.GetLocationByName("UserRole", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            location = Support.LocationManager.Instance.GetLocationByName("UserRole", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                var topLevelParent = MasterPage.FindTopLevelParent(location);

                MasterPage.CurrentLocation = location;
                MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            }
        }


        private string GetPermissionName(User_role_action_PK userRoleAction)
        {
            if (userRoleAction == null) return null;

            var location = _locationOperations.GetEntity(userRoleAction.location_FK);
            var userAction = _userActionOperations.GetEntity(userRoleAction.user_action_FK);

            return GetPermissionName(location, userAction);
        }

        private static string GetPermissionName(Location_PK location, User_action_PK userAction)
        {
            if (location != null && userAction != null)
            {
                return string.Format("{0} ({1})", !String.IsNullOrWhiteSpace(location.full_unique_path) ? location.full_unique_path : location.unique_name, userAction.display_name);
            }

            return null;
        }

        private void AddPermissionForAllLocations(string userActionUniqueName)
        {
            var locationList = _locationOperations.GetEntities()
                .Where(x => x.show_location == true)
                .OrderBy(x => x.full_unique_path).ToList();

            foreach (var location in locationList)
            {
                if (!location.location_PK.HasValue) continue;

                var userActions = _userActionOperations.GetEntitiesByLocation(location.location_PK.Value).FindAll(x => x.unique_name.StartsWith(userActionUniqueName));

                foreach (var userAction in userActions)
                {
                    if (userAction == null) continue;

                    string permissionName = GetPermissionName(location, userAction);

                    if (lbExtSelectedPermissions.GetDataEntities<User_role_action_PK>().Exists(x => x.location_FK == location.location_PK && x.user_action_FK == userAction.user_action_PK)) continue;

                    var userActionRole = new User_role_action_PK()
                    {
                        location_FK = location.location_PK,
                        user_action_FK = userAction.user_action_PK,
                        user_role_action_PK = lbExtSelectedPermissions.GetNextIdForNewEntity<User_role_action_PK>()
                    };

                    string listItemvalue = Convert.ToString(userActionRole.user_role_action_PK);
                    lbExtSelectedPermissions.LbInput.Items.Add(new ListItem(permissionName, listItemvalue));
                    lbExtSelectedPermissions.AddEntityToData(listItemvalue, userActionRole);
                }
            }
        }

        private void RemovePermissionForAllLocations(string userActionUniqueName)
        {
            var listItemValuesToDelete = new List<string>();

            foreach (var userRoleAction in lbExtSelectedPermissions.GetDataEntities<User_role_action_PK>())
            {
                if (!userRoleAction.location_FK.HasValue) continue;

                var userActions = _userActionOperations.GetEntitiesByLocation(userRoleAction.location_FK.Value);

                var userActionList = userActions.FindAll(x => x.unique_name.StartsWith(userActionUniqueName));

                foreach (var userAction in userActionList)
                {
                    if (userAction == null) continue;

                    if (userRoleAction.user_action_FK == userAction.user_action_PK) listItemValuesToDelete.Add(Convert.ToString(userRoleAction.user_role_action_PK));
                }
            }

            foreach (var listItemValue in listItemValuesToDelete)
            {
                lbExtSelectedPermissions.RemoveEntityFromData<User_role_action_PK>(listItemValue);
                var listItem = lbExtSelectedPermissions.LbInput.Items.FindByValue(listItemValue);
                lbExtSelectedPermissions.LbInput.Items.Remove(listItem);
            }
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            Location_PK parentLocation = null;
            var isPermittedInsertUserRole = false;
            if (EntityContext == EntityContext.UserRole)
            {
                parentLocation = Support.LocationManager.Instance.GetLocationByName("UserRole", Support.CacheManager.Instance.AppLocations);
                if (FormType == FormType.New) isPermittedInsertUserRole = SecurityHelper.IsPermitted(Permission.InsertUserRole, parentLocation);
                else if (FormType == FormType.Edit) isPermittedInsertUserRole = SecurityHelper.IsPermitted( Permission.EditUserRole, parentLocation);
            }

            if (isPermittedInsertUserRole)
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