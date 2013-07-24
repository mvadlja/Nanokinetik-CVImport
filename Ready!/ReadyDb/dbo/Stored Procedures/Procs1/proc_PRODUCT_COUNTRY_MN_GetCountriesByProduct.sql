-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_COUNTRY_MN_GetCountriesByProduct]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT c.abbreviation		
	FROM dbo.COUNTRY c
	LEFT JOIN dbo.PRODUCT_COUNTRY_MN pc
	ON pc.country_FK = c.country_PK
	where pc.product_FK = @Product_FK
	ORDER BY c.name

END
