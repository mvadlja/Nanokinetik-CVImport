-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_ROLE_GetAvailableEntitiesByOrganization]
	@OrganizationPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT r.*
	FROM dbo.ORGANIZATION_ROLE r
	WHERE r.role_org_PK NOT IN 
	(
		SELECT orMn.role_org_FK
		FROM [dbo].[ORGANIZATION_IN_ROLE] orMn
		WHERE orMn.organization_FK = @OrganizationPk AND @OrganizationPk IS NOT NULL
	)
END