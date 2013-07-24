create FUNCTION [dbo].[ReturnCountriesOfProduct]

(
	@product_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @country_code nvarchar(max)

	SELECT @country_code = COALESCE(@country_code + ', ', '') +
			isnull(rtrim(ltrim(c.name)), '')
			
	FROM dbo.COUNTRY c

	LEFT JOIN dbo.PRODUCT_COUNTRY_MN cmn
	ON c.country_pk = cmn.country_FK
	where cmn.product_FK = @product_PK

	ORDER BY c.name
  
    RETURN @country_code
    
  END
