-- Delete
CREATE PROCEDURE  [dbo].[proc_TASK_SAVED_SEARCH_Delete]
	@task_saved_search_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[TASK_SAVED_SEARCH] WHERE [task_saved_search_PK] = @task_saved_search_PK
END
