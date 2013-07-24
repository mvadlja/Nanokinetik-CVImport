-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_USER_IN_ROLE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [user_in_role_PK]) AS RowNum,
		[user_in_role_PK], [user_FK], [user_role_FK], [row_version]
		FROM [dbo].[USER_IN_ROLE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[USER_IN_ROLE]
END
