using System;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormINTCOD : DetailsForm
    {
        #region Declarations

        ISubstance_translations_PKOperations _substanceTranslation;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "InternationalCode";
   
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
            get { return (int)Session["INTCOD_id"]; }
            set { Session["INTCOD_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["INTCOD_entityOC"]; }
            set { Session["INTCOD_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["INTCOD_entityParentOC"]; }
            set { Session["INTCOD_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["INTCOD_popupFormMode"]; }
            set { Session["INTCOD_popupFormMode"] = value; }
        }
        private International_code_PK entity
        {
            get { return (International_code_PK)Session["INTCOD_entity"]; }
            set { Session["INTCOD_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, International_code_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new International_code_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.international_code_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.international_code_PK != null)
                    _id = (int)inEntity.international_code_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new International_code_PK();
                SaveForm(_id, null);
                entity.international_code_PK = _id;

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
            //_substanceTranslation = new Substance_translations_PKDAL();
            _ssiControlledVocabularyOperations=new Ssi__cont_voc_PKDAL();

            base.OnInit(e);

        }

        public override object SaveForm(object id, string arg)
        {
            entity.referencetext = ctlReferenceText.ControlValue.ToString();
            //entity.sourcecode = ctlSourceCode.ControlValue.ToString();

            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlReferenceText.ControlValue = "";
            //ctlSourceCode.ControlValue = "";
        }

        public override void FillDataDefinitions(string arg)
        {
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            if (string.IsNullOrEmpty(ctlReferenceText.ControlValue.ToString())) errorMessage += ctlReferenceText.ControlEmptyErrorMessage + "<br />";
            //if (string.IsNullOrEmpty(ctlSourceCode.ControlValue.ToString())) errorMessage += ctlSourceCode.ControlEmptyErrorMessage;

            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                var masterPage = (AspNetUI.Views.Shared.Template.Default)Page.Master;
                if (masterPage != null)
                {
                    masterPage.ModalPopup.ShowModalPopup("Error!", errorMessage);
                }
                return false;
            }
            else return true;
        }
        
        public override void BindForm(object id, string arg)
        {
            if (id != null && id.ToString() != "" && popupFormMode==PopupFormMode.Edit) // Edit
            {
                ctlReferenceText.ControlValue = entity.referencetext;
                //ctlSourceCode.ControlValue = entity.sourcecode;
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