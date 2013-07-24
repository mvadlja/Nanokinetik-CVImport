using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using AspNetUIFramework;
using GEM2Common;
using Ready.Model;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucSearcherSUBST : ListForm
    {
        ISubstance_PKOperations _substanceOperations;

        int _searcherTotalItemsCount = 0;

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

        private string SearchType
        {
            get { return ViewState["SearchType"] != null ? (string)ViewState["SearchType"] : String.Empty; }
            set { ViewState["SearchType"] = value; }
        }


        #endregion

        #region Operations

        public void ShowModalSearcher(string searchType)
        {
            PopupControls_SearchEntity_Container.Style["display"] = "inline";
            SearchType = searchType;

            switch (SearchType)
            {
                case "SubName":
                    pnlSubName.Visible = true;
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

            _substanceOperations = new Substance_PKDAL();

            base.OnInit(e);
        }

        protected void gvData_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
           
        }

        public override void gvData_RowCreated(Object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow && gvData.HeaderRow.Cells.Count > 1)
            {
                switch (SearchType)
                {
                    case "SubName":
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[4].Visible = true;
                        break;

                }
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {

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

                        //break;
                    }
                    else
                    {

                    }

                }

                if (e.Row.RowType == DataControlRowType.Header && gvData.HeaderRow.Cells.Count > 1)
                {

                    gvData.HeaderRow.Cells[1].Visible = false;

                }
                else if (e.Row.RowType == DataControlRowType.DataRow && gvData.HeaderRow.Cells.Count > 1)
                {
                    //gvData.HeaderRow.Cells[1].Visible = false;
                    e.Row.Cells[1].Visible = false;
                }

            }



            base.gvData_RowCreated(sender, e);

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
            gvPager.TotalRecordsCount = _searcherTotalItemsCount;
            gvData.DataSource = entities;
            base.BindForm(id, arg);


            if (gvData.HeaderRow != null && gvData.HeaderRow.Cells.Count > 2)
            {
                switch (SearchType)
                {
                    case "SubName":
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        gvData.HeaderRow.Cells[4].Visible = true;
                        break;
                }
            }
        }

        public override void gvData_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            PopupControls_SearchEntity_Container.Style["display"] = "none";

            base.gvData_SelectedIndexChanging(sender, e);
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
                GEMOrderBy gob = new GEMOrderBy(gvPager.SortOrderBy, gvPager.SortReverseOrder == true ? GEMOrderByType.DESC : GEMOrderByType.ASC);
                List<GEMOrderBy> gobList = new List<GEMOrderBy>() { gob };

                string name;
                string evcode;

                switch (SearchType)
                {
                    case "SubName":
                        name = txtSubName.Text;
                        evcode = txtSubEVCODE.Text;
                        gvData.AutoGenerateColumns = true;
                        entities = _substanceOperations.GetSubstancesByNameSearcher(name, evcode, currentPage, recordsPerPage, gobList, out tempCount);
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

        #region Security

        public override ListPermissionType CheckAccess()
        {
            return ListPermissionType.READ_WRITE;
        }

        #endregion
    }
}