-- Save
create PROCEDURE proc_ROLE_ACTION_MN_Save
	@role_action_mn_PK int = NULL,
	@role_unique_name nvarchar(450) = NULL,
	@action_unique_name nvarchar(450) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ROLE_ACTION_MN]
	SET
	[role_unique_name] = @role_unique_name,
	[action_unique_name] = @action_unique_name
	WHERE [role_action_mn_PK] = @role_action_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ROLE_ACTION_MN]
		([role_unique_name], [action_unique_name])
		VALUES
		(@role_unique_name, @action_unique_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END