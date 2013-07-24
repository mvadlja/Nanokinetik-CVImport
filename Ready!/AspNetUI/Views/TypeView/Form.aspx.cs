using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI.Views.TypeView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idType;

        private IType_PKOperations _typeOperations;
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

            _idType = ValidationHelper.IsValidInt(Request.QueryString["idType"]) ? int.Parse(Request.QueryString["idType"]) : (int?)null;

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
            txtTypeName.Text = String.Empty;
            txtDescription.Text = String.Empty;
            ddlTypeGroup.SelectedValue = String.Empty;
            txtLastChange.Text = String.Empty;
        }

        private void FillFormControls(object arg)
        {
            FillDdlTypeGroup();
        }

        private void SetFormControlsDefaults(object arg)
        {
            txtLastChange.Enabled = false;
        }

        private void FillDdlTypeGroup()
        {
            var typeGroupDataSet = _typeOperations.GetGroups();

            ddlTypeGroup.DdlInput.Items.Clear();
            ddlTypeGroup.DdlInput.Items.Add(new ListItem(Constant.ControlDefault.DdlText, Constant.ControlDefault.DdlValue));

            if (typeGroupDataSet != null && typeGroupDataSet.Tables.Count > 0 && typeGroupDataSet.Tables[0].Columns.Contains("group") && 
                typeGroupDataSet.Tables[0].Columns.Contains("group_description"))
            {
                foreach (DataRow row in typeGroupDataSet.Tables[0].Rows)
                {
                    string text = string.Format("{0} - {1}", Convert.ToString(row["group"]), Convert.ToString(row["group_description"]));
                    string value = Convert.ToString(row["group"]);

                    ddlTypeGroup.DdlInput.Items.Add(new ListItem(text, value));
                }
            }

            ddlTypeGroup.SortItemsByText();
        }
        
        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idType.HasValue) return;

            var type = _typeOperations.GetEntity(_idType.Value);
            if (type == null || !type.type_PK.HasValue) return;

            // Type name
            txtTypeName.Text = type.name;

            // Description
            txtDescription.Text = type.description;

            // Type group
            ddlTypeGroup.SelectedValue = type.group;

            // Last change
            txtLastChange.Text = LastChange.GetFormattedString(type.type_PK, "TYPE", _lastChangeOperations, _personOperations);
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(txtTypeName.Text))
            {
                errorMessage += "Name can't be empty.<br />";
                txtTypeName.ValidationError = "Name can't be empty.";
            }

            if (string.IsNullOrWhiteSpace(Convert.ToString(ddlTypeGroup.SelectedValue)))
            {
                errorMessage += "Type can't be empty.<br />";
                ddlTypeGroup.ValidationError = "Type can't be empty.";
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
            txtTypeName.ValidationError = String.Empty;
            ddlTypeGroup.ValidationError = String.Empty;

            // Right pane
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);
            
            var type = new Type_PK();

            if (FormType == FormType.Edit && _idType.HasValue)
            {
                type = _typeOperations.GetEntity(_idType.Value);
            }

            if (type == null) return null;

            // Type name
            type.name = txtTypeName.Text;

            // Description
            type.description = txtDescription.Text;

            // Group
            type.group = Convert.ToString(ddlTypeGroup.SelectedValue);

            // Type description
            var typeGroupList = _typeOperations.GetTypesForDDL(type.group);
            type.group_description = typeGroupList.Count > 0 ? typeGroupList[0].group_description : null;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                type = _typeOperations.Save(type);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, type.type_PK, "TYPE", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, type.type_PK, "TYPE", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return type;
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
                    if (From == "Type") Response.Redirect(string.Format("~/Views/TypeView/List.aspx?EntityContext={0}", EntityContext.Type));
                    Response.Redirect(string.Format("~/Views/TypeView/List.aspx?EntityContext={0}", EntityContext.Type));
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedType = SaveForm(null);

                        Response.Redirect(string.Format("~/Views/TypeView/List.aspx?EntityContext={0}", EntityContext.Type));
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
            var location = Support.LocationManager.Instance.GetLocationByName("Type", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            location = Support.LocationManager.Instance.GetLocationByName("Type", Support.CacheManager.Instance.AppLocations);
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
            var isPermittedInsertType = false;
            if (EntityContext == EntityContext.Type)
            {
                parentLocation = Support.LocationManager.Instance.GetLocationByName("Type", Support.CacheManager.Instance.AppLocations);
                if (FormType == FormType.New) isPermittedInsertType = SecurityHelper.IsPermitted(Permission.InsertType, parentLocation);
                else if (FormType == FormType.Edit) isPermittedInsertType = SecurityHelper.IsPermitted(Permission.EditType, parentLocation);
            }
            
            if (isPermittedInsertType)
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