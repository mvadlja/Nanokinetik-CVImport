
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_VERSION_SUBSTANCE_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [version_substance_mn_PK]) AS RowNum,
		[version_substance_mn_PK], [version_FK], [substance_FK]
		FROM [dbo].[VERSION_SUBSTANCE_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[VERSION_SUBSTANCE_MN]
END
