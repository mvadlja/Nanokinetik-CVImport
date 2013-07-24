-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_INTERNATIONAL_CODE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [international_code_PK]) AS RowNum,
		[international_code_PK], [sourcecode], [resolutionmode_sources], [referencetext], [resolutionmode_substance]
		FROM [dbo].[INTERNATIONAL_CODE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[INTERNATIONAL_CODE]
END
