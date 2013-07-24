-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_DOMAIN_MN_GetDomainByProduct]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.product_domain_mn_PK, p.product_PK, d.domain_PK, d.name
	FROM [dbo].[PRODUCT_DOMAIN_MN] as mn
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	LEFT JOIN [dbo].[DOMAIN] d ON d.domain_PK = mn.domain_FK
	WHERE (mn.product_FK = @Product_FK OR @Product_FK IS NULL)
	
END
