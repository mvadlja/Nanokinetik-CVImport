-- Save
CREATE PROCEDURE  [dbo].[proc_USER_GRID_SETTINGS_Save]
	@user_grid_settings_PK int = NULL,
	@user_FK int = NULL,
	@grid_layout nvarchar(MAX) = NULL,
	@isdefault bit = NULL,
	@timestamp datetime = NULL,
	@ql_visible bit = NULL,
	@grid_ID nvarchar(100) = NULL,
	@display_name nvarchar(100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[USER_GRID_SETTINGS]
	SET
	[user_FK] = @user_FK,
	[grid_layout] = @grid_layout,
	[isdefault] = @isdefault,
	[timestamp] = @timestamp,
	[ql_visible] = @ql_visible,
	[grid_ID] = @grid_ID,
	[display_name] = @display_name
	WHERE [user_grid_settings_PK] = @user_grid_settings_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[USER_GRID_SETTINGS]
		([user_FK], [grid_layout], [isdefault], [timestamp], [ql_visible], [grid_ID], [display_name])
		VALUES
		(@user_FK, @grid_layout, @isdefault, @timestamp, @ql_visible, @grid_ID,@display_name)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
