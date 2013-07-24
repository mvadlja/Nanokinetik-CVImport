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

namespace AspNetUI.Views.UserRoleView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;
        private string _gridId;

        private IUSEROperations _userOperations;
        private IUSER_ROLEOperations _userRoleOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;

        #endregion

        #region Properties

        private int? _userRolePkToDelete
        {
            get { return (int?)ViewState["_userRolePkToDelete"]; }
            set { ViewState["_userRolePkToDelete"] = value; }
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
            _userRoleOperations = new USER_ROLEDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();

            LoadActionQuery();

            if (ListType == ListType.Search)
            {
                UserRoleGrid.GridVersion = UserRoleGrid.GridVersion + ListType.ToString();
            }

            _gridId = UserRoleGrid.GridId + "_" + UserRoleGrid.GridVersion;
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

            UserRoleGrid.OnRebindRequired += UserRoleGridOnRebindRequired;
            UserRoleGrid.OnHtmlRowPrepared += UserRoleGridOnHtmlRowPrepared;
            UserRoleGrid.OnHtmlCellPrepared += UserRoleGridOnHtmlCellPrepared;
            UserRoleGrid.OnExcelCellPrepared += UserRoleGridOnExcelCellPrepared;
            UserRoleGrid.OnLoadClientLayout += UserRoleGridOnLoadClientLayout;
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
            UserRoleGrid.SetComboList("Active", article57RelevantList);
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
                UserRoleGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (UserRoleGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(UserRoleGrid.SecondSortingColumn, UserRoleGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (UserRoleGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(UserRoleGrid.MainSortingColumn, UserRoleGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("user_role_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _userRoleOperations.GetListFormDataSet(filters, UserRoleGrid.CurrentPage, UserRoleGrid.PageSize, gobList, out itemCount);
            }

            UserRoleGrid.TotalRecords = itemCount;

            if (UserRoleGrid.CurrentPage > UserRoleGrid.TotalPages || (UserRoleGrid.CurrentPage == 0 && UserRoleGrid.TotalPages > 0))
            {
                if (UserRoleGrid.CurrentPage > UserRoleGrid.TotalPages) UserRoleGrid.CurrentPage = UserRoleGrid.TotalPages; else UserRoleGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    ds = _userRoleOperations.GetListFormDataSet(filters, UserRoleGrid.CurrentPage, UserRoleGrid.PageSize, gobList, out itemCount);
                }
            }

            UserRoleGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            UserRoleGrid.DataBind();

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
                    _userRoleOperations.Delete(entityPk);
                    Response.Redirect("~/Views/UserRoleView/List.aspx?EntityContext=UserRole");
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
                if (!SecurityHelper.IsPermitted(Permission.DeleteUserRole)) return;

                _userRolePkToDelete = ValidationHelper.IsValidInt(commandArgumentString) ? int.Parse(commandArgumentString) : (int?)null;
                if (_userRolePkToDelete.HasValue) mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
            }
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_userRolePkToDelete);
        }

        #endregion

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.New:
                    {
                        Response.Redirect(string.Format("~/Views/UserRoleView/Form.aspx?EntityContext={0}&Action=New&From=UserRole", EntityContext.UserRole));
                    }
                    break;
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in UserRoleGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("user_role_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (UserRoleGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            UserRoleGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            UserRoleGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            
            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = UserRoleGrid.GetClientLayoutString(),
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
            if (UserRoleGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(UserRoleGrid.SecondSortingColumn, UserRoleGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (UserRoleGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(UserRoleGrid.MainSortingColumn, UserRoleGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("user_role_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _userRoleOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            UserRoleGrid["user_role_PK"].Visible = true;
            UserRoleGrid["Delete"].Visible = false;
            if (ds != null) UserRoleGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            UserRoleGrid["user_role_PK"].Visible = false;
            UserRoleGrid["Delete"].Visible = true;

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            UserRoleGrid.ResetVisibleColumns();
            UserRoleGrid.SecondSortingColumn = null;
            UserRoleGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
            BindGrid();
        }

        #endregion

        #region Grid

        void UserRoleGridOnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {

        }

        void UserRoleGridOnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {

        }

        void UserRoleGridOnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void UserRoleGridOnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!UserRoleGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = UserRoleGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (UserRoleGrid.SortCount > 1 && e.FieldName == UserRoleGrid.MainSortingColumn)
                return;
        }

        void UserRoleGridOnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
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
                location = Support.LocationManager.Instance.GetLocationByName("UserRole", Support.CacheManager.Instance.AppLocations);
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
                location = Support.LocationManager.Instance.GetLocationByName("UserRole", Support.CacheManager.Instance.AppLocations);
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
            var filters = UserRoleGrid.GetFilters();

            return filters;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            base.SecurityPageSpecific();

            var isPermittedInsertUserRole = false;
            if (EntityContext == EntityContext.UserRole) isPermittedInsertUserRole = SecurityHelper.IsPermitted(Permission.InsertUserRole);

            if (isPermittedInsertUserRole)
            {
                MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }
            else
            {
                MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }

            if (SecurityHelper.IsPermitted(Permission.DeleteUserRole))
            {
                SecurityHelper.EnableImageButtonsWithCommandName(PnlGrid, "Delete");
            }
            else SecurityHelper.DisableImageButtonsWithCommandName(PnlGrid, "Delete");

            return true;
        }

        #endregion
    }
}