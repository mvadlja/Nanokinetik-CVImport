
-- Save
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_TYPE_Save]
	@official_name_type_PK int = NULL,
	@type_name nchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[OFFICIAL_NAME_TYPE]
	SET
	[type_name] = @type_name
	WHERE [official_name_type_PK] = @official_name_type_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[OFFICIAL_NAME_TYPE]
		([type_name])
		VALUES
		(@type_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
