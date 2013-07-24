using System;
using System.Collections.Generic;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormRS : DetailsForm
    {
        IReference_source_PKOperations _reference_source_PKOperations;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "ReferenceSource";

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
            get { return (int)Session["RS_id"]; }
            set { Session["RS_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["RS_entityOC"]; }
            set { Session["RS_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["RS_entityParentOC"]; }
            set { Session["RS_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["RS_popupFormMode"]; }
            set { Session["RS_popupFormMode"] = value; }
        }
        private Reference_source_PK entity
        {
            get { return (Reference_source_PK)Session["RS_entity"]; }
            set { Session["RS_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Reference_source_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Reference_source_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.reference_source_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.reference_source_PK != null)
                    _id = (int)inEntity.reference_source_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);
                
                BindForm(_id, null);
                entity = new Reference_source_PK();
                SaveForm(_id, null);
                entity.reference_source_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects = inEntityOC.AssignedObjects;

                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
            }
            
           // BindForm(_id, null);
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            _reference_source_PKOperations = new Reference_source_PKDAL();
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();

            base.OnInit(e);
        }

        public override object SaveForm(object id, string arg)
        {
            if (RadioButtonYes.Checked) entity.public_domain = true;
            if (RadioButtonNo.Checked) entity.public_domain = false;
            entity.rs_type_FK = ValidationHelper.IsValidInt(ctlRefSourceType.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlRefSourceType.ControlValue) : null;
            entity.rs_class_FK = ValidationHelper.IsValidInt(ctlRefSourceClass.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlRefSourceClass.ControlValue) : null;
            entity.rs_id = ctlRefSourceId.ControlValue.ToString();
            entity.rs_citation = ctlRefSourceCitation.ControlValue.ToString();

            return entity;
        }

        public override void ClearForm(string arg)
        {
            RadioButtonYes.Checked = false;
            RadioButtonNo.Checked = false;
            asterix.ForeColor = System.Drawing.Color.Red;
            ctlRefSourceType.ControlValue = String.Empty;
            ctlRefSourceClass.ControlValue = String.Empty;
            ctlRefSourceId.ControlValue = String.Empty;
            ctlRefSourceCitation.ControlValue = String.Empty;

        }

        public override void FillDataDefinitions(string arg)
        {
            BindDDLRefSourceClass();
            BindDDLRefSourceType();
            BindCBPublicDomain();
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            if ((!RadioButtonNo.Checked) && (!RadioButtonYes.Checked)) errorMessage += "Public domain must be selected <br/>";
            if (String.IsNullOrEmpty(ctlRefSourceType.ControlValue.ToString())) errorMessage += ctlRefSourceType.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlRefSourceClass.ControlValue.ToString())) errorMessage += ctlRefSourceClass.ControlEmptyErrorMessage + "<br />";

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

        void BindCBPublicDomain()
        {
            RadioButtonNo.Checked = false;
            RadioButtonYes.Checked = false;
            asterix.ForeColor = System.Drawing.Color.Red;
            if ((entity != null) && (entity.public_domain != null))
            {
                if (entity.public_domain == true) RadioButtonYes.Checked = true;
                else RadioButtonNo.Checked = true;
                asterix.ForeColor = System.Drawing.Color.Green;
            }
        }

        private void BindDDLRefSourceClass()
        {
            ctlRefSourceClass.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Source Class");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlRefSourceClass.SourceValueProperty = "ssi__cont_voc_PK";
            ctlRefSourceClass.SourceTextExpression = "term_name_english";
            ctlRefSourceClass.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLRefSourceType()
        {
            ctlRefSourceType.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Source");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlRefSourceType.SourceValueProperty = "ssi__cont_voc_PK";
            ctlRefSourceType.SourceTextExpression = "term_name_english";
            ctlRefSourceType.FillControl<Ssi__cont_voc_PK>(items);
        }

        public override void BindForm(object id, string arg)
        {
            if (popupFormMode == PopupFormMode.Edit)
            {
                BindCBPublicDomain();
                ctlRefSourceType.ControlValue = entity.rs_type_FK == null ? String.Empty : entity.rs_type_FK.ToString();
                ctlRefSourceClass.ControlValue = entity.rs_class_FK == null ? String.Empty : entity.rs_class_FK.ToString();
                ctlRefSourceId.ControlValue = entity.rs_id == null ? String.Empty : entity.rs_id.ToString();
                ctlRefSourceCitation.ControlValue = entity.rs_citation == null ? String.Empty : entity.rs_citation.ToString();
            }
        }

        #endregion

        #region Form methods

        public void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                SaveForm(_id, null);
                SSIRep.SaveState(entityOC, entityType, entityParentOC);

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