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

namespace AspNetUI.Views.SubstanceView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;
        private string _gridId;

        private ISubstance_PKOperations _substanceOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IUSEROperations _userOperations;

        #endregion

        #region Properties

        private int? _substancePkToDelete
        {
            get { return (int?)ViewState["_substancePkToDelete"]; }
            set { ViewState["_substancePkToDelete"] = value; }
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

            _substanceOperations = new Substance_PKDAL(); 
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();

            if (ListType == ListType.Search)
            {
                SubstanceGrid.GridVersion = SubstanceGrid.GridVersion + ListType.ToString();
            }

            _gridId = SubstanceGrid.GridId + "_" + SubstanceGrid.GridVersion;
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

            SubstanceGrid.OnRebindRequired += SubstanceGrid_OnRebindRequired;
            SubstanceGrid.OnHtmlRowPrepared += SubstanceGrid_OnHtmlRowPrepared;
            SubstanceGrid.OnHtmlCellPrepared += SubstanceGrid_OnHtmlCellPrepared;
            SubstanceGrid.OnExcelCellPrepared += SubstanceGrid_OnExcelCellPrepared;
            SubstanceGrid.OnLoadClientLayout += SubstanceGrid_OnLoadClientLayout;
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
                SubstanceGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (SubstanceGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(SubstanceGrid.SecondSortingColumn, SubstanceGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (SubstanceGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(SubstanceGrid.MainSortingColumn, SubstanceGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("substance_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _substanceOperations.GetListFormDataSet(filters, SubstanceGrid.CurrentPage, SubstanceGrid.PageSize, gobList, out itemCount);
            }

            SubstanceGrid.TotalRecords = itemCount;

            if (SubstanceGrid.CurrentPage > SubstanceGrid.TotalPages || (SubstanceGrid.CurrentPage == 0 && SubstanceGrid.TotalPages > 0))
            {
                if (SubstanceGrid.CurrentPage > SubstanceGrid.TotalPages) SubstanceGrid.CurrentPage = SubstanceGrid.TotalPages; else SubstanceGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    ds = _substanceOperations.GetListFormDataSet(filters, SubstanceGrid.CurrentPage, SubstanceGrid.PageSize, gobList, out itemCount);
                }
            }

            SubstanceGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            SubstanceGrid.DataBind();

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
                    _substanceOperations.Delete(entityPk);
                    Response.Redirect(string.Format("~/Views/SubstanceView/List.aspx?EntityContext={0}", EntityContext.Substance));
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
                if (!SecurityHelper.IsPermitted(Permission.DeleteSubstance)) return;

                _substancePkToDelete = ValidationHelper.IsValidInt(commandArgumentString) ? int.Parse(commandArgumentString) : (int?)null;
                if (_substancePkToDelete.HasValue) mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
            }
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_substancePkToDelete);
        }

        #endregion

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.New:
                    {
                        Response.Redirect(string.Format("~/Views/SubstanceView/Form.aspx?EntityContext={0}&Action=New&From=Substance", EntityContext.Substance));
                    }
                    break;
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in SubstanceGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("substance_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (SubstanceGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            SubstanceGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            SubstanceGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            
            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = SubstanceGrid.GetClientLayoutString(),
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
            if (SubstanceGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(SubstanceGrid.SecondSortingColumn, SubstanceGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (SubstanceGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(SubstanceGrid.MainSortingColumn, SubstanceGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("substance_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _substanceOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            SubstanceGrid["substance_PK"].Visible = true;
            SubstanceGrid["Delete"].Visible = false;
            if (ds != null) SubstanceGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            SubstanceGrid["substance_PK"].Visible = false;
            SubstanceGrid["Delete"].Visible = true;

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            SubstanceGrid.ResetVisibleColumns();
            SubstanceGrid.SecondSortingColumn = null;
            SubstanceGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
            BindGrid();
        }

        #endregion

        #region Grid

        void SubstanceGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {

        }

        void SubstanceGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {

        }

        void SubstanceGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void SubstanceGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!SubstanceGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = SubstanceGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (SubstanceGrid.SortCount > 1 && e.FieldName == SubstanceGrid.MainSortingColumn)
                return;
        }

        void SubstanceGrid_OnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
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
                location = Support.LocationManager.Instance.GetLocationByName("Substance", Support.CacheManager.Instance.AppLocations);
                if (location != null)
            {
                    MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                    MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                tabMenu.Visible = false;
            }
        }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            if (ListType == ListType.List)
            {
                location = Support.LocationManager.Instance.GetLocationByName("Substance", Support.CacheManager.Instance.AppLocations);
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
            var filters = SubstanceGrid.GetFilters();

            return filters;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            base.SecurityPageSpecific();

            var isPermittedInsertSubstance = false;
            if (EntityContext == EntityContext.Substance) isPermittedInsertSubstance = SecurityHelper.IsPermitted(Permission.InsertSubstance);

            if (isPermittedInsertSubstance)
            {
                MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }
            else
            {
                MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }

            if (SecurityHelper.IsPermitted(Permission.DeleteSubstance))
            {
                SecurityHelper.EnableImageButtonsWithCommandName(PnlGrid, "Delete");
            }
            else SecurityHelper.DisableImageButtonsWithCommandName(PnlGrid, "Delete");

            return true;
        }

        #endregion
    }
}