using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using AspNetUIFramework;

namespace AspNetUI.Support
{
    public partial class GridViewPager : System.Web.UI.UserControl, IGridViewPager
    {
        public GridViewPager() : base() { }

        #region IGridViewPager members

        public event EventHandler<PageChangedEventArgs> PageChanged;

        // Default = 0
        public int CurrentPage
        {
            get { return ViewState["CurrentPage_" + this.ID] == null ? 1 : Convert.ToInt32(ViewState["CurrentPage_" + this.ID]); }
            set { ViewState["CurrentPage_" + this.ID] = value; }
        }

        // Default = 50
        public int RecordsPerPage
        {
            get { return ViewState["RecordsPerPage_" + this.ID] == null ? 50 : Convert.ToInt32(ViewState["RecordsPerPage_" + this.ID]); }
            set { ViewState["RecordsPerPage_" + this.ID] = value; }
        }

        public int TotalRecordsCount
        {
            get { return ViewState["TotalRecordsCount_" + this.ID] == null ? 0 : Convert.ToInt32(ViewState["TotalRecordsCount_" + this.ID]); }
            set { ViewState["TotalRecordsCount_" + this.ID] = value; }
        }

        // Sorting support
        public string SortOrderBy
        {
            get { return ViewState["SortExpression_" + this.ID] == null ? String.Empty : ViewState["SortExpression_" + this.ID].ToString(); }
            set { ViewState["SortExpression_" + this.ID] = value; }
        }

        // Default = false
        public bool SortReverseOrder
        {
            get { return ViewState["SortReverseOrder_" + this.ID] == null ? false : Convert.ToBoolean(ViewState["SortReverseOrder_" + this.ID]); }
            set { ViewState["SortReverseOrder_" + this.ID] = value; }
        }

        // Default = gvData
        public string AssociatedGridViewID
        {
            get { return ViewState["AssociatedGridViewID_" + this.ID] == null ? "gvData" : ViewState["AssociatedGridViewID_" + this.ID].ToString(); }
            set { ViewState["AssociatedGridViewID_" + this.ID] = value; }
        }


        public void BindGridViewPager()
        {
            int pagesCount = TotalRecordsCount / RecordsPerPage;
            if (TotalRecordsCount % RecordsPerPage > 0) pagesCount++;

            txtJumpToPage.Text = CurrentPage.ToString();

            // Binding labels
            lblPageCount.Text = pagesCount.ToString();
            lblPagerStatus.Text = TotalRecordsCount.ToString();

            int i;

            pagesHolder.Text = "";

            //TODO
            for (i = 0; i < pagesCount; i++)
            {

                // uvijek prikazi +-2 od current ako je curent + 2 veæe od 3 onda prikaži i prva 3 ako je  current + 2 manje od ukupni broj - 3 prikaži i zadnja 3

                if (i < 3 || (i > (CurrentPage - 3) && i < (CurrentPage + 1)) || i > pagesCount - 4)
                {
                    if (CurrentPage == (i + 1))
                    {
                        pagesHolder.Controls.Add(new LiteralControl("<a class='current_page'>[" + (i + 1).ToString() + "]</a>"));
                    }
                    else
                    {
                        LinkButton lb = new LinkButton();
                        lb.Click += new EventHandler(ibGoToPageE_Click);
                        lb.CommandArgument = (i + 1).ToString();
                        lb.ID = "lbid" + (i + 1).ToString();
                        lb.Text = (i + 1).ToString();
                        pagesHolder.Controls.Add(lb);
                    }
                }

                if ((i == 3 && CurrentPage > 5) || (i < pagesCount - 3) && i == CurrentPage + 1)
                {
                    pagesHolder.Controls.Add(new LiteralControl("<span class='dots'>...</span>"));
                }

            }

            // Enableing / disableing buttons
            if (CurrentPage <= 1)
            {
                ibPreviousPage.Enabled = false;
                //lblPreviousPage.Enabled = false;
            }
            else
            {
                ibPreviousPage.Enabled = true;
                //lblPreviousPage.Enabled = true;
            }

            if (CurrentPage == pagesCount)
            {
                ibNextPage.Enabled = false;
                //lblNextPage.Enabled = false;
            }
            else
            {
                ibNextPage.Enabled = true;
                //lblNextPage.Enabled = true;
            }
        }

        #endregion

        protected void Page_Init(object sender, EventArgs e)
        {
            this.PageChanged += new EventHandler<PageChangedEventArgs>(OnPageChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.BindGridViewPager();
        }

        protected void txtJumpToPage_TextChanged(object sender, EventArgs e)
        {
            int currentPage = 0;
            int.TryParse(txtJumpToPage.Text, out currentPage);
            int pagesCount = TotalRecordsCount / RecordsPerPage;
            if (TotalRecordsCount % RecordsPerPage > 0) pagesCount++;

            if (currentPage > 0 && currentPage <= pagesCount) CurrentPage = currentPage;

            if (PageChanged != null) PageChanged(this, new PageChangedEventArgs(CurrentPage));
        }

        protected void ibGoToPage_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton ib = (ImageButton)sender;

            switch (ib.ID)
            {
                case "ibFirstPage":
                    CurrentPage = 1;
                    break;
                case "ibPreviousPage":
                    CurrentPage--;
                    break;
                case "ibNextPage":
                    CurrentPage++;
                    break;
                case "ibLastPage":
                    int pagesCount = TotalRecordsCount / RecordsPerPage;
                    if (TotalRecordsCount % RecordsPerPage > 0) pagesCount++;
                    CurrentPage = pagesCount;
                    break;
            }

            if (PageChanged != null) PageChanged(this, new PageChangedEventArgs(CurrentPage));
        }

        protected void ibGoToPage_Click(object sender, EventArgs e)
        {
            LinkButton ib = (LinkButton)sender;

            switch (ib.CommandArgument)
            {
                case "ibFirstPage":
                    CurrentPage = 1;
                    break;
                case "ibPreviousPage":
                    CurrentPage--;
                    break;
                case "ibNextPage":
                    CurrentPage++;
                    break;
                case "ibLastPage":
                    int pagesCount = TotalRecordsCount / RecordsPerPage;
                    if (TotalRecordsCount % RecordsPerPage > 0) pagesCount++;
                    CurrentPage = pagesCount;
                    break;
            }

            if (PageChanged != null) PageChanged(this, new PageChangedEventArgs(CurrentPage));
        }


        protected void ibGoToPageE_Click(object sender, EventArgs e)
        {
            LinkButton lb = (LinkButton)sender;

            CurrentPage = Int32.Parse(lb.CommandArgument);

            if (PageChanged != null) PageChanged(this, new PageChangedEventArgs(CurrentPage));
        }


        protected void OnPageChanged(object sender, PageChangedEventArgs e)
        {
            this.BindGridViewPager();
        }
    }
}