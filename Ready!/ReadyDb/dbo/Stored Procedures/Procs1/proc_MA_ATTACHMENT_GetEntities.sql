-- GetEntities
create PROCEDURE [dbo].[proc_MA_ATTACHMENT_GetEntities]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
	[ma_attachment_PK], [file_name], [file_path], [file_data], [last_change], [deleted]
	FROM [dbo].[MA_ATTACHMENT]
END
