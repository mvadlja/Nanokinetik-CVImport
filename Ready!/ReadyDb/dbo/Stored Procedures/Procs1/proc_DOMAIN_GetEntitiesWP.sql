-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_DOMAIN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [domain_PK]) AS RowNum,
		[domain_PK], [name]
		FROM [dbo].[DOMAIN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[DOMAIN]
END
