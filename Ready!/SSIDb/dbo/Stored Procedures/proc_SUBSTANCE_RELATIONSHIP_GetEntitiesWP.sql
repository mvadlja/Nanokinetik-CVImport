
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_RELATIONSHIP_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [substance_relationship_PK]) AS RowNum,
		[substance_relationship_PK], [relationship], [interaction_type], [substance_id], [substance_name], [amount_type], [amount_FK]
		FROM [dbo].[SUBSTANCE_RELATIONSHIP]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBSTANCE_RELATIONSHIP]
END
