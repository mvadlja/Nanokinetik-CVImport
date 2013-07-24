-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_MEDDRA_AP_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [meddra_ap_mn_PK]) AS RowNum,
		[meddra_ap_mn_PK], [ap_FK], [meddra_FK]
		FROM [dbo].[MEDDRA_AP_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MEDDRA_AP_MN]
END
