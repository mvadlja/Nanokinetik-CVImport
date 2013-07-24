using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormSCLF : DetailsForm
    {
        IReference_source_PKOperations _reference_source_PKOperations;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "SubstanceClassification";

        private List<Subtype_PK> stList;
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
            get { return (int)Session["SCLF_id"]; }
            set { Session["SCLF_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["SCLF_entityOC"]; }
            set { Session["SCLF_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["SCLF_entityParentOC"]; }
            set { Session["SCLF_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["SCLF_popupFormMode"]; }
            set { Session["SCLF_popupFormMode"] = value; }
        }
        private Subst_clf_PK entity
        {
            get { return (Subst_clf_PK)Session["SCLF_entity"]; }
            set { Session["SCLF_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Subst_clf_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Subst_clf_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.subst_clf_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.subst_clf_PK != null)
                    _id = (int)inEntity.subst_clf_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Subst_clf_PK();
                SaveForm(_id, null);
                entity.subst_clf_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects = inEntityOC.AssignedObjects;

                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
            }

            BindStList();
            BindRsList();
        }


        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            _reference_source_PKOperations = new Reference_source_PKDAL();
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();

            UcPopupFormRs.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormRs_OnOkButtonClick);
            UcPopupFormRs.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormRs_OnCancelButtonClick);

            UcPopupFormST.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormST_OnOkButtonClick);
            UcPopupFormST.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormST_OnCancelButtonClick);

            stList = new List<Subtype_PK>();
            refSourceList = new List<Reference_source_PK>();

            base.OnInit(e);

            if (!IsPostBack)
                FillDataDefinitions(null);
        }

        public override object SaveForm(object id, string arg)
        {
            entity.domain = ctlSclfDomain.ControlValue.ToString();
            entity.sclf_code = ctlSclfCode.ControlValue.ToString();
            entity.sclf_type = ctlSclfType.ControlValue.ToString();
            entity.substance_classification = ctlSclf.ControlValue.ToString();

            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlSclfDomain.ControlValue = "";
            ctlSclf.ControlValue = "";
            ctlSclfCode.ControlValue = "";
            ctlSclfType.ControlValue = "";
            ctlRs.ControlBoundItems.Clear();
            ctlSt.ControlBoundItems.Clear();

        }

        public override void FillDataDefinitions(string arg)
        {
            BindDomain();
            BindSubstanceClass();
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ctlSclfDomain.ControlValue.ToString())) errorMessage += ctlSclfDomain.ControlEmptyErrorMessage + "</br>";
            if (String.IsNullOrEmpty(ctlSclf.ControlValue.ToString())) errorMessage += ctlSclf.ControlEmptyErrorMessage+"</br>";
            if (String.IsNullOrEmpty(ctlSclfType.ControlValue.ToString())) errorMessage += ctlSclfType.ControlEmptyErrorMessage + "</br>";
            if (ctlSt.ControlBoundItems.Count == 0) errorMessage += "Subtype can't be empty" + "</br>";
            if (ctlRs.ControlBoundItems.Count == 0) errorMessage += "Reference can't be empty" + "</br>";

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
                ctlSclfDomain.ControlValue = entity.domain;
                ctlSclf.ControlValue = entity.substance_classification;
                ctlSclfCode.ControlValue = entity.sclf_code;
                ctlSclfType.ControlValue = entity.sclf_type;
            }
        }
        #endregion

        #region BindData
        public void BindDomain()
        {
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Domain");
            ctlSclfDomain.SourceValueProperty = "ssi__cont_voc_PK";
            ctlSclfDomain.SourceTextExpression = "term_name_english";
            ctlSclfDomain.FillControl<Ssi__cont_voc_PK>(items);
        }

        public void BindSubstanceClass()
        {
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Substance Class");
            ctlSclf.SourceValueProperty = "ssi__cont_voc_PK";
            ctlSclf.SourceTextExpression = "term_name_english";
            ctlSclf.FillControl<Ssi__cont_voc_PK>(items);
        }

        public void BindSubstanceType()
        {
            if (ctlSclf.ControlValue.ToString() != "" && (ctlSclf.ControlValue.ToString() == "1" || ctlSclf.ControlValue.ToString() == "2"))//1-Mixture 2-Single
            {
                List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Substance Type");
                ctlSclfType.SourceValueProperty = "ssi__cont_voc_PK";
                ctlSclfType.SourceTextExpression = "term_name_english";
                ctlSclfType.FillControl<Ssi__cont_voc_PK>(items);
            }
            else if (ctlSclf.ControlValue.ToString() != "" && ctlSclf.ControlValue.ToString() == "3")//3-Specified Substance
            {
                List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Specified Substance Type");
                ctlSclfType.SourceValueProperty = "ssi__cont_voc_PK";
                ctlSclfType.SourceTextExpression = "term_name_english";
                ctlSclfType.FillControl<Ssi__cont_voc_PK>(items);
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

        #region Event Handlers
        public void ctlSclfInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            BindSubstanceType();
        }
#endregion

        #region PopupForms

            #region Subtype
        public void ctlStInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditSt.Enabled = false;
            btnRemoveSt.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlSt.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveSt.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditSt.Enabled = true;
                btnRemoveSt.Enabled = true;
            }
        }
        public void btnAddStOnClick(object sender, EventArgs e)
        {
            UcPopupFormST.ShowModalForm("", "Subtype", null,entityOC);
        }

        public void btnEditStOnClick(object sender, EventArgs e)
        {
            string idSt = "";
            foreach (ListItem item in ctlSt.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSt = item.Value.ToString();
                    break;
                }
            }
            if (idSt == "")
                return;
            Subtype_PK subtype = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idSt), "Subtype", entityOC) as Subtype_PK;

            UcPopupFormST.ShowModalForm("", "Subtype", subtype, entityOC);
            BindStList();
        }

        public void btnRemoveStOnClick(object sender, EventArgs e)
        {
            string idSt="";

            foreach (ListItem item in ctlSt.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSt = item.Value;
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idSt), "Subtype", entityOC);
            BindStList();
            btnEditSt.Enabled = false;
            btnRemoveSt.Enabled = false;
        }

        private void BindStList()
        {
            ctlSt.ControlBoundItems.Clear();
            List<Subtype_PK> items = SSIRep.GetObjectsList<Subtype_PK>("Subtype", entityOC);
            ctlSt.SourceTextExpression = "substance_class_subtype";
            ctlSt.SourceValueProperty = "subtype_PK";
            ctlSt.FillControl<Subtype_PK>(items);
        }

        void UcPopupFormST_OnOkButtonClick(object sender, FormDetailsEventArgs e)
        {
            BindStList();
            ListItemCollection lic = ctlSt.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSt.Enabled = false;
            btnRemoveSt.Enabled = false;
        }

        void UcPopupFormST_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlSt.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSt.Enabled = false;
            btnRemoveSt.Enabled = false;
        }

        #endregion

            #region Reference source
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
            if (idRs == "")
                return;
            Reference_source_PK refSource = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idRs), "ReferenceSource", entityOC) as Reference_source_PK;
            UcPopupFormRs.ShowModalForm("", "Reference source", refSource, entityOC);
        }

        public void btnRemoveRsOnClick(object sender, EventArgs e)
        {
            string idRs="";

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
            btnEditRs.Enabled = false;
            btnRemoveRs.Enabled = false;
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