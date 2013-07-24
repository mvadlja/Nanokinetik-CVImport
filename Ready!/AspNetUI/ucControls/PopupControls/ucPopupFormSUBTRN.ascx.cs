using System;
using System.Collections.Generic;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormSUBTRN : DetailsForm
    {
        #region Declarations

        ISubstance_translations_PKOperations _substanceTranslation;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "SubstanceTranslation";
   
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
            get { return (int)Session["SUBTRN_id"]; }
            set { Session["SUBTRN_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["SUBTRN_entityOC"]; }
            set { Session["SUBTRN_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["SUBTRN_entityParentOC"]; }
            set { Session["SUBTRN_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["SUBTRN_popupFormMode"]; }
            set { Session["SUBTRN_popupFormMode"] = value; }
        }
        private Substance_translations_PK entity
        {
            get { return (Substance_translations_PK)Session["SUBTRN_entity"]; }
            set { Session["SUBTRN_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Substance_translations_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Substance_translations_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.substance_translations_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.substance_translations_PK != null)
                    _id = (int)inEntity.substance_translations_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Substance_translations_PK();
                SaveForm(_id, null);
                entity.substance_translations_PK = _id;

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
            if (string.IsNullOrEmpty(ctlTerm.ControlValue.ToString())) errorMessage += ctlTerm.ControlEmptyErrorMessage;

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
            BindLanguage();

            if (id != null && id.ToString() != "" && popupFormMode==PopupFormMode.Edit) // Edit
            {
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