using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUIFramework;
using GEM2Common;
using PossGrid;
using Ready.Model;

namespace AspNetUI.Views.PharmaceuticalProductView
{
    public partial class List : ListPage
    {
        #region Declarations

        private IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        private IPharmaceutical_product_saved_search_PKOperations _pharmaceuticalProductsOperations;
        private IProduct_PKOperations _productOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IUSEROperations _userOperations;
        private IPerson_PKOperations _personOperations;
        private IPharmaceutical_form_PKOperations _pharmaceuticalFormOperations;

        private int _sortCount;
        private bool _flip = true;
        private const int NumLayoutToKeep = 5;
        string _idLay;
        private string _gridId;

        private int? _idProd;
        private int? _idSearch;

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

            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;
            _idSearch = ValidationHelper.IsValidInt(Request.QueryString["idSearch"]) ? int.Parse(Request.QueryString["idSearch"]) : (int?)null;

            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _productOperations = new Product_PKDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();
            _personOperations = new Person_PKDAL();
            _pharmaceuticalFormOperations = new Pharmaceutical_form_PKDAL();
            _pharmaceuticalProductsOperations = new Pharmaceutical_product_saved_search_PKDAL();

            if (ListType == ListType.Search)
            {
                PharmaceuticalProductGrid.GridVersion = PharmaceuticalProductGrid.GridVersion + ListType.ToString();
            }
            else if (EntityContext != EntityContext.Default)
            {
                PharmaceuticalProductGrid.GridVersion = PharmaceuticalProductGrid.GridVersion + EntityContext.ToString();
            }

            _gridId = PharmaceuticalProductGrid.GridId + "_" + PharmaceuticalProductGrid.GridVersion;
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
                    QuickLinksPopup.OnOkButtonClick += QuickLinksPopup_OnOkButtonClick;

                    btnExportLower.Click += btnExport_Click;

                    break;
            }

            PharmaceuticalProductGrid.OnRebindRequired += PharmaceuticalProductGrid_OnRebindRequired;
            PharmaceuticalProductGrid.OnHtmlRowPrepared += PharmaceuticalProductGrid_OnHtmlRowPrepared;
            PharmaceuticalProductGrid.OnHtmlCellPrepared += PharmaceuticalProductGrid_OnHtmlCellPrepared;
            PharmaceuticalProductGrid.OnExcelCellPrepared += PharmaceuticalProductGrid_OnExcelCellPrepared;
            PharmaceuticalProductGrid.OnLoadClientLayout += PharmaceuticalProductGrid_OnLoadClientLayout;
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
                    FillDdlPharmaceuticalForm();
                    FillDdlResponsibleUsers();
                    break;
            }
        }

        void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    HandleListModeByProduct();
                    HideSearch();

                    if (Request.QueryString["idLay"] == "default")
                    {
                        PharmaceuticalProductGrid.ClearFilters();
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
        }

        /// <summary>
        /// Fills responsible users drop down list
        /// </summary>
        private void FillDdlResponsibleUsers()
        {
            var responsibleUsers = _personOperations.GetPersonsByRole(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUsers, "FullName", "person_PK");
            ddlResponsibleUser.SortItemsByText();
        }

        /// <summary>
        /// Fills responsible users drop down list
        /// </summary>
        private void FillDdlPharmaceuticalForm()
        {
            var pharmaceuticalFormList = _pharmaceuticalFormOperations.GetEntities();
            ddlPharmaceuticalForm.Fill(pharmaceuticalFormList, "name", "pharmaceutical_form_PK");
            ddlPharmaceuticalForm.SortItemsByText();
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                PharmaceuticalProductGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }

            if (!_idSearch.HasValue || Request.QueryString["Clear"] == "true") return;

            var savedPharmacuticalProductSearch = _pharmaceuticalProductsOperations.GetEntity(_idSearch);

            if (savedPharmacuticalProductSearch == null) return;

            if (ListType == ListType.Search)
            {
                BindProduct(savedPharmacuticalProductSearch.product_FK);
                txtPharmaceuticalProductName.Text = savedPharmacuticalProductSearch.name;
                ddlResponsibleUser.SelectedId = savedPharmacuticalProductSearch.responsible_user_FK;
                txtDescription.Text = savedPharmacuticalProductSearch.description;
                ddlPharmaceuticalForm.SelectedId = savedPharmacuticalProductSearch.Pharmform_FK;
                txtAdministrationRoutes.Text = savedPharmacuticalProductSearch.administrationRoutes;
                txtActiveIngredients.Text = savedPharmacuticalProductSearch.activeIngridients;
                txtExcipients.Text = savedPharmacuticalProductSearch.excipients;
                txtAdjuvants.Text = savedPharmacuticalProductSearch.adjuvants;
                txtMedicalDevices.Text = savedPharmacuticalProductSearch.medical_devices;
                txtComment.Text = savedPharmacuticalProductSearch.comments;
            }

            PharmaceuticalProductGrid.SetClientLayout(savedPharmacuticalProductSearch.gridLayout);
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy>() { };
            if (PharmaceuticalProductGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(PharmaceuticalProductGrid.SecondSortingColumn, PharmaceuticalProductGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (PharmaceuticalProductGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(PharmaceuticalProductGrid.MainSortingColumn, PharmaceuticalProductGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("pharmaceutical_product_PK", GEMOrderByType.DESC));

            var itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _pharmaceuticalProductOperations.GetListFormSearchDataSet(filters, PharmaceuticalProductGrid.CurrentPage, PharmaceuticalProductGrid.PageSize, gobList, out itemCount);
                else ds = _pharmaceuticalProductOperations.GetListFormDataSet(filters, PharmaceuticalProductGrid.CurrentPage, PharmaceuticalProductGrid.PageSize, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _pharmaceuticalProductOperations.GetListFormSearchDataSet(filters, PharmaceuticalProductGrid.CurrentPage, PharmaceuticalProductGrid.PageSize, gobList, out itemCount);
            }

            PharmaceuticalProductGrid.TotalRecords = itemCount;

            if (PharmaceuticalProductGrid.CurrentPage > PharmaceuticalProductGrid.TotalPages || (PharmaceuticalProductGrid.CurrentPage == 0 && PharmaceuticalProductGrid.TotalPages > 0))
            {
                if (PharmaceuticalProductGrid.CurrentPage > PharmaceuticalProductGrid.TotalPages) PharmaceuticalProductGrid.CurrentPage = PharmaceuticalProductGrid.TotalPages; else PharmaceuticalProductGrid.CurrentPage = 1;

                if (ListType == ListType.List)
                {
                    if (_idSearch.HasValue) ds = _pharmaceuticalProductOperations.GetListFormSearchDataSet(filters, PharmaceuticalProductGrid.CurrentPage, PharmaceuticalProductGrid.PageSize, gobList, out itemCount);
                    else  ds = _pharmaceuticalProductOperations.GetListFormDataSet(filters, PharmaceuticalProductGrid.CurrentPage, PharmaceuticalProductGrid.PageSize, gobList, out itemCount);
                }
                else if (ListType == ListType.Search)
                {
                    ds = _pharmaceuticalProductOperations.GetListFormSearchDataSet(filters, PharmaceuticalProductGrid.CurrentPage, PharmaceuticalProductGrid.PageSize, gobList, out itemCount);
                }
            }

            PharmaceuticalProductGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            PharmaceuticalProductGrid.DataBind();

            MasterPage.UpCommon.Update();
        }

        private void BindProduct(int? product_FK)
        {
            var relatedProduct = _productOperations.GetEntity(product_FK);
            if (relatedProduct != null)
            {
                txtSrProduct.SelectedEntityId = product_FK;
                txtSrProduct.Text = relatedProduct.GetNameFormatted();
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
                        if (EntityContext == EntityContext.Product && _idProd.HasValue) Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/Form.aspx?EntityContext={0}&Action=New&idProd={1}&From=ProdPharmProdList", EntityContext.Product, _idProd.Value));
                        Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/Form.aspx?EntityContext={0}&Action=New&From=PharmProd", EntityContext.Default));
                    }
                    break;
                case ContextMenuEventTypes.Back:
                    {
                        if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext.Product));
                        Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext.PharmaceuticalProduct));
                    }
                    break;
            }
        }

        #endregion

        #region Grid

        void PharmaceuticalProductGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {
            if (args.RowType != DataControlRowType.DataRow) return;
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in PharmaceuticalProductGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("pharmaceutical_product_PK"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (PharmaceuticalProductGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            PharmaceuticalProductGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            PharmaceuticalProductGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            
            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = PharmaceuticalProductGrid.GetClientLayoutString(),
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
            if (PharmaceuticalProductGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(PharmaceuticalProductGrid.SecondSortingColumn, PharmaceuticalProductGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (PharmaceuticalProductGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(PharmaceuticalProductGrid.MainSortingColumn, PharmaceuticalProductGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("pharmaceutical_product_PK", GEMOrderByType.DESC));

            int itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _pharmaceuticalProductOperations.GetListFormSearchDataSet(filters, PharmaceuticalProductGrid.CurrentPage, PharmaceuticalProductGrid.PageSize, gobList, out itemCount); // Quick link
                else ds = _pharmaceuticalProductOperations.GetListFormDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _pharmaceuticalProductOperations.GetListFormSearchDataSet(filters, 1, Int32.MaxValue, gobList, out itemCount);
            }

            PharmaceuticalProductGrid["pharmaceutical_product_PK"].Visible = true;
            if (ds != null) PharmaceuticalProductGrid.ExportDataToXlsx(PrepareDataForExport(ds.Tables[0]), new PossGrid.ExcellExportOptions("grid"));
            PharmaceuticalProductGrid["pharmaceutical_product_PK"].Visible = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            PharmaceuticalProductGrid.ResetVisibleColumns();
            PharmaceuticalProductGrid.SecondSortingColumn = null;
            PharmaceuticalProductGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
            BindGrid();
        }

        void PharmaceuticalProductGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
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
                    if (SecurityHelper.IsPermitted(Permission.View) && SecurityHelper.IsPermitted(Permission.InsertDocument)) BindNewDocumentLink(hlNewDocument);
                    else hlNewDocument.NavigateUrl = string.Empty;
                }
            }

            var pnlAdministrationRoutes = e.FindControl("pnlAdministrationRoutes") as Panel;

            if (pnlAdministrationRoutes != null)
            {
                var administrationRoutes = Convert.ToString(e.GetValue("AdministrationRoutes"));
                var administrationRouteList = !string.IsNullOrWhiteSpace(administrationRoutes) ? administrationRoutes.Split(new[] { " ||| " }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

                if (administrationRouteList.Length > 0)
                {
                    pnlAdministrationRoutes.Controls.Add(new LiteralControl("<ul class='devExUL'>"));
                    foreach (var administrationRoute in administrationRouteList)
                    {
                        pnlAdministrationRoutes.Controls.Add(new LiteralControl(String.Format("<li>{0}</li>", administrationRoute)));
                    }
                    pnlAdministrationRoutes.Controls.Add(new LiteralControl("</ul>"));
                }
            }

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

            var pnlExcipients = e.FindControl("pnlExcipients") as Panel;

            if (pnlExcipients != null)
            {
                var excipients = Convert.ToString(e.GetValue("Excipients"));
                var excipientList = !string.IsNullOrWhiteSpace(excipients) ? excipients.Split(new[] { " ||| " }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

                if (excipientList.Length > 0)
                {
                    pnlExcipients.Controls.Add(new LiteralControl("<ul class='devExUL'>"));
                    foreach (var excipient in excipientList)
                    {
                        pnlExcipients.Controls.Add(new LiteralControl(String.Format("<li>{0}</li>", excipient)));
                    }
                    pnlExcipients.Controls.Add(new LiteralControl("</ul>"));
                }
            }

            var pnlAdjuvants = e.FindControl("pnlAdjuvants") as Panel;

            if (pnlAdjuvants != null)
            {
                var adjuvants = Convert.ToString(e.GetValue("Adjuvants"));
                var adjuvantList = !string.IsNullOrWhiteSpace(adjuvants) ? adjuvants.Split(new[] { " ||| " }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

                if (adjuvantList.Length > 0)
                {
                    pnlAdjuvants.Controls.Add(new LiteralControl("<ul class='devExUL'>"));
                    foreach (var adjuvant in adjuvantList)
                    {
                        pnlAdjuvants.Controls.Add(new LiteralControl(String.Format("<li>{0}</li>", adjuvant)));
                    }
                    pnlAdjuvants.Controls.Add(new LiteralControl("</ul>"));
                }
            }
        }

        void PharmaceuticalProductGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void PharmaceuticalProductGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!PharmaceuticalProductGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = PharmaceuticalProductGrid.SortCount;
                _flip = !_flip;
            }

            _sortCount--;

            if (PharmaceuticalProductGrid.SortCount > 1 && e.FieldName == PharmaceuticalProductGrid.MainSortingColumn)
                return;

            if (e.FieldName != "DocCount")
            {
                e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        void PharmaceuticalProductGrid_OnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                args.ClientLayoutString = userGridSettings.grid_layout;
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
            Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}&Action=Search&Clear=true{1}", EntityContext.PharmaceuticalProduct, (_idSearch.HasValue ? "&idSearch=" + _idSearch : string.Empty)));
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

            txtSrProduct.Text = product.GetNameFormatted(string.Empty);
            txtSrProduct.SelectedEntityId = product.product_PK;
        }

        #endregion

        #region Quick links

        public void btnSaveSearchClick(object sender, EventArgs e)
        {
            QuickLink quickLink = null;
            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                var savedSearch = _pharmaceuticalProductsOperations.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
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

            _pharmaceuticalProductsOperations.Delete(Convert.ToInt32(Request.QueryString["idSearch"]));
            Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}&Action=Search", EntityContext.PharmaceuticalProduct));
        }

        private void QuickLinksPopup_OnOkButtonClick(object sender, FormEventArgs<QuickLink> e)
        {
            Pharmaceutical_product_saved_search_PK savedPharmacuticalProductSearch = null;

            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                savedPharmacuticalProductSearch = _pharmaceuticalProductsOperations.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
            }

            if (savedPharmacuticalProductSearch == null)
            {
                savedPharmacuticalProductSearch = new Pharmaceutical_product_saved_search_PK();
            }

            savedPharmacuticalProductSearch.responsible_user_FK = ddlResponsibleUser.SelectedId;
            savedPharmacuticalProductSearch.Pharmform_FK = ddlPharmaceuticalForm.SelectedId;
            savedPharmacuticalProductSearch.product_FK = txtSrProduct.SelectedEntityId;
            savedPharmacuticalProductSearch.description = txtDescription.Text;
            savedPharmacuticalProductSearch.comments = txtComment.Text;
            savedPharmacuticalProductSearch.name = txtPharmaceuticalProductName.Text;
            savedPharmacuticalProductSearch.administrationRoutes = txtAdministrationRoutes.Text;
            savedPharmacuticalProductSearch.activeIngridients = txtActiveIngredients.Text;
            savedPharmacuticalProductSearch.excipients = txtExcipients.Text;
            savedPharmacuticalProductSearch.adjuvants = txtAdjuvants.Text;
            savedPharmacuticalProductSearch.medical_devices = txtMedicalDevices.Text;
            savedPharmacuticalProductSearch.gridLayout = PharmaceuticalProductGrid.GetClientLayoutString();

            var quickLink = e.Data;
            if (quickLink != null)
            {
                savedPharmacuticalProductSearch.displayName = quickLink.Name;
                savedPharmacuticalProductSearch.isPublic = quickLink.IsPublic.HasValue && quickLink.IsPublic.Value;
            }

            var user = _userOperations.GetUserByUsername(SessionManager.Instance.CurrentUser.Username);
            if (user != null)
            {
                savedPharmacuticalProductSearch.user_FK = user.Person_FK;
            }

            savedPharmacuticalProductSearch = _pharmaceuticalProductsOperations.Save(savedPharmacuticalProductSearch);
            Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}&Action=Search&idSearch={1}", EntityContext.PharmaceuticalProduct, savedPharmacuticalProductSearch.pharmaceutical_products_PK));
        }

        #endregion

        #endregion

        #region Support methods

        private void ClearSearch()
        {
            txtSrProduct.Text = string.Empty;
            txtPharmaceuticalProductName.Text = string.Empty;
            ddlResponsibleUser.SelectedValue = string.Empty;
            txtDescription.Text = string.Empty;
            ddlPharmaceuticalForm.SelectedValue = string.Empty;
            txtAdministrationRoutes.Text = string.Empty;
            txtActiveIngredients.Text = string.Empty;
            txtExcipients.Text = string.Empty;
            txtAdjuvants.Text = string.Empty;
            txtMedicalDevices.Text = string.Empty;
            txtComment.Text = string.Empty;
        }

        private Dictionary<string, string> GetFilters()
        {
            var filters = PharmaceuticalProductGrid.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (_idSearch.HasValue)
                    {
                        Pharmaceutical_product_saved_search_PK savedPharmacuticalProductSearch = _pharmaceuticalProductsOperations.GetEntity(_idSearch);
                        FillFilters(savedPharmacuticalProductSearch, filters);
                    }
                    if (EntityContext == EntityContext.Product)
                    {
                        filters.Add("QueryBy", EntityContext.ToString());
                        filters.Add("EntityPk", Convert.ToString(_idProd));
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
            if (!string.IsNullOrWhiteSpace(txtPharmaceuticalProductName.Text)) filters.Add("SearchName", txtPharmaceuticalProductName.Text);
            if (ddlResponsibleUser.SelectedId.HasValue) filters.Add("SearchResponsibleUserPk", ddlResponsibleUser.SelectedId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtDescription.Text)) filters.Add("SearchDescription", txtDescription.Text);
            if (ddlPharmaceuticalForm.SelectedId.HasValue) filters.Add("SearchPharmaceuticalFormPk", ddlPharmaceuticalForm.SelectedId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtAdministrationRoutes.Text)) filters.Add("SearchAdministrationRoutes", txtAdministrationRoutes.Text);
            if (!string.IsNullOrWhiteSpace(txtActiveIngredients.Text)) filters.Add("SearchActiveSubstances", txtActiveIngredients.Text);
            if (!string.IsNullOrWhiteSpace(txtExcipients.Text)) filters.Add("SearchExcipients", txtExcipients.Text);
            if (!string.IsNullOrWhiteSpace(txtAdjuvants.Text)) filters.Add("SearchAdjuvants", txtAdjuvants.Text);
            if (!string.IsNullOrWhiteSpace(txtMedicalDevices.Text)) filters.Add("SearchMedicalDevices", txtMedicalDevices.Text);
            if (!string.IsNullOrWhiteSpace(txtComment.Text)) filters.Add("SearchComment", txtComment.Text);
        }

        private void FillFilters(Pharmaceutical_product_saved_search_PK savedPharmacuticalProductSearch, Dictionary<string, string> filters)
        {
            if (savedPharmacuticalProductSearch.product_FK.HasValue) filters.Add("SearchProductPk", savedPharmacuticalProductSearch.product_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedPharmacuticalProductSearch.name)) filters.Add("SearchName", savedPharmacuticalProductSearch.name);
            if (savedPharmacuticalProductSearch.responsible_user_FK.HasValue) filters.Add("SearchResponsibleUserPk", savedPharmacuticalProductSearch.responsible_user_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedPharmacuticalProductSearch.description)) filters.Add("SearchDescription", savedPharmacuticalProductSearch.description);
            if (savedPharmacuticalProductSearch.Pharmform_FK.HasValue) filters.Add("SearchPharmaceuticalFormPk", savedPharmacuticalProductSearch.Pharmform_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedPharmacuticalProductSearch.administrationRoutes)) filters.Add("SearchAdministrationRoutes", savedPharmacuticalProductSearch.administrationRoutes);
            if (!string.IsNullOrWhiteSpace(savedPharmacuticalProductSearch.activeIngridients)) filters.Add("SearchActiveSubstances", savedPharmacuticalProductSearch.activeIngridients);
            if (!string.IsNullOrWhiteSpace(savedPharmacuticalProductSearch.excipients)) filters.Add("SearchExcipients", savedPharmacuticalProductSearch.excipients);
            if (!string.IsNullOrWhiteSpace(savedPharmacuticalProductSearch.adjuvants)) filters.Add("SearchAdjuvants", savedPharmacuticalProductSearch.adjuvants);
            if (!string.IsNullOrWhiteSpace(savedPharmacuticalProductSearch.medical_devices)) filters.Add("SearchMedicalDevices", savedPharmacuticalProductSearch.medical_devices);
            if (!string.IsNullOrWhiteSpace(savedPharmacuticalProductSearch.comments)) filters.Add("SearchComment", savedPharmacuticalProductSearch.comments);
            if(!string.IsNullOrWhiteSpace(savedPharmacuticalProductSearch.gridLayout)) PharmaceuticalProductGrid.SetClientLayout(savedPharmacuticalProductSearch.gridLayout);
        }

        private void GenerateContextMenuItems()
        {
            var contexMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                switch (EntityContext)
                {
                    case EntityContext.PharmaceuticalProduct:
                        contexMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") };
                        break;
                    case EntityContext.Product:
                        contexMenu = new[]{
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
            Location_PK location;

            if (ListType == ListType.List)
            {
                if (EntityContext == EntityContext.PharmaceuticalProduct)
                {
                    location =  Support.LocationManager.Instance.GetLocationByName("PharmProd", Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                    if (location != null)
                    {
                        MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                        MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    }
                }
                else if (EntityContext == EntityContext.Product)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("ProdPharmProdList", Support.CacheManager.Instance.AppLocations);
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
                location =  Support.LocationManager.Instance.GetLocationByName("PharmProdSearch", Support.CacheManager.Instance.AppLocations);
                tabMenu.Visible = false;
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
            //    if (EntityContext == EntityContext.PharmaceuticalProduct) location = Support.LocationManager.Instance.GetLocationByName("PharmProd", Support.CacheManager.Instance.AppLocations);
            //    else if (EntityContext == EntityContext.Product) location = Support.LocationManager.Instance.GetLocationByName("ProdPharmProdList", Support.CacheManager.Instance.AppLocations);
            //}
            //else if (ListType == ListType.Search)
            //{
            //    location = Support.LocationManager.Instance.GetLocationByName("PharmProdSearch", Support.CacheManager.Instance.AppLocations);
            //}

            //if (location != null)
            //{
            //    var topLevelParent = MasterPage.FindTopLevelParent(location);

            //    MasterPage.CurrentLocation = location;
            //    MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            //}
        }

        private DataTable PrepareDataForExport(DataTable pharmaceuticalProductDataTable)
        {
            if (pharmaceuticalProductDataTable == null || pharmaceuticalProductDataTable.Rows.Count == 0) return pharmaceuticalProductDataTable;

            if (!pharmaceuticalProductDataTable.Columns.Contains("Products") ||
                !pharmaceuticalProductDataTable.Columns.Contains("AdministrationRoutes") ||
                !pharmaceuticalProductDataTable.Columns.Contains("ActiveSubstances") ||
                !pharmaceuticalProductDataTable.Columns.Contains("Excipients") ||
                !pharmaceuticalProductDataTable.Columns.Contains("Adjuvants")) return pharmaceuticalProductDataTable;

            foreach (DataRow row in pharmaceuticalProductDataTable.Rows)
            {
                var products = Convert.ToString(row["Products"]);
                row["Products"] = !string.IsNullOrWhiteSpace(products) ? Regex.Replace(Regex.Replace(products, @" \|\| [0-9]* \|\|\|", "", RegexOptions.None), @" \|\| [0-9]*", "", RegexOptions.None) : string.Empty;

                var administrationRoutes = Convert.ToString(row["AdministrationRoutes"]);
                row["AdministrationRoutes"] = !string.IsNullOrWhiteSpace(administrationRoutes) ? administrationRoutes.Replace(" ||| ", "") : string.Empty;

                var activeSubstances = Convert.ToString(row["ActiveSubstances"]);
                row["ActiveSubstances"] = !string.IsNullOrWhiteSpace(activeSubstances) ? activeSubstances.Replace(" ||| ", "") : string.Empty;

                var excipients = Convert.ToString(row["Excipients"]);
                row["Excipients"] = !string.IsNullOrWhiteSpace(excipients) ? excipients.Replace(" ||| ", "") : string.Empty;

                var adjuvants = Convert.ToString(row["Adjuvants"]);
                row["Adjuvants"] = !string.IsNullOrWhiteSpace(adjuvants) ? adjuvants.Replace(" ||| ", "") : string.Empty;
            }

            return pharmaceuticalProductDataTable;
        }

        public string HandleMissing(object value)
        {
            if (value != null && value.ToString().Trim() != "")
            {
                return value.ToString();
            }

            return "Missing";
        }

        private void HandleListModeByProduct()
        {
            if (EntityContext != EntityContext.Product) return;

            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Product:";

            var product = _productOperations.GetEntity(_idProd);

            lblPrvParentEntity.Text = product != null && !string.IsNullOrWhiteSpace(product.name) ? product.name : Constant.ControlDefault.LbPrvText;
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
                    if (EntityContext == EntityContext.PharmaceuticalProduct) hlNewLink.NavigateUrl += "&From=PharmProd";
                    else if (EntityContext == EntityContext.Product && _idProd.HasValue) hlNewLink.NavigateUrl += string.Format("&From=ProdPharmProdList&idProd={0}", _idProd);
                    break;

                case ListType.Search:
                    hlNewLink.NavigateUrl += "&From=PharmProdSearch";
                    break;
            }
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedInsertPharmaceuticalProduct = SecurityHelper.IsPermitted(Permission.InsertPharmaceuticalProduct);
  
            if (isPermittedInsertPharmaceuticalProduct)
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