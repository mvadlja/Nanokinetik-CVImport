
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_ON_ONJ_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [on_onj_mn_PK]) AS RowNum,
		[on_onj_mn_PK], [onj_FK], [on_FK]
		FROM [dbo].[ON_ONJ_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ON_ONJ_MN]
END
