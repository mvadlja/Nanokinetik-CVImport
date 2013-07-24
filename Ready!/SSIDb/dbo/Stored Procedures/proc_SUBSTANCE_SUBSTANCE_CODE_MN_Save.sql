
-- Save
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_SUBSTANCE_CODE_MN_Save]
	@substance_substance_code_mn_PK int = NULL,
	@substance_FK int = NULL,
	@substance_code_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCE_SUBSTANCE_CODE_MN]
	SET
	[substance_FK] = @substance_FK,
	[substance_code_FK] = @substance_code_FK
	WHERE [substance_substance_code_mn_PK] = @substance_substance_code_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCE_SUBSTANCE_CODE_MN]
		([substance_FK], [substance_code_FK])
		VALUES
		(@substance_FK, @substance_code_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
