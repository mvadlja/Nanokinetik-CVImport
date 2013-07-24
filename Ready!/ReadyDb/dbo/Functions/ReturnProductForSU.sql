CREATE FUNCTION [dbo].[ReturnProductForSU]

(
	@sUnit int
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
			JOIN PRODUCT_SUB_UNIT_MN as suMN on suMN.product_FK=pro.product_PK
			where submission_unit_FK= @sUnit
	ORDER BY pro.name
	
    RETURN @product
    
  END
