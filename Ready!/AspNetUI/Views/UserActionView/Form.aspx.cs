using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI.Views.UserActionView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idUserAction;

        private IUser_action_PKOperations _userActionOperations;
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

            _userActionOperations = new User_action_PKDAL();
            _userOperations = new USERDAL();
            _personOperations = new Person_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();

            var idUserActionString = Request.QueryString["idUserAction"];
            _idUserAction = ValidationHelper.IsValidInt(idUserActionString) ? int.Parse(idUserActionString) : (int?)null;

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
            // Left pane
            txtDisplayName.Text = String.Empty;
            txtAction.Text = String.Empty;
            txtDescription.Text = String.Empty;
            rbYnActive.SelectedValue = null;
            txtLastChange.Text = String.Empty;

            // Right pane
        }

        private void FillFormControls(object arg)
        {
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
            if (!_idUserAction.HasValue) return;

            var userAction = _userActionOperations.GetEntity(_idUserAction.Value);
            if (userAction == null || !userAction.user_action_PK.HasValue) return;

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------

            // Display name
            txtDisplayName.Text = userAction.display_name;

            // Action
            txtAction.Text = userAction.unique_name;

            // Description
            txtDescription.Text = userAction.description;

            // Active
            rbYnActive.SelectedValue = userAction.active;

            // Last change
            txtLastChange.Text = LastChange.GetFormattedString(userAction.user_action_PK, "USER_ACTION", _lastChangeOperations, _personOperations);

            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------

        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(txtAction.Text))
            {
                errorMessage += "Action can't be empty.<br />";
                txtAction.ValidationError = "Action can't be empty.";
            }
            else if ((FormType == FormType.New || FormType == FormType.SaveAs) && _userActionOperations.GetEntities().Exists(x => x.unique_name == txtAction.Text))
            {
                errorMessage += "Action unique name already exists! Please choose another unique name.<br />";
                txtAction.ValidationError = "Action unique name already exists! Please choose another unique name.";
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
            txtAction.ValidationError = String.Empty;

            // Right pane
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            var userAction = new User_action_PK();

            if (FormType == FormType.Edit && _idUserAction.HasValue)
            {
                userAction = _userActionOperations.GetEntity(_idUserAction.Value);
            }

            if (userAction == null) return null;

            userAction.display_name = !string.IsNullOrWhiteSpace(txtDisplayName.Text) ? txtDisplayName.Text : txtAction.Text;
            userAction.unique_name = txtAction.Text;
            userAction.description = txtDescription.Text;
            userAction.active = rbYnActive.SelectedValue;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                userAction = _userActionOperations.Save(userAction);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, userAction.user_action_PK, "USER_ACTION", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, userAction.user_action_PK, "USER_ACTION", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return userAction;
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
                    if (From == "UserAction") Response.Redirect(string.Format("~/Views/UserActionView/List.aspx?EntityContext={0}", EntityContext.UserAction));
                    Response.Redirect(string.Format("~/Views/UserActionView/List.aspx?EntityContext={0}", EntityContext.UserAction));
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedUserAction = SaveForm(null);

                        Response.Redirect(string.Format("~/Views/UserActionView/List.aspx?EntityContext={0}", EntityContext.UserAction));
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
            var location = Support.LocationManager.Instance.GetLocationByName("UserAction", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            location = Support.LocationManager.Instance.GetLocationByName("UserAction", Support.CacheManager.Instance.AppLocations);
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

            Location_PK parentLocation = null;
            var isPermittedInsertUserRAction = false;
            if (EntityContext == EntityContext.UserAction)
            {
                parentLocation = Support.LocationManager.Instance.GetLocationByName("UserAction", Support.CacheManager.Instance.AppLocations);
                if (FormType == FormType.New) isPermittedInsertUserRAction = SecurityHelper.IsPermitted(Permission.InsertUserAction, parentLocation);
                else if (FormType == FormType.Edit) isPermittedInsertUserRAction = SecurityHelper.IsPermitted(Permission.EditUserAction, parentLocation);
            }

            if (isPermittedInsertUserRAction)
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