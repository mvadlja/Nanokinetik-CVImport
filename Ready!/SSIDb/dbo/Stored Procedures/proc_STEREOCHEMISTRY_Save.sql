
-- Save
CREATE PROCEDURE [dbo].[proc_STEREOCHEMISTRY_Save]
	@stereochemistry_PK int = NULL,
	@name nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[STEREOCHEMISTRY]
	SET
	[name] = @name
	WHERE [stereochemistry_PK] = @stereochemistry_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[STEREOCHEMISTRY]
		([name])
		VALUES
		(@name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
