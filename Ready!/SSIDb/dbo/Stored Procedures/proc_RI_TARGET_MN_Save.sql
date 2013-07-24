
-- Save
CREATE PROCEDURE [dbo].[proc_RI_TARGET_MN_Save]
	@ri_target_mn_PK int = NULL,
	@ri_FK int = NULL,
	@target_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RI_TARGET_MN]
	SET
	[ri_FK] = @ri_FK,
	[target_FK] = @target_FK
	WHERE [ri_target_mn_PK] = @ri_target_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RI_TARGET_MN]
		([ri_FK], [target_FK])
		VALUES
		(@ri_FK, @target_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
