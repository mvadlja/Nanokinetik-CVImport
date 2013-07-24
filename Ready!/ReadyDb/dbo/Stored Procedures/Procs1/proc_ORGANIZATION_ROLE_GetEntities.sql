-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_ROLE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[role_org_PK], [role_number], [role_name]
	FROM [dbo].[ORGANIZATION_ROLE]
END
