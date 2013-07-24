-- Save
CREATE PROCEDURE  [dbo].[proc_LANGUAGE_CODE_Save]
	@languagecode_PK int = NULL,
	@name nvarchar(30) = NULL,
	@code nvarchar(4) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[LANGUAGE_CODE]
	SET
	[name] = @name,
	[code] = @code
	WHERE [languagecode_PK] = @languagecode_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[LANGUAGE_CODE]
		([name], [code])
		VALUES
		(@name, @code)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
