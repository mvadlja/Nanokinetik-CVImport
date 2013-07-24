
-- Save
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_SUBSTANCE_NAME_MN_Save]
	@substance_substance_name_mn_PK int = NULL,
	@substance_FK int = NULL,
	@substance_name_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBSTANCE_SUBSTANCE_NAME_MN]
	SET
	[substance_FK] = @substance_FK,
	[substance_name_FK] = @substance_name_FK
	WHERE [substance_substance_name_mn_PK] = @substance_substance_name_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBSTANCE_SUBSTANCE_NAME_MN]
		([substance_FK], [substance_name_FK])
		VALUES
		(@substance_FK, @substance_name_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
