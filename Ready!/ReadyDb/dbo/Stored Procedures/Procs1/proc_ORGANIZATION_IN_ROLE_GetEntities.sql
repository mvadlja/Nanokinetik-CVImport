-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_IN_ROLE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[organization_in_role_ID], [organization_FK], [role_org_FK]
	FROM [dbo].[ORGANIZATION_IN_ROLE]
END
