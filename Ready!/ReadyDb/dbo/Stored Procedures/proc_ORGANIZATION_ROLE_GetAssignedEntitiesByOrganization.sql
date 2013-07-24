-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_ROLE_GetAssignedEntitiesByOrganization]
	@OrganizationPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT r.*
	FROM [dbo].[ORGANIZATION_IN_ROLE] orMn
	JOIN dbo.ORGANIZATION_ROLE r ON r.role_org_PK = orMn.role_org_FK
	WHERE orMn.organization_FK = @OrganizationPk AND @OrganizationPk IS NOT NULL
END