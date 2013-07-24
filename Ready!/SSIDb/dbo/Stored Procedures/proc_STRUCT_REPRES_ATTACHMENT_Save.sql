
-- Save
CREATE PROCEDURE [dbo].[proc_STRUCT_REPRES_ATTACHMENT_Save]
	@struct_repres_attach_PK int = NULL,
	@Id uniqueidentifier = NULL,
	@disk_file varbinary(MAX) = NULL,
	@attachmentname varchar(2000) = NULL,
	@filetype varchar(25) = NULL,
	@userID int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[STRUCT_REPRES_ATTACHMENT]
	SET
	[Id] = @Id,
	[disk_file] = @disk_file,
	[attachmentname] = @attachmentname,
	[filetype] = @filetype,
	[userID] = @userID
	WHERE [struct_repres_attach_PK] = @struct_repres_attach_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[STRUCT_REPRES_ATTACHMENT]
		([Id], [disk_file], [attachmentname], [filetype], [userID])
		VALUES
		(@Id, @disk_file, @attachmentname, @filetype, @userID)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
