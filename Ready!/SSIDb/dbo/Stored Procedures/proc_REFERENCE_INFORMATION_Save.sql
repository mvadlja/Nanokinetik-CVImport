
-- Save
CREATE PROCEDURE [dbo].[proc_REFERENCE_INFORMATION_Save]
	@reference_info_PK int = NULL,
	@comment varchar(4000) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[REFERENCE_INFORMATION]
	SET
	[comment] = @comment
	WHERE [reference_info_PK] = @reference_info_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[REFERENCE_INFORMATION]
		([comment])
		VALUES
		(@comment)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
