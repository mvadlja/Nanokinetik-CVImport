
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_RI_GE_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ri_ge_mn_PK]) AS RowNum,
		[ri_ge_mn_PK], [ri_FK], [ge_FK]
		FROM [dbo].[RI_GE_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[RI_GE_MN]
END
