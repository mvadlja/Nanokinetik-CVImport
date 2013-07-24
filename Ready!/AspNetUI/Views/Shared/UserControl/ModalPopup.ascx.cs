using System;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace AspNetUI.Views.Shared.UserControl
{
    public enum ModalPopupMode
    {
        Null,
        Ok,
        OkCancel,
        YesNo,
        YesNoCancel
    }

    public partial class ModalPopup : System.Web.UI.UserControl
    {
        #region Declarations

        public virtual event EventHandler<EventArgs> OnOkButtonClick;
        public virtual event EventHandler<EventArgs> OnYesButtonClick;
        public virtual event EventHandler<EventArgs> OnNoButtonClick;
        public virtual event EventHandler<EventArgs> OnCancelButtonClick;
        public virtual event EventHandler<EventArgs> OnCloseButtonClick;

        private ModalPopupMode _modalPopupMode;

        #endregion

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return mpContainer.Style["Width"]; }
            set { mpContainer.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return mpContainer.Style["Height"]; }
            set { mpContainer.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return mpContainer.Style["padding"]; }
            set { mpContainer.Style["padding"] = value; }
        }

        public bool IsVisible
        {
            get
            {
                return mpContainer.Attributes.CssStyle.Value.Contains("inline");
            }
        }

        public Panel PnlContent
        {
            get { return pnlContent; }
        }

        public Panel PnlHeader
        {
            get { return pnlHeader; }
        }

        public Button BtnOk
        {
            get { return btnOk; }
        }

        public Button BtnYes
        {
            get { return btnYes; }
        }

        public Button BtnNo
        {
            get { return btnNo; }
        }

        public Button BtnCancel
        {
            get { return btnCancel; }
        }

        public ModalPopupMode ModalPopupMode
        {
            get { return _modalPopupMode; }
            set
            {
                _modalPopupMode = value;
                switch (value)
                {
                    case ModalPopupMode.Null:
                        HideAllActions();
                        break;
                    case ModalPopupMode.Ok:
                        ShowOk();
                        break;
                    case ModalPopupMode.OkCancel:
                        ShowOkCancel();
                        break;
                    case ModalPopupMode.YesNo:
                        ShowYesNo();
                        break;
                    case ModalPopupMode.YesNoCancel:
                        ShowYesNoCancel();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("value");
                }
            }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            _modalPopupMode = ModalPopupMode.YesNoCancel;
            
            mpContainer.Style["display"] = "none";
        }

        #endregion

        #region Forms methods

        public void ClearForm(object args)
        {
            pnlHeader.Controls.Clear();
            pnlContent.Controls.Clear();
        }

        public void BindForm(object args)
        {
          
        }

        #endregion

        #region Event handlers

        protected void btnClose_Click(object sender, EventArgs e)
        {
            mpContainer.Style["display"] = "none";
            if (OnCloseButtonClick != null)
            {
                OnCloseButtonClick(sender, e);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            mpContainer.Style["display"] = "none";
            if (OnOkButtonClick != null)
            {
                OnOkButtonClick(sender, e);
            }
        }

        protected void btnYes_Click(object sender, EventArgs e)
        {
            mpContainer.Style["display"] = "none";
            if (OnYesButtonClick != null)
            {
                OnYesButtonClick(sender, e);
            }
        }

        protected void btnNo_Click(object sender, EventArgs e)
        {
            mpContainer.Style["display"] = "none";
            if (OnNoButtonClick != null)
            {
                OnNoButtonClick(sender, e);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            mpContainer.Style["display"] = "none";
            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick(sender, e);
            }
        }

        #endregion

        #region Support methods

        public void ShowModalPopup(object mpHeader, object mpContent, ModalPopupMode modalPopupMode = ModalPopupMode.Ok)
        {
            mpContainer.Style["display"] = "inline";

            ClearForm(null);

            ModalPopupMode = modalPopupMode;

            SetPanel(mpHeader, pnlHeader);
            SetPanel(mpContent, pnlContent);
        }

        private static void SetPanel(object content, Control panel) 
        {
            if (content is Panel) panel.Controls.Add((Panel)content);
            else if (content is String) panel.Controls.Add(new LiteralControl(content.ToString()));
            else panel.Controls.Add((Control)content);
        }

        private void HideAllActions()
        {
            btnOk.Visible = false;
            btnYes.Visible = false;
            btnNo.Visible = false;
            btnCancel.Visible = false;
        }

        private void ShowOk()
        {
            btnOk.Visible = true;
            btnYes.Visible = false;
            btnNo.Visible = false;
            btnCancel.Visible = false;
        }

        private void ShowOkCancel()
        {
            btnOk.Visible = true;
            btnYes.Visible = false;
            btnNo.Visible = false;
            btnCancel.Visible = true;
        }

        private void ShowYesNo()
        {
            btnOk.Visible = false;
            btnYes.Visible = true;
            btnNo.Visible = true;
            btnCancel.Visible = false;
        }

        private void ShowYesNoCancel()
        {
            btnOk.Visible = false;
            btnYes.Visible = true;
            btnNo.Visible = true;
            btnCancel.Visible = true;
        }

        #endregion
    }
}