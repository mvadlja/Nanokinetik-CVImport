-- Delete
CREATE PROCEDURE  [dbo].[proc_PP_ADJUVANT_DeleteNULLByUserID]
	@UserID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PP_ADJUVANT]
	WHERE [dbo].[PP_ADJUVANT].[userID] = @UserID
	AND [dbo].[PP_ADJUVANT].pp_FK IS NULL
END
