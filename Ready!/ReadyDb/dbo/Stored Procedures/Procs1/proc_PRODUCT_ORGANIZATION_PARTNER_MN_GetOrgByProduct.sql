-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ORGANIZATION_PARTNER_MN_GetOrgByProduct]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT par.product_organization_partner_mn_PK, p.product_PK, o.organization_PK, o.name_org
	FROM [dbo].[PRODUCT_ORGANIZATION_PARTNER_MN] as par
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = par.product_FK
	LEFT JOIN [dbo].[ORGANIZATION] o ON o.organization_PK = par.organization_FK
	WHERE (par.product_FK = @Product_FK OR @Product_FK IS NULL)

END
