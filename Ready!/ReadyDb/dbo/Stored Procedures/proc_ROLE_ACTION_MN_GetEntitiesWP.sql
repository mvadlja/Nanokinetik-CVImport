-- GetEntitiesWP
create PROCEDURE proc_ROLE_ACTION_MN_GetEntitiesWP
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [role_action_mn_PK]) AS RowNum,
		[role_action_mn_PK], [role_unique_name], [action_unique_name]
		FROM [dbo].[ROLE_ACTION_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ROLE_ACTION_MN]
END