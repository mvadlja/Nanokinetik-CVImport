
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_RI_TARGET_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ri_target_mn_PK]) AS RowNum,
		[ri_target_mn_PK], [ri_FK], [target_FK]
		FROM [dbo].[RI_TARGET_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[RI_TARGET_MN]
END
