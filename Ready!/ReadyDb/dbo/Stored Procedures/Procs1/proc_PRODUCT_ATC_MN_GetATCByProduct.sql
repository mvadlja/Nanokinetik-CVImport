-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ATC_MN_GetATCByProduct]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.product_atc_mn_PK, p.product_PK, a.atc_PK, a.atccode, a.name
	FROM [dbo].[PRODUCT_ATC_MN] as mn
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	LEFT JOIN [dbo].[ATC] a ON a.atc_PK = mn.atc_FK
	WHERE (mn.product_FK = @Product_FK OR @Product_FK IS NULL)

END
