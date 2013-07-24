-- Delete
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_Delete]
	@document_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[DOCUMENT] WHERE [document_PK] = @document_PK
END
