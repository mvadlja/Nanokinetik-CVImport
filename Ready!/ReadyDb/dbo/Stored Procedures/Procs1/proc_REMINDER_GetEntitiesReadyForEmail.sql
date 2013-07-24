-- GetEntitiesReadyForEmail
CREATE PROCEDURE  [dbo].[proc_REMINDER_GetEntitiesReadyForEmail]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT reminder.*, reminderDates.reminder_date_PK AS reminder_date_PK, reminderDates.reminder_date, reminderStatus.name AS status, reminderRepeatingMode.name AS repeating_mode
	FROM [dbo].[REMINDER] reminder
	LEFT JOIN REMINDER_DATES reminderDates ON reminderDates.reminder_FK = reminder.reminder_PK
	LEFT JOIN REMINDER_STATUS reminderStatus ON reminderStatus.reminder_status_PK = reminderDates.reminder_status_FK
	LEFT JOIN REMINDER_REPEATING_MODES reminderRepeatingMode ON reminderRepeatingMode.reminder_repeating_mode_PK = reminderDates.reminder_repeating_mode_FK	
	WHERE reminderStatus.name = 'Active'
	AND reminder.[remind_me_on_email] = 1
	AND reminder_date <= GETDATE()--DATEADD(day,- isnull(time_before_activation,0),reminder_date) <= GETDATE()

END
