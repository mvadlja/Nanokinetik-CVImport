-- Delete
CREATE PROCEDURE  [dbo].[proc_SUBSTANCE_ATTACHMENT_Delete]
	@substance_attachment_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[SUBSTANCE_ATTACHMENT] WHERE [substance_attachment_PK] = @substance_attachment_PK
END
