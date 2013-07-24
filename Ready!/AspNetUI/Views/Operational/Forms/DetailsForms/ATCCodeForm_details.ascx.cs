using System;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.Views
{
    public partial class ATCCodeForm_details : DetailsForm
    {
        int _mainTotalItemsCount = 0;
        string s_apid = "";

        // Model data managers
        IAtc_PKOperations _atc_PKOperations;
        ILast_change_PKOperations _last_change_PKOperations;
        IUSEROperations _user_PKOperations;
        IPerson_PKOperations _person_PKOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnSaveButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;

        // Form initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Retreiving model data managers from central configuration
            _atc_PKOperations = new Atc_PKDAL();
            _last_change_PKOperations = new Last_change_PKDAL();
            _user_PKOperations = new USERDAL();
            _person_PKOperations = new Person_PKDAL();

        }

        public void btnSaveOnClick(object sender, EventArgs e)
        {
            if (OnSaveButtonClick != null)
                OnSaveButtonClick(null, new FormDetailsEventArgs(null));
        }

        public void btnCancelOnClick(object sender, EventArgs e)
        {
            if (OnCancelButtonClick != null)
                OnCancelButtonClick(null, new FormDetailsEventArgs(null));
        }

        #region FormOverrides

        // Saves form
        public override object SaveForm(object id, string arg)
        {

            Atc_PK entity = null;

            entity = _atc_PKOperations.GetEntity(id);

            if (entity == null)
            {
                entity = new Atc_PK();
            }
            
            entity.name = ctlName.ControlValue.ToString();
            entity.atccode = ctlATCCode.ControlValue.ToString();
            entity.is_maunal_entry = true;

            entity = _atc_PKOperations.Save(entity);

            if (!ListOperations.ListsEquals<int>(InitialFormHash, AuditTrailHelper.GetPanelHashValue(pnlDataDetails)))
            {
                AuditTrailHelper.UpdateLastChange(entity.atc_PK, "ATC_CODE", _last_change_PKOperations, _user_PKOperations);
            }

            return entity;

        }

        // Clears form
        public override void ClearForm(string arg)
        {
            ctlName.ControlValue = String.Empty;
            ctlATCCode.ControlValue = String.Empty;
        }

        // Fills all form controls with data
        public override void FillDataDefinitions(string arg)
        {
            if (Request.QueryString["id"] != null)
            {
                BindForm(Request.QueryString["id"].ToString(), null);
            }
        }

        // Binds form
        public override void BindForm(object id, string arg)
        {
            Atc_PK entity = null;

            if (id != null)
            {
                entity = _atc_PKOperations.GetEntity(Request.QueryString["id"].ToString());
                lblLastChange.ControlValue = AuditTrailHelper.GetLastChangeFormattedString(entity != null ? entity.atc_PK : null, "ATC_CODE", _last_change_PKOperations, _person_PKOperations);
                if (entity != null)
                {
                    ctlName.ControlValue = entity.name != "" ? entity.name : String.Empty;
                    ctlATCCode.ControlValue = entity.atccode != "" ? entity.atccode : String.Empty;

                    if (!IsInitialHashWritten)
                    {
                        InitialFormHash = AuditTrailHelper.GetPanelHashValue(pnlDataDetails);
                        IsInitialHashWritten = true;
                    }
                }
            }

        }

        // Validates form
        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ctlName.ControlValue.ToString())) errorMessage += ctlName.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlATCCode.ControlValue.ToString())) errorMessage += ctlATCCode.ControlEmptyErrorMessage + "<br />";

            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
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