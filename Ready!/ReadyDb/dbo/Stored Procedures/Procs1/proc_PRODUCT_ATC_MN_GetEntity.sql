-- GetEntity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ATC_MN_GetEntity]
	@product_atc_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_atc_mn_PK], [product_FK], [atc_FK]
	FROM [dbo].[PRODUCT_ATC_MN]
	WHERE [product_atc_mn_PK] = @product_atc_mn_PK
END
