-- GetEntitiesWP
CREATE PROCEDURE proc_TIME_UNIT_NAME_GetEntitiesWP
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [time_unit_name_PK]) AS RowNum,
		[time_unit_name_PK], [time_unit_name], [billable]
		FROM [dbo].[TIME_UNIT_NAME]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[TIME_UNIT_NAME]
END
