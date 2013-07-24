using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Interface;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class TextBox : System.Web.UI.UserControl, ILastChange, IArticle57Relevant, IXevprmValidationError
    {
        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivTextBox
        {
            get { return divTextBox; }
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

        public string Label
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public bool FontItalic
        {
            get { return lblName.Font.Italic; }
            set { lblName.Font.Italic = value; }
        }

        public string Text
        {
            get { return txtInput.Text ?? string.Empty; }
            set { txtInput.Text = value ?? string.Empty; }
        }

        public int MaxLength
        {
            get { return txtInput.MaxLength; }
            set { txtInput.MaxLength = value; }
        }

        public TextBoxMode TextMode
        {
            get { return txtInput.TextMode; }
            set { txtInput.TextMode = value; }
        }

        public int Rows
        {
            get { return txtInput.Rows; }
            set { txtInput.Rows = value; }
        }

        public int Columns
        {
            get { return txtInput.Columns; }
            set { txtInput.Columns = value; }
        }

        public bool Required
        {
            get { return spanRequired.Visible; }
            set { spanRequired.Visible = value; }
        }

        public bool Enabled
        {
            get { return txtInput.Enabled; }
            set { txtInput.Enabled = value; }
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

        public bool AutoPostback
        {
            get { return txtInput.AutoPostBack; }
            set { txtInput.AutoPostBack = value; }
        }

        public string ValidationError
        {
            get { return lblError.Text; }
            set { lblError.Text = value; }
        }

        public string OldValue
        {
            get { return ViewState["OldValue"] != null ? ViewState["OldValue"].ToString() : string.Empty; }
            set { ViewState["OldValue"] = value; }
        }

        #region ILastChange

        private bool? _isModified;
        public bool IsModified
        {
            get
            {
                if (!_isModified.HasValue) return OldValue != txtInput.Text;
                return _isModified.Value;
            }
            set { _isModified = value; }
        }

        #endregion

        #endregion

        #region Page methods

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OldValue = txtInput.Text;
            }
        }

        #endregion
    }
}