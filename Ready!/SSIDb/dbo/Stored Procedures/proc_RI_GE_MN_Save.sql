
-- Save
CREATE PROCEDURE [dbo].[proc_RI_GE_MN_Save]
	@ri_ge_mn_PK int = NULL,
	@ri_FK int = NULL,
	@ge_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RI_GE_MN]
	SET
	[ri_FK] = @ri_FK,
	[ge_FK] = @ge_FK
	WHERE [ri_ge_mn_PK] = @ri_ge_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RI_GE_MN]
		([ri_FK], [ge_FK])
		VALUES
		(@ri_FK, @ge_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
