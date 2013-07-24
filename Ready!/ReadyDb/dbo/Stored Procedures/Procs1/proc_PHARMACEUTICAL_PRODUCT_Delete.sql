-- Delete
CREATE PROCEDURE  [dbo].[proc_PHARMACEUTICAL_PRODUCT_Delete]
	@pharmaceutical_product_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PHARMACEUTICAL_PRODUCT] WHERE [pharmaceutical_product_PK] = @pharmaceutical_product_PK
END
