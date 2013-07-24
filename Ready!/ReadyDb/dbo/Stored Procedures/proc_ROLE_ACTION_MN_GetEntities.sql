-- GetEntities
create PROCEDURE proc_ROLE_ACTION_MN_GetEntities
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[role_action_mn_PK], [role_unique_name], [action_unique_name]
	FROM [dbo].[ROLE_ACTION_MN]
END