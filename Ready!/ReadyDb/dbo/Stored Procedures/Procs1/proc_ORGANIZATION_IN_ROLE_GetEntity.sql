-- GetEntity
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_IN_ROLE_GetEntity]
	@organization_in_role_ID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[organization_in_role_ID], [organization_FK], [role_org_FK]
	FROM [dbo].[ORGANIZATION_IN_ROLE]
	WHERE [organization_in_role_ID] = @organization_in_role_ID
END
