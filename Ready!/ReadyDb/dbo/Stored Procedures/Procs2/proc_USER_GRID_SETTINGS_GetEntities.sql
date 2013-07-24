-- GetEntities
CREATE PROCEDURE  [dbo].[proc_USER_GRID_SETTINGS_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[user_grid_settings_PK], [user_FK], [grid_layout], [isdefault], [timestamp], [ql_visible], [grid_ID]
	FROM [dbo].[USER_GRID_SETTINGS]
END
