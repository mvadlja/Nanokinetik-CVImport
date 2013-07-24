
-- Save
CREATE PROCEDURE [dbo].[proc_RS_GE_MN_Save]
	@rs_ge_mn_PK int = NULL,
	@rs_FK int = NULL,
	@ge_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RS_GE_MN]
	SET
	[rs_FK] = @rs_FK,
	[ge_FK] = @ge_FK
	WHERE [rs_ge_mn_PK] = @rs_ge_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RS_GE_MN]
		([rs_FK], [ge_FK])
		VALUES
		(@rs_FK, @ge_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
