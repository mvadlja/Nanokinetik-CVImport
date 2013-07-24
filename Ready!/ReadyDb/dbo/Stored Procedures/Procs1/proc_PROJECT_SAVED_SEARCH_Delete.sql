-- Delete
CREATE PROCEDURE  [dbo].[proc_PROJECT_SAVED_SEARCH_Delete]
	@project_saved_search_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PROJECT_SAVED_SEARCH] WHERE [project_saved_search_PK] = @project_saved_search_PK
END
