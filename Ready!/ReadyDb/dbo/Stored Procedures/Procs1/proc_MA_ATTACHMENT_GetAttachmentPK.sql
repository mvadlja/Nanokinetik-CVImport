-- GetEntity
create PROCEDURE [dbo].[proc_MA_ATTACHMENT_GetAttachmentPK]
	@file_name NVARCHAR(200) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT TOP 1
	[ma_attachment_PK]
	FROM [dbo].[MA_ATTACHMENT]
	WHERE [file_name] = @file_name
END
