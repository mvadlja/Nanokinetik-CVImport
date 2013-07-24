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

namespace AspNetUI.Views.TaskNameView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;
        private string _gridId;

        private ITask_name_PKOperations _taskNameOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IUSEROperations _userOperations;

        #endregion

        #region Properties

        private int? _taskNamePkToDelete
        {
            get { return (int?)ViewState["_taskNamePkToDelete"]; }
            set { ViewState["_taskNamePkToDelete"] = value; }
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

            _taskNameOperations = new Task_name_PKDAL(); 
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();

            if (ListType == ListType.Search)
            {
                TaskNameGrid.GridVersion = TaskNameGrid.GridVersion + ListType.ToString();
            }

            _gridId = TaskNameGrid.GridId + "_" + TaskNameGrid.GridVersion;
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

            TaskNameGrid.OnRebindRequired += TaskNameGrid_OnRebindRequired;
            TaskNameGrid.OnHtmlRowPrepared += TaskNameGrid_OnHtmlRowPrepared;
            TaskNameGrid.OnHtmlCellPrepared += TaskNameGrid_OnHtmlCellPrepared;
            TaskNameGrid.OnExcelCellPrepared += TaskNameGrid_OnExcelCellPrepared;
            TaskNameGrid.OnLoadClientLayout += TaskNameGrid_OnLoadClientLayout;
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
                TaskNameGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (TaskNameGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(TaskNameGrid.SecondSortingColumn, TaskNameGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (TaskNameGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(TaskNameGrid.MainSortingColumn, TaskNameGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("task_name_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _taskNameOperations.GetListFormDataSet(filters, TaskNameGrid.CurrentPage, TaskNameGrid.PageSize, gobList, out itemCount);
            }

            TaskNameGrid.TotalRecords = itemCount;

            if (TaskNameGrid.CurrentPage > TaskNameGrid.TotalPages || (TaskNameGrid.CurrentPage == 0 && TaskNameGrid.TotalPages > 0))
            {
                if (TaskNameGrid.CurrentPage > TaskNameGrid.TotalPages) TaskNameGrid.CurrentPage = TaskNameGrid.TotalPages; else TaskNameGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    ds = _taskNameOperations.GetListFormDataSet(filters, TaskNameGrid.CurrentPage, TaskNameGrid.PageSize, gobList, out itemCount);
                }
            }

            TaskNameGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            TaskNameGrid.DataBind();

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
                    _taskNameOperations.Delete(entityPk);
                    Response.Redirect(string.Format("~/Views/TaskNameView/List.aspx?EntityContext={0}", EntityContext.TaskName));
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
                if (!SecurityHelper.IsPermitted(Permission.DeleteTaskName)) return;

                _taskNamePkToDelete = ValidationHelper.IsValidInt(commandArgumentString) ? int.Parse(commandArgumentString) : (int?)null;
                if (_taskNamePkToDelete.HasValue) mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
            }
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_taskNamePkToDelete);
        }

        #endregion

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.New:
                    {
                        Response.Redirect(string.Format("~/Views/TaskNameView/Form.aspx?EntityContext={0}&Action=New&&From=TaskName", EntityContext.TaskName));
                    }
                    break;
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in TaskNameGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("task_name_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (TaskNameGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            TaskNameGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            TaskNameGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            
            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = TaskNameGrid.GetClientLayoutString(),
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
            if (TaskNameGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(TaskNameGrid.SecondSortingColumn, TaskNameGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (TaskNameGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(TaskNameGrid.MainSortingColumn, TaskNameGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("task_name_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _taskNameOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            TaskNameGrid["task_name_PK"].Visible = true;
            TaskNameGrid["Delete"].Visible = false;
            if (ds != null) TaskNameGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            TaskNameGrid["task_name_PK"].Visible = false;
            TaskNameGrid["Delete"].Visible = true;

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            TaskNameGrid.ResetVisibleColumns();
            TaskNameGrid.SecondSortingColumn = null;
            TaskNameGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
            BindGrid();
        }

        #endregion

        #region Grid

        void TaskNameGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {

        }

        void TaskNameGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {

        }

        void TaskNameGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void TaskNameGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!TaskNameGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = TaskNameGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (TaskNameGrid.SortCount > 1 && e.FieldName == TaskNameGrid.MainSortingColumn)
                return;
        }

        void TaskNameGrid_OnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
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
                location = Support.LocationManager.Instance.GetLocationByName("TaskName", Support.CacheManager.Instance.AppLocations);
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
                location = Support.LocationManager.Instance.GetLocationByName("TaskName", Support.CacheManager.Instance.AppLocations);
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
            var filters = TaskNameGrid.GetFilters();

            return filters;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            base.SecurityPageSpecific();

            var isPermittedInsertTaskName = false;
            if (EntityContext == EntityContext.TaskName) isPermittedInsertTaskName = SecurityHelper.IsPermitted(Permission.InsertTaskName);

            if (isPermittedInsertTaskName)
            {
                MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }
            else
            {
                MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }

            if (SecurityHelper.IsPermitted(Permission.DeleteTaskName))
            {
                SecurityHelper.EnableImageButtonsWithCommandName(PnlGrid, "Delete");
            }
            else SecurityHelper.DisableImageButtonsWithCommandName(PnlGrid, "Delete");

            return true;
        }

        #endregion
    }
}