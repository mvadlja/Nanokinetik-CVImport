using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormNAME : DetailsForm
    {
        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;

        public TextBox_CT displayName;
        public CheckBox isPublic;

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
            divHeader.InnerHtml = "Save Quick link";

            displayName = ctlSearchName;
            isPublic = cbxPublic;

            base.OnInit(e);
        }

        public override object SaveForm(object id, string arg)
        {
            return null;
        }

        public override void ClearForm(string arg)
        {
            
        }

        public override void FillDataDefinitions(string arg)
        {
            
        }

     

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ctlSearchName.ControlValue.ToString())) errorMessage += ctlSearchName.ControlEmptyErrorMessage + "</br>";
           
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
                List<object> arguments = new List<object>() { ctlSearchName.ControlValue, cbxPublic.Checked };
                OnOkButtonClick(sender, new FormDetailsEventArgs(arguments));
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

        #region Security

        public override DetailsPermissionType CheckAccess()
        {
            if (SecurityOperations.CheckUserRole("Office"))
            {
                return DetailsPermissionType.READ_WRITE;
            }

            if (SecurityOperations.CheckUserRole("User"))
            {
                return DetailsPermissionType.READ;
            }

            return DetailsPermissionType.READ;
        }

        #endregion
    }
}