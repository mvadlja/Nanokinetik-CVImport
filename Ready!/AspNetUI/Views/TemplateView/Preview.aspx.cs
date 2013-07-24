using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.TemplateView
{
    public partial class Preview : System.Web.UI.Page
    {
        #region Declarations

        private PreviewType _previewType;

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
            _previewType = PreviewType.Preview;

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