using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormGN : DetailsForm
    {

        IReference_source_PKOperations _reference_source_PKOperations;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "Gene";

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
            get { return (int)Session["GN_id"]; }
            set { Session["GN_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["GN_entityOC"]; }
            set { Session["GN_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["GN_entityParentOC"]; }
            set { Session["GN_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["GN_popupFormMode"]; }
            set { Session["GN_popupFormMode"] = value; }
        }
        private Gene_PK entity
        {
            get { return (Gene_PK)Session["GN_entity"]; }
            set { Session["GN_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Gene_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Entity_Container.Style["display"] = "inline";

            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Gene_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.gene_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.gene_PK != null)
                    _id = (int)inEntity.gene_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Gene_PK();
                SaveForm(_id, null);
                entity.gene_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects = inEntityOC.AssignedObjects;

                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
            }

            entityOC = SSIRep.GetObjectContainer(entity, entityType, entityParentOC);
            
            BindForm(_id, null);
        }

        #endregion

        public void ddlGeneValueChanged(object sender, ValueChangedEventArgs e)
        {
            switch ((sender as DropDownList_CT).ID)
            {
                case "ddlGeneId":
                    ddlGeneName.ControlValue = (sender as DropDownList_CT).ControlValue;
                    break;
                case "ddlGeneName":
                    ddlGeneId.ControlValue = (sender as DropDownList_CT).ControlValue;
                    break;
            }
        }

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Entity_Container.Style["display"] = "none";

            _reference_source_PKOperations = new Reference_source_PKDAL();
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();

            UcPopupFormRs.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(RefSourcesPopupForm_OnOkClick);
            UcPopupFormRs.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(RefSourcesPopupForm_OnCancelClick);

            base.OnInit(e);
        }

        public override object SaveForm(object id, string arg)
        {
            entity.gene_id = ddlGeneId.ControlValue.ToString();
            entity.gene_name = ddlGeneName.ControlValue.ToString();
            entity.gene_sequence_origin = ddlGeneSequenceOrigin.ControlValue.ToString();
            return entity;
        }

        public override void ClearForm(string arg)
        {
            ddlGeneSequenceOrigin.ControlValue = "";
            ddlGeneId.ControlValue = "";
            ddlGeneName.ControlValue = "";
            asterix.ForeColor = System.Drawing.Color.Red;
        }

        public override void FillDataDefinitions(string arg)
        {
            FillGeneSequenceOrigin();
            FillGeneName();
            FillGeneId();
        }

        private void FillGeneName()
        {
           ddlGeneName.ControlBoundItems.Clear();
           List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Gene Name");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.list_name.CompareTo(s2.list_name);
            });

            ddlGeneName.SourceValueProperty = "ssi__cont_voc_PK";
            ddlGeneName.SourceTextExpression = "term_name_english";
            ddlGeneName.FillControl<Ssi__cont_voc_PK>(items);
        }

        private void FillGeneId()
        {
            ddlGeneId.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Gene Name");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.list_name.CompareTo(s2.list_name);
            });

            ddlGeneId.SourceValueProperty = "ssi__cont_voc_PK";
            ddlGeneId.SourceTextExpression = "term_id";
            ddlGeneId.FillControl<Ssi__cont_voc_PK>(items);
        }

        private void FillGeneSequenceOrigin()
        {
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Gene name");
            ddlGeneSequenceOrigin.SourceValueProperty = "ssi__cont_voc_PK";
            ddlGeneSequenceOrigin.SourceTextExpression = "term_name_english";
            ddlGeneSequenceOrigin.FillControl<Ssi__cont_voc_PK>(items);
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ddlGeneName.ControlValue.ToString())) errorMessage += ddlGeneName.ControlEmptyErrorMessage;

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
                ddlGeneSequenceOrigin.ControlValue = entity.gene_sequence_origin;
                ddlGeneId.ControlValue = entity.gene_id;
                ddlGeneName.ControlValue = entity.gene_name;
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
            UcPopupFormRs.ShowModalForm(_id.ToString(), "Reference source", null, entityOC);
        }

        void RefSourcesPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
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

        void RefSourcesPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlRs.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditRs.Enabled = false;
            btnRemoveRs.Enabled = false;
        }
        

        public void btnEditRsOnClick(object sender, EventArgs e)
        {
            string idRS = "0";
            foreach (ListItem item in ctlRs.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idRS = item.Value.ToString();
                    break;
                }
            }
            Reference_source_PK rs = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idRS), "ReferenceSource", entityOC) as Reference_source_PK;

            UcPopupFormRs.ShowModalForm(_id.ToString(), "Reference source", rs, entityOC);
        }

        public void btnRemoveRsOnClick(object sender, EventArgs e)
        {
            string idRS = "0";

            foreach (ListItem item in ctlRs.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idRS = item.Value;
                    break;
                }
            }

            SSIRep.DeleteObjectByID(Int32.Parse(idRS), "ReferenceSource", entityOC);

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
            asterix.ForeColor = ctlRs.ControlBoundItems.Count > 0 ? System.Drawing.Color.Green : System.Drawing.Color.Red;
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