-- Delete
CREATE PROCEDURE  [dbo].[proc_REMINDER_DeleteOldDismissedAutomaticReminders]
	@remider_date datetime = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[REMINDER] WHERE reminder_PK IN 
	(
		SELECT reminder.reminder_PK 
		FROM [dbo].[REMINDER] reminder
		LEFT JOIN REMINDER_DATES reminderDates ON reminderDates.reminder_FK = reminder.reminder_PK
		LEFT JOIN REMINDER_STATUS reminderStatus ON reminderStatus.reminder_status_PK = reminderDates.reminder_status_FK
		WHERE 
			reminder.is_automatic = 1 AND	
			reminderDates.reminder_date < @remider_date AND 
			reminderStatus.name = 'Dissmised'
	)
END
