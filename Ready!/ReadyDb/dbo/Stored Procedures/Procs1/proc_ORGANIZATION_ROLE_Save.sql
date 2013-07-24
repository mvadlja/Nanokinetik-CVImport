-- Save
CREATE PROCEDURE  [dbo].[proc_ORGANIZATION_ROLE_Save]
	@role_org_PK int = NULL,
	@role_number nvarchar(15) = NULL,
	@role_name nvarchar(30) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ORGANIZATION_ROLE]
	SET
	[role_number] = @role_number,
	[role_name] = @role_name
	WHERE [role_org_PK] = @role_org_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ORGANIZATION_ROLE]
		([role_number], [role_name])
		VALUES
		(@role_number, @role_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
