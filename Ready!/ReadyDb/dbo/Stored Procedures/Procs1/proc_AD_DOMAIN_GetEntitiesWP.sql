-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_AD_DOMAIN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ad_domain_PK]) AS RowNum,
		[ad_domain_PK], [domain_alias], [domain_connection_string]
		FROM [dbo].[AD_DOMAIN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[AD_DOMAIN]
END
