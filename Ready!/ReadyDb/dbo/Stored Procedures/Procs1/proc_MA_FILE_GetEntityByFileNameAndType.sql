﻿-- GetEntity
create PROCEDURE [dbo].[proc_MA_FILE_GetEntityByFileNameAndType]
	@file_name NVARCHAR(200) = NULL,
	@file_type_FK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
	[ma_file_PK], [file_type_FK], [file_name], [file_path], [file_data], [ready_id_FK]
	FROM [dbo].[MA_FILE]
	WHERE [file_name] = @file_name AND [file_type_FK] = @file_type_FK
END
