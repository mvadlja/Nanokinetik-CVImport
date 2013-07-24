-- GetEntity
create PROCEDURE proc_ROLE_ACTION_MN_GetEntity
	@role_action_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[role_action_mn_PK], [role_unique_name], [action_unique_name]
	FROM [dbo].[ROLE_ACTION_MN]
	WHERE [role_action_mn_PK] = @role_action_mn_PK
END