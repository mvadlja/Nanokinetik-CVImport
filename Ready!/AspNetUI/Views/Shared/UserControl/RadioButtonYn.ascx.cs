using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Interface;
using AspNetUIFramework;

namespace  AspNetUI.Views.Shared.UserControl
{
    public partial class RadioButtonYn : System.Web.UI.UserControl, ILastChange, IArticle57Relevant, IXevprmValidationError
    {        
        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivRadioButtonYn
        {
            get { return divRadioButtonYn; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public RadioButtonList RbYn
        {
            get { return rbYn; }
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

        public bool Enabled
        {
            get { return rbYn.Enabled; }
            set { rbYn.Enabled = value; }
        }

        public Unit LabelWidth
        {
            get { return lblName.Width; }
            set { lblName.Width = value; }
        }

        public ListItem SelectedItem
        {
            get { return rbYn.SelectedItem; }
        }

        public bool? SelectedValue
        {
            get
            {
                switch (rbYn.SelectedValue)
                {
                    case "Yes":
                        return true;
                    case "No":
                        return false;
                }
                return null;
            }
            set
            {
                switch (value)
                {
                    case true:
                        rbYn.SelectedValue = "Yes";
                        break;
                    case false:
                        rbYn.SelectedValue = "No";
                        break;
                }
            }
        }

        public bool AutoPostback
        {
            get { return rbYn.AutoPostBack; }
            set { rbYn.AutoPostBack = value; }
        }

        public string ValidationError
        {
            get { return lblError.Text; }
            set { lblError.Text = value; }
        }

        public bool? OldValue
        {
            get { return ViewState["OldValue"] != null && ValidationHelper.IsValidBool(ViewState["OldValue"].ToString()) ? (bool?)ViewState["OldValue"] : null; }
            set { ViewState["OldValue"] = value; }
        }

        public bool IsModified
        {
            get { return OldValue != SelectedValue; }
        }

        #endregion

        #region Page methods

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OldValue = SelectedValue;
            }
        }

        #endregion
    }
}