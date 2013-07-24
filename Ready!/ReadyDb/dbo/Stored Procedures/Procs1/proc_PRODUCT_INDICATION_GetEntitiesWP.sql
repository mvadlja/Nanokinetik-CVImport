-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_INDICATION_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [product_indications_PK]) AS RowNum,
		[product_indications_PK], [meddraversion], [meddralevel], [meddracode], [name]
		FROM [dbo].[PRODUCT_INDICATION]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PRODUCT_INDICATION]
END
