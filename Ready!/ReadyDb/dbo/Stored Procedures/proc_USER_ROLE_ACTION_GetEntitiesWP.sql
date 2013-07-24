-- GetEntitiesWP
create PROCEDURE proc_USER_ROLE_ACTION_GetEntitiesWP
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [user_role_action_PK]) AS RowNum,
		[user_role_action_PK], [user_role_FK], [location_FK], [user_action_FK]
		FROM [dbo].[USER_ROLE_ACTION]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[USER_ROLE_ACTION]
END