-- GetEntity
create PROCEDURE proc_USER_ACTION_GetEntity
	@user_action_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[user_action_PK], [unique_name], [display_name], [description], [active]
	FROM [dbo].[USER_ACTION]
	WHERE [user_action_PK] = @user_action_PK
END