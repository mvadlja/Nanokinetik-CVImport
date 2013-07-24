using System;
using System.Collections.Generic;
using System.Linq;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormPKPartnerType : DetailsForm
    {
        IOrg_in_type_for_partnerOperations _organization_in_type_partner_Operations; //spojna tablica
        IOrganization_PKOperations _organizationOperations;
        IType_PKOperations _type_PKOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;

        private enum PopupFormMode { New, Edit };

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
       
        private int _id
        {
            get { return (int)Session["OrgInTypeForPartner_id"]; }
            set { Session["OrgInTypeForPartner_id"] = value; }
        }

        private int? _pid
        {
            get { return (int?)Session["OrgInTypeForPartner_pid"]; }
            set { Session["OrgInTypeForPartner_pid"] = value; }
        }
        
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["OrgInTypeForPartner_popupFormMode"]; }
            set { Session["OrgInTypeForPartner_popupFormMode"] = value; }
        }
        private Org_in_type_for_partner entity
        {
            get { return (Org_in_type_for_partner)Session["OrgInTypeForPartner_entity"]; }
            set { Session["OrgInTypeForPartner_entity"] = value; }
        }

        public List<Org_in_type_for_partner> PartnerTypes
        {
            get { return (List<Org_in_type_for_partner>)Session["PartnerTypes"]; }
            set { Session["PartnerTypes"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Org_in_type_for_partner inEntity)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            divHeader.InnerHtml = header;

            _pid = null;
            if (ValidationHelper.IsValidInt(id))
                _pid = (int?)Int32.Parse(id);

            if (inEntity == null)
            {
                entity = new Org_in_type_for_partner();
                popupFormMode = PopupFormMode.New;

                _id = 1;
                if (PartnerTypes != null)
                {
                    int? highID = PartnerTypes.Max(partnerType => partnerType.org_in_type_for_partner_ID);
                    _id = highID != null ?(int) highID + 1 : 1;
                }
                    

                entity.org_in_type_for_partner_ID = _id;
                entity.product_FK = _pid;
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;

                if (inEntity.org_in_type_for_partner_ID != null)
                    _id = (int)inEntity.org_in_type_for_partner_ID;
            }
            
           BindForm(_id, null);
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            _organizationOperations = new Organization_PKDAL();
            _organization_in_type_partner_Operations = new Org_in_type_for_partner_DAL();
            _type_PKOperations = new Type_PKDAL();

            base.OnInit(e);
        }

        public override object SaveForm(object id, string arg)
        {
            entity.organization_FK = ValidationHelper.IsValidInt(ctlPartners.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlPartners.ControlValue) : null;
            entity.org_type_for_partner_FK = ValidationHelper.IsValidInt(ctlPartnerTypes.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlPartnerTypes.ControlValue) : null;
            
            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlPartners.ControlValue = String.Empty;
            ctlPartnerTypes.ControlValue = String.Empty;
        }

        public override void FillDataDefinitions(string arg)
        {
            BindDDLPartnerTypes();
            BindDDLPartners();
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (ctlPartners.IsMandatory == true && String.IsNullOrEmpty(ctlPartners.ControlValue.ToString())) errorMessage += ctlPartners.ControlEmptyErrorMessage + "<br />";
            if (ctlPartnerTypes.IsMandatory == true && String.IsNullOrEmpty(ctlPartnerTypes.ControlValue.ToString())) errorMessage += ctlPartnerTypes.ControlEmptyErrorMessage + "<br />";

            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }
        #endregion

        #region Binds

        private void BindDDLPartnerTypes()
        {
            //ctlPartnerTypes.ControlBoundItems.Clear();
            //List<Org_type_for_partner_PK> items = _organization_partner_type_Operations.GetEntities();

            //items.Sort(delegate(Org_type_for_partner_PK s1, Org_type_for_partner_PK s2)
            //{
            //    return s1.org_type_name.CompareTo(s2.org_type_name);
            //});

            //ctlPartnerTypes.SourceValueProperty = "org_type_for_partner_PK";
            //ctlPartnerTypes.SourceTextExpression = "org_type_name";
            //ctlPartnerTypes.FillControl<Org_type_for_partner_PK>(items);

            ctlPartnerTypes.ControlBoundItems.Clear();
            List<Type_PK> items = _type_PKOperations.GetTypesForDDL("PT");
            items.Sort(delegate(Type_PK t1, Type_PK t2)
            {
                return t1.name.CompareTo(t2.name);
            });

            ctlPartnerTypes.SourceValueProperty = "type_PK";
            ctlPartnerTypes.SourceTextExpression = "name";
            ctlPartnerTypes.FillControl<Type_PK>(items);
        }
        private void BindDDLPartners()
        {
            ctlPartners.ControlBoundItems.Clear();
            List<Organization_PK> items = _organizationOperations.GetOrganizationsByRole("Partner");

            items.Sort(delegate(Organization_PK s1, Organization_PK s2)
            {
                return s1.name_org.CompareTo(s2.name_org);
            });

            ctlPartners.SourceValueProperty = "organization_PK";
            ctlPartners.SourceTextExpression = "name_org";
            ctlPartners.FillControl<Organization_PK>(items);
        }

        public override void BindForm(object id, string arg)
        {
            if (popupFormMode == PopupFormMode.Edit)
            {
                ctlPartners.ControlValue = entity.organization_FK == null ? String.Empty : entity.organization_FK.ToString();
                ctlPartnerTypes.ControlValue = entity.org_type_for_partner_FK == null ? String.Empty : entity.org_type_for_partner_FK.ToString();
            }
        }

        #endregion

        #region Form methods

        public void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                SaveForm(_id, null);
                PopupControls_Entity_Container.Style["display"] = "none";
                OnOkButtonClick(sender, new FormDetailsEventArgs(entity));
                ClearForm(null);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ClearForm(null);
            OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
        }

        protected void btnClosePopupForm_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ClearForm(null);

            OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
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