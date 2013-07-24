-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_SAVED_SEARCH_Delete]
	@product_saved_search_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_SAVED_SEARCH] WHERE [product_saved_search_PK] = @product_saved_search_PK
END
