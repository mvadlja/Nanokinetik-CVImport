CREATE FUNCTION [dbo].[ReturnPPIngrStrForm]

(
	@pp_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @IngrStrForm nvarchar(max)

	SELECT @IngrStrForm = COALESCE(@IngrStrForm + '+', '') +
			isnull(rtrim(ltrim(
		CASE
			WHEN ppai.concise IS NULL OR ppai.concise = '' THEN ss.substance_name
			ELSE ppai.concise
		END
     )), '')     
			from dbo.PP_ACTIVE_INGREDIENT ppai
			left join dbo.SUBSTANCES ss on ppai.substancecode_FK=ss.substance_PK
			LEFT JOIN [dbo].[PHARMACEUTICAL_PRODUCT] pp ON pp.pharmaceutical_product_PK = ppai.pp_FK
			LEFT JOIN [dbo].PHARMACEUTICAL_FORM pf ON pf.pharmaceutical_form_PK = pp.Pharmform_FK
			where pp.pharmaceutical_product_PK = @pp_PK
	
    RETURN @IngrStrForm
    
  END
