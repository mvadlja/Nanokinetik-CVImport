using System;
using System.Collections.Generic;
using AspNetUIFramework;
using AspNetUI.Support;
using Ready.Model;

namespace AspNetUI.Views.Shared.UserControl.Popup
{
    public partial class QuickLinksPopup : System.Web.UI.UserControl
    {
        #region Declarations

        public virtual event EventHandler<FormEventArgs<QuickLink>> OnOkButtonClick;
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

        private QuickLink QuickLink
        {
            get { return (QuickLink)ViewState["QuickLinksPopup_QuickLink"]; }
            set { ViewState["QuickLinksPopup_QuickLink"] = value; }
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

        public void ShowModalForm(QuickLink quickLink)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            QuickLink = quickLink ?? new QuickLink();

            InitForm(null);

            if (quickLink != null)
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
            txtQuickLinkName.Text = string.Empty;
            chcIsPublic.Checked = false;
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
            txtQuickLinkName.Text = QuickLink.Name;
            chcIsPublic.Checked = QuickLink.IsPublic.HasValue && QuickLink.IsPublic.Value;
        }

        #endregion

        #region Validate

        public bool ValidateForm(string arg)
        {
            ClearValidationErrors();

            var errorMessage = String.Empty;

            if (string.IsNullOrWhiteSpace(txtQuickLinkName.Text))
            {
                errorMessage += "Quick link name can't be empty.<br />";
                txtQuickLinkName.ValidationError = "Quick link name can't be empty.<br />";
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
            txtQuickLinkName.ValidationError = string.Empty;
        }

        #endregion

        #region Save

        public object SaveForm(object args)
        {
            QuickLink.Name = txtQuickLinkName.Text;
            QuickLink.IsPublic = chcIsPublic.Checked;

            return QuickLink;
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
                    OnOkButtonClick(sender, new FormEventArgs<QuickLink>(QuickLink));
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