﻿using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI.Views.AtcView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idAtc;

        private IAtc_PKOperations _atcOperations;
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
            LoadActionQuery();

            _idAtc = ValidationHelper.IsValidInt(Request.QueryString["idAtc"]) ? int.Parse(Request.QueryString["idAtc"]) : (int?)null;

            _atcOperations = new Atc_PKDAL();
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
            txtAtcName.Text = String.Empty;
            txtAtcCode.Text = String.Empty;
            txtLastChange.Text = String.Empty;

            // Right pane

        }

        private void FillFormControls(object arg)
        {
        }

        private void SetFormControlsDefaults(object arg)
        {
            txtLastChange.Enabled = false;
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idAtc.HasValue) return;

            var atc = _atcOperations.GetEntity(_idAtc.Value);
            if (atc == null || !atc.atc_PK.HasValue) return;

            // Atc name
            txtAtcName.Text = atc.name;

            // Atc code
            txtAtcCode.Text = atc.atccode;

            // Last change
            txtLastChange.Text = LastChange.GetFormattedString(atc.atc_PK, "ATC_CODE", _lastChangeOperations, _personOperations);
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(txtAtcName.Text))
            {
                errorMessage += "Name can't be empty.<br />";
                txtAtcName.ValidationError = "Name can't be empty.";
            }

            if (string.IsNullOrWhiteSpace(txtAtcCode.Text))
            {
                errorMessage += "ATC code can't be empty.<br />";
                txtAtcCode.ValidationError = "ATC code can't be empty.";
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
            txtAtcName.ValidationError = String.Empty;
            txtAtcCode.ValidationError = String.Empty;

            // Right pane
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);
            
            var atc = new Atc_PK();

            if (FormType == FormType.Edit && _idAtc.HasValue)
            {
                atc = _atcOperations.GetEntity(_idAtc.Value);
            }

            if (atc == null) return null;

            // Atc name
            atc.name = txtAtcName.Text;

            // Atc code
            atc.atccode = txtAtcCode.Text;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                atc = _atcOperations.Save(atc);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, atc.atc_PK, "ATC_CODE", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, atc.atc_PK, "ATC_CODE", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return atc;
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
                    if (From == "Atc") Response.Redirect(string.Format("~/Views/AtcView/List.aspx?EntityContext={0}", EntityContext.Atc));
                    Response.Redirect(string.Format("~/Views/AtcView/List.aspx?EntityContext={0}", EntityContext.Atc));
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedAtc = SaveForm(null);

                        Response.Redirect(string.Format("~/Views/AtcView/List.aspx?EntityContext={0}", EntityContext.Atc));
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
            var location = Support.LocationManager.Instance.GetLocationByName("Atc", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            location = Support.LocationManager.Instance.GetLocationByName("Atc", Support.CacheManager.Instance.AppLocations);
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
            var isPermittedInsertAtc = false;
            if (EntityContext == EntityContext.Atc)
            {
                parentLocation = Support.LocationManager.Instance.GetLocationByName("Atc", Support.CacheManager.Instance.AppLocations);
                if (FormType == FormType.New) isPermittedInsertAtc = SecurityHelper.IsPermitted(Permission.InsertAtc, parentLocation);
                else if (FormType == FormType.Edit) isPermittedInsertAtc = SecurityHelper.IsPermitted( Permission.EditAtc, parentLocation);
            }

            if (isPermittedInsertAtc)
            {
                SecurityHelper.SetControlsForReadWrite(MasterPage.ContextMenu, new[] {new ContextMenuItem(ContextMenuEventTypes.Save, "Save")}, new List<Panel> {PnlForm}, new Dictionary<Panel, List<string>> {{PnlFooter, new List<string> {"Save"}}});
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