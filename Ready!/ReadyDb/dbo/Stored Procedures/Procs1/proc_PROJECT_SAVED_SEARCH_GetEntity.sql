-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PROJECT_SAVED_SEARCH_GetEntity]
	@project_saved_search_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[project_saved_search_PK], [name], [user_FK], [internal_status_type_FK], [country_FK], [start_date_from], [start_date_to], [expected_finished_date_from], [expected_finished_dat_to], [actual_finished_date_from], [actual_finished_date_to], [displayName], [user_FK1], [gridLayout], [isPublic]
	FROM [dbo].[PROJECT_SAVED_SEARCH]
	WHERE [project_saved_search_PK] = @project_saved_search_PK
END
