using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;


namespace AspNetUI.Views.Shared.UserControl.Popup
{
    public partial class IndicationsPopup : System.Web.UI.UserControl
    {
        #region Declarations
        
        public virtual event EventHandler<FormEventArgs<Meddra_pk>> OnOkButtonClick;
        public virtual event EventHandler<EventArgs> OnCancelButtonClick;

        IMeddra_pkOperations _meddraOperations;
        IType_PKOperations _typeOperations;
        IAuthorisedProductOperations _authorisedProductOperations;

        #endregion

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return PopupControls_Struct_Container.Style["Width"]; }
            set { PopupControls_Struct_Container.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return PopupControls_Struct_Container.Style["Height"]; }
            set { PopupControls_Struct_Container.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style["padding"]; }
            set { modalPopupContainerBody.Style["padding"] = value; }
        }

        private Meddra_pk Indication
        {
            get { return (Meddra_pk)ViewState["IndicationsPopup_Indication"]; }
            set { ViewState["IndicationsPopup_Indication"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PopupControls_Struct_Container.Style["display"] = "none";

            _meddraOperations = new Meddra_pkDAL();
            _typeOperations = new Type_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();
        }

        #endregion

        #region Form methods

        #region Initialize

        public void ShowModalForm(Meddra_pk indication)
        {
            PopupControls_Struct_Container.Style["display"] = "inline";

            Indication = indication ?? new Meddra_pk();

            InitForm(null);

            if (indication != null)
            {
                BindForm(null);
            }
        }

        void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
            SetFormControlsDefaults(null);
        }

        public void ClearForm(string arg)
        {
            ddlMeddraVersion.Text = string.Empty;
            ddlMeddraLevel.Text = string.Empty;
            txtMeddraCode.Text = string.Empty;
            txtMeddraTerm.Text = string.Empty;
        }

        #endregion

        #region Fill

        private void FillFormControls(object args)
        {
            FillDdlMeddraVersion();
            FillDdlMeddraLevel();
        }

        void SetFormControlsDefaults(object arg)
        {

        }

        private void FillDdlMeddraVersion(int? selectedMeddraVersion = null)
        {
            var meddraVersionList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.MeddraVersion);

            ddlMeddraVersion.Fill(meddraVersionList, "name", "type_PK", false);
            ddlMeddraVersion.SortItemsByText(SortType.Asc, false, false);

            var lastItem = ddlMeddraVersion.DdlInput.Items.Cast<ListItem>().LastOrDefault();
            if (lastItem != null)
            {
                lastItem.Selected = true;
            }
            
            if (selectedMeddraVersion != null) ddlMeddraVersion.SelectedValue = selectedMeddraVersion;
        }

        private void FillDdlMeddraLevel(int? selectedMeddraLevel = null)
        {
            var meddraLevelList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.MeddraLevel);

            meddraLevelList.Sort((t1, t2) => t1.custom_sort.GetValueOrDefault().CompareTo(t2.custom_sort.GetValueOrDefault()));
            ddlMeddraLevel.Fill(meddraLevelList, "name", "type_PK", false);

            if (selectedMeddraLevel != null) ddlMeddraVersion.SelectedValue = selectedMeddraLevel;
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            ddlMeddraVersion.SelectedValue = Indication.version_type_FK;
            ddlMeddraLevel.SelectedValue = Indication.level_type_FK;
            txtMeddraCode.Text = Indication.code;
            txtMeddraTerm.Text = Indication.term;
        }

        #endregion

        #region Validate

        public bool ValidateForm(string arg)
        {
            ClearValidationErrors();

            var errorMessage = String.Empty;

            if (!ddlMeddraVersion.SelectedId.HasValue)
            {
                errorMessage += "MEDDRA Version can't be empty.<br />";
            } 
            
            if (!ddlMeddraLevel.SelectedId.HasValue)
            {
                errorMessage += "MEDDRA Level can't be empty.<br />";
            }

            if (string.IsNullOrWhiteSpace(txtMeddraCode.Text))
            {
                errorMessage += "MEDDRA Code can't be empty.<br />";
            } 

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
            ddlMeddraVersion.ValidationError = string.Empty;
            ddlMeddraLevel.ValidationError = string.Empty;
            txtMeddraCode.ValidationError = string.Empty;
            txtMeddraTerm.ValidationError = string.Empty;
        }

        #endregion

        #region Save

        public object SaveForm(object args)
        {
            Indication.level_type_FK = ddlMeddraLevel.SelectedId;
            Indication.version_type_FK = ddlMeddraVersion.SelectedId;
            Indication.code = txtMeddraCode.Text; 
            Indication.term = txtMeddraTerm.Text;
            Indication.MeddraFullName = Indications.GetFormattedText(Indication, string.Empty);

            return Indication;
        }

        #endregion

        #endregion

        #region Event handlers

        public void btnOk_OnClick(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                SaveForm(null);
                PopupControls_Struct_Container.Style["display"] = "none";

                if (OnOkButtonClick != null)
                {
                    OnOkButtonClick(sender, new FormEventArgs<Meddra_pk>(Indication));
                }
            }
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            PopupControls_Struct_Container.Style["display"] = "none";

            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick(sender, e);
            }
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            PopupControls_Struct_Container.Style["display"] = "none";

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