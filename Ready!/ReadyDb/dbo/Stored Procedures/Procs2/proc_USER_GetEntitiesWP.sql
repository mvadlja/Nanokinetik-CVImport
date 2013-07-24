-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_USER_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [user_PK]) AS RowNum,
		[user_PK], [person_FK], [username], [password], [user_start_date], [user_end_date], [country_FK], [description], [email], [active]
		FROM [dbo].[USER]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[USER]
END
