-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PP_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [product_mn_PK]) AS RowNum,
		[product_mn_PK], [product_FK], [pp_FK]
		FROM [dbo].[PRODUCT_PP_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PRODUCT_PP_MN]
END
