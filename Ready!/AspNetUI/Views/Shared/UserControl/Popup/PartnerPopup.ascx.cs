using System;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.Views.Shared.UserControl.Popup
{
    public partial class PartnerPopup : System.Web.UI.UserControl
    {
        IOrganization_PKOperations _organizationOperations;
        IType_PKOperations _typeOperations;

        public virtual event EventHandler<FormEventArgs<Org_in_type_for_partner>> OnOkButtonClick;
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

        private Org_in_type_for_partner Partner
        {
            get { return (Org_in_type_for_partner)ViewState["PartnerPopup_Partner"]; }
            set { ViewState["PartnerPopup_Partner"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            PopupControls_Entity_Container.Style["display"] = "none";

            _organizationOperations = new Organization_PKDAL();
            _typeOperations = new Type_PKDAL();
        }

        #endregion

        #region Form methods

        #region Initialize

        public void ShowModalForm(Org_in_type_for_partner partner)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            Partner = partner ?? new Org_in_type_for_partner();

            InitForm(null);

            if (partner != null)
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
            ddlPartner.Text = string.Empty;
            ddlPartnerType.Text = string.Empty;
        }

        #endregion

        #region Fill

        private void FillFormControls(object args)
        {
            FillDdlPartnerType();
            FillDdlPartner();
        }

        void SetFormControlsDefaults(object arg)
        {

        }

        private void FillDdlPartnerType()
        {
            var partnerTypeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.Partner);
            ddlPartnerType.Fill(partnerTypeList, "name", "type_PK");
            ddlPartnerType.SortItemsByText();
        }

        private void FillDdlPartner()
        {
            var partnerList = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.Partner);
            ddlPartner.Fill(partnerList, "name_org", "organization_PK");
            ddlPartner.SortItemsByText();
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            // Partner
            ddlPartner.SelectedValue = Partner.organization_FK;

            // Partner type
            ddlPartnerType.SelectedValue = Partner.org_type_for_partner_FK;
        }

        #endregion

        #region Validate

        public bool ValidateForm(string arg)
        {
            ClearValidationErrors();

            var errorMessage = String.Empty;

            if (!ddlPartner.SelectedId.HasValue)
            {
                errorMessage += "Partner can't be empty.<br />";
                ddlPartner.ValidationError = "Partner can't be empty.";
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
            ddlPartner.ValidationError = string.Empty;
        }

        #endregion

        #region Save

        public object SaveForm(object args)
        {
            // Partner
            Partner.organization_FK = ddlPartner.SelectedId;
            Partner.PartnerName = ddlPartner.SelectedId.HasValue ? ddlPartner.SelectedItem.Text : null;

            // Partner type
            Partner.org_type_for_partner_FK = ddlPartnerType.SelectedId;
            Partner.PartnerTypeName = ddlPartnerType.SelectedId.HasValue ? ddlPartnerType.SelectedItem.Text : null;

            return Partner;
        }

        #endregion

        #endregion

        #region Event handlers

        public void btnOk_OnClick(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                SaveForm(null);
                PopupControls_Entity_Container.Style["display"] = "none";

                if (OnOkButtonClick != null)
                {
                    OnOkButtonClick(sender, new FormEventArgs<Org_in_type_for_partner>(Partner));
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