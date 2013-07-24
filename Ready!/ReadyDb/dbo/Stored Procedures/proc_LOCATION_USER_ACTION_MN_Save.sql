-- Save
create PROCEDURE proc_LOCATION_USER_ACTION_MN_Save
	@location_user_action_mn_PK int = NULL,
	@location_FK int = NULL,
	@user_action_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[LOCATION_USER_ACTION_MN]
	SET
	[location_FK] = @location_FK,
	[user_action_FK] = @user_action_FK
	WHERE [location_user_action_mn_PK] = @location_user_action_mn_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[LOCATION_USER_ACTION_MN]
		([location_FK], [user_action_FK])
		VALUES
		(@location_FK, @user_action_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END