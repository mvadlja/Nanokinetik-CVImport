using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUIFramework;
using GEM2Common;
using PossGrid;
using Ready.Model;

namespace AspNetUI.Views.TimeUnitView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;
        private int? _idAct;
        private int? _idSearch;
        private string _gridId;

        private ITime_unit_PKOperations _timeUnitOperations;
        private ITime_unit_name_PKOperations _timeUnitNameOperations;
        private IActivity_PKOperations _activityOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IUSEROperations _userOperations;
        private IPerson_PKOperations _personOperations;
        private ITime_unit_saved_search_PKOperations _timeUnitSavedSearchOperations;

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

            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;
            _idSearch = ValidationHelper.IsValidInt(Request.QueryString["idSearch"]) ? int.Parse(Request.QueryString["idSearch"]) : (int?)null;

            _timeUnitOperations = new Time_unit_PKDAL();
            _timeUnitNameOperations = new Time_unit_name_PKDAL();
            _activityOperations = new Activity_PKDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();
            _personOperations = new Person_PKDAL();
            _timeUnitSavedSearchOperations = new Time_unit_saved_search_PKDAL();

            if (ListType == ListType.Search)
            {
                TimeUnitGrid.GridVersion = TimeUnitGrid.GridVersion + ListType.ToString();
                TimeUnitGrid.AllowGrouping = false;
            }
            else if (EntityContext != EntityContext.Default)
            {
                TimeUnitGrid.GridVersion = TimeUnitGrid.GridVersion + EntityContext.ToString();
            }

            _gridId = TimeUnitGrid.GridId + "_" + TimeUnitGrid.GridVersion;

            if (!IsPostBack && HttpContext.Current.Session[TimeUnitGrid.ID + TimeUnitGrid.GridVersion + "_groupingSortOrder"] == null)
            {
                HttpContext.Current.Session[TimeUnitGrid.ID + TimeUnitGrid.GridVersion + "_groupingSortOrder"] = SortOrder.DESC.ToString();
            }
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

                    break;
            }

            TimeUnitGrid.OnRebindRequired += TimeUnitGridOnRebindRequired;
            TimeUnitGrid.OnHtmlRowPrepared += TimeUnitGridOnHtmlRowPrepared;
            TimeUnitGrid.OnHtmlCellPrepared += TimeUnitGridOnHtmlCellPrepared;
            TimeUnitGrid.OnExcelCellPrepared += TimeUnitGridOnExcelCellPrepared;
            TimeUnitGrid.OnLoadClientLayout += TimeUnitGridOnLoadClientLayout;
            TimeUnitGrid.OnHtmlGroupPrepared += TimeUnitGridOnOnHtmlGroupPrepared;
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
                    FillName();
                    FillDdlResponsibleUsers();
                    break;
            }
        }

        private void FillName()
        {
            var timeUnitNameList = _timeUnitNameOperations.GetEntities();
            ddlName.Fill(timeUnitNameList, "time_unit_name", "time_unit_name_PK");
            ddlName.SortItemsByText();
        }

        private void FillDdlResponsibleUsers()
        {
            var responsibleUserList = _personOperations.GetEntitiesByRoleName(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, "FullName", "person_PK");
            ddlResponsibleUser.SortItemsByText();
        }

        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    HandleListModeByActivity();
                    HideSearch();

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
                TimeUnitGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }

            if (!_idSearch.HasValue || Request.QueryString["Clear"] == "true") return;

            var savedTimeUnitSearch = _timeUnitSavedSearchOperations.GetEntity(_idSearch);

            if (savedTimeUnitSearch == null) return;

            if (ListType == ListType.Search)
            {
                BindActivity(savedTimeUnitSearch.activity_FK);
                ddlName.SelectedId = savedTimeUnitSearch.time_unit_FK;
                ddlResponsibleUser.SelectedId = savedTimeUnitSearch.user_FK;
                dtRngActualDate.TextFrom = savedTimeUnitSearch.actual_date_from.HasValue ? savedTimeUnitSearch.actual_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngActualDate.TextTo = savedTimeUnitSearch.actual_date_to.HasValue ? savedTimeUnitSearch.actual_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
            }

            TimeUnitGrid.SetClientLayout(savedTimeUnitSearch.gridLayout);
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();

            if (ListType == ListType.List && TimeUnitGrid.AllowGrouping)
            {
                gobList.Add(new GEMOrderBy(TimeUnitGrid.GroupingColumn, TimeUnitGrid.GroupingSortOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            }

            if (TimeUnitGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(TimeUnitGrid.SecondSortingColumn, TimeUnitGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (TimeUnitGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(TimeUnitGrid.MainSortingColumn, TimeUnitGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("time_unit_PK", GEMOrderByType.ASC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _timeUnitOperations.GetListFormSearchDataSet(filters, TimeUnitGrid.CurrentPage, TimeUnitGrid.PageSize, gobList, out itemCount);
                else ds = _timeUnitOperations.GetListFormDataSet(filters, TimeUnitGrid.CurrentPage, TimeUnitGrid.PageSize, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _timeUnitOperations.GetListFormSearchDataSet(filters, TimeUnitGrid.CurrentPage, TimeUnitGrid.PageSize, gobList, out itemCount);
            }

            TimeUnitGrid.TotalRecords = itemCount;

            if (TimeUnitGrid.CurrentPage > TimeUnitGrid.TotalPages || (TimeUnitGrid.CurrentPage == 0 && TimeUnitGrid.TotalPages > 0))
            {
                TimeUnitGrid.CurrentPage = TimeUnitGrid.CurrentPage > TimeUnitGrid.TotalPages ? TimeUnitGrid.TotalPages : 1;

                if (ListType == ListType.List)
                {
                    if (_idSearch.HasValue) ds = _timeUnitOperations.GetListFormSearchDataSet(filters, TimeUnitGrid.CurrentPage, TimeUnitGrid.PageSize, gobList, out itemCount);
                    else ds = _timeUnitOperations.GetListFormDataSet(filters, TimeUnitGrid.CurrentPage, TimeUnitGrid.PageSize, gobList, out itemCount);
                }
                else if (ListType == ListType.Search)
                {
                    ds = _timeUnitOperations.GetListFormSearchDataSet(filters, TimeUnitGrid.CurrentPage, TimeUnitGrid.PageSize, gobList, out itemCount);
                }
            }

            TimeUnitGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            TimeUnitGrid.DataBind();

            MasterPage.UpCommon.Update();
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
                        if (EntityContext == EntityContext.TimeUnit) Response.Redirect(string.Format("~/Views/TimeUnitView/Form.aspx?EntityContext={0}&Action=New&From=TimeUnit", EntityContext.Default));
                        else if (EntityContext == EntityContext.TimeUnitMy) Response.Redirect(string.Format("~/Views/TimeUnitView/Form.aspx?EntityContext={0}&Action=New&From=TimeUnitMy", EntityContext.Default));
                        else if (EntityContext == EntityContext.Activity && _idAct.HasValue) Response.Redirect(string.Format("~/Views/TimeUnitView/Form.aspx?EntityContext={0}&Action=New&idAct={1}&From=ActTimeUnitList", EntityContext, _idAct));
                        else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) Response.Redirect(string.Format("~/Views/TimeUnitView/Form.aspx?EntityContext={0}&Action=New&idAct={1}&From=ActMyTimeUnitList", EntityContext, _idAct));
                        Response.Redirect(string.Format("~/Views/TimeUnitView/Form.aspx?EntityContext={0}&Action=New&From=TimeUnit", EntityContext.Default));
                    }
                    break;

                case ContextMenuEventTypes.Back:
                    {
                        if (EntityContext == EntityContext.TimeUnit) Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext.TimeUnit));
                        else if (EntityContext == EntityContext.TimeUnitMy) Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext.TimeUnitMy));
                        else if (EntityContext == EntityContext.Activity) Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext.Activity));
                        else if (EntityContext == EntityContext.ActivityMy) Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext.ActivityMy));
                        Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext.TimeUnit));
                    }
                    break;
            }
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
            Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}&Action=Search&Clear=true{1}", EntityContext.TimeUnit, (_idSearch.HasValue ? "&idSearch=" + _idSearch : string.Empty)));
        }

        #endregion

        #region Activity searcher

        void ActivitySearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var activity = _activityOperations.GetEntity(e.Data);

            if (activity == null || activity.activity_PK == null) return;

            txtSrActivity.Text = activity.name;
            txtSrActivity.SelectedEntityId = activity.activity_PK;
        }

        #endregion

        #region Grid

        void TimeUnitGridOnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {
            if (args.RowType != DataControlRowType.DataRow) return;
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in TimeUnitGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("time_unit_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (TimeUnitGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            TimeUnitGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();

            //MasterPage.UpCommon.Update();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            TimeUnitGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);

            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = TimeUnitGrid.GetClientLayoutString(),
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
            if (TimeUnitGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(TimeUnitGrid.SecondSortingColumn, TimeUnitGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (TimeUnitGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(TimeUnitGrid.MainSortingColumn, TimeUnitGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("time_unit_PK", GEMOrderByType.ASC));

            int itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                ds = _timeUnitOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _timeUnitOperations.GetListFormSearchDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            TimeUnitGrid["time_unit_PK"].Visible = true;
            if (ds != null) TimeUnitGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            TimeUnitGrid["time_unit_PK"].Visible = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            TimeUnitGrid.ResetVisibleColumns();
            TimeUnitGrid.SecondSortingColumn = null;
            TimeUnitGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
            TimeUnitGrid.GroupingSortOrder = PossGrid.SortOrder.DESC;
            BindGrid();
        }

        void TimeUnitGridOnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            var pnlProducts = e.FindControl("pnlProducts") as Panel;

            if (pnlProducts != null)
            {
                var products = Convert.ToString(e.GetValue("Products"));
                var productList = !string.IsNullOrWhiteSpace(products) ? products.Split(new[] { " ||| " }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

                if (productList.Length > 0)
                {
                    foreach (var product in productList)
                    {
                        var productProperties = product.Split(new[] { " || " }, StringSplitOptions.RemoveEmptyEntries);
                        if (productProperties.Length < 2) continue;

                        var hl = new HyperLink
                        {
                            NavigateUrl = string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, productProperties[1]),
                            Text = productProperties[0]
                        };

                        pnlProducts.Controls.Add(hl);
                        pnlProducts.Controls.Add(new LiteralControl("<br />"));
                    }
                }
            }

            var hlId = e.FindControl("hlId") as HyperLink;
            if (hlId != null)
            {
                BindIdLink(hlId);

                if (Request.QueryString["idLay"] != null)
                {
                    hlId.NavigateUrl += string.Format("&idLay={0}", Request.QueryString["idLay"]);
                }
            }

            var hlActivity = e.FindControl("hlActivity") as HyperLink;
            if (hlActivity != null)
            {
                BindActivityLink(hlActivity);
            }
        }

        void TimeUnitGridOnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void TimeUnitGridOnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!TimeUnitGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = TimeUnitGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (TimeUnitGrid.SortCount > 1 && e.FieldName == TimeUnitGrid.MainSortingColumn)
                return;

            if (e.FieldName != "actual_date" && e.FieldName != "Time")
            {
                e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        void TimeUnitGridOnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {
            var userGridSettings = MasterPage != null ? _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, MasterPage.CurrentLocation.display_name) : null;
            if (userGridSettings != null && !string.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                args.ClientLayoutString = userGridSettings.grid_layout;
            }
        }

        private void TimeUnitGridOnOnHtmlGroupPrepared(object sender, PossGridGroupEventArgs args)
        {
            args.GroupTitle = "Actual date";
            args.GroupValue = !(args.GroupObjectValue is System.DBNull) && args.GroupObjectValue != null ? ((DateTime)args.GroupObjectValue).ToString(Constant.DateTimeFormat) : "N/A";
            args.CalculatedValueLabel = "Total time";

            int hours = 0;
            int mins = 0;

            if (args.GroupOrder == GroupOrder.Middle ||
                (args.GroupOrder == GroupOrder.First && TimeUnitGrid.PageIndex == 1) ||
                (args.GroupOrder == GroupOrder.Last && TimeUnitGrid.PageIndex == TimeUnitGrid.PageCount))
            {
                foreach (var dataRowView in args.DataRows)
                {
                    string time = Convert.ToString(dataRowView["time"]);

                    var timeMatch = Regex.Match(time, @"(?<hours>\d{2}):(?<mins>\d{2})");

                    if (timeMatch.Success)
                    {
                        hours += int.Parse(timeMatch.Groups["hours"].Value);
                        mins += int.Parse(timeMatch.Groups["mins"].Value);
                    }
                }
            }
            else
            {
                DateTime? actualDate = !(args.GroupObjectValue is DBNull) && args.GroupObjectValue != null ? ((DateTime)args.GroupObjectValue) : (DateTime?)null;

                var filters = GetFilters();

                var gobList = new List<GEMOrderBy>();

                if (ListType == ListType.List && TimeUnitGrid.AllowGrouping)
                {
                    gobList.Add(new GEMOrderBy(TimeUnitGrid.GroupingColumn, TimeUnitGrid.GroupingSortOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
                }

                if (TimeUnitGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(TimeUnitGrid.SecondSortingColumn, TimeUnitGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

                if (TimeUnitGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(TimeUnitGrid.MainSortingColumn, TimeUnitGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
                if (gobList.Count == 0) gobList.Add(new GEMOrderBy("time_unit_PK", GEMOrderByType.ASC));

                var groupDataSet = _timeUnitOperations.GetListFormGroupDataSet(actualDate, filters, TimeUnitGrid.CurrentPage, TimeUnitGrid.PageSize, gobList);
                DataTable groupDataTable = null;
                if (groupDataSet != null && groupDataSet.Tables.Count > 0 && groupDataSet.Tables[0].Rows.Count == 1 &&
                    groupDataSet.Tables[0].Columns.Contains("Hours") &&
                    groupDataSet.Tables[0].Columns.Contains("Mins") &&
                    groupDataSet.Tables[0].Columns.Contains("Continued"))
                {
                    groupDataTable = groupDataSet.Tables[0];
                }

                if (groupDataTable != null)
                {
                    int tmpHours;
                    if (int.TryParse(Convert.ToString(groupDataTable.Rows[0]["Hours"]), NumberStyles.None, CultureInfoHr, out tmpHours)) hours = tmpHours;
                    int tmpMins;
                    if (int.TryParse(Convert.ToString(groupDataTable.Rows[0]["Mins"]), NumberStyles.None, CultureInfoHr, out tmpMins)) mins = tmpMins;

                    string continued = Convert.ToString(groupDataTable.Rows[0]["Continued"]);

                    if (continued == "Previous")
                    {
                        args.GroupOrder = GroupOrder.ContinuedFromPreviousPage;
                    }
                    else if (continued == "Next")
                    {
                        args.GroupOrder = GroupOrder.ContinuedOnNextPage;
                    }
                }
            }

            hours += mins / 60;
            mins = mins % 60;

            args.CalculatedValue = "Total time";
            args.CalculatedValue = string.Format("{0:00}:{1:00}", hours, mins);
        }

        #endregion

        #region Quick links

        public void btnSaveSearchClick(object sender, EventArgs e)
        {
            QuickLink quickLink = null;
            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                var savedSearch = _timeUnitSavedSearchOperations.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
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

            _timeUnitSavedSearchOperations.Delete(Convert.ToInt32(Request.QueryString["idSearch"]));
            Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}&Action=Search", EntityContext.TimeUnit));
        }

        protected void QuickLinksPopup_OnOkButtonClick(object sender, FormEventArgs<QuickLink> e)
        {
            Time_unit_saved_search_PK savedTimeUnitSearch = null;

            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                savedTimeUnitSearch = _timeUnitSavedSearchOperations.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
            }

            if (savedTimeUnitSearch == null)
            {
                savedTimeUnitSearch = new Time_unit_saved_search_PK();
            }

            var actualDateFrom = ValidationHelper.IsValidDateTime(dtRngActualDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngActualDate.TextFrom, CultureInfoHr) : null;
            var actualDateTo = ValidationHelper.IsValidDateTime(dtRngActualDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngActualDate.TextTo, CultureInfoHr) : null;

            savedTimeUnitSearch.time_unit_FK = ddlName.SelectedId;
            savedTimeUnitSearch.user_FK = ddlResponsibleUser.SelectedId;
            savedTimeUnitSearch.activity_FK = txtSrActivity.SelectedEntityId;
            savedTimeUnitSearch.actual_date_from = actualDateFrom;
            savedTimeUnitSearch.actual_date_to = actualDateTo;
            savedTimeUnitSearch.gridLayout = TimeUnitGrid.GetClientLayoutString();

            var quickLink = e.Data;
            if (quickLink != null)
            {
                savedTimeUnitSearch.displayName = quickLink.Name;
                savedTimeUnitSearch.isPublic = quickLink.IsPublic.HasValue && quickLink.IsPublic.Value;
            }

            var user = _userOperations.GetUserByUsername(SessionManager.Instance.CurrentUser.Username);
            if (user != null)
            {
                savedTimeUnitSearch.user_FK1 = user.Person_FK;
            }

            savedTimeUnitSearch = _timeUnitSavedSearchOperations.Save(savedTimeUnitSearch);
            Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}&Action=Search&idSearch={1}", EntityContext.TimeUnit, savedTimeUnitSearch.time_unit_saved_search_PK));
        }

        #endregion

        #endregion

        #region Support methods

        private void BindIdLink(HyperLink hlId)
        {
            BindTimeUnitEntityContextLink(hlId);
        }

        private void BindActivityLink(HyperLink hlActivity)
        {
            BindTimeUnitEntityContextLink(hlActivity);
        }

        private void BindTimeUnitEntityContextLink(HyperLink hlTimeUnitEntityContextLink)
        {
            if (EntityContext == EntityContext.TimeUnit || EntityContext == EntityContext.TimeUnitMy) hlTimeUnitEntityContextLink.NavigateUrl += string.Format("&EntityContext={0}", EntityContext);
            else hlTimeUnitEntityContextLink.NavigateUrl += string.Format("&EntityContext={0}", EntityContext.TimeUnit);
        }

        private void ClearSearch()
        {
            ddlName.SelectedValue = string.Empty;
            ddlResponsibleUser.SelectedValue = string.Empty;
            txtSrActivity.Clear();
            dtRngActualDate.Clear();
        }

        private Dictionary<string, string> GetFilters()
        {
            var filters = TimeUnitGrid.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (_idSearch.HasValue)
                    {
                        var savedTimeUnitSearch = _timeUnitSavedSearchOperations.GetEntity(_idSearch);
                        FillFilters(savedTimeUnitSearch, filters);
                    }

                    if ((EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) && _idAct.HasValue)
                    {
                        filters.Add("ActivityFk", _idAct.Value.ToString());
                    }

                    if (EntityContext == EntityContext.TimeUnitMy)
                    {
                        var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                        if (user != null && user.Person_FK.HasValue)
                        {
                            filters.Add("ResponsibleUserFk", user.Person_FK.Value.ToString());
                        }
                    }

                    if (Request.QueryString["idLay"] == "my" && !filters.ContainsKey("ResponsibleUserFk"))
                    {
                        var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                        if (user != null && user.Person_FK.HasValue)
                        {
                            filters.Add("ResponsibleUserFk", user.Person_FK.Value.ToString());
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
            if (ddlName.SelectedId.HasValue) filters.Add("SearchTimeUnitNamePk", ddlName.SelectedId.Value.ToString());
            if (ddlResponsibleUser.SelectedId.HasValue) filters.Add("SearchResponsibleUserPk", ddlResponsibleUser.SelectedId.Value.ToString());
            if (txtSrActivity.SelectedEntityId.HasValue) filters.Add("SearchActivityPk", txtSrActivity.SelectedEntityId.Value.ToString());
            if (ValidationHelper.IsValidDateTime(dtRngActualDate.TextFrom, CultureInfoHr)) filters.Add("SearchActualDateFrom", dtRngActualDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngActualDate.TextTo, CultureInfoHr)) filters.Add("SearchActualDateTo", dtRngActualDate.TextTo);
        }

        private void FillFilters(Time_unit_saved_search_PK savedTimeUnitSearch, Dictionary<string, string> filters)
        {
            if (savedTimeUnitSearch.time_unit_FK.HasValue) filters.Add("SearchTimeUnitNamePk", savedTimeUnitSearch.time_unit_FK.Value.ToString());
            if (savedTimeUnitSearch.user_FK.HasValue) filters.Add("SearchResponsibleUserPk", savedTimeUnitSearch.user_FK.Value.ToString());
            if (savedTimeUnitSearch.activity_FK.HasValue) filters.Add("SearchActivityPk", savedTimeUnitSearch.activity_FK.Value.ToString());
            if (savedTimeUnitSearch.actual_date_from.HasValue) filters.Add("SearchActualDateFrom", savedTimeUnitSearch.actual_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedTimeUnitSearch.actual_date_to.HasValue) filters.Add("SearchActualDateTo", savedTimeUnitSearch.actual_date_to.Value.ToString(Constant.DateTimeFormat));
        }

        private void GenerateContextMenuItems()
        {
            var contextMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                switch (EntityContext)
                {
                    case EntityContext.TimeUnit:
                    case EntityContext.TimeUnitMy:
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
            Location_PK location;

            if (ListType == ListType.List)
            {
                if (EntityContext == EntityContext.TimeUnit)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("TimeUnit", Support.CacheManager.Instance.AppLocations);
                    MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                    MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                }
                else if (EntityContext == EntityContext.TimeUnitMy)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("TimeUnitMy", Support.CacheManager.Instance.AppLocations);
                    MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                    MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                }
                else if (EntityContext == EntityContext.Activity)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("ActTimeUnitList", Support.CacheManager.Instance.AppLocations);
                    MasterPage.TabMenu.TabControls.Clear();
                    tabMenu.Visible = true;
                    tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                    tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
                else if (EntityContext == EntityContext.ActivityMy)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("ActMyTimeUnitList", Support.CacheManager.Instance.AppLocations);
                    MasterPage.TabMenu.TabControls.Clear();
                    tabMenu.Visible = true;
                    tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                    tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
            else if (ListType == ListType.Search)
            {
                location = Support.LocationManager.Instance.GetLocationByName("TimeUnitSearch", Support.CacheManager.Instance.AppLocations);
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            //Location_PK location = null;

            //if (ListType == ListType.List)
            //{
            //    location =  Support.LocationManager.Instance.GetLocationByName("TimeUnit", Support.CacheManager.Instance.AppLocations);
            //}
            //else if (ListType == ListType.Search)
            //{
            //    location =  Support.LocationManager.Instance.GetLocationByName("TimeUnitSearch", Support.CacheManager.Instance.AppLocations);
            //}

            //var topLevelParent = MasterPage.FindTopLevelParent(location);

            //MasterPage.CurrentLocation = location;
            //MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
        }

        private DataTable PrepareDataForExport(DataTable timeUnitDataTable)
        {
            if (timeUnitDataTable == null || timeUnitDataTable.Rows.Count == 0) return timeUnitDataTable;

            if (!timeUnitDataTable.Columns.Contains("Products")) return timeUnitDataTable;

            foreach (DataRow row in timeUnitDataTable.Rows)
            {
                var products = Convert.ToString(row["Products"]);
                row["Products"] = !string.IsNullOrWhiteSpace(products) ? Regex.Replace(Regex.Replace(products, @" \|\| [0-9]* \|\|\|", "", RegexOptions.None), @" \|\| [0-9]*", "", RegexOptions.None) : string.Empty;
            }

            return timeUnitDataTable;
        }

        public string HandleMissing(object value)
        {
            if (value != null && value.ToString().Trim() != "")
            {
                return value.ToString();
            }

            return "Missing";
        }

        private void HandleListModeByActivity()
        {
            if (EntityContext != EntityContext.Activity && EntityContext != EntityContext.ActivityMy) return;

            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Activity:";

            var activity = _activityOperations.GetEntity(_idAct);

            lblPrvParentEntity.Text = activity != null && !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.ControlDefault.LbPrvText;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedInsertTimeUnit = SecurityHelper.IsPermitted(Permission.InsertTimeUnit);

            if (isPermittedInsertTimeUnit)
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