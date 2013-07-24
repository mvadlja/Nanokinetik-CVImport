-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'time_unit_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[dbo].[TIME_UNIT].*, [dbo].[TIME_UNIT_NAME].time_unit_name AS Name
		FROM [dbo].[TIME_UNIT]
		LEFT JOIN [dbo].[TIME_UNIT_NAME] ON [dbo].[TIME_UNIT].time_unit_name_FK = [dbo].[TIME_UNIT_NAME].time_unit_name_PK
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[TIME_UNIT]
END
