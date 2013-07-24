-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_COUNTRY_MN_GetCountriesListByProduct]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [product_country_mn_PK], [country_FK], [product_FK]
	FROM [dbo].[PRODUCT_COUNTRY_MN]
	where [dbo].[PRODUCT_COUNTRY_MN].product_FK = @Product_FK

END
