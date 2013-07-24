using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormQPPV : DetailsForm
    {
        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;

        public TextBox_CT displayName;
        public CheckBox isPublic;

        private Qppv_code_PK editingQppv = null;

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

        public int EditingIndex
        {
            get
            {
                if (ViewState["QppvEditingIndex"] != null)
                    return (int)ViewState["QppvEditingIndex"];
                else
                    return -1;
            }

            set 
            {
                ViewState["QppvEditingIndex"] = value;
            }
        }

        #endregion

        #region Operations

        public void ShowModalForm()
        {
            
            PopupControls_Entity_Container.Style["display"] = "inline";
        }

        public void ShowModalForm(Qppv_code_PK item, int index)
        {

            PopupControls_Entity_Container.Style["display"] = "inline";
            ctlQPPV.ControlValue = item.qppv_code;
            this.EditingIndex = index;
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            divHeader.InnerHtml = "QPPV code";


            base.OnInit(e);
        }

        public override object SaveForm(object id, string arg)
        {
            return null;
        }

        public override void ClearForm(string arg)
        {
            ctlQPPV.ControlValue = String.Empty;
            this.EditingIndex = -1;
            
        }

        public override void FillDataDefinitions(string arg)
        {

        }


        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ctlQPPV.ControlValue.ToString())) errorMessage += ctlQPPV.ControlEmptyErrorMessage + "</br>";

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
                if (this.editingQppv == null) this.editingQppv = new Qppv_code_PK();
                this.editingQppv.qppv_code = ctlQPPV.ControlTextValue;

                if (OnOkButtonClick!=null) OnOkButtonClick(sender, new FormDetailsEventArgs(new List<Object>{this.editingQppv, this.EditingIndex}));
                ClearForm(null);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ClearForm(null);
            if (OnCancelButtonClick!=null) OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
        }

        protected void btnClosePopupForm_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ClearForm(null);

           if (OnCancelButtonClick!=null) OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
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