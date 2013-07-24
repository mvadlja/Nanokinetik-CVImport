CREATE FUNCTION [dbo].[ReturnProductIngrStrForm]

(
	@product_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @IngrStrForm nvarchar(max)
	declare @PharmForm nvarchar(max)
	
	SELECT @IngrStrForm = COALESCE(@IngrStrForm + '+', '') +
			isnull(rtrim(ltrim(
		CASE
			WHEN ppai.concise IS NULL OR ppai.concise = '' THEN ss.substance_name
			ELSE ppai.concise
		END
     )), ''), @PharmForm = 
		case
			when pf.name is null or pf.name = '' then ' (N/A)'
			else ' ('+pf.name+')'
		end
			from dbo.PP_ACTIVE_INGREDIENT ppai
			left join dbo.SUBSTANCES ss on ppai.substancecode_FK=ss.substance_PK
			LEFT JOIN [dbo].[PHARMACEUTICAL_PRODUCT] pp ON pp.pharmaceutical_product_PK = ppai.pp_FK
			LEFT JOIN [dbo].PHARMACEUTICAL_FORM pf ON pf.pharmaceutical_form_PK = pp.Pharmform_FK
			left join [dbo].[PRODUCT_PP_MN] ppmn ON pp.pharmaceutical_product_PK = ppmn.pp_FK
			where ppmn.product_FK = @product_PK
	ORDER BY ss.substance_name ASC
	
    RETURN @IngrStrForm + @PharmForm
    
  END
