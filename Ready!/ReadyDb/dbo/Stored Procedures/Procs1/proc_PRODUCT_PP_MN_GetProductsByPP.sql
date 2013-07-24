-- GetProductsByActivity
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PP_MN_GetProductsByPP]
	@pp_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT p.product_PK, p.name, p.product_number
	FROM [dbo].PRODUCT_PP_MN mn
	JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	WHERE mn.pp_FK = @pp_PK AND @pp_PK IS NOT NULL

END
