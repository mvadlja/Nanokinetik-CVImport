using System;
using System.Collections.Generic;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormALSTRN : DetailsForm
    {
        #region Declarations

        ISubstance_translations_PKOperations _substanceTranslation;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "AliasTranslation";

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
            get { return (int)Session["ALSTRN_id"]; }
            set { Session["ALSTRN_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["ALSTRN_entityOC"]; }
            set { Session["ALSTRN_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["ALSTRN_entityParentOC"]; }
            set { Session["ALSTRN_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["ALSTRN_popupFormMode"]; }
            set { Session["ALSTRN_popupFormMode"] = value; }
        }
        private Substance_alias_translation_PK entity
        {
            get { return (Substance_alias_translation_PK)Session["ALSTRN_entity"]; }
            set { Session["ALSTRN_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Substance_alias_translation_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Substance_alias_translation_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.substance_alias_translation_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.substance_alias_translation_PK != null)
                    _id = (int)inEntity.substance_alias_translation_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Substance_alias_translation_PK();
                SaveForm(_id, null);
                entity.substance_alias_translation_PK = _id;

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

            BindLanguage();

            base.OnInit(e);

        }

        public override object SaveForm(object id, string arg)
        {
            entity.languagecode = ctlLanguage.ControlValue.ToString();
            entity.term = ctlTerm.ControlValue.ToString();

            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlLanguage.ControlValue = "";
            ctlTerm.ControlValue = "";
        }

        public override void FillDataDefinitions(string arg)
        {

        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            if (string.IsNullOrEmpty(ctlLanguage.ControlValue.ToString())) errorMessage += ctlLanguage.ControlEmptyErrorMessage + "<br />";
            if (string.IsNullOrEmpty(ctlTerm.ControlValue.ToString())) errorMessage += ctlTerm.ControlEmptyErrorMessage + "<br />";

            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                var masterPage = (Views.Shared.Template.Default)Page.Master;
                if (masterPage != null)
                {
                    masterPage.ModalPopup.ShowModalPopup("Error!", errorMessage);
                }
                return false;
            }
            else return true;
        }

        private void BindLanguage()
        {
            ctlLanguage.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Language");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlLanguage.SourceValueProperty = "term_name_english";
            ctlLanguage.SourceTextExpression = "term_name_english";
            ctlLanguage.FillControl<Ssi__cont_voc_PK>(items);
        }


        public override void BindForm(object id, string arg)
        {
            if (id != null && id.ToString() != "" && popupFormMode == PopupFormMode.Edit) // Edit
            {
                BindLanguage();

                ctlLanguage.ControlValue = entity.languagecode;
                ctlTerm.ControlValue = entity.term;
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