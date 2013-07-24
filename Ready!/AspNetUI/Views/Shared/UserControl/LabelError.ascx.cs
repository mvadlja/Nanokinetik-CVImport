using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Interface;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class LabelError : System.Web.UI.UserControl, IXevprmValidationError
    {
        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivLabelError
        {
            get { return divLabelError; }
        }

        public string Text
        {
            get { return lblError.Text ?? string.Empty; }
            set
            {
                lblError.Text = value;
                lblError.Visible = !IsEmpty;
            }
        }

        public Unit TextWidth
        {
            get { return lblError.Width; }
            set { lblError.Width = value; }
        }

        public bool TextBold
        {
            get
            {
                return lblError.Font.Bold;
            }
            set
            {
                lblError.Font.Bold = value;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return string.IsNullOrWhiteSpace(lblError.Text);
            }
        }

        #endregion

        public string ValidationError
        {
            get { return Text; }
            set { Text = value; }
        }
    }
}