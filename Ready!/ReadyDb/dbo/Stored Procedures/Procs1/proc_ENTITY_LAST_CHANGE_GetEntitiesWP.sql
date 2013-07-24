-- GetEntitiesWP
CREATE PROCEDURE  proc_ENTITY_LAST_CHANGE_GetEntitiesWP
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [last_change_PK]) AS RowNum,
		[last_change_PK], [change_table], [change_date], [user_FK], [entity_FK]
		FROM [dbo].[ENTITY_LAST_CHANGE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ENTITY_LAST_CHANGE]
END
