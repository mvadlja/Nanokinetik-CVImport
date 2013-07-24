
create FUNCTION [dbo].[ReturnProductsByPDocument]
(
	@document_FK int,
	@product_FK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @Products nvarchar(max)

	SELECT @Products = COALESCE(@Products + ', ', '') +
			isnull(rtrim(ltrim(
			CASE 
				WHEN (product_number= '' OR product_number is NULL) THEN p.name
				else p.name+' ('+p.product_number+')'
			END
			)), '')
			
    FROM [dbo].PRODUCT_DOCUMENT_MN mn
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	WHERE 
		(p.product_PK = @product_FK OR @product_FK IS NULL) AND 
		(mn.document_FK = @document_FK OR @document_FK IS NULL)
    RETURN @Products
    
  END