-- GetEntity
create PROCEDURE [dbo].[proc_MA_FILE_GetEntityByReadyIdAndType]
	@ready_id_FK NVARCHAR(20) = NULL,
	@file_type_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ma_file_PK], [file_type_FK], [file_name], [file_path], [file_data], [ready_id_FK]
	FROM [dbo].[MA_FILE]
	WHERE [ready_id_FK] = @ready_id_FK AND [file_type_FK] = @file_type_FK
END
