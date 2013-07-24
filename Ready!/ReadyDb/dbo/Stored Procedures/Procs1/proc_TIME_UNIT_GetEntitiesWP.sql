-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_TIME_UNIT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [time_unit_PK]) AS RowNum,
		[dbo].[TIME_UNIT].*, [dbo].[TIME_UNIT_NAME].time_unit_name AS Name
		FROM [dbo].[TIME_UNIT]
		LEFT JOIN [dbo].[TIME_UNIT_NAME] ON [dbo].[TIME_UNIT].time_unit_name_FK = [dbo].[TIME_UNIT_NAME].time_unit_name_PK

	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[TIME_UNIT]
END
