-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_AUDITING_DETAILS_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [auditing_detail_PK]) AS RowNum,
		[auditing_detail_PK], [master_ID], [column_name], [old_value], [new_value], [PK_value]
		FROM [dbo].[AUDITING_DETAILS]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[AUDITING_DETAILS]
END
