
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_NS_MOIETY_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ns_moiety_mn_PK]) AS RowNum,
		[ns_moiety_mn_PK], [moiety_FK], [ns_FK]
		FROM [dbo].[NS_MOIETY_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[NS_MOIETY_MN]
END
