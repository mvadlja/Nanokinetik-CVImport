
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_MOIETY_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [moiety_PK]) AS RowNum,
		[moiety_PK], [moiety_role], [moiety_name], [moiety_id], [amount_type], [amount_FK]
		FROM [dbo].[MOIETY]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MOIETY]
END
