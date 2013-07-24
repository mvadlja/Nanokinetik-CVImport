-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_USER_ROLE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [user_role_PK]) AS RowNum,
		[user_role_PK], [name], [display_name], [description], [active], [row_version]
		FROM [dbo].[USER_ROLE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[USER_ROLE]
END
