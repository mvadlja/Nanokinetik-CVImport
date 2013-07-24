-- Save
create PROCEDURE proc_USER_ROLE_ACTION_Save
	@user_role_action_PK int = NULL,
	@user_role_FK int = NULL,
	@location_FK int = NULL,
	@user_action_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[USER_ROLE_ACTION]
	SET
	[user_role_FK] = @user_role_FK,
	[location_FK] = @location_FK,
	[user_action_FK] = @user_action_FK
	WHERE [user_role_action_PK] = @user_role_action_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[USER_ROLE_ACTION]
		([user_role_FK], [location_FK], [user_action_FK])
		VALUES
		(@user_role_FK, @location_FK, @user_action_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END