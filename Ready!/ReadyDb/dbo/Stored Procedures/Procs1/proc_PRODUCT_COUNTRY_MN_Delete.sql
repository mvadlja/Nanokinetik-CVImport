-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_COUNTRY_MN_Delete]
	@product_country_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_COUNTRY_MN] WHERE [product_country_mn_PK] = @product_country_mn_PK
END
