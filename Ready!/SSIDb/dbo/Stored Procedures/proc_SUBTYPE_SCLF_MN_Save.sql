
-- Save
CREATE PROCEDURE [dbo].[proc_SUBTYPE_SCLF_MN_Save]
	@subtype_sclf_mn_PK int = NULL,
	@subtype_FK int = NULL,
	@sclf_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[SUBTYPE_SCLF_MN]
	SET
	[subtype_FK] = @subtype_FK,
	[sclf_FK] = @sclf_FK
	WHERE [subtype_sclf_mn_PK] = @subtype_sclf_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[SUBTYPE_SCLF_MN]
		([subtype_FK], [sclf_FK])
		VALUES
		(@subtype_FK, @sclf_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
