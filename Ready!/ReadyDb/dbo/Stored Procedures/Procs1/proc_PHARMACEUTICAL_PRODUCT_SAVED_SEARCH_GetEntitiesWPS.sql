-- GetEntitiesWPS
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_GetEntitiesWPS]
	@PageNum int = 1,
	@PageSize int = 20,
	@OrderByQuery nvarchar(1000) = 'pharmaceutical_products_PK'
AS

DECLARE @Query nvarchar(MAX);

BEGIN
	SET NOCOUNT ON;

	SET @Query = '
	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY ' + @OrderByQuery + ') AS RowNum,
		[pharmaceutical_products_PK], [name], 
		[responsible_user_FK], [description], [product_FK],
		 [Pharmform_FK], [comments], [displayName], [user_FK],
		  [gridLayout], [isPublic],[administrationRoutes],
		  [activeIngridients],[excipients],[adjuvants],[medical_devices],pp_FK
		FROM [dbo].[PHARMACEUTICAL_PRODUCT_SAVED_SEARCH]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (' + CONVERT (nvarchar(10), @PageNum) + ' - 1) * ' + CONVERT (nvarchar(10), @PageSize) + ' + 1 AND ' + CONVERT (nvarchar(10), @PageNum) + ' * ' + CONVERT (nvarchar(10), @PageSize)

	EXECUTE sp_executesql @Query;

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PHARMACEUTICAL_PRODUCT_SAVED_SEARCH]
END
