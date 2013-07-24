-- Delete
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_SUBSTANCESSI_MN_Delete]
	@approved_subst_substancessi_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[APPROVED_SUBST_SUBSTANCESSI_MN] WHERE [approved_subst_substancessi_PK] = @approved_subst_substancessi_PK
END
