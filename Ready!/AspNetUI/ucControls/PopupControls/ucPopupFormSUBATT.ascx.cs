using System;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormSUBATT : DetailsForm
    {
        #region Declarations

        ISubstance_translations_PKOperations _substanceTranslation;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "SubstanceAttachment";

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
            get { return (int)Session["SUBATT_id"]; }
            set { Session["SUBATT_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["SUBATT_entityOC"]; }
            set { Session["SUBATT_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["SUBATT_entityParentOC"]; }
            set { Session["SUBATT_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["SUBATT_popupFormMode"]; }
            set { Session["SUBATT_popupFormMode"] = value; }
        }
        private Substance_attachment_PK entity
        {
            get { return (Substance_attachment_PK)Session["SUBATT_entity"]; }
            set { Session["SUBATT_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Substance_attachment_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Substance_attachment_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.substance_attachment_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.substance_attachment_PK != null)
                    _id = (int)inEntity.substance_attachment_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Substance_attachment_PK();
                SaveForm(_id, null);
                entity.substance_attachment_PK = _id;

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
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();

            base.OnInit(e);

        }

        public override object SaveForm(object id, string arg)
        {
            entity.validitydeclaration = Int32.Parse(ctlValidityDeclaration.ControlValue.ToString());
            entity.resolutionmode = Int32.Parse(ctlResolutionMode.ControlValue.ToString());
            entity.attachmentreference = ctlAttachmentCode.ControlValue.ToString();

            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlAttachmentCode.ControlValue = "";
            ctlResolutionMode.ControlValue = "";
            ctlValidityDeclaration.ControlValue = "";
        }

        public override void FillDataDefinitions(string arg)
        {

        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (string.IsNullOrEmpty(ctlValidityDeclaration.ControlValue.ToString())) errorMessage += ctlValidityDeclaration.ControlEmptyErrorMessage + "<br />";
            else if (!ValidationHelper.IsValidInt(ctlValidityDeclaration.ControlValue.ToString())) errorMessage += ctlValidityDeclaration.ControlErrorMessage + "<br />";

            if (string.IsNullOrEmpty(ctlAttachmentCode.ControlValue.ToString()))errorMessage += ctlAttachmentCode.ControlEmptyErrorMessage + "<br />";
            if (!ValidationHelper.IsValidInt(ctlResolutionMode.ControlValue.ToString()))errorMessage += ctlResolutionMode.ControlErrorMessage + "<br />";

            // If errors were found, showing them in modal popup
            if (!string.IsNullOrEmpty(errorMessage))
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
            if (id != null && id.ToString() != "" && popupFormMode == PopupFormMode.Edit) // Edit
            {
                ctlAttachmentCode.ControlValue = entity.attachmentreference;
                ctlResolutionMode.ControlValue = entity.resolutionmode;
                ctlValidityDeclaration.ControlValue = entity.validitydeclaration;

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