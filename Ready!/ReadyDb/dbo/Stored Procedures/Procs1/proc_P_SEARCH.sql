CREATE PROCEDURE  [dbo].[proc_P_SEARCH]
AS
BEGIN
	SET NOCOUNT ON;
	
    SELECT 
		P.product_PK
		, P.name
		, P.eu_number --P.authorization_number
		, (SELECT COUNT(ap_PK) FROM dbo.AUTHORISED_PRODUCT WHERE product_FK = product_ID) AS AuthorizedProducts
		, (SELECT abbreviation + ', ' as 'data()'  FROM dbo.COUNTRY AS C WHERE C.country_PK in (SELECT country_FK FROM dbo.PRODUCT_COUNTRY_MN WHERE product_FK = p.product_PK) FOR XML PATH('')) AS Countries
    FROM 
		dbo.PRODUCT AS P
END
