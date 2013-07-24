
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_RI_SCLF_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ri_sclf_mn_PK]) AS RowNum,
		[ri_sclf_mn_PK], [ref_info_FK], [sclf_FK]
		FROM [dbo].[RI_SCLF_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[RI_SCLF_MN]
END
