-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_MEDDRA_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [meddra_pk]) AS RowNum,
		[meddra_pk], [version_type_FK], [level_type_FK], [code], [term]
		FROM [dbo].[MEDDRA]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MEDDRA]
END
