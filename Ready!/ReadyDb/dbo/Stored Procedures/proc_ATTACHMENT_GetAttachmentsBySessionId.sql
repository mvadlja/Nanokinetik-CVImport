
CREATE PROCEDURE  [dbo].[proc_ATTACHMENT_GetAttachmentsBySessionId]
	@sessionId uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	attachment_PK, document_FK, attachmentname, filetype, userID, pom_type, session_id, ev_code, type_for_fts, file_id, modified_date, [EDMSDocumentId], [EDMSBindingRule], [EDMSAttachmentFormat],
	[lock_owner_FK], [lock_date], [check_in_for_attach_FK], [check_in_session_id]
	FROM [dbo].[ATTACHMENT] a
	where a.session_id = @sessionId
END