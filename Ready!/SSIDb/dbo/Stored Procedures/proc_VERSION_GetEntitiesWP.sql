
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_VERSION_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [version_PK]) AS RowNum,
		[version_PK], [version_number], [effectve_date], [change_made]
		FROM [dbo].[VERSION]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[VERSION]
END
