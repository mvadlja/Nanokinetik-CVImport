-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_ORG_IN_TYPE_FOR_MANUFACTURER_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [org_in_type_for_manufacturer_ID]) AS RowNum,
		[org_in_type_for_manufacturer_ID], [organization_FK], [org_type_for_manu_FK], [product_FK],  [substance_FK]
		FROM [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ORG_IN_TYPE_FOR_MANUFACTURER]
END
