using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormTRG : DetailsForm
    {
        IReference_source_PKOperations _reference_source_PKOperations;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "Target";
 
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
            get { return (int)Session["TRG_id"]; }
            set { Session["TRG_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["TRG_entityOC"]; }
            set { Session["TRG_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["TRG_entityParentOC"]; }
            set { Session["TRG_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["TRG_popupFormMode"]; }
            set { Session["TRG_popupFormMode"] = value; }
        }
        private Target_PK entity
        {
            get { return (Target_PK)Session["TRG_entity"]; }
            set { Session["TRG_entity"] = value; }
        }

        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Target_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Target_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.target_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.target_PK != null)
                    _id = (int)inEntity.target_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Target_PK();
                SaveForm(_id, null);
                entity.target_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects = inEntityOC.AssignedObjects;

                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
            }

            BindRefSources();
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            _reference_source_PKOperations = new Reference_source_PKDAL();
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();

            RefSourcesPopupForm.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(RefSourcesPopupForm_OnCancelClick);
            RefSourcesPopupForm.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(RefSourcesPopupForm_OnOkClick);

            base.OnInit(e);

            
        }

        public override object SaveForm(object id, string arg)
        {
            entity.interaction_type = ddlInteractionType.ControlValue.ToString();
            entity.target_gene_id = ddlTargetGeneId.ControlValue.ToString();
            entity.target_gene_name = ddlTargetGeneName.ControlValue.ToString();
            entity.target_organism_type = ddlTargetOrganismType.ControlValue.ToString();
            entity.target_type = ddlTargetType.ControlValue.ToString();
            return entity;
        }

        public override void ClearForm(string arg)
        {
            ddlInteractionType.ControlValue = "";
            ddlTargetGeneId.ControlValue = "";
            ddlTargetGeneName.ControlValue = "";
            ddlTargetOrganismType.ControlValue = "";
            ddlTargetType.ControlValue = "";
        }

        public override void FillDataDefinitions(string arg)
        {
            FillTargetGene();
            FillInteractionType();
            FillTargetOrganismType();
            FillTargetType();
        }

        private void FillTargetGene()
        {
            ddlTargetGeneId.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Target Gene");

            ddlTargetGeneId.SourceValueProperty = "ssi__cont_voc_PK";
            ddlTargetGeneId.SourceTextExpression = "term_id";
            ddlTargetGeneId.FillControl<Ssi__cont_voc_PK>(items);

            ddlTargetGeneName.SourceValueProperty = "ssi__cont_voc_PK";
            ddlTargetGeneName.SourceTextExpression = "term_name_english";
            ddlTargetGeneName.FillControl<Ssi__cont_voc_PK>(items);
        }

        private void FillInteractionType()
        {
            ddlInteractionType.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Target interaction type");

            ddlInteractionType.SourceValueProperty = "ssi__cont_voc_PK";
            ddlInteractionType.SourceTextExpression = "term_name_english";
            ddlInteractionType.FillControl<Ssi__cont_voc_PK>(items);
        }

        private void FillTargetOrganismType()
        {
            ddlTargetOrganismType.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Organism type");

            ddlTargetOrganismType.SourceValueProperty = "ssi__cont_voc_PK";
            ddlTargetOrganismType.SourceTextExpression = "term_name_english";
            ddlTargetOrganismType.FillControl<Ssi__cont_voc_PK>(items);
        }

        private void FillTargetType()
        {
            ddlTargetType.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Target type");

            ddlTargetType.SourceValueProperty = "ssi__cont_voc_PK";
            ddlTargetType.SourceTextExpression = "term_name_english";
            ddlTargetType.FillControl<Ssi__cont_voc_PK>(items);
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            if (String.IsNullOrEmpty(ddlTargetGeneName.ControlValue.ToString())) errorMessage += ddlTargetGeneName.ControlEmptyErrorMessage + "</br>";
            if (String.IsNullOrEmpty(ddlInteractionType.ControlValue.ToString())) errorMessage += ddlInteractionType.ControlEmptyErrorMessage;

            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }

        public void TargetNameValueChanged(object sender, ValueChangedEventArgs e)
        {
            switch ((sender as DropDownList_CT).ID)
            {
                case "ddlTargetGeneId":
                    ddlTargetGeneName.ControlValue = (sender as DropDownList_CT).ControlValue;
                    break;
                case "ddlTargetGeneName":
                    ddlTargetGeneId.ControlValue = (sender as DropDownList_CT).ControlValue;
                    break;
            }
        }

        public override void BindForm(object id, string arg)
        {
            if (popupFormMode == PopupFormMode.Edit)
            {
                ddlTargetGeneName.ControlValue = entity.target_gene_name;
                ddlInteractionType.ControlValue = entity.interaction_type;
                ddlTargetGeneId.ControlValue = entity.target_gene_id;
                ddlTargetOrganismType.ControlValue = entity.target_organism_type;
                ddlTargetType.ControlValue = entity.target_type;
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