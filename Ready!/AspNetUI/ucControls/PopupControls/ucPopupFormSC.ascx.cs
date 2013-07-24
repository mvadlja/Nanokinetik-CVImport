using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;


namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormSC : DetailsForm
    {        
        #region Declarations

        ISubstance_code_PKOperations _substance_code_PKOperations;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "SubstanceCode";

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
            get { return (int)Session["SC_id"]; }
            set { Session["SC_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["SC_entityOC"]; }
            set { Session["SC_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["SC_entityParentOC"]; }
            set { Session["SC_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["SC_popupFormMode"]; }
            set { Session["SC_popupFormMode"] = value; }
        }
        private Substance_code_PK entity
        {
            get { return (Substance_code_PK)Session["SC_entity"]; }
            set { Session["SC_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Substance_code_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Substance_code_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.substance_code_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {  
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.substance_code_PK != null)
                    _id = (int)inEntity.substance_code_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);
                
                BindForm(_id, null);
                entity = new Substance_code_PK();
                SaveForm(_id, null);
                entity.substance_code_PK = _id;

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

            _substance_code_PKOperations = new Substance_code_PKDAL();
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();

            RefSourcesPopupForm.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(RefSourcesPopupForm_OnOkClick);
            RefSourcesPopupForm.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(RefSourcesPopupForm_OnCancelClick);

            base.OnInit(e);
        }

        public override object SaveForm(object id, string arg)
        {
            entity.code = ctlSubCode.ControlValue.ToString();
            entity.code_system_FK = ValidationHelper.IsValidInt(ctlSubCodeSystem.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlSubCodeSystem.ControlValue) : null;
            entity.code_system_id_FK = ValidationHelper.IsValidInt(ctlSubCodeSystemID.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlSubCodeSystemID.ControlValue) : null;
            entity.code_system_status_FK = ValidationHelper.IsValidInt(ctlSubCodeSystemStatus.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlSubCodeSystemStatus.ControlValue) : null;
            entity.code_system_changedate = ValidationHelper.IsValidDateTime(ctlSubCodeSystemChangeDate.ControlValue.ToString()) ? ctlSubCodeSystemChangeDate.ControlValue.ToString() : null;
            entity.comment = ctlSubCodeComment.ControlValue.ToString();

            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlSubCode.ControlValue = String.Empty;
            ctlSubCodeSystem.ControlValue = String.Empty;
            ctlSubCodeSystemID.ControlValue = String.Empty;
            ctlSubCodeSystemStatus.ControlValue = String.Empty;
            ctlSubCodeSystemChangeDate.ControlValue = String.Empty;
            ctlSubCodeComment.ControlValue = String.Empty;
            ctlRefSources.ControlBoundItems.Clear();
        }

        public override void FillDataDefinitions(string arg)
        {
            BindDDLCodeSystem();
            BindDDLCodeSystemID();
            BindDDLCodeSystemStatus();
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ctlSubCode.ControlValue.ToString())) errorMessage += ctlSubCode.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlSubCodeSystem.ControlValue.ToString())) errorMessage += ctlSubCodeSystem.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlSubCodeSystemStatus.ControlValue.ToString())) errorMessage += ctlSubCodeSystemStatus.ControlEmptyErrorMessage + "<br />";
            System.Globalization.CultureInfo cultureInfo = new System.Globalization.CultureInfo("hr-HR");
            if (ctlSubCodeSystemChangeDate.ControlValue.ToString() != "" && !ValidationHelper.IsValidDateTime(ctlSubCodeSystemChangeDate.ControlValue.ToString(), cultureInfo)) errorMessage += ctlSubCodeSystemChangeDate.ControlErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlRefSources.ControlValue.ToString())) errorMessage += ctlRefSources.ControlEmptyErrorMessage + "<br />";
            if (ctlRefSources.ControlBoundItems.Count == 0) errorMessage += "Reference sources can't be empty";
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }
        private void BindDDLCodeSystem()
        {
            ctlSubCodeSystem.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Code System");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlSubCodeSystem.SourceValueProperty = "ssi__cont_voc_PK";
            ctlSubCodeSystem.SourceTextExpression = "term_name_english";
            ctlSubCodeSystem.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLCodeSystemID()
        {
            ctlSubCodeSystemID.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Code System");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlSubCodeSystemID.SourceValueProperty = "ssi__cont_voc_PK";
            ctlSubCodeSystemID.SourceTextExpression = "term_id";
            ctlSubCodeSystemID.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLCodeSystemStatus()
        {
            ctlSubCodeSystemStatus.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Status");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlSubCodeSystemStatus.SourceValueProperty = "ssi__cont_voc_PK";
            ctlSubCodeSystemStatus.SourceTextExpression = "term_name_english";
            ctlSubCodeSystemStatus.FillControl<Ssi__cont_voc_PK>(items);

        }
        private void BindRefSources()
        {
            ctlRefSources.ControlBoundItems.Clear();
            List<Reference_source_PK> items = SSIRep.GetObjectsList<Reference_source_PK>("ReferenceSource", entityOC);

            foreach (Reference_source_PK rs in items)
            {
                string rstype = _ssiControlledVocabularyOperations.GetEntity(rs.rs_type_FK).term_name_english;
                string rsclass = _ssiControlledVocabularyOperations.GetEntity(rs.rs_class_FK).term_name_english;
                if (rstype != null && rsclass != null)
                    ctlRefSources.ControlBoundItems.Add(new ListItem(String.Format("{0} {1}", rstype, rsclass), rs.reference_source_PK.ToString()));
            }
        }

        public override void BindForm(object id, string arg)
        {
            if (popupFormMode == PopupFormMode.Edit)
            {
                ctlSubCode.ControlValue = entity.code == null ? String.Empty : entity.code;
                ctlSubCodeSystem.ControlValue = entity.code_system_FK == null ? String.Empty : entity.code_system_FK.ToString();
                ctlSubCodeSystemID.ControlValue = entity.code_system_id_FK == null ? String.Empty : entity.code_system_id_FK.ToString();
                ctlSubCodeSystemStatus.ControlValue = entity.code_system_status_FK == null ? String.Empty : entity.code_system_status_FK.ToString();
                ctlSubCodeSystemChangeDate.ControlValue = entity.code_system_changedate == null ? String.Empty : entity.code_system_changedate.ToString();
                ctlSubCodeComment.ControlValue = entity.comment == null ? String.Empty : entity.comment.ToString();
            }
        }

        private void BindAssignedObjects()
        {
            BindRefSources();
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
            btnRemoveRefSources.Enabled = false;
            btnEditRefSource.Enabled = false;
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