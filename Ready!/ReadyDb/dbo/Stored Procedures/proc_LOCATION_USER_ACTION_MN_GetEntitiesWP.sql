-- GetEntitiesWP
create PROCEDURE proc_LOCATION_USER_ACTION_MN_GetEntitiesWP
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [location_user_action_mn_PK]) AS RowNum,
		[location_user_action_mn_PK], [location_FK], [user_action_FK]
		FROM [dbo].[LOCATION_USER_ACTION_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[LOCATION_USER_ACTION_MN]
END