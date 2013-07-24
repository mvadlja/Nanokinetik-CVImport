-- Delete
create PROCEDURE [dbo].[proc_EMAIL_NOTIFICATION_Delete]
	@email_notification_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[EMAIL_NOTIFICATION] WHERE [email_notification_PK] = @email_notification_PK
END
