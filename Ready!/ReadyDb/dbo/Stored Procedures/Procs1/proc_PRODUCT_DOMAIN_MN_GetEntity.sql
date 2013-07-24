-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOMAIN_MN_GetEntity]
	@product_domain_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_domain_mn_PK], [product_FK], [domain_FK]
	FROM [dbo].[PRODUCT_DOMAIN_MN]
	WHERE [product_domain_mn_PK] = @product_domain_mn_PK
END
