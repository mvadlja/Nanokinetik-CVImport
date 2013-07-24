using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Interface;
using AspNetUIFramework;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class DateTimeRangeBox : System.Web.UI.UserControl, ILastChange, IArticle57Relevant
    {
        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivTextBox
        {
            get { return divDateBox; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public System.Web.UI.WebControls.TextBox TxtInputFrom
        {
            get { return txtInputFrom; }
        }

        public System.Web.UI.WebControls.TextBox TxtInputTo
        {
            get { return txtInputTo; }
        }

        public Label LblError
        {
            get { return lblError; }
        }

        public string Label
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public string TextFrom
        {
            get { return txtInputFrom.Text ?? string.Empty; }
            set { txtInputFrom.Text = value; }
        }

        public string TextTo
        {
            get { return txtInputTo.Text ?? string.Empty; }
            set { txtInputTo.Text = value; }
        }

        public int MaxLengthFrom
        {
            get { return txtInputFrom.MaxLength; }
            set { txtInputFrom.MaxLength = value; }
        }

        public int MaxLengthTo
        {
            get { return txtInputTo.MaxLength; }
            set { txtInputTo.MaxLength = value; }
        }

        public bool Required
        {
            get { return spanRequired.Visible; }
            set { spanRequired.Visible = value; }
        }

        public Unit LabelWidth
        {
            get { return lblName.Width; }
            set { lblName.Width = value; }
        }

        public Unit TextWidthFrom
        {
            get { return txtInputFrom.Width; }
            set { txtInputFrom.Width = value; }
        }

        public Unit TextWidthTo
        {
            get { return txtInputTo.Width; }
            set { txtInputTo.Width = value; }
        }

        public string ValidationError
        {
            get { return lblError.Text; }
            set { lblError.Text = value; }
        }

        public string OldValueFrom
        {
            get { return ViewState["OldValueFrom"] != null && ValidationHelper.IsValidDateTime(ViewState["OldValueFrom"].ToString()) ? ViewState["OldValueFrom"].ToString() : string.Empty; }
            set { ViewState["OldValueFrom"] = value; }
        }

        public string OldValueTo
        {
            get { return ViewState["OldValueTo"] != null && ValidationHelper.IsValidDateTime(ViewState["OldValueTo"].ToString()) ? ViewState["OldValueTo"].ToString() : string.Empty; }
            set { ViewState["OldValueTo"] = value; }
        }

        public bool IsModified
        {
            get { return OldValueFrom != txtInputFrom.Text || OldValueTo != txtInputTo.Text; }
        }

        #endregion

        #region Page methods

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OldValueFrom = txtInputFrom.Text;
                OldValueTo = txtInputTo.Text;
            }
        }

        #endregion
    }
}