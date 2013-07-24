-- GetEntities
create PROCEDURE [dbo].[proc_MA_FILE_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ma_file_PK], [file_type_FK], [file_name], [file_path], [file_data], [ready_id_FK]
	FROM [dbo].[MA_FILE]
END
