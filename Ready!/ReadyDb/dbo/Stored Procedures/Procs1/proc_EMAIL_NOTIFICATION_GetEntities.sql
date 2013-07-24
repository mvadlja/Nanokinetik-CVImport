-- GetEntities
create PROCEDURE [dbo].[proc_EMAIL_NOTIFICATION_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[email_notification_PK], [notification_type_FK], [email]
	FROM [dbo].[EMAIL_NOTIFICATION]
END
