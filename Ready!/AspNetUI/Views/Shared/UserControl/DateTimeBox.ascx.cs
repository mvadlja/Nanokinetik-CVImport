using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Interface;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class DateTimeBox : System.Web.UI.UserControl, ILastChange, ICustomEnabled, IArticle57Relevant, IXevprmValidationError, IReminderCustomControl
    {
        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivTextBox
        {
            get { return divDateBox; }
        }

        public string CssClass
        {
            get { return divDateBox.Attributes["class"] ?? string.Empty; }
            set { divDateBox.Attributes["class"] = value; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public System.Web.UI.WebControls.TextBox TxtInput
        {
            get { return txtInput; }
        }

        public Label LblError
        {
            get { return lblError; }
        }

        public LinkButton LnkSetReminder
        {
            get { return lnkSetReminder; }
        }

        public string Label
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public string Text
        {
            get { return txtInput.Text ?? string.Empty; }
            set { txtInput.Text = value; }
        }

        public int MaxLength
        {
            get { return txtInput.MaxLength; }
            set { txtInput.MaxLength = value; }
        }

        public bool Required
        {
            get { return spanRequired.Visible; }
            set { spanRequired.Visible = value; }
        }

        public bool Enabled
        {
            get { return txtInput.Enabled; }
            set
            {
                txtInput.Enabled = value;
                ceInput.Enabled = value;
            }
        }

        public Unit LabelWidth
        {
            get { return lblName.Width; }
            set { lblName.Width = value; }
        }

        public Unit TextWidth
        {
            get { return txtInput.Width; }
            set { txtInput.Width = value; }
        }

        public bool ShowReminder
        {
            get { return lnkSetReminder.Visible; }
            set { lnkSetReminder.Visible = value; }
        }

        public string ValidationError
        {
            get { return lblError.Text; }
            set { lblError.Text = value; }
        }

        public bool ErrorDisplayInline
        {
            set { divDateBox.Attributes.Add("class", value ? "row error-display-inline" : "error-display-inline"); }
        }

        public string OldValue
        {
            get { return ViewState["OldValue"] != null ? ViewState["OldValue"].ToString() : string.Empty; }
            set { ViewState["OldValue"] = value; }
        }

        public bool IsModified
        {
            get { return OldValue != txtInput.Text; }
        }

        #endregion

        #region Page methods

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OldValue = txtInput.Text;
                if (Request.QueryString["Action"] != null && Request.QueryString["Action"] == "New")
                {
                    lnkSetReminder.Visible = false;
                }
            }
        }

        #endregion

        #region ICustomEnabled

        public void Enable(List<WebControl> controlList = null)
        {
            if (controlList != null && controlList.Any()) controlList.ForEach(c => { if (c != null) c.Enabled = true; });
            else
            {
                lnkSetReminder.Enabled = true;
                ceInput.Enabled = true;
                txtInput.Enabled = true;
            }
        }

        public void Disable(List<WebControl> controlList = null)
        {
            if (controlList != null && controlList.Any()) controlList.ForEach(c => { if (c != null) c.Enabled = false; });
            else
            {
                lnkSetReminder.Enabled = false;
                ceInput.Enabled = false;
                txtInput.Enabled = false;
            }
        }

        #endregion
    }
}