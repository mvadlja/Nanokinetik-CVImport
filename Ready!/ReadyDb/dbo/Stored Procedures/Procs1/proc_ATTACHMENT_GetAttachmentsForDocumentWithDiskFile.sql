-- GetEntities
CREATE PROCEDURE  [dbo].[proc_ATTACHMENT_GetAttachmentsForDocumentWithDiskFile]
	@document_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[attachment_PK], [session_id], [disk_file], 
	[document_FK], [attachmentname], [filetype], [type_for_fts], [userID], [ev_code], [modified_date], [EDMSDocumentId], [EDMSBindingRule], [EDMSAttachmentFormat],
	[lock_owner_FK], [lock_date], [check_in_for_attach_FK], [check_in_session_id]
	FROM [dbo].[ATTACHMENT]
	WHERE ([dbo].[ATTACHMENT].[document_FK] = @document_FK OR @document_FK IS NULL)
END
