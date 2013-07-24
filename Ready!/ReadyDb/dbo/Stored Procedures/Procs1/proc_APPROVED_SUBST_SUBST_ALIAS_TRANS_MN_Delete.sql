-- Delete
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBST_ALIAS_TRANS_MN_Delete]
	@approved_subst_sub_alias_trans_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[APPROVED_SUBST_SUBST_ALIAS_TRANS_MN] WHERE [approved_subst_sub_alias_trans_PK] = @approved_subst_sub_alias_trans_PK
END
