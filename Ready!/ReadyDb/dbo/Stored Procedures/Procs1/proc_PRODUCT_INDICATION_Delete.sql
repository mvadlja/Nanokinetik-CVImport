-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_INDICATION_Delete]
	@product_indications_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_INDICATION] WHERE [product_indications_PK] = @product_indications_PK
END
