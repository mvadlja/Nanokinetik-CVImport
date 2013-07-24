-- Delete
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_SAVED_SEARCH_Delete]
	@activity_saved_search_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ACTIVITY_SAVED_SEARCH] WHERE [activity_saved_search_PK] = @activity_saved_search_PK
END
