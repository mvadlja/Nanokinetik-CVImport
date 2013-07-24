-- GetEntities
CREATE PROCEDURE  [dbo].[proc_REMINDER_EMAIL_RECIPIENT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[reminder_email_recipient_PK], [reminder_FK], [person_FK]
	FROM [dbo].[REMINDER_EMAIL_RECIPIENT]
END
