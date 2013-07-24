using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using AspNetUIFramework;
using GEM2Common;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public enum SelectMode { Single, Multiple }

    public partial class ucSearcher : ListForm
    {
        IProduct_PKOperations _productOperations;
        IOrganization_in_role_Operations _organizationInRoleOperations;
        IMaster_file_location_PKOperations _mflOperations;
        IPerson_in_role_PKOperations _personMNOperations;
        IAdminroute_PKOperations _adminRouteOperations;
        IMedicaldevice_PKOperations _medDeviceOperations;
        IOrg_in_type_for_partnerOperations _orgInTypeForPartnerOperations;
        IProduct_pi_mn_PKOperations _productIndicationMNOperations;
        IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        IAuthorisedProductOperations _authProductOperations;
        IAtc_PKOperations _atcOperations;
        ISubstance_PKOperations _substanceOperations;
        IProject_PKOperations _projectOperations;
        IActivity_PKOperations _activityOperations;
        ITask_PKOperations _taskOperations;
        IActiveingredient_PKOperations _activeIngrOperations;
        IPerson_PKOperations _person_PKOperations;
        IQppv_code_PKOperations _qppv_codePKOperations;
        int _searcherTotalItemsCount = 0;

        private static bool correctlySorted = false;

        public virtual event EventHandler<FormListEventArgs> OnOkButtonClick;

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
            get { 
                if(PopupControls_SearchEntity_Container.Attributes.CssStyle.Value.Contains("inline") )
                    return true;
                else
                    return false;
            }
        }

        public List<int> SelectedItems
        {
            get { return ViewState["SelectedItems"] != null ? (List<int>)ViewState["SelectedItems"] : new List<int>(); }
            set { ViewState["SelectedItems"] = value; }
        }

        private string SearchType
        {
            get { return ViewState["SearchType"] != null ? (string)ViewState["SearchType"] : String.Empty; }
            set { ViewState["SearchType"] = value; }
        }

        private SelectMode SelectMode
        {
            get { return ViewState["SelectMode"] != null ? (SelectMode)ViewState["SelectMode"] : SelectMode.Single; }
            set { ViewState["SelectMode"] = value; }
        }
        

        #endregion

        #region Operations

        public void ShowModalSearcher(string searchType, SelectMode inSelectMode = SelectMode.Single)
        {
            PopupControls_SearchEntity_Container.Style["display"] = "inline";

            SearchType = searchType;
            SelectMode = inSelectMode;

            SelectedItems.Clear();

            if (SelectMode == SelectMode.Multiple)
            {
                btnOK.Visible = true;
            }
            else
            {
                btnOK.Visible = false;
            }

            switch (SearchType)
            {
                case "Products":
                    pnlProductsSearch.Visible = true;
                    break;
                case "AuthProd":
                    pnlProductsSearch.Visible = true;
                    break;
                case "MAH":
                    pnlMAHSearch.Visible = true;
                    break;
                case "SubName":
                    pnlSubName.Visible = true;
                    break;
                case "AdminRoute":
                    pnlAdminRoute.Visible = true;
                    break;
                case "MedDevice":
                    pnlMedDevice.Visible = true;
                    break;
                case "PPIAttachment":
                    pnlPPIAttachmentSearch.Visible = true;
                    break;
                case "Distributor":
                    pnlDistributorSearch.Visible = true;
                    break;
                case "MasterFile":
                    pnlMasterFileSearch.Visible = true;
                    break;
                case "QPPV":
                    pnlQPPVSearch.Visible = true;
                    break;
                case "Manufacturer":
                    pnlManufacturer.Visible = true;
                    break;
                //case "ATC":
                //    pnlATC.Visible = true;
                //    break;
                case "Client":
                    pnlClient.Visible = true;
                    break;
                case "Indication":
                    pnlIndication.Visible = true;
                    break;
                case "PharmaProducts":
                    pnlPharmaProduct.Visible = true;
                    break;
                case "Project":
                    pnlProjectSearch.Visible = true;
                    break;
                case "Applicant":
                    pnlApplicant.Visible = true;
                    break;
                case "Activities":
                    pnlActivitiesSearch.Visible = true;
                    break;
                case "Tasks":
                    pnlTasksSearch.Visible = true;
                    break;
                case "ActIng":
                    pnlActIngSearch.Visible = true;
                    break;
                case "PersonEmail":
                    pnlPersonSearch.Visible = true;
                    break;

            }
            Search("");
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_SearchEntity_Container.Style["display"] = "none";

            gvData.RowDataBound += new GridViewRowEventHandler(gvData_RowDataBound);
            //gvData.RowCreated += new GridViewRowEventHandler(gvData_RowCreated);

            _productOperations = new Product_PKDAL();
            _organizationInRoleOperations = new Organization_in_role_DAL();
            _mflOperations = new Master_file_location_PKDAL();
            _personMNOperations = new Person_in_role_PKDAL();
            _adminRouteOperations = new Adminroute_PKDAL();
            _medDeviceOperations = new Medicaldevice_PKDAL();
            _orgInTypeForPartnerOperations = new Org_in_type_for_partner_DAL();
            _productIndicationMNOperations = new Product_pi_mn_PKDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _atcOperations = new Atc_PKDAL();
            _substanceOperations = new Substance_PKDAL();
            _authProductOperations = new AuthorisedProductDAL();
            _projectOperations = new Project_PKDAL();
            _activityOperations = new Activity_PKDAL();
            _taskOperations = new Task_PKDAL();
            _activeIngrOperations = new Activeingredient_PKDAL();
            _person_PKOperations = new Person_PKDAL();
            _qppv_codePKOperations = new Qppv_code_PKDAL();

            base.OnInit(e);

            if (!IsPostBack)
            {
                correctlySorted = false;
            }

        }

        public override void gvData_RowCreated(Object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && gvData.HeaderRow.Cells.Count > 1)
            {
                switch (SearchType)
                {
                    case "Products":
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        break;
                    case "PharmaProducts":
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        break;
                    case "AuthProd":
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        break;
                    //case "ATC":
                    //    e.Row.Cells[1].Visible = false;
                    //    e.Row.Cells[2].Visible = false;
                    //    e.Row.Cells[3].Visible = false;
                    //    e.Row.Cells[4].Visible = true;
                    //    break;
                    case "SubName":
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        break;
                    case "Activities":
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        break;
                    case "Project":
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        break;
                    case "Tasks":
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        break;
                    case "PersonEmail":
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        break;
                    case "QPPV":
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

                        LinkButton lbtEdit = new LinkButton();

                        lbtEdit.ID = "lbtEdit";

                        lbtEdit.Attributes.Add("CssClass", "gvLinkTextBold");
                        lbtEdit.CommandName = "Select";

                        lbtEdit.Attributes.Add("runat", "server");

                        lbtEdit.Text = "odaberi";

                        e.Row.Cells[0].Controls.Add(lbtEdit);
                    }
                }
            }
            base.gvData_RowCreated(sender, e);
            gvData.SelectedIndex = -1;
        }

        public override void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (SearchType == "Project")
                {
                    if (e.Row.Cells.Count > 4 && e.Row.Cells[4].Controls.Count > 0 && e.Row.Cells[4].Controls[0] is LinkButton)
                    {
                        (e.Row.Cells[4].Controls[0] as LinkButton).Text = "Internal status";
                    }
                }
            }

            base.gvData_RowDataBound(sender, e);
        }

        public override void ClearForm(string arg)
        {
            //txtID.Text = String.Empty;
            //txtName.Text = String.Empty;
        }

        public override void FillDataDefinitions(string arg)
        {

        }

        public override bool ValidateForm(string arg)
        {
            return true;
        }

        public override void BindForm(object id, string arg)
        {

            //Bind grid
            DataSet entities = GetSearchResults("");

            if (SearchType != "PersonEmail")
            {
                DataTable dt = entities != null && entities.Tables.Count > 0 ? entities.Tables[0] : null;
                if (dt == null) return;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (dt.Rows[i][j] != null)
                        {
                            dt.Rows[i][j] = StringOperations.TrimAfter(dt.Rows[i][j].ToString(), 60);
                        }
                    }
                }
            }

            if (SearchType == "QPPV")
            {
                DataTable dt = entities != null && entities.Tables.Count > 0 ? entities.Tables[0] : null;
                if (dt == null) return;

                List<DataRow> rowsToDelete = new List<DataRow>();
                foreach (DataRow item in dt.Rows) 
                {
                    string personName = (string)item[2];

                    int numberOfPersons = 0;
                    foreach (DataRow item2 in dt.Rows) 
                    {
                        if (personName == (string)item2[2]) numberOfPersons++;
                        if (numberOfPersons >= 2) break;
                    }
                    
                    // Remove blank QPPV code from view
                    if (numberOfPersons >= 2) 
                    {
                        var row = dt.Rows.Cast<DataRow>().Where(r => (string)r.ItemArray[2] == personName && String.IsNullOrWhiteSpace((string)r.ItemArray[3])).FirstOrDefault();

                        if (row != null && !rowsToDelete.Contains(row)) rowsToDelete.Add(row);
                    }
                }

                foreach (DataRow row in rowsToDelete)
                {
                    dt.Rows.Remove(row);
                }
            }

            gvPager.TotalRecordsCount = _searcherTotalItemsCount;
            gvData.DataSource = entities;
            base.BindForm(id, arg);

            //if (SearchType == "ATC" && !correctlySorted)
            //{
            //    correctlySorted = true;
            //    gvData.Sort("ATCcode", SortDirection.Ascending);
            //}


            if (gvData.HeaderRow != null && gvData.HeaderRow.Cells.Count > 2)
            {
                switch (SearchType)
                {
                    case "Products":
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        //gvData.HeaderRow.Cells[4].Text = "Internal status";
                        break;
                    case "PharmaProducts":
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        break;
                    case "AuthProd":
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        break;
                    //case "ATC":
                    //    gvData.HeaderRow.Cells[1].Visible = false;
                    //    gvData.HeaderRow.Cells[2].Visible = false;
                    //    gvData.HeaderRow.Cells[3].Visible = false;
                    //    gvData.HeaderRow.Cells[4].Visible = true;
                    //    break;
                    case "SubName":
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        break;
                    case "Activities":
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        break;
                    case "Project":
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        break;
                    case "Tasks":
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        break;
                    case "PersonEmail":
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        break;
                    case "QPPV":
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        break;
                }
            }
        }

        public override void gvData_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            if (SearchType == "PersonEmail")
            {
                if (!ValidationHelper.IsValidEmail(gvData.Rows[e.NewSelectedIndex].Cells[4].Text.Trim()))
                {
                    gvData.SelectedIndex = -1;
                    FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", "Recipient doesn't have valid email.");
                    return;
                }
            }

            if (SelectMode == SelectMode.Single)
            {
                PopupControls_SearchEntity_Container.Style["display"] = "none";
                PopupControls_SearchEntity_Container.Attributes.Remove("displayed");
                base.gvData_SelectedIndexChanging(sender, e);
            }
            else
            {
                if (SelectedItems.Contains((int)gvData.DataKeys[e.NewSelectedIndex].Value))
                {
                    List<int> items = SelectedItems;
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
                    List<int> items = SelectedItems;
                    items.Add((int)gvData.DataKeys[e.NewSelectedIndex].Value);
                    SelectedItems = items;
                    gvData.Rows[e.NewSelectedIndex].Attributes.Add("style", "background:Yellow");
                }
                gvData.SelectedIndex = -1;
            }

        }


        #endregion

        #region Searcher methods

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Search("");
        }

        

        protected void btnClear_Click(object sender, EventArgs e)
        {
            ClearForm("");
        }

        #endregion

        #region Searcher extra stuff

        DataSet GetSearchResults(string arg)
        {
            DataSet entities = null;
            int tempCount = 0;

            if (ValidateForm(""))
            {
                gvPager.RecordsPerPage = 10;

                int currentPage = gvPager.CurrentPage;
                int recordsPerPage = gvPager.RecordsPerPage;
                GEMOrderBy gob = new GEMOrderBy("["+gvPager.SortOrderBy+"]", gvPager.SortReverseOrder == true ? GEMOrderByType.DESC : GEMOrderByType.ASC);
                List<GEMOrderBy> gobList = new List<GEMOrderBy>() { gob };

                string name;
                string description;
                string countries;
                string concise;
                string evcode;
                string applicant;
                string internalStatus;
                string activity;

                switch (SearchType)
                {
                    case "Products":
                        name = txtName.Text;
                        //description = txtDescription.Text;
                        countries = txtCountries.Text;
                        gvData.AutoGenerateColumns = true;
                        entities = _productOperations.ProductsSearcher(name, countries, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "AuthProd":
                        name = txtName.Text;
                        //description = txtDescription.Text;
                        countries = txtCountries.Text;
                        gvData.AutoGenerateColumns = true;
                        entities = _authProductOperations.AProductsSearcher(name, countries, currentPage, recordsPerPage, gobList, out tempCount);
                        break;
                    case "MAH":
                        name = mahTxtName.Text;
                        gvData.AutoGenerateColumns = false;
                        entities = _organizationInRoleOperations.GetOrganizationsByRoleSearcher("Licence holder", name, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "SubName":
                        name = txtSubName.Text;
                        evcode = txtSubEVCODE.Text;
                        gvData.AutoGenerateColumns = true;
                        entities = _substanceOperations.GetSubstancesByNameSearcher(name, evcode, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "AdminRoute":
                        name = txtAdminRouteCode.Text;
                        gvData.AutoGenerateColumns = false;
                        entities = _adminRouteOperations.GetAdminRoutesByCodeSearcher(name, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "MedDevice":
                        name = txtMedDeviceCode.Text;
                        gvData.AutoGenerateColumns = false;
                        entities = _medDeviceOperations.GetMedDevicesByCodeSearcher(name, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "PPIAttachment":
                        name = ppiTxtName.Text;
                        description = ppiTxtDescription.Text;
                        break;

                    case "Distributor":
                        name = distributorTxtName.Text;
                        description = distributorTxtDescription.Text;
                        break;

                    case "MasterFile":
                        name = masterFileTxtName.Text;
                        gvData.AutoGenerateColumns = false;
                        entities = _organizationInRoleOperations.GetOrganizationsByRoleSearcher("Master File Location", name, currentPage, recordsPerPage, gobList, out tempCount);
                       
                        //entities = _mflOperations.MFLSearcher(name, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "QPPV":
                        name = qppvTxtName.Text;
                        String qppvCode = qppvTxtCode.Text;
                        gvData.AutoGenerateColumns = true;
                        entities = _qppv_codePKOperations.GetQppvByPersonCodeSearcher(name, qppvCode, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "Manufacturer":
                        name = txtManufacturer.Text;
                        entities = _organizationInRoleOperations.GetOrganizationsByRoleSearcher("Manufacturer", name, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "Applicant":
                        name = txtApplicant.Text;
                        entities = _organizationInRoleOperations.GetOrganizationsByRoleSearcher("Applicant", name, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    //case "ATC":
                    //    code = txtATC.Text;
                    //    name = txtATCName.Text;
                    //    gvData.AutoGenerateColumns = true;
                    //    entities = _atcOperations.GetATCSearcher(code, name, currentPage, recordsPerPage, gobList, out tempCount);
                    //    break;

                    case "Client":
                        name = txtClient.Text;
                        //entities = _orgInTypeForPartnerOperations.GetOrganizationsByPartnerRoleSearcher("Client", name, currentPage, recordsPerPage, gobList, out tempCount);
                        entities = _organizationInRoleOperations.GetOrganizationsByRoleSearcher("Client", name, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "Indication":
                        name = txtIndication.Text;
                        entities = _productIndicationMNOperations.GetPISearcher(name, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "PharmaProducts":
                        name = txtPharmaProduct.Text;
                        concise = txtConcise.Text;
                        gvData.AutoGenerateColumns = true;
                        entities = _pharmaceuticalProductOperations.GetPPSearcher(name, concise, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "Project":
                        name = txtProjectName.Text;
                        internalStatus = txtProjectInernalStatus.Text;
                        gvData.AutoGenerateColumns = true;
                        entities = _projectOperations.GetPPSearcher(name, internalStatus, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "Activities":
                        name = txtActivityName.Text;
                        applicant = txtActivityApplicant.Text;
                        gvData.AutoGenerateColumns = true;
                        entities = _activityOperations.GetActivitySearcherDataSet(name, applicant, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "Tasks":
                        name = txtTaskName.Text;
                        activity = txtTaskActivity.Text;
                        gvData.AutoGenerateColumns = true;
                        entities = _taskOperations.GetTaskSearcherDataSet(name, activity, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                    case "ActIng":
                         name = txtActiveIngrName.Text;
                        entities = _activeIngrOperations.GetPPSearcher(name, currentPage, recordsPerPage, gobList, out tempCount);
                        break;
                    case "PersonEmail":
                        gvData.AutoGenerateColumns = true;
                        entities = _person_PKOperations.GetPersonsSearcher(txtPersonName.Text, txtEmail.Text, currentPage, recordsPerPage, gobList, out tempCount);
                        break;

                }


                _searcherTotalItemsCount = tempCount;
            }

            return entities;
        }

        #endregion

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PopupControls_SearchEntity_Container.Style["display"] = "none";
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            PopupControls_SearchEntity_Container.Style["display"] = "none";
            if (OnOkButtonClick != null)
            {
                OnOkButtonClick(sender, new FormListEventArgs(SelectedItems));
            }
        }

        #region Security

        public override ListPermissionType CheckAccess() {
            return ListPermissionType.READ_WRITE;
        }

        #endregion

    }
}