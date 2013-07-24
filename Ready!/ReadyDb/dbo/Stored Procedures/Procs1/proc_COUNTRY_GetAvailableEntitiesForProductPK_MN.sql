-- GetEntities
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetAvailableEntitiesForProductPK_MN]
	@product_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[country_PK], [name], [abbreviation], [region], [code], [custom_sort_ID]
	FROM [dbo].[COUNTRY]
	WHERE
	country_PK 
	NOT IN
	(
		select country_FK from dbo.PRODUCT_COUNTRY_MN 
			where product_FK = @product_PK AND country_FK is not null
	)
	ORDER BY [custom_sort_ID] ASC
END
