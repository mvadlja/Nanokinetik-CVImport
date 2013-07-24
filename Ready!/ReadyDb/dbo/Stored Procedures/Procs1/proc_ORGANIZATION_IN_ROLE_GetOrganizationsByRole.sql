-- GetDocumentsByAP
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_IN_ROLE_GetOrganizationsByRole]
	@role_name nvarchar(30) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT o.organization_PK, o.name_org
	FROM [dbo].[ORGANIZATION_IN_ROLE]
	LEFT JOIN [dbo].[ORGANIZATION] o ON o.organization_PK = [dbo].[ORGANIZATION_IN_ROLE].organization_FK
	LEFT JOIN [dbo].[ORGANIZATION_ROLE] r ON r.role_org_PK = [dbo].[ORGANIZATION_IN_ROLE].[role_org_FK]
	WHERE (r.role_name = @role_name OR @role_name IS NULL)

END
