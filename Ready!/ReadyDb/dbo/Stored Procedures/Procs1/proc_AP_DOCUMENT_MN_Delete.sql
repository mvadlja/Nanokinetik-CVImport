-- Delete
CREATE PROCEDURE  [dbo].[proc_AP_DOCUMENT_MN_Delete]
	@ap_document_mn_PK int = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM [dbo].[AP_DOCUMENT_MN] WHERE [ap_document_mn_PK] = @ap_document_mn_PK
END
