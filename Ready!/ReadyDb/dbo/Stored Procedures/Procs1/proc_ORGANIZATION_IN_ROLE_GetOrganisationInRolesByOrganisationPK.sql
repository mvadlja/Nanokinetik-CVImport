-- GetOrganisationInRolesByOrganisationPK
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_IN_ROLE_GetOrganisationInRolesByOrganisationPK]
	@organization_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT [dbo].[ORGANIZATION_IN_ROLE].[organization_in_role_ID], [dbo].[ORGANIZATION_IN_ROLE].[organization_FK], [dbo].[ORGANIZATION_IN_ROLE].[role_org_FK]
	FROM [dbo].[ORGANIZATION_IN_ROLE]
	WHERE ([dbo].[ORGANIZATION_IN_ROLE].[organization_FK] = @organization_FK OR @organization_FK IS NULL)

END
