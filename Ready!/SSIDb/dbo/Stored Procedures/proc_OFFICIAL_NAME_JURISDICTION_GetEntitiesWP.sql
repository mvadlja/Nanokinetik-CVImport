
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_JURISDICTION_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [jurisdiction_PK]) AS RowNum,
		[jurisdiction_PK], [on_jurisd]
		FROM [dbo].[OFFICIAL_NAME_JURISDICTION]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[OFFICIAL_NAME_JURISDICTION]
END
