using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AspNetUI.Views.TemplateView
{
    public partial class Form : System.Web.UI.Page
    {
        #region Declarations

        private FormType _formType;

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

            if (_formType == FormType.Edit || _formType == FormType.SaveAs)
            {
                BindForm(null);
            }
        }

        #endregion

        #region Form methods

        void InitForm(object arg)
        {
            switch (Request.QueryString["Action"])
            {
                case "New":
                    _formType = FormType.New;
                    break;
                case "Edit":
                    _formType = FormType.Edit;
                    break;
                case "SaveAs":
                    _formType = FormType.SaveAs;
                    break;
                default:
                    _formType = FormType.New;
                    break;
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