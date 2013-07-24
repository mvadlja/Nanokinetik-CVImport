CREATE FUNCTION [dbo].[ReturnProductsByActivity]

(
	@activity_PK int
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
			
	FROM [dbo].[ACTIVITY_PRODUCT_MN] mn
	--LEFT JOIN [dbo].[ACTIVITY] a ON a.activity_PK = mn.activity_FK
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	WHERE (mn.activity_FK = @activity_PK)-- OR @activity_PK IS NULL)
    --order by p.name
    RETURN @Products
    
  END
