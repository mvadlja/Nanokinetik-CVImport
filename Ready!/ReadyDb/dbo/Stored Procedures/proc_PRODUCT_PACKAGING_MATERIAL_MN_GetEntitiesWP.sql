
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_PRODUCT_PACKAGING_MATERIAL_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [product_packaging_material_mn_PK]) AS RowNum,
		[product_packaging_material_mn_PK], [product_FK], [type_FK]
		FROM [dbo].[PRODUCT_PACKAGING_MATERIAL_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PRODUCT_PACKAGING_MATERIAL_MN]
END