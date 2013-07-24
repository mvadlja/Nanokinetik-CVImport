
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_NS_PROPERTY_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ns_property_mn_PK]) AS RowNum,
		[ns_property_mn_PK], [ns_FK], [property_FK]
		FROM [dbo].[NS_PROPERTY_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[NS_PROPERTY_MN]
END
