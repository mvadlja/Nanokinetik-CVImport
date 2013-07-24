-- Save
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBST_ATT_MN_Save]
	@approved_subst_subst_att_PK int = NULL,
	@approved_substance_FK int = NULL,
	@substance_attachment_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[APPROVED_SUBST_SUBST_ATT_MN]
	SET
	[approved_substance_FK] = @approved_substance_FK,
	[substance_attachment_FK] = @substance_attachment_FK
	WHERE [approved_subst_subst_att_PK] = @approved_subst_subst_att_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[APPROVED_SUBST_SUBST_ATT_MN]
		([approved_substance_FK], [substance_attachment_FK])
		VALUES
		(@approved_substance_FK, @substance_attachment_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
