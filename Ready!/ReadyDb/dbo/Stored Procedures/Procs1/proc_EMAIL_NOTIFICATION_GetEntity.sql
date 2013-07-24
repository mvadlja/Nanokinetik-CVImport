-- GetEntity
create PROCEDURE [dbo].[proc_EMAIL_NOTIFICATION_GetEntity]
	@email_notification_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[email_notification_PK], [notification_type_FK], [email]
	FROM [dbo].[EMAIL_NOTIFICATION]
	WHERE [email_notification_PK] = @email_notification_PK
END
