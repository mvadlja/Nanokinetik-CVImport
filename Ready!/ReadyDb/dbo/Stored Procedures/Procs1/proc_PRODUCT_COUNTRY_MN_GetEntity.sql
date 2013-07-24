-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_COUNTRY_MN_GetEntity]
	@product_country_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_country_mn_PK], [country_FK], [product_FK]
	FROM [dbo].[PRODUCT_COUNTRY_MN]
	WHERE [product_country_mn_PK] = @product_country_mn_PK
END
