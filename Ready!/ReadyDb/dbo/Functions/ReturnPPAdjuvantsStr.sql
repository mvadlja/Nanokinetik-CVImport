CREATE FUNCTION [dbo].[ReturnPPAdjuvantsStr]

(
	@pp_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @AdjuvantsStr nvarchar(max)

	SELECT @AdjuvantsStr = COALESCE(@AdjuvantsStr + ', ', '') +
			isnull(rtrim(ltrim(
		CASE
			WHEN ppadj.concise IS NULL OR ppadj.concise = '' THEN ss.substance_name
		--	ELSE ss.substance_name +' ' +ppadj.concise
			ELSE ppadj.concise
		END
     )), '')     
			from dbo.PP_ADJUVANT ppadj
			left join dbo.SUBSTANCES ss on ppadj.substancecode_FK=ss.substance_PK
			LEFT JOIN [dbo].[PHARMACEUTICAL_PRODUCT] pp ON pp.pharmaceutical_product_PK = ppadj.pp_FK
			where pp.pharmaceutical_product_PK = @pp_PK
	
    RETURN @AdjuvantsStr
    
  END
