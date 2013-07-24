using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormSUBALS : DetailsForm
    {
        #region Declarations

        ISubstance_translations_PKOperations _substanceTranslation;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "SubstanceAlias";
   
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
            get { return (int)Session["SUBALS_id"]; }
            set { Session["SUBALS_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["SUBALS_entityOC"]; }
            set { Session["SUBALS_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["SUBALS_entityParentOC"]; }
            set { Session["SUBALS_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["SUBALS_popupFormMode"]; }
            set { Session["SUBALS_popupFormMode"] = value; }
        }
        private Substance_alias_PK entity
        {
            get { return (Substance_alias_PK)Session["SUBALS_entity"]; }
            set { Session["SUBALS_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Substance_alias_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Substance_alias_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.substance_alias_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.substance_alias_PK != null)
                    _id = (int)inEntity.substance_alias_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Substance_alias_PK();
                SaveForm(_id, null);
                entity.substance_alias_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects = inEntityOC.AssignedObjects;

                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
            }

            BindAliasTranslation();
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            //_substanceTranslation = new Substance_translations_PKDAL();
            _ssiControlledVocabularyOperations=new Ssi__cont_voc_PKDAL();

            ucPopupFormALSSTRN.OnCancelButtonClick += ALSTRNPopupForm_OnCancelClick;
            ucPopupFormALSSTRN.OnOkButtonClick += ALSTRNPopupForm_OnOkClick;
            base.OnInit(e);

        }

        public override object SaveForm(object id, string arg)
        {
            entity.aliasname = ctlAliasName.ControlValue.ToString();
            //entity.sourcecode = ctlSourceCode.ControlValue.ToString();
            

            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlAliasName.ControlValue = "";
            //ctlSourceCode.ControlValue = "";
            ctlAliasTranslation.ControlBoundItems.Clear();
        }

        public override void FillDataDefinitions(string arg)
        {
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            if (string.IsNullOrEmpty(ctlAliasName.ControlValue.ToString())) errorMessage += ctlAliasName.ControlEmptyErrorMessage + "<br />";
           // if (string.IsNullOrEmpty(ctlSourceCode.ControlValue.ToString())) errorMessage += ctlSourceCode.ControlEmptyErrorMessage;

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
                ctlAliasName.ControlValue = entity.aliasname;
                //ctlSourceCode.ControlValue = entity.sourcecode;
                //BindAliasTranslation();
            }
        }

        public void BindAliasTranslation()
        {
            ctlAliasTranslation.ControlBoundItems.Clear();
            List<Substance_alias_translation_PK> items = SSIRep.GetObjectsList<Substance_alias_translation_PK>("AliasTranslation", entityOC);
            foreach (var item in items)
            {
                string displayItem = "";
                if (!string.IsNullOrWhiteSpace(item.languagecode))
                    displayItem += item.languagecode;
                if (!string.IsNullOrWhiteSpace(item.term))
                    displayItem += " - " + item.term;
                ctlAliasTranslation.ControlBoundItems.Add(new ListItem(displayItem, item.substance_alias_translation_PK.ToString()));
            }
            //ctlAliasTranslation.SourceTextExpression = "languagecode";
            //ctlAliasTranslation.SourceValueProperty = "substance_alias_translation_PK";
            //ctlAliasTranslation.FillControl(items);
        }

        #endregion

        #region Alias Translation

        public void ctlAliasTranslationListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditAliasTranslation.Enabled = false;
            btnRemoveAliasTranslation.Enabled = false;
            int numSelected = 0;

            foreach (ListItem item in ctlAliasTranslation.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveAliasTranslation.Enabled = true;
                    break;
                }
            }

            if (numSelected == 1)
            {
                btnEditAliasTranslation.Enabled = true;
                btnRemoveAliasTranslation.Enabled = true;
            }
        }

        void ALSTRNPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindAliasTranslation();
            ListItemCollection lic = ctlAliasTranslation.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditAliasTranslation.Enabled = false;
            btnRemoveAliasTranslation.Enabled = false;
        }

        void ALSTRNPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlAliasTranslation.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditAliasTranslation.Enabled = false;
            btnRemoveAliasTranslation.Enabled = false;
        }

        public void btnAddAliasTranslationOnClick(object sender, EventArgs e)
        {
            ucPopupFormALSSTRN.ShowModalForm("", "Alias Translation", null, entityOC);
        }

        public void btnEditAliasTranslationOnClick(object sender, EventArgs e)
        {
            string idAliasTranslation = "";
            foreach (ListItem item in ctlAliasTranslation.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idAliasTranslation = item.Value.ToString();
                    break;
                }
            }
            Substance_alias_translation_PK aliasTranslation = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idAliasTranslation), "AliasTranslation", entityOC) as Substance_alias_translation_PK;
            ucPopupFormALSSTRN.ShowModalForm("", "Alias Translation", aliasTranslation, entityOC);
            btnEditAliasTranslation.Enabled = false;
        }

        public void btnRemoveAliasTranslationOnClick(object sender, EventArgs e)
        {
            string idAliasTranslation = "";

            foreach (ListItem item in ctlAliasTranslation.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idAliasTranslation = item.Value;
                    ctlAliasTranslation.ControlBoundItems.Remove(item);
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idAliasTranslation), "AliasTranslation", entityOC);
            BindAliasTranslation();

            btnEditAliasTranslation.Enabled = false;
            btnRemoveAliasTranslation.Enabled = false;
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