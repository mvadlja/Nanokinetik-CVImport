using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using GEM2Common;
using Ready.Model;

namespace AspNetUI.Views.UserView
{
    public partial class List : ListPage
    {
        #region Declarations

        private ListType _listType;
        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;
        private CultureInfo CultureInfoHr;

        private IUSEROperations _userOperations;

        #endregion

        #region Properties

        private int? _userPkToDelete
        {
            get { return (int?)ViewState["_userPkToDelete"]; }
            set { ViewState["_userPkToDelete"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadFormVariables();
            if (_listType == ListType.Unknown) Response.Redirect("~\\Views\\UserView\\List.aspx");

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

        private void LoadFormVariables()
        {
            _listType = ListType.Unknown;

            switch (Request.QueryString["Action"])
            {
                default:
                    _listType = ListType.List;
                    break;
            }

            _userOperations = new USERDAL();

            CultureInfoHr = new CultureInfo("hr-HR");
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
                mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
            }

            switch (_listType)
            {
                case ListType.List:
                    btnExport.Click += btnExport_Click;

                    break;
            }

            UserGrid.OnRebindRequired += UserGrid_OnRebindRequired;
            UserGrid.OnHtmlRowPrepared += UserGrid_OnHtmlRowPrepared;
            UserGrid.OnHtmlCellPrepared += UserGrid_OnHtmlCellPrepared;
            UserGrid.OnExcelCellPrepared += UserGrid_OnExcelCellPrepared;
            UserGrid.OnLoadClientLayout += UserGrid_OnLoadClientLayout;
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
            switch (_listType)
            {
                case ListType.List:
                    break;
            }
        }

        void FillFormControls(object arg)
        {
            switch (_listType)
            {
                case ListType.List:
                    break;
            }
        }

        void SetFormControlsDefaults(object arg)
        {
            switch (_listType)
            {
                case ListType.List:
                    break;
            }

            BindDynamicControls(null);
        }

        void FillComboActive(object arg)
        {
            // Fill authorisation procedure combo box
            var activeListItems = new List<ListItem>() { new ListItem("", ""), new ListItem("True", "True"), new ListItem("False", "False") };
            UserGrid.SetComboList("active", activeListItems);
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {

        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (UserGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(UserGrid.SecondSortingColumn, UserGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (UserGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(UserGrid.MainSortingColumn, UserGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("user_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (_listType == ListType.List)
            {
                ds = _userOperations.GetListFormDataSet(filters, UserGrid.CurrentPage, UserGrid.PageSize, gobList, out itemCount);
            }

            UserGrid.TotalRecords = itemCount;

            if (UserGrid.CurrentPage > UserGrid.TotalPages || (UserGrid.CurrentPage == 0 && UserGrid.TotalPages > 0))
            {
                if (UserGrid.CurrentPage > UserGrid.TotalPages) UserGrid.CurrentPage = UserGrid.TotalPages; else UserGrid.CurrentPage = 1;

                if (_listType == ListType.List)
                {
                    ds = _userOperations.GetListFormDataSet(filters, UserGrid.CurrentPage, UserGrid.PageSize, gobList, out itemCount);
                }
            }

            UserGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            UserGrid.DataBind();

            MasterPage.UpCommon.Update();
        }

        private void BindDynamicControls(object args)
        {
            FillComboActive(null);

            if (_listType == ListType.Search) subtabs.Controls.Clear();
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
                _userOperations.Delete(entityPk);
                Response.Redirect("~/Views/UserView/List.aspx");
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
                _userPkToDelete = ValidationHelper.IsValidInt(commandArgumentString) ? int.Parse(commandArgumentString) : (int?)null;
                if (_userPkToDelete.HasValue) mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
            }
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_userPkToDelete);
        }

        #endregion

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.New:
                    {
                        Response.Redirect("~/Views/Operational/UsersView.aspx");
                    }
                    break;
            }
        }

        #endregion

        #region Grid

        void UserGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {

        }

        void btnExport_Click(object sender, EventArgs e)
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>() { };
            if (UserGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(UserGrid.SecondSortingColumn, UserGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (UserGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(UserGrid.MainSortingColumn, UserGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("user_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (_listType == ListType.List)
            {
                ds = _userOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            UserGrid["user_PK"].Visible = true;
            if (ds != null) UserGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            UserGrid["user_PK"].Visible = false;

        }

        void UserGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            var lbtnDelete = e.FindControl("lbtnDeleteEntity") as LinkButton;

            if (lbtnDelete == null) return;

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
            {
                scriptManager.RegisterAsyncPostBackControl(lbtnDelete);
            }
        }

        void UserGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void UserGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!UserGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = UserGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (UserGrid.SortCount > 1 && e.FieldName == UserGrid.MainSortingColumn)
                return;
        }

        void UserGrid_OnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {

        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContextMenuItems()
        {
            var contexMenu = new ContextMenuItem[] { };

            if (_listType == ListType.List)
            {
                contexMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") };
            }

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contexMenu);
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location;

            if (_listType == ListType.List)
            {
                location =  Support.LocationManager.Instance.GetLocationByName("User", Support.CacheManager.Instance.AppLocations);
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                tabMenu.Visible = false;
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            if (_listType == ListType.List)
            {
                location =  Support.LocationManager.Instance.GetLocationByName("User", Support.CacheManager.Instance.AppLocations);
            }

            var topLevelParent = MasterPage.FindTopLevelParent(location);

            MasterPage.CurrentLocation = location;
            MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
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
            var filters = UserGrid.GetFilters();

            return filters;
        }

        #endregion
    }
}