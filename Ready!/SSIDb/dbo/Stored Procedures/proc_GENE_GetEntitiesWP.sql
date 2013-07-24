
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_GENE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [gene_PK]) AS RowNum,
		[gene_PK], [gene_sequence_origin], [gene_id], [gene_name]
		FROM [dbo].[GENE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[GENE]
END
