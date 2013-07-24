
create FUNCTION [dbo].[ReturnProductsByAPDocument]
(
	@document_FK int,
	@ap_FK int
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
			
    FROM [dbo].AP_DOCUMENT_MN mn
	LEFT JOIN [dbo].AUTHORISED_PRODUCT ap ON ap.ap_PK = mn.ap_FK
	LEFT JOIN [dbo].PRODUCT p ON p.product_PK = ap.product_FK
	WHERE 
		(ap.ap_PK = @ap_FK OR @ap_FK IS NULL) AND 
		(mn.document_FK = @document_FK OR @document_FK IS NULL)
    RETURN @Products
    
  END