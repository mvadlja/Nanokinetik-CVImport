using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using AspNetUI.Support;
using AspNetUI.Views;
using AspNetUIFramework;
using Ready.Model;

namespace AspNetUI.ucControls.PopupControls
{
    public partial class ucReminder : DetailsForm
    {
        public event EventHandler<EventArgs> OnConfirmInputButtonProcess_Click;

        IPerson_PKOperations _person_PKOperations;
        private IReminder_repeating_mode_PKOperations _reminderRepeatingModeOperations;
        private CultureInfo CultureInfoHr;

        #region Properties

        public string ModalPopupContainerWidth
        {
            get { return messageModalPopupContainer.Style["Width"]; }
            set { messageModalPopupContainer.Style["Width"] = value; }
        }

        public string ModalPopupContainerHeight
        {
            get { return messageModalPopupContainer.Style["Height"]; }
            set { messageModalPopupContainer.Style["Height"] = value; }
        }

        public string ModalPopupContainerBodyPadding
        {
            get { return modalPopupContainerBody.Style["padding"]; }
            set { modalPopupContainerBody.Style["padding"] = value; }
        }

        #endregion

        public Reminder_PK Reminder
        {
            get { return ViewState["Reminder"] != null ? (Reminder_PK)ViewState["Reminder"] : new Reminder_PK(); }
            set { ViewState["Reminder"] = value; }
        }

        public List<int> ReminderEmailRecipients
        {
            get { return ViewState["ReminderEmailrecipients"] != null ? (List<int>)ViewState["ReminderEmailrecipients"] : new List<int>(); }
            set { ViewState["ReminderEmailrecipients"] = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            messageModalPopupContainer.Style["display"] = "none";

            base.OnInit(e);

            _person_PKOperations = new Person_PKDAL();
            _reminderRepeatingModeOperations = new Reminder_repeating_mode_PKDAL();
            CultureInfoHr = new CultureInfo("hr-HR");

            EmailSearcher.OnOkButtonClick += new EventHandler<FormListEventArgs>(emailSearcherOkClick);
            EmailSearcher.OnListItemSelected += new EventHandler<FormListEventArgs>(emailSearcherListItemSelected);
            EmailSearcherDisplay.OnSearchClick += new EventHandler<EventArgs>(emailSearcherAddClick);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void ShowModalPopup(string header)
        {
            divHeader.InnerHtml = header;

            ClearForm(null);
            BindForm(null, null);

            messageModalPopupContainer.Style["display"] = "inline";
        }


        public override object SaveForm(object id, string arg)
        {
            Reminder_PK reminder = Reminder;

            //var reminderDates = lbReminderDates.Items;
            const int reminderStatus = (int)ReminderStatus.Active;
            Int32 repeatingMode = ddlReminderRepeatMode.SelectedIndex + 1;

            //foreach (ListItem reminderDate in reminderDates)
            //{
            //    var tmpReminderDate = new Reminder_date_PK
            //    {
            //        reminder_date = Convert.ToDateTime(reminderDate.Text).AddHours(5),
            //        reminder_repeating_mode_FK = repeatingMode,
            //        reminder_status_FK = reminderStatus
            //    };
            //    reminder.ReminderDates.Add(tmpReminderDate);
            //}

            var tmpReminderDate = new Reminder_date_PK
            {
                reminder_date = Convert.ToDateTime(txtReminderDate.Text).AddHours(5),
                reminder_repeating_mode_FK = repeatingMode,
                reminder_status_FK = reminderStatus
            };
            reminder.ReminderDates.Add(tmpReminderDate);

            reminder.time_before_activation = ValidationHelper.IsValidInt(ctlTimeBeforeActivation.ControlValue.ToString()) ? Convert.ToInt64(ctlTimeBeforeActivation.ControlValue.ToString()) : 0;
            reminder.responsible_user_FK = ValidationHelper.IsValidInt(ctlResponsibleUser.ControlValue.ToString()) ? (int?)Convert.ToInt32(ctlResponsibleUser.ControlValue.ToString()) : null;

            reminder.description = ctlDescription.ControlValue.ToString();
            reminder.remind_me_on_email = cbRemindOnEmail.Checked;
            reminder.is_automatic = false;

            string relatedEntityFk = Request.QueryString["idProd"] ?? Request.QueryString["idAuthProd"] ?? Request.QueryString["idProj"] ?? Request.QueryString["projid"] ?? Request.QueryString["idTask"] ?? Request.QueryString["idAct"] ?? Request.QueryString["id"];
            if (ValidationHelper.IsValidInt(relatedEntityFk)) reminder.related_entity_FK = Convert.ToInt32(relatedEntityFk);

            Reminder = reminder;

            string additionalEmails = ctlAdditionalEmails.ControlValue.ToString();
            string[] additionalEmailsArray = additionalEmails.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            reminder.additional_emails = string.Empty;
            if (cbRemindOnEmail.Checked)
            {
                foreach (string additionalEmail in additionalEmailsArray)
                {
                    if (ValidationHelper.IsValidEmail(additionalEmail.Trim()))
                    {
                        reminder.additional_emails += reminder.additional_emails == string.Empty
                                                          ? additionalEmail.Trim()
                                                          : ", " + additionalEmail.Trim();
                    }
                }
            }

            List<int> reminderEmailrecipients = ReminderEmailRecipients;
            reminderEmailrecipients.Clear();

            if (cbRemindOnEmail.Checked)
            {
                foreach (ListItem listItem in ctlEmailRecipients.ControlBoundItems)
                {
                    if (ValidationHelper.IsValidInt(listItem.Value))
                    {
                        reminderEmailrecipients.Add(int.Parse(listItem.Value));
                    }
                }
            }

            ReminderEmailRecipients = reminderEmailrecipients;

            return reminder;
        }

        public override void ClearForm(string arg)
        {
            ctlReminderType.ControlValue = String.Empty;
            ctlRelatedAttributeName.ControlValue = String.Empty;
            ctlRelatedAttributeValue.ControlValue = String.Empty;
            ctlDescription.ControlValue = String.Empty;
            ctlAdditionalEmails.ControlValue = String.Empty;
           
            ctlTimeBeforeActivation.ControlValue = String.Empty;

            cbRemindOnEmail.Checked = false;
            cbRemindOnEmail_CheckedChanged(null, null);

            ctlEmailRecipients.ControlBoundItems.Clear();
            ReminderEmailRecipients.Clear();
            //lbReminderDates.Items.Clear();
            txtReminderDate.Text = string.Empty;
            ddlReminderRepeatMode.SelectedIndex = 0;
        }

        public override void FillDataDefinitions(string arg)
        {
            BindResponsibleUser();
            BindReminderRepeatingMode();
        }

        void BindResponsibleUser()
        {
            List<Person_PK> items = CBLoader.LoadResponsibleUsers();

            ctlResponsibleUser.SourceValueProperty = "person_PK";
            ctlResponsibleUser.SourceTextExpression = "name";
            ctlResponsibleUser.FillControl<Person_PK>(items);
        }

        void BindReminderRepeatingMode()
        {
            List<Reminder_repeating_mode_PK> items = _reminderRepeatingModeOperations.GetEntities();
            ddlReminderRepeatMode.DataTextField = "name";
            ddlReminderRepeatMode.DataValueField = "reminder_repeating_mode_PK";
            ddlReminderRepeatMode.DataSource = items;
            ddlReminderRepeatMode.DataBind();
        }

        public override bool ValidateForm(string arg)
        {
            string errorMessage = String.Empty;

            var cultureInfo = new CultureInfo("hr-HR");

            //if (lbReminderDates.Items.Count == 0)
            //{
            //    errorMessage += "Alert date can't be empty.<br/>";
            //}
            //else
            //{
            //    var reminderDates = lbReminderDates.Items;
            //    bool alertDateBeforeToday = reminderDates.Cast<ListItem>().Any(reminderDate => Convert.ToDateTime(reminderDate.Text).Date < DateTime.Now.Date);
            //    if (alertDateBeforeToday)
            //    {
            //        errorMessage += "Alert date can't be before today.<br/>";
            //    }
            //}

            if (string.IsNullOrWhiteSpace(txtReminderDate.Text))
            {
                errorMessage += "Alert date can't be empty.<br/>";
            }
            else
            {
                bool alertDateBeforeToday = Convert.ToDateTime(txtReminderDate.Text).Date < DateTime.Now.Date;
                if (alertDateBeforeToday)
                {
                    errorMessage += "Alert date can't be before today.<br/>";
                }
            }

            if (string.IsNullOrWhiteSpace(ctlResponsibleUser.ControlValue.ToString()))
            {
                if (ctlResponsibleUser.IsMandatory)
                    errorMessage += ctlResponsibleUser.ControlEmptyErrorMessage + "<br/>";
            }
            else if (!ValidationHelper.IsValidInt(ctlResponsibleUser.ControlValue.ToString())) errorMessage += ctlResponsibleUser.ControlErrorMessage + "<br/>";

            if (cbRemindOnEmail.Checked)
            {
                bool statusOK = true;

                foreach (ListItem listItem in ctlEmailRecipients.ControlBoundItems)
                {
                    listItem.Selected = false;
                    if (!ValidationHelper.IsValidEmail(listItem.Text.Trim()))
                    {
                        errorMessage += "\"" + listItem.Text + "\" is not valid Email. <br/>";
                        listItem.Selected = true;
                        ctlEmailRecipients.CurrentControlState = ControlState.IAmInvalid;
                        ctlEmailRecipients.ControlErrorMessage = "Some Emails are not valid.";
                        statusOK = false;
                    }
                }

                if (statusOK) ctlEmailRecipients.CurrentControlState = ControlState.ReadyForAction;

                statusOK = true;
                string additionalEmails = ctlAdditionalEmails.ControlValue.ToString();
                string[] additionalEmailsArray = additionalEmails.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string additionalEmail in additionalEmailsArray)
                {
                    if (!ValidationHelper.IsValidEmail(additionalEmail.Trim()))
                    {
                        errorMessage += "\"" + additionalEmail + "\" is not valid Email. <br/>";
                        ctlAdditionalEmails.CurrentControlState = ControlState.IAmInvalid;
                        ctlAdditionalEmails.ControlErrorMessage = "Some Emails are not valid.";
                        statusOK = false;
                    }
                }
                if (statusOK) ctlAdditionalEmails.CurrentControlState = ControlState.ReadyForAction;

                if (ctlEmailRecipients.ControlBoundItems.Count == 0 && !additionalEmailsArray.Any())
                    errorMessage += "At least one Email must be specified.<br/>";
            }

            if (!String.IsNullOrEmpty(errorMessage))
            {
                FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", errorMessage);
                return false;
            }
            else return true;
        }

        public override void BindForm(object id, string arg)
        {
            ctlReminderType.ControlValue = Reminder.reminder_type + ": " + Reminder.reminder_name;
            ctlRelatedAttributeName.ControlValue = Reminder.related_attribute_name;
            ctlRelatedAttributeValue.ControlValue = Reminder.related_attribute_value;

            var cultureInfo = new System.Globalization.CultureInfo("hr-HR");

            if (!ValidationHelper.IsValidDateTime(ctlRelatedAttributeValue.ControlValue.ToString(), cultureInfo))
            {
                ctlTimeBeforeActivation.CurrentControlState = ControlState.YouCantChangeMe;
                ctlTimeBeforeActivation.FontBold = false;
                ctlTimeBeforeActivation.FontItalic = false;
            }
            else
            {
                ctlTimeBeforeActivation.CurrentControlState = ControlState.ReadyForAction;
                ctlTimeBeforeActivation.FontBold = false;
                ctlTimeBeforeActivation.FontItalic = false;
            }

            Person_PK person = _person_PKOperations.GetPersonByUserID(SessionManager.Instance.CurrentUser.UserID);

            if (person != null)
            {
                ctlResponsibleUser.ControlValue = person.person_PK;
                Reminder.responsible_user_FK = person.person_PK;

                if (person.email != null && ValidationHelper.IsValidEmail(person.email.Trim()))
                {
                    ctlEmailRecipients.ControlBoundItems.Add(new ListItem(person.email, person.person_PK.Value.ToString()));
                }
                else
                {
                    FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", "Responsible user doesn't have valid email.");
                }
            }
        }

        public void cbRemindOnEmail_CheckedChanged(object sender, EventArgs e)
        {
            divEmails.Visible = cbRemindOnEmail.Checked;
        }

        public void TimeBeforeActivationChanged(object sender, ValueChangedEventArgs e)
        {
            if (ValidationHelper.IsValidInt(ctlTimeBeforeActivation.ControlValue.ToString()) &&
                ValidationHelper.IsValidDateTime(ctlRelatedAttributeValue.ControlValue.ToString(), CultureInfoHr))
            {
                DateTime relatedDate = Convert.ToDateTime(ctlRelatedAttributeValue.ControlValue.ToString());
                relatedDate = relatedDate.AddDays((-1) * int.Parse(ctlTimeBeforeActivation.ControlValue.ToString()));

                txtReminderDate.Text = relatedDate.ToString(Constant.DateTimeFormat);

                //AddReminderDate(relatedDate.ToString(Constant.DateTimeFormat));
            }
        }

        //protected void lnkAddReminderDate_OnClick(object sender, EventArgs e)
        //{
        //    var reminderDate = txtReminderDate.Text;

        //    AddReminderDate(reminderDate);
        //}

        private void AddReminderDate(string reminderDate)
        {
            //if (!string.IsNullOrWhiteSpace(reminderDate) && ValidationHelper.IsValidDateTime(reminderDate, CultureInfoHr) && lbReminderDates.Items.FindByText(reminderDate) == null)
            //{
            //    lbReminderDates.Items.Add(new ListItem(reminderDate));
            //    ctlTimeBeforeActivation.ControlValue = string.Empty;
            //    txtReminderDate.Text = string.Empty;
            //}
        }

        //protected void lnkRemoveReminderDate_OnClick(object sender, EventArgs e)
        //{
        //    lbReminderDates.RemoveSelected();
        //    txtReminderDate.Text = string.Empty;
        //}

        //protected void lbReminderDates_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var selectedItems = lbReminderDates.GetSelectedItems();
        //    if (selectedItems != null && selectedItems.Count() == 1) txtReminderDate.Text = selectedItems.FirstOrDefault().Text;
        //}

        public void ResponsibleUserChanged(object sender, ValueChangedEventArgs e)
        {
            if (Reminder.responsible_user_FK.HasValue)
            {
                ListItem resUserEmail = ctlEmailRecipients.ControlBoundItems.FindByValue(Reminder.responsible_user_FK.Value.ToString());
                if (resUserEmail != null)
                {
                    ctlEmailRecipients.ControlBoundItems.Remove(resUserEmail);
                }
            }

            int? responsibleUserPK = ValidationHelper.IsValidInt(ctlResponsibleUser.ControlValue.ToString()) ? (int?)int.Parse(ctlResponsibleUser.ControlValue.ToString()) : null;

            if (responsibleUserPK.HasValue)
            {
                Person_PK person = _person_PKOperations.GetEntity(responsibleUserPK);

                if (person != null)
                {
                    Reminder.responsible_user_FK = responsibleUserPK;

                    if (person.email != null && ValidationHelper.IsValidEmail(person.email.Trim()))
                    {
                        ctlEmailRecipients.ControlBoundItems.Add(new ListItem(person.email, person.person_PK.Value.ToString()));
                    }
                    else
                    {
                        FormHolder.MasterPage.MessageModalPopup.ShowModalPopup("Error!", "Responsible user doesn't have valid email.");
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

                messageModalPopupContainer.Style["display"] = "none";
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            messageModalPopupContainer.Style["display"] = "none";
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            messageModalPopupContainer.Style["display"] = "none";
        }


        public void ctlEmailrecipientsInputValueChanged(object sender, ValueChangedEventArgs e)
        {

        }

        protected void btnRemoveEmailOnClick(object sender, EventArgs e)
        {
            List<ListItem> itemsToRemove = new List<ListItem>();
            foreach (ListItem item in ctlEmailRecipients.ControlBoundItems)
            {
                if (item.Selected)
                {
                    itemsToRemove.Add(item);
                }
            }

            foreach (ListItem item in itemsToRemove)
            {
                ctlEmailRecipients.ControlBoundItems.Remove(item);
            }
        }

        void emailSearcherListItemSelected(object sender, FormListEventArgs e)
        {
            IPerson_PKOperations _person_PKOperations = new Person_PKDAL();
            Person_PK person = _person_PKOperations.GetEntity(e.DataItemID);


            if (person != null)
            {
                ListItem item = new ListItem(person.email, person.person_PK.ToString());
                if (!ctlEmailRecipients.ControlBoundItems.Contains(item))
                    ctlEmailRecipients.ControlBoundItems.Add(item);
            }
        }

        void emailSearcherOkClick(object sender, FormListEventArgs e)
        {
            IPerson_PKOperations _person_PKOperations = new Person_PKDAL();
            foreach (int selectedItem in EmailSearcher.SelectedItems)
            {
                Person_PK person = _person_PKOperations.GetEntity(selectedItem);

                if (person != null)
                {
                    ListItem item = new ListItem(person.email, person.person_PK.ToString());
                    if (!ctlEmailRecipients.ControlBoundItems.Contains(item))
                        ctlEmailRecipients.ControlBoundItems.Add(item);
                }
            }
        }

        void emailSearcherAddClick(object sender, EventArgs e)
        {
            EmailSearcher.ShowModalSearcher("PersonEmail", ucControls.PopupControls.SelectMode.Multiple);
        }

        void ReminderDateChanged(object sender, ValueChangedEventArgs e)
        {
            ctlTimeBeforeActivation.ControlValue = null;
        }

        #region IModalPopup Members


        public void ShowModalPopup(string header, string message)
        {
            divHeader.InnerHtml = header;

            messageModalPopupContainer.Style["display"] = "inline";
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