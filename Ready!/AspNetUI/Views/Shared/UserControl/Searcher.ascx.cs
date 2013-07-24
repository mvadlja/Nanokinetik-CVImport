using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using AspNetUI.Support;
using AspNetUIFramework;
using GEM2Common;
using Ready.Model;

namespace AspNetUI.Views.Shared.UserControl
{
    public enum SelectMode
    {
        Single,
        Multiple
    }

    public enum SearchType
    {
        Null,
        Atc,
        Product,
        AuthorisedProduct,
        LicenceHolder,
        Substance,
        AdministrationRoute,
        MedicalDevice,
        PpiAttachment,
        Distributor,
        MasterFileLocation,
        Qppv,
        LocalQPPV,
        Manufacturer,
        Client,
        ProductIndication,
        PharmaceuticalProduct,
        Project,
        Applicant,
        Activity,
        Task,
        ActiveIngredient,
        PersonEmail,
        Document
    }

    public partial class Searcher : System.Web.UI.UserControl
    {
        #region Declarations

        private IProduct_PKOperations _productOperations;
        private IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        private ISubstance_PKOperations _substanceOperations;
        private IPerson_PKOperations _personOperations;
        private IQppv_code_PKOperations _qppvCodeOperations;
        private IAtc_PKOperations _atcOperations;
        private IAdminroute_PKOperations _administrationRouteOperations;
        private IMedicaldevice_PKOperations _medicalDeviceOperations;
        private IOrganization_in_role_Operations _organizationInRoleOperations;
        private IActivity_PKOperations _activityOperations;
        private IProject_PKOperations _projectOperations;
        private ITask_PKOperations _taskOperations;
        private IAuthorisedProductOperations _authorisedProductOperations;
        private IDocument_PKOperations _documentOperations;

        public Template.Default MasterPage;

        int _searcherTotalItemsCount;

        public virtual event EventHandler<FormEventArgs<List<int>>> OnOkButtonClick;
        public virtual event EventHandler<FormEventArgs<int>> OnListItemSelected;

        #endregion

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return PopupControls_SearchEntity_Container.Style["Width"]; }
            set { PopupControls_SearchEntity_Container.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return PopupControls_SearchEntity_Container.Style["Height"]; }
            set { PopupControls_SearchEntity_Container.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style["padding"]; }
            set { modalPopupContainerBody.Style["padding"] = value; }
        }

        public bool IsVisible
        {
            get { return PopupControls_SearchEntity_Container.Attributes.CssStyle.Value.Contains("inline"); }
        }

        public List<int> SelectedItems
        {
            get { return ViewState["SelectedItems"] != null ? (List<int>)ViewState["SelectedItems"] : new List<int>(); }
            set { ViewState["SelectedItems"] = value; }
        }

        private SelectMode SelectMode
        {
            get { return ViewState["SelectMode"] != null ? (SelectMode)ViewState["SelectMode"] : SelectMode.Single; }
            set { ViewState["SelectMode"] = value; }
        }

        private SearchType SearchType
        {
            get { return ViewState["SearchType"] != null ? (SearchType)ViewState["SearchType"] : SearchType.Null; }
            set { ViewState["SearchType"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadFormVariables();
            BindEventHandlers();
        }

        #endregion

        #region Form methods

        #region Initialize

        private void LoadFormVariables()
        {
            PopupControls_SearchEntity_Container.Style["display"] = "none";

            _productOperations = new Product_PKDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _substanceOperations = new Substance_PKDAL();
            _personOperations = new Person_PKDAL();
            _qppvCodeOperations = new Qppv_code_PKDAL();
            _atcOperations = new Atc_PKDAL();
            _administrationRouteOperations = new Adminroute_PKDAL();
            _medicalDeviceOperations = new Medicaldevice_PKDAL();
            _organizationInRoleOperations = new Organization_in_role_DAL();
            _activityOperations = new Activity_PKDAL();
            _projectOperations = new Project_PKDAL();
            _taskOperations = new Task_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();
            _documentOperations = new Document_PKDAL();

            MasterPage = (Template.Default)Page.Master;
        }

        private void BindEventHandlers()
        {
            gvData.RowDataBound += gvData_RowDataBound;
            gvData.RowCreated += gvData_RowCreated;
            gvData.SelectedIndexChanging += gvData_SelectedIndexChanging;
            gvData.Sorting += gvData_Sorting;
            gvPager.PageChanged += gvPager_PageChanged;
        }

        #endregion

        #region Clear

        public void ClearForm(string arg)
        {
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
        }

        #endregion

        #region Bind

        public void BindForm(object args)
        {
            var searchResults = GetSearchResults(null);

            if (_searcherTotalItemsCount < gvPager.RecordsPerPage * gvPager.CurrentPage)
            {
                gvPager.CurrentPage = _searcherTotalItemsCount / gvPager.RecordsPerPage + 1;
                searchResults = GetSearchResults(null);
            }

            if (SearchType != SearchType.PersonEmail)
            {
                var dt = searchResults != null && searchResults.Tables.Count > 0 ? searchResults.Tables[0] : null;
                HandleNonPersonMailSearchType(dt);
            }

            if (SearchType == SearchType.Qppv)
            {
                var dt = searchResults != null && searchResults.Tables.Count > 0 ? searchResults.Tables[0] : null;
                HandleQppvPersonSearchType(dt);
            }

            // TODO: search type local qppv

            gvData.DataSource = searchResults;
            gvData.DataBind();

            gvPager.TotalRecordsCount = _searcherTotalItemsCount;
            gvPager.BindGridViewPager();
            gvPager.Visible = _searcherTotalItemsCount > 0;

            if (SearchType == SearchType.AdministrationRoute || SearchType == SearchType.MedicalDevice ||
                SearchType == SearchType.Manufacturer || SearchType == SearchType.LicenceHolder ||
                SearchType == SearchType.Client || SearchType == SearchType.Document)
            {
                txtDescription.Visible = false;
            }

            if (gvData.HeaderRow != null && gvData.HeaderRow.Cells.Count > 3)
            {
                switch (SearchType)
                {
                    case SearchType.Atc:
                    case SearchType.PersonEmail:
                    case SearchType.Product:
                    case SearchType.PharmaceuticalProduct:
                    case SearchType.Qppv:
                    case SearchType.LocalQPPV:
                    case SearchType.Substance:
                    case SearchType.Client:
                    case SearchType.LicenceHolder:
                    case SearchType.Manufacturer:
                    case SearchType.Activity:
                    case SearchType.Project:
                    case SearchType.Task:
                    case SearchType.Document:
                    case SearchType.AuthorisedProduct:
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        break;
                }
            }
        }

        #endregion

        #endregion

        #region Support methods

        public void ShowModalSearcher(SearchType inSearchType, SelectMode inSelectMode = SelectMode.Single)
        {
            PopupControls_SearchEntity_Container.Style["display"] = "inline";

            SearchType = inSearchType;
            SelectMode = inSelectMode;

            SelectedItems.Clear();

            btnOK.Visible = SelectMode == SelectMode.Multiple;

            ClearForm(null);
            BindForm(null);
        }

        DataSet GetSearchResults(string arg)
        {
            DataSet entities = null;
            var tempCount = 0;

            gvPager.RecordsPerPage = 10;

            var currentPage = gvPager.CurrentPage;
            var recordsPerPage = gvPager.RecordsPerPage;
            var gob = new GEMOrderBy("[" + gvPager.SortOrderBy + "]", gvPager.SortReverseOrder ? GEMOrderByType.DESC : GEMOrderByType.ASC);
            var gobList = new List<GEMOrderBy> { gob };

            var name = txtName.Text;
            var description = txtDescription.Text;
            gvData.AutoGenerateColumns = true;

            switch (SearchType)
            {
                case SearchType.AdministrationRoute:
                    gvData.AutoGenerateColumns = false;
                    entities = _administrationRouteOperations.GetAdminRoutesByCodeSearcher(name, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.Atc:
                    entities = _atcOperations.GetATCSearcher(name, description, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.MedicalDevice:
                    gvData.AutoGenerateColumns = false;
                    entities = _medicalDeviceOperations.GetMedDevicesByCodeSearcher(name, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.Product:
                    entities = _productOperations.ProductsSearcher(name, description, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.PharmaceuticalProduct:
                    entities = _pharmaceuticalProductOperations.GetPPSearcher(name, description, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.Qppv:
                    entities = _qppvCodeOperations.GetQppvByPersonCodeSearcher(name, description, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.LocalQPPV:
                    entities = _qppvCodeOperations.GetLocalQppvByPersonRoleSearcher(name, description, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.PersonEmail:
                    entities = _personOperations.GetPersonsSearcher(name, description, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.Substance:
                    entities = _substanceOperations.GetSubstancesByNameSearcher(name, description, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.Client:
                    entities = _organizationInRoleOperations.GetOrganizationsByRoleSearcher(Constant.OrganizationRoleName.Client, name, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.LicenceHolder:
                    entities = _organizationInRoleOperations.GetOrganizationsByRoleSearcher(Constant.OrganizationRoleName.LicenceHolder, name, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.Manufacturer:
                    entities = _organizationInRoleOperations.GetOrganizationsByRoleSearcher(Constant.OrganizationRoleName.Manufacturer, name, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.Activity:
                    entities = _activityOperations.GetActivitySearcherDataSet(name, description, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.Project:
                    entities = _projectOperations.GetPPSearcher(name, description, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.Task:
                    entities = _taskOperations.GetTaskSearcherDataSet(name, description, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.AuthorisedProduct:
                    entities = _authorisedProductOperations.AProductsSearcher(name, description, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
                case SearchType.Document:
                    entities = _documentOperations.GetDocumentSearcher(name, currentPage, recordsPerPage, gobList, out tempCount);
                    break;
            }

            _searcherTotalItemsCount = tempCount;

            return entities;
        }

        // TODO: handle local qppv person search type if necessary

        private void HandleQppvPersonSearchType(DataTable dt)
        {
            if (dt == null) return;

            var rowsToDelete = new List<DataRow>();
            foreach (DataRow item in dt.Rows)
            {
                var personName = (string)item[2];

                var numberOfPersons = 0;
                foreach (DataRow item2 in dt.Rows)
                {
                    if (personName == (string)item2[2]) numberOfPersons++;
                    if (numberOfPersons >= 2) break;
                }

                // Remove blank QPPV code from view
                if (numberOfPersons >= 2)
                {
                    var row = dt.Rows.Cast<DataRow>().FirstOrDefault(r => (string)r.ItemArray[2] == personName && String.IsNullOrWhiteSpace((string)r.ItemArray[3]));

                    if (row != null && !rowsToDelete.Contains(row)) rowsToDelete.Add(row);
                }
            }

            foreach (var row in rowsToDelete)
            {
                dt.Rows.Remove(row);
            }
        }

        private void HandleNonPersonMailSearchType(DataTable dt)
        {
            if (dt == null) return;
            for (var i = 0; i < dt.Rows.Count; i++)
            {
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j] != null)
                    {
                        dt.Rows[i][j] = StringOperations.TrimAfter(dt.Rows[i][j].ToString(), 60);
                    }
                }
            }
        }

        #endregion

        #region Event handlers

        #region Grid

        public virtual void gvPager_PageChanged(object sender, PageChangedEventArgs e)
        {
            BindForm(null);
        }

        public virtual void gvData_Sorting(object sender, GridViewSortEventArgs e)
        {
            if (e.SortExpression == gvPager.SortOrderBy)
            {
                gvPager.SortReverseOrder = !gvPager.SortReverseOrder;
            }
            else
            {
                gvPager.SortOrderBy = e.SortExpression;
                gvPager.SortReverseOrder = false;
            }

            BindForm(null);
        }

        public void gvData_RowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvData.HeaderRow.Cells.Count > 3)
            {
                switch (SearchType)
                {
                    case SearchType.Atc:
                    case SearchType.Qppv:
                    case SearchType.LocalQPPV:
                    case SearchType.PersonEmail:
                    case SearchType.PharmaceuticalProduct:
                    case SearchType.Product:
                    case SearchType.Substance:
                    case SearchType.Client:
                    case SearchType.LicenceHolder:
                    case SearchType.Manufacturer:
                    case SearchType.Activity:
                    case SearchType.Project:
                    case SearchType.Task:
                    case SearchType.AuthorisedProduct:
                    case SearchType.Document:
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        break;
                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (SelectMode == SelectMode.Multiple)
                {
                    if (e.Row.RowIndex > 0 && SelectedItems.Contains((int)gvData.DataKeys[e.Row.RowIndex].Value))
                    {
                        e.Row.Attributes.Add("style", "background:Yellow");
                    }
                }

                if (gvData.HeaderRow.Cells.Count > 0)
                {
                    if (e.Row.Cells[0].FindControl("lbtEdit") == null)
                    {
                        e.Row.Cells[0].Controls.Clear();

                        var lbtEdit = new LinkButton
                                          {
                                              ID = "lbtEdit",
                                              CommandName = "Select",
                                              Text = "odaberi"
                                          };

                        lbtEdit.Attributes.Add("CssClass", "gvLinkTextBold");
                        lbtEdit.Attributes.Add("runat", "server");

                        e.Row.Cells[0].Controls.Add(lbtEdit);
                    }
                }
            }
            gvData.SelectedIndex = -1;
        }

        public void gvData_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            if (SearchType == SearchType.PersonEmail)
            {
                if (!ValidationHelper.IsValidEmail(gvData.Rows[e.NewSelectedIndex].Cells[4].Text.Trim()))
                {
                    gvData.SelectedIndex = -1;
                    MasterPage.ModalPopup.ShowModalPopup("Error!", "Recipient doesn't have valid email.");
                    return;
                }
            }

            if (SelectMode == SelectMode.Single)
            {
                PopupControls_SearchEntity_Container.Style["display"] = "none";
                PopupControls_SearchEntity_Container.Attributes.Remove("displayed");

                OnListItemSelected(sender, new FormEventArgs<int>((int)gvData.DataKeys[e.NewSelectedIndex].Value));
            }
            else
            {
                if (SelectedItems.Contains((int)gvData.DataKeys[e.NewSelectedIndex].Value))
                {
                    var items = SelectedItems;
                    items.Remove((int)gvData.DataKeys[e.NewSelectedIndex].Value);
                    SelectedItems = items;
                    gvData.Rows[e.NewSelectedIndex].Attributes.Remove("style");
                    if (e.NewSelectedIndex % 2 == 0)
                    {
                        gvData.SelectedRowStyle.CssClass = gvData.RowStyle.CssClass;
                    }
                    else
                    {
                        gvData.SelectedRowStyle.CssClass = gvData.AlternatingRowStyle.CssClass;
                    }
                }
                else
                {
                    var items = SelectedItems;
                    items.Add((int)gvData.DataKeys[e.NewSelectedIndex].Value);
                    SelectedItems = items;
                    gvData.Rows[e.NewSelectedIndex].Attributes.Add("style", "background:Yellow");
                }
                gvData.SelectedIndex = -1;
            }
        }

        public void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                switch (SearchType)
                {
                    case SearchType.Atc:
                        if (e.Row.Cells.Count > 0 && e.Row.Cells[0].HasControls() && e.Row.Cells[4].HasControls() &&
                            e.Row.Cells[0].Controls[0] is LinkButton &&
                            e.Row.Cells[4].Controls[0] is LinkButton)
                        {
                            ((LinkButton)e.Row.Cells[0].Controls[0]).Text = "ATC Code";
                            ((LinkButton)e.Row.Cells[4].Controls[0]).Text = "Name";
                        }
                        break;
                    case SearchType.Project:
                        if (e.Row.Cells.Count > 4 && e.Row.Cells[4].HasControls() &&
                            e.Row.Cells[4].Controls[0] is LinkButton)
                        {
                            (e.Row.Cells[4].Controls[0] as LinkButton).Text = "Internal status";
                        }
                        break;
                }
            }
        }

        #endregion

        protected void btnSearch_OnClick(object sender, EventArgs e)
        {
            BindForm(null);
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            PopupControls_SearchEntity_Container.Style["display"] = "none";
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            PopupControls_SearchEntity_Container.Style["display"] = "none";
        }

        protected void btnOK_OnClick(object sender, EventArgs e)
        {
            PopupControls_SearchEntity_Container.Style["display"] = "none";
            if (OnOkButtonClick != null)
            {
                OnOkButtonClick(sender, new FormEventArgs<List<int>>(SelectedItems));
            }
        }

        public string HandleMissing(object value)
        {
            if (!string.IsNullOrWhiteSpace(Convert.ToString(value)))
            {
                return StringOperations.TrimAfter(value.ToString(), 59);
            }

            return "Missing";
        }

        #endregion
    }
}