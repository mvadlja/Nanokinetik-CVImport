CREATE FUNCTION [dbo].[ReturnProductATCString]

(
	@p_PK int
)

RETURNS NVARCHAR(4000)
AS
  BEGIN
  
	declare @drug_atcs nvarchar(max)

	SELECT @drug_atcs = COALESCE(@drug_atcs + ', ', '') +
			isnull(rtrim(ltrim( at.atccode + '(' + at.name + ')' )), '')     
			from dbo.ATC at
			JOIN [dbo].[PRODUCT_ATC_MN] pa_mn ON pa_mn.atc_FK = at.atc_PK
			JOIN [dbo].[PRODUCT] p on p.product_PK = pa_mn.product_FK
			where p.product_PK = @p_PK
	
    RETURN @drug_atcs
    
  END
