using System;
using System.Collections.Generic;
using System.Linq;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormPKManuType : DetailsForm
    {
        IOrg_in_type_for_manufacturerOperations _organization_in_type_manufacturer_Operations; //spojna tablica
        IOrganization_PKOperations _organizationOperations;
        IType_PKOperations _type_PKOperations;
        ISubstance_PKOperations _substance_PKOperations;

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
            get { return (int)Session["OrgInTypeForManufacturer_id"]; }
            set { Session["OrgInTypeForManufacturer_id"] = value; }
        }

        private int? _pid
        {
            get { return (int?)Session["OrgInTypeForManufacturer_pid"]; }
            set { Session["OrgInTypeForManufacturer_pid"] = value; }
        }
        
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["OrgInTypeForManufacturer_popupFormMode"]; }
            set { Session["OrgInTypeForManufacturer_popupFormMode"] = value; }
        }
        private Org_in_type_for_manufacturer entity
        {
            get { return (Org_in_type_for_manufacturer)Session["OrgInTypeForManufacturer_entity"]; }
            set { Session["OrgInTypeForManufacturer_entity"] = value; }
        }

        public List<Org_in_type_for_manufacturer> ManTypes
        {
            get { return (List<Org_in_type_for_manufacturer>)Session["ManufacturerTypes"]; }
            set { Session["ManufacturerTypes"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Org_in_type_for_manufacturer inEntity)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            divHeader.InnerHtml = header;

            _pid = null;
            if (ValidationHelper.IsValidInt(id))
                _pid = (int?)Int32.Parse(id);

            if (inEntity == null)
            {
                entity = new Org_in_type_for_manufacturer();
                popupFormMode = PopupFormMode.New;

                _id = 1;
                if (ManTypes != null)
                {
                    int? highID = ManTypes.Max(manType => manType.org_in_type_for_manufacturer_ID);
                    _id = highID != null ?(int) highID + 1 : 1;
                }
                    

                entity.org_in_type_for_manufacturer_ID = _id;
                entity.product_FK = _pid;
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;

                if (inEntity.org_in_type_for_manufacturer_ID != null)
                    _id = (int)inEntity.org_in_type_for_manufacturer_ID;
            }
            
           BindForm(_id, null);
        }

        void SubNameSearcher_OnListItemSelected(object sender, FormListEventArgs e)
        {
            Substance_PK sub = _substance_PKOperations.GetEntity(e.DataItemID);

            if (sub != null && sub.substance_name != null)
            {
                ctlSubstanceSearcherDisplay.SetSelectedObject(sub.substance_PK, sub.substance_name);

            }
        }

        void SubNameSearcher_OnSearchClick(object sender, EventArgs e)
        {
            ctlSubstanceSearcher.ShowModalSearcher("SubName");
        }

        void SubNameSearcherDisplay_OnRemoveClick(object sender, EventArgs e)
        {
            ctlSubstanceSearcherDisplay.EnableSearcher(true);
        }

        void ManufacterTypeChanged(object sender, ValueChangedEventArgs e)
        {
       
            if (e.NewValue == null || String.IsNullOrEmpty(e.NewValue.ToString())) {
                this.RemoveSusbtanceSelector();
                return;
            }
            if (e.NewValue == null || !ValidationHelper.IsValidInt(e.NewValue.ToString())) return;
            Type_PK type = _type_PKOperations.GetEntity(e.NewValue.ToString());
            if (type.name.Trim().ToLower() == "active substance")
            {
                this.AddSubstanceSelector();
            }
            else
            {
                this.RemoveSusbtanceSelector();
            }
        }

        private void RemoveSusbtanceSelector()
        {
            ctlSubstanceLabel.Visible = false;
            ctlSubstanceSearcher.Visible = false;
            ctlSubstanceSearcherDisplay.Visible = false;
            ctlSubstanceSearcherDisplay.SetSelectedObject(null, null);
        }

        private void AddSubstanceSelector() {
            ctlSubstanceLabel.Visible = true;
            ctlSubstanceSearcher.Visible = true;
            ctlSubstanceSearcherDisplay.Visible = true;
            ctlSubstanceSearcherDisplay.SetSelectedObject(null, null);
        }

        #endregion

        #region FormOverrides

      

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            _organizationOperations = new Organization_PKDAL();
            _organization_in_type_manufacturer_Operations = new Org_in_type_for_manufacturer_DAL();
            _type_PKOperations = new Type_PKDAL();
            _substance_PKOperations = new Substance_PKDAL();
            ctlSubstanceSearcherDisplay.OnSearchClick += new EventHandler<EventArgs>(this.SubNameSearcher_OnSearchClick);
            ctlSubstanceSearcher.OnListItemSelected += new EventHandler<AspNetUIFramework.FormListEventArgs>(this.SubNameSearcher_OnListItemSelected);
            ctlSubstanceSearcherDisplay.OnRemoveClick += new EventHandler<EventArgs>(this.SubNameSearcherDisplay_OnRemoveClick);
            ctlManufacterTypes.InputValueChanged += new EventHandler<ValueChangedEventArgs>(this.ManufacterTypeChanged);

          

            base.OnInit(e);
        }

        public override object SaveForm(object id, string arg)
        {
            entity.organization_FK = ValidationHelper.IsValidInt(ctlManufacters.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlManufacters.ControlValue) : null;
            entity.org_type_for_manu_FK = ValidationHelper.IsValidInt(ctlManufacterTypes.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlManufacterTypes.ControlValue) : null;
            entity.substance_FK =ctlSubstanceSearcherDisplay.SelectedObject!=null ? (ValidationHelper.IsValidInt(ctlSubstanceSearcherDisplay.SelectedObject.ToString()) ? (int?)Convert.ToInt32(ctlSubstanceSearcherDisplay.SelectedObject.ToString()) : null):null;
            return entity;
        }

        public override void ClearForm(string arg)
        {
          
            ctlManufacters.ControlValue = String.Empty;
            ctlManufacterTypes.ControlValue = String.Empty;
            this.RemoveSusbtanceSelector();
        }

        public override void FillDataDefinitions(string arg)
        {
            BindDDLManufacterTypes();
            BindDDLManufacters();
        }

        public override bool ValidateForm(string arg)
        {
            
            string errorMessage = String.Empty;

            if (ctlManufacters.IsMandatory == true && String.IsNullOrEmpty(ctlManufacters.ControlValue.ToString())) errorMessage += ctlManufacters.ControlEmptyErrorMessage + "<br />";
            if (ctlManufacterTypes.IsMandatory == true && String.IsNullOrEmpty(ctlManufacterTypes.ControlValue.ToString())) errorMessage += ctlManufacterTypes.ControlEmptyErrorMessage + "<br />";
            
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

        private void BindDDLManufacterTypes()
        {
            //ctlManufacterTypes.ControlBoundItems.Clear();
            //List<Org_in_type_for_manufacturer_PK> items = _organization_manufacturer_type_Operations.GetEntities();

            //items.Sort(delegate(Org_in_type_for_manufacturer_PK s1, Org_in_type_for_manufacturer_PK s2)
            //{
            //    return s1.org_type_name.CompareTo(s2.org_type_name);
            //});

            //ctlManufacterTypes.SourceValueProperty = "org_in_type_for_manufacturer_PK";
            //ctlManufacterTypes.SourceTextExpression = "org_type_name";
            //ctlManufacterTypes.FillControl<Org_in_type_for_manufacturer_PK>(items);

            ctlManufacterTypes.ControlBoundItems.Clear();
            List<Type_PK> items = _type_PKOperations.GetTypesForDDL("MT");
            items.Sort(delegate(Type_PK t1, Type_PK t2){
                return t1.name.CompareTo(t2.name);
            });

            ctlManufacterTypes.SourceValueProperty = "type_PK";
            ctlManufacterTypes.SourceTextExpression = "name";
            ctlManufacterTypes.FillControl<Type_PK>(items);
            
        }
        private void BindDDLManufacters()
        {
            ctlManufacters.ControlBoundItems.Clear();
            List<Organization_PK> items = _organizationOperations.GetOrganizationsByRole("Manufacturer");

            items.Sort(delegate(Organization_PK s1, Organization_PK s2)
            {
                return s1.name_org.CompareTo(s2.name_org);
            });

            ctlManufacters.SourceValueProperty = "organization_PK";
            ctlManufacters.SourceTextExpression = "name_org";
            ctlManufacters.FillControl<Organization_PK>(items);
        }

        public override void BindForm(object id, string arg)
        {
            if (popupFormMode == PopupFormMode.Edit)
            {
                ctlManufacters.ControlValue = entity.organization_FK == null ? String.Empty : entity.organization_FK.ToString();
                ctlManufacterTypes.ControlValue = entity.org_type_for_manu_FK == null ? String.Empty : entity.org_type_for_manu_FK.ToString();
                Type_PK type = _type_PKOperations.GetEntity(entity.org_type_for_manu_FK);
                if (type != null && type.name.ToLower().Trim() == "active substance") this.AddSubstanceSelector();
                Substance_PK sub = entity.substance_FK != null? _substance_PKOperations.GetEntity(entity.substance_FK):null;
                if (sub !=null)
                {

                    ctlSubstanceSearcherDisplay.SetSelectedObject(sub.substance_PK, sub.substance_name);
                }
               
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