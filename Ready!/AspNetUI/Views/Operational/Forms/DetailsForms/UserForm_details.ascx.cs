using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using GEM2Common;
using CommonTypes;
using Kmis.Model;
using System.Transactions;
using System.Web.Security;
using System.Net.Mail;
using System.Configuration;
using CommonComponents;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.Views
{
    public partial class UserForm_details : DetailsForm
    {
        // Model data managers
        IUSEROperations _userOperations;
        IUSER_IN_ROLEOperations _userInRoleOperations;
        IUSER_ROLEOperations _roleOperations;
        USER _user;

		// Form initialization
		protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Retreiving model data managers from central configuration
            _userOperations = new USERDAL();
            _userInRoleOperations = new USER_IN_ROLEDAL();
            _roleOperations = new USER_ROLEDAL();
        }

        #region FormOverrides

		// Saves form
        public override object SaveForm(object id, string arg)
        {
            USER entity = null;

            entity = _userOperations.GetEntity(id);

            if (entity == null)
            {
                entity = new USER();
            }
			
			entity.Username = ctlUserName.ControlValue.ToString();
            entity.Active = (ctlActive.ControlValue as List<string>).Count > 0 ? true : false;

            if (!string.IsNullOrWhiteSpace(ctlPassword.ControlValue.ToString()))
            {
                entity.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(ctlPassword.ControlValue.ToString(), "SHA1");
            }

            using (TransactionScope ts = new TransactionScope())
            {
                entity = _userOperations.Save(entity);

                List<USER_IN_ROLE> existingRoles = _userInRoleOperations.GetUsersInRolesByUserID(entity.User_PK.Value);
                List<USER_IN_ROLE> updatedRoles = new List<USER_IN_ROLE>();
                List<int> rolesToRemove = new List<int>();

                // Deleting removed roles, and adding new
                foreach (ListItem li in ctlUserRoles.ControlBoundItems)
                {
                    USER_IN_ROLE existingUir = existingRoles.Find((USER_IN_ROLE uir) => uir.User_role_FK.ToString() == li.Value);

                    // Add
                    if (li.Selected)
                    {
                        if (existingUir != null) updatedRoles.Add(existingUir);
                        else updatedRoles.Add(new USER_IN_ROLE(null, entity.User_PK.Value, Convert.ToInt32(li.Value), null));
                    }
                    // Remove
                    else
                    {
                        if (existingUir != null) rolesToRemove.Add(existingUir.User_in_role_PK.Value);
                    }
                }

                // Delete roles for removal
                _userInRoleOperations.DeleteCollection(rolesToRemove);
                // Save roles for update
                _userInRoleOperations.SaveCollection(updatedRoles);

                ts.Complete();
            }

            return entity;
        }

        // Clears form
        public override void ClearForm(string arg)
        {
			ctlUserName.ControlValue = String.Empty;
            ctlPassword.ControlValue = String.Empty;
            ctlUserRoles.ControlValue = String.Empty;
            BindChkActive();
        }

		// Fills all form controls with data
        public override void FillDataDefinitions(string arg)
        {
            BindLBRoles();
            BindChkActive();

            if (Request.QueryString["id"] != null)
            {
                BindForm(Request.QueryString["id"].ToString(), null);
            }
        }

        // Binds form
        public override void BindForm(object id, string arg)
        {
            USER entity = null;

            // Edit
            if (id != null)
            {
                entity = _userOperations.GetEntity(id);

                if (entity != null)
                {
					ctlUserName.ControlValue = entity.Username == null ? String.Empty : entity.Username.ToString();
                    ctlActive.ControlValue = !entity.Active.HasValue ? "" : entity.Active.Value.ToString();

                    List<USER_ROLE> usersRoles = _roleOperations.GetRolesByUserID(Convert.ToInt32(id));

                    // Selecting user's roles
                    foreach (ListItem li in ctlUserRoles.ControlBoundItems)
                    {
                        if (usersRoles.Find((USER_ROLE r) => r.User_role_PK.ToString() == li.Value) != null) li.Selected = true;
                        else li.Selected = false;
                    }
                }            
            }
        }

        // Validates form
        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrWhiteSpace(ctlUserName.ControlValue.ToString())) errorMessage += ctlUserName.ControlEmptyErrorMessage + "<br />";

            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }

        #endregion

        void BindLBRoles()
        {
            ctlUserRoles.ControlBoundItems.Clear();

            List<USER_ROLE> items = _roleOperations.GetEntities();

            items.Sort(delegate(USER_ROLE p1, USER_ROLE p2)
            {
                return p1.Name.CompareTo(p2.Name);
            });

            
            // LB binding
            ctlUserRoles.SourceValueProperty = "User_role_PK";
            ctlUserRoles.SourceTextExpression = "Name";
            ctlUserRoles.FillControl<USER_ROLE>(items);
        }

        void BindChkActive()
        {
            List<bool> active = new List<bool>() { true };
            ctlActive.FillControl<bool>(active);
        }

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