-- Save
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBST_ALIAS_MN_Save]
	@approved_substance_subst_alias_PK int = NULL,
	@approved_substance_FK int = NULL,
	@substance_alias_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[APPROVED_SUBST_SUBST_ALIAS_MN]
	SET
	[approved_substance_FK] = @approved_substance_FK,
	[substance_alias_FK] = @substance_alias_FK
	WHERE [approved_substance_subst_alias_PK] = @approved_substance_subst_alias_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[APPROVED_SUBST_SUBST_ALIAS_MN]
		([approved_substance_FK], [substance_alias_FK])
		VALUES
		(@approved_substance_FK, @substance_alias_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
