
-- Save
CREATE PROCEDURE [dbo].[proc_REMINDER_REPEATING_MODES_Save]
	@reminder_repeating_mode_PK int = NULL,
	@name nvarchar(100) = NULL,
	@enum_name nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[REMINDER_REPEATING_MODES]
	SET
	[name] = @name,
	[enum_name] = @enum_name
	WHERE [reminder_repeating_mode_PK] = @reminder_repeating_mode_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[REMINDER_REPEATING_MODES]
		([name], [enum_name])
		VALUES
		(@name, @enum_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END