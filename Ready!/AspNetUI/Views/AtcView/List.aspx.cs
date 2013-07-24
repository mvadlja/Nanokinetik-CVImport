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

namespace AspNetUI.Views.AtcView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;
        private string _gridId;

        private IAtc_PKOperations _atcCodeOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IUSEROperations _userOperations;

        #endregion

        #region Properties

        private int? _atcPkToDelete
        {
            get { return (int?)ViewState["_atcPkToDelete"]; }
            set { ViewState["_atcPkToDelete"] = value; }
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

            BindGrid();
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

            _atcCodeOperations = new Atc_PKDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();

            if (ListType == ListType.Search)
            {
                AtcGrid.GridVersion = AtcGrid.GridVersion + ListType.ToString();
            }

            _gridId = AtcGrid.GridId + "_" + AtcGrid.GridVersion;
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

            AtcGrid.OnRebindRequired += AtcGrid_OnRebindRequired;
            AtcGrid.OnHtmlRowPrepared += AtcGrid_OnHtmlRowPrepared;
            AtcGrid.OnHtmlCellPrepared += AtcGrid_OnHtmlCellPrepared;
            AtcGrid.OnExcelCellPrepared += AtcGrid_OnExcelCellPrepared;
            AtcGrid.OnLoadClientLayout += AtcGrid_OnLoadClientLayout;
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

        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    break;
            }
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                AtcGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (AtcGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(AtcGrid.SecondSortingColumn, AtcGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (AtcGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(AtcGrid.MainSortingColumn, AtcGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("atc_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _atcCodeOperations.GetListFormDataSet(filters, AtcGrid.CurrentPage, AtcGrid.PageSize, gobList, out itemCount);
            }

            AtcGrid.TotalRecords = itemCount;

            if (AtcGrid.CurrentPage > AtcGrid.TotalPages || (AtcGrid.CurrentPage == 0 && AtcGrid.TotalPages > 0))
            {
                if (AtcGrid.CurrentPage > AtcGrid.TotalPages) AtcGrid.CurrentPage = AtcGrid.TotalPages; else AtcGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    ds = _atcCodeOperations.GetListFormDataSet(filters, AtcGrid.CurrentPage, AtcGrid.PageSize, gobList, out itemCount);
                }
            }

            AtcGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            AtcGrid.DataBind();

            MasterPage.UpCommon.Update();
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
                    _atcCodeOperations.Delete(entityPk);
                    Response.Redirect(string.Format("~/Views/AtcView/List.aspx?EntityContext={0}", EntityContext.Atc));
                }
                catch {}
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
                if (!SecurityHelper.IsPermitted(Permission.DeleteAtc)) return;

                _atcPkToDelete = ValidationHelper.IsValidInt(commandArgumentString) ? int.Parse(commandArgumentString) : (int?)null;
                if (_atcPkToDelete.HasValue) mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
            }
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_atcPkToDelete);
        }

        #endregion

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.New:
                    {
                        Response.Redirect(string.Format("~/Views/AtcView/Form.aspx?EntityContext={0}&Action=New&From=Atc", EntityContext.Atc));
                    }
                    break;
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in AtcGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("atc_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (AtcGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            AtcGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();
            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            AtcGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            
            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = AtcGrid.GetClientLayoutString(),
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
            if (AtcGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(AtcGrid.SecondSortingColumn, AtcGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (AtcGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(AtcGrid.MainSortingColumn, AtcGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("atc_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _atcCodeOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            AtcGrid["atc_PK"].Visible = true;
            AtcGrid["Delete"].Visible = false;
            if (ds != null) AtcGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            AtcGrid["atc_PK"].Visible = false;
            AtcGrid["Delete"].Visible = true;

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            AtcGrid.ResetVisibleColumns();
            AtcGrid.SecondSortingColumn = null;
            AtcGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
            BindGrid();
        }

        #endregion

        #region Grid

        void AtcGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {

        }

        void AtcGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            var lbtnDelete = e.FindControl("lbtnDeleteEntity") as LinkButton;

            if (lbtnDelete == null) return;

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
            {
                scriptManager.RegisterAsyncPostBackControl(lbtnDelete);
            }
        }

        void AtcGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void AtcGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!AtcGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = AtcGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (AtcGrid.SortCount > 1 && e.FieldName == AtcGrid.MainSortingColumn)
                return;
        }

        void AtcGrid_OnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
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
            Location_PK location;

            if (ListType == ListType.List)
            {
                location = Support.LocationManager.Instance.GetLocationByName("Atc", Support.CacheManager.Instance.AppLocations);
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
                location = Support.LocationManager.Instance.GetLocationByName("Atc", Support.CacheManager.Instance.AppLocations);
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
            var filters = AtcGrid.GetFilters();

            return filters;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            base.SecurityPageSpecific();

            var isPermittedInsertAtc = false;
            if (EntityContext == EntityContext.Atc) isPermittedInsertAtc = SecurityHelper.IsPermitted(Permission.InsertAtc);

            if (isPermittedInsertAtc)
            {
                MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }
            else
            {
                MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }

            if (SecurityHelper.IsPermitted(Permission.DeleteAtc))
            {
                SecurityHelper.EnableImageButtonsWithCommandName(PnlGrid, "Delete");
            }
            else SecurityHelper.DisableImageButtonsWithCommandName(PnlGrid, "Delete");

            return true;
        }

        #endregion
    }
}