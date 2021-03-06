﻿-- Save
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBST_ALIAS_TRANS_MN_Save]
	@approved_subst_sub_alias_trans_PK int = NULL,
	@approved_substance_FK int = NULL,
	@substance_alias_translation_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[APPROVED_SUBST_SUBST_ALIAS_TRANS_MN]
	SET
	[approved_substance_FK] = @approved_substance_FK,
	[substance_alias_translation_FK] = @substance_alias_translation_FK
	WHERE [approved_subst_sub_alias_trans_PK] = @approved_subst_sub_alias_trans_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[APPROVED_SUBST_SUBST_ALIAS_TRANS_MN]
		([approved_substance_FK], [substance_alias_translation_FK])
		VALUES
		(@approved_substance_FK, @substance_alias_translation_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
