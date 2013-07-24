using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;
using System.Configuration;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucPopupFormSTRUCT : DetailsForm
    {
        #region Declarations
        string s_apid = "";

        ISubstance_PKOperations _substanceOperations;
        IIsotope_PKOperations _isotopeOperations;
        ISubtype_PKOperations _substitutionTypeOperations;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;
        IStruct_repres_attach_PKOperations _structAttachmentOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnOkButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;
        private enum PopupFormMode { New, Edit };
        private const string entityType = "Structure";


        #endregion

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return PopupControls_Struct_Container.Style["Width"]; }
            set { PopupControls_Struct_Container.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return PopupControls_Struct_Container.Style["Height"]; }
            set { PopupControls_Struct_Container.Style["Height"] = value; }
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
            get { return (int)Session["STRUCT_id"]; }
            set { Session["STRUCT_id"] = value; }
        }
        private ObjectContainer entityOC
        {
            get { return (ObjectContainer)Session["STRUCT_entityOC"]; }
            set { Session["STRUCT_entityOC"] = value; }
        }
        private ObjectContainer entityParentOC
        {
            get { return (ObjectContainer)Session["STRUCT_entityParentOC"]; }
            set { Session["STRUCT_entityParentOC"] = value; }
        }
        private PopupFormMode popupFormMode
        {
            get { return (PopupFormMode)Session["STRUCT_popupFormMode"]; }
            set { Session["STRUCT_popupFormMode"] = value; }
        }
        private Structure_PK entity
        {
            get { return (Structure_PK)Session["STRUCT_entity"]; }
            set { Session["STRUCT_entity"] = value; }
        }
        #endregion

        #region Operations

        public void ShowModalForm(string id, string header, Structure_PK inEntity, ObjectContainer inParentOC)
        {
            PopupControls_Struct_Container.Style["display"] = "inline";
            entityParentOC = inParentOC;
            divHeader.InnerHtml = header;

            if (inEntity == null)
            {
                entity = new Structure_PK();
                popupFormMode = PopupFormMode.New;
                _id = SSIRep.ObjectHighestID(entityType);
                entity.structure_PK = _id;
                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.SetState(ActionType.New, StatusType.Temp);
            }
            else
            {
                popupFormMode = PopupFormMode.Edit;
                entity = inEntity;
                if (inEntity.structure_PK != null)
                    _id = (int)inEntity.structure_PK;

                ObjectContainer inEntityOC = SSIRep.GetObjectContainer(inEntity, entityType, entityParentOC);
                inEntityOC.SetState(ActionType.Delete, StatusType.Temp);

                BindForm(_id, null);
                entity = new Structure_PK();
                SaveForm(_id, null);
                entity.structure_PK = _id;

                entityOC = SSIRep.AddObject(_id, entity, entityType, entityParentOC);
                entityOC.EditedObjectContainer = inEntityOC;
                entityOC.AssignedObjects = inEntityOC.AssignedObjects;

                if (inEntityOC.ActionOld == ActionType.New)
                    entityOC.SetState(ActionType.New, StatusType.Temp);
                else
                    entityOC.SetState(ActionType.Edit, StatusType.Temp);
            }

            BindIso();
        }

        #endregion

        #region FormOverrides

        protected override void OnInit(EventArgs e)
        {
            PopupControls_Struct_Container.Style["display"] = "none";

            _substanceOperations = new Substance_PKDAL();
            _isotopeOperations = new Isotope_PKDAL();
            _substitutionTypeOperations = new Subtype_PKDAL();
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();
            _structAttachmentOperations = new Struct_repres_attach_PKDAL();

            PopupFormISO.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(ISOPopupForm_OnOkClick);
            PopupFormISO.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(ISOPopupForm_OnCancelClick);

           

            base.OnInit(e);

        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";

            //TODO: potrebno je registrirati na full post back save iz context menia
            //ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(btnSubmit);

            //if (Session["UploadedAttachs"] == null) _attachementOperations.DeleteNULLByUserID(SessionManager.Instance.CurrentUser.UserID);

            //BindGridAttachments();
        }

        public override object SaveForm(object id, string arg)
        {
            entity.molecular_formula = ctlMolecularFormula.ControlValue.ToString();
            entity.optical_activity = ctlOpticalActivity.ControlValue.ToString();
            entity.stereochemistry_FK = Int32.Parse(ctlStereochemistry.ControlValue.ToString());
            entity.struct_representation = ctlStructRepresentation.ControlValue.ToString();
            entity.struct_repres_type_FK = Int32.Parse(ctlStructRepresentationType.ControlValue.ToString());
            if (lblUploadedFile.Text != "")
            {
                entity.struct_repres_attach_FK = Int32.Parse(lblUploadedFile.Text);
            }

            return entity;
        }

        public override void ClearForm(string arg)
        {
            ctlMolecularFormula.ControlValue = "";
            ctlOpticalActivity.ControlValue = "";
            ctlStereochemistry.ControlValue = "";
            ctlStructRepresentation.ControlValue = "";
            ctlStructRepresentationType.ControlValue = "";
            ctlISO.ControlBoundItems.Clear();
        }

        public override void FillDataDefinitions(string arg)
        {
            BindDDLSturcutRepresType();
            BindDDLStereochemistry();
        }

        public override void BindForm(object id, string arg)
        {
            if (id != null && id.ToString() != "" && popupFormMode == PopupFormMode.Edit) // Edit
            {
                ctlMolecularFormula.ControlValue = entity.molecular_formula;
                ctlOpticalActivity.ControlValue = entity.optical_activity;
                ctlStereochemistry.ControlValue = entity.stereochemistry_FK;
                ctlStructRepresentation.ControlValue = entity.struct_representation;
                ctlStructRepresentationType.ControlValue = entity.struct_repres_type_FK;
            }
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;


            if (String.IsNullOrEmpty(ctlStructRepresentationType.ControlValue.ToString())) errorMessage += ctlStructRepresentationType.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlStructRepresentation.ControlValue.ToString())) errorMessage += ctlStructRepresentation.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlStereochemistry.ControlValue.ToString())) errorMessage += ctlStereochemistry.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlOpticalActivity.ControlValue.ToString())) errorMessage += ctlOpticalActivity.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlMolecularFormula.ControlValue.ToString())) errorMessage += ctlMolecularFormula.ControlEmptyErrorMessage + "<br />";

            //if (String.IsNullOrEmpty(ctlname.ControlValue.ToString())) errorMessage += ctlname.ControlEmptyErrorMessage + "<br />";

            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }

        #endregion

        #region Form methods

        public void btnOk_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                SaveForm(_id, null);
                SSIRep.SaveState(entityOC, entityType, entityParentOC);

                PopupControls_Struct_Container.Style["display"] = "none";
                OnOkButtonClick(sender, new FormDetailsEventArgs(entity));
                ClearForm(null);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopupControls_Struct_Container.Style["display"] = "none";
            ClearForm(null);

            SSIRep.DiscardObjectAndRestore(entityOC, entityType, entityParentOC);
            OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
        }

        protected void btnClosePopupForm_Click(object sender, EventArgs e)
        {
            PopupControls_Struct_Container.Style["display"] = "none";
            ClearForm(null);

            SSIRep.DiscardObjectAndRestore(entityOC, entityType, entityParentOC);
            OnCancelButtonClick(sender, new FormDetailsEventArgs(null));
        }

        #endregion

        #region Binds

        private void BindDDLSturcutRepresType()
        {
            ctlStructRepresentationType.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Structural Representation Type");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.list_name.CompareTo(s2.list_name);
            });

            ctlStructRepresentationType.SourceValueProperty = "ssi__cont_voc_PK";
            ctlStructRepresentationType.SourceTextExpression = "term_name_english";
            ctlStructRepresentationType.FillControl<Ssi__cont_voc_PK>(items);
        }

        private void BindDDLStereochemistry()
        {
            ctlStereochemistry.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Stereochemistry");


            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.list_name.CompareTo(s2.list_name);
            });

            ctlStereochemistry.SourceValueProperty = "ssi__cont_voc_PK";
            ctlStereochemistry.SourceTextExpression = "term_name_english";
            ctlStereochemistry.FillControl<Ssi__cont_voc_PK>(items);
        }
        #endregion

        #region PopupForms

        #region ISO
        public void btnAddISOOnClick(object sender, EventArgs e)
        {
            PopupFormISO.ShowModalForm("", "Isotope", null, entityOC);
        }

        public void ctlISOListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditISO.Enabled = false;
            btnRemoveISO.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlISO.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveISO.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditISO.Enabled = true;
                btnRemoveISO.Enabled = true;
            }
        }

        void ISOPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindIso();
            ListItemCollection lic = ctlISO.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditISO.Enabled = false;
            btnRemoveISO.Enabled = false;
        }

        void ISOPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlISO.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditISO.Enabled = false;
            btnRemoveISO.Enabled = false;
        }

        protected void btnEditISO_Click(object sender, EventArgs e)
        {
            string idIso = "";
            foreach (ListItem item in ctlISO.ControlBoundItems)
            {
                if (item.Selected == true)
                    idIso = item.Value.ToString();
            }

            Isotope_PK iso = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idIso), "Isotope", entityOC) as Isotope_PK;
            PopupFormISO.ShowModalForm(s_apid, "Isotope", iso, entityOC);
        }


        public void btnRemoveISOOnClick(object sender, EventArgs e)
        {
            string idIso = "";
            foreach (ListItem item in ctlISO.ControlBoundItems)
            {
                if (item.Selected == true)
                    idIso = item.Value.ToString();
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idIso), "Isotope", entityOC);
            BindIso();
            btnEditISO.Enabled = false;
            btnRemoveISO.Enabled = false;
        }

        private void BindIso()
        {
            ctlISO.ControlBoundItems.Clear();
            List<Isotope_PK> items = SSIRep.GetObjectsList<Isotope_PK>("Isotope", entityOC);
            ctlISO.SourceTextExpression = "nuclide_name";
            ctlISO.SourceValueProperty = "isotope_PK";
            ctlISO.FillControl<Isotope_PK>(items);
        }

        #endregion
#endregion

        #region Upload files

        public void AsyncFileUpload1_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            System.Threading.Thread.Sleep(2000);

            if (FileUpload2.HasFile)
            {
                Struct_repres_attach_PK attachement = new Struct_repres_attach_PK();

                attachement.attachmentname = FileUpload2.FileName;
                attachement.filetype = FileUpload2.ContentType;
                attachement.disk_file = FileUpload2.FileBytes;

                Guid guid = Guid.NewGuid();
                attachement.Id = guid;

                attachement.userID = SessionManager.Instance.CurrentUser.UserID;

                attachement = _structAttachmentOperations.Save(attachement);

                Session["UploadedAttachs"] = (int)attachement.struct_repres_attach_PK;

                BindGridAttachment(attachement.struct_repres_attach_PK);

            }

        }

        // Binds attachment grid
        public void BindGridAttachment(int? struct_repres_attach_PK)
        {
            Struct_repres_attach_PK entity = _structAttachmentOperations.GetEntity(struct_repres_attach_PK);

            if (Session["UploadedAttachs"] != null)
            {
                int key = (int)Session["UploadedAttachs"];
                if (key > 0)
                {
                    entity = _structAttachmentOperations.GetEntity(key);
                }
            }

            List<Struct_repres_attach_PK> entities = new List<Struct_repres_attach_PK>();
            entities.Add(entity);

            gvData.DataSource = entities;
            gvData.DataBind();
            if (entities.Count > 0)
            {
                //pnlDataList.Visible = true;  
            }

            lblUploadedFile.Text = entity.struct_repres_attach_PK.ToString();

        }

        public void gvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        public void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Struct_repres_attach_PK attach = (Struct_repres_attach_PK)e.Row.DataItem;

                Button btnDownload = (Button)e.Row.FindControl("btnDownload");

                string fullPath = ConfigurationManager.AppSettings["AttachTmpDir"] + attach.attachmentname;
                string imeFile = fullPath.Substring(fullPath.LastIndexOf("\\") + 1);

                if (btnDownload.CommandArgument != null && btnDownload.CommandArgument != "")
                {
                    btnDownload.Attributes.Add("onclick", "SaveTheDownloadBtnAttach('" + btnDownload.ClientID + "," + btnDownload.CommandArgument + "');");
                }
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