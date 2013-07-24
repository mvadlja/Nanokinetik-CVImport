-- GetEntity
CREATE PROCEDURE [dbo].[proc_ATTACHMENT_GetEntityByEVCode]
	@ev_code nvarchar(60) = null 
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
	[attachment_PK], [session_id], [disk_file], [document_FK], [attachmentname], [filetype], [ev_code], [modified_date], [EDMSDocumentId], [EDMSBindingRule], [EDMSAttachmentFormat],
	[lock_owner_FK], [lock_date], [check_in_for_attach_FK], [check_in_session_id]
	FROM [dbo].[ATTACHMENT]
	WHERE [ev_code] = @ev_code
	ORDER BY [attachment_PK] DESC
END
