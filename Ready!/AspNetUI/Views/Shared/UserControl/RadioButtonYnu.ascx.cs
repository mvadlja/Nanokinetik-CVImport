using System;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Interface;
using AspNetUIFramework;

namespace  AspNetUI.Views.Shared.UserControl
{
    public partial class RadioButtonYnu : System.Web.UI.UserControl, ILastChange, IArticle57Relevant, IXevprmValidationError
    {
        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivRadioButtonYnu
        {
            get { return divRadioButtonYnu; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public RadioButtonList RbYnu
        {
            get { return rbYnu; }
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
            get { return rbYnu.Enabled; }
            set { rbYnu.Enabled = value; }
        }

        public Unit LabelWidth
        {
            get { return lblName.Width; }
            set { lblName.Width = value; }
        }

        public ListItem SelectedItem
        {
            get { return rbYnu.SelectedItem; }
        }

        public bool? SelectedValue
        {
            get
            {
                bool? selectedValue = null; 
                switch(rbYnu.SelectedValue)
                {
                    case "NULL":
                        selectedValue = null;
                        break;
                    case "Yes":
                        selectedValue = true;
                        break;
                    case "No":
                        selectedValue = false;
                        break;
                }

                return selectedValue;
            }
            set
            {
                switch (value)
                {
                    case null:
                        rbYnu.SelectedValue = "NULL";
                        break;
                    case true:
                        rbYnu.SelectedValue = "Yes";
                        break;
                    case false:
                        rbYnu.SelectedValue = "No";
                        break;
                }
            }
        }

        public string SelectedText
        {
            get
            {
                if (rbYnu.SelectedItem == null) return null;
                return rbYnu.SelectedItem.Text;
            }
            set
            {
                var firstMatch = rbYnu.Items.FindByText(value);

                if (firstMatch != null)
                {
                    firstMatch.Selected = true;
                }
            }
        }

        public bool AutoPostback
        {
            get { return rbYnu.AutoPostBack; }
            set { rbYnu.AutoPostBack = value; }
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SelectedValue = null;
        }

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