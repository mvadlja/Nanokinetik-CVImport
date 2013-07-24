-- GetEntities
CREATE PROCEDURE  [dbo].[proc_REMINDER_EMAIL_RECIPIENT_GetEntitiesByReminder]
@reminder_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reminder_email_recipient_PK], [reminder_FK], [person_FK]
	FROM [dbo].[REMINDER_EMAIL_RECIPIENT]
	where [dbo].[REMINDER_EMAIL_RECIPIENT].[reminder_FK] = @reminder_FK or @reminder_FK is null
END
