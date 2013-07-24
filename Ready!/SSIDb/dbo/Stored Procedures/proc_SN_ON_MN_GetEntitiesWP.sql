
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_SN_ON_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [sn_on_mn_PK]) AS RowNum,
		[sn_on_mn_PK], [official_name_FK], [substance_name_FK]
		FROM [dbo].[SN_ON_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[SN_ON_MN]
END
