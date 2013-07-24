-- GetEntities
CREATE PROCEDURE  [dbo].[proc_DOMAIN_GetAvailableEntitiesByProduct]
	@ProductPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (@ProductPk IS NULL)

	SELECT 
	[dbo].[DOMAIN].domain_PK, [dbo].[DOMAIN].name
	FROM [dbo].[DOMAIN]

	ELSE

	SELECT [dbo].[DOMAIN].domain_PK, [dbo].[DOMAIN].name
	FROM [dbo].[DOMAIN]
	WHERE [dbo].[DOMAIN].domain_PK NOT IN
	(
		SELECT 
		dbo.PRODUCT_DOMAIN_MN.domain_FK 
		FROM dbo.PRODUCT_DOMAIN_MN 
		WHERE product_FK = @ProductPk AND dbo.PRODUCT_DOMAIN_MN.domain_FK IS NOT NULL
	)
END