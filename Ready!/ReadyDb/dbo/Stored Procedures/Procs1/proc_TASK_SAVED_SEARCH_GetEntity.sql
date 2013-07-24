-- GetEntity
CREATE PROCEDURE  [dbo].[proc_TASK_SAVED_SEARCH_GetEntity]
	@task_saved_search_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[task_saved_search_PK], [activity_FK], [name], [user_FK], [type_internal_status_FK], [country_FK], [start_date_from], [start_date_to], [expected_finished_date_from], [expected_finished_date_to], [actual_finished_date_from], [actual_finished_date_to], [displayName], [user_FK1], [gridLayout], [isPublic]
	FROM [dbo].[TASK_SAVED_SEARCH]
	WHERE [task_saved_search_PK] = @task_saved_search_PK
END
