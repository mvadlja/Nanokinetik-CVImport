using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormGE : DetailsForm
    {

        IReference_source_PKOperations _reference_source_PKOperations;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;
        ISubstance_PKOperations _substanceOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "GeneElement";

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
            get { return (int)Session["GE_id"]; }
            set { Session["GE_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["GE_entityOC"]; }
            set { Session["GE_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["GE_entityParentOC"]; }
            set { Session["GE_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["GE_popupFormMode"]; }
            set { Session["GE_popupFormMode"] = value; }
        }
        private Gene_element_PK entity
        {
            get { return (Gene_element_PK)Session["GE_entity"]; }
            set { Session["GE_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Gene_element_PK inEntity, ObjectContainer inParentOC)
        {

            PopupControls_Entity_Container.Style["display"] = "inline";
            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Gene_element_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.gene_element_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.gene_element_PK != null)
                    _id = (int)inEntity.gene_element_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Gene_element_PK();
                SaveForm(_id, null);
                entity.gene_element_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects = inEntityOC.AssignedObjects;

                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
            }
            BindRsList();
            
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            _reference_source_PKOperations = new Reference_source_PKDAL();
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();
            _substanceOperations = new Substance_PKDAL();

            EVCODESearcher.OnListItemSelected += new EventHandler<FormListEventArgs>(EVCODESearcher_OnListItemSelected);
            EVCODESearcherDisplay.OnSearchClick += new EventHandler<EventArgs>(EVCODESearcher_OnSearchClick);
            EVCODESearcherDisplay.OnRemoveClick += new EventHandler<EventArgs>(EVCODESearcherDisplay_OnRemoveClick);

            UcPopupFormRs.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormRs_OnOkButtonClick);
            UcPopupFormRs.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormRs_OnCancelButtonClick);

            base.OnInit(e);
        }

        public override object SaveForm(object id, string arg)
        {
            entity.ge_name = tbxGeneElementName.ControlValue.ToString();
            entity.ge_type = ddlGeneElementType.ControlValue.ToString();
            if (EVCODESearcherDisplay.SelectedObject != null)
            {
                entity.ge_id = EVCODESearcherDisplay.SelectedObject.ToString();
            }
            return entity;
        }

        public override void ClearForm(string arg)
        {
            tbxGeneElementName.ControlValue = "";
            ddlGeneElementType.ControlValue = "";
            EVCODESearcherDisplay.SetSelectedObject(null, "");
            asterix.ForeColor = System.Drawing.Color.Red;
            ctlRs.ControlBoundItems.Clear();
        }

        public override void FillDataDefinitions(string arg)
        {
            FillGeneElementType();
        }

        private void FillGeneElementType()
        {
            ddlGeneElementType.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Gene Element Type");
            ddlGeneElementType.SourceValueProperty = "ssi__cont_voc_PK";
            ddlGeneElementType.SourceTextExpression = "term_name_english";
            ddlGeneElementType.FillControl<Ssi__cont_voc_PK>(items);
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ddlGeneElementType.ControlValue.ToString())) errorMessage += ddlGeneElementType.ControlEmptyErrorMessage + "</br>";
            if (String.IsNullOrEmpty(tbxGeneElementName.ControlValue.ToString())) errorMessage += tbxGeneElementName.ControlEmptyErrorMessage + "</br>";
            if (ctlRs.ControlBoundItems.Count == 0)
                errorMessage += "Reference source can't be empty";

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
                ddlGeneElementType.ControlValue = entity.ge_type;
                tbxGeneElementName.ControlValue = entity.ge_name;
                EVCODESearcherDisplay.SetSelectedObject(entity.ge_id, entity.ge_id);
                asterix.ForeColor = ctlRs.ControlBoundItems.Count > 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red;
                BindRsList();
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

        #region ReferenceSource
        public void ctlRsInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditRs.Enabled = false;
            btnRemoveRs.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlRs.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveRs.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditRs.Enabled = true;
                btnRemoveRs.Enabled = true;
            }
            asterix.ForeColor = ctlRs.ControlBoundItems.Count > 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red;
        }
        public void btnAddRsOnClick(object sender, EventArgs e)
        {
            UcPopupFormRs.ShowModalForm("", "Reference source", null, entityOC);
        }

        public void btnEditRsOnClick(object sender, EventArgs e)
        {
            string idRs = "";
            foreach (ListItem item in ctlRs.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idRs = item.Value.ToString();
                    break;
                }
            }
            Reference_source_PK refSource = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idRs), "ReferenceSource", entityOC) as Reference_source_PK;
            UcPopupFormRs.ShowModalForm("", "Reference source", refSource, entityOC);
            btnEditRs.Enabled = false;
        }

        public void btnRemoveRsOnClick(object sender, EventArgs e)
        {
            string idRs = "";

            foreach (ListItem item in ctlRs.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idRs = item.Value;
                    break;
                }
            }

            SSIRep.DeleteObjectByID(Int32.Parse(idRs), "ReferenceSource", entityOC);
            BindRsList();
            btnRemoveRs.Enabled = false;
            btnEditRs.Enabled = false;
        }
        
        private void BindRsList()
        {
            ctlRs.ControlBoundItems.Clear();
            List<Reference_source_PK> items = SSIRep.GetObjectsList<Reference_source_PK>("ReferenceSource", entityOC);

            foreach (Reference_source_PK rs in items)
            {
                string rstype = _ssiControlledVocabularyOperations.GetEntity(rs.rs_type_FK).term_name_english;
                string rsclass = _ssiControlledVocabularyOperations.GetEntity(rs.rs_class_FK).term_name_english;
                if (rstype != null && rsclass != null)
                    ctlRs.ControlBoundItems.Add(new ListItem(String.Format("{0} {1}", rstype, rsclass), rs.reference_source_PK.ToString()));
            }
            asterix.ForeColor = ctlRs.ControlBoundItems.Count > 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red;
        }

        void UcPopupFormRs_OnOkButtonClick(object sender, FormDetailsEventArgs e)
        {
            BindRsList();
            ListItemCollection lic = ctlRs.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditRs.Enabled = false;
            btnRemoveRs.Enabled = false;
        }

        void UcPopupFormRs_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlRs.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditRs.Enabled = false;
            btnRemoveRs.Enabled = false;
        }
        #endregion

        #region Searchers
        // EVCODE
        void EVCODESearcher_OnListItemSelected(object sender, FormListEventArgs e)
        {
            Substance_PK substance = _substanceOperations.GetEntity(e.DataItemID);

            if (substance != null && substance.ev_code != null)
            {
                EVCODESearcherDisplay.SetSelectedObject(substance.substance_PK, substance.ev_code);
                tbxGeneElementName.ControlValue = substance.substance_name;
            }

        }

        void EVCODESearcher_OnSearchClick(object sender, EventArgs e)
        {
            EVCODESearcher.ShowModalSearcher("SubName");
        }

        void EVCODESearcherDisplay_OnRemoveClick(object sender, EventArgs e)
        {
            EVCODESearcherDisplay.EnableSearcher(true);
            tbxGeneElementName.ControlValue = "";
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