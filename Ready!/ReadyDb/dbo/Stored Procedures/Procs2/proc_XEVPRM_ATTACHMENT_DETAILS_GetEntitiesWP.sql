-- GetEntitiesWP
CREATE PROCEDURE  [dbo].[proc_XEVPRM_ATTACHMENT_DETAILS_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [xevprm_attachment_details_PK]) AS RowNum,
		[xevprm_attachment_details_PK], [attachment_FK], [file_name], [file_type], [attachment_name], [attachment_type], [language_code], [attachment_version], [attachment_version_date], [operation_type], [ev_code]
		FROM [dbo].[XEVPRM_ATTACHMENT_DETAILS]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[XEVPRM_ATTACHMENT_DETAILS]
END
