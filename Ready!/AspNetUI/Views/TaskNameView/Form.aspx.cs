using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI.Views.TaskNameView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idTaskName;

        private ITask_name_PKOperations _taskNameOperations;
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

            _idTaskName = ValidationHelper.IsValidInt(Request.QueryString["idTaskName"]) ? int.Parse(Request.QueryString["idTaskName"]) : (int?)null;

            _taskNameOperations = new Task_name_PKDAL();
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
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idTaskName.HasValue) return;

            var taskName = _taskNameOperations.GetEntity(_idTaskName.Value);
            if (taskName == null || !taskName.task_name_PK.HasValue) return;

            // Task name
            txtName.Text = taskName.task_name;

            // Last change
            txtLastChange.Text = AuditTrailHelper.GetLastChangeFormattedString(taskName.task_name_PK, "TASK_NAME", _lastChangeOperations, _personOperations);
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

            var taskName = new Task_name_PK();

            if (FormType == FormType.Edit && _idTaskName.HasValue)
            {
                taskName = _taskNameOperations.GetEntity(_idTaskName.Value);
            }

            if (taskName == null) return null;

            // Task name
            taskName.task_name = txtName.Text;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                taskName = _taskNameOperations.Save(taskName);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, taskName.task_name_PK, "TASK_NAME", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, taskName.task_name_PK, "TASK_NAME", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return taskName;
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
                    if (From == "TaskName") Response.Redirect(string.Format("~/Views/TaskNameView/List.aspx?EntityContext={0}", EntityContext.TaskName));
                    Response.Redirect(string.Format("~/Views/TaskNameView/List.aspx?EntityContext={0}", EntityContext.TaskName));
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedTaskName = SaveForm(null);

                        Response.Redirect(string.Format("~/Views/TaskNameView/List.aspx?EntityContext={0}", EntityContext.TaskName));
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
            var location = Support.LocationManager.Instance.GetLocationByName("TaskName", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            location = Support.LocationManager.Instance.GetLocationByName("TaskName", Support.CacheManager.Instance.AppLocations);
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
            var isPermittedInsertTaskName = false;
            if (EntityContext == EntityContext.TaskName)
            {
                parentLocation = Support.LocationManager.Instance.GetLocationByName("TaskName", Support.CacheManager.Instance.AppLocations);
                if (FormType == FormType.New) isPermittedInsertTaskName = SecurityHelper.IsPermitted(Permission.InsertTaskName, parentLocation);
                else if (FormType == FormType.Edit) isPermittedInsertTaskName = SecurityHelper.IsPermitted( Permission.EditTaskName, parentLocation);
            }

            if (isPermittedInsertTaskName)
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