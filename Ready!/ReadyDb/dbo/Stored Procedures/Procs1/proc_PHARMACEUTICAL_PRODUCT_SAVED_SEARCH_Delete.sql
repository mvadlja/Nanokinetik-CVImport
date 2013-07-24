-- Delete
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_SAVED_SEARCH_Delete]
	@pharmaceutical_products_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PHARMACEUTICAL_PRODUCT_SAVED_SEARCH] WHERE [pharmaceutical_products_PK] = @pharmaceutical_products_PK
END
