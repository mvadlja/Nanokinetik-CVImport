using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUIFramework;
using Ready.Model;
using AspNetUI.Support;
using System.Threading;

namespace AspNetUI.Views
{
    public partial class SSIForm_details : DetailsForm
    {

        #region Declarations

        private SSIRepository SSIRep
        {
            get { return (SSIRepository)Session["SSIRepository"]; }
            set { Session["SSIRepository"] = value; }
        }

        private ObjectContainer RefInfoOC
        {
            get { return (ObjectContainer)Session["RI_entityOC"]; }
            set
            {
                Session["RI_entityOC"] = value;
                ((ObjectContainer)Session["RI_entityOC"]).Type = "ReferenceInformation";
            }
        }

        private ObjectContainer SingOC
        {
            get { return (ObjectContainer)Session["Sing_entityOC"]; }
            set { Session["Sing_entityOC"] = value; }
        }

        private ObjectContainer ChemicalOC
        {
            get { return (ObjectContainer)Session["Chemical_entityOC"]; }
            set { Session["Chemical_entityOC"] = value; }
        }

        private ObjectContainer NonStoOC
        {
            get { return (ObjectContainer)Session["NonSto_entityOC"]; }
            set
            {
                Session["NonSto_entityOC"] = value;
                ((ObjectContainer)Session["NonSto_entityOC"]).Type = "NonStoichiometric";
            }
        }

        string s_apid = "";

        // Model data managers

        IPharmaceutical_product_PKOperations _pharmaceutical_product_PKOperations;
        IPerson_PKOperations _personOperations;
        ISubstance_PKOperations _substanceOperations;
        ILanguagecode_PKOperations _languageCodeOperations;
        ISubstance_code_PKOperations _substanceCodeOperations;
        ISubst_clf_PKOperations _substanceClassOperations;
        IStereochemistry_PKOperations _stereochemistryOperations;
        IStruct_repres_type_PKOperations _structRepresTypeOperations;
        IStruct_repres_attach_PKOperations _structAttachmentOperations;
        ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;
        IMoiety_PKOperations _moietyOperations;
        IProperty_PKOperations _propertyOperations;
        ISubstance_s_PKOperations _substance_s_PKOperations;
        IReference_info_PKOperations _reference_info_PKOperations;
        IReference_source_PKOperations _referenceSource;
        IUSEROperations _userOperations;
        #endregion

        #region Init handlers

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            s_apid = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";

            // Retreiving model data managers from central configuration

            _pharmaceutical_product_PKOperations = new Pharmaceutical_product_PKDAL();
            _personOperations = new Person_PKDAL();
            _substanceOperations = new Substance_PKDAL();
            _languageCodeOperations = new Languagecode_PKDAL();
            _substanceCodeOperations = new Substance_code_PKDAL();
            _substanceClassOperations = new Subst_clf_PKDAL();
            _stereochemistryOperations = new Stereochemistry_PKDAL();
            _structRepresTypeOperations = new Struct_repres_type_PKDAL();
            _structAttachmentOperations = new Struct_repres_attach_PKDAL();
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();
            _moietyOperations = new Moiety_PKDAL();
            _propertyOperations = new Property_PKDAL();
            _substance_s_PKOperations = new Substance_s_PKDAL();
            _reference_info_PKOperations = new Reference_info_PKDAL();
            _referenceSource = new Reference_source_PKDAL();
            _userOperations = new USERDAL();

            EVCODESearcher.OnListItemSelected += new EventHandler<FormListEventArgs>(EVCODESearcher_OnListItemSelected);
            EVCODESearcherDisplay.OnSearchClick += new EventHandler<EventArgs>(EVCODESearcher_OnSearchClick);
            EVCODESearcherDisplay.OnRemoveClick += new EventHandler<EventArgs>(EVCODESearcherDisplay_OnRemoveClick);

            SubCodesPopupForm.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(SubCodesPopupForm_OnOkClick);
            SubCodesPopupForm.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(SubCodesPopupForm_OnCancelClick);

            SubNamesPopupForm.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(SubNamesPopupForm_OnOkClick);
            SubNamesPopupForm.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(SubNamesPopupForm_OnCancelClick);

            UcPopupFormSTRUCT.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(STRUCTPopupForm_OnOkClick);
            UcPopupFormSTRUCT.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(STRUCTPopupForm_OnCancelClick);

            UcPopupFormVER.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(VerPopupForm_OnOkClick);
            UcPopupFormVER.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(VerPopupForm_OnCancelClick);

            UcPopupFormSCLF.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(SclfPopupForm_OnOkClick);
            UcPopupFormSCLF.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(SclfPopupForm_OnCancelClick);

            MoietyPopupForm.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(MoietyPopupForm_OnOkClick);
            MoietyPopupForm.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(MoietyPopupForm_OnCancelClick);

            PropertyPopupForm.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(PropertyPopupForm_OnOkClick);
            PropertyPopupForm.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(PropertyPopupForm_OnCancelClick);

            UcPopupFormREL.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormREL_OnOkButtonClick);
            UcPopupFormREL.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormREL_OnCancelButtonClick);

            UcPopupFormGE.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormGE_OnOkButtonClick);
            UcPopupFormGE.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormGE_OnCancelButtonClick);

            UcPopupFormGN.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormGN_OnOkButtonClick);
            UcPopupFormGN.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormGN_OnCancelButtonClick);

            UcPopupFormTRG.OnOkButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormTRG_OnOkButtonClick);
            UcPopupFormTRG.OnCancelButtonClick += new EventHandler<FormDetailsEventArgs>(UcPopupFormTRG_OnCancelButtonClick);


            if (!IsPostBack)
            {
                SSIRep = new SSIRepository();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Page.Form.Enctype = "multipart/form-data";

            if (Session["UploadedAttachs"] == null) _structAttachmentOperations.DeleteNULLByUserID(SessionManager.Instance.CurrentUser.UserID);

            if (Session["SubNameSCMandatory"] != null && (bool)Session["SubNameSCMandatory"] == true)
            {
                lblSubCodesReq.Visible = true;
            }
            else
            {
                lblSubCodesReq.Visible = false;
            }

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

        #endregion

        #region FormOverrides

        public override object SaveForm(object id, string arg)
        {
            Substance_s_PK entity = null;
            if (id != null && id.ToString() != "")
                entity = _substance_s_PKOperations.GetEntity(id);

            if (entity == null)
            {
                entity = new Substance_s_PK();
            }

            entity.substance_id = EVCODESearcherDisplay.SelectedObject != null && ValidationHelper.IsValidInt(EVCODESearcherDisplay.SelectedObject.ToString()) ? (int?)Convert.ToInt32(EVCODESearcherDisplay.SelectedObject.ToString()) : null;
            entity.substance_class = ValidationHelper.IsValidInt(ctlSubstanceClass.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlSubstanceClass.ControlValue) : null;
            entity.language = ValidationHelper.IsValidInt(ctlLanguages.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlLanguages.ControlValue) : null;

            entity.name = ctlname.ControlValue.ToString();
            entity.responsible_user_FK = ValidationHelper.IsValidInt(ctlresponsible_user.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlresponsible_user.ControlValue) : null;
            entity.description = ctldescription.ControlValue.ToString();
            entity.comments = ctlcomments.ControlValue.ToString();

            (RefInfoOC.Object as Reference_info_PK).comment = tbxComment.ControlValue.ToString();
            RefInfoOC.Object = _reference_info_PKOperations.Save(RefInfoOC.Object as Reference_info_PK);
            entity.ref_info_FK = (RefInfoOC.Object as Reference_info_PK).reference_info_PK;
            RefInfoOC.ID = (int)entity.ref_info_FK;

            if (SingOC != null)
            {
                ISing_PKOperations _sing_PKOperations = new Sing_PKDAL();

                if (ChemicalOC != null)
                {
                    INon_stoichiometric_PKOperations _non_stoichiometric_PKOperations = new Non_stoichiometric_PKDAL();
                    IChemical_PKOperations _chemical_PKOperations = new Chemical_PKDAL();

                    if (NonStoOC != null)
                    {
                        (NonStoOC.Object as Non_stoichiometric_PK).number_moieties = NumMoiety.Text == "" ? 0 : Int32.Parse(NumMoiety.Text);
                        NonStoOC.Object = _non_stoichiometric_PKOperations.Save(NonStoOC.Object as Non_stoichiometric_PK);
                        (ChemicalOC.Object as Chemical_PK).non_stoichio_FK = (NonStoOC.Object as Non_stoichiometric_PK).non_stoichiometric_PK;
                        NonStoOC.ID = (int)(NonStoOC.Object as Non_stoichiometric_PK).non_stoichiometric_PK;
                    }

                    Byte[] array = new Byte[1];
                    if (RadioButtonYes.Checked)
                    {
                        array[0] = Convert.ToByte(true);
                        (ChemicalOC.Object as Chemical_PK).stoichiometric = array;
                    }
                    else if (RadioButtonNo.Checked)
                    {
                        array[0] = Convert.ToByte(false);
                        (ChemicalOC.Object as Chemical_PK).stoichiometric = array;
                    }

                    (ChemicalOC.Object as Chemical_PK).comment = ctlcomment.ControlValue.ToString();
                    ChemicalOC.Object = _chemical_PKOperations.Save(ChemicalOC.Object as Chemical_PK);
                    (SingOC.Object as Sing_PK).chemical_FK = (ChemicalOC.Object as Chemical_PK).chemical_PK;
                    ChemicalOC.ID = (int)(ChemicalOC.Object as Chemical_PK).chemical_PK;
                }

                SingOC.Object = _sing_PKOperations.Save(SingOC.Object as Sing_PK);
                entity.sing_FK = (SingOC.Object as Sing_PK).sing_PK;
                SingOC.ID = (int)entity.sing_FK;
            }

            entity = _substance_s_PKOperations.Save(entity);

            return entity;
        }

        // Clears form
        public override void ClearForm(string arg)
        {
            //ctlLanguages.ControlValue = String.Empty;
            RadioButtonYes.Checked = false;
            RadioButtonNo.Checked = false;

            DisableAssignAndRemove();
        }

        // Fills all form controls with data
        public override void FillDataDefinitions(string arg)
        {
            //BindDDLResponsibleUser();
            BindLanguages();
            BindSubstanceClassification();
            BindDDLResponsibleUser();
            BindCBStoichiomertic();

            BindForm(null, "");

            USER user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            ctlresponsible_user.ControlValue = user.Person_FK;


        }
        void BindDDLResponsibleUser()
        {
            List<Person_PK> items = _personOperations.GetEntities(); // TODO: .GetPersonsByRole("Responsible user");

            items.Sort(delegate(Person_PK c1, Person_PK c2)
            {
                return c1.name.CompareTo(c2.name);
            });

            foreach (Person_PK item in items)
            {
                item.name += " " + item.familyname;
            }

            ctlresponsible_user.SourceValueProperty = "person_PK";
            ctlresponsible_user.SourceTextExpression = "name";
            ctlresponsible_user.FillControl<Person_PK>(items);

            //Person_PK person = items.Find(p => p.name == SessionManager.Instance.CurrentUser.Username);
            //if (person != null)
            //    this.ctlresponsible_user.ControlValue = person.person_PK;
        }
        // Binds form
        public override void BindForm(object id, string arg)
        {
            if (id == null)
                id = Request.QueryString["id"] != null ? Request.QueryString["id"].ToString() : "";

            if (id != null && id.ToString() != "")
            {
                Substance_s_PK entity = _substance_s_PKOperations.GetEntity(id);

                ctlname.ControlValue = entity.name == null ? String.Empty : entity.name.ToString();
                ctlresponsible_user.ControlValue = entity.responsible_user_FK == null ? String.Empty : entity.responsible_user_FK.ToString();
                ctldescription.ControlValue = entity.description == null ? String.Empty : entity.description.ToString();
                ctlcomments.ControlValue = entity.comments == null ? String.Empty : entity.comments.ToString();

                ctlLanguages.ControlValue = entity.language == null ? String.Empty : entity.language.ToString();
                ctlSubstanceClass.ControlValue = entity.substance_class == null ? String.Empty : entity.substance_class.ToString();

                Substance_PK substance = _substanceOperations.GetEntity(entity.substance_id);
                if (substance != null) EVCODESearcherDisplay.SetSelectedObject(substance.substance_PK, substance.ev_code);

                SSIRep.Reset();
                SSIRep.LoadSubstanceFromDb(entity);

                Reference_info_PK refInfo = SSIRep.GetNotDeletedObjectByID((int)entity.ref_info_FK, "ReferenceInformation", null) as Reference_info_PK;
                RefInfoOC = SSIRep.GetObjectContainer(refInfo, "ReferenceInformation", null);
                tbxComment.ControlValue = refInfo != null && refInfo.comment != null ? refInfo.comment : String.Empty;

                BindSubNames();
                BindSubCodes();
                BindSubstanceRelationship();
                BindTarget();
                BindGene();
                BindGeneElement();
                BindSclf();
                BindVersionList();
                if (entity.substance_class == 2)
                    ctlSubstanceClassInputValueChanged(null, null);
                if (entity.sing_FK != null)
                {
                    Sing_PK sing = SSIRep.GetNotDeletedObjectByID((int)entity.sing_FK, "Sing", null) as Sing_PK;
                    SingOC = SSIRep.GetObjectContainer(sing, "Sing", null);
                    if (sing != null)
                    {
                        BindStruct(null);
                        if (sing.chemical_FK != null)
                        {

                            Chemical_PK chemical = SSIRep.GetNotDeletedObjectByID((int)sing.chemical_FK, "Chemical", SingOC) as Chemical_PK;
                            ChemicalOC = SSIRep.GetObjectContainer(chemical, "Chemical", SingOC);


                            ctlcomment.ControlValue = chemical != null && chemical.comment != null ? chemical.comment : String.Empty;

                            if (chemical.non_stoichio_FK != null)
                            {
                                Non_stoichiometric_PK nonSto = SSIRep.GetNotDeletedObjectByID((int)chemical.non_stoichio_FK, "NonStoichiometric", ChemicalOC) as Non_stoichiometric_PK;
                                NonStoOC = SSIRep.GetObjectContainer(nonSto, "NonStoichiometric", ChemicalOC);

                                NumMoiety.Text = nonSto != null && nonSto.number_moieties != null ? nonSto.number_moieties.ToString() : String.Empty;
                            }
                            ctlSubstanceType.ControlValue = 36;
                            ctlSubstanceTypeInputValueChanged(null, null);

                            BindMoiety();
                            BindProperty();
                        }
                    }
                }
            }
            else
            {
                Reference_info_PK refInfo = new Reference_info_PK();
                refInfo.reference_info_PK = SSIRep.ObjectHighestID("ReferenceInformation");
                RefInfoOC = SSIRep.AddObject((int)refInfo.reference_info_PK, refInfo, "ReferenceInformation", null);
                RefInfoOC.SetState(ActionType.New, StatusType.Saved);
            }
        }

        // Validates form
        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            if (String.IsNullOrEmpty(ctlname.ControlValue.ToString())) errorMessage += ctlname.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlLanguages.ControlValue.ToString())) errorMessage += ctlLanguages.ControlEmptyErrorMessage + "<br />";
            if (String.IsNullOrEmpty(ctlSubstanceClass.ControlValue.ToString())) errorMessage += ctlSubstanceClass.ControlEmptyErrorMessage + "<br />";
            if (ctlSubNames.ControlBoundItems.Count == 0) errorMessage += ctlSubNames.ControlEmptyErrorMessage + "<br />";
            if (Session["SubNameSCMandatory"] != null && (bool)Session["SubNameSCMandatory"] == true)
                if (ctlSubCodes.ControlBoundItems.Count == 0) errorMessage += ctlSubCodes.ControlEmptyErrorMessage + "<br />";
            if (ctlVersion.ControlBoundItems.Count == 0) errorMessage += ctlVersion.ControlEmptyErrorMessage + "<br />";
            if (ctlScfl.ControlBoundItems.Count == 0) errorMessage += ctlScfl.ControlEmptyErrorMessage + "<br />";

            if (ctlSubstanceType.ControlValue.ToString() == "36") // Chemical type
            {
                if ((!RadioButtonNo.Checked) && (!RadioButtonYes.Checked)) errorMessage += "Stoichiometric can't be empty" + "<br />";
                if (RadioButtonNo.Checked && ((String.IsNullOrEmpty(NumMoiety.Text)) || (Convert.ToInt32(NumMoiety.Text) < 2)))
                    errorMessage += "Number of assigned moieties can not be lower than 2." + "<br />";
            }

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

        void BindStruct(object id)
        {
            ctlStructList.ControlBoundItems.Clear();
            List<Structure_PK> structureList = SSIRep.GetObjectsList<Structure_PK>("Structure", SingOC);
            ctlStructList.SourceTextExpression = "struct_representation";
            ctlStructList.SourceValueProperty = "structure_PK";
            ctlStructList.FillControl(structureList);
        }

        void BindLanguages()
        {
            ctlLanguages.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Language");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.term_name_english.CompareTo(s2.term_name_english);
            });

            ctlLanguages.SourceValueProperty = "ssi__cont_voc_PK";
            ctlLanguages.SourceTextExpression = "term_name_english";
            ctlLanguages.FillControl<Ssi__cont_voc_PK>(items);
        }

        void BindSubstanceClassification()
        {
            ctlSubstanceClass.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Substance Class");

            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.list_name.CompareTo(s2.list_name);
            });

            ctlSubstanceClass.SourceValueProperty = "ssi__cont_voc_PK";
            ctlSubstanceClass.SourceTextExpression = "term_name_english";
            ctlSubstanceClass.FillControl<Ssi__cont_voc_PK>(items);
        }

        void BindSubstanceType()
        {
            ctlSubstanceType.ControlBoundItems.Clear();
            List<Ssi__cont_voc_PK> items = _ssiControlledVocabularyOperations.GetEntitiesByListName("Substance Type");
            items.Sort(delegate(Ssi__cont_voc_PK s1, Ssi__cont_voc_PK s2)
            {
                return s1.list_name.CompareTo(s2.list_name);
            });
            ctlSubstanceType.SourceValueProperty = "ssi__cont_voc_PK";
            ctlSubstanceType.SourceTextExpression = "term_name_english";
            ctlSubstanceType.FillControl<Ssi__cont_voc_PK>(items);
        }

        void BindCBStoichiomertic()
        {
            //List<bool> items = new List<bool>() { true };
            //ctlStoichiometric.FillControl<bool>(items);
        }

        #endregion

        #region Panel mux

        protected void ctlSubstanceClassInputValueChanged(object sender, EventArgs e)
        {
            ListItemCollection lic = ctlSubstanceClass.ControlBoundItems;
            pnlExtended.Visible = false;
            pnlSingleSubstance.Visible = false;
            pnlChemical.Visible = false;
            //pnlNS.Visible = false;
            // TODO: postaviti svim panelima Visible = false;

            string selectedItem = "";
            foreach (ListItem item in lic)
            {
                if (item.Selected) selectedItem = item.Value;
            }

            switch (selectedItem)
            {
                case "2": // SingleSubstance
                    pnlExtended.Visible = true;
                    pnlSingleSubstance.Visible = true;
                    if (SingOC == null)
                    {
                        Sing_PK sing = new Sing_PK();
                        sing.sing_PK = SSIRep.ObjectHighestID("Sing");
                        SingOC = SSIRep.AddObject((int)sing.sing_PK, sing, "Sing", null);
                        SingOC.SetState(ActionType.New, StatusType.Saved);
                    }
                    BindSubstanceType();
                    break;
                default:
                    SingOC = null;
                    break;
                // TODO: napraviti za ostale panele koji se mijenjaju
            }
        }

        protected void ctlSubstanceTypeInputValueChanged(object sender, EventArgs e)
        {
            ListItemCollection lic = ctlSubstanceType.ControlBoundItems;
            pnlChemical.Visible = false;
            //pnlNS.Visible = true;
            //ctlNumMoieties.ControlValue = "2";
            // TODO: postaviti svim panelima Visible = false;

            string selectedItem = "";
            foreach (ListItem item in lic)
            {
                if (item.Selected) selectedItem = item.Value;
            }

            switch (selectedItem)
            {
                case "36": // SubstanceType
                    pnlChemical.Visible = true;
                    pnlGene.Visible = false;

                    if (NonStoOC == null)
                    {
                        Chemical_PK chemical = new Chemical_PK();
                        chemical.chemical_PK = SSIRep.ObjectHighestID("Chemical");
                        ChemicalOC = SSIRep.AddObject((int)chemical.chemical_PK, chemical, "Chemical", SingOC);
                        ChemicalOC.SetState(ActionType.New, StatusType.Saved);

                        Non_stoichiometric_PK nonSto = new Non_stoichiometric_PK();
                        nonSto.non_stoichiometric_PK = SSIRep.ObjectHighestID("NonStoichiometric");
                        NonStoOC = SSIRep.AddObject((int)nonSto.non_stoichiometric_PK, nonSto, "NonStoichiometric", ChemicalOC);
                        NonStoOC.SetState(ActionType.New, StatusType.Saved);
                    }

                    BindCBStoichiomertic();
                    break;

                default:
                    ChemicalOC = null;
                    NonStoOC = null;
                    break;
                // TODO: napraviti za ostale panele koji se mijenjaju
            }
        }

        protected void ctlStoichiometricInputValueChanged(object sender, EventArgs e)
        {
            //if ((ctlStoichiometric.ControlValue as List<string>).Count > 0)
            //{
            //    Moiety_asterix.Visible = false;
            //}
            //else
            //{
            //    Moiety_asterix.Visible = true;
            //}
        }

        #endregion

        #region Searchers

        #region EVCODE
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

        #endregion

        #region PopupForms

        #region Structure

        public void ctlStructListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnStructEdit.Enabled = false;
            btnStructRemove.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlStructList.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnStructRemove.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnStructEdit.Enabled = true;
                btnStructRemove.Enabled = true;
            }
        }

        //STRUCT
        void STRUCTPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindStruct(null);
            ListItemCollection lic = ctlStructList.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnStructEdit.Enabled = false;
            btnStructRemove.Enabled = false;
        }

        void STRUCTPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlStructList.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnStructEdit.Enabled = false;
            btnStructRemove.Enabled = false;
        }

        public void btnAddStructCodeOnClick(object sender, EventArgs e)
        {
            UcPopupFormSTRUCT.ShowModalForm(s_apid, "Structure", null, SingOC);
        }

        public void btnEditStructCodeOnClick(object sender, EventArgs e)
        {
            string idStr = "";
            foreach (ListItem item in ctlStructList.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idStr = item.Value.ToString();
                    break;
                }
            }
            Structure_PK structure = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idStr), "Structure", SingOC) as Structure_PK;
            UcPopupFormSTRUCT.ShowModalForm(s_apid, "Structure", structure, SingOC);
        }

        public void btnRemoveStructOnClick(object sender, EventArgs e)
        {
            string idStr = "";

            foreach (ListItem item in ctlStructList.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idStr = item.Value;
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idStr), "Structure", SingOC);
            BindStruct(null);
            btnStructRemove.Enabled = false;
            btnStructEdit.Enabled = false;
        }

        #endregion

        #region Substance codes

        void BindSubCodes()
        {
            ctlSubCodes.ControlBoundItems.Clear();
            List<Substance_code_PK> items = SSIRep.GetObjectsList<Substance_code_PK>("SubstanceCode", null);

            items.Sort(delegate(Substance_code_PK item1, Substance_code_PK item2)
            {
                return item1.code.CompareTo(item2.code);
            });

            ctlSubCodes.SourceTextExpression = "code";
            ctlSubCodes.SourceValueProperty = "substance_code_PK";
            ctlSubCodes.FillControl<Substance_code_PK>(items);
        }

        public void ctlSubCodesListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditSubCode.Enabled = false;
            btnRemoveSubCodes.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlSubCodes.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveSubCodes.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditSubCode.Enabled = true;
                btnRemoveSubCodes.Enabled = true;
            }
        }

        public void btnAddSubCodeOnClick(object sender, EventArgs e)
        {
            SubCodesPopupForm.ShowModalForm(s_apid, "Substance code", null, null);
        }

        void SubCodesPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindSubCodes();
            ListItemCollection lic = ctlSubCodes.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSubCode.Enabled = false;
            btnRemoveSubCodes.Enabled = false;
        }
        void SubCodesPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlSubCodes.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSubCode.Enabled = false;
            btnRemoveSubCodes.Enabled = false;
        }
        public void btnEditSubCodeOnClick(object sender, EventArgs e)
        {
            string idSC = "0";
            foreach (ListItem item in ctlSubCodes.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSC = item.Value.ToString();
                    break;
                }
            }
            Substance_code_PK sc = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idSC), "SubstanceCode", null) as Substance_code_PK;

            SubCodesPopupForm.ShowModalForm(s_apid, "Substance code", sc, null);
        }

        public void btnRemoveSubCodesOnClick(object sender, EventArgs e)
        {
            string idSC = "0";

            foreach (ListItem item in ctlSubCodes.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSC = item.Value;
                    ctlSubCodes.ControlBoundItems.Remove(item);
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idSC), "SubstanceCode", null);
            BindSubCodes();
            btnRemoveSubCodes.Enabled = false;
            btnEditSubCode.Enabled = false;
        }
        #endregion

        #region Substance names
        void BindSubNames()
        {
            ctlSubNames.ControlBoundItems.Clear();
            List<Substance_name_PK> items = SSIRep.GetObjectsList<Substance_name_PK>("SubstanceName", null);

            items.Sort(delegate(Substance_name_PK item1, Substance_name_PK item2)
            {
                return item1.subst_name.CompareTo(item2.subst_name);
            });

            ctlSubNames.SourceTextExpression = "subst_name";
            ctlSubNames.SourceValueProperty = "substance_name_PK";
            ctlSubNames.FillControl<Substance_name_PK>(items);
        }

        public void ctlSubNamesListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditSubName.Enabled = false;
            btnRemoveSubNames.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlSubNames.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveSubNames.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditSubName.Enabled = true;
                btnRemoveSubNames.Enabled = true;
            }
        }

        public void btnAddSubNameOnClick(object sender, EventArgs e)
        {
            SubNamesPopupForm.ShowModalForm(s_apid, "Substance Name", null, null);
        }

        void SubNamesPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindSubNames();
            ListItemCollection lic = ctlSubNames.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSubName.Enabled = false;
            btnRemoveSubNames.Enabled = false;
        }

        void SubNamesPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlSubNames.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSubName.Enabled = false;
            btnRemoveSubNames.Enabled = false;
        }

        public void btnEditSubNameOnClick(object sender, EventArgs e)
        {
            string idSC = "0";
            foreach (ListItem item in ctlSubNames.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSC = item.Value.ToString();
                    break;
                }
            }
            Substance_name_PK sc = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idSC), "SubstanceName", null) as Substance_name_PK;

            SubNamesPopupForm.ShowModalForm(s_apid, "Substance name", sc, null);
        }

        public void btnRemoveSubNamesOnClick(object sender, EventArgs e)
        {
            string idSC = "0";

            foreach (ListItem item in ctlSubNames.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSC = item.Value;
                    ctlSubNames.ControlBoundItems.Remove(item);
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idSC), "SubstanceName", null);
            BindSubNames();
            btnEditSubName.Enabled = false;
            btnRemoveSubNames.Enabled = false;
        }
        #endregion

        #region VERSION

        public void ctlVersionListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditVER.Enabled = false;
            btnDeleteVER.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlVersion.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnDeleteVER.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditVER.Enabled = true;
                btnDeleteVER.Enabled = true;
            }
        }

        public void btnAddVerOnClick(object sender, EventArgs e)
        {
            UcPopupFormVER.ShowModalForm("", "Version", null, null);
        }

        public void btnEditVerOnClick(object sender, EventArgs e)
        {
            string idVer = "";
            foreach (ListItem item in ctlVersion.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idVer = item.Value.ToString();
                    break;
                }
            }

            Version_PK version = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idVer), "Version", null) as Version_PK;

            UcPopupFormVER.ShowModalForm(s_apid, "Version", version, null);
        }

        public void btnRemoveVerOnClick(object sender, EventArgs e)
        {
            string versionPk = "";

            foreach (ListItem item in ctlVersion.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    versionPk = item.Value;
                    break;
                }
            }

            SSIRep.DeleteObjectByID(Int32.Parse(versionPk), "Version", null);
            BindVersionList();
            btnDeleteVER.Enabled = false;
            btnEditVER.Enabled = false;
        }

        void VerPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindVersionList();
            ListItemCollection lic = ctlVersion.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditVER.Enabled = false;
            btnDeleteVER.Enabled = false;
        }

        void VerPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlVersion.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditVER.Enabled = false;
            btnDeleteVER.Enabled = false;
        }

        private void BindVersionList()
        {
            ctlVersion.ControlBoundItems.Clear();
            List<Version_PK> items = SSIRep.GetObjectsList<Version_PK>("Version", null);
            ctlVersion.SourceTextExpression = "version_number";
            ctlVersion.SourceValueProperty = "version_PK";
            ctlVersion.FillControl<Version_PK>(items);
        }


        #endregion

        #region SubstanceClassification

        public void ctlScflInputValueChanged(object sender, ValueChangedEventArgs e)
        {

            btnEditSclf.Enabled = false;
            btnRemoveSclf.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlScfl.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveSclf.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditSclf.Enabled = true;
                btnRemoveSclf.Enabled = true;
            }
        }

        public void btnAddSclfOnClick(object sender, EventArgs e)
        {
            UcPopupFormSCLF.ShowModalForm("", "Substance classification", null, RefInfoOC);
        }

        public void btnEditSclfOnClick(object sender, EventArgs e)
        {
            string idSclf = "";
            foreach (ListItem item in ctlScfl.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSclf = item.Value.ToString();
                    break;
                }
            }

            Subst_clf_PK sclf = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idSclf), "SubstanceClassification", RefInfoOC) as Subst_clf_PK;
            UcPopupFormSCLF.ShowModalForm("", "Substance classification", sclf, RefInfoOC);
        }

        public void btnRemoveSclfOnClick(object sender, EventArgs e)
        {
            string idSclf = "";

            foreach (ListItem item in ctlScfl.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idSclf = item.Value;
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idSclf), "SubstanceClassification", RefInfoOC);
            BindSclf();
            btnRemoveSclf.Enabled = false;
            btnEditSclf.Enabled = false;
        }
        void SclfPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindSclf();
            ListItemCollection lic = ctlScfl.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSclf.Enabled = false;
            btnRemoveSclf.Enabled = false;
        }

        void SclfPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlScfl.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditSclf.Enabled = false;
            btnRemoveSclf.Enabled = false;
        }

        private void BindSclf()
        {
            ctlScfl.ControlBoundItems.Clear();
            List<Subst_clf_PK> items = SSIRep.GetObjectsList<Subst_clf_PK>("SubstanceClassification", RefInfoOC);

            foreach (Subst_clf_PK item in items)
            {
                string domain = _ssiControlledVocabularyOperations.GetEntity(item.domain).term_name_english;
                string sclf = _ssiControlledVocabularyOperations.GetEntity(item.substance_classification).term_name_english;

                if (domain != null && sclf != null)
                    ctlScfl.ControlBoundItems.Add(new ListItem(String.Format("{0} {1}", domain, sclf), item.subst_clf_PK.ToString()));
            }
            //ctlScfl.SourceTextExpression = "sclf_code";
            //ctlScfl.SourceValueProperty = "subst_clf_PK";
            //ctlScfl.FillControl<Subst_clf_PK>(items);
        }
        #endregion

        #region Substance relationship
        public void ctlRelInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditRel.Enabled = false;
            btnRemoveRel.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlRel.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveRel.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditRel.Enabled = true;
                btnRemoveRel.Enabled = true;
            }
        }

        public void btnAddRelOnClick(object sender, EventArgs e)
        {
            UcPopupFormREL.ShowModalForm("", "Substance relationship", null, RefInfoOC);
        }

        public void btnEditRelOnClick(object sender, EventArgs e)
        {
            string idRel = "";
            foreach (ListItem item in ctlRel.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idRel = item.Value.ToString();
                    break;
                }
            }
            Substance_relationship_PK rel = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idRel), "SubstanceRelationship", RefInfoOC) as Substance_relationship_PK;
            UcPopupFormREL.ShowModalForm("", "Substance relationship", rel, RefInfoOC);
        }

        public void btnRemoveRelOnClick(object sender, EventArgs e)
        {
            string idRel = "";

            foreach (ListItem item in ctlRel.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idRel = item.Value;
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idRel), "SubstanceRelationship", RefInfoOC);
            BindSubstanceRelationship();
            btnRemoveRel.Enabled = false;
            btnEditRel.Enabled = false;
        }
        void UcPopupFormREL_OnOkButtonClick(object sender, FormDetailsEventArgs e)
        {
            BindSubstanceRelationship();
            ListItemCollection lic = ctlRel.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnRemoveRel.Enabled = false;
            btnEditRel.Enabled = false;
        }

        void UcPopupFormREL_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlRel.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnRemoveRel.Enabled = false;
            btnEditRel.Enabled = false;
        }

        private void BindSubstanceRelationship()
        {
            ctlRel.ControlBoundItems.Clear();
            List<Substance_relationship_PK> items = SSIRep.GetObjectsList<Substance_relationship_PK>("SubstanceRelationship", RefInfoOC);
            ctlRel.SourceTextExpression = "substance_name";
            ctlRel.SourceValueProperty = "substance_relationship_PK";
            ctlRel.FillControl<Substance_relationship_PK>(items);
        }
        #endregion

        #region Moieties

        public void ctlMoietyListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditMoiety.Enabled = false;
            btnRemoveMoiety.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlMoiety.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveMoiety.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditMoiety.Enabled = true;
                btnRemoveMoiety.Enabled = true;
            }
            NumMoiety.Text = ctlMoiety.ControlBoundItems.Count.ToString();
        }

        public void btnAddMoietyOnClick(object sender, EventArgs e)
        {
            MoietyPopupForm.ShowModalForm("", "Moiety", null, NonStoOC);
        }

        void MoietyPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindMoiety();
            ListItemCollection lic = ctlMoiety.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditMoiety.Enabled = false;
            btnRemoveMoiety.Enabled = false;
        }

        void MoietyPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlMoiety.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditMoiety.Enabled = false;
            btnRemoveMoiety.Enabled = false;
        }

        public void btnEditMoietyOnClick(object sender, EventArgs e)
        {
            string idMoiety = "";

            foreach (ListItem item in ctlMoiety.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idMoiety = item.Value.ToString();
                    break;
                }
            }
            Moiety_PK moiety = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idMoiety), "Moiety", NonStoOC) as Moiety_PK;

            MoietyPopupForm.ShowModalForm(s_apid, "Moiety", moiety, NonStoOC);
        }

        public void btnRemoveMoietyOnClick(object sender, EventArgs e)
        {
            string idMoiety = "";

            foreach (ListItem item in ctlMoiety.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idMoiety = item.Value;
                    ctlMoiety.ControlBoundItems.Remove(item);
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idMoiety), "Moiety", NonStoOC);
            BindMoiety();
            btnEditMoiety.Enabled = false;
            btnRemoveMoiety.Enabled = false;
        }

        private void BindMoiety()
        {
            ctlMoiety.ControlBoundItems.Clear();
            List<Moiety_PK> items = SSIRep.GetObjectsList<Moiety_PK>("Moiety", NonStoOC);
            ctlMoiety.SourceTextExpression = "moiety_name";
            ctlMoiety.SourceValueProperty = "moiety_PK";
            ctlMoiety.FillControl<Moiety_PK>(items);
            NumMoiety.Text = ctlMoiety.ControlBoundItems.Count.ToString();
            Moiety_asterix.ForeColor = ctlMoiety.ControlBoundItems.Count < 2 ? System.Drawing.Color.Red : System.Drawing.Color.Green;
        }

        #endregion

        #region Gene
        public void ctlGnInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditGn.Enabled = false;
            btnRemoveGn.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlGene.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveGn.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditGn.Enabled = true;
                btnRemoveGn.Enabled = true;
            }
        }

        public void btnAddGnOnClick(object sender, EventArgs e)
        {
            UcPopupFormGN.ShowModalForm("", "Gene", null, RefInfoOC);
        }

        public void btnRemoveGnOnClick(object sender, EventArgs e)
        {
            string idGn = "";
            foreach (ListItem item in ctlGene.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idGn = item.Value.ToString();
                    break;
                }
            }

            SSIRep.DeleteObjectByID(Int32.Parse(idGn), "Gene", RefInfoOC);
            BindGene();
            btnEditGn.Enabled = false;
            btnRemoveGn.Enabled = false;
        }

        private void BindGene()
        {
            ctlGene.ControlBoundItems.Clear();
            List<Gene_PK> items = SSIRep.GetObjectsList<Gene_PK>("Gene", RefInfoOC);
            ctlGene.SourceTextExpression = "gene_name";
            ctlGene.SourceValueProperty = "gene_PK";
            ctlGene.FillControl<Gene_PK>(items);
        }

        public void btnEditGnOnClick(object sender, EventArgs e)
        {
            string idGn = "";
            foreach (ListItem item in ctlGene.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idGn = item.Value.ToString();
                    break;
                }
            }
            Gene_PK gene = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idGn), "Gene", RefInfoOC) as Gene_PK;
            UcPopupFormGN.ShowModalForm("", "Gene", gene, RefInfoOC);
        }

        public void UcPopupFormGN_OnOkButtonClick(object sender, FormDetailsEventArgs e)
        {
            BindGene();
            ListItemCollection lic = ctlGene.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditGn.Enabled = false;
            btnRemoveGn.Enabled = false;
        }

        void UcPopupFormGN_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlGene.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditGn.Enabled = false;
            btnRemoveGn.Enabled = false;
        }

        #endregion

        #region Property

        public void ctlPropertyListInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditProperty.Enabled = false;
            btnRemoveProperty.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlProperty.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveProperty.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditProperty.Enabled = true;
                btnRemoveProperty.Enabled = true;
            }
        }

        public void btnAddPropertyOnClick(object sender, EventArgs e)
        {
            PropertyPopupForm.ShowModalForm("", "Property", null, NonStoOC);
        }

        void PropertyPopupForm_OnOkClick(object sender, FormDetailsEventArgs e)
        {
            BindProperty();
            ListItemCollection lic = ctlProperty.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditProperty.Enabled = false;
            btnRemoveProperty.Enabled = false;
        }

        void PropertyPopupForm_OnCancelClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlProperty.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditProperty.Enabled = false;
            btnRemoveProperty.Enabled = false;
        }
        public void btnEditPropertyOnClick(object sender, EventArgs e)
        {
            string idProperty = "";
            foreach (ListItem item in ctlProperty.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idProperty = item.Value.ToString();
                    break;
                }
            }
            Property_PK property = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idProperty), "Property", NonStoOC) as Property_PK;

            PropertyPopupForm.ShowModalForm(s_apid, "Property", property, NonStoOC);
        }

        public void btnRemovePropertyOnClick(object sender, EventArgs e)
        {
            string idProperty = "";

            foreach (ListItem item in ctlProperty.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idProperty = item.Value;
                    ctlProperty.ControlBoundItems.Remove(item);
                    break;
                }
            }
            SSIRep.DeleteObjectByID(Int32.Parse(idProperty), "Property", NonStoOC);
            BindProperty();
            btnEditProperty.Enabled = false;
            btnRemoveProperty.Enabled = false;
        }

        private void BindProperty()
        {
            ctlProperty.ControlBoundItems.Clear();
            List<Property_PK> items = SSIRep.GetObjectsList<Property_PK>("Property", NonStoOC);
            ctlProperty.SourceTextExpression = "property_name";
            ctlProperty.SourceValueProperty = "property_PK";
            ctlProperty.FillControl<Property_PK>(items);
        }

        #endregion

        #region Gene element
        public void ctlGeInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditGe.Enabled = false;
            btnRemoveGe.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlGe.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveGe.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditGe.Enabled = true;
                btnRemoveGe.Enabled = true;
            }
        }

        public void btnAddGeOnClick(object sender, EventArgs e)
        {
            UcPopupFormGE.ShowModalForm("", "Gene element", null, RefInfoOC);
        }

        public void btnEditGeOnClick(object sender, EventArgs e)
        {
            string idGe = "";
            foreach (ListItem item in ctlGe.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idGe = item.Value.ToString();
                    break;
                }
            }

            Gene_element_PK geneElement = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idGe), "GeneElement", RefInfoOC) as Gene_element_PK;
            UcPopupFormGE.ShowModalForm("", "Gene", geneElement, RefInfoOC);
        }

        public void btnRemoveGeOnClick(object sender, EventArgs e)
        {
            string idGe = "";
            foreach (ListItem item in ctlGe.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idGe = item.Value.ToString();
                    break;
                }
            }

            SSIRep.DeleteObjectByID(Int32.Parse(idGe), "GeneElement", RefInfoOC);
            BindGeneElement();
            btnEditGe.Enabled = false;
            btnRemoveGe.Enabled = false;

        }

        private void BindGeneElement()
        {
            ctlGe.ControlBoundItems.Clear();
            List<Gene_element_PK> items = SSIRep.GetObjectsList<Gene_element_PK>("GeneElement", RefInfoOC);
            ctlGe.SourceTextExpression = "ge_name";
            ctlGe.SourceValueProperty = "gene_element_PK";
            ctlGe.FillControl<Gene_element_PK>(items);
        }

        public void UcPopupFormGE_OnOkButtonClick(object sender, FormDetailsEventArgs e)
        {
            BindGeneElement();
            ListItemCollection lic = ctlGe.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditGe.Enabled = false;
            btnRemoveGe.Enabled = false;
        }

        void UcPopupFormGE_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlGe.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditGe.Enabled = false;
            btnRemoveGe.Enabled = false;
        }
        #endregion

        #region Target

        public void ctlTrgInputValueChanged(object sender, ValueChangedEventArgs e)
        {
            btnEditTrg.Enabled = false;
            btnRemoveTrg.Enabled = false;
            int numSelected = 0;
            foreach (ListItem item in ctlTrg.ControlBoundItems)
            {
                if (item.Selected == true)
                    numSelected++;
                if (numSelected > 1)
                {
                    btnRemoveTrg.Enabled = true;
                    break;
                }
            }
            if (numSelected == 1)
            {
                btnEditTrg.Enabled = true;
                btnRemoveTrg.Enabled = true;
            }
        }

        public void btnAddTrgOnClick(object sender, EventArgs e)
        {
            UcPopupFormTRG.ShowModalForm("", "Target", null, RefInfoOC);
        }

        public void btnEditTrgOnClick(object sender, EventArgs e)
        {
            string idTrg = "";
            foreach (ListItem item in ctlTrg.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idTrg = item.Value.ToString();
                    break;
                }
            }

            Target_PK target = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idTrg), "Target", RefInfoOC) as Target_PK;
            UcPopupFormTRG.ShowModalForm("", "Target", target, RefInfoOC);
        }

        public void btnRemoveTrgOnClick(object sender, EventArgs e)
        {
            string idTrg = "";
            foreach (ListItem item in ctlTrg.ControlBoundItems)
            {
                if (item.Selected == true)
                {
                    idTrg = item.Value.ToString();
                    break;
                }
            }

            SSIRep.DeleteObjectByID(Int32.Parse(idTrg), "Target", RefInfoOC);
            BindTarget();
            btnEditTrg.Enabled = false;
            btnRemoveTrg.Enabled = false;
        }

        public void UcPopupFormTRG_OnOkButtonClick(object sender, FormDetailsEventArgs e)
        {
            BindTarget();
            ListItemCollection lic = ctlTrg.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditTrg.Enabled = false;
            btnRemoveTrg.Enabled = false;
        }

        void UcPopupFormTRG_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            ListItemCollection lic = ctlTrg.ControlBoundItems;
            foreach (ListItem li in lic)
            {
                li.Selected = false;
            }
            btnEditTrg.Enabled = false;
            btnRemoveTrg.Enabled = false;
        }

        private void BindTarget()
        {
            ctlTrg.ControlBoundItems.Clear();
            List<Target_PK> items = SSIRep.GetObjectsList<Target_PK>("Target", RefInfoOC);
            ctlTrg.SourceTextExpression = "target_gene_name";
            ctlTrg.SourceValueProperty = "target_PK";
            ctlTrg.FillControl<Target_PK>(items);
        }
        #endregion

        #endregion


        #region RadioButtons
        protected virtual void NSYesChecked(object sender, EventArgs e)
        {
            Moiety_asterix.Visible = false;
        }
        protected virtual void NSNoChecked(object sender, EventArgs e)
        {
            Moiety_asterix.Visible = true;
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
