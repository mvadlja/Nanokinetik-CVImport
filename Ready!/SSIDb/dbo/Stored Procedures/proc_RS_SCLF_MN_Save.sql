
-- Save
CREATE PROCEDURE [dbo].[proc_RS_SCLF_MN_Save]
	@rs_sclf_mn_PK int = NULL,
	@sclf_FK int = NULL,
	@rs_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RS_SCLF_MN]
	SET
	[sclf_FK] = @sclf_FK,
	[rs_FK] = @rs_FK
	WHERE [rs_sclf_mn_PK] = @rs_sclf_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RS_SCLF_MN]
		([sclf_FK], [rs_FK])
		VALUES
		(@sclf_FK, @rs_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
