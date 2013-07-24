-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PROJECT_COUNTRY_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [project_country_PK]) AS RowNum,
		[project_country_PK], [project_FK], [country_FK]
		FROM [dbo].[PROJECT_COUNTRY_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PROJECT_COUNTRY_MN]
END
