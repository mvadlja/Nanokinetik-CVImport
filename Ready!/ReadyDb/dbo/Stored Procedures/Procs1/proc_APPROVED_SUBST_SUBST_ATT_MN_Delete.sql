-- Delete
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBST_ATT_MN_Delete]
	@approved_subst_subst_att_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[APPROVED_SUBST_SUBST_ATT_MN] WHERE [approved_subst_subst_att_PK] = @approved_subst_subst_att_PK
END
