using System;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using System.Linq;

namespace AspNetUI.Views.Shared.Template
{
    public abstract class ListPage : DefaultPage
    {
        #region Properties

        public ListType ListType;
        public Panel PnlGrid;
        public Panel PnlSearch;

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        #endregion

        #region Page override methods

        /// <summary>
        /// Securifies page specifics
        /// </summary>
        /// <returns>True, if page is secured, i.e. page security is resolved (enabled/disabled actions). False, if page security is yet to be determined.</returns>
        public override bool SecurityPageSpecific()
        {
            base.SecurityPageSpecific();

            if (!SecurityHelper.IsPermitted(Permission.QuickLinkCreate))
            {
                Panel pnlSearchButtons = null;
                if (PnlSearch.Controls.Count > 1 && PnlSearch.Controls[1].Controls.Count > 0)
                {
                    pnlSearchButtons = PnlSearch.Controls[1].Controls[0].Controls.OfType<Panel>().FirstOrDefault(pnl => pnl.ID == "pnlSearchButtons");
                }

                if (pnlSearchButtons != null)
                {
                    StyleHelper.DisableLinkButtonsWithCssClass(pnlSearchButtons, "CreateQuickLink");
                }
            }

            if (!SecurityHelper.IsPermitted(Permission.QuickLinkDelete))
            {
                Panel pnlSearchButtons = null;
                if (PnlSearch.Controls.Count > 1 && PnlSearch.Controls[1].Controls.Count > 0)
                {
                    pnlSearchButtons = PnlSearch.Controls[1].Controls[0].Controls.OfType<Panel>().FirstOrDefault(pnl => pnl.ID == "pnlSearchButtons");
                }

                if (pnlSearchButtons != null)
                {
                    StyleHelper.DisableLinkButtonsWithCssClass(pnlSearchButtons, "DeleteQuickLink");
                }
            }

            return false;
        }

        public override void LoadFormVariables()
        {
            PageType = PageType.List;

            base.LoadFormVariables();
        }

        #endregion

        #region Page virtual methods

        public virtual void LoadActionQuery()
        {
            switch (Request.QueryString["Action"])
            {
                case "List":
                    ListType = ListType.List;
                    break;
                case "Search":
                    ListType = ListType.Search;
                    break;
                default:
                    ListType = ListType.List;
                    break;
            }
        }

        #endregion

        #region Support methods

        public void AssociatePanels(Panel pnlSearch, Panel pnlGrid)
        {
            PnlSearch = pnlSearch;
            PnlGrid = pnlGrid;
        }

        public void HideSearch()
        {
            if (PnlGrid != null)
            {
                PnlGrid.RemoveCssClass("display-none");
                PnlGrid.AddCssClass("display-block");
            }

            if (PnlSearch != null)
            {
                PnlSearch.RemoveCssClass("display-block");
                PnlSearch.AddCssClass("display-none");
            }
        }

        public void ShowSearch()
        {
            if (PnlGrid != null)
            {
                PnlGrid.RemoveCssClass("display-block");
                PnlGrid.AddCssClass("display-none");
            }

            if (PnlSearch != null)
            {
                PnlSearch.RemoveCssClass("display-none");
                PnlSearch.AddCssClass("display-block");
            }
        }

        public void ShowAll()
        {
            if (PnlGrid != null)
            {
                PnlGrid.RemoveCssClass("display-none");
                PnlGrid.AddCssClass("display-block");
            }

            if (PnlSearch != null)
            {
                PnlSearch.RemoveCssClass("display-none");
                PnlSearch.AddCssClass("display-block");
            }
        }

        public void HideAll()
        {
            if (PnlGrid != null)
            {
                PnlGrid.RemoveCssClass("display-block");
                PnlGrid.AddCssClass("display-none");
            }

            if (PnlSearch != null)
            {
                PnlSearch.RemoveCssClass("display-block");
                PnlSearch.AddCssClass("display-none");
            }
        }

        public bool? IsPostbackFromGrid()
        {
            if (PnlSearch == null || PnlGrid == null) return null;

            var postbackControl = ResponseHelper.GetPostBackControl(this);

            if (postbackControl != null)
            {
                if (postbackControl == PnlSearch) return false;

                while (postbackControl != null)
                {
                    postbackControl = postbackControl.Parent;

                    if (postbackControl == PnlSearch) return false;
                    if (postbackControl == PnlGrid) return true;
                }
            }

            return null;
        }

        public bool? IsPostbackFromSearch()
        {
            if (PnlSearch == null || PnlGrid == null) return null;

            var postbackControl = ResponseHelper.GetPostBackControl(this);

            if (postbackControl != null)
            {
                if (postbackControl == PnlSearch) return true;

                while (postbackControl != null)
                {
                    postbackControl = postbackControl.Parent;

                    if (postbackControl == PnlSearch) return true;
                    if (postbackControl == PnlGrid) return false;
                }
            }

            return null;
        }

        #endregion
    }
}