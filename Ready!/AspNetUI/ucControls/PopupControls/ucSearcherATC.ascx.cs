using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using AspNetUIFramework;
using GEM2Common;
using Ready.Model;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucSearcherATC : ListForm
    {
        IAtc_PKOperations _atcOperations;

        int _searcherTotalItemsCount = 0;

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
                case "ATC":
                    pnlATC.Visible = true;
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

            _atcOperations = new Atc_PKDAL();

            base.OnInit(e);
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
                    case "AuthProd":
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        break;
                    case "ATC":
                        e.Row.Cells[1].Visible = false;
                        e.Row.Cells[2].Visible = false;
                        e.Row.Cells[3].Visible = false;
                        e.Row.Cells[4].Visible = true;
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
            gvData.SelectedIndex = -1;
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
            if (SearchType == "ATC")
            {
                gvData.Columns[0].HeaderText = "ATC Code";
            }

            //Bind grid
            DataSet entities = GetSearchResults("");
            gvPager.TotalRecordsCount = _searcherTotalItemsCount;
            gvData.DataSource = entities;
            base.BindForm(id, arg);


            if (gvData.HeaderRow != null && gvData.HeaderRow.Cells.Count > 2)
            {
                switch (SearchType)
                {
                    case "Products":
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        break;
                    case "AuthProd":
                        gvData.HeaderRow.Cells[1].Visible = false;
                        gvData.HeaderRow.Cells[2].Visible = false;
                        gvData.HeaderRow.Cells[3].Visible = false;
                        break;
                    case "ATC":
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
            if (SelectMode == SelectMode.Single)
            {
                PopupControls_SearchEntity_Container.Style["display"] = "none";
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
                GEMOrderBy gob = new GEMOrderBy(gvPager.SortOrderBy, gvPager.SortReverseOrder == true ? GEMOrderByType.DESC : GEMOrderByType.ASC);
                List<GEMOrderBy> gobList = new List<GEMOrderBy>() { gob };

                string name;
                string code;

                switch (SearchType)
                {
                    

                    case "ATC":
                        code = txtATC.Text;
                        name = txtATCName.Text;
                        gvData.AutoGenerateColumns = true;
                        entities = _atcOperations.GetATCSearcher(code, name, currentPage, recordsPerPage, gobList, out tempCount);
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

        public override ListPermissionType CheckAccess()
        {
            return ListPermissionType.READ_WRITE;
        }

        #endregion
    }
}