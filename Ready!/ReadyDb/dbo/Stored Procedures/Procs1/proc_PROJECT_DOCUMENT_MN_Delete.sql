-- Delete
CREATE PROCEDURE  [dbo].[proc_PROJECT_DOCUMENT_MN_Delete]
	@project_document_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PROJECT_DOCUMENT_MN] WHERE [project_document_PK] = @project_document_PK
END
