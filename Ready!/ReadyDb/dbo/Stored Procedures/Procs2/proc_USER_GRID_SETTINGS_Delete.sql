-- Delete
CREATE PROCEDURE  [dbo].[proc_USER_GRID_SETTINGS_Delete]
	@user_grid_settings_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[USER_GRID_SETTINGS] WHERE [user_grid_settings_PK] = @user_grid_settings_PK
END
