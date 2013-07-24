-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PI_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [product_pi_mn_PK]) AS RowNum,
		[product_pi_mn_PK], [product_indications_FK], [product_FK]
		FROM [dbo].[PRODUCT_PI_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PRODUCT_PI_MN]
END
