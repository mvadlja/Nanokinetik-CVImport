-- GetEntity
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_ROLE_GetEntity]
	@role_org_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[role_org_PK], [role_number], [role_name]
	FROM [dbo].[ORGANIZATION_ROLE]
	WHERE [role_org_PK] = @role_org_PK
END
