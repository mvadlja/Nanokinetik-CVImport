-- GetEntityByUniqueName
create PROCEDURE proc_LOCATION_GetEntityByUniqueName
	@uniqueName nvarchar(MAX) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[location_PK], [unique_name], [display_name], [description], [navigation_level], [generate_in_top_menu], [generate_in_tab_menu], [active], [parent_unique_name], [location_target], [full_unique_path], [location_url], [old_location], [menu_order]
	FROM [dbo].[LOCATION]
	WHERE [unique_name] = @uniqueName
END