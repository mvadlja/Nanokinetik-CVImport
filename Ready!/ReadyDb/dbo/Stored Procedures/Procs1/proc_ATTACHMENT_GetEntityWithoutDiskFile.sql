-- GetEntityWithoutDiskFile
CREATE PROCEDURE  [dbo].[proc_ATTACHMENT_GetEntityWithoutDiskFile]
	@attachment_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[attachment_PK], [session_id], [document_FK], [attachmentname], [filetype], [type_for_fts] , [ev_code], [modified_date], [EDMSDocumentId], [EDMSBindingRule], [EDMSAttachmentFormat],
	[lock_owner_FK], [lock_date], [check_in_for_attach_FK], [check_in_session_id]
	FROM [dbo].[ATTACHMENT]
	WHERE [attachment_PK] = @attachment_PK
END
