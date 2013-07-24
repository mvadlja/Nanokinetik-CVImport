using System;
using AspNetUIFramework;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormSave : DetailsForm
    {
        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;

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
      
        #endregion

        #region Operations

        public void ShowModalForm()
        {
            PopupControls_Entity_Container.Style["display"] = "inline";
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            base.OnInit(e);
        }

        public override object SaveForm(object id, string arg)
        {
            return null;
        }

        public override void ClearForm(string arg)
        {
            ctlName.ControlValue = "";
        }

        public override void FillDataDefinitions(string arg)
        {
            
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ctlName.ControlValue.ToString())) errorMessage += ctlName.ControlEmptyErrorMessage + "</br>";
           
            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else
                return true;
        }


        public override void BindForm(object id, string arg)
        {
           
        }

        #endregion

        #region Form methods
        public void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                PopupControls_Entity_Container.Style["display"] = "none";
                OnOkButtonClick(sender, new FormDetailsEventArgs(ctlName.ControlValue));
                ClearForm(null);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ClearForm(null);

            OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
        }

        protected void btnClosePopupForm_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ClearForm(null);

            OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
        }

        #endregion

        public override DetailsForm.DetailsPermissionType CheckAccess()
        {
            throw new NotImplementedException();
        }
    }
}