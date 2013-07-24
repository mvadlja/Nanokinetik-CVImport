-- GetEntitiesWP
create PROCEDURE proc_USER_ACTION_GetEntitiesWP
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [user_action_PK]) AS RowNum,
		[user_action_PK], [unique_name], [display_name], [description], [active]
		FROM [dbo].[USER_ACTION]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[USER_ACTION]
END