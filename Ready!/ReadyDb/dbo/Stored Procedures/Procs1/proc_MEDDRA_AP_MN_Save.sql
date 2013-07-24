-- Save
CREATE PROCEDURE  [dbo].[proc_MEDDRA_AP_MN_Save]
	@meddra_ap_mn_PK int = NULL,
	@ap_FK int = NULL,
	@meddra_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MEDDRA_AP_MN]
	SET
	[ap_FK] = @ap_FK,
	[meddra_FK] = @meddra_FK
	WHERE [meddra_ap_mn_PK] = @meddra_ap_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[MEDDRA_AP_MN]
		([ap_FK], [meddra_FK])
		VALUES
		(@ap_FK, @meddra_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
