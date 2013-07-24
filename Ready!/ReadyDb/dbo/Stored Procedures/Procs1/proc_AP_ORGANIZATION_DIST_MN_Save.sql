-- Save
CREATE PROCEDURE  [dbo].[proc_AP_ORGANIZATION_DIST_MN_Save]
	@ap_organizatation_dist_mn_PK int = NULL,
	@organization_FK int = NULL,
	@ap_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[AP_ORGANIZATION_DIST_MN]
	SET
	[organization_FK] = @organization_FK,
	[ap_FK] = @ap_FK
	WHERE [ap_organizatation_dist_mn_PK] = @ap_organizatation_dist_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[AP_ORGANIZATION_DIST_MN]
		([organization_FK], [ap_FK])
		VALUES
		(@organization_FK, @ap_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
