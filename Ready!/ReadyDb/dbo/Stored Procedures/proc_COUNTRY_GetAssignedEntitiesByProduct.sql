-- GetEntities
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetAssignedEntitiesByProduct]
	@ProductPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
	JOIN [dbo].[PRODUCT_COUNTRY_MN] ON [dbo].[PRODUCT_COUNTRY_MN].country_FK = [dbo].[COUNTRY].country_PK
	WHERE [dbo].[PRODUCT_COUNTRY_MN].product_FK = @ProductPk AND @ProductPk IS NOT NULL
	ORDER BY [dbo].[COUNTRY].[custom_sort_ID] ASC
END