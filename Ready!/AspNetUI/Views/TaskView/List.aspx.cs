using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUIFramework;
using GEM2Common;
using PossGrid;
using Ready.Model;

namespace AspNetUI.Views.TaskView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private bool isRed;
        private const int NumLayoutToKeep = 5;
        private int? _idSearch;
        private int? _idAct;
        private string _gridId;

        private IActivity_PKOperations _activityOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IUSEROperations _userOperations;
        private IType_PKOperations _typeOperations;
        private ICountry_PKOperations _countryOperations;
        private IPerson_PKOperations _personOperations;
        private ITask_PKOperations _taskOperations;
        private ITask_saved_search_PKOperations _savedTaskSearch;
        private IActivity_product_PKOperations _activityProductMnOperations;

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
            AssociatePanels(pnlSearch, pnlGrid);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var scriptManager = ScriptManager.GetCurrent(Page);
            if (scriptManager != null)
            {
                scriptManager.RegisterPostBackControl(btnExport);
                scriptManager.RegisterPostBackControl(btnExportLower);
            }

            BindGridDynamicControls();

            if (!IsPostBack)
            {
                InitForm(null);
                BindForm(null);
            }

            if (ListType == ListType.Search)
            {
                if (IsPostbackFromGrid() != false)
                {
                    BindGrid();
                }
            }
            else
            {
                BindGrid();
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

            _idSearch = ValidationHelper.IsValidInt(Request.QueryString["idSearch"]) ? int.Parse(Request.QueryString["idSearch"]) : (int?)null;
            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;

            _activityOperations = new Activity_PKDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();
            _typeOperations = new Type_PKDAL();
            _countryOperations = new Country_PKDAL();
            _personOperations = new Person_PKDAL();
            _taskOperations = new Task_PKDAL();
            _savedTaskSearch = new Task_saved_search_PKDAL();
            _activityProductMnOperations = new Activity_product_PKDAL();

            if (ListType == ListType.Search)
            {
                TaskGrid.GridVersion = TaskGrid.GridVersion + ListType.ToString();
            }
            else if (EntityContext != EntityContext.Default)
            {
                TaskGrid.GridVersion = TaskGrid.GridVersion + EntityContext.ToString();
            }

            _gridId = TaskGrid.GridId + "_" + TaskGrid.GridVersion;
        }

       private void BindEventHandlers()
        {
            if (MasterPage != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
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
                case ListType.Search:
                    txtSrActivity.Searcher.OnListItemSelected += ActivitySearcher_OnListItemSelected;
                    QuickLinksPopup.OnOkButtonClick += QuickLinksPopup_OnOkButtonClick;

                    btnExportLower.Click += btnExport_Click;
                    subtabs.Controls.Clear();
                    break;
            }

            TaskGrid.OnRebindRequired += TaskGrid_OnRebindRequired;
            TaskGrid.OnHtmlRowPrepared += TaskGrid_OnHtmlRowPrepared;
            TaskGrid.OnHtmlCellPrepared += TaskGrid_OnHtmlCellPrepared;
            TaskGrid.OnExcelCellPrepared += TaskGrid_OnExcelCellPrepared;
            TaskGrid.OnLoadClientLayout += TaskGrid_OnLoadClientLayout;
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
                case ListType.Search:
                    ClearSearch();
                    break;
            }
        }

        void FillFormControls(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    break;
                case ListType.Search:
                    FillDdlResponsibleUsers();
                    FillDdlInternalStatus();
                    FillDdlCountry();
                    break;
            }
        }

        private void FillDdlResponsibleUsers()
        {
            var responsibleUserList = _personOperations.GetEntitiesByRoleName(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, "FullName", "person_PK");
            ddlResponsibleUser.SortItemsByText();
        }

        private void FillDdlInternalStatus()
        {
            var internalStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.InternalStatus);
            ddlInternalStatus.Fill(internalStatusList, "name", "type_PK");
            ddlInternalStatus.SortItemsByText();
        }

        private void FillDdlCountry()
        {
            var countryList = _countryOperations.GetEntitiesCustomSort();
            ddlCountry.Fill(countryList, Constant.Countries.DisplayNameFormat, "country_PK");
        }

        private void FillComboRegulatoryStatus()
        {
            var regulatoryStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.RegulatoryStatus);
            var regulatoryStatusListItems = new List<ListItem> { new ListItem("", "") };
            regulatoryStatusList.ForEach(item => regulatoryStatusListItems.Add(new ListItem(item.name, item.name)));
            TaskGrid.SetComboList("RegulatoryStatus", regulatoryStatusListItems);
        }

        private void FillComboInternalStatus()
        {
            var internalStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.InternalStatus);
            var internalStatusListItems = new List<ListItem> { new ListItem("", "") };
            internalStatusList.ForEach(item => internalStatusListItems.Add(new ListItem(item.name, item.name)));
            TaskGrid.SetComboList("InternalStatus", internalStatusListItems);
        }

        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) HandleEntityContextActivity();
                    HideSearch();

                    if (Request.QueryString["idLay"] == "default")
                    {
                        TaskGrid.ClearFilters();
                    }
                    break;
                case ListType.Search:
                    if (!IsPostBack)
                    {
                        var clear = Request.QueryString["Clear"];
                        if (_idSearch.HasValue && string.IsNullOrWhiteSpace(clear)) ShowAll();
                        else ShowSearch();
                        btnExportLower.Visible = _idSearch.HasValue && string.IsNullOrWhiteSpace(clear);
                        btnDeleteSearch.Visible = (string.IsNullOrWhiteSpace(clear) || clear == "true") && _idSearch.HasValue;
                    }

                    btnSaveLayout.Visible = false;
                    btnClearLayout.Visible = false;
                    btnExport.Visible = false;
                    btnColumns.Visible = false;
                    btnReset.Visible = false;

                    break;
            }

            BindDynamicControls(null);
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                TaskGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }

            if (!_idSearch.HasValue || Request.QueryString["Clear"] == "true") return;

            var savedTaskSearch = _savedTaskSearch.GetEntity(_idSearch);

            if (savedTaskSearch == null) return;

            if (ListType == ListType.Search)
            {
                BindActivity(savedTaskSearch.activity_FK);
                txtTaskName.Text = savedTaskSearch.name;
                ddlResponsibleUser.SelectedId = savedTaskSearch.user_FK;
                ddlInternalStatus.SelectedId = savedTaskSearch.type_internal_status_FK;
                ddlCountry.SelectedId = savedTaskSearch.country_FK;
                dtRngStartDate.TextFrom = savedTaskSearch.start_date_from.HasValue ? savedTaskSearch.start_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngStartDate.TextTo = savedTaskSearch.start_date_to.HasValue ? savedTaskSearch.start_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngExpectedFinishedDate.TextFrom = savedTaskSearch.expected_finished_date_from.HasValue ? savedTaskSearch.expected_finished_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngExpectedFinishedDate.TextTo = savedTaskSearch.expected_finished_date_to.HasValue ? savedTaskSearch.expected_finished_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngActualFinishedDate.TextFrom = savedTaskSearch.actual_finished_date_from.HasValue ? savedTaskSearch.actual_finished_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngActualFinishedDate.TextTo = savedTaskSearch.actual_finished_date_to.HasValue ? savedTaskSearch.actual_finished_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
            }
            
            TaskGrid.SetClientLayout(savedTaskSearch.gridLayout);
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (TaskGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(TaskGrid.SecondSortingColumn, TaskGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (TaskGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(TaskGrid.MainSortingColumn, TaskGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("task_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _taskOperations.GetListFormSearchDataSet(filters, TaskGrid.CurrentPage, TaskGrid.PageSize, gobList, out itemCount);
                else ds = _taskOperations.GetListFormDataSet(filters, TaskGrid.CurrentPage, TaskGrid.PageSize, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _taskOperations.GetListFormSearchDataSet(filters, TaskGrid.CurrentPage, TaskGrid.PageSize, gobList, out itemCount);
            }

            TaskGrid.TotalRecords = itemCount;

            if (TaskGrid.CurrentPage > TaskGrid.TotalPages || (TaskGrid.CurrentPage == 0 && TaskGrid.TotalPages > 0))
            {
                if (TaskGrid.CurrentPage > TaskGrid.TotalPages) TaskGrid.CurrentPage = TaskGrid.TotalPages; else TaskGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    if (_idSearch.HasValue) ds = _taskOperations.GetListFormSearchDataSet(filters, TaskGrid.CurrentPage, TaskGrid.PageSize, gobList, out itemCount);
                    else ds = _taskOperations.GetListFormDataSet(filters, TaskGrid.CurrentPage, TaskGrid.PageSize, gobList, out itemCount);
                }
                else if (ListType == ListType.Search)
                {
                    ds = _taskOperations.GetListFormSearchDataSet(filters, TaskGrid.CurrentPage, TaskGrid.PageSize, gobList, out itemCount);
                }
            }

            TaskGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            TaskGrid.DataBind();

            MasterPage.UpCommon.Update();
        }

        private void BindGridDynamicControls()
        {
            FillComboRegulatoryStatus();
            FillComboInternalStatus();
        }

        private void BindDynamicControls(object args)
        {
            if (ListType == ListType.Search) subtabs.Controls.Clear();
        }

        private void BindActivity(int? activityFk)
        {
            var activity = _activityOperations.GetEntity(activityFk);
            if (activity == null || activity.activity_PK == null) return;

            txtSrActivity.SelectedEntityId = activity.activity_PK;
            txtSrActivity.Text = !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.MissingValue;
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
                case ContextMenuEventTypes.New:
                    {
                        MasterPage.OneTimePermissionToken = Permission.View;
                        if (EntityContext == EntityContext.Activity && _idAct.HasValue) Response.Redirect(string.Format("~/Views/TaskView/Form.aspx?EntityContext={0}&Action=New&idAct={1}&From=ActTaskList", EntityContext, _idAct.Value));
                        else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) Response.Redirect(string.Format("~/Views/TaskView/Form.aspx?EntityContext={0}&Action=New&idAct={1}&From=ActMyTaskList", EntityContext, _idAct.Value));
                        Response.Redirect(string.Format("~/Views/TaskView/Form.aspx?EntityContext={0}&Action=New&From=Task", EntityContext.Default));
                    }
                    break;

                case ContextMenuEventTypes.Back:
                    {
                        if (EntityContext == EntityContext.Task) Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
                        Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext.Task));
                    }
                    break;
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in TaskGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("task_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (TaskGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            TaskGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            TaskGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            
            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = TaskGrid.GetClientLayoutString(),
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
            if (TaskGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(TaskGrid.SecondSortingColumn, TaskGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (TaskGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(TaskGrid.MainSortingColumn, TaskGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("task_PK", GEMOrderByType.DESC));

            int itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _taskOperations.GetListFormSearchDataSet(filters, 1, int.MaxValue, gobList, out itemCount); // Quick link
                else ds = _taskOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _taskOperations.GetListFormSearchDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            TaskGrid["task_PK"].Visible = true;
            if (ds != null) TaskGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            TaskGrid["task_PK"].Visible = false;

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            TaskGrid.ResetVisibleColumns();
            TaskGrid.SecondSortingColumn = null;
            TaskGrid.MainSortingOrder = PossGrid.SortOrder.DESC;
            BindGrid();
        }

        #endregion

        #region Search buttons

        public void btnSearchClick(object sender, EventArgs e)
        {
            pnlGrid.AddCssClass("display-block");
            btnExportLower.Visible = true;
            BindGrid();
        }

        public void btnClearClick(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}&Action=Search&Clear=true{1}", EntityContext.Task, (_idSearch.HasValue ? "&idSearch=" + _idSearch : string.Empty)));
        }

        #endregion

        #region Grid

        void TaskGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {
            if (args.RowType != DataControlRowType.DataRow) return;

            if (args.FieldName == "TaskName")
            {
                string appUrl = ConfigurationManager.AppSettings["ApplicationURL"];
                appUrl = appUrl.StartsWith("http://") ? appUrl : "http://" + appUrl;
                args.Cell.Url = appUrl + "/Views/Business/TaskProperties.aspx?f=l&idTask=" + Convert.ToString(args.Row["task_PK"]);
                args.Cell.FontUnderline = true;
                args.Cell.Text = HandleMissing(args.Cell.Text);
            }
        }

        void TaskGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var taskPk = ValidationHelper.IsValidInt(Convert.ToString(e.GetValue("task_PK"))) ? (int?)Convert.ToInt32(e.GetValue("task_PK")) : null;

            string internalStatus = e.GetValue("InternalStatus").ToString();
            internalStatus = !string.IsNullOrWhiteSpace(internalStatus) ? internalStatus.Trim() : string.Empty;

            isRed = false;

            var expectedFinishedDate = ValidationHelper.IsValidDateTime(Convert.ToString(e.GetValue("ExpectedFinishedDate"))) ? Convert.ToDateTime(e.GetValue("ExpectedFinishedDate")) : (DateTime?)null;

            SetGridRowColor(expectedFinishedDate, internalStatus.ToLower(), e);

            var pnlStatusColor = e.FindControl("pnlStatusColor") as HtmlGenericControl;
            if (pnlStatusColor != null)
            {
                SetGridStatusColor(pnlStatusColor, internalStatus.ToLower());
            }

            var pnlProducts = e.FindControl("pnlProducts") as Panel;
            if (pnlProducts != null && taskPk.HasValue)
            {
                BindProducts(pnlProducts, taskPk.Value);
            }

            if (Request.QueryString["idLay"] != null)
            {
                var hlId = e.FindControl("hlId") as HyperLink;
                if (hlId != null)
                {
                    hlId.NavigateUrl += string.Format("&idLay={0}", Request.QueryString["idLay"]);
                }
            }

            var hlNewDocument = e.FindControl("hlNewDocument") as HyperLink;
            if (hlNewDocument != null)
            {
                if (SecurityHelper.IsPermitted(Permission.InsertDocument)) BindNewDocumentLink(hlNewDocument);
                else hlNewDocument.NavigateUrl = string.Empty;
            }

            var hlNewSubUnit = e.FindControl("hlNewSubUnit") as HyperLink;
            if (hlNewSubUnit != null)
            {
                if (SecurityHelper.IsPermitted(Permission.InsertSubmissionUnit)) BindNewSubmissionUnitLink(hlNewSubUnit);
                else hlNewSubUnit.NavigateUrl = string.Empty;
            } 
        }

        void TaskGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void TaskGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!TaskGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = TaskGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (TaskGrid.SortCount > 1 && e.FieldName == TaskGrid.MainSortingColumn)
                return;

            if (e.FieldName == "TaskName" || e.FieldName == "Countries" || e.FieldName == "ActivityName" || e.FieldName == "InternalStatus" || e.FieldName == "Products")
            {
                e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        void TaskGrid_OnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                args.ClientLayoutString = userGridSettings.grid_layout;
            }
        }

        #endregion

        #region Activity searcher

        /// <summary>
        /// Handles activity list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ActivitySearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var activity = _activityOperations.GetEntity(e.Data);

            if (activity == null || activity.activity_PK == null) return;

            txtSrActivity.Text = activity.name;
            txtSrActivity.SelectedEntityId = activity.activity_PK;
        }

        #endregion

        #region Quick links

        public void btnSaveSearchClick(object sender, EventArgs e)
        {
            QuickLink quickLink = null;
            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                var savedSearch = _savedTaskSearch.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
                if (savedSearch != null)
                {
                    quickLink = new QuickLink
                    {
                        Name = savedSearch.displayName,
                        IsPublic = savedSearch.isPublic
                    };
                }
            }

            QuickLinksPopup.ShowModalForm(quickLink);
        }

        public void btnDeleteSearchClick(object sender, EventArgs e)
        {
            if (!ValidationHelper.IsValidInt(Request.QueryString["idSearch"])) return;

            _savedTaskSearch.Delete(Convert.ToInt32(Request.QueryString["idSearch"]));
            Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}&Action=Search", EntityContext.Task));
        }

        protected void QuickLinksPopup_OnOkButtonClick(object sender, FormEventArgs<QuickLink> e)
        {
            Task_saved_search_PK savedTaskSearch = null;

            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                savedTaskSearch = _savedTaskSearch.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
            }

            if (savedTaskSearch == null)
            {
                savedTaskSearch = new Task_saved_search_PK();
            }

            var nextStartDateFrom = ValidationHelper.IsValidDateTime(dtRngStartDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngStartDate.TextFrom, CultureInfoHr) : null;
            var nextStartDateTo = ValidationHelper.IsValidDateTime(dtRngStartDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngStartDate.TextTo, CultureInfoHr) : null;
            var nextExpectedFinishedDateFrom = ValidationHelper.IsValidDateTime(dtRngExpectedFinishedDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngExpectedFinishedDate.TextFrom, CultureInfoHr) : null;
            var nextExpectedFinishedDateTo = ValidationHelper.IsValidDateTime(dtRngExpectedFinishedDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngExpectedFinishedDate.TextTo, CultureInfoHr) : null;
            var nextActualFinishedDateFrom = ValidationHelper.IsValidDateTime(dtRngActualFinishedDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngActualFinishedDate.TextFrom, CultureInfoHr) : null;
            var nextActualFinishedDateTo = ValidationHelper.IsValidDateTime(dtRngActualFinishedDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngActualFinishedDate.TextTo, CultureInfoHr) : null;

            savedTaskSearch.activity_FK = txtSrActivity.SelectedEntityId;
            savedTaskSearch.name = txtTaskName.Text;
            savedTaskSearch.user_FK = ddlResponsibleUser.SelectedId;
            savedTaskSearch.type_internal_status_FK = ddlInternalStatus.SelectedId;
            savedTaskSearch.country_FK = ddlCountry.SelectedId;
            savedTaskSearch.start_date_from = nextStartDateFrom;
            savedTaskSearch.start_date_to = nextStartDateTo;
            savedTaskSearch.expected_finished_date_from = nextExpectedFinishedDateFrom;
            savedTaskSearch.expected_finished_date_to = nextExpectedFinishedDateTo;
            savedTaskSearch.actual_finished_date_from = nextActualFinishedDateFrom;
            savedTaskSearch.actual_finished_date_to = nextActualFinishedDateTo;
            savedTaskSearch.gridLayout = TaskGrid.GetClientLayoutString();

            var quickLink = e.Data;
            if (quickLink != null)
            {
                savedTaskSearch.displayName = quickLink.Name;
                savedTaskSearch.isPublic = quickLink.IsPublic.HasValue && quickLink.IsPublic.Value;
            }

            var user = _userOperations.GetUserByUsername(SessionManager.Instance.CurrentUser.Username);
            if (user != null)
            {
                savedTaskSearch.user_FK1 = user.Person_FK;
            }

            savedTaskSearch = _savedTaskSearch.Save(savedTaskSearch);
            Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}&Action=Search&idSearch={1}", EntityContext.Task, savedTaskSearch.task_saved_search_PK));
        }

        #endregion

        #endregion

        #region Support methods

        private void ClearSearch()
        {
            txtTaskName.Text = string.Empty;
            ddlResponsibleUser.SelectedValue = string.Empty;
            ddlInternalStatus.SelectedValue = string.Empty;
            ddlCountry.SelectedValue = string.Empty;
            dtRngStartDate.Clear();
            dtRngExpectedFinishedDate.Clear();
            dtRngActualFinishedDate.Clear();
        }

        private Dictionary<string, string> GetFilters()
        {
            var filters = TaskGrid.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (_idSearch.HasValue)
                    {
                        Task_saved_search_PK savedTaskSearc = _savedTaskSearch.GetEntity(_idSearch);
                        FillFilters(savedTaskSearc, filters);
                    }
                    if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
                    {
                        filters.Add("QueryBy", EntityContext.Activity.ToString());
                        filters.Add("EntityPk", Convert.ToString(_idAct));
                    }

                    if (Request.QueryString["idLay"] == "my" && !filters.ContainsKey("ResponsibleUserFk"))
                    {
                        var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                        if (user != null && user.Person_FK.HasValue)
                        {
                            filters.Add("ResponsibleUserFk", user.Person_FK.Value.ToString());
                        }
                    }
                    else if (Request.QueryString["idLay"] == "alert")
                    {
                        if (!filters.ContainsKey("ResponsibleUserFk"))
                        {
                            var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                            if (user != null && user.Person_FK.HasValue)
                            {
                                filters.Add("ResponsibleUserFk", user.Person_FK.Value.ToString());
                            }
                        }

                        if (filters.ContainsKey("QueryBy"))
                        {
                            filters["QueryBy"] = "TaskMyAlerts";
                        }
                        else
                        {
                            filters.Add("QueryBy", "TaskMyAlerts");
                        }
                    }
                    break;
                case ListType.Search:
                    FillFilters(filters);
                    break;
            }

            return filters;
        }

        private void FillFilters(Dictionary<string, string> filters)
        {
            if (txtSrActivity.SelectedEntityId.HasValue) filters.Add("SearchActivityPk", txtSrActivity.SelectedEntityId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtTaskName.Text)) filters.Add("SearchTaskName", txtTaskName.Text);
            if (ddlResponsibleUser.SelectedId.HasValue) filters.Add("SearchResponsibleUserPk", ddlResponsibleUser.SelectedId.Value.ToString());
            if (ddlInternalStatus.SelectedId.HasValue) filters.Add("SearchInternalStatusPk", ddlInternalStatus.SelectedId.Value.ToString());
            if (ddlCountry.SelectedId.HasValue) filters.Add("SearchCountryPk", ddlCountry.SelectedId.Value.ToString());
            if (ValidationHelper.IsValidDateTime(dtRngStartDate.TextFrom, CultureInfoHr)) filters.Add("SearchStartDateFrom", dtRngStartDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngStartDate.TextTo, CultureInfoHr)) filters.Add("SearchStartDateTo", dtRngStartDate.TextTo);
            if (ValidationHelper.IsValidDateTime(dtRngExpectedFinishedDate.TextFrom, CultureInfoHr)) filters.Add("SearchExpectedFinishedDateFrom", dtRngExpectedFinishedDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngExpectedFinishedDate.TextTo, CultureInfoHr)) filters.Add("SearchExpectedFinishedDateTo", dtRngExpectedFinishedDate.TextTo);
            if (ValidationHelper.IsValidDateTime(dtRngActualFinishedDate.TextFrom, CultureInfoHr)) filters.Add("SearchActualFinishedDateFrom", dtRngActualFinishedDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngActualFinishedDate.TextTo, CultureInfoHr)) filters.Add("SearchActualFinishedDateTo", dtRngActualFinishedDate.TextTo);
        }

        private void FillFilters(Task_saved_search_PK savedTaskSearch, Dictionary<string, string> filters)
        {
            if (savedTaskSearch.activity_FK.HasValue) filters.Add("SearchActivityPk", savedTaskSearch.activity_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedTaskSearch.name)) filters.Add("SearchTaskName", savedTaskSearch.name);
            if (savedTaskSearch.user_FK.HasValue) filters.Add("SearchResponsibleUserPk", savedTaskSearch.user_FK.Value.ToString());
            if (savedTaskSearch.type_internal_status_FK.HasValue) filters.Add("SearchInternalStatusPk", savedTaskSearch.type_internal_status_FK.Value.ToString());
            if (savedTaskSearch.country_FK.HasValue) filters.Add("SearchCountryPk", savedTaskSearch.country_FK.Value.ToString());
            if (savedTaskSearch.start_date_from.HasValue) filters.Add("SearchStartDateFrom", savedTaskSearch.start_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedTaskSearch.start_date_to.HasValue) filters.Add("SearchStartDateTo", savedTaskSearch.start_date_to.Value.ToString(Constant.DateTimeFormat));
            if (savedTaskSearch.expected_finished_date_from.HasValue) filters.Add("SearchExpectedFinishedDateFrom", savedTaskSearch.expected_finished_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedTaskSearch.expected_finished_date_to.HasValue) filters.Add("SearchExpectedFinishedDateTo", savedTaskSearch.expected_finished_date_to.Value.ToString(Constant.DateTimeFormat));
            if (savedTaskSearch.actual_finished_date_from.HasValue) filters.Add("SearchActualFinishedDateFrom", savedTaskSearch.actual_finished_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedTaskSearch.actual_finished_date_to.HasValue) filters.Add("SearchActualFinishedDateTo", savedTaskSearch.actual_finished_date_to.Value.ToString(Constant.DateTimeFormat));
        }

        private void GenerateContextMenuItems()
        {
            var contextMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                switch (EntityContext)
                {
                    case EntityContext.Task:
                        contextMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") };
                        break;
                    case EntityContext.Activity:
                    case EntityContext.ActivityMy:
                        contextMenu = new[] { 
                                                new ContextMenuItem(ContextMenuEventTypes.Back, "Back"), 
                                                new ContextMenuItem(ContextMenuEventTypes.New, "New") 
                                           };
                        break;
                }

                MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);
            }
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location = null;

            if (ListType == ListType.List)
            {
                if (EntityContext == EntityContext.Task)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("Task", Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                    if (location != null)
                    {
                        MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                        MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    }
                    return;
                }
               
                if (EntityContext == EntityContext.Activity) location = Support.LocationManager.Instance.GetLocationByName("ActTaskList", Support.CacheManager.Instance.AppLocations);
                else if (EntityContext == EntityContext.ActivityMy) location = Support.LocationManager.Instance.GetLocationByName("ActMyTaskList", Support.CacheManager.Instance.AppLocations);
                 
                    MasterPage.TabMenu.TabControls.Clear();
                    tabMenu.Visible = true;
                if (location != null)
                {
                    tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                    tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
            else if (ListType == ListType.Search)
            {
                location = Support.LocationManager.Instance.GetLocationByName("TaskSearch", Support.CacheManager.Instance.AppLocations);
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            //Location_PK location = null;

            //if (ListType == ListType.List)
            //{
            //    location = Support.LocationManager.Instance.GetLocationByName("Task", Support.CacheManager.Instance.AppLocations);
            //}
            //else if (ListType == ListType.Search)
            //{
            //    location = Support.LocationManager.Instance.GetLocationByName("TaskSearch", Support.CacheManager.Instance.AppLocations);
            //}

            //if (location != null)
            //{
            //    var topLevelParent = MasterPage.FindTopLevelParent(location);

            //    MasterPage.CurrentLocation = location;
            //    MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            //}
        }

        private DataTable PrepareDataForExport(DataTable taskDataTable)
        {
            if (taskDataTable == null || taskDataTable.Rows.Count == 0) return taskDataTable;

            return taskDataTable;
        }

        public string HandleMissing(object value)
        {
            if (value != null && value.ToString().Trim() != "")
            {
                return value.ToString();
            }

            return "Missing";
        }

        private void SetGridStatusColor(HtmlGenericControl pnlStatusColor, string internalStatus)
        {
            switch (internalStatus)
            {
                case "active":
                    pnlStatusColor.Attributes.Add("class", "statusGreen");
                    break;
                case "pending":
                    pnlStatusColor.Attributes.Add("class", "statusGold");
                    break;
                case "finished":
                    pnlStatusColor.Attributes.Add("class", "statusBlack");
                    break;
                default:
                    pnlStatusColor.Attributes.Add("class", "statusBlack");
                    break;
            }
        }

        private void SetGridRowColor(DateTime? expectedFinishedDate, string internalStatus, PossGrid.PossGridRowEventArgs e)
        {
            var controls = new[] { "hlID", "hlViewDocs", "hlNewDocument", "hlViewSub", "hlNewSubUnit", "hlViewActivity" };

            if (expectedFinishedDate.HasValue && expectedFinishedDate <= DateTime.Now && internalStatus != "finished")
            {
                foreach (var item in controls)
                {
                    var control = e.FindControl(item) as WebControl;
                    if (control != null)
                    {
                        control.ForeColor = ColorTranslator.FromHtml("#ff0000");
                    }
                }

                e.Row.CssClass = e.Row.DataItemIndex % 2 == 0 ? "dxgvDataRow_readyRed" : "dxgvDataRowAlt_readyRed";

                e.Row.ForeColor = Color.Red;
                isRed = true;
            }
            else
            {
                e.Row.ForeColor = Color.Black;
            }
        }

        private void BindProducts(Panel pnlProducts, int taskPk)
        {
            var task = _taskOperations.GetEntity(taskPk);
            if (task == null || task.activity_FK == null) return;

            var ds = _activityProductMnOperations.GetProductsByActivity(task.activity_FK);
            var dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
            if (dt == null || !dt.Columns.Contains("product_PK") || !dt.Columns.Contains("name")) return;

            foreach (DataRow dr in dt.Rows)
            {
                var hl = new HyperLink
                {
                    NavigateUrl = string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, dr["product_PK"]),
                    Text = dr["name"].ToString()
                };
                if (isRed) hl.ForeColor = ColorTranslator.FromHtml("#ff0000");
                pnlProducts.Controls.Add(hl);
                pnlProducts.Controls.Add(new LiteralControl("<br />"));
            }
        }

        private void HandleEntityContextActivity()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Activity:";

            var activity = _activityOperations.GetEntity(_idAct);

            lblPrvParentEntity.Text = activity != null && !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.ControlDefault.LbPrvText;
        }

        private void BindNewDocumentLink(HyperLink hlNewDocument)
        {
            BindGridNewLink(hlNewDocument);
        }

        private void BindNewSubmissionUnitLink(HyperLink hlNewSubUnit)
        {
            BindGridNewLink(hlNewSubUnit);
        }

        private void BindGridNewLink(HyperLink hlNewLink)
        {
            switch (ListType)
            {
                case ListType.List:
                    if (EntityContext == EntityContext.Task) hlNewLink.NavigateUrl += "&From=Task";
                    else if (EntityContext == EntityContext.Activity && _idAct.HasValue) hlNewLink.NavigateUrl += string.Format("&From=ActTaskList&idAct={0}", _idAct);
                    else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) hlNewLink.NavigateUrl += string.Format("&From=ActMyTaskList&idAct={0}", _idAct);
                    break;

                case ListType.Search:
                    hlNewLink.NavigateUrl += "&From=TaskSearch";
                    break;
            }
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedInsertTask = SecurityHelper.IsPermitted(Permission.InsertTask);

            if (isPermittedInsertTask)
            {
                MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }
            else
            {
                MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            }

            return true;
        }

        #endregion
    }
}