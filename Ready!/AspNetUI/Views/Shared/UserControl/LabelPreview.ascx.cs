using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Interface;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class LabelPreview : System.Web.UI.UserControl, ICustomEnabled, IArticle57Relevant, IReminderCustomControl
    {
        #region Properties

        public System.Web.UI.HtmlControls.HtmlGenericControl DivLabelPreview
        {
            get { return divLabelPreview; }
        }

        public Label LblName
        {
            get { return lblName; }
        }

        public Label LblValue
        {
            get { return lblValue; }
        }

        public LinkButton LnkSetReminder
        {
            get { return lnkSetReminder; }
        }

        public Panel PnlLinks
        {
            get { return pnlLinks; }
        }

        public string Label
        {
            get { return lblName.Text; }
            set { lblName.Text = value; }
        }

        public string Text
        {
            get { return lblValue.Text ?? string.Empty; }
            set { lblValue.Text = value; }
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
            get { return lblValue.Visible ? lblValue.Width : pnlLinks.Width; }
            set
            {
                lblValue.Width = value;
                pnlLinks.Width = value;
            }
        }

        public bool ShowReminder
        {
            get { return lnkSetReminder.Visible; }
            set { lnkSetReminder.Visible = value; }
        }

        public bool ShowLinks
        {
            get { return pnlLinks.Visible; }
            set
            {
                if (value)
                {
                    lblValue.Visible = false;
                    pnlLinks.Visible = true;

                    divLabelPreview.Attributes["class"] = "row labelPv-links";

                    // Removes Constant.DefaultEmptyValue
                    if (pnlLinks.Controls.Count > 0) pnlLinks.Controls.Clear();
                }
                else
                {
                    lblValue.Visible = true;
                    pnlLinks.Visible = false;

                    divLabelPreview.Attributes["class"] = "row";

                    // Adds Constant.DefaultEmptyValue
                    pnlLinks.Controls.Clear();
                    pnlLinks.Controls.Add(new LiteralControl { Text = Constant.DefaultEmptyValue });
                }
            }
        }

        public bool TextBold
        {
            get 
            {
                return lblValue.Visible ? lblValue.Font.Bold : pnlLinks.Font.Bold;
            }
            set
            {
                lblValue.Font.Bold = value;
                pnlLinks.AddCssClass("bold");
            }
        }

        public bool IsEmpty
        {
            get
            {
                if (lblValue.Visible)
                {
                    return string.IsNullOrWhiteSpace(lblValue.Text) || lblValue.Text == Constant.DefaultEmptyValue;
                }

                return pnlLinks.Controls.Count == 0 || pnlLinks.Controls.Cast<Control>().OfType<LiteralControl>().Any(c => c.Text == Constant.DefaultEmptyValue);
            }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            lblValue.Text = Constant.DefaultEmptyValue;
            pnlLinks.Controls.Add(new LiteralControl() { Text = Constant.DefaultEmptyValue });
        }

        #endregion

        #region ICustomEnabled

        public void Enable(List<WebControl> controlList = null)
        {
            if (controlList != null && controlList.Any()) controlList.ForEach(c => { if (c != null) c.Enabled = true; });
            else
        {
            lnkSetReminder.Enabled = true;
        }
        }

        public void Disable(List<WebControl> controlList = null)
        {
            if (controlList != null && controlList.Any()) controlList.ForEach(c => { if (c != null) c.Enabled = false; });
            else
        {
            lnkSetReminder.Enabled = false;
        }
        }

        #endregion
    }
}