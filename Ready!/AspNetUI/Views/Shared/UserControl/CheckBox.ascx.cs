using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Interface;
using AspNetUIFramework;

namespace  AspNetUI.Views.Shared.UserControl
{
    public partial class CheckBox : System.Web.UI.UserControl, ILastChange, IArticle57Relevant
    {
        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivCheckBox
        {
            get { return divCheckBox; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public System.Web.UI.WebControls.CheckBox CbInput
        {
            get { return cbInput; }
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

        public bool AutoPostback
        {
            get { return cbInput.AutoPostBack; }
            set { cbInput.AutoPostBack = value; }
        }

        public bool Checked 
        {
            get { return cbInput.Checked; }
            set { cbInput.Checked = value; }
        }

        public string ValidationError
        {
            get { return lblError.Text; }
            set { lblError.Text = value; }
        }

        public bool OldValue
        {
            get { return ViewState["OldValue"] != null && ValidationHelper.IsValidBool(ViewState["OldValue"].ToString()) ? (bool)ViewState["OldValue"] : false; }
            set { ViewState["OldValue"] = value; }
        }

        public bool IsModified
        {
            get { return OldValue != cbInput.Checked; }
        }

        #endregion

        #region Page methods

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OldValue = cbInput.Checked;
            }
        }

        #endregion
    }
}