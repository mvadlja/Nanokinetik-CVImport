-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_PERSON_IN_ROLE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [person_in_role_PK]) AS RowNum,
		[person_in_role_PK], [person_FK], [person_role_FK]
		FROM [dbo].[PERSON_IN_ROLE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[PERSON_IN_ROLE]
END
