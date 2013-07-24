-- Delete
CREATE PROCEDURE  [dbo].[proc_ATTACHMENT_Delete]
	@attachment_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ATTACHMENT] WHERE [attachment_PK] = @attachment_PK
END
