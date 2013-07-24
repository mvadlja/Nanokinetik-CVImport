-- GetEntity
CREATE PROCEDURE  [dbo].[proc_USER_GRID_SETTINGS_GetEntity]
	@user_grid_settings_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[user_grid_settings_PK], [user_FK], [grid_layout], [isdefault], [timestamp], [ql_visible], [grid_ID], [display_name]
	FROM [dbo].[USER_GRID_SETTINGS]
	WHERE [user_grid_settings_PK] = @user_grid_settings_PK
END
