
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_RS_GE_MN_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [rs_ge_mn_PK]) AS RowNum,
		[rs_ge_mn_PK], [rs_FK], [ge_FK]
		FROM [dbo].[RS_GE_MN]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[RS_GE_MN]
END
