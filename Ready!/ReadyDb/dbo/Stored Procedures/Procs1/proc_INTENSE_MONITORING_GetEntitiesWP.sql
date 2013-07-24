-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_INTENSE_MONITORING_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [intense_monitoring_PK]) AS RowNum,
		[intense_monitoring_PK], [name]
		FROM [dbo].[INTENSE_MONITORING]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[INTENSE_MONITORING]
END
