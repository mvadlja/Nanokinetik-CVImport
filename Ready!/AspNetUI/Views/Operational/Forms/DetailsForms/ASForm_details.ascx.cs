using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;

namespace AspNetUI.Views
{
    public partial class ASForm_details : DetailsForm
    {

        #region Declarations

        private SSIRepository SSIRep
        {
            get { return (SSIRepository)Session["SSIRepository"]; }
            set { Session["SSIRepository"] = value; }
        }

        string s_apid = "";
        bool isFormBound = false;
        bool isClassBound = false;

        // Model data managers
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;
        ISubstance_PKOperations _substanceOperations;
        IApproved_substance_PKOperations _approvedSubstance;
        ILast_change_PKOperations _last_change_PKOperations;
        IUSEROperations _userOperations;

        public virtual event EventHandler<FormDetailsEventArgs> OnSaveButtonClick;
        public virtual event EventHandler<FormDetailsEventArgs> OnCancelButtonClick;

        #endregion

        #region Init handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Retreiving model data managers from central configuration
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();
            _substanceOperations = new Substance_PKDAL();
            _approvedSubstance = new Approved_substance_PKDAL();
            _last_change_PKOperations = new Last_change_PKDAL();
            _userOperations = new USERDAL();

            EVCODESearcher.OnListItemSelected += new EventHandler<FormListEventArgs>(EVCODESearcher_OnListItemSelected);
            EVCODESearcherDisplay.OnSearchClick += new EventHandler<EventArgs>(EVCODESearcher_OnSearchClick);
            EVCODESearcherDisplay.OnRemoveClick += new EventHandler<EventArgs>(EVCODESearcherDisplay_OnRemoveClick);

            ucPopupFormSUBTRN1.OnOkButtonClick += SUBTRNPopupForm_OnOkClick;
            ucPopupFormSUBTRN1.OnCancelButtonClick += SUBTRNPopupForm_OnCancelClick;

            UcPopupFormINTCOD.OnCancelButtonClick += INTCODPopupForm_OnCancelClick;
            UcPopupFormINTCOD.OnOkButtonClick += INTCODPopupForm_OnOkClick;

            UcPopupFormPREEVCODE.OnCancelButtonClick += PREEVCODEPopupForm_OnCancelClick;
            UcPopupFormPREEVCODE.OnOkButtonClick += PREEVCODEPopupForm_OnOkClick;

            UcPopupFormSUBALS.OnCancelButtonClick += SUBALSPopupForm_OnCancelClick;
            UcPopupFormSUBALS.OnOkButtonClick += SUBALSPopupForm_OnOkClick;

            UcPopupFormSUBATT.OnCancelButtonClick += SUBATTPopupForm_OnCancelClick;
            UcPopupFormSUBATT.OnOkButtonClick += SUBATTPopupForm_OnOkClick;


            if (!IsPostBack)
            {
                SSIRep = new SSIRepository();
            }

            DisableUnderline();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";

            //  if (Session["UploadedAttachs"] == null) _structAttachmentOperations.DeleteNULLByUserID(SessionManager.Instance.CurrentUser.UserID);
        }

        void DisableAssignAndRemove()
        {
            //btnAssignAdminRoute.Enabled = false;
            //btnRemoveAdminRoute.Enabled = false;
            //btnEditISO.Enabled = false;
            //btnRemoveISO.Enabled = false;
            //btnEditSubCode.Enabled = false;
            //btnRemoveSubCodes.Enabled = false;
        }

        void DisableUnderline()
        {
            System.Web.UI.WebControls.Style myStyle = new System.Web.UI.WebControls.Style();
            myStyle.Font.Underline = false;
            btnRemoveInternationalCodes.ApplyStyle(myStyle);
            btnEditInternationalCodes.ApplyStyle(myStyle);

            btnEditPreviousEvcode.ApplyStyle(myStyle);
            btnRemovePreviousEvcode.ApplyStyle(myStyle);

            btnEditSubAliases.ApplyStyle(myStyle);
            btnRemoveSubAliases.ApplyStyle(myStyle);

            btnEditSubAttachment.ApplyStyle(myStyle);
            btnRemoveSubAttachment.ApplyStyle(myStyle);

            btnEditSubTranslation.ApplyStyle(myStyle);
            btnRemoveSubTranslation.ApplyStyle(myStyle);
        }
        #endregion

        #region FormOverrides

        public override object SaveForm(object id, string arg)
        {
            Approved_substance_PK entity = null;

            entity = _approvedSubstance.GetEntity(id);

            if (entity == null)
            {
                entity = new Approved_substance_PK();
            }

            entity.casnumber = ctlCasNumber.ControlValue.ToString();
            entity.cbd = ctlCbd.ControlValue.ToString();
            entity.Class = !string.IsNullOrEmpty(ctlClass.ControlValue.ToString()) ? (int?)Int32.Parse(ctlClass.ControlValue.ToString()) : null;
            entity.comments = ctlComment.ControlValue.ToString();

            if (EVCODESearcherDisplay.SelectedObject != null)
                entity.ev_code = EVCODESearcherDisplay.SelectedObject.ToString();

            entity.molecularformula = ctlMolecularFormula.ControlValue.ToString();
            entity.substancename = ctlSubstanceName.ControlValue.ToString();

            entity = _approvedSubstance.Save(entity);

            if (!ListOperations.ListsEquals<int>(InitialFormHash, AuditTrailHelper.GetPanelHashValue(pnlDataDetails)))
            {
                AuditTrailHelper.UpdateLastChange(entity.approved_substance_PK, "APPROVED_SUBSTANCE", _last_change_PKOperations, _userOperations);
            }

            return entity;
        }

        // Clears form
        public override void ClearForm(string arg)
        {
            if (isFormBound)
                return;

            ctlCasNumber.ControlValue = "";
            ctlCbd.ControlValue = "";
            ctlClass.ControlValue = "";
            ctlComment.ControlValue = "";
            ctlInternationalCodes.ControlBoundItems.Clear();
            ctlMolecularFormula.ControlValue = "";
            ctlPreviousEvcode.ControlBoundItems.Clear();
            ctlSubAttachment.ControlBoundItems.Clear();
            ctlSubstanceAliases.ControlBoundItems.Clear();
            ctlSubstanceName.ControlValue = "";
            ctlSubstanceSsi.ControlBoundItems.Clear();
            ctlSubstanceTranslation.ControlBoundItems.Clear();
            DisableAssignAndRemove();
            EVCODESearcherDisplay.ClearSelectedObject();
        }

        // Fills all form controls with data
        public override void FillDataDefinitions(string arg)
        {
            BindClass();
        }

        // Binds form
        public override void BindForm(object id, string arg)
        {
            if (id != null)
            {
                Approved_substance_PK entity = _approvedSubstance.GetEntity(Int32.Parse(id.ToString()));

                ctlCasNumber.ControlValue = entity.casnumber;
                ctlCbd.ControlValue = entity.cbd;

                ctlComment.ControlValue = entity.comments;
                ctlMolecularFormula.ControlValue = entity.molecularformula;
                ctlSubstanceName.ControlValue = entity.substancename;

                EVCODESearcherDisplay.SetSelectedObject(entity.ev_code, entity.ev_code);

                SSIRep.Reset();
                SSIRep.LoadApprovedSubstanceFromDb(entity);

                BindInternationalCode();
                BindPreviousEVCODE();
                BindSubstanceAliases();
                BindSubstanceAttachment();
                BindSubstanceTranslation();

                BindClass();
                ctlClass.ControlValue = entity.Class;
                isClassBound = true;

                isFormBound = true;

                if (!IsInitialHashWritten)
                {
                    InitialFormHash = AuditTrailHelper.GetPanelHashValue(pnlDataDetails);
                    IsInitialHashWritten = true;
                }
            }

        }

        // Validates form
        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;
            //if (!ValidationHelper.IsValidInt(ctlVirtual.ControlValue.ToString()))
            //    errorMessage += ctlVirtual.ControlErrorMessage + "<br />";
            //if (string.IsNullOrEmpty(ctlSourceCode.ControlValue.ToString()))
            //    errorMessage += ctlSourceCode.ControlEmptyErrorMessage + "<br />";
            //if (string.IsNullOrEmpty(ctlResolutionMode.ControlValue.ToString()))
            //    errorMessage += ctlResolutionMode.ControlEmptyErrorMessage + "<br />";
            if (string.IsNullOrEmpty(ctlSubstanceName.ControlValue.ToString()))
                errorMessage += ctlSubstanceName.ControlEmptyErrorMessage + "<br />";


            // If errors were found, showing them in modal popup
            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }

        #endregion

        #region Upload files

        //protected void AsyncFileUpload1_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        //{
        //    System.Threading.Thread.Sleep(2000);

        //    if (FileUpload2.HasFile)
        //    {
        //        Struct_repres_attach_PK attachement = new Struct_repres_attach_PK();

        //        attachement.attachmentname = FileUpload2.FileName;
        //        attachement.filetype = FileUpload2.ContentType;

        //        attachement.disk_file = FileUpload2.FileBytes;

        //        Guid guid = Guid.NewGuid();
        //        attachement.Id = guid;

        //        attachement.userID = SessionManager.Instance.CurrentUser.UserID;

        //        SSIForm.

        //        //attachement = _structAttachmentOperations.Save(attachement);

        //        //Session["UploadedAttachs"] = (int)attachement.struct_repres_attach_PK;

        //        BindGridAttachment(attachement.struct_repres_attach_PK);
        //    }
        //}

        #endregion

        #region Data bindings
        public void BindClass()
        {
            if (isClassBound)
                return;

            ctlClass.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Substance Class");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.list_name.CompareTo(s2.list_name);
            });

            ctlClass.SourceValueProperty = "ssi__cont_voc_PK";
            ctlClass.SourceTextExpression = "term_name_english";
            ctlClass.FillControl<Ssi__cont_voc_PK>(items);
        }

        public void BindSubstanceTranslation()
        {
            ctlSubstanceTranslation.ControlBoundItems.Clear();
            List<Substance_translations_PK> items = SSIRep.GetObjectsList<Substance_translations_PK>("SubstanceTranslation", null);
            foreach (var item in items)
            {
                string displayItem = "";
                if (!string.IsNullOrWhiteSpace(item.languagecode))
                    displayItem += item.languagecode;
                if (!string.IsNullOrWhiteSpace(item.term))
                    displayItem += " - " + item.term;
                ctlSubstanceTranslation.ControlBoundItems.Add(new ListItem(displayItem, item.substance_translations_PK.ToString()));
            }
            //ctlSubstanceTranslation.SourceTextExpression = "languagecode";
            //ctlSubstanceTranslation.SourceValueProperty = "substance_translations_PK";
            //ctlSubstanceTranslation.FillControl(items);
        }

        public void BindSubstanceAliases()
        {
            ctlSubstanceAliases.ControlBoundItems.Clear();
            List<Substance_alias_PK> items = SSIRep.GetObjectsList<Substance_alias_PK>("SubstanceAlias", null);
            ctlSubstanceAliases.SourceTextExpression = "aliasname";
            ctlSubstanceAliases.SourceValueProperty = "substance_alias_PK";
            ctlSubstanceAliases.FillControl(items);
        }

        public void BindInternationalCode()
        {
            ctlInternationalCodes.ControlBoundItems.Clear();
            List<International_code_PK> items = SSIRep.GetObjectsList<International_code_PK>("InternationalCode", null);
            ctlInternationalCodes.SourceTextExpression = "referencetext";
            ctlInternationalCodes.SourceValueProperty = "international_code_PK";
            ctlInternationalCodes.FillControl(items);
        }

        public void BindPreviousEVCODE()
        {
            ctlPreviousEvcode.ControlBoundItems.Clear();
            List<As_previous_ev_code_PK> items = SSIRep.GetObjectsList<As_previous_ev_code_PK>("PreviousEvcode", null);
            ctlPreviousEvcode.SourceTextExpression = "devevcode";
            ctlPreviousEvcode.SourceValueProperty = "as_previous_ev_code_PK";
            ctlPreviousEvcode.FillControl(items);
        }

        public void BindSubstanceAttachment()
        {
            ctlSubAttachment.ControlBoundItems.Clear();
            List<Substance_attachment_PK> items = SSIRep.GetObjectsList<Substance_attachment_PK>("SubstanceAttachment", null);
            ctlSubAttachment.SourceTextExpression = "validitydeclaration";
            ctlSubAttachment.SourceValueProperty = "substance_attachment_PK";
            ctlSubAttachment.FillControl(items);
        }

        #endregion

        #region Searchers

        #region EVCODE
        void EVCODESearcher_OnListItemSelected(object sender, FormListEventArgs e)
        {
            Substance_PK substance = _substanceOperations.GetEntity(e.DataItemID);

            if (substance != null && substance.ev_code != null)
                EVCODESearcherDisplay.SetSelectedObject(substance.ev_code, substance.ev_code);

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

        #endregion

        #region Event Handlers

        public void btnSaveOnClick(object sender, EventArgs e)
        {
            if (OnSaveButtonClick != null)
                OnSaveButtonClick(null, new FormDetailsEventArgs(null));
        }

        public void btnCancelOnClick(object sender, EventArgs e)
        {
            if (OnCancelButtonClick != null)
                OnCancelButtonClick(null, new FormDetailsEventArgs(null));
        }

        #endregion

        #region PopupForms

        #region Substance Translation

        public void ctlSubstanceTranslationListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditSubTranslation.Enabled = false;
            btnRemoveSubTranslation.Enabled = false;
            int numSelected = 0;

            foreach (ListItem item in ctlSubstanceTranslation.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveSubTranslation.Enabled = true;
                    break;
                }
            }

            if (numSelected == 1)
            {
                btnEditSubTranslation.Enabled = true;
                btnRemoveSubTranslation.Enabled = true;
            }
        }

        void SUBTRNPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindSubstanceTranslation();
            ListItemCollection lic = ctlSubstanceTranslation.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSubTranslation.Enabled = false;
            btnRemoveSubTranslation.Enabled = false;
        }

        void SUBTRNPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlSubstanceTranslation.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSubTranslation.Enabled = false;
            btnRemoveSubTranslation.Enabled = false;
        }

        public void btnAddSubTranslationOnClick(object sender, EventArgs e)
        {
            ucPopupFormSUBTRN1.ShowModalForm("", "Substance Translation", null, null);
        }

        public void btnEditSubTranslationOnClick(object sender, EventArgs e)
        {
            string idSubTranslation = "";
            foreach (ListItem item in ctlSubstanceTranslation.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSubTranslation = item.Value.ToString();
                    break;
                }
            }
            Substance_translations_PK subTranslation = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idSubTranslation), "SubstanceTranslation", null) as Substance_translations_PK;
            ucPopupFormSUBTRN1.ShowModalForm(s_apid, "Substance Translation", subTranslation, null);
            btnEditSubTranslation.Enabled = false;
        }

        public void btnRemoveSubTranslationOnClick(object sender, EventArgs e)
        {
            string idSubTranslation = "";

            foreach (ListItem item in ctlSubstanceTranslation.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSubTranslation = item.Value;
                    ctlSubstanceTranslation.ControlBoundItems.Remove(item);
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idSubTranslation), "SubstanceTranslation", null);
            BindSubstanceTranslation();

            btnEditSubTranslation.Enabled = false;
            btnRemoveSubTranslation.Enabled = false;
        }

        #endregion

        #region Substance Aliases

        public void ctlSubstanceAliasesListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditSubAliases.Enabled = false;
            btnRemoveSubAliases.Enabled = false;
            int numSelected = 0;

            foreach (ListItem item in ctlSubstanceAliases.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveSubAliases.Enabled = true;
                    break;
                }
            }

            if (numSelected == 1)
            {
                btnEditSubAliases.Enabled = true;
                btnRemoveSubAliases.Enabled = true;
            }
        }


        void SUBALSPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindSubstanceAliases();
            ListItemCollection lic = ctlSubstanceAliases.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSubAliases.Enabled = false;
            btnRemoveSubAliases.Enabled = false;
        }

        void SUBALSPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlSubstanceAliases.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSubAliases.Enabled = false;
            btnRemoveSubAliases.Enabled = false;
        }

        public void btnAddSubAliasesOnClick(object sender, EventArgs e)
        {
            UcPopupFormSUBALS.ShowModalForm("", "Substance alias", null, null);
        }

        public void btnEditSubAliasesOnClick(object sender, EventArgs e)
        {
            string idSubAliases = "";
            foreach (ListItem item in ctlSubstanceAliases.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSubAliases = item.Value.ToString();

                    break;
                }
            }
            Substance_alias_PK subAlias = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idSubAliases), "SubstanceAlias", null) as Substance_alias_PK;
            UcPopupFormSUBALS.ShowModalForm("", "Substance alias", subAlias, null);
            btnEditSubAliases.Enabled = false;
        }

        public void btnRemoveSubAliasesOnClick(object sender, EventArgs e)
        {
            string idSubAliases = "";

            foreach (ListItem item in ctlSubstanceAliases.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSubAliases = item.Value;
                    ctlSubstanceAliases.ControlBoundItems.Remove(item);
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idSubAliases), "SubstanceAlias", null);
            BindSubstanceAliases();

            btnEditSubAliases.Enabled = false;
            btnRemoveSubAliases.Enabled = false;
        }

        #endregion

        #region International codes

        public void ctlInternationalCodesListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditInternationalCodes.Enabled = false;
            btnRemoveInternationalCodes.Enabled = false;
            int numSelected = 0;

            foreach (ListItem item in ctlInternationalCodes.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveInternationalCodes.Enabled = true;
                    break;
                }
            }

            if (numSelected == 1)
            {
                btnEditInternationalCodes.Enabled = true;
                btnRemoveInternationalCodes.Enabled = true;
            }
        }

        void INTCODPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindInternationalCode();
            ListItemCollection lic = ctlInternationalCodes.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditInternationalCodes.Enabled = false;
            btnRemoveInternationalCodes.Enabled = false;
        }

        void INTCODPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlInternationalCodes.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditInternationalCodes.Enabled = false;
            btnRemoveInternationalCodes.Enabled = false;
        }

        public void btnAddInternationalCodesOnClick(object sender, EventArgs e)
        {
            UcPopupFormINTCOD.ShowModalForm("", "International code", null, null);
        }

        public void btnEditInternationalCodesOnClick(object sender, EventArgs e)
        {
            string idInternationalCodes = "";
            foreach (ListItem item in ctlInternationalCodes.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idInternationalCodes = item.Value.ToString();

                    break;
                }
            }
            International_code_PK intCode = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idInternationalCodes), "InternationalCode", null) as International_code_PK;
            UcPopupFormINTCOD.ShowModalForm("", "International code", intCode, null);
            btnEditInternationalCodes.Enabled = false;
        }

        public void btnRemoveInternationalCodesOnClick(object sender, EventArgs e)
        {
            string idInternationalCodes = "";

            foreach (ListItem item in ctlInternationalCodes.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idInternationalCodes = item.Value;
                    ctlInternationalCodes.ControlBoundItems.Remove(item);
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idInternationalCodes), "InternationalCode", null);
            BindInternationalCode();

            btnEditInternationalCodes.Enabled = false;
            btnRemoveInternationalCodes.Enabled = false;
        }

        #endregion


        #region Previous evcodes

        public void ctlPreviousEvcodeListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditPreviousEvcode.Enabled = false;
            btnRemovePreviousEvcode.Enabled = false;
            int numSelected = 0;

            foreach (ListItem item in ctlPreviousEvcode.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemovePreviousEvcode.Enabled = true;
                    break;
                }
            }

            if (numSelected == 1)
            {
                btnEditPreviousEvcode.Enabled = true;
                btnRemovePreviousEvcode.Enabled = true;
            }
        }

        void PREEVCODEPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindPreviousEVCODE();
            ListItemCollection lic = ctlPreviousEvcode.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditPreviousEvcode.Enabled = false;
            btnRemovePreviousEvcode.Enabled = false;
        }

        void PREEVCODEPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlPreviousEvcode.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditPreviousEvcode.Enabled = false;
            btnRemovePreviousEvcode.Enabled = false;
        }

        public void btnAddPreviousEvcodeOnClick(object sender, EventArgs e)
        {
            UcPopupFormPREEVCODE.ShowModalForm("", "Previous EVCODE", null, null);
        }

        public void btnEditPreviousEvcodeOnClick(object sender, EventArgs e)
        {
            string idPreviousEvcode = "";
            foreach (ListItem item in ctlPreviousEvcode.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idPreviousEvcode = item.Value.ToString();

                    break;
                }
            }
            As_previous_ev_code_PK prevEvcode = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idPreviousEvcode), "PreviousEvcode", null) as As_previous_ev_code_PK;
            UcPopupFormPREEVCODE.ShowModalForm(prevEvcode.devevcode, "Previous EVCODE", prevEvcode, null);
            UcPopupFormPREEVCODE.BindForm(prevEvcode.devevcode, "");
            btnEditPreviousEvcode.Enabled = false;
        }

        public void btnRemovePreviousEvcodeOnClick(object sender, EventArgs e)
        {
            string idPreviousEvcode = "";

            foreach (ListItem item in ctlPreviousEvcode.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idPreviousEvcode = item.Value;
                    ctlPreviousEvcode.ControlBoundItems.Remove(item);
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idPreviousEvcode), "PreviousEvcode", null);
            BindPreviousEVCODE();

            btnEditPreviousEvcode.Enabled = false;
            btnRemovePreviousEvcode.Enabled = false;
        }

        #endregion

        #region Substance SSI

        public void ctlSubstanceSsiListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditSubstanceSsi.Enabled = false;
            btnRemoveSubstanceSsi.Enabled = false;
            int numSelected = 0;

            foreach (ListItem item in ctlSubstanceSsi.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveSubstanceSsi.Enabled = true;
                    break;
                }
            }

            if (numSelected == 1)
            {
                btnEditSubstanceSsi.Enabled = true;
                btnRemoveSubstanceSsi.Enabled = true;
            }
        }

        ////STRUCT
        //void STRUCTPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        //{
        //    //BindStruct(null);
        //    //ListItemCollection lic = ctlSubstanceSsi.ControlBoundItems;
        //    //foreach (ListItem li in lic)
        //    //{
        //    //    li.Selected = false;
        //    //}
        //    //btnStructEdit.Enabled = false;
        //    //btnStructRemove.Enabled = false;
        //}

        //void STRUCTPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        //{
        //    //ListItemCollection lic = ctlStructList.ControlBoundItems;
        //    //foreach (ListItem li in lic)
        //    //{
        //    //    li.Selected = false;
        //    //}
        //    //btnStructEdit.Enabled = false;
        //    //btnStructRemove.Enabled = false;
        //}

        public void btnAddSubstanceSsiOnClick(object sender, EventArgs e)
        {
            ctlSubstanceSsi.ControlBoundItems.Add(new ListItem("Dodano", DateTime.Now.Second.ToString()));
        }

        public void btnEditSubstanceSsiOnClick(object sender, EventArgs e)
        {
            string idSubstanceSsi = "";
            foreach (ListItem item in ctlSubstanceSsi.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSubstanceSsi = item.Value.ToString();

                    break;
                }
            }
            //Structure_PK structure = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idStr), "Structure", null) as Structure_PK;
            //UcPopupFormSTRUCT.ShowModalForm(s_apid, "Structure", structure, null);
            btnEditSubstanceSsi.Enabled = false;
        }

        public void btnRemoveSubstanceSsiOnClick(object sender, EventArgs e)
        {
            string idSubstanceSsi = "";

            foreach (ListItem item in ctlSubstanceSsi.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSubstanceSsi = item.Value;
                    ctlSubstanceSsi.ControlBoundItems.Remove(item);
                    break;
                }
            }
            //SSIRep.DeleteObjectByID(Int32.Parse(idStr), "Structure", null);
            //BindStruct(null);

            btnEditSubstanceSsi.Enabled = false;
            btnRemoveSubstanceSsi.Enabled = false;
        }

        #endregion

        #region Substance Attachment

        public void ctlSubAttachmentListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditSubAttachment.Enabled = false;
            btnRemoveSubAttachment.Enabled = false;
            int numSelected = 0;

            foreach (ListItem item in ctlSubAttachment.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveSubAttachment.Enabled = true;
                    break;
                }
            }

            if (numSelected == 1)
            {
                btnEditSubAttachment.Enabled = true;
                btnRemoveSubAttachment.Enabled = true;
            }
        }


        void SUBATTPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindSubstanceAttachment();
            ListItemCollection lic = ctlSubAttachment.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSubAttachment.Enabled = false;
            btnRemoveSubAttachment.Enabled = false;
        }

        void SUBATTPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlSubAttachment.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSubAttachment.Enabled = false;
            btnRemoveSubAttachment.Enabled = false;
        }

        public void btnAddSubAttachmentOnClick(object sender, EventArgs e)
        {
            UcPopupFormSUBATT.ShowModalForm("", "Substance attachment", null, null);
        }

        public void btnEditSubAttachmentOnClick(object sender, EventArgs e)
        {
            string idSubAttachment = "";
            foreach (ListItem item in ctlSubAttachment.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSubAttachment = item.Value.ToString();

                    break;
                }
            }
            Substance_attachment_PK subAtt = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idSubAttachment), "SubstanceAttachment", null) as Substance_attachment_PK;
            UcPopupFormSUBATT.ShowModalForm("", "SubstanceAttachment", subAtt, null);
            btnEditSubAttachment.Enabled = false;
        }

        public void btnRemoveSubAttachmentOnClick(object sender, EventArgs e)
        {
            string idSubAttachment = "";

            foreach (ListItem item in ctlSubAttachment.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSubAttachment = item.Value;
                    ctlSubAttachment.ControlBoundItems.Remove(item);
                    break;
                }
            }
            //SSIRep.DeleteObjectByID(Int32.Parse(idStr), "Structure", null);
            //BindStruct(null);

            btnEditSubAttachment.Enabled = false;
            btnRemoveSubAttachment.Enabled = false;
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
