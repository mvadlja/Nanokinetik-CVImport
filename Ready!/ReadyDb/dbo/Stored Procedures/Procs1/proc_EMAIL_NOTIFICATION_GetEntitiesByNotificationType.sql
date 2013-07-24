-- GetEntities
create PROCEDURE [dbo].[proc_EMAIL_NOTIFICATION_GetEntitiesByNotificationType]
	@notification_type_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[email_notification_PK], [notification_type_FK], [email]
	FROM [dbo].[EMAIL_NOTIFICATION]

	WHERE dbo.[EMAIL_NOTIFICATION].[notification_type_FK] = @notification_type_FK
END
