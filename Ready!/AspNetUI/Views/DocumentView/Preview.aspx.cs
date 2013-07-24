using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Transactions;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.NanokinetikEDMS;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Interface;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUI.Views.Shared.UserControl.Popup;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Views.DocumentView
{
    public partial class Preview : PreviewPage
    {
        #region Declarations

        private int? _idAct;
        private int? _idProd;
        private int? _idAuthProd;
        private int? _idTask;
        private int? _idProj;
        private int? _idDoc;
        private int? _idPharmProd;
        private bool? _isResponsibleUser;

        private IProduct_PKOperations _productOperations;
        private IPharmaceutical_product_PKOperations _pharmaceuticalProductOperations;
        private IAuthorisedProductOperations _authorisedProductOperations;
        private IActivity_PKOperations _activityOperations;
        private ITask_PKOperations _taskOperations;
        private IProject_PKOperations _projectOperations;
        private IPerson_PKOperations _personOperations;
        private IType_PKOperations _typeOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private IReminder_PKOperations _reminderOperations;
        private IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations;
        private IReminder_date_PKOperations _reminderDateOperations;
        private IDocument_PKOperations _documentOperations;
        private ILanguagecode_PKOperations _languageCodeOperations;
        private IAttachment_PKOperations _attachmentOperations;
        private IAp_document_mn_PKOperations _authorisedProductDocumentMnOperations;
        private IProduct_document_mn_PKOperations _productDocumentMnOperations;
        private IActivity_document_PKOperations _activityDocumentMnOperations;
        private ITask_document_PKOperations _taskDocumentMnOperations;
        private IProject_document_PKOperations _projectDocumentMnOperations;
        private IPp_document_PKOperations _pharmaceuticalProductDocumentMnOperations;
        private ITask_name_PKOperations _taskNameOperations;
        private IUSEROperations _userOperations;
        SearchType _searchType = SearchType.Null;

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

            _idDoc = ValidationHelper.IsValidInt(Request.QueryString["idDoc"]) ? int.Parse(Request.QueryString["idDoc"]) : (int?)null;
            _idAuthProd = ValidationHelper.IsValidInt(Request.QueryString["idAuthProd"]) ? int.Parse(Request.QueryString["idAuthProd"]) : (int?)null;
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;
            _idPharmProd = ValidationHelper.IsValidInt(Request.QueryString["idPharmProd"]) ? int.Parse(Request.QueryString["idPharmProd"]) : (int?)null;
            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;
            _idTask = ValidationHelper.IsValidInt(Request.QueryString["idTask"]) ? int.Parse(Request.QueryString["idTask"]) : (int?)null;
            _idProj = ValidationHelper.IsValidInt(Request.QueryString["idProj"]) ? int.Parse(Request.QueryString["idProj"]) : (int?)null;

            _productOperations = new Product_PKDAL();
            _pharmaceuticalProductOperations = new Pharmaceutical_product_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();
            _activityOperations = new Activity_PKDAL();
            _taskOperations = new Task_PKDAL();
            _projectOperations = new Project_PKDAL();
            _personOperations = new Person_PKDAL();
            _typeOperations = new Type_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _reminderOperations = new Reminder_PKDAL();
            _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            _reminderDateOperations = new Reminder_date_PKDAL();
            _documentOperations = new Document_PKDAL();
            _languageCodeOperations = new Languagecode_PKDAL();
            _attachmentOperations = new Attachment_PKDAL();
            _authorisedProductDocumentMnOperations = new Ap_document_mn_PKDAL();
            _productDocumentMnOperations = new Product_document_mn_PKDAL();
            _pharmaceuticalProductDocumentMnOperations = new Pp_document_PKDAL();
            _taskDocumentMnOperations = new Task_document_PKDAL();
            _projectDocumentMnOperations = new Project_document_PKDAL();
            _activityDocumentMnOperations = new Activity_document_PKDAL();
            _taskNameOperations = new Task_name_PKDAL();
            _userOperations = new USERDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            lblPrvEffectiveStartDate.LnkSetReminder.Click += LblPrvEffectiveStartDateSetReminder_OnClick;
            lblPrvEffectiveEndDate.LnkSetReminder.Click += LblPrvEffectiveEndDateSetReminder_OnClick;
            Reminder.OnConfirmInputButtonProcess_Click += Reminder_OnConfirmInputButtonProcess_Click;

            btnDelete.Click += btnDelete_OnClick;
            mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
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
            if (From.NotIn(FormType.New + "Document", FormType.Edit + "Document", FormType.SaveAs + "Document")) SynchronizeEDMSDocument();
        }

        private void SynchronizeEDMSDocument()
        {
            if (!_idDoc.HasValue) return;

            var document = _documentOperations.GetEntity(_idDoc);

            if (document == null) return;

            if (document.EDMSDocument == null || !document.EDMSDocument.Value) return;

            var username = SessionManager.Instance.CurrentUser.Username; 
            var documentVersion = document.EDMSVersionNumber;

            if (document.EDMSBindingRule.ToLower() == EDMSDocumentVersion.Current.ToString().ToLower()) documentVersion = EDMSDocumentVersion.Current.ToString().ToUpper();
               
            var documentFormat = _typeOperations.GetEntity(document.attachment_type_FK);
            var _formatType = formatType.ORIGINAL;
            if (documentFormat != null && documentFormat.name.Trim().ToLower() == "pdf") _formatType = formatType.PDF;

            if (!string.IsNullOrWhiteSpace(document.EDMSDocumentId))
            {
                try
                {
                    EDMSDocument edmsDocument = null;
                    using (var edmsWsClient = new EDMS_WSClient())
                    {
                        edmsDocument = edmsWsClient.getDocument(document.EDMSDocumentId, username, documentVersion, _formatType);
                    }

                    if (edmsDocument == null)
                    {
                        //MasterPage.ModalPopup.ShowModalPopup("Error!", string.Format("There was a problem during automated EDMS document synchronization. Returned document is NULL. <br />Method call parameters: edmsWsClient.getDocument({0}, {1}, {2}, {3})", document.EDMSDocumentId, username, documentVersion, _formatType));
                        return;
                    }

                    document.name = edmsDocument.documentName;

                    var updatingErrorMessage = string.Empty;

                    var versionNumber = TypePkHelper.GetTypeValue(_typeOperations,
                                                                  new[] {Constant.TypeGroupName.VersionNumber},
                                                                  edmsDocument.versionNumber);
                    //if (!versionNumber.HasValue) updatingErrorMessage += "Returned EDMS document version number does not exist in READY! Value not updated.<br />";
                    document.version_number = versionNumber;

                    var versionLabel = TypePkHelper.GetTypeValue(_typeOperations,
                                                                 new[] {Constant.TypeGroupName.VersionLabel},
                                                                 edmsDocument.versionLabel.ToUpper());
                    //if (!versionLabel.HasValue) updatingErrorMessage += "Returned EDMS document version label does not exist in READY! Value not updated.<br />"; 
                    document.version_label = versionLabel;

                    var attachmentTypeFk = TypePkHelper.GetTypeValue(_typeOperations,
                                                                     new[]
                                                                         {
                                                                             Constant.TypeGroupName.AttachmentType,
                                                                             Constant.TypeGroupName.AttachmentTypeEDMS
                                                                         },
                                                                     edmsDocument.format.ToUpper(),
                                                                     Constant.TypeGroupName.AttachmentTypeEDMS);
                    //if (!attachmentTypeFk.HasValue) updatingErrorMessage += "Returned EDMS document atachment type does not exist in READY! Value not updated.<br />";
                    document.attachment_type_FK = attachmentTypeFk;

                    document.EDMSModifyDate = edmsDocument.modifyDate;
                    document.EDMSBindingRule = edmsDocument.currentVersion ? "Current" : "Selected version";

                    _documentOperations.Save(document);

                    //MasterPage.ModalPopup.ShowModalPopup("Success!", string.Format("EDMS document synchronized. <br />Method call parameters: edmsWsClient.getDocument({0}, {1}, {2}, {3})<br />{4}", document.EDMSDocumentId, username, documentVersion, _formatType, updatingErrorMessage));    
                }
                catch (Exception)
                {
                    //MasterPage.ModalPopup.ShowModalPopup("Error!", string.Format("There was a problem during automated EDMS document synchronization. <br />Method call parameters: edmsWsClient.getDocument({0}, {1}, {2}, {3})", document.EDMSDocumentId, username, documentVersion, _formatType));    
                }
            }
        }

        void SetFormControlsDefaults(object arg)
        {
            if (!_idDoc.HasValue) return;

            switch (EntityContext)
            {
                case EntityContext.Product:
                    btnDelete.Visible = _productDocumentMnOperations.AbleToDeleteEntity(_idDoc.Value);
                    break;
                case EntityContext.AuthorisedProduct:
                    btnDelete.Visible = _authorisedProductDocumentMnOperations.AbleToDeleteEntity(_idDoc.Value);
                    break;
                case EntityContext.Activity:
                case EntityContext.ActivityMy:
                    btnDelete.Visible = _activityDocumentMnOperations.AbleToDeleteEntity(_idDoc.Value);
                    break;
                case EntityContext.PharmaceuticalProduct:
                    btnDelete.Visible = _pharmaceuticalProductDocumentMnOperations.AbleToDeleteEntity(_idDoc.Value);
                    break;
                case EntityContext.Project:
                    btnDelete.Visible = _projectDocumentMnOperations.AbleToDeleteEntity(_idDoc.Value);
                    break;
                case EntityContext.Task:
                    btnDelete.Visible = _taskDocumentMnOperations.AbleToDeleteEntity(_idDoc.Value);
                    break;
                case EntityContext.Document:
                    var relatedEntitiesDataSet = _documentOperations.GetDocumentRelatedEntities(_idDoc.Value);
                    btnDelete.Visible = relatedEntitiesDataSet != null && relatedEntitiesDataSet.Tables.Count > 0 && relatedEntitiesDataSet.Tables[0].Rows.Count == 1;
                    break;
            }

            pnlFooter.CssClass = btnDelete.Visible ? "clear previewBottom" : "clear previewBottom35";

            BindDynamicControls(null);
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            if (!_idDoc.HasValue) return;

            var document = _documentOperations.GetEntity(_idDoc);

            if (document == null || !document.document_PK.HasValue) return;

            if (EntityContext == EntityContext.Document)
            {
                lblPrvParentEntity.Text = document.name;
                lblPrvParentEntity.Label = "Document:";
            }

            //---------------------------------------------------------------LEFT PANE --------------------------------------------------------------
            // Document name
            lblPrvDocumentName.Text = !string.IsNullOrWhiteSpace(document.name) ? document.name : Constant.ControlDefault.LbPrvText;

            // Document description
            lblPrvDescription.Text = !string.IsNullOrWhiteSpace(document.description) ? document.description : Constant.ControlDefault.LbPrvText;

            // Document type
            BindDocumentType(document.type_FK);

            // Version number
            BindVersionNumber(document.version_number);

            // Responsible user
            BindResponsibleUser(document.person_FK);

            // Version label
            BindVersionLabel(document.version_label);

            // Document name
            lblPrvDocumentNumber.Text = !string.IsNullOrWhiteSpace(document.document_code) ? document.document_code : Constant.ControlDefault.LbPrvText;

            // Regulatory status
            BindRegulatoryStatus(document.regulatory_status);

            // Comment
            lblPrvComment.Text = !string.IsNullOrWhiteSpace(document.comment) ? document.comment : Constant.ControlDefault.LbPrvText;

            // Attachment type
            BindAttachmentType(document.attachment_type_FK);

            // Binding rule
            lblPrvEDMSBindingRule.Text = !string.IsNullOrWhiteSpace(document.EDMSBindingRule) ? document.EDMSBindingRule : Constant.ControlDefault.LbPrvText;

            // Language code
            BindLanguageCodes(document.document_PK);

            //---------------------------------------------------------------RIGHT PANE --------------------------------------------------------------
            // Change date
            lblPrvChangeDate.Text = document.change_date.HasValue ? document.change_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Effective start date
            lblPrvEffectiveStartDate.Text = document.effective_start_date.HasValue ? document.effective_start_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Effective end date
            lblPrvEffectiveEndDate.Text = document.effective_end_date.HasValue ? document.effective_end_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Modify date
            lblPrvEDMSModifyDate.Text = document.EDMSModifyDate.HasValue ? document.EDMSModifyDate.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Version date
            lblPrvVersionDate.Text = document.version_date.HasValue ? document.version_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Last change
            lblPrvLastChange.Text = LastChange.GetFormattedString(document.document_PK, "DOCUMENT", _lastChangeOperations, _personOperations);

            var user = _userOperations.GetEntity(SessionManager.Instance.CurrentUser.UserID);
            if (user != null) _isResponsibleUser = document.person_FK == user.Person_FK;
        }

        private void BindAttachments(int? documentPk)
        {
            if (!documentPk.HasValue) return;
            var attachments = _attachmentOperations.GetAttachmentsForDocument(documentPk.Value);
            if (attachments.Any())
            {
                lblPrvAttachments.ShowLinks = true;
                lblPrvAttachments.TextBold = true;
                var pnlLinks = lblPrvAttachments.PnlLinks;
                int attachmentCount = attachments.Count;
                foreach (var attachment in attachments)
                {
                    if (!attachment.attachment_PK.HasValue) continue;
                    var hlAttachment = new HyperLink();
                    hlAttachment.ID = string.Format("attachment_{0}", attachment.attachment_PK);

                    var rootLocation = Support.LocationManager.Instance.GetLocationByName("Root", Support.CacheManager.Instance.AppLocations);
                    if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation))
                    {
                        if (string.IsNullOrWhiteSpace(attachment.EDMSDocumentId)) hlAttachment.NavigateUrl = string.Format("~/Views/Business/FileDownload.ashx?attachID={0}", attachment.attachment_PK);
                        else hlAttachment.NavigateUrl = string.Format("~/Views/Business/FileDownload.ashx?attachID={0}|{{{1};{2};{3}}}", attachment.attachment_PK, attachment.EDMSDocumentId, attachment.EDMSBindingRule, attachment.EDMSAttachmentFormat);
                    }

                    hlAttachment.Text = !string.IsNullOrWhiteSpace(attachment.attachmentname) ? attachment.attachmentname : Constant.MissingValue;
                    pnlLinks.Controls.Add(hlAttachment);
                    if (--attachmentCount > 0) pnlLinks.Controls.Add(new LiteralControl("<br />"));
                }
            }
            else
            {
                lblPrvAttachments.Text = Constant.ControlDefault.LbPrvText;
            }
        }

        private void BindLanguageCodes(int? documentPk)
        {
            var languageCodes = _languageCodeOperations.GetLanguageCodeByDocument(documentPk);
            var rowNum = languageCodes.Count;
            if (rowNum != 0) lblPrvLanguageCode.Text = string.Empty;
            foreach (var lc in languageCodes) lblPrvLanguageCode.Text += (--rowNum) != 0 ? (lc.code ?? "") + ", " : (lc.code ?? "");
        }

        private void BindAttachmentType(int? attachmentTypeFk)
        {
            if (!attachmentTypeFk.HasValue) return;
            var type = _typeOperations.GetEntity(attachmentTypeFk);
            var typeName = type != null ? type.name : null;
            lblPrvAttachmentType.Text = !string.IsNullOrWhiteSpace(typeName) ? typeName : Constant.ControlDefault.LbPrvText;
        }

        private void BindRegulatoryStatus(int? regulatoryStatus)
        {
            if (!regulatoryStatus.HasValue) return;
            var type = _typeOperations.GetEntity(regulatoryStatus);
            var typeName = type != null ? type.name : null;
            lblPrvRegulatoryStatus.Text = !string.IsNullOrWhiteSpace(typeName) ? typeName : Constant.ControlDefault.LbPrvText;
        }

        private void BindVersionLabel(int? versionLabel)
        {
            if (!versionLabel.HasValue) return;
            var type = _typeOperations.GetEntity(versionLabel);
            var typeName = type != null ? type.name : null;
            lblPrvVersionLabel.Text = !string.IsNullOrWhiteSpace(typeName) ? typeName : Constant.ControlDefault.LbPrvText;
        }

        private void BindVersionNumber(int? versionNumber)
        {
            if (!versionNumber.HasValue) return;
            var type = _typeOperations.GetEntity(versionNumber);
            var typeName = type != null ? type.name : null;
            lblPrvVersionNumber.Text = !string.IsNullOrWhiteSpace(typeName) ? typeName : Constant.ControlDefault.LbPrvText;
        }

        private void BindDocumentType(int? typeFk)
        {
            if (!typeFk.HasValue) return;
            var type = _typeOperations.GetEntity(typeFk);
            var typeName = type != null ? type.name : null;
            lblPrvDocumentType.Text = !string.IsNullOrWhiteSpace(typeName) ? typeName : Constant.ControlDefault.LbPrvText;
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idDoc.HasValue) return;

            HandleDocumentType(_idDoc.Value);

            BindRelatedEntities(_idDoc.Value);

            // Attachment(s)
            BindAttachments(_idDoc);

            // Coloring
            if (EntityContext == EntityContext.AuthorisedProduct && lblPrvDocumentType.Text.Trim().ToLower() == "ppi" ||
                EntityContext == EntityContext.Document && lblPrvDocumentType.Text.Trim().ToLower() == "ppi") StylizeArticle57RelevantControls(true);

            RefreshReminderStatus();
        }

        private void BindRelatedEntities(int idDoc)
        {
            var pnlLinks = lblPrvRelatedEntityName.PnlLinks;

            var authorisedProductDocumentMnList = _authorisedProductDocumentMnOperations.GetAuthorizedProductsByDocumentFK(idDoc);
            if (authorisedProductDocumentMnList != null && authorisedProductDocumentMnList.Any())
            {
                lblPrvRelatedEntityName.ShowLinks = true;
                lblPrvRelatedEntityName.Label = "Authorised product:";
                _searchType = SearchType.AuthorisedProduct;
                if (EntityContext == EntityContext.AuthorisedProduct) lblPrvParentEntity.Label = "Authorised product:";
                foreach (var item in authorisedProductDocumentMnList)
                {
                    if (!item.ap_FK.HasValue) continue;
                    var authorisedProduct = _authorisedProductOperations.GetEntity(item.ap_FK);
                    if (authorisedProduct != null && authorisedProduct.ap_PK.HasValue)
                    {
                        if (EntityContext == EntityContext.AuthorisedProduct) lblPrvParentEntity.Text = !string.IsNullOrWhiteSpace(authorisedProduct.product_name) ? authorisedProduct.product_name : Constant.MissingValue;
                        var hlAuthorisedProduct = new HyperLink
                        {
                            ID = string.Format("authorisedProduct_{0}", authorisedProduct.ap_PK),
                            NavigateUrl = string.Format("~/Views/AuthorisedProductView/Preview.aspx?EntityContext={0}&idAuthProd={1}", EntityContext.AuthorisedProduct, authorisedProduct.ap_PK),
                            Text = !string.IsNullOrWhiteSpace(authorisedProduct.product_name) ? authorisedProduct.product_name : Constant.MissingValue
                        };

                        pnlLinks.Controls.Add(hlAuthorisedProduct);
                        pnlLinks.Controls.Add(new LiteralControl("<br />"));
                    }
                }
                return;
            }

            var productDocumentMnList = _productDocumentMnOperations.GetProductsByDocumentFK(idDoc);
            if (productDocumentMnList != null && productDocumentMnList.Any())
            {
                lblPrvRelatedEntityName.ShowLinks = true;
                lblPrvRelatedEntityName.Label = "Product:";
                _searchType = SearchType.Product;
                if (EntityContext == EntityContext.Product) lblPrvParentEntity.Label = "Product:";
                foreach (var item in productDocumentMnList)
                {
                    if (!item.product_FK.HasValue) continue;
                    var product = _productOperations.GetEntity(item.product_FK);
                    if (product != null && product.product_PK.HasValue)
                    {
                        if (EntityContext == EntityContext.Product) lblPrvParentEntity.Text = !string.IsNullOrWhiteSpace(product.name) ? product.name : Constant.MissingValue;
                        var hlProduct = new HyperLink
                        {
                            ID = string.Format("authorisedProduct_{0}", product.product_PK),
                            NavigateUrl = string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, product.product_PK),
                            Text = !string.IsNullOrWhiteSpace(product.name) ? product.name : Constant.MissingValue
                        };

                        pnlLinks.Controls.Add(hlProduct);
                        pnlLinks.Controls.Add(new LiteralControl("<br />"));
                    }
                }
                return;
            }

            var projectDocumentMnList = _projectDocumentMnOperations.GetProjectMNByDocumentFK(idDoc);
            if (projectDocumentMnList != null && projectDocumentMnList.Any())
            {
                lblPrvRelatedEntityName.ShowLinks = true;
                lblPrvRelatedEntityName.Label = "Project:";
                _searchType = SearchType.Project;
                if (EntityContext == EntityContext.Project) lblPrvParentEntity.Label = "Project:";
                foreach (var item in projectDocumentMnList)
                {
                    if (!item.project_FK.HasValue) continue;
                    var project = _projectOperations.GetEntity(item.project_FK);
                    if (project != null && project.project_PK.HasValue)
                    {
                        if (EntityContext == EntityContext.Project) lblPrvParentEntity.Text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.MissingValue;
                        var hlProject = new HyperLink
                        {
                            ID = string.Format("project_{0}", project.project_PK),
                            NavigateUrl = string.Format("~/Views/ProjectView/Preview.aspx?EntityContext={0}&idProj={1}", EntityContext.Project, project.project_PK),
                            Text = !string.IsNullOrWhiteSpace(project.name) ? project.name : Constant.MissingValue
                        };

                        pnlLinks.Controls.Add(hlProject);
                        pnlLinks.Controls.Add(new LiteralControl("<br />"));
                    }
                }
                return;
            }

            var activityDocumentMnList = _activityDocumentMnOperations.GetActivitiesMNByDocument(idDoc);
            if (activityDocumentMnList != null && activityDocumentMnList.Any())
            {
                lblPrvRelatedEntityName.ShowLinks = true;
                lblPrvRelatedEntityName.Label = "Activity:";
                _searchType = SearchType.Activity;
                if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) lblPrvParentEntity.Label = "Activity:";
                foreach (var item in activityDocumentMnList)
                {
                    if (!item.activity_FK.HasValue) continue;
                    var activity = _activityOperations.GetEntity(item.activity_FK);
                    if (activity != null && activity.activity_PK.HasValue)
                    {
                        if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy) lblPrvParentEntity.Text = !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.MissingValue;
                        var hlActivity = new HyperLink
                        {
                            ID = string.Format("activity_{0}", activity.activity_PK),
                            NavigateUrl = string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext.Activity, activity.activity_PK),
                            Text = !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.MissingValue
                        };

                        pnlLinks.Controls.Add(hlActivity);
                        pnlLinks.Controls.Add(new LiteralControl("<br />"));
                    }
                }
                return;
            }

            var taskDocumentMnList = _taskDocumentMnOperations.GetTasksMNByDocument(idDoc);
            if (taskDocumentMnList != null && taskDocumentMnList.Any())
            {
                lblPrvRelatedEntityName.ShowLinks = true;
                lblPrvRelatedEntityName.Label = "Task:";
                _searchType = SearchType.Task;
                if (EntityContext == EntityContext.Task) lblPrvParentEntity.Label = "Task:";
                foreach (var item in taskDocumentMnList)
                {
                    if (!item.task_FK.HasValue) continue;
                    var task = _taskOperations.GetEntity(item.task_FK);
                    if (task == null || !task.task_PK.HasValue) continue;
                    var taskName = _taskNameOperations.GetEntity(task.task_name_FK);
                    if (EntityContext == EntityContext.Task) lblPrvParentEntity.Text = taskName != null && !string.IsNullOrWhiteSpace(taskName.task_name) ? taskName.task_name : Constant.MissingValue;
                    var hlTask = new HyperLink
                    {
                        ID = string.Format("task_{0}", task.task_PK),
                        NavigateUrl = string.Format("~/Views/TaskView/Preview.aspx?EntityContext={0}&idTask={1}", EntityContext.Task, task.task_PK),
                        Text = taskName != null && !string.IsNullOrWhiteSpace(taskName.task_name) ? taskName.task_name : Constant.MissingValue
                    };
                    pnlLinks.Controls.Add(hlTask);
                    pnlLinks.Controls.Add(new LiteralControl("<br />"));
                }
                return;
            }

            var pharmaceuticalProductMnList = _pharmaceuticalProductDocumentMnOperations.GetPProductsByDocumentFK(idDoc);
            if (pharmaceuticalProductMnList != null && pharmaceuticalProductMnList.Any())
            {
                lblPrvRelatedEntityName.ShowLinks = true;
                lblPrvRelatedEntityName.Label = "Pharmaceutical product:";
                _searchType = SearchType.PharmaceuticalProduct;
                if (EntityContext == EntityContext.PharmaceuticalProduct) lblPrvParentEntity.Label = "Pharmaceutical product:";
                foreach (var item in pharmaceuticalProductMnList)
                {
                    if (!item.pp_FK.HasValue) continue;
                    var pharmaceuticalProduct = _pharmaceuticalProductOperations.GetEntity(item.pp_FK);
                    if (pharmaceuticalProduct != null && pharmaceuticalProduct.pharmaceutical_product_PK.HasValue)
                    {
                        if (EntityContext == EntityContext.PharmaceuticalProduct) lblPrvParentEntity.Text = !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.MissingValue;
                        var hlTask = new HyperLink
                        {
                            ID = string.Format("task_{0}", pharmaceuticalProduct.pharmaceutical_product_PK),
                            NavigateUrl = string.Format("~/Views/PharmaceuticalProductView/Preview.aspx?EntityContext={0}&idPharmProd={1}", EntityContext.PharmaceuticalProduct, pharmaceuticalProduct.pharmaceutical_product_PK),
                            Text = !string.IsNullOrWhiteSpace(pharmaceuticalProduct.name) ? pharmaceuticalProduct.name : Constant.MissingValue
                        };

                        pnlLinks.Controls.Add(hlTask);
                        pnlLinks.Controls.Add(new LiteralControl("<br />"));
                    }
                }
                return;
            }
        }

        private void BindResponsibleUser(int? responsibleUserFk)
        {
            var responsibleUser = responsibleUserFk != null ? _personOperations.GetEntity(responsibleUserFk) : null;
            lblPrvResponsibleUser.Text = responsibleUser != null && !string.IsNullOrWhiteSpace(responsibleUser.FullName) ? responsibleUser.FullName : Constant.ControlDefault.LbPrvText;
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
            try
            {
                if (!entityPk.HasValue) return;

                switch (EntityContext)
                {
                    case EntityContext.AuthorisedProduct:
                        if (_authorisedProductDocumentMnOperations.AbleToDeleteEntity(entityPk.Value))
                        {
                            _documentOperations.Delete(entityPk.Value);
                            if (_idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idAuthProd={1}", EntityContext, _idAuthProd));
                        }
                        break;

                    case EntityContext.Product:
                        if (_productDocumentMnOperations.AbleToDeleteEntity(entityPk.Value))
                        {
                            _documentOperations.Delete(entityPk.Value);
                            if (_idProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                        }
                        break;

                    case EntityContext.Project:
                        if (_projectDocumentMnOperations.AbleToDeleteEntity(entityPk.Value))
                        {
                            _documentOperations.Delete(entityPk.Value);
                            if (_idProj.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idProj={1}", EntityContext, _idProj));
                        }
                        break;

                    case EntityContext.Activity:
                        if (_activityDocumentMnOperations.AbleToDeleteEntity(entityPk.Value))
                        {
                            _documentOperations.Delete(entityPk.Value);
                            if (_idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                        }
                        break;

                    case EntityContext.ActivityMy:
                        if (_activityDocumentMnOperations.AbleToDeleteEntity(entityPk.Value))
                        {
                            _documentOperations.Delete(entityPk.Value);
                            if (_idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idLay=My&idAct={1}", EntityContext, _idAct));
                        }
                        break;

                    case EntityContext.Task:
                        if (_taskDocumentMnOperations.AbleToDeleteEntity(entityPk.Value))
                        {
                            _documentOperations.Delete(entityPk.Value);
                            if (_idTask.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idTask={1}", EntityContext, _idTask));
                        }
                        break;

                    case EntityContext.PharmaceuticalProduct:
                        if (_pharmaceuticalProductDocumentMnOperations.AbleToDeleteEntity(entityPk.Value))
                        {
                            _documentOperations.Delete(entityPk.Value);
                            if (_idPharmProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idPharmProd={1}", EntityContext, _idPharmProd));
                        }
                        break;

                    case EntityContext.Default:
                        _documentOperations.Delete(entityPk.Value);
                        Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}", EntityContext));
                        break;

                    case EntityContext.Document:
                        _documentOperations.Delete(entityPk.Value);
                        Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}", EntityContext));
                        break;

                    default:
                        Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}", EntityContext.Document));
                        break;
                }
            }
            catch (Exception ex)
            {
                MasterPage.ModalPopup.ShowModalPopup("Error!", "Could not delete entity! Contact your system administrator.");
            }
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

                        if (EntityContext == EntityContext.Product && _idProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                        else if (EntityContext == EntityContext.AuthorisedProduct && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idAuthProd={1}", EntityContext, _idAuthProd));
                        else if (EntityContext == EntityContext.PharmaceuticalProduct && _idPharmProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idPharmProd={1}", EntityContext, _idPharmProd));
                        else if (EntityContext == EntityContext.Activity && _idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                        else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                        else if (EntityContext == EntityContext.Task && _idTask.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idTask={1}", EntityContext, _idTask));
                        else if (EntityContext == EntityContext.Project && _idProj.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/List.aspx?EntityContext={0}&idProj={1}", EntityContext, _idProj));
                        else if (EntityContext == EntityContext.Document) Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}{1}", EntityContext.Document, query));

                        Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}", EntityContext.Document));
                    }
                    break;

                case ContextMenuEventTypes.Edit:
                    {
                        if (_idDoc.HasValue)
                        {
                            if (EntityContext == EntityContext.Product && _idProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=Edit&idProd={1}&idDoc={2}&From=ProdDocPreview", EntityContext, _idProd, _idDoc));
                            else if (EntityContext == EntityContext.AuthorisedProduct && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=Edit&idAuthProd={1}&idDoc={2}&From=AuthProdDocPreview", EntityContext, _idAuthProd, _idDoc));
                            else if (EntityContext == EntityContext.PharmaceuticalProduct && _idPharmProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=Edit&idPharmProd={1}&idDoc={2}&From=PharmProdDocPreview", EntityContext, _idPharmProd, _idDoc));
                            else if (EntityContext == EntityContext.Project && _idProj.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=Edit&idProj={1}&idDoc={2}&From=ProjDocPreview", EntityContext, _idProj, _idDoc));
                            else if (EntityContext == EntityContext.Activity && _idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=Edit&idAct={1}&idDoc={2}&From=ActDocPreview", EntityContext, _idAct, _idDoc));
                            else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=Edit&idAct={1}&idDoc={2}&From=ActMyDocPreview", EntityContext, _idAct, _idDoc));
                            else if (EntityContext == EntityContext.Task && _idTask.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=Edit&idTask={1}&idDoc={2}&From=TaskDocPreview", EntityContext, _idTask, _idDoc));
                            else if (EntityContext == EntityContext.Document) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=Edit&idDoc={1}&From=DocPreview", EntityContext, _idDoc));
                        }
                        Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}", EntityContext.Document));
                    }
                    break;

                case ContextMenuEventTypes.SaveAs:
                    {
                        if (_idDoc.HasValue)
                        {
                            if (EntityContext == EntityContext.Product && _idProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=SaveAs&idProd={1}&idDoc={2}&From=ProdDocPreview", EntityContext, _idProd, _idDoc));
                            else if (EntityContext == EntityContext.AuthorisedProduct && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=SaveAs&idAuthProd={1}&idDoc={2}&From=AuthProdDocPreview", EntityContext, _idAuthProd, _idDoc));
                            else if (EntityContext == EntityContext.PharmaceuticalProduct && _idPharmProd.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=SaveAs&idPharmProd={1}&idDoc={2}&From=PharmProdDocPreview", EntityContext, _idPharmProd, _idDoc));
                            else if (EntityContext == EntityContext.Project && _idProj.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=SaveAs&idProj={1}&idDoc={2}&From=ProjDocPreview", EntityContext, _idProj, _idDoc));
                            else if (EntityContext == EntityContext.Activity && _idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=SaveAs&idAct={1}&idDoc={2}&From=ActDocPreview", EntityContext, _idAct, _idDoc));
                            else if (EntityContext == EntityContext.ActivityMy && _idAct.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=SaveAs&idAct={1}&idDoc={2}&From=ActMyDocPreview", EntityContext, _idAct, _idDoc));
                            else if (EntityContext == EntityContext.Task && _idTask.HasValue) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=SaveAs&idTask={1}&idDoc={2}&From=TaskDocPreview", EntityContext, _idTask, _idDoc));
                            else if (EntityContext == EntityContext.Document) Response.Redirect(string.Format("~/Views/DocumentView/Form.aspx?EntityContext={0}&Action=SaveAs&idDoc={1}&From=DocPreview", EntityContext, _idDoc));
                        }
                        Response.Redirect(string.Format("~/Views/DocumentViewAll/List.aspx?EntityContext={0}", EntityContext.Document));
                    }
                    break;
            }
        }

        #endregion

        #region Reminders

        void Reminder_OnConfirmInputButtonProcess_Click(object sender, EventArgs e)
        {
            var reminder = Reminder.ReminderVs;

            reminder.user_FK = SessionManager.Instance.CurrentUser.UserID;
            int? relatedEntityFk = null;
            var tableName = ReminderTableName.NULL;
            if (EntityContext == EntityContext.AuthorisedProduct)
            {
                if (_idDoc.HasValue && _idAuthProd.HasValue)
                {
                    relatedEntityFk = _idAuthProd;
                    reminder.navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idAuthProd={2}", EntityContext, _idDoc, _idAuthProd);
                }
                tableName = ReminderTableName.AUTHORISED_PRODUCT;
            }
            else if (EntityContext == EntityContext.Product)
            {
                if (_idDoc.HasValue && _idProd.HasValue)
                {
                    relatedEntityFk = _idProd;
                    reminder.navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idProd={2}", EntityContext, _idDoc, _idProd);
                }
                tableName = ReminderTableName.PRODUCT;
            }
            else if (EntityContext == EntityContext.Activity)
            {
                if (_idDoc.HasValue && _idAct.HasValue)
                {
                    relatedEntityFk = _idAct;
                    reminder.navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idAct={2}", EntityContext, _idDoc, _idAct);
                }
                tableName = ReminderTableName.ACTIVITY;
            }
            else if (EntityContext == EntityContext.ActivityMy)
            {
                if (_idDoc.HasValue && _idAct.HasValue)
                {
                    relatedEntityFk = _idAct;
                    reminder.navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idAct={2}", EntityContext, _idDoc, _idAct);
                }
                tableName = ReminderTableName.ACTIVITY;
            }
            else if (EntityContext == EntityContext.Task)
            {
                if (_idDoc.HasValue && _idTask.HasValue)
                {
                    relatedEntityFk = _idTask;
                    reminder.navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idTask={2}", EntityContext, _idDoc, _idTask);
                }
                tableName = ReminderTableName.TASK;
            }
            else if (EntityContext == EntityContext.Project)
            {
                if (_idDoc.HasValue && _idProj.HasValue)
                {
                    relatedEntityFk = _idProj;
                    reminder.navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}&idProj={2}", EntityContext, _idDoc, _idProj);
                }
                tableName = ReminderTableName.PROJECT;
            }
            else if (EntityContext == EntityContext.Document)
            {
                if (_idDoc.HasValue)
                {
                    relatedEntityFk = _idDoc;
                    reminder.navigate_url = string.Format("~/Views/DocumentView/Preview.aspx?EntityContext={0}&idDoc={1}", EntityContext, _idDoc);
                }
                tableName = ReminderTableName.DOCUMENT;
            }

            reminder.TableName = tableName;
            reminder.entity_FK = _idDoc;
            reminder.related_entity_FK = relatedEntityFk;

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

        private void LblPrvEffectiveStartDateSetReminder_OnClick(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(lblPrvEffectiveStartDate.Label)), lblPrvEffectiveStartDate.Text);
        }

        private void LblPrvEffectiveEndDateSetReminder_OnClick(object sender, EventArgs e)
        {
            SetReminder(StringOperations.RemoveHtmlTags(StringOperations.GetRelatedName(lblPrvEffectiveEndDate.Label)), lblPrvEffectiveEndDate.Text);
        }

        #endregion

        #region Delete

        protected void btnDelete_OnClick(object sender, EventArgs e)
        {
            mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_idDoc);
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
            };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location = null;

            if (EntityContext == EntityContext.Product) location = Support.LocationManager.Instance.GetLocationByName("ProdDocList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.AuthorisedProduct) location = Support.LocationManager.Instance.GetLocationByName("AuthProdDocList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.PharmaceuticalProduct) location = Support.LocationManager.Instance.GetLocationByName("PharmProdDocList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Activity) location = Support.LocationManager.Instance.GetLocationByName("ActDocList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.ActivityMy) location = Support.LocationManager.Instance.GetLocationByName("ActMyDocList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Project) location = Support.LocationManager.Instance.GetLocationByName("ProjDocList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Task) location = Support.LocationManager.Instance.GetLocationByName("TaskDocList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Document) location = Support.LocationManager.Instance.GetLocationByName("DocPreview", Support.CacheManager.Instance.AppLocations);

            if (location != null)
            {
                MasterPage.TabMenu.TabControls.Clear();
                tabMenu.Visible = true;
                tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location);
                tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void StylizeArticle57RelevantControls(bool? isArticle57Relevant)
        {
            if (IsPostBack) return;

            lblPrvDocumentName.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvDocumentName.Text, isArticle57Relevant));
            lblPrvDocumentType.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvDocumentType.Text, isArticle57Relevant));
            lblPrvVersionNumber.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvVersionNumber.Text, isArticle57Relevant));
            lblPrvRegulatoryStatus.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvRegulatoryStatus.Text, isArticle57Relevant));
            lblPrvAttachmentType.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvAttachmentType.Text, isArticle57Relevant));
            lblPrvLanguageCode.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvLanguageCode.Text, isArticle57Relevant));
            lblPrvAttachments.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvAttachments.IsEmpty, isArticle57Relevant));
            lblPrvVersionDate.LblName.AddCssClass(Article57Reporting.GetCssClass(false, true, lblPrvVersionDate.Text, isArticle57Relevant));
        }

        public void RefreshReminderStatus()
        {
            var tableName = string.Empty;

            if (_searchType == SearchType.Product) tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.PRODUCT);
            else if (_searchType == SearchType.AuthorisedProduct) tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.AUTHORISED_PRODUCT);
            else if (_searchType == SearchType.PharmaceuticalProduct) tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.PHARMACEUTICAL_PRODUCT);
            else if (_searchType == SearchType.Activity) tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.ACTIVITY);
            else if (_searchType == SearchType.Task) tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.TASK);
            else if (_searchType == SearchType.Project) tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.PROJECT);

            if (EntityContext == EntityContext.Document && _searchType != SearchType.Null) tableName = Enum.GetName(typeof(ReminderTableName), ReminderTableName.DOCUMENT);

            AlerterHelper.RefreshReminderStatus(_reminderOperations, MasterPage, new List<IReminderCustomControl> { lblPrvEffectiveStartDate, lblPrvEffectiveEndDate }, tableName, _idDoc);
        }

        public void SetReminder(String attributeName, String attributeValue)
        {
            var reminder_type = string.Empty;

            if (_searchType == SearchType.AuthorisedProduct) reminder_type = "AP Document";
            else if (_searchType == SearchType.Product) reminder_type = "P Document";
            else if (_searchType == SearchType.PharmaceuticalProduct) reminder_type = "PP Document";
            else if (_searchType == SearchType.Activity || EntityContext == EntityContext.ActivityMy) reminder_type = "A Document";
            else if (_searchType == SearchType.Task) reminder_type = "Task Document";
            else if (_searchType == SearchType.Project) reminder_type = "Project Document";

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

        private void HandleDocumentType(int idDoc)
        {
            var documentTypePpi = lblPrvDocumentType.Text.Trim().ToLower() == "ppi";

            if (documentTypePpi)
            {
                SetVisibleFieldsDocumentTypePpi();
                SetRequiredFieldsDocumentTypePpi();
            }
            else
            {
                SetVisibleFieldsDocumentTypeNonPpi();
                SetRequiredFieldsDocumentTypeNonPpi();
            }

            SetVisibleFieldsDocumentTypeEDMS(idDoc);
        }

        private void SetVisibleFieldsDocumentTypeEDMS(int idDoc)
        {
            var document = _documentOperations.GetEntity(idDoc);
            if (document == null) return;
            var isEDMSDocument = document.EDMSDocument != null && document.EDMSDocument.Value;
            var isPPIDocument = lblPrvDocumentType.Text.Trim().ToLower() == "ppi";

            lblPrvAttachmentType.Visible = isEDMSDocument || isPPIDocument;
            lblPrvEDMSBindingRule.Visible = isEDMSDocument;
            lblPrvEDMSModifyDate.Visible = isEDMSDocument;
        }

        private void SetRequiredFieldsDocumentTypePpi()
        {
            lblPrvLanguageCode.Required = true;
            lblPrvAttachments.Required = true;
        }

        private void SetVisibleFieldsDocumentTypePpi()
        {
            lblPrvAttachmentType.Visible = true;
            lblPrvVersionDate.Visible = true;
            lblPrvEvcode.Visible = true;
        }

        private void SetRequiredFieldsDocumentTypeNonPpi()
        {
            lblPrvLanguageCode.Required = false;
            lblPrvAttachments.Required = false;
        }

        private void SetVisibleFieldsDocumentTypeNonPpi()
        {
            lblPrvAttachmentType.Visible = false;
            lblPrvVersionDate.Visible = false;
            lblPrvEvcode.Visible = false;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            if (!base.SecurityPageSpecific())
            {
                if (SecurityHelper.IsPermitted(Permission.SaveAsDocument)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });

                if (SecurityHelper.IsPermitted(Permission.EditDocument)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });

                if (SecurityHelper.IsPermitted(Permission.DeleteDocument)) StyleHelper.EnableLinkButtonsWithCssClass(PnlFooter, "Delete");
                else StyleHelper.DisableLinkButtonsWithCssClass(PnlFooter, "Delete");

                SecurityPageSpecificMy(_isResponsibleUser);
            }

            return true;
        }

        #endregion
    }
}