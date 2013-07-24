
-- Save
CREATE PROCEDURE [dbo].[proc_VERSION_SUBSTANCE_MN_Save]
	@version_substance_mn_PK int = NULL,
	@version_FK int = NULL,
	@substance_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[VERSION_SUBSTANCE_MN]
	SET
	[version_FK] = @version_FK,
	[substance_FK] = @substance_FK
	WHERE [version_substance_mn_PK] = @version_substance_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[VERSION_SUBSTANCE_MN]
		([version_FK], [substance_FK])
		VALUES
		(@version_FK, @substance_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
