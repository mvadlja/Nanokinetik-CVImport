using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUIFramework;
using GEM2Common;
using PossGrid;
using Ready.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.ProductView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;
        private int? _idTimeUnit;
        private int? _idSearch;
        private string _gridId;

        private IProduct_PKOperations _productOperations;
        private ITime_unit_PKOperations _timeUnitOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IUSEROperations _userOperations;
        private IType_PKOperations _typeOperations;
        private IOrganization_PKOperations _organizationOperations;
        private ICountry_PKOperations _countryOperations;
        private IDomain_PKOperations _domainOperations;
        private IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        private ISubstance_PKOperations _substanceOperations;
        private IPerson_PKOperations _personOperations;
        private IProduct_saved_search_PKOperations _savedProductSearch;

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

            _idTimeUnit = ValidationHelper.IsValidInt(Request.QueryString["idTimeUnit"]) ? int.Parse(Request.QueryString["idTimeUnit"]) : (int?)null;
            _idSearch = ValidationHelper.IsValidInt(Request.QueryString["idSearch"]) ? int.Parse(Request.QueryString["idSearch"]) : (int?)null;

            _productOperations = new Product_PKDAL();
            _timeUnitOperations = new Time_unit_PKDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();
            _typeOperations = new Type_PKDAL();
            _organizationOperations = new Organization_PKDAL();
            _countryOperations = new Country_PKDAL();
            _domainOperations = new Domain_PKDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _substanceOperations = new Substance_PKDAL();
            _personOperations = new Person_PKDAL();
            _savedProductSearch = new Product_saved_search_PKDAL();

            if (ListType == ListType.Search)
            {
                ProductGrid.GridVersion = ProductGrid.GridVersion + ListType.ToString();
            }
            else if (EntityContext != EntityContext.Default)
            {
                ProductGrid.GridVersion = ProductGrid.GridVersion + EntityContext.ToString();
            }

            _gridId = ProductGrid.GridId + "_" + ProductGrid.GridVersion;
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
                    txtSrPharmaceuticalProducts.Searcher.OnListItemSelected += PharmaceuticalProductsSearcher_OnListItemSelected;
                    txtSrManufacturers.Searcher.OnListItemSelected += ManufacturersSearcher_OnListItemSelected;
                    lbSrActiveSubstances.Searcher.OnOkButtonClick += LbSrActiveSubstances_OnOkButtonClick;

                    QuickLinksPopup.OnOkButtonClick += QuickLinksPopup_OnOkButtonClick;

                    btnExportLower.Click += btnExport_Click;

                    break;
            }

            ProductGrid.OnRebindRequired += ProductGrid_OnRebindRequired;
            ProductGrid.OnHtmlRowPrepared += ProductGrid_OnHtmlRowPrepared;
            ProductGrid.OnHtmlCellPrepared += ProductGrid_OnHtmlCellPrepared;
            ProductGrid.OnExcelCellPrepared += ProductGrid_OnExcelCellPrepared;
            ProductGrid.OnLoadClientLayout += ProductGrid_OnLoadClientLayout;
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
                    FillDdlAuthorisationProcedure();
                    FillDdlDomains();
                    FillDdlType();
                    FillDdlCountries();
                    break;
            }
        }

        void FillComboAuthorisationProcedure(object arg)
        {
            // Fill authorisation procedure combo box
            var authorisationProcedureTypeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.AuthorisationProcedure);
            var authorisationProcedureListItems = new List<ListItem>() { new ListItem("", "") };
            authorisationProcedureTypeList.ForEach(item => authorisationProcedureListItems.Add(new ListItem(item.name, item.name)));
            ProductGrid.SetComboList("AuthorisationProcedure", authorisationProcedureListItems);
        }

        /// <summary>
        /// Fills responsible users drop down list
        /// </summary>
        private void FillDdlResponsibleUsers()
        {
            var responsibleUserList = _personOperations.GetEntitiesByRoleName(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, "FullName", "person_PK");
            ddlResponsibleUser.SortItemsByText();
        }

        /// <summary>
        /// Fills authorisation procedure drop down list
        /// </summary>
        private void FillDdlAuthorisationProcedure()
        {
            var authorisationProcedureList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.AuthorisationProcedure);
            ddlAuthorisationProcedure.Fill(authorisationProcedureList, "name", "type_PK");
            ddlAuthorisationProcedure.SortItemsByText();
        }

        /// <summary>
        /// Fills domains drop down list
        /// </summary>
        private void FillDdlDomains()
        {
            var domainList = _domainOperations.GetEntities();
            ddlDomains.Fill(domainList, "name", "domain_PK");
            ddlDomains.SortItemsByText();
        }

        /// Fills type drop down list
        private void FillDdlType()
        {
            var typeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.Type);
            ddlType.Fill(typeList, "name", "type_PK");
            ddlType.SortItemsByText();
        }

        /// <summary>
        /// Fills authorisation countries drop down list
        /// </summary>
        private void FillDdlCountries()
        {
            var authorisationCountries = _countryOperations.GetEntitiesCustomSort();
            ddlCountry.Fill(authorisationCountries, Constant.Countries.DisplayNameFormat, "country_PK");
        }
        
        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    HandleEntityContextTimeUnit();
                    HideSearch();

                    if (Request.QueryString["idLay"] == "default")
                    {
                        ProductGrid.ClearFilters();
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
                ProductGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }

            if (!_idSearch.HasValue || Request.QueryString["Clear"] == "true") return;

            var savedProductSearch = _savedProductSearch.GetEntity(_idSearch);

            if (savedProductSearch == null) return;

            if (ListType == ListType.Search)
            {
                txtProductName.Text = savedProductSearch.name;
                ddlResponsibleUser.SelectedId = savedProductSearch.responsible_user_FK;
                BindPharmaceuticalProduct(savedProductSearch.pharmaaceutical_product_FK);
                txtClient.Text = savedProductSearch.client_name;
                ddlAuthorisationProcedure.SelectedId = savedProductSearch.procedure_type;
                ddlCountry.SelectedId = savedProductSearch.country_FK;
                ddlDomains.SelectedId = savedProductSearch.domain_FK;
                ddlType.SelectedId = savedProductSearch.type_product_FK;
                BindManufacturer(savedProductSearch.manufacturer_FK);
                BindActiveSubstances(savedProductSearch.activeSubstances);
                dtRngNextDlp.TextFrom = savedProductSearch.nextdlp_from.HasValue ? savedProductSearch.nextdlp_from.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngNextDlp.TextTo = savedProductSearch.nextdlp_to.HasValue ? savedProductSearch.nextdlp_to.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                txtPsurCycle.Text = savedProductSearch.psur;
                txtProductId.Text = savedProductSearch.product_ID;
                txtProductNumber.Text = savedProductSearch.product_number;
                txtDrugAtcs.Text = savedProductSearch.drug_atcs;
                if (savedProductSearch.article57_reporting.HasValue) rbYnArticle57Reporting.SelectedValue = savedProductSearch.article57_reporting;
            }

            ProductGrid.SetClientLayout(savedProductSearch.gridLayout);
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>();
            if (ProductGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ProductGrid.SecondSortingColumn, ProductGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ProductGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ProductGrid.MainSortingColumn, ProductGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("product_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _productOperations.GetListFormSearchDataSet(filters, ProductGrid.CurrentPage, ProductGrid.PageSize, gobList, out itemCount);
                else ds = _productOperations.GetListFormDataSet(filters, ProductGrid.CurrentPage, ProductGrid.PageSize, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _productOperations.GetListFormSearchDataSet(filters, ProductGrid.CurrentPage, ProductGrid.PageSize, gobList, out itemCount);
            }

            ProductGrid.TotalRecords = itemCount;

            if (ProductGrid.CurrentPage > ProductGrid.TotalPages || (ProductGrid.CurrentPage == 0 && ProductGrid.TotalPages > 0))
            {
                ProductGrid.CurrentPage = ProductGrid.CurrentPage > ProductGrid.TotalPages ? ProductGrid.TotalPages : 1;

                if (ListType == ListType.List)
                {
                    if (_idSearch.HasValue) ds = _productOperations.GetListFormSearchDataSet(filters, ProductGrid.CurrentPage, ProductGrid.PageSize, gobList, out itemCount);
                    else ds = _productOperations.GetListFormDataSet(filters, ProductGrid.CurrentPage, ProductGrid.PageSize, gobList, out itemCount);
                }
                else if (ListType == ListType.Search)
                {
                    ds = _productOperations.GetListFormSearchDataSet(filters, ProductGrid.CurrentPage, ProductGrid.PageSize, gobList, out itemCount);
                }
            }

            ProductGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            ProductGrid.DataBind();

            MasterPage.UpCommon.Update();
        }

        private void BindGridDynamicControls()
        {
            FillComboAuthorisationProcedure(null);
        }

        private void BindDynamicControls(object args)
        {
            if (ListType == ListType.Search) subtabs.Controls.Clear();
        }

        private void BindPharmaceuticalProduct(int? pharmaceuticalProductFk)
        {
            var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(pharmaceuticalProductFk);

            if (pharmaceuticalProduct == null || pharmaceuticalProduct.pharmaceutical_product_PK == null) return;

            txtSrPharmaceuticalProducts.SelectedEntityId = pharmaceuticalProduct.pharmaceutical_product_PK;
            txtSrPharmaceuticalProducts.Text = !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.MissingValue;
        }

        private void BindManufacturer(int? manufacturerFk)
        {
            var manufacturer = _organizationOperations.GetEntity(manufacturerFk);
            if (manufacturer == null || manufacturer.organization_PK == null) return;

            txtSrManufacturers.SelectedEntityId = manufacturer.organization_PK;
            txtSrManufacturers.Text = manufacturer.name_org;
        }

        private void BindActiveSubstances(String activeSubstanceIds)
        {

            if (String.IsNullOrWhiteSpace(activeSubstanceIds)) return;

            string[] activeSubstanceIdArray = activeSubstanceIds.Split(',');

            foreach (var selectedId in activeSubstanceIdArray)
            {
                var activeSubstance = _substanceOperations.GetEntity(selectedId);

                if (activeSubstance != null)
                {
                    var substanceEvCode = !string.IsNullOrWhiteSpace(activeSubstance.ev_code) ? activeSubstance.ev_code : Constant.UnknownValue;
                    var substanceName = !string.IsNullOrWhiteSpace(activeSubstance.substance_name) ? activeSubstance.substance_name : Constant.UnknownValue;

                    var text = string.Format("{0} ({1})", substanceName, substanceEvCode);

                    lbSrActiveSubstances.LbInput.Items.Add(new ListItem(text, selectedId.ToString()));
                }
            }
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
                        if (EntityContext == EntityContext.TimeUnit && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Form.aspx?EntityContext={0}&Action=New&idTimeUnit={1}&From=TimeUnitProdList", EntityContext, _idTimeUnit));
                        else if (EntityContext == EntityContext.TimeUnitMy && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Form.aspx?EntityContext={0}&Action=New&idTimeUnit={1}&From=TimeUnitMyProdList", EntityContext, _idTimeUnit));
                        Response.Redirect(string.Format("~/Views/ProductView/Form.aspx?EntityContext={0}&Action=New&From=Prod", EntityContext.Default));
                    }
                    break;
                case ContextMenuEventTypes.Back:
                    {
                        if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
                        else if (EntityContext == EntityContext.TimeUnit) Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.TimeUnitMy) Response.Redirect(string.Format("~/Views/TimeUnitView/List.aspx?EntityContext={0}", EntityContext));
                        Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
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
            Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&Action=Search&Clear=true{1}", EntityContext.Product, (_idSearch.HasValue ? "&idSearch=" + _idSearch : string.Empty)));
        }

        #endregion

        #region Pharmaceutical product searcher

        /// <summary>
        /// Handles pharmaceutical product list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void PharmaceuticalProductsSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(e.Data);

            if (pharmaceuticalProduct == null || pharmaceuticalProduct.pharmaceutical_product_PK == null) return;

            txtSrPharmaceuticalProducts.Text = pharmaceuticalProduct.name;
            txtSrPharmaceuticalProducts.SelectedEntityId = pharmaceuticalProduct.pharmaceutical_product_PK;
        }

        #endregion

        #region Manufacturers searcher

        /// <summary>
        /// Handles Manufacturers list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ManufacturersSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var manufacturer = _organizationOperations.GetEntity(e.Data);

            if (manufacturer == null || manufacturer.organization_PK == null) return;

            txtSrManufacturers.Text = manufacturer.name_org;
            txtSrManufacturers.SelectedEntityId = manufacturer.organization_PK;
        }

        #endregion

        #region Active substances searcher

        /// <summary>
        /// Handles Active substances list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LbSrActiveSubstances_OnOkButtonClick(object sender, FormEventArgs<List<int>> e)
        {
            foreach (var selectedId in lbSrActiveSubstances.Searcher.SelectedItems)
            {
                if (lbSrActiveSubstances.LbInput.Items.FindByValue(selectedId.ToString()) != null) continue;

                var activeSubstance = _substanceOperations.GetEntity(selectedId);
            
                if (activeSubstance != null)
                {
                    var substanceEvCode = !string.IsNullOrWhiteSpace(activeSubstance.ev_code) ? activeSubstance.ev_code : Constant.UnknownValue;
                    var substanceName = !string.IsNullOrWhiteSpace(activeSubstance.substance_name) ? activeSubstance.substance_name : Constant.UnknownValue;

                    var text = string.Format("{0} ({1})", substanceName, substanceEvCode);

                    lbSrActiveSubstances.LbInput.Items.Add(new ListItem(text, selectedId.ToString()));
                }
            }
        }

        #endregion

        #region Grid

        void ProductGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {

        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in ProductGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("product_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (ProductGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            ProductGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            ProductGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            
            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = ProductGrid.GetClientLayoutString(),
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
            if (ProductGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(ProductGrid.SecondSortingColumn, ProductGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (ProductGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(ProductGrid.MainSortingColumn, ProductGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("product_PK", GEMOrderByType.DESC));

            int itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _productOperations.GetListFormSearchDataSet(filters, 1, int.MaxValue, gobList, out itemCount); // Quick link
                else ds = _productOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _productOperations.GetListFormSearchDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            ProductGrid["product_PK"].Visible = true;
            if (ds != null) ProductGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            ProductGrid["product_PK"].Visible = false;

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            ProductGrid.ResetVisibleColumns();
            ProductGrid.SecondSortingColumn = null;
            ProductGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
            BindGrid();
        }

        void ProductGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            var pnlActiveSubstances = e.FindControl("pnlActiveSubstances") as Panel;
            if (pnlActiveSubstances != null)
            {
                var activeSubstances = Convert.ToString(e.GetValue("ActiveSubstances"));
                var activeSubstanceList = !string.IsNullOrWhiteSpace(activeSubstances) ? activeSubstances.Split(new[] { " ||| " }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

                if (activeSubstanceList.Length > 0)
                {
                    pnlActiveSubstances.Controls.Add(new LiteralControl("<ul class='devExUL'>"));
                    foreach (var activeSubstance in activeSubstanceList)
                    {
                        pnlActiveSubstances.Controls.Add(new LiteralControl(String.Format("<li>{0}</li>", activeSubstance)));
                    }
                    pnlActiveSubstances.Controls.Add(new LiteralControl("</ul>"));
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

            var hlNewPharmProd = e.FindControl("hlNewPharmProd") as HyperLink;
            if (hlNewPharmProd != null)
            {
                if (SecurityHelper.IsPermitted(Permission.InsertPharmaceuticalProduct)) BindNewPharmaceuticalProductLink(hlNewPharmProd);
                else hlNewPharmProd.NavigateUrl = string.Empty;
            }

            var hlNewSubUnit = e.FindControl("hlNewSubUnit") as HyperLink;
            if (hlNewSubUnit != null)
            {
                if (SecurityHelper.IsPermitted(Permission.InsertSubmissionUnit)) BindNewSubmissionUnitLink(hlNewSubUnit);
                else hlNewSubUnit.NavigateUrl = string.Empty;
            }

            var hlNewAuthProd = e.FindControl("hlNewAuthProd") as HyperLink;
            if (hlNewAuthProd != null)
            {
                if (SecurityHelper.IsPermitted(Permission.InsertAuthorisedProduct)) BindNewAuthorisedProductLink(hlNewAuthProd);
                else hlNewAuthProd.NavigateUrl = string.Empty;
            }
        }

        void ProductGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void ProductGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!ProductGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = ProductGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (ProductGrid.SortCount > 1 && e.FieldName == ProductGrid.MainSortingColumn)
                return;

            if (e.FieldName == "ProductName" || e.FieldName == "ActiveSubstances" || e.FieldName == "product_number" || e.FieldName == "AuthorisationProcedure" || e.FieldName == "Countries" || e.FieldName == "Client")
            {
                e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        void ProductGrid_OnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
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
                var savedSearch = _savedProductSearch.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
                if (savedSearch != null)
                {
                    quickLink = new QuickLink
                    {
                        Name = savedSearch.displayName,
                        IsPublic = savedSearch.isPublic,
                    };
                }
            }

            QuickLinksPopup.ShowModalForm(quickLink);
        }

        public void btnDeleteSearchClick(object sender, EventArgs e)
        {
            if (!ValidationHelper.IsValidInt(Request.QueryString["idSearch"])) return;

            _savedProductSearch.Delete(Convert.ToInt32(Request.QueryString["idSearch"]));
            Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&Action=Search", EntityContext.Product));
        }

        protected void QuickLinksPopup_OnOkButtonClick(object sender, FormEventArgs<QuickLink> e)
        {
            Product_saved_search_PK savedProductSearch = null;

            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                savedProductSearch = _savedProductSearch.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
            }

            if (savedProductSearch == null)
            {
                savedProductSearch = new Product_saved_search_PK();
            }

            var nextDlpFrom = ValidationHelper.IsValidDateTime(dtRngNextDlp.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngNextDlp.TextFrom, CultureInfoHr) : null;
            var nextDlpTo = ValidationHelper.IsValidDateTime(dtRngNextDlp.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngNextDlp.TextTo, CultureInfoHr) : null;

            savedProductSearch.name = txtProductName.Text;
            savedProductSearch.responsible_user_FK = ddlResponsibleUser.SelectedId;
            savedProductSearch.pharmaaceutical_product_FK = txtSrPharmaceuticalProducts.SelectedEntityId;
            savedProductSearch.client_name = txtClient.Text;
            savedProductSearch.procedure_type = ddlAuthorisationProcedure.SelectedId;
            savedProductSearch.country_FK = ddlCountry.SelectedId;
            savedProductSearch.domain_FK = ddlDomains.SelectedId;
            savedProductSearch.type_product_FK = ddlType.SelectedId;
            savedProductSearch.manufacturer_FK = txtSrManufacturers.SelectedEntityId;
            //TODO:
            var activeSubstanceIds = "";
            foreach (ListItem activeSubstance in lbSrActiveSubstances.LbInput.Items) activeSubstanceIds += activeSubstance.Value.ToString() + ",";
            savedProductSearch.activeSubstances = activeSubstanceIds.TrimEnd(',').Trim();

            savedProductSearch.nextdlp_from = nextDlpFrom;
            savedProductSearch.nextdlp_to = nextDlpTo;
            savedProductSearch.psur = txtPsurCycle.Text;
            savedProductSearch.product_ID = txtProductId.Text;
            savedProductSearch.product_number = txtProductNumber.Text;
            savedProductSearch.drug_atcs = txtDrugAtcs.Text;
            savedProductSearch.article57_reporting = rbYnArticle57Reporting.SelectedValue;
            savedProductSearch.gridLayout = ProductGrid.GetClientLayoutString();

            var quickLink = e.Data;
            if (quickLink != null)
            {
                savedProductSearch.displayName = quickLink.Name;
                savedProductSearch.isPublic = quickLink.IsPublic.HasValue && quickLink.IsPublic.Value;
            }

            var user = _userOperations.GetUserByUsername(SessionManager.Instance.CurrentUser.Username);
            if (user != null)
            {
                savedProductSearch.user_FK = user.Person_FK;
            }

            savedProductSearch = _savedProductSearch.Save(savedProductSearch);
            Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&Action=Search&idSearch={1}", EntityContext.Product, savedProductSearch.product_saved_search_PK));
        }

        #endregion

        #endregion

        #region Support methods

        private void ClearSearch()
        {
            txtProductName.Text = string.Empty;
            ddlResponsibleUser.SelectedValue = string.Empty;
            txtSrPharmaceuticalProducts.Clear();
            txtProductNumber.Text = string.Empty;
            ddlAuthorisationProcedure.SelectedValue = string.Empty;
            txtDrugAtcs.Text = string.Empty;
            txtClient.Text = string.Empty;
            ddlDomains.SelectedValue = string.Empty;
            ddlType.SelectedValue = string.Empty;
            txtProductId.Text = string.Empty;
            ddlCountry.SelectedValue = string.Empty;
            txtSrManufacturers.Text = string.Empty;
            txtPsurCycle.Text = string.Empty;
            dtRngNextDlp.Clear();
        }

        private Dictionary<string, string> GetFilters()
        {
            var filters = ProductGrid.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (_idSearch.HasValue)
                    {
                        Product_saved_search_PK savedProductSearch = _savedProductSearch.GetEntity(_idSearch);
                        FillFilters(savedProductSearch, filters);
                    }

                    if (EntityContext.In(EntityContext.TimeUnit, EntityContext.TimeUnitMy))
                    {
                        filters.Add("QueryBy", EntityContext.TimeUnit.ToString());
                        filters.Add("EntityPk", Convert.ToString(_idTimeUnit));
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
            if (!string.IsNullOrWhiteSpace(txtProductName.Text)) filters.Add("SearchProductName", txtProductName.Text);
            if (ddlResponsibleUser.SelectedId.HasValue) filters.Add("SearchResponsibleUserPk", ddlResponsibleUser.SelectedId.Value.ToString());
            if (txtSrPharmaceuticalProducts.SelectedEntityId.HasValue) filters.Add("SearchPharmaceuticalProductPk", txtSrPharmaceuticalProducts.SelectedEntityId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtProductNumber.Text)) filters.Add("SearchProductNumber", txtProductNumber.Text);
            if (ddlAuthorisationProcedure.SelectedId.HasValue) filters.Add("SearchAuthorisationProcedurePk", ddlAuthorisationProcedure.SelectedId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtDrugAtcs.Text)) filters.Add("SearchDrugAtcs", txtDrugAtcs.Text);
            if (!string.IsNullOrWhiteSpace(txtClient.Text)) filters.Add("SearchClient", txtClient.Text);
            if (ddlDomains.SelectedId.HasValue) filters.Add("SearchDomainPk", ddlDomains.SelectedId.Value.ToString());
            if (ddlType.SelectedId.HasValue) filters.Add("SearchTypePk", ddlType.SelectedId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtProductId.Text)) filters.Add("SearchProductId", txtProductId.Text);
            if (ddlCountry.SelectedId.HasValue) filters.Add("SearchCountryPk", ddlCountry.SelectedId.Value.ToString());
            if (txtSrManufacturers.SelectedEntityId.HasValue) filters.Add("SearchManufacturerPk", txtSrManufacturers.SelectedEntityId.Value.ToString());
            if (lbSrActiveSubstances.LbInput.Items.Count > 0)
            {
                var activeSubstanceIds = String.Join(",", lbSrActiveSubstances.LbInput.Items.Cast<ListItem>().Select(x => x.Value));
                filters.Add("SearchActiveSubstancePK", activeSubstanceIds);
            }
            if (!string.IsNullOrWhiteSpace(txtPsurCycle.Text)) filters.Add("SearchPsurCycle", txtPsurCycle.Text);
            if (rbYnArticle57Reporting.SelectedItem != null) filters.Add("SearchArcticle57", rbYnArticle57Reporting.SelectedValue.Value ? "Yes" : "No"); else filters.Add("SearchArcticle57", "");
            if (ValidationHelper.IsValidDateTime(dtRngNextDlp.TextFrom, CultureInfoHr)) filters.Add("SearchNextDlpFrom", dtRngNextDlp.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngNextDlp.TextTo, CultureInfoHr)) filters.Add("SearchNextDlpTo", dtRngNextDlp.TextTo);
        }

        private void FillFilters(Product_saved_search_PK savedProductSearch, Dictionary<string, string> filters)
        {
            if (!string.IsNullOrWhiteSpace(savedProductSearch.name)) filters.Add("SearchProductName", savedProductSearch.name);
            if (savedProductSearch.responsible_user_FK.HasValue) filters.Add("SearchResponsibleUserPk", savedProductSearch.responsible_user_FK.Value.ToString());
            if (savedProductSearch.pharmaaceutical_product_FK.HasValue) filters.Add("SearchPharmaceuticalProductPk", savedProductSearch.pharmaaceutical_product_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedProductSearch.product_number)) filters.Add("SearchProductNumber", savedProductSearch.product_number);
            if (savedProductSearch.procedure_type.HasValue) filters.Add("SearchAuthorisationProcedurePk", savedProductSearch.procedure_type.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedProductSearch.drug_atcs)) filters.Add("SearchDrugAtcs", savedProductSearch.drug_atcs);
            if (!string.IsNullOrWhiteSpace(savedProductSearch.client_name)) filters.Add("SearchClient", savedProductSearch.client_name);
            if (savedProductSearch.domain_FK.HasValue) filters.Add("SearchDomainPk", savedProductSearch.domain_FK.Value.ToString());
            if (savedProductSearch.type_product_FK.HasValue) filters.Add("SearchTypePk", savedProductSearch.type_product_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedProductSearch.product_ID)) filters.Add("SearchProductId", savedProductSearch.product_ID);
            if (savedProductSearch.country_FK.HasValue) filters.Add("SearchCountryPk", savedProductSearch.country_FK.Value.ToString());
            if (savedProductSearch.manufacturer_FK.HasValue) filters.Add("SearchManufacturerPk", savedProductSearch.manufacturer_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedProductSearch.activeSubstances)) filters.Add("SearchActiveSubstancePK", savedProductSearch.activeSubstances);
            if (!string.IsNullOrWhiteSpace(savedProductSearch.psur)) filters.Add("SearchPsurCycle", savedProductSearch.psur);
            if (savedProductSearch.article57_reporting.HasValue) filters.Add("SearchArcticle57", savedProductSearch.article57_reporting.Value ? "Yes" : "No"); else filters.Add("SearchArcticle57", "");
            if (savedProductSearch.nextdlp_from.HasValue) filters.Add("SearchNextDlpFrom", savedProductSearch.nextdlp_from.Value.ToString(Constant.DateTimeFormat));
            if (savedProductSearch.nextdlp_to.HasValue) filters.Add("SearchNextDlpTo", savedProductSearch.nextdlp_to.Value.ToString(Constant.DateTimeFormat));
        }

        private void GenerateContextMenuItems()
        {
            var contexMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                switch (EntityContext)
                {
                    case EntityContext.Product:
                        contexMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") };
                        break;

                    case EntityContext.TimeUnit:
                    case EntityContext.TimeUnitMy:
                        contexMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.Back, "Back") };
                        break;
                }

                MasterPage.ContextMenu.SetContextMenuItemsVisible(contexMenu);
            }
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location;

            if (ListType == ListType.List)
            {
                if (EntityContext == EntityContext.Product)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("Prod", Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                    if (location != null)
                    {
                        MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                        MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    }
                }
                else if (EntityContext == EntityContext.TimeUnit)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("TimeUnitProdList", Support.CacheManager.Instance.AppLocations);
                    MasterPage.TabMenu.TabControls.Clear();
                    tabMenu.Visible = true;
                    if (location != null)
                    {
                        tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                        tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    }
                }
                else if (EntityContext == EntityContext.TimeUnitMy)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("TimeUnitMyProdList", Support.CacheManager.Instance.AppLocations);
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
                location = Support.LocationManager.Instance.GetLocationByName("ProdListSearch", Support.CacheManager.Instance.AppLocations);
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
            //    location = Support.LocationManager.Instance.GetLocationByName("Prod", Support.CacheManager.Instance.AppLocations);
            //}
            //else if (ListType == ListType.Search)
            //{
            //    location = Support.LocationManager.Instance.GetLocationByName("ProdListSearch", Support.CacheManager.Instance.AppLocations);
            //}

            //if (location != null)
            //{
            //    var topLevelParent = MasterPage.FindTopLevelParent(location);

            //    MasterPage.CurrentLocation = location;
            //    MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            //}
        }

        private DataTable PrepareDataForExport(DataTable productDataTable)
        {
            if (productDataTable == null || productDataTable.Rows.Count == 0) return productDataTable;

            if (!productDataTable.Columns.Contains("ActiveSubstances")) return productDataTable;

            foreach (DataRow row in productDataTable.Rows)
            {
                var activeSubstances = Convert.ToString(row["ActiveSubstances"]);
                row["ActiveSubstances"] = !string.IsNullOrWhiteSpace(activeSubstances) ? activeSubstances.Replace(" ||| ", "") : string.Empty;
            }

            return productDataTable;
        }

        public string HandleMissing(object value)
        {
            if (value != null && value.ToString().Trim() != "")
            {
                return value.ToString();
            }

            return "Missing";
        }

        private void HandleEntityContextTimeUnit()
        {
            if (EntityContext != EntityContext.TimeUnit && EntityContext != EntityContext.TimeUnitMy) return;

            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Time unit:";

            var timeUnit = _timeUnitOperations.GetEntity(_idTimeUnit);

            lblPrvParentEntity.Text = timeUnit != null && !string.IsNullOrWhiteSpace(timeUnit.Name) ? timeUnit.Name : Constant.ControlDefault.LbPrvText;
        }

        private void BindNewAuthorisedProductLink(HyperLink hlNewAuthProd)
        {
            BindGridNewLink(hlNewAuthProd);
        }

        private void BindNewSubmissionUnitLink(HyperLink hlNewSubUnit)
        {
            BindGridNewLink(hlNewSubUnit);
        }

        private void BindNewPharmaceuticalProductLink(HyperLink hlNewPharmProd)
        {
            BindGridNewLink(hlNewPharmProd);
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
                    if (EntityContext == EntityContext.Product) hlNewLink.NavigateUrl += "&From=Prod";
                    else if (EntityContext == EntityContext.TimeUnit && _idTimeUnit.HasValue) hlNewLink.NavigateUrl += string.Format("&From=TimeUnitProdList&idTimeUnit={0}", _idTimeUnit);
                    else if (EntityContext == EntityContext.TimeUnitMy) hlNewLink.NavigateUrl += string.Format("&From=TimeUnitMyProdList&idTimeUnit={0}", _idTimeUnit);
                    break;

                case ListType.Search:
                    hlNewLink.NavigateUrl += "&From=ProdSearch";
                    break;
            }
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedInsertProduct = SecurityHelper.IsPermitted(Permission.InsertProduct);
            if (isPermittedInsertProduct)
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