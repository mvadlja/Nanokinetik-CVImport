-- Delete
create PROCEDURE proc_USER_ACTION_Delete
	@user_action_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[USER_ACTION] WHERE [user_action_PK] = @user_action_PK
END