
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_GENE_ELEMENT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [gene_element_PK]) AS RowNum,
		[gene_element_PK], [ge_type], [ge_id], [ge_name]
		FROM [dbo].[GENE_ELEMENT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[GENE_ELEMENT]
END
