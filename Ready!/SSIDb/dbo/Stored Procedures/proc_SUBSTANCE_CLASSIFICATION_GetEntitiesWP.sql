
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_CLASSIFICATION_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [subst_clf_PK]) AS RowNum,
		[subst_clf_PK], [domain], [substance_classification], [sclf_type], [sclf_code]
		FROM [dbo].[SUBSTANCE_CLASSIFICATION]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBSTANCE_CLASSIFICATION]
END
