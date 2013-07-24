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

namespace AspNetUI.Views.ProjectView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private bool isRed;
        private const int NumLayoutToKeep = 5;
        private int? _idSearch;
        private string _gridId;

        private IProject_PKOperations _projectOperations;
        private IActivity_PKOperations _activityOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IUSEROperations _userOperations;
        private IType_PKOperations _typeOperations;
        private ICountry_PKOperations _countryOperations;
        private IPerson_PKOperations _personOperations;
        private IProject_saved_search_PKOperations _savedProjectSearch;

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

            _idSearch = ValidationHelper.IsValidInt(Request.QueryString["idSearch"]) ? int.Parse(Request.QueryString["idSearch"]) : (int?)null;

            _projectOperations = new Project_PKDAL();
            _activityOperations = new Activity_PKDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();
            _typeOperations = new Type_PKDAL();
            _countryOperations = new Country_PKDAL();
            _personOperations = new Person_PKDAL();
            _savedProjectSearch = new Project_saved_search_PKDAL();

            if (ListType == ListType.Search)
            {
                ProjectGrid.GridVersion = ProjectGrid.GridVersion + ListType.ToString();
            }
            else if (EntityContext != EntityContext.Default)
            {
                ProjectGrid.GridVersion = ProjectGrid.GridVersion + EntityContext.ToString();
            }

            _gridId = ProjectGrid.GridId + "_" + ProjectGrid.GridVersion;
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

                    QuickLinksPopup.OnOkButtonClick += QuickLinksPopup_OnOkButtonClick;

                    btnExportLower.Click += btnExport_Click;
                    subtabs.Controls.Clear();
                    break;
            }

            ProjectGrid.OnRebindRequired += ProjectGrid_OnRebindRequired;
            ProjectGrid.OnHtmlRowPrepared += ProjectGrid_OnHtmlRowPrepared;
            ProjectGrid.OnHtmlCellPrepared += ProjectGrid_OnHtmlCellPrepared;
            ProjectGrid.OnExcelCellPrepared += ProjectGrid_OnExcelCellPrepared;
            ProjectGrid.OnLoadClientLayout += ProjectGrid_OnLoadClientLayout;
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

        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    HideSearch();

                    if (Request.QueryString["idLay"] == "default")
                    {
                        ProjectGrid.ClearFilters();
                    }
                    break;
                case ListType.Search:
                    if (!IsPostBack)
                    {
                        var clear = Request.QueryString["Clear"];

                        if (_idSearch.HasValue && string.IsNullOrWhiteSpace(clear))
                        {
                            ShowAll();
                            btnExportLower.Visible = true;
                            btnDeleteSearch.Visible = true;
                        }
                        else
                        {
                            ShowSearch();
                            btnExportLower.Visible = false;
                            btnDeleteSearch.Visible = clear == "true";
                        }
                    }

                    btnSaveLayout.Visible = false;
                    btnClearLayout.Visible = false;
                    btnExport.Visible = false;
                    btnColumns.Visible = false;
                    btnReset.Visible = false;

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
                ProjectGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }

            if (!_idSearch.HasValue || Request.QueryString["Clear"] == "true") return;

            var savedProjectSearch = _savedProjectSearch.GetEntity(_idSearch);

            if (savedProjectSearch == null) return;

            if (ListType == ListType.Search)
            {
                txtProjectName.Text = savedProjectSearch.name;
                ddlResponsibleUser.SelectedId = savedProjectSearch.user_FK;
                ddlInternalStatus.SelectedId = savedProjectSearch.internal_status_type_FK;
                ddlCountry.SelectedId = savedProjectSearch.country_FK;
                dtRngStartDate.TextFrom = savedProjectSearch.start_date_from.HasValue ? savedProjectSearch.start_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngStartDate.TextTo = savedProjectSearch.start_date_to.HasValue ? savedProjectSearch.start_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngExpectedFinishedDate.TextFrom = savedProjectSearch.expected_finished_date_from.HasValue ? savedProjectSearch.expected_finished_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngExpectedFinishedDate.TextTo = savedProjectSearch.expected_finished_dat_to.HasValue ? savedProjectSearch.expected_finished_dat_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngActualFinishedDate.TextFrom = savedProjectSearch.actual_finished_date_from.HasValue ? savedProjectSearch.actual_finished_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngActualFinishedDate.TextTo = savedProjectSearch.actual_finished_date_to.HasValue ? savedProjectSearch.actual_finished_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
            }

            ProjectGrid.SetClientLayout(savedProjectSearch.gridLayout);
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (ProjectGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ProjectGrid.SecondSortingColumn, ProjectGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ProjectGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ProjectGrid.MainSortingColumn, ProjectGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("project_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _projectOperations.GetListFormSearchDataSet(filters, ProjectGrid.CurrentPage, ProjectGrid.PageSize, gobList, out itemCount);
                else ds = _projectOperations.GetListFormDataSet(filters, ProjectGrid.CurrentPage, ProjectGrid.PageSize, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _projectOperations.GetListFormSearchDataSet(filters, ProjectGrid.CurrentPage, ProjectGrid.PageSize, gobList, out itemCount);
            }

            ProjectGrid.TotalRecords = itemCount;

            if (ProjectGrid.CurrentPage > ProjectGrid.TotalPages || (ProjectGrid.CurrentPage == 0 && ProjectGrid.TotalPages > 0))
            {
                if (ProjectGrid.CurrentPage > ProjectGrid.TotalPages) ProjectGrid.CurrentPage = ProjectGrid.TotalPages; else ProjectGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    if (_idSearch.HasValue) ds = _projectOperations.GetListFormSearchDataSet(filters, ProjectGrid.CurrentPage, ProjectGrid.PageSize, gobList, out itemCount);
                    else ds = _projectOperations.GetListFormDataSet(filters, ProjectGrid.CurrentPage, ProjectGrid.PageSize, gobList, out itemCount);
                }
                else if (ListType == ListType.Search)
                {
                    ds = _projectOperations.GetListFormSearchDataSet(filters, ProjectGrid.CurrentPage, ProjectGrid.PageSize, gobList, out itemCount);
                }
            }

            ProjectGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            ProjectGrid.DataBind();

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
                        if (EntityContext == EntityContext.Project) Response.Redirect(string.Format("~/Views/ProjectView/Form.aspx?EntityContext={0}&Action=New&From=Proj", EntityContext.Default));
                        Response.Redirect(string.Format("~/Views/ProjectView/Form.aspx?EntityContext={0}&Action=New&From=Proj", EntityContext.Default));
                    }
                    break;
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in ProjectGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("project_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (ProjectGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            ProjectGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            ProjectGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);

            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = ProjectGrid.GetClientLayoutString(),
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

            var gobList = new List<GEMOrderBy>();
            if (ProjectGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ProjectGrid.SecondSortingColumn, ProjectGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ProjectGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ProjectGrid.MainSortingColumn, ProjectGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("project_PK", GEMOrderByType.DESC));

            int itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _projectOperations.GetListFormSearchDataSet(filters, ProjectGrid.CurrentPage, ProjectGrid.PageSize, gobList, out itemCount); // Quick link
                else ds = _projectOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _projectOperations.GetListFormSearchDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            ProjectGrid["project_PK"].Visible = true;
            if (ds != null) ProjectGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            ProjectGrid["project_PK"].Visible = false;

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            ProjectGrid.ResetVisibleColumns();
            ProjectGrid.SecondSortingColumn = null;
            ProjectGrid.MainSortingOrder = PossGrid.SortOrder.DESC;
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
            Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}&Action=Search&Clear=true{1}", EntityContext.Project, (_idSearch.HasValue ? "&idSearch=" + _idSearch : string.Empty)));
        }

        #endregion

        #region Grid

        void ProjectGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {
            if (args.RowType != DataControlRowType.DataRow) return;

            if (args.FieldName == "ProjectName")
            {
                string appUrl = ConfigurationManager.AppSettings["ApplicationURL"];
                appUrl = appUrl.StartsWith("http://") ? appUrl : "http://" + appUrl;
                args.Cell.Url = appUrl + "/Views/Business/ProjectProperties.aspx?f=l&projid=" + Convert.ToString(args.Row["project_PK"]);
                args.Cell.FontUnderline = true;
                args.Cell.Text = HandleMissing(args.Cell.Text);
                args.Cell.FontColor = Color.Blue;
            }
        }

        void ProjectGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            string status = e.GetValue("InternalStatus").ToString();
            status = !string.IsNullOrWhiteSpace(status) ? status.Trim() : string.Empty;

            isRed = false;

            DateTime? expectedDate = ValidationHelper.IsValidDateTime(Convert.ToString(e.GetValue("expected_finished_date"))) ? Convert.ToDateTime(e.GetValue("expected_finished_date")) : (DateTime?)null;
            if (expectedDate.HasValue && expectedDate <= DateTime.Now && status != "Finished")
            {
                foreach (var item in new[] { "hlID", "hlPreviewDoc", "hlNewDocument", "hlPreviewAct", "hlNewActivity" })
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
                isRed = false;
            }

            var pnlStatusColor = e.FindControl("pnlStatusColor") as HtmlGenericControl;

            if (pnlStatusColor != null)
            {
                if (status == "Active")
                {
                    pnlStatusColor.Attributes.Add("class", "statusGreen");
                }
                else if (status == "Pending")
                {
                    pnlStatusColor.Attributes.Add("class", "statusGold");
                }
                else if (status == "Finished")
                {
                    pnlStatusColor.Attributes.Add("class", "statusBlack");
                }
            }

            var pnlActivities = e.FindControl("pnlActivities") as Panel;

            if (pnlActivities != null)
            {
                List<Activity_PK> activitiesOnProjects = _activityOperations.GetActivityFromProject(Convert.ToInt32(e.GetValue("project_PK")));
                activitiesOnProjects.Sort((a1, a2) => a1.name.CompareTo(a2.name));

                if (activitiesOnProjects.Count > 0)
                {
                    foreach (var item in activitiesOnProjects)
                    {
                        var hl = new HyperLink();
                        hl.NavigateUrl = string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext.Activity, item.activity_PK);
                        hl.Text = item.name;
                        if (isRed) hl.ForeColor = ColorTranslator.FromHtml("#ff0000");
                        pnlActivities.Controls.Add(hl);
                        pnlActivities.Controls.Add(new LiteralControl("<br />"));
                    }
                }
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

            var hlNewActivity = e.FindControl("hlNewActivity") as HyperLink;
            if (hlNewActivity != null)
            {
                if (SecurityHelper.IsPermitted(Permission.InsertActivity)) BindNewActivityLink(hlNewActivity);
                else hlNewActivity.NavigateUrl = string.Empty;
            }
        }

        void ProjectGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void ProjectGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!ProjectGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = ProjectGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (ProjectGrid.SortCount > 1 && e.FieldName == ProjectGrid.MainSortingColumn)
                return;

            if (e.FieldName == "ProjectName" || e.FieldName == "Countries" || e.FieldName == "Activities" || e.FieldName == "InternalStatus")
            {
                e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        void ProjectGrid_OnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                args.ClientLayoutString = userGridSettings.grid_layout;
            }
        }

        #endregion

        #region Quick links

        public void btnSaveSearchClick(object sender, EventArgs e)
        {
            QuickLink quickLink = null;
            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                var savedSearch = _savedProjectSearch.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
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

            _savedProjectSearch.Delete(Convert.ToInt32(Request.QueryString["idSearch"]));
            Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}&Action=Search", EntityContext.Project));
        }

        protected void QuickLinksPopup_OnOkButtonClick(object sender, FormEventArgs<QuickLink> e)
        {
            Project_saved_search_PK savedProjectSearch = null;

            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                savedProjectSearch = _savedProjectSearch.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
            }

            if (savedProjectSearch == null)
            {
                savedProjectSearch = new Project_saved_search_PK();
            }

            var nextStartDateFrom = ValidationHelper.IsValidDateTime(dtRngStartDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngStartDate.TextFrom, CultureInfoHr) : null;
            var nextStartDateTo = ValidationHelper.IsValidDateTime(dtRngStartDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngStartDate.TextTo, CultureInfoHr) : null;
            var nextExpectedFinishedDateFrom = ValidationHelper.IsValidDateTime(dtRngExpectedFinishedDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngExpectedFinishedDate.TextFrom, CultureInfoHr) : null;
            var nextExpectedFinishedDateTo = ValidationHelper.IsValidDateTime(dtRngExpectedFinishedDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngExpectedFinishedDate.TextTo, CultureInfoHr) : null;
            var nextActualFinishedDateFrom = ValidationHelper.IsValidDateTime(dtRngActualFinishedDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngActualFinishedDate.TextFrom, CultureInfoHr) : null;
            var nextActualFinishedDateTo = ValidationHelper.IsValidDateTime(dtRngActualFinishedDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngActualFinishedDate.TextTo, CultureInfoHr) : null;

            savedProjectSearch.name = txtProjectName.Text;
            savedProjectSearch.user_FK = ddlResponsibleUser.SelectedId;
            savedProjectSearch.internal_status_type_FK = ddlInternalStatus.SelectedId;
            savedProjectSearch.country_FK = ddlCountry.SelectedId;
            savedProjectSearch.start_date_from = nextStartDateFrom;
            savedProjectSearch.start_date_to = nextStartDateTo;
            savedProjectSearch.expected_finished_date_from = nextExpectedFinishedDateFrom;
            savedProjectSearch.expected_finished_dat_to = nextExpectedFinishedDateTo;
            savedProjectSearch.actual_finished_date_from = nextActualFinishedDateFrom;
            savedProjectSearch.actual_finished_date_to = nextActualFinishedDateTo;
            savedProjectSearch.gridLayout = ProjectGrid.GetClientLayoutString();

            var quickLink = e.Data;
            if (quickLink != null)
            {
                savedProjectSearch.displayName = quickLink.Name;
                savedProjectSearch.isPublic = quickLink.IsPublic.HasValue && quickLink.IsPublic.Value;
            }

            var user = _userOperations.GetUserByUsername(SessionManager.Instance.CurrentUser.Username);
            if (user != null)
            {
                savedProjectSearch.user_FK1 = user.Person_FK;
            }

            savedProjectSearch = _savedProjectSearch.Save(savedProjectSearch);
            Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}&Action=Search&idSearch={1}", EntityContext.Project, savedProjectSearch.project_saved_search_PK));
        }

        #endregion

        #endregion

        #region Support methods

        private void ClearSearch()
        {
            txtProjectName.Text = string.Empty;
            ddlResponsibleUser.SelectedValue = string.Empty;
            ddlInternalStatus.SelectedValue = string.Empty;
            ddlCountry.SelectedValue = string.Empty;
            dtRngStartDate.Clear();
            dtRngExpectedFinishedDate.Clear();
            dtRngActualFinishedDate.Clear();
        }

        private Dictionary<string, string> GetFilters()
        {
            var filters = ProjectGrid.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (_idSearch.HasValue)
                    {
                        Project_saved_search_PK savedProjectSearch = _savedProjectSearch.GetEntity(_idSearch);
                        FillFilters(savedProjectSearch, filters);
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
                            filters["QueryBy"] = "ProjectMyAlerts";
                        }
                        else
                        {
                            filters.Add("QueryBy", "ProjectMyAlerts");
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
            if (!string.IsNullOrWhiteSpace(txtProjectName.Text)) filters.Add("SearchProjectName", txtProjectName.Text);
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

        private void FillFilters(Project_saved_search_PK savedProjectSearch, Dictionary<string, string> filters)
        {
            if (!string.IsNullOrWhiteSpace(savedProjectSearch.name)) filters.Add("SearchProjectName", savedProjectSearch.name);
            if (savedProjectSearch.user_FK.HasValue) filters.Add("SearchResponsibleUserPk", savedProjectSearch.user_FK.Value.ToString());
            if (savedProjectSearch.internal_status_type_FK.HasValue) filters.Add("SearchInternalStatusPk", savedProjectSearch.internal_status_type_FK.Value.ToString());
            if (savedProjectSearch.country_FK.HasValue) filters.Add("SearchCountryPk", savedProjectSearch.country_FK.Value.ToString());
            if (savedProjectSearch.start_date_from.HasValue) filters.Add("SearchStartDateFrom", savedProjectSearch.start_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedProjectSearch.start_date_to.HasValue) filters.Add("SearchStartDateTo", savedProjectSearch.start_date_to.Value.ToString(Constant.DateTimeFormat));
            if (savedProjectSearch.expected_finished_date_from.HasValue) filters.Add("SearchExpectedFinishedDateFrom", savedProjectSearch.expected_finished_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedProjectSearch.expected_finished_dat_to.HasValue) filters.Add("SearchExpectedFinishedDateTo", savedProjectSearch.expected_finished_dat_to.Value.ToString(Constant.DateTimeFormat));
            if (savedProjectSearch.actual_finished_date_from.HasValue) filters.Add("SearchActualFinishedDateFrom", savedProjectSearch.actual_finished_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedProjectSearch.actual_finished_date_to.HasValue) filters.Add("SearchActualFinishedDateTo", savedProjectSearch.actual_finished_date_to.Value.ToString(Constant.DateTimeFormat));
        }

        private void GenerateContextMenuItems()
        {
            var contextMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                contextMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") };

                MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);
            }
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location;

            if (ListType == ListType.List)
            {
                if (EntityContext == EntityContext.Project)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("Proj", Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                    if (location != null)
                    {
                        MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                        MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    }
                }
            }
            else if (ListType == ListType.Search)
            {
                location = Support.LocationManager.Instance.GetLocationByName("ProjListSearch", Support.CacheManager.Instance.AppLocations);
                if (location != null)
                {
                    MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                    MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
        }

        private void GenerateTopMenuItems()
        {
            //Location_PK location = null;

            //if (ListType == ListType.List)
            //{
            //    location = Support.LocationManager.Instance.GetLocationByName("Proj", Support.CacheManager.Instance.AppLocations);
            //}
            //else if (ListType == ListType.Search)
            //{
            //    location = Support.LocationManager.Instance.GetLocationByName("ProjListSearch", Support.CacheManager.Instance.AppLocations);
            //}

            //if (location != null)
            //{
            //    var topLevelParent = MasterPage.FindTopLevelParent(location);

            //    MasterPage.CurrentLocation = location;
            //    MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            //}
        }

        private DataTable PrepareDataForExport(DataTable projectDataTable)
        {
            if (projectDataTable == null || projectDataTable.Rows.Count == 0) return projectDataTable;

            return projectDataTable;
        }

        public string HandleMissing(object value)
        {
            if (value != null && value.ToString().Trim() != "")
            {
                return value.ToString();
            }

            return "Missing";
        }

        private void BindNewActivityLink(HyperLink hlNewActivity)
        {
            BindGridNewLink(hlNewActivity);
        }

        private void BindNewDocumentLink(HyperLink hlNewDocument)
        {
            BindGridNewLink(hlNewDocument);
        }

        private void BindGridNewLink(HyperLink hlNewLink)
        {
            switch (ListType)
            {
                case ListType.List:
                    hlNewLink.NavigateUrl += "&From=Proj";
                    break;
                case ListType.Search:
                    hlNewLink.NavigateUrl += "&From=ProjSearch";
                    break;
            }
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedInsertProject = SecurityHelper.IsPermitted(Permission.InsertProject);

            if (isPermittedInsertProject)
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