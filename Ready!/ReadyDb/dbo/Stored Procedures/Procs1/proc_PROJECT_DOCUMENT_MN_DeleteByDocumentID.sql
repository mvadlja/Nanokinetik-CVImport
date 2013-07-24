-- Delete
CREATE PROCEDURE  [dbo].[proc_PROJECT_DOCUMENT_MN_DeleteByDocumentID]
	@document_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PROJECT_DOCUMENT_MN] WHERE document_FK = @document_PK
END
