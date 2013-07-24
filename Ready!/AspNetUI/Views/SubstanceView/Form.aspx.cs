using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI.Views.SubstanceView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idSub;

        private ISubstance_PKOperations _substanceOperations;
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

            _idSub = ValidationHelper.IsValidInt(Request.QueryString["idSub"]) ? int.Parse(Request.QueryString["idSub"]) : (int?)null;

            _substanceOperations = new Substance_PKDAL();
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
            txtSubstanceName.Text = String.Empty;
            txtEvCode.Text = String.Empty;
            txtLastChange.Text = String.Empty;
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
            if (!_idSub.HasValue) return;

            var substance = _substanceOperations.GetEntity(_idSub.Value);
            if (substance == null || !substance.substance_PK.HasValue) return;

            // Atc name
            txtSubstanceName.Text = substance.substance_name;

            // Atc code
            txtEvCode.Text = substance.ev_code;

            // Last change
            txtLastChange.Text = LastChange.GetFormattedString(substance.substance_PK, "SUBSTANCE", _lastChangeOperations, _personOperations);
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(txtSubstanceName.Text))
            {
                errorMessage += "Substance name can't be empty.<br />";
                txtSubstanceName.ValidationError = "Substance name can't be empty.";
            }

            if (!string.IsNullOrWhiteSpace(txtEvCode.Text) && FormType == FormType.New && _substanceOperations.EntityWithEvCodeExists(txtEvCode.Text))
            {
                errorMessage += "Substance with this EV Code already exists.<br />";
                txtEvCode.ValidationError = "Substance with this EV Code already exists.";
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
            txtSubstanceName.ValidationError = String.Empty;
            txtEvCode.ValidationError = String.Empty;

            // Right pane
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);
            
            var substance = new Substance_PK();

            if (FormType == FormType.Edit && _idSub.HasValue)
            {
                substance = _substanceOperations.GetEntity(_idSub.Value);
            }

            if (substance == null) return null;

            // Substance name
            substance.substance_name = txtSubstanceName.Text;

            // Ev Code
            substance.ev_code = txtEvCode.Text;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                substance = _substanceOperations.Save(substance);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, substance.substance_PK, "SUBSTANCE", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, substance.substance_PK, "SUBSTANCE", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return substance;
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
                    if (From == "Substance") Response.Redirect(string.Format("~/Views/SubstanceView/List.aspx?EntityContext={0}", EntityContext.Substance));
                    Response.Redirect(string.Format("~/Views/SubstanceView/List.aspx?EntityContext={0}", EntityContext.Substance));
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedSubstance = SaveForm(null);

                        Response.Redirect(string.Format("~/Views/SubstanceView/List.aspx?EntityContext={0}", EntityContext.Substance));
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
            var location = Support.LocationManager.Instance.GetLocationByName("Substance", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            location = Support.LocationManager.Instance.GetLocationByName("Substance", Support.CacheManager.Instance.AppLocations);
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
            var isPermittedInsertSubstance = false;
            if (EntityContext == EntityContext.Substance)
            {
                parentLocation = Support.LocationManager.Instance.GetLocationByName("Substance", Support.CacheManager.Instance.AppLocations);
                if (FormType == FormType.New) isPermittedInsertSubstance = SecurityHelper.IsPermitted(Permission.InsertSubstance, parentLocation);
                else if (FormType == FormType.Edit) isPermittedInsertSubstance = SecurityHelper.IsPermitted( Permission.EditSubstance, parentLocation);
            }

            if (isPermittedInsertSubstance)
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