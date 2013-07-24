-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_PROJECT_SAVED_SEARCH_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'project_saved_search_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[project_saved_search_PK], [name], [user_FK], [internal_status_type_FK], [country_FK], [start_date_from], [start_date_to], [expected_finished_date_from], [expected_finished_dat_to], [actual_finished_date_from], [actual_finished_date_to], [displayName], [user_FK1], [gridLayout], [isPublic]
		FROM [dbo].[PROJECT_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PROJECT_SAVED_SEARCH]
END
