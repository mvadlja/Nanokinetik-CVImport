-- GetEntity
create PROCEDURE [dbo].[proc_MA_FILE_GetEntity]
	@ma_file_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ma_file_PK], [file_type_FK], [file_name], [file_path], [file_data], [ready_id_FK]
	FROM [dbo].[MA_FILE]
	WHERE [ma_file_PK] = @ma_file_PK
END
