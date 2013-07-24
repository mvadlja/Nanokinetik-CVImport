
-- Save
CREATE PROCEDURE [dbo].[proc_RI_SCLF_MN_Save]
	@ri_sclf_mn_PK int = NULL,
	@ref_info_FK int = NULL,
	@sclf_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RI_SCLF_MN]
	SET
	[ref_info_FK] = @ref_info_FK,
	[sclf_FK] = @sclf_FK
	WHERE [ri_sclf_mn_PK] = @ri_sclf_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RI_SCLF_MN]
		([ref_info_FK], [sclf_FK])
		VALUES
		(@ref_info_FK, @sclf_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
