
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_ON_DOMAIN_ON_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [on_domain_on_mn_PK]) AS RowNum,
		[on_domain_on_mn_PK], [on_domain_FK], [on_FK]
		FROM [dbo].[ON_DOMAIN_ON_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ON_DOMAIN_ON_MN]
END
