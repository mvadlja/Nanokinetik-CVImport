-- GetEntities
create PROCEDURE proc_USER_ACTION_GetEntities
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[user_action_PK], [unique_name], [display_name], [description], [active]
	FROM [dbo].[USER_ACTION]
END