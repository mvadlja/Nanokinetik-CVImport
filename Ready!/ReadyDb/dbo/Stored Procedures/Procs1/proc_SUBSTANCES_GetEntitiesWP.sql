-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_SUBSTANCES_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [substance_PK]) AS RowNum,
		[substance_PK], [ev_code], [substance_name], [synonym1], [synonym1_language], [synonym2], [synonym2_language], [synonym3], [synonym3_language], [synonym4], [synonym4_language], [synonym5], [synonym5_language], [synonym6], [synonym6_language], [synonym7], [synonym7_language], [synonym8], [synonym8_language], [synonym9], [synonym9_language], [synonym10], [synonym10_language], [synonym11], [synonym11_language], [synonym12], [synonym12_language], [synonym13], [synonym13_language], [synonym14], [synonym14_language], [synonym15], [synonym15_language], [synonym16], [synonym16_language], [synonym17], [synonym17_language], [synonym18], [synonym18_language], [synonym19], [synonym19_language], [synonym20], [synonym20_language], [synonym21], [synonym21_language]
		FROM [dbo].[SUBSTANCES]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SUBSTANCES]
END
