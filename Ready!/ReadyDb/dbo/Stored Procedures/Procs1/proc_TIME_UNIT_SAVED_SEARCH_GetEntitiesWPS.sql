-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_SAVED_SEARCH_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'time_unit_saved_search_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[time_unit_saved_search_PK], [activity_FK], [time_unit_FK], [user_FK], [actual_date_from], [actual_date_to], [displayName], [user_FK1], [gridLayout], [isPublic]
		FROM [dbo].[TIME_UNIT_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[TIME_UNIT_SAVED_SEARCH]
END
