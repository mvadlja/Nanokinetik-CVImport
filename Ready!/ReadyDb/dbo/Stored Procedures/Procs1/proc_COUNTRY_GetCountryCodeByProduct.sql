-- GetRolesByUserID
CREATE PROCEDURE  [dbo].[proc_COUNTRY_GetCountryCodeByProduct]
	@product_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[COUNTRY].[country_PK], [dbo].[COUNTRY].[name], [dbo].[COUNTRY].[abbreviation], [dbo].[COUNTRY].[region], [dbo].[COUNTRY].[code]
	FROM [dbo].[COUNTRY]
	LEFT JOIN [dbo].[PRODUCT_COUNTRY_MN] ON COUNTRY.country_PK = PRODUCT_COUNTRY_MN.country_FK 
	WHERE [dbo].[PRODUCT_COUNTRY_MN].[product_FK] = @product_PK

END
