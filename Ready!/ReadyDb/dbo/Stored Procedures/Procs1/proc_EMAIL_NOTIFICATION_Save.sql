-- Save
create PROCEDURE [dbo].[proc_EMAIL_NOTIFICATION_Save]
	@email_notification_PK int = NULL,
	@notification_type_FK int = NULL,
	@email nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[EMAIL_NOTIFICATION]
	SET
	[notification_type_FK] = @notification_type_FK,
	[email] = @email
	WHERE [email_notification_PK] = @email_notification_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[EMAIL_NOTIFICATION]
		([notification_type_FK], [email])
		VALUES
		(@notification_type_FK, @email)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
