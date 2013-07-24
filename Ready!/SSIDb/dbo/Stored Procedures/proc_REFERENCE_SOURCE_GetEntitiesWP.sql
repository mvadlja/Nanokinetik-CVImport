
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_REFERENCE_SOURCE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [reference_source_PK]) AS RowNum,
		[reference_source_PK], [public_domain], [rs_type_FK], [rs_class_FK], [rs_id], [rs_citation]
		FROM [dbo].[REFERENCE_SOURCE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[REFERENCE_SOURCE]
END
