using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace AspNetUI.Views.DocumentView
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
        private int? _idProd;
        private int? _idAuthProd;
        private int? _idProj;
        private int? _idTask;
        private int? _idPharmProd;
        private int? _idDoc;
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
            //GenerateTopMenuItems();
            MasterPage.UpTopMenu.Update();
        }

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();
            LoadActionQuery();

            _idDoc = ValidationHelper.IsValidInt(Request.QueryString["idDoc"]) ? int.Parse(Request.QueryString["idDoc"]) : (int?)null;
            _idSearch = ValidationHelper.IsValidInt(Request.QueryString["idSearch"]) ? int.Parse(Request.QueryString["idSearch"]) : (int?)null;
            _idAuthProd = ValidationHelper.IsValidInt(Request.QueryString["idAuthProd"]) ? int.Parse(Request.QueryString["idAuthProd"]) : (int?)null;
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;
            _idPharmProd = ValidationHelper.IsValidInt(Request.QueryString["idPharmProd"]) ? int.Parse(Request.QueryString["idPharmProd"]) : (int?)null;
            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;
            _idTask = ValidationHelper.IsValidInt(Request.QueryString["idTask"]) ? int.Parse(Request.QueryString["idTask"]) : (int?)null;
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
                DocumentGrid.GridVersion = DocumentGrid.GridVersion + ListType.ToString();
            }
            else if (EntityContext != EntityContext.Default)
            {
                DocumentGrid.GridVersion = DocumentGrid.GridVersion + EntityContext.ToString();
            }

            _gridId = DocumentGrid.GridId + "_" + DocumentGrid.GridVersion;
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
                    subtabs.Controls.Clear();
                    break;
            }

            DocumentGrid.OnRebindRequired += DocumentGridOnRebindRequired;
            DocumentGrid.OnHtmlRowPrepared += DocumentGridOnHtmlRowPrepared;
            DocumentGrid.OnHtmlCellPrepared += DocumentGridOnHtmlCellPrepared;
            DocumentGrid.OnExcelCellPrepared += DocumentGridOnExcelCellPrepared;
            DocumentGrid.OnLoadClientLayout += DocumentGridOnLoadClientLayout;
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
            }
        }

        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    if (EntityContext == EntityContext.Product) HandleEntityContextProduct();
                    else if (EntityContext == EntityContext.AuthorisedProduct) HandleEntityContextAuthorisedProduct();
                    else if (EntityContext == EntityContext.Product) HandleEntityContextProduct();
                    else if (EntityContext == EntityContext.PharmaceuticalProduct) HandleEntityContextPharmaceuticalProduct();
                    else if (EntityContext == EntityContext.Project) HandleEntityContextProject();
                    else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) HandleEntityContextActivity();
                    else if (EntityContext == EntityContext.Task) HandleEntityContextTask();

                    HideSearch();

                    break;
                case ListType.Search:
                    if (_idSearch.HasValue) ShowAll();
                    else ShowSearch();
                    btnSaveLayout.Visible = false;
                    btnClearLayout.Visible = false;
                    btnColumns.Visible = false;

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
                DocumentGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (DocumentGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(DocumentGrid.SecondSortingColumn, DocumentGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (DocumentGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(DocumentGrid.MainSortingColumn, DocumentGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("document_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _documentOperations.GetListFormSearchDataSet(filters, DocumentGrid.CurrentPage, DocumentGrid.PageSize, gobList, out itemCount);
                else ds = _documentOperations.GetListFormDataSet(filters, DocumentGrid.CurrentPage, DocumentGrid.PageSize, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _documentOperations.GetListFormSearchDataSet(filters, DocumentGrid.CurrentPage, DocumentGrid.PageSize, gobList, out itemCount);
            }

            DocumentGrid.TotalRecords = itemCount;

            if (DocumentGrid.CurrentPage > DocumentGrid.TotalPages || (DocumentGrid.CurrentPage == 0 && DocumentGrid.TotalPages > 0))
            {
                if (DocumentGrid.CurrentPage > DocumentGrid.TotalPages) DocumentGrid.CurrentPage = DocumentGrid.TotalPages; else DocumentGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    if (_idSearch.HasValue) ds = _documentOperations.GetListFormSearchDataSet(filters, DocumentGrid.CurrentPage, DocumentGrid.PageSize, gobList, out itemCount);
                    else ds = _documentOperations.GetListFormDataSet(filters, DocumentGrid.CurrentPage, DocumentGrid.PageSize, gobList, out itemCount);
                }
                else if (ListType == ListType.Search)
                {
                    ds = _documentOperations.GetListFormSearchDataSet(filters, DocumentGrid.CurrentPage, DocumentGrid.PageSize, gobList, out itemCount);
                }
            }

            DocumentGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            DocumentGrid.DataBind();

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
                case ContextMenuEventTypes.New:
                    {
                        MasterPage.OneTimePermissionToken = Permission.View;
                        if (EntityContext == EntityContext.Product && _idProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idProd={1}&From=ProdDocList", EntityContext, _idProd));
                        else if (EntityContext == EntityContext.AuthorisedProduct && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idAuthProd={1}&From=AuthProdDocList", EntityContext, _idAuthProd));
                        else if (EntityContext == EntityContext.PharmaceuticalProduct && _idPharmProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idPharmProd={1}&From=PharmProdDocList", EntityContext, _idPharmProd));
                        else if (EntityContext == EntityContext.Project && _idProj.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idProj={1}&From=ProjDocList", EntityContext, _idProj));
                        else if (EntityContext == EntityContext.Activity && _idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idAct={1}&From=ActDocList", EntityContext, _idAct));
                        else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idAct={1}&From=ActMyDocList", EntityContext, _idAct));
                        else if (EntityContext == EntityContext.Task && _idTask.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idTask={1}&From=TaskDocList", EntityContext, _idTask));

                        Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}&From=DocList", EntityContext.Document));
                    }
                    break;
                case ContextMenuEventTypes.Back:
                    {
                        if (EntityContext == EntityContext.Document) Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.AuthorisedProduct) Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.PharmaceuticalProduct) Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Project) Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Task) Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext));

                        Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}", EntityContext.Document));
                    }
                    break;
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in DocumentGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("document_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (DocumentGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            DocumentGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            DocumentGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            
            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = DocumentGrid.GetClientLayoutString(),
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
            if (DocumentGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(DocumentGrid.SecondSortingColumn, DocumentGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (DocumentGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(DocumentGrid.MainSortingColumn, DocumentGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("document_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _documentOperations.GetListFormSearchDataSet(filters, DocumentGrid.CurrentPage, DocumentGrid.PageSize, gobList, out itemCount); // Quick link
                else ds = _documentOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _documentOperations.GetListFormSearchDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            DocumentGrid["document_PK"].Visible = true;
            if (ds != null) DocumentGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            DocumentGrid["document_PK"].Visible = false;

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            DocumentGrid.ResetVisibleColumns();
            DocumentGrid.SecondSortingColumn = null;
            DocumentGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
            BindGrid();
        }

        #endregion

        #region Grid

        void DocumentGridOnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {
            if (args.RowType != DataControlRowType.DataRow) return;
        }

        void DocumentGridOnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
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

                var hlDocumentName = e.FindControl("hlDocumentName") as HyperLink;
                if (hlDocumentName != null)
                {
                    BindDocumentLink(hlDocumentName, documentPk);
                }
            }
        }

        private void BindDocumentLink(HyperLink hlDocumentName, int? documentPk)
        {
            if (!documentPk.HasValue) return;
            switch (EntityContext)
            {
                case EntityContext.AuthorisedProduct:
                    if (_idAuthProd.HasValue) hlDocumentName.NavigateUrl = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idAuthProd={2}", EntityContext, documentPk, _idAuthProd);
                    break;
                case EntityContext.Product:
                    if (_idProd.HasValue) hlDocumentName.NavigateUrl = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idProd={2}", EntityContext, documentPk, _idProd);
                    break;
                case EntityContext.Project:
                    if (_idProj.HasValue) hlDocumentName.NavigateUrl = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idProj={2}", EntityContext, documentPk, _idProj);
                    break;
                case EntityContext.Activity:
                    if (_idAct.HasValue) hlDocumentName.NavigateUrl = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idAct={2}", EntityContext, documentPk, _idAct);
                    break;
                case EntityContext.ActivityMy:
                    if (_idAct.HasValue) hlDocumentName.NavigateUrl = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idAct={2}", EntityContext, documentPk, _idAct);
                    break;
                case EntityContext.Task:
                    if (_idTask.HasValue) hlDocumentName.NavigateUrl = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idTask={2}", EntityContext, documentPk, _idTask);
                    break;
                case EntityContext.PharmaceuticalProduct:
                    if (_idPharmProd.HasValue) hlDocumentName.NavigateUrl = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idPharmProd={2}", EntityContext, documentPk, _idPharmProd);
                    break;
                default:
                    hlDocumentName.NavigateUrl = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}", EntityContext.Default, documentPk);
                    break;
            }
        }

        void DocumentGridOnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void DocumentGridOnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!DocumentGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = DocumentGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (DocumentGrid.SortCount > 1 && e.FieldName == DocumentGrid.MainSortingColumn) return;

            var leftAlignedColumns = new List<string> { "DocumentName", "Attachments", "DocumentType", "RelatedEntities", "VersionNumber", "VersionLabel", "RegulatoryStatus", "DocumentNumber", "LanguageCode", "ResponsibleUser" };
            if (leftAlignedColumns.Contains(e.FieldName)) e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            else e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";

        }

        void DocumentGridOnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {
            var userGridSettings = MasterPage != null ? _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, MasterPage.CurrentLocation.display_name) : null;
            if (userGridSettings != null && !string.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                args.ClientLayoutString = userGridSettings.grid_layout;
            }
        }

        #endregion

        #endregion

        #region Support methods

        private void ClearSearch()
        {
           
        }

        private Dictionary<string, string> GetFilters()
        {
            var filters = DocumentGrid.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (EntityContext == EntityContext.Product || EntityContext == EntityContext.AuthorisedProduct || EntityContext == EntityContext.PharmaceuticalProduct ||
                        EntityContext == EntityContext.Project || EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy || EntityContext == EntityContext.Task)
                    {
                        filters.Add("QueryBy", EntityContext.ToString());
                        if (EntityContext == EntityContext.Product) filters.Add("EntityPk", Convert.ToString(_idProd));
                        else if (EntityContext == EntityContext.AuthorisedProduct) filters.Add("EntityPk", Convert.ToString(_idAuthProd));
                        else if (EntityContext == EntityContext.PharmaceuticalProduct) filters.Add("EntityPk", Convert.ToString(_idPharmProd));
                        else if (EntityContext == EntityContext.Project) filters.Add("EntityPk", Convert.ToString(_idProj));
                        else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) filters.Add("EntityPk", Convert.ToString(_idAct));
                        else if (EntityContext == EntityContext.Task) filters.Add("EntityPk", Convert.ToString(_idTask));
                    }
                    break;
            }

            return filters;
        }

        private void GenerateContextMenuItems()
        {
            var contexMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                switch (EntityContext)
                {
                    case EntityContext.Default:
                        contexMenu = new[] {  
                                                new ContextMenuItem(ContextMenuEventTypes.New, "New") 
                                           };
                        break;
                    case EntityContext.Product:
                    case EntityContext.AuthorisedProduct:
                    case EntityContext.PharmaceuticalProduct:
                    case EntityContext.Activity:
                    case EntityContext.ActivityMy:
                    case EntityContext.Project:
                    case EntityContext.Task:
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
                    case EntityContext.Default:
                        location = Support.LocationManager.Instance.GetLocationByName("Doc", Support.CacheManager.Instance.AppLocations);

                        MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                        MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                        tabMenu.Visible = false;
                        break;
                    case EntityContext.Product:
                    case EntityContext.AuthorisedProduct:
                    case EntityContext.PharmaceuticalProduct:
                    case EntityContext.Activity:
                    case EntityContext.ActivityMy:
                    case EntityContext.Project:
                    case EntityContext.Task:
                        switch (EntityContext)
                        {
                            case EntityContext.Product:
                                location = Support.LocationManager.Instance.GetLocationByName("ProdDocList", Support.CacheManager.Instance.AppLocations);
                                break;
                            case EntityContext.AuthorisedProduct:
                                location = Support.LocationManager.Instance.GetLocationByName("AuthProdDocList", Support.CacheManager.Instance.AppLocations);
                                break;
                            case EntityContext.PharmaceuticalProduct:
                                location = Support.LocationManager.Instance.GetLocationByName("PharmProdDocList", Support.CacheManager.Instance.AppLocations);
                                break;
                            case EntityContext.Project:
                                location = Support.LocationManager.Instance.GetLocationByName("ProjDocList", Support.CacheManager.Instance.AppLocations);
                                break;
                            case EntityContext.Activity:
                                location = Support.LocationManager.Instance.GetLocationByName("ActDocList", Support.CacheManager.Instance.AppLocations);
                                break;
                            case EntityContext.ActivityMy:
                                location = Support.LocationManager.Instance.GetLocationByName("ActMyDocList", Support.CacheManager.Instance.AppLocations);
                                break;
                            case EntityContext.Task:
                                location = Support.LocationManager.Instance.GetLocationByName("TaskDocList", Support.CacheManager.Instance.AppLocations);
                                break;   
                        }
                        MasterPage.TabMenu.TabControls.Clear();
                        tabMenu.Visible = true;
                        tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                        tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                        break;
                }
            }
            else if (ListType == ListType.Search)
            {
                location = Support.LocationManager.Instance.GetLocationByName("DocListSearch", Support.CacheManager.Instance.AppLocations);
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            //Location_PK location = null;

            //if (ListType == ListType.List)
            //{
            //    location = Support.LocationManager.Instance.GetLocationByName("Doc", Support.CacheManager.Instance.AppLocations);
            //}
            //else if (ListType == ListType.Search)
            //{
            //    location = Support.LocationManager.Instance.GetLocationByName("DocListSearch", Support.CacheManager.Instance.AppLocations);
            //}

            //var topLevelParent = MasterPage.FindTopLevelParent(location);

            //MasterPage.CurrentLocation = location;
            //MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
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

        private void SetGridRowColor(DateTime? expectedFinishedDate, string internalStatus, PossGrid.PossGridRowEventArgs e)
        {
            var controls = new[] { "hlID", "hlTaskCount", "hlNewTask", "hlTimeCount", "hlNewTime", "hlDocumentsCount", "hlNewDocuments" };

            if (expectedFinishedDate.HasValue && expectedFinishedDate <= DateTime.Now && internalStatus != "finished")
            {
                foreach (var item in controls)
                {
                    var control = e.FindControl(item) as WebControl;
                    if (control != null)
                    {
                        control.ForeColor = System.Drawing.ColorTranslator.FromHtml("#ff0000");
                    }
                }

                e.Row.CssClass = e.Row.DataItemIndex % 2 == 0 ? "dxgvDataRow_readyRed" : "dxgvDataRowAlt_readyRed";

                isRed = true;
            }
            else
            {
                isRed = false;
            }
        }

        private void BindAttachents(Panel pnlAttachment, int documentPk)
        {
            List<Attachment_PK> attachments = _attachmentOperations.GetAttachmentsForDocument(documentPk);

            foreach (var item in attachments)
            {
                var hl = new HyperLink();

                var rootLocation = Support.LocationManager.Instance.GetLocationByName("Root", Support.CacheManager.Instance.AppLocations);
                if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation))
                {
                    if (string.IsNullOrWhiteSpace(item.EDMSDocumentId)) hl.NavigateUrl = string.Format("~/Views/Business/FileDownload.ashx?attachID={0}", item.attachment_PK);
                    else hl.NavigateUrl = string.Format("~/Views/Business/FileDownload.ashx?attachID={0}|{{{1};{2};{3}}}", item.attachment_PK, item.EDMSDocumentId, item.EDMSBindingRule, item.EDMSAttachmentFormat);
                }

                hl.Text = item.attachmentname;
                pnlAttachment.Controls.Add(hl);
                pnlAttachment.Controls.Add(new LiteralControl("<br />"));
            }
        }

        private void HandleEntityContextProduct()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Product:";

            var product = _productOperations.GetEntity(_idProd);

            lblPrvParentEntity.Text = product != null && !string.IsNullOrWhiteSpace(product.name) ? product.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextAuthorisedProduct()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Authorised product:";

            var authorisedProduct = _authorisedProductOperations.GetEntity(_idAuthProd);

            lblPrvParentEntity.Text = authorisedProduct != null && !string.IsNullOrWhiteSpace(authorisedProduct.product_name) ? authorisedProduct.product_name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextPharmaceuticalProduct()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Pharmaceutical product:";

            var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(_idPharmProd);

            lblPrvParentEntity.Text = pharmaceuticalProduct != null && !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextProject()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Project:";

            var project = _projectOperations.GetEntity(_idProj);

            lblPrvParentEntity.Text = project != null && !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextActivity()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Activity:";

            var activity = _activityOperations.GetEntity(_idAct);

            lblPrvParentEntity.Text = activity != null && !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.ControlDefault.LbPrvText;
        }

        private void HandleEntityContextTask()
        {
            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Task:";

            var task = _taskOperations.GetEntity(_idTask);

            if (task == null) return;

            var taskName = _taskNameOperations.GetEntity(task.task_name_FK);

            lblPrvParentEntity.Text = taskName != null && !string.IsNullOrWhiteSpace(taskName.task_name) ? taskName.task_name : Constant.ControlDefault.LbPrvText;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            if (SecurityHelper.IsPermitted(Permission.InsertDocument)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
           
            return true;
        }

        #endregion
    }
}