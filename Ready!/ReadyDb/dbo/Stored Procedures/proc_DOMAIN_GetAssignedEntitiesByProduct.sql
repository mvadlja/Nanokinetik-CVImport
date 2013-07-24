-- GetEntities
CREATE PROCEDURE  [dbo].[proc_DOMAIN_GetAssignedEntitiesByProduct]
	@ProductPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
	[dbo].[DOMAIN].domain_PK, [dbo].[DOMAIN].name
	FROM [dbo].[DOMAIN]
	JOIN [dbo].[PRODUCT_DOMAIN_MN] ON [dbo].[PRODUCT_DOMAIN_MN].domain_FK = [dbo].[DOMAIN].domain_PK
	WHERE [dbo].[PRODUCT_DOMAIN_MN].product_FK = @ProductPk AND @ProductPk IS NOT NULL

END