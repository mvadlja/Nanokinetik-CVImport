using System;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.Views.Shared.UserControl.Popup
{
    public partial class AlerterUserStatusPopup : System.Web.UI.UserControl
    {
        private IReminder_user_status_PKOperations _reminderUserStatusOperations;

        public virtual event EventHandler<FormEventArgs<int?>> OnOkButtonClick;
        public virtual event EventHandler<EventArgs> OnCancelButtonClick;

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

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PopupControls_Entity_Container.Style["display"] = "none";

            _reminderUserStatusOperations = new Reminder_user_status_PKDAL(); 
        }

        #endregion

        #region Form methods

        #region Initialize

        public void ShowModalPopup(int? reminderUserStatusPk)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            InitForm(null);

            BindForm(reminderUserStatusPk);
        }

        void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
            SetFormControlsDefaults(null);
        }

        public void ClearForm(string arg)
        {
            ddlAlerterUserStatus.Text = string.Empty;
        }

        #endregion

        #region Fill

        private void FillFormControls(object args)
        {
            FillDdlAlerterUserStatus();
        }

        void SetFormControlsDefaults(object arg)
        {

        }

        private void FillDdlAlerterUserStatus()
        {
            var alerteruserStatusList = _reminderUserStatusOperations.GetEntities();
            ddlAlerterUserStatus.Fill(alerteruserStatusList, x=> x.name, x=> x.reminder_user_status_PK);
            ddlAlerterUserStatus.SortItemsByText();
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            //Alerter user status
            ddlAlerterUserStatus.SelectedValue = arg;
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
            ddlAlerterUserStatus.ValidationError = string.Empty;
        }

        #endregion

        #region Save

        public object SaveForm(object args)
        {
            return ddlAlerterUserStatus.SelectedId;
        }

        #endregion

        #endregion

        #region Event handlers

        public void btnOk_OnClick(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                PopupControls_Entity_Container.Style["display"] = "none";

                if (OnOkButtonClick != null)
                {
                    OnOkButtonClick(sender, new FormEventArgs<int?>(ddlAlerterUserStatus.SelectedId));
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