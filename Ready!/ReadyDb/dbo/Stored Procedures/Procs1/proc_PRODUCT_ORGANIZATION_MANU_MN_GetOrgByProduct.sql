-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_PRODUCT_ORGANIZATION_MANU_MN_GetOrgByProduct]
	@Product_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT mn.product_organization_manu_mn_PK, p.product_PK, o.organization_PK, o.name_org
	FROM [dbo].[PRODUCT_ORGANIZATION_MANU_MN] as mn
	LEFT JOIN [dbo].[PRODUCT] p ON p.product_PK = mn.product_FK
	LEFT JOIN [dbo].[ORGANIZATION] o ON o.organization_PK = mn.organization_FK
	WHERE (mn.product_FK = @Product_FK OR @Product_FK IS NULL)

END
