using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI.Views.TimeUnitNameView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idTimeUnitName;

        private ITime_unit_name_PKOperations _timeUnitNameOperations;
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

            _idTimeUnitName = ValidationHelper.IsValidInt(Request.QueryString["idTimeUnitName"]) ? int.Parse(Request.QueryString["idTimeUnitName"]) : (int?)null;

            _timeUnitNameOperations = new Time_unit_name_PKDAL();
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
            txtName.Text = String.Empty;
            txtLastChange.Text = String.Empty;
        }

        private void FillFormControls(object arg)
        {
        }

        private void SetFormControlsDefaults(object arg)
        {
            txtLastChange.Enabled = false;

            if (FormType == FormType.New)
            {
                rbYnBillable.SelectedValue = true;
            }
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idTimeUnitName.HasValue) return;

            var timeUnitName = _timeUnitNameOperations.GetEntity(_idTimeUnitName.Value);
            if (timeUnitName == null || !timeUnitName.time_unit_name_PK.HasValue) return;

            // Time unit name
            txtName.Text = timeUnitName.time_unit_name;

            // Billable
            rbYnBillable.SelectedValue = !timeUnitName.billable.HasValue || timeUnitName.billable.Value;

            // Last change
            txtLastChange.Text = LastChange.GetFormattedString(timeUnitName.time_unit_name_PK, "TIME_UNIT_NAME", _lastChangeOperations, _personOperations);
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
            // Left pane
            txtName.ValidationError = String.Empty;

            // Right pane
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            var timeUnitName = new Time_unit_name_PK();

            if (FormType == FormType.Edit && _idTimeUnitName.HasValue)
            {
                timeUnitName = _timeUnitNameOperations.GetEntity(_idTimeUnitName.Value);
            }

            if (timeUnitName == null) return null;

            // Time unit name
            timeUnitName.time_unit_name = txtName.Text;

            // Bilable
            timeUnitName.billable = rbYnBillable.SelectedValue;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                timeUnitName = _timeUnitNameOperations.Save(timeUnitName);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, timeUnitName.time_unit_name_PK, "TIME_UNIT_NAME", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, timeUnitName.time_unit_name_PK, "TIME_UNIT_NAME", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return timeUnitName;
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
                    if (From == "TimeUnitName") Response.Redirect(string.Format("~/Views/TimeUnitNameView/List.aspx?EntityContext={0}", EntityContext.TimeUnitName));
                    Response.Redirect(string.Format("~/Views/TimeUnitNameView/List.aspx?EntityContext={0}", EntityContext.TimeUnitName));
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedTimeUnitName = SaveForm(null);

                        Response.Redirect(string.Format("~/Views/TimeUnitNameView/List.aspx?EntityContext={0}", EntityContext.TimeUnitName));
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
            var location = Support.LocationManager.Instance.GetLocationByName("TimeUnitName", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            location = Support.LocationManager.Instance.GetLocationByName("TimeUnitName", Support.CacheManager.Instance.AppLocations);
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
            var isPermittedInsertTimeUnitName = false;
            if (EntityContext == EntityContext.TimeUnitName)
            {
                parentLocation = Support.LocationManager.Instance.GetLocationByName("TimeUnitName", Support.CacheManager.Instance.AppLocations);
                if (FormType == FormType.New) isPermittedInsertTimeUnitName = SecurityHelper.IsPermitted(Permission.InsertTimeUnitName, parentLocation);
                else if (FormType == FormType.Edit) isPermittedInsertTimeUnitName = SecurityHelper.IsPermitted(Permission.EditTimeUnitName, parentLocation);
            }

            if (isPermittedInsertTimeUnitName)
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