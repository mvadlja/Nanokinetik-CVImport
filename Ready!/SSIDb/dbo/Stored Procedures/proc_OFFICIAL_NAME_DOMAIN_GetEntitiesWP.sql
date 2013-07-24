
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_OFFICIAL_NAME_DOMAIN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [on_domain_PK]) AS RowNum,
		[on_domain_PK], [name_domain]
		FROM [dbo].[OFFICIAL_NAME_DOMAIN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[OFFICIAL_NAME_DOMAIN]
END
