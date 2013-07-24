-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PROJECT_SAVED_SEARCH_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [project_saved_search_PK]) AS RowNum,
		[project_saved_search_PK], [name], [user_FK], [internal_status_type_FK], [country_FK], [start_date_from], [start_date_to], [expected_finished_date_from], [expected_finished_dat_to], [actual_finished_date_from], [actual_finished_date_to], [displayName], [user_FK1], [gridLayout], [isPublic]
		FROM [dbo].[PROJECT_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PROJECT_SAVED_SEARCH]
END
