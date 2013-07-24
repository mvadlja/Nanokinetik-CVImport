-- GetEntitiesWP
create PROCEDURE proc_LOCATION_GetEntitiesWP
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [location_PK]) AS RowNum,
		[location_PK], [unique_name], [display_name], [description], [navigation_level], [generate_in_top_menu], [generate_in_tab_menu], [active], [parent_unique_name], [location_target], [full_unique_path], [location_url], [old_location], [menu_order], [show_location]
		FROM [dbo].[LOCATION]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[LOCATION]
END