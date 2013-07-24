-- GetEntities
CREATE PROCEDURE  [dbo].[proc_DOMAIN_GetAvailableDomainsForProductPK_MN]
	@product_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[DOMAIN].domain_PK, [dbo].[DOMAIN].name
	FROM [dbo].[DOMAIN]
	WHERE
	domain_PK
	NOT IN
	(
		select dbo.PRODUCT_DOMAIN_MN.domain_FK from dbo.PRODUCT_DOMAIN_MN where product_FK=@product_PK AND dbo.PRODUCT_DOMAIN_MN.domain_FK is NOT NULL
	)
END
