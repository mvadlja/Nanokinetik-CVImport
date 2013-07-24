using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUIFramework;
using GEM2Common;
using System.Data;
using PossGrid;
using Ready.Model;
using System.Threading;
using System.Web.UI.HtmlControls;

namespace AspNetUI.Views.AuthorisedProductView
{
    public partial class List : ListPage
    {
        #region Declarations

        private int? _idProd;
        private int? _idSearch;
        private const int NumLayoutToKeep = 5;
        private int _sortCount;
        private bool _flip = true;
        private string _gridId;

        private IProduct_PKOperations _productOperations;
        private IAuthorisedProductOperations _authorizedProductOperations;
        private IUser_grid_settings_PKOperations _userGridSettingsOperations;
        private IPerson_PKOperations _personOperations;
        private IType_PKOperations _typeOperations;
        private IOrganization_PKOperations _organizationOperations;
        private ICountry_PKOperations _countryOperations;
        private IAp_saved_search_PKOperations _savedAuthorisedProductSearchOperations;
        private IUSEROperations _userOperations;

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

            _productOperations = new Product_PKDAL();
            _authorizedProductOperations = new AuthorisedProductDAL();
            _personOperations = new Person_PKDAL();
            _typeOperations = new Type_PKDAL();
            _organizationOperations = new Organization_PKDAL();
            _countryOperations = new Country_PKDAL();
            _savedAuthorisedProductSearchOperations = new Ap_saved_search_PKDAL();
            _userGridSettingsOperations = new User_grid_settings_PKDAL();
            _userOperations = new USERDAL();

            if (ListType == ListType.Search)
            {
                APGrid.GridVersion = APGrid.GridVersion + ListType.ToString();
            }
            else if (EntityContext != EntityContext.Default)
            {
                APGrid.GridVersion = APGrid.GridVersion + EntityContext.ToString();
            }

            _gridId = APGrid.GridId + "_" + APGrid.GridVersion;
        }

        private void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
        }

        #endregion

        #region Fill

        private void ClearForm(object arg)
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

        private void FillFormControls(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    FillComboXevprmStatus();
                    FillComboAuthorisationStatus();
                    FillComboArticle57Relevant();
                    break;
                case ListType.Search:
                    FillDdlQppvs();
                    FillDdlLocalQppvs();
                    FillDdlMasterFileLocations();
                    FillDdlResponsibleUsers();
                    FillDdlLegalStatus();
                    FillDdlLocalRepresentatives();
                    FillDdlAuthorisationCountries();
                    FillDdlAuthorisationStatus();
                    break;
            }
        }

        private void SetFormControlsDefaults(object arg)
        {
            switch (ListType)
            {
                case ListType.List:
                    HandleListModeByProduct();
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
        /// Fills qppv person drop down list
        /// </summary>
        private void FillDdlQppvs()
        {
            var qppvList = _personOperations.GetPersonsByRole(Constant.PersonRoleName.Qppv);
            ddlQppv.Fill(qppvList, "FullName", "person_PK");
            ddlQppv.SortItemsByText();
        }

        /// <summary>
        /// Fills qppv person drop down list
        /// </summary>
        private void FillDdlLocalQppvs()
        {
            var localQppvList = _personOperations.GetPersonsByRole(Constant.PersonRoleName.LocalQppv);
            ddlLocalQppv.Fill(localQppvList, "FullName", "person_PK");
            ddlLocalQppv.SortItemsByText();
        }

        /// <summary>
        /// Binds master file locations drop down list
        /// </summary>
        /// <param name="args"></param>
        private void FillDdlMasterFileLocations()
        {
            var masterFileLocations = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.MasterFileLocation);
            ddlMasterFileLocation.Fill(masterFileLocations, "name_org", "organization_PK");
            ddlMasterFileLocation.SortItemsByText();
        }

        /// <summary>
        /// Fills legal status down list
        /// </summary>
        private void FillDdlLegalStatus()
        {
            var legalStatus = _typeOperations.GetTypesForDDL("LS");
            ddlLegalStatus.Fill(legalStatus, "name", "type_PK");
            ddlLegalStatus.SortItemsByText();
        }

        /// <summary>
        /// Fills local representatives drop down list
        /// </summary>
        private void FillDdlLocalRepresentatives()
        {
            var localRepresentatives = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.LocalRepresentative);
            ddlLocalRepresentative.Fill(localRepresentatives, "name_org", "organization_PK");
            ddlLocalRepresentative.SortItemsByText();
        }

        /// <summary>
        /// Fills authorisation countries drop down list
        /// </summary>
        private void FillDdlAuthorisationCountries()
        {
            var authorisationCountries = _countryOperations.GetEntitiesCustomSort();
            ddlAuthorisationCountry.Fill(authorisationCountries, Constant.Countries.DisplayNameFormat, "country_PK");
        }

        /// <summary>
        /// Binds authorisation countries drop down list
        /// </summary>
        private void FillDdlAuthorisationStatus()
        {
            var authorisationStatus = _typeOperations.GetTypesForDDL("AS");
            ddlAuthorisationStatus.Fill(authorisationStatus, "name", "type_PK");
            ddlAuthorisationStatus.SortItemsByText();
        }

        private void FillComboArticle57Relevant()
        {
            var article57RelevantList = new List<ListItem>
                                            {
                                                new ListItem(""), 
                                                new ListItem("Yes"), 
                                                new ListItem("No")
                                            };
            APGrid.SetComboList("Article57Relevant", article57RelevantList);
        }

        private void FillComboAuthorisationStatus()
        {
            var authorisationStatus = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.AuthorisationStatus);
            var authorisationStatusList = new List<ListItem> { new ListItem("", "") };
            authorisationStatus.ForEach(item => authorisationStatusList.Add(new ListItem(item.name)));
            APGrid.SetComboList("AuthorisationStatus", authorisationStatusList);
        }

        private void FillComboXevprmStatus()
        {
            var xevprmStatusList = new List<ListItem>() {new ListItem("","")};
            xevprmStatusList.AddRange(from operationType in new[] {"Insert", "Update", "Variation", "Nullify", "Withdraw"}
                                      from status in new[] { "Created", "Validation failed", "Validation OK", "Ready to submit", "Submission aborted", "MDN pending", 
                                          "MDN received error", "MDN received successful", "ACK received", "ACK delivery failed", "ACK delivered" }
                                      select new ListItem(string.Format("{0} ({1})", status, operationType)));

            APGrid.SetComboList("XevprmStatus", xevprmStatusList);
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                APGrid.SetClientLayoutBeforeBind(userGridSettings.grid_layout);
            }

            if (!_idSearch.HasValue || Request.QueryString["Clear"] == "true") return;

            var savedAuthorisedProductSearch = _savedAuthorisedProductSearchOperations.GetEntity(_idSearch);

            if (savedAuthorisedProductSearch == null) return;

            if (ListType == ListType.Search)
            {
                ddlAuthorisationCountry.SelectedId = savedAuthorisedProductSearch.authorisationcountrycode_FK;
                dtRngAuthorisationDate.TextFrom = savedAuthorisedProductSearch.authorisationdateFrom.HasValue ? savedAuthorisedProductSearch.authorisationdateFrom.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngAuthorisationDate.TextTo = savedAuthorisedProductSearch.authorisationdateTo.HasValue ? savedAuthorisedProductSearch.authorisationdateTo.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngAuthorisationExpiryDate.TextFrom = savedAuthorisedProductSearch.authorisationexpdateFrom.HasValue ? savedAuthorisedProductSearch.authorisationexpdateFrom.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngAuthorisationExpiryDate.TextTo = savedAuthorisedProductSearch.authorisationexpdateTo.HasValue ? savedAuthorisedProductSearch.authorisationexpdateTo.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngSunsetClause.TextFrom = savedAuthorisedProductSearch.sunsetclauseFrom.HasValue ? savedAuthorisedProductSearch.sunsetclauseFrom.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                dtRngSunsetClause.TextTo = savedAuthorisedProductSearch.sunsetclauseTo.HasValue ? savedAuthorisedProductSearch.sunsetclauseTo.Value.ToString(Constant.DateTimeFormat) : string.Empty;
                ddlQppv.SelectedId = savedAuthorisedProductSearch.qppv_person_FK;
                ddlLocalQppv.SelectedId = savedAuthorisedProductSearch.local_qppv_person_FK;
                ddlAuthorisationStatus.SelectedId = savedAuthorisedProductSearch.authorisationstatus_FK;
                ddlLegalStatus.SelectedId = ValidationHelper.IsValidInt(savedAuthorisedProductSearch.legalstatus) ? (int?)Convert.ToInt32(savedAuthorisedProductSearch.legalstatus) : null;
                if (savedAuthorisedProductSearch.marketed.HasValue) rbYnMarketed.SelectedValue = savedAuthorisedProductSearch.marketed.Value;
                if (savedAuthorisedProductSearch.article57_reporting.HasValue) rbYnArticle57Reporting.SelectedValue = savedAuthorisedProductSearch.article57_reporting.Value;
                if (savedAuthorisedProductSearch.substance_translations.HasValue) rbYnSubstanceTranslations.SelectedValue = savedAuthorisedProductSearch.substance_translations.Value;
                BindLicenceHolder(savedAuthorisedProductSearch.organizationmahcode_FK);
                ddlMasterFileLocation.SelectedId = savedAuthorisedProductSearch.mflcode_FK;                
                ddlLocalRepresentative.SelectedId = savedAuthorisedProductSearch.local_representative_FK;
                txtPackageDescription.Text = savedAuthorisedProductSearch.packagedesc;
                BindRelatedProduct(savedAuthorisedProductSearch.product_FK);
                txtFullPresentationName.Text = savedAuthorisedProductSearch.productshortname;
                ddlResponsibleUser.SelectedId = savedAuthorisedProductSearch.responsible_user_person_FK;
                txtEvcode.Text = savedAuthorisedProductSearch.ev_code;
                BindClient(savedAuthorisedProductSearch.client_org_FK);
            }

            APGrid.SetClientLayout(savedAuthorisedProductSearch.gridLayout);
        }

        private void BindGrid()
        {
            var filters = GetFilters();

            var gobList = new List<GEMOrderBy> { };
            if (APGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(APGrid.SecondSortingColumn, APGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (APGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(APGrid.MainSortingColumn, APGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("ap_PK", GEMOrderByType.DESC));

            int itemCount = 0;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _authorizedProductOperations.GetListFormSearchDataSet(filters, APGrid.CurrentPage, APGrid.PageSize, gobList, out itemCount); // Quick link
                else ds = _authorizedProductOperations.GetListFormDataSet(filters, APGrid.CurrentPage, APGrid.PageSize, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _authorizedProductOperations.GetListFormSearchDataSet(filters, APGrid.CurrentPage, APGrid.PageSize, gobList, out itemCount);
            }

            APGrid.TotalRecords = itemCount;

            if (APGrid.CurrentPage > APGrid.TotalPages || (APGrid.CurrentPage == 0 && APGrid.TotalPages > 0))
            {
                APGrid.CurrentPage = APGrid.CurrentPage > APGrid.TotalPages ? APGrid.TotalPages : 1;
                if (ListType == ListType.List)
                {
                    if (_idSearch.HasValue) ds = _authorizedProductOperations.GetListFormSearchDataSet(filters, APGrid.CurrentPage, APGrid.PageSize, gobList, out itemCount); // Quick link
                    else ds = _authorizedProductOperations.GetListFormDataSet(filters, APGrid.CurrentPage, APGrid.PageSize, gobList, out itemCount);
                }
                else if (ListType == ListType.Search)
                {
                    ds = _authorizedProductOperations.GetListFormSearchDataSet(filters, APGrid.CurrentPage, APGrid.PageSize, gobList, out itemCount);
                }
            }

            APGrid.DataSource = ds != null ? ds.Tables[0].DefaultView : null;
            APGrid.DataBind();

            MasterPage.UpCommon.Update();
        }

        private void BindGridDynamicControls()
        {
            FillComboXevprmStatus();
            FillComboAuthorisationStatus();
            FillComboArticle57Relevant();
        }

        private void BindDynamicControls(object args)
        {
            if (ListType == ListType.Search) subtabs.Controls.Clear();
        }

        private void BindLicenceHolder(int? organizationmahcodeFk)
        {
            var licenceHolder = _organizationOperations.GetEntity(organizationmahcodeFk);
            if (licenceHolder == null || licenceHolder.organization_PK == null) return;

            txtSrLicenceHolder.SelectedEntityId = licenceHolder.organization_PK;
            txtSrLicenceHolder.Text = !string.IsNullOrWhiteSpace(licenceHolder.name_org) ? licenceHolder.name_org : Constant.MissingValue;
        }

        private void BindClient(int? clientOrgFk)
        {
            var client = _organizationOperations.GetEntity(clientOrgFk);
            if (client == null || client.organization_PK == null) return;

            txtSrClient.SelectedEntityId = client.organization_PK;
            txtSrClient.Text = !string.IsNullOrWhiteSpace(client.name_org) ? client.name_org : Constant.MissingValue;
        }

        private void BindRelatedProduct(int? productFk)
        {
            var relatedProduct = _productOperations.GetEntity(productFk);
            if (relatedProduct == null || relatedProduct.product_PK == null) return;

            txtSrRelatedProduct.SelectedEntityId = relatedProduct.product_PK;
            txtSrRelatedProduct.Text = relatedProduct.GetNameFormatted();
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                MasterPage.ModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }

            return true;
        }

        private void ClearValidationErrors()
        {

        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            return null;
        }

        #endregion

        #region Delete

        private void DeleteEntity(object arg)
        {

        }

        #endregion

        #endregion

        #region Event handlers

        #region Grid

        void APGrid_OnExcelCellPrepared(object sender, PossGrid.PossGridExportCellRenderArgs args)
        {
            if (args.RowType != DataControlRowType.DataRow) return;
        }

        void APGrid_OnHtmlRowPrepared(object sender, PossGrid.PossGridRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) return;

            string colorStatus = Convert.ToString(e.GetValue("AuthorisationStatus"));

            var pnlStatusColor = e.FindControl("pnlStatusColorAP") as HtmlGenericControl;
            if (pnlStatusColor != null)
            {
                switch (colorStatus)
                {
                    case "Valid (after renewal)":
                    case "Valid":
                        pnlStatusColor.Attributes.Add("class", "statusGreen");
                        break;
                    case "Planned":
                    case "Pending":
                        pnlStatusColor.Attributes.Add("class", "statusYellow");
                        break;
                    default:
                        pnlStatusColor.Attributes.Add("class", "statusBlack");
                        break;
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

        void APGrid_OnRebindRequired(PossGrid.PossGrid grid)
        {
            BindGrid();
        }

        void APGrid_OnHtmlCellPrepared(object sender, PossGrid.PossGridCellEventArgs e)
        {
            if (!APGrid.IsSorted(e.FieldName)) return;

            if (_sortCount == 0)
            {
                _sortCount = APGrid.SortCount;
                _flip = !_flip;
            }
            _sortCount--;

            if (APGrid.SortCount > 1 && e.FieldName == APGrid.MainSortingColumn)
                return;

            if (e.FieldName != "AuthorisationExpiryDate" && e.FieldName != "AuthorisationExpiryDate" && e.FieldName != "DocumentsCount" && e.FieldName != "AuthorisationDate")
            {
                e.Cell.CssClass = _flip ? "sorted_column_even" : "sorted_column_odd";
            }
            else
            {
                e.Cell.CssClass = _flip ? "sorted_column_even_right" : "sorted_column_odd_right";
            }
        }

        void APGrid_OnLoadClientLayout(object sender, PossGrid.ClientLayoutEventArgs args)
        {
            var userGridSettings = _userGridSettingsOperations.GetDefaultLayoutByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            if (userGridSettings != null && !String.IsNullOrWhiteSpace(userGridSettings.grid_layout))
            {
                args.ClientLayoutString = userGridSettings.grid_layout;
            }
        }

        #endregion

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.New:
                    {
                        MasterPage.OneTimePermissionToken = Permission.View;
                        if (EntityContext == EntityContext.Product && _idProd.HasValue) Response.Redirect(string.Format("~/Views/AuthorisedProductView/Form.aspx?EntityContext={0}&Action=New&idProd={1}&From=ProdAuthProdList", EntityContext.Product, _idProd.Value));
                        else Response.Redirect(string.Format("~/Views/AuthorisedProductView/Form.aspx?EntityContext={0}&Action=New&From=AuthProd", EntityContext.Default));
                    }
                    break;

                case ContextMenuEventTypes.Back:
                    {
                        if (EntityContext == EntityContext.AuthorisedProduct) Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                        Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext.AuthorisedProduct));
                    }
                    break;
            }
        }

        void btnColumns_OnClick(object sender, EventArgs e)
        {
            ColumnsPopup.SelectedColumns.Clear();
            ColumnsPopup.AvailableColumns.Clear();

            foreach (DataControlField column in APGrid.Columns)
            {
                if (column is IFilteredColumn && (column as IFilteredColumn).FieldName.NotIn("ap_PK", "ActiveSubstances"))
                {
                    string caption = !string.IsNullOrWhiteSpace(column.HeaderText) ? column.HeaderText : Constant.NoCaption;

                    if (APGrid.VisibleColumns.Contains((column as IFilteredColumn).FieldName))
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
            APGrid.VisibleColumns = ColumnsPopup.SelectedColumns.Cast<ListItem>().Select(x => x.Value).ToList();

            BindGrid();
        }

        void btnClearLayout_Click(object sender, EventArgs e)
        {
            APGrid.ClearFilters();
        }

        void btnSaveLayout_Click(object sender, EventArgs e)
        {
            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);

            var userGridSettings = new User_grid_settings_PK
            {
                grid_ID = _gridId,
                user_FK = user != null ? user.User_PK : null,
                grid_layout = APGrid.GetClientLayoutString(),
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
            if (APGrid.SecondSortingColumn != null) gobList.Add(new GEMOrderBy(APGrid.SecondSortingColumn, APGrid.SecondSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));

            if (APGrid.MainSortingColumn != null) gobList.Add(new GEMOrderBy(APGrid.MainSortingColumn, APGrid.MainSortingOrder == PossGrid.SortOrder.ASC ? GEMOrderByType.ASC : GEMOrderByType.DESC));
            if (gobList.Count == 0) gobList.Add(new GEMOrderBy("ap_PK", GEMOrderByType.DESC));

            int itemCount;
            DataSet ds = null;

            if (ListType == ListType.List)
            {
                if (_idSearch.HasValue) ds = _authorizedProductOperations.GetListFormSearchDataSet(filters, 1, int.MaxValue, gobList, out itemCount); // Quick link
                else ds = _authorizedProductOperations.GetListFormDataSet(filters, 1, int.MaxValue, gobList, out itemCount);
            }
            else if (ListType == ListType.Search)
            {
                ds = _authorizedProductOperations.GetListFormSearchDataSet(filters, 1, int.MaxValue, gobList, out itemCount);
            }

            APGrid["ap_PK"].Visible = true;
            APGrid["ActiveSubstances"].Visible = true;
            if (ds != null) APGrid.ExportDataToXlsx(ds.Tables[0], new PossGrid.ExcellExportOptions("grid"));
            APGrid["ap_PK"].Visible = false;
            APGrid["ActiveSubstances"].Visible = false;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _userGridSettingsOperations.DeleteLayoutsByUsernameAndGrid(Thread.CurrentPrincipal.Identity.Name, _gridId);
            APGrid.ResetVisibleColumns();
            APGrid.SecondSortingColumn = null;
            APGrid.MainSortingOrder = PossGrid.SortOrder.ASC;
            
            BindGrid();

        }

        #endregion

        #region Related product searcher

        /// <summary>
        /// Handles related product list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void RelatedProductSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var product = _productOperations.GetEntity(e.Data);

            if (product == null || product.product_PK == null) return;

            txtSrRelatedProduct.Text = product.GetNameFormatted(string.Empty);
            txtSrRelatedProduct.SelectedEntityId = product.product_PK;
        }

        #endregion

        #region Licence holder searcher

        /// <summary>
        /// Handles licence holder list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void LicenceHolderSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var licenceHolder = _organizationOperations.GetEntity(e.Data);

            if (licenceHolder == null || licenceHolder.organization_PK == null) return;

            txtSrLicenceHolder.Text = licenceHolder.name_org;
            txtSrLicenceHolder.SelectedEntityId = licenceHolder.organization_PK;
        }

        #endregion

        #region Client

        /// <summary>
        /// Handles Client list item selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ClientSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var client = _organizationOperations.GetEntity(e.Data);

            if (client == null || client.organization_PK == null) return;

            txtSrClient.Text = client.name_org;
            txtSrClient.SelectedEntityId = client.organization_PK;
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
            Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}&Action=Search&Clear=true{1}", EntityContext.AuthorisedProduct, (_idSearch.HasValue ? "&idSearch=" + _idSearch : string.Empty)));
        }

        #endregion

        #region Quick links

        public void btnSaveSearchClick(object sender, EventArgs e)
        {
            QuickLink quickLink = null;
            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                var savedSearch = _savedAuthorisedProductSearchOperations.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
                if (savedSearch != null)
                {
                    quickLink = new QuickLink
                    {
                        Name = savedSearch.displayName,
                        IsPublic = savedSearch.ispublic
                    };
                }
            }

            QuickLinksPopup.ShowModalForm(quickLink);
        }

        public void btnDeleteSearchClick(object sender, EventArgs e)
        {
            if (!ValidationHelper.IsValidInt(Request.QueryString["idSearch"])) return;

            _savedAuthorisedProductSearchOperations.Delete(Convert.ToInt32(Request.QueryString["idSearch"]));
            Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}&Action=Search", EntityContext.AuthorisedProduct));
        }

        protected void QuickLinksPopup_OnOkButtonClick(object sender, FormEventArgs<QuickLink> e)
        {
            Ap_saved_search_PK savedAuthorisedProductSearch = null;

            if (ValidationHelper.IsValidInt(Request.QueryString["idSearch"]))
            {
                savedAuthorisedProductSearch =
                    _savedAuthorisedProductSearchOperations.GetEntity(Convert.ToInt32(Request.QueryString["idSearch"]));
            }

            if (savedAuthorisedProductSearch == null)
            {
                savedAuthorisedProductSearch = new Ap_saved_search_PK();
            }

            var authorisationDateFrom = ValidationHelper.IsValidDateTime(dtRngAuthorisationDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngAuthorisationDate.TextFrom, CultureInfoHr) : null;
            var authorisationDateTo = ValidationHelper.IsValidDateTime(dtRngAuthorisationDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngAuthorisationDate.TextTo, CultureInfoHr) : null;
            var authorisationExpiryDateFrom = ValidationHelper.IsValidDateTime(dtRngAuthorisationExpiryDate.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngAuthorisationExpiryDate.TextFrom, CultureInfoHr) : null;
            var authorisationExpiryDateTo = ValidationHelper.IsValidDateTime(dtRngAuthorisationExpiryDate.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngAuthorisationExpiryDate.TextTo, CultureInfoHr) : null;
            var sunsetClauseFrom = ValidationHelper.IsValidDateTime(dtRngSunsetClause.TextFrom, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngSunsetClause.TextFrom, CultureInfoHr) : null;
            var sunsetClauseTo = ValidationHelper.IsValidDateTime(dtRngSunsetClause.TextTo, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtRngSunsetClause.TextTo, CultureInfoHr) : null;

            savedAuthorisedProductSearch.authorisationcountrycode_FK = ddlAuthorisationCountry.SelectedId;
            savedAuthorisedProductSearch.authorisationdateFrom = authorisationDateFrom;
            savedAuthorisedProductSearch.authorisationdateTo = authorisationDateTo;
            savedAuthorisedProductSearch.authorisationexpdateFrom = authorisationExpiryDateFrom;
            savedAuthorisedProductSearch.authorisationexpdateTo = authorisationExpiryDateTo;
            savedAuthorisedProductSearch.sunsetclauseFrom = sunsetClauseFrom;
            savedAuthorisedProductSearch.sunsetclauseTo = sunsetClauseTo;
            savedAuthorisedProductSearch.qppv_person_FK = ddlQppv.SelectedId;
            savedAuthorisedProductSearch.local_qppv_person_FK = ddlLocalQppv.SelectedId;
            savedAuthorisedProductSearch.authorisationstatus_FK = ddlAuthorisationStatus.SelectedId;
            savedAuthorisedProductSearch.legalstatus = Convert.ToString(ddlLegalStatus.SelectedId);
            savedAuthorisedProductSearch.marketed = rbYnMarketed.SelectedValue;
            savedAuthorisedProductSearch.article57_reporting = rbYnArticle57Reporting.SelectedValue;
            savedAuthorisedProductSearch.substance_translations = rbYnSubstanceTranslations.SelectedValue;
            savedAuthorisedProductSearch.organizationmahcode_FK = txtSrLicenceHolder.SelectedEntityId;
            savedAuthorisedProductSearch.mflcode_FK = ddlMasterFileLocation.SelectedId;
            savedAuthorisedProductSearch.local_representative_FK = ddlLocalRepresentative.SelectedId;
            savedAuthorisedProductSearch.packagedesc = txtPackageDescription.Text;
            savedAuthorisedProductSearch.product_FK = txtSrRelatedProduct.SelectedEntityId;
            savedAuthorisedProductSearch.productshortname = txtFullPresentationName.Text;
            savedAuthorisedProductSearch.responsible_user_person_FK = ddlResponsibleUser.SelectedId;
            savedAuthorisedProductSearch.ev_code = txtEvcode.Text;
            savedAuthorisedProductSearch.client_org_FK = txtSrClient.SelectedEntityId;
            savedAuthorisedProductSearch.indications = txtIndications.Text;
            savedAuthorisedProductSearch.gridLayout = APGrid.GetClientLayoutString();

            var quickLink = e.Data;
            if (quickLink != null)
            {
                savedAuthorisedProductSearch.displayName = quickLink.Name;
                savedAuthorisedProductSearch.ispublic = quickLink.IsPublic.HasValue && quickLink.IsPublic.Value;
            }

            var user = _userOperations.GetUserByUsername(SessionManager.Instance.CurrentUser.Username);
            if (user != null)
            {
                savedAuthorisedProductSearch.user_fk = user.Person_FK;
            }

            savedAuthorisedProductSearch = _savedAuthorisedProductSearchOperations.Save(savedAuthorisedProductSearch);
            Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}&Action=Search&idSearch={1}", EntityContext.AuthorisedProduct, savedAuthorisedProductSearch.ap_saved_search_PK));
        }

        #endregion

        #endregion

        #region Support

        private void ClearSearch()
        {
            txtSrRelatedProduct.Text = String.Empty;
            txtEvcode.Text = String.Empty;
            txtFullPresentationName.Text = String.Empty;
            txtPackageDescription.Text = String.Empty;
            ddlResponsibleUser.SelectedValue = String.Empty;
            ddlLegalStatus.SelectedValue = String.Empty;
            txtSrLicenceHolder.Text = String.Empty;
            ddlMasterFileLocation.SelectedValue = String.Empty;
            ddlLocalRepresentative.SelectedValue = String.Empty;
            ddlQppv.SelectedValue = String.Empty;
            ddlLocalQppv.SelectedValue = String.Empty;
            txtIndications.Text = String.Empty;
            ddlAuthorisationCountry.SelectedValue = String.Empty;
            ddlAuthorisationStatus.SelectedValue = String.Empty;
            txtSrClient.Text = String.Empty;
            dtRngAuthorisationDate.Clear();
            dtRngAuthorisationExpiryDate.Clear();
            dtRngSunsetClause.Clear();
        }

        private Dictionary<string, string> GetFilters()
        {
            var filters = APGrid.GetFilters();

            switch (ListType)
            {
                case ListType.List:
                    if (_idSearch.HasValue)
                    {
                        Ap_saved_search_PK savedAuthorisedProductSearch = _savedAuthorisedProductSearchOperations.GetEntity(_idSearch);
                        FillFilters(savedAuthorisedProductSearch, filters);
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
            if (txtSrRelatedProduct.SelectedEntityId.HasValue) filters.Add("SearchRelatedProductPk", txtSrRelatedProduct.SelectedEntityId.Value.ToString());
            if (rbYnArticle57Reporting.SelectedItem != null) filters.Add("SearchArcticle57", rbYnArticle57Reporting.SelectedValue != null && rbYnArticle57Reporting.SelectedValue.Value ? "Yes" : "No"); else filters.Add("SearchArcticle57", "");
            if (rbYnSubstanceTranslations.SelectedItem != null) filters.Add("SearchSubstanceTranslations", rbYnSubstanceTranslations.SelectedValue != null && rbYnSubstanceTranslations.SelectedValue.Value ? "Yes" : "No"); else filters.Add("SearchSubstanceTranslations", "");
            if (!string.IsNullOrWhiteSpace(txtEvcode.Text)) filters.Add("SearchEvcode", txtEvcode.Text);
            if (!string.IsNullOrWhiteSpace(txtFullPresentationName.Text)) filters.Add("SearchFullPresentationName", txtFullPresentationName.Text);
            if (!string.IsNullOrWhiteSpace(txtPackageDescription.Text)) filters.Add("SearchPackageDescription", txtPackageDescription.Text);
            if (ddlResponsibleUser.SelectedId.HasValue) filters.Add("SearchResponsibleUserPk", ddlResponsibleUser.SelectedId.Value.ToString());
            if (rbYnMarketed.SelectedItem != null) filters.Add("SearchMarketed", rbYnMarketed.SelectedValue != null && rbYnMarketed.SelectedValue.Value ? "Yes" : "No"); else filters.Add("SearchMarketed", "");
            if (ddlLegalStatus.SelectedId.HasValue) filters.Add("SearchLegalStatusPk", ddlLegalStatus.SelectedId.Value.ToString());
            if (txtSrLicenceHolder.SelectedEntityId.HasValue) filters.Add("SearchLicenceHolderPk", txtSrLicenceHolder.SelectedEntityId.Value.ToString());
            if (ddlMasterFileLocation.SelectedId.HasValue) filters.Add("SearchMasterFileLocationPk", ddlMasterFileLocation.SelectedId.Value.ToString());
            if (ddlLocalRepresentative.SelectedId.HasValue) filters.Add("SearchLocalRepresentativePk", ddlLocalRepresentative.SelectedId.Value.ToString());
            if (ddlQppv.SelectedId.HasValue) filters.Add("SearchQppvPk", ddlQppv.SelectedId.Value.ToString());
            if (ddlLocalQppv.SelectedId.HasValue) filters.Add("SearchLocalQppvPk", ddlLocalQppv.SelectedId.Value.ToString());
            if (!string.IsNullOrWhiteSpace(txtIndications.Text)) filters.Add("SearchIndications", txtIndications.Text);
            if (ddlAuthorisationCountry.SelectedId.HasValue) filters.Add("SearchAuthorisationCountryPk", ddlAuthorisationCountry.SelectedId.Value.ToString());
            if (ddlAuthorisationStatus.SelectedId.HasValue) filters.Add("SearchAuthorisationStatusPk", ddlAuthorisationStatus.SelectedId.Value.ToString());
            if (txtSrClient.SelectedEntityId.HasValue) filters.Add("SearchClientPk", txtSrClient.SelectedEntityId.Value.ToString());
            if (ValidationHelper.IsValidDateTime(dtRngAuthorisationDate.TextFrom, CultureInfoHr)) filters.Add("SearchAuthorisationDateFrom", dtRngAuthorisationDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngAuthorisationDate.TextTo, CultureInfoHr)) filters.Add("SearchAuthorisationDateTo", dtRngAuthorisationDate.TextTo);
            if (ValidationHelper.IsValidDateTime(dtRngAuthorisationExpiryDate.TextFrom, CultureInfoHr)) filters.Add("SearchAuthorisationExpiryDateFrom", dtRngAuthorisationExpiryDate.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngAuthorisationExpiryDate.TextTo, CultureInfoHr)) filters.Add("SearchAuthorisationExpiryDateTo", dtRngAuthorisationExpiryDate.TextTo);
            if (ValidationHelper.IsValidDateTime(dtRngSunsetClause.TextFrom, CultureInfoHr)) filters.Add("SearchSunsetClauseFrom", dtRngSunsetClause.TextFrom);
            if (ValidationHelper.IsValidDateTime(dtRngSunsetClause.TextTo, CultureInfoHr)) filters.Add("SearchSunsetClauseTo", dtRngSunsetClause.TextTo);
        }

        private void FillFilters(Ap_saved_search_PK savedAuthorisedProductSearch, Dictionary<string, string> filters)
        {
            if (savedAuthorisedProductSearch.product_FK.HasValue) filters.Add("SearchRelatedProductPk", savedAuthorisedProductSearch.product_FK.Value.ToString());
            if (savedAuthorisedProductSearch.article57_reporting.HasValue) filters.Add("SearchArcticle57", savedAuthorisedProductSearch.article57_reporting.Value ? "Yes" : "No"); else filters.Add("SearchArcticle57", "");
            if (savedAuthorisedProductSearch.substance_translations.HasValue) filters.Add("SearchSubstanceTranslations", savedAuthorisedProductSearch.substance_translations.Value ? "Yes" : "No"); else filters.Add("SearchSubstanceTranslations", "");
            if (!string.IsNullOrWhiteSpace(savedAuthorisedProductSearch.ev_code)) filters.Add("SearchEvcode", savedAuthorisedProductSearch.ev_code);
            if (!string.IsNullOrWhiteSpace(savedAuthorisedProductSearch.productshortname)) filters.Add("SearchFullPresentationName", savedAuthorisedProductSearch.productshortname);
            if (!string.IsNullOrWhiteSpace(savedAuthorisedProductSearch.packagedesc)) filters.Add("SearchPackageDescription", savedAuthorisedProductSearch.packagedesc);
            if (savedAuthorisedProductSearch.responsible_user_person_FK.HasValue) filters.Add("SearchResponsibleUserPk", savedAuthorisedProductSearch.responsible_user_person_FK.Value.ToString());
            if (savedAuthorisedProductSearch.marketed.HasValue) filters.Add("SearchMarketed", savedAuthorisedProductSearch.marketed.Value ? "Yes" : "No"); else filters.Add("SearchMarketed", "");
            if (!string.IsNullOrWhiteSpace(savedAuthorisedProductSearch.legalstatus)) filters.Add("SearchLegalStatusPk", savedAuthorisedProductSearch.legalstatus);
            if (savedAuthorisedProductSearch.organizationmahcode_FK.HasValue) filters.Add("SearchLicenceHolderPk", savedAuthorisedProductSearch.organizationmahcode_FK.Value.ToString());
            if (savedAuthorisedProductSearch.mflcode_FK.HasValue) filters.Add("SearchMasterFileLocationPk", savedAuthorisedProductSearch.mflcode_FK.Value.ToString());
            if (savedAuthorisedProductSearch.local_representative_FK.HasValue) filters.Add("SearchLocalRepresentativePk", savedAuthorisedProductSearch.local_representative_FK.Value.ToString());
            if (savedAuthorisedProductSearch.qppv_person_FK.HasValue) filters.Add("SearchQppvPk", savedAuthorisedProductSearch.qppv_person_FK.Value.ToString());
            if (savedAuthorisedProductSearch.local_qppv_person_FK.HasValue) filters.Add("SearchLocalQppvPk", savedAuthorisedProductSearch.local_qppv_person_FK.Value.ToString());
            if (!string.IsNullOrWhiteSpace(savedAuthorisedProductSearch.indications)) filters.Add("SearchIndications", savedAuthorisedProductSearch.indications);
            if (savedAuthorisedProductSearch.authorisationcountrycode_FK.HasValue) filters.Add("SearchAuthorisationCountryPk", savedAuthorisedProductSearch.authorisationcountrycode_FK.Value.ToString());
            if (savedAuthorisedProductSearch.authorisationstatus_FK.HasValue) filters.Add("SearchAuthorisationStatusPk", savedAuthorisedProductSearch.authorisationstatus_FK.Value.ToString());
            if (savedAuthorisedProductSearch.client_org_FK.HasValue) filters.Add("SearchClientPk", savedAuthorisedProductSearch.client_org_FK.Value.ToString());
            if (savedAuthorisedProductSearch.authorisationdateFrom.HasValue) filters.Add("SearchAuthorisationDateFrom", savedAuthorisedProductSearch.authorisationdateFrom.Value.ToString(Constant.DateTimeFormat));
            if (savedAuthorisedProductSearch.authorisationdateTo.HasValue) filters.Add("SearchAuthorisationDateTo", savedAuthorisedProductSearch.authorisationdateTo.Value.ToString(Constant.DateTimeFormat));
            if (savedAuthorisedProductSearch.authorisationexpdateFrom.HasValue) filters.Add("SearchAuthorisationExpiryDateFrom", savedAuthorisedProductSearch.authorisationexpdateFrom.Value.ToString(Constant.DateTimeFormat));
            if (savedAuthorisedProductSearch.authorisationexpdateTo.HasValue) filters.Add("SearchAuthorisationExpiryDateTo", savedAuthorisedProductSearch.authorisationexpdateTo.Value.ToString(Constant.DateTimeFormat));
            if (savedAuthorisedProductSearch.sunsetclauseFrom.HasValue) filters.Add("SearchSunsetClauseFrom", savedAuthorisedProductSearch.sunsetclauseFrom.Value.ToString(Constant.DateTimeFormat));
            if (savedAuthorisedProductSearch.sunsetclauseTo.HasValue) filters.Add("SearchSunsetClauseTo", savedAuthorisedProductSearch.sunsetclauseTo.Value.ToString(Constant.DateTimeFormat));
            if (!string.IsNullOrWhiteSpace(savedAuthorisedProductSearch.gridLayout)) APGrid.SetClientLayout(savedAuthorisedProductSearch.gridLayout);
        }

        private void GenerateContextMenuItems()
        {
            var contextMenu = new ContextMenuItem[] { };

            if (ListType == ListType.List)
            {
                switch (EntityContext)
                {
                    case EntityContext.Default:
                    case EntityContext.AuthorisedProduct:
                        contextMenu = new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") };
                        break;
                    case EntityContext.Product:
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
                if (EntityContext == EntityContext.AuthorisedProduct)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("AuthProd", Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                    if (location != null)
                    {
                        MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                        MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    }
                }
                else if (EntityContext == EntityContext.Product)
                {
                    location = Support.LocationManager.Instance.GetLocationByName("ProdAuthProdList", Support.CacheManager.Instance.AppLocations);
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
                location = Support.LocationManager.Instance.GetLocationByName("AuthProdSearch", Support.CacheManager.Instance.AppLocations);
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
            //    if(EntityContext == EntityContext.AuthorisedProduct) location = Support.LocationManager.Instance.GetLocationByName("AuthProd", Support.CacheManager.Instance.AppLocations);
            //}
            //else if (ListType == ListType.Search)
            //{
            //    location = Support.LocationManager.Instance.GetLocationByName("AuthProdSearch", Support.CacheManager.Instance.AppLocations);
            //}

            //if (location != null)
            //{
            //    var topLevelParent = MasterPage.FindTopLevelParent(location);

            //    MasterPage.CurrentLocation = location;
            //    MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            //}
        }

        private void HandleListModeByProduct()
        {
            if (EntityContext != EntityContext.Product) return;

            lblPrvParentEntity.Visible = true;
            lblPrvParentEntity.Label = "Product:";

            var product = _productOperations.GetEntity(_idProd);

            lblPrvParentEntity.Text = product != null && !string.IsNullOrWhiteSpace(product.name) ? product.name : Constant.ControlDefault.LbPrvText;
        }

        public string HandleMissing(object value)
        {
            if (value != null && value.ToString().Trim() != "")
            {
                return value.ToString();
            }

            return "Missing";
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
                    txtSrRelatedProduct.Searcher.OnListItemSelected += RelatedProductSearcher_OnListItemSelected;
                    txtSrClient.Searcher.OnListItemSelected += ClientSearcher_OnListItemSelected;
                    txtSrLicenceHolder.Searcher.OnListItemSelected += LicenceHolderSearcher_OnListItemSelected;

                    QuickLinksPopup.OnOkButtonClick += QuickLinksPopup_OnOkButtonClick;

                    btnExportLower.Click += btnExport_Click;
                    break;
            }

            APGrid.OnRebindRequired += APGrid_OnRebindRequired;
            APGrid.OnHtmlRowPrepared += APGrid_OnHtmlRowPrepared;
            APGrid.OnHtmlCellPrepared += APGrid_OnHtmlCellPrepared;
            APGrid.OnExcelCellPrepared += APGrid_OnExcelCellPrepared;
            APGrid.OnLoadClientLayout += APGrid_OnLoadClientLayout;
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
                    if (EntityContext == EntityContext.AuthorisedProduct) hlNewLink.NavigateUrl += "&From=AuthProd";
                    else if (EntityContext == EntityContext.Product && _idProd.HasValue) hlNewLink.NavigateUrl += string.Format("&From=ProdAuthProdList&idProd={0}", _idProd);
                    break;

                case ListType.Search:
                    hlNewLink.NavigateUrl += "&From=AuthProdSearch";
                    break;
            }
        }


        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedInsertAuthorisedProduct = SecurityHelper.IsPermitted(Permission.InsertAuthorisedProduct);

            if (isPermittedInsertAuthorisedProduct) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });
            else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.New, "New") });

            return true;
        }

        #endregion
    }
}