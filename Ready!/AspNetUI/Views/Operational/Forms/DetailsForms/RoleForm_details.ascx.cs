using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using GEM2Common;
using CommonTypes;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.Views
{
    public partial class RoleForm_details : DetailsForm
    {
        int _mainTotalItemsCount = 0;

        // Model data managers
        IUSER_ROLEOperations _roleOperations;

		// Form initialization
		protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Retreiving model data managers from central configuration
            _roleOperations = new USER_ROLEDAL();
        }

        #region FormOverrides

		// Saves form
        public override object SaveForm(object id, string arg)
        {
            USER_ROLE entity = null;

            entity = _roleOperations.GetEntity(id);

            if (entity == null)
            {
                // new Role
                entity = new USER_ROLE();
            }
			
			entity.Name = ctlName.ControlValue.ToString();
			entity.Display_name = ctlDisplayName.ControlValue.ToString();
			entity.Description = ctlDescription.ControlValue.ToString();
			entity.Active = (ctlActive.ControlValue as List<string>).Count > 0 ? true : false;

            entity = _roleOperations.Save(entity);

            return entity;
        }

        // Clears form
        public override void ClearForm(string arg)
        {
			ctlID.ControlLabel = String.Empty;
			
			ctlName.ControlValue = String.Empty;
			ctlDisplayName.ControlValue = String.Empty;
			ctlDescription.ControlValue = String.Empty;
            FillDataDefinitions("");
        }

		// Fills all form controls with data
        public override void FillDataDefinitions(string arg)
        {
            ctlActive.FillControl<bool>(new List<bool>() { true });
        }

        // Binds form
        public override void BindForm(object id, string arg)
        {
            USER_ROLE entity = null;

            if (id != null)
            {
                entity = _roleOperations.GetEntity(id);

                if (entity != null)
                {
                    ctlID.ControlLabel = "Entity ID: " + entity.User_role_PK.ToString() + "<hr/>";
					
					ctlName.ControlValue = entity.Name == null ? String.Empty : entity.Name.ToString();
                    ctlDisplayName.ControlValue = entity.Display_name == null ? String.Empty : entity.Display_name.ToString();
					ctlDescription.ControlValue = entity.Description == null ? String.Empty : entity.Description.ToString();
					ctlActive.ControlValue = !entity.Active.HasValue ? String.Empty : entity.Active.Value.ToString();
                }                                                
            }
            else
            {
                ctlID.ControlLabel = "New entity<hr/>";
            }
        }

        // Validates form
        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ctlName.ControlValue.ToString())) errorMessage += ctlName.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlDisplayName.ControlValue.ToString())) errorMessage += ctlDisplayName.ControlEmptyErrorMessage + "<br />";

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