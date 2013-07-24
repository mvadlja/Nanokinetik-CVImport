-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_USER_GRID_SETTINGS_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [user_grid_settings_PK]) AS RowNum,
		[user_grid_settings_PK], [user_FK], [grid_layout], [isdefault], [timestamp], [ql_visible], [grid_ID], [display_name]
		FROM [dbo].[USER_GRID_SETTINGS]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[USER_GRID_SETTINGS]
END
