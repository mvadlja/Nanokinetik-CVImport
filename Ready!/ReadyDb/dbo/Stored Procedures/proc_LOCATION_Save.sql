-- Save
create PROCEDURE proc_LOCATION_Save
	@location_PK int = NULL,
	@unique_name nvarchar(450) = NULL,
	@display_name nvarchar(1000) = NULL,
	@description nvarchar(MAX) = NULL,
	@navigation_level int = NULL,
	@generate_in_top_menu bit = NULL,
	@generate_in_tab_menu bit = NULL,
	@active bit = NULL,
	@parent_unique_name nvarchar(450) = NULL,
	@location_target nvarchar(50) = NULL,
	@full_unique_path nvarchar(500) = NULL,
	@location_url nvarchar(500) = NULL,
	@old_location bit = NULL,
	@menu_order int = NULL,
	@show_location bit = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[LOCATION]
	SET
	[unique_name] = @unique_name,
	[display_name] = @display_name,
	[description] = @description,
	[navigation_level] = @navigation_level,
	[generate_in_top_menu] = @generate_in_top_menu,
	[generate_in_tab_menu] = @generate_in_tab_menu,
	[active] = @active,
	[parent_unique_name] = @parent_unique_name,
	[location_target] = @location_target,
	[full_unique_path] = @full_unique_path,
	[location_url] = @location_url,
	[old_location] = @old_location,
	[menu_order] = @menu_order,
	[show_location] = @show_location
	WHERE [location_PK] = @location_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[LOCATION]
		([unique_name], [display_name], [description], [navigation_level], [generate_in_top_menu], [generate_in_tab_menu], [active], [parent_unique_name], [location_target], [full_unique_path], [location_url], [old_location], [menu_order], [show_location])
		VALUES
		(@unique_name, @display_name, @description, @navigation_level, @generate_in_top_menu, @generate_in_tab_menu, @active, @parent_unique_name, @location_target, @full_unique_path, @location_url, @old_location, @menu_order, @show_location)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END