-- GetEntities
CREATE PROCEDURE  [dbo].[proc_USER_GRID_SETTINGS_SetDefaultAndKeepFirstNLayouts]
	@username nvarchar(50) = NULL,
	@grid_ID nvarchar(100) = NULL,
	@default_ugs_PK int = NULL,
	@num_to_keep int = null
AS
declare @user_FK int = null;
BEGIN
	SET NOCOUNT ON;
		
		Select @user_FK = [dbo].[USER].user_PK
		from [dbo].[USER]
		where [dbo].[USER].username = @username
		
		delete from [dbo].[USER_GRID_SETTINGS]
		where [dbo].[USER_GRID_SETTINGS].user_grid_settings_PK not in 
			(SELECT TOP (@num_to_keep) [dbo].[USER_GRID_SETTINGS].user_grid_settings_PK
				FROM [dbo].[USER_GRID_SETTINGS]
				where	[dbo].[USER_GRID_SETTINGS].user_FK = @user_FK and
						[dbo].[USER_GRID_SETTINGS].grid_ID = @grid_ID
				order by [dbo].[USER_GRID_SETTINGS].[timestamp] DESC) and
			[dbo].[USER_GRID_SETTINGS].user_FK = @user_FK and
			[dbo].[USER_GRID_SETTINGS].grid_ID = @grid_ID and
			[dbo].[USER_GRID_SETTINGS].user_grid_settings_PK != @default_ugs_PK
		
		update [dbo].[USER_GRID_SETTINGS] set isdefault = 0
		where [dbo].[USER_GRID_SETTINGS].user_FK = @user_FK and
		[dbo].[USER_GRID_SETTINGS].grid_ID = @grid_ID and
		[dbo].[USER_GRID_SETTINGS].user_grid_settings_PK != @default_ugs_PK
		
		update [dbo].[USER_GRID_SETTINGS] set isdefault = 1
		where [dbo].[USER_GRID_SETTINGS].user_FK = @user_FK and
		[dbo].[USER_GRID_SETTINGS].grid_ID = @grid_ID and
		[dbo].[USER_GRID_SETTINGS].user_grid_settings_PK = @default_ugs_PK
		
END
