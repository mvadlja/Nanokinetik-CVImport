-- Delete
CREATE PROCEDURE  [dbo].[proc_DOCUMENT_LANGUAGE_MN_Delete]
	@document_language_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[DOCUMENT_LANGUAGE_MN] WHERE [document_language_mn_PK] = @document_language_mn_PK
END
