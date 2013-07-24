-- Delete
create PROCEDURE proc_ROLE_ACTION_MN_Delete
	@role_action_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ROLE_ACTION_MN] WHERE [role_action_mn_PK] = @role_action_mn_PK
END