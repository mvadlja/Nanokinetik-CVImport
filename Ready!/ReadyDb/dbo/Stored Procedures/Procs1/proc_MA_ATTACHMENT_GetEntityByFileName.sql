-- GetEntity
create PROCEDURE [dbo].[proc_MA_ATTACHMENT_GetEntityByFileName]
	@file_name NVARCHAR(200) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
	[ma_attachment_PK], [file_name], [file_path], [file_data], [last_change], [deleted]
	FROM [dbo].[MA_ATTACHMENT]
	WHERE [file_name] = @file_name
END
