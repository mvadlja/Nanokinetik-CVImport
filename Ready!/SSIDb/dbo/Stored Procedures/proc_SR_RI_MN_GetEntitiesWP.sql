
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_SR_RI_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [sr_ri_mn_PK]) AS RowNum,
		[sr_ri_mn_PK], [ri_FK], [sr_FK]
		FROM [dbo].[SR_RI_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SR_RI_MN]
END
