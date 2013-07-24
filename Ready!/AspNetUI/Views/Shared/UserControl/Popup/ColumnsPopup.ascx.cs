using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.Views.Shared.UserControl.Popup
{
    public partial class ColumnsPopup : System.Web.UI.UserControl
    {
        #region Declarations

        public virtual event EventHandler<EventArgs> OnOkButtonClick;
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

        public ListItemCollection AvailableColumns
        {
            get { return lbAuColumns.LbInputFrom.Items; }
        }

        public ListItemCollection SelectedColumns
        {
            get { return lbAuColumns.LbInputTo.Items; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ModalPopupContainerBodyPadding = "30px";
            PopupControls_Entity_Container.Style["display"] = "none";
        }

        #endregion

        #region Form methods

        #region Initialize

        public void ShowModalForm()
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            lbAuColumns.LbInputFrom.SortItemsByText();
            lbAuColumns.LbInputTo.SortItemsByText();
        }

        void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
            SetFormControlsDefaults(null);
        }

        public void ClearForm(string arg)
        {

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

        }

        #endregion

        #region Validate

        public bool ValidateForm(string arg)
        {
            ClearValidationErrors();

            var errorMessage = String.Empty;

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

        }

        #endregion

        #region Save

        public object SaveForm(object args)
        {
            return null;
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
                    OnOkButtonClick(sender, e);
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