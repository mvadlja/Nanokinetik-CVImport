using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUIFramework;
using GEM2Common;
using PossGrid;
using Ready.Model;
using CacheManager = AspNetUI.Support.CacheManager;
using LocationManager = AspNetUI.Support.LocationManager;

namespace AspNetUI.Views.SubmissionUnitView
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
        private int? _idSubUnit;
        private int? _idTask;
        private int? _idProd;
        private string _gridId;

        private ISubbmission_unit_PKOperations _submissionUnitOperations;
        private ISubmission_unit_saved_search_PKOperations _savedSubmissionUnitSearch;
        private IActivity_PKOperations _activityOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IUSEROperations _userOperations;
        private IType_PKOperations _typeOperations;
        private ICountry_PKOperations _countryOperations;
        private ITask_PKOperations _taskOperations;
        private IProduct_PKOperations _productOperations;
        private ITask_name_PKOperations _taskNameOperations;
        private IOrganization_PKOperations _organizationOperations;
        private IAttachment_PKOperations _attachmentOperations;

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
            MasterPage.UpTopMenu.Update();
        }

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();
            LoadActionQuery();

            _idSearch = ValidationHelper.IsValidInt( Request.QueryString["idSearch"]) ? int.Parse( Request.QueryString["idSearch"]) : (int?)null;
            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"] ) ? int.Parse(Request.QueryString["idAct"] ) : (int?)null;
            _idTask = ValidationHelper.IsValidInt( Request.QueryString["idTask"] ) ? int.Parse( Request.QueryString["idTask"] ) : (int?)null;
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;
            _idSubUnit = ValidationHelper.IsValidInt(Request.QueryString["idSubUnit"]) ? int.Parse(Request.QueryString["idSubUnit"]) : (int?)null;

            _activityOperations = new Activity_PKDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();
            _typeOperations = new Type_PKDAL();
            _countryOperations = new Country_PKDAL();
            _taskOperations = new Task_PKDAL();
            _submissionUnitOperations = new Subbmission_unit_PKDAL();
            _productOperations = new Product_PKDAL();
            _taskNameOperations = new Task_name_PKDAL();
            _savedSubmissionUnitSearch = new Submission_unit_saved_search_PKDAL();
            _organizationOperations = new Organization_PKDAL();
            _attachmentOperations = new Attachment_PKDAL();

            if (ListType == ListType.Search)
            {
                SubmissionUnitGrid.GridVersion = SubmissionUnitGrid.GridVersion + ListType.ToString();
            }
            else if (EntityContext != EntityContext.Default)
            {
                SubmissionUnitGrid.GridVersion = SubmissionUnitGrid.GridVersion + EntityContext.ToString();
            }
            
            _gridId = SubmissionUnitGrid.GridId + "_" + SubmissionUnitGrid.GridVersion;
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
                    txtSrActivity.Searcher.OnListItemSelected += ActivitySearcher_OnListItemSelected;
                    txtSrTask.Searcher.OnListItemSelected += TaskSearcher_OnListItemSelected;
                    QuickLinksPopup.OnOkButtonClick += QuickLinksPopup_OnOkButtonClick;

                    btnExportLower.Click += btnExport_Click;
                    subtabs.Controls.Clear();
                    break;
            }

            SubmissionUnitGrid.OnRebindRequired += SubmissionUnitGrid_OnRebindRequired;
            SubmissionUnitGrid.OnHtmlRowPrepared += SubmissionUnitGrid_OnHtmlRowPrepared;
            SubmissionUnitGrid.OnHtmlCellPrepared += SubmissionUnitGrid_OnHtmlCellPrepared;
            SubmissionUnitGrid.OnExcelCellPrepared += SubmissionUnitGrid_OnExcelCellPrepared;
            SubmissionUnitGrid.OnLoadClientLayout += SubmissionUnitGrid_OnLoadClientLayout;
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
                    FillDdlSubmissionUnitDescription();
                    FillDdlAgency();
                    FillDdlRms();
                    FillDdlSubmissionFormat();
                    FillDdlDtdSchemaVersion();
                    break;
            }
        }

        private void FillDdlSubmissionUnitDescription()
        {
            var submissionUnitDescriptionList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.SubmissionUnitDescription);
            ddlSubmissionUnitDescription.Fill(submissionUnitDescriptionList, "name", "type_PK");
            ddlSubmissionUnitDescription.SortItemsByText();
        }

        private void FillDdlAgency()
        {
            var agencyList = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.Agency);
            ddlAgency.Fill(agencyList, "name_org", "organization_PK");
            ddlAgency.SortItemsByText();
        }

        private void FillDdlRms()
        {
            var countryList = _countryOperations.GetEntitiesCustomSort();
            var countryListRms = countryList.FindAll(item => (item != null && item.region != null && item.region.Trim().ToLower() == "eu"));
            ddlRms.Fill(countryListRms, Constant.Countries.DisplayNameFormat, "country_PK");
        }

        private void FillDdlSubmissionFormat()
        {
            var submissionFormatList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.SubmissionUnitFormat);
            ddlSubmissionFormat.Fill(submissionFormatList, "name", "type_PK");
            ddlSubmissionFormat.SortItemsByText();
        }

        private void FillDdlDtdSchemaVersion()
        {
            var dtdSchemaVersionList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.DtdSchemaVersion);
            ddlDtdSchemaVersion.Fill(dtdSchemaVersionList, "name", "type_PK");
            ddlDtdSchemaVersion.SortItemsByText();
        }

        private void FillComboSubmissionFormat()
        {
            var submissionFormatList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.SubmissionUnitFormat);
            var submissionFormatListItems = new List<ListItem> { new ListItem("", "") };
            submissionFormatList.ForEach(item => submissionFormatListItems.Add(new ListItem(item.name, item.name)));
            SubmissionUnitGrid.SetComboList("SubmissionFormat", submissionFormatListItems);
        }

        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    if (EntityContext == EntityContext.ActivityMy || EntityContext == EntityContext.Activity) HandleListModeByActivity();
                    else if (EntityContext == EntityContext.Task) HandleListModeByTask();
                    else if (EntityContext == EntityContext.Product) HandleListModeByProduct();
                    HideSearch();

                    if (Request.QueryString["idLay"] == "default")
                    {
                        SubmissionUnitGrid.ClearFilters();
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
                SubmissionUnitGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }

            if (!_idSearch.HasValue || Request.QueryString["Clear"] == "true") return;

            var savedSubmissionUnitSearch = _savedSubmissionUnitSearch.GetEntity(_idSearch);

            if (savedSubmissionUnitSearch == null) return;

            if (ListType == ListType.Search)
            {
                BindProduct(savedSubmissionUnitSearch.product_FK);
                BindActivity(savedSubmissionUnitSearch.activity_FK);
                BindTask(savedSubmissionUnitSearch.task_FK);
                ddlSubmissionUnitDescription.SelectedId = savedSubmissionUnitSearch.description_type_FK;
                ddlAgency.SelectedId = savedSubmissionUnitSearch.agency_FK;
                ddlRms.SelectedId = savedSubmissionUnitSearch.rms_FK;
                txtSubmissionId.Text = savedSubmissionUnitSearch.submission_ID;
                ddlSubmissionFormat.SelectedId = savedSubmissionUnitSearch.s_format_FK;
                txtSequence.Text = savedSubmissionUnitSearch.sequence;
                ddlDtdSchemaVersion.SelectedId = savedSubmissionUnitSearch.dtd_schema_FK;
                dtRngDispatchDate.TextFrom = savedSubmissionUnitSearch.dispatch_date_from.HasValue ? savedSubmissionUnitSearch.dispatch_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngDispatchDate.TextTo = savedSubmissionUnitSearch.dispatch_date_to.HasValue ? savedSubmissionUnitSearch.dispatch_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngReceiptDate.TextFrom = savedSubmissionUnitSearch.receipt_date_from.HasValue ? savedSubmissionUnitSearch.receipt_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngReceiptDate.TextTo = savedSubmissionUnitSearch.receipt_to.HasValue ? savedSubmissionUnitSearch.receipt_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
            }

            SubmissionUnitGrid.SetClientLayout(savedSubmissionUnitSearch.gridLayout);
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (SubmissionUnitGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(SubmissionUnitGrid.SecondSortingColumn, SubmissionUnitGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (SubmissionUnitGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(SubmissionUnitGrid.MainSortingColumn, SubmissionUnitGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("subbmission_unit_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _submissionUnitOperations.GetListFormSearchDataSet(filters, SubmissionUnitGrid.CurrentPage, SubmissionUnitGrid.PageSize, gobList, out itemCount);
                else ds = _submissionUnitOperations.GetListFormDataSet(filters, SubmissionUnitGrid.CurrentPage, SubmissionUnitGrid.PageSize, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _submissionUnitOperations.GetListFormSearchDataSet(filters, SubmissionUnitGrid.CurrentPage, SubmissionUnitGrid.PageSize, gobList, out itemCount);
            }

            SubmissionUnitGrid.TotalRecords = itemCount;

            if (SubmissionUnitGrid.CurrentPage > SubmissionUnitGrid.TotalPages || (SubmissionUnitGrid.CurrentPage == 0 && SubmissionUnitGrid.TotalPages > 0))
            {
                if (SubmissionUnitGrid.CurrentPage > SubmissionUnitGrid.TotalPages) SubmissionUnitGrid.CurrentPage = SubmissionUnitGrid.TotalPages; else SubmissionUnitGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    if (_idSearch.HasValue) ds = _submissionUnitOperations.GetListFormSearchDataSet(filters, SubmissionUnitGrid.CurrentPage, SubmissionUnitGrid.PageSize, gobList, out itemCount);
                    else ds = _submissionUnitOperations.GetListFormDataSet(filters, SubmissionUnitGrid.CurrentPage, SubmissionUnitGrid.PageSize, gobList, out itemCount);
                }
                else if (ListType == ListType.Search)
                {
                    ds = _submissionUnitOperations.GetListFormSearchDataSet(filters, SubmissionUnitGrid.CurrentPage, SubmissionUnitGrid.PageSize, gobList, out itemCount);
                }
            }

            SubmissionUnitGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            SubmissionUnitGrid.DataBind();

            MasterPage.UpCommon.Update();
        }

        private void BindGridDynamicControls()
        {
            FillComboSubmissionFormat();
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

        private void BindProduct(int? productFk)
        {
            var product = _productOperations.GetEntity(productFk);
            if (product == null || product.product_PK == null) return;

            txtSrProduct.SelectedEntityId = product.product_PK;
            txtSrProduct.Text = product.GetNameFormatted();
        }

        private void BindTask(int? taskFk)
        {
            var task = _taskOperations.GetEntity(taskFk);
            if (task == null || task.task_PK == null || task.task_name_FK == null) return;
            var taskName = _taskNameOperations.GetEntity(task.task_name_FK);

            txtSrTask.SelectedEntityId = task.task_PK;
            txtSrTask.Text = !string.IsNullOrWhiteSpace(taskName.task_name) ? taskName.task_name : Constant.MissingValue;
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
                        if (EntityContext == EntityContext.Task && _idTask.HasValue) Response.Redirect(string.Format("~/Views/SubmissionUnitView/Form.aspx?EntityContext={0}&Action=New&idTask={1}&From=TaskSubUnitList", EntityContext, _idTask));
                        else if (EntityContext == EntityContext.Activity && _idAct.HasValue) Response.Redirect(string.Format("~/Views/SubmissionUnitView/Form.aspx?EntityContext={0}&Action=New&idAct={1}&From=ActSubUnitList", EntityContext, _idAct));
                        else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) Response.Redirect(string.Format("~/Views/SubmissionUnitView/Form.aspx?EntityContext={0}&Action=New&idAct={1}&From=ActMySubUnitList", EntityContext, _idAct));
                        else if (EntityContext == EntityContext.Product && _idProd.HasValue)Response.Redirect(string.Format("~/Views/SubmissionUnitView/Form.aspx?EntityContext={0}&Action=New&idProd={1}&From=ProdSubUnitList", EntityContext, _idProd));
                        Response.Redirect(string.Format("~/Views/SubmissionUnitView/Form.aspx?EntityContext={0}&Action=New&From=SubUnit", EntityContext.Default));
                    }
                    break;
                case ContextMenuEventTypes.Back:
                    {
                        if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Task) Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
                        Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}", EntityContext.SubmissionUnit));
                    }
                    break;
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in SubmissionUnitGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("subbmission_unit_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (SubmissionUnitGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            SubmissionUnitGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            SubmissionUnitGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            
            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = SubmissionUnitGrid.GetClientLayoutString(),
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
            if (SubmissionUnitGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(SubmissionUnitGrid.SecondSortingColumn, SubmissionUnitGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (SubmissionUnitGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(SubmissionUnitGrid.MainSortingColumn, SubmissionUnitGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("subbmission_unit_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _submissionUnitOperations.GetListFormSearchDataSet(filters, SubmissionUnitGrid.CurrentPage, SubmissionUnitGrid.PageSize, gobList, out itemCount); // Quick link
                else ds = _submissionUnitOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _submissionUnitOperations.GetListFormSearchDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            if (ds != null) SubmissionUnitGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            SubmissionUnitGrid.ResetVisibleColumns();
            SubmissionUnitGrid.SecondSortingColumn = null;
            SubmissionUnitGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
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
            Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}&Action=Search&Clear=true{1}", EntityContext.SubmissionUnit, (_idSearch.HasValue ? "&idSearch=" + _idSearch : string.Empty)));
        }

        #endregion

        #region Grid

        void SubmissionUnitGrid_OnExcelCellPrepared(object sender, PossGridExportCellRenderArgs args)
        {
            if (args.RowType != DataControlRowType.DataRow) return;
        }

        void SubmissionUnitGrid_OnHtmlRowPrepared(object sender, PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var submissionUnitPk = ValidationHelper.IsValidInt(Convert.ToString(e.GetValue("subbmission_unit_PK"))) ? (int?)Convert.ToInt32(e.GetValue("subbmission_unit_PK")) : null;

            var pnlProducts = e.FindControl("pnlProducts") as Panel;
            if (pnlProducts != null && submissionUnitPk.HasValue)
            {
                BindProducts(pnlProducts, submissionUnitPk.Value);
            }

            var documentFk = ValidationHelper.IsValidInt(Convert.ToString(e.GetValue("document_FK"))) ? (int?)Convert.ToInt32(e.GetValue("document_FK")) : null;
            var neesFk = ValidationHelper.IsValidInt(Convert.ToString(e.GetValue("ness_FK"))) ? (int?)Convert.ToInt32(e.GetValue("ness_FK")) : null;
            var ectdFk = ValidationHelper.IsValidInt(Convert.ToString(e.GetValue("ectd_FK"))) ? (int?)Convert.ToInt32(e.GetValue("ectd_FK")) : null;
           
            var sequence = Convert.ToString(e.GetValue("Sequence"));
            var pnlAttachments = e.FindControl("pnlAttachments") as Panel;
            if (pnlAttachments != null)
            {
                BindAttachents(pnlAttachments, documentFk);
                BindAttachents(pnlAttachments, neesFk, sequence);
                BindAttachents(pnlAttachments, ectdFk, sequence);
            }

            var pnlSequence = e.FindControl("pnlSequence") as Panel;
            if (pnlSequence != null)
            {
                BindSequence(pnlSequence, sequence, neesFk, ectdFk);
            }
        }

        void SubmissionUnitGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void SubmissionUnitGrid_OnHtmlCellPrepared(object sender, PossGridCellEventArgs e)
        {
            if (!SubmissionUnitGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = SubmissionUnitGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (SubmissionUnitGrid.SortCount > 1 && e.FieldName == SubmissionUnitGrid.MainSortingColumn)
                return;

            var leftAlignedColumns = new List<string> { "SubmissionUnitDescription", "Products", "ActivityName", "TaskName", "SubmissionId", "SubmissionFormat", "Sequence", "Agency" };
            if (leftAlignedColumns.Contains(e.FieldName))
            {
                e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
                if (isRed) e.Cell.ForeColor = System.Drawing.Color.Red;
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
                if (isRed) e.Cell.ForeColor = System.Drawing.Color.Red;
            }
        }

        void SubmissionUnitGrid_OnLoadClientLayout(object sender, ClientLayoutEventArgs args)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
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

            txtSrProduct.Text = product.name;
            txtSrProduct.SelectedEntityId = product.product_PK;
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

        #region Task searcher

        /// <summary>
        /// Handles task list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TaskSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var task = _taskOperations.GetEntity(e.Data);

            if (task == null || task.task_PK == null || task.task_name_FK == null) return;

            var taskName = _taskNameOperations.GetEntity(task.task_name_FK);

            if (!string.IsNullOrWhiteSpace(taskName.task_name)) txtSrTask.Text = taskName.task_name;
            else txtSrTask.Text = "Missing";
            txtSrTask.SelectedEntityId = task.task_PK;
        }

        #endregion

        #region Quick links

        public void btnSaveSearchClick(object sender, EventArgs e)
        {
            QuickLink quickLink = null;
            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                var savedSearch = _savedSubmissionUnitSearch.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
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

            _savedSubmissionUnitSearch.Delete(Convert.ToInt32(Request.QueryString["idSearch"]));
            Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}&Action=Search", EntityContext.SubmissionUnit));
        }

        protected void QuickLinksPopup_OnOkButtonClick(object sender, FormEventArgs<QuickLink> e)
        {
            Submission_unit_saved_search_PK savedSubmissionUnitSearch = null;

            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                savedSubmissionUnitSearch = _savedSubmissionUnitSearch.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
            }

            if (savedSubmissionUnitSearch == null)
            {
                savedSubmissionUnitSearch = new Submission_unit_saved_search_PK();
            }

            var dispatchDateFrom = ValidationHelper.IsValidDateTime(dtRngDispatchDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngDispatchDate.TextFrom, CultureInfoHr) : null;
            var dispatchDateTo = ValidationHelper.IsValidDateTime(dtRngDispatchDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngDispatchDate.TextTo, CultureInfoHr) : null;
            var receiptDateFrom = ValidationHelper.IsValidDateTime(dtRngReceiptDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngReceiptDate.TextFrom, CultureInfoHr) : null;
            var receiptDateTo = ValidationHelper.IsValidDateTime(dtRngReceiptDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngReceiptDate.TextTo, CultureInfoHr) : null;

            savedSubmissionUnitSearch.product_FK = txtSrProduct.SelectedEntityId;
            savedSubmissionUnitSearch.activity_FK = txtSrActivity.SelectedEntityId;
            savedSubmissionUnitSearch.task_FK = txtSrTask.SelectedEntityId;
            savedSubmissionUnitSearch.description_type_FK = ddlSubmissionUnitDescription.SelectedId;
            savedSubmissionUnitSearch.agency_FK = ddlAgency.SelectedId;
            savedSubmissionUnitSearch.rms_FK = ddlRms.SelectedId;
            savedSubmissionUnitSearch.submission_ID = txtSubmissionId.Text;
            savedSubmissionUnitSearch.s_format_FK = ddlSubmissionFormat.SelectedId;
            savedSubmissionUnitSearch.sequence = txtSequence.Text;
            savedSubmissionUnitSearch.dtd_schema_FK = ddlDtdSchemaVersion.SelectedId;
            savedSubmissionUnitSearch.dispatch_date_from = dispatchDateFrom;
            savedSubmissionUnitSearch.dispatch_date_to = dispatchDateTo;
            savedSubmissionUnitSearch.receipt_date_from = receiptDateFrom;
            savedSubmissionUnitSearch.receipt_to = receiptDateTo;

            var quickLink = e.Data;
            if (quickLink != null)
            {
                savedSubmissionUnitSearch.displayName = quickLink.Name;
                savedSubmissionUnitSearch.isPublic = quickLink.IsPublic.HasValue && quickLink.IsPublic.Value;
            }

            var user = _userOperations.GetUserByUsername(SessionManager.Instance.CurrentUser.Username);
            if (user != null)
            {
                savedSubmissionUnitSearch.user_FK = user.Person_FK;
            }

            savedSubmissionUnitSearch = _savedSubmissionUnitSearch.Save(savedSubmissionUnitSearch);
            Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}&Action=Search&idSearch={1}", EntityContext.SubmissionUnit, savedSubmissionUnitSearch.submission_unit_saved_search_PK));
        }

        #endregion

        #endregion

        #region Support methods

        private void ClearSearch()
        {
            txtSrProduct.Text = string.Empty;
            txtSrActivity.Text = string.Empty;
            txtSrTask.Text = string.Empty;
            ddlSubmissionUnitDescription.SelectedValue = string.Empty;
            ddlAgency.SelectedValue = string.Empty;
            ddlRms.SelectedValue = string.Empty;
            txtSubmissionId.Text = string.Empty;
            ddlSubmissionFormat.SelectedValue = string.Empty;
            txtSequence.Text = string.Empty;
            ddlDtdSchemaVersion.SelectedValue = string.Empty;
            dtRngDispatchDate.Clear();
            dtRngReceiptDate.Clear();
        }

        private Dictionary<string, string> GetFilters()
        {
            var filters = SubmissionUnitGrid.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (_idSearch.HasValue)
                    {
                        Submission_unit_saved_search_PK savedSubmissionUnitSearch = _savedSubmissionUnitSearch.GetEntity(_idSearch);
                        FillFilters(savedSubmissionUnitSearch, filters);
                    }
                    switch (EntityContext)
                    {
                        case EntityContext.Activity:
                        case EntityContext.ActivityMy:
                            filters.Add("QueryBy", EntityContext.ToString());
                            filters.Add("EntityPk", Convert.ToString(_idAct));
                            break;
                        case EntityContext.Task:
                            filters.Add("QueryBy", EntityContext.ToString());
                            filters.Add("EntityPk", Convert.ToString(_idTask));
                            break;
                        case EntityContext.Product:
                            filters.Add("QueryBy", EntityContext.ToString());
                            filters.Add("EntityPk", Convert.ToString(_idProd));
                            break;  
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
            if (txtSrActivity.SelectedEntityId.HasValue) filters.Add("SearchActivityPk", txtSrActivity.SelectedEntityId.Value.ToString());
            if (txtSrTask.SelectedEntityId.HasValue) filters.Add("SearchTaskPk", txtSrTask.SelectedEntityId.Value.ToString());
            if (ddlSubmissionUnitDescription.SelectedId.HasValue) filters.Add("SearchSubmissionUnitDescriptionPk", ddlSubmissionUnitDescription.SelectedId.Value.ToString());
            if (ddlAgency.SelectedId.HasValue) filters.Add("SearchAgencyPk", ddlAgency.SelectedId.Value.ToString());
            if (ddlRms.SelectedId.HasValue) filters.Add("SearchRmsPk", ddlRms.SelectedId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtSubmissionId.Text)) filters.Add("SearchSubmissionId", txtSubmissionId.Text);
            if (ddlSubmissionFormat.SelectedId.HasValue) filters.Add("SearchSubmissionFormatPk", ddlSubmissionFormat.SelectedId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtSequence.Text)) filters.Add("SearchSequenceId", txtSequence.Text);
            if (ddlDtdSchemaVersion.SelectedId.HasValue) filters.Add("SearchDtdSchemaVersionPk", ddlDtdSchemaVersion.SelectedId.Value.ToString());
            if (ValidationHelper.IsValidDateTime(dtRngDispatchDate.TextFrom,CultureInfoHr)) filters.Add("SearchDispatchDateFrom", dtRngDispatchDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngDispatchDate.TextTo,CultureInfoHr)) filters.Add("SearchDispatchDateTo", dtRngDispatchDate.TextTo);
            if (ValidationHelper.IsValidDateTime(dtRngReceiptDate.TextFrom,CultureInfoHr)) filters.Add("SearchReceiptDateFrom", dtRngReceiptDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngReceiptDate.TextTo,CultureInfoHr)) filters.Add("SearchReceiptDateTo", dtRngReceiptDate.TextTo);
        }

        private void FillFilters(Submission_unit_saved_search_PK savedSubmissionUnitSearch, Dictionary<string, string> filters)
        {
            if (savedSubmissionUnitSearch.product_FK.HasValue) filters.Add("SearchProductPk", savedSubmissionUnitSearch.product_FK.Value.ToString());
            if (savedSubmissionUnitSearch.activity_FK.HasValue) filters.Add("SearchActivityPk", savedSubmissionUnitSearch.activity_FK.Value.ToString());
            if (savedSubmissionUnitSearch.task_FK.HasValue) filters.Add("SearchTaskPk", savedSubmissionUnitSearch.task_FK.Value.ToString());
            if (savedSubmissionUnitSearch.description_type_FK.HasValue) filters.Add("SearchSubmissionUnitDescriptionPk", savedSubmissionUnitSearch.description_type_FK.Value.ToString());
            if (savedSubmissionUnitSearch.agency_FK.HasValue) filters.Add("SearchAgencyPk", savedSubmissionUnitSearch.agency_FK.Value.ToString());
            if (savedSubmissionUnitSearch.rms_FK.HasValue) filters.Add("SearchRmsPk", savedSubmissionUnitSearch.rms_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedSubmissionUnitSearch.submission_ID)) filters.Add("SearchSubmissionId", savedSubmissionUnitSearch.submission_ID);
            if (savedSubmissionUnitSearch.s_format_FK.HasValue) filters.Add("SearchSubmissionFormatPk", savedSubmissionUnitSearch.s_format_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedSubmissionUnitSearch.sequence)) filters.Add("SearchSequenceId", savedSubmissionUnitSearch.sequence);
            if (savedSubmissionUnitSearch.dtd_schema_FK.HasValue) filters.Add("SearchDtdSchemaVersionPk", savedSubmissionUnitSearch.dtd_schema_FK.Value.ToString());
            if (savedSubmissionUnitSearch.dispatch_date_from.HasValue) filters.Add("SearchDispatchDateFrom", savedSubmissionUnitSearch.dispatch_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedSubmissionUnitSearch.dispatch_date_to.HasValue) filters.Add("SearchDispatchDateTo", savedSubmissionUnitSearch.dispatch_date_to.Value.ToString(Constant.DateTimeFormat));
            if (savedSubmissionUnitSearch.receipt_date_from.HasValue) filters.Add("SearchReceiptDateFrom", savedSubmissionUnitSearch.receipt_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedSubmissionUnitSearch.dispatch_date_to.HasValue) filters.Add("SearchReceiptDateTo", savedSubmissionUnitSearch.dispatch_date_to.Value.ToString(Constant.DateTimeFormat));
        }

        private void GenerateContextMenuItems()
        {
            var contexMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                switch (EntityContext)
                {
                    case EntityContext.SubmissionUnit:
                        contexMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") };
                        break;
                    case EntityContext.Task:
                    case EntityContext.Activity:
                    case EntityContext.ActivityMy:
                    case EntityContext.Product:
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
                if (EntityContext == EntityContext.SubmissionUnit)
                {
                    location =  Support.LocationManager.Instance.GetLocationByName("SubUnit", Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                    if (location != null)
                    {
                        MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                        MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    }
                }
                else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy || EntityContext == EntityContext.Task || EntityContext == EntityContext.Product)
                {
                    if (EntityContext == EntityContext.Activity) location = Support.LocationManager.Instance.GetLocationByName("ActSubUnitList", Support.CacheManager.Instance.AppLocations);
                    else if (EntityContext == EntityContext.ActivityMy) location = Support.LocationManager.Instance.GetLocationByName("ActMySubUnitList", Support.CacheManager.Instance.AppLocations);
                    else if (EntityContext == EntityContext.Task) location = Support.LocationManager.Instance.GetLocationByName("TaskSubUnitList", Support.CacheManager.Instance.AppLocations);
                    else if (EntityContext == EntityContext.Product) location = Support.LocationManager.Instance.GetLocationByName("ProdSubUnitList", Support.CacheManager.Instance.AppLocations);
                    
                    MasterPage.TabMenu.TabControls.Clear();
                    tabMenu.Visible = true;
                    if (location != null)
                    {
                        tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                        tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    }
                }
            }
            else if (ListType == ListType.Search)
            {
                location = Support.LocationManager.Instance.GetLocationByName("SubUnitListSearch", Support.CacheManager.Instance.AppLocations);
                if (location != null)
                {
                    MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                    MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
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

        private void BindProducts(Panel pnlProducts, int submissionUnitPk)
        {
            var submissionUnit = _submissionUnitOperations.GetEntity(submissionUnitPk);
            if (submissionUnit == null || submissionUnit.subbmission_unit_PK == null) return;

            List<Product_PK> products = _productOperations.GetAssignedProductsForSubmissionUnit(submissionUnitPk);

            foreach (var product in products)
            {
                var hl = new HyperLink
                                   {
                                       NavigateUrl = string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, product.product_PK),
                                       Text = product.GetNameFormatted()
                                   };
                pnlProducts.Controls.Add(hl);
                pnlProducts.Controls.Add(new LiteralControl("<br />"));
            }
        }

        private void BindAttachents(Panel pnlAttachment, int? documentFk, string sequence = null)
        {
            if (documentFk == null) return;

            List<Attachment_PK> attachments = _attachmentOperations.GetAttachmentsForDocument((int)documentFk);
            if(!string.IsNullOrWhiteSpace(sequence)) attachments = attachments.Where(a => a.attachmentname.Contains(sequence)).ToList();

            foreach (var item in attachments)
            {
                var hl = new HyperLink();

                var rootLocation = LocationManager.Instance.GetLocationByName("Root", CacheManager.Instance.AppLocations);
                if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation)) hl.NavigateUrl = "~/Views/Business/FileDownload.ashx?attachID=" + item.attachment_PK;
                
                hl.Text = item.attachmentname;
                pnlAttachment.Controls.Add(hl);
                pnlAttachment.Controls.Add(new LiteralControl("<br />"));
            }
        }

        private void BindSequence(Panel pnlSequence, string sequence, int? neesFk, int? ectdFk)
        {
            var seqLink = new HyperLink();
            if (!string.IsNullOrEmpty(sequence))
            {
                seqLink.Text = sequence;
            }

            if (neesFk != null)
            {
                List<Attachment_PK> attachments = _attachmentOperations.GetAttachmentsForDocument((int)neesFk);

                foreach (var attachment in attachments)
                {
                    if (!attachment.attachmentname.Contains("working"))
                    {
                        var rootLocation = LocationManager.Instance.GetLocationByName("Root", CacheManager.Instance.AppLocations);
                        if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation))
                        {
                            seqLink.NavigateUrl = string.Format("~/Views/Business/ShowSequenceFile.ashx?attachmentPk={0}", attachment.attachment_PK);
                            seqLink.Attributes.Add("target", "_blank");
                        }
                    }
                }
            }

            else if (ectdFk != null)
            {
                List<Attachment_PK> attachments = _attachmentOperations.GetAttachmentsForDocument((int)ectdFk);

                foreach (var attachment in attachments)
                {
                    if (!attachment.attachmentname.Contains("working"))
                    {
                        var rootLocation = LocationManager.Instance.GetLocationByName("Root", CacheManager.Instance.AppLocations);
                        if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation))
                        {
                            seqLink.NavigateUrl = string.Format("~/Views/Business/ShowSequenceFile.ashx?attachmentPk={0}", attachment.attachment_PK);  
                            seqLink.Attributes.Add("target", "_blank");
                        }
                    }
                }
            }

            if (seqLink.NavigateUrl == String.Empty)
            {
                seqLink.Attributes.Add("style", "text-decoration: none!important; border-bottom: 0px!important;");
            }

            pnlSequence.Controls.Add(seqLink);
        }

        private void HandleListModeByActivity()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Activity:";

            var activity = _activityOperations.GetEntity(_idAct);

            lblPrvParentEntity.Text = activity != null && !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleListModeByTask()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Task:";

            var task = _taskOperations.GetEntity(_idTask);

            if (task == null) return;

            var taskName = _taskNameOperations.GetEntity(task.task_name_FK);

            lblPrvParentEntity.Text = taskName != null && !string.IsNullOrWhiteSpace(taskName.task_name) ? taskName.task_name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleListModeByProduct()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Product:";

            var product = _productOperations.GetEntity(_idProd);

            lblPrvParentEntity.Text = product != null && !string.IsNullOrWhiteSpace(product.name) ? product.name : Constant.ControlDefault.LbPrvText;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedInsertSubmissionUnit = SecurityHelper.IsPermitted(Permission.InsertSubmissionUnit);

            if (isPermittedInsertSubmissionUnit)
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