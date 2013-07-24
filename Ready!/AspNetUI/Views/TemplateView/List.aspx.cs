using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.TemplateView
{
    public partial class List : System.Web.UI.Page
    {
        #region Declarations

        private ListType _listType;

        #endregion

        #region Properties

        #endregion

        #region Page methods

        protected void OnInit(object sender, EventArgs e)
        {
            base.OnInit(e);
        }

        protected void OnLoad(object sender, EventArgs e)
        {
            base.OnLoad(e);

            InitForm(null);
            BindForm(null);
        }

        #endregion

        #region Form methods

        void InitForm(object arg)
        {
            _listType = ListType.List;

            if (Request.QueryString["Action"] == ListType.Search.ToString())
            {
                _listType = ListType.Search;
            }

            ClearForm(null);
            FillFormControls(null);
            SetFormControlsDefaults(null);
        }

        void ClearForm(object arg)
        {

        }

        void FillFormControls(object arg)
        {

        }

        void SetFormControlsDefaults(object arg)
        {

        }

        void BindForm(object arg)
        {

        }

        void ValidateForm(object arg)
        {

        }

        void SaveForm(object arg)
        {

        }

        void DeleteEntity(object arg)
        {

        }

        #endregion

        #region Binding methods

        #endregion

        #region Event handlers

        #endregion

        #region Support methods

        #endregion

        #region Security

        #endregion
    }
}