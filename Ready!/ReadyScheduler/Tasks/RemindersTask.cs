using System;
using System.Collections.Generic;
using System.Net.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using Ready.Model;
using System.Configuration;
using System.Net.Mail;
using System.IO;
using System.Data;
using System.Linq;

namespace ReadyScheduler.Tasks
{
    public static class RemindersTask
    {
        public static void CheckReminders()
        {
            LogEvent("\r\nChecking reminders started " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            try
            {
                IReminder_PKOperations _reminderOperations = new Reminder_PKDAL();
                IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
                IPerson_PKOperations _personOperations = new Person_PKDAL();
                
                List<Reminder_PK> reminders = _reminderOperations.GetEntitiesReadyForEmail();

                foreach (Reminder_PK reminder in reminders)
                {
                    try
                    {
                        List<string> emailRecipients = new List<string>();
                        List<Reminder_email_recipient_PK> emailRecipientList = _reminderEmailRecipientOperations.GetEntitiesByReminder(reminder.reminder_PK);

                        foreach (Reminder_email_recipient_PK emailRecipient in emailRecipientList)
                        {
                            Person_PK person = _personOperations.GetEntity(emailRecipient.person_FK);

                            if (person != null && IsValidEmail(person.email.Trim()))
                            {
                                emailRecipients.Add(person.email.Trim());
                            }
                        }

                        string[] additionalEmailsArray = reminder.additional_emails != null ? reminder.additional_emails.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };

                        foreach (string additionalEmail in additionalEmailsArray)
                        {
                            if (IsValidEmail(additionalEmail.Trim()))
                            {
                                emailRecipients.Add(additionalEmail.Trim());
                            }
                        }

                        if (emailRecipients.Count > 0)
                        {
                            string subject = CreateReminderEmailSubject(reminder);
                            string body = CreateReminderEmailBody(reminder);
                            
                            SendEmail(subject, body, emailRecipients);

                            HandleRepeatingDate(reminder);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEvent("ERROR! Sending email failed! ReminderID = " + reminder.reminder_PK + "\r\n\t->Exception:\r\n\t->Message: " + ex.Message + "\r\n\t->StackTrace: " + ex.StackTrace + "\r\n\t->InnerException: " + ex.InnerException);
                    }
                }

                LogEvent("Checking reminders finished " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception e)
            {
                LogEvent("ERROR! Checking reminders failed! \r\n\t->Exception:\r\n\t->Message: " + e.Message + "\r\n\t->StackTrace: " + e.StackTrace + "\r\n\t->InnerException: " + e.InnerException);
            }
        }

        private static void HandleRepeatingDate(Reminder_PK reminder)
        {
            var reminderStatus = (int)ReminderStatus.EmailSent;
            var newReminderDate = new DateTime();
            if (reminder.repeating_mode == ReminderRepeatingMode.None.ToString())
            {
                reminderStatus = (int)ReminderStatus.EmailSent;
            }
            else if (reminder.repeating_mode == ReminderRepeatingMode.Daily.ToString())
            {
                reminderStatus = (int)ReminderStatus.Active;
                newReminderDate = reminder.reminder_date.Value.AddDays(1);
            }
            else if (reminder.repeating_mode == ReminderRepeatingMode.Weekly.ToString())
            {
                reminderStatus = (int)ReminderStatus.Active;
                newReminderDate = reminder.reminder_date.Value.AddDays(7);
            }
            else if (reminder.repeating_mode == ReminderRepeatingMode.Monthly.ToString())
            {
                reminderStatus = (int)ReminderStatus.Active;
                newReminderDate = reminder.reminder_date.Value.AddMonths(1);
            }
            else if (reminder.repeating_mode == ReminderRepeatingMode.Yearly.ToString())
            {
                reminderStatus = (int)ReminderStatus.Active;
                newReminderDate = reminder.reminder_date.Value.AddYears(1);
            }

            if(reminder.repeating_mode != ReminderRepeatingMode.None.ToString())
            {
                IReminder_date_PKOperations _reminderDateOperations = new Reminder_date_PKDAL();
                var reminderDate = _reminderDateOperations.GetEntity(reminder.reminder_date_PK);
                reminderDate.reminder_date = newReminderDate;
                _reminderDateOperations.Save(reminderDate);
            }

            IReminder_PKOperations _reminderOperations = new Reminder_PKDAL();
            if (reminderStatus != (int)ReminderStatus.Active)
            {
                _reminderOperations.SetReminderStatus(reminder.reminder_date_PK.Value, reminderStatus);
            }
        }

        public static void CreateAutomaticReminders()
        {
            LogEvent("\r\nCreating automatic reminders started " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

            try
            {
                IReminder_PKOperations reminderOperations = new Reminder_PKDAL();
                IReminder_email_recipient_PKOperations reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
                IReminder_date_PKOperations reminderDateOperations = new Reminder_date_PKDAL();
               
                IActivity_PKOperations activityOperations = new Activity_PKDAL();
                IProject_PKOperations projectOperations = new Project_PKDAL();
                ITask_PKOperations taskOperations = new Task_PKDAL();
                ITask_name_PKOperations taskNameOperations = new Task_name_PKDAL();
                IType_PKOperations typeOperations = new Type_PKDAL();

                #region Projects

                List<Project_PK> projects = projectOperations.GetEntities();

                foreach (Project_PK project in projects)
                {
                    try
                    {
                        bool isInternalStatusFinished = false;
                        bool isStartDateToday = false;
                        bool isExpectedFinishedDateToday = false;

                        Type_PK internalStatus = project.internal_status_type_FK != null ? typeOperations.GetEntity(project.internal_status_type_FK) : null;
                        if (internalStatus != null && internalStatus.name.Trim().ToLower() == "finished")
                        {
                            isInternalStatusFinished = true;
                        }

                        if (project.start_date.HasValue && project.start_date.Value.Date == DateTime.Now.Date)
                        {
                            isStartDateToday = true;
                        }
                        if (project.expected_finished_date.HasValue && project.expected_finished_date.Value.Date == DateTime.Now.Date)
                        {
                            isExpectedFinishedDateToday = true;
                        }

                        if (project.automatic_alerts_on && !project.prevent_start_date_alert && isStartDateToday)
                        {
                            var reminder = new Reminder_PK();

                            reminder.related_attribute_name = "Start date";
                            reminder.related_attribute_value = project.start_date.Value.ToString("dd.MM.yyyy");
                            
                            reminder.time_before_activation = 0;

                            reminder.reminder_type = "Project";
                            reminder.reminder_name = project.name;
                            reminder.TableName = ReminderTableName.PROJECT;
                            reminder.entity_FK = project.project_PK;
                            reminder.responsible_user_FK = project.user_FK;
                            reminder.description = "Automatic alert";
                            reminder.navigate_url = string.Format("~/Views/ProjectView/Preview.aspx?EntityContext=Project&idProj={0}", project.project_PK);
                            reminder.remind_me_on_email = project.user_FK.HasValue;
                            reminder.is_automatic = true;

                            bool reminderExists = reminderOperations.DoesAutomaticReminderAlreadyExists(reminder.table_name, reminder.entity_FK, reminder.related_attribute_name, DateTime.Now.Date.AddHours(5));

                            if (!reminderExists)
                            {
                                reminder = reminderOperations.Save(reminder);
                                
                                var reminderDate = new Reminder_date_PK
                                {
                                    reminder_date = DateTime.Now.Date.AddHours(5),
                                    reminder_repeating_mode_FK = (int)ReminderRepeatingMode.None,
                                    reminder_FK = reminder.reminder_PK,
                                    reminder_status_FK = (int)ReminderStatus.Active
                                };

                                reminderDateOperations.Save(reminderDate);

                                if (project.user_FK.HasValue)
                                {
                                    reminderEmailRecipientOperations.Save(new Reminder_email_recipient_PK(null, reminder.reminder_PK, project.user_FK));
                                }
                            }
                        }

                        if (project.automatic_alerts_on && !project.prevent_exp_finish_date_alert && !isInternalStatusFinished && isExpectedFinishedDateToday)
                        {
                            var reminder = new Reminder_PK();

                            reminder.related_attribute_name = "Expected finished date";
                            reminder.related_attribute_value = project.expected_finished_date.Value.ToString("dd.MM.yyyy");

                            reminder.time_before_activation = 0;

                            reminder.reminder_type = "Project";
                            reminder.reminder_name = project.name;
                            reminder.TableName = ReminderTableName.PROJECT;
                            reminder.entity_FK = project.project_PK;
                            reminder.responsible_user_FK = project.user_FK;
                            reminder.description = "Automatic alert";
                            reminder.navigate_url = string.Format("~/Views/ProjectView/Preview.aspx?EntityContext=Project&idProj={0}", project.project_PK);
                            reminder.remind_me_on_email = project.user_FK.HasValue;
                            reminder.is_automatic = true;

                            bool reminderExists = reminderOperations.DoesAutomaticReminderAlreadyExists(reminder.table_name, reminder.entity_FK, reminder.related_attribute_name, DateTime.Now.Date.AddHours(5));

                            if (!reminderExists)
                            {
                                reminder = reminderOperations.Save(reminder);

                                var reminderDate = new Reminder_date_PK
                                {
                                    reminder_date = DateTime.Now.Date.AddHours(5),
                                    reminder_repeating_mode_FK = (int)ReminderRepeatingMode.None,
                                    reminder_FK = reminder.reminder_PK,
                                    reminder_status_FK = (int)ReminderStatus.Active
                                };

                                reminderDateOperations.Save(reminderDate);

                                if (project.user_FK.HasValue)
                                {
                                    reminderEmailRecipientOperations.Save(new Reminder_email_recipient_PK(null, reminder.reminder_PK, project.user_FK));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEvent("ERROR! Creating automatic alert failed! ProjectID = " + project.project_PK + "\r\n\t->Exception:\r\n\t->Message: " + ex.Message + "\r\n\t->StackTrace: " + ex.StackTrace + "\r\n\t->InnerException: " + ex.InnerException);
                    }
                }

                #endregion


                #region Activities

                List<Activity_PK> activities = activityOperations.GetEntities();

                foreach (Activity_PK activity in activities)
                {
                    try
                    {
                        bool isInternalStatusFinished = false;
                        bool isStartDateToday = false;
                        bool isExpectedFinishedDateToday = false;

                        Type_PK internalStatus = activity.internal_status_FK != null ? typeOperations.GetEntity(activity.internal_status_FK) : null;
                        if (internalStatus != null && internalStatus.name.Trim().ToLower() == "finished")
                        {
                            isInternalStatusFinished = true;
                        }

                        if (activity.start_date.HasValue && activity.start_date.Value.Date == DateTime.Now.Date)
                        {
                            isStartDateToday = true;
                        }
                        if (activity.expected_finished_date.HasValue && activity.expected_finished_date.Value.Date == DateTime.Now.Date)
                        {
                            isExpectedFinishedDateToday = true;
                        }

                        if (activity.automatic_alerts_on && !activity.prevent_start_date_alert && isStartDateToday)
                        {
                            var reminder = new Reminder_PK();

                            reminder.related_attribute_name = "Start date";
                            reminder.related_attribute_value = activity.start_date.Value.ToString("dd.MM.yyyy");

                            reminder.time_before_activation = 0;

                            reminder.reminder_type = "Activity";
                            reminder.reminder_name = activity.name;
                            reminder.TableName = ReminderTableName.ACTIVITY;
                            reminder.entity_FK = activity.activity_PK;
                            reminder.responsible_user_FK = activity.user_FK;
                            reminder.description = "Automatic alert";
                            reminder.navigate_url = string.Format("~/Views/ActivityView/Preview.aspx?EntityContext=Activity&idAct={0}", activity.activity_PK);
                            reminder.remind_me_on_email = activity.user_FK.HasValue;
                            reminder.is_automatic = true;

                            bool reminderExists = reminderOperations.DoesAutomaticReminderAlreadyExists(reminder.table_name, reminder.entity_FK, reminder.related_attribute_name, DateTime.Now.Date.AddHours(5));

                            if (!reminderExists)
                            {
                                reminder = reminderOperations.Save(reminder);

                                var reminderDate = new Reminder_date_PK
                                {
                                    reminder_date = DateTime.Now.Date.AddHours(5),
                                    reminder_repeating_mode_FK = (int)ReminderRepeatingMode.None,
                                    reminder_FK = reminder.reminder_PK,
                                    reminder_status_FK = (int)ReminderStatus.Active
                                };

                                reminderDateOperations.Save(reminderDate);

                                if (activity.user_FK.HasValue)
                                {
                                    reminderEmailRecipientOperations.Save(new Reminder_email_recipient_PK(null, reminder.reminder_PK, activity.user_FK));
                                }
                            }
                        }

                        if (activity.automatic_alerts_on && !activity.prevent_exp_finish_date_alert && !isInternalStatusFinished && isExpectedFinishedDateToday)
                        {
                            var reminder = new Reminder_PK();

                            reminder.related_attribute_name = "Expected finished date";
                            reminder.related_attribute_value = activity.expected_finished_date.Value.ToString("dd.MM.yyyy");

                            reminder.time_before_activation = 0;

                            reminder.reminder_type = "Activity";
                            reminder.reminder_name = activity.name;
                            reminder.TableName = ReminderTableName.ACTIVITY;
                            reminder.entity_FK = activity.activity_PK;
                            reminder.responsible_user_FK = activity.user_FK;
                            reminder.description = "Automatic alert";
                            reminder.navigate_url = string.Format("~/Views/ActivityView/Preview.aspx?EntityContext=Activity&idAct={0}", activity.activity_PK);
                            reminder.remind_me_on_email = activity.user_FK.HasValue;
                            reminder.is_automatic = true;

                            bool reminderExists = reminderOperations.DoesAutomaticReminderAlreadyExists(reminder.table_name, reminder.entity_FK, reminder.related_attribute_name, DateTime.Now.Date.AddHours(5));

                            if (!reminderExists)
                            {
                                reminder = reminderOperations.Save(reminder);

                                var reminderDate = new Reminder_date_PK
                                {
                                    reminder_date = DateTime.Now.Date.AddHours(5),
                                    reminder_repeating_mode_FK = (int)ReminderRepeatingMode.None,
                                    reminder_FK = reminder.reminder_PK,
                                    reminder_status_FK = (int)ReminderStatus.Active
                                };

                                reminderDateOperations.Save(reminderDate);

                                if (activity.user_FK.HasValue)
                                {
                                    reminderEmailRecipientOperations.Save(new Reminder_email_recipient_PK(null, reminder.reminder_PK, activity.user_FK));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEvent("ERROR! Creating automatic alert failed! ActivityID = " + activity.activity_PK + "\r\n\t->Exception:\r\n\t->Message: " + ex.Message + "\r\n\t->StackTrace: " + ex.StackTrace + "\r\n\t->InnerException: " + ex.InnerException);
                    }

                }

                #endregion

                #region Tasks

                List<Task_PK> tasks = taskOperations.GetEntities();

                foreach (Task_PK task in tasks)
                {
                    try
                    {
                        bool isInternalStatusFinished = false;
                        bool isStartDateToday = false;
                        bool isExpectedFinishedDateToday = false;

                        Type_PK internalStatus = task.type_internal_status_FK != null ? typeOperations.GetEntity(task.type_internal_status_FK) : null;
                        if (internalStatus != null && internalStatus.name.Trim().ToLower() == "finished")
                        {
                            isInternalStatusFinished = true;
                        }

                        if (task.start_date.HasValue && task.start_date.Value.Date == DateTime.Now.Date)
                        {
                            isStartDateToday = true;
                        }
                        if (task.expected_finished_date.HasValue && task.expected_finished_date.Value.Date == DateTime.Now.Date)
                        {
                            isExpectedFinishedDateToday = true;
                        }

                        if (task.automatic_alerts_on && !task.prevent_start_date_alert && isStartDateToday)
                        {
                            var reminder = new Reminder_PK();

                            reminder.related_attribute_name = "Start date";
                            reminder.related_attribute_value = task.start_date.Value.ToString("dd.MM.yyyy");

                            reminder.time_before_activation = 0;

                            reminder.reminder_type = "Task";

                            Task_name_PK taskName = taskNameOperations.GetEntity(task.task_name_FK);

                            reminder.reminder_name = taskName != null ? taskName.task_name : string.Empty;
                            reminder.TableName = ReminderTableName.TASK;
                            reminder.entity_FK = task.task_PK;
                            reminder.responsible_user_FK = task.user_FK;
                            reminder.description = "Automatic alert";
                            reminder.navigate_url = string.Format("~/Views/TaskView/Preview.aspx?EntityContext=Task&idTask={0}", task.task_PK);
                            reminder.remind_me_on_email = task.user_FK.HasValue ? true : false;
                            reminder.is_automatic = true;

                            bool reminderExists = reminderOperations.DoesAutomaticReminderAlreadyExists(reminder.table_name, reminder.entity_FK, reminder.related_attribute_name, DateTime.Now.Date.AddHours(5));

                            if (!reminderExists)
                            {
                                reminder = reminderOperations.Save(reminder);

                                var reminderDate = new Reminder_date_PK
                                {
                                    reminder_date = DateTime.Now.Date.AddHours(5),
                                    reminder_repeating_mode_FK = (int)ReminderRepeatingMode.None,
                                    reminder_FK = reminder.reminder_PK,
                                    reminder_status_FK = (int)ReminderStatus.Active
                                };

                                reminderDateOperations.Save(reminderDate);

                                if (task.user_FK.HasValue)
                                {
                                    reminderEmailRecipientOperations.Save(new Reminder_email_recipient_PK(null, reminder.reminder_PK, task.user_FK));
                                }
                            }
                        }

                        if (task.automatic_alerts_on && !task.prevent_exp_finish_date_alert && !isInternalStatusFinished && isExpectedFinishedDateToday)
                        {
                            var reminder = new Reminder_PK();

                            reminder.related_attribute_name = "Expected finished date";
                            reminder.related_attribute_value = task.expected_finished_date.Value.ToString("dd.MM.yyyy");

                            reminder.time_before_activation = 0;

                            reminder.reminder_type = "Task";

                            Task_name_PK taskName = taskNameOperations.GetEntity(task.task_name_FK);

                            reminder.reminder_name = taskName != null ? taskName.task_name : string.Empty;
                            reminder.TableName = ReminderTableName.TASK;
                            reminder.entity_FK = task.task_PK;
                            reminder.responsible_user_FK = task.user_FK;
                            reminder.description = "Automatic alert";
                            reminder.navigate_url = string.Format("~/Views/TaskView/Preview.aspx?EntityContext=Task&idTask={0}", task.task_PK);
                            reminder.remind_me_on_email = task.user_FK.HasValue ? true : false;
                            reminder.is_automatic = true;

                            bool reminderExists = reminderOperations.DoesAutomaticReminderAlreadyExists(reminder.table_name, reminder.entity_FK, reminder.related_attribute_name, DateTime.Now.Date.AddHours(5));

                            if (!reminderExists)
                            {
                                reminder = reminderOperations.Save(reminder);

                                var reminderDate = new Reminder_date_PK
                                {
                                    reminder_date = DateTime.Now.Date.AddHours(5),
                                    reminder_repeating_mode_FK = (int)ReminderRepeatingMode.None,
                                    reminder_FK = reminder.reminder_PK,
                                    reminder_status_FK = (int)ReminderStatus.Active
                                };

                                reminderDateOperations.Save(reminderDate);
                                if (task.user_FK.HasValue)
                                {
                                    reminderEmailRecipientOperations.Save(new Reminder_email_recipient_PK(null, reminder.reminder_PK, task.user_FK));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEvent("ERROR! Creating automatic alert failed! TaskID = " + task.task_PK + "\r\n\t->Exception:\r\n\t->Message: " + ex.Message + "\r\n\t->StackTrace: " + ex.StackTrace + "\r\n\t->InnerException: " + ex.InnerException);
                    }
                }

                #endregion

                LogEvent("Creating automatic reminders finished " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
            catch (Exception e)
            {
                LogEvent("ERROR! Creating automatic alert failed! \r\n\t->Exception:\r\n\t->Message: " + e.Message + "\r\n\t->StackTrace: " + e.StackTrace + "\r\n\t->InnerException: " + e.InnerException);
            }
        }

        public static void DeleteOldDismissedAutomaticReminders()
        {
            IReminder_PKOperations _reminderOperations = new Reminder_PKDAL();

            try
            {
                _reminderOperations.DeleteOldDismissedAutomaticReminders(DateTime.Now.Date);
            }
            catch (Exception ex)
            {
                LogEvent("ERROR! Deleting old dissmised automatic alerts failed!\r\n\t->Exception:\r\n\t->Message: " + ex.Message + "\r\n\t->StackTrace: " + ex.StackTrace + "\r\n\t->InnerException: " + ex.InnerException);
            }

        }

        private static string CreateReminderEmailBody(Reminder_PK reminder)
        {
            IReminder_email_recipient_PKOperations _reminderEmailRecipientOperations = new Reminder_email_recipient_PKDAL();
            IReminder_date_PKOperations _reminderDates = new Reminder_date_PKDAL();
            IPerson_PKOperations _personOperations = new Person_PKDAL();
            StringBuilder sb = new StringBuilder();

            sb.Append("<html><body style='font-family: Arial; font-size: 12px;'>");
            sb.Append("Dear READY! User," + "<br/><br/>");

            sb.Append("<div style='margin-bottom: 10px'>");
            if (reminder.is_automatic == true)
            {
                sb.Append("You have received an automatic alert with the following information:");
            }
            else
            {
                sb.Append("You have received an alert with the following information:");
            }
            sb.Append("</div>");

            sb.Append("<table cellpadding='0' cellspacing='0' style='table-layout: fixed'>");
            string navigateUrl = ConfigurationManager.AppSettings["AppURL"] + (reminder.navigate_url.StartsWith("~") ? reminder.navigate_url.Substring(1) : reminder.navigate_url);

            sb.Append("<tr><td style='width: 160px; font-family: Arial; font-size: 12px; word-wrap: break-word;'>" + reminder.reminder_type + ":</td><td style='vertical-align: top; word-wrap: break-word;'><b><a style='font-family: Arial; font-size: 12px;' href=\"" + navigateUrl + "\">" + reminder.reminder_name + "</a></b></td></tr>");

            sb.Append(GetRelatedEntities(reminder.reminder_type, reminder.entity_FK));

            var reminderDates = _reminderDates.GetEntitiesByReminder(reminder.reminder_PK); 

            bool overDue = reminderDates.Any(r => r.reminder_date.HasValue && r.reminder_date < DateTime.Now);
            String overDueText = overDue ? "<span style='color: red; font-family: Arial; font-size: 12px;'>Yes</span>" : "No";
            
            sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Over due:</td><td style='vertical-align: top'><b style='font-family: Arial; font-size: 12px;'>" + overDueText + "</b></td></tr>");
            sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Related date name:</td><td style='vertical-align: top'><b style='font-family: Arial; font-size: 12px;'>" + reminder.related_attribute_name + "</b></td></tr>");
            sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Related date:</td><td style='vertical-align: top'><b style='font-family: Arial; font-size: 12px;'>" + reminder.related_attribute_value + "</b></td></tr>");
            sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Alert date:</td><td style='vertical-align: top'><b style='font-family: Arial; font-size: 12px;'>" + (reminder.reminder_date.HasValue ? reminder.reminder_date.Value.ToString("dd.MM.yyyy") : "") + "</b></td></tr>");
            sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Alert description:</td><td style='vertical-align: top'><b style='font-family: Arial; font-size: 12px;'>" + reminder.description + "</b></td></tr>");
            
            sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Alert sent to:</td>");

            List<string> emailRecipients = new List<string>();
            List<Reminder_email_recipient_PK> emailRecipientList = _reminderEmailRecipientOperations.GetEntitiesByReminder(reminder.reminder_PK);
            foreach (Reminder_email_recipient_PK emailRecipient in emailRecipientList)
            {
                Person_PK person = _personOperations.GetEntity(emailRecipient.person_FK);
                if (person != null && IsValidEmail(person.email.Trim()))
                {
                    emailRecipients.Add(person.email.Trim());
                }
            }

            string[] additionalEmailsArray = reminder.additional_emails != null ? reminder.additional_emails.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries) : new string[] { };
            foreach (string additionalEmail in additionalEmailsArray)
            {
                if (IsValidEmail(additionalEmail.Trim()))
                {
                    emailRecipients.Add(additionalEmail.Trim());
                }
            }

            sb.Append("<td style='vertical-align: top; word-wrap: break-word;'>");
            foreach (String email in emailRecipients)
            {
                sb.Append("<div style='margin-bottom: 4px;'><b style='font-family: Arial; font-size: 12px;'>" + email + "</b></div>");
            }

            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("<div style='margin-top: 10px'>READY! from Nanokinetik</div>");
            sb.Append("</body></html>");

            return sb.ToString();
        }

        private static string CreateReminderEmailSubject(Reminder_PK reminder)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("READY! Alert: ");
            sb.Append(reminder.related_attribute_value);
            sb.Append(": ");
            sb.Append(reminder.related_attribute_name);
            sb.Append(", ");
            sb.Append(reminder.reminder_name);

            return sb.ToString();
        }

        private static bool IsValidEmail(string s)
        {
            const string reEmail = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";

            if (string.IsNullOrWhiteSpace(s))
                return false;
            Regex r = new Regex(reEmail);
            return r.IsMatch(s);
        }

        private static void SendEmail(string subject, string body, IEnumerable<string> recipients)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var mailSettings = config.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
          
            var mail = new MailMessage
            {
                Subject = subject,
                Body = body,
                From = new MailAddress(mailSettings.Smtp.From),
                IsBodyHtml = true
            };

            foreach (string recipient in recipients)
            {
                mail.To.Add(new MailAddress(recipient));
            }

            //System.Net.Mail.Attachment attachment;
            //attachment = new System.Net.Mail.Attachment("c:/textfile.txt");
            //mail.Attachments.Add(attachment);

            //EmbedCompanyLogo(mail);

            var smtpServer = new SmtpClient()
            {
                Port = mailSettings.Smtp.Network.Port,
                Credentials = new System.Net.NetworkCredential(mailSettings.Smtp.Network.UserName, mailSettings.Smtp.Network.Password),
                EnableSsl = mailSettings.Smtp.Network.EnableSsl,
                Host = mailSettings.Smtp.Network.Host,
            };

            smtpServer.Send(mail);
        }

        private static void EmbedCompanyLogo(MailMessage message)
        {
            AlternateView av1 = AlternateView.CreateAlternateViewFromString(message.Body, null, System.Net.Mime.MediaTypeNames.Text.Html);
            string filepath = ConfigurationManager.AppSettings["ReminderEmailCompanyLogoFile"];
            LinkedResource logo = new LinkedResource(filepath, System.Net.Mime.MediaTypeNames.Image.Jpeg);
            logo.ContentId = "companylogo";
            //To refer to this image in the html body, use <img src="cid:companylogo"/>  
            av1.LinkedResources.Add(logo);
            message.AlternateViews.Add(av1);
        }

        private static void LogEvent(String text)
        {
            StreamWriter fileWriter = null;

            string logFile = ConfigurationManager.AppSettings["ReminderLogFile"];

            if (!File.Exists(logFile))
            {
                if (string.IsNullOrWhiteSpace(logFile))
                    logFile = @"c:\reminders.log";
            }

            if (File.Exists(logFile))
                fileWriter = File.AppendText(logFile);
            else
                fileWriter = File.CreateText(logFile);

            fileWriter.WriteLine(text);

            fileWriter.Flush();
            fileWriter.Close();
        }

        private static string GetRelatedEntities(string reminder_type, int? entity_FK)
        {
            string related_entities = "";

            switch (reminder_type)
            {
                case "Activity" :
                    related_entities = GetRelatedEntitiesForActivity(entity_FK);
                    break;

                case "Authorised product" :
                    related_entities = GetRelatedEntitiesForAuthorisedProduct(entity_FK);
                    break;

                case "Document" :
                    related_entities = GetRelatedEntitiesForDocument(entity_FK);
                    break;

                case "Task" :
                    related_entities = GetRelatedEntitiesForTask(entity_FK);
                    break;

            }

            return related_entities;
        }

        private static string GetRelatedEntitiesForActivity(int? entity_FK)
        {
            IActivity_product_PKOperations _activityProductMNOperations = new Activity_product_PKDAL();
            IActivity_country_PKOperations _activityCountryMNOperations = new Activity_country_PKDAL();
            IActivity_project_PKOperations _activityProjectOperations = new Activity_project_PKDAL();
            IProject_PKOperations _projectOperations = new Project_PKDAL();

            string appURL = ConfigurationManager.AppSettings["AppURL"];
           
            //Products related to the activity
            DataSet products = _activityProductMNOperations.GetProductsByActivity(Convert.ToInt32(entity_FK));
            
            StringBuilder sb = new StringBuilder();
            sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Products:</td><td style='vertical-align: top; word-wrap: break-word;'>");
            int numRows = 0;       
            DataTable dt = products.Tables.Count > 0 ? products.Tables[0] : null;
            if (dt != null)
            {
                numRows = dt.Rows.Count;
                foreach (DataRow dr in dt.Rows)
                {
                    sb.Append("<div style='font-family: Arial; font-size: 12px; margin-bottom: 4px;'>");
                    if (string.IsNullOrWhiteSpace(dr["name"].ToString()))
                    {
                        sb.Append("-");
                    }
                    else
                    {                        
                        string productUrl = string.Format("{0}/Views/ProductView/Preview.aspx?EntityContext=Product&idProd={1}", appURL, dr["product_PK"].ToString());
                        sb.Append("<a style='font-family: Arial; font-weight: bold; font-size: 12px;' href='" + productUrl + "'>" + dr["name"].ToString() + "</a>");
                    }
                    sb.Append("</div>");
                }
            }
            else
            {
                sb.Append("-");
            }
            sb.Append("</td></tr>");

            //Projects related to the activity
            List<Activity_project_PK> activityProject = _activityProjectOperations.GetEntities().FindAll(item => item.activity_FK == Convert.ToInt32(entity_FK));

            sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Projects:</td><td style='vertical-align: top; word-wrap: break-word;'>");
            if (activityProject != null)
            {
                numRows = activityProject.Count;
                foreach (Activity_project_PK item in activityProject)
                {
                    Project_PK project = _projectOperations.GetEntity(item.project_FK);
                    
                    string projectUrl = string.Format("{0}/Views/ProjectView/Preview.aspx?EntityContext=Project&idProj={1}", appURL, project.project_PK.ToString());

                    sb.Append("<div style='margin-bottom: 4px;'><a style='font-family: Arial; font-weight: bold; font-size: 12px;' href='" + projectUrl + "'>" + project.name + "</a></div>");
                }
                if (numRows == 0) sb.Append("-");
            }
            else
            {
                sb.Append("-");
            }
            sb.Append("</td></tr>");
            
            //Countries related to the activity
            sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Countries:</td><td style='vertical-align: top; word-wrap: break-word;'>");

            DataSet activityCountries = _activityCountryMNOperations.GetCountriesByActivity(Convert.ToInt32(entity_FK));
           
            DataTable dtActivityCountries = activityCountries.Tables.Count > 0 ? activityCountries.Tables[0] : null;
            numRows = 0;
            var delimiter = "";
            if (dtActivityCountries != null)
            {
                dtActivityCountries.DefaultView.Sort = "abbreviation ASC";
                dtActivityCountries = dtActivityCountries.DefaultView.ToTable();

                numRows = dtActivityCountries.Rows.Count;
                foreach (DataRow dr in dtActivityCountries.Rows)
                {
                    if (string.IsNullOrWhiteSpace(dr["abbreviation"].ToString()))
                    {
                        sb.Append("");
                    }
                    else
                    {
                        sb.Append(delimiter + "<b style='font-family: Arial; font-size: 12px;'>" + dr["abbreviation"].ToString() + "</b>");
                        delimiter = ",";
                    }                    
                }
            }
            else
            {
                sb.Append("-");
            }
            sb.Append("</td></tr>");

            return sb.ToString();
        }

        private static string GetRelatedEntitiesForAuthorisedProduct(int? entity_FK)
        {
            IAuthorisedProductOperations _authorizedProductOperations = new AuthorisedProductDAL();
            IProduct_PKOperations _productOperations = new Product_PKDAL();

            string appURL = ConfigurationManager.AppSettings["AppURL"];

            StringBuilder sb = new StringBuilder();

            if (entity_FK != null)
            {
                AuthorisedProduct ap = _authorizedProductOperations.GetEntity((int)entity_FK);
                Product_PK product = ap != null && ap.product_FK != null ? _productOperations.GetEntity(ap.product_FK) : null;
            
                sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Product:</td><td style='vertical-align: top; word-wrap: break-word;'>");
                if (product != null)
                {
                    string productUrl = string.Format("{0}/Views/ProductView/Preview.aspx?EntityContext=Product&idProd={1}", appURL, product.product_PK);

                    string relatedProduct = "";
                    if (product.name != null && product.name.Trim() != "")
                    {
                        if (!string.IsNullOrEmpty(product.product_number)) 
                            relatedProduct = product.name + " (" + product.product_number + ")";

                        else relatedProduct = product.name;
                    }

                    sb.Append("<div style='margin-bottom: 4px;'><a style='font-family: Arial; font-weight: bold; font-size: 12px;' href='" + productUrl + "'>" + relatedProduct + "</a></div>");
                }
                else
                {
                    sb.Append("-");
                }
                sb.Append("</td>");

                return sb.ToString();
            }

            sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Product:</td><td style='vertical-align: top; word-wrap: break-word;'>");
            sb.Append("-");
            sb.Append("</td>");
            return sb.ToString();
        }

        private static string GetRelatedEntitiesForDocument(int? entity_FK)
        {
            IDocument_PKOperations _document_PKOperations = new Document_PKDAL();
            Document_PK document = _document_PKOperations.GetEntity(entity_FK);

            string appURL = ConfigurationManager.AppSettings["AppURL"];

            StringBuilder sb = new StringBuilder();

            //Check if related to Authorized product
            IAp_document_mn_PKOperations _apDocumentMnOperations = new Ap_document_mn_PKDAL();
            IAuthorisedProductOperations _authProductOperations = new AuthorisedProductDAL();

            var apDocListMN = _apDocumentMnOperations.GetAuthorizedProductsByDocumentFK(entity_FK);
            if (apDocListMN != null && apDocListMN.Count != 0)
            {
                sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Authorised products:</td><td style='vertical-align: top; word-wrap: break-word;'>");

                foreach (var item in apDocListMN)
                {
                    AuthorisedProduct ap = _authProductOperations.GetEntity(item.ap_FK);
                    if (ap != null)
                    {
                        string authorisedProductUrl = string.Format("{0}/Views/AuthorisedProductView/Preview.aspx?EntityContext=AuthorisedProduct&idAuthProd={1}", appURL, ap.ap_PK);
                        string authorisedProductName = ap.product_name;

                        sb.Append("<div style='margin-bottom: 4px;'><a style='font-family: Arial; font-weight: bold; font-size: 12px;' href='" + authorisedProductUrl + "'>" + authorisedProductName + "</a></div>");
                    }
                }
                sb.Append("</td></tr>");
            }

            //Check if related to Product
            IProduct_document_mn_PKOperations _productDocumentMNOperations = new Product_document_mn_PKDAL();
            IProduct_PKOperations _productOperations = new Product_PKDAL();

            var productDocListMN = _productDocumentMNOperations.GetProductsByDocumentFK(entity_FK);
            if (productDocListMN != null && productDocListMN.Count != 0)
            {
                sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Products:</td><td style='vertical-align: top; word-wrap: break-word;'>");

                foreach (var item in productDocListMN)
                {
                    Product_PK product = _productOperations.GetEntity(item.product_FK);
                    if (product != null)
                    {
                        string productUrl = string.Format("{0}/Views/ProductView/Preview.aspx?EntityContext=Product&idProd={1}", appURL, product.product_PK);
                        string productName = product.name;

                        sb.Append("<div style='margin-bottom: 4px;'><a style='font-family: Arial; font-weight: bold; font-size: 12px;' href='" + productUrl + "'>" + productName + "</a></div>");
                    }
                }
                sb.Append("</td></tr>");
            }

            //Check if related to PharmaProduct
            IPp_document_PKOperations _pPDocumentOperations = new Pp_document_PKDAL();
            IPharmaceutical_product_PKOperations _pProductOperations = new Pharmaceutical_product_PKDAL();

            var pharmaProductDocListMN = _pPDocumentOperations.GetPProductsByDocumentFK(entity_FK);
            if (pharmaProductDocListMN != null && pharmaProductDocListMN.Count != 0)
            {
                sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Pharmaceutical products:</td><td style='vertical-align: top; word-wrap: break-word;'>");

                foreach (var item in pharmaProductDocListMN)
                {
                    Pharmaceutical_product_PK product = _pProductOperations.GetEntity(item.pp_FK);
                    if (product != null)
                    {
                        string pharmaceuticalProductUrl = string.Format("{0}/Views/PharmaceuticalProductView/Preview.aspx?EntityContext=PharmaceuticalProduct&idPharmProd={1}", appURL, product.pharmaceutical_product_PK);
                        string pharmaceuticalProductName = product.name;

                        sb.Append("<div style='margin-bottom: 4px;'><a style='font-family: Arial; font-weight: bold; font-size: 12px;' href='" + pharmaceuticalProductUrl + "'>" + pharmaceuticalProductName + "</a></div>");
                    }
                }
            }

            //Check if related to Project
            IProject_document_PKOperations _projectDocumentMNOperations = new Project_document_PKDAL();
            IProject_PKOperations _projectOperations = new Project_PKDAL();

            var projectDocListMN = _projectDocumentMNOperations.GetProjectMNByDocumentFK(entity_FK);
            if (projectDocListMN != null && projectDocListMN.Count != 0)
            {
                sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Project:</td><td style='vertical-align: top; word-wrap: break-word;'>");

                foreach (var item in projectDocListMN)
                {
                    Project_PK project = _projectOperations.GetEntity(item.project_FK);
                    if (project != null)
                    {
                        string projectUrl = string.Format("{0}/Views/ProjectView/Preview.aspx?EntityContext=Project&idProj={1}", appURL, project.project_PK);
                        string projectName = project.name;

                        sb.Append("<div style='margin-bottom: 4px;'><a style='font-family: Arial; font-weight: bold; font-size: 12px;' href='" + projectUrl + "'>" + projectName + "</a></div>");
                    }
                }
            }

            //Check if related to Activity
            IActivity_document_PKOperations _activityDocumentMNOperations = new Activity_document_PKDAL();
            IActivity_PKOperations _activityOperations = new Activity_PKDAL();

            var activityDocListMN = _activityDocumentMNOperations.GetActivitiesMNByDocument(entity_FK);
            if (activityDocListMN != null && activityDocListMN.Count != 0)
            {
                sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Activity:</td><td style='vertical-align: top; word-wrap: break-word;'>");

                foreach (var item in activityDocListMN)
                {
                    Activity_PK activity = _activityOperations.GetEntity(item.activity_FK);
                    if (activity != null)
                    {
                        string activityUrl = string.Format("{0}/Views/ActivityView/Preview.aspx?EntityContext=Activity&idAct={1}", appURL, activity.activity_PK);
                        string activityName = activity.name;

                        sb.Append("<div style='margin-bottom: 4px;'><a style='font-family: Arial; font-weight: bold; font-size: 12px;' href='" + activityUrl + "'>" + activityName + "</a></div>");
                    }
                }
            }

            //Check if related to Task
            ITask_document_PKOperations _taskDocumentMNOperations = new Task_document_PKDAL();
            ITask_PKOperations _taskOperations = new Task_PKDAL();
            ITask_name_PKOperations _taskNameOperations = new Task_name_PKDAL();

            var taskDocListMN = _taskDocumentMNOperations.GetTasksMNByDocument(entity_FK);
            if (taskDocListMN != null && taskDocListMN.Count != 0)
            {
                sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Task:</td><td style='vertical-align: top; word-wrap: break-word;'>");

                foreach (var item in taskDocListMN)
                {
                    Task_PK task = _taskOperations.GetEntity(item.task_FK);
                    if (task != null)
                    {
                        var taskName = _taskNameOperations.GetEntity(task.task_name_FK);
                        string realTaskName = taskName != null ? taskName.task_name : "Missing";
                        string taskUrl = string.Format("{0}/Views/TaskView/Preview.aspx?EntityContext=Task&idTask={1}", appURL, task.task_PK);

                        sb.Append("<div style='margin-bottom: 4px;'><a style='font-family: Arial; font-weight: bold; font-size: 12px;' href='" + taskUrl + "'>" + realTaskName + "</a></div>");
                    }
                }
            }

            return sb.ToString();
        }

        private static string GetRelatedEntitiesForTask(int? entity_FK)
        {
            ITask_PKOperations _task_PKOperations = new Task_PKDAL();
            IActivity_PKOperations _activity_PKOperations = new Activity_PKDAL();
            IActivity_product_PKOperations _activityProductMNOperations = new Activity_product_PKDAL();

            string appURL = ConfigurationManager.AppSettings["AppURL"];

            Task_PK task = _task_PKOperations.GetEntity(entity_FK);
            Activity_PK activity = _activity_PKOperations.GetEntity(task.activity_FK);

            StringBuilder sb = new StringBuilder();
            sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Activity:</td><td style='vertical-align: top; word-wrap: break-word;'>");
            if (activity != null)
            {
                string ActivityUrl = string.Format("{0}/Views/ActivityView/Preview.aspx?EntityContext=Activity&idAct={1}", appURL, activity.activity_PK);
                string ActitvityName = activity.name != "" ? activity.name.ToString() : "-";

                sb.Append("<div style='margin-bottom: 4px;'><a style='font-family: Arial; font-weight: bold; font-size: 12px;' href='" + ActivityUrl + "'>" + ActitvityName + "</a></div>");
            }
            else
            {
                sb.Append("-");
            }
            sb.Append("</td>");
            
            sb.Append("<tr><td style='font-family: Arial; font-size: 12px; vertical-align: top; word-wrap: break-word;'>Products:</td><td style='vertical-align: top; word-wrap: break-word;'>");
            if (task != null && task.activity_FK != null)
            {
                DataSet ds = _activityProductMNOperations.GetProductsByActivity(task.activity_FK);
                DataTable dt = ds.Tables.Count > 0 ? ds.Tables[0] : null;
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string productUrl = string.Format("{0}/Views/ProductView/Preview.aspx?EntityContext=Product&idProd={1}", appURL, dr["product_PK"].ToString());
                        string productName = dr["name"] != "" ? dr["name"].ToString() : "-";

                        sb.Append("<div style='margin-bottom: 4px;'><a style='font-family: Arial; font-weight: bold; font-size: 12px;' href='" + productUrl + "'>" + productName + "</a></div>");
                    }
                }
                else
                {
                    sb.Append("-");
                }
            } 
            else 
            {
                sb.Append("-");
            }
            sb.Append("</td>");

            return sb.ToString();
        }
    }
}
