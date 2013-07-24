-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_MA_ATTACHMENT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ma_attachment_PK]) AS RowNum,
		[ma_attachment_PK], [file_name], [file_path], [file_data], [last_change], [deleted]
		FROM [dbo].[MA_ATTACHMENT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MA_ATTACHMENT]
END
