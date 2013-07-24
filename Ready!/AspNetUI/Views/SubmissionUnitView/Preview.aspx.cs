using System;
using System.Linq;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using AspNetUIFramework;
using Ready.Model;
using CacheManager = AspNetUI.Support.CacheManager;
using LocationManager = AspNetUI.Support.LocationManager;

namespace AspNetUI.Views.SubmissionUnitView
{
    public partial class Preview : PreviewPage
    {
        #region Declarations

        private int? _idSubUnit;
        private int? _idAct;
        private int? _idTask;
        private bool? _isResponsibleUser;

        private IActivity_PKOperations _activityOperations;
        private IProduct_PKOperations _productOperations;
        private IType_PKOperations _typeOperations;
        private IOrganization_PKOperations _organizationOperations;
        private ICountry_PKOperations _countryOperations;
        private IPerson_PKOperations _personOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private ISubbmission_unit_PKOperations _submissionUnitOperations;
        private IAttachment_PKOperations _attachmentOperations;
        private ITask_PKOperations _taskOperations;
        private ITask_name_PKOperations _taskNameOperations;
        private IUSEROperations _userOperations;

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
            GenerateContexMenuItems();
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

            _idSubUnit = ValidationHelper.IsValidInt(Request.QueryString["idSubUnit"]) ? int.Parse(Request.QueryString["idSubUnit"]) : (int?)null;

            _activityOperations = new Activity_PKDAL();
            _productOperations = new Product_PKDAL();
            _typeOperations = new Type_PKDAL();
            _organizationOperations = new Organization_PKDAL();
            _countryOperations = new Country_PKDAL();
            _personOperations = new Person_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _submissionUnitOperations = new Subbmission_unit_PKDAL();
            _attachmentOperations = new Attachment_PKDAL();
            _taskOperations = new Task_PKDAL();
            _taskNameOperations = new Task_name_PKDAL();
            _userOperations = new USERDAL();
        }

        private void BindEventHandlers()
        {
            mpDelete.OnYesButtonClick += mpDelete_OnYesButtonClick;
            btnDelete.Click += btnDelete_OnClick;

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
            if (!_idSubUnit.HasValue) return;

            btnDelete.Visible = _submissionUnitOperations.AbleToDeleteEntity(_idSubUnit.Value);
            pnlFooter.CssClass = btnDelete.Visible ? "clear previewBottom" : "clear previewBottom35";

            BindDynamicControls(null);
        }

        #endregion

        #region Bind

        void BindForm(object arg)
        {
            if (!_idSubUnit.HasValue) return;

            var submissionUnit = _submissionUnitOperations.GetEntity(_idSubUnit);
            if (submissionUnit == null || !submissionUnit.subbmission_unit_PK.HasValue) return;

            // Entity
            // Submission unit description
            BindSubmissionUnitDescription(submissionUnit.description_type_FK);

            // Responsible user
            BindResponsibleUser(submissionUnit.person_FK);

            // Agencies
            BindAgencies(submissionUnit.subbmission_unit_PK);

            // RMS
            BindRms(submissionUnit.agency_role_FK);

            // Submission ID
            lblPrvSubmissionId.Text = !string.IsNullOrWhiteSpace(submissionUnit.submission_ID) ? submissionUnit.submission_ID : Constant.ControlDefault.LbPrvText;

            // Submission format
            BindSubmissionFormat(submissionUnit.s_format_FK);

            // Comment
            lblPrvComment.Text = !string.IsNullOrWhiteSpace(submissionUnit.comment) ? submissionUnit.comment : Constant.ControlDefault.LbPrvText;

            // Dispatch date
            lblPrvDispatchDate.Text = submissionUnit.dispatch_date.HasValue ? submissionUnit.dispatch_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Receipt date
            lblPrvReceiptDate.Text = submissionUnit.receipt_date.HasValue ? submissionUnit.receipt_date.Value.ToString(Constant.DateTimeFormat) : Constant.ControlDefault.LbPrvText;

            // Last change
            lblPrvLastChange.Text = LastChange.GetFormattedString(submissionUnit.subbmission_unit_PK, "SUBMISSION_UNIT", _lastChangeOperations, _personOperations);

            var user = _userOperations.GetUserByUsername(Thread.CurrentPrincipal.Identity.Name);
            if (user != null) _isResponsibleUser = submissionUnit.person_FK == user.Person_FK;
        }

        private void BindSubmissionTypeAttachments(string sequence, int? nessFk, int? ectdFk)
        {
            if (nessFk.HasValue)
            {
                if (FillSubmissionTypeLinks(sequence, nessFk.Value))
                {
                    lblPrvSubmissionTypeAttachments.Label = "NeeS attachments:";
                    return;
                }
            }
            else if (ectdFk.HasValue)
            {
                if (FillSubmissionTypeLinks(sequence, ectdFk.Value))
                {
                    lblPrvSubmissionTypeAttachments.Label = "eCTD attachments:";
                    return;
                }
            }

            lblPrvSubmissionTypeAttachments.Text = Constant.ControlDefault.LbPrvText;
        }

        private void BindSequence(string sequence, int? nessFk, int? ectdFk)
        {
            if (nessFk.HasValue)
            {
                if (FillSequenceLink(sequence, nessFk.Value)) return;
            }
            else if (ectdFk.HasValue)
            {
                if (FillSequenceLink(sequence, ectdFk.Value)) return;
            }

            lblPrvSequence.Text = !string.IsNullOrWhiteSpace(sequence) ? sequence : Constant.ControlDefault.LbPrvText;
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
                   
                    var rootLocation = LocationManager.Instance.GetLocationByName("Root", CacheManager.Instance.AppLocations);
                    if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation)) hlAttachment.NavigateUrl = string.Format("~/Views/Business/FileDownload.ashx?attachID={0}", attachment.attachment_PK);
                   
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

        private void BindSubmissionFormat(int? submissionFormatPk)
        {
            var submissionFormat = submissionFormatPk.HasValue ? _typeOperations.GetEntity(submissionFormatPk) : null;
            lblPrvSubmissionFormat.Text = submissionFormat != null && !string.IsNullOrWhiteSpace(submissionFormat.name) ? submissionFormat.name : Constant.ControlDefault.LbPrvText;
        }

        private void BindRms(int? agencyRolePk)
        {
            var rms = agencyRolePk.HasValue ? _countryOperations.GetEntity(agencyRolePk) : null;
            lblPrvRms.Text = rms != null ? rms.GetNameFormatted() : Constant.ControlDefault.LbPrvText;
        }

        private void BindAgencies(int? subbmissionUnitPk)
        {
            var agencyList = _organizationOperations.GetAssignedAgenciesForSubmissionUnit(subbmissionUnitPk);
            int agencyCount = agencyList.Count;
            if (agencyCount == 0) lblPrvAgencies.Text = Constant.ControlDefault.LbPrvText;
            else
            {
                lblPrvAgencies.Text = string.Empty;
                foreach (var agency in agencyList)
                {
                    lblPrvAgencies.Text += !string.IsNullOrWhiteSpace(agency.name_org) ? (--agencyCount) > 0 ? agency.name_org + ", " : agency.name_org : Constant.MissingValue;
                }
            }
        }

        private void BindSubmissionUnitDescription(int? descriptionTypeFk)
        {
            var submissionUnitDescription = descriptionTypeFk.HasValue ? _typeOperations.GetEntity(descriptionTypeFk) : null;
            lblPrvSubmissionUnit.Text = lblPrvSubmissionUnitDescription.Text = submissionUnitDescription != null && !string.IsNullOrWhiteSpace(submissionUnitDescription.name) ? submissionUnitDescription.name : Constant.ControlDefault.LbPrvText;
        }

        private void BindResponsibleUser(int? responsibleUserFk)
        {
            var responsibleUser = responsibleUserFk != null ? _personOperations.GetEntity(responsibleUserFk) : null;
            lblPrvResponsibleUser.Text = responsibleUser != null && !string.IsNullOrWhiteSpace(responsibleUser.FullName) ? responsibleUser.FullName : Constant.ControlDefault.LbPrvText;
        }

        private void BindDynamicControls(object arg)
        {
            if (!_idSubUnit.HasValue) return;

            var submissionUnit = _submissionUnitOperations.GetEntity(_idSubUnit);
            if (submissionUnit != null) _idTask = submissionUnit.task_FK;
            if (_idTask.HasValue)
            {
                var task = _taskOperations.GetEntity(_idTask);
                if (task != null && task.activity_FK.HasValue) _idAct = task.activity_FK;
            }

            // Products
            BindProducts(_idSubUnit.Value);

            // Activity
            if (_idAct.HasValue) BindActivity(_idAct.Value);

            // Task
            if (_idTask.HasValue) BindTask(_idTask.Value);

            // Sequence
            BindSequence(submissionUnit.sequence, submissionUnit.ness_FK, submissionUnit.ectd_FK);

            // Submission type attachments
            BindSubmissionTypeAttachments(submissionUnit.sequence, submissionUnit.ness_FK, submissionUnit.ectd_FK);

            // Attachments
            BindAttachments(submissionUnit.document_FK);
        }

        private void BindTask(int taskPk)
        {
            var task = _taskOperations.GetEntity(taskPk);
            if (task == null || !task.task_PK.HasValue)
            {
                lblPrvTask.Text = Constant.ControlDefault.LbPrvText;
                return;
            }

            var taskName = _taskNameOperations.GetEntity(task.task_name_FK);

            lblPrvTask.ShowLinks = true;
            lblPrvTask.TextBold = true;
            lblPrvTask.PnlLinks.Width = Unit.Pixel(800);

            lblPrvTask.PnlLinks.Controls.Add(new HyperLink
            {
                ID = string.Format("Task_{0}", task.task_PK),
                NavigateUrl = string.Format("~/Views/TaskView/Preview.aspx?EntityContext={0}&idTask={1}", EntityContext.Task, task.task_PK),
                Text = taskName != null && !string.IsNullOrWhiteSpace(taskName.task_name) ? taskName.task_name : Constant.MissingValue
            });
        }

        private void BindActivity(int activityPk)
        {
            var activity = _activityOperations.GetEntity(activityPk);
            if (activity == null || !activity.activity_PK.HasValue)
            {
                lblPrvActivity.Text = Constant.ControlDefault.LbPrvText;
                return;
            }

            lblPrvActivity.ShowLinks = true;
            lblPrvActivity.TextBold = true;
            lblPrvActivity.PnlLinks.Width = Unit.Pixel(800);

            lblPrvActivity.PnlLinks.Controls.Add(new HyperLink
                {
                    ID = string.Format("Activity_{0}", activity.activity_PK),
                    NavigateUrl = string.Format("~/Views/ActivityView/Preview.aspx?EntityContext={0}&idAct={1}", EntityContext.Activity, activity.activity_PK),
                    Text = !string.IsNullOrWhiteSpace(activity.name) ? activity.name : Constant.MissingValue
                });
        }

        private void BindProducts(int submissionUnitPk)
        {
            lblPrvProducts.Text = Constant.ControlDefault.LbPrvText;

            var productList = _productOperations.GetAssignedProductsForSubmissionUnit(submissionUnitPk);

            if (productList == null || productList.Count == 0) return;

            lblPrvProducts.ShowLinks = true;
            lblPrvProducts.TextBold = true;
            lblPrvProducts.PnlLinks.Width = Unit.Pixel(800);

            foreach (var product in productList)
            {
                lblPrvProducts.PnlLinks.Controls.Add(new HyperLink
                {
                    ID = string.Format("Product_{0}", product.product_PK),
                    NavigateUrl = string.Format("~/Views/ProductView/Preview.aspx?EntityContext={0}&idProd={1}", EntityContext.Product, product.product_PK),
                    Text = StringOperations.ReplaceNullOrWhiteSpace(product.name, Constant.UnknownValue)
                });
                lblPrvProducts.PnlLinks.Controls.Add(new LiteralControl("<br/>"));
            }
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

        void DeleteEntity(int? entityPk)
        {
            if (entityPk.HasValue)
            {
                try
                {
                    _submissionUnitOperations.Delete(entityPk);
                    Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}", EntityContext.SubmissionUnit));
                }
                catch { }

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
                        if (EntityContext == EntityContext.SubmissionUnit) Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Task) Response.Redirect(string.Format("~/Views/TaskView/List.aspx?EntityContext={0}", EntityContext));
                        else if (EntityContext == EntityContext.Product) Response.Redirect(string.Format("~/Views/ProductView/List.aspx?EntityContext={0}", EntityContext));
                        Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}", EntityContext.SubmissionUnit));
                    }
                    break;

                case ContextMenuEventTypes.Edit:
                    {
                        if (EntityContext == EntityContext.SubmissionUnit && _idSubUnit.HasValue) Response.Redirect(string.Format("~/Views/SubmissionUnitView/Form.aspx?EntityContext={0}&Action=Edit&idSubUnit={1}&From=SubUnitPreview", EntityContext, _idSubUnit));
                        Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}", EntityContext.SubmissionUnit));
                    }
                    break;

                case ContextMenuEventTypes.SaveAs:
                    {
                        if (EntityContext == EntityContext.SubmissionUnit && _idSubUnit.HasValue) Response.Redirect(string.Format("~/Views/SubmissionUnitView/Form.aspx?EntityContext={0}&Action=SaveAs&idSubUnit={1}&From=SubUnitPreview", EntityContext, _idSubUnit));
                        Response.Redirect(string.Format("~/Views/SubmissionUnitView/List.aspx?EntityContext={0}", EntityContext.SubmissionUnit));
                    }
                    break;
            }
        }

        #endregion

        #region Delete

        private void btnDelete_OnClick(object sender, EventArgs eventArgs)
        {
            mpDelete.ShowModalPopup("Warning!", "<center>Are you sure that you want to delete this record?</center><br />", ModalPopupMode.YesNo);
        }

        private void mpDelete_OnYesButtonClick(object sender, EventArgs e)
        {
            DeleteEntity(_idSubUnit);
        }

        #endregion

        #endregion

        #region Support methods

        private void GenerateContexMenuItems()
        {
            var contextMenu = new[]
            {
                new ContextMenuItem(ContextMenuEventTypes.Back, "Back"), 
                new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit"), 
                new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As")
            };

            MasterPage.ContextMenu.SetContextMenuItemsVisible(contextMenu);
        }

        private void GenerateTabMenuItems()
        {
            tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, MasterPage.CurrentLocation);
            tabMenu.SelectItem(MasterPage.CurrentLocation, Support.CacheManager.Instance.AppLocations);
        }

        private bool FillSequenceLink(string sequence, int documentFk)
        {
            var attachments = _attachmentOperations.GetAttachmentsForDocument(documentFk);
            if (attachments != null && attachments.Any())
            {
                attachments.RemoveAll(item => item.attachmentname.Substring(0, 4) != sequence || item.attachmentname.Contains("working"));
                var sequenceAttachment = attachments.FirstOrDefault();
                if (sequenceAttachment != null)
                {
                    lblPrvSequence.ShowLinks = true;
                    lblPrvSequence.TextBold = true;
                    var hlSequenceDisplay = new HyperLink();
                   
                    var rootLocation = LocationManager.Instance.GetLocationByName("Root", CacheManager.Instance.AppLocations);
                    if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation)) hlSequenceDisplay.NavigateUrl = string.Format("~/Views/Business/ShowSequenceFile.ashx?attachmentPk={0}", sequenceAttachment.attachment_PK);
                    
                    hlSequenceDisplay.Text = !string.IsNullOrWhiteSpace(sequence) ? sequence : Constant.ControlDefault.LbPrvText;
                    hlSequenceDisplay.Attributes.Add("target", "_blank");

                    lblPrvSequence.PnlLinks.Controls.Add(hlSequenceDisplay);
                    return true;
                }
            }
            return false;
        }

        private bool FillSubmissionTypeLinks(string sequence, int documentFk)
        {
            var attachments = _attachmentOperations.GetAttachmentsForDocument(documentFk);
            if (attachments != null && attachments.Any())
            {
                attachments.RemoveAll(item => item.attachmentname.Substring(0, 4) != sequence);

                Panel pnlLinks = null;
                int attachmentCount = attachments.Count;
                if (attachmentCount != 0)
                {
                    lblPrvSubmissionTypeAttachments.ShowLinks = true;
                    lblPrvSubmissionTypeAttachments.TextBold = true;
                    pnlLinks = lblPrvSubmissionTypeAttachments.PnlLinks;
                } 
                else
                {
                    lblPrvSubmissionTypeAttachments.Text = Constant.ControlDefault.LbPrvText;
                }

                foreach (var attachment in attachments)
                {
                    if (!attachment.attachment_PK.HasValue) continue;

                    var hlSequenceAttachment = new HyperLink();
                    hlSequenceAttachment.ID = string.Format("submissionTypeAttachment_{0}", attachment.attachment_PK);
                   
                    var rootLocation = LocationManager.Instance.GetLocationByName("Root", CacheManager.Instance.AppLocations); 
                    if (rootLocation != null && SecurityHelper.IsPermitted(Permission.DownloadAttachment, rootLocation)) hlSequenceAttachment.NavigateUrl = string.Format("~/Views/Business/FileDownload.ashx?attachID={0}", attachment.attachment_PK);
                    
                    hlSequenceAttachment.Text = !string.IsNullOrWhiteSpace(attachment.attachmentname) ? attachment.attachmentname : Constant.MissingValue;

                    if (pnlLinks != null)
                    {
                        pnlLinks.Controls.Add(hlSequenceAttachment);
                        if (--attachmentCount > 0) pnlLinks.Controls.Add(new LiteralControl("<br />"));
                    }
                }

                return true;
            }
            return false;
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            if (!base.SecurityPageSpecific())
            {
                if (SecurityHelper.IsPermitted(Permission.SaveAsSubmissionUnit)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.SaveAs, "Save As") });

                if (SecurityHelper.IsPermitted(Permission.EditSubmissionUnit)) MasterPage.ContextMenu.SetContextMenuItemsEnabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });
                else MasterPage.ContextMenu.SetContextMenuItemsDisabled(new[] { new ContextMenuItem(ContextMenuEventTypes.Edit, "Edit") });

                if (SecurityHelper.IsPermitted(Permission.DeleteSubmissionUnit)) StyleHelper.EnableLinkButtonsWithCssClass(PnlFooter, "Delete");
                else StyleHelper.DisableLinkButtonsWithCssClass(PnlFooter, "Delete");

                SecurityPageSpecificMy(_isResponsibleUser);
            }

            return true;
        }

        #endregion
    }
}