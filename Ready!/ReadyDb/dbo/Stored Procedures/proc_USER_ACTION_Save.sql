-- Save
create PROCEDURE proc_USER_ACTION_Save
	@user_action_PK int = NULL,
	@unique_name nvarchar(450) = NULL,
	@display_name nvarchar(1000) = NULL,
	@description nvarchar(MAX) = NULL,
	@active bit = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[USER_ACTION]
	SET
	[unique_name] = @unique_name,
	[display_name] = @display_name,
	[description] = @description,
	[active] = @active
	WHERE [user_action_PK] = @user_action_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[USER_ACTION]
		([unique_name], [display_name], [description], [active])
		VALUES
		(@unique_name, @display_name, @description, @active)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END