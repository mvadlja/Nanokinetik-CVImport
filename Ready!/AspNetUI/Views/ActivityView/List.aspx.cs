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

namespace AspNetUI.Views.ActivityView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private bool isRed;
        private const int NumLayoutToKeep = 5;
        private int? _idSearch;
        private int? _idProd;
        private int? _idProj;
        string _idLay;
        private string _gridId;

        private IActivity_PKOperations _activityOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IUSEROperations _userOperations;
        private IType_PKOperations _typeOperations;
        private ICountry_PKOperations _countryOperations;
        private IPerson_PKOperations _personOperations;
        private IActivity_product_PKOperations _activityProductMnOperations;
        private IProduct_PKOperations _productOperations;
        private IProject_PKOperations _projectOperations;
        private IActivity_saved_search_PKOperations _savedActivitySearch;
        private IOrganization_PKOperations _organizationOperations;

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
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;
            _idProj = ValidationHelper.IsValidInt(Request.QueryString["idProj"]) ? int.Parse(Request.QueryString["idProj"]) : (int?)null;

            _activityOperations = new Activity_PKDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();
            _typeOperations = new Type_PKDAL();
            _countryOperations = new Country_PKDAL();
            _personOperations = new Person_PKDAL();
            _activityProductMnOperations = new Activity_product_PKDAL();
            _productOperations = new Product_PKDAL();
            _projectOperations = new Project_PKDAL();
            _savedActivitySearch = new Activity_saved_search_PKDAL();
            _organizationOperations = new Organization_PKDAL();

            if (ListType == ListType.Search)
            {
                ActivityGrid.GridVersion = ActivityGrid.GridVersion + ListType.ToString();
            }
            else if (EntityContext != EntityContext.Default)
            {
                ActivityGrid.GridVersion = ActivityGrid.GridVersion + EntityContext.ToString();
            }

            _gridId = ActivityGrid.GridId + "_" + ActivityGrid.GridVersion;
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
                    txtSrProduct.Searcher.OnListItemSelected += ProductSearcher_OnListItemSelected;
                    txtSrProject.Searcher.OnListItemSelected += ProjectSearcher_OnListItemSelected;
                    QuickLinksPopup.OnOkButtonClick += QuickLinksPopup_OnOkButtonClick;

                    btnExportLower.Click += btnExport_Click;
                    subtabs.Controls.Clear();
                    break;
            }

            ActivityGrid.OnRebindRequired += ActivityGridOnRebindRequired;
            ActivityGrid.OnHtmlRowPrepared += ActivityGridOnHtmlRowPrepared;
            ActivityGrid.OnHtmlCellPrepared += ActivityGridOnHtmlCellPrepared;
            ActivityGrid.OnExcelCellPrepared += ActivityGridOnExcelCellPrepared;
            ActivityGrid.OnLoadClientLayout += ActivityGridOnLoadClientLayout;
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
                    FillDdlResponsibleUser();
                    FillDdlProcedureType();
                    FillDdlTypes();
                    FillDdlRegulatoryStatus();
                    FillDdlInternalStatus();
                    FillDdlActivityMode();
                    FillDdlApplicant();
                    FillDdlCountry();
                    break;
            }
        }

        private void FillDdlResponsibleUser()
        {
            var responsibleUserList = _personOperations.GetEntitiesByRoleName(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, "FullName", "person_PK");
            ddlResponsibleUser.SortItemsByText();
        }

        private void FillDdlProcedureType()
        {
            var procedureTypeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.AuthorisationProcedure);
            ddlProcedureType.Fill(procedureTypeList, "name", "type_PK");
            ddlProcedureType.SortItemsByText();
        }

        private void FillDdlTypes()
        {
            var typeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.Types);
            ddlType.Fill(typeList, "name", "type_PK");
            ddlType.SortItemsByText();
        }

        private void FillDdlRegulatoryStatus()
        {
            var regulatoryStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.RegulatoryStatus);
            ddlRegulatoryStatus.Fill(regulatoryStatusList, "name", "type_PK");
            ddlRegulatoryStatus.SortItemsByText();
        }

        private void FillDdlInternalStatus()
        {
            var internalStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.InternalStatus);
            ddlInternalStatus.Fill(internalStatusList, "name", "type_PK");
            ddlInternalStatus.SortItemsByText();
        }

        private void FillDdlActivityMode()
        {
            var activityModeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.ActivityMode);
            ddlActivityMode.Fill(activityModeList, "name", "type_PK");
            ddlActivityMode.SortItemsByText();
        }

        private void FillDdlApplicant()
        {
            var applicantList = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.Applicant);
            ddlApplicant.Fill(applicantList, "name_org", "organization_PK");
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
            ActivityGrid.SetComboList("RegulatoryStatus", regulatoryStatusListItems);
        }

        private void FillComboInternalStatus()
        {
            var internalStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.InternalStatus);
            var internalStatusListItems = new List<ListItem> { new ListItem("", "") };
            internalStatusList.ForEach(item => internalStatusListItems.Add(new ListItem(item.name, item.name)));
            ActivityGrid.SetComboList("InternalStatus", internalStatusListItems);
        }

        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    if (EntityContext == EntityContext.Product) HandleEntityContextProduct();
                    else if (EntityContext == EntityContext.Project) HandleEntityContextProject();

                    HideSearch();

                    if (Request.QueryString["idLay"] == "default")
                    {
                        ActivityGrid.ClearFilters();
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
                ActivityGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }

            if (!_idSearch.HasValue || Request.QueryString["Clear"] == "true") return;

            var savedActivitySearch = _savedActivitySearch.GetEntity(_idSearch);

            if (savedActivitySearch == null) return;

            if (ListType == ListType.Search)
            {
                BindProduct(savedActivitySearch.product_FK);
                BindProject(savedActivitySearch.project_FK);
                txtActivityName.Text = savedActivitySearch.name;
                ddlResponsibleUser.SelectedId = savedActivitySearch.user_FK;
                txtProcedureNumber.Text = savedActivitySearch.procedure_number;
                ddlProcedureType.SelectedId = savedActivitySearch.procedure_type_FK;
                ddlType.SelectedId = savedActivitySearch.type_FK;
                ddlRegulatoryStatus.SelectedId = savedActivitySearch.regulatory_status_FK;
                ddlInternalStatus.SelectedId = savedActivitySearch.internal_status_FK;
                ddlActivityMode.SelectedId = savedActivitySearch.activity_mode_FK;
                ddlApplicant.SelectedId = savedActivitySearch.applicant_FK;
                ddlCountry.SelectedId = savedActivitySearch.country_FK;
                txtLegalBasis.Text = savedActivitySearch.legal;
                txtActivityId.Text = savedActivitySearch.activity_Id;
                if (savedActivitySearch.billable.HasValue) rbYnBillable.SelectedValue = savedActivitySearch.billable.Value;
                dtRngStartDate.TextFrom = savedActivitySearch.start_date_from.HasValue ? savedActivitySearch.start_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngStartDate.TextTo = savedActivitySearch.start_date_to.HasValue ? savedActivitySearch.start_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngExpectedFinishedDate.TextFrom = savedActivitySearch.expected_finished_date_from.HasValue ? savedActivitySearch.expected_finished_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngExpectedFinishedDate.TextTo = savedActivitySearch.expected_finished_date_to.HasValue ? savedActivitySearch.expected_finished_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngActualFinishedDate.TextFrom = savedActivitySearch.actual_finished_date_from.HasValue ? savedActivitySearch.actual_finished_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngActualFinishedDate.TextTo = savedActivitySearch.actual_finished_date_to.HasValue ? savedActivitySearch.actual_finished_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngSubmissionDate.TextFrom = savedActivitySearch.submission_date_from.HasValue ? savedActivitySearch.submission_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngSubmissionDate.TextTo = savedActivitySearch.submission_date_to.HasValue ? savedActivitySearch.submission_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngApprovalDate.TextFrom = savedActivitySearch.approval_date_from.HasValue ? savedActivitySearch.approval_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngApprovalDate.TextTo = savedActivitySearch.approval_date_to.HasValue ? savedActivitySearch.approval_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
            }

            ActivityGrid.SetClientLayout(savedActivitySearch.gridLayout);
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (ActivityGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ActivityGrid.SecondSortingColumn, ActivityGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ActivityGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ActivityGrid.MainSortingColumn, ActivityGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("activity_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _activityOperations.GetListFormSearchDataSet(filters, ActivityGrid.CurrentPage, ActivityGrid.PageSize, gobList, out itemCount);
                else ds = _activityOperations.GetListFormDataSet(filters, ActivityGrid.CurrentPage, ActivityGrid.PageSize, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _activityOperations.GetListFormSearchDataSet(filters, ActivityGrid.CurrentPage, ActivityGrid.PageSize, gobList, out itemCount);
            }

            ActivityGrid.TotalRecords = itemCount;

            if (ActivityGrid.CurrentPage > ActivityGrid.TotalPages || (ActivityGrid.CurrentPage == 0 && ActivityGrid.TotalPages > 0))
            {
                if (ActivityGrid.CurrentPage > ActivityGrid.TotalPages) ActivityGrid.CurrentPage = ActivityGrid.TotalPages; else ActivityGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    if (_idSearch.HasValue) ds = _activityOperations.GetListFormSearchDataSet(filters, ActivityGrid.CurrentPage, ActivityGrid.PageSize, gobList, out itemCount);
                    else ds = _activityOperations.GetListFormDataSet(filters, ActivityGrid.CurrentPage, ActivityGrid.PageSize, gobList, out itemCount);
                }
                else if (ListType == ListType.Search)
                {
                    ds = _activityOperations.GetListFormSearchDataSet(filters, ActivityGrid.CurrentPage, ActivityGrid.PageSize, gobList, out itemCount);
                }
            }

            ActivityGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            ActivityGrid.DataBind();

            MasterPage.UpCommon.Update();
        }

        private void BindDynamicControls(object o)
        {
            if (ListType == ListType.Search) subtabs.Controls.Clear();
        }

        private void BindGridDynamicControls()
        {
            FillComboRegulatoryStatus();
            FillComboInternalStatus();
        }

        private void BindProduct(int? productFk)
        {
            var product = _productOperations.GetEntity(productFk);
            if (product == null || product.product_PK == null) return;
            
            txtSrProduct.SelectedEntityId = product.product_PK;
            txtSrProduct.Text = product.GetNameFormatted();
        }

        private void BindProject(int? projectFk)
        {
            var project = _projectOperations.GetEntity(projectFk);
            if (project == null || project.project_PK == null) return;

            txtSrProject.SelectedEntityId = project.project_PK;
            txtSrProject.Text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.MissingValue;
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
                        if (EntityContext == EntityContext.Product && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=New&idProd={1}&From=ProdActList", EntityContext.Product, _idProd.Value));
                        else if (EntityContext == EntityContext.Project && _idProj.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=New&idProj={1}&From=ProjActList", EntityContext.Project, _idProj.Value));
                        else if (EntityContext == EntityContext.Activity) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=New&From=Act", EntityContext.Default));
                        else if (EntityContext == EntityContext.ActivityMy) Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=New&From=ActMy", EntityContext.Default));
                        Response.Redirect(string.Format("~/Views/ActivityView/Form.aspx?EntityContext={0}&Action=New", EntityContext.Default));
                        }
                    break;

                case ContextMenuEventTypes.Back:
                    {
                        if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Project) Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext));
                        Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext.Activity));
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
            Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}&Action=Search&Clear=true{1}", EntityContext.Activity, (_idSearch.HasValue ? "&idSearch=" + _idSearch : string.Empty)));
        }

        #endregion

        #region Grid

        void ActivityGridOnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {
            if (args.RowType != DataControlRowType.DataRow) return;

            if (args.FieldName == "ActivityName")
            {
                string appUrl = ConfigurationManager.AppSettings["ApplicationURL"];
                appUrl = appUrl.StartsWith("http://") ? appUrl : "http://" + appUrl;
                args.Cell.Url = appUrl + "/Views/Business/APropertiesView.aspx?f=l&idAct=" + Convert.ToString(args.Row["activity_PK"]);
                args.Cell.FontUnderline = true;
                args.Cell.Text = HandleMissing(args.Cell.Text);
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in ActivityGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("activity_PK", "ExpectedFinishedDate"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (ActivityGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            ActivityGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            ActivityGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            
            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = ActivityGrid.GetClientLayoutString(),
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
            if (ActivityGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ActivityGrid.SecondSortingColumn, ActivityGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ActivityGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ActivityGrid.MainSortingColumn, ActivityGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("activity_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _activityOperations.GetListFormSearchDataSet(filters, 1, int.MaxValue, gobList, out itemCount); // Quick link
                else ds = _activityOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _activityOperations.GetListFormSearchDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            ActivityGrid["activity_PK"].Visible = true;
            if (ds != null) ActivityGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            ActivityGrid["activity_PK"].Visible = false;

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            ActivityGrid.ResetVisibleColumns();
            ActivityGrid.SecondSortingColumn = null;
            ActivityGrid.MainSortingOrder = PossGrid.SortOrder.DESC;
            BindGrid();
        }

        void ActivityGridOnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var activityPk = ValidationHelper.IsValidInt(Convert.ToString(e.GetValue("activity_PK"))) ? (int?)Convert.ToInt32(e.GetValue("activity_PK")) : null;

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
            if (pnlProducts != null && activityPk.HasValue)
            {
                BindProducts(pnlProducts, activityPk.Value);
            }

            var hlNewDocument = e.FindControl("hlNewDocument") as HyperLink;
            if (hlNewDocument != null)
            {
                if (SecurityHelper.IsPermitted(Permission.View) && SecurityHelper.IsPermitted(Permission.InsertDocument)) BindNewDocumentLink(hlNewDocument);
                else hlNewDocument.NavigateUrl = string.Empty;
            }

            var hlNewTask = e.FindControl("hlNewTask") as HyperLink;
            if (hlNewTask != null)
            {
                if (SecurityHelper.IsPermitted(Permission.View) && SecurityHelper.IsPermitted(Permission.InsertTask)) BindNewTaskLink(hlNewTask);
                else hlNewTask.NavigateUrl = string.Empty;
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

            var hlNewTime = e.FindControl("hlNewTime") as HyperLink;
            if (hlNewTime != null)
            {
                if (SecurityHelper.IsPermitted(Permission.View) && SecurityHelper.IsPermitted(Permission.InsertTimeUnit)) BindNewTimeLink(hlNewTime);
                else hlNewTime.NavigateUrl = string.Empty;
            }

            var hlTaskCount = e.FindControl("hlTaskCount") as HyperLink;
            if (hlTaskCount != null)
            {
                BindTaskCountLink(hlTaskCount);
            }

            var hlTimeCount = e.FindControl("hlTimeCount") as HyperLink;
            if (hlTimeCount != null)
            {
                BindTimeUnitCountLink(hlTimeCount);
            }

            var hlDocumentsCount = e.FindControl("hlDocumentsCount") as HyperLink;
            if (hlDocumentsCount != null)
            {
                BindDocumentCountLink(hlDocumentsCount);
            }
        }

        void ActivityGridOnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void ActivityGridOnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!ActivityGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = ActivityGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (ActivityGrid.SortCount > 1 && e.FieldName == ActivityGrid.MainSortingColumn)
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

        void ActivityGridOnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {
            var userGridSettings = MasterPage != null ? _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, MasterPage.CurrentLocation.display_name) : null;
            if (userGridSettings != null && !string.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                args.ClientLayoutString = userGridSettings.grid_layout;
            }
        }

        #endregion

        #region Product searcher

        /// <summary>
        /// Handles product list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProductSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var product = _productOperations.GetEntity(e.Data);

            if (product == null || product.product_PK == null) return;

            txtSrProduct.Text = product.GetNameFormatted();
            txtSrProduct.SelectedEntityId = product.product_PK;
        }

        #endregion

        #region Project searcher

        /// <summary>
        /// Handles project list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ProjectSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var project = _projectOperations.GetEntity(e.Data);

            if (project == null || project.project_PK == null) return;

            txtSrProject.Text = project.name;
            txtSrProject.SelectedEntityId = project.project_PK;
        }

        #endregion

        #region Quick links

        public void btnSaveSearchClick(object sender, EventArgs e)
        {
            QuickLink quickLink = null;
            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                var savedSearch = _savedActivitySearch.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
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

            _savedActivitySearch.Delete(Convert.ToInt32(Request.QueryString["idSearch"]));
            Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}&Action=Search", EntityContext.Activity));
        }

        protected void QuickLinksPopup_OnOkButtonClick(object sender, FormEventArgs<QuickLink> e)
        {
            Activity_saved_search_PK savedActivitySearch = null;

            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                savedActivitySearch = _savedActivitySearch.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
            }

            if (savedActivitySearch == null)
            {
                savedActivitySearch = new Activity_saved_search_PK();
            }

            var nextStartDateFrom = ValidationHelper.IsValidDateTime(dtRngStartDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngStartDate.TextFrom, CultureInfoHr) : null;
            var nextStartDateTo = ValidationHelper.IsValidDateTime(dtRngStartDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngStartDate.TextTo, CultureInfoHr) : null;
            var nextExpectedFinishedDateFrom = ValidationHelper.IsValidDateTime(dtRngExpectedFinishedDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngExpectedFinishedDate.TextFrom, CultureInfoHr) : null;
            var nextExpectedFinishedDateTo = ValidationHelper.IsValidDateTime(dtRngExpectedFinishedDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngExpectedFinishedDate.TextTo, CultureInfoHr) : null;
            var nextActualFinishedDateFrom = ValidationHelper.IsValidDateTime(dtRngActualFinishedDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngActualFinishedDate.TextFrom, CultureInfoHr) : null;
            var nextActualFinishedDateTo = ValidationHelper.IsValidDateTime(dtRngActualFinishedDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngActualFinishedDate.TextTo, CultureInfoHr) : null;
            var submissionDateFrom = ValidationHelper.IsValidDateTime(dtRngSubmissionDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngSubmissionDate.TextFrom, CultureInfoHr) : null;
            var submissionDateTo = ValidationHelper.IsValidDateTime(dtRngSubmissionDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngSubmissionDate.TextTo, CultureInfoHr) : null;
            var approvalDateFrom = ValidationHelper.IsValidDateTime(dtRngApprovalDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngApprovalDate.TextFrom, CultureInfoHr) : null;
            var approvalDateTo = ValidationHelper.IsValidDateTime(dtRngApprovalDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngApprovalDate.TextTo, CultureInfoHr) : null;

            savedActivitySearch.product_FK = txtSrProduct.SelectedEntityId;
            savedActivitySearch.project_FK = txtSrProject.SelectedEntityId;
            savedActivitySearch.name = txtActivityName.Text;
            savedActivitySearch.user_FK = ddlResponsibleUser.SelectedId;
            savedActivitySearch.procedure_number = txtProcedureNumber.Text;
            savedActivitySearch.procedure_type_FK = ddlProcedureType.SelectedId;
            savedActivitySearch.type_FK = ddlType.SelectedId;
            savedActivitySearch.regulatory_status_FK = ddlRegulatoryStatus.SelectedId;
            savedActivitySearch.internal_status_FK = ddlInternalStatus.SelectedId;
            savedActivitySearch.activity_mode_FK = ddlActivityMode.SelectedId;
            savedActivitySearch.country_FK = ddlCountry.SelectedId;
            savedActivitySearch.applicant_FK = ddlApplicant.SelectedId;
            savedActivitySearch.legal = txtLegalBasis.Text;
            savedActivitySearch.activity_Id = txtActivityId.Text;
            savedActivitySearch.billable = rbYnBillable.SelectedValue;
            savedActivitySearch.start_date_from = nextStartDateFrom;
            savedActivitySearch.start_date_to = nextStartDateTo;
            savedActivitySearch.expected_finished_date_from = nextExpectedFinishedDateFrom;
            savedActivitySearch.expected_finished_date_to = nextExpectedFinishedDateTo;
            savedActivitySearch.actual_finished_date_from = nextActualFinishedDateFrom;
            savedActivitySearch.actual_finished_date_to = nextActualFinishedDateTo;
            savedActivitySearch.submission_date_from = submissionDateFrom;
            savedActivitySearch.submission_date_to = submissionDateTo;
            savedActivitySearch.approval_date_from = approvalDateFrom;
            savedActivitySearch.approval_date_to = approvalDateTo;
            savedActivitySearch.gridLayout = ActivityGrid.GetClientLayoutString();

            var quickLink = e.Data;
            if (quickLink != null)
            {
                savedActivitySearch.displayName = quickLink.Name;
                savedActivitySearch.isPublic = quickLink.IsPublic.HasValue && quickLink.IsPublic.Value;
            }

            var user = _userOperations.GetUserByUsername(SessionManager.Instance.CurrentUser.Username);
            if (user != null)
            {
                savedActivitySearch.user_FK1 = user.Person_FK;
            }

            savedActivitySearch = _savedActivitySearch.Save(savedActivitySearch);
            Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}&Action=Search&idSearch={1}", EntityContext.Activity, savedActivitySearch.activity_saved_search_PK));
        }

        #endregion

        #endregion

        #region Support methods

        private void ClearSearch()
        {
            txtSrProduct.Text = string.Empty;
            txtSrProject.Text = string.Empty;
            txtActivityName.Text = string.Empty;
            ddlResponsibleUser.SelectedValue = string.Empty;
            txtProcedureNumber.Text = string.Empty;
            ddlProcedureType.SelectedValue = string.Empty;
            ddlType.SelectedValue = string.Empty;
            ddlRegulatoryStatus.SelectedValue = string.Empty;
            ddlInternalStatus.SelectedValue = string.Empty;
            ddlActivityMode.SelectedValue = string.Empty;
            ddlCountry.SelectedValue = string.Empty;
            dtRngStartDate.Clear();
            dtRngExpectedFinishedDate.Clear();
            dtRngActualFinishedDate.Clear();
            dtRngSubmissionDate.Clear();
            dtRngApprovalDate.Clear();
        }

        private Dictionary<string, string> GetFilters()
        {
            var filters = ActivityGrid.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (_idSearch.HasValue)
                    {
                        Activity_saved_search_PK savedActivitySearch = _savedActivitySearch.GetEntity(_idSearch);
                        FillFilters(savedActivitySearch, filters);
                    }

                    if (EntityContext == EntityContext.Product || EntityContext == EntityContext.Project)
                    {
                        filters.Add("QueryBy", EntityContext.ToString());
                        if (EntityContext == EntityContext.Product) filters.Add("EntityPk", Convert.ToString(_idProd));
                        else if (EntityContext == EntityContext.Project) filters.Add("EntityPk", Convert.ToString(_idProj));
                    }

                    if (EntityContext == EntityContext.ActivityMy)
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
                            filters["QueryBy"] = "ActivityMyAlerts";
                        }
                        else
                        {
                            filters.Add("QueryBy", "ActivityMyAlerts");
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
            if (txtSrProduct.SelectedEntityId.HasValue) filters.Add("SearchProductPk", txtSrProduct.SelectedEntityId.Value.ToString());
            if (txtSrProject.SelectedEntityId.HasValue) filters.Add("SearchProjectPk", txtSrProject.SelectedEntityId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtActivityName.Text)) filters.Add("SearchActivityName", txtActivityName.Text);
            if (ddlResponsibleUser.SelectedId.HasValue) filters.Add("SearchResponsibleUserPk", ddlResponsibleUser.SelectedId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtProcedureNumber.Text)) filters.Add("SearchProcedureNumber", txtProcedureNumber.Text);
            if (ddlProcedureType.SelectedId.HasValue) filters.Add("SearchProcedureTypePk", ddlProcedureType.SelectedId.Value.ToString());
            if (ddlType.SelectedId.HasValue) filters.Add("SearchTypePk", ddlType.SelectedId.Value.ToString());
            if (ddlRegulatoryStatus.SelectedId.HasValue) filters.Add("SearchRegulatoryStatusPk", ddlRegulatoryStatus.SelectedId.Value.ToString());
            if (ddlInternalStatus.SelectedId.HasValue) filters.Add("SearchInternalStatusPk", ddlInternalStatus.SelectedId.Value.ToString());
            if (ddlActivityMode.SelectedId.HasValue) filters.Add("SearchActivityModePk", ddlActivityMode.SelectedId.Value.ToString());
            if (ddlApplicant.SelectedId.HasValue) filters.Add("SearchApplicantPk", ddlApplicant.SelectedId.Value.ToString());
            if (ddlCountry.SelectedId.HasValue) filters.Add("SearchCountryPk", ddlCountry.SelectedId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtLegalBasis.Text)) filters.Add("SearchLegalBasis", txtLegalBasis.Text);
            if (!string.IsNullOrWhiteSpace(txtActivityId.Text)) filters.Add("SearchActivityID", txtActivityId.Text);
            if (rbYnBillable.SelectedItem != null) filters.Add("SearchBillable", rbYnBillable.SelectedValue != null && rbYnBillable.SelectedValue.Value ? "Yes" : "No"); else filters.Add("SearchBillable", "");
            if (ValidationHelper.IsValidDateTime(dtRngStartDate.TextFrom, CultureInfoHr)) filters.Add("SearchStartDateFrom", dtRngStartDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngStartDate.TextTo, CultureInfoHr)) filters.Add("SearchStartDateTo", dtRngStartDate.TextTo);
            if (ValidationHelper.IsValidDateTime(dtRngExpectedFinishedDate.TextFrom, CultureInfoHr)) filters.Add("SearchExpectedFinishedDateFrom", dtRngExpectedFinishedDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngExpectedFinishedDate.TextTo, CultureInfoHr)) filters.Add("SearchExpectedFinishedDateTo", dtRngExpectedFinishedDate.TextTo);
            if (ValidationHelper.IsValidDateTime(dtRngActualFinishedDate.TextFrom, CultureInfoHr)) filters.Add("SearchActualFinishedDateFrom", dtRngActualFinishedDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngActualFinishedDate.TextTo, CultureInfoHr)) filters.Add("SearchActualFinishedDateTo", dtRngActualFinishedDate.TextTo);
            if (ValidationHelper.IsValidDateTime(dtRngSubmissionDate.TextFrom, CultureInfoHr)) filters.Add("SearchSubmissionDateFrom", dtRngSubmissionDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngSubmissionDate.TextTo, CultureInfoHr)) filters.Add("SearchSubmissionDateTo", dtRngSubmissionDate.TextTo);
            if (ValidationHelper.IsValidDateTime(dtRngApprovalDate.TextFrom, CultureInfoHr)) filters.Add("SearchApprovalDateFrom", dtRngApprovalDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngApprovalDate.TextTo, CultureInfoHr)) filters.Add("SearchApprovalDateTo", dtRngApprovalDate.TextTo);
        }

        private void FillFilters(Activity_saved_search_PK savedActivitySearch, Dictionary<string, string> filters)
        {
            if (savedActivitySearch.product_FK.HasValue) filters.Add("SearchProductPk", savedActivitySearch.product_FK.Value.ToString());
            if (savedActivitySearch.project_FK.HasValue) filters.Add("SearchProjectPk", savedActivitySearch.project_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedActivitySearch.name)) filters.Add("SearchActivityName", savedActivitySearch.name);
            if (savedActivitySearch.user_FK.HasValue) filters.Add("SearchResponsibleUserPk", savedActivitySearch.user_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedActivitySearch.procedure_number)) filters.Add("SearchProcedureNumber", savedActivitySearch.procedure_number);
            if (savedActivitySearch.procedure_type_FK.HasValue) filters.Add("SearchProcedureTypePk", savedActivitySearch.procedure_type_FK.Value.ToString());
            if (savedActivitySearch.type_FK.HasValue) filters.Add("SearchTypePk", savedActivitySearch.type_FK.Value.ToString());
            if (savedActivitySearch.regulatory_status_FK.HasValue) filters.Add("SearchRegulatoryStatusPk", savedActivitySearch.regulatory_status_FK.Value.ToString());
            if (savedActivitySearch.internal_status_FK.HasValue) filters.Add("SearchInternalStatusPk", savedActivitySearch.internal_status_FK.Value.ToString());
            if (savedActivitySearch.activity_mode_FK.HasValue) filters.Add("SearchActivityModePk", savedActivitySearch.activity_mode_FK.Value.ToString());
            if (savedActivitySearch.applicant_FK.HasValue) filters.Add("SearchApplicantPk", savedActivitySearch.applicant_FK.Value.ToString());
            if (savedActivitySearch.country_FK.HasValue) filters.Add("SearchCountryPk", savedActivitySearch.country_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedActivitySearch.legal)) filters.Add("SearchLegalBasis", savedActivitySearch.legal);
            if (!string.IsNullOrWhiteSpace(savedActivitySearch.activity_Id)) filters.Add("SearchActivityID", savedActivitySearch.activity_Id);
            if (savedActivitySearch.billable.HasValue) filters.Add("SearchBillable", savedActivitySearch.billable.Value ? "Yes" : "No"); else filters.Add("SearchBillable", "");
            if (savedActivitySearch.start_date_from.HasValue) filters.Add("SearchStartDateFrom", savedActivitySearch.start_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedActivitySearch.start_date_to.HasValue) filters.Add("SearchStartDateTo", savedActivitySearch.start_date_to.Value.ToString(Constant.DateTimeFormat));
            if (savedActivitySearch.expected_finished_date_from.HasValue) filters.Add("SearchExpectedFinishedDateFrom", savedActivitySearch.expected_finished_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedActivitySearch.expected_finished_date_to.HasValue) filters.Add("SearchExpectedFinishedDateTo", savedActivitySearch.expected_finished_date_to.Value.ToString(Constant.DateTimeFormat));
            if (savedActivitySearch.actual_finished_date_from.HasValue) filters.Add("SearchActualFinishedDateFrom", savedActivitySearch.actual_finished_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedActivitySearch.actual_finished_date_to.HasValue) filters.Add("SearchActualFinishedDateTo", savedActivitySearch.actual_finished_date_to.Value.ToString(Constant.DateTimeFormat));
            if (savedActivitySearch.submission_date_from.HasValue) filters.Add("SearchSubmissionDateFrom", savedActivitySearch.submission_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedActivitySearch.submission_date_to.HasValue) filters.Add("SearchSubmissionDateTo", savedActivitySearch.submission_date_to.Value.ToString(Constant.DateTimeFormat));
            if (savedActivitySearch.approval_date_from.HasValue) filters.Add("SearchApprovalDateFrom", savedActivitySearch.approval_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedActivitySearch.approval_date_to.HasValue) filters.Add("SearchApprovalDateTo", savedActivitySearch.approval_date_to.Value.ToString(Constant.DateTimeFormat));
        }

        private void GenerateContextMenuItems()
        {
            var contexMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                switch (EntityContext)
                {
                    case EntityContext.Activity:
                    case EntityContext.ActivityMy:
                        contexMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") };
                        break;
                    case EntityContext.Product:
                    case EntityContext.Project:
                        contexMenu = new[] { 
                                                new ContextMenuItem(ContextMenuEventTypes.Back, "Back"), 
                                                new ContextMenuItem(ContextMenuEventTypes.New, "New") 
                                           };
                        break;
                }

                MasterPage.ContextMenu.SetContextMenuItemsVisible(contexMenu);
            }
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location = null;

            if (ListType == ListType.List)
            {
                switch (EntityContext)
                {
                    case EntityContext.Activity:
                    case EntityContext.ActivityMy:
                        location = Support.LocationManager.Instance.GetLocationByName(EntityContext == EntityContext.Activity ? "Act" : "ActMy", Support.CacheManager.Instance.AppLocations);
                        tabMenu.Visible = false;
                        if (location != null)
                        {
                            MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                            MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                        }
                                break;

                    case EntityContext.Project:
                    case EntityContext.Product:
                        if (EntityContext == EntityContext.Product) location = Support.LocationManager.Instance.GetLocationByName("ProdActList", Support.CacheManager.Instance.AppLocations);
                        else if (EntityContext == EntityContext.Project) location = Support.LocationManager.Instance.GetLocationByName("ProjActList", Support.CacheManager.Instance.AppLocations);

                        MasterPage.TabMenu.TabControls.Clear();
                        tabMenu.Visible = true;
                        if (location != null)
                        {
                            tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                            tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                        } 
                        break;
                }
            }
            else if (ListType == ListType.Search)
            {
                location = Support.LocationManager.Instance.GetLocationByName("ActSearch", Support.CacheManager.Instance.AppLocations);
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            //Location_PK location = null;

            //if (ListType == ListType.List)
            //{
            //    location = Support.LocationManager.Instance.GetLocationByName("Act", Support.CacheManager.Instance.AppLocations);
            //}
            //else if (ListType == ListType.Search)
            //{
            //    location = Support.LocationManager.Instance.GetLocationByName("ActSearch", Support.CacheManager.Instance.AppLocations);
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
            var controls = new[] { "hlID", "hlTaskCount", "hlNewTask", "hlTimeCount", "hlNewTime", "hlDocumentsCount", "hlNewDocument" };

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
                isRed = false;
            }
        }

        private void BindProducts(Panel pnlProducts, int activityPk)
        {
            var activity = _activityOperations.GetEntity(activityPk);
            if (activity == null || activity.activity_PK == null) return;

            var ds = _activityProductMnOperations.GetProductsByActivity(activity.activity_PK);
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

        private void HandleEntityContextProduct()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Product:";

            var product = _productOperations.GetEntity(_idProd);

            lblPrvParentEntity.Text = product != null && !string.IsNullOrWhiteSpace(product.name) ? product.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextProject()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Project:";

            var project = _projectOperations.GetEntity(_idProj);

            lblPrvParentEntity.Text = project != null && !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.ControlDefault.LbPrvText;
        }

        private void BindIdLink(HyperLink hlId)
        {
            BindActivityEntityContextLink(hlId);
        }

        private void BindDocumentCountLink(HyperLink hlDocumentsCount)
        {
            BindActivityEntityContextLink(hlDocumentsCount);
        }

        private void BindTaskCountLink(HyperLink hlDocumentsCount)
        {
            BindActivityEntityContextLink(hlDocumentsCount);
        }

        private void BindTimeUnitCountLink(HyperLink hlDocumentsCount)
        {
            BindActivityEntityContextLink(hlDocumentsCount);
        }

        private void BindNewDocumentLink(HyperLink hlNewDocument)
        {
            BindGridNewLink(hlNewDocument);
        }

        private void BindNewTaskLink(HyperLink hlNewTask)
        {
            BindGridNewLink(hlNewTask);
        }

        private void BindNewTimeLink(HyperLink hlNewTime)
        {
            BindGridNewLink(hlNewTime);
        }

        private void BindGridNewLink(HyperLink hlNewLink)
        {
            switch (ListType)
            {
                case ListType.List:
                    if (EntityContext == EntityContext.Activity) hlNewLink.NavigateUrl += "&From=Act";
                    else if (EntityContext == EntityContext.ActivityMy) hlNewLink.NavigateUrl += "&From=ActMy";
                    else if (EntityContext == EntityContext.Product && _idProd.HasValue) hlNewLink.NavigateUrl += string.Format("&From=ProdActList&idProd={0}", _idProd);
                    else if (EntityContext == EntityContext.Project && _idProj.HasValue) hlNewLink.NavigateUrl += string.Format("&From=ProjActList&idProj={0}", _idProj);
                    break;

                case ListType.Search:
                    hlNewLink.NavigateUrl += "&From=ActSearch";
                    break;
            }

            BindActivityEntityContextLink(hlNewLink);
        }

        private void BindActivityEntityContextLink(HyperLink hlActivityEntityContext)
        {
            if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) hlActivityEntityContext.NavigateUrl += string.Format("&EntityContext={0}", EntityContext);
            else hlActivityEntityContext.NavigateUrl += string.Format("&EntityContext={0}", EntityContext.Activity);
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedInsertActivity = SecurityHelper.IsPermitted(Permission.InsertActivity);
                
            if (isPermittedInsertActivity)
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