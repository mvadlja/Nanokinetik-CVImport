-- Delete
CREATE PROCEDURE  [dbo].[proc_REMINDER_GetActiveRemindersForUserEntity]
	@user_PK int = null,
	@table_name nvarchar(100) = null,
	@related_attribute_name nvarchar(max) = null,
	@entity_FK int = null
AS
declare @person_FK int = null;
BEGIN
	
	select COUNT(*) as NumReminders
	FROM [dbo].[REMINDER]
	LEFT JOIN REMINDER_DATES reminderDates ON reminderDates.reminder_FK = reminder.reminder_PK
	LEFT JOIN REMINDER_STATUS reminderStatus ON reminderStatus.reminder_status_PK = reminderDates.reminder_status_FK
	WHERE --responsible_user_FK = @person_FK and
	table_name = @table_name
	AND entity_FK = @entity_FK
	AND related_attribute_name = @related_attribute_name 
	AND is_automatic = 0
	and (reminderStatus.name = 'Active' or reminderStatus.name = 'EmailSent')
	
END
