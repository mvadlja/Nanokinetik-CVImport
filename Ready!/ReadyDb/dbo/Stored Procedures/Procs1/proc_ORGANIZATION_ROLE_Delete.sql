-- Delete
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_ROLE_Delete]
	@role_org_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ORGANIZATION_ROLE] WHERE [role_org_PK] = @role_org_PK
END
