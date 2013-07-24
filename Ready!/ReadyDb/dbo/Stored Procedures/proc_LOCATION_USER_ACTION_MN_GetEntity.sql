-- GetEntity
create PROCEDURE proc_LOCATION_USER_ACTION_MN_GetEntity
	@location_user_action_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[location_user_action_mn_PK], [location_FK], [user_action_FK]
	FROM [dbo].[LOCATION_USER_ACTION_MN]
	WHERE [location_user_action_mn_PK] = @location_user_action_mn_PK
END