-- Delete
CREATE PROCEDURE  [dbo].[proc_REMINDER_EMAIL_RECIPIENT_Delete]
	@reminder_email_recipient_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[REMINDER_EMAIL_RECIPIENT] WHERE [reminder_email_recipient_PK] = @reminder_email_recipient_PK
END
