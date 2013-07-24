
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_STRUCTURE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [structure_PK]) AS RowNum,
		[structure_PK], [struct_representation], [struct_repres_attach_FK], [optical_activity], [molecular_formula]
		FROM [dbo].[STRUCTURE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[STRUCTURE]
END
