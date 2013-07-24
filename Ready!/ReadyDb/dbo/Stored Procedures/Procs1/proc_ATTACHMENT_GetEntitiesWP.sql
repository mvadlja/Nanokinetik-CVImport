-- GetEntitiesWP
CREATE PROCEDURE [dbo].[proc_ATTACHMENT_GetEntitiesWP]
	@PageNum int = 1,
	@PageSize int = 20
AS
BEGIN
	SET NOCOUNT ON;

	WITH PagingResult AS
	(
		SELECT ROW_NUMBER() OVER(ORDER BY [attachment_PK]) AS RowNum,
		[attachment_PK], [session_id], [disk_file], [document_FK], [attachmentname], [filetype], [userID], [ev_code], [modified_date], [EDMSDocumentId], [EDMSBindingRule], [EDMSAttachmentFormat],
		[lock_owner_FK], [lock_date], [check_in_for_attach_FK], [check_in_session_id]
		FROM [dbo].[ATTACHMENT]
	)

	SELECT *
	FROM PagingResult
	WHERE RowNum BETWEEN (@PageNum - 1) * @PageSize + 1 AND @PageNum * @PageSize

	-- Total count
	SELECT COUNT(*) FROM [dbo].[ATTACHMENT]
END
