using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormREL : DetailsForm
    {
        IReference_source_PKOperations _reference_source_PKOperations;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;
        ISubstance_PKOperations _substanceOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "SubstanceRelationship";
        private List<Amount_PK> amtList;
        private List<Reference_source_PK> refSourceList;

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
            get { return (int)Session["REL_id"]; }
            set { Session["REL_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["REL_entityOC"]; }
            set { Session["REL_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["REL_entityParentOC"]; }
            set { Session["REL_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["REL_popupFormMode"]; }
            set { Session["REL_popupFormMode"] = value; }
        }
        private Substance_relationship_PK entity
        {
            get { return (Substance_relationship_PK)Session["REL_entity"]; }
            set { Session["REL_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Substance_relationship_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Substance_relationship_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.substance_relationship_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.substance_relationship_PK != null)
                    _id = (int)inEntity.substance_relationship_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Substance_relationship_PK();
                SaveForm(_id, null);
                entity.substance_relationship_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects = inEntityOC.AssignedObjects;

                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
            }
            BindAmtList();
            BindRsList();
            // BindForm(_id, null);
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            _reference_source_PKOperations = new Reference_source_PKDAL();
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();
            _substanceOperations = new Substance_PKDAL();

            EVCODESearcherRel.OnListItemSelected += new EventHandler<FormListEventArgs>(EVCODESearcher_OnListItemSelected);
            EVCODESearcherDisplayRel.OnSearchClick += new EventHandler<EventArgs>(EVCODESearcher_OnSearchClick);
            EVCODESearcherDisplayRel.OnRemoveClick += new EventHandler<EventArgs>(EVCODESearcherDisplay_OnRemoveClick);

            UcPopupFormRs.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormRs_OnOkButtonClick);
            UcPopupFormRs.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormRs_OnCancelButtonClick);
            UcPopupFormAmt.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormAmt_OnOkButtonClick);
            UcPopupFormAmt.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormAmt_OnCancelButtonClick);


            amtList = new List<Amount_PK>();
            refSourceList = new List<Reference_source_PK>();

            base.OnInit(e);

            if (!IsPostBack)
                FillDataDefinitions(null);

        }

        public override object SaveForm(object id, string arg)
        {
            entity.relationship = ddlRelationship.ControlValue.ToString();
            entity.amount_type = ddlAmountType.ControlValue.ToString();
            entity.interaction_type = ddlInteractionType.ControlValue.ToString();
            entity.substance_id = EVCODESearcherDisplayRel.SelectedObject != null ? EVCODESearcherDisplayRel.SelectedObject.ToString(): String.Empty;
            entity.substance_name = tbxSubstanceName.ControlValue.ToString();
            return entity;
        }

        public override void ClearForm(string arg)
        {
            ddlRelationship.ControlValue = "";
            ddlAmountType.ControlValue = "";
            ddlInteractionType.ControlValue = "";
            tbxSubstanceName.ControlValue = "";

            ctlAmt.ControlBoundItems.Clear();
            ctlRs.ControlBoundItems.Clear();
           
            EVCODESearcherDisplayRel.EnableSearcher(true);
        }

        public override void FillDataDefinitions(string arg)
        {
            BindRelationship();
            BindInteractionType();
            BindSubstanceName();
            BindAmountType();
        }

        #region BindData
        private void BindRelationship()
        {
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("relationship type");
            ddlRelationship.SourceValueProperty = "ssi__cont_voc_PK";
            ddlRelationship.SourceTextExpression = "term_name_english";
            ddlRelationship.FillControl<Ssi__cont_voc_PK>(items);
        }

        private void BindInteractionType()
        {
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Interaction Type");
            ddlInteractionType.SourceValueProperty = "ssi__cont_voc_PK";
            ddlInteractionType.SourceTextExpression = "term_name_english";
            ddlInteractionType.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindSubstanceName()
        {
            //List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Substance Name");
            //ddlSubstanceName.SourceValueProperty = "ssi__cont_voc_PK";
            //ddlSubstanceName.SourceTextExpression = "term_name_english";
            //ddlSubstanceName.FillControl<Ssi__cont_voc_PK>(items);
        }

        private void BindAmountType()
        {
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Amount Type");
            ddlAmountType.SourceValueProperty = "ssi__cont_voc_PK";
            ddlAmountType.SourceTextExpression = "term_name_english";
            ddlAmountType.FillControl<Ssi__cont_voc_PK>(items);
        }
        #endregion

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ddlRelationship.ControlValue.ToString())) errorMessage += ddlRelationship.ControlEmptyErrorMessage + "</br>";
            if (String.IsNullOrEmpty(tbxSubstanceName.ControlValue.ToString())) errorMessage += tbxSubstanceName.ControlEmptyErrorMessage + "</br>";
            if (ctlRs.ControlBoundItems.Count == 0) errorMessage += "Reference source can't be empty";

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
                ddlRelationship.ControlValue = entity.relationship;
                ddlAmountType.ControlValue = entity.amount_type;
                ddlInteractionType.ControlValue = entity.interaction_type;
                tbxSubstanceName.ControlValue = entity.substance_name;
                EVCODESearcherDisplayRel.SetSelectedObject(entity.substance_id, entity.substance_id);
            }
        }

        #endregion

        private void BindAmt()
        {
            ctlAmt.ControlBoundItems.Clear();
            List<Amount_PK> items = SSIRep.GetObjectsList<Amount_PK>("Amount", entityOC);
            foreach (Amount_PK item in items)
            {
                if (item.quantity != null || item.quantity != "")
                {
                    string quantity = _ssiControlledVocabularyOperations.GetEntity(Convert.ToInt32(item.quantity)).term_name_english.ToString() + ": ";//_amountOperations.GetEntity(item.)
                    string rest = item.lownumvalue + " " + item.lownumprefix + item.lownumunit + " per " + item.lowdenomvalue + " " + item.lowdenomprefix + item.lowdenomunit;

                    if (quantity == "Range: ")
                    {
                        quantity += rest + " - ";
                        rest = item.highnumvalue + " " + item.highnumprefix + item.highnumunit + " per " + item.highdenomvalue + " " + item.highdenomprefix + item.highdenomunit;
                    }

                    if (quantity != null && rest != null)
                        ctlAmt.ControlBoundItems.Add(new ListItem(String.Format("{0} {1}", quantity, rest), item.amount_PK.ToString()));
                }
                else ctlAmt.ControlBoundItems.Add(new ListItem(String.Format("{0}", item.nonnumericvalue), item.amount_PK.ToString()));
            }

        }

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
        }

        protected void btnClosePopupForm_Click(object sender, EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            ClearForm(null);

            SSIRep.DiscardObjectAndRestore(entityOC, entityType, entityParentOC);
        }
#endregion

        #region PopupForms

        #region AmountPopUp
        public void ctlAmtInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditAmt.Enabled = false;
            btnRemoveAmt.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlAmt.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveAmt.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditAmt.Enabled = true;
                btnRemoveAmt.Enabled = true;
            }
            if (ctlAmt.ControlBoundItems.Count > 0)
                btnAddAmt.Enabled = false;
            else
                btnAddAmt.Enabled = true;
        }
        public void btnAddAmtOnClick(object sender, EventArgs e)
        {
            UcPopupFormAmt.ShowModalForm(_id.ToString(), "Amount", null,entityOC);
        }

        public void btnEditAmtOnClick(object sender, EventArgs e)
        {
            string idAmt = "";
            foreach (ListItem item in ctlAmt.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idAmt = item.Value.ToString();
                    break;
                }
            }
            Amount_PK amount = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idAmt), "Amount", entityOC) as Amount_PK;
            UcPopupFormAmt.ShowModalForm(_id.ToString(),"Amount", amount, entityOC);
            btnEditAmt.Enabled = false;
            BindAmtList();
        }

        public void btnRemoveAmtOnClick(object sender, EventArgs e)
        {
            string idAmt="";

            foreach (ListItem item in ctlAmt.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idAmt = item.Value;
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idAmt), "Amount", entityOC);
            BindAmtList();
            btnRemoveAmt.Enabled = false;
            btnEditAmt.Enabled = false;
        }

        private void BindAmtList()
        {
            ctlAmt.ControlBoundItems.Clear();
            List<Amount_PK> items = SSIRep.GetObjectsList<Amount_PK>("Amount", entityOC);
            foreach (Amount_PK item in items)
            {
                if (item.quantity != null || item.quantity != "")
                {
                    string quantity = _ssiControlledVocabularyOperations.GetEntity(Convert.ToInt32(item.quantity)).term_name_english.ToString() + ": ";//_amountOperations.GetEntity(item.)
                    string rest = item.lownumvalue + " " + item.lownumprefix + item.lownumunit + " per " + item.lowdenomvalue + " " + item.lowdenomprefix + item.lowdenomunit;

                    if (quantity == "Range: ")
                    {
                        quantity += rest + " - ";
                        rest = item.highnumvalue + " " + item.highnumprefix + item.highnumunit + " per " + item.highdenomvalue + " " + item.highdenomprefix + item.highdenomunit;
                    }

                    if (quantity != null && rest != null)
                        ctlAmt.ControlBoundItems.Add(new ListItem(String.Format("{0} {1}", quantity, rest), item.amount_PK.ToString()));
                }
                else ctlAmt.ControlBoundItems.Add(new ListItem(String.Format("{0}", item.nonnumericvalue), item.amount_PK.ToString()));

        }


            if (ctlAmt.ControlBoundItems.Count > 0)
                btnAddAmt.Enabled = false;
            else
                btnAddAmt.Enabled = true;
        }

        void UcPopupFormAmt_OnOkButtonClick(object sender, FormDetailsEventArgs e)
        {
            BindAmtList();
            ListItemCollection lic = ctlAmt.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditAmt.Enabled = false;
            btnRemoveAmt.Enabled = false;
        }

        void UcPopupFormAmt_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlAmt.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditAmt.Enabled = false;
            btnRemoveAmt.Enabled = false;
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

        #endregion

        #region Searchers
        // EVCODE
        void EVCODESearcher_OnListItemSelected(object sender, FormListEventArgs e)
        {
            Substance_PK substance = _substanceOperations.GetEntity(e.DataItemID);

            if (substance != null && substance.ev_code != null)
            {
                EVCODESearcherDisplayRel.SetSelectedObject(substance.ev_code, substance.ev_code);
                tbxSubstanceName.ControlValue = substance.substance_name;
            }

        }

        void EVCODESearcher_OnSearchClick(object sender, EventArgs e)
        {
            EVCODESearcherRel.ShowModalSearcher("SubName");
        }

        void EVCODESearcherDisplay_OnRemoveClick(object sender, EventArgs e)
        {
            EVCODESearcherDisplayRel.EnableSearcher(true);
            tbxSubstanceName.ControlValue = "";
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