using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using GEM2Common;
using PossGrid;
using Ready.Model;

namespace AspNetUI.Views.UserActionView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;
        private string _gridId;

        private IUSEROperations _userOperations;
        private IUser_action_PKOperations _userActionOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;

        #endregion

        #region Properties

        private int? _userActionPkToDelete
        {
            get { return (int?)ViewState["_userActionPkToDelete"]; }
            set { ViewState["_userActionPkToDelete"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadFormVariables();
            BindEventHandlers();
            GenerateContextMenuItems();
            AssociatePanels(pnlSearch, pnlGrid);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
            {
                scriptManager.RegisterPostBackControl(btnExport);
            }

            if (!IsPostBack)
            {
                InitForm(null);
                BindForm(null);
            }

            SetFormControlsDefaults(null);
            BindGrid();
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

            _userOperations = new USERDAL();
            _userActionOperations = new User_action_PKDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();

            LoadActionQuery();

            if (ListType == ListType.Search)
            {
                UserActionGrid.GridVersion = UserActionGrid.GridVersion + ListType.ToString();
            }

            _gridId = UserActionGrid.GridId + "_" + UserActionGrid.GridVersion;
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
                mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
            }

            switch (ListType)
            {
                case ListType.List:
                    btnSaveLayout.Click += btnSaveLayout_Click;
                    btnClearLayout.Click += btnClearLayout_Click;
                    btnExport.Click += btnExport_Click;
                    btnColumns.Click += btnColumns_OnClick;
                    ColumnsPopup.OnOkButtonClick += ColumnsPopup_OnOkButtonClick;
                    btnReset.Click += btnReset_Click;

                    break;
            }

            UserActionGrid.OnRebindRequired += UserActionGridOnRebindRequired;
            UserActionGrid.OnHtmlRowPrepared += UserActionGridOnHtmlRowPrepared;
            UserActionGrid.OnHtmlCellPrepared += UserActionGridOnHtmlCellPrepared;
            UserActionGrid.OnExcelCellPrepared += UserActionGridOnExcelCellPrepared;
            UserActionGrid.OnLoadClientLayout += UserActionGridOnLoadClientLayout;
        }

        void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
        }

        #endregion

        #region Fill

        void ClearForm(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    break;
            }
        }

        void FillFormControls(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    break;
            }
        }

        private void FillComboActive()
        {
            var article57RelevantList = new List<ListItem>
                                            {
                                                new ListItem(""), 
                                                new ListItem("Yes"), 
                                                new ListItem("No")
                                            };
            UserActionGrid.SetComboList("Active", article57RelevantList);
        }

        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    break;
            }

            BindDynamicControls();
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                UserActionGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (UserActionGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(UserActionGrid.SecondSortingColumn, UserActionGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (UserActionGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(UserActionGrid.MainSortingColumn, UserActionGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("user_action_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _userActionOperations.GetListFormDataSet(filters, UserActionGrid.CurrentPage, UserActionGrid.PageSize, gobList, out itemCount);
            }

            UserActionGrid.TotalRecords = itemCount;

            if (UserActionGrid.CurrentPage > UserActionGrid.TotalPages || (UserActionGrid.CurrentPage == 0 && UserActionGrid.TotalPages > 0))
            {
                if (UserActionGrid.CurrentPage > UserActionGrid.TotalPages) UserActionGrid.CurrentPage = UserActionGrid.TotalPages; else UserActionGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    ds = _userActionOperations.GetListFormDataSet(filters, UserActionGrid.CurrentPage, UserActionGrid.PageSize, gobList, out itemCount);
                }
            }

            UserActionGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            UserActionGrid.DataBind();

            MasterPage.UpCommon.Update();
        }

        private void BindDynamicControls()
        {
            FillComboActive();
        }

        #endregion

        #region Validate

        void ValidateForm(object arg)
        {

        }

        #endregion

        #region Save

        void SaveForm(object arg)
        {

        }

        #endregion

        #region Delete

        private void DeleteEntity(int? entityPk)
        {
            if (entityPk.HasValue)
            {
                try
                {
                    _userActionOperations.Delete(entityPk);
                    Response.Redirect("~/Views/UserActionView/List.aspx?EntityContext=UserAction");
                }
                catch { }
            }

            MasterPage.ModalPopup.ShowModalPopup("Error!", "Could not delete entity! Contact your system administrator.");
        }

        #endregion

        #endregion

        #region Event handlers

        #region Delete

        public void btnDeleteEntity_OnClick(object sender, EventArgs e)
        {
            var deleteButton = sender as ImageButton;
            if (deleteButton == null) return;

            var commandNameString = Convert.ToString(deleteButton.CommandName);
            var commandArgumentString = Convert.ToString(deleteButton.CommandArgument);

            if (commandNameString == Constant.CommandArgument.Delete)
            {
                if (!SecurityHelper.IsPermitted(Permission.DeleteUserAction)) return;

                _userActionPkToDelete = ValidationHelper.IsValidInt(commandArgumentString) ? int.Parse(commandArgumentString) : (int?)null;
                if (_userActionPkToDelete.HasValue) mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
            }
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_userActionPkToDelete);
        }

        #endregion

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.New:
                    {
                        Response.Redirect(string.Format("~/Views/UserActionView/Form.aspx?EntityContext={0}&Action=New&From=UserAction", EntityContext.UserAction));
                    }
                    break;
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in UserActionGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("user_action_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (UserActionGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
                    {
                        ColumnsPopup.SelectedColumns.Add(new ListItem(caption, (column as IFilteredColumn).FieldName));
                    }
                    else
                    {
                        ColumnsPopup.AvailableColumns.Add(new ListItem(caption, (column as IFilteredColumn).FieldName));
                    }
                }
            }

            ColumnsPopup.ShowModalForm();
        }

        public void ColumnsPopup_OnOkButtonClick(object sender, EventArgs e)
        {
            UserActionGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            UserActionGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            
            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = UserActionGrid.GetClientLayoutString(),
                timestamp = DateTime.Now,
                ql_visible = true,
                isdefault = true,
                display_name = "SavedLayout"
            };

            userGridSettings = _userGridSettingsOperations.Save(userGridSettings);
            if (userGridSettings != null)
            {
                _userGridSettingsOperations.SetDefaultAndKeepFirstNLayouts(
                    Thread.CurrentPrincipal.Identity.Name,
                    _gridId,
                    userGridSettings.user_grid_settings_PK,
                    NumLayoutToKeep);
            }
        }

        void btnExport_Click(object sender, EventArgs e)
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>() { };
            if (UserActionGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(UserActionGrid.SecondSortingColumn, UserActionGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (UserActionGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(UserActionGrid.MainSortingColumn, UserActionGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("user_action_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _userActionOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            UserActionGrid["user_action_PK"].Visible = true;
            UserActionGrid["Delete"].Visible = false;
            if (ds != null) UserActionGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            UserActionGrid["Delete"].Visible = true;
            UserActionGrid["user_action_PK"].Visible = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            UserActionGrid.ResetVisibleColumns();
            UserActionGrid.SecondSortingColumn = null;
            UserActionGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
            BindGrid();
        }

        #endregion

        #region Grid

        void UserActionGridOnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {

        }
        
        void UserActionGridOnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {

        }

        void UserActionGridOnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void UserActionGridOnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!UserActionGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = UserActionGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (UserActionGrid.SortCount > 1 && e.FieldName == UserActionGrid.MainSortingColumn)
                return;
        }

        void UserActionGridOnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                args.ClientLayoutString = userGridSettings.grid_layout;
            }
        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContextMenuItems()
        {
            var contexMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                contexMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") };
            }

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contexMenu);
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location = null;

            if (ListType == ListType.List)
            {
                location = Support.LocationManager.Instance.GetLocationByName("UserAction", Support.CacheManager.Instance.AppLocations);
                if (location != null)
                {
                    MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                    MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            if (ListType == ListType.List)
            {
                location = Support.LocationManager.Instance.GetLocationByName("UserAction", Support.CacheManager.Instance.AppLocations);
            }

            if (location != null)
            {
                var topLevelParent = MasterPage.FindTopLevelParent(location);

                MasterPage.CurrentLocation = location;
                MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            }
        }

        private DataTable PrepareDataForExport(DataTable atcDataTable)
        {
            if (atcDataTable == null || atcDataTable.Rows.Count == 0) return atcDataTable;

            return atcDataTable;
        }

        public string HandleMissing(object value)
        {
            if (value != null && value.ToString().Trim() != "")
            {
                return value.ToString();
            }

            return "Missing";
        }
        private Dictionary<string, string> GetFilters()
        {
            var filters = UserActionGrid.GetFilters();

            return filters;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            base.SecurityPageSpecific();

            var isPermittedInsertUserAction = false;
            if (EntityContext == EntityContext.UserAction) isPermittedInsertUserAction = SecurityHelper.IsPermitted(Permission.InsertUserAction);

            if (isPermittedInsertUserAction)
            {
                MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }
            else
            {
                MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }

            if (SecurityHelper.IsPermitted(Permission.DeleteUserAction))
            {
                SecurityHelper.EnableImageButtonsWithCommandName(PnlGrid, "Delete");
            }
            else SecurityHelper.DisableImageButtonsWithCommandName(PnlGrid, "Delete");

            return true;
        }

        #endregion
    }
}