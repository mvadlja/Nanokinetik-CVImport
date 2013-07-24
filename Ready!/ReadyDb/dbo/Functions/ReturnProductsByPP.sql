CREATE FUNCTION [dbo].[ReturnProductsByPP]

(
	@pp_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @Products nvarchar(max)

	SELECT @Products = COALESCE(@Products + ', ', '') +
			isnull(rtrim(ltrim(p.name)), '')

	FROM [dbo].PRODUCT_PP_MN mn
	LEFT JOIN [dbo].[PHARMACEUTICAL_PRODUCT] pp ON pp.pharmaceutical_product_PK = mn.pp_FK
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	WHERE (mn.product_FK = @pp_PK OR @pp_PK IS NULL)
	order by p.name
    RETURN @Products
    
  END
