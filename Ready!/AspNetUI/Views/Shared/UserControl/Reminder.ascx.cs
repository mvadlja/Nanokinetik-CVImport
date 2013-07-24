using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.Views.Shared.UserControl
{
    public partial class Reminder : System.Web.UI.UserControl
    {
        #region Declarations

        public event EventHandler<EventArgs> OnConfirmInputButtonProcess_Click;

        IPerson_PKOperations _personOperations;
        private IReminder_repeating_mode_PKOperations _reminderRepeatingModeOperations;
        private IReminder_user_status_PKOperations _reminderUserStatusOperations;
        private CultureInfo CultureInfoHr;

        private Template.Default _masterPage;

        #endregion

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return reminderContainer.Style["Width"]; }
            set { reminderContainer.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return reminderContainer.Style["Height"]; }
            set { reminderContainer.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return reminderBody.Style["padding"]; }
            set { reminderBody.Style["padding"] = value; }
        }

        public Reminder_PK ReminderVs
        {
            get { return ViewState["Reminder"] != null ? (Reminder_PK)ViewState["Reminder"] : new Reminder_PK(); }
            set { ViewState["Reminder"] = value; }
        }

        public List<int> ReminderEmailRecipients
        {
            get { return ViewState["ReminderEmailrecipients"] != null ? (List<int>)ViewState["ReminderEmailrecipients"] : new List<int>(); }
            set { ViewState["ReminderEmailrecipients"] = value; }
        }

        public System.Web.UI.HtmlControls.HtmlGenericControl DivHeader
        {
            get { return divHeader; }
        }

        #endregion

        #region Page methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            reminderContainer.Style["display"] = "none";

            _masterPage = (Template.Default)Page.Master;

            _personOperations = new Person_PKDAL();
            _reminderRepeatingModeOperations = new Reminder_repeating_mode_PKDAL();
            _reminderUserStatusOperations = new Reminder_user_status_PKDAL();
            CultureInfoHr = new CultureInfo("hr-HR");

            EmailSearcher.OnOkButtonClick += emailSearcherOkClick;

            //dtReminderDate.TxtInput.TextChanged += dtReminderDateTxtInput_TextChanged;
            txtTimeBeforeActivation.TxtInput.TextChanged += txtTimeBeforeActivation_TextChanged;
            ddlResponsibleUser.DdlInput.SelectedIndexChanged += ddlResponsibleUserDdlInput_SelectedIndexChanged;
            cbRemindOnEmail.CbInput.CheckedChanged += cbRemindOnEmailCbInput_CheckedChanged;
            txtReminderDate.TextChanged += txtReminderDate_TextChanged;

            lbExtEmails.OnAddClick += emailSearcherAddClick;
            lbExtEmails.OnRemoveClick += btnRemoveEmailOnClick;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (IsPostBack) return;

            InitForm(null);
            BindForm(null);
            SetFormControlsDefaults(null);
        }

        #endregion

        #region Form methods

        void InitForm(object arg)
        {
            ClearForm(null);
            FillFormControls(null);
        }

        void ClearForm(object arg)
        {
            lblPrvReminderType.Text = String.Empty;
            txtTimeBeforeActivation.Text = String.Empty;
            lblPrvRelatedAttributeName.Text = String.Empty;
            lblPrvRelatedAttributeValue.Text = String.Empty;
            txtTimeBeforeActivation.Text = String.Empty;
            txtDescription.Text = String.Empty;
            txtAdditionalEmails.Text = String.Empty;
            cbRemindOnEmail.Checked = false;
            cbRemindOnEmailCbInput_CheckedChanged(null, null);
            lbExtEmails.LbInput.Items.Clear();
            ReminderEmailRecipients.Clear();
            txtReminderDate.Text = string.Empty;
            ddlReminderRepeatMode.SelectedIndex = 0;
            ddlAlerterUserStatus.SelectedValue = String.Empty;
            txtComment.Text = String.Empty;
        }

        void FillFormControls(object arg)
        {
            BindResponsibleUser();
            BindReminderRepeatingMode();
            FillDdlAlerterUserStatus();
        }

        private void FillDdlAlerterUserStatus()
        {
            var alerterUserStatusList = _reminderUserStatusOperations.GetEntities();
            ddlAlerterUserStatus.Fill(alerterUserStatusList, x => x.name, x => x.reminder_user_status_PK);
            ddlAlerterUserStatus.SortItemsByText();
        }

        void SetFormControlsDefaults(object arg)
        {
            var pendingListItem = ddlAlerterUserStatus.DdlInput.Items.FindByText("Pending");
            if (pendingListItem != null)
            {
                ddlAlerterUserStatus.SelectedValue = pendingListItem.Value;
            }
        }

        void BindForm(object arg)
        {
            lblPrvReminderType.Text = ReminderVs.reminder_type + ": " + ReminderVs.reminder_name;
            lblPrvRelatedAttributeName.Text = ReminderVs.related_attribute_name;
            lblPrvRelatedAttributeValue.Text = ReminderVs.related_attribute_value;

            var cultureInfo = new System.Globalization.CultureInfo("hr-HR");

            if (!ValidationHelper.IsValidDateTime(lblPrvRelatedAttributeValue.Text, cultureInfo))
            {
                txtTimeBeforeActivation.TxtInput.Enabled = false;
                //ctlTimeBeforeActivation.FontBold = false;
                //ctlTimeBeforeActivation.FontItalic = false;
            }
            else
            {
                txtTimeBeforeActivation.TxtInput.Enabled = true;

                //ctlTimeBeforeActivation.FontBold = false;
                //ctlTimeBeforeActivation.FontItalic = false;
            }

            Person_PK person = _personOperations.GetPersonByUserID(SessionManager.Instance.CurrentUser.UserID);

            if (person != null)
            {
                if (person.person_PK != null)
                {
                    ddlResponsibleUser.SelectedId = person.person_PK;
                    ReminderVs.responsible_user_FK = person.person_PK;

                    if (person.email != null && ValidationHelper.IsValidEmail(person.email.Trim()))
                    {
                        lbExtEmails.LbInput.Items.Add(new ListItem(person.email, person.person_PK.Value.ToString()));
                    }
                    else
                    {
                        if (IsPostBack) _masterPage.ModalPopup.ShowModalPopup("Error!", "Responsible user doesn't have valid email.");
                    }
                }
            }
        }

        bool ValidateForm(object arg)
        {
            var errorMessage = String.Empty;
            ClearValidationErrors();

            if (string.IsNullOrWhiteSpace(txtReminderDate.Text))
            {
                errorMessage += "Alert date can't be empty.<br/>";
                AlertDateValidationError.InnerText = "Alert date(s) can't be empty.";
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

            if (string.IsNullOrWhiteSpace(ddlResponsibleUser.SelectedValue.ToString()))
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
                    listItem.Selected = true;
                }

                var additionalEmails = txtAdditionalEmails.Text;
                var additionalEmailsArray = additionalEmails.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var additionalEmail in additionalEmailsArray)
                {
                    if (!ValidationHelper.IsValidEmail(additionalEmail.Trim()))
                    {
                        errorMessage += "\"" + additionalEmail + "\" is not valid Email. <br/>";
                    }
                }

                if (lbExtEmails.LbInput.Items.Count == 0 && !additionalEmailsArray.Any())
                    errorMessage += "At least one Email must be specified.<br/>";
            }

            if (!String.IsNullOrEmpty(errorMessage))
            {
                _masterPage.ModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }

            return true;
        }

        public object SaveForm(object id, string arg)
        {
            var reminder = ReminderVs;

            const int reminderStatus = (int)ReminderStatus.Active;
            Int32 repeatingMode = ddlReminderRepeatMode.SelectedIndex + 1;

            var tmpReminderDate = new Reminder_date_PK
            {
                reminder_date = Convert.ToDateTime(txtReminderDate.Text).AddHours(5),
                reminder_repeating_mode_FK = repeatingMode,
                reminder_status_FK = reminderStatus
            };
            reminder.ReminderDates.Add(tmpReminderDate);

            reminder.time_before_activation = ValidationHelper.IsValidInt(txtTimeBeforeActivation.Text) ? (long?)Convert.ToInt64(txtTimeBeforeActivation.Text) : null;
            reminder.responsible_user_FK = ddlResponsibleUser.SelectedId;

            reminder.description = txtDescription.Text;
            reminder.remind_me_on_email = cbRemindOnEmail.Checked;
            reminder.is_automatic = false;

            reminder.reminder_user_status_FK = ddlAlerterUserStatus.SelectedId;
            reminder.comment = txtComment.Text;

            string relatedEntityFk = Request.QueryString["idProd"] ?? Request.QueryString["idAuthProd"] ?? Request.QueryString["idProj"] ?? Request.QueryString["projid"] ?? Request.QueryString["idTask"] ?? Request.QueryString["idAct"] ?? Request.QueryString["id"];
            if (ValidationHelper.IsValidInt(relatedEntityFk)) reminder.related_entity_FK = Convert.ToInt32(relatedEntityFk);

            ReminderVs = reminder;

            var additionalEmails = txtAdditionalEmails.Text;
            var additionalEmailsArray = additionalEmails.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            reminder.additional_emails = string.Empty;
            if (cbRemindOnEmail.Checked)
            {
                foreach (string additionalEmail in additionalEmailsArray)
                {
                    if (ValidationHelper.IsValidEmail(additionalEmail.Trim()))
                    {
                        reminder.additional_emails += reminder.additional_emails == string.Empty ? additionalEmail.Trim() : ", " + additionalEmail.Trim();
                    }
                }
            }

            var reminderEmailrecipients = ReminderEmailRecipients;
            reminderEmailrecipients.Clear();

            if (cbRemindOnEmail.Checked)
            {
                reminderEmailrecipients.AddRange(from ListItem listItem in lbExtEmails.LbInput.Items where ValidationHelper.IsValidInt(listItem.Value) select int.Parse(listItem.Value));
            }

            ReminderEmailRecipients = reminderEmailrecipients;

            return reminder;
        }

        #endregion

        #region Binding methods

        void BindResponsibleUser()
        {
            List<Person_PK> items = CBLoader.LoadResponsibleUsers();

            ddlResponsibleUser.Fill(items, "name", "person_PK");
        }

        void BindReminderRepeatingMode()
        {
            List<Reminder_repeating_mode_PK> items = _reminderRepeatingModeOperations.GetEntities();
            ddlReminderRepeatMode.DataTextField = "name";
            ddlReminderRepeatMode.DataValueField = "reminder_repeating_mode_PK";
            ddlReminderRepeatMode.DataSource = items;
            ddlReminderRepeatMode.DataBind();
        }

        #endregion

        #region Event handlers

        void txtReminderDate_TextChanged(object sender, EventArgs e)
        {
            txtTimeBeforeActivation.Text = string.Empty;
        }

        public void cbRemindOnEmailCbInput_CheckedChanged(object sender, EventArgs e)
        {
            lbExtEmails.Visible = txtAdditionalEmails.Visible = cbRemindOnEmail.Checked;
        }

        public void txtTimeBeforeActivation_TextChanged(object sender, EventArgs e)
        {
            if (ValidationHelper.IsValidInt(txtTimeBeforeActivation.Text) && ValidationHelper.IsValidDateTime(lblPrvRelatedAttributeValue.Text, CultureInfoHr))
            {
                DateTime relatedDate = Convert.ToDateTime(lblPrvRelatedAttributeValue.Text);
                relatedDate = relatedDate.AddDays((-1) * int.Parse(txtTimeBeforeActivation.Text));

                txtReminderDate.Text = relatedDate.ToString(Constant.DateTimeFormat);
            }
        }

        public void ddlResponsibleUserDdlInput_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ReminderVs.responsible_user_FK.HasValue)
            {
                ListItem resUserEmail = lbExtEmails.LbInput.Items.FindByValue(ReminderVs.responsible_user_FK.Value.ToString());
                if (resUserEmail != null)
                {
                    lbExtEmails.LbInput.Items.Remove(resUserEmail);
                }
            }

            int? responsibleUserPk = ddlResponsibleUser.SelectedId;

            if (responsibleUserPk.HasValue)
            {
                var person = _personOperations.GetEntity(responsibleUserPk);

                if (person != null)
                {
                    ReminderVs.responsible_user_FK = responsibleUserPk;

                    if (person.email != null && ValidationHelper.IsValidEmail(person.email.Trim()))
                    {
                        if (person.person_PK != null)
                            lbExtEmails.LbInput.Items.Add(new ListItem(person.email, person.person_PK.Value.ToString()));
                    }
                    else
                    {
                        _masterPage.ModalPopup.ShowModalPopup("Error!", "Responsible user doesn't have valid email.", ModalPopupMode.Ok);
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm(null))
            {
                SaveForm(null, null);

                if (OnConfirmInputButtonProcess_Click != null)
                {
                    OnConfirmInputButtonProcess_Click(sender, e);
                }

                reminderContainer.Style["display"] = "none";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            reminderContainer.Style["display"] = "none";
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            reminderContainer.Style["display"] = "none";
        }

        protected void btnRemoveEmailOnClick(object sender, EventArgs e)
        {
            var itemsToAdd = lbExtEmails.LbInput.Items.Cast<ListItem>().Where(item => !item.Selected).ToArray();
            lbExtEmails.LbInput.Items.Clear();
            lbExtEmails.LbInput.Items.AddRange(itemsToAdd);
        }

        void emailSearcherOkClick(object sender, FormEventArgs<List<int>> e)
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

        public void ShowModalPopup(string header, string message)
        {
            divHeader.InnerText = header;

            reminderContainer.Style["display"] = "inline";
        }

        public void ShowModalPopup(string header)
        {
            divHeader.InnerText = header;

            ClearForm(null);
            BindForm(null);
            SetFormControlsDefaults(null);

            reminderContainer.Style["display"] = "inline";
        }

        private void ClearValidationErrors()
        {
            txtTimeBeforeActivation.ValidationError = String.Empty;
            dtReminderTime.ValidationError = String.Empty;
            txtDescription.ValidationError = String.Empty;
            ddlResponsibleUser.ValidationError = String.Empty;
            cbRemindOnEmail.ValidationError = String.Empty;
            txtAdditionalEmails.ValidationError = String.Empty;
            AlertDateValidationError.InnerText = String.Empty;
        }

        #endregion

        #region Security

        #endregion
    }
}