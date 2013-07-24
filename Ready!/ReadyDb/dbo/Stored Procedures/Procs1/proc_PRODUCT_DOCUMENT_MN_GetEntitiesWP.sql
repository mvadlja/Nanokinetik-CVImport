-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOCUMENT_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [product_document_mn_PK]) AS RowNum,
		[product_document_mn_PK], [product_FK], [document_FK]
		FROM [dbo].[PRODUCT_DOCUMENT_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PRODUCT_DOCUMENT_MN]
END
