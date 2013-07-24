-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [pharmaceutical_products_PK]) AS RowNum,
		[pharmaceutical_products_PK], [name], 
		[responsible_user_FK], [description],
		 [product_FK], [Pharmform_FK], [comments],
		  [displayName], [user_FK], [gridLayout], [isPublic],pp_FK
		FROM [dbo].[PHARMACEUTICAL_PRODUCT_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PHARMACEUTICAL_PRODUCT_SAVED_SEARCH]
END
