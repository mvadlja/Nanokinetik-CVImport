-- Delete
create PROCEDURE [dbo].[proc_MA_FILE_Delete]
	@ma_file_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[MA_FILE] WHERE [ma_file_PK] = @ma_file_PK
END
