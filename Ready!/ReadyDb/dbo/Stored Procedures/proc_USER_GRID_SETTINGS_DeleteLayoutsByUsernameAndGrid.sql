
CREATE PROCEDURE [dbo].[proc_USER_GRID_SETTINGS_DeleteLayoutsByUsernameAndGrid]
	@username nvarchar(50) = NULL,
	@grid_ID nvarchar(100) = NULL	
AS
BEGIN

	SET NOCOUNT ON;

	DELETE [dbo].[USER_GRID_SETTINGS]
	FROM [dbo].[USER_GRID_SETTINGS]
	left join [dbo].[USER] on [dbo].[USER].user_PK = [dbo].[USER_GRID_SETTINGS].user_FK
	where	[dbo].[USER].username = @username and
			[dbo].[USER_GRID_SETTINGS].grid_ID = @grid_ID
	
END