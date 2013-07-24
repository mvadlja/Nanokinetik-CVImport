-- Delete
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_INTER_CODE_MN_Delete]
	@approved_subst_inter_code_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[APPROVED_SUBST_INTER_CODE_MN] WHERE [approved_subst_inter_code_PK] = @approved_subst_inter_code_PK
END
