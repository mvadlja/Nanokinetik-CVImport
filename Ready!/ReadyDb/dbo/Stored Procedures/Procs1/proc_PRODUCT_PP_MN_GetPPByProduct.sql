-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_PP_MN_GetPPByProduct]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT mn.product_mn_PK, p.product_PK, pp.name, pp.pharmaceutical_product_PK
	FROM [dbo].[PRODUCT_PP_MN] as mn
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	LEFT JOIN [dbo].[PHARMACEUTICAL_PRODUCT] pp ON pp.pharmaceutical_product_PK = mn.pp_FK
	WHERE (mn.product_FK = @Product_FK OR @Product_FK IS NULL)

END
