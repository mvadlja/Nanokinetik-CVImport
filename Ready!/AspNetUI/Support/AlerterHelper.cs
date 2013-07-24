using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNetUI.Views;
using AspNetUI.Views.Shared.Interface;
using AspNetUI.Views.Shared.UserControl;
using Ready.Model;

namespace AspNetUI.Support
{
    public static class AlerterHelper
    {
        public static void SaveEmailRecipients(List<Reminder_email_recipient_PK> reminderEmailRecipientPkList, Reminder_PK reminder, string auditTrailSessionToken)
        {
            IPerson_PKOperations personOperations = new Person_PKDAL();
            IReminder_email_recipient_PKOperations reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            var complexAuditNewValue = "";
            const string complexAuditOldValue = "";

            foreach (Reminder_email_recipient_PK reminderEmailRecipient in reminderEmailRecipientPkList)
            {
                var person = personOperations.GetEntity(reminderEmailRecipient.person_FK);

                if (person == null) continue;

                if (!string.IsNullOrEmpty(complexAuditNewValue)) complexAuditNewValue += ", ";
                complexAuditNewValue += person.email;
            }

            reminderEmailRecipientOperations.SaveCollection(reminderEmailRecipientPkList);

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, reminder.reminder_PK.ToString(), "REMINDER_EMAIL_RECIPIENT");
        }

        public static void SaveReminderDates(Reminder_PK reminder, string auditTrailSessionToken)
        {
            var complexAuditNewValue = "";
            const string complexAuditOldValue = "";

            IReminder_repeating_mode_PKOperations reminderRepeatingModeOperations = new Reminder_repeating_mode_PKDAL();
            IReminder_date_PKOperations reminderDateOperations = new Reminder_date_PKDAL();

            foreach (var reminderDate in reminder.ReminderDates)
            {
                reminderDate.reminder_FK = reminder.reminder_PK;
                var repeatingMode = reminderRepeatingModeOperations.GetEntity(reminderDate.reminder_repeating_mode_FK);

                if (!string.IsNullOrEmpty(complexAuditOldValue)) complexAuditNewValue += ", ";

                string reminderDateString = reminderDate.reminder_date.HasValue ? reminderDate.reminder_date.Value.ToString(Constant.DateTimeFormat) : "N/A";
                string repeatingModeString = repeatingMode != null ? repeatingMode.name : "N/A";

                complexAuditNewValue += string.Format("{0} ({1})", reminderDateString, repeatingModeString);
            }

            reminderDateOperations.SaveCollection(reminder.ReminderDates);

            AuditTrailHelper.SaveAuditDetail(complexAuditNewValue, complexAuditOldValue, auditTrailSessionToken, reminder.reminder_PK.ToString(), "REMINDER_DATE");
        }

        public static void RefreshReminderStatus(IReminder_PKOperations reminderOperations, Views.Shared.Template.Default masterPage, List<IReminderCustomControl> reminderControls, string tableName, int? entityFk)
        {
            var existsClass = string.Empty;
            var notExistsClass = string.Empty;

            foreach (var reminderControl in reminderControls)
            {
                if (reminderControl is LabelPreview)
                {
                    existsClass = "lblPrv-reminder-icon-exists";
                    notExistsClass = "lblPrv-reminder-icon-not-exists";
                }
                else if (reminderControl is DateTimeBox)
                {
                    existsClass = "dt-reminder-icon-exists";
                    notExistsClass = "dt-reminder-icon-not-exists";
                }
                reminderControl.LnkSetReminder.RemoveCssClasses(new List<string> { existsClass, notExistsClass });

                string relatedAttributeName = StringOperations.GetRelatedName(reminderControl.Label);
                reminderControl.LnkSetReminder.CssClass = reminderOperations.DoesActiveReminderExists(null, tableName, entityFk, relatedAttributeName) ? existsClass : notExistsClass;
            }

            masterPage.SetReminderTextForUser();
        }
    }
}