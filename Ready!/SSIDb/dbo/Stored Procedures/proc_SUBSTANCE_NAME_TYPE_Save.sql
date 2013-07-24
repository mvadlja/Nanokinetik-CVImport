
-- Save
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_NAME_TYPE_Save]
	@substance_name_type_PK int = NULL,
	@name nchar(250) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCE_NAME_TYPE]
	SET
	[name] = @name
	WHERE [substance_name_type_PK] = @substance_name_type_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCE_NAME_TYPE]
		([name])
		VALUES
		(@name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
