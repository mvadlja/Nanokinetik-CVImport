-- GetEntities
CREATE PROCEDURE  [dbo].[proc_USER_GRID_SETTINGS_GetDefaultLayoutByUsernameAndGrid]
	@username nvarchar(50) = NULL,
	@grid_ID nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT top 1
	[user_grid_settings_PK], [user_FK], [grid_layout], [isdefault], [timestamp], [ql_visible], [grid_ID], [display_name]
	FROM [dbo].[USER_GRID_SETTINGS]
	left join [dbo].[USER] on [dbo].[USER].user_PK = [dbo].[USER_GRID_SETTINGS].user_FK
	where	[dbo].[USER].username like @username and
			[dbo].[USER_GRID_SETTINGS].grid_ID like @grid_ID and
			[dbo].[USER_GRID_SETTINGS].isdefault = 1
	order by [dbo].[USER_GRID_SETTINGS].[timestamp] DESC
END
