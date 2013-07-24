using System;
using System.Collections.Generic;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormST : DetailsForm
    {
        IReference_source_PKOperations _reference_source_PKOperations;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "Subtype";

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
            get { return (int)Session["ST_id"]; }
            set { Session["ST_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["ST_entityOC"]; }
            set { Session["ST_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["ST_entityParentOC"]; }
            set { Session["ST_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["ST_popupFormMode"]; }
            set { Session["ST_popupFormMode"] = value; }
        }
        private Subtype_PK entity
        {
            get { return (Subtype_PK)Session["ST_entity"]; }
            set { Session["ST_entity"] = value; }
        }
        #endregion

        #region Operations
        public void ShowModalForm(string id, string header, Subtype_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Subtype_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.subtype_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.subtype_PK != null)
                    _id = (int)inEntity.subtype_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Subtype_PK();
                SaveForm(_id, null);
                entity.subtype_PK = _id;

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
            FillDataDefinitions(null);
        }

        public override object SaveForm(object id, string arg)
        {
            entity.substance_class_subtype = tbxSubstanceClassSubtype.ControlValue.ToString();
            return entity;
        }

        public override void ClearForm(string arg)
        {
            tbxSubstanceClassSubtype.ControlValue = "";
        }

        public override void FillDataDefinitions(string arg)
        {
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Substance subtype");
            tbxSubstanceClassSubtype.SourceTextExpression = "term_name_english";
            tbxSubstanceClassSubtype.SourceValueProperty = "term_name_english";
            tbxSubstanceClassSubtype.FillControl<Ssi__cont_voc_PK>(items);
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            if (String.IsNullOrEmpty(tbxSubstanceClassSubtype.ControlValue.ToString()))
                errorMessage += tbxSubstanceClassSubtype.ControlEmptyErrorMessage + "</br>";

            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }


        public override void BindForm(object id, string arg)
        {
            if (popupFormMode == PopupFormMode.Edit)
            {
                tbxSubstanceClassSubtype.ControlValue = entity.substance_class_subtype;
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