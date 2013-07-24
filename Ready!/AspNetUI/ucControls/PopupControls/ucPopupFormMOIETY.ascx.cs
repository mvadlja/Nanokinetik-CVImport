using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormMOIETY : DetailsForm
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
        private const string entityType = "Moiety";

        private string s_apid = "";
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
            get { return (int)Session["MOIETY_id"]; }
            set { Session["MOIETY_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["MOIETY_entityOC"]; }
            set { Session["MOIETY_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["MOIETY_entityParentOC"]; }
            set { Session["MOIETY_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["MOIETY_popupFormMode"]; }
            set { Session["MOIETY_popupFormMode"] = value; }
        }
        private Moiety_PK entity
        {
            get { return (Moiety_PK)Session["MOIETY_entity"]; }
            set { Session["MOIETY_entity"] = value; }
        }

        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Moiety_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Moiety_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.moiety_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.moiety_PK != null)
                    _id = (int)inEntity.moiety_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Moiety_PK();
                SaveForm(_id, null);
                entity.moiety_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects = inEntityOC.AssignedObjects;
                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
            }
            BindAmount();
            //BindAmount();
        }

        #endregion
        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";
            s_apid = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";
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
            entity.moiety_id = EVCODESearcherDisplay.Text;
            entity.moiety_name = SUBSearcherDisplay.SelectedObject != null ? SUBSearcherDisplay.SelectedObject.ToString() : String.Empty;
            entity.moiety_role = ctlMoietyRole.ControlValue != null ? ctlMoietyRole.ControlValue.ToString() : "";
            entity.amount_type = ctlAmountType.ControlValue != null ? ctlAmountType.ControlValue.ToString() : "";

            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlMoietyRole.ControlValue = String.Empty;
            ctlAmountType.ControlValue = String.Empty;
            ctlAmount.ControlBoundItems.Clear();
            SUBSearcherDisplay.ClearSelectedObject();
            EVCODESearcherDisplay.Text = String.Empty;
            MoietyNameAsterix.ForeColor = System.Drawing.Color.Red;
        }

        public override void FillDataDefinitions(string arg)
        {
            BindDDLMoietyRole();
            BindDDLAmountType();
            BindAmount();
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            
            if (String.IsNullOrEmpty(ctlMoietyRole.ControlValue.ToString())) errorMessage += ctlMoietyRole.ControlEmptyErrorMessage + "<br />";
            if  ((SUBSearcherDisplay == null) || String.IsNullOrEmpty(SUBSearcherDisplay.SelectedObject.ToString())) errorMessage += "Moiety role can't be empty" + "<br />";
            if (ctlAmount.ControlBoundItems.Count > 1) errorMessage += "Amount can have only one assigned value" + "<br />";

            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }
        private void BindDDLMoietyRole()
        {
            ctlMoietyRole.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssi_cont_voc_PKOperations.GetEntitiesByListName("Role");

            items.Sort(delegate(Ssi__cont_voc_PK c1, Ssi__cont_voc_PK c2)
            {
                return c1.term_name_english.ToString().CompareTo(c2.term_name_english.ToString());
            });

            ctlMoietyRole.SourceValueProperty = "ssi__cont_voc_PK";
            ctlMoietyRole.SourceTextExpression = "term_name_english";
            ctlMoietyRole.FillControl<Ssi__cont_voc_PK>(items);
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
            if (popupFormMode == PopupFormMode.Edit)
            {
                ctlMoietyRole.ControlValue = entity.moiety_role;// == null ? String.Empty : entity.moiety_role.ToString();
                ctlAmountType.ControlValue = entity.amount_type;// == null ? String.Empty : entity.amount_type.ToString();
                EVCODESearcherDisplay.Text = entity.moiety_id;
                SUBSearcherDisplay.SetSelectedObject(null, entity.moiety_name);
                BindAmount();
                if (SUBSearcherDisplay.SelectedObject != null) MoietyNameAsterix.ForeColor = System.Drawing.Color.Green;
            }
        }

        #endregion

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

        #region Form methods

        public void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null) || (ValidateForm("")))
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
            if (ctlAmount.ControlBoundItems.Count > 0)
                btnAddAmount.Enabled = false;
            else
                btnAddAmount.Enabled = true;
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

        void AmountPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
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
                MoietyNameAsterix.ForeColor = substance.substance_PK != null ? System.Drawing.Color.Green : System.Drawing.Color.Red;
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
            MoietyNameAsterix.ForeColor = System.Drawing.Color.Red;
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