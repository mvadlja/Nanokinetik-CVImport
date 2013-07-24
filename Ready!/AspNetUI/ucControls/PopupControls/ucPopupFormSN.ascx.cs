using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using AspNetUIFramework;
using AspNetUI;
using GEM2Common;
using Kmis.Model;
using Ready.Model;
using AspNetUI.Support;


namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormSN : DetailsForm
    {
        
        #region Declarations
        ISubstance_name_PKOperations _substance_name_PKOperations;
        ISsi__cont_voc_PKOperations _ssi__cont_voc_PKOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "SubstanceName";

        #endregion

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

        private SSIRepository SSIRep
        {
            get { return (SSIRepository)Session["SSIRepository"]; }
            set { Session["SSIRepository"] = value; }
        }
        private int _id
        {
            get { return (int)Session["SN_id"]; }
            set { Session["SN_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["SN_entityOC"]; }
            set { Session["SN_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["SN_entityParentOC"]; }
            set { Session["SN_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["SN_popupFormMode"]; }
            set { Session["SN_popupFormMode"] = value; }
        }
        private Substance_name_PK entity
        {
            get { return (Substance_name_PK)Session["SN_entity"]; }
            set { Session["SN_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Substance_name_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Substance_name_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.substance_name_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {  
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.substance_name_PK != null)
                    _id = (int)inEntity.substance_name_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);
                
                BindForm(_id, null);
                entity = new Substance_name_PK();
                SaveForm(_id, null);
                entity.substance_name_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects =  inEntityOC.AssignedObjects;
                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
                
                BindAssignedObjects();
            }
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            _substance_name_PKOperations = new Substance_name_PKDAL();
            _ssi__cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();

            RefSourcesPopupForm.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(RefSourcesPopupForm_OnCancelClick);
            RefSourcesPopupForm.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(RefSourcesPopupForm_OnOkClick);
            ONPopupForm.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(ONPopupForm_OnCancelClick);
            ONPopupForm.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(ONPopupForm_OnOkClick);

            base.OnInit(e);
        }

        public override object SaveForm(object id, string arg)
        {
            entity.subst_name = ctlSubName.ControlValue.ToString();
            entity.subst_name_type_FK = ValidationHelper.IsValidInt(ctlSubNameType.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlSubNameType.ControlValue) : null;
            entity.language_FK = ValidationHelper.IsValidInt(ctlLanguage.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlLanguage.ControlValue) : null;
            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlSubName.ControlValue = String.Empty;
            ctlSubNameType.ControlValue = String.Empty;
            ctlLanguage.ControlValue = String.Empty;
            ctlON.ControlBoundItems.Clear();
            ctlRefSources.ControlBoundItems.Clear();
        }

        public override void FillDataDefinitions(string arg)
        {
            BindDDLNameType();
            BindDDLLanguage();
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ctlSubName.ControlValue.ToString())) errorMessage += ctlSubName.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlSubNameType.ControlValue.ToString())) errorMessage += ctlSubNameType.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlLanguage.ControlValue.ToString())) errorMessage += ctlLanguage.ControlEmptyErrorMessage + "<br />";

            if (Session["SubNameONMandatory"] != null && (bool)Session["SubNameONMandatory"] == true)
                if (ctlON.ControlBoundItems.Count == 0) errorMessage += "Official name can't be empty<br />";
            if (ctlRefSources.ControlBoundItems.Count == 0) errorMessage += "Reference source can't be empty<br />";
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }

        private void BindDDLNameType()
        {
            ctlSubNameType.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi__cont_voc_PKOperations.GetEntitiesByListName("Substance Name Type");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlSubNameType.SourceValueProperty = "ssi__cont_voc_PK";
            ctlSubNameType.SourceTextExpression = "term_name_english";
            ctlSubNameType.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLLanguage()
        {
            ctlLanguage.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi__cont_voc_PKOperations.GetEntitiesByListName("Language");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlLanguage.SourceValueProperty = "ssi__cont_voc_PK";
            ctlLanguage.SourceTextExpression = "term_name_english";
            ctlLanguage.FillControl<Ssi__cont_voc_PK>(items);
        }
    
        private void BindRefSources()
        {
            ctlRefSources.ControlBoundItems.Clear();
            List<Reference_source_PK> items = SSIRep.GetObjectsList<Reference_source_PK>("ReferenceSource", entityOC);

            foreach (Reference_source_PK rs in items)
            {
                string rstype = _ssi__cont_voc_PKOperations.GetEntity(rs.rs_type_FK).term_name_english;
                string rsclass = _ssi__cont_voc_PKOperations.GetEntity(rs.rs_class_FK).term_name_english;
                if (rstype != null && rsclass != null)
                    ctlRefSources.ControlBoundItems.Add(new ListItem(String.Format("{0} {1}", rstype, rsclass), rs.reference_source_PK.ToString()));
            }
            
        }
        private void BindON()
        {
            ctlON.ControlBoundItems.Clear();
            List<Official_name_PK> items = SSIRep.GetObjectsList<Official_name_PK>("OfficialName", entityOC);

            foreach (Official_name_PK on in items)
            {
                string ontype = _ssi__cont_voc_PKOperations.GetEntity(on.on_type_FK).term_name_english;
                string onstatus = _ssi__cont_voc_PKOperations.GetEntity(on.on_status_FK).term_name_english;
                if (ontype != null && onstatus!=null)
                    ctlON.ControlBoundItems.Add(new ListItem(String.Format("{0} {1}", ontype, onstatus), on.official_name_PK.ToString()));
            }
            if (ctlON.ControlBoundItems.Count == 0)
            {
                btnAddON.Enabled = true;
                btnEditON.Enabled = btnRemoveON.Enabled = false;
            }
            else
            {
                btnEditON.Enabled = btnAddON.Enabled = true;
                btnAddON.Enabled = false;
            }   
        }
        public override void BindForm(object id, string arg)
        {
            if (popupFormMode == PopupFormMode.Edit)
            {
                ctlSubName.ControlValue = entity.subst_name == null ? String.Empty : entity.subst_name;
                ctlSubNameType.ControlValue = entity.subst_name_type_FK == null ? String.Empty : entity.subst_name_type_FK.ToString();
                ctlLanguage.ControlValue = entity.language_FK == null ? String.Empty : entity.language_FK.ToString();
            }
        }

        private void BindAssignedObjects()
        {
            BindRefSources();
            BindON();
        }

        #endregion
#region Events
        public void ctlNameTypeInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (ctlSubNameType.ControlTextValue.Contains("Official"))
            {
                lblONReq.Visible = true;
                Session["SubNameONMandatory"] = true;
                Session["SubNameSCMandatory"] = false;
            }
            else if (ctlSubNameType.ControlTextValue.ToString().Contains("Code"))
            {
                lblONReq.Visible = false;
                Session["SubNameONMandatory"] = false;
                Session["SubNameSCMandatory"] = true;
            }
            else
            {
                lblONReq.Visible = false;
                Session["SubNameONMandatory"] = false;
                Session["SubNameSCMandatory"] = false;
            }
        }
#endregion

        #region Form methods

        public void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                SaveForm(_id, null);
                SSIRep.SaveState(entityOC, entityType,entityParentOC);

                PopupControls_Entity_Container.Style["display"] = "none";
                OnOkButtonClick(sender, new FormDetailsEventArgs(entity));
                ClearForm(null);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ClearForm(null);

            SSIRep.DiscardObjectAndRestore(entityOC, entityType, entityParentOC);
            OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
        }

        protected void btnClosePopupForm_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ClearForm(null);

            SSIRep.DiscardObjectAndRestore(entityOC, entityType, entityParentOC);
            OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
        }

        #endregion

        #region PopupForms

        #region Official names

        public void ctlONListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditON.Enabled = false;
            btnRemoveON.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlON.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveON.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditON.Enabled = true;
                btnRemoveON.Enabled = true;
            }
        }

        public void btnAddONOnClick(object sender, EventArgs e)
        {
            ONPopupForm.ShowModalForm(_id.ToString(), "Official name", null, entityOC);
        }

        void ONPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindON();
            ListItemCollection lic = ctlON.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditON.Enabled = false;
            btnRemoveON.Enabled = false;
        }
        void ONPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlON.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditON.Enabled = false;
            btnRemoveON.Enabled = false;
        }
        public void btnEditONOnClick(object sender, EventArgs e)
        {
            string idRS = "0";
            foreach (ListItem item in ctlON.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idRS = item.Value.ToString();
                    break;
                }
            }
            Official_name_PK rs = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idRS), "OfficialName", entityOC) as Official_name_PK;

            ONPopupForm.ShowModalForm(_id.ToString(), "Official name", rs, entityOC);
        }

        public void btnRemoveONOnClick(object sender, EventArgs e)
        {
            string idRS = "0";

            foreach (ListItem item in ctlON.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idRS = item.Value;
                    ctlON.ControlBoundItems.Remove(item);
                    break;
                }
            }

            SSIRep.DeleteObjectByID(Int32.Parse(idRS), "OfficialName", entityOC);

            BindON();
            btnEditON.Enabled = false;
            btnRemoveON.Enabled = false;
        }

        #endregion

        #region Reference sources

        public void ctlRefSourcesListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditRefSource.Enabled = false;
            btnRemoveRefSources.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlRefSources.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveRefSources.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditRefSource.Enabled = true;
                btnRemoveRefSources.Enabled = true;
            }
        }

        public void btnAddRefSourceOnClick(object sender, EventArgs e)
        {
            RefSourcesPopupForm.ShowModalForm(_id.ToString(), "Reference source", null, entityOC);
        }

        void RefSourcesPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindRefSources();
            ListItemCollection lic = ctlRefSources.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditRefSource.Enabled = false;
            btnRemoveRefSources.Enabled = false;
        }
        void RefSourcesPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlRefSources.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditRefSource.Enabled = false;
            btnRemoveRefSources.Enabled = false;
        }
        public void btnEditRefSourceOnClick(object sender, EventArgs e)
        {
            string idRS = "0";
            foreach (ListItem item in ctlRefSources.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idRS = item.Value.ToString();
                    break;
                }
            }
            Reference_source_PK rs = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idRS), "ReferenceSource", entityOC) as Reference_source_PK;

            RefSourcesPopupForm.ShowModalForm(_id.ToString(), "Reference source", rs, entityOC);
        }

        public void btnRemoveRefSourcesOnClick(object sender, EventArgs e)
        {
            string idRS = "0";

            foreach (ListItem item in ctlRefSources.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idRS = item.Value;
                    ctlRefSources.ControlBoundItems.Remove(item);
                    break;
                }
            }

            SSIRep.DeleteObjectByID(Int32.Parse(idRS), "ReferenceSource", entityOC);

            BindRefSources();
            btnEditRefSource.Enabled = false;
            btnRemoveRefSources.Enabled = false;
        }

        #endregion

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

