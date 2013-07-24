-- SearchUsers
CREATE PROCEDURE  [dbo].[proc_USER_SearchUsers]
	@UserName nvarchar(50) = NULL,
	@CountryID int = NULL,
	@RoleID int = NULL,
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [user_PK]) AS RowNum,
		DistinctUsers.* FROM
		(
			SELECT DISTINCT
			[dbo].[USER].[user_PK], [dbo].[USER].[username], [dbo].[USER].[password], [dbo].[USER].[country_FK], [dbo].[USER].[description], [dbo].[USER].[email], [dbo].[USER].[active],[USER].isAdUser, [USER].adDomain
			FROM [dbo].[USER]
			LEFT JOIN [dbo].[USER_IN_ROLE] ON [USER].user_PK = [USER_IN_ROLE].user_FK 
			WHERE ([dbo].[USER].[username] LIKE '%' + @UserName + '%' OR @UserName IS NULL)
			AND ([dbo].[USER].[country_FK] = @CountryID OR @CountryID IS NULL)
			AND ([dbo].[USER_IN_ROLE].[user_in_role_PK] = @RoleID OR @RoleID IS NULL)
		) DistinctUsers
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(DISTINCT [dbo].[USER].[user_PK]) FROM [dbo].[USER]
	LEFT JOIN [dbo].[USER_IN_ROLE] ON [USER].user_PK = [USER_IN_ROLE].user_FK 
	WHERE ([dbo].[USER].[username] LIKE '%' + @UserName + '%' OR @UserName IS NULL)
	AND ([dbo].[USER].[country_FK] = @CountryID OR @CountryID IS NULL)
	AND ([dbo].[USER_IN_ROLE].[user_in_role_PK] = @RoleID OR @RoleID IS NULL)

END
