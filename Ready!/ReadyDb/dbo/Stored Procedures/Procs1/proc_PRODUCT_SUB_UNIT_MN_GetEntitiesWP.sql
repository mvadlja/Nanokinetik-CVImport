-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_SUB_UNIT_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [product_submission_unit_PK]) AS RowNum,
		[product_submission_unit_PK], [product_FK], [submission_unit_FK]
		FROM [dbo].[PRODUCT_SUB_UNIT_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PRODUCT_SUB_UNIT_MN]
END
