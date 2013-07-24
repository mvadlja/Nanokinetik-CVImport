using System;
using System.Web.UI;
using AspNetUIFramework;

namespace AspNetUI.Support
{
    public partial class ucConfirmAction : UserControl
    {
        public event EventHandler<FormPopupEventArgs> OnConfirmInputButtonYes_Click;
        public event EventHandler<FormPopupEventArgs> OnConfirmInputButtonNo_Click;
        public event EventHandler<FormPopupEventArgs> OnConfirmInputButtonCancel_Click;

        public enum ConfirmActionType
        {
            YES_NO_DIALOG,
            YES_NO_CANCEL_DIALOG
        };

        private ConfirmActionType actionType;

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return PopupControls_ConfirmInput_Container.Style["Width"]; }
            set { PopupControls_ConfirmInput_Container.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return PopupControls_ConfirmInput_Container.Style["Height"]; }
            set { PopupControls_ConfirmInput_Container.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style["padding"]; }
            set { modalPopupContainerBody.Style["padding"] = value; }
        }

        private string _action
        {
            get { return (string)ViewState["ucConfirmAction_Action"]; }
            set { ViewState["ucConfirmAction_Action"] = value; }
        }

        private object _args
        {
            get { return (object)ViewState["ucConfirmAction_Args"]; }
            set { ViewState["ucConfirmAction_Args"] = value; }
        }

        private ConfirmActionType PopupType {
            get { return actionType; }
            set { this.actionType = value; }
        }

        #endregion

        #region Operations

        public void ShowModalPopup(string action, object args = null, string message = "Are you sure?", string header = "Confirm action", ConfirmActionType actionType=ConfirmActionType.YES_NO_DIALOG)
        {
            PopupControls_ConfirmInput_Container.Style["display"] = "inline";

            _action = action;
            _args = args;
            
            divHeader.InnerText = header;
            divMessage.InnerText = message;
            btnYes.CommandName = action.ToUpper();
            switch (actionType) { 
                case ConfirmActionType.YES_NO_DIALOG:
                    btnYes.Text = "Yes";
                    btnNo.Text = "No";
                    cancelButtonContainer.Visible = false;
                    break;
                case ConfirmActionType.YES_NO_CANCEL_DIALOG:
                    btnYes.Text = "Yes";
                    btnNo.Text = "No";
                    cancelButtonContainer.Visible = true;
                    break;
                default:
                    btnYes.Text = "Yes";
                    btnNo.Text = "No";
                    cancelButtonContainer.Visible = true;
                    break;
            }

            //btnYes.Text = buttonOKText;
            //btnNo.Text = buttonCancelText;
        }


        /*
        //public void ShowModalPopup(string action, object args = null, string buttonOKText = "Yes", string buttonCancelText = "No")
        //{
        //    PopupControls_ConfirmInput_Container.Style["display"] = "inline";

        //    _action = action;
        //    _args = args;

        //    btnOK.Text = buttonOKText;
        //    btnCancel.Text = buttonCancelText;
        //}
        */
        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_ConfirmInput_Container.Style["display"] = "none";

            base.OnInit(e);
        }

        #endregion

        #region methods

        #endregion

        protected void btnYes_Click(object sender, EventArgs e)
        {
            if (OnConfirmInputButtonYes_Click != null)
            {
                OnConfirmInputButtonYes_Click(sender, new FormPopupEventArgs(_action,_args));
            }
            PopupControls_ConfirmInput_Container.Style["display"] = "none";
        }
        
        protected void btnNo_Click(object sender, EventArgs e)
        {
            if (OnConfirmInputButtonNo_Click != null)
            {
                OnConfirmInputButtonNo_Click(sender, new FormPopupEventArgs(_action, _args));
            }
            PopupControls_ConfirmInput_Container.Style["display"] = "none";
        }

        protected void btnCancel_Click(object sender, EventArgs e) {
            if (OnConfirmInputButtonCancel_Click != null) 
            {
                OnConfirmInputButtonCancel_Click(sender, new FormPopupEventArgs(_action, _args));
            }
            PopupControls_ConfirmInput_Container.Style["display"] = "none";
        } 
    }
}