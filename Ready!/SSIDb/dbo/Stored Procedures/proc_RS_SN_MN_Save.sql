
-- Save
CREATE PROCEDURE [dbo].[proc_RS_SN_MN_Save]
	@rs_sn_mn_PK int = NULL,
	@rs_FK int = NULL,
	@substance_name_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RS_SN_MN]
	SET
	[rs_FK] = @rs_FK,
	[substance_name_FK] = @substance_name_FK
	WHERE [rs_sn_mn_PK] = @rs_sn_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RS_SN_MN]
		([rs_FK], [substance_name_FK])
		VALUES
		(@rs_FK, @substance_name_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
