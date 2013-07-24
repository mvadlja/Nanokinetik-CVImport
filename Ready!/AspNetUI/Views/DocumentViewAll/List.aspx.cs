using System;
using System.Collections.Generic;
using System.Data;
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

namespace AspNetUI.Views.DocumentViewAll
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;
        private int? _idSearch;
        private string _gridLayout;
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
        private IOrganization_PKOperations _organizationOperations;
        private IAuthorisedProductOperations _authorisedProductOperations;
        private IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        private ITask_PKOperations _taskOperations;
        private ITask_name_PKOperations _taskNameOperations;
        private IDocument_saved_search_PKOperations _savedDocumentSearch;
        private IDocument_PKOperations _documentOperations;
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

            if (!IsPostBack)
            {
                InitForm(null);
                BindForm(null);
            }

            _gridLayout = DocumentGridAll.GetClientLayoutString();

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

            _activityOperations = new Activity_PKDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();
            _typeOperations = new Type_PKDAL();
            _countryOperations = new Country_PKDAL();
            _personOperations = new Person_PKDAL();
            _activityProductMnOperations = new Activity_product_PKDAL();
            _productOperations = new Product_PKDAL();
            _projectOperations = new Project_PKDAL();
            _organizationOperations = new Organization_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _taskNameOperations = new Task_name_PKDAL();
            _taskOperations = new Task_PKDAL();
            _savedDocumentSearch = new Document_saved_search_PKDAL();
            _documentOperations = new Document_PKDAL();
            _attachmentOperations = new Attachment_PKDAL();

            if (ListType == ListType.Search)
            {
                DocumentGridAll.GridVersion = DocumentGridAll.GridVersion + ListType.ToString();
            }

            _gridId = DocumentGridAll.GridId + "_" + DocumentGridAll.GridVersion;
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
                    txtSrAuthorisedProduct.Searcher.OnListItemSelected += AuthorisedProductSearcher_OnListItemSelected;
                    txtSrPharmaceuticalProduct.Searcher.OnListItemSelected += PharmaceuticalProductSearcher_OnListItemSelected;
                    txtSrProject.Searcher.OnListItemSelected += ProjectSearcher_OnListItemSelected;
                    txtSrActivity.Searcher.OnListItemSelected += ActivitySearcher_OnListItemSelected;
                    txtSrTask.Searcher.OnListItemSelected += TaskSearcher_OnListItemSelected;
                    QuickLinksPopup.OnOkButtonClick += QuickLinksPopup_OnOkButtonClick;

                    btnExportLower.Click += btnExport_Click;
                    subtabs.Controls.Clear();
                    break;
            }

            DocumentGridAll.OnRebindRequired += DocumentGridAllOnRebindRequired;
            DocumentGridAll.OnHtmlRowPrepared += DocumentGridAllOnHtmlRowPrepared;
            DocumentGridAll.OnHtmlCellPrepared += DocumentGridAllOnHtmlCellPrepared;
            DocumentGridAll.OnExcelCellPrepared += DocumentGridAllOnExcelCellPrepared;
            DocumentGridAll.OnLoadClientLayout += DocumentGridAllOnLoadClientLayout;
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
                    FillDdlDocumentType();
                    FillDdlResponsibleUser();
                    FillDdlVersionLabel();
                    FillDdlRegulatoryStatus();
                    break;
            }
        }

        private void FillDdlDocumentType()
        {
            var documentTypeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.DocumentType);
            ddlDocumentType.Fill(documentTypeList, "name", "type_PK");
            ddlDocumentType.SortItemsByText();
        }

        private void FillDdlResponsibleUser()
        {
            var responsibleUserList = _personOperations.GetEntitiesByRoleName(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, "FullName", "person_PK");
            ddlResponsibleUser.SortItemsByText();
        }

        private void FillDdlVersionLabel()
        {
            var versionLabelList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.VersionLabel);
            ddlVersionLabel.Fill(versionLabelList, "name", "type_PK");
            ddlVersionLabel.SortItemsByText();
        }

        private void FillDdlRegulatoryStatus()
        {
            var regulatoryStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.RegulatoryStatusDocuments);
            ddlRegulatoryStatus.Fill(regulatoryStatusList, "name", "type_PK");
            ddlRegulatoryStatus.SortItemsByText();
        }

        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    HideSearch();
                    contextMenu_ContextMenuLayout.Visible = true;

                    if (Request.QueryString["idLay"] == "default")
                    {
                        DocumentGridAll.ClearFilters();
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
                    contextMenu_ContextMenuLayout.Visible = false;
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
                _gridLayout = userGridSettings.grid_layout;
                DocumentGridAll.SetClientLayoutBeforeBind(_gridLayout);
            }

            if (!_idSearch.HasValue || Request.QueryString["Clear"] == "true") return;

            Document_saved_search_PK savedDocumentSearch = _savedDocumentSearch.GetEntity(_idSearch);

            if (savedDocumentSearch == null) return;

            if (ListType == ListType.Search)
            {
                BindProduct(savedDocumentSearch.product_FK);
                BindAuthorisedProduct(savedDocumentSearch.ap_FK);
                BindPharmaceuticalProduct(savedDocumentSearch.pp_FK);
                BindProject(savedDocumentSearch.project_FK);
                BindActivity(savedDocumentSearch.activity_FK);
                BindTask(savedDocumentSearch.task_FK);
                txtEvCode.Text = savedDocumentSearch.ev_code;
                txtDocumentName.Text = savedDocumentSearch.name;
                txtTextSearch.Text = savedDocumentSearch.content;
                ddlDocumentType.SelectedId = savedDocumentSearch.type_FK;
                txtVersionNumber.Text = Convert.ToString(savedDocumentSearch.version_number);
                ddlResponsibleUser.SelectedId = savedDocumentSearch.person_FK;
                ddlVersionLabel.SelectedId = savedDocumentSearch.version_label;
                txtDocumentNumber.Text = savedDocumentSearch.document_number;
                ddlRegulatoryStatus.SelectedId = savedDocumentSearch.regulatory_status;
                txtLanguageCode.Text = savedDocumentSearch.language_code;
                txtComments.Text = savedDocumentSearch.comments;
                dtRngChangeDate.TextFrom = savedDocumentSearch.change_date_from.HasValue ? savedDocumentSearch.change_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngChangeDate.TextTo = savedDocumentSearch.change_date_to.HasValue ? savedDocumentSearch.change_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngEffectiveStartDate.TextFrom = savedDocumentSearch.effective_start_date_from.HasValue ? savedDocumentSearch.effective_start_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngEffectiveStartDate.TextTo = savedDocumentSearch.effective_start_date_to.HasValue ? savedDocumentSearch.effective_start_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngEffectiveEndDate.TextFrom = savedDocumentSearch.effective_end_date_from.HasValue ? savedDocumentSearch.effective_end_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngEffectiveEndDate.TextTo = savedDocumentSearch.effective_end_date_to.HasValue ? savedDocumentSearch.effective_end_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngVersionDate.TextFrom = savedDocumentSearch.version_date_from.HasValue ? savedDocumentSearch.version_date_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngVersionDate.TextTo = savedDocumentSearch.version_date_to.HasValue ? savedDocumentSearch.version_date_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
            }

            DocumentGridAll.SetClientLayout(savedDocumentSearch.gridLayout);
        }

        private void BindAuthorisedProduct(int? apFk)
        {
            var authorisedProduct = _authorisedProductOperations.GetEntity(apFk);
            if (authorisedProduct == null || authorisedProduct.ap_PK == null) return;

            txtSrAuthorisedProduct.SelectedEntityId = authorisedProduct.ap_PK;
            txtSrAuthorisedProduct.Text = !string.IsNullOrWhiteSpace(authorisedProduct.product_name) ? authorisedProduct.product_name : Constant.MissingValue;
        }

        private void BindPharmaceuticalProduct(int? ppFk)
        {
            var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(ppFk);
            if (pharmaceuticalProduct == null || pharmaceuticalProduct.pharmaceutical_product_PK == null) return;

            txtSrPharmaceuticalProduct.SelectedEntityId = pharmaceuticalProduct.pharmaceutical_product_PK;
            txtSrPharmaceuticalProduct.Text = !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.MissingValue;
        }

        private void BindProject(int? projectFk)
        {
            var project = _projectOperations.GetEntity(projectFk);
            if (project == null || project.project_PK == null) return;

            txtSrProject.SelectedEntityId = project.project_PK;
            txtSrProject.Text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.MissingValue;
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

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (DocumentGridAll.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(DocumentGridAll.SecondSortingColumn, DocumentGridAll.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (DocumentGridAll.MainSortingColumn != null) gobList.Add(new GEMOrderBy(DocumentGridAll.MainSortingColumn, DocumentGridAll.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("document_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _documentOperations.GetListFormSearchDataSet(filters, DocumentGridAll.CurrentPage, DocumentGridAll.PageSize, gobList, out itemCount);
                else ds = _documentOperations.GetListFormDataSet(filters, DocumentGridAll.CurrentPage, DocumentGridAll.PageSize, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _documentOperations.GetListFormSearchDataSet(filters, DocumentGridAll.CurrentPage, DocumentGridAll.PageSize, gobList, out itemCount);
            }

            DocumentGridAll.TotalRecords = itemCount;

            if (DocumentGridAll.CurrentPage > DocumentGridAll.TotalPages || (DocumentGridAll.CurrentPage == 0 && DocumentGridAll.TotalPages > 0))
            {
                if (DocumentGridAll.CurrentPage > DocumentGridAll.TotalPages) DocumentGridAll.CurrentPage = DocumentGridAll.TotalPages; else DocumentGridAll.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    if (_idSearch.HasValue) ds = _documentOperations.GetListFormSearchDataSet(filters, DocumentGridAll.CurrentPage, DocumentGridAll.PageSize, gobList, out itemCount);
                    else ds = _documentOperations.GetListFormDataSet(filters, DocumentGridAll.CurrentPage, DocumentGridAll.PageSize, gobList, out itemCount);
                }
                else if (ListType == ListType.Search)
                {
                    ds = _documentOperations.GetListFormSearchDataSet(filters, DocumentGridAll.CurrentPage, DocumentGridAll.PageSize, gobList, out itemCount);
                }
            }

            DocumentGridAll.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            DocumentGridAll.DataBind();

            MasterPage.UpCommon.Update();
        }

        private void BindDynamicControls(object args)
        {
            if (ListType == ListType.Search) subtabs.Controls.Clear();
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
                default:
                    break;
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in DocumentGridAll.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("document_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (DocumentGridAll.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            DocumentGridAll.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            DocumentGridAll.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            string devLayout = _gridLayout;

            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = devLayout,
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
            if (DocumentGridAll.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(DocumentGridAll.SecondSortingColumn, DocumentGridAll.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (DocumentGridAll.MainSortingColumn != null) gobList.Add(new GEMOrderBy(DocumentGridAll.MainSortingColumn, DocumentGridAll.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("document_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _documentOperations.GetListFormSearchDataSet(filters, DocumentGridAll.CurrentPage, DocumentGridAll.PageSize, gobList, out itemCount); // Quick link
                else ds = _documentOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _documentOperations.GetListFormSearchDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            DocumentGridAll["document_PK"].Visible = true;
            if (ds != null) DocumentGridAll.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            DocumentGridAll["document_PK"].Visible = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            DocumentGridAll.ResetVisibleColumns();
            DocumentGridAll.SecondSortingColumn = null;
            DocumentGridAll.MainSortingOrder = PossGrid.SortOrder.ASC;
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
            Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}&Action=Search&Clear=true{1}", EntityContext.Document, (_idSearch.HasValue ? "&idSearch=" + _idSearch : string.Empty)));
        }

        #endregion

        #region Grid

        void DocumentGridAllOnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {
            if (args.RowType != DataControlRowType.DataRow) return;

            //if (args.FieldName == "DocumentName")
            //{
            //    string appUrl = ConfigurationManager.AppSettings["ApplicationURL"];
            //    appUrl = appUrl.StartsWith("http://") ? appUrl : "http://" + appUrl;
            //    args.Cell.Url = appUrl + "/Views/Business/DocumentsView.aspx?f=p&idDoc=" + Convert.ToString(args.Row["document_PK"]);
            //    args.Cell.FontUnderline = true;
            //    args.Cell.Text = HandleMissing(args.Cell.Text);
            //}
        }

        void DocumentGridAllOnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;
            var documentPk = ValidationHelper.IsValidInt(Convert.ToString(e.GetValue("document_PK"))) ? (int?)Convert.ToInt32(e.GetValue("document_PK")) : null;

            string versionLabel = Convert.ToString(e.GetValue("VersionLabel"));
            versionLabel = !string.IsNullOrWhiteSpace(versionLabel) ? versionLabel : string.Empty;
            var pnlStatusColor = e.FindControl("pnlStatusColor") as HtmlGenericControl;
            if (pnlStatusColor != null)
            {
                SetGridStatusColor(pnlStatusColor, versionLabel.Trim().ToUpper());
            }

            if (documentPk.HasValue)
            {
                var pnlAttachments = e.FindControl("pnlAttachments") as Panel;
                if (pnlAttachments != null)
                {
                    BindAttachents(pnlAttachments, documentPk.Value);
                }

                var pnlRelatedEntities = e.FindControl("pnlRelatedEntities") as Panel;
                if (pnlRelatedEntities != null)
                {
                    BindRelatedEntities(pnlRelatedEntities, documentPk.Value);
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
        }

        void DocumentGridAllOnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void DocumentGridAllOnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!DocumentGridAll.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = DocumentGridAll.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (DocumentGridAll.SortCount > 1 && e.FieldName == DocumentGridAll.MainSortingColumn) return;

            var leftAlignedColumns = new List<string> { "DocumentName", "Attachments", "DocumentType", "RelatedEntities", "VersionNumber", "VersionLabel", "RegulatoryStatus", "DocumentNumber", "LanguageCode", "ResponsibleUser" };
            if (leftAlignedColumns.Contains(e.FieldName)) e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            else e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";

        }

        void DocumentGridAllOnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
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

            txtSrProduct.Text = product.name;
            txtSrProduct.SelectedEntityId = product.product_PK;
        }

        #endregion

        #region Authorised product searcher

        /// <summary>
        /// Handles authorised product list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AuthorisedProductSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var authorisedProduct = _authorisedProductOperations.GetEntity(e.Data);

            if (authorisedProduct == null || authorisedProduct.ap_PK == null) return;

            txtSrAuthorisedProduct.Text = authorisedProduct.product_name;
            txtSrAuthorisedProduct.SelectedEntityId = authorisedProduct.ap_PK;
        }

        #endregion

        #region Pharmaceutical product searcher

        /// <summary>
        /// Handles pharmaceutical product list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PharmaceuticalProductSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(e.Data);

            if (pharmaceuticalProduct == null || pharmaceuticalProduct.pharmaceutical_product_PK == null) return;

            txtSrPharmaceuticalProduct.Text = pharmaceuticalProduct.name;
            txtSrPharmaceuticalProduct.SelectedEntityId = pharmaceuticalProduct.pharmaceutical_product_PK;
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

        #region Activity searcher

        /// <summary>
        /// Handles activity list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ActivitySearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var actvivity = _activityOperations.GetEntity(e.Data);

            if (actvivity == null || actvivity.activity_PK == null) return;

            txtSrActivity.Text = actvivity.name;
            txtSrActivity.SelectedEntityId = actvivity.activity_PK;
        }

        #endregion

        #region Quick links

        public void btnSaveSearchClick(object sender, EventArgs e)
        {
            QuickLink quickLink = null;
            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                var savedSearch = _savedDocumentSearch.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
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
            if (!_idSearch.HasValue) return;

            _savedDocumentSearch.Delete(_idSearch);
            Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}&Action=Search", EntityContext.Document));
        }

        protected void QuickLinksPopup_OnOkButtonClick(object sender, FormEventArgs<QuickLink> e)
        {
            Document_saved_search_PK savedDocumentSearch = null;

            if (_idSearch.HasValue)
            {
                savedDocumentSearch = _savedDocumentSearch.GetEntity(_idSearch);
            }

            if (savedDocumentSearch == null)
            {
                savedDocumentSearch = new Document_saved_search_PK();
            }

            var changeDateFrom = ValidationHelper.IsValidDateTime(dtRngChangeDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngChangeDate.TextFrom, CultureInfoHr) : null;
            var changeDateTo = ValidationHelper.IsValidDateTime(dtRngChangeDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngChangeDate.TextTo, CultureInfoHr) : null;
            var effectiveStartDateFrom = ValidationHelper.IsValidDateTime(dtRngEffectiveStartDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngEffectiveStartDate.TextFrom, CultureInfoHr) : null;
            var effectiveStartDateTo = ValidationHelper.IsValidDateTime(dtRngEffectiveStartDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngEffectiveStartDate.TextTo, CultureInfoHr) : null;
            var effectiveEndDateFrom = ValidationHelper.IsValidDateTime(dtRngEffectiveEndDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngEffectiveEndDate.TextFrom, CultureInfoHr) : null;
            var effectiveEndDateTo = ValidationHelper.IsValidDateTime(dtRngEffectiveEndDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngEffectiveEndDate.TextTo, CultureInfoHr) : null;
            var versionDateFrom = ValidationHelper.IsValidDateTime(dtRngVersionDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngVersionDate.TextFrom, CultureInfoHr) : null;
            var versionDateTo = ValidationHelper.IsValidDateTime(dtRngVersionDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngVersionDate.TextTo, CultureInfoHr) : null;

            savedDocumentSearch.product_FK = txtSrProduct.SelectedEntityId;
            savedDocumentSearch.ap_FK = txtSrAuthorisedProduct.SelectedEntityId;
            savedDocumentSearch.pp_FK = txtSrPharmaceuticalProduct.SelectedEntityId;
            savedDocumentSearch.project_FK = txtSrProject.SelectedEntityId;
            savedDocumentSearch.activity_FK = txtSrActivity.SelectedEntityId;
            savedDocumentSearch.task_FK = txtSrTask.SelectedEntityId;
            savedDocumentSearch.ev_code = txtEvCode.Text;
            savedDocumentSearch.name = txtDocumentName.Text;
            savedDocumentSearch.type_FK = ddlDocumentType.SelectedId;
            savedDocumentSearch.version_number = ValidationHelper.IsValidInt(txtVersionNumber.Text) ? (int?)Convert.ToInt32(txtVersionNumber.Text) : null;
            savedDocumentSearch.person_FK = ddlResponsibleUser.SelectedId;
            savedDocumentSearch.version_label = ddlVersionLabel.SelectedId;
            savedDocumentSearch.document_number = txtDocumentNumber.Text;
            savedDocumentSearch.regulatory_status = ddlRegulatoryStatus.SelectedId;
            savedDocumentSearch.language_code = txtLanguageCode.Text;
            savedDocumentSearch.comments = txtComments.Text;
            savedDocumentSearch.change_date_from = changeDateFrom;
            savedDocumentSearch.change_date_to = changeDateTo;
            savedDocumentSearch.effective_start_date_from = effectiveStartDateFrom;
            savedDocumentSearch.effective_start_date_to = effectiveStartDateTo;
            savedDocumentSearch.effective_end_date_from = effectiveEndDateFrom;
            savedDocumentSearch.effective_end_date_to = effectiveEndDateTo;
            savedDocumentSearch.version_date_from = versionDateFrom;
            savedDocumentSearch.version_date_to = versionDateTo;
            savedDocumentSearch.gridLayout = _gridLayout;

            var quickLink = e.Data;
            if (quickLink != null)
            {
                savedDocumentSearch.displayName = quickLink.Name;
                savedDocumentSearch.isPublic = quickLink.IsPublic.HasValue && quickLink.IsPublic.Value;
            }

            var user = _userOperations.GetUserByUsername(SessionManager.Instance.CurrentUser.Username);
            if (user != null)
            {
                savedDocumentSearch.user_FK1 = user.Person_FK;
            }

            savedDocumentSearch = _savedDocumentSearch.Save(savedDocumentSearch);
            Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}&Action=Search&idSearch={1}", EntityContext.Document, savedDocumentSearch.document_saved_search_PK));
        }

        #endregion

        #endregion

        #region Support methods

        private void ClearSearch()
        {
            txtSrProduct.Text = string.Empty;
            txtSrAuthorisedProduct.Text = string.Empty;
            txtSrPharmaceuticalProduct.Text = string.Empty;
            txtSrProject.Text = string.Empty;
            txtSrActivity.Text = string.Empty;
            txtSrTask.Text = string.Empty;
            txtEvCode.Text = string.Empty;
            txtDocumentName.Text = string.Empty;
            txtTextSearch.Text = string.Empty;
            ddlDocumentType.SelectedValue = string.Empty;
            txtVersionNumber.Text = string.Empty;
            ddlResponsibleUser.SelectedValue = string.Empty;
            ddlVersionLabel.SelectedValue = string.Empty;
            txtDocumentNumber.Text = string.Empty;
            ddlRegulatoryStatus.SelectedValue = string.Empty;
            dtRngChangeDate.Clear();
            dtRngEffectiveStartDate.Clear();
            dtRngEffectiveEndDate.Clear();
            dtRngVersionDate.Clear();
        }

        private Dictionary<string, string> GetFilters()
        {
            var filters = DocumentGridAll.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (_idSearch.HasValue)
                    {
                        Document_saved_search_PK savedDocumentSearch = _savedDocumentSearch.GetEntity(_idSearch);
                        FillFilters(savedDocumentSearch, filters);
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
            if (txtSrProduct.SelectedEntityId.HasValue) filters.Add("SearchProductPk", txtSrProduct.SelectedEntityId.Value.ToString());
            if (txtSrAuthorisedProduct.SelectedEntityId.HasValue) filters.Add("SearchAuthorisedProductPk", txtSrAuthorisedProduct.SelectedEntityId.Value.ToString());
            if (txtSrPharmaceuticalProduct.SelectedEntityId.HasValue) filters.Add("SearchPharmaceuticalProductPk", txtSrPharmaceuticalProduct.SelectedEntityId.Value.ToString());
            if (txtSrProject.SelectedEntityId.HasValue) filters.Add("SearchProjectPk", txtSrProject.SelectedEntityId.Value.ToString());
            if (txtSrActivity.SelectedEntityId.HasValue) filters.Add("SearchActivityPk", txtSrActivity.SelectedEntityId.Value.ToString());
            if (txtSrTask.SelectedEntityId.HasValue) filters.Add("SearchTaskPk", txtSrTask.SelectedEntityId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtEvCode.Text)) filters.Add("SearchEvCodeName", txtEvCode.Text);
            if (!string.IsNullOrWhiteSpace(txtDocumentName.Text)) filters.Add("SearchDocumentName", txtDocumentName.Text);
            if (!string.IsNullOrWhiteSpace(txtTextSearch.Text)) filters.Add("SearchTextSearch", txtTextSearch.Text);
            if (ddlDocumentType.SelectedId.HasValue) filters.Add("SearchDocumentTypePk", ddlDocumentType.SelectedId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtVersionNumber.Text)) filters.Add("SearchVersionNumber", txtVersionNumber.Text);
            if (ddlResponsibleUser.SelectedId.HasValue) filters.Add("SearchResponsibleUserPk", ddlResponsibleUser.SelectedId.Value.ToString());
            if (ddlVersionLabel.SelectedId.HasValue) filters.Add("SearchVersionLabelPk", ddlVersionLabel.SelectedId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtDocumentNumber.Text)) filters.Add("SearchDocumentNumber", txtDocumentNumber.Text);
            if (ddlRegulatoryStatus.SelectedId.HasValue) filters.Add("SearchRegulatoryStatusPk", ddlRegulatoryStatus.SelectedId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtLanguageCode.Text)) filters.Add("SearchLanguageCode", txtLanguageCode.Text);
            if (!string.IsNullOrWhiteSpace(txtComments.Text)) filters.Add("SearchComments", txtComments.Text);
            if (ValidationHelper.IsValidDateTime(dtRngChangeDate.TextFrom, CultureInfoHr)) filters.Add("SearchChangeDateFrom", dtRngChangeDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngChangeDate.TextTo, CultureInfoHr)) filters.Add("SearchChangeDateTo", dtRngChangeDate.TextTo);
            if (ValidationHelper.IsValidDateTime(dtRngEffectiveStartDate.TextFrom, CultureInfoHr)) filters.Add("SearchEffectiveStartDateFrom", dtRngEffectiveStartDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngEffectiveStartDate.TextTo, CultureInfoHr)) filters.Add("SearchEffectiveStartDateTo", dtRngEffectiveStartDate.TextTo);
            if (ValidationHelper.IsValidDateTime(dtRngEffectiveEndDate.TextFrom, CultureInfoHr)) filters.Add("SearchEffectiveEndDateFrom", dtRngEffectiveEndDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngEffectiveEndDate.TextTo, CultureInfoHr)) filters.Add("SearchEffectiveEndDateTo", dtRngEffectiveEndDate.TextTo);
            if (ValidationHelper.IsValidDateTime(dtRngVersionDate.TextFrom, CultureInfoHr)) filters.Add("SearchVersionDateFrom", dtRngVersionDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngVersionDate.TextTo, CultureInfoHr)) filters.Add("SearchVersionDateTo", dtRngVersionDate.TextTo);
        }

        private void FillFilters(Document_saved_search_PK savedDocumentSearch, Dictionary<string, string> filters)
        {
            if (savedDocumentSearch.product_FK.HasValue) filters.Add("SearchProductPk", savedDocumentSearch.product_FK.Value.ToString());
            if (savedDocumentSearch.ap_FK.HasValue) filters.Add("SearchAuthorisedProductPk", savedDocumentSearch.ap_FK.Value.ToString());
            if (savedDocumentSearch.pp_FK.HasValue) filters.Add("SearchPharmaceuticalProductPk", savedDocumentSearch.pp_FK.Value.ToString());
            if (savedDocumentSearch.product_FK.HasValue) filters.Add("SearchProjectPk", savedDocumentSearch.product_FK.Value.ToString());
            if (savedDocumentSearch.activity_FK.HasValue) filters.Add("SearchActivityPk", savedDocumentSearch.activity_FK.Value.ToString());
            if (savedDocumentSearch.task_FK.HasValue) filters.Add("SearchTaskPk", savedDocumentSearch.task_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedDocumentSearch.ev_code)) filters.Add("SearchEvCodeName", savedDocumentSearch.ev_code);
            if (!string.IsNullOrWhiteSpace(savedDocumentSearch.name)) filters.Add("SearchDocumentName", savedDocumentSearch.name);
            if (!string.IsNullOrWhiteSpace(savedDocumentSearch.content)) filters.Add("SearchTextSearch", savedDocumentSearch.content);
            if (savedDocumentSearch.type_FK.HasValue) filters.Add("SearchDocumentTypePk", savedDocumentSearch.type_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(Convert.ToString(savedDocumentSearch.version_number))) filters.Add("SearchVersionNumber", Convert.ToString(savedDocumentSearch.version_number));
            if (savedDocumentSearch.person_FK.HasValue) filters.Add("SearchResponsibleUserPk", savedDocumentSearch.person_FK.Value.ToString());
            if (savedDocumentSearch.version_label.HasValue) filters.Add("SearchVersionLabelPk", savedDocumentSearch.version_label.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedDocumentSearch.document_number)) filters.Add("SearchDocumentNumber", savedDocumentSearch.document_number);
            if (savedDocumentSearch.regulatory_status.HasValue) filters.Add("SearchRegulatoryStatusPk", savedDocumentSearch.regulatory_status.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedDocumentSearch.language_code)) filters.Add("SearchLanguageCode", savedDocumentSearch.language_code);
            if (!string.IsNullOrWhiteSpace(savedDocumentSearch.comments)) filters.Add("SearchComments", savedDocumentSearch.comments);
            if (savedDocumentSearch.change_date_from.HasValue) filters.Add("SearchChangeDateFrom", savedDocumentSearch.change_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedDocumentSearch.change_date_to.HasValue) filters.Add("SearchChangeDateTo", savedDocumentSearch.change_date_to.Value.ToString(Constant.DateTimeFormat));
            if (savedDocumentSearch.effective_start_date_from.HasValue) filters.Add("SearchEffectiveStartDateFrom", savedDocumentSearch.effective_start_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedDocumentSearch.effective_start_date_to.HasValue) filters.Add("SearchEffectiveStartDateTo", savedDocumentSearch.effective_start_date_to.Value.ToString(Constant.DateTimeFormat));
            if (savedDocumentSearch.effective_end_date_from.HasValue) filters.Add("SearchEffectiveEndDateFrom", savedDocumentSearch.effective_end_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedDocumentSearch.effective_end_date_to.HasValue) filters.Add("SearchEffectiveEndDateTo", savedDocumentSearch.effective_end_date_to.Value.ToString(Constant.DateTimeFormat));
            if (savedDocumentSearch.version_date_from.HasValue) filters.Add("SearchVersionDateFrom", savedDocumentSearch.version_date_from.Value.ToString(Constant.DateTimeFormat));
            if (savedDocumentSearch.version_date_to.HasValue) filters.Add("SearchVersionDateTo", savedDocumentSearch.version_date_to.Value.ToString(Constant.DateTimeFormat));
        }

        private void GenerateContextMenuItems()
        {
            var contextMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);
            }
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location = null;

            if (ListType == ListType.List)
            {
                location = Support.LocationManager.Instance.GetLocationByName("Doc", Support.CacheManager.Instance.AppLocations);
                tabMenu.Visible = false;
                if (location != null)
                {
                    MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                    MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                }
            }
            else if (ListType == ListType.Search)
            {
                location = Support.LocationManager.Instance.GetLocationByName("DocSearch", Support.CacheManager.Instance.AppLocations);
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
                location = Support.LocationManager.Instance.GetLocationByName("Doc", Support.CacheManager.Instance.AppLocations);
            }
            else if (ListType == ListType.Search)
            {
                location = Support.LocationManager.Instance.GetLocationByName("DocSearch", Support.CacheManager.Instance.AppLocations);
            }
            if (location != null)
            {
                var topLevelParent = MasterPage.FindTopLevelParent(location);

                MasterPage.CurrentLocation = location;
                MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
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

        private void SetGridStatusColor(HtmlGenericControl pnlStatusColor, string versionLabel)
        {
            switch (versionLabel)
            {
                case "EFFECTIVE":
                    pnlStatusColor.Attributes.Add("class", "statusGreen");
                    break;
                case "OBSOLETE":
                    pnlStatusColor.Attributes.Add("class", "statusBlack");
                    break;
                case "APPROVED":
                    pnlStatusColor.Attributes.Add("class", "statusYellow");
                    break;
                case "DRAFT":
                    pnlStatusColor.Attributes.Add("class", "statusGray");
                    break;
                default:
                    pnlStatusColor.Attributes.Add("class", "statusBlack");
                    break;
            }
        }

        private void BindAttachents(Panel pnlAttachment, int documentPk)
        {
            List<Attachment_PK> attachments = _attachmentOperations.GetAttachmentsForDocument(documentPk);

            foreach (var item in attachments)
            {
                var hl = new HyperLink();

                var rootLocation = Support.LocationManager.Instance.GetLocationByName("Root", Support.CacheManager.Instance.AppLocations);
                if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation)) hl.NavigateUrl = "~/Views/Business/FileDownload.ashx?attachID=" + item.attachment_PK;

                hl.Text = item.attachmentname;
                pnlAttachment.Controls.Add(hl);
                pnlAttachment.Controls.Add(new LiteralControl("<br />"));
            }
        }

        private void BindRelatedEntities(Panel pnlRelatedEntities, int documentPk)
        {
            DataSet relatedTo = _documentOperations.GetDocumentRelatedEntities(documentPk);
            pnlRelatedEntities.Controls.Clear();

            if (relatedTo == null || relatedTo.Tables.Count == 0) return;
            if (relatedTo.Tables[0].Rows.Count == 0 || !relatedTo.Tables[0].Columns.Contains("FullURL") || !relatedTo.Tables[0].Columns.Contains("Name")) return;

            foreach (DataRow item in relatedTo.Tables[0].Rows)
            {
                if (item["FullURL"] is DBNull || item["Name"] is DBNull ||
                    string.IsNullOrWhiteSpace((string)item["FullURL"]) || 
                    string.IsNullOrWhiteSpace((string)item["Name"])) continue;

                var hl = new HyperLink
                             {
                                 NavigateUrl = "~/" + item["FullURL"],
                                 Text = Convert.ToString(item["Name"])
                             };
                pnlRelatedEntities.Controls.Add(hl);
                pnlRelatedEntities.Controls.Add(new LiteralControl("<br />"));
            }
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            return true;
        }

        #endregion
    }
}