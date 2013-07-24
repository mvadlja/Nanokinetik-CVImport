using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Interface;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Views.AuthorisedProductView
{
    public partial class Preview : PreviewPage
    {
        #region Declarations

        private int? _idAuthProd;
        private bool? _isResponsibleUser;
  
        IAuthorisedProductOperations _authorizedProductOperations;
        IProduct_PKOperations _productOperations;
        IType_PKOperations _typeOperations;
        ICountry_PKOperations _countryOperations;
        IOrganization_PKOperations _organizationOperations;
        IPerson_PKOperations _personOperations;
        IAp_document_mn_PKOperations _authorisedProductDocumentMnOperations;
        IMeddra_pkOperations _meddraOperations;
        IReminder_PKOperations _reminderOperations;
        IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations;
        ILast_change_PKOperations _lastChangeOperations;
        IReminder_date_PKOperations _reminderDateOperations;
        IUSEROperations _userOperations;

        #endregion

        #region Properties

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            LoadFormVariables();
            BindEventHandlers();
            GenerateTabMenuItems();
            GenerateContextMenuItems();
            AssociatePanels(pnlProperties, pnlFooter);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsPostBack)
            {
                BindDynamicControls(null);
                return;
            }

            InitForm(null);
            BindForm(null);
            SetFormControlsDefaults(null);
            SecurityPageSpecific();
        }

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();
            LoadActionQuery();

            _idAuthProd = ValidationHelper.IsValidInt(Request.QueryString["idAuthProd"]) ? int.Parse(Request.QueryString["idAuthProd"]) : (int?)null;

            _authorizedProductOperations = new AuthorisedProductDAL();
            _productOperations = new Product_PKDAL();
            _typeOperations = new Type_PKDAL();
            _countryOperations = new Country_PKDAL();
            _organizationOperations = new Organization_PKDAL();
            _personOperations = new Person_PKDAL();
            _authorisedProductDocumentMnOperations = new Ap_document_mn_PKDAL();
            _meddraOperations = new Meddra_pkDAL();
            _reminderOperations = new Reminder_PKDAL();
            _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _reminderDateOperations = new Reminder_date_PKDAL();
            _userOperations = new USERDAL();
        }

        private void BindEventHandlers()
        {
            lblPrvAuthorisationDate.LnkSetReminder.Click += LnkSetAuthorisationDateReminder_Click;
            lblPrvAuthorisationExpiryDate.LnkSetReminder.Click += LnkSetAuthorisationExpiryDateReminder_Click;
            lblPrvLaunchDate.LnkSetReminder.Click += LnkSetLaunchDateReminder_Click;
            lblPrvWithdrawnDate.LnkSetReminder.Click += LnkSetWithdrawnDateReminder_Click;
            lblPrvSunsetClause.LnkSetReminder.Click += LnkSetSunsetClauseReminder_Click;

            Reminder.OnConfirmInputButtonProcess_Click += Reminder_OnConfirmInputButtonProcess_Click;

            mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
            btnDelete.Click += btnDelete_Click;

            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }
        }

        void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
        }

        #endregion

        #region Fill

        void ClearForm(object arg)
        {

        }

        void FillFormControls(object arg)
        {

        }

        void SetFormControlsDefaults(object arg)
        {
            // Set Delete button visibility rules
            btnDelete.Visible = _idAuthProd.HasValue && _authorizedProductOperations.AbleToDeleteEntity(_idAuthProd.Value);

            if (!_idAuthProd.HasValue) return;

            var rootLocation = AspNetUI.Support.LocationManager.Instance.GetLocationByName("Root", AspNetUI.Support.CacheManager.Instance.AppLocations);
            if (rootLocation == null || !SecurityHelper.IsPermitted(Permission.AuthProdAndProdAdditionalAttributes, rootLocation))
            {
                lblPrvReimbursmentStatus.Visible = false;
                lblPrvReservationConfirmed.Visible = false;
                lblPrvLicenceHolderGroup.Visible = false;
                lblPrvReservedTo.Visible = false;
                lblPrvLocalCodes.Visible = false;
                lblPrvPackSize.Visible = false;
            }

            if (rootLocation == null || !SecurityHelper.IsPermitted(Permission.XevprmShowFormAttributes, rootLocation))
            {
                lblPrvArticle57Reporting.Visible = false;
                lblPrvSubstanceTranslations.Visible = false;
                lblPrvXevprmStatus.Visible = false;
                lblPrvProductGenericName.Visible = false;
                lblPrvProductCompanyName.Visible = false;
                lblPrvProductStrengthName.Visible = false;
                lblPrvProductFormName.Visible = false;
                lblPrvCommentEVPRM.Visible = false;
                lblPrvInfoDate.Visible = false;
            }

            BindDynamicControls(null);
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            if (!_idAuthProd.HasValue) return;

            var authorisedProduct = _authorizedProductOperations.GetEntity(_idAuthProd);
            if (authorisedProduct == null) return;

            // Entity
            lblPrvAuthorisedProduct.Text = authorisedProduct.product_name;

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------
            // Article 57 reporting
            lblPrvArticle57Reporting.Text = authorisedProduct.article_57_reporting.HasValue ? authorisedProduct.article_57_reporting.Value ? "Yes" : "No" : Constant.ControlDefault.LbPrvText;

            // Substance translations
            lblPrvSubstanceTranslations.Text = authorisedProduct.substance_translations.HasValue ? authorisedProduct.substance_translations.Value ? "Yes" : "No" : Constant.ControlDefault.LbPrvText;

            // EVCODE
            lblPrvEvcode.Text = !string.IsNullOrWhiteSpace(authorisedProduct.ev_code) ? authorisedProduct.ev_code : Constant.ControlDefault.LbPrvText;

            // XEVPRM status
            lblPrvXevprmStatus.Text = !string.IsNullOrWhiteSpace(authorisedProduct.XEVPRM_status) ? authorisedProduct.XEVPRM_status : Constant.ControlDefault.LbPrvText;
            
            // Full presentation name
            lblPrvFullPresentationName.Text = !string.IsNullOrWhiteSpace(authorisedProduct.product_name) ? authorisedProduct.product_name : Constant.ControlDefault.LbPrvText;

            // Product short name
            lblPrvProductShortName.Text = !string.IsNullOrWhiteSpace(authorisedProduct.productshortname) ? authorisedProduct.productshortname : Constant.ControlDefault.LbPrvText;

            // Product generic name
            lblPrvProductGenericName.Text = !string.IsNullOrWhiteSpace(authorisedProduct.productgenericname) ? authorisedProduct.productgenericname : Constant.ControlDefault.LbPrvText;

            // Product company name
            lblPrvProductCompanyName.Text = !string.IsNullOrWhiteSpace(authorisedProduct.productcompanyname) ? authorisedProduct.productcompanyname : Constant.ControlDefault.LbPrvText;

            // Product strength name
            lblPrvProductStrengthName.Text = !string.IsNullOrWhiteSpace(authorisedProduct.productstrenght) ? authorisedProduct.productstrenght :  Constant.ControlDefault.LbPrvText;

            // Product form name
            lblPrvProductFormName.Text = !string.IsNullOrWhiteSpace(authorisedProduct.productform) ? authorisedProduct.productform :  Constant.ControlDefault.LbPrvText;

            // Package description
            lblPrvPackageDescription.Text = !string.IsNullOrWhiteSpace(authorisedProduct.packagedesc) ? authorisedProduct.packagedesc :  Constant.ControlDefault.LbPrvText;

            // Comment (EVPRM)
            lblPrvCommentEVPRM.Text = !string.IsNullOrWhiteSpace(authorisedProduct.evprm_comments) ? authorisedProduct.evprm_comments :  Constant.ControlDefault.LbPrvText;

            // Responsible user
            BindResponsibleUser(authorisedProduct.responsible_user_person_FK);

            // Description
            lblPrvDescription.Text = !string.IsNullOrWhiteSpace(authorisedProduct.description) ? authorisedProduct.description :  Constant.ControlDefault.LbPrvText;

            // Authorised product ID
            lblPrvAuthorisedProductID.Text = !string.IsNullOrWhiteSpace(authorisedProduct.product_ID) ? authorisedProduct.product_ID :  Constant.ControlDefault.LbPrvText;

            // Shelf life
            lblPrvShelfLife.Text = !string.IsNullOrWhiteSpace(authorisedProduct.shelflife) ? authorisedProduct.shelflife :  Constant.ControlDefault.LbPrvText;

            // Marketed
            lblPrvMarketed.Text = authorisedProduct.marketed.HasValue ? authorisedProduct.marketed.Value ? "Yes" : "No" : Constant.ControlDefault.LbPrvText;

            // Reimbursment status
            lblPrvReimbursmentStatus.Text = authorisedProduct.reimbursment_status.HasValue ? authorisedProduct.reimbursment_status.Value ? "Yes" : "No" : Constant.ControlDefault.LbPrvText;

            // Reservation confirmed
            lblPrvReservationConfirmed.Text = authorisedProduct.reservation_confirmed.HasValue ? authorisedProduct.reservation_confirmed.Value ? "Yes" : "No" : Constant.ControlDefault.LbPrvText;

            // Legal status
            BindLegalStatus(authorisedProduct.legalstatus);

            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------
            // Licence holder
            BindLicenceHolder(authorisedProduct.organizationmahcode_FK);

            // Licence holder group
            BindLicenceHolderGroup(authorisedProduct.license_holder_group_FK);

            // Local representative 
            BindLocalRepresentative(authorisedProduct.local_representative_FK);

            // QPPV
            lblPrvQppv.Text = PersonHelper.GetQppvNameFormatted(authorisedProduct.qppv_code_FK, Constant.ControlDefault.LbPrvText);

            // Local QPPV
            lblPrvLocalQppv.Text = PersonHelper.GetQppvNameFormatted(authorisedProduct.local_qppv_code_FK, Constant.ControlDefault.LbPrvText);

            // Master File Location
            BindMasterFileLocation(authorisedProduct.mflcode_FK);

            // PhV EMail
            lblPrvPhVEmail.Text = !string.IsNullOrWhiteSpace(authorisedProduct.phv_email) ? authorisedProduct.phv_email :  Constant.ControlDefault.LbPrvText;

            // PhV Phone
            lblPrvPhVPhone.Text = !string.IsNullOrWhiteSpace(authorisedProduct.phv_phone) ? authorisedProduct.phv_phone :  Constant.ControlDefault.LbPrvText;

            // Authorisation country
            BindAuthorisationCountry(authorisedProduct.authorisationcountrycode_FK);

            // Authorisation status
            BindAuthorisationStatus(authorisedProduct.authorisationstatus_FK);

            // Authorisation number
            lblPrvAuthorisationNumber.Text = !string.IsNullOrWhiteSpace(authorisedProduct.authorisationnumber) ? authorisedProduct.authorisationnumber :  Constant.ControlDefault.LbPrvText;

            // Comment
            lblPrvComment.Text = !string.IsNullOrWhiteSpace(authorisedProduct.comment) ? authorisedProduct.comment :  Constant.ControlDefault.LbPrvText;

            // Authorisation date
            lblPrvAuthorisationDate.Text = authorisedProduct.authorisationdate != null && authorisedProduct.authorisationdate.HasValue ? authorisedProduct.authorisationdate.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Authorisation expiry date
            lblPrvAuthorisationExpiryDate.Text = authorisedProduct.authorisationexpdate != null && authorisedProduct.authorisationexpdate.HasValue ? authorisedProduct.authorisationexpdate.Value.ToString(Constant.DateTimeFormat) :  Constant.ControlDefault.LbPrvText;

            // Launch date
            lblPrvLaunchDate.Text = authorisedProduct.launchdate != null && authorisedProduct.launchdate.HasValue ? authorisedProduct.launchdate.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Withdrawn date
            lblPrvWithdrawnDate.Text = authorisedProduct.authorisationwithdrawndate != null && authorisedProduct.authorisationwithdrawndate.HasValue ? authorisedProduct.authorisationwithdrawndate.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Info date
            lblPrvInfoDate.Text = authorisedProduct.infodate != null && authorisedProduct.infodate.HasValue ? authorisedProduct.infodate.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Sunset clause
            lblPrvSunsetClause.Text = authorisedProduct.sunsetclause != null && authorisedProduct.sunsetclause.HasValue ? authorisedProduct.sunsetclause.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Reserved to
            lblPrvReservedTo.Text = !string.IsNullOrWhiteSpace(authorisedProduct.reserved_to) ? authorisedProduct.reserved_to : Constant.ControlDefault.LbPrvText;

            // Local codes
            lblPrvLocalCodes.Text = !string.IsNullOrWhiteSpace(authorisedProduct.local_codes) ? authorisedProduct.local_codes : Constant.ControlDefault.LbPrvText;

            // Pack size
            lblPrvPackSize.Text = !string.IsNullOrWhiteSpace(authorisedProduct.pack_size) ? authorisedProduct.pack_size : Constant.ControlDefault.LbPrvText;

            // Last change
            lblPrvLastChange.Text = LastChange.GetFormattedString(authorisedProduct.ap_PK, "AUTHORISED_PRODUCT", _lastChangeOperations, _personOperations);

            StylizeArticle57RelevantControls(authorisedProduct.article_57_reporting, false);

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if(user != null) _isResponsibleUser = authorisedProduct.responsible_user_person_FK == user.Person_FK;
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idAuthProd.HasValue) return;

            var authorisedProduct = _authorizedProductOperations.GetEntity(_idAuthProd);
            if (authorisedProduct == null) return;

            // Related product
            BindRelatedProductLink(authorisedProduct.product_FK);

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------
            // Distributor Assignments
            BindDistributors(authorisedProduct.ap_PK);

            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------
            // Indications
            BindIndications(authorisedProduct.ap_PK);

            // PPI Attachment
            BindPpiAttachments(authorisedProduct.ap_PK);

            StylizeArticle57RelevantControls(authorisedProduct.article_57_reporting, true);

            BindFooterButtons();

            RefreshReminderStatus();
        }

        private void BindFooterButtons()
        {
            if (SecurityHelper.IsPermitted(Permission.InsertDocument)) btnAddDocument.PostBackUrl = string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=New&idAuthProd={1}&From=AuthProdPreview", EntityContext, _idAuthProd);
            else btnAddDocument.Disable();
        }

        private void BindRelatedProductLink(int? relatedProductPk)
        {
            var relatedProduct = _productOperations.GetEntity(relatedProductPk);
            if (relatedProduct != null && relatedProduct.product_PK.HasValue)
            {
                lblPrvRelatedProductName.ShowLinks = true;
                var pnlLinks = lblPrvRelatedProductName.PnlLinks;

                var hlRelatedProduct = new HyperLink
                {
                    ID = string.Format("Product_{0}", relatedProduct.product_PK),
                    NavigateUrl = string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, relatedProduct.product_PK),
                    Text = relatedProduct.GetNameFormatted()
                };

                pnlLinks.Controls.Add(hlRelatedProduct);
            }
            else
            {
                lblPrvRelatedProductName.Text =  Constant.ControlDefault.LbPrvText;
            }

            pnlProperties.CssClass = "properties padding-0";
        }

        private void BindDistributors(int? authorisedProductPk)
        {
            var distributors = _organizationOperations.GetAssignedDistributorsByAp(authorisedProductPk);

            if (distributors != null && distributors.Count > 0)
            {
                var rowNum = distributors.Count;

                lblPrvDistributors.Text = "";
                foreach (var distributor in distributors)
                {
                    lblPrvDistributors.Text += (--rowNum) != 0 ? distributor.name_org + ", " : distributor.name_org;
                }
            }
            else
            {
                lblPrvDistributors.Text =  Constant.ControlDefault.LbPrvText;
            }
        }

        private void BindIndications(int? authorisedProductPk)
        {
            var indications = _meddraOperations.GetMeddraByAp(authorisedProductPk);
            var formattedText = string.Empty;
            formattedText = indications.Aggregate(formattedText, (current, indication) => indications.Count > 1 ? current + (indication.MeddraFullName + "<br/>") : current + indication.MeddraFullName);

            if (!string.IsNullOrWhiteSpace(formattedText.Trim())) lblPrvIndications.Text = formattedText;
        }

        private void BindPpiAttachments(int? authorisedProductPk)
        {
            var dsPpiAttachments = _authorisedProductDocumentMnOperations.GetAttachmentsByAP(authorisedProductPk);
            var dtPpiAttachments = dsPpiAttachments != null && dsPpiAttachments.Tables.Count > 0 ? dsPpiAttachments.Tables[0] : null;

            if (dtPpiAttachments != null && dtPpiAttachments.Rows.Count > 0 && dtPpiAttachments.Columns.Contains("attachmentname") && dtPpiAttachments.Columns.Contains("attachment_PK"))
            {
                lblPrvPpiAttachment.ShowLinks = true;
                var pnlLinks = lblPrvPpiAttachment.PnlLinks;

                foreach (DataRow dr in dtPpiAttachments.Rows)
                {
                    if (dr["attachment_PK"].ToString() != "")
                    {
                        var hlRelatedProduct = new HyperLink();
                        hlRelatedProduct.ID = "PpiAttachment_" + dr["attachment_PK"];

                        var rootLocation = Support.LocationManager.Instance.GetLocationByName("Root", Support.CacheManager.Instance.AppLocations);
                        if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation)) hlRelatedProduct.NavigateUrl = "~/Views/Business/FileDownload.ashx?attachID=" + dr["attachment_PK"];
                       
                        hlRelatedProduct.Text = dr["attachmentname"].ToString();

                        pnlLinks.Controls.Add(hlRelatedProduct);
                    }
                }
            }
            else
            {
                lblPrvPpiAttachment.Text =  Constant.ControlDefault.LbPrvText;
            }
        }

        private void BindAuthorisationStatus(int? authorisationStatusFk)
        {
            var authorisationStatus = authorisationStatusFk != null ? _typeOperations.GetEntity(authorisationStatusFk) : null;
            lblPrvAuthorisationStatus.Text = authorisationStatus != null && !string.IsNullOrWhiteSpace(authorisationStatus.name) ? authorisationStatus.name :  Constant.ControlDefault.LbPrvText;
        }

        private void BindAuthorisationCountry(int? authorisationCountryFk)
        {
            var authorisationCountry = authorisationCountryFk != null ? _countryOperations.GetEntity(authorisationCountryFk) : null;
            lblPrvAuthorisationCountry.Text = authorisationCountry.GetNameFormatted();
        }

        private void BindMasterFileLocation(int? masterFileLocationFk)
        {
            var masterFileLocation = masterFileLocationFk != null ? _organizationOperations.GetEntity(masterFileLocationFk) : null;
            lblPrvMasterFileLocation.Text = masterFileLocation != null && !string.IsNullOrWhiteSpace(masterFileLocation.name_org) ? masterFileLocation.name_org :  Constant.ControlDefault.LbPrvText;
        }

        private void BindLocalRepresentative(int? localRepresentativeFk)
        {
            var localRepresentative = localRepresentativeFk != null ? _organizationOperations.GetEntity(localRepresentativeFk) : null;
            lblPrvLocalRepresentative.Text = localRepresentative != null && !string.IsNullOrWhiteSpace(localRepresentative.name_org) ? localRepresentative.name_org :  Constant.ControlDefault.LbPrvText;
        }

        private void BindLicenceHolder(int? licenceHolderFk)
        {
            var licenceHolder = licenceHolderFk != null ? _organizationOperations.GetEntity(licenceHolderFk) : null;
            lblPrvLicenceHolder.Text = licenceHolder != null && !string.IsNullOrWhiteSpace(licenceHolder.name_org) ? licenceHolder.name_org :  Constant.ControlDefault.LbPrvText;
        }

        private void BindLicenceHolderGroup(int? licenceHolderGroupFk)
        {
            var licenceHolderGroup = licenceHolderGroupFk != null ? _organizationOperations.GetEntity(licenceHolderGroupFk) : null;
            lblPrvLicenceHolderGroup.Text = licenceHolderGroup != null && !string.IsNullOrWhiteSpace(licenceHolderGroup.name_org) ? licenceHolderGroup.name_org : Constant.ControlDefault.LbPrvText;
        }

        private void BindLegalStatus(string legalStatusFk)
        {
            int _legalStatusFk = -1;
            if (string.IsNullOrWhiteSpace(legalStatusFk) || !Int32.TryParse(legalStatusFk, out _legalStatusFk))
            {
                lblPrvLegalStatus.Text =  Constant.ControlDefault.LbPrvText;
            }
            else
            {
                var legalStatus = _typeOperations.GetEntity(_legalStatusFk);
                lblPrvLegalStatus.Text = legalStatus != null && !string.IsNullOrWhiteSpace(legalStatus.name) ? legalStatus.name :  Constant.ControlDefault.LbPrvText;
            }
        }

        private void BindResponsibleUser(int? responsibleUserFk)
        {
            var responsibleUser = responsibleUserFk != null ? _personOperations.GetEntity(responsibleUserFk) : null;
            lblPrvResponsibleUser.Text = responsibleUser != null && !string.IsNullOrWhiteSpace(responsibleUser.FullName) ? responsibleUser.FullName :  Constant.ControlDefault.LbPrvText;
        }

        #endregion

        #region Validate

        void ValidateForm(object arg)
        {

        }

        #endregion

        #region Save

        void SaveForm(object arg)
        {

        }

        #endregion

        #region Delete

        private void DeleteEntity(int? entityPk)
        {
            if (entityPk.HasValue)
            {
                _authorizedProductOperations.Delete(entityPk);
                Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext));
            } 
           
            MasterPage.ModalPopup.ShowModalPopup("Error!", "Could not delete entity! Contact your system administrator.");
        }

        #endregion

        #endregion

        #region Event handlers

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.Back:
                    {
                        var query = Request.QueryString["idLay"] != null ? string.Format("&idLay={0}", Request.QueryString["idLay"]) : null;

                        if (EntityContext == EntityContext.AuthorisedProduct) Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}{1}", EntityContext.AuthorisedProduct, query));
                        else if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}{1}", EntityContext.Product, query));
                    }
                    break;

                case ContextMenuEventTypes.Edit:
                    {
                        if (EntityContext == EntityContext.AuthorisedProduct && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/AuthorisedProductView/Form.aspx?EntityContext={0}&Action=Edit&idAuthProd={1}", EntityContext.AuthorisedProduct, _idAuthProd));
                    }
                    break;

                case ContextMenuEventTypes.SaveAs:
                    {
                        if (EntityContext == EntityContext.AuthorisedProduct && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/AuthorisedProductView/Form.aspx?EntityContext={0}&Action=SaveAs&idAuthProd={1}", EntityContext.AuthorisedProduct, _idAuthProd));
                    }
                    break;

                case ContextMenuEventTypes.PreviousItem:
                    {
                        if (_idAuthProd.HasValue)
                        {
                            Int32? prevAuthorisedProductPk = _authorizedProductOperations.GetPrevAlphabeticalEntity(_idAuthProd);

                            if (prevAuthorisedProductPk.HasValue)
                            {
                                Response.Redirect(string.Format("~/Views/AuthorisedProductView/Preview.aspx?EntityContext={0}&idAuthProd={1}",EntityContext.AuthorisedProduct, prevAuthorisedProductPk));
                            }
                        }
                        Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext.AuthorisedProduct));
                    }
                    break;

                case ContextMenuEventTypes.NextItem:
                    {
                        if (_idAuthProd.HasValue)
                        {
                            Int32? nextAuthorisedProductPk = _authorizedProductOperations.GetNextAlphabeticalEntity(_idAuthProd);

                            if (nextAuthorisedProductPk.HasValue)
                            {
                                Response.Redirect(string.Format("~/Views/AuthorisedProductView/Preview.aspx?EntityContext={0}&idAuthProd={1}", EntityContext.AuthorisedProduct, nextAuthorisedProductPk));
                            }
                        }
                        Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext.AuthorisedProduct));

                    }
                    break;
            }
        }

        #endregion  

        #region Reminders

        private void LnkSetAuthorisationDateReminder_Click(object sender, EventArgs e)
        {
            SetReminder(StringOperations.GetRelatedName(lblPrvAuthorisationDate.Label), lblPrvAuthorisationDate.Text);
        }

        public void LnkSetAuthorisationExpiryDateReminder_Click(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(lblPrvAuthorisationExpiryDate.Label)), lblPrvAuthorisationExpiryDate.Text);
        }

        public void LnkSetLaunchDateReminder_Click(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(lblPrvLaunchDate.Label)), lblPrvLaunchDate.Text);
        }

        public void LnkSetWithdrawnDateReminder_Click(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(lblPrvWithdrawnDate.Label)), lblPrvWithdrawnDate.Text);
        }

        public void LnkSetSunsetClauseReminder_Click(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(lblPrvSunsetClause.Label)), lblPrvSunsetClause.Text);
        }

        public void Reminder_OnConfirmInputButtonProcess_Click(object sender, EventArgs e)
        {
            Reminder_PK reminder = Reminder.ReminderVs;

            reminder.user_FK = SessionManager.Instance.CurrentUser.UserID;

            reminder.navigate_url = string.Format("~/Views/AuthorisedProductView/Preview.aspx?EntityContext={0}&idAuthProd={1}", EntityContext.AuthorisedProduct, _idAuthProd);
            reminder.TableName = ReminderTableName.AUTHORISED_PRODUCT;
            reminder.entity_FK = _idAuthProd;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;

                reminder = _reminderOperations.Save(reminder);

                if (!reminder.reminder_PK.HasValue) return;

                var reminderEmailRecipientPkList = Reminder.ReminderEmailRecipients.Select(reminderEmailrecipient => new Reminder_email_recipient_PK(null, reminder.reminder_PK, reminderEmailrecipient)).ToList();

                AlerterHelper.SaveEmailRecipients(reminderEmailRecipientPkList, reminder, auditTrailSessionToken);
                AlerterHelper.SaveReminderDates(reminder, auditTrailSessionToken);

                ts.Complete();
            }

            RefreshReminderStatus();
        }

        #endregion

        #region Delete

        private void btnDelete_Click(object sender, EventArgs eventArgs)
        {
            mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_idAuthProd);
        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContextMenuItems()
        {
            var contextMenu = new[]
            {
                new ContextMenuItem(ContextMenuEventTypes.Back, "Back"), 
                new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit"), 
                new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As"),
                new ContextMenuItem(ContextMenuEventTypes.PreviousItem, "Previous"),
                new ContextMenuItem(ContextMenuEventTypes.NextItem, "Next")
            };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);

            if (SecurityHelper.IsPermitted(Permission.View))
            {
                int? nextId = null;
                int? prevId = null;

                if (_idAuthProd.HasValue)
                {
                    nextId = _authorizedProductOperations.GetNextAlphabeticalEntity(_idAuthProd);
                    prevId = _authorizedProductOperations.GetPrevAlphabeticalEntity(_idAuthProd);
                }

                if (prevId == null) MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.PreviousItem, "Previous") });
                if (nextId == null) MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.NextItem, "Next") });
            }
            else MasterPage.ContextMenu.SetContextMenuItemsDisabled(
                new[] { 
                        new ContextMenuItem(ContextMenuEventTypes.NextItem, "Next"), 
                        new ContextMenuItem(ContextMenuEventTypes.PreviousItem, "Previous") 
                      });
        }

        private void GenerateTabMenuItems()
        {
            tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, MasterPage.CurrentLocation);
            tabMenu.SelectItem(MasterPage.CurrentLocation, Support.CacheManager.Instance.AppLocations);
        }

        public void RefreshReminderStatus()
        {
            string tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.AUTHORISED_PRODUCT);
            AlerterHelper.RefreshReminderStatus(_reminderOperations, MasterPage, new List<IReminderCustomControl> { lblPrvAuthorisationDate, lblPrvWithdrawnDate, lblPrvLaunchDate, lblPrvAuthorisationExpiryDate, lblPrvSunsetClause }, tableName, _idAuthProd);
        }

        public void SetReminder(String attributeName, String attributeValue)
        {
            var reminder = new Reminder_PK
                               {
                                   reminder_type = "Authorised product",
                                   reminder_name = lblPrvFullPresentationName.Text,
                                   related_attribute_name = attributeName,
                                   related_attribute_value = attributeValue
                               };

            Reminder.ReminderVs = reminder;
            Reminder.ShowModalPopup("Set alert");
            RefreshReminderStatus();
        }

        private void StylizeArticle57RelevantControls(bool? isArticle57Relevant, bool stylizeDynamicControls)
        {
            if (!stylizeDynamicControls)
            {
                // Left pane
                lblPrvEvcode.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvEvcode.Text, isArticle57Relevant));
                lblPrvFullPresentationName.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvFullPresentationName.Text, isArticle57Relevant));
                lblPrvProductShortName.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvProductShortName.Text, isArticle57Relevant));
                lblPrvProductGenericName.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvProductGenericName.Text, isArticle57Relevant));
                lblPrvProductCompanyName.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvProductCompanyName.Text, isArticle57Relevant));
                lblPrvProductStrengthName.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvProductStrengthName.Text, isArticle57Relevant));
                lblPrvProductFormName.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvProductFormName.Text, isArticle57Relevant));
                lblPrvPackageDescription.LblName.AddCssClass(Article57Reporting.GetCssClass(false, false, lblPrvPackageDescription.Text, isArticle57Relevant));
                lblPrvCommentEVPRM.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvCommentEVPRM.Text, isArticle57Relevant));

                // Right pane
                lblPrvLicenceHolder.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvLicenceHolder.Text, isArticle57Relevant));
                lblPrvQppv.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvQppv.Text, isArticle57Relevant));
                // Local qppv is not stylized because there was no request for it
                lblPrvMasterFileLocation.LblName.AddCssClass(Article57Reporting.GetCssClass(false, false, lblPrvMasterFileLocation.Text, isArticle57Relevant));
                lblPrvPhVEmail.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvPhVEmail.Text, isArticle57Relevant));
                lblPrvPhVPhone.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvPhVPhone.Text, isArticle57Relevant));
                lblPrvAuthorisationCountry.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvAuthorisationCountry.Text, isArticle57Relevant));
                lblPrvAuthorisationStatus.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvAuthorisationStatus.Text, isArticle57Relevant));
                lblPrvAuthorisationNumber.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvAuthorisationNumber.Text, isArticle57Relevant));
                lblPrvAuthorisationDate.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvAuthorisationDate.Text, isArticle57Relevant));
                StylizeWithdrawnDate();
                lblPrvInfoDate.LblName.AddCssClass(Article57Reporting.GetCssClass(false, false, lblPrvInfoDate.Text, isArticle57Relevant));
            }
            else
            {
                // Right pane
                lblPrvIndications.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvIndications.Text, isArticle57Relevant));
                lblPrvPpiAttachment.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvPpiAttachment.IsEmpty, isArticle57Relevant));
            }
        }

        private void StylizeWithdrawnDate()
        {
            lblPrvWithdrawnDate.LblName.RemoveCssClassContains("article57");
            var isArticle57Relevant = lblPrvArticle57Reporting.Text.ToLower() == "yes"; 
            if (isArticle57Relevant && (lblPrvAuthorisationStatus.Text.ToLower().Contains("not valid") || lblPrvAuthorisationStatus.Text.ToLower().Contains("suspended")))
            {
                lblPrvWithdrawnDate.LblName.AddCssClass(Article57Reporting.GetCssClass(true, true, lblPrvWithdrawnDate.Text, true));
            }
            else
            {
                lblPrvWithdrawnDate.LblName.AddCssClass(Article57Reporting.GetCssClass(true, false, lblPrvWithdrawnDate.Text, isArticle57Relevant));
            }
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            if (!base.SecurityPageSpecific())
            {
                if (SecurityHelper.IsPermitted(Permission.SaveAsAuthorisedProduct)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] {new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As")});
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] {new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As")});

                if (SecurityHelper.IsPermitted(Permission.EditAuthorisedProduct)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] {new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit")});
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] {new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit")});

                if (SecurityHelper.IsPermitted(Permission.DeleteAuthorisedProduct)) StyleHelper.EnableLinkButtonsWithCssClass(PnlFooter, "Delete");
                else StyleHelper.DisableLinkButtonsWithCssClass(PnlFooter, "Delete");

                SecurityPageSpecificMy(_isResponsibleUser);
            }

            return true;
        }

        #endregion
    }
}