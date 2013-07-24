using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormPROP : DetailsForm
    {
        #region Declarations

        ISubstance_code_PKOperations _substance_code_PKOperations;
        ISsi__cont_voc_PKOperations _ssi_cont_voc_PKOperations;
        ISubstance_name_PKOperations _substance_nameOperations;
        ISubstance_PKOperations _substanceOperations;
        IAmount_PKOperations _amountOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "Property";

        private List<Amount_PK> amountList;

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
            get { return (int)Session["PROP_id"]; }
            set { Session["PROP_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["PROP_entityOC"]; }
            set { Session["PROP_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["PROP_entityParentOC"]; }
            set { Session["PROP_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["PROP_popupFormMode"]; }
            set { Session["PROP_popupFormMode"] = value; }
        }
        private Property_PK entity
        {
            get { return (Property_PK)Session["PROP_entity"]; }
            set { Session["PROP_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Property_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Property_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.property_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.property_PK != null)
                    _id = (int)inEntity.property_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Property_PK();
                SaveForm(_id, null);
                entity.property_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects = inEntityOC.AssignedObjects;
                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
            }
            BindAmount();
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            _substance_code_PKOperations = new Substance_code_PKDAL();
            _ssi_cont_voc_PKOperations = new Ssi__cont_voc_PKDAL();
            _substance_nameOperations = new Substance_name_PKDAL();
            _substanceOperations = new Substance_PKDAL();
            _amountOperations = new Amount_PKDAL();

            AmountPopupForm.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(AmountPopupForm_OnOkClick);
            AmountPopupForm.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(AmountPopupForm_OnCancelClick);
            amountList = new List<Amount_PK>();

            //EVCODESearcher.OnListItemSelected += new EventHandler<FormListEventArgs>(EVCODESearcher_OnListItemSelected);
            //EVCODESearcherDisplay.OnSearchClick += new EventHandler<EventArgs>(EVCODESearcher_OnSearchClick);
            //EVCODESearcherDisplay.OnRemoveClick += new EventHandler<EventArgs>(EVCODESearcherDisplay_OnRemoveClick);

            SUBSearcher.OnListItemSelected += new EventHandler<FormListEventArgs>(SUBSearcher_OnListItemSelected);
            SUBSearcherDisplay.OnSearchClick += new EventHandler<EventArgs>(SUBSearcher_OnSearchClick);
            SUBSearcherDisplay.OnRemoveClick += new EventHandler<EventArgs>(SUBSearcherDisplay_OnRemoveClick);

            base.OnInit(e);

        }

        public override object SaveForm(object id, string arg)
        {

            entity.property_name = ctlPropertyName.ControlValue.ToString();
            entity.property_type = ctlPropertyType.ControlValue.ToString();
            entity.amount_type = ctlAmountType.ControlValue.ToString();

            entity.substance_id = EVCODESearcherDisplay.Text;
            entity.substance_name = SUBSearcherDisplay.SelectedObject != null ? SUBSearcherDisplay.SelectedObject.ToString() : String.Empty;

            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlPropertyName.ControlValue = String.Empty;
            ctlPropertyType.ControlValue = String.Empty;
            ctlAmountType.ControlValue = String.Empty;
            EVCODESearcherDisplay.Text = String.Empty;
            SUBSearcherDisplay.ClearSelectedObject();
            ctlAmount.ControlBoundItems.Clear();
        }

        public override void FillDataDefinitions(string arg)
        {
            BindDDLPropertyName();
            BindDDLPropertyType();
            BindDDLAmountType();
            BindAmount();
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ctlPropertyName.ControlValue.ToString())) errorMessage += ctlPropertyName.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlPropertyType.ControlValue.ToString())) errorMessage += ctlPropertyType.ControlEmptyErrorMessage + "<br />";
            if (ctlAmount.ControlBoundItems.Count < 1) errorMessage += "Amount can't be empty" + "<br />";
            if (ctlAmount.ControlBoundItems.Count > 1) errorMessage += "Amount can have only one value assigned" + "<br />";

            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }

        private void BindDDLPropertyName()
        {
            ctlPropertyName.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_PKOperations.GetEntitiesByListName("Parameter");

            items.Sort(delegate(Ssi__cont_voc_PK c1, Ssi__cont_voc_PK c2)
            {
                return c1.term_id.ToString().CompareTo(c2.term_id.ToString());
            });

            ctlPropertyName.SourceValueProperty = "ssi__cont_voc_PK";
            ctlPropertyName.SourceTextExpression = "term_name_english";
            ctlPropertyName.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLPropertyType()
        {
            ctlPropertyType.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_PKOperations.GetEntitiesByListName("Property Type");

            items.Sort(delegate(Ssi__cont_voc_PK c1, Ssi__cont_voc_PK c2)
            {
                return c1.term_name_english.ToString().CompareTo(c2.term_name_english.ToString());
            });

            ctlPropertyType.SourceValueProperty = "ssi__cont_voc_PK";
            ctlPropertyType.SourceTextExpression = "term_name_english";
            ctlPropertyType.FillControl<Ssi__cont_voc_PK>(items);
        }
        private void BindDDLAmountType()
        {
            ctlAmountType.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_PKOperations.GetEntitiesByListName("Amount Type");

            items.Sort(delegate(Ssi__cont_voc_PK c1, Ssi__cont_voc_PK c2)
            {
                return c1.term_name_english.ToString().CompareTo(c2.term_name_english.ToString());
            });

            ctlAmountType.SourceValueProperty = "ssi__cont_voc_PK";
            ctlAmountType.SourceTextExpression = "term_name_english";
            ctlAmountType.FillControl<Ssi__cont_voc_PK>(items);
        }

        public override void BindForm(object id, string arg)
        {
            //if (id != null && id.ToString() != "")
            if (popupFormMode == PopupFormMode.Edit)
            {
                ctlPropertyName.ControlValue = entity.property_name;// == null ? String.Empty : entity.property_name;
                ctlPropertyType.ControlValue = entity.property_type;// == null ? String.Empty : entity.property_type.ToString();
                ctlAmountType.ControlValue = entity.amount_type;// == null ? String.Empty : entity.amount_type.ToString();
                EVCODESearcherDisplay.Text = entity.substance_id;
                SUBSearcherDisplay.SetSelectedObject(null, entity.substance_name);
                BindAmount();
            }
        }

        #endregion


        #region Form methods

        public void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null) || (ValidateForm("")))
            {
                SaveForm(_id, "");
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

        #region PopupForms

        #region Amount

        public void btnAddAmountOnClick(object sender, EventArgs e)
        {
            if (ctlAmount.ControlBoundItems.Count == 0) AmountPopupForm.ShowModalForm(_id.ToString(), "Amount", null, entityOC);
        }

        public void ctlAmountListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditAmount.Enabled = false;
            btnRemoveAmount.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlAmount.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveAmount.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditAmount.Enabled = true;
                btnRemoveAmount.Enabled = true;
            }
        }

        void AmountPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindAmount();
            ListItemCollection lic = ctlAmount.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditAmount.Enabled = false;
            btnRemoveAmount.Enabled = false;
        }

        void AmountPopupForm_OnCancelClick (object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlAmount.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditAmount.Enabled = false;
            btnRemoveAmount.Enabled = false;
        }

        protected void btnEditAmountOnClick(object sender, EventArgs e)
        {
            string idAmount = "";
            foreach (ListItem item in ctlAmount.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idAmount = item.Value.ToString();
                    break;
                }
            }

            Amount_PK amount = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idAmount), "Amount", entityOC) as Amount_PK;
            AmountPopupForm.ShowModalForm(_id.ToString(), "Amount", amount, entityOC);
        }

        public void btnRemoveAmountOnClick(object sender, EventArgs e)
        {
            string idAmount = "";
            foreach (ListItem item in ctlAmount.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idAmount = item.Value.ToString();
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idAmount), "Amount", entityOC);
            BindAmount();
            btnEditAmount.Enabled = false;
            btnRemoveAmount.Enabled = false;
        }

        private void BindAmount()
        {
            ctlAmount.ControlBoundItems.Clear();
            List<Amount_PK> items = SSIRep.GetObjectsList<Amount_PK>("Amount", entityOC);
            foreach (Amount_PK item in items)
            {
                if (item.quantity != null || item.quantity != "")
                {
                    string quantity = _ssi_cont_voc_PKOperations.GetEntity(Convert.ToInt32(item.quantity)).term_name_english.ToString() + ": ";//_amountOperations.GetEntity(item.)
                    string rest = item.lownumvalue + " " + item.lownumprefix + item.lownumunit + " per " + item.lowdenomvalue + " " + item.lowdenomprefix + item.lowdenomunit;
                    
                    if (quantity == "Range: ")
                    {
                        quantity += rest + " - ";
                        rest = item.highnumvalue + " " + item.highnumprefix + item.highnumunit + " per " + item.highdenomvalue + " " + item.highdenomprefix + item.highdenomunit;
                    }
                    
                    if (quantity != null && rest != null)
                        ctlAmount.ControlBoundItems.Add(new ListItem(String.Format("{0} {1}", quantity, rest), item.amount_PK.ToString()));
                }
                else ctlAmount.ControlBoundItems.Add(new ListItem(String.Format("{0}", item.nonnumericvalue), item.amount_PK.ToString()));
            }
            
        }

        #endregion

        #endregion

        #region Searchers

        //#region EVCODE
        //void EVCODESearcher_OnListItemSelected(object sender, FormListEventArgs e)
        //{
        //    Substance_PK substance = _substanceOperations.GetEntity(e.DataItemID);

        //    if (substance != null && substance.ev_code != null)
        //        EVCODESearcherDisplay.SetSelectedObject(substance.substance_PK, substance.ev_code);

        //}

        //void EVCODESearcher_OnSearchClick(object sender, EventArgs e)
        //{
        //    EVCODESearcher.ShowModalSearcher("SubName");
        //}

        //void EVCODESearcherDisplay_OnRemoveClick(object sender, EventArgs e)
        //{
        //    EVCODESearcherDisplay.EnableSearcher(true);
        //}
        //#endregion

        #region SUBSTANCE_NAME
        void SUBSearcher_OnListItemSelected(object sender, FormListEventArgs e)
        {
            Substance_PK substance = _substanceOperations.GetEntity(e.DataItemID);

            if (substance != null && substance.substance_name != null)
            {
                SUBSearcherDisplay.SetSelectedObject(substance.substance_PK, substance.substance_name);
                EVCODESearcherDisplay.Text = substance.ev_code;
            }
        }

        void SUBSearcher_OnSearchClick(object sender, EventArgs e)
        {
            SUBSearcher.ShowModalSearcher("SubName");
        }

        void SUBSearcherDisplay_OnRemoveClick(object sender, EventArgs e)
        {
            SUBSearcherDisplay.EnableSearcher(true);
            EVCODESearcherDisplay.Text = "";
        }
        #endregion

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