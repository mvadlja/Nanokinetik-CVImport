-- GetEntities
CREATE PROCEDURE  proc_COUNTRY_GetAvailableEntitiesByProduct
	@ProductPk INT = NULL
AS
BEGIN
	SET NOCOUNT ON;

	IF (@ProductPk IS NULL)

	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]

	ELSE

	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
	WHERE [dbo].[COUNTRY].country_PK NOT IN
	(	
		SELECT country_FK 
		FROM dbo.PRODUCT_COUNTRY_MN 
		WHERE product_FK = @ProductPk AND country_FK is not null 
	)
	ORDER BY [custom_sort_ID] ASC
END