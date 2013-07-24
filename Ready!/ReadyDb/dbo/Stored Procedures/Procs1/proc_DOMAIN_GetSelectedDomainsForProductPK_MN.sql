-- GetEntities
CREATE PROCEDURE  [dbo].[proc_DOMAIN_GetSelectedDomainsForProductPK_MN]
	@product_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[DOMAIN].domain_PK, [dbo].[DOMAIN].name
	FROM [dbo].[DOMAIN]
	WHERE
	domain_PK
	IN
	(
		select dbo.PRODUCT_DOMAIN_MN.domain_FK from dbo.PRODUCT_DOMAIN_MN where product_FK = @product_PK
	)
END
