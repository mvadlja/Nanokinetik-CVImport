using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using GEM2Common;
using CommonTypes;
using Ready.Model;

namespace AspNetUI.Views
{
    public partial class RoleForm_list : ListForm
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

        // Binds form
        public override void BindForm(object id, string arg)
        {
            // Retreives collection from model and binds grid and pager
            List<USER_ROLE> entities = _roleOperations.GetEntities(gvPager.CurrentPage, gvPager.RecordsPerPage, out _mainTotalItemsCount);
            gvPager.TotalRecordsCount = _mainTotalItemsCount;
            gvData.DataSource = entities;
            gvPager.BindGridViewPager();
            gvData.DataBind();

            base.BindForm(id, arg);
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
        public override void DeleteItem(object id, string arg)
        {
            // Deletes record from model
            _roleOperations.Delete<int>(Convert.ToInt32(id));

            // Base method takes care of what happens after delete
            base.DeleteItem(id, arg);
        }
		
		// Optionally modify row after it was initially bound
        public override void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
				// 
            }
        }

        #endregion

        #region Security

        public override ListPermissionType CheckAccess()
        {
            return ListPermissionType.READ_WRITE;
        }

        #endregion
    }
}