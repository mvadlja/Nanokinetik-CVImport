-- Save
CREATE PROCEDURE  [dbo].[proc_REMINDER_EMAIL_RECIPIENT_Save]
	@reminder_email_recipient_PK int = NULL,
	@reminder_FK int = NULL,
	@person_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[REMINDER_EMAIL_RECIPIENT]
	SET
	[reminder_FK] = @reminder_FK,
	[person_FK] = @person_FK
	WHERE [reminder_email_recipient_PK] = @reminder_email_recipient_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[REMINDER_EMAIL_RECIPIENT]
		([reminder_FK], [person_FK])
		VALUES
		(@reminder_FK, @person_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
