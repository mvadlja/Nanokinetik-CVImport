
CREATE PROCEDURE  [dbo].[proc_ATTACHMENT_GetCheckedInAttachment]
	@attachmentPk int = NULL,
	@sessionId uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
	attachment_PK, document_FK, attachmentname, filetype, userID, pom_type, session_id, ev_code, type_for_fts, disk_file, [file_id], modified_date, [EDMSDocumentId], [EDMSBindingRule], [EDMSAttachmentFormat],
	[lock_owner_FK], [lock_date], [check_in_for_attach_FK], [check_in_session_id]
	FROM [dbo].[ATTACHMENT] a
	where a.check_in_session_id = @sessionId AND a.check_in_for_attach_FK = @attachmentPk AND
	a.modified_date > dateadd(d,-2, getDate())
END