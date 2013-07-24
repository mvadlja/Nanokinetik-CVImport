-- Delete
CREATE PROCEDURE [dbo].[proc_MA_ATTACHMENT_Delete]
	@ma_attachment_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[MA_ATTACHMENT] WHERE [ma_attachment_PK] = @ma_attachment_PK
END
