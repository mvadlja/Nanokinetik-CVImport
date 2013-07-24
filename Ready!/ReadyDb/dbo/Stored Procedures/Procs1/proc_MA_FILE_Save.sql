-- Save
create PROCEDURE [dbo].[proc_MA_FILE_Save]
	@ma_file_PK int = NULL,
	@file_type_FK int = NULL,
	@file_name nvarchar(200) = NULL,
	@file_path nvarchar(500) = NULL,
	@file_data varbinary(MAX) = NULL,
	@ready_id_FK nvarchar(50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE [dbo].[MA_FILE]
	SET
	[file_type_FK] = @file_type_FK,
	[file_name] = @file_name,
	[file_path] = @file_path,
	[file_data] = @file_data,
	[ready_id_FK] = @ready_id_FK
	WHERE [ma_file_PK] = @ma_file_PK

	IF @@ROWCOUNT = 0
	BEGIN
		DECLARE @ScopeIdentity int

		INSERT INTO [dbo].[MA_FILE]
		([file_type_FK], [file_name], [file_path], [file_data], [ready_id_FK])
		VALUES
		(@file_type_FK, @file_name, @file_path, @file_data, @ready_id_FK)

		SET @ScopeIdentity = SCOPE_IDENTITY();
		SELECT @ScopeIdentity AS ScopeIdentity
	END
END
