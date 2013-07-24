using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using AspNetUI.NanokinetikEDMS;
using AspNetUI.Views.Shared.Interface;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUI.Views.Shared.UserControl.Popup;
using EVMessage.Xevprm;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;
using System.Web.UI.WebControls;
using CacheManager = AspNetUI.Support.CacheManager;
using LocationManager = AspNetUI.Support.LocationManager;

namespace AspNetUI.Views.DocumentView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idAct;
        private int? _idProd;
        private int? _idAuthProd;
        private int? _idTask;
        private int? _idProj;
        private int? _idDoc;
        private int? _idPharmProd;
        private int? _idSubUnit;
        private int? _idTimeUnit;
        private bool? _isResponsibleUser;
        private List<Attachment_PK> _uploadedAttachments;
        private string _executedCheckAction;
        private int? _executedOnAttachmentPk;

        private IAuthorisedProductOperations _authorisedProductOperations;
        private IProduct_PKOperations _productOperations;
        private IPerson_PKOperations _personOperations;
        private IType_PKOperations _typeOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private IReminder_PKOperations _reminderOperations;
        private IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations;
        private IUSEROperations _userOperations;
        private IReminder_date_PKOperations _reminderDateOperations;
        private IDocument_PKOperations _documentOperations;
        private IAttachment_PKOperations _attachmentOperations;
        private IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        private IProject_PKOperations _projectOperations;
        private ITask_PKOperations _taskOperations;
        private IActivity_PKOperations _activityOperations;
        private ITask_name_PKOperations _taskNameOperations;
        private ILanguagecode_PKOperations _languageCodeOperations;
        private IAp_document_mn_PKOperations _authorisedProductDocumentMnOperations;
        private IProduct_document_mn_PKOperations _productDocumentMnOperations;
        private IActivity_document_PKOperations _activityDocumentMnOperations;
        private ITask_document_PKOperations _taskDocumentMnOperations;
        private IProject_document_PKOperations _projectDocumentMnOperations;
        private IPp_document_PKOperations _pharmaceuticalProductDocumentMnOperations;
        private IDocument_language_mn_PKOperations _documentLanguageMnOperations;

        #endregion

        #region Properties

        public SearchType SearchType
        {
            get { return ViewState["SearchType"] != null ? (SearchType)ViewState["SearchType"] : SearchType.Null; }
            set { ViewState["SearchType"] = value; }
        }

        private List<int?> _attachmentPkListToDelete
        {
            get
            {
                var _tmpAttachmentPkListToDelete = new List<int?>();
                if (ViewState["_attachmentPkListToDelete"] != null) _tmpAttachmentPkListToDelete = (List<int?>)ViewState["_attachmentPkListToDelete"];
                else ViewState["_attachmentPkListToDelete"] = _tmpAttachmentPkListToDelete;

                return _tmpAttachmentPkListToDelete;
            }
        }

        private int? _attachmentPkToDelete
        {
            get { return (int?)ViewState["_attachmentPkToDelete"]; }
            set { ViewState["_attachmentPkToDelete"] = value; }
        }

        private string _selectedPrevDocumentType
        {
            get { return (string)ViewState["_selectedPrevDocumentType"]; }
            set { ViewState["_selectedPrevDocumentType"] = value; }
        }

        private bool? _editingPPIDocument
        {
            get { return ViewState["_editingPPIDocument"] != null ? (bool?)ViewState["_editingPPIDocument"] : null; }
            set { ViewState["_editingPPIDocument"] = value; }
        }

        private bool? _isEDMSDocument
        {
            get { return ViewState["_isEDMSDocument"] != null ? (bool?)ViewState["_isEDMSDocument"] : null; }
            set { ViewState["_isEDMSDocument"] = value; }
        }

        private List<int?> _EDMSattachmentPkList
        {
            get
            {
                var _tmpEDMSattachmentPkList = new List<int?>();
                if (ViewState["_EDMSattachmentPkList"] != null) _tmpEDMSattachmentPkList = (List<int?>)ViewState["_EDMSattachmentPkList"];
                else ViewState["_EDMSattachmentPkList"] = _tmpEDMSattachmentPkList;

                return _tmpEDMSattachmentPkList;
            }
            set { ViewState["_EDMSattachmentPkList"] = value; }
        }

        private EDMSDocument _EDMSAttachment
        {
            get { return ViewState["_EDMSAttachment"] != null ? (EDMSDocument)ViewState["_EDMSAttachment"] : null; }
            set { ViewState["_EDMSAttachment"] = value; }
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

            if (IsPostBack)
            {
                BindDynamicControls(null);
                return;
            }

            InitForm(null);

            if (FormType == FormType.Edit || FormType == FormType.SaveAs)
            {
                BindForm(null);
            }

            SetFormControlsDefaults(null);
            SecurityPageSpecific();
        }

        private void BindDynamicControls(object o)
        {
            lbSrRelatedEntities.SearchType = SearchType;

            if (SearchType != SearchType.Null)
            {
                if (SearchType == SearchType.Product) lbSrRelatedEntities.Label = "Products:";
                else if (SearchType == SearchType.AuthorisedProduct) lbSrRelatedEntities.Label = "Authorised products:";
                else if (SearchType == SearchType.PharmaceuticalProduct) lbSrRelatedEntities.Label = "Pharmaceutical products:";
                else if (SearchType == SearchType.Activity) lbSrRelatedEntities.Label = "Activities:";
                else if (SearchType == SearchType.Task) lbSrRelatedEntities.Label = "Tasks:";
                else if (SearchType == SearchType.Project) lbSrRelatedEntities.Label = "Projects:";

                if (SearchType != SearchType.AuthorisedProduct)
                {
                    ddlDocumentType.AutoPostback = true;
                    var documentTypePpi = ddlDocumentType.DdlInput.Items.FindByText("PPI") ?? ddlDocumentType.DdlInput.Items.FindByText("ppi");
                    if (documentTypePpi != null) ddlDocumentType.DdlInput.Items.Remove(documentTypePpi);
                    ddlDocumentType.DdlInput.SelectedIndexChanged += ddlDocumentType_SelectedIndexChanged;
                }
                else
                {
                    ddlDocumentType.AutoPostback = true;
                    ddlDocumentType.DdlInput.SelectedIndexChanged += ddlDocumentType_SelectedIndexChanged;
                }
            }
            else
            {
                lbSrRelatedEntities.Label = "Related entity:";
            }

            var attachmentsForThisSession = new List<Attachment_PK>();

            if (FormType == FormType.SaveAs) _idDoc = null;

            attachmentsForThisSession = Attachment_PK.GetAttachmentsForThisSession(_attachmentOperations, _idDoc, upAttachments.AttachmentSessionId);

            if (!IsPostBack)
            {
                if (attachmentsForThisSession != null && attachmentsForThisSession.Any())
                {
                    upAttachments.AttachmentIdOldValue = attachmentsForThisSession.Select(a => a.attachment_PK != null ? a.attachment_PK.Value : 0).ToList();
                }
            }

            var refAttachmentPkListToDelete = _attachmentPkListToDelete;
            attachmentsForThisSession = attachmentsForThisSession.Where(a => a.attachment_PK != null && !refAttachmentPkListToDelete.Contains(a.attachment_PK.Value)).ToList();
            _EDMSattachmentPkList = attachmentsForThisSession.Where(a => a.EDMSDocumentId != null).Select(a => a.attachment_PK).ToList();

            upAttachments.NumberOfUploadedAttachments = attachmentsForThisSession.Count;
            if (attachmentsForThisSession.Count > 0)
            {
                upAttachments.AsyncFileUpload.CssClass = "async-upload-control-items";
                upAttachments.DivHrHolder.Visible = true;
                upAttachments.RefreshAttachments = false;
                BindAttachments(attachmentsForThisSession);
            }
            else
            {
                upAttachments.DivHrHolder.Visible = false;
                upAttachments.AsyncFileUpload.CssClass = "async-upload-control-no-items";
            }

            HandleDocumentType();
            RefreshReminderStatus();
        }

        private void SetEDMSSecurityPermission()
        {
            var rootLocation = LocationManager.Instance.GetLocationByName("Root", CacheManager.Instance.AppLocations);
            if (rootLocation != null && SecurityHelper.IsPermitted(Permission.LinkFromEDMS, rootLocation))
            {
                btnBrowseEDMSUC.Click += btnBrowseEDMSUC_OnClick;
                BrowseEDMSPopup.OnOkButtonClick += BrowseEDMSPopup_OnOkButtonClick;
            }
            else
            {
                btnBrowseEDMSUC.Click -= btnBrowseEDMSUC_OnClick;
                BrowseEDMSPopup.OnOkButtonClick -= BrowseEDMSPopup_OnOkButtonClick;
            }
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

            Page.Form.Enctype = "multipart/form-data";

            _idDoc = ValidationHelper.IsValidInt(Request.QueryString["idDoc"]) ? int.Parse(Request.QueryString["idDoc"]) : (int?)null;
            _idAuthProd = ValidationHelper.IsValidInt(Request.QueryString["idAuthProd"]) ? int.Parse(Request.QueryString["idAuthProd"]) : (int?)null;
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;
            _idPharmProd = ValidationHelper.IsValidInt(Request.QueryString["idPharmProd"]) ? int.Parse(Request.QueryString["idPharmProd"]) : (int?)null;
            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;
            _idTask = ValidationHelper.IsValidInt(Request.QueryString["idTask"]) ? int.Parse(Request.QueryString["idTask"]) : (int?)null;
            _idProj = ValidationHelper.IsValidInt(Request.QueryString["idProj"]) ? int.Parse(Request.QueryString["idProj"]) : (int?)null;
            _idSubUnit = ValidationHelper.IsValidInt(Request.QueryString["idSubUnit"]) ? int.Parse(Request.QueryString["idSubUnit"]) : (int?)null;
            _idTimeUnit = ValidationHelper.IsValidInt(Request.QueryString["idTimeUnit"]) ? int.Parse(Request.QueryString["idTimeUnit"]) : (int?)null;

            _uploadedAttachments = new List<Attachment_PK>();

            _authorisedProductOperations = new AuthorisedProductDAL();
            _productOperations = new Product_PKDAL();
            _personOperations = new Person_PKDAL();
            _typeOperations = new Type_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _reminderOperations = new Reminder_PKDAL();
            _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            _userOperations = new USERDAL();
            _reminderDateOperations = new Reminder_date_PKDAL();
            _languageCodeOperations = new Languagecode_PKDAL();
            _documentOperations = new Document_PKDAL();
            _attachmentOperations = new Attachment_PKDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _projectOperations = new Project_PKDAL();
            _taskOperations = new Task_PKDAL();
            _activityOperations = new Activity_PKDAL();
            _taskNameOperations = new Task_name_PKDAL();
            _authorisedProductDocumentMnOperations = new Ap_document_mn_PKDAL();
            _productDocumentMnOperations = new Product_document_mn_PKDAL();
            _pharmaceuticalProductDocumentMnOperations = new Pp_document_PKDAL();
            _taskDocumentMnOperations = new Task_document_PKDAL();
            _projectDocumentMnOperations = new Project_document_PKDAL();
            _activityDocumentMnOperations = new Activity_document_PKDAL();
            _documentLanguageMnOperations = new Document_language_mn_PKDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            lbSrRelatedEntities.Searcher.OnOkButtonClick += LbSrRelatedEntities_OnOkButtonClick;

            dtEffectiveStartDate.LnkSetReminder.Click += LnkSetEffectiveStartDateReminder_Click;
            dtEffectiveEndDate.LnkSetReminder.Click += LnkSetEffectiveEndDateReminder_Click;

            Reminder.OnConfirmInputButtonProcess_Click += Reminder_OnConfirmInputButtonProcess_Click;

            upAttachments.GvData.RowDataBound += GvData_RowDataBound;
            upAttachments.GvData.RowCommand += GvData_RowCommand;
            upAttachments.AsyncFileUpload.UploadedComplete += AsyncFileUpload_UploadedComplete;
            upAttachments.OnDelete += upAttachments_OnDelete;

            mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
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
            // Entity name
            lblPrvParentEntity.Text = Constant.ControlDefault.LbPrvText;

            // Left pane
            lbSrRelatedEntities.Clear();
            txtDocumentName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            ddlDocumentType.SelectedValue = String.Empty;
            ddlVersionNumber.SelectedValue = String.Empty;
            ddlResponsibleUser.SelectedValue = String.Empty;
            ddlVersionLabel.SelectedValue = String.Empty;
            txtDocumentNumber.Text = String.Empty;
            ddlRegulatoryStatus.SelectedValue = String.Empty;
            txtComment.Text = string.Empty;
            lbLanguageCodes.Clear();
            dtChangeDate.Text = string.Empty;
            dtEffectiveStartDate.Text = string.Empty;
            dtEffectiveEndDate.Text = string.Empty;
            dtVersionDate.Text = string.Empty;
            txtEvcode.Text = string.Empty;
        }

        private void FillFormControls(object arg)
        {
            FillDdlDocumentType(null);
            FillDdlVersionNumber(null);
            FillDdlResponsibleUsers(null);
            FillDdlVersionLabel(null);
            FillDdlRegulatoryStatus(null);
            FillDdlAttachmentType(null);
            FillLbLanguageCodes(null);
        }

        private void FillDdlDocumentType(object arg)
        {
            var documentTypeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.DocumentType);

            ddlDocumentType.Fill(documentTypeList, x => x.name, x => x.type_PK);
            ddlDocumentType.SortItemsByText();

            var documentTypeNa = documentTypeList.Find(dt => dt.name.ToLower() == "n/a");
            if (documentTypeNa != null) ddlDocumentType.SelectedId = documentTypeNa.type_PK;

            if (string.IsNullOrEmpty(_selectedPrevDocumentType)) _selectedPrevDocumentType = ddlDocumentType.Text;
        }

        private void FillDdlVersionNumber(object arg)
        {
            if (arg is string && (arg as string) == "PPI")
            {
                var versionNumberList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.PPIVersionNumber);
                ddlVersionNumber.Fill(versionNumberList, x => x.name, x => x.type_PK, false);
                ddlVersionNumber.SortItemsByText(SortType.Asc, false, false);
            }
            else
            {
                var versionNumberList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.VersionNumber);
                var versionNumberNADs = versionNumberList.Find(t => t.name == "N/A");
                if (versionNumberNADs != null) versionNumberList.Remove(versionNumberNADs);

                ddlVersionNumber.Fill(versionNumberList, x => x.name, x => x.type_PK);
                ddlVersionNumber.SortItemsByText();

                if (versionNumberNADs != null) ddlVersionNumber.DdlInput.Items.Insert(1, new ListItem(versionNumberNADs.name, versionNumberNADs.type_PK.ToString()));

                var versionNumberNA = ddlVersionNumber.DdlInput.Items.FindByText("N/A");
                if (versionNumberNA != null) ddlVersionNumber.SelectedValue = versionNumberNA.Value;
            }
        }

        private void FillDdlVersionLabel(object arg)
        {
            var versionLabelList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.VersionLabel);
            ddlVersionLabel.Fill(versionLabelList, x => x.name, x => x.type_PK);
            ddlVersionLabel.SortItemsByText();

            if (arg is string)
            {
                var selectedItem = ddlVersionLabel.DdlInput.Items.FindByText(arg as string);
                if (selectedItem != null) ddlVersionLabel.SelectedValue = selectedItem.Value;
            }
        }

        private void FillDdlAttachmentType(object arg)
        {
            var attachmentTypeList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.AttachmentType);

            var isEDMSDocument = _isEDMSDocument.HasValue && _isEDMSDocument.Value;
            if (isEDMSDocument)
            {
                var attachmentTypeEDMSList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.AttachmentTypeEDMS);
                attachmentTypeList.AddRange(attachmentTypeEDMSList);
            }

            ddlAttachmentType.Fill(attachmentTypeList, x => x.name, x => x.type_PK, false);
            ddlAttachmentType.SortItemsByText(SortType.Asc, false, false);

            int selectedValue = -1;
            Int32.TryParse(arg as string, NumberStyles.None, CultureInfoHr, out selectedValue);
            if (selectedValue != -1)
            {
                ddlAttachmentType.SelectedValue = selectedValue;
            }
            else if (arg is string)
            {
                var selectedItem = ddlAttachmentType.DdlInput.Items.FindByText(arg as string);
                if (selectedItem != null) ddlAttachmentType.SelectedValue = selectedItem.Value;
            }
        }

        private void FillDdlRegulatoryStatus(object arg)
        {
            if (arg is string && (arg as string) == "PPI")
            {
                var regulatoryStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.PPIRegulatoryStatusDocuments);
                ddlRegulatoryStatus.Fill(regulatoryStatusList, x => x.name, x => x.type_PK, false);
                ddlRegulatoryStatus.SortItemsByText(SortType.Asc, false, false);
            }
            else
            {
                var regulatoryStatusList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.RegulatoryStatusDocuments);
                ddlRegulatoryStatus.Fill(regulatoryStatusList, x => x.name, x => x.type_PK);
                ddlRegulatoryStatus.SortItemsByText();
            }
        }

        private void FillLbLanguageCodes(object arg)
        {
            var languageCodeList = _languageCodeOperations.GetEntities();
            lbLanguageCodes.Fill(languageCodeList, "code", "languagecode_PK");
            lbLanguageCodes.LbInput.SortItemsByText();
        }

        private void FillEDMSFields(Tuple<EDMSDocument, EDMSDocumentVersion> edmsContainer)
        {
            var edmsDocument = edmsContainer.Item1;
            var edmsDocumentVersion = edmsContainer.Item2;

            txtDocumentName.Text = edmsDocument.documentName;
            ddlVersionNumber.SelectedText = edmsDocument.versionNumber;
            ddlVersionLabel.SelectedText = edmsDocument.versionLabel.ToUpper();
            ddlAttachmentType.SelectedText = edmsDocument.format.ToUpper();
            ddlRegulatoryStatus.SelectedText = Constant.UnknownValue;
            dtEDMSModifyDate.Text = edmsDocument.modifyDate.ToString(Constant.DateTimeFormat);
            txtEDMSBindingRule.Text = edmsDocumentVersion == EDMSDocumentVersion.Current ? "Current" : "Selected version";
        }

        private void SetFormControlsDefaults(object arg)
        {
            switch (EntityContext)
            {
                case EntityContext.Product:
                    {
                        lblPrvParentEntity.Label = "Product:";
                        var product = _productOperations.GetEntity(_idProd);
                        if (product != null && !string.IsNullOrWhiteSpace(product.name)) lblPrvParentEntity.Text = product.name;
                        else lblPrvParentEntity.Text = Constant.DefaultEmptyValue;
                    }
                    break;

                case EntityContext.AuthorisedProduct:
                    {
                        lblPrvParentEntity.Label = "Authorised product:";
                        var authorisedProduct = _authorisedProductOperations.GetEntity(_idAuthProd);
                        if (authorisedProduct != null && !string.IsNullOrWhiteSpace(authorisedProduct.product_name)) lblPrvParentEntity.Text = authorisedProduct.product_name;
                        else lblPrvParentEntity.Text = Constant.DefaultEmptyValue;
                    }
                    break;

                case EntityContext.PharmaceuticalProduct:
                    {
                        lblPrvParentEntity.Label = "Pharmaceutical product:";
                        var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(_idPharmProd);
                        if (pharmaceuticalProduct != null && !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name)) lblPrvParentEntity.Text = pharmaceuticalProduct.name;
                        else lblPrvParentEntity.Text = Constant.DefaultEmptyValue;
                    }
                    break;

                case EntityContext.Project:
                    {
                        lblPrvParentEntity.Label = "Project:";
                        var project = _projectOperations.GetEntity(_idProj);
                        if (project != null && !string.IsNullOrWhiteSpace(project.name)) lblPrvParentEntity.Text = project.name;
                        else lblPrvParentEntity.Text = Constant.DefaultEmptyValue;
                    }
                    break;

                case EntityContext.ActivityMy:
                case EntityContext.Activity:
                    {
                        lblPrvParentEntity.Label = "Activity:";
                        var activity = _activityOperations.GetEntity(_idAct);
                        if (activity != null && !string.IsNullOrWhiteSpace(activity.name)) lblPrvParentEntity.Text = activity.name;
                        else lblPrvParentEntity.Text = Constant.DefaultEmptyValue;
                    }
                    break;

                case EntityContext.Task:
                    {
                        lblPrvParentEntity.Label = "Task:";
                        var task = _taskOperations.GetEntity(_idTask);
                        if (task != null)
                        {
                            var taskName = _taskNameOperations.GetEntity(task.task_name_FK);
                            if (taskName != null && !string.IsNullOrWhiteSpace(taskName.task_name)) lblPrvParentEntity.Text = taskName.task_name;
                            else lblPrvParentEntity.Text = Constant.DefaultEmptyValue;
                        }
                        else lblPrvParentEntity.Text = Constant.DefaultEmptyValue;
                    }
                    break;
            }

            if (FormType == FormType.New || FormType == FormType.SaveAs)
            {
                var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                ddlResponsibleUser.SelectedValue = user != null ? user.Person_FK : null;

                HideReminders();
            }

            if (FormType == FormType.New)
            {
                if (EntityContext != EntityContext.Document)
                {
                    BindRelatedEntity(null);
                }

                dtChangeDate.Text = DateTime.Now.ToString(Constant.DateTimeFormat);
            }

            if (FormType == FormType.SaveAs && _idDoc.HasValue)
            {
                var attachmentsInDb = _attachmentOperations.GetAttachmentsForDocumentWithDiskFile(_idDoc.Value);
                var saveAsAttachmentList = new List<Attachment_PK>();
                saveAsAttachmentList.AddRange(attachmentsInDb);
                saveAsAttachmentList.ForEach(a =>
                {
                    a.attachment_PK = null;
                    a.session_id = upAttachments.AttachmentSessionId;
                    a.document_FK = null;
                });

                _attachmentOperations.SaveCollection(saveAsAttachmentList);
            }

            BindDynamicControls(null);
        }

        private void BindRelatedEntity(object o)
        {
            var text = Constant.MissingValue;
            Int32? value = null;
            switch (EntityContext)
            {
                case EntityContext.Product:
                    {
                        var product = _productOperations.GetEntity(_idProd);
                        text = product.GetNameFormatted();
                        value = _idProd;
                        lbSrRelatedEntities.Label = "Products:";
                        SearchType = SearchType.Product;
                    }
                    break;
                case EntityContext.AuthorisedProduct:
                    {
                        var authorisedProduct = _authorisedProductOperations.GetEntity(_idAuthProd);
                        text = !string.IsNullOrWhiteSpace(authorisedProduct.product_name) ? authorisedProduct.product_name : Constant.MissingValue;
                        value = _idAuthProd;
                        lbSrRelatedEntities.Label = "Authorised products:";
                        SearchType = SearchType.AuthorisedProduct;
                    }
                    break;
                case EntityContext.PharmaceuticalProduct:
                    {
                        var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(_idPharmProd);
                        text = !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.MissingValue;
                        value = _idPharmProd;
                        lbSrRelatedEntities.Label = "Pharmaceutical products:";
                        SearchType = SearchType.PharmaceuticalProduct;
                    }
                    break;
                case EntityContext.ActivityMy:
                case EntityContext.Activity:
                    {
                        var activity = _activityOperations.GetEntity(_idAct);
                        text = !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.MissingValue;
                        value = _idAct;
                        lbSrRelatedEntities.Label = "Activities:";
                        SearchType = SearchType.Activity;
                    }
                    break;
                case EntityContext.Project:
                    {
                        var project = _projectOperations.GetEntity(_idProj);
                        text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.MissingValue;
                        value = _idProj;
                        lbSrRelatedEntities.Label = "Projects:";
                        SearchType = SearchType.Project;
                    }
                    break;
                case EntityContext.Task:
                    {
                        var task = _taskOperations.GetEntity(_idTask);
                        if (task != null)
                        {
                            var taskName = _taskNameOperations.GetEntity(task.task_name_FK);
                            text = taskName != null && !string.IsNullOrWhiteSpace(taskName.task_name) ? taskName.task_name : Constant.MissingValue;
                            value = _idTask;
                            lbSrRelatedEntities.Label = "Tasks:";
                            SearchType = SearchType.Task;
                        }
                        else text = Constant.MissingValue;
                    }
                    break;
                case EntityContext.Document:
                    lbSrRelatedEntities.Label = "Related entity:";
                    break;
            }

            lbSrRelatedEntities.LbInput.Items.Add(new ListItem(text, value.ToString()));
        }

        /// <summary>
        /// Binds responsible users drop down list
        /// </summary>
        /// <param name="args"></param>
        private void FillDdlResponsibleUsers(object args)
        {
            var responsibleUserList = _personOperations.GetPersonsByRole(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, r => r.FullName, r => r.person_PK);
            ddlResponsibleUser.SortItemsByText();
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idDoc.HasValue) return;

            var document = _documentOperations.GetEntity(_idDoc);

            if (document == null || !document.document_PK.HasValue) return;

            if (EntityContext == EntityContext.Document)
            {
                lblPrvParentEntity.Text = !string.IsNullOrWhiteSpace(document.name) ? document.name : Constant.ControlDefault.LbPrvText;
                lblPrvParentEntity.Label = "Document:";
            }

            BindRelatedEntities(document.document_PK);

            // Document name
            txtDocumentName.Text = document.name;

            // Description
            txtDescription.Text = document.description;

            // Document type
            BindDocumentType(document.type_FK);

            var documentType = ddlDocumentType.SelectedItem;
            if ((SearchType == SearchType.AuthorisedProduct || SearchType == SearchType.Null) && documentType != null && documentType.Text.ToLower() == "ppi")
            {
                FillDdlAttachmentType("DOC");
                FillDdlRegulatoryStatus("PPI");
                FillDdlVersionLabel("N/A");
                FillDdlVersionNumber("PPI");
                _selectedPrevDocumentType = documentType.Text;
                _editingPPIDocument = true;
            }
            //else if (documentType != null && documentType.Text.Trim().ToLower() == "edms document")
            //{
            //    FillDdlAttachmentType(null);
            //    _editingPPIDocument = false;
            //}
            else
            {
                _editingPPIDocument = false;
            }

            // Version number
            BindVersionNumber(document.version_number);

            // Responsible user
            BindResponsibleUser(document.person_FK);

            // Version label
            BindVersionLabel(document.version_label);

            // Document number
            txtDocumentNumber.Text = document.document_code;

            // Regulatory status
            BindRegulatoryStatus(document.regulatory_status);

            // Comment 
            txtComment.Text = document.comment;

            // Attachment type
            BindAttachmentType(document.attachment_type_FK);

            // Language code
            BindLanguageCode(document.document_PK);

            // Binding rule
            txtEDMSBindingRule.Text = document.EDMSBindingRule;

            // Change date
            dtChangeDate.Text = document.change_date.HasValue ? ((DateTime)document.change_date).ToString(Constant.DateTimeFormat) : String.Empty;

            // Effective start date
            dtEffectiveStartDate.Text = document.effective_start_date.HasValue ? ((DateTime)document.effective_start_date).ToString(Constant.DateTimeFormat) : String.Empty;

            // Effective end date
            dtEffectiveEndDate.Text = document.effective_end_date.HasValue ? ((DateTime)document.effective_end_date).ToString(Constant.DateTimeFormat) : String.Empty;

            // EDMS modify date
            dtEDMSModifyDate.Text = document.EDMSModifyDate.HasValue ? ((DateTime)document.EDMSModifyDate).ToString(Constant.DateTimeFormat) : String.Empty;

            // Version date
            dtVersionDate.Text = document.version_date.HasValue ? ((DateTime)document.version_date).ToString(Constant.DateTimeFormat) : String.Empty;

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = document.person_FK == user.Person_FK;

            // EDMS Document
            _isEDMSDocument = document.EDMSDocument.HasValue && document.EDMSDocument.Value;

            HandleDocumentType();

            if (Request.QueryString["XevprmValidation"] != null)
            {
                ValidateFormForXevprm(document);
            }
        }

        private void BindRelatedEntities(int? documentPk)
        {
            var text = Constant.MissingValue;
            var authorisedProductDocumentMnList = _authorisedProductDocumentMnOperations.GetAuthorizedProductsByDocumentFK(documentPk);
            if (authorisedProductDocumentMnList != null && authorisedProductDocumentMnList.Any())
            {
                foreach (var item in authorisedProductDocumentMnList)
                {
                    if (!item.ap_FK.HasValue) continue;
                    var authorisedProduct = _authorisedProductOperations.GetEntity(item.ap_FK);
                    if (authorisedProduct != null && authorisedProduct.ap_PK.HasValue)
                    {
                        text = !string.IsNullOrWhiteSpace(authorisedProduct.product_name) ? authorisedProduct.product_name : Constant.MissingValue;
                        lbSrRelatedEntities.LbInput.Items.Add(new ListItem(text, authorisedProduct.ap_PK.ToString()));
                    }
                }
                SearchType = SearchType.AuthorisedProduct;
                return;
            }

            var productDocumentMnList = _productDocumentMnOperations.GetProductsByDocumentFK(documentPk);
            if (productDocumentMnList != null && productDocumentMnList.Any())
            {
                foreach (var item in productDocumentMnList)
                {
                    if (!item.product_FK.HasValue) continue;
                    var product = _productOperations.GetEntity(item.product_FK);
                    if (product != null && product.product_PK.HasValue)
                    {
                        text = product.GetNameFormatted();
                        lbSrRelatedEntities.LbInput.Items.Add(new ListItem(text, product.product_PK.ToString()));
                    }
                }
                SearchType = SearchType.Product;
                return;
            }

            var projectDocumentMnList = _projectDocumentMnOperations.GetProjectMNByDocumentFK(documentPk);
            if (projectDocumentMnList != null && projectDocumentMnList.Any())
            {
                foreach (var item in projectDocumentMnList)
                {
                    if (!item.project_FK.HasValue) continue;
                    var project = _projectOperations.GetEntity(item.project_FK);
                    if (project != null && project.project_PK.HasValue)
                    {
                        text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.MissingValue;
                        lbSrRelatedEntities.LbInput.Items.Add(new ListItem(text, project.project_PK.ToString()));
                    }
                }
                SearchType = SearchType.Project;
                return;
            }

            var activityDocumentMnList = _activityDocumentMnOperations.GetActivitiesMNByDocument(documentPk);
            if (activityDocumentMnList != null && activityDocumentMnList.Any())
            {
                foreach (var item in activityDocumentMnList)
                {
                    if (!item.activity_FK.HasValue) continue;
                    var activity = _activityOperations.GetEntity(item.activity_FK);
                    if (activity != null && activity.activity_PK.HasValue)
                    {
                        text = !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.MissingValue;
                        lbSrRelatedEntities.LbInput.Items.Add(new ListItem(text, activity.activity_PK.ToString()));
                    }
                }
                SearchType = SearchType.Activity;
                return;
            }

            var taskDocumentMnList = _taskDocumentMnOperations.GetTasksMNByDocument(documentPk);
            if (taskDocumentMnList != null && taskDocumentMnList.Any())
            {
                foreach (var item in taskDocumentMnList)
                {
                    if (!item.task_FK.HasValue) continue;
                    var task = _taskOperations.GetEntity(item.task_FK);
                    if (task == null || !task.task_PK.HasValue) continue;
                    var taskName = _taskNameOperations.GetEntity(task.task_name_FK);
                    text = !string.IsNullOrWhiteSpace(taskName.task_name) ? taskName.task_name : Constant.MissingValue;
                    lbSrRelatedEntities.LbInput.Items.Add(new ListItem(text, task.task_PK.ToString()));
                }
                SearchType = SearchType.Task;
                return;
            }

            var pharmaceuticalProductMnList = _pharmaceuticalProductDocumentMnOperations.GetPProductsByDocumentFK(documentPk);
            if (pharmaceuticalProductMnList != null && pharmaceuticalProductMnList.Any())
            {
                foreach (var item in pharmaceuticalProductMnList)
                {
                    if (!item.pp_FK.HasValue) continue;
                    var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(item.pp_FK);
                    if (pharmaceuticalProduct != null && pharmaceuticalProduct.pharmaceutical_product_PK.HasValue)
                    {
                        text = !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.MissingValue;
                        lbSrRelatedEntities.LbInput.Items.Add(new ListItem(text, pharmaceuticalProduct.pharmaceutical_product_PK.ToString()));
                    }
                }
                SearchType = SearchType.PharmaceuticalProduct;
                return;
            }
        }

        private void BindDocumentType(int? documentTypePk)
        {
            var documentType = _typeOperations.GetEntity(documentTypePk);
            ddlDocumentType.SelectedValue = documentType != null ? documentType.type_PK : null;
            if (string.IsNullOrWhiteSpace(_selectedPrevDocumentType) && documentType != null) _selectedPrevDocumentType = documentType.name;
        }

        private void BindVersionNumber(int? versionNumberPk)
        {
            var versionNumber = _typeOperations.GetEntity(versionNumberPk);
            ddlVersionNumber.SelectedValue = versionNumber != null ? versionNumber.type_PK : null;
        }

        private void BindResponsibleUser(int? responsibleUserPk)
        {
            var selectedResponsibleUser = _personOperations.GetEntity(responsibleUserPk);
            ddlResponsibleUser.SelectedValue = selectedResponsibleUser != null ? selectedResponsibleUser.person_PK : null;
        }

        private void BindVersionLabel(int? versionLabelPk)
        {
            var versionLabel = _typeOperations.GetEntity(versionLabelPk);
            ddlVersionLabel.SelectedValue = versionLabel != null ? versionLabel.type_PK : null;
        }

        private void BindRegulatoryStatus(int? regulatoryStatusPk)
        {
            var regulatoryStatus = _typeOperations.GetEntity(regulatoryStatusPk);
            ddlRegulatoryStatus.SelectedValue = regulatoryStatus != null ? regulatoryStatus.type_PK : null;
        }

        private void BindAttachmentType(int? attachmentTypePk)
        {
            var attachmentType = _typeOperations.GetEntity(attachmentTypePk);
            ddlAttachmentType.SelectedValue = attachmentType != null ? attachmentType.type_PK : null;
        }

        private void BindLanguageCode(int? documentPk)
        {
            var selectedLanguageCodeList = _languageCodeOperations.GetLanguageCodeByDocument(documentPk).Select(lc => lc.languagecode_PK).ToList();
            var languageCodeList = lbLanguageCodes.LbInput.Items;
            foreach (ListItem languageCode in languageCodeList)
            {
                if (selectedLanguageCodeList.Contains(Convert.ToInt32(languageCode.Value))) languageCode.Selected = true;
            }
        }

        private void BindAttachments(List<Attachment_PK> attachmentsForThisSession)
        {
            var refAttachmentPkListToDelete = _attachmentPkListToDelete;
            attachmentsForThisSession = attachmentsForThisSession.Where(a => a.attachment_PK != null && !refAttachmentPkListToDelete.Contains(a.attachment_PK.Value)).ToList();

            upAttachments.AttachmentIdNewValue = attachmentsForThisSession.Select(a => a.attachment_PK != null ? a.attachment_PK.Value : 0).ToList();
            upAttachments.NumberOfUploadedAttachments = attachmentsForThisSession.Count;
            upAttachments.PnlUploadedFiles.Visible = true;
            upAttachments.GvData.DataSource = attachmentsForThisSession;
            upAttachments.GvData.DataBind();
            MasterPage.UpTopMenu.Update();
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (lbSrRelatedEntities.Required && lbSrRelatedEntities.LbInput.Items.Count == 0)
            {
                string entityType = "Related entities";
                switch (SearchType)
                {
                    case SearchType.Product:
                        entityType = "Products";
                        break;
                    case SearchType.Project:
                        entityType = "Projects";
                        break;
                    case SearchType.Task:
                        entityType = "Tasks";
                        break;
                    case SearchType.PharmaceuticalProduct:
                        entityType = "Pharmaceutical products";
                        break;
                    case SearchType.AuthorisedProduct:
                        entityType = "Authorised products";
                        break;
                    case SearchType.Activity:
                        entityType = "Activities";
                        break;
                }
                errorMessage += string.Format("{0} can't be empty.<br />", entityType);
                lbSrRelatedEntities.ValidationError = string.Format("{0} can't be empty.", entityType);
            }

            if (txtDocumentName.Required && string.IsNullOrWhiteSpace(txtDocumentName.Text))
            {
                errorMessage += "Document name can't be empty.<br />";
                txtDocumentName.ValidationError = "Document name can't be empty.";
            }

            if (ddlDocumentType.Required && !ddlDocumentType.SelectedId.HasValue)
            {
                errorMessage += "Document type can't be empty.<br />";
                ddlDocumentType.ValidationError = "Document type can't be empty.";
            }

            if (ddlVersionNumber.Required && !ddlVersionNumber.SelectedId.HasValue)
            {
                errorMessage += "Version number can't be empty.<br />";
                ddlVersionNumber.ValidationError = "Version number can't be empty.";
            }

            if (ddlResponsibleUser.Required && !ddlResponsibleUser.SelectedId.HasValue)
            {
                errorMessage += "Responsible user can't be empty.<br />";
                ddlResponsibleUser.ValidationError = "Responsible user can't be empty.";
            }

            if (ddlVersionLabel.Required && !ddlVersionLabel.SelectedId.HasValue)
            {
                errorMessage += "Version label can't be empty.<br />";
                ddlVersionLabel.ValidationError = "Version label can't be empty.";
            }

            if (ddlRegulatoryStatus.Required && !ddlRegulatoryStatus.SelectedId.HasValue)
            {
                errorMessage += "Regulatory status can't be empty.<br />";
                ddlRegulatoryStatus.ValidationError = "Regulatory status can't be empty.";
            }

            if (ddlAttachmentType.Required && !ddlAttachmentType.SelectedId.HasValue)
            {
                errorMessage += "Attachment tpye can't be empty.<br />";
                ddlAttachmentType.ValidationError = "Attachment type can't be empty.";
            }

            if (!string.IsNullOrWhiteSpace(dtChangeDate.Text) && !ValidationHelper.IsValidDateTime(dtChangeDate.Text, CultureInfoHr))
            {
                errorMessage += "Change date is not a valid date.<br />";
                dtChangeDate.ValidationError = "Change date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtEffectiveStartDate.Text) && !ValidationHelper.IsValidDateTime(dtEffectiveStartDate.Text, CultureInfoHr))
            {
                errorMessage += "Effective start date is not a valid date.<br />";
                dtEffectiveStartDate.ValidationError = "Effective start date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtEffectiveEndDate.Text) && !ValidationHelper.IsValidDateTime(dtEffectiveEndDate.Text, CultureInfoHr))
            {
                errorMessage += "Effective end date is not a valid date.<br />";
                dtEffectiveEndDate.ValidationError = "Effective end date is not a valid date.";
            }

            if (dtVersionDate.Visible && !string.IsNullOrWhiteSpace(dtVersionDate.Text) && !ValidationHelper.IsValidDateTime(dtVersionDate.Text, CultureInfoHr))
            {
                errorMessage += "Version date is not a valid date.<br />";
                dtVersionDate.ValidationError = "Version date is not a valid date.";
            }

            var documentTypePpi = ddlDocumentType.SelectedItem != null && ddlDocumentType.SelectedItem.Text.ToLower() == "ppi";
            if (documentTypePpi)
            {
                if (lbSrRelatedEntities.LbInput.Items.Count > 1)
                {
                    errorMessage += "Only one authorised product can be assigned to PPI document.<br />";
                    lbSrRelatedEntities.ValidationError += "Only one authorised product can be assigned to PPI document.";
                }

                if (lbLanguageCodes.Required)
                {
                    if (lbLanguageCodes.GetSelectedItems().Count == 0)
                    {
                        errorMessage += "Language code can't be empty!<br />";
                        lbLanguageCodes.ValidationError = "Language code can't be empty!";
                    }
                    else if (lbLanguageCodes.GetSelectedItems().Count > 1)
                    {
                        errorMessage += "With PPI document type you can select only one language code!<br />";
                        lbLanguageCodes.ValidationError = "With PPI document type you can select only one language code!";
                    }
                }

                var attachmentsForThisSession = Attachment_PK.GetAttachmentsForThisSession(_attachmentOperations, _idDoc, upAttachments.AttachmentSessionId);

                var refAttachmentPkListToDelete = _attachmentPkListToDelete;
                attachmentsForThisSession = attachmentsForThisSession.Where(a => a.attachment_PK != null && !refAttachmentPkListToDelete.Contains(a.attachment_PK.Value)).ToList();

                if (attachmentsForThisSession != null && attachmentsForThisSession.Any())
                {
                    if (attachmentsForThisSession.Count != 1)
                    {
                        errorMessage += "Authorised product can have only one PPI document.<br />";
                        upAttachments.ValidationError = "Authorised product can have only one PPI document.";
                    }
                    else
                    {
                        var ppiAttachment = attachmentsForThisSession.FirstOrDefault();
                        if (ppiAttachment != null)
                        {
                            var apPpiDocuments = new List<Document_PK>();
                            if (lbSrRelatedEntities.LbInput.Items.Count == 1)
                            {
                                var apPkString = lbSrRelatedEntities.LbInput.Items[0].Value;
                                var apPk = ValidationHelper.IsValidInt(apPkString) ? (int?)Convert.ToInt32(apPkString) : null;
                                if (apPk.HasValue) apPpiDocuments = _documentOperations.GetDocumentsByAP(apPk.Value, "PPI");
                            }

                            if (apPpiDocuments != null && apPpiDocuments.Count > 0 && (FormType == FormType.New || FormType == FormType.SaveAs))
                            {
                                errorMessage += "Authorised product can have only one PPI document.<br />";
                                upAttachments.ValidationError = "Authorised product can have only one PPI document.";
                            }
                            else
                            {
                                if (FormType == FormType.Edit &&
                                    ((_editingPPIDocument != null && !_editingPPIDocument.Value && apPpiDocuments.Count > 0) ||
                                      apPpiDocuments.Count > 1))
                                {
                                    errorMessage += "Authorised product can have only one PPI document.<br />";
                                    upAttachments.ValidationError = "Authorised product can have only one PPI document.";
                                }
                                else
                                {
                                    var selectedAttachmentType = ddlAttachmentType.SelectedItem.Text.ToLower();
                                    var ppiAttachmentFileExtension = Path.GetExtension(ppiAttachment.attachmentname).Replace(".", "").ToLower();
                                    if (selectedAttachmentType != ppiAttachmentFileExtension)
                                    {
                                        errorMessage += "Attachment type and file extension must match.<br />";
                                        ddlAttachmentType.ValidationError = "Attachment type and file extension must match.";
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    errorMessage += "Attachments on PPI document can't be empty. <br />";
                    upAttachments.ValidationError = "Attachments on PPI document can't be empty. ";
                }
            }

            var isEDMSDocument = _isEDMSDocument.HasValue && _isEDMSDocument.Value;
            var refEDMSattachmentPkList = _EDMSattachmentPkList;
            if (isEDMSDocument && refEDMSattachmentPkList.Count == 0)
            {
                errorMessage += "This document is flagged as an EDMS document, but does not have any linked EDMS documents. <br />Please link at least one EDMS document or flag this document as a non-EDMS document.<br />";
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
            lbSrRelatedEntities.ValidationError = string.Empty;
            txtDocumentName.ValidationError = string.Empty;
            ddlDocumentType.ValidationError = string.Empty;
            ddlVersionNumber.ValidationError = string.Empty;
            ddlResponsibleUser.ValidationError = string.Empty;
            ddlVersionLabel.ValidationError = string.Empty;
            ddlRegulatoryStatus.ValidationError = string.Empty;
            ddlAttachmentType.ValidationError = string.Empty;
            lbLanguageCodes.ValidationError = string.Empty;
            dtChangeDate.ValidationError = string.Empty;
            dtEffectiveStartDate.ValidationError = string.Empty;
            dtEffectiveEndDate.ValidationError = string.Empty;
            dtVersionDate.ValidationError = string.Empty;
            upAttachments.ValidationError = string.Empty;
        }

        private void ValidateFormForXevprm(Document_PK entity)
        {
            if (Request.QueryString["XevprmValidation"].NotIn("1", "2", "3", "4", "6")) return;

            IAuthorisedProductOperations authorisedProductOperations = new AuthorisedProductDAL();
            var authorisedProduct = ValidationHelper.IsValidInt(Request.QueryString["idAuthProd"]) ? authorisedProductOperations.GetEntity(int.Parse(Request.QueryString["idAuthProd"])) : null;

            if (authorisedProduct == null) return;

            XevprmOperationType operationType;
            Enum.TryParse(Request.QueryString["XevprmValidation"], true, out operationType);

            var validationResult = XevprmXml.ValidateAuthorisedProduct(authorisedProduct, operationType);

            if (validationResult.XevprmValidationExceptions.Count == 0) return;

            var errorControlMaping = new Dictionary<string, Control>()
                {
                {XevprmValidationRules.AP.PPI.attachmentcode.DataType.RuleId, txtEvcode},
                {XevprmValidationRules.ATT.operationtype.BR1.RuleId, null},
                {XevprmValidationRules.ATT.localnumber.DataType.RuleId, null},
                {XevprmValidationRules.ATT.localnumber.Cardinality.RuleId, null},
                {XevprmValidationRules.ATT.attachmentversiondate.BR3.RuleId, dtVersionDate},
                {XevprmValidationRules.ATT.attachmentversiondate.Cardinality.RuleId, dtVersionDate},
                {XevprmValidationRules.ATT.filename.DataType.RuleId, upAttachments},
                {XevprmValidationRules.ATT.filename.Cardinality.RuleId, upAttachments},
                {XevprmValidationRules.ATT.BRCustom1.RuleId, upAttachments},
                {XevprmValidationRules.ATT.attachmentname.DataType.RuleId, txtDocumentName},
                {XevprmValidationRules.ATT.attachmentname.Cardinality.RuleId, txtDocumentName},
                {XevprmValidationRules.ATT.attachmentversion.DataType.RuleId, ddlVersionNumber},
                {XevprmValidationRules.ATT.attachmentversion.Cardinality.RuleId, ddlVersionNumber},
                {XevprmValidationRules.ATT.filetype.DataType.RuleId, ddlAttachmentType},
                {XevprmValidationRules.ATT.filetype.Cardinality.RuleId, ddlAttachmentType},
                {XevprmValidationRules.ATT.languagecode.DataType.RuleId, lbLanguageCodes},
                {XevprmValidationRules.ATT.languagecode.Cardinality.RuleId, lbLanguageCodes},
            };

            foreach (var error in validationResult.XevprmValidationExceptions)
            {
                if (error.XevprmValidationRuleId == null || !errorControlMaping.Keys.Contains(error.XevprmValidationRuleId)) continue;
                //if (error.ReadyRootEntityType != typeof(Document_PK) || error.ReadyRootEntityPk != _idDoc) continue;

                var control = errorControlMaping[error.XevprmValidationRuleId] as Shared.Interface.IXevprmValidationError;
                if (control == null) continue;
                control.ValidationError = error.ReadyMessage;
            }
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            Document_PK document;
            switch (FormType)
            {
                case FormType.SaveAs:
                case FormType.New:
                    document = new Document_PK();
                    break;
                case FormType.Edit:
                    if (_idDoc == null) return null;
                    document = _documentOperations.GetEntity(_idDoc);
                    break;
                default:
                    return null;
            }

            if (document == null) return null;

            document.name = txtDocumentName.Text;
            document.description = txtDescription.Text;
            document.type_FK = ddlDocumentType.SelectedId;
            document.version_number = ddlVersionNumber.SelectedId;
            document.person_FK = ddlResponsibleUser.SelectedId;
            document.version_label = ddlVersionLabel.SelectedId;
            document.document_code = txtDocumentNumber.Text;
            document.regulatory_status = ddlRegulatoryStatus.SelectedId;
            document.comment = txtComment.Text;
            document.attachment_type_FK = ddlAttachmentType.SelectedId;
            document.change_date = ValidationHelper.IsValidDateTime(dtChangeDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtChangeDate.Text) : null;
            document.effective_start_date = ValidationHelper.IsValidDateTime(dtEffectiveStartDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtEffectiveStartDate.Text) : null;
            document.effective_end_date = ValidationHelper.IsValidDateTime(dtEffectiveEndDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtEffectiveEndDate.Text) : null;
            document.version_date = ValidationHelper.IsValidDateTime(dtVersionDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtVersionDate.Text) : null;
            document.EDMSModifyDate = ValidationHelper.IsValidDateTime(dtEDMSModifyDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtEDMSModifyDate.Text) : null;
            document.EDMSBindingRule = txtEDMSBindingRule.Text;
            document.EDMSDocument = _isEDMSDocument;

            var refEDMSattachmentPkList = _EDMSattachmentPkList;
            if (_EDMSAttachment != null && FormType.In(FormType.New, FormType.Edit))
            {
                document.EDMSDocumentId = _EDMSAttachment.documentID;
                document.EDMSVersionNumber = _EDMSAttachment.versionNumber;
                document.EDMSModifyDate = _EDMSAttachment.modifyDate;
            }
            else if (FormType == FormType.SaveAs)
            {
                var _idDocOld = ValidationHelper.IsValidInt(Request.QueryString["idDoc"]) ? int.Parse(Request.QueryString["idDoc"]) : (int?)null;
                if (_idDocOld.HasValue && refEDMSattachmentPkList.Count > 0)
                {
                    var oldDocument = _documentOperations.GetEntity(_idDocOld);
                    if (oldDocument != null)
                    {
                        document.EDMSDocumentId = oldDocument.EDMSDocumentId;
                        document.EDMSVersionNumber = oldDocument.EDMSVersionNumber;
                        document.EDMSModifyDate = oldDocument.EDMSModifyDate;
                        document.EDMSDocument = oldDocument.EDMSDocument;
                    }
                }
            }
            
            if (refEDMSattachmentPkList.Count == 0)
            {
                document.EDMSDocumentId = null;
                document.EDMSDocumentId = null;
                document.EDMSVersionNumber = null;
                document.EDMSModifyDate = null;
                document.EDMSBindingRule = null;
            }

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;

                document = _documentOperations.Save(document);

                if (!document.document_PK.HasValue) return null;

                SaveAssignedEntities(document.document_PK, auditTrailSessionToken);
                SaveLanguageCodes(document.document_PK, auditTrailSessionToken);
                SaveAttachments(document.document_PK, auditTrailSessionToken);

                _documentOperations.UpdateCalculatedColumn(document.document_PK.Value, Document_PK.CalculatedColumn.All);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, document.document_PK, "DOCUMENT", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, document.document_PK, "DOCUMENT", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return document;
        }

        private void SaveAttachments(int? documentPk, string auditTrailSessionToken)
        {
            var attachmentsToSave = _attachmentOperations.GetAttachmentsBySessionId(upAttachments.AttachmentSessionId);
            var refAttachmentPkListToDelete = _attachmentPkListToDelete;
            attachmentsToSave = attachmentsToSave.Where(a => a.attachment_PK != null && !refAttachmentPkListToDelete.Contains(a.attachment_PK.Value)).ToList();

            foreach (var attachment in attachmentsToSave)
            {
                _attachmentOperations.SaveLinkToDocument(attachment.attachment_PK, documentPk);
            }

            foreach (var attachmentPk in refAttachmentPkListToDelete)
            {
                _attachmentOperations.Delete(attachmentPk);
            }
        }

        private void SaveAssignedEntities(int? documentPk, string auditTrailSessionToken)
        {
            if (SearchType == SearchType.Null) return;

            var columnName = string.Empty;
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            if (SearchType == SearchType.Product) HandleProductDocumentSave(documentPk, out columnName, out complexAuditOldValue, out complexAuditNewValue);
            else if (SearchType == SearchType.AuthorisedProduct) HandleAuthorisedProductDocumentSave(documentPk, out columnName, out complexAuditOldValue, out complexAuditNewValue);
            else if (SearchType == SearchType.PharmaceuticalProduct) HandlePharmaceuticalProductDocumentSave(documentPk, out columnName, out complexAuditOldValue, out complexAuditNewValue);
            else if (SearchType == SearchType.Activity) HandleActivityDocumentSave(documentPk, out columnName, out complexAuditOldValue, out complexAuditNewValue);
            else if (SearchType == SearchType.Task) HandleTaskDocumentSave(documentPk, out columnName, out complexAuditOldValue, out complexAuditNewValue);
            else if (SearchType == SearchType.Project) HandleProjectDocumentSave(documentPk, out columnName, out complexAuditOldValue, out complexAuditNewValue);

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, documentPk.ToString(), columnName);
        }

        private void HandleAuthorisedProductDocumentSave(int? documentPk, out string columnName, out string complexAuditOldValue, out string complexAuditNewValue)
        {
            var _complexAuditNewValue = string.Empty;
            var _complexAuditOldValue = string.Empty;

            var oldAuthorisedProductDocumentMnList = _authorisedProductDocumentMnOperations.GetAuthorizedProductsByDocumentFK(documentPk);

            // Calculate old document complex audit value
            foreach (var oldAuthorisedProductDocument in oldAuthorisedProductDocumentMnList)
            {
                if (_complexAuditOldValue != "") _complexAuditOldValue += "|||";
                var authorisedProduct = _authorisedProductOperations.GetEntity(oldAuthorisedProductDocument.ap_FK);
                if (authorisedProduct != null && !string.IsNullOrWhiteSpace(authorisedProduct.product_name)) _complexAuditOldValue += authorisedProduct.product_name;
            }

            // Delete old document assignments
            if (oldAuthorisedProductDocumentMnList.Count > 0)
            {
                var oldAuthorisedProductDocumentsPks = new List<int>();
                oldAuthorisedProductDocumentsPks.AddRange(oldAuthorisedProductDocumentMnList.Select(apDocMn => apDocMn.ap_document_mn_PK != null ? (int)apDocMn.ap_document_mn_PK : 0));
                _authorisedProductDocumentMnOperations.DeleteCollection(oldAuthorisedProductDocumentsPks);
            }

            // Add new items to a new language code list to save.
            // Calculate new language codes complex audit value
            var newAuthorisedProductDocumentLb = lbSrRelatedEntities.LbInput.Items;
            var newAuthorisedProductDocuments = new List<Ap_document_mn_PK>();
            foreach (ListItem newAuthorisedProductDocument in newAuthorisedProductDocumentLb)
            {
                newAuthorisedProductDocuments.Add(new Ap_document_mn_PK(null, documentPk, Int32.Parse(newAuthorisedProductDocument.Value)));
                if (_complexAuditNewValue != "") _complexAuditNewValue += "|||";
                _complexAuditNewValue += newAuthorisedProductDocument.Text;
            }

            // Save new distributors
            if (newAuthorisedProductDocuments.Count > 0)
            {
                _authorisedProductDocumentMnOperations.SaveCollection(newAuthorisedProductDocuments);
            }

            columnName = "DOCUMENT_AP_MN";
            complexAuditOldValue = _complexAuditOldValue;
            complexAuditNewValue = _complexAuditNewValue;
        }

        private void HandleProductDocumentSave(int? documentPk, out string columnName, out string complexAuditOldValue, out string complexAuditNewValue)
        {
            var _complexAuditNewValue = string.Empty;
            var _complexAuditOldValue = string.Empty;

            var oldProductDocumentMnList = _productDocumentMnOperations.GetProductsByDocumentFK(documentPk);

            // Calculate old document complex audit value
            foreach (var oldProductDocument in oldProductDocumentMnList)
            {
                if (_complexAuditOldValue != "") _complexAuditOldValue += "|||";
                var product = _productOperations.GetEntity(oldProductDocument.product_FK);
                if (product != null && !string.IsNullOrWhiteSpace(product.GetNameFormatted())) _complexAuditOldValue += product.GetNameFormatted();
            }

            // Delete old document assignments
            if (oldProductDocumentMnList.Count > 0)
            {
                var oldProductDocumentsPks = new List<int>();
                oldProductDocumentsPks.AddRange(oldProductDocumentMnList.Select(pDocMn => pDocMn.product_document_mn_PK != null ? (int)pDocMn.product_document_mn_PK : 0));
                _productDocumentMnOperations.DeleteCollection(oldProductDocumentsPks);
            }

            // Add new items to a new odcument assignments list to save.
            // Calculate new document assignments complex audit value
            var newProductDocumentLb = lbSrRelatedEntities.LbInput.Items;
            var newProductDocuments = new List<Product_document_mn_PK>();
            foreach (ListItem newProductDocument in newProductDocumentLb)
            {
                newProductDocuments.Add(new Product_document_mn_PK(null, Int32.Parse(newProductDocument.Value), documentPk));
                if (_complexAuditNewValue != "") _complexAuditNewValue += "|||";
                _complexAuditNewValue += newProductDocument.Text;
            }

            // Save new assignments
            if (newProductDocuments.Count > 0)
            {
                _productDocumentMnOperations.SaveCollection(newProductDocuments);
            }

            columnName = "DOCUMENT_PRODUCT_MN";
            complexAuditOldValue = _complexAuditOldValue;
            complexAuditNewValue = _complexAuditNewValue;
        }

        private void HandleProjectDocumentSave(int? documentPk, out string columnName, out string complexAuditOldValue, out string complexAuditNewValue)
        {
            var _complexAuditNewValue = string.Empty;
            var _complexAuditOldValue = string.Empty;

            var oldProjectDocumentMnList = _projectDocumentMnOperations.GetProjectMNByDocumentFK(documentPk);

            // Calculate old document complex audit value
            foreach (var oldProjectDocument in oldProjectDocumentMnList)
            {
                if (_complexAuditOldValue != "") _complexAuditOldValue += "|||";
                var project = _projectOperations.GetEntity(oldProjectDocument.project_FK);
                if (project != null && !string.IsNullOrWhiteSpace(project.name)) _complexAuditOldValue += project.name;
            }

            // Delete old document assignments
            if (oldProjectDocumentMnList.Count > 0)
            {
                var oldProjectDocumentsPks = new List<int>();
                oldProjectDocumentsPks.AddRange(oldProjectDocumentMnList.Select(projDocMn => projDocMn.project_document_PK != null ? (int)projDocMn.project_document_PK : 0));
                _projectDocumentMnOperations.DeleteCollection(oldProjectDocumentsPks);
            }

            // Add new items to a new odcument assignments list to save.
            // Calculate new document assignments complex audit value
            var newProjectDocumentLb = lbSrRelatedEntities.LbInput.Items;
            var newProjectDocuments = new List<Project_document_PK>();
            foreach (ListItem newProjectDocument in newProjectDocumentLb)
            {
                newProjectDocuments.Add(new Project_document_PK(null, Int32.Parse(newProjectDocument.Value), documentPk));
                if (_complexAuditNewValue != "") _complexAuditNewValue += "|||";
                _complexAuditNewValue += newProjectDocument.Text;
            }

            // Save new assignments
            if (newProjectDocuments.Count > 0)
            {
                _projectDocumentMnOperations.SaveCollection(newProjectDocuments);
            }

            columnName = "DOCUMENT_PROJECT_MN";
            complexAuditOldValue = _complexAuditOldValue;
            complexAuditNewValue = _complexAuditNewValue;
        }

        private void HandleTaskDocumentSave(int? documentPk, out string columnName, out string complexAuditOldValue, out string complexAuditNewValue)
        {
            var _complexAuditNewValue = string.Empty;
            var _complexAuditOldValue = string.Empty;

            var oldTaskDocumentMnList = _taskDocumentMnOperations.GetTasksMNByDocument(documentPk);

            // Calculate old document complex audit value
            foreach (var oldTaskDocument in oldTaskDocumentMnList)
            {
                if (_complexAuditOldValue != "") _complexAuditOldValue += "|||";
                var task = _taskOperations.GetEntity(oldTaskDocument.task_FK);
                if (task == null) continue;
                var taskName = _taskNameOperations.GetEntity(task.task_name_FK);
                if (taskName != null && !string.IsNullOrWhiteSpace(taskName.task_name)) _complexAuditOldValue += taskName.task_name;
            }

            // Delete old document assignments
            if (oldTaskDocumentMnList.Count > 0)
            {
                var oldTaskDocumentsPks = new List<int>();
                oldTaskDocumentsPks.AddRange(oldTaskDocumentMnList.Select(tDocMn => tDocMn.task_document_PK != null ? (int)tDocMn.task_document_PK : 0));
                _taskDocumentMnOperations.DeleteCollection(oldTaskDocumentsPks);
            }

            // Add new items to a new odcument assignments list to save.
            // Calculate new document assignments complex audit value
            var newTaskDocumentLb = lbSrRelatedEntities.LbInput.Items;
            var newTaskDocuments = new List<Task_document_PK>();
            foreach (ListItem newTaskDocument in newTaskDocumentLb)
            {
                newTaskDocuments.Add(new Task_document_PK(null, Int32.Parse(newTaskDocument.Value), documentPk));
                if (_complexAuditNewValue != "") _complexAuditNewValue += "|||";
                _complexAuditNewValue += newTaskDocument.Text;
            }

            // Save new assignments
            if (newTaskDocuments.Count > 0)
            {
                _taskDocumentMnOperations.SaveCollection(newTaskDocuments);
            }

            columnName = "DOCUMENT_TASK_MN";
            complexAuditOldValue = _complexAuditOldValue;
            complexAuditNewValue = _complexAuditNewValue;
        }

        private void HandleActivityDocumentSave(int? documentPk, out string columnName, out string complexAuditOldValue, out string complexAuditNewValue)
        {
            var _complexAuditNewValue = string.Empty;
            var _complexAuditOldValue = string.Empty;

            var oldActivityDocumentMnList = _activityDocumentMnOperations.GetActivitiesMNByDocument(documentPk);

            // Calculate old document complex audit value
            foreach (var oldActivityDocument in oldActivityDocumentMnList)
            {
                if (_complexAuditOldValue != "") _complexAuditOldValue += "|||";
                var activity = _activityOperations.GetEntity(oldActivityDocument.activity_FK);
                if (activity != null && !string.IsNullOrWhiteSpace(activity.name)) _complexAuditOldValue += activity.name;
            }

            // Delete old document assignments
            if (oldActivityDocumentMnList.Count > 0)
            {
                var oldActivityDocumentsPks = new List<int>();
                oldActivityDocumentsPks.AddRange(oldActivityDocumentMnList.Select(aDocMn => aDocMn.activity_document_PK != null ? (int)aDocMn.activity_document_PK : 0));
                _activityDocumentMnOperations.DeleteCollection(oldActivityDocumentsPks);
            }

            // Add new items to a new odcument assignments list to save.
            // Calculate new document assignments complex audit value
            var newActivityDocumentLb = lbSrRelatedEntities.LbInput.Items;
            var newActivityDocuments = new List<Activity_document_PK>();
            foreach (ListItem newActivityDocument in newActivityDocumentLb)
            {
                newActivityDocuments.Add(new Activity_document_PK(null, Int32.Parse(newActivityDocument.Value), documentPk));
                if (_complexAuditNewValue != "") _complexAuditNewValue += "|||";
                _complexAuditNewValue += newActivityDocument.Text;
            }

            // Save new assignments
            if (newActivityDocuments.Count > 0)
            {
                _activityDocumentMnOperations.SaveCollection(newActivityDocuments);
            }

            columnName = "DOCUMENT_ACTIVITY_MN";
            complexAuditOldValue = _complexAuditOldValue;
            complexAuditNewValue = _complexAuditNewValue;
        }

        private void HandlePharmaceuticalProductDocumentSave(int? documentPk, out string columnName, out string complexAuditOldValue, out string complexAuditNewValue)
        {
            var _complexAuditNewValue = string.Empty;
            var _complexAuditOldValue = string.Empty;

            var oldPharmaceuticalProductDocumentMnList = _pharmaceuticalProductDocumentMnOperations.GetPProductsByDocumentFK(documentPk);

            // Calculate old document complex audit value
            foreach (var oldPharmaceuticalProductDocument in oldPharmaceuticalProductDocumentMnList)
            {
                if (_complexAuditOldValue != "") _complexAuditOldValue += "|||";
                var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(oldPharmaceuticalProductDocument.pp_FK);
                if (pharmaceuticalProduct != null && !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name)) _complexAuditOldValue += pharmaceuticalProduct.name;
            }

            // Delete old document assignments
            if (oldPharmaceuticalProductDocumentMnList.Count > 0)
            {
                var oldPharmaceuticalProductDocumentsPks = new List<int>();
                oldPharmaceuticalProductDocumentsPks.AddRange(oldPharmaceuticalProductDocumentMnList.Select(pDocMn => pDocMn.pp_document_PK != null ? (int)pDocMn.pp_document_PK : 0));
                _pharmaceuticalProductDocumentMnOperations.DeleteCollection(oldPharmaceuticalProductDocumentsPks);
            }

            // Add new items to a new odcument assignments list to save.
            // Calculate new document assignments complex audit value
            var newPharmaceuticalProductDocumentLb = lbSrRelatedEntities.LbInput.Items;
            var newPharmaceuticalProductDocuments = new List<Pp_document_PK>();
            foreach (ListItem newPharmaceuticalProductDocument in newPharmaceuticalProductDocumentLb)
            {
                newPharmaceuticalProductDocuments.Add(new Pp_document_PK(null, Int32.Parse(newPharmaceuticalProductDocument.Value), documentPk));
                if (_complexAuditNewValue != "") _complexAuditNewValue += "|||";
                _complexAuditNewValue += newPharmaceuticalProductDocument.Text;
            }

            // Save new assignments
            if (newPharmaceuticalProductDocuments.Count > 0)
            {
                _pharmaceuticalProductDocumentMnOperations.SaveCollection(newPharmaceuticalProductDocuments);
            }

            columnName = "DOCUMENT_PP_MN";
            complexAuditOldValue = _complexAuditOldValue;
            complexAuditNewValue = _complexAuditNewValue;
        }

        private void SaveLanguageCodes(int? documentPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = "";
            var complexAuditOldValue = "";

            var oldLanguageCodes = _documentLanguageMnOperations.GetLanguagesByDocument(documentPk);

            // Calculate old language codes complex audit value
            foreach (var oldLanguageCode in oldLanguageCodes)
            {
                if (complexAuditOldValue != "") complexAuditOldValue += "|||";
                var languageCode = _languageCodeOperations.GetEntity(oldLanguageCode.language_FK);
                if (languageCode != null && !string.IsNullOrWhiteSpace(languageCode.code)) complexAuditOldValue += languageCode.code;
            }

            // Delete old language codes
            if (oldLanguageCodes.Count > 0)
            {
                var oldLanguageCodesPks = new List<int>();
                oldLanguageCodesPks.AddRange(oldLanguageCodes.Select(docLangMn => docLangMn.document_language_mn_PK != null ? (int)docLangMn.document_language_mn_PK : 0));
                _documentLanguageMnOperations.DeleteCollection(oldLanguageCodesPks);
            }

            // Add new items to a new odcument assignments list to save.
            // Calculate new document assignments complex audit value
            var newLanguageCodesLb = lbLanguageCodes.GetSelectedItems();
            var newLanguageCodes = new List<Document_language_mn_PK>();
            foreach (ListItem newLanguageCode in newLanguageCodesLb)
            {
                newLanguageCodes.Add(new Document_language_mn_PK(null, documentPk, Int32.Parse(newLanguageCode.Value)));
                if (complexAuditNewValue != "") complexAuditNewValue += "|||";
                complexAuditNewValue += newLanguageCode.Text;
            }

            // Save new assignments
            if (newLanguageCodes.Count > 0)
            {
                _documentLanguageMnOperations.SaveCollection(newLanguageCodes);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, documentPk.ToString(), "DOCUMENT_LANGUAGE_CODE");
        }

        #endregion

        #region Delete

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        private void DeleteEntity(object arg)
        {

        }

        #endregion

        #endregion

        #region Event handlers

        #region EDMS

        void btnBrowseEDMSUC_OnClick(object sender, EventArgs e)
        {
            BrowseEDMSPopup.ShowModalForm();
        }

        void BrowseEDMSPopup_OnOkButtonClick(object sender, FormEventArgs<Tuple<EDMSDocument, EDMSDocumentVersion>> e)
        {
            var EDMSContainer = e.Data;
            var refEDMSattachmentPkList = _EDMSattachmentPkList;

            var edmsDocument = EDMSContainer.Item1;
            if (refEDMSattachmentPkList.Count == 0)
            {
                _EDMSAttachment = e.Data.Item1;
                _EDMSAttachment.content = null;
                var documentType = ddlDocumentType.SelectedItem;
                if ((SearchType == SearchType.AuthorisedProduct || SearchType == SearchType.Null) && documentType != null && documentType.Text.ToLower() == "ppi"
                    && edmsDocument.documentName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                {
                    MasterPage.ModalPopup.ShowModalPopup("Error!", "EDMS attachment name contains special file name characters!");
                    return;
                }
                else { FillEDMSFields(EDMSContainer); }
            }

            if (edmsDocument != null)
            {
                var documentType = ddlDocumentType.SelectedItem;
                if ((SearchType == SearchType.AuthorisedProduct || SearchType == SearchType.Null) && documentType != null && documentType.Text.ToLower() == "ppi"
                    && edmsDocument.documentName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
                {
                    MasterPage.ModalPopup.ShowModalPopup("Error!", "EDMS attachment name contains special file name characters!");
                    return;
                }

                var attachment = new Attachment_PK();

                attachment.attachmentname = edmsDocument.documentName + (!String.IsNullOrWhiteSpace(edmsDocument.format) ? "." + edmsDocument.format : string.Empty);
                attachment.filetype = !String.IsNullOrWhiteSpace(edmsDocument.format) ? AttachmentHelper.GetMIMEType(edmsDocument.format, true) : string.Empty;
                attachment.session_id = upAttachments.AttachmentSessionId;
                attachment.userID = SessionManager.Instance.CurrentUser.UserID;
                attachment.modified_date = edmsDocument.modifyDate;
                attachment.EDMSDocumentId = edmsDocument.documentID;
                attachment.EDMSBindingRule = edmsDocument.versionNumber;
                attachment.EDMSAttachmentFormat = edmsDocument.format;

                attachment = _attachmentOperations.Save(attachment);

                refEDMSattachmentPkList.Add(attachment.attachment_PK);

                _isEDMSDocument = refEDMSattachmentPkList.Count > 0;

                BindDynamicControls(null);
            }
        }

        #endregion

        #region Attachments panel

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            var refAttachmentPkListToDelete = _attachmentPkListToDelete;

            if (_attachmentPkToDelete.HasValue)
            {
                var refEDMSattachmentPkList = _EDMSattachmentPkList;
                if (refEDMSattachmentPkList.Count > 0 && refEDMSattachmentPkList.Contains(_attachmentPkToDelete))
                {
                    refEDMSattachmentPkList.Remove(_attachmentPkToDelete);
                }

                refAttachmentPkListToDelete.Add(_attachmentPkToDelete.Value);
                _isEDMSDocument = refEDMSattachmentPkList.Count > 0;
                SetVisibleFieldsDocumentTypeEDMS();
            }

            var attachmentsForThisSession = Attachment_PK.GetAttachmentsForThisSession(_attachmentOperations, _idDoc, upAttachments.AttachmentSessionId);
            if (attachmentsForThisSession.Count > 0)
            {
                upAttachments.AsyncFileUpload.CssClass = "async-upload-control-items";
                BindAttachments(attachmentsForThisSession);
            }
            else
            {
                upAttachments.AsyncFileUpload.CssClass = "async-upload-control-no-items";
                upAttachments.PnlUploadedFiles.Visible = false;
            }
        }

        protected void GvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var btnDownload = (LinkButton)e.Row.FindControl("btnDownload");
                if (!string.IsNullOrWhiteSpace(btnDownload.CommandArgument))
                {
                    var rootLocation = LocationManager.Instance.GetLocationByName("Root", CacheManager.Instance.AppLocations);
                    if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation))
                    {
                        btnDownload.Attributes.Add("onclick", "SaveTheDownloadBtnAttach('" + btnDownload.ClientID + "," + btnDownload.CommandArgument + "');");
                    }
                }

                var attachment = e.Row.DataItem as Attachment_PK;

                if (attachment != null)
                {
                    if (_EDMSattachmentPkList.Contains(attachment.attachment_PK) || attachment.EDMSDocumentId != null)
                    {
                        var grid = sender as GridView;
                        if (grid != null)
                        {
                            var gridDataSource = grid.DataSource as List<Attachment_PK>;

                            if (gridDataSource != null && gridDataSource.TrueForAll(x=> x.EDMSDocumentId != null || _EDMSattachmentPkList.Contains(x.attachment_PK)))
                            {
                                e.Row.Cells[2].Visible = false;    
                            }
                        }

                        return;
                    }

                    var btnCheckOut = (LinkButton)e.Row.FindControl("btnCheckOut");
                    var btnCheckIn = (LinkButton)e.Row.FindControl("btnCheckIn");
                    var btnCancelCheckout = (LinkButton)e.Row.FindControl("btnCancelCheckout");
                    var asyncCheckIn = e.Row.FindControl("asyncCheckIn") as AjaxControlToolkit.AsyncFileUpload;
                    var lblCheckInThrobber = e.Row.FindControl("CheckInThrobber") as Label;

                    var divCheckAction = e.Row.FindControl("divCheckAction") as HtmlGenericControl;

                    if (btnCheckOut != null && btnCheckIn != null && btnCancelCheckout != null && asyncCheckIn != null && divCheckAction != null && lblCheckInThrobber != null)
                    {
                        btnCheckOut.Visible = !attachment.lock_owner_FK.HasValue;

                        if (attachment.lock_owner_FK.HasValue && attachment.lock_owner_FK == SessionManager.Instance.CurrentUser.UserID)
                        {
                            btnCheckIn.Visible = true;
                            btnCancelCheckout.Visible = true;
                            divCheckAction.Attributes["style"] = "width:300px;";
                            asyncCheckIn.Visible = true;
                            lblCheckInThrobber.Visible = true;
                            asyncCheckIn.UploadedComplete += CheckInAsyncFileUpload_UploadedComplete;
                            asyncCheckIn.Attributes["data-id"] = attachment.attachment_PK.ToString();
                        }
                        else
                        {
                            btnCheckIn.Visible = false;
                            btnCancelCheckout.Visible = attachment.lock_owner_FK.HasValue && SessionManager.Instance.CurrentUser.Roles.Any(x => x != null && x.ToLower() == "admin");
                            asyncCheckIn.Visible = false;
                            lblCheckInThrobber.Visible = false;
                        }

                        if (attachment.lock_owner_FK.HasValue && attachment.lock_owner_FK != SessionManager.Instance.CurrentUser.UserID)
                        {
                            var imgBtnDeleteAttachment = (ImageButton)e.Row.FindControl("imgBtnDeleteAttachment");

                            if (imgBtnDeleteAttachment != null)
                            {
                                imgBtnDeleteAttachment.Visible = false;
                            }
                        }
                    }

                    var spanCheckOutStatus = e.Row.FindControl("spanCheckOutStatus") as HtmlGenericControl;

                    if (spanCheckOutStatus != null)
                    {
                        if (attachment.lock_owner_FK.HasValue)
                        {
                            if (attachment.lock_owner_FK == SessionManager.Instance.CurrentUser.UserID)
                            {
                                spanCheckOutStatus.Attributes["class"] = "check-status checked-out-my";
                            }
                            else
                            {
                                spanCheckOutStatus.Attributes["class"] = "check-status checked-out-other";
                            }
                        }

                        if (_executedOnAttachmentPk.HasValue && _executedOnAttachmentPk == attachment.attachment_PK && _executedCheckAction != null)
                        {
                            var url = string.Format("{0}://{1}{2}", Request.Url.Scheme, Request.Url.Authority, Request.Url.LocalPath.Substring(0, Request.Url.LocalPath.IndexOf("/Views", System.StringComparison.Ordinal)));
                            string functionCall = null;
                            var dir = ConfigurationManager.AppSettings["CheckInLocalDirectory"];
                            if (string.IsNullOrWhiteSpace(dir))
                            {
                                dir = "C:\\";
                            }
                            else
                            {
                                dir = dir.Replace("\\", "\\\\");
                            }

                            var file = string.Format("{0}{1}", dir, Regex.Replace(attachment.attachmentname, "[/\\:*?\"<>|]", "_"));

                            if (_executedCheckAction == "CheckOut" || _executedCheckAction == "CheckOutFileNameWarning")
                            {
                                url = string.Format("{0}/Views/Business/FileDownload.ashx?attachID={1}", url, _executedOnAttachmentPk);
                                functionCall = string.Format("handleExecutedCheckAction('{0}','{1}','{2}')",
                                _executedCheckAction, url, file);
                            }
                            else if (_executedCheckAction == "CheckIn")
                            {
                                if (btnCheckIn != null)
                                {
                                    url = string.Format("{0}/Views/Business/FileUpload.ashx?action=checkin&attachmentId={1}&sessionId={2}", url, _executedOnAttachmentPk, upAttachments.AttachmentSessionId);
                                    functionCall = string.Format("handleExecutedCheckAction('{0}','{1}','{2}','{3}')",
                                                                 _executedCheckAction, url, file, btnCheckIn.ClientID);
                                }
                            }
                            else if (_executedCheckAction == "CancelCheckout")
                            {
                                functionCall = string.Format("handleExecutedCheckAction('{0}','{1}','{2}')",
                                _executedCheckAction, null, file);
                            }
                            else
                            {
                                functionCall = string.Format("handleExecutedCheckAction('{0}','{1}','{2}')",
                                _executedCheckAction, null, null);
                            }

                            if (functionCall != null)
                            {
                                ScriptManager.RegisterStartupScript(upAttachments, upAttachments.GetType(), "HandleCheckStatusChange", functionCall, true);
                            }
                        }
                    }
                }
            }
        }

        private void GvData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int attachmentPk = 0;
            if (e.CommandName == "CheckOut")
            {
                if (int.TryParse(Convert.ToString(e.CommandArgument), out attachmentPk))
                {
                    var attachment = _attachmentOperations.GetEntity(attachmentPk);

                    if (attachment != null)
                    {
                        if (attachment.lock_owner_FK.HasValue)
                        {
                            _executedCheckAction = "Refresh";
                            _executedOnAttachmentPk = attachmentPk;
                            BindAttachments();
                            return;
                        }

                        attachment.lock_date = DateTime.Now;
                        attachment.lock_owner_FK = SessionManager.Instance.CurrentUser.UserID;

                        _attachmentOperations.Save(attachment);

                        if (new[] { "/", "\\", ":", "*", "?", "\"", "<", ">", "|" }.Any(x => attachment.attachmentname.Contains(x)))
                        {
                            _executedCheckAction = "CheckOutFileNameWarning";
                            _executedOnAttachmentPk = attachmentPk;
                        }
                        else
                        {
                            _executedCheckAction = e.CommandName;
                            _executedOnAttachmentPk = attachmentPk;
                        }
                    }
                }
            }
            else if (e.CommandName == "CheckIn")
            {
                if (int.TryParse(Convert.ToString(e.CommandArgument), out attachmentPk))
                {
                    var attachment = _attachmentOperations.GetEntity(attachmentPk);

                    if (attachment != null)
                    {
                        if (!attachment.lock_owner_FK.HasValue || attachment.lock_owner_FK != SessionManager.Instance.CurrentUser.UserID)
                        {
                            _executedCheckAction = "Refresh";
                            _executedOnAttachmentPk = attachmentPk;
                            BindAttachments();
                            return;
                        }

                        Attachment_PK newAttachment = null;

                        newAttachment = _attachmentOperations.GetCheckedInAttachment(attachmentPk, upAttachments.AttachmentSessionId);

                        if (newAttachment != null)
                        {
                            if (AttachmentHelper.GetFileExtension(attachment.attachmentname) != AttachmentHelper.GetFileExtension(newAttachment.attachmentname))
                            {
                                _executedCheckAction = "CheckInInvalidFileType";
                                _executedOnAttachmentPk = attachmentPk;

                                _attachmentOperations.Delete(newAttachment.attachment_PK);
                            }
                            else
                            {
                                attachment.lock_owner_FK = null;
                                attachment.lock_date = null;
                                attachment.disk_file = newAttachment.disk_file;
                                _attachmentOperations.Save(attachment);

                                _attachmentOperations.Delete(newAttachment.attachment_PK);
                            }
                        }
                        else
                        {
                            _executedCheckAction = e.CommandName;
                            _executedOnAttachmentPk = attachmentPk;
                        }
                    }
                }
            }
            else if (e.CommandName == "CancelCheckout")
            {
                if (int.TryParse(Convert.ToString(e.CommandArgument), out attachmentPk))
                {
                    var attachment = _attachmentOperations.GetEntity(attachmentPk);

                    if (attachment != null)
                    {
                        if (!attachment.lock_owner_FK.HasValue || (attachment.lock_owner_FK != SessionManager.Instance.CurrentUser.UserID &&
                            !SessionManager.Instance.CurrentUser.Roles.Any(x => x != null && x.ToLower() == "admin")))
                        {
                            _executedCheckAction = "Refresh";
                            _executedOnAttachmentPk = attachmentPk;
                            BindAttachments();
                            return;
                        }

                        attachment.lock_owner_FK = null;
                        attachment.lock_date = null;

                        _attachmentOperations.Save(attachment);

                        _executedCheckAction = e.CommandName;
                        _executedOnAttachmentPk = attachmentPk;
                    }
                }
            }

            BindAttachments();
        }

        private void BindAttachments()
        {
            var attachmentsForThisSession = Attachment_PK.GetAttachmentsForThisSession(_attachmentOperations, _idDoc, upAttachments.AttachmentSessionId);
            if (attachmentsForThisSession.Count > 0)
            {
                upAttachments.AsyncFileUpload.CssClass = "async-upload-control-items";
                BindAttachments(attachmentsForThisSession);
            }
            else
            {
                upAttachments.AsyncFileUpload.CssClass = "async-upload-control-no-items";
                upAttachments.PnlUploadedFiles.Visible = false;
            }
        }

        void CheckInAsyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            Thread.Sleep(2000);

            var asyncCheckIn = sender as AjaxControlToolkit.AsyncFileUpload;

            if (asyncCheckIn != null)
            {
                int attachmentPk;

                if (int.TryParse(asyncCheckIn.Attributes["data-id"], out attachmentPk))
                {
                    if (asyncCheckIn.HasFile && asyncCheckIn.FileBytes.Count() > 0)
                    {
                        Attachment_PK attachment = null;

                        attachment = _attachmentOperations.GetCheckedInAttachment(attachmentPk, upAttachments.AttachmentSessionId) ?? new Attachment_PK();

                        attachment.attachmentname = asyncCheckIn.FileName;
                        attachment.filetype = asyncCheckIn.ContentType;
                        attachment.type_for_fts = AttachmentHelper.GetFileExtension(attachment.attachmentname);
                        attachment.disk_file = asyncCheckIn.FileBytes;
                        attachment.session_id = System.Guid.NewGuid();
                        attachment.userID = SessionManager.Instance.CurrentUser.UserID;
                        attachment.modified_date = DateTime.Now;
                        attachment.check_in_for_attach_FK = attachmentPk;
                        attachment.check_in_session_id = upAttachments.AttachmentSessionId;
                        _attachmentOperations.Save(attachment);
                    }
                }
            }
        }

        void AsyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            Thread.Sleep(2000);

            if (upAttachments.AsyncFileUpload.HasFile && upAttachments.AsyncFileUpload.FileBytes.Count() > 0)
            {
                var attachment = new Attachment_PK();

                attachment.attachmentname = upAttachments.AsyncFileUpload.FileName;
                attachment.filetype = upAttachments.AsyncFileUpload.ContentType;
                attachment.type_for_fts = AttachmentHelper.GetFileExtension(attachment.attachmentname);
                attachment.disk_file = upAttachments.AsyncFileUpload.FileBytes;
                attachment.session_id = upAttachments.AttachmentSessionId;
                attachment.userID = SessionManager.Instance.CurrentUser.UserID;
                attachment.modified_date = DateTime.Now;
                _attachmentOperations.Save(attachment);

                upAttachments.RefreshAttachments = true;
            }
        }

        void upAttachments_OnDelete(object sender, EventArgs e)
        {
            var button = (sender as ImageButton);

            if (button == null) return;

            int attachmentPkToDelete;
            if (int.TryParse(button.CommandArgument, out attachmentPkToDelete))
            {
                _attachmentPkToDelete = attachmentPkToDelete;
                mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
            }
        }

        #endregion

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.Cancel:
                    _idDoc = ValidationHelper.IsValidInt(Request.QueryString["idDoc"]) ? int.Parse(Request.QueryString["idDoc"]) : (int?)null;
                    if (EntityContext == EntityContext.AuthorisedProduct)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idAuthProd.HasValue && _idDoc.HasValue)
                        {
                            if (From == "AuthProdDocPreview") Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idAuthProd={1}&idDoc={2}", EntityContext, _idAuthProd, _idDoc));
                        }
                        else if (FormType == FormType.New)
                        {
                            if (From == "AuthProdPreview" && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/AuthorisedProductView/Preview.aspx?EntityContext={0}&idAuthProd={1}", EntityContext, _idAuthProd));
                            else if (From == "AuthProd") Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "AuthProdSearch") Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if (From == "ProdAuthProdList" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, _idProd));
                            else if (From == "AuthProdDocList" && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idAuthProd={1}", EntityContext, _idAuthProd));
                        }
                        Response.Redirect(string.Format("~/Views/AuthorisedProductView/List.aspx?EntityContext={0}", EntityContext));
                    }
                    else if (EntityContext == EntityContext.Product)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idProd.HasValue && _idDoc.HasValue)
                        {
                            if (From == "ProdDocPreview") Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idProd={1}&idDoc={2}", EntityContext, _idProd, _idDoc));
                        }
                        else if (FormType == FormType.New)
                        {
                            if (From == "ProdPreview" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                            else if (From == "Prod") Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "ProdSearch") Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if (From == "ProdDocList" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                        }
                        Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                    }
                    else if (EntityContext == EntityContext.Task)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idTask.HasValue && _idDoc.HasValue)
                        {
                            if (From == "TaskDocPreview") Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idTask={1}&idDoc={2}", EntityContext, _idTask, _idDoc));
                        }
                        else if (FormType == FormType.New)
                        {
                            if (From == "TaskPreview" && _idTask.HasValue) Response.Redirect(string.Format("~/Views/TaskView/Preview.aspx?EntityContext={0}&idTask={1}", EntityContext, _idTask));
                            else if (From == "Task") Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "TaskSearch") Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if (From == "TaskDocList" && _idTask.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idTask={1}", EntityContext, _idTask));
                        }
                        Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext));
                    }
                    else if (EntityContext == EntityContext.Project)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idProj.HasValue && _idDoc.HasValue)
                        {
                            if (From == "ProjDocPreview") Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idProj={1}&idDoc={2}", EntityContext, _idTask, _idDoc));
                        }
                        else if (FormType == FormType.New)
                        {
                            if (From == "ProjPreview" && _idProj.HasValue) Response.Redirect(string.Format("~/Views/ProjectView/Preview.aspx?EntityContext={0}&idProj={1}", EntityContext, _idProj));
                            else if (From == "Proj") Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "ProjSearch") Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if (From == "ProjDocList" && _idProj.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idProj={1}", EntityContext, _idProj));
                        }
                        Response.Redirect(string.Format("~/Views/ProjectView/List.aspx?EntityContext={0}", EntityContext));
                    }
                    else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idAct.HasValue && _idDoc.HasValue)
                        {
                            if (From == "ActDocPreview" || From == "ActMyDocPreview") Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idAct={1}&idDoc={2}", EntityContext, _idAct, _idDoc));
                        }
                        else if (FormType == FormType.New)
                        {
                            if ((From == "ActPreview" || From == "ActMyPreview") && _idAct.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                            else if (From == "Act" || From == "ActMy") Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "ActSearch") Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if (From == "ProdActList" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, _idProd));
                            else if ((From == "ActDocList" || From == "ActMyDocList") && _idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                            else if (From == "SubUnitActPreview" && _idSubUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idSubUnit={1}", EntityContext.SubmissionUnit, _idSubUnit));
                            else if (From == "TimeUnitActPreview" && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnit, _idTimeUnit));
                            else if (From == "TimeUnitMyActPreview" && _idTimeUnit.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idTimeUnit={1}", EntityContext.TimeUnitMy, _idTimeUnit));
                        }
                        Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
                    }
                    else if (EntityContext == EntityContext.PharmaceuticalProduct)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idPharmProd.HasValue && _idDoc.HasValue)
                        {
                            if (From == "PharmProdDocPreview") Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idPharmProd={1}&idDoc={2}", EntityContext, _idPharmProd, _idDoc));
                        }
                        else if (FormType == FormType.New)
                        {
                            if (From == "PharmProdPreview" && _idPharmProd.HasValue) Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/Preview.aspx?EntityContext={0}&idPharmProd={1}", EntityContext, _idPharmProd));
                            else if (From == "PharmProd") Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "PharmProdSearch") Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if (From == "ProdPharmProdList" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, _idProd));
                            else if (From == "ProdPharmDocList" && _idPharmProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idPharmProd={1}", EntityContext, _idPharmProd));
                        }
                        Response.Redirect(string.Format("~/Views/PharmaceuticalProductView/List.aspx?EntityContext={0}", EntityContext));
                    }
                    else if (EntityContext == EntityContext.Document)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idDoc.HasValue)
                        {
                            if (From == "DocPreview") Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}", EntityContext, _idDoc));
                        }
                    }
                    Response.Redirect(string.Format("~/Views/DocumentAllView/List.aspx?EntityContext={0}", EntityContext.Document));

                    break;
                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedDocument = SaveForm(null);
                        if (savedDocument is Document_PK)
                        {
                            var document = savedDocument as Document_PK;
                            if (document.document_PK.HasValue)
                            {
                                MasterPage.OneTimePermissionToken = Permission.View;
                                if (EntityContext == EntityContext.Product && _idProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idProd={2}&From={3}Document", EntityContext, document.document_PK, _idProd, FormType));
                                else if (EntityContext == EntityContext.AuthorisedProduct && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idAuthProd={2}&From={3}Document", EntityContext, document.document_PK, _idAuthProd, FormType));
                                else if (EntityContext == EntityContext.PharmaceuticalProduct && _idPharmProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idPharmProd={2}&From={3}Document", EntityContext, document.document_PK, _idPharmProd, FormType));
                                else if (EntityContext == EntityContext.Project && _idProj.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idProj={2}&From={3}Document", EntityContext, document.document_PK, _idProj, FormType));
                                else if (EntityContext == EntityContext.Activity && _idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idAct={2}&From={3}Document", EntityContext, document.document_PK, _idAct, FormType));
                                else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idAct={2}&From={3}Document", EntityContext, document.document_PK, _idAct, FormType));
                                else if (EntityContext == EntityContext.Task && _idTask.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idTask={2}&From={3}Document", EntityContext, document.document_PK, _idTask, FormType));
                                else if (EntityContext == EntityContext.Document) Response.Redirect(string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&From={2}Document", EntityContext.Document, document.document_PK, FormType));
                            }
                        }
                        Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}", EntityContext.Document));
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

        #region Reminders

        private void LnkSetEffectiveStartDateReminder_Click(object sender, EventArgs e)
        {
            SetReminder(StringOperations.GetRelatedName(dtEffectiveStartDate.Label), dtEffectiveStartDate.Text);
        }

        public void LnkSetEffectiveEndDateReminder_Click(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(dtEffectiveEndDate.Label)), dtEffectiveEndDate.Text);
        }

        public void Reminder_OnConfirmInputButtonProcess_Click(object sender, EventArgs e)
        {
            Reminder_PK reminder = Reminder.ReminderVs;
            reminder.user_FK = SessionManager.Instance.CurrentUser.UserID;

            string navigate_url = string.Empty;
            int? relatedEntityFk = null;
            var TableName = ReminderTableName.NULL;
            if (_idDoc.HasValue)
            {
                if (EntityContext == EntityContext.AuthorisedProduct && _idAuthProd.HasValue)
                {
                    TableName = ReminderTableName.AUTHORISED_PRODUCT;
                    relatedEntityFk = _idAuthProd;
                    navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idAuthProd={1}&idDoc={2}", EntityContext, _idAuthProd, _idDoc);
                }
                else if (EntityContext == EntityContext.Product && _idProd.HasValue)
                {
                    TableName = ReminderTableName.PRODUCT;
                    relatedEntityFk = _idProd;
                    navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idProd={1}&idDoc={2}", EntityContext, _idProd, _idDoc);
                }
                else if (EntityContext == EntityContext.PharmaceuticalProduct && _idPharmProd.HasValue)
                {
                    TableName = ReminderTableName.PHARMACEUTICAL_PRODUCT;
                    relatedEntityFk = _idPharmProd;
                    navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idPharmProd={1}&idDoc={2}", EntityContext, _idPharmProd, _idDoc);
                }
                else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy && _idAct.HasValue)
                {
                    TableName = ReminderTableName.ACTIVITY;
                    relatedEntityFk = _idAct;
                    navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idAct={1}&idDoc={2}", EntityContext.Activity, _idAct, _idDoc);
                }
                else if (EntityContext == EntityContext.Task && _idTask.HasValue)
                {
                    TableName = ReminderTableName.TASK;
                    relatedEntityFk = _idTask;
                    navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idTask={1}&idDoc={2}", EntityContext, _idTask, _idDoc);
                }
                else if (EntityContext == EntityContext.Project && _idProj.HasValue)
                {
                    TableName = ReminderTableName.PROJECT;
                    relatedEntityFk = _idProj;
                    navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idProj={1}&idDoc={2}", EntityContext, _idProj, _idDoc);
                }
                else if (EntityContext == EntityContext.Document)
                {
                    TableName = ReminderTableName.DOCUMENT;
                    relatedEntityFk = _idDoc;
                    navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}", EntityContext, _idDoc);
                }
            }

            reminder.TableName = TableName;
            reminder.related_entity_FK = relatedEntityFk;
            reminder.entity_FK = _idDoc;
            reminder.navigate_url = navigate_url;

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

        #region Related entities

        private void LbSrRelatedEntities_OnOkButtonClick(object sender, FormEventArgs<List<int>> e)
        {
            foreach (var selectedId in lbSrRelatedEntities.Searcher.SelectedItems)
            {
                if (lbSrRelatedEntities.LbInput.Items.FindByValue(selectedId.ToString()) != null) continue;
                var text = Constant.MissingValue;
                if (SearchType == SearchType.Product)
                {
                    var product = _productOperations.GetEntity(selectedId);
                    text = product.GetNameFormatted();
                }
                else if (SearchType == SearchType.AuthorisedProduct)
                {
                    var authorisedProduct = _authorisedProductOperations.GetEntity(selectedId);
                    text = !string.IsNullOrWhiteSpace(authorisedProduct.product_name) ? authorisedProduct.product_name : Constant.MissingValue;
                }
                else if (SearchType == SearchType.PharmaceuticalProduct)
                {
                    var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(selectedId);
                    text = !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.MissingValue;
                }
                else if (SearchType == SearchType.Activity)
                {
                    var activity = _activityOperations.GetEntity(selectedId);
                    text = !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.MissingValue;
                }
                else if (SearchType == SearchType.Project)
                {
                    var project = _projectOperations.GetEntity(selectedId);
                    text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.MissingValue;
                }
                else if (SearchType == SearchType.Task)
                {
                    var task = _taskOperations.GetEntity(selectedId);
                    if (task != null)
                    {
                        var taskName = _taskNameOperations.GetEntity(task.task_name_FK);
                        text = taskName != null && !string.IsNullOrWhiteSpace(taskName.task_name) ? taskName.task_name : Constant.MissingValue;
                    }
                    else text = Constant.MissingValue;
                }

                lbSrRelatedEntities.LbInput.Items.Add(new ListItem(text, selectedId.ToString()));
            }
        }

        #endregion

        #region Document type

        private void ddlDocumentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            HandleDocumentType();

            var documentType = ddlDocumentType.SelectedItem;
            if ((SearchType == SearchType.AuthorisedProduct || SearchType == SearchType.Null) && documentType != null && documentType.Text.ToLower() == "ppi")
            {
                FillDdlAttachmentType("DOC");
                FillDdlRegulatoryStatus("PPI");
                FillDdlVersionLabel("N/A");
                FillDdlVersionNumber("PPI");

                StylizeArticle57RelevantControls(true);
                _selectedPrevDocumentType = documentType.Text;
            }
            else
            {
                if (!string.IsNullOrEmpty(_selectedPrevDocumentType) && _selectedPrevDocumentType.ToLower() == "ppi")
                {
                    FillDdlRegulatoryStatus(null);
                    FillDdlVersionNumber(null);
                    _selectedPrevDocumentType = documentType != null ? documentType.Text : "";
                }
            }
        }

        private void HandleDocumentType()
        {
            var documentType = ddlDocumentType.SelectedItem;
            if ((SearchType == SearchType.AuthorisedProduct || SearchType == SearchType.Null) && documentType != null && documentType.Text.ToLower() == "ppi")
            {
                SetVisibleFieldsDocumentTypePpi();
                SetRequiredFieldsDocumentTypePpi();
                if (!IsPostBack) StylizeArticle57RelevantControls(true);

                if (string.IsNullOrWhiteSpace(dtVersionDate.Text)) dtVersionDate.Text = DateTime.Now.ToString(Constant.DateTimeFormat);
                if (string.IsNullOrWhiteSpace(txtDocumentName.Text)) txtDocumentName.Text = "ppiattachment";
            }
            else
            {
                SetVisibleFieldsDocumentTypeNonPpi();
                SetRequiredFieldsDocumentTypeNonPpi();

                Article57Reporting.RemoveAllArticle57CssClasses(pnlForm);
            }

            SetVisibleFieldsDocumentTypeEDMS();
        }

        #endregion

        #endregion

        #region Support methods

        private void HideReminders()
        {
            dtEffectiveStartDate.ShowReminder = false;
            dtEffectiveEndDate.ShowReminder = false;
        }

        public void RefreshReminderStatus()
        {
            var tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.DOCUMENT);

            AlerterHelper.RefreshReminderStatus(_reminderOperations, MasterPage, new List<IReminderCustomControl> { dtEffectiveStartDate, dtEffectiveEndDate, }, tableName, _idDoc);
        }

        public void SetReminder(String attributeName, String attributeValue)
        {
            var reminder_type = string.Empty;
            if (SearchType == SearchType.AuthorisedProduct) reminder_type = "AP Document";
            else if (SearchType == SearchType.Product) reminder_type = "P Document";
            else if (SearchType == SearchType.PharmaceuticalProduct) reminder_type = "PP Document";
            else if (SearchType == SearchType.Activity || EntityContext == EntityContext.ActivityMy) reminder_type = "A Document";
            else if (SearchType == SearchType.Task) reminder_type = "Task Document";
            else if (SearchType == SearchType.Project) reminder_type = "Project Document";

            var reminder = new Reminder_PK
            {
                reminder_type = reminder_type,
                reminder_name = lblPrvParentEntity.Text,
                related_attribute_name = attributeName,
                related_attribute_value = attributeValue
            };

            Reminder.ReminderVs = reminder;
            Reminder.ShowModalPopup("Set alert");
            RefreshReminderStatus();
        }

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
            Location_PK location = null;

            if (EntityContext == EntityContext.Product) location = LocationManager.Instance.GetLocationByName("ProdDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.AuthorisedProduct) location = LocationManager.Instance.GetLocationByName("AuthProdDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.PharmaceuticalProduct) location = LocationManager.Instance.GetLocationByName("PharmProdDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Activity) location = LocationManager.Instance.GetLocationByName("ActDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.ActivityMy) location = LocationManager.Instance.GetLocationByName("ActMyDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Project) location = LocationManager.Instance.GetLocationByName("ProjDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Task) location = LocationManager.Instance.GetLocationByName("TaskDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Document) location = LocationManager.Instance.GetLocationByName("DocPreview", CacheManager.Instance.AppLocations);

            if (location != null)
            {
                MasterPage.TabMenu.TabControls.Clear();
                tabMenu.Visible = true;
                tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void GenerateTopMenuItems()
        {
            Location_PK location = null;

            if (EntityContext == EntityContext.Product) location = LocationManager.Instance.GetLocationByName("ProdDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.AuthorisedProduct) location = LocationManager.Instance.GetLocationByName("AuthProdDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.PharmaceuticalProduct) location = LocationManager.Instance.GetLocationByName("AuthProdDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Project) location = LocationManager.Instance.GetLocationByName("ProjDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Activity) location = LocationManager.Instance.GetLocationByName("ActDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.ActivityMy) location = LocationManager.Instance.GetLocationByName("ActMyDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Task) location = LocationManager.Instance.GetLocationByName("TaskDocList", CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Document) location = LocationManager.Instance.GetLocationByName("DocPreview", CacheManager.Instance.AppLocations);

            var topLevelParent = MasterPage.FindTopLevelParent(location);

            if (location != null)
            {
                MasterPage.CurrentLocation = location;
                MasterPage.TopMenu.GenerateNewTopMenu(CacheManager.Instance.AppLocations, topLevelParent, location);
            }
        }

        private void StylizeArticle57RelevantControls(bool? isArticle57Relevant)
        {
            Article57Reporting.RemoveAllArticle57CssClasses(pnlForm);

            var newForm = FormType == FormType.New;

            txtDocumentName.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, (newForm || string.IsNullOrEmpty(txtDocumentName.Text)), isArticle57Relevant));
            ddlDocumentType.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, (newForm || string.IsNullOrEmpty(ddlDocumentType.HasValue())), isArticle57Relevant));
            ddlVersionNumber.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, (newForm || string.IsNullOrEmpty(ddlVersionNumber.HasValue())), isArticle57Relevant));
            ddlRegulatoryStatus.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, (newForm || string.IsNullOrEmpty(ddlRegulatoryStatus.HasValue())), isArticle57Relevant));
            ddlAttachmentType.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, (newForm || string.IsNullOrEmpty(ddlAttachmentType.HasValue())), isArticle57Relevant));
            lbLanguageCodes.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, (newForm || string.IsNullOrEmpty(lbLanguageCodes.HasValue())), isArticle57Relevant));
            dtVersionDate.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, (newForm || string.IsNullOrEmpty(dtVersionDate.Text)), isArticle57Relevant));
            upAttachments.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, newForm || upAttachments.IsEmpty, isArticle57Relevant));
        }

        private void SetVisibleFieldsDocumentTypeEDMS()
        {
            var isEDMSDocument = _isEDMSDocument.HasValue && _isEDMSDocument.Value;

            var documentType = ddlDocumentType.SelectedItem;
            var isPPIDocument = (SearchType == SearchType.AuthorisedProduct || SearchType == SearchType.Null) && documentType != null && documentType.Text.ToLower() == "ppi";
            ddlAttachmentType.Visible = isEDMSDocument || isPPIDocument;
            txtEDMSBindingRule.Visible = isEDMSDocument;
            dtEDMSModifyDate.Visible = isEDMSDocument;
            SetEDMSSecurityPermission();

            FillDdlAttachmentType(ddlAttachmentType.SelectedValue);
        }

        private void SetRequiredFieldsDocumentTypeNonPpi()
        {
            lbSrRelatedEntities.Required = true;
            txtDocumentName.Required = true;
            ddlDocumentType.Required = true;
            ddlVersionNumber.Required = true;
            ddlResponsibleUser.Required = true;
            ddlVersionLabel.Required = true;
            ddlRegulatoryStatus.Required = true;
            lbLanguageCodes.Required = false;
            upAttachments.Required = false;
        }

        private void SetVisibleFieldsDocumentTypeNonPpi()
        {
            ddlAttachmentType.Visible = false;
            dtVersionDate.Visible = false;
            txtEvcode.Visible = false;
        }

        private void SetRequiredFieldsDocumentTypePpi()
        {
            lbSrRelatedEntities.Required = true;
            txtDocumentName.Required = true;
            ddlDocumentType.Required = true;
            ddlVersionNumber.Required = true;
            ddlResponsibleUser.Required = true;
            ddlVersionLabel.Required = true;
            ddlRegulatoryStatus.Required = true;
            ddlAttachmentType.Required = true;
            lbLanguageCodes.Required = true;
            upAttachments.Required = true;
        }

        private void SetVisibleFieldsDocumentTypePpi()
        {
            ddlAttachmentType.Visible = true;
            dtVersionDate.Visible = true;
            txtEvcode.Visible = true;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            if (MasterPage.RefererLocation != null)
            {
                var isPermittedInsertDocument = SecurityHelper.IsPermittedAny(new List<Permission> { Permission.InsertDocument, Permission.SaveAsDocument, Permission.EditDocument }, MasterPage.RefererLocation);

                if (isPermittedInsertDocument)
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

            SecurityPageSpecificMy(_isResponsibleUser);

            txtEvcode.Enabled = false;
            HandleDocumentType();

            return true;
        }

        #endregion
    }
}