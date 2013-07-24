
-- Save
CREATE PROCEDURE [dbo].[proc_SR_RI_MN_Save]
	@sr_ri_mn_PK int = NULL,
	@ri_FK int = NULL,
	@sr_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SR_RI_MN]
	SET
	[ri_FK] = @ri_FK,
	[sr_FK] = @sr_FK
	WHERE [sr_ri_mn_PK] = @sr_ri_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SR_RI_MN]
		([ri_FK], [sr_FK])
		VALUES
		(@ri_FK, @sr_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
