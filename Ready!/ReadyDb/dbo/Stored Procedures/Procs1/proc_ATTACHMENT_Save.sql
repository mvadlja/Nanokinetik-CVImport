-- Save
CREATE PROCEDURE  [dbo].[proc_ATTACHMENT_Save]
	@attachment_PK int = NULL,
	@disk_file varbinary(MAX) = NULL,
	@document_FK int = NULL,
	@attachmentname nvarchar(2000) = NULL,
	@filetype nvarchar(50) = NULL,
	@userID int = NULL,
	@pom_type nvarchar(50) = NULL,
	@session_id uniqueidentifier = NULL,
	@ev_code nvarchar(255) = NULL,
	@type_for_fts nvarchar(8) = NULL,
	@modified_date datetime = NULL, 
	@EDMSDocumentId nvarchar(128) = NULL,
	@EDMSBindingRule nvarchar(128) = NULL,
	@EDMSAttachmentFormat nvarchar(128) = NULL,
	@lock_owner_FK int = NULL,
	@lock_date datetime = NULL, 
	@check_in_for_attach_FK int = NULL, 
	@check_in_session_id uniqueidentifier = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[ATTACHMENT]
	SET
	[disk_file] = @disk_file,
	[document_FK] = @document_FK,
	[attachmentname] = @attachmentname,
	[filetype] = @filetype,
	[userID] = @userID,
	[pom_type] = @pom_type,
	[session_id] = @session_id,
	[ev_code] = @ev_code,
	[type_for_fts] = @type_for_fts,
	[modified_date] = @modified_date,
	[EDMSDocumentId] = @EDMSDocumentId,
	[EDMSBindingRule] = @EDMSBindingRule,
	[EDMSAttachmentFormat] = @EDMSAttachmentFormat,
	[lock_owner_FK] = @lock_owner_FK,
	[lock_date] = @lock_date, 
	[check_in_for_attach_FK] = @check_in_for_attach_FK, 
	[check_in_session_id] = @check_in_session_id
	WHERE [attachment_PK] = @attachment_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[ATTACHMENT]
		([disk_file], [document_FK], [attachmentname], [filetype], [userID], [pom_type], [session_id], [ev_code], [type_for_fts], [modified_date], [EDMSDocumentId], [EDMSBindingRule], [EDMSAttachmentFormat], [lock_owner_FK], [lock_date], [check_in_for_attach_FK], [check_in_session_id])
		VALUES
		(@disk_file, @document_FK, @attachmentname, @filetype, @userID, @pom_type, @session_id, @ev_code, @type_for_fts, @modified_date, @EDMSDocumentId, @EDMSBindingRule, @EDMSAttachmentFormat, @lock_owner_FK, @lock_date, @check_in_for_attach_FK, @check_in_session_id)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
