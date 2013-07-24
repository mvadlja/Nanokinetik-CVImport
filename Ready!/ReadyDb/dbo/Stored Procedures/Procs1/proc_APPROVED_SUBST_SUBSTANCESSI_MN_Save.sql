-- Save
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBSTANCESSI_MN_Save]
	@approved_subst_substancessi_PK int = NULL,
	@approved_substance_FK int = NULL,
	@substancessi_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[APPROVED_SUBST_SUBSTANCESSI_MN]
	SET
	[approved_substance_FK] = @approved_substance_FK,
	[substancessi_FK] = @substancessi_FK
	WHERE [approved_subst_substancessi_PK] = @approved_subst_substancessi_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[APPROVED_SUBST_SUBSTANCESSI_MN]
		([approved_substance_FK], [substancessi_FK])
		VALUES
		(@approved_substance_FK, @substancessi_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
