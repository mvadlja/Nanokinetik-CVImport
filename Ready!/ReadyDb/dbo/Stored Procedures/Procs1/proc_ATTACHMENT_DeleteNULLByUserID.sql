-- Delete
CREATE PROCEDURE  [dbo].[proc_ATTACHMENT_DeleteNULLByUserID]
	@UserID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ATTACHMENT] 
	WHERE [dbo].[ATTACHMENT].[userID] = @UserID
	AND [dbo].[ATTACHMENT].document_FK IS NULL
END
