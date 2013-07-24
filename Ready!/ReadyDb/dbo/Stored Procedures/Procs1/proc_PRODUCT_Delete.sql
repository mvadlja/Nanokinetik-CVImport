-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_Delete]
	@product_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT] WHERE [product_PK] = @product_PK
END
