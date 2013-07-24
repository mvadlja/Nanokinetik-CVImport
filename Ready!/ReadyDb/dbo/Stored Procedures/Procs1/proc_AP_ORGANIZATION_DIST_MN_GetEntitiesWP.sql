-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_AP_ORGANIZATION_DIST_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ap_organizatation_dist_mn_PK]) AS RowNum,
		[ap_organizatation_dist_mn_PK], [organization_FK], [ap_FK]
		FROM [dbo].[AP_ORGANIZATION_DIST_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[AP_ORGANIZATION_DIST_MN]
END
