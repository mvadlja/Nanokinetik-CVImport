-- Delete
CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOMAIN_MN_Delete]
	@product_domain_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[PRODUCT_DOMAIN_MN] WHERE [product_domain_mn_PK] = @product_domain_mn_PK
END
