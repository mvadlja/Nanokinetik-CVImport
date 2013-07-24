-- Save
create PROCEDURE [dbo].[proc_MA_ATTACHMENT_Save]
	@ma_attachment_PK int = NULL,
	@file_name nvarchar(200) = NULL,
	@file_path nvarchar(500) = NULL,
	@file_data varbinary(MAX) = NULL,
	@last_change datetime = NULL,
	@deleted bit = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MA_ATTACHMENT]
	SET
	[file_name] = @file_name,
	[file_path] = @file_path,
	[file_data] = @file_data,
	[last_change] = @last_change,
	[deleted] = @deleted
	WHERE [ma_attachment_PK] = @ma_attachment_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[MA_ATTACHMENT]
		([file_name], [file_path], [file_data], [last_change], [deleted])
		VALUES
		(@file_name, @file_path, @file_data, @last_change, @deleted)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END

	--DBCC FREEPROCCACHE
	--DBCC DROPCLEANBUFFERS

END
