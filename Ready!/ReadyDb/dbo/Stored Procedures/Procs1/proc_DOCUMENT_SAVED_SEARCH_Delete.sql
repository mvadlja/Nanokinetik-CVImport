-- Delete
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_SAVED_SEARCH_Delete]
	@document_saved_search_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[DOCUMENT_SAVED_SEARCH] WHERE [document_saved_search_PK] = @document_saved_search_PK
END
