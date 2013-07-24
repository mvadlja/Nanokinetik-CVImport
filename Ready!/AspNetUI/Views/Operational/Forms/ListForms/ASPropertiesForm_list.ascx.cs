using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using GEM2Common;
using CommonTypes;
using Kmis.Model;
using Ready.Model;
using System.Data;
using System.Drawing;
using AspNetUI.Support;

namespace AspNetUI.Views
{
    public partial class ASPropertiesForm_list : DetailsForm
    {
        // Model data managers
        IApproved_substance_PKOperations _approvedSubstanceOperations;
        ILast_change_PKOperations _last_change_PKOperations;
        IPerson_PKOperations _person_PKOperations;


        // Form initialization
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Retreiving model data managers from central configuration
            _approvedSubstanceOperations = new Approved_substance_PKDAL();
            _last_change_PKOperations = new Last_change_PKDAL();
            _person_PKOperations = new Person_PKDAL();

        }


        protected override void OnLoad(EventArgs e)
        {

        }

        #region FormOverrides

        public override object SaveForm(object id, string arg)
        {
            //
            object o = null;
            return o;
        }

        // Binds form
        public override void BindForm(object id, string arg)
        {
            string idAS = Request.QueryString["idAS"];
            if (idAS != null & idAS != "")
            {
                Approved_substance_PK entity = _approvedSubstanceOperations.GetEntity(Int32.Parse(idAS));
                if (entity == null)
                {
                    entity = new Approved_substance_PK();
                }
                lblLastChange.Text = AuditTrailHelper.GetLastChangeFormattedString(entity.approved_substance_PK, "APPROVED_SUBSTANCE", _last_change_PKOperations, _person_PKOperations);
                //lblSubstanceName.Text = !string.IsNullOrEmpty(entity.substancename) ? entity.substancename : "-";
                lblCasNumber.Text = !string.IsNullOrEmpty(entity.casnumber) ? entity.casnumber : "-";
                lblCbd.Text = !string.IsNullOrEmpty(entity.cbd) ? entity.cbd : "-";
                lblClass.Text = entity.Class.HasValue ? entity.Class.Value.ToString() : "-";
                lblComment.Text = !string.IsNullOrEmpty(entity.comments) ? entity.comments : "-";
                lblEvcodeName.Text = !string.IsNullOrEmpty(entity.ev_code) ? entity.ev_code : "-";
                lblMolecularFormula.Text = !string.IsNullOrEmpty(entity.molecularformula) ? entity.molecularformula : "-";
            }

        }

        // Clears form
        public override void ClearForm(string arg)
        {
            //
        }

        // Fills all form controls with data
        public override void FillDataDefinitions(string arg)
        {
            //
        }

        // Validates form
        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }

        // Deletes record
        //public override void DeleteItem(object id, string arg)
        //{
        //    // Deletes record from model
        //    //_APPropertiesOperations.Delete<int>(Convert.ToInt32(id));

        //    // Base method takes care of what happens after delete
        //    //base.DeleteItem(id, arg);
        //}

        // Optionally modify row after it was initially bound
        //public override void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        // 
        //    }
        //}

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