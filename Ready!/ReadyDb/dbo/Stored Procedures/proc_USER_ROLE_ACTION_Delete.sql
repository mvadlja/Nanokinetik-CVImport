-- Delete
create PROCEDURE proc_USER_ROLE_ACTION_Delete
	@user_role_action_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[USER_ROLE_ACTION] WHERE [user_role_action_PK] = @user_role_action_PK
END