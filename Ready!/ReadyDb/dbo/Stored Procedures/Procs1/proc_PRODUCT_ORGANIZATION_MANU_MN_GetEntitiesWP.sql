-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ORGANIZATION_MANU_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [product_organization_manu_mn_PK]) AS RowNum,
		[product_organization_manu_mn_PK], [organization_FK], [product_FK]
		FROM [dbo].[PRODUCT_ORGANIZATION_MANU_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PRODUCT_ORGANIZATION_MANU_MN]
END
