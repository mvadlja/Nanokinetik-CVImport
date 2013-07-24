-- Save
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_IN_ROLE_Save]
	@organization_in_role_ID int = NULL,
	@organization_FK int = NULL,
	@role_org_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ORGANIZATION_IN_ROLE]
	SET
	[organization_FK] = @organization_FK,
	[role_org_FK] = @role_org_FK
	WHERE [organization_in_role_ID] = @organization_in_role_ID

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ORGANIZATION_IN_ROLE]
		([organization_FK], [role_org_FK])
		VALUES
		(@organization_FK, @role_org_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
