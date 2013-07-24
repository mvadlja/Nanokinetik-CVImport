-- Delete
CREATE PROCEDURE  [dbo].[proc_SUB_ALIAS_SUB_ALIAS_TRAN_MN_Delete]
	@sub_alias_sub_alias_tran_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUB_ALIAS_SUB_ALIAS_TRAN_MN] WHERE [sub_alias_sub_alias_tran_PK] = @sub_alias_sub_alias_tran_PK
END
