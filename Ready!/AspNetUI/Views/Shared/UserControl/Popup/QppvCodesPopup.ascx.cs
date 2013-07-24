using System;
using AspNetUI.Support;
using Ready.Model;

namespace AspNetUI.Views.Shared.UserControl.Popup
{
    public partial class QppvCodesPopup : System.Web.UI.UserControl
    {
        #region Declarations

        public virtual event EventHandler<FormEventArgs<Qppv_code_PK>> OnOkButtonClick;
        public virtual event EventHandler<EventArgs> OnCancelButtonClick;

        #endregion

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return PopupControls_Entity_Container.Style["Width"]; }
            set { PopupControls_Entity_Container.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return PopupControls_Entity_Container.Style["Height"]; }
            set { PopupControls_Entity_Container.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style["padding"]; }
            set { modalPopupContainerBody.Style["padding"] = value; }
        }

        private Qppv_code_PK QppvCode
        {
            get { return (Qppv_code_PK)ViewState["QppvCodesPopup_QuickLink"]; }
            set { ViewState["QppvCodesPopup_QuickLink"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PopupControls_Entity_Container.Style["display"] = "none";
        }

        #endregion

        #region Form methods

        #region Initialize

        public void ShowModalForm(Qppv_code_PK qppvCode)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            QppvCode = qppvCode ?? new Qppv_code_PK();

            InitForm(null);

            if (qppvCode != null)
            {
                BindForm(null);
            }
        }

        void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
            SetFormControlsDefaults(null);
        }

        public void ClearForm(string arg)
        {
            txtQppvCode.Text = string.Empty;
        }

        #endregion

        #region Fill

        private void FillFormControls(object args)
        {

        }

        void SetFormControlsDefaults(object arg)
        {

        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            txtQppvCode.Text = QppvCode.qppv_code;
        }

        #endregion

        #region Validate

        public bool ValidateForm(string arg)
        {
            ClearValidationErrors();

            var errorMessage = String.Empty;

            if (string.IsNullOrWhiteSpace(txtQppvCode.Text))
            {
                errorMessage += "QPPV code can't be empty.<br />";
                txtQppvCode.ValidationError = "QPPV code can't be empty.<br />";
            }

            // If errors were found, showing them in modal popup
            if (!string.IsNullOrEmpty(errorMessage))
            {
                var masterPage = (Template.Default)Page.Master;

                if (masterPage != null)
                {
                    masterPage.ModalPopup.ShowModalPopup("Error!", errorMessage);
                }

                return false;
            }

            return true;
        }

        private void ClearValidationErrors()
        {
            txtQppvCode.ValidationError = string.Empty;
        }

        #endregion

        #region Save

        public object SaveForm(object args)
        {
            QppvCode.qppv_code = txtQppvCode.Text;

            return QppvCode;
        }

        #endregion

        #endregion

        #region Event handlers

        public void btnOk_OnClick(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                SaveForm(null);
                PopupControls_Entity_Container.Style["display"] = "none";

                if (OnOkButtonClick != null)
                {
                    OnOkButtonClick(sender, new FormEventArgs<Qppv_code_PK>(QppvCode));
                }
            }
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick(sender, e);
            }
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick(sender, e);
            }
        }

        #endregion

        #region Security

        #endregion
    }
}