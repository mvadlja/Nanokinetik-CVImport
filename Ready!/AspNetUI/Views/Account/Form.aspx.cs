using System;
using System.Collections.Generic;
using System.Threading;
using System.Web.Security;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;
using LocationManager = AspNetUI.Support.LocationManager;

namespace AspNetUI.Views.AccountView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private IUSEROperations _userOperations;
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

            txtOldPassword.TxtInput.Attributes["value"] = txtOldPassword.Text;
            txtNewPassword.TxtInput.Attributes["value"] = txtNewPassword.Text;
            txtRepeatedNewPassword.TxtInput.Attributes["value"] = txtRepeatedNewPassword.Text;

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

            _userOperations = new USERDAL();
            _lastChangeOperations = new Last_change_PKDAL();
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
            txtOldPassword.Text = String.Empty;
            txtNewPassword.Text = String.Empty;
            txtRepeatedNewPassword.Text = String.Empty;

            pnlConfirmationMsg.Visible = false;
        }

        private void FillFormControls(object arg)
        {
        }

        private void SetFormControlsDefaults(object arg)
        {
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(txtOldPassword.Text))
            {
                errorMessage += "Old password can't be empty.<br />";
                txtOldPassword.ValidationError = "Old password can't be empty.";
            }
            else
            {
                USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);

                var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(txtOldPassword.Text, "SHA1");
                if (hashPasswordForStoringInConfigFile != null && (user != null && user.Password.ToLower() != hashPasswordForStoringInConfigFile.ToLower()))
                {
                    errorMessage += "Your old password was incorrectly typed.<br />";
                    txtOldPassword.ValidationError = "Your old password was incorrectly typed.";
                }
            }

            if (string.IsNullOrWhiteSpace(txtNewPassword.Text))
            {
                errorMessage += "New password can't be empty.<br />";
                txtNewPassword.ValidationError = "New password can't be empty.";
            }
            
            if (string.IsNullOrWhiteSpace(txtRepeatedNewPassword.Text))
            {
                errorMessage += "Repeated password can't be empty.<br />";
                txtRepeatedNewPassword.ValidationError = "Repeated password can't be empty.";
            }

            if (txtNewPassword.Text != txtRepeatedNewPassword.Text)
            {
                errorMessage += "New password and repeated password do not match.<br />";
                txtRepeatedNewPassword.ValidationError = "New password and repeated password do not match.";
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
            txtOldPassword.ValidationError = String.Empty;
            txtNewPassword.ValidationError = String.Empty;
            txtRepeatedNewPassword.ValidationError = String.Empty;
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);

            if (user == null) return null;

            user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(txtNewPassword.Text, "SHA1");

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                user = _userOperations.Save(user);

                pnlConfirmationMsg.Visible = true;

                LastChange.HandleLastChange(pnlForm, user.User_PK, "USER", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return user;
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
                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedUser = SaveForm(null);
                        var redirectLocation = LocationManager.GetFirstAuthorisedLocation();

                        Response.Redirect(redirectLocation.location_url);
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
            var contextMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);
        }

        private void GenerateTabMenuItems()
        {

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

            var isPermittedChangePassword = SecurityHelper.IsPermitted(Permission.ChangePassword);

            if (isPermittedChangePassword)
            {
                SecurityHelper.SetControlsForReadWrite(MasterPage.ContextMenu, new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") }, new List<Panel> { PnlForm }, new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } });
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

            return true;
        }

        #endregion
    }
}