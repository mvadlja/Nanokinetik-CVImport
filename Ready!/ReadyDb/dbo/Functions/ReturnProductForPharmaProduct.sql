CREATE FUNCTION [dbo].[ReturnProductForPharmaProduct]

(
	@PharmaUnit_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @product nvarchar(max)

	SELECT @product = COALESCE(@product + ', ', '') +
			isnull(rtrim(ltrim(
			CASE 
				WHEN (product_number='' or product_number is null) THEN pro.name
				ELSE pro.name +' ('+pro.product_number+')'
				
			END)) , '')
			from PRODUCT as pro
			JOIN PRODUCT_PP_MN as ppMN on ppMN.product_FK=pro.product_PK
			where ppMN.pp_FK=@PharmaUnit_PK
	ORDER BY pro.name
	
    RETURN @product
    
  END
