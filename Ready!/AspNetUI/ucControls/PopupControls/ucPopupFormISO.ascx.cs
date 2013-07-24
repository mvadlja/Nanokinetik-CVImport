using System;
using System.Collections.Generic;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormISO : DetailsForm
    {
        #region Declarations

        ISubstance_PKOperations _substanceOperations;
        IIsotope_PKOperations _isotopeOperations;
        ISubtype_PKOperations _substitutionTypeOperations;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "Isotope";
   
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
            get { return (int)Session["ISO_id"]; }
            set { Session["ISO_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["ISO_entityOC"]; }
            set { Session["ISO_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["ISO_entityParentOC"]; }
            set { Session["ISO_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["ISO_popupFormMode"]; }
            set { Session["ISO_popupFormMode"] = value; }
        }
        private Isotope_PK entity
        {
            get { return (Isotope_PK)Session["ISO_entity"]; }
            set { Session["ISO_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Isotope_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Isotope_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.isotope_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.isotope_PK != null)
                    _id = (int)inEntity.isotope_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Isotope_PK();
                SaveForm(_id, null);
                entity.isotope_PK = _id;

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
            
            _substanceOperations = new Substance_PKDAL();
            _isotopeOperations = new Isotope_PKDAL();
            _substitutionTypeOperations = new Subtype_PKDAL();
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();

            EVCODESearcher.OnListItemSelected += new EventHandler<FormListEventArgs>(EVCODESearcher_OnListItemSelected);
            EVCODESearcherDisplay.OnSearchClick += new EventHandler<EventArgs>(EVCODESearcher_OnSearchClick);
            EVCODESearcherDisplay.OnRemoveClick += new EventHandler<EventArgs>(EVCODESearcherDisplay_OnRemoveClick);

            base.OnInit(e);

        }

        public override object SaveForm(object id, string arg)
        {
            Substance_PK substanceID = EVCODESearcherDisplay.SelectedObject != null ? _substanceOperations.GetEntity((int)EVCODESearcherDisplay.SelectedObject) : null;
           
            entity.nuclide_id = substanceID != null ? substanceID.ev_code != null ? substanceID.ev_code : null : null;
            entity.nuclide_name = ctlNuclideName.ControlValue.ToString();
            entity.substitution_type = ctlSubstitutionType.ControlValue.ToString();
            
            return entity;
        }

        public override void ClearForm(string arg)
        {
            EVCODESearcherDisplay.ClearSelectedObject();
            ctlNuclideName.ControlValue = String.Empty;
            ctlSubstitutionType.ControlValue = String.Empty;
        }

        public override void FillDataDefinitions(string arg)
        {
            BindDDLSubstitutionType();
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            if (String.IsNullOrEmpty(ctlNuclideName.ControlValue.ToString())) errorMessage += ctlNuclideName.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlSubstitutionType.ControlValue.ToString())) errorMessage += ctlSubstitutionType.ControlEmptyErrorMessage + "<br />";

            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }

        private void BindDDLSubstitutionType()
        {
             ctlSubstitutionType.ControlBoundItems.Clear();
             List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Substitution type");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.list_name.CompareTo(s2.list_name);
            });

            ctlSubstitutionType.SourceValueProperty = "ssi__cont_voc_PK";
            ctlSubstitutionType.SourceTextExpression = "term_name_english";
            ctlSubstitutionType.FillControl<Ssi__cont_voc_PK>(items);
        }
     

        public override void BindForm(object id, string arg)
        {
            if (id != null && id.ToString() != "" && popupFormMode==PopupFormMode.Edit) // Edit
            {
                EVCODESearcherDisplay.SetSelectedObject(entity.isotope_PK, entity.nuclide_id);
                ctlNuclideName.ControlValue = entity.nuclide_name;
                ctlSubstitutionType.ControlValue = entity.substitution_type;
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

        #region Searchers


        // EVCODE
        void EVCODESearcher_OnListItemSelected(object sender, FormListEventArgs e)
        {
            Substance_PK substance = _substanceOperations.GetEntity(e.DataItemID);

            if (substance != null && substance.ev_code != null)
                EVCODESearcherDisplay.SetSelectedObject(substance.substance_PK, substance.ev_code);
        }

        void EVCODESearcher_OnSearchClick(object sender, EventArgs e)
        {
            EVCODESearcher.ShowModalSearcher("SubName");
        }

        void EVCODESearcherDisplay_OnRemoveClick(object sender, EventArgs e)
        {
            EVCODESearcherDisplay.EnableSearcher(true);
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