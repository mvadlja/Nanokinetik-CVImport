using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI.Views.ApprovedSubstanceView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idAprSub;

        private IApproved_substance_PKOperations _approvedSubstanceOperations;
        private ISubstance_PKOperations _substanceOperations;
        private ISsi__cont_voc_PKOperations _ssiControlledVocabularyOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private IUSEROperations _userOperations;

        #endregion

        #region Properties

        private SSIRepository SSIRep
        {
            get { return (SSIRepository)Session["SSIRepository"] ?? (SSIRepository)(Session["SSIRepository"] = new SSIRepository()); }
            set { Session["SSIRepository"] = value; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadFormVariables();
            BindEventHandlers();
            GenerateContextMenuItems();
            AssociatePanels(pnlForm, pnlFooter);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsPostBack) return;

            InitForm(null);

            if (FormType == FormType.Edit || FormType == FormType.SaveAs)
            {
                BindForm(null);
            }

            SetFormControlsDefaults(null);
            SecurityPageSpecific();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            GenerateTabMenuItems();
            GenerateTopMenuItems();
            MasterPage.UpTopMenu.Update();
        }

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();
            LoadActionQuery();

            _idAprSub = ValidationHelper.IsValidInt(Request.QueryString["idAprSub"]) ? int.Parse(Request.QueryString["idAprSub"]) : (int?)null;

            _approvedSubstanceOperations = new Approved_substance_PKDAL();
            _substanceOperations = new Substance_PKDAL();
            _ssiControlledVocabularyOperations = new Ssi__cont_voc_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _userOperations = new USERDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            txtSrSubstanceEvCode.Searcher.OnListItemSelected += TxtSrSubstanceEvCode_OnListItemSelected;

            PopupSubstanceTranslation.OnOkButtonClick += PopupSubstanceTranslation_OnOkButtonClick;
            PopupSubstanceTranslation.OnCancelButtonClick += PopupSubstanceTranslation_OnCancelButtonClick;

            lbExtSubstanceTranslation.OnAddClick += LbExtSubstanceTranslation_OnAddClick;
            lbExtSubstanceTranslation.OnEditClick += LbExtSubstanceTranslation_OnEditClick;
            lbExtSubstanceTranslation.OnRemoveClick += LbExtSubstanceTranslation_OnRemoveClick;

            PopupSubstanceAlias.OnOkButtonClick += PopupSubstanceAlias_OnOkButtonClick;
            PopupSubstanceAlias.OnCancelButtonClick += PopupSubstanceAlias_OnCancelButtonClick;

            lbExtSubstanceAliases.OnAddClick += LbExtSubstanceAliases_OnAddClick;
            lbExtSubstanceAliases.OnEditClick += LbExtSubstanceAliases_OnEditClick;
            lbExtSubstanceAliases.OnRemoveClick += LbExtSubstanceAliases_OnRemoveClick;

            PopupInternationalCode.OnOkButtonClick += PopupInternationalCode_OnOkButtonClick;
            PopupInternationalCode.OnCancelButtonClick += PopupInternationalCode_OnCancelButtonClick;

            lbExtInternationalCodes.OnAddClick += LbExtInternationalCodes_OnAddClick;
            lbExtInternationalCodes.OnEditClick += LbExtInternationalCodes_OnEditClick;
            lbExtInternationalCodes.OnRemoveClick += LbExtInternationalCodes_OnRemoveClick;

            PopupPreviousEvCode.OnOkButtonClick += PopupPreviousEvCode_OnOkButtonClick;
            PopupPreviousEvCode.OnCancelButtonClick += PopupPreviousEvCode_OnCancelButtonClick;

            lbExtPreviousEvCodes.OnAddClick += LbExtPreviousEvCodes_OnAddClick;
            lbExtPreviousEvCodes.OnEditClick += LbExtPreviousEvCodes_OnEditClick;
            lbExtPreviousEvCodes.OnRemoveClick += LbExtPreviousEvCodes_OnRemoveClick;

            PopupSubstanceAttachment.OnOkButtonClick += PopupSubstanceAttachment_OnOkButtonClick;
            PopupSubstanceAttachment.OnCancelButtonClick += PopupSubstanceAttachment_OnCancelButtonClick;

            lbExtSubstanceAttachment.OnAddClick += LbExtSubstanceAttachment_OnAddClick;
            lbExtSubstanceAttachment.OnEditClick += LbExtSubstanceAttachment_OnEditClick;
            lbExtSubstanceAttachment.OnRemoveClick += LbExtSubstanceAttachment_OnRemoveClick;
        }

        private void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
        }

        #endregion

        #region Fill

        private void ClearForm(object arg)
        {
            txtSrSubstanceEvCode.Clear();
            txtSubstanceName.Text = String.Empty;
            txtCasNumber.Text = String.Empty;
            txtMolecularFormula.Text = String.Empty;
            ddlSubstanceClass.SelectedValue = String.Empty;
            txtCbd.Text = String.Empty;
            lbExtSubstanceTranslation.Clear();
            lbExtSubstanceAliases.Clear();
            lbExtInternationalCodes.Clear();
            lbExtPreviousEvCodes.Clear();
            lbExtSubstanceAttachment.Clear();
            txtComments.Text = String.Empty;

            SSIRep = new SSIRepository();
        }

        private void FillFormControls(object arg)
        {
            FillDdlSubstanceClass();
        }

        private void SetFormControlsDefaults(object arg)
        {

        }

        private void FillDdlSubstanceClass()
        {
            var substanceClassList = _ssiControlledVocabularyOperations.GetEntitiesByListName(Constant.SSIListName.SubstanceClass);
            ddlSubstanceClass.Fill(substanceClassList, x => x.term_name_english, x => x.ssi__cont_voc_PK);
            ddlSubstanceClass.SortItemsByText();
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idAprSub.HasValue) return;

            var approvedSubstance = _approvedSubstanceOperations.GetEntity(_idAprSub.Value);
            if (approvedSubstance == null || !approvedSubstance.approved_substance_PK.HasValue) return;

            // Substance EvCode
            txtSrSubstanceEvCode.SelectedValue = approvedSubstance.ev_code;
            txtSrSubstanceEvCode.Text = approvedSubstance.ev_code;

            // Substance name
            txtSubstanceName.Text = approvedSubstance.substancename;

            // CAS number
            txtCasNumber.Text = approvedSubstance.casnumber;

            // Molecular formula
            txtMolecularFormula.Text = approvedSubstance.molecularformula;

            // Substance class
            ddlSubstanceClass.SelectedId = approvedSubstance.Class;

            // CBD
            txtCbd.Text = approvedSubstance.cbd;

            SSIRep.LoadApprovedSubstanceFromDb(approvedSubstance);

            // Substance translations
            BindSubstanceTranslations();

            // Substance aliases
            BindSubstanceAliases();

            // International codes
            BindInternationalCodes();

            // Previous evcodes
            BindPreviousEvCodes();

            // Substance attachments
            BindSubstanceAttachments();

            // Comments
            txtComments.Text = approvedSubstance.comments;
        }

        private void BindSubstanceTranslations()
        {
            var substanceTranslationList = SSIRep.GetObjectsList<Substance_translations_PK>("SubstanceTranslation", null);
            lbExtSubstanceTranslation.Fill(
                substanceTranslationList,
                x => string.Format("{0}{1}", x.languagecode, !string.IsNullOrWhiteSpace(x.term) ? " - " + x.term : ""),
                x => x.substance_translations_PK);
            lbExtSubstanceTranslation.LbInput.SortItemsByText();
        }

        private void BindSubstanceAliases()
        {
            var substanceAliasList = SSIRep.GetObjectsList<Substance_alias_PK>("SubstanceAlias", null);
            lbExtSubstanceAliases.Fill(substanceAliasList, x => x.aliasname, x => x.substance_alias_PK);
            lbExtSubstanceAliases.LbInput.SortItemsByText();
        }

        private void BindInternationalCodes()
        {
            var internationalCodeList = SSIRep.GetObjectsList<International_code_PK>("InternationalCode", null);
            lbExtInternationalCodes.Fill(internationalCodeList, x => x.referencetext, x => x.international_code_PK);
            lbExtInternationalCodes.LbInput.SortItemsByText();
        }

        private void BindPreviousEvCodes()
        {
            var previousEvCodeList = SSIRep.GetObjectsList<As_previous_ev_code_PK>("PreviousEvcode", null);
            lbExtPreviousEvCodes.Fill(previousEvCodeList, x => x.devevcode, x => x.as_previous_ev_code_PK);
            lbExtPreviousEvCodes.LbInput.SortItemsByText();
        }

        private void BindSubstanceAttachments()
        {
            var substanceAttachmentList = SSIRep.GetObjectsList<Substance_attachment_PK>("SubstanceAttachment", null);
            lbExtSubstanceAttachment.Fill(substanceAttachmentList, x => x.validitydeclaration, x => x.substance_attachment_PK);
            lbExtSubstanceAttachment.LbInput.SortItemsByText();
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(txtSubstanceName.Text))
            {
                errorMessage += "Substance name can't be empty.<br />";
                txtSubstanceName.ValidationError = "Substance name can't be empty.";
            }

            if (!String.IsNullOrWhiteSpace(errorMessage))
            {
                MasterPage.ModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }

            return true;
        }

        private void ClearValidationErrors()
        {
            // Left pane
            txtSubstanceName.ValidationError = String.Empty;

            // Right pane
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);
            
            var approvedSubstance = new Approved_substance_PK();

            if (FormType == FormType.Edit)
            {
                approvedSubstance = _approvedSubstanceOperations.GetEntity(_idAprSub);
            }

            if (approvedSubstance == null) return null;

            approvedSubstance.ev_code = txtSrSubstanceEvCode.SelectedValue;
            approvedSubstance.substancename = txtSubstanceName.Text;
            approvedSubstance.casnumber = txtCasNumber.Text;
            approvedSubstance.molecularformula = txtMolecularFormula.Text;
            approvedSubstance.Class = ddlSubstanceClass.SelectedId;
            approvedSubstance.cbd = txtCbd.Text;
            approvedSubstance.comments = txtComments.Text;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                approvedSubstance = _approvedSubstanceOperations.Save(approvedSubstance);

                if (!approvedSubstance.approved_substance_PK.HasValue) return null;

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, approvedSubstance.approved_substance_PK, "APPROVED_SUBSTANCE", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, approvedSubstance.approved_substance_PK, "APPROVED_SUBSTANCE", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return approvedSubstance;
        }

        #endregion

        #region Delete

        private void DeleteEntity(object arg)
        {

        }

        #endregion

        #endregion

        #region Event handlers

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.Cancel:
                    if (From == "ApprovedSubstance") Response.Redirect(string.Format("~/Views/ApprovedSubstanceView/List.aspx?EntityContext={0}", EntityContext.ApprovedSubstance));
                    Response.Redirect(string.Format("~/Views/ApprovedSubstanceView/List.aspx?EntityContext={0}", EntityContext.ApprovedSubstance));
                    break;

                case ContextMenuEventTypes.Save:

                    if (ValidateForm(null))
                    {
                        var savedApprovedSubstance = SaveForm(null);
                        if (savedApprovedSubstance is Approved_substance_PK)
                        {
                            var approvedSubstance = savedApprovedSubstance as Approved_substance_PK;
                            if (approvedSubstance.approved_substance_PK.HasValue)
                        {
                            SSIRep.SaveASToDb(approvedSubstance.approved_substance_PK.Value);

                                Response.Redirect(string.Format("~/Views/ApprovedSubstanceView/List.aspx?EntityContext={0}", EntityContext.ApprovedSubstance));
                            }
                        }
                        Response.Redirect(string.Format("~/Views/ApprovedSubstanceView/List.aspx?EntityContext={0}", EntityContext.ApprovedSubstance));
                    }
                    break;
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            OnContextMenuItemClick(null, new ContextMenuEventArgs(ContextMenuEventTypes.Save));
        }

        protected void btnCancel_OnClick(object sender, EventArgs e)
        {
            OnContextMenuItemClick(null, new ContextMenuEventArgs(ContextMenuEventTypes.Cancel));
        }

        #endregion

        #region Substance EvCode

        private void TxtSrSubstanceEvCode_OnListItemSelected(object sender, FormEventArgs<int> e)
        {
            var substance = _substanceOperations.GetEntity(e.Data);

            if (substance != null && substance.ev_code != null)
            {
                txtSrSubstanceEvCode.SelectedValue = substance.ev_code;
                txtSrSubstanceEvCode.Text = substance.ev_code;
            }
        }

        #endregion

        #region Substance translation

        private void LbExtSubstanceTranslation_OnAddClick(object sender, EventArgs e)
        {
            PopupSubstanceTranslation.ShowModalForm("", "Substance translation", null, null);
        }

        private void LbExtSubstanceTranslation_OnEditClick(object sender, EventArgs eventArgs)
        {
            string idSubTranslation = lbExtSubstanceTranslation.LbInput.GetFirstSelectedValue();

            if (!ValidationHelper.IsValidInt(idSubTranslation)) return;

            var subTranslation = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idSubTranslation), "SubstanceTranslation", null) as Substance_translations_PK;
            PopupSubstanceTranslation.ShowModalForm(null, "Substance translation", subTranslation, null);
        }

        private void LbExtSubstanceTranslation_OnRemoveClick(object sender, EventArgs eventArgs)
        {
            string idSubTranslation = lbExtSubstanceTranslation.LbInput.GetFirstSelectedValue();

            if (!ValidationHelper.IsValidInt(idSubTranslation)) return;

            SSIRep.DeleteObjectByID(Int32.Parse(idSubTranslation), "SubstanceTranslation", null);
            BindSubstanceTranslations();
        }

        void PopupSubstanceTranslation_OnOkButtonClick(object sender, FormDetailsEventArgs e)
        {
            BindSubstanceTranslations();
            lbExtSubstanceTranslation.LbInput.UnselectAll();
        }

        void PopupSubstanceTranslation_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            lbExtSubstanceTranslation.LbInput.UnselectAll();
        }

        #endregion

        #region Substance alias

        private void LbExtSubstanceAliases_OnAddClick(object sender, EventArgs eventArgs)
        {
            PopupSubstanceAlias.ShowModalForm("", "Substance alias", null, null);
        }

        private void LbExtSubstanceAliases_OnEditClick(object sender, EventArgs eventArgs)
        {
            string idSubAlias = lbExtSubstanceAliases.LbInput.GetFirstSelectedValue();

            if (!ValidationHelper.IsValidInt(idSubAlias)) return;

            var subAlias = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idSubAlias), "SubstanceAlias", null) as Substance_alias_PK;
            PopupSubstanceAlias.ShowModalForm(null, "Substance alias", subAlias, null);
        }

        private void LbExtSubstanceAliases_OnRemoveClick(object sender, EventArgs eventArgs)
        {
            string idSubAlias = lbExtSubstanceAliases.LbInput.GetFirstSelectedValue();

            if (!ValidationHelper.IsValidInt(idSubAlias)) return;

            SSIRep.DeleteObjectByID(Int32.Parse(idSubAlias), "SubstanceAlias", null);
            BindSubstanceTranslations();
        }

        void PopupSubstanceAlias_OnOkButtonClick(object sender, FormDetailsEventArgs e)
        {
            BindSubstanceAliases();
            lbExtSubstanceAliases.LbInput.UnselectAll();
        }

        void PopupSubstanceAlias_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            lbExtSubstanceAliases.LbInput.UnselectAll();
        }

        #endregion

        #region International code

        private void LbExtInternationalCodes_OnAddClick(object sender, EventArgs eventArgs)
        {
            PopupInternationalCode.ShowModalForm("", "International code", null, null);
        }

        private void LbExtInternationalCodes_OnEditClick(object sender, EventArgs eventArgs)
        {
            string idInternationalCode = lbExtInternationalCodes.LbInput.GetFirstSelectedValue();

            if (!ValidationHelper.IsValidInt(idInternationalCode)) return;

            var internationalCode = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idInternationalCode), "InternationalCode", null) as International_code_PK;
            PopupInternationalCode.ShowModalForm(null, "International code", internationalCode, null);
        }

        private void LbExtInternationalCodes_OnRemoveClick(object sender, EventArgs eventArgs)
        {
            string idInternationalCode = lbExtInternationalCodes.LbInput.GetFirstSelectedValue();

            if (!ValidationHelper.IsValidInt(idInternationalCode)) return;

            SSIRep.DeleteObjectByID(Int32.Parse(idInternationalCode), "InternationalCode", null);
            BindInternationalCodes();
        }

        void PopupInternationalCode_OnOkButtonClick(object sender, FormDetailsEventArgs e)
        {
            BindInternationalCodes();
            lbExtInternationalCodes.LbInput.UnselectAll();
        }

        void PopupInternationalCode_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            lbExtInternationalCodes.LbInput.UnselectAll();
        }

        #endregion

        #region Previous EvCode

        private void LbExtPreviousEvCodes_OnAddClick(object sender, EventArgs eventArgs)
        {
            PopupPreviousEvCode.ShowModalForm("", "Previous EVCODE", null, null);
        }

        private void LbExtPreviousEvCodes_OnEditClick(object sender, EventArgs eventArgs)
        {
            string idPreviousEvCode = lbExtPreviousEvCodes.LbInput.GetFirstSelectedValue();

            if (!ValidationHelper.IsValidInt(idPreviousEvCode)) return;

            var previousEvCode = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idPreviousEvCode), "PreviousEvcode", null) as As_previous_ev_code_PK;
            PopupPreviousEvCode.ShowModalForm(null, "Previous EVCODE", previousEvCode, null);
        }

        private void LbExtPreviousEvCodes_OnRemoveClick(object sender, EventArgs eventArgs)
        {
            string idPreviousEvCode = lbExtPreviousEvCodes.LbInput.GetFirstSelectedValue();

            if (!ValidationHelper.IsValidInt(idPreviousEvCode)) return;

            SSIRep.DeleteObjectByID(Int32.Parse(idPreviousEvCode), "PreviousEvcode", null);
            BindPreviousEvCodes();
        }

        void PopupPreviousEvCode_OnOkButtonClick(object sender, FormDetailsEventArgs e)
        {
            BindPreviousEvCodes();
            lbExtPreviousEvCodes.LbInput.UnselectAll();
        }

        void PopupPreviousEvCode_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            lbExtPreviousEvCodes.LbInput.UnselectAll();
        }

        #endregion

        #region Substance attachment

        private void LbExtSubstanceAttachment_OnAddClick(object sender, EventArgs eventArgs)
        {
            PopupSubstanceAttachment.ShowModalForm("", "Substance attachment", null, null);
        }

        private void LbExtSubstanceAttachment_OnEditClick(object sender, EventArgs eventArgs)
        {
            string idSubAttachment = lbExtSubstanceAttachment.LbInput.GetFirstSelectedValue();

            if (!ValidationHelper.IsValidInt(idSubAttachment)) return;

            var subAttachment = SSIRep.GetNotDeletedObjectByID(Int32.Parse(idSubAttachment), "SubstanceAttachment", null) as Substance_attachment_PK;
            PopupSubstanceAttachment.ShowModalForm(null, "Substance attachment", subAttachment, null);
        }

        private void LbExtSubstanceAttachment_OnRemoveClick(object sender, EventArgs eventArgs)
        {
            string idSubAttachment = lbExtSubstanceAttachment.LbInput.GetFirstSelectedValue();

            if (!ValidationHelper.IsValidInt(idSubAttachment)) return;

            SSIRep.DeleteObjectByID(Int32.Parse(idSubAttachment), "SubstanceAttachment", null);
            BindSubstanceAttachments();
        }

        void PopupSubstanceAttachment_OnOkButtonClick(object sender, FormDetailsEventArgs e)
        {
            BindSubstanceAttachments();
            lbExtSubstanceAttachment.LbInput.UnselectAll();
        }

        void PopupSubstanceAttachment_OnCancelButtonClick(object sender, FormDetailsEventArgs e)
        {
            lbExtSubstanceAttachment.LbInput.UnselectAll();
        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContextMenuItems()
        {
            var contextMenu = new[]
            {
                new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), 
                new ContextMenuItem(ContextMenuEventTypes.Save, "Save"), 
            };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);
        }

        private void GenerateTabMenuItems()
        {
            var location = Support.LocationManager.Instance.GetLocationByName("ApprovedSubstance", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            location = Support.LocationManager.Instance.GetLocationByName("ApprovedSubstance", Support.CacheManager.Instance.AppLocations);
            if (location != null)
            {
                var topLevelParent = MasterPage.FindTopLevelParent(location);

                MasterPage.CurrentLocation = location;
                MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
            }
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            Location_PK parentLocation = null;
            var isPermittedInsertApprovedSubstance = false;
            if (EntityContext == EntityContext.ApprovedSubstance)
            {
                parentLocation = Support.LocationManager.Instance.GetLocationByName("ApprovedSubstance", Support.CacheManager.Instance.AppLocations);
                if (FormType == FormType.New) isPermittedInsertApprovedSubstance = SecurityHelper.IsPermitted(Permission.InsertApprovedSubstance, parentLocation);
                else if (FormType == FormType.Edit) isPermittedInsertApprovedSubstance = SecurityHelper.IsPermitted(Permission.EditApprovedSubstance, parentLocation);
            }

            if (isPermittedInsertApprovedSubstance)
            {
                SecurityHelper.SetControlsForReadWrite(
                                MasterPage.ContextMenu,
                                new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") },
                                new List<Panel> { PnlForm },
                                new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } }
                            );
            }
            else
            {
                SecurityHelper.SetControlsForRead(
                                  MasterPage.ContextMenu,
                                  new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") },
                                  new List<Panel> { PnlForm },
                                  new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } }
                              );
            }

            return true;
        }

        #endregion
    }
}