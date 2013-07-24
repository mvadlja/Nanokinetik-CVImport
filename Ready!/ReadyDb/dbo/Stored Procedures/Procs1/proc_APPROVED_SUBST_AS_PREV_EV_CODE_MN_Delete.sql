-- Delete
CREATE PROCEDURE  [dbo].[proc_APPROVED_SUBST_AS_PREV_EV_CODE_MN_Delete]
	@approved_subst_prev_ev_code_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[APPROVED_SUBST_AS_PREV_EV_CODE_MN] WHERE [approved_subst_prev_ev_code_PK] = @approved_subst_prev_ev_code_PK
END
