-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_SAVED_SEARCH_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [time_unit_saved_search_PK]) AS RowNum,
		[time_unit_saved_search_PK], [activity_FK], [time_unit_FK], [user_FK], [actual_date_from], [actual_date_to], [displayName], [user_FK1], [gridLayout], [isPublic]
		FROM [dbo].[TIME_UNIT_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[TIME_UNIT_SAVED_SEARCH]
END
