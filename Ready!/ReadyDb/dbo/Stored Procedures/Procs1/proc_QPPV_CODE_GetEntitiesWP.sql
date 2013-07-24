-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_QPPV_CODE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [qppv_code_PK]) AS RowNum,
		qppv_code_PK, person_FK, qppv_code
		FROM [dbo].[QPPV_CODE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[QPPV_CODE]
END
