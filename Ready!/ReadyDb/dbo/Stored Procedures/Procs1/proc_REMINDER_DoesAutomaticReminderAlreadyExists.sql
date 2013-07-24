CREATE PROCEDURE  [dbo].[proc_REMINDER_DoesAutomaticReminderAlreadyExists]
	@table_name nvarchar(50) = null,
	@entity_FK int = null,
	@related_attribute_name nvarchar(50) = null,
	@reminder_date datetime = null

AS
declare @numReminders int = null;
BEGIN
	
	select reminder.*, reminderDates.reminder_date_PK AS reminder_date_PK, reminderDates.reminder_date, reminderStatus.name AS status--, reminderRepeatingMode.name AS repeating_mode
	
	--@numReminders = COUNT(reminder.reminder_PK) 
	FROM [dbo].[REMINDER] reminder
	LEFT JOIN REMINDER_DATES reminderDates ON reminderDates.reminder_FK = reminder.reminder_PK
	LEFT JOIN REMINDER_STATUS reminderStatus ON reminderStatus.reminder_status_PK = reminderDates.reminder_status_FK
	WHERE (reminder.table_name = @table_name or @table_name is null)
	and (reminder.entity_FK = @entity_FK or @table_name is null)
	and (reminder.related_attribute_name = @related_attribute_name or @table_name is null)
	and (reminderDates.reminder_date = @reminder_date or @table_name is null)
	and reminder.is_automatic = 1

	if @numReminders is null set @numReminders = 0;

	select @numReminders as NumReminders
END
