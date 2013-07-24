
-- Save
CREATE PROCEDURE [dbo].[proc_REMINDER_USER_STATUS_Save]
	@reminder_user_status_PK int = NULL,
	@name nvarchar(100) = NULL,
	@enum_name nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[REMINDER_USER_STATUS]
	SET
	[name] = @name,
	[enum_name] = @enum_name
	WHERE [reminder_user_status_PK] = @reminder_user_status_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[REMINDER_USER_STATUS]
		([name], [enum_name])
		VALUES
		(@name, @enum_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END