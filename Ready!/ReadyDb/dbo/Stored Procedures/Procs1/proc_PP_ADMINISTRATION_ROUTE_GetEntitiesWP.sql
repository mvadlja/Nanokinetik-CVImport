-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PP_ADMINISTRATION_ROUTE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [adminroute_PK]) AS RowNum,
		[adminroute_PK], [adminroutecode], [resolutionmode], [ev_code]
		FROM [dbo].[PP_ADMINISTRATION_ROUTE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PP_ADMINISTRATION_ROUTE]
END
