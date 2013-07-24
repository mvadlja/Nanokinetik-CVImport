
-- Save
CREATE PROCEDURE [dbo].[proc_RS_TARGET_MN_Save]
	@rs_target_mn_PK int = NULL,
	@rs_FK int = NULL,
	@target_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RS_TARGET_MN]
	SET
	[rs_FK] = @rs_FK,
	[target_FK] = @target_FK
	WHERE [rs_target_mn_PK] = @rs_target_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RS_TARGET_MN]
		([rs_FK], [target_FK])
		VALUES
		(@rs_FK, @target_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
