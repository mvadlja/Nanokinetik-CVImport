-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_COUNTRY_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_country_mn_PK], [country_FK], [product_FK]
	FROM [dbo].[PRODUCT_COUNTRY_MN]
END
