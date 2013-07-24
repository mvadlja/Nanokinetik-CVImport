using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Threading;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using Ionic.Zip;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;
using System.Web.UI.WebControls;
using System.Linq;

namespace AspNetUI.Views.SubmissionUnitView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idAct;
        private int? _idProd;
        private int? _idProj;
        private int? _idSubUnit;
        private int? _idTask;
        private int? _idPharmProd;
        private int? _idAuthProd;
        private bool? _isResponsibleUser;
        private SequenceStatus _sequenceStatus;
        private Dictionary<int, SequenceStatus> _productSequenceStatus;
        private bool _saveAttachment = true;
        private Attachment_PK _attachmentToDelete;

        private IActivity_PKOperations _activityOperations;
        private IProduct_PKOperations _productOperations;
        private IPerson_PKOperations _personOperations;
        private IType_PKOperations _typeOperations;
        private IOrganization_PKOperations _organizationOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private IUSEROperations _userOperations;
        private ISubbmission_unit_PKOperations _submissionUnitOperations;
        private IAttachment_PKOperations _attachmentOperations;
        private ITask_PKOperations _taskOperations;
        private ITask_name_PKOperations _taskNameOperations;
        private IProduct_submission_unit_PKOperations _productSubmissionUnitMnOperations;
        private ISu_agency_mn_PKOperations _suAgencyMnOperations;
        private IDocument_PKOperations _documentOperations;
        private ICountry_PKOperations _countryOperations;
        private IAuthorisedProductOperations _authorisedProductOperations;
        private IActivity_product_PKOperations _activityProductMnOperations;

        #endregion

        #region Properties

        private int? AttachmentPkToDelete
        {
            get { return (int?)ViewState["AttachmentPkToDelete"]; }
            set { ViewState["AttachmentPkToDelete"] = value; }
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

        private int? UploadedSequenceAttachmentId
        {
            get { return Session["UploadedSequenceAttachmentId"] != null ? (int?)Session["UploadedSequenceAttachmentId"] : null; }
            set { Session["UploadedSequenceAttachmentId"] = value; }
        }

        private int? UploadedSequenceWorkingAttachmentId
        {
            get { return Session["UploadedSequenceWorkingAttachmentId"] != null ? (int?)Session["UploadedSequenceWorkingAttachmentId"] : null; }
            set { Session["UploadedSequenceWorkingAttachmentId"] = value; }
        }

        private int? UploadedSequenceAttachmentIdOld
        {
            get { return Session["UploadedSequenceAttachmentIdOld"] != null ? (int?)Session["UploadedSequenceAttachmentIdOld"] : null; }
            set { Session["UploadedSequenceAttachmentIdOld"] = value; }
        }

        private int? UploadedSequenceWorkingAttachmentIdOld
        {
            get { return Session["UploadedSequenceWorkingAttachmentIdOld"] != null ? (int?)Session["UploadedSequenceWorkingAttachmentIdOld"] : null; }
            set { Session["UploadedSequenceWorkingAttachmentIdOld"] = value; }
        }

        private int? ProductSet
        {
            get { return Session["productSet"] != null ? (int?)Session["productSet"] : null; }
            set { Session["productSet"] = value; }
        }

        private string UploadSequenceErrorMessage
        {
            get { return Session["UploadSequenceErrorMessage"] != null ? (string)Session["UploadSequenceErrorMessage"] : null; }
            set { Session["UploadSequenceErrorMessage"] = value; }
        }

        private string InitialSequence
        {
            get { return (string)Session["InitialSequence"]; }
            set { Session["InitialSequence"] = value; }
        }

        private string UploadedSequenceFullPath
        {
            get { return (string)ViewState["UploadedSequenceFullPath"]; }
            set { Session["UploadedSequenceFullPath"] = value; }
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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            GenerateTabMenuItems();
            GenerateTopMenuItems();

            if (!string.IsNullOrWhiteSpace(UploadSequenceErrorMessage))
            {
                mpSequenceUploadError.ShowModalPopup("Error!", UploadSequenceErrorMessage);
                UploadSequenceErrorMessage = null;
            }

            MasterPage.UpTopMenu.Update();
        }

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();
            LoadActionQuery();

            _idSubUnit = ValidationHelper.IsValidInt(Request.QueryString["idSubUnit"]) ? int.Parse(Request.QueryString["idSubUnit"]) : (int?)null;
            _idAuthProd = ValidationHelper.IsValidInt(Request.QueryString["idAuthProd"]) ? int.Parse(Request.QueryString["idAuthProd"]) : (int?)null;
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;
            _idPharmProd = ValidationHelper.IsValidInt(Request.QueryString["idPharmProd"]) ? int.Parse(Request.QueryString["idPharmProd"]) : (int?)null;
            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;
            _idTask = ValidationHelper.IsValidInt(Request.QueryString["idTask"]) ? int.Parse(Request.QueryString["idTask"]) : (int?)null;
            _idProj = ValidationHelper.IsValidInt(Request.QueryString["idProj"]) ? int.Parse(Request.QueryString["idProj"]) : (int?)null;

            _activityOperations = new Activity_PKDAL();
            _activityOperations = new Activity_PKDAL();
            _productOperations = new Product_PKDAL();
            _personOperations = new Person_PKDAL();
            _typeOperations = new Type_PKDAL();
            _organizationOperations = new Organization_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _userOperations = new USERDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _submissionUnitOperations = new Subbmission_unit_PKDAL();
            _attachmentOperations = new Attachment_PKDAL();
            _taskOperations = new Task_PKDAL();
            _taskNameOperations = new Task_name_PKDAL();
            _productSubmissionUnitMnOperations = new Product_submission_unit_PKDAL();
            _suAgencyMnOperations = new Su_agency_mn_PKDAL();
            _documentOperations = new Document_PKDAL();
            _countryOperations = new Country_PKDAL();
            _authorisedProductOperations = new AuthorisedProductDAL();
            _activityProductMnOperations = new Activity_product_PKDAL();
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            lbSrProducts.Searcher.OnOkButtonClick += LbSrProductsSearcher_OnOkButtonClick;
            lbSrProducts.OnRemoveClick += LbSrProducts_OnRemoveClick;

            lbAuAgencies.OnAssignClick += lbAuAgencies_OnAssignClick;
            lbAuAgencies.OnUnassignClick += lbAuAgencies_OnUnassignClick;

            ddlSubmissionFormat.DdlInput.SelectedIndexChanged += DdlSubmissionFormat_SelectedIndexChanged;
            ddlActivity.DdlInput.SelectedIndexChanged += DdlActivity_SelectedIndexChanged;

            upAttachments.GvData.RowDataBound += GvData_RowDataBound;
            upAttachments.AsyncFileUpload.UploadedComplete += UpAttachmentsAsyncFileUpload_UploadedComplete;
            upAttachments.OnDelete += upAttachments_OnDelete;

            upSubmissionFormatSequenceType.GvData.RowDataBound += GvData_RowDataBound;
            upSubmissionFormatSequenceType.AsyncFileUpload.UploadedComplete += UpSubmissionFormatSequenceTypeAsyncFileUpload_UploadedComplete;
            upSubmissionFormatSequenceType.OnDelete += upAttachments_OnDelete;

            upSubmissionFormatSequenceTypeWorking.GvData.RowDataBound += GvData_RowDataBound;
            upSubmissionFormatSequenceTypeWorking.AsyncFileUpload.UploadedComplete += UpSubmissionFormatSequenceTypeWorkingAsyncFileUpload_UploadedComplete;
            upAttachments.OnDelete += upAttachments_OnDelete;

            mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;

            _productSequenceStatus = new Dictionary<int, SequenceStatus>();
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
            lblPrvParentEntity.Text = Constant.DefaultEmptyValue;

            lbSrProducts.Clear();
            ddlActivity.Text = string.Empty;
            ddlTask.Text = string.Empty;
            ddlSubmissionDescription.Text = string.Empty;
            ddlResponsibleUser.Text = string.Empty;
            lbAuAgencies.Clear();
            txtSubmissionId.Text = string.Empty;
            ddlSubmissionFormat.Text = string.Empty;
            txtSequence.Text = string.Empty;
            txtComment.Text = string.Empty;
            dtDispatchDate.Text = string.Empty;
            dtReceiptDate.Text = string.Empty;
            InitialSequence = string.Empty;
            ProductSet = null;
        }

        private void FillFormControls(object arg)
        {
            FillSubmissionDescription();
            FillDdlResponsibleUser();
            FillDdlSubmissionFormat();
            FillDdlRms();

            if (FormType == FormType.New)
            {
                FillLbAuAgencies();
                FillDependentDdls();
            }
        }

        private void FillDependentDdls()
        {
            if (!_idTask.HasValue)
            {
                FillDdlActivity();
                FillDdlTask();
                return;
            }

            var task = _taskOperations.GetEntity(_idTask);
            if (task == null || task.activity_FK == null) return;

            var productList = _productOperations.GetProductsByActivity(task.activity_FK);
            BindProducts(null, productList);

            FillDdlActivity(task.activity_FK);
            FillDdlTask(task.task_PK);
        }

        private void FillDdlActivity(int? selectedItem = null)
        {
            var activityList = new List<Activity_PK>();
            foreach (ListItem item in lbSrProducts.LbInput.Items)
            {
                activityList.AddRange(_activityOperations.GetActivityFromProduct(Int32.Parse(item.Value)));
            }
            activityList.ForEach(a => a.name = a.GetNameFormatted());
            ddlActivity.Fill(activityList, a => a.name, a => a.activity_PK);
            ddlActivity.SortItemsByText();
            if (ddlActivity.DdlInput.Items.Count > 1)
            {
                if (selectedItem.HasValue) ddlActivity.SelectedId = selectedItem;
                else ddlActivity.SelectedId = ValidationHelper.IsValidInt(ddlActivity.DdlInput.Items[1].Value) ? (int?)Convert.ToInt32(ddlActivity.DdlInput.Items[1].Value) : null;
            }
        }

        private void FillDdlTask(int? selectedItem = null)
        {
            List<Task_PK> taskList = _taskOperations.GetTasksByActivity(ddlActivity.SelectedId);

            // Comment field is just a placeholder for task_name
            foreach (var task in taskList)
            {
                var taskName = _taskNameOperations.GetEntity(task.task_name_FK);
                task.comment = taskName != null ? taskName.task_name : Constant.MissingValue;
            }
            ddlTask.Fill(taskList, t => t.comment, t => t.task_PK);
            ddlTask.SortItemsByText();
            if (ddlTask.DdlInput.Items.Count > 1)
            {
                if (selectedItem.HasValue) ddlTask.SelectedId = selectedItem;
                else ddlTask.SelectedId = ValidationHelper.IsValidInt(ddlTask.DdlInput.Items[1].Value) ? (int?)Convert.ToInt32(ddlTask.DdlInput.Items[1].Value) : null;
            }
        }

        private void FillSubmissionDescription()
        {
            var submissionDescriptionList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.SubmissionUnitDescription);
            ddlSubmissionDescription.Fill(submissionDescriptionList, x => x.name, x => x.type_PK);
            ddlSubmissionDescription.SortItemsByText();
        }

        private void FillDdlResponsibleUser()
        {
            var responsibleUserList = _personOperations.GetPersonsByRole(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, x => x.FullName, x => x.person_PK);
            ddlResponsibleUser.SortItemsByText();
        }

        private void FillLbAuAgencies()
        {
            var agencyList = _organizationOperations.GetOrganizationsByRole(Constant.OrganizationRoleName.Agency);
            lbAuAgencies.LbInputFrom.Fill(agencyList, x => x.name_org, x => x.organization_PK);
            lbAuAgencies.LbInputFrom.SortItemsByText();
        }

        private void FillDdlSubmissionFormat()
        {
            var submissionFormatList = _typeOperations.GetTypesForDDL(Constant.TypeGroupName.SubmissionUnitFormat);
            ddlSubmissionFormat.Fill(submissionFormatList, x => x.name, x => x.type_PK);
            ddlSubmissionFormat.SortItemsByText();
        }

        private void FillDdlRms()
        {
            var countryList = _countryOperations.GetEntitiesCustomSort();
            countryList = countryList.FindAll(c => (c != null && c.region != null && c.region.Trim().ToLower() == "eu"));
            countryList.SortByField("custom_sort_ID");
            ddlRms.Fill(countryList, Constant.Countries.DisplayNameFormat, "country_PK");
        }

        private void SetFormControlsDefaults(object arg)
        {
            var submissionUnit = _submissionUnitOperations.GetEntity(_idSubUnit);

            lblPrvParentEntity.Visible = true;
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

                case EntityContext.SubmissionUnit:
                case EntityContext.Default:
                    {
                        if (FormType != FormType.New)
                        {
                            lblPrvParentEntity.Label = "Submission unit:";
                            var submissionUnitDescription = submissionUnit.description_type_FK.HasValue ? _typeOperations.GetEntity(submissionUnit.description_type_FK) : null;
                            lblPrvParentEntity.Text = submissionUnitDescription != null && !string.IsNullOrWhiteSpace(submissionUnitDescription.name) ? submissionUnitDescription.name : Constant.ControlDefault.LbPrvText;
                        }
                        else
                        {
                            lblPrvParentEntity.Visible = false;
                        }
                    }
                    break;
            }

            if (FormType == FormType.New || FormType == FormType.SaveAs)
            {
                var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
                ddlResponsibleUser.SelectedValue = user != null ? user.Person_FK : null;
            }

            if (FormType == FormType.New)
            {
                if (EntityContext == EntityContext.Product)
                {
                    BindParentEntityProduct();
                    FillDependentDdls();
                }
            }

            if (FormType == FormType.SaveAs && submissionUnit != null && submissionUnit.document_FK.HasValue)
            {
                var attachmentsInDb = _attachmentOperations.GetAttachmentsForDocumentWithDiskFile(submissionUnit.document_FK.Value);
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

            if (FormType == FormType.SaveAs && submissionUnit != null)
            {
                if (submissionUnit.ness_FK != null || submissionUnit.ectd_FK != null)
                {
                    var documentFk = submissionUnit.ness_FK ?? submissionUnit.ectd_FK;
                    // TODO: Copy attachments on db
                    var attachmentsInDb = _attachmentOperations.GetAttachmentsForDocumentWithDiskFile(documentFk.Value);
                    if (attachmentsInDb.Any()) attachmentsInDb = attachmentsInDb.Where(a => a.attachmentname.Contains(txtSequence.Text)).ToList();
                    var saveAsAttachmentList = new List<Attachment_PK>();
                    saveAsAttachmentList.AddRange(attachmentsInDb);
                    saveAsAttachmentList.ForEach(a =>
                    {
                        a.attachment_PK = null;
                        a.session_id = upSubmissionFormatSequenceType.AttachmentSessionId;
                        a.document_FK = null;
                    });

                    _attachmentOperations.SaveCollection(saveAsAttachmentList);
                }
            }

            SetSubmissionFormatVisibleFields();

            upSubmissionFormatSequenceType.DivHrHolder.Visible = upSubmissionFormatSequenceTypeWorking.DivHrHolder.Visible = false;
            upSubmissionFormatSequenceTypeWorking.PnlUploadedFiles.Visible = false;
            UploadedSequenceAttachmentId = UploadedSequenceWorkingAttachmentId = UploadedSequenceAttachmentIdOld = UploadedSequenceWorkingAttachmentIdOld = null;
            UploadSequenceErrorMessage = string.Empty;
            upSubmissionFormatSequenceType.DivUploadPanel.Attributes.Add("class", "row");
            upSubmissionFormatSequenceTypeWorking.DivUploadPanel.Attributes.Add("class", "row");

            BindDynamicControls(null);
        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idSubUnit.HasValue) return;

            var submissionUnit = _submissionUnitOperations.GetEntity(_idSubUnit.Value);
            if (submissionUnit == null || !submissionUnit.subbmission_unit_PK.HasValue) return;

            // Entity
            // Submission unit description
            BindSubmissionUnitDescription(submissionUnit.description_type_FK);

            // Bind products
            BindProducts(submissionUnit.subbmission_unit_PK.Value);

            FillDdlActivity();
            // Bind activity
            BindActivity(submissionUnit.task_FK);

            FillDdlTask();
            ddlTask.SelectedId = submissionUnit.task_FK;

            // Submission description
            ddlSubmissionDescription.SelectedId = submissionUnit.description_type_FK;

            // Responsible user
            ddlResponsibleUser.SelectedId = submissionUnit.person_FK;

            // RMS
            ddlRms.SelectedId = submissionUnit.agency_role_FK;

            // Agencies
            BindAgencies(submissionUnit.subbmission_unit_PK);

            // Submission ID
            txtSubmissionId.Text = submissionUnit.submission_ID;

            // Submission format
            ddlSubmissionFormat.SelectedId = submissionUnit.s_format_FK;
            if (ddlSubmissionFormat.SelectedItem != null && ddlSubmissionFormat.SelectedItem.Text.ToLower() == "ectd" && submissionUnit.ectd_FK != null)
            {
                ProductSet = submissionUnit.ectd_FK;
            }

            // Sequence
            txtSequence.Text = submissionUnit.sequence;
            if (FormType == FormType.Edit) InitialSequence = txtSequence.Text;

            // Comment
            txtComment.Text = submissionUnit.comment;

            // Dispatch date
            dtDispatchDate.Text = submissionUnit.dispatch_date.HasValue ? submissionUnit.dispatch_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            // Receipt date
            dtReceiptDate.Text = submissionUnit.receipt_date.HasValue ? submissionUnit.receipt_date.Value.ToString(Constant.DateTimeFormat) : String.Empty;

            _isResponsibleUser = submissionUnit.person_FK == SessionManager.Instance.CurrentUser.UserID;
        }

        private void BindParentEntityProduct()
        {
            var product = _productOperations.GetEntity(_idProd);

            if (product == null) return;

            lblPrvParentEntity.Text = product.name;
            lbSrProducts.LbInput.Items.Add(new ListItem(product.GetNameFormatted(Constant.UnknownValue), Convert.ToString(product.product_PK)));
        }

        private void BindAgencies(int? submissionUnitPk)
        {
            var agencyAvailableList = _organizationOperations.GetAvailableAgenciesForSubmissionUnit(submissionUnitPk);
            agencyAvailableList.SortByField(a => a.name_org);
            lbAuAgencies.LbInputFrom.Fill(agencyAvailableList, a => a.name_org, x => x.organization_PK);

            var agencyAssignedList = _organizationOperations.GetAssignedAgenciesForSubmissionUnit(submissionUnitPk);
            agencyAssignedList.SortByField(a => a.name_org);
            lbAuAgencies.LbInputTo.Fill(agencyAssignedList, a => a.name_org, x => x.organization_PK);
        }

        private void BindActivity(int? taskFk)
        {
            var task = _taskOperations.GetEntity(taskFk);
            if (task != null && task.activity_FK.HasValue)
            {
                var activity = _activityOperations.GetEntity(task.activity_FK);
                if (activity != null) ddlActivity.SelectedId = activity.activity_PK;
            }
        }

        private void BindSubmissionUnitDescription(int? descriptionTypeFk)
        {
            var submissionUnitDescription = descriptionTypeFk.HasValue ? _typeOperations.GetEntity(descriptionTypeFk) : null;
            ddlSubmissionDescription.SelectedId = submissionUnitDescription != null ? submissionUnitDescription.type_PK : null;
        }

        private void BindProducts(int? submissionUnitPk = null, List<Product_PK> productList = null)
        {
            if (submissionUnitPk.HasValue) productList = _productOperations.GetAssignedProductsForSubmissionUnit(submissionUnitPk);

            if (productList == null) return;

            productList.ForEach(x => x.name = x.GetNameFormatted(Constant.UnknownValue));
            lbSrProducts.Fill(productList, x => x.name, x => x.product_PK);
            lbSrProducts.LbInput.SortItemsByText();
        }

        private void BindUpAttachments(Subbmission_unit_PK submissionUnit = null)
        {
            if (submissionUnit == null)
            {
                submissionUnit = _submissionUnitOperations.GetEntity(_idSubUnit);
            }

            var idDoc = submissionUnit != null ? submissionUnit.document_FK : null;
            if (FormType == FormType.SaveAs) idDoc = null;

            var attachmentsForThisSession = Attachment_PK.GetAttachmentsForThisSession(_attachmentOperations, idDoc, upAttachments.AttachmentSessionId);
            var refAttachmentPkListToDelete = _attachmentPkListToDelete;
            attachmentsForThisSession = attachmentsForThisSession.Where(a => a.attachment_PK != null && !refAttachmentPkListToDelete.Contains(a.attachment_PK.Value)).ToList();

            upAttachments.NumberOfUploadedAttachments = attachmentsForThisSession.Count;
            if (!IsPostBack)
            {
                var oldAttachments = Attachment_PK.GetAttachmentsForThisSession(_attachmentOperations, idDoc, null);
                if (oldAttachments != null && oldAttachments.Any())
                {
                    upAttachments.AttachmentIdOldValue = oldAttachments.Select(a => a.attachment_PK != null ? a.attachment_PK.Value : 0).ToList();
                }
            }

            if (attachmentsForThisSession.Count > 0)
            {
                upAttachments.AttachmentIdNewValue = attachmentsForThisSession.Select(a => a.attachment_PK != null ? a.attachment_PK.Value : 0).ToList();
                upAttachments.AsyncFileUpload.CssClass = "async-upload-control-items";
                upAttachments.DivHrHolder.Visible = true;
                upAttachments.RefreshAttachments = false;
                upAttachments.PnlUploadedFiles.Visible = true;
            }
            else
            {
                upAttachments.AttachmentIdNewValue = null;
                upAttachments.DivHrHolder.Visible = false;
                upAttachments.AsyncFileUpload.CssClass = "async-upload-control-no-items";
            }

            upAttachments.GvData.DataSource = attachmentsForThisSession;
            upAttachments.GvData.DataBind();
            MasterPage.UpTopMenu.Update();
        }

        private void BindSequenceAttachments(Subbmission_unit_PK submissionUnit = null)
        {
            int? documentFk = null;
            if (submissionUnit == null)
            {
                submissionUnit = _submissionUnitOperations.GetEntity(_idSubUnit);
            }

            var selectedSubmissionFormat = ddlSubmissionFormat.SelectedItem;
            if (selectedSubmissionFormat != null && selectedSubmissionFormat.Text.ToLower() == "ectd")
            {
                documentFk = submissionUnit != null ? submissionUnit.ectd_FK : null;
                upSubmissionFormatSequenceType.Label = "eCTD sequence:";
                upSubmissionFormatSequenceTypeWorking.Label = "eCTD working:";
            }
            else if (selectedSubmissionFormat != null && selectedSubmissionFormat.Text.ToLower() == "nees")
            {
                documentFk = submissionUnit != null ? submissionUnit.ness_FK : null;
                upSubmissionFormatSequenceType.Label = "NeeS sequence:";
                upSubmissionFormatSequenceTypeWorking.Label = "NeeS working:";
            }

            Guid? attachmentSessionId = null;
            if (FormType == FormType.SaveAs)
            {
                documentFk = null;
                attachmentSessionId = upSubmissionFormatSequenceType.AttachmentSessionId;
            }

            var uploadedAttachments = new List<int>();
            List<Attachment_PK> attachmentsForThisSession = Attachment_PK.GetAttachmentsForThisSession(_attachmentOperations, documentFk, attachmentSessionId);
            var refAttachmentPkListToDelete = _attachmentPkListToDelete;
            attachmentsForThisSession = attachmentsForThisSession.Where(a => a.attachment_PK != null && !refAttachmentPkListToDelete.Contains(a.attachment_PK.Value)).ToList();
            attachmentsForThisSession = attachmentsForThisSession.Where(a => a.attachmentname.Contains(txtSequence.Text)).ToList();

            if (IsPostBack)
            {
                if (UploadedSequenceAttachmentId.HasValue && !uploadedAttachments.Contains(UploadedSequenceAttachmentId.Value)) uploadedAttachments.Add(UploadedSequenceAttachmentId.Value);
                if (UploadedSequenceWorkingAttachmentId.HasValue && !uploadedAttachments.Contains(UploadedSequenceWorkingAttachmentId.Value)) uploadedAttachments.Add(UploadedSequenceWorkingAttachmentId.Value);
                attachmentsForThisSession = attachmentsForThisSession.Where(a => a.attachment_PK != null && uploadedAttachments.Contains(a.attachment_PK.Value)).ToList();
            }

            Attachment_PK currentSequence = null;
            Attachment_PK currentWorkingSequence = null;
            Attachment_PK dbSequence = null;
            Attachment_PK dbWorkingSequence = null;

            if (!IsPostBack)
            {
                var oldAttachments = Attachment_PK.GetAttachmentsForThisSession(_attachmentOperations, documentFk, null);
                if (oldAttachments != null && oldAttachments.Any())
                {
                    upSubmissionFormatSequenceType.AttachmentIdOldValue = oldAttachments.Where(a => a.attachmentname.Contains(txtSequence.Text)).Select(a => a.attachment_PK != null ? a.attachment_PK.Value : 0).ToList();
                }
            }

            if (attachmentsForThisSession.Count > 0)
            {
                dbSequence = attachmentsForThisSession.Find(a => !a.attachmentname.Contains("working"));
                dbWorkingSequence = attachmentsForThisSession.Find(a => a.attachmentname.Contains("working"));
                attachmentsForThisSession.Clear();
                if (!IsPostBack)
                {
                    if (dbSequence != null) UploadedSequenceAttachmentId = UploadedSequenceAttachmentIdOld = dbSequence.attachment_PK;
                    if (dbWorkingSequence != null) UploadedSequenceWorkingAttachmentId = UploadedSequenceWorkingAttachmentIdOld = dbWorkingSequence.attachment_PK;
                }
            }

            if (UploadedSequenceAttachmentId.HasValue && !refAttachmentPkListToDelete.Contains(UploadedSequenceAttachmentId)) currentSequence = _attachmentOperations.GetEntityWithoutDiskFile(UploadedSequenceAttachmentId);
            if (UploadedSequenceWorkingAttachmentId.HasValue && !refAttachmentPkListToDelete.Contains(UploadedSequenceWorkingAttachmentId)) currentWorkingSequence = _attachmentOperations.GetEntityWithoutDiskFile(UploadedSequenceWorkingAttachmentId);

            if (dbSequence != null)
            {
                if ((currentSequence != null && currentSequence.attachment_PK != dbSequence.attachment_PK)) attachmentsForThisSession.Add(currentSequence);
                else attachmentsForThisSession.Add(dbSequence);
            }
            else if (currentSequence != null)
            {
                attachmentsForThisSession.Add(currentSequence);
            }

            if (dbWorkingSequence != null)
            {
                if ((currentWorkingSequence != null && currentWorkingSequence.attachment_PK != dbWorkingSequence.attachment_PK)) attachmentsForThisSession.Add(currentWorkingSequence);
                else attachmentsForThisSession.Add(dbWorkingSequence);
            }
            else if (currentWorkingSequence != null)
            {
                attachmentsForThisSession.Add(currentWorkingSequence);
            }

            upSubmissionFormatSequenceType.NumberOfUploadedAttachments = attachmentsForThisSession.Count;
            if (attachmentsForThisSession.Count > 0)
            {
                upSubmissionFormatSequenceType.AttachmentIdNewValue = attachmentsForThisSession.Select(a => a.attachment_PK != null ? a.attachment_PK.Value : 0).ToList();
                upSubmissionFormatSequenceType.RefreshAttachments = false;
                upSubmissionFormatSequenceType.PnlUploadedFiles.Visible = true;
                upSubmissionFormatSequenceType.PnlUploadedFiles.CssClass = "async-upload-control-no-items position-absolute";
                upSubmissionFormatSequenceType.PnlUploadFilesMain.CssClass = "margin-top_11";
                if (attachmentsForThisSession.Count == 1) upSubmissionFormatSequenceTypeWorking.PnlUploadFilesMain.CssClass = "async-upload-control-no-items padding-bottom-70";
                else if (attachmentsForThisSession.Count == 2) upSubmissionFormatSequenceTypeWorking.PnlUploadFilesMain.CssClass = "async-upload-control-no-items padding-bottom-100";
            }
            else
            {
                upSubmissionFormatSequenceType.AttachmentIdNewValue = null;
                upSubmissionFormatSequenceType.PnlUploadedFiles.CssClass = "async-upload-control-no-items position-absolute";
                upSubmissionFormatSequenceType.PnlUploadFilesMain.CssClass = "margin-top_11";
                upSubmissionFormatSequenceTypeWorking.PnlUploadFilesMain.CssClass = "async-upload-control-no-items";
            }

            upSubmissionFormatSequenceType.GvData.DataSource = attachmentsForThisSession;
            upSubmissionFormatSequenceType.GvData.DataBind();
            MasterPage.UpTopMenu.Update();
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (!string.IsNullOrWhiteSpace(txtSequence.Text))
            {
                var sequenceStatus = CheckSequenceTextChange();
                if (sequenceStatus != SequenceStatus.Valid && sequenceStatus != SequenceStatus.NewSequence && sequenceStatus != SequenceStatus.ProductSetAlreadyExist) errorMessage += ConvertToString(_sequenceStatus);
            }

            if (lbSrProducts.LbInput.Items.Count == 0)
            {
                errorMessage += "Products can't be empty.<br />";
                lbSrProducts.ValidationError = "Products can't be empty.";
            }

            if (!ddlActivity.SelectedId.HasValue)
            {
                errorMessage += "You must select one activity.<br />";
                ddlActivity.ValidationError = "You must select one activity.";
            }

            if (!ddlTask.SelectedId.HasValue)
            {
                errorMessage += "Task can't be empty.<br />";
                ddlTask.ValidationError = "Task can't be empty.";
            }

            if (!ddlSubmissionDescription.SelectedId.HasValue)
            {
                errorMessage += "Submission description can't be empty.<br />";
                ddlSubmissionDescription.ValidationError = "Submission description can't be empty.";
            }

            if (lbAuAgencies.LbInputTo.Items.Count == 0)
            {
                errorMessage += "Agencies can't be empty.<br />";
                lbAuAgencies.ValidationError = "Agencies can't be empty.";
            }

            if (!string.IsNullOrWhiteSpace(dtDispatchDate.Text) &&
                !ValidationHelper.IsValidDateTime(dtDispatchDate.Text, CultureInfoHr))
            {
                errorMessage += "Dispatch date is not a valid date.<br />";
                dtDispatchDate.ValidationError = "Dispatch date is not a valid date.";
            }

            if (!string.IsNullOrWhiteSpace(dtReceiptDate.Text) &&
                !ValidationHelper.IsValidDateTime(dtReceiptDate.Text, CultureInfoHr))
            {
                errorMessage += "Receipt date is not a valid date.<br />";
                dtReceiptDate.ValidationError = "Receipt date is not a valid date.";
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
            lbSrProducts.ValidationError = string.Empty;
            ddlActivity.ValidationError = string.Empty;
            ddlTask.ValidationError = string.Empty;
            ddlSubmissionDescription.ValidationError = string.Empty;
            ddlResponsibleUser.ValidationError = string.Empty;
            lbAuAgencies.ValidationError = string.Empty;
            txtSubmissionId.ValidationError = string.Empty;
            ddlSubmissionFormat.ValidationError = string.Empty;
            txtSequence.ValidationError = string.Empty;
            txtComment.ValidationError = string.Empty;
            upSubmissionFormatSequenceType.ValidationError = string.Empty;
            upSubmissionFormatSequenceTypeWorking.ValidationError = string.Empty;
            dtDispatchDate.ValidationError = string.Empty;
            dtReceiptDate.ValidationError = string.Empty;
            upAttachments.ValidationError = string.Empty;
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            var submissionUnit = new Subbmission_unit_PK();

            if (FormType == FormType.Edit)
            {
                submissionUnit = _submissionUnitOperations.GetEntity(_idSubUnit);
            }

            if (submissionUnit == null) return null;

            submissionUnit.task_FK = ddlTask.SelectedId;
            submissionUnit.description_type_FK = ddlSubmissionDescription.SelectedId;
            submissionUnit.person_FK = ddlResponsibleUser.SelectedId;
            submissionUnit.agency_role_FK = ddlRms.Visible ? ddlRms.SelectedId : null;
            submissionUnit.submission_ID = txtSubmissionId.Text;
            submissionUnit.s_format_FK = ddlSubmissionFormat.SelectedId;
            submissionUnit.sequence = txtSequence.Text;
            submissionUnit.comment = txtComment.Text;
            submissionUnit.dispatch_date = ValidationHelper.IsValidDateTime(dtDispatchDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtDispatchDate.Text) : null;
            submissionUnit.receipt_date = ValidationHelper.IsValidDateTime(dtReceiptDate.Text, CultureInfoHr) ? (DateTime?)Convert.ToDateTime(dtReceiptDate.Text) : null;

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                submissionUnit = _submissionUnitOperations.Save(submissionUnit);

                if (!submissionUnit.subbmission_unit_PK.HasValue) return null;

                SaveProducts(submissionUnit.subbmission_unit_PK, auditTrailSessionToken);
                SaveAgencies(submissionUnit.subbmission_unit_PK, auditTrailSessionToken);
                SaveAttachments(submissionUnit);
                SaveSequences(submissionUnit);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, submissionUnit.subbmission_unit_PK, "SUBMISSION_UNIT", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, submissionUnit.subbmission_unit_PK, "SUBMISSION_UNIT", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return submissionUnit;
        }

        private void SaveSequences(Subbmission_unit_PK submissionUnit)
        {
            if (!string.IsNullOrWhiteSpace(txtSequence.Text))
            {
                var selectedSubmissionFormat = ddlSubmissionFormat.SelectedItem;
                if (selectedSubmissionFormat != null && selectedSubmissionFormat.Text.ToLower() == "ectd")
                {
                    SaveSubmissionFormatEctd(submissionUnit);
                }
                else if (selectedSubmissionFormat != null && selectedSubmissionFormat.Text.ToLower() == "nees")
                {
                    SaveSubmissionFormatNeeS(submissionUnit);
                }
                else
                {
                    submissionUnit.ness_FK = null;
                    submissionUnit.ectd_FK = null;
                }
                _submissionUnitOperations.Save(submissionUnit);
            }
        }

        private void SaveSubmissionFormatEctd(Subbmission_unit_PK submissionUnit)
        {
            if (ProductSet.HasValue)
            {
                submissionUnit.ectd_FK = ProductSet;
            }
            else
            {
                var ectd = new Document_PK();
                ectd = _documentOperations.Save(ectd);
                submissionUnit.ectd_FK = ectd.document_PK;
            }
            submissionUnit.ness_FK = null;

            if (UploadedSequenceAttachmentId.HasValue)
            {
                Attachment_PK attachment = _attachmentOperations.GetEntityWithoutDiskFile(UploadedSequenceAttachmentId);
                if (attachment != null)
                {
                    attachment.document_FK = submissionUnit.ectd_FK;
                    attachment.userID = SessionManager.Instance.CurrentUser.UserID;
                    _attachmentOperations.SaveLinkToDocument(attachment.attachment_PK, attachment.document_FK);

                    // Delete Old Attachment
                    int uploadedAttachSequenceOld = UploadedSequenceAttachmentIdOld.HasValue ? UploadedSequenceAttachmentIdOld.Value : -1;
                    if (uploadedAttachSequenceOld != -1 && uploadedAttachSequenceOld != attachment.attachment_PK)
                    {
                        _attachmentToDelete = _attachmentOperations.GetEntityWithoutDiskFile(uploadedAttachSequenceOld);
                        if (_attachmentToDelete != null) _attachmentOperations.Delete(uploadedAttachSequenceOld);
                    }
                }
            }
            else if (UploadedSequenceAttachmentIdOld.HasValue)
            {
                Attachment_PK attachment = _attachmentOperations.GetEntityWithoutDiskFile(UploadedSequenceAttachmentIdOld.Value);
                if (attachment != null)
                {
                    _attachmentOperations.Delete(UploadedSequenceAttachmentIdOld);

                    if (submissionUnit.ectd_FK != null)
                    {
                        List<Attachment_PK> assignedAttachments = _attachmentOperations.GetAttachmentsForDocument((int)submissionUnit.ectd_FK);

                        if (assignedAttachments.Count == 0)
                        {
                            _documentOperations.Delete(submissionUnit.ectd_FK);
                            submissionUnit.ectd_FK = null;
                        }
                    }
                }
            }

            if (UploadedSequenceWorkingAttachmentId.HasValue)
            {
                Attachment_PK attachment = _attachmentOperations.GetEntityWithoutDiskFile(UploadedSequenceWorkingAttachmentId.Value);
                if (attachment != null)
                {
                    attachment.document_FK = submissionUnit.ectd_FK;
                    attachment.userID = SessionManager.Instance.CurrentUser.UserID;
                    _attachmentOperations.SaveLinkToDocument(attachment.attachment_PK, attachment.document_FK);

                    // Delete Old Attachment
                    int uploadedAttachWorkingOld = UploadedSequenceWorkingAttachmentIdOld.HasValue ? UploadedSequenceWorkingAttachmentIdOld.Value : -1;
                    if (uploadedAttachWorkingOld != -1 && uploadedAttachWorkingOld != attachment.attachment_PK)
                    {
                        _attachmentToDelete = _attachmentOperations.GetEntityWithoutDiskFile(uploadedAttachWorkingOld);
                        if (_attachmentToDelete != null) _attachmentOperations.Delete(uploadedAttachWorkingOld);
                    }
                }
            }
            else if (UploadedSequenceWorkingAttachmentIdOld.HasValue)
            {
                Attachment_PK attachment = _attachmentOperations.GetEntityWithoutDiskFile(UploadedSequenceWorkingAttachmentIdOld.Value);
                if (attachment != null)
                {
                    _attachmentOperations.Delete(UploadedSequenceWorkingAttachmentIdOld);

                    if (submissionUnit.ectd_FK != null)
                    {
                        List<Attachment_PK> assignedAttachments = _attachmentOperations.GetAttachmentsForDocument((int)submissionUnit.ectd_FK);

                        if (assignedAttachments.Count == 0)
                        {
                            _documentOperations.Delete(submissionUnit.ectd_FK);
                            submissionUnit.ectd_FK = null;
                        }
                    }
                }
            }
        }

        private void SaveSubmissionFormatNeeS(Subbmission_unit_PK submissionUnit)
        {
            if (submissionUnit.ness_FK == null)
            {
                var nees = new Document_PK();
                nees = _documentOperations.Save(nees);
                submissionUnit.ness_FK = nees.document_PK;
            }
            submissionUnit.ectd_FK = null;

            if (UploadedSequenceAttachmentId.HasValue)
            {
                Attachment_PK attachment = _attachmentOperations.GetEntityWithoutDiskFile(UploadedSequenceAttachmentId.Value);
                if (attachment != null)
                {
                    attachment.document_FK = submissionUnit.ness_FK;
                    attachment.userID = SessionManager.Instance.CurrentUser.UserID;
                    _attachmentOperations.SaveLinkToDocument(attachment.attachment_PK, attachment.document_FK);

                    // Delete Old Attachment
                    int uploadedAttachSequenceOld = UploadedSequenceAttachmentIdOld.HasValue ? UploadedSequenceAttachmentIdOld.Value : -1;
                    if (uploadedAttachSequenceOld != -1 && uploadedAttachSequenceOld != attachment.attachment_PK)
                    {
                        _attachmentToDelete = _attachmentOperations.GetEntityWithoutDiskFile(uploadedAttachSequenceOld);
                        if (_attachmentToDelete != null) _attachmentOperations.Delete(uploadedAttachSequenceOld);
                    }
                }
            }
            else if (UploadedSequenceAttachmentIdOld.HasValue)
            {
                Attachment_PK attachment = _attachmentOperations.GetEntityWithoutDiskFile(UploadedSequenceAttachmentIdOld.Value);
                if (attachment != null)
                {
                    _attachmentOperations.Delete(UploadedSequenceAttachmentIdOld);

                    if (submissionUnit.ness_FK != null)
                    {
                        List<Attachment_PK> assignedAttachments = _attachmentOperations.GetAttachmentsForDocument((int)submissionUnit.ness_FK);

                        if (assignedAttachments.Count == 0)
                        {
                            _documentOperations.Delete(submissionUnit.ness_FK);
                            submissionUnit.ness_FK = null;
                        }
                    }
                }
            }

            if (UploadedSequenceWorkingAttachmentId.HasValue)
            {
                Attachment_PK attachment = _attachmentOperations.GetEntityWithoutDiskFile(UploadedSequenceWorkingAttachmentId.Value);
                if (attachment != null)
                {
                    attachment.document_FK = submissionUnit.ness_FK;
                    attachment.userID = SessionManager.Instance.CurrentUser.UserID;
                    _attachmentOperations.SaveLinkToDocument(attachment.attachment_PK, attachment.document_FK);

                    // Delete Old Attachment
                    int uploadedAttachWorkingOld = UploadedSequenceWorkingAttachmentIdOld.HasValue ? UploadedSequenceWorkingAttachmentIdOld.Value : -1;
                    if (uploadedAttachWorkingOld != -1 && uploadedAttachWorkingOld != attachment.attachment_PK)
                    {
                        _attachmentToDelete = _attachmentOperations.GetEntityWithoutDiskFile(uploadedAttachWorkingOld);
                        if (_attachmentToDelete != null) _attachmentOperations.Delete(uploadedAttachWorkingOld);
                    }
                }
            }
            else if (UploadedSequenceWorkingAttachmentIdOld.HasValue)
            {
                Attachment_PK attachment = _attachmentOperations.GetEntityWithoutDiskFile(UploadedSequenceWorkingAttachmentIdOld.Value);
                if (attachment != null)
                {
                    _attachmentOperations.Delete(UploadedSequenceWorkingAttachmentIdOld);

                    if (submissionUnit.ness_FK != null)
                    {
                        List<Attachment_PK> assignedAttachments = _attachmentOperations.GetAttachmentsForDocument((int)submissionUnit.ness_FK);

                        if (assignedAttachments.Count == 0)
                        {
                            _documentOperations.Delete(submissionUnit.ness_FK);
                            submissionUnit.ness_FK = null;
                        }
                    }
                }
            }
        }

        private void SaveAttachments(Subbmission_unit_PK submissionUnit)
        {
            var attachmentsToSave = _attachmentOperations.GetAttachmentsBySessionId(upAttachments.AttachmentSessionId);
            var refAttachmentPkListToDelete = _attachmentPkListToDelete;
            attachmentsToSave = attachmentsToSave.Where(a => a.attachment_PK != null && !refAttachmentPkListToDelete.Contains(a.attachment_PK.Value)).ToList();

            if (!submissionUnit.document_FK.HasValue && attachmentsToSave.Any())
            {
                var document = new Document_PK();
                document = _documentOperations.Save(document);
                if (document.document_PK.HasValue)
                {
                    submissionUnit.document_FK = document.document_PK;
                    _submissionUnitOperations.Save(submissionUnit);
                }
            }

            foreach (var attachment in attachmentsToSave)
            {
                _attachmentOperations.SaveLinkToDocument(attachment.attachment_PK, submissionUnit.document_FK);
            }

            foreach (var attachmentPk in refAttachmentPkListToDelete)
            {
                _attachmentOperations.Delete(attachmentPk);
            }
        }

        private void SaveProducts(int? submissionUnitPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var productList = _productOperations.GetAssignedProductsForSubmissionUnit(submissionUnitPk);
            productList.SortByField(x => x.name);

            foreach (var product in productList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += product.name;
            }

            _productSubmissionUnitMnOperations.DeleteBySubmissionUnitPK(submissionUnitPk);

            var submissionUnitProductMnList = new List<Product_submission_unit_PK>(lbSrProducts.LbInput.Items.Count);

            foreach (ListItem listItem in lbSrProducts.LbInput.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                submissionUnitProductMnList.Add(new Product_submission_unit_PK(null, int.Parse(listItem.Value), submissionUnitPk));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (submissionUnitProductMnList.Count > 0)
            {
                _productSubmissionUnitMnOperations.SaveCollection(submissionUnitProductMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, submissionUnitPk.ToString(), "PRODUCT_SUB_UNIT_MN");
        }

        private void SaveAgencies(int? submissionUnitPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = string.Empty;
            var complexAuditOldValue = string.Empty;

            var agencyList = _organizationOperations.GetAssignedAgenciesForSubmissionUnit(submissionUnitPk);
            agencyList.SortByField(x => x.name_org);

            foreach (var agency in agencyList)
            {
                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += "|||";
                complexAuditOldValue += agency.name_org;
            }

            _suAgencyMnOperations.DeleteBySubmissionUnitPK(submissionUnitPk);

            var suAgencyMnList = new List<Su_agency_mn_PK>(lbAuAgencies.LbInputTo.Items.Count);

            foreach (ListItem listItem in lbAuAgencies.LbInputTo.Items)
            {
                if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                suAgencyMnList.Add(new Su_agency_mn_PK(null, int.Parse(listItem.Value), submissionUnitPk));

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += "|||";
                complexAuditNewValue += listItem.Text;
            }

            if (suAgencyMnList.Count > 0)
            {
                _suAgencyMnOperations.SaveCollection(suAgencyMnList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, submissionUnitPk.ToString(), "SU_AGENCY_MN");
        }

        #endregion

        #region Delete

        private void DeleteEntity(object arg)
        {

        }

        #endregion

        #endregion

        #region Event handlers

        #region Attachments panel

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            if (AttachmentPkToDelete.HasValue)
            {
                var refAttachmentPkListToDelete = _attachmentPkListToDelete;
                refAttachmentPkListToDelete.Add(AttachmentPkToDelete.Value);

                BindUpAttachments();
                BindSequenceAttachments();
            }
        }

        protected void GvData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var btnDownload = (LinkButton)e.Row.FindControl("btnDownload");
                if (!string.IsNullOrWhiteSpace(btnDownload.CommandArgument))
                {
                    var rootLocation = Support.LocationManager.Instance.GetLocationByName("Root", Support.CacheManager.Instance.AppLocations);
                    if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation))
                    {
                        btnDownload.Attributes.Add("onclick", "SaveTheDownloadBtnAttach('" + btnDownload.ClientID + "," + btnDownload.CommandArgument + "');");
                    }
                }
            }
        }

        protected void UpAttachmentsAsyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
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

        protected void UpSubmissionFormatSequenceTypeAsyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            Thread.Sleep(2000);

            if (upSubmissionFormatSequenceType.AsyncFileUpload.HasFile && upSubmissionFormatSequenceType.AsyncFileUpload.FileBytes.Count() > 0)
            {
                if (CheckSequence()) SaveSequenceAttachment();
                upSubmissionFormatSequenceType.RefreshAttachments = true;
            }
        }

        protected void UpSubmissionFormatSequenceTypeWorkingAsyncFileUpload_UploadedComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            Thread.Sleep(2000);

            if (upSubmissionFormatSequenceTypeWorking.AsyncFileUpload.HasFile && upSubmissionFormatSequenceTypeWorking.AsyncFileUpload.FileBytes.Count() > 0)
            {
                if (CheckWorkingSequence()) SaveWorkingSequenceAttachment();
                upSubmissionFormatSequenceTypeWorking.RefreshAttachments = true;
            }
        }

        protected void upAttachments_OnDelete(object sender, EventArgs e)
        {
            var button = (sender as ImageButton);

            if (button == null || string.IsNullOrWhiteSpace(button.CommandArgument)) return;

            int attachmentPkToDelete;
            var showPopup = false;

            if (int.TryParse(button.CommandArgument, out attachmentPkToDelete))
            {
                AttachmentPkToDelete = attachmentPkToDelete;
                showPopup = true;
            }

            if (showPopup) mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
        }

        #endregion

        #region Context menu

        public void OnContextMenuItemClick(object sender, ContextMenuEventArgs e)
        {
            switch (e.EventType)
            {
                case ContextMenuEventTypes.Cancel:
                    if (EntityContext == EntityContext.SubmissionUnit)
                    {
                        if ((FormType == FormType.Edit || FormType == FormType.SaveAs) && _idSubUnit.HasValue) Response.Redirect(string.Format("~/Views/SubmissionUnitView/Preview.aspx?EntityContext={0}&idSubUnit={1}", EntityContext, _idSubUnit));
                    }
                    else if (EntityContext == EntityContext.Product)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "ProdSubUnitList" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                            else if (From == "ProdPreview" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext, _idProd));
                            else if (From == "Prod") Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "ProdSearch") Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                        }
                    }
                    else if (EntityContext == EntityContext.Task)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "TaskSubUnitList" && _idTask.HasValue) Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}&idTask={1}", EntityContext, _idTask));
                            else if (From == "TaskPreview" && _idTask.HasValue) Response.Redirect(string.Format("~/Views/TaskView/Preview.aspx?EntityContext={0}&idTask={1}", EntityContext, _idTask));
                            else if (From == "Task") Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "TaskSearch") Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if (From == "ActTaskList" && _idAct.HasValue) Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}&idAct={1}", EntityContext.Activity, _idAct));
                            else if (From == "ActMyTaskList" && _idAct.HasValue) Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}&idAct={1}", EntityContext.ActivityMy, _idAct));
                        }
                    }
                    else if (EntityContext == EntityContext.Activity || EntityContext == EntityContext.ActivityMy)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "Act" || From == "ActMy") Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}", EntityContext));
                            else if (From == "ActSearch") Response.Redirect(string.Format("~/Views/ActivityView/List.aspx?EntityContext={0}&Action=Search", EntityContext));
                            else if ((From == "ActPreview" || From == "ActMyPreview") && _idAct.HasValue) Response.Redirect(string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                            else if ((From == "ActSubUnitList" || From == "ActMySubUnitList") && _idAct.HasValue) Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                            else if (_idAct.HasValue) Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}&idAct={1}", EntityContext, _idAct));
                        }
                    }
                    else if (EntityContext == EntityContext.Default)
                    {
                        if (FormType == FormType.New && !string.IsNullOrWhiteSpace(From))
                        {
                            if (From == "SubUnit") Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}", EntityContext.SubmissionUnit));
                        }
                    }
                    Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}", EntityContext.SubmissionUnit));
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedSubmissionUnit = SaveForm(null);
                        if (savedSubmissionUnit is Subbmission_unit_PK)
                        {
                            var submissionUnit = savedSubmissionUnit as Subbmission_unit_PK;
                            if (submissionUnit.subbmission_unit_PK.HasValue)
                            {
                                MasterPage.OneTimePermissionToken = Permission.View;
                                Response.Redirect(string.Format("~/Views/SubmissionUnitView/Preview.aspx?EntityContext={0}&idSubUnit={1}", EntityContext.SubmissionUnit, submissionUnit.subbmission_unit_PK));
                            }
                        }
                        Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}", EntityContext.SubmissionUnit));
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

        #region Products searcher

        private void LbSrProductsSearcher_OnOkButtonClick(object sender, FormEventArgs<List<int>> formEventArgs)
        {
            foreach (var selectedId in lbSrProducts.Searcher.SelectedItems)
            {
                if (lbSrProducts.LbInput.Items.FindByValue(selectedId.ToString()) != null) continue;

                var product = _productOperations.GetEntity(selectedId);

                if (product == null || !product.product_PK.HasValue) continue;

                lbSrProducts.LbInput.Items.Add(new ListItem(product.GetNameFormatted(Constant.UnknownValue), selectedId.ToString()));
                FillDdlActivity();
                FillDdlTask();
                BindRms();
            }
        }

        void LbSrProducts_OnRemoveClick(object sender, EventArgs e)
        {
            lbSrProducts.LbInput.RemoveSelected();
            FillDdlActivity();
            FillDdlTask();
        }

        #endregion

        #region Agencies

        private void lbAuAgencies_OnUnassignClick(object sender, EventArgs e)
        {
            lbAuAgencies.LbInputTo.MoveSelectedItemsTo(lbAuAgencies.LbInputFrom);
            lbAuAgencies.LbInputFrom.SortItemsByText();
        }

        private void lbAuAgencies_OnAssignClick(object sender, EventArgs e)
        {
            lbAuAgencies.LbInputFrom.MoveSelectedItemsTo(lbAuAgencies.LbInputTo);
            lbAuAgencies.LbInputTo.SortItemsByText();
        }

        #endregion

        #region Submission format

        void DdlSubmissionFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetSubmissionFormatVisibleFields();
        }

        #endregion

        #region Activity

        void DdlActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDdlTask();
            BindRms();
        }

        #endregion

        #endregion

        #region Support methods

        #region Sequences

        private bool CheckSequence()
        {
            var selectedSubmissionFormat = ddlSubmissionFormat.SelectedItem;
            var sequenceType = SequenceType.Other;
            if (selectedSubmissionFormat != null)
            {
                sequenceType = ConvertFromString(selectedSubmissionFormat.Text.ToLower());
            }

            if (sequenceType == SequenceType.eCTD || sequenceType == SequenceType.NeeS)
            {
                string fileName = upSubmissionFormatSequenceType.AsyncFileUpload.FileName;
                string fullPath = Server.MapPath("~") + "\\" + ConfigurationManager.AppSettings["AttachTmpDir"] + "\\" + fileName;
                File.WriteAllBytes(fullPath, upSubmissionFormatSequenceType.AsyncFileUpload.FileBytes);
                UploadedSequenceFullPath = fullPath;
                _sequenceStatus = CheckSequenceValidity(sequenceType, fullPath, true);

                if (_sequenceStatus == SequenceStatus.Valid || _sequenceStatus == SequenceStatus.CheckForProductSet)
                {
                    ListItemCollection productList = lbSrProducts.LbInput.Items;
                    foreach (ListItem item in productList)
                    {
                        var product = new Product_PK();
                        int productPk = 0;

                        if (item.Value != null && item.Value.Trim() != "")
                        {
                            productPk = Convert.ToInt32(item.Value);
                            product = _productOperations.GetEntity(productPk);

                            if (_sequenceStatus == SequenceStatus.Valid)
                            {
                                _sequenceStatus = SequenceStatus.NewSequence;
                            }
                            else if (_sequenceStatus == SequenceStatus.CheckForProductSet)
                            {
                                _sequenceStatus = ResolveProductSet(product, true);
                            }
                        }

                        // handle multiple product SequenceStatus to dictionary
                        if (productPk != 0 && !_productSequenceStatus.ContainsKey(productPk)) _productSequenceStatus.Add(productPk, _sequenceStatus);
                    }

                    _saveAttachment = true;
                    foreach (SequenceStatus status in _productSequenceStatus.Values)
                    {
                        if (status == SequenceStatus.SequenceAlreadyExist)
                        {
                            _saveAttachment = false;
                            break;
                        }
                    }

                    if (_saveAttachment)
                    {
                        return true;
                    }
                    else
                    {
                        UploadSequenceErrorMessage = ConvertToString(SequenceStatus.SequenceAlreadyExist);
                        return false;
                    }
                }
                else
                {
                    UploadSequenceErrorMessage = ConvertToString(_sequenceStatus);
                    return false;
                }
            }

            return true;
        }

        private bool CheckWorkingSequence()
        {
            string fileName = upSubmissionFormatSequenceTypeWorking.AsyncFileUpload.FileName;
            string fullPath = Server.MapPath("~") + "\\" + ConfigurationManager.AppSettings["AttachTmpDir"] + "\\" + fileName;
            File.WriteAllBytes(fullPath, upSubmissionFormatSequenceTypeWorking.AsyncFileUpload.FileBytes);

            _sequenceStatus = CheckWorkingSequenceValidity(fullPath);
            if (_sequenceStatus == SequenceStatus.Valid) return true;

            UploadSequenceErrorMessage = ConvertToString(_sequenceStatus);
            return false;
        }

        private void SaveSequenceAttachment()
        {
            try
            {
                var attachement = new Attachment_PK();
                attachement.attachmentname = upSubmissionFormatSequenceType.AsyncFileUpload.FileName;
                attachement.filetype = upSubmissionFormatSequenceType.AsyncFileUpload.ContentType;
                attachement.type_for_fts = AttachmentHelper.GetFileExtension(attachement.attachmentname);
                attachement.disk_file = upSubmissionFormatSequenceType.AsyncFileUpload.FileBytes;
                attachement.session_id = upSubmissionFormatSequenceType.AttachmentSessionId;
                attachement.modified_date = DateTime.Now;
                attachement.userID = SessionManager.Instance.CurrentUser.UserID;

                attachement = _attachmentOperations.Save(attachement);

                UploadedSequenceAttachmentId = attachement.attachment_PK;
            }
            catch (Exception ex)
            {
                UploadSequenceErrorMessage = ConvertToString(SequenceStatus.ErrorSavingFileToDb);
            }
        }

        private void SaveWorkingSequenceAttachment()
        {
            try
            {
                var attachement = new Attachment_PK();

                attachement.attachmentname = upSubmissionFormatSequenceTypeWorking.AsyncFileUpload.FileName;
                attachement.filetype = upSubmissionFormatSequenceTypeWorking.AsyncFileUpload.ContentType;
                attachement.type_for_fts = AttachmentHelper.GetFileExtension(attachement.attachmentname);
                attachement.disk_file = upSubmissionFormatSequenceTypeWorking.AsyncFileUpload.FileBytes;
                attachement.session_id = upSubmissionFormatSequenceTypeWorking.AttachmentSessionId;
                attachement.userID = SessionManager.Instance.CurrentUser.UserID;
                attachement.modified_date = DateTime.Now;

                attachement = _attachmentOperations.Save(attachement);

                UploadedSequenceWorkingAttachmentId = attachement.attachment_PK;
            }
            catch (Exception ex)
            {
                UploadSequenceErrorMessage = ConvertToString(SequenceStatus.ErrorSavingFileToDb);
            }
        }

        private SequenceStatus CheckSequenceValidity(SequenceType sequenceType, string fullPath, bool checkForZipExtensionAndName)
        {
            if (checkForZipExtensionAndName)
            {
                // Check uploaded file rules
                // Check unzipped rules
                // Uploaded file must be .zip type
                string fileExtension = String.Empty;
                if (!String.IsNullOrWhiteSpace(fullPath))
                {
                    var pathExtension = Path.GetExtension(fullPath);
                    if (pathExtension != null) fileExtension = pathExtension.Trim().ToLower();
                }
                if (fileExtension == ".zip")
                {
                    // Uploaded file name must match entered sequence name, ctlSequence control
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);
                    if (fileNameWithoutExtension != null)
                    {
                        string fileName = fileNameWithoutExtension.Trim().ToLower();

                        string fileNameEnterned = String.Empty;
                        if (!String.IsNullOrWhiteSpace(txtSequence.Text)) fileNameEnterned = txtSequence.Text.Trim().ToLower();
                        if (fileName == fileNameEnterned)
                        {
                            //  Check zipped rules
                            ZipFile uploadedZipFile;
                            try
                            {
                                uploadedZipFile = ZipFile.Read(fullPath);
                            }
                            catch (Exception ex)
                            {
                                return SequenceStatus.ErrorReadingZipFile;
                            }

                            if (uploadedZipFile != null)
                            {
                                // Root zip file must contain only one folder
                                bool hasOnlyOneRootFolder = false;

                                try
                                {
                                    hasOnlyOneRootFolder = uploadedZipFile[1].FileName.IndexOf("/") == uploadedZipFile[1].FileName.LastIndexOf("/");
                                }
                                catch (Exception e)
                                {
                                    return SequenceStatus.InvalidSequenceZipStructure;
                                }

                                if (hasOnlyOneRootFolder)
                                {
                                    // Root must be directory
                                    if (uploadedZipFile[0].IsDirectory)
                                    {
                                        // Folder name must match uploaded .zip file (and entered sequence name) 
                                        string uploadedZipFileName = String.Empty;
                                        if (!String.IsNullOrWhiteSpace(uploadedZipFile[0].FileName)) uploadedZipFileName = uploadedZipFile[0].FileName.Trim().ToLower().Remove(4, 1);
                                        if (uploadedZipFileName == fileNameEnterned)
                                        {
                                            // Submission format rules
                                            // eCTD type must have index.xml file
                                            if (sequenceType == SequenceType.eCTD)
                                            {
                                                if (uploadedZipFile.ContainsEntry(fileNameEnterned + "/index.xml")) return SequenceStatus.CheckForProductSet;
                                                else return SequenceStatus.IndexXmlDontExist;
                                            }
                                            // NeeS type must have ctd-toc.pdf file
                                            else if (sequenceType == SequenceType.NeeS)
                                            {
                                                if (uploadedZipFile.ContainsEntry(fileNameEnterned + "/ctd-toc.pdf")) return SequenceStatus.Valid;
                                                else return SequenceStatus.CtdTocPdfDontExist;
                                            }
                                        }
                                        else
                                        {
                                            return SequenceStatus.ZipFilenameDontMatchRootFolder;
                                        }
                                    }
                                    else
                                    {
                                        return SequenceStatus.ZipRootIsNotDirectory;
                                    }
                                }
                                else
                                {
                                    return SequenceStatus.ZippedFileContainsMultipleRootFolders;
                                }

                            }
                            else
                            {
                                return SequenceStatus.NullReference;
                            }
                        }
                        else
                        {
                            return SequenceStatus.ZipFilenameDontMatchSequence;
                        }
                    }
                }
                else
                {
                    return SequenceStatus.NotZipFile;
                }

                return SequenceStatus.Invalid;
            }
            else
            {
                string fileExtension = String.Empty;
                if (!String.IsNullOrWhiteSpace(fullPath))
                {
                    var extension = Path.GetExtension(fullPath);
                    if (extension != null) fileExtension = extension.Trim().ToLower();
                }
                if (sequenceType == SequenceType.eCTD)
                {
                    if (fileExtension != ".xml") return SequenceStatus.IndexXmlDontExist;
                    else return SequenceStatus.CheckForProductSet;
                }
                // NeeS type must have ctd-toc.pdf file
                else if (sequenceType == SequenceType.NeeS)
                {
                    if (fileExtension != ".pdf") return SequenceStatus.CtdTocPdfDontExist;
                    else return SequenceStatus.Valid;
                }

                return SequenceStatus.Invalid;
            }
        }

        private SequenceStatus CheckWorkingSequenceValidity(string fullPath)
        {
            string fileExtension = String.Empty;
            if (!String.IsNullOrWhiteSpace(fullPath))
            {
                var extension = Path.GetExtension(fullPath);
                if (extension != null) fileExtension = extension.Trim().ToLower();
            }
            if (fileExtension == ".zip")
            {
                // Uploaded file name must match entered sequence name, ctlSequence control + contain "working"
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fullPath);
                if (fileNameWithoutExtension != null)
                {
                    string fileName = fileNameWithoutExtension.Trim().ToLower();

                    string fileNameEnterned = String.Empty;
                    if (!String.IsNullOrWhiteSpace(txtSequence.Text)) fileNameEnterned = txtSequence.Text.Trim().ToLower();
                    if (fileName.Contains("working"))
                    {
                        if (fileName.Substring(0, 4) == fileNameEnterned)
                        {
                            // Check zipped rules
                            ZipFile uploadedZipFile = null;
                            try
                            {
                                uploadedZipFile = ZipFile.Read(fullPath);
                            }
                            catch (Exception ex)
                            {
                                return SequenceStatus.ErrorReadingZipFile;
                            }

                            if (uploadedZipFile != null)
                            {
                                return SequenceStatus.Valid;
                            }
                            else
                            {
                                return SequenceStatus.NullReference;
                            }
                        }
                        else
                        {
                            return SequenceStatus.ZipFilenameDontMatchSequence;
                        }
                    }
                    else
                    {
                        return SequenceStatus.NotWorkingSequence;
                    }
                }

                return SequenceStatus.FilenameWithoutExtension;
            }
            else
            {
                return SequenceStatus.NotZipFile;
            }
        }

        private SequenceStatus CheckProductSet()
        {
            ListItemCollection productList = lbSrProducts.LbInput.Items;
            foreach (ListItem item in productList)
            {
                int productPk = 0;

                if (item.Value != null && item.Value.Trim() != "")
                {
                    productPk = Convert.ToInt32(item.Value);
                    var product = _productOperations.GetEntity(productPk);

                    _sequenceStatus = ResolveProductSet(product, true);
                }

                // Handle multiple product SequenceStatus to dictionary
                if (productPk != 0 && !_productSequenceStatus.ContainsKey(productPk)) _productSequenceStatus.Add(productPk, _sequenceStatus);
            }

            foreach (SequenceStatus status in _productSequenceStatus.Values)
            {
                if (status == SequenceStatus.SequenceAlreadyExist && InitialSequence != txtSequence.Text)
                {
                    return SequenceStatus.SequenceAlreadyExist;
                }
            }

            if (FormType == FormType.Edit) return SequenceStatus.Valid;
            return SequenceStatus.NewSequence;
        }

        private SequenceStatus CheckSequenceTextChange()
        {
            var sequenceTypeS = ddlSubmissionFormat.SelectedId != null ? ddlSubmissionFormat.SelectedItem.Text.Trim().ToLower() : String.Empty;
            SequenceType sequenceType = ConvertFromString(sequenceTypeS);

            if (sequenceType == SequenceType.eCTD || sequenceType == SequenceType.NeeS)
            {
                var refAttachmentPkListToDelete = _attachmentPkListToDelete;

                if (UploadedSequenceAttachmentId.HasValue && !refAttachmentPkListToDelete.Contains(UploadedSequenceAttachmentId.Value))
                {
                    Attachment_PK attachment = _attachmentOperations.GetEntity(UploadedSequenceAttachmentId);
                    if (attachment != null)
                    {
                        if (attachment.attachmentname.Substring(0, 4) != txtSequence.Text.Trim())
                        {
                            _sequenceStatus = SequenceStatus.ZipFilenameDontMatchSequence;
                        }
                        else
                        {
                            _sequenceStatus = sequenceType == SequenceType.eCTD ? CheckProductSet() : SequenceStatus.Valid;
                        }
                    }
                }
                else
                {
                    _sequenceStatus = sequenceType == SequenceType.eCTD ? CheckProductSet() : SequenceStatus.Valid;
                }

                if (_sequenceStatus == SequenceStatus.Valid)
                {
                    if (UploadedSequenceWorkingAttachmentId.HasValue && !refAttachmentPkListToDelete.Contains(UploadedSequenceWorkingAttachmentId.Value))
                    {
                        Attachment_PK attachment = _attachmentOperations.GetEntity(UploadedSequenceWorkingAttachmentId);
                        if (attachment != null)
                        {
                            _sequenceStatus = attachment.attachmentname.Substring(0, 4) != txtSequence.Text.Trim() ? SequenceStatus.ZipFilenameDontMatchSequence : SequenceStatus.Valid;
                        }
                    }
                }
            }

            return _sequenceStatus;
        }

        private SequenceStatus ResolveProductSet(Product_PK product, bool sequenceAlreadyExists)
        {
            // Check Product - Submission unit rules
            // Is there any submission unit for this product, table [PRODUCT_SUB_UNIT_MN]
            DataSet productSuMn = _productSubmissionUnitMnOperations.GetSUByProduct(product.product_PK);
            if (productSuMn.Tables.Count > 0 && productSuMn.Tables[0].Rows.Count > 0)
            {
                DataRow dr = productSuMn.Tables[0].Rows[0];
                if (dr["submission_unit_FK"] != null)
                {
                    // Check if this product already has productSet, take first SU from PRODUCT_SUB_UNIT_MN and check if there is productSet defined, nees_FK && ectd_FK IS NOT NULL
                    int submission_unit_FK = Convert.ToInt32(dr["submission_unit_FK"]);
                    Subbmission_unit_PK su = _submissionUnitOperations.GetEntity(submission_unit_FK);

                    if (su != null)
                    {
                        if (su.ness_FK != null && su.ectd_FK != null)
                        {
                            // productSet = new Document_PK, handled in Save method
                            return SequenceStatus.NewSequence;
                        }
                        else
                        {
                            // Check if this productSet has this sequence
                            Int32? productSet = 0;

                            if (su.ectd_FK != null) productSet = su.ectd_FK;

                            if (productSet != null)
                            {
                                List<Attachment_PK> attachmentsList = _attachmentOperations.GetAttachmentsForDocument((int)productSet);

                                string sequence = !String.IsNullOrWhiteSpace(txtSequence.Text) ? txtSequence.Text.Trim().ToLower() : String.Empty;

                                // Remove .zip from attachment name and compare it to entered sequence
                                if (sequenceAlreadyExists) sequenceAlreadyExists = attachmentsList.Find(item => item.attachmentname.Substring(0, 4) == sequence) != null;

                                if (sequenceAlreadyExists)
                                {
                                    // error, handled in FileUploadSequence_UploadedComplete method
                                    return SequenceStatus.SequenceAlreadyExist;
                                }
                                else
                                {
                                    // productSet = nees_FK/ectd_FK od the first SU, handled in Save method
                                    ProductSet = productSet != 0 ? productSet : null;
                                    if (productSet != 0) return SequenceStatus.ProductSetAlreadyExist;
                                    else return SequenceStatus.NewSequence;
                                }
                            }
                            else
                            {
                                return SequenceStatus.NullReference;
                            }
                        }
                    }
                    else
                    {
                        return SequenceStatus.NullReference;
                    }
                }
                else
                {
                    return SequenceStatus.NullReference;
                }
            }
            else
            {
                // productSet = new Document_PK, handled in Save method
                return SequenceStatus.NewSequence;
            }
        }

        private string ConvertToString(SequenceStatus sequenceStatus)
        {
            string sequenceTypeString = sequenceStatus.ToString();
            switch (sequenceStatus)
            {
                case SequenceStatus.CtdTocPdfDontExist:
                    sequenceTypeString = "ctd-toc.pdf don't exist.";
                    break;
                case SequenceStatus.ErrorReadingZipFile:
                    sequenceTypeString = "Error reading zip file.";
                    break;
                case SequenceStatus.IndexXmlDontExist:
                    sequenceTypeString = "index.xml don't exist.";
                    break;
                case SequenceStatus.Invalid:
                    sequenceTypeString = "Invalid.";
                    break;
                case SequenceStatus.NewSequence:
                    sequenceTypeString = "New sequence";
                    break;
                case SequenceStatus.NotZipFile:
                    sequenceTypeString = "Not zip file.";
                    break;
                case SequenceStatus.NullReference:
                    sequenceTypeString = "Null reference.";
                    break;
                case SequenceStatus.OtherType:
                    sequenceTypeString = "Other type.";
                    break;
                case SequenceStatus.ProductSetAlreadyExist:
                    sequenceTypeString = "Product set already exists.";
                    break;
                case SequenceStatus.SequenceAlreadyExist:
                    sequenceTypeString = "Sequence already exists.";
                    break;
                case SequenceStatus.Valid:
                    sequenceTypeString = "Valid.";
                    break;
                case SequenceStatus.ZipFilenameDontMatchRootFolder:
                    sequenceTypeString = "Zip filename don't match root folder.";
                    break;
                case SequenceStatus.ZipFilenameDontMatchSequence:
                    sequenceTypeString = "Zip filename don't match root sequence.";
                    break;
                case SequenceStatus.ZipRootIsNotDirectory:
                    sequenceTypeString = "Zip root is not directory.";
                    break;
                case SequenceStatus.ZippedFileContainsMultipleRootFolders:
                    sequenceTypeString = "Zipped file contains multiple folders.";
                    break;
                case SequenceStatus.NotWorkingSequence:
                    sequenceTypeString = "Not working sequence.";
                    break;
                case SequenceStatus.CheckForProductSet:
                    sequenceTypeString = "Check for product set.";
                    break;
                case SequenceStatus.ErrorSavingFileToDb:
                    sequenceTypeString = "Error while saving file to database.";
                    break;
                case SequenceStatus.InvalidSequenceZipStructure:
                    sequenceTypeString = "Invalid sequence zip structure.";
                    break;
            }

            return sequenceTypeString;
        }

        private SequenceType ConvertFromString(string sequenceTypeString)
        {
            if (!String.IsNullOrWhiteSpace(sequenceTypeString))
            {
                if (sequenceTypeString == "ectd") return SequenceType.eCTD;
                else if (sequenceTypeString == "nees") return SequenceType.NeeS;
                else return SequenceType.Other;
            }

            return SequenceType.Other;
        }

        #endregion

        private void SetSubmissionFormatVisibleFields()
        {
            var selectedSubmissionFormat = ddlSubmissionFormat.SelectedItem;
            if (selectedSubmissionFormat != null && (selectedSubmissionFormat.Text.ToLower() == "ectd" || selectedSubmissionFormat.Text.ToLower() == "nees"))
            {
                upSubmissionFormatSequenceType.Visible = upSubmissionFormatSequenceTypeWorking.Visible = true;
            }
            else
            {
                upSubmissionFormatSequenceType.Visible = upSubmissionFormatSequenceTypeWorking.Visible = false;
            }
        }

        private void GenerateContextMenuItems()
        {
            var contextMenu = new[]
            {
                new ContextMenuItem(ContextMenuEventTypes.Cancel, "Cancel"), 
                new ContextMenuItem(ContextMenuEventTypes.Save, "Save")
            };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);
        }

        private void GenerateTabMenuItems()
        {
            Location_PK location = null;

            if (EntityContext == EntityContext.Product) location = Support.LocationManager.Instance.GetLocationByName("ProdSubUnitList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Task) location = Support.LocationManager.Instance.GetLocationByName("TaskSubUnitList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Activity) location = Support.LocationManager.Instance.GetLocationByName("ActSubUnitList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.ActivityMy) location = Support.LocationManager.Instance.GetLocationByName("ActMySubUnitList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.SubmissionUnit) location = Support.LocationManager.Instance.GetLocationByName("SubUnitPreview", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Default)
            {
                location = Support.LocationManager.Instance.GetLocationByName("SubUnitFormNew", Support.CacheManager.Instance.AppLocations);
                if (location != null)
                {
                    MasterPage.TabMenu.GenerateMenuItemsByRights(Support.CacheManager.Instance.AppLocations, location);
                    MasterPage.TabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
                    tabMenu.Visible = false;
                }
                return;
            }

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
            Location_PK topLevelParent = null;

            if (EntityContext == EntityContext.Product) location = Support.LocationManager.Instance.GetLocationByName("ProdSubUnitList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Task) location = Support.LocationManager.Instance.GetLocationByName("TaskSubUnitList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.SubmissionUnit) location = Support.LocationManager.Instance.GetLocationByName("SubUnitPreview", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Activity) location = Support.LocationManager.Instance.GetLocationByName("ActSubUnitList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.ActivityMy) location = Support.LocationManager.Instance.GetLocationByName("ActMySubUnitList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Default)
            {
                location = Support.LocationManager.Instance.GetLocationByName("SubUnitFormNew", Support.CacheManager.Instance.AppLocations);

                if (location != null)
                {
                    topLevelParent = MasterPage.FindTopLevelParent(location);

                    MasterPage.CurrentLocation = location;
                    MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
                }
                return;
            }

            topLevelParent = MasterPage.FindTopLevelParent(location);
            MasterPage.CurrentLocation = location;
            MasterPage.TopMenu.GenerateNewTopMenu(Support.CacheManager.Instance.AppLocations, topLevelParent, location);
        }

        private void BindDynamicControls(object args)
        {
            BindRms();
            BindSequenceAttachments();
            BindUpAttachments();
        }

        private void BindRms()
        {
            Task_PK task = _taskOperations.GetEntity(ddlTask.SelectedId);
            if (task == null)
            {
                SetRmsNonVisibleNonRequired();
                return;
            }
            Activity_PK activity = _activityOperations.GetEntity(task.activity_FK);
            if (activity == null)
            {
                SetRmsNonVisibleNonRequired();
                return;
            }

            if (activity.procedure_type_FK == null)
            {
                SetRmsNonVisibleNonRequired();
                return;
            }

            Type_PK type = _typeOperations.GetEntity(activity.procedure_type_FK);
            if (type == null)
            {
                SetRmsNonVisibleNonRequired();
                return;
            }

            if (type.name.ToLower().Trim() == "decentralised" || type.name.ToLower().Trim() == "mutual-recognition")
            {
                SetRmsVisibleNonRequired();
            }
            else
            {
                SetRmsNonVisibleNonRequired();
            }
        }

        private void SetRmsNonVisibleNonRequired()
        {
            ddlRms.Visible = false;
            ddlRms.Required = false;
        }

        private void SetRmsVisibleNonRequired()
        {
            ddlRms.Visible = true;
            ddlRms.Required = false;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            var isPermittedInsertSubmissionUnit = false;
            if (EntityContext == EntityContext.Default) isPermittedInsertSubmissionUnit = SecurityHelper.IsPermitted(Permission.InsertSubmissionUnit);

            if (isPermittedInsertSubmissionUnit)
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

            if (MasterPage.RefererLocation != null)
            {
                isPermittedInsertSubmissionUnit = SecurityHelper.IsPermittedAny(new List<Permission> { Permission.InsertSubmissionUnit, Permission.SaveAsSubmissionUnit, Permission.EditSubmissionUnit }, MasterPage.RefererLocation);
                if (isPermittedInsertSubmissionUnit)
                {
                    SecurityHelper.SetControlsForReadWrite(
                                   MasterPage.ContextMenu,
                                   new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") },
                                   new List<Panel> { PnlForm },
                                   new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } }
                               );
                }
            }

            SecurityPageSpecificMy(_isResponsibleUser);

            return true;
        }

        #endregion
    }
}