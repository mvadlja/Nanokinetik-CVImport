-- GetEntitiesWP
create PROCEDURE [dbo].[proc_MA_FILE_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [ma_file_PK]) AS RowNum,
		[ma_file_PK], [file_type_FK], [file_name], [file_path], [file_data], [ready_id_FK]
		FROM [dbo].[MA_FILE]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[MA_FILE]
END
