
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_SUBSTANCE_SUBSTANCE_CODE_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [substance_substance_code_mn_PK]) AS RowNum,
		[substance_substance_code_mn_PK], [substance_FK], [substance_code_FK]
		FROM [dbo].[SUBSTANCE_SUBSTANCE_CODE_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBSTANCE_SUBSTANCE_CODE_MN]
END
