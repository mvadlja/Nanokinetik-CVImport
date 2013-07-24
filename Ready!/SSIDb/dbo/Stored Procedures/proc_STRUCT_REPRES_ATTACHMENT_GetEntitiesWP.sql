
-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_STRUCT_REPRES_ATTACHMENT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [struct_repres_attach_PK]) AS RowNum,
		[struct_repres_attach_PK], [Id], [disk_file], [attachmentname], [filetype], [userID]
		FROM [dbo].[STRUCT_REPRES_ATTACHMENT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[STRUCT_REPRES_ATTACHMENT]
END
