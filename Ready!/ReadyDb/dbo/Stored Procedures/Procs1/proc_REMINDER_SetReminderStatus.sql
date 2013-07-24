
CREATE PROCEDURE  [dbo].[proc_REMINDER_SetReminderStatus]
	@reminder_date_FK int = NULL,
	@reminder_status_FK int = NULL
AS
BEGIN
	
	UPDATE [REMINDER_DATES]
	set [reminder_status_FK] = @reminder_status_FK
	where reminder_date_PK = @reminder_date_FK
END
