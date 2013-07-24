-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOMAIN_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_domain_mn_PK], [product_FK], [domain_FK]
	FROM [dbo].[PRODUCT_DOMAIN_MN]
END
