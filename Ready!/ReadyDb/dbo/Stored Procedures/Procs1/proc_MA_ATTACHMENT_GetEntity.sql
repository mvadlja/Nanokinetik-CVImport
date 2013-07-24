-- GetEntity
create PROCEDURE [dbo].[proc_MA_ATTACHMENT_GetEntity]
	@ma_attachment_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ma_attachment_PK], [file_name], [file_path], [file_data], [last_change], [deleted]
	FROM [dbo].[MA_ATTACHMENT]
	WHERE [ma_attachment_PK] = @ma_attachment_PK
END
