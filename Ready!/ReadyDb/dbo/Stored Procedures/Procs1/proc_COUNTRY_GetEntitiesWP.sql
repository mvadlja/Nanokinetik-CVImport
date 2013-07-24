-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [country_PK]) AS RowNum,
		[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
		FROM [dbo].[COUNTRY]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[COUNTRY]
END
