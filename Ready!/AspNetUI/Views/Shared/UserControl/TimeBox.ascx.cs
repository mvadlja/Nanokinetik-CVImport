using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Interface;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class TimeBox : System.Web.UI.UserControl, ILastChange, IArticle57Relevant, IXevprmValidationError
    {
        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivTimeBox
        {
            get { return divTimeBox; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public System.Web.UI.WebControls.TextBox TxtInputHours
        {
            get { return txtInputHours; }
        }

        public System.Web.UI.WebControls.TextBox TxtInputMinutes
        {
            get { return txtInputMinutes; }
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

        public string TextHours
        {
            get { return txtInputHours.Text ?? string.Empty; }
            set { txtInputHours.Text = value ?? string.Empty; }
        }

        public string TextMinutes
        {
            get { return txtInputMinutes.Text ?? string.Empty; }
            set { txtInputMinutes.Text = value ?? string.Empty; }
        }

        public int MaxLengthHours
        {
            get { return txtInputHours.MaxLength; }
            set { txtInputHours.MaxLength = value; }
        }

        public int MaxLengthMinutes
        {
            get { return txtInputMinutes.MaxLength; }
            set { txtInputMinutes.MaxLength = value; }
        }

        public bool Required
        {
            get { return spanRequired.Visible; }
            set { spanRequired.Visible = value; }
        }

        public bool Enabled
        {
            get { return txtInputHours.Enabled && txtInputMinutes.Enabled; }
            set
            {
                txtInputHours.Enabled = value;
                txtInputMinutes.Enabled = value;
            }
        }

        public Unit LabelWidth
        {
            get { return lblName.Width; }
            set { lblName.Width = value; }
        }

        public Unit TextWidthHours
        {
            get { return txtInputHours.Width; }
            set { txtInputHours.Width = value; }
        }

        public Unit TextWidthMinutes
        {
            get { return txtInputMinutes.Width; }
            set { txtInputMinutes.Width = value; }
        }

        public bool AutoPostback
        {
            get { return txtInputHours.AutoPostBack && txtInputMinutes.AutoPostBack; }
            set
            {
                txtInputHours.AutoPostBack = value;
                txtInputMinutes.AutoPostBack = value;
            }
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
                if (!_isModified.HasValue) return OldValue != txtInputHours.Text;
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
                OldValue = txtInputHours.Text + txtInputMinutes.Text;
            }
        }

        #endregion

        #region Methods

        public void Clear()
        {
            txtInputHours.Text = string.Empty;
            txtInputMinutes.Text = string.Empty;
        }

        #endregion
    }
}