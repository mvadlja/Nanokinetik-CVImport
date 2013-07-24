-- Delete
create PROCEDURE proc_LOCATION_USER_ACTION_MN_Delete
	@location_user_action_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[LOCATION_USER_ACTION_MN] WHERE [location_user_action_mn_PK] = @location_user_action_mn_PK
END