using System;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.Views.Shared.UserControl.Popup
{
    public partial class ManufacturerPopup : System.Web.UI.UserControl
    {
        IOrganization_PKOperations _organizationOperations;
        IType_PKOperations _typeOperations;
        ISubstance_PKOperations _substanceOperations;

        public virtual event EventHandler<FormEventArgs<Org_in_type_for_manufacturer>> OnOkButtonClick;
        public virtual event EventHandler<EventArgs> OnCancelButtonClick;

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return PopupControls_Entity_Container.Style["Width"]; }
            set { PopupControls_Entity_Container.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return PopupControls_Entity_Container.Style["Height"]; }
            set { PopupControls_Entity_Container.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style["padding"]; }
            set { modalPopupContainerBody.Style["padding"] = value; }
        }

        private Org_in_type_for_manufacturer Manufacturer
        {
            get { return (Org_in_type_for_manufacturer)ViewState["ManufacturerPopup_Manufacturer"]; }
            set { ViewState["ManufacturerPopup_Manufacturer"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PopupControls_Entity_Container.Style["display"] = "none";

            _organizationOperations = new Organization_PKDAL();
            _typeOperations = new Type_PKDAL();
            _substanceOperations = new Substance_PKDAL();

            ddlManufacturerType.DdlInput.SelectedIndexChanged += DdlInputSelectedIndexChanged;
            txtSrActiveSubstance.Searcher.OnListItemSelected += TxtSrActiveSubstanceSearcher_OnListItemSelected;
        }

        #endregion

        #region Form methods

        #region Initialize

        public void ShowModalForm(Org_in_type_for_manufacturer manufacturer)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            Manufacturer = manufacturer ?? new Org_in_type_for_manufacturer();

            InitForm(null);

            if (manufacturer != null)
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
            ddlManufacturer.Text = string.Empty;
            ddlManufacturerType.Text = string.Empty;
            txtSrActiveSubstance.Clear();
            txtSrActiveSubstance.Visible = false;
        }

        #endregion

        #region Fill

        private void FillFormControls(object args)
        {
            FillDdlManufacterTypes();
            FillDdlManufacters();
        }

        void SetFormControlsDefaults(object arg)
        {

        }

        private void FillDdlManufacterTypes()
        {
            var manufacturerTypeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.Manufacturer);
            ddlManufacturerType.Fill(manufacturerTypeList, "name", "type_PK");
            ddlManufacturerType.SortItemsByText();
        }

        private void FillDdlManufacters()
        {
            var manufacturerList = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.Manufacturer);
            ddlManufacturer.Fill(manufacturerList, "name_org", "organization_PK");
            ddlManufacturer.SortItemsByText();
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            //Manufacturer
            ddlManufacturer.SelectedValue = Manufacturer.organization_FK;

            //Manufacturer type
            ddlManufacturerType.SelectedValue = Manufacturer.org_type_for_manu_FK;

            //Active substance
            if (Manufacturer.ManufacturerTypeName != null && Manufacturer.ManufacturerTypeName.ToLower() == "active substance")
            {
                txtSrActiveSubstance.Visible = true;
                txtSrActiveSubstance.SelectedEntityId = Manufacturer.substance_FK;
                txtSrActiveSubstance.Text = Manufacturer.substance_FK.HasValue ? Manufacturer.SubstanceName : string.Empty;
            }
            else
            {
                txtSrActiveSubstance.Visible = false;
                txtSrActiveSubstance.Clear();
            }
        }

        #endregion

        #region Validate

        public bool ValidateForm(string arg)
        {
            ClearValidationErrors();

            var errorMessage = String.Empty;

            if (!ddlManufacturer.SelectedId.HasValue)
            {
                errorMessage += "Manufacturer can't be empty.<br />";
                ddlManufacturer.ValidationError = "Manufacturer can't be empty.";
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
            ddlManufacturer.ValidationError = string.Empty;
        }

        #endregion

        #region Save

        public object SaveForm(object args)
        {
            //Manufacturer
            Manufacturer.organization_FK = ddlManufacturer.SelectedId;
            Manufacturer.ManufacturerName = ddlManufacturer.SelectedItem.Text;

            //Manufacturer type
            Manufacturer.org_type_for_manu_FK = ddlManufacturerType.SelectedId;
            Manufacturer.ManufacturerTypeName = ddlManufacturerType.SelectedId.HasValue ? ddlManufacturerType.SelectedItem.Text : null;

            //Active substance
            Manufacturer.substance_FK = txtSrActiveSubstance.SelectedEntityId;
            Manufacturer.SubstanceName = txtSrActiveSubstance.SelectedEntityId.HasValue ? txtSrActiveSubstance.Text : null;
            
            return Manufacturer;
        }

        #endregion

        #endregion

        #region Event handlers

        void DdlInputSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlManufacturerType.SelectedItem.Text.ToLower() == "active substance")
            {
                txtSrActiveSubstance.Visible = true;
            }
            else
            {
                txtSrActiveSubstance.Visible = false;
                txtSrActiveSubstance.Clear();
            }
        }

        void TxtSrActiveSubstanceSearcher_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var substance = _substanceOperations.GetEntity(e.Data);

            if (substance == null || string.IsNullOrWhiteSpace(substance.substance_name)) return;

            txtSrActiveSubstance.SelectedEntityId = substance.substance_PK;
            txtSrActiveSubstance.Text = substance.substance_name;
        }

        public void btnOk_OnClick(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                SaveForm(null);
                PopupControls_Entity_Container.Style["display"] = "none";

                if (OnOkButtonClick != null)
                {
                    OnOkButtonClick(sender, new FormEventArgs<Org_in_type_for_manufacturer>(Manufacturer));
                }
            }
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            if (OnCancelButtonClick != null)
            {
                OnCancelButtonClick(sender, e);
            }
        }

        protected void btnClose_OnClick(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

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