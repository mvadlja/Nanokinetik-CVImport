
-- Save
CREATE PROCEDURE [dbo].[proc_RS_SC_MN_Save]
	@rs_sc_mn_PK int = NULL,
	@rs_FK int = NULL,
	@sc_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RS_SC_MN]
	SET
	[rs_FK] = @rs_FK,
	[sc_FK] = @sc_FK
	WHERE [rs_sc_mn_PK] = @rs_sc_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RS_SC_MN]
		([rs_FK], [sc_FK])
		VALUES
		(@rs_FK, @sc_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
