-- GetEntities
CREATE PROCEDURE [dbo].[proc_ATTACHMENT_GetEntitiesWithoutDiskFile]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[attachment_PK], [session_id], [document_FK], [attachmentname], [filetype], [userID], [ev_code], [modified_date], [EDMSDocumentId], [EDMSBindingRule], [EDMSAttachmentFormat],
	[lock_owner_FK], [lock_date], [check_in_for_attach_FK], [check_in_session_id]
	FROM [dbo].[ATTACHMENT]
END
