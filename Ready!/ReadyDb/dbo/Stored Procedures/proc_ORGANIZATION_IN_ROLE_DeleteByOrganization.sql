-- Delete
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_IN_ROLE_DeleteByOrganization]
	@OrganizationPk int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ORGANIZATION_IN_ROLE] WHERE organization_FK = @OrganizationPk AND @OrganizationPk IS NOT NULL
END