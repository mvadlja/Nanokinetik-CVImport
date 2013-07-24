using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Interface;
using AspNetUIFramework;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class TextBoxSr : System.Web.UI.UserControl, ILastChange, ICustomEnabled, IArticle57Relevant, IXevprmValidationError
    {
        #region Declarations

        public EventHandler<FormEventArgs<object>> OnSelect;
        public EventHandler<FormEventArgs<object>> OnRemove;

        #endregion

        #region Properties

        public Searcher Searcher
        {
            get { return searcher; }
        }

        public System.Web.UI.HtmlControls.HtmlGenericControl DivTextBoxSr
        {
            get { return divTextBoxSr; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public System.Web.UI.WebControls.TextBox TxtInput
        {
            get { return txtInput; }
        }

        public LinkButton LbtnSelect
        {
            get { return lbtnSelect; }
        }

        public System.Web.UI.HtmlControls.HtmlGenericControl SpanSrSeparator
        {
            get { return spanSrSeparator; }
        }

        public LinkButton LbtnRemove
        {
            get { return lbtnRemove; }
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

        public SearchType SearchType { get; set; }

        public string ValidationError
        {
            get { return lblError.Text; }
            set { lblError.Text = value; }
        }

        public int? SelectedEntityId
        {
            get { return ValidationHelper.IsValidInt(selectedValue.Value) ? (int?)Convert.ToInt32(selectedValue.Value) : null; }
            set { selectedValue.Value = Convert.ToString(value); }
        }

        public string SelectedValue
        {
            get { return selectedValue.Value; }
            set { selectedValue.Value = value; }
        }

        public void Clear()
        {
            txtInput.Text = string.Empty;
            selectedValue.Value = null;
        }

        public int? OldValue
        {
            get { return ViewState["OldValue"] != null && ValidationHelper.IsValidInt(ViewState["OldValue"].ToString()) ? (int?)ViewState["OldValue"] : null; }
            set { ViewState["OldValue"] = value; }
        }

        public bool IsModified
        {
            get { return OldValue != SelectedEntityId; }
        }

        #endregion

        #region Page methods

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                OldValue = SelectedEntityId;
            }
        }

        #endregion

        #region Event Handlers

        protected void lbtnSelect_OnClick(object sender, EventArgs e)
        {
            if (OnSelect != null)
            {
                OnSelect(sender, new FormEventArgs<object>());
            }
            else
            {
                searcher.ShowModalSearcher(SearchType);    
            }
        }

        protected void lbtnRemove_OnClick(object sender, EventArgs e)
        {
            if (OnRemove != null)
            {
                OnRemove(sender, new FormEventArgs<object>());
            }
            else
            {
                txtInput.Text = String.Empty;
                selectedValue.Value = null;
            }
        }

        #endregion

        #region ICustomEnabled

        public void Enable(List<WebControl> controlList = null)
        {
            if (controlList != null && controlList.Any()) controlList.ForEach(c => c.Enabled = true);
            else
        {
            txtInput.Enabled = false;
            lbtnSelect.Enabled = true;
            lbtnRemove.Enabled = true;
        }
        }

        public void Disable(List<WebControl> controlList = null)
        {
            if (controlList != null && controlList.Any()) controlList.ForEach(c => { if (c != null) c.Enabled = false; });
            else
        {
            txtInput.Enabled = false;
            lbtnSelect.Enabled = false;
            lbtnRemove.Enabled = false;
        }
        }

        #endregion
    }
}