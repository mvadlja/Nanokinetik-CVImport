
CREATE PROCEDURE  [dbo].[proc_REMINDER_DismissReminder]
	@reminder_PK int = NULL
AS
BEGIN
	
	UPDATE dbo.REMINDER_DATES  
	set reminder_status_FK = 3 -- Dissmised
	where reminder_FK = @reminder_PK
	
END
