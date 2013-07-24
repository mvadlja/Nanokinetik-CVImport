-- Delete
CREATE PROCEDURE  [dbo].[proc_ACTIVITY_DOCUMENT_MN_Delete]
	@activity_document_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[ACTIVITY_DOCUMENT_MN] WHERE [activity_document_PK] = @activity_document_PK
END
