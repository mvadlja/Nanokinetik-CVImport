
-- Save
CREATE PROCEDURE [dbo].[proc_RS_CHEMICAL_MN_Save]
	@rs_chemical_mn_PK int = NULL,
	@rs_FK int = NULL,
	@chemical_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[RS_CHEMICAL_MN]
	SET
	[rs_FK] = @rs_FK,
	[chemical_FK] = @chemical_FK
	WHERE [rs_chemical_mn_PK] = @rs_chemical_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[RS_CHEMICAL_MN]
		([rs_FK], [chemical_FK])
		VALUES
		(@rs_FK, @chemical_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
