using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;

namespace AspNetUI.Support
{
    public partial class InboxGridViewPager : UserControl
    {
        public event EventHandler<PageChangedEventArgs> PageChanged;

        public string Width
        {
            get { return MainTable.Width; }
            set { MainTable.Width = value; }
        }

        // Default = 0
        public int CurrentPage
        {
            get { return ViewState["CurrentPage_" + this.ID] == null ? 1 : Convert.ToInt32(ViewState["CurrentPage_" + this.ID]); }
            set { ViewState["CurrentPage_" + this.ID] = value; }
        }

        // Default = 20
        public int RecordsPerPage
        {
            get { return ViewState["RecordsPerPage_" + this.ID] == null ? 20 : Convert.ToInt32(ViewState["RecordsPerPage_" + this.ID]); }
            set { ViewState["RecordsPerPage_" + this.ID] = value; }
        }

        public int CurrentRecordsCount
        {
            get { return ViewState["CurrentRecordsCount_" + this.ID] == null ? 0 : Convert.ToInt32(ViewState["CurrentRecordsCount_" + this.ID]); }
            set { ViewState["CurrentRecordsCount_" + this.ID] = value; }
        }

        public Dictionary<int, int> CurrentPageAndIndex
        {
            get { return ViewState["CurrentPageAndIndex_" + this.ID] == null ? new Dictionary<int, int>() : (Dictionary<int, int>)ViewState["CurrentPageAndIndex_" + this.ID]; }
            set { ViewState["CurrentPageAndIndex_" + this.ID] = value; }
        }

        // Default = gvData
        public string AssociatedGridViewID
        {
            get { return ViewState["AssociatedGridViewID_" + this.ID] == null ? "gvData" : ViewState["AssociatedGridViewID_" + this.ID].ToString(); }
            set { ViewState["AssociatedGridViewID_" + this.ID] = value; }
        }

        public void BindGridViewPager()
        {
            txtCurrentPage.Text = CurrentPage.ToString();
            
            // Enableing / disableing buttons
            if (CurrentPage <= 1)
            {
                ibFirstPage.Enabled = false;
                ibPreviousPage.Enabled = false;
            }
            else
            {
                ibFirstPage.Enabled = true;
                ibPreviousPage.Enabled = true;
            }

            ibNextPage.Enabled = CurrentRecordsCount >= RecordsPerPage;
        }
        
        protected void Page_Init(object sender, EventArgs e)
        {
            this.PageChanged += new EventHandler<PageChangedEventArgs>(OnPageChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.BindGridViewPager();
            }
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
            }

            if (PageChanged != null) PageChanged(this, new PageChangedEventArgs(CurrentPage));
        }

        protected void OnPageChanged(object sender, PageChangedEventArgs e)
        {
            this.BindGridViewPager();
        }
    }
}