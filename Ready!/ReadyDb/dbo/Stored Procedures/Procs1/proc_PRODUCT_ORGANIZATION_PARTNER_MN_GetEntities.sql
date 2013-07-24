-- GetEntities
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ORGANIZATION_PARTNER_MN_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[product_organization_partner_mn_PK], [organization_FK], [product_FK]
	FROM [dbo].[PRODUCT_ORGANIZATION_PARTNER_MN]
END
