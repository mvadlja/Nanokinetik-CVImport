using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using AspNetUI.Views.Shared.Template;
using AspNetUI.Views.Shared.UserControl;
using Ready.Model;
using System.Transactions;
using AspNetUI.Support;
using AspNetUIFramework;

namespace AspNetUI.Views.AlerterView
{
    public partial class Form : FormPage
    {
        #region Declarations

        private int? _idAlert;
        private int? _idAct;
        private int? _idProd;
        private int? _idAuthProd;
        private int? _idTask;
        private int? _idProj;
        private int? _idDoc;

        private IReminder_PKOperations _reminderOperations;
        private IReminder_date_PKOperations _reminderDateOperations;
        private IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations;
        private IReminder_repeating_mode_PKOperations _reminderRepeatingModeOperations;
        private IReminder_user_status_PKOperations _reminderUserStatusOperations;
        private IPerson_PKOperations _personOperations;
        private ILast_change_PKOperations _lastChangeOperations;
        private IUSEROperations _userOperations;

        #endregion

        #region Properties

        public int? ResponsibleUserPk
        {
            get { return (int?)ViewState["ResponsibleUserPk"]; }
            set { ViewState["ResponsibleUserPk"] = value; }

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
            MasterPage.UpTopMenu.Update();
        }

        #endregion

        #region Form methods

        #region Initialize

        public override void LoadFormVariables()
        {
            base.LoadFormVariables();
            LoadActionQuery();

            _reminderOperations = new Reminder_PKDAL();
            _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            _reminderDateOperations = new Reminder_date_PKDAL();
            _reminderRepeatingModeOperations = new Reminder_repeating_mode_PKDAL();
            _reminderUserStatusOperations = new Reminder_user_status_PKDAL();
            _personOperations = new Person_PKDAL();
            _lastChangeOperations = new Last_change_PKDAL();
            _userOperations = new USERDAL();

            _idAlert = ValidationHelper.IsValidInt(Request.QueryString["idAlert"]) ? int.Parse(Request.QueryString["idAlert"]) : (int?)null;
            _idDoc = ValidationHelper.IsValidInt(Request.QueryString["idDoc"]) ? int.Parse(Request.QueryString["idDoc"]) : (int?)null;
            _idAuthProd = ValidationHelper.IsValidInt(Request.QueryString["idAuthProd"]) ? int.Parse(Request.QueryString["idAuthProd"]) : (int?)null;
            _idProd = ValidationHelper.IsValidInt(Request.QueryString["idProd"]) ? int.Parse(Request.QueryString["idProd"]) : (int?)null;
            _idAct = ValidationHelper.IsValidInt(Request.QueryString["idAct"]) ? int.Parse(Request.QueryString["idAct"]) : (int?)null;
            _idTask = ValidationHelper.IsValidInt(Request.QueryString["idTask"]) ? int.Parse(Request.QueryString["idTask"]) : (int?)null;
            _idProj = ValidationHelper.IsValidInt(Request.QueryString["idProj"]) ? int.Parse(Request.QueryString["idProj"]) : (int?)null;

            SetParentEntityLink();
        }

        private void SetParentEntityLink()
        {
            if (!_idAlert.HasValue) return;

            var reminder = _reminderOperations.GetEntity(_idAlert);
            if (reminder != null)
            {
                lblPrvParentEntity.Visible = true;
                lblPrvParentEntity.ShowLinks = true;
                lblPrvParentEntity.Label = "Alert:";

                var pnlLinks = lblPrvParentEntity.PnlLinks;

                var lbParentEntity = new LinkButton
                                            {
                                                Text = reminder.reminder_type + ": " + reminder.reminder_name,
                                                CommandArgument = reminder.navigate_url,
                                                CssClass = "alertParentLink"
                                            };

                lbParentEntity.Click += lbParentEntity_Click;

                pnlLinks.Controls.Add(lbParentEntity);
            }
        }

        private void BindEventHandlers()
        {
            if (MasterPage != null && MasterPage.ContextMenu != null)
            {
                MasterPage.ContextMenu.OnContextMenuItemClick += OnContextMenuItemClick;
            }

            EmailSearcher.OnOkButtonClick += emailSearcher_OnOkClick;

            txtTimeBeforeActivation.TxtInput.TextChanged += txtTimeBeforeActivation_OnTextChanged;
            ddlResponsibleUser.DdlInput.SelectedIndexChanged += ddlResponsibleUserDdlInput_OnSelectedIndexChanged;
            cbRemindOnEmail.CbInput.CheckedChanged += cbRemindOnEmailCbInput_OnCheckedChanged;

            lbExtEmails.OnAddClick += emailSearcherAddClick;
            lbExtEmails.OnRemoveClick += btnRemoveEmail_OnClick;
            txtReminderDate.TextChanged += txtReminderDate_TextChanged;
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
            lblPrvReminderType.Text = String.Empty;
            lblPrvRelatedAttributeName.Text = String.Empty;
            lblPrvRelatedAttributeValue.Text = String.Empty;
            txtTimeBeforeActivation.Text = String.Empty;
            txtDescription.Text = String.Empty;
            txtAdditionalEmails.Text = String.Empty;
            cbRemindOnEmail.Checked = false;
            lbExtEmails.Visible = false;
            txtAdditionalEmails.Visible = false;
            lbExtEmails.LbInput.Items.Clear();
            txtReminderDate.Text = String.Empty;
            ddlReminderRepeatMode.SelectedIndex = 0;
            ddlAlerterUserStatus.SelectedValue = String.Empty;
            txtComment.Text = String.Empty;
        }

        private void FillFormControls(object arg)
        {
            FillDdlResponsibleUser();
            FillDdlReminderRepeatingMode();
            FillDdlAlerterUserStatus();
        }

        private void FillDdlResponsibleUser()
        {
            var responsibleUserList = _personOperations.GetPersonsByRole(Constant.PersonRoleName.ResponsibleUser);
            ddlResponsibleUser.Fill(responsibleUserList, x => x.FullName, x => x.person_PK);
            ddlResponsibleUser.SortItemsByText();
        }

        private void FillDdlReminderRepeatingMode()
        {
            var reminderRepeatingModeList = _reminderRepeatingModeOperations.GetEntities();
            reminderRepeatingModeList.SortByField(x => x.name);
            ddlReminderRepeatMode.Fill(reminderRepeatingModeList, x => x.name, x => x.reminder_repeating_mode_PK, false);
        }

        private void FillDdlAlerterUserStatus()
        {
            var alerterUserStatusList = _reminderUserStatusOperations.GetEntities();
            ddlAlerterUserStatus.Fill(alerterUserStatusList, x => x.name, x => x.reminder_user_status_PK);
            ddlAlerterUserStatus.SortItemsByText();
        }

        private void SetFormControlsDefaults(object arg)
        {

        }

        #endregion

        #region Bind

        private void BindForm(object arg)
        {
            if (!_idAlert.HasValue) return;

            var reminder = _reminderOperations.GetEntity(_idAlert.Value);
            if (reminder == null || !reminder.reminder_PK.HasValue) return;

            // Reminder type
            lblPrvReminderType.Text = reminder.reminder_type + ": " + reminder.reminder_name;

            // Related attribute name
            lblPrvRelatedAttributeName.Text = reminder.related_attribute_name;

            // Related attribute value
            lblPrvRelatedAttributeValue.Text = reminder.related_attribute_value;

            // Time before activation
            txtTimeBeforeActivation.Text = reminder.time_before_activation.HasValue ? reminder.time_before_activation.Value.ToString() : null;
            txtTimeBeforeActivation.Enabled = ValidationHelper.IsValidDateTime(reminder.related_attribute_value, CultureInfoHr);

            // Reminder date
            var remiderDateList = _reminderDateOperations.GetEntitiesByReminder(reminder.reminder_PK);
            var reminderDate = remiderDateList.FirstOrDefault();
            txtReminderDate.Text = reminderDate != null && reminderDate.reminder_date.HasValue ? reminderDate.reminder_date.Value.ToString(Constant.DateTimeFormat) : null;

            // Repeating mode
            string repeatingMode = reminderDate != null ? Convert.ToString(reminderDate.reminder_repeating_mode_FK) : null;
            ddlReminderRepeatMode.SelectedValue = ddlReminderRepeatMode.Items.FindByValue(repeatingMode) != null ? repeatingMode : null;

            // Description
            txtDescription.Text = reminder.description;

            // Responsible user
            ddlResponsibleUser.SelectedId = reminder.responsible_user_FK;
            ResponsibleUserPk = reminder.responsible_user_FK;

            // Remind me on email
            cbRemindOnEmail.Checked = reminder.remind_me_on_email.HasValue && reminder.remind_me_on_email.Value;
            cbRemindOnEmailCbInput_OnCheckedChanged(null, null);

            // Email recipients
            BindEmailRecipients(reminder);

            // Aditional emails
            txtAdditionalEmails.Text = reminder.additional_emails;

            // Alerter user status
            ddlAlerterUserStatus.SelectedId = reminder.reminder_user_status_FK;

            // Comment
            txtComment.Text = reminder.comment;
        }

        private void BindEmailRecipients(Reminder_PK reminder)
        {
            var reminderEmailRecipientList = _reminderEmailRecipientOperations.GetEntitiesByReminder(reminder.reminder_PK);
            var emailRecipientList = new List<Person_PK>();
            foreach (var reminderEmailRecipient in reminderEmailRecipientList)
            {
                var person = reminderEmailRecipient.person_FK.HasValue ? _personOperations.GetEntity(reminderEmailRecipient.person_FK) : null;
                if (person != null) emailRecipientList.Add(person);
            }
            lbExtEmails.Fill(emailRecipientList, x => x.email, x => x.person_PK);
            lbExtEmails.LbInput.SortItemsByText();
        }

        #endregion

        #region Validate

        private bool ValidateForm(object arg)
        {
            var errorMessage = string.Empty;
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(txtReminderDate.Text))
            {
                errorMessage += "Alert date can't be empty.<br/>";
                AlertDateValidationError.InnerText = "Alert date can't be empty.";
            }
            else
            {
                bool alertDateBeforeToday = Convert.ToDateTime(txtReminderDate.Text).Date < DateTime.Now.Date;
                if (alertDateBeforeToday)
                {
                    errorMessage += "Alert date can't be before today.<br/>";
                    AlertDateValidationError.InnerText = "Alert date can't be before today.";
                }
            }

            if (!ddlResponsibleUser.SelectedId.HasValue)
            {
                if (ddlResponsibleUser.Required)
                {
                    errorMessage += "Responsible user can't be empty.<br/>";
                    ddlResponsibleUser.ValidationError = "Responsible user can't be empty.";
                }
            }

            if (cbRemindOnEmail.Checked)
            {
                foreach (ListItem listItem in lbExtEmails.LbInput.Items)
                {
                    listItem.Selected = false;
                    if (ValidationHelper.IsValidEmail(listItem.Text.Trim())) continue;

                    errorMessage += "\"" + listItem.Text + "\" is not valid Email. <br/>";
                    lbExtEmails.ValidationError += "\"" + listItem.Text + "\" is not valid Email. <br/>";
                    listItem.Selected = true;
                }

                var additionalEmails = txtAdditionalEmails.Text;
                var additionalEmailsArray = additionalEmails.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var additionalEmail in additionalEmailsArray)
                {
                    if (!ValidationHelper.IsValidEmail(additionalEmail.Trim()))
                    {
                        errorMessage += "\"" + additionalEmail + "\" is not valid Email. <br/>";
                        txtAdditionalEmails.ValidationError += "\"" + additionalEmail + "\" is not valid Email. <br/>";
                    }
                }

                if (lbExtEmails.LbInput.Items.Count == 0 && !additionalEmailsArray.Any())
                {
                    errorMessage += "At least one Email recipient or additional Email must be specified.<br/>";
                    lbExtEmails.ValidationError += "At least one Email recipient or additional Email must be specified.";
                    txtAdditionalEmails.ValidationError += "At least one additional Email or Email recipient must be specified.";
                }
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
            txtTimeBeforeActivation.ValidationError = String.Empty;
            txtDescription.ValidationError = String.Empty;
            ddlResponsibleUser.ValidationError = String.Empty;
            cbRemindOnEmail.ValidationError = String.Empty;
            lbExtEmails.ValidationError = string.Empty;
            txtAdditionalEmails.ValidationError = String.Empty;
            AlertDateValidationError.InnerText = String.Empty;

            // Right pane
        }

        #endregion

        #region Save

        public override object SaveForm(object arg)
        {
            base.SaveForm(arg);

            var reminder = new Reminder_PK();

            if (FormType == FormType.Edit && _idAlert.HasValue)
            {
                reminder = _reminderOperations.GetEntity(_idAlert.Value);
            }

            if (reminder == null) return null;

            // Time before activation
            reminder.time_before_activation = ValidationHelper.IsValidInt(txtTimeBeforeActivation.Text) ? (long?)Convert.ToInt64(txtTimeBeforeActivation.Text) : null;

            // Description
            reminder.description = txtDescription.Text;

            // Responsible user
            reminder.responsible_user_FK = ddlResponsibleUser.SelectedId;

            // Remind me on email
            reminder.remind_me_on_email = cbRemindOnEmail.Checked;

            // Additional emails
            var additionalEmailsArray = txtAdditionalEmails.Text.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
            reminder.additional_emails = null;

            // Alerter user status
            reminder.reminder_user_status_FK = ddlAlerterUserStatus.SelectedId;

            // Comments
            reminder.comment = txtComment.Text;

            if (cbRemindOnEmail.Checked && additionalEmailsArray.All(ValidationHelper.IsValidEmail))
            {
                reminder.additional_emails = String.Join(", ", additionalEmailsArray);
            }

            using (var ts = new TransactionScope())
            {
                var auditTrailSessionToken = StringOperations.GetRandomStringWord(32);
                Session["AUDIT_TRAIL_TOKEN"] = auditTrailSessionToken;
                reminder = _reminderOperations.Save(reminder);

                if (!reminder.reminder_PK.HasValue) return null;

                SaveEmailRecipients(reminder.reminder_PK.Value, auditTrailSessionToken);

                SaveReminderDates(reminder.reminder_PK.Value, auditTrailSessionToken);

                if (FormType == FormType.SaveAs) LastChange.HandleLastChange(pnlForm, reminder.reminder_PK, "REMINDER", _lastChangeOperations, _userOperations, true);
                else LastChange.HandleLastChange(pnlForm, reminder.reminder_PK, "REMINDER", _lastChangeOperations, _userOperations);

                ts.Complete();
            }

            return reminder;
        }

        private void SaveEmailRecipients(int reminderPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = "";
            var complexAuditOldValue = "";

            var reminderEmailRecipientList = _reminderEmailRecipientOperations.GetEntitiesByReminder(reminderPk);

            foreach (var reminderEmailRecipient in reminderEmailRecipientList)
            {
                var person = _personOperations.GetEntity(reminderEmailRecipient.person_FK);

                if (person == null) continue;

                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += ", ";
                complexAuditOldValue += person.email;
            }

            var reminderEmailRecipientPkList = reminderEmailRecipientList.Select(x => x.reminder_email_recipient_PK.HasValue ? x.reminder_email_recipient_PK.Value : 0).ToList();
            _reminderEmailRecipientOperations.DeleteCollection(reminderEmailRecipientPkList);

            if (cbRemindOnEmail.Checked)
            {
                reminderEmailRecipientList = new List<Reminder_email_recipient_PK>(lbExtEmails.LbInput.Items.Count);
                foreach (ListItem listItem in lbExtEmails.LbInput.Items)
                {
                    if (!ValidationHelper.IsValidInt(listItem.Value)) continue;

                    if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += ", ";
                    complexAuditNewValue += listItem.Text;

                    var reminderEmailRecipient = new Reminder_email_recipient_PK(null, reminderPk, int.Parse(listItem.Value));
                    reminderEmailRecipientList.Add(reminderEmailRecipient);
                }

                _reminderEmailRecipientOperations.SaveCollection(reminderEmailRecipientList);
            }

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, reminderPk.ToString(), "REMINDER_EMAIL_RECIPIENT");
        }

        private void SaveReminderDates(int reminderPk, string auditTrailSessionToken)
        {
            var complexAuditNewValue = "";
            var complexAuditOldValue = "";

            var reminderDateList = _reminderDateOperations.GetEntitiesByReminder(reminderPk);

            foreach (var reminderDate in reminderDateList)
            {
                var repeatingMode = _reminderRepeatingModeOperations.GetEntity(reminderDate.reminder_repeating_mode_FK);

                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditOldValue += ", ";

                string reminderDateString = reminderDate.reminder_date.HasValue ? reminderDate.reminder_date.Value.ToString(Constant.DateTimeFormat) : "N/A";
                string repeatingModeString = repeatingMode != null ? repeatingMode.name : "N/A";

                complexAuditOldValue += string.Format("{0} ({1})", reminderDateString, repeatingModeString);
            }

            var reminderDatePkList = reminderDateList.Select(r => r.reminder_date_PK != null ? r.reminder_date_PK.Value : 0).ToList();
            _reminderDateOperations.DeleteCollection(reminderDatePkList);

            var newReminderDate = new Reminder_date_PK
            {
                reminder_date = Convert.ToDateTime(txtReminderDate.Text).AddHours(5),
                reminder_repeating_mode_FK = int.Parse(ddlReminderRepeatMode.SelectedValue),
                reminder_status_FK = (int)ReminderStatus.Active,
                reminder_FK = reminderPk
            };

            {
                string reminderDateString = newReminderDate.reminder_date.HasValue ? newReminderDate.reminder_date.Value.ToString(Constant.DateTimeFormat) : "N/A";
                string repeatingModeString = ddlReminderRepeatMode.SelectedItem.Text;

                complexAuditNewValue += string.Format("{0} ({1})", reminderDateString, repeatingModeString);
            }

            _reminderDateOperations.Save(newReminderDate);

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, reminderPk.ToString(), "REMINDER_DATE");
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
                    Redirect();
                    break;

                case ContextMenuEventTypes.Save:
                    if (ValidateForm(null))
                    {
                        var savedAlerter = SaveForm(null);

                        Redirect();
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

        public void lbParentEntity_Click(object sender, EventArgs e)
        {
            var lb = sender as LinkButton;
        
            if (lb != null)
            {
                MasterPage.OneTimePermissionToken = Permission.View;
                Response.Redirect(lb.CommandArgument);
            }
        }

        void txtReminderDate_TextChanged(object sender, EventArgs e)
        {
            txtTimeBeforeActivation.Text = string.Empty;
        }

        public void cbRemindOnEmailCbInput_OnCheckedChanged(object sender, EventArgs e)
        {
            lbExtEmails.Visible = txtAdditionalEmails.Visible = cbRemindOnEmail.Checked;
        }

        public void txtTimeBeforeActivation_OnTextChanged(object sender, EventArgs e)
        {
            if (ValidationHelper.IsValidInt(txtTimeBeforeActivation.Text) && ValidationHelper.IsValidDateTime(lblPrvRelatedAttributeValue.Text, CultureInfoHr))
            {
                DateTime relatedDate = Convert.ToDateTime(lblPrvRelatedAttributeValue.Text);
                relatedDate = relatedDate.AddDays((-1) * int.Parse(txtTimeBeforeActivation.Text));

                txtReminderDate.Text = relatedDate.ToString(Constant.DateTimeFormat);
            }
        }

        public void ddlResponsibleUserDdlInput_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ResponsibleUserPk.HasValue)
            {
                ListItem resUserEmail = lbExtEmails.LbInput.Items.FindByValue(ResponsibleUserPk.Value.ToString());
                if (resUserEmail != null)
                {
                    lbExtEmails.LbInput.Items.Remove(resUserEmail);
                }
            }

            int? responsibleUserPk = ddlResponsibleUser.SelectedId;

            if (!responsibleUserPk.HasValue) return;

            var person = _personOperations.GetEntity(responsibleUserPk);

            if (person == null) return;

            ResponsibleUserPk = responsibleUserPk;

            if (person.email != null && ValidationHelper.IsValidEmail(person.email.Trim()))
            {
                lbExtEmails.LbInput.Items.Add(new ListItem(person.email, person.person_PK.Value.ToString()));
            }
            else
            {
                MasterPage.ModalPopup.ShowModalPopup("Error!", "Responsible user doesn't have valid email.");
            }
        }

        protected void btnRemoveEmail_OnClick(object sender, EventArgs e)
        {
            var itemsToAdd = lbExtEmails.LbInput.Items.Cast<ListItem>().Where(item => !item.Selected).ToArray();
            lbExtEmails.LbInput.Items.Clear();
            lbExtEmails.LbInput.Items.AddRange(itemsToAdd);
        }

        protected void emailSearcher_OnOkClick(object sender, FormEventArgs<List<int>> e)
        {
            var personOperations = new Person_PKDAL();
            foreach (var selectedItem in EmailSearcher.SelectedItems)
            {
                var person = personOperations.GetEntity(selectedItem);

                if (person != null && person.person_PK != null && !string.IsNullOrWhiteSpace(person.email) && ValidationHelper.IsValidEmail(person.email))
                {
                    var emailItem = new ListItem(person.email, person.person_PK.ToString());
                    if (lbExtEmails.LbInput.Items.FindByValue(person.person_PK.ToString()) == null) lbExtEmails.LbInput.Items.Add(emailItem);
                }
            }
        }

        void emailSearcherAddClick(object sender, EventArgs e)
        {
            EmailSearcher.ShowModalSearcher(SearchType.PersonEmail, SelectMode.Multiple);
        }

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
            Location_PK location = null;
            List<string> locationNamesToGenerate = null;

            location = Support.LocationManager.Instance.GetLocationByName("ReminderFormEdit", Support.CacheManager.Instance.AppLocations);
            locationNamesToGenerate = new List<string> { "AlertAuditTrailList", "ReminderFormEdit" };

            if (location != null)
            {
                tabMenu.GenerateTabMenuItems(Support.CacheManager.Instance.AppLocations, location, locationNamesToGenerate);
                tabMenu.SelectItem(location, Support.CacheManager.Instance.AppLocations);
            }
        }

        private void Redirect()
        {
            MasterPage.OneTimePermissionToken = Permission.View;
            var showAll = Request.QueryString["ShowAll"];
            string showAllQuery = string.Empty;
            if(!string.IsNullOrWhiteSpace(showAll))
            {
                if(showAll == "True") showAllQuery = "&ShowAll=True";
                else if(showAll == "False") showAllQuery = "&ShowAll=False";
            }

            var query = Request.QueryString["idLay"] != null ? string.Format("&idLay={0}", Request.QueryString["idLay"]) : null;

            if (From == "AlertSearch") Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&Action=Search", EntityContext.Alerter));
            else if (From == "AlertList" && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}{1}", EntityContext.Alerter, showAllQuery));
            else if (From == "AuthProdAlertList" && _idAuthProd.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idAuthProd={1}{2}", EntityContext.AuthorisedProduct, _idAuthProd, showAllQuery));
            else if (From == "ProdAlertList" && _idProd.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idProd={1}{2}", EntityContext.Product, _idAuthProd, showAllQuery));
            else if (From == "ProjAlertList" && _idProj.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idProj={1}{2}", EntityContext.Project, _idAuthProd, showAllQuery));
            else if (From == "TaskAlertList" && _idTask.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idTask={1}{2}", EntityContext.Task, _idTask, showAllQuery));
            else if (From == "ActAlertList" && _idAct.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idAct={1}{2}", EntityContext.Activity, _idAct, showAllQuery));
            else if (From == "ActMyAlertList" && _idAct.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idAct={1}{2}", EntityContext.ActivityMy, _idAct, showAllQuery));
            else if (From == "DocAlertList" && _idDoc.HasValue) Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}&idDoc={1}{2}", EntityContext.Document, _idDoc, showAllQuery));

            Response.Redirect(string.Format("~/Views/AlerterView/List.aspx?EntityContext={0}{1}{2}", EntityContext.Alerter, showAllQuery, query));
        }

        #endregion

        #region Security

        public override bool SecurityPageSpecific()
        {
            if (IsPostBack) return true;

            base.SecurityPageSpecific();

            Location_PK parentLocation = null;
            var isPermittedInsertAlerter = false;
            if (EntityContext == EntityContext.Alerter) parentLocation = Support.LocationManager.Instance.GetLocationByName("Alerter", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.AuthorisedProduct)parentLocation = Support.LocationManager.Instance.GetLocationByName("AuthProdAlertList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Product) parentLocation = Support.LocationManager.Instance.GetLocationByName("ProdAlertList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Project) parentLocation = Support.LocationManager.Instance.GetLocationByName("ProjAlertList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Task) parentLocation = Support.LocationManager.Instance.GetLocationByName("TaskAlertList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Activity) parentLocation = Support.LocationManager.Instance.GetLocationByName("ActAlertList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.ActivityMy) parentLocation = Support.LocationManager.Instance.GetLocationByName("ActMyAlertList", Support.CacheManager.Instance.AppLocations);
            else if (EntityContext == EntityContext.Document) parentLocation = Support.LocationManager.Instance.GetLocationByName("DocAlertList", Support.CacheManager.Instance.AppLocations);

            if (FormType == FormType.New) isPermittedInsertAlerter = SecurityHelper.IsPermitted(Permission.InsertAlerter, parentLocation);
            else if (FormType == FormType.Edit) isPermittedInsertAlerter = SecurityHelper.IsPermittedAny(new List<Permission> { Permission.EditMy, Permission.EditAlerter }, parentLocation);

            if (isPermittedInsertAlerter)
            {
                SecurityHelper.SetControlsForReadWrite(MasterPage.ContextMenu, new[] { new ContextMenuItem(ContextMenuEventTypes.Save, "Save") }, new List<Panel> { PnlForm }, new Dictionary<Panel, List<string>> { { PnlFooter, new List<string> { "Save" } } });
            }

            return true;
        }

        #endregion
    }
}