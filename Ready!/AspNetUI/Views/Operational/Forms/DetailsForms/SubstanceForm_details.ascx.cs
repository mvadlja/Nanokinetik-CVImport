using System;
using System.Collections.Generic;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.Views
{
    public partial class SubstanceForm_details : DetailsForm
    {
        int _mainTotalItemsCount = 0;
        string s_apid = "";

        // Model data managers
        ISubstance_PKOperations _substanceOperations;
        ILast_change_PKOperations _last_change_PKOperations;
        IUSEROperations _user_PKOperations;
        IPerson_PKOperations _person_PKOperations;
		// Form initialization

        public virtual event EventHandler<FormDetailsEventArgs> OnSaveButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;

		protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            s_apid = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";

            // Retreiving model data managers from central configuration
            _substanceOperations = new Substance_PKDAL();
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
            Substance_PK entity = null;

            entity = _substanceOperations.GetEntity(id);

            if (entity == null)
            {
                entity = new Substance_PK();
            }

            entity.substance_name = ctlName.ControlValue.ToString();
            entity.ev_code = ctlEVCode.ControlValue.ToString();

            //entity.synonym1 = ctlSynonym1.ControlValue.ToString();
            //entity.synonym1_language = ctlSynonym1Language.ControlValue.ToString();
            //entity.synonym2 = ctlSynonym2.ControlValue.ToString();
            //entity.synonym2_language = ctlSynonym2Language.ControlValue.ToString();

            entity = _substanceOperations.Save(entity);

            if (!ListOperations.ListsEquals<int>(InitialFormHash, AuditTrailHelper.GetPanelHashValue(pnlDataDetails)))
            {
                AuditTrailHelper.UpdateLastChange(entity.substance_PK, "SUBSTANCE", _last_change_PKOperations, _user_PKOperations);
            }

            return entity;
        }

        // Clears form
        public override void ClearForm(string arg)
        {
            ctlName.ControlValue = String.Empty;
            ctlEVCode.ControlValue = String.Empty;

            //lblName.Text = String.Empty;
            //lblEVcode.Text = String.Empty;
            //lblSynonym1.Text = String.Empty;
            //lblSynonym1Language.Text = String.Empty;
            //lblSynonym2.Text = String.Empty;
            //lblSynonym2Language.Text = String.Empty;
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
            Substance_PK entity = null;

            if (id != null)
            {
                entity = _substanceOperations.GetEntity(Request.QueryString["id"].ToString());

                lblLastChange.ControlValue = AuditTrailHelper.GetLastChangeFormattedString(entity != null ? entity.substance_PK : null, "SUBSTANCE", _last_change_PKOperations, _person_PKOperations);

                if (entity != null)
                {
                    ctlName.ControlValue = !String.IsNullOrWhiteSpace(entity.substance_name) ? entity.substance_name : String.Empty;
                    ctlEVCode.ControlValue = !String.IsNullOrWhiteSpace(entity.ev_code) ? entity.ev_code : String.Empty;

                    //lblName.Text = entity.substance_name != "" ? entity.substance_name : String.Empty;
                    //lblEVcode.Text = entity.ev_code != "" ? entity.ev_code : String.Empty;
                    //lblSynonym1.Text = entity.synonym1 != "" ? entity.synonym1 : String.Empty;
                    //lblSynonym1Language.Text = entity.synonym1_language != "" ? entity.synonym1_language : String.Empty;
                    //lblSynonym2.Text = entity.synonym2 != "" ? entity.synonym2 : String.Empty;
                    //lblSynonym2Language.Text = entity.synonym2_language != "" ? entity.synonym2_language : String.Empty;

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

            if (String.IsNullOrWhiteSpace(ctlName.ControlValue.ToString())) errorMessage += ctlName.ControlEmptyErrorMessage + "<br />";
            //if (String.IsNullOrWhiteSpace(ctlEVCode.ControlValue.ToString())) errorMessage += ctlEVCode.ControlEmptyErrorMessage + "<br />";
            if (!String.IsNullOrWhiteSpace(ctlEVCode.ControlValue.ToString()))
            {
                if (Request.QueryString["f"] == "dn")
                {
                    List<Substance_PK> listSubstances = _substanceOperations.GetEntities();
                    string evcode = ctlEVCode.ControlValue.ToString().Trim().ToLower();
                    if (listSubstances.Find(item => !string.IsNullOrWhiteSpace(item.ev_code) && item.ev_code.Trim().ToLower() == evcode) != null) errorMessage += "Substance with this EV Code already exists.<br />";
                }
            }
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