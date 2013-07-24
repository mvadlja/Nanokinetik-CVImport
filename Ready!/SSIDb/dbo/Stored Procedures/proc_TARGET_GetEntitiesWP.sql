
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_TARGET_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [target_PK]) AS RowNum,
		[target_PK], [target_gene_id], [target_gene_name], [interaction_type], [target_organism_type], [target_type]
		FROM [dbo].[TARGET]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[TARGET]
END
